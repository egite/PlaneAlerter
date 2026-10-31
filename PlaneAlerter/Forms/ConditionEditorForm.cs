using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using PlaneAlerter.Enums;
using PlaneAlerter.Models;
using PlaneAlerter.Services;

namespace PlaneAlerter.Forms {
	/// <summary>
	/// Master-detail editor for the condition list: conditions on the left, the selected one on the right
	/// </summary>
	internal partial class ConditionEditorForm :Form {
		private readonly IConditionManagerService _conditionManagerService;
		private readonly IStringFormatterService _stringFormatterService;
		private readonly IVrsEnumService _vrsEnumService;

		/// <summary>
		/// Working copy of the conditions, in display order
		/// </summary>
		private readonly List<Condition> _conditions = new();

		/// <summary>
		/// Serialised conditions as they were when the editor opened
		/// </summary>
		private readonly string _openedSnapshot;

		/// <summary>
		/// Index of the condition shown in the editor panel, -1 when none
		/// </summary>
		private int _selectedIndex = -1;

		/// <summary>
		/// True when the editor panel differs from the selected condition
		/// </summary>
		private bool _editorDirty;

		/// <summary>
		/// Suppresses change handlers while the editor is being populated
		/// </summary>
		private bool _loading;

		/// <summary>
		/// Set once the conditions have been written to disk
		/// </summary>
		private bool _saved;

		/// <summary>
		/// Condition id to select when the form opens
		/// </summary>
		private int? _pendingSelection;

		/// <summary>
		/// Index being dragged in the list, or -1
		/// </summary>
		private int _dragIndex = -1;
		private Point _dragStart;

		/// <summary>
		/// Alert types in combobox order
		/// </summary>
		private static readonly AlertType[] AlertTypes =
		{
			AlertType.First_and_Last_Contact, AlertType.First_Contact, AlertType.Last_Contact, AlertType.Disabled
		};

		private static readonly string[] AlertTypeLabels =
		{
			"First and last contact", "First contact only", "Last contact only", "Disabled"
		};

		/// <summary>
		/// Properties in the order they appear in the trigger dropdown (alphabetical by display name)
		/// </summary>
		private static readonly List<VrsProperty> SortedProperties = Enum.GetValues(typeof(VrsProperty)).Cast<VrsProperty>()
			.OrderBy(p => p.ToString().Replace('_', ' '), StringComparer.OrdinalIgnoreCase).ToList();

		/// <summary>
		/// Aircraft used for the subject previews
		/// </summary>
		private static readonly Aircraft SampleAircraft = BuildSampleAircraft();

		public ConditionEditorForm(IConditionManagerService conditionManagerService,
			IStringFormatterService stringFormatterService, IVrsEnumService vrsEnumService) {
			_conditionManagerService = conditionManagerService;
			_stringFormatterService = stringFormatterService;
			_vrsEnumService = vrsEnumService;

			InitializeComponent();
			Theme.Apply(this);
			Theme.MakePrimary(saveButton);
			Theme.MakeHeader(conditionsHeaderLabel);
			Theme.MakeHeader(triggersHeaderLabel);
			errorLabel.ForeColor = Theme.LogError;
			firstPreviewLabel.ForeColor = Theme.TextMuted;
			lastPreviewLabel.ForeColor = Theme.TextMuted;
			firstPreviewLabel.Font = new Font(Font, FontStyle.Italic);
			lastPreviewLabel.Font = new Font(Font, FontStyle.Italic);

			alertTypeComboBox.Items.AddRange(AlertTypeLabels.Cast<object>().ToArray());

			//Load a working copy of the conditions
			foreach (var key in _conditionManagerService.Conditions.Keys.OrderBy(k => k))
				_conditions.Add(_conditionManagerService.Conditions[key]);
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
			var select = _conditions.Count == 0 ? -1 : Math.Clamp(_pendingSelection ?? 0, 0, _conditions.Count - 1);
			RefreshList(select);
		}

		private string Snapshot() => JsonConvert.SerializeObject(_conditions);

		#region Condition list

		/// <summary>
		/// Rebuild the list box and select an index, loading it into the editor
		/// </summary>
		private void RefreshList(int selectIndex)
		{
			_loading = true;
			conditionsListBox.BeginUpdate();
			conditionsListBox.Items.Clear();
			foreach (var condition in _conditions)
				conditionsListBox.Items.Add(DescribeForList(condition));
			conditionsListBox.EndUpdate();
			_loading = false;

			conditionsHeaderLabel.Text = _conditions.Count == 0 ? "Conditions" : $"Conditions ({_conditions.Count})";

			if (selectIndex >= 0 && selectIndex < _conditions.Count)
			{
				_selectedIndex = -1; //force a load even if the index did not change
				conditionsListBox.SelectedIndex = selectIndex;
				if (_selectedIndex != selectIndex)
					LoadEditor(selectIndex);
			}
			else
			{
				_selectedIndex = -1;
				LoadEditor(-1);
			}

			UpdateListButtons();
		}

		private static string DescribeForList(Condition condition)
		{
			var name = string.IsNullOrWhiteSpace(condition.Name) ? "(unnamed)" : condition.Name;
			return condition.AlertType == AlertType.Disabled ? name + "  (disabled)" : name;
		}

		private void UpdateListButtons()
		{
			var hasSelection = _selectedIndex >= 0;
			duplicateButton.Enabled = hasSelection;
			removeButton.Enabled = hasSelection;
			moveUpButton.Enabled = hasSelection && _selectedIndex > 0;
			moveDownButton.Enabled = hasSelection && _selectedIndex < _conditions.Count - 1;
		}

		private void conditionsListBox_SelectedIndexChanged(object sender, EventArgs e)
		{
			if (_loading)
				return;

			var newIndex = conditionsListBox.SelectedIndex;
			if (newIndex == _selectedIndex)
				return;

			//Commit the condition being left before moving on
			if (_selectedIndex >= 0 && _editorDirty && !TryCommitEditor(_selectedIndex))
			{
				_loading = true;
				conditionsListBox.SelectedIndex = _selectedIndex;
				_loading = false;
				return;
			}

			LoadEditor(newIndex);
			UpdateListButtons();
		}

		private void addButton_Click(object sender, EventArgs e)
		{
			if (!CommitIfDirty())
				return;

			_conditions.Add(new Condition("New condition", AlertType.First_and_Last_Contact, false, false, true,
				"First Contact Alert! [ConditionName]: [Reg]", "Last Contact Alert! [ConditionName]: [Reg]",
				new List<string>()));

			RefreshList(_conditions.Count - 1);
			conditionNameTextBox.Focus();
			conditionNameTextBox.SelectAll();
		}

		private void duplicateButton_Click(object sender, EventArgs e)
		{
			if (_selectedIndex < 0 || !CommitIfDirty())
				return;

			var source = _conditions[_selectedIndex];
			var copy = new Condition(source.Name + " (copy)", source.AlertType, source.IgnoreFollowing,
				source.TriggersUseOrLogic, source.EmailEnabled, source.EmailFirstFormat, source.EmailLastFormat,
				new List<string>(source.ReceiverEmails))
			{
				Triggers = source.Triggers.ToDictionary(t => t.Key,
					t => new Trigger(t.Value.Property, t.Value.Value, t.Value.ComparisonType))
			};

			_conditions.Insert(_selectedIndex + 1, copy);
			RefreshList(_selectedIndex + 1);
		}

		private void removeButton_Click(object sender, EventArgs e)
		{
			if (_selectedIndex < 0)
				return;

			var name = _conditions[_selectedIndex].Name;
			if (MessageBox.Show(this, $"Remove the condition \"{name}\"?", "Remove Condition",
				    MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes)
				return;

			var removedIndex = _selectedIndex;
			_conditions.RemoveAt(removedIndex);
			_editorDirty = false;
			RefreshList(Math.Min(removedIndex, _conditions.Count - 1));
		}

		private void moveUpButton_Click(object sender, EventArgs e)
		{
			MoveSelected(-1);
		}

		private void moveDownButton_Click(object sender, EventArgs e)
		{
			MoveSelected(1);
		}

		private void MoveSelected(int delta)
		{
			var target = _selectedIndex + delta;
			if (_selectedIndex < 0 || target < 0 || target >= _conditions.Count || !CommitIfDirty())
				return;

			(_conditions[_selectedIndex], _conditions[target]) = (_conditions[target], _conditions[_selectedIndex]);
			RefreshList(target);
		}

		/// <summary>
		/// Commit the editor if it has changes. Returns false if validation blocked it.
		/// </summary>
		private bool CommitIfDirty()
		{
			return _selectedIndex < 0 || !_editorDirty || TryCommitEditor(_selectedIndex);
		}

		#endregion

		#region Drag to reorder

		private void conditionsListBox_MouseDown(object sender, MouseEventArgs e)
		{
			_dragIndex = e.Button == MouseButtons.Left ? conditionsListBox.IndexFromPoint(e.Location) : -1;
			_dragStart = e.Location;
		}

		private void conditionsListBox_MouseMove(object sender, MouseEventArgs e)
		{
			if (_dragIndex < 0 || e.Button != MouseButtons.Left)
				return;

			if (Math.Abs(e.X - _dragStart.X) < SystemInformation.DragSize.Width &&
			    Math.Abs(e.Y - _dragStart.Y) < SystemInformation.DragSize.Height)
				return;

			if (!CommitIfDirty())
			{
				_dragIndex = -1;
				return;
			}

			var index = _dragIndex;
			_dragIndex = -1;
			conditionsListBox.DoDragDrop(index, DragDropEffects.Move);
		}

		private void conditionsListBox_DragOver(object sender, DragEventArgs e)
		{
			e.Effect = e.Data?.GetDataPresent(typeof(int)) == true ? DragDropEffects.Move : DragDropEffects.None;
		}

		private void conditionsListBox_DragDrop(object sender, DragEventArgs e)
		{
			if (e.Data?.GetData(typeof(int)) is not int fromIndex)
				return;

			var point = conditionsListBox.PointToClient(new Point(e.X, e.Y));
			var toIndex = conditionsListBox.IndexFromPoint(point);
			if (toIndex < 0)
				toIndex = _conditions.Count - 1;

			if (fromIndex == toIndex || fromIndex < 0 || fromIndex >= _conditions.Count)
				return;

			var moved = _conditions[fromIndex];
			_conditions.RemoveAt(fromIndex);
			_conditions.Insert(toIndex, moved);
			RefreshList(toIndex);
		}

		#endregion

		#region Editor panel

		/// <summary>
		/// Show a condition in the editor panel (-1 clears and disables it)
		/// </summary>
		private void LoadEditor(int index)
		{
			_loading = true;
			ClearErrors();

			_selectedIndex = index;
			var enabled = index >= 0;
			editorPanel.Enabled = true;
			foreach (Control control in editorPanel.Controls)
			{
				if (control != cancelButton && control != saveButton && control != propertyInfoButton && control != errorLabel)
					control.Enabled = enabled;
			}

			ResetTriggerGrid();

			if (!enabled)
			{
				conditionNameTextBox.Text = "";
				receiverEmailTextBox.Text = "";
				emailFirstFormatTextBox.Text = "";
				emailLastFormatTextBox.Text = "";
				alertTypeComboBox.SelectedIndex = -1;
				errorLabel.ForeColor = Theme.TextMuted;
				errorLabel.Text = _conditions.Count == 0
					? "No conditions yet. Click Add to create one."
					: "Select a condition on the left to edit it.";
				firstPreviewLabel.Text = "";
				lastPreviewLabel.Text = "";
			}
			else
			{
				var c = _conditions[index];
				conditionNameTextBox.Text = c.Name;
				ignoreFollowingCheckbox.Checked = c.IgnoreFollowing;
				triggersLogicAnyRadioButton.Checked = c.TriggersUseOrLogic;
				triggersLogicAllRadioButton.Checked = !c.TriggersUseOrLogic;
				alertTypeComboBox.SelectedIndex = Math.Max(0, Array.IndexOf(AlertTypes, c.AlertType));
				emailCheckBox.Checked = c.EmailEnabled;
				emailFirstFormatTextBox.Text = c.EmailFirstFormat;
				emailLastFormatTextBox.Text = c.EmailLastFormat;
				receiverEmailTextBox.Text = string.Join(Environment.NewLine, c.ReceiverEmails);

				foreach (var trigger in c.Triggers.Values.OrderBy(t => t.Property.ToString()))
				{
					var rowIndex = triggerDataGridView.Rows.Add();
					var row = triggerDataGridView.Rows[rowIndex];
					InitNewRow(row);
					row.Cells[0].Value = DisplayName(trigger.Property);
					ConfigureRowForProperty(row, trigger.Property, trigger.ComparisonType, trigger.Value);
				}

				UpdateUiState();
				UpdatePreviews();
			}

			_loading = false;
			_editorDirty = false;
		}

		private void MarkDirty()
		{
			if (_loading)
				return;

			_editorDirty = true;
		}

		private void editorValueChanged(object sender, EventArgs e)
		{
			MarkDirty();
		}

		private void conditionNameTextBox_TextChanged(object sender, EventArgs e)
		{
			if (_loading)
				return;

			MarkDirty();
			UpdatePreviews();

			//Keep the list in step with the name being typed
			if (_selectedIndex >= 0 && _selectedIndex < conditionsListBox.Items.Count)
			{
				_loading = true;
				var name = string.IsNullOrWhiteSpace(conditionNameTextBox.Text) ? "(unnamed)" : conditionNameTextBox.Text;
				conditionsListBox.Items[_selectedIndex] = SelectedAlertType() == AlertType.Disabled ? name + "  (disabled)" : name;
				conditionsListBox.SelectedIndex = _selectedIndex;
				_loading = false;
			}
		}

		private void alertTypeComboBox_SelectedIndexChanged(object sender, EventArgs e)
		{
			if (_loading)
				return;

			MarkDirty();
			UpdateUiState();
			conditionNameTextBox_TextChanged(sender, e);
		}

		private void emailCheckBox_CheckedChanged(object sender, EventArgs e)
		{
			MarkDirty();
			UpdateUiState();
		}

		private void subjectFormat_TextChanged(object sender, EventArgs e)
		{
			MarkDirty();
			UpdatePreviews();
		}

		private AlertType SelectedAlertType()
		{
			var index = alertTypeComboBox.SelectedIndex;
			return index >= 0 && index < AlertTypes.Length ? AlertTypes[index] : AlertType.First_and_Last_Contact;
		}

		/// <summary>
		/// Update enabled state of the email controls based on the alert type and email toggle
		/// </summary>
		private void UpdateUiState() {
			if (_selectedIndex < 0)
				return;

			var alertType = SelectedAlertType();
			var email = emailCheckBox.Checked;

			emailFirstFormatTextBox.Enabled = email && alertType != AlertType.Last_Contact;
			insertFirstButton.Enabled = emailFirstFormatTextBox.Enabled;
			emailLastFormatTextBox.Enabled = email && alertType != AlertType.First_Contact;
			insertLastButton.Enabled = emailLastFormatTextBox.Enabled;
			receiverEmailTextBox.Enabled = email;
		}

		private void UpdatePreviews()
		{
			if (_selectedIndex < 0)
				return;

			var previewCondition = new Condition(conditionNameTextBox.Text, AlertType.Disabled, false, false, false, "", "", new List<string>());

			firstPreviewLabel.Text = Preview(emailFirstFormatTextBox.Text, previewCondition);
			lastPreviewLabel.Text = Preview(emailLastFormatTextBox.Text, previewCondition);
		}

		private string Preview(string format, Condition condition)
		{
			if (string.IsNullOrWhiteSpace(format))
				return "";

			try
			{
				return "e.g. " + _stringFormatterService.Format(format, SampleAircraft, condition);
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

		private void insertFirstButton_Click(object sender, EventArgs e)
		{
			ShowPlaceholderMenu(insertFirstButton, emailFirstFormatTextBox);
		}

		private void insertLastButton_Click(object sender, EventArgs e)
		{
			ShowPlaceholderMenu(insertLastButton, emailLastFormatTextBox);
		}

		private void ShowPlaceholderMenu(Control anchor, TextBox target)
		{
			var menu = new ContextMenuStrip();
			menu.Renderer = new ToolStripProfessionalRenderer();

			void Add(ToolStripDropDown parent, string token, string description)
			{
				var item = new ToolStripMenuItem($"[{token}]   {description}") { Tag = token };
				item.Click += (_, _) => InsertPlaceholder(target, (string)item.Tag!);
				parent.Items.Add(item);
			}

			Add(menu, "ConditionName", "Condition name");
			Add(menu, "RcvrName", "Receiver name");
			Add(menu, "Date", "Today's date");
			Add(menu, "Time", "Current time");
			menu.Items.Add(new ToolStripSeparator());

			//Common aircraft fields at the top level
			foreach (var property in new[] { VrsProperty.Registration, VrsProperty.Callsign, VrsProperty.Aircraft_Model_Icao,
				         VrsProperty.Aircraft_Model, VrsProperty.Operator, VrsProperty.Icao, VrsProperty.Altitude })
			{
				if (VrsProperties.VrsPropertyData.TryGetValue(property, out var data))
					Add(menu, data[2], DisplayName(property));
			}

			//Everything else under a submenu, alphabetical
			var more = new ToolStripMenuItem("More properties");
			foreach (var property in SortedProperties)
			{
				var data = VrsProperties.VrsPropertyData[property];
				Add(more.DropDown, data[2], DisplayName(property));
			}
			menu.Items.Add(new ToolStripSeparator());
			menu.Items.Add(more);

			menu.Show(anchor, new Point(0, anchor.Height));
		}

		private static void InsertPlaceholder(TextBox target, string token)
		{
			var text = $"[{token}]";
			var start = target.SelectionStart;
			target.Text = target.Text.Remove(start, target.SelectionLength).Insert(start, text);
			target.SelectionStart = start + text.Length;
			target.Focus();
		}

		#endregion

		#region Trigger grid

		private static string DisplayName(VrsProperty property) => property.ToString().Replace('_', ' ');

		private static VrsProperty? ParseProperty(string? displayName)
		{
			if (string.IsNullOrEmpty(displayName))
				return null;

			return Enum.TryParse<VrsProperty>(displayName.Replace(' ', '_'), out var property) ? property : null;
		}

		private void ResetTriggerGrid()
		{
			triggerDataGridView.Rows.Clear();
			foreach (DataGridViewRow row in triggerDataGridView.Rows)
				InitNewRow(row);
		}

		private void InitNewRow(DataGridViewRow row)
		{
			var propertyCell = (DataGridViewComboBoxCell)row.Cells[0];
			propertyCell.Items.Clear();
			foreach (var property in SortedProperties)
				propertyCell.Items.Add(DisplayName(property));

			row.Cells[1].ReadOnly = true;
			row.Cells[2].ReadOnly = true;
			row.Cells[3].Value = "";
		}

		/// <summary>
		/// Set up the comparison, value and type cells for a property
		/// </summary>
		private void ConfigureRowForProperty(DataGridViewRow row, VrsProperty property, string? comparisonType, string? value)
		{
			var data = VrsProperties.VrsPropertyData[property];
			var valueType = data[0];
			var supported = data[1];
			var internalName = data[2];

			var comparisonCell = (DataGridViewComboBoxCell)row.Cells[1];
			comparisonCell.Items.Clear();
			foreach (var group in new[] { "A", "B", "C", "D", "E" })
			{
				if (supported.Contains(group))
					foreach (var comparison in VrsProperties.ComparisonTypes[group])
						comparisonCell.Items.Add(comparison);
			}
			comparisonCell.ReadOnly = false;
			comparisonCell.Value = comparisonType != null && comparisonCell.Items.Contains(comparisonType)
				? comparisonType
				: comparisonCell.Items[0];

			var enumValues = _vrsEnumService.GetValues(internalName);

			if (valueType == "Boolean")
			{
				row.Cells[2] = new DataGridViewTextBoxCell { Value = "True" };
				row.Cells[2].ReadOnly = true;
				row.Cells[3].Value = "Yes/No";
			}
			else if (enumValues != null)
			{
				var cell = new DataGridViewComboBoxCell { FlatStyle = FlatStyle.Flat };
				foreach (var pair in enumValues.OrderBy(p => p.Key))
					cell.Items.Add($"{pair.Key} - {pair.Value}");
				row.Cells[2] = cell;
				row.Cells[2].ReadOnly = false;
				row.Cells[2].Value = int.TryParse(value, out var numeric) && enumValues.ContainsKey(numeric)
					? $"{numeric} - {enumValues[numeric]}"
					: cell.Items[0];
				row.Cells[3].Value = "List";
			}
			else
			{
				if (row.Cells[2] is not DataGridViewTextBoxCell)
					row.Cells[2] = new DataGridViewTextBoxCell();
				row.Cells[2].ReadOnly = false;
				row.Cells[2].Value = value ?? "";
				row.Cells[3].Value = valueType == "Number" ? "Number" : "Text";
			}
		}

		/// <summary>
		/// Read the value cell back as the string stored in the trigger
		/// </summary>
		private static string ReadTriggerValue(DataGridViewRow row)
		{
			var raw = row.Cells[2].Value?.ToString() ?? "";
			if (row.Cells[2] is DataGridViewComboBoxCell)
			{
				var dash = raw.IndexOf(" - ", StringComparison.Ordinal);
				return dash > 0 ? raw.Substring(0, dash) : raw;
			}
			return raw.Trim();
		}

		private void triggerDataGridView_CurrentCellDirtyStateChanged(object sender, EventArgs e)
		{
			//Commit combobox picks immediately so CellValueChanged fires on selection
			if (triggerDataGridView.IsCurrentCellDirty && triggerDataGridView.CurrentCell is DataGridViewComboBoxCell)
				triggerDataGridView.CommitEdit(DataGridViewDataErrorContexts.Commit);
		}

		private void triggerDataGridView_CellValueChanged(object sender, DataGridViewCellEventArgs e)
		{
			if (_loading || e.RowIndex < 0)
				return;

			MarkDirty();

			var row = triggerDataGridView.Rows[e.RowIndex];

			if (e.ColumnIndex == 0)
			{
				var property = ParseProperty(row.Cells[0].Value?.ToString());
				if (property == null)
				{
					((DataGridViewComboBoxCell)row.Cells[1]).Items.Clear();
					row.Cells[1].Value = null;
					row.Cells[1].ReadOnly = true;
					row.Cells[2].Value = "";
					row.Cells[2].ReadOnly = true;
					row.Cells[3].Value = "";
					return;
				}

				ConfigureRowForProperty(row, property.Value, null, null);
			}
			else if (e.ColumnIndex == 2 && row.Cells[2] is DataGridViewTextBoxCell)
			{
				//Flag non-numeric values for numeric properties
				var property = ParseProperty(row.Cells[0].Value?.ToString());
				if (property != null && VrsProperties.VrsPropertyData[property.Value][0] == "Number" &&
				    !string.IsNullOrEmpty(row.Cells[2].Value?.ToString()) &&
				    !double.TryParse(row.Cells[2].Value?.ToString(), out _))
				{
					row.Cells[2].ErrorText = "Must be a number";
				}
				else
					row.Cells[2].ErrorText = "";
			}
		}

		private void TriggerDataGridViewUserAddedRow(object sender, DataGridViewRowEventArgs e)
		{
			InitNewRow(e.Row);
		}

		private void triggerDataGridView_RowsRemoved(object sender, DataGridViewRowsRemovedEventArgs e)
		{
			MarkDirty();
		}

		private void triggerDataGridView_DataError(object sender, DataGridViewDataErrorEventArgs e)
		{
			//Swallow combobox value mismatches while cells are being swapped
			e.ThrowException = false;
		}

		private void triggerDataGridView_CellMouseClick(object sender, DataGridViewCellMouseEventArgs e) {
			if (e.RowIndex == -1 || e.ColumnIndex < 0)
				return;

			//Open combobox cells on a single click
			if (triggerDataGridView[e.ColumnIndex, e.RowIndex] is DataGridViewComboBoxCell cell && !cell.ReadOnly) {
				triggerDataGridView.CurrentCell = cell;
				triggerDataGridView.BeginEdit(true);

				if (triggerDataGridView.EditingControl is DataGridViewComboBoxEditingControl editingControl)
					editingControl.DroppedDown = true;
			}
		}

		private void triggerDataGridView_CellContentClick(object sender, DataGridViewCellEventArgs e)
		{
			//Delete row on delete button cell click
			if (e.ColumnIndex == 4 && e.RowIndex >= 0 && !triggerDataGridView.Rows[e.RowIndex].IsNewRow)
			{
				triggerDataGridView.EndEdit();
				triggerDataGridView.Rows.RemoveAt(e.RowIndex);
			}
		}

		#endregion

		#region Validation and commit

		private void ClearErrors()
		{
			errorProvider.Clear();
			errorLabel.Text = "";
			errorLabel.ForeColor = Theme.LogError;
		}

		/// <summary>
		/// Validate the editor panel and, if valid, replace the condition at the index with its contents
		/// </summary>
		private bool TryCommitEditor(int index)
		{
			triggerDataGridView.EndEdit();
			ClearErrors();

			var errors = new List<string>();

			var name = conditionNameTextBox.Text.Trim();
			if (name.Length == 0)
			{
				errorProvider.SetError(conditionNameTextBox, "A name is required");
				errors.Add("Condition name is required");
			}

			var alertType = SelectedAlertType();

			//Triggers
			var triggers = new Dictionary<int, Trigger>();
			foreach (DataGridViewRow row in triggerDataGridView.Rows)
			{
				if (row.IsNewRow)
					continue;

				var property = ParseProperty(row.Cells[0].Value?.ToString());
				var comparison = row.Cells[1].Value?.ToString();

				if (property == null)
				{
					errors.Add($"Trigger {row.Index + 1}: choose a property");
					continue;
				}
				if (string.IsNullOrEmpty(comparison))
				{
					errors.Add($"Trigger {row.Index + 1}: choose a comparison");
					continue;
				}

				var value = ReadTriggerValue(row);
				var valueType = VrsProperties.VrsPropertyData[property.Value][0];
				if (valueType == "Number" && (comparison == "Higher Than" || comparison == "Lower Than") &&
				    !double.TryParse(value, out _))
					errors.Add($"Trigger {row.Index + 1}: {DisplayName(property.Value)} needs a numeric value");

				triggers.Add(triggers.Count, new Trigger(property.Value, value, comparison));
			}

			if (triggers.Count == 0)
			{
				errorProvider.SetError(triggerDataGridView, "Add at least one trigger");
				errors.Add("At least one trigger is required");
			}

			//Email
			var recipients = receiverEmailTextBox.Lines.Select(l => l.Trim()).Where(l => l.Length > 0).ToList();
			if (emailCheckBox.Checked)
			{
				if (alertType != AlertType.Last_Contact && string.IsNullOrWhiteSpace(emailFirstFormatTextBox.Text))
				{
					errorProvider.SetError(emailFirstFormatTextBox, "Required");
					errors.Add("First contact subject is required");
				}
				if (alertType != AlertType.First_Contact && string.IsNullOrWhiteSpace(emailLastFormatTextBox.Text))
				{
					errorProvider.SetError(emailLastFormatTextBox, "Required");
					errors.Add("Last contact subject is required");
				}
				if (recipients.Count == 0)
				{
					errorProvider.SetError(receiverEmailTextBox, "Add at least one address");
					errors.Add("At least one recipient email is required");
				}
				else
				{
					var invalid = recipients.Where(r => !IsValidEmail(r)).ToList();
					if (invalid.Count > 0)
					{
						errorProvider.SetError(receiverEmailTextBox, "Invalid: " + string.Join(", ", invalid));
						errors.Add($"Invalid email address: {string.Join(", ", invalid)}");
					}
				}
			}

			if (errors.Count > 0)
			{
				errorLabel.Text = errors.Count == 1
					? errors[0]
					: $"{errors.Count} problems: " + string.Join("; ", errors);
				toolTip.SetToolTip(errorLabel, string.Join(Environment.NewLine, errors));
				return false;
			}

			_conditions[index] = new Condition(name, alertType, ignoreFollowingCheckbox.Checked,
				triggersLogicAnyRadioButton.Checked, emailCheckBox.Checked,
				emailFirstFormatTextBox.Text, emailLastFormatTextBox.Text, recipients)
			{
				Triggers = triggers
			};

			_editorDirty = false;
			return true;
		}

		private static bool IsValidEmail(string address)
		{
			try
			{
				_ = new System.Net.Mail.MailAddress(address);
				return true;
			}
			catch (FormatException)
			{
				return false;
			}
		}

		/// <summary>
		/// Write the working copy to disk
		/// </summary>
		private bool SaveAll()
		{
			if (!CommitIfDirty())
			{
				if (_selectedIndex >= 0)
					conditionsListBox.SelectedIndex = _selectedIndex;
				return false;
			}

			var sorted = new SortedDictionary<int, Condition>();
			for (var i = 0; i < _conditions.Count; i++)
				sorted.Add(i, _conditions[i]);

			_conditionManagerService.EditorConditions = sorted;
			_conditionManagerService.SaveEditorConditions();
			_saved = true;
			return true;
		}

		private void saveButton_Click(object sender, EventArgs e)
		{
			if (!SaveAll())
				return;

			DialogResult = DialogResult.OK;
			Close();
		}

		private void cancelButton_Click(object sender, EventArgs e)
		{
			Close();
		}

		private void ConditionEditorForm_FormClosing(object sender, FormClosingEventArgs e)
		{
			if (_saved)
				return;

			var changed = _editorDirty || Snapshot() != _openedSnapshot;
			if (!changed)
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

		private void propertyInfoButton_Click(object sender, EventArgs e) {
			var propertyInfoForm = Program.ServiceProvider.GetRequiredService<PropertyInfoForm>();
			propertyInfoForm.Show(this);
		}

		#endregion
	}
}
