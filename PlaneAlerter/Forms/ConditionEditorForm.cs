using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using PlaneAlerter.Controls;
using PlaneAlerter.Enums;
using PlaneAlerter.Models;
using PlaneAlerter.Services;

namespace PlaneAlerter.Forms {
	/// <summary>
	/// Condition list on the left, the selected condition on the right. Edits apply to a working copy
	/// as you make them; nothing is written to conditions.json until Save and close.
	/// </summary>
	internal partial class ConditionEditorForm :Form {
		private readonly IConditionManagerService _conditionManagerService;
		private readonly IStringFormatterService _stringFormatterService;
		private readonly IVrsEnumService _vrsEnumService;
		private readonly ICheckerService _checkerService;

		/// <summary>
		/// Working copy of the conditions, in check order
		/// </summary>
		private readonly List<ConditionDraft> _drafts = new();

		/// <summary>
		/// Serialised drafts as they were when the editor opened
		/// </summary>
		private readonly string _openedSnapshot;

		/// <summary>
		/// Condition shown in the editor, null when there are none
		/// </summary>
		private ConditionDraft? _current;

		/// <summary>
		/// Suppresses change handlers while controls are being populated
		/// </summary>
		private bool _loading;

		private bool _saved;
		private int? _pendingSelection;

		private int _dragIndex = -1;
		private Point _dragStart;

		/// <summary>
		/// Last deleted condition and where it was, for Undo
		/// </summary>
		private (ConditionDraft Draft, int Index)? _lastDeleted;

		/// <summary>
		/// Last non-empty radar snapshot. The checker clears the list while it downloads, so this bridges the gap.
		/// </summary>
		private IReadOnlyCollection<Aircraft> _radar = Array.Empty<Aircraft>();
		private DateTime _radarTime = DateTime.MinValue;

		/// <summary>
		/// Open subject field picker, closed when another condition is shown
		/// </summary>
		private FilterPopup? _placeholderPopup;

		private static readonly AlertType[] Timings = { AlertType.First_Contact, AlertType.Last_Contact, AlertType.First_and_Last_Contact };

		private static readonly Aircraft SampleAircraft = BuildSampleAircraft();

		public ConditionEditorForm(IConditionManagerService conditionManagerService,
			IStringFormatterService stringFormatterService, IVrsEnumService vrsEnumService, ICheckerService checkerService) {
			_conditionManagerService = conditionManagerService;
			_stringFormatterService = stringFormatterService;
			_vrsEnumService = vrsEnumService;
			_checkerService = checkerService;

			InitializeComponent();
			BuildLayout();

			foreach (var key in _conditionManagerService.Conditions.Keys.OrderBy(k => k))
				_drafts.Add(ConditionDraft.From(_conditionManagerService.Conditions[key]));
			_openedSnapshot = Snapshot();
		}

		/// <summary>
		/// Select a condition when the editor opens
		/// </summary>
		public void SelectCondition(int conditionId)
		{
			_pendingSelection = conditionId;
		}

		private void ConditionEditorForm_Load(object sender, EventArgs e)
		{
			ConditionDraft? select = null;
			if (_drafts.Count > 0)
				select = _drafts[Math.Clamp(_pendingSelection ?? 0, 0, _drafts.Count - 1)];

			ShowCondition(select);
			RefreshList();
			ActiveControl = conditionList;
			radarRefreshTimer.Start();
		}

		private string Snapshot() => JsonConvert.SerializeObject(_drafts);

		private static AlertType TimingFromIndex(int index) => index >= 0 && index < Timings.Length ? Timings[index] : AlertType.First_and_Last_Contact;

		#region Condition list

		private (string, bool) DescribeForList(ConditionDraft draft)
		{
			if (draft.ShowProblems)
			{
				var problems = draft.Problems();
				if (problems.Count > 0)
					return (problems.Count == 1 ? problems[0].Message : $"{problems[0].Message} (+{problems.Count - 1} more)", true);
			}
			return (draft.Summary(_vrsEnumService), false);
		}

		/// <summary>
		/// Rebuild the list from the drafts, applying the filter
		/// </summary>
		private void RefreshList()
		{
			var filter = filterBox.Text.Trim();
			var visible = _drafts.Where(d => filter.Length == 0 ||
				d.Name.Contains(filter, StringComparison.OrdinalIgnoreCase) ||
				d.Summary(_vrsEnumService).Contains(filter, StringComparison.OrdinalIgnoreCase)).ToList();

			var wasLoading = _loading;
			_loading = true;
			conditionList.BeginUpdate();
			conditionList.Items.Clear();
			foreach (var draft in visible)
				conditionList.Items.Add(draft);
			conditionList.SelectedItem = _current != null && visible.Contains(_current) ? _current : null;
			conditionList.EndUpdate();
			_loading = wasLoading;

			countLabel.Text = filter.Length > 0 ? $"{visible.Count} of {_drafts.Count}" : _drafts.Count.ToString();
		}

		private void conditionList_SelectedIndexChanged(object? sender, EventArgs e)
		{
			if (_loading || conditionList.SelectedItem is not ConditionDraft draft || draft == _current)
				return;

			ShowCondition(draft);
		}

		private void conditionList_KeyDown(object? sender, KeyEventArgs e)
		{
			if (e.KeyCode == Keys.Delete)
				DeleteCurrent();
			else if (e.Alt && e.KeyCode == Keys.Up)
				MoveCurrent(-1);
			else if (e.Alt && e.KeyCode == Keys.Down)
				MoveCurrent(1);
			else
				return;

			e.Handled = true;
			e.SuppressKeyPress = true;
		}

		private void UpdateMenuState()
		{
			var index = _current == null ? -1 : _drafts.IndexOf(_current);
			duplicateMenuItem.Enabled = index >= 0;
			deleteMenuItem.Enabled = index >= 0;
			moveUpMenuItem.Enabled = index > 0;
			moveDownMenuItem.Enabled = index >= 0 && index < _drafts.Count - 1;
		}

		private void AddCondition()
		{
			//New conditions usually go to the same people, so start with the current recipients
			var recipients = (_current ?? _drafts.LastOrDefault(d => d.Recipients.Count > 0))?.Recipients.ToList() ?? new List<string>();

			var draft = new ConditionDraft
			{
				Name = "New condition",
				FirstSubject = "First contact alert! [ConditionName]: [Reg]",
				LastSubject = "Last contact alert! [ConditionName]: [Reg]",
				Recipients = recipients,
				Rules = { new RuleDraft() }
			};
			_drafts.Add(draft);
			HideStatus();

			filterBox.Text = "";
			ShowCondition(draft);
			RefreshList();

			nameBox.Inner.Focus();
			nameBox.Inner.SelectAll();
		}

		private void DuplicateCurrent()
		{
			if (_current == null)
				return;

			var copy = _current.Clone();
			copy.Name = _current.Name + " (copy)";
			_drafts.Insert(_drafts.IndexOf(_current) + 1, copy);
			HideStatus();
			ShowCondition(copy);
			RefreshList();
			nameBox.Inner.Focus();
			nameBox.Inner.SelectAll();
		}

		private void DeleteCurrent()
		{
			if (_current == null)
				return;

			var index = _drafts.IndexOf(_current);
			var deleted = _current;
			_drafts.RemoveAt(index);
			_lastDeleted = (deleted, index);

			_current = null;
			ShowCondition(_drafts.Count == 0 ? null : _drafts[Math.Min(index, _drafts.Count - 1)]);
			RefreshList();

			var name = string.IsNullOrWhiteSpace(deleted.Name) ? "Untitled condition" : deleted.Name;
			ShowStatus($"Deleted “{name}”.", Theme.TextMuted);
			undoLink.Visible = true;
		}

		private void UndoDelete()
		{
			if (_lastDeleted == null)
				return;

			var (draft, index) = _lastDeleted.Value;
			_drafts.Insert(Math.Min(index, _drafts.Count), draft);
			HideStatus();
			ShowCondition(draft);
			RefreshList();
		}

		private void MoveCurrent(int delta)
		{
			if (_current == null)
				return;

			var index = _drafts.IndexOf(_current);
			var target = index + delta;
			if (target < 0 || target >= _drafts.Count)
				return;

			(_drafts[index], _drafts[target]) = (_drafts[target], _drafts[index]);
			RefreshList();
		}

		#endregion

		#region Drag to reorder

		private void conditionList_MouseDown(object? sender, MouseEventArgs e)
		{
			var index = conditionList.IndexFromPoint(e.Location);

			//Right-click selects the row the menu is for
			if (e.Button == MouseButtons.Right && index >= 0)
				conditionList.SelectedIndex = index;

			_dragIndex = e.Button == MouseButtons.Left ? index : -1;
			_dragStart = e.Location;
		}

		private void conditionList_MouseMove(object? sender, MouseEventArgs e)
		{
			if (_dragIndex < 0 || e.Button != MouseButtons.Left || filterBox.Text.Length > 0)
				return;

			if (Math.Abs(e.X - _dragStart.X) < SystemInformation.DragSize.Width &&
			    Math.Abs(e.Y - _dragStart.Y) < SystemInformation.DragSize.Height)
				return;

			var draft = conditionList.Items[_dragIndex] as ConditionDraft;
			_dragIndex = -1;
			if (draft != null)
				conditionList.DoDragDrop(draft, DragDropEffects.Move);
			conditionList.DropIndex = -1;
		}

		private int DropTarget(DragEventArgs e)
		{
			var point = conditionList.PointToClient(new Point(e.X, e.Y));
			var index = conditionList.IndexFromPoint(point);
			if (index < 0)
				return conditionList.Items.Count;

			var rect = conditionList.GetItemRectangle(index);
			return point.Y > rect.Top + rect.Height / 2 ? index + 1 : index;
		}

		private void conditionList_DragOver(object? sender, DragEventArgs e)
		{
			if (e.Data?.GetDataPresent(typeof(ConditionDraft)) != true)
			{
				e.Effect = DragDropEffects.None;
				return;
			}

			e.Effect = DragDropEffects.Move;
			conditionList.DropIndex = DropTarget(e);
		}

		private void conditionList_DragDrop(object? sender, DragEventArgs e)
		{
			conditionList.DropIndex = -1;
			if (e.Data?.GetData(typeof(ConditionDraft)) is not ConditionDraft draft)
				return;

			var from = _drafts.IndexOf(draft);
			var to = DropTarget(e);
			if (from < 0)
				return;
			if (to > from)
				to--;
			if (to == from)
				return;

			_drafts.RemoveAt(from);
			_drafts.Insert(to, draft);
			RefreshList();
		}

		#endregion

		#region Editor

		/// <summary>
		/// Show a condition in the editor (null shows the empty state)
		/// </summary>
		private void ShowCondition(ConditionDraft? draft)
		{
			//Problems on the condition being left are now worth pointing out
			if (_current != null && _current != draft)
			{
				_current.ShowProblems = true;
				conditionList.InvalidateItem(_current);
			}

			_placeholderPopup?.Close();
			_current = draft;
			_loading = true;

			emptyPanel.Visible = draft == null;
			editorHost.Visible = draft != null;

			if (draft != null)
			{
				editorHost.SuspendLayout();
				editorHost.AutoScrollPosition = Point.Empty;

				nameBox.Text = draft.Name;
				enabledToggle.Checked = draft.Enabled;
				enabledToggle.Text = draft.Enabled ? "Enabled" : "Off";
				logicSegment.SelectedIndex = draft.UseOrLogic ? 1 : 0;
				timingSegment.SelectedIndex = Array.IndexOf(Timings, draft.Timing);
				emailToggle.Checked = draft.EmailEnabled;
				recipientsInput.SetValues(draft.Recipients);
				firstSubjectBox.Text = draft.FirstSubject;
				lastSubjectBox.Text = draft.LastSubject;
				ignoreFollowingCheck.Checked = draft.IgnoreFollowing;
				SetAdvancedOpen(draft.IgnoreFollowing);

				rulesTable.SuspendLayout();
				foreach (Control row in rulesTable.Controls.Cast<Control>().ToList())
				{
					rulesTable.Controls.Remove(row);
					row.Dispose();
				}
				rulesTable.RowStyles.Clear();
				rulesTable.RowCount = 0;
				foreach (var rule in draft.Rules)
					AddRuleRow(rule);
				rulesTable.ResumeLayout();

				UpdateVisibility();
				UpdatePreviews();
				ShowProblems();
				editorHost.ResumeLayout();
			}

			_loading = false;

			if (draft != null && conditionList.SelectedItem != draft && conditionList.Items.Contains(draft))
			{
				_loading = true;
				conditionList.SelectedItem = draft;
				_loading = false;
			}

			UpdateMatches();
		}

		/// <summary>
		/// Apply an edit to the current condition and refresh everything that depends on it
		/// </summary>
		private void Edit(Action<ConditionDraft> change)
		{
			if (_loading || _current == null)
				return;

			change(_current);
			conditionList.InvalidateItem(_current);
			if (_current.ShowProblems)
				ShowProblems();
			UpdatePreviews();

			matchTimer.Stop();
			matchTimer.Start();
		}

		private RuleRow AddRuleRow(RuleDraft rule)
		{
			var row = new RuleRow(rule, _vrsEnumService, toolTip, Font)
			{
				Anchor = AnchorStyles.Left | AnchorStyles.Right,
				Margin = new Padding(0, 0, 0, S(4))
			};
			row.Changed += (_, _) => Edit(_ => { });
			row.RemoveRequested += (_, _) => RemoveRule(row);
			rulesTable.Controls.Add(row);
			return row;
		}

		private void AddRule()
		{
			if (_current == null)
				return;

			var rule = new RuleDraft();
			_current.Rules.Add(rule);
			var row = AddRuleRow(rule);
			Edit(_ => { });

			editorHost.ScrollControlIntoView(addRuleButton);
			BeginInvoke(new Action(row.OpenPropertyPicker));
		}

		private void RemoveRule(RuleRow row)
		{
			if (_current == null)
				return;

			_current.Rules.Remove(row.Rule);
			rulesTable.Controls.Remove(row);
			row.Dispose();
			Edit(_ => { });
			addRuleButton.Focus();
		}

		private void SetAdvancedOpen(bool open)
		{
			advancedPanel.Visible = open;
			advancedButton.Text = (open ? "▾" : "▸") + "  More options";
		}

		/// <summary>
		/// Show only the email fields the alert timing uses
		/// </summary>
		private void UpdateVisibility()
		{
			if (_current == null)
				return;

			var timing = TimingFromIndex(timingSegment.SelectedIndex);
			timingHint.Text = timing switch
			{
				AlertType.First_Contact => "When a matching aircraft first appears on the radar.",
				AlertType.Last_Contact => "When a matching aircraft leaves the radar.",
				_ => "When a matching aircraft appears, and again when it leaves."
			};

			emailTable.SuspendLayout();
			emailTable.Visible = emailToggle.Checked;
			var first = timing != AlertType.Last_Contact;
			var last = timing != AlertType.First_Contact;
			firstSubjectLabel.Visible = firstSubjectRow.Visible = firstPreview.Visible = first;
			lastSubjectLabel.Visible = lastSubjectRow.Visible = lastPreview.Visible = last;
			emailTable.ResumeLayout();
		}

		/// <summary>
		/// Show problems next to the fields they belong to, once the condition has been left or saving was tried
		/// </summary>
		private void ShowProblems()
		{
			if (_current == null)
				return;

			var problems = _current.ShowProblems ? _current.Problems() : new List<ConditionDraft.Problem>();
			string? Message(ConditionDraft.Field field) => problems.FirstOrDefault(p => p.Field == field && p.Rule == null)?.Message;

			SetError(nameError, Message(ConditionDraft.Field.Name));
			nameBox.HasError = nameError.Visible;

			SetError(rulesError, Message(ConditionDraft.Field.Rules));
			foreach (var row in rulesTable.Controls.OfType<RuleRow>())
				row.ShowError(problems.FirstOrDefault(p => p.Rule == row.Rule)?.Message);

			SetError(recipientsError, Message(ConditionDraft.Field.Recipients));
			recipientsInput.HasError = recipientsError.Visible;

			var firstError = Message(ConditionDraft.Field.FirstSubject);
			var lastError = Message(ConditionDraft.Field.LastSubject);
			firstSubjectBox.HasError = firstError != null;
			lastSubjectBox.HasError = lastError != null;
			UpdatePreviews(firstError, lastError);
		}

		private static void SetError(Label label, string? message)
		{
			label.Text = message ?? "";
			label.Visible = message != null;
		}

		private void UpdatePreviews() => UpdatePreviews(
			firstSubjectBox.HasError ? _current?.Problems().FirstOrDefault(p => p.Field == ConditionDraft.Field.FirstSubject)?.Message : null,
			lastSubjectBox.HasError ? _current?.Problems().FirstOrDefault(p => p.Field == ConditionDraft.Field.LastSubject)?.Message : null);

		private void UpdatePreviews(string? firstError, string? lastError)
		{
			if (_current == null)
				return;

			var condition = new Condition(_current.Name, AlertType.Disabled, false, false, false, "", "", new List<string>());

			void Set(Label label, string? error, string format)
			{
				if (error != null)
				{
					label.ForeColor = Theme.ErrorText;
					label.Text = error;
				}
				else
				{
					label.ForeColor = Theme.TextMuted;
					label.Text = Preview(format, condition);
				}
			}

			Set(firstPreview, firstError, _current.FirstSubject);
			Set(lastPreview, lastError, _current.LastSubject);
		}

		private string Preview(string format, Condition condition)
		{
			if (string.IsNullOrWhiteSpace(format))
				return "Type [ to insert aircraft details such as the registration.";

			try
			{
				return "Preview: " + _stringFormatterService.Format(format, SampleAircraft, condition);
			}
			catch (Exception)
			{
				return "";
			}
		}

		private static Aircraft BuildSampleAircraft()
		{
			var aircraft = new Aircraft("A1B2C3");
			var sample = new Dictionary<string, string>
			{
				{ "Icao", "A1B2C3" }, { "Reg", "N123AB" }, { "Call", "UAL123" }, { "Type", "B738" },
				{ "Mdl", "Boeing 737-800" }, { "Man", "Boeing" }, { "Op", "United Airlines" }, { "OpIcao", "UAL" },
				{ "Alt", "35000" }, { "GAlt", "35200" }, { "Spd", "450" }, { "Vsi", "0" }, { "Trak", "92" },
				{ "Lat", "37.6213" }, { "Long", "-122.379" }, { "Sqk", "4521" }, { "Species", "1" }, { "WTC", "2" },
				{ "EngType", "3" }, { "Engines", "2" }, { "Mil", "False" }, { "Cou", "United States" },
				{ "From", "KSFO San Francisco" }, { "To", "KJFK New York" }, { "Trt", "5" }, { "Rcvr", "1" },
				{ "Dst", "12.4" }, { "Brng", "215" }, { "Year", "2016" }, { "Gnd", "False" }, { "TSecs", "610" }
			};
			foreach (var pair in sample)
				aircraft.AddProperty(pair.Key, pair.Value);
			return aircraft;
		}

		#endregion

		#region Placeholders

		/// <summary>
		/// Show the field picker for a subject. bracketIndex is the position of a just-typed "[" to replace, or -1.
		/// </summary>
		private void ShowPlaceholderPopup(FieldBox box, int bracketIndex)
		{
			var entries = new List<FilterPopup.Entry>
			{
				new("This alert", "Condition name", "[ConditionName]", "ConditionName"),
				new("This alert", "Receiver name", "[RcvrName]", "RcvrName"),
				new("This alert", "Today's date", "[Date]", "Date"),
				new("This alert", "Current time", "[Time]", "Time")
			};
			foreach (var (category, properties) in PropertyMeta.Grouped())
			{
				foreach (var p in properties)
				{
					var key = PropertyMeta.InternalName(p);
					entries.Add(new FilterPopup.Entry(category, PropertyMeta.Label(p), $"[{key}]", key, p.ToString().Replace('_', ' ')));
				}
			}

			var popup = new FilterPopup(entries, "Search fields", S(340), Font);
			popup.Picked += entry =>
			{
				var token = $"[{entry.Value}]";
				var inner = box.Inner;
				int start;
				if (bracketIndex >= 0 && bracketIndex < inner.TextLength && inner.Text[bracketIndex] == '[')
				{
					inner.Text = inner.Text.Remove(bracketIndex, 1).Insert(bracketIndex, token);
					start = bracketIndex;
				}
				else
				{
					start = inner.SelectionStart;
					inner.Text = inner.Text.Remove(start, inner.SelectionLength).Insert(start, token);
				}
				inner.Focus();
				inner.SelectionStart = start + token.Length;
				inner.SelectionLength = 0;
			};
			_placeholderPopup = popup;
			popup.Closed += (_, _) =>
			{
				_placeholderPopup = null;
				popup.BeginInvoke(new Action(popup.Dispose));
			};

			var location = new Point(0, box.Height + S(2));
			if (bracketIndex >= 0 && bracketIndex < box.Inner.TextLength)
			{
				var caret = box.Inner.GetPositionFromCharIndex(bracketIndex);
				location = new Point(Math.Max(0, box.Inner.Left + caret.X - S(8)), box.Height + S(2));
			}
			popup.ShowFor(box, location);
		}

		#endregion

		#region Live matches

		private void matchTimer_Tick(object? sender, EventArgs e)
		{
			matchTimer.Stop();
			UpdateMatches();
		}

		private void radarRefreshTimer_Tick(object? sender, EventArgs e)
		{
			UpdateMatches();
		}

		private IReadOnlyCollection<Aircraft> CurrentRadar()
		{
			try
			{
				var list = _checkerService.LastAircraftList;
				if (list.Count > 0)
				{
					_radar = list;
					_radarTime = DateTime.Now;
				}
			}
			catch (Exception)
			{
				//The checker may be replacing the list on another thread; use the last snapshot
			}

			return DateTime.Now - _radarTime < TimeSpan.FromMinutes(2) ? _radar : Array.Empty<Aircraft>();
		}

		/// <summary>
		/// Count aircraft currently on the radar that the condition would match
		/// </summary>
		private void UpdateMatches()
		{
			if (_current == null)
				return;

			var preview = _current.ToPreviewCondition();
			if (preview == null)
			{
				SetMatchText("Finish a rule to see which aircraft on the radar match.", Theme.TextMuted, null);
				return;
			}

			var radar = CurrentRadar();
			if (radar.Count == 0)
			{
				SetMatchText("No radar data yet. Start checking to see which aircraft match.", Theme.TextMuted, null);
				return;
			}

			var matches = new List<Aircraft>();
			foreach (var aircraft in radar)
			{
				try
				{
					if (ConditionMatcher.HasDatabaseInfo(aircraft) && ConditionMatcher.Matches(preview, aircraft))
						matches.Add(aircraft);
				}
				catch (Exception)
				{
					//Unparseable value on this aircraft, treat as no match
				}
			}

			var incomplete = preview.Triggers.Count < _current.Rules.Count ? " (ignoring unfinished rules)" : "";
			var text = matches.Count == 0
				? $"○  None of the {radar.Count} aircraft on the radar match right now{incomplete}"
				: $"●  {matches.Count} of {radar.Count} aircraft on the radar match right now{incomplete}";

			string? tip = null;
			if (matches.Count > 0)
			{
				var lines = matches.Take(15).Select(a => string.Join("   ",
					new[] { a.GetProperty("Reg"), a.GetProperty("Type"), a.GetProperty("Call"), a.GetProperty("Op") }
						.Where(v => !string.IsNullOrWhiteSpace(v)).DefaultIfEmpty(a.Icao)));
				tip = string.Join(Environment.NewLine, lines);
				if (matches.Count > 15)
					tip += Environment.NewLine + $"and {matches.Count - 15} more";
			}

			SetMatchText(text, matches.Count > 0 ? Theme.Success : Theme.TextMuted, tip);
		}

		private void SetMatchText(string text, Color color, string? tip)
		{
			if (matchLabel.Text != text)
				matchLabel.Text = text;
			matchLabel.ForeColor = color;
			toolTip.SetToolTip(matchLabel, tip ?? "Matches are checked against the aircraft from the last radar update.");
		}

		#endregion

		#region Status, save and close

		private void ShowStatus(string text, Color color)
		{
			statusLabel.Text = text;
			statusLabel.ForeColor = color;
			undoLink.Visible = false;
		}

		private void HideStatus()
		{
			statusLabel.Text = "";
			undoLink.Visible = false;
			_lastDeleted = null;
		}

		/// <summary>
		/// Pick up anything typed but not yet committed (an address still in the box)
		/// </summary>
		private void CaptureEditor()
		{
			if (_current != null && _current.EmailEnabled)
				_current.Recipients = recipientsInput.Values;
		}

		private bool SaveAll()
		{
			CaptureEditor();

			var invalid = _drafts.Where(d => d.Problems().Count > 0).ToList();
			if (invalid.Count > 0)
			{
				foreach (var draft in invalid)
					draft.ShowProblems = true;

				var first = invalid.Contains(_current!) ? _current! : invalid[0];
				if (first != _current)
				{
					filterBox.Text = "";
					ShowCondition(first);
					RefreshList();
				}
				ShowProblems();
				conditionList.Invalidate();

				var name = string.IsNullOrWhiteSpace(first.Name) ? "this condition" : $"“{first.Name}”";
				ShowStatus(invalid.Count == 1
					? $"Fix the problems in {name} before saving."
					: $"{invalid.Count} conditions have problems. Fix them before saving.", Theme.ErrorText);
				FocusFirstProblem(first);
				return false;
			}

			var sorted = new SortedDictionary<int, Condition>();
			for (var i = 0; i < _drafts.Count; i++)
				sorted.Add(i, _drafts[i].ToCondition());

			_conditionManagerService.EditorConditions = sorted;
			_conditionManagerService.SaveEditorConditions();
			_saved = true;
			return true;
		}

		private void FocusFirstProblem(ConditionDraft draft)
		{
			var problem = draft.Problems().FirstOrDefault();
			Control? target = problem?.Field switch
			{
				ConditionDraft.Field.Name => nameBox.Inner,
				ConditionDraft.Field.Rules => problem.Rule != null
					? rulesTable.Controls.OfType<RuleRow>().FirstOrDefault(r => r.Rule == problem.Rule)
					: addRuleButton,
				ConditionDraft.Field.Recipients => recipientsInput,
				ConditionDraft.Field.FirstSubject => firstSubjectBox.Inner,
				ConditionDraft.Field.LastSubject => lastSubjectBox.Inner,
				_ => null
			};
			if (target == null)
				return;

			editorHost.ScrollControlIntoView(target);
			if (target is ChipInput chips)
				chips.FocusInput();
			else if (target is RuleRow row)
				row.SelectNextControl(row, true, true, true, false);
			else
				target.Focus();
		}

		private void SaveAndClose()
		{
			if (!SaveAll())
				return;

			DialogResult = DialogResult.OK;
			Close();
		}

		private void ConditionEditorForm_FormClosing(object sender, FormClosingEventArgs e)
		{
			if (_saved)
				return;

			CaptureEditor();
			if (Snapshot() == _openedSnapshot)
			{
				DialogResult = DialogResult.Cancel;
				return;
			}

			var result = MessageBox.Show(this, "You have unsaved changes to your conditions. Save them before closing?",
				"Unsaved Changes", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Warning);

			switch (result)
			{
				case DialogResult.Yes:
					if (!SaveAll())
					{
						e.Cancel = true;
						return;
					}
					DialogResult = DialogResult.OK;
					break;
				case DialogResult.No:
					DialogResult = DialogResult.Cancel;
					break;
				default:
					e.Cancel = true;
					break;
			}
		}

		protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
		{
			switch (keyData)
			{
				case Keys.Control | Keys.N:
					AddCondition();
					return true;
				case Keys.Control | Keys.D:
					DuplicateCurrent();
					return true;
				case Keys.Control | Keys.S:
					SaveAndClose();
					return true;
				case Keys.Control | Keys.F:
					filterBox.Inner.Focus();
					filterBox.Inner.SelectAll();
					return true;
			}
			return base.ProcessCmdKey(ref msg, keyData);
		}

		private void ShowPropertyReference()
		{
			var propertyInfoForm = Program.ServiceProvider.GetRequiredService<PropertyInfoForm>();
			propertyInfoForm.Show(this);
		}

		#endregion
	}
}
