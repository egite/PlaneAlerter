using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Media;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows.Forms;
using Microsoft.Extensions.DependencyInjection;
using PlaneAlerter.Controls;
using PlaneAlerter.Enums;
using PlaneAlerter.Models;
using PlaneAlerter.Services;

namespace PlaneAlerter.Forms
{
	/// <summary>
	/// Main form
	/// </summary>
	internal partial class MainForm : Form
	{
		private readonly ISettingsManagerService _settingsManagerService;
		private readonly IConditionManagerService _conditionManagerService;
		private readonly IStatsService _statsService;
		private readonly IThreadManagerService _threadManagerService;
		private readonly ICheckerService _checkerService;
		private readonly ILoggerWithQueue _logger;

		/// <summary>
		/// Maximum lines kept in the console history before the oldest are dropped
		/// </summary>
		private const int MaxConsoleLines = 4000;

		/// <summary>
		/// Every message logged this session (capped), so the console can be re-filtered
		/// </summary>
		private readonly List<LogMessage> _logHistory = new();

		/// <summary>
		/// Lines currently shown in the console
		/// </summary>
		private int _consoleLineCount;

		/// <summary>
		/// Set while the window is hidden to the tray
		/// </summary>
		private bool _hiddenToTray;

		private const int WM_USER = 0x400;
		private const int EM_GETSCROLLPOS = WM_USER + 221;
		private const int EM_SETSCROLLPOS = WM_USER + 222;

		[DllImport("user32.dll")]
		private static extern int SendMessage(IntPtr hWnd, int wMsg, int wParam, ref Point lParam);

		/// <summary>
		/// Flash a window
		/// </summary>
		[DllImport("user32.dll")]
		[return: MarshalAs(UnmanagedType.Bool)]
		private static extern bool FlashWindowEx(ref FLASHWINFO pwfi);

		private const uint FLASHW_ALL = 3;
		private const uint FLASHW_TIMERNOFG = 12;

		[StructLayout(LayoutKind.Sequential)]
		private struct FLASHWINFO
		{
			public uint cbSize;
			public IntPtr hwnd;
			public uint dwFlags;
			public uint uCount;
			public uint dwTimeout;
		}

		/// <summary>
		/// Flash a window
		/// </summary>
		/// <param name="form">The form to flash</param>
		private static bool FlashWindowEx(Form form)
		{
			var fInfo = new FLASHWINFO();

			fInfo.cbSize = Convert.ToUInt32(Marshal.SizeOf(fInfo));
			fInfo.hwnd = form.Handle;
			fInfo.dwFlags = FLASHW_ALL | FLASHW_TIMERNOFG;
			fInfo.uCount = uint.MaxValue;
			fInfo.dwTimeout = 0;

			return FlashWindowEx(ref fInfo);
		}

		/// <summary>
		/// Constructor
		/// </summary>
		public MainForm(ISettingsManagerService settingsManagerService, IConditionManagerService conditionManagerService,
			IStatsService statsService, IThreadManagerService threadManagerService,
			ICheckerService checkerService, ILoggerWithQueue logger) {
			_settingsManagerService = settingsManagerService;
			_conditionManagerService = conditionManagerService;
			_statsService = statsService;
			_threadManagerService = threadManagerService;
			_checkerService = checkerService;
			_logger = logger;

			//Set culture to invariant
			Thread.CurrentThread.CurrentCulture = System.Globalization.CultureInfo.InvariantCulture;

			//Initialise UI components
			InitializeComponent();
			Theme.Apply(this);
			Theme.MakePrimary(startStopButton);
			Theme.MakePrimary(addConditionButton);
			Theme.MakeHeader(conditionsHeaderLabel);
			Theme.MakeHeader(activeAlertsLabel);
			Theme.MakeHeader(consoleLabel);
			emptyStateLabel.ForeColor = Theme.TextMuted;
			consoleFilterComboBox.SelectedIndex = 0;
			SetUpConditionToggles();

			//Services raise these from the checker thread, so every handler hops to the UI thread
			_logger.NewLogMessage += (_, _) => OnUi(FlushLogQueue);
			_checkerService.StatusChanged += (_, status) => OnUi(() => UpdateStatusLabel(status));
			_threadManagerService.StatusChanged += (_, _) => OnUi(UpdateCheckerControls);
			_checkerService.SendingAlert += (condition, aircraft, _, _) => OnUi(() => NotifyAlert(condition, aircraft));
			_statsService.StatsChanged += (_, _) => OnUi(UpdateStats);

			//When shown, wait until everything has loaded then start threads
			Shown += delegate
			{
				RestoreLayout();

				_settingsManagerService.Load();
				_conditionManagerService.LoadConditions();

				UpdateConditionList();
				UpdateStats();
				UpdateCheckerControls();
				uiTimer.Start();

				if (_settingsManagerService.Settings.StartOnStart && _conditionManagerService.Conditions.Count > 0)
					_threadManagerService.Start();
			};
		}

		/// <summary>
		/// Run an action on the UI thread, ignoring it if the form is gone
		/// </summary>
		private void OnUi(Action action)
		{
			if (IsDisposed || !IsHandleCreated)
				return;

			if (InvokeRequired)
			{
				try
				{
					BeginInvoke(action);
				}
				catch (ObjectDisposedException)
				{
				}
				catch (InvalidOperationException)
				{
				}
			}
			else
				action();
		}

		#region Checker state

		private void UpdateCheckerControls()
		{
			switch (_threadManagerService.ThreadStatus)
			{
				case CheckerStatus.Stopped:
					startStopButton.Text = "Start";
					startStopButton.Enabled = true;
					restartButton.Enabled = false;
					trayStartStopMenuItem.Text = "Start";
					trayStartStopMenuItem.Enabled = true;
					UpdateStatusLabel("Idle");
					UpdateStats();
					break;
				case CheckerStatus.Running:
					startStopButton.Text = "Stop";
					startStopButton.Enabled = true;
					restartButton.Enabled = true;
					trayStartStopMenuItem.Text = "Stop";
					trayStartStopMenuItem.Enabled = true;
					break;
				case CheckerStatus.Stopping:
					startStopButton.Text = "Stopping...";
					startStopButton.Enabled = false;
					restartButton.Enabled = false;
					trayStartStopMenuItem.Enabled = false;
					break;
			}

			notifyIcon.Text = "PlaneAlerter - " + (_threadManagerService.ThreadStatus == CheckerStatus.Running ? "running" : "stopped");
		}

		/// <summary>
		/// Updates the status label
		/// </summary>
		/// <param name="text">Text to update label with</param>
		private void UpdateStatusLabel(string text) {
			statusLabel.Text = text;
		}

		private void uiTimer_Tick(object sender, EventArgs e)
		{
			if (_threadManagerService.ThreadStatus == CheckerStatus.Running)
			{
				var remaining = _checkerService.NextCheck - DateTime.Now;
				nextCheckLabel.Text = remaining.TotalSeconds > 0
					? $"Next check in {Math.Ceiling(remaining.TotalSeconds):0}s"
					: "Checking...";
				aircraftCountLabel.Text = $"Aircraft on radar: {_checkerService.LastAircraftCount}";
			}
			else
			{
				nextCheckLabel.Text = "Next check: —";
				aircraftCountLabel.Text = "Aircraft on radar: —";
			}

			var uptime = DateTime.Now - _statsService.TimeStarted;
			uptimeLabel.Text = uptime.TotalHours >= 24
				? $"Up {(int)uptime.TotalDays}d {uptime.Hours}h"
				: uptime.TotalHours >= 1
					? $"Up {(int)uptime.TotalHours}h {uptime.Minutes:00}m"
					: $"Up {uptime.Minutes}m";

			//Signal-lost ages change every second
			if (_checkerService.ActiveMatches.Values.Any(m => m.SignalLost))
				UpdateActiveMatches();
		}

		private void NotifyAlert(Condition condition, Aircraft aircraft)
		{
			//Make a ding noise
			if (_settingsManagerService.Settings.SoundAlerts)
				SystemSounds.Exclamation.Play();

			//Show a notification
			if (_settingsManagerService.Settings.ShowNotifications)
				notifyIcon.ShowBalloonTip(5000, "Plane Alert!",
					$"Condition: {condition.Name}\nAircraft: {aircraft.GetProperty("Icao")} | {aircraft.GetProperty("Reg")} | {aircraft.GetProperty("Type")} | {aircraft.GetProperty("Call")}",
					ToolTipIcon.Info);

			//Flash the window
			if (_settingsManagerService.Settings.FlashWindow)
				FlashWindowEx(this);
		}

		#endregion

		#region Console

		/// <summary>
		/// Pull queued log messages from the logger into the history and console
		/// </summary>
		private void FlushLogQueue()
		{
			var messages = _logger.DequeueLogsForUi(int.MaxValue).ToList();
			if (messages.Count == 0)
				return;

			foreach (var message in messages)
			{
				_logHistory.Add(message);
				if (MessagePassesFilter(message))
					AppendToConsole(message);
			}

			if (_logHistory.Count > MaxConsoleLines)
				_logHistory.RemoveRange(0, _logHistory.Count - MaxConsoleLines);
		}

		/// <summary>
		/// Which messages the console filter lets through
		/// </summary>
		private bool MessagePassesFilter(LogMessage message)
		{
			var argb = message.Color?.ToArgb();
			var isError = argb == Color.Red.ToArgb();
			var isWarning = argb == Color.Orange.ToArgb();
			var isAlertActivity = argb == Color.LightGreen.ToArgb() || argb == Color.LightBlue.ToArgb() ||
			                      argb == Color.LightGoldenrodYellow.ToArgb();

			return consoleFilterComboBox.SelectedIndex switch
			{
				1 => isAlertActivity || isWarning,
				2 => isError || isWarning,
				_ => true
			};
		}

		private void AppendToConsole(LogMessage message)
		{
			try
			{
				//Keep the console from growing without bound
				if (++_consoleLineCount > MaxConsoleLines)
				{
					var trimTo = console.GetFirstCharIndexFromLine(MaxConsoleLines / 4);
					if (trimTo > 0)
					{
						console.Select(0, trimTo);
						console.SelectedText = "";
						_consoleLineCount -= MaxConsoleLines / 4;
					}
				}

				var scrollPos = Point.Empty;
				if (!autoScrollCheckBox.Checked)
					SendMessage(console.Handle, EM_GETSCROLLPOS, 0, ref scrollPos);

				console.Select(console.TextLength, 0);
				console.SelectionColor = Theme.LogColor(message.Color);
				console.AppendText(message.Message + Environment.NewLine);

				if (autoScrollCheckBox.Checked)
					console.ScrollToCaret();
				else
					SendMessage(console.Handle, EM_SETSCROLLPOS, 0, ref scrollPos);
			}
			catch (Exception)
			{
			}
		}

		/// <summary>
		/// Redraw the console from history using the current filter
		/// </summary>
		private void RebuildConsole()
		{
			console.SuspendLayout();
			console.Clear();
			_consoleLineCount = 0;
			foreach (var message in _logHistory.Where(MessagePassesFilter))
				AppendToConsole(message);
			console.ResumeLayout();
		}

		private void consoleFilterComboBox_SelectedIndexChanged(object sender, EventArgs e)
		{
			if (IsHandleCreated)
				RebuildConsole();
		}

		private void copyConsoleButton_Click(object sender, EventArgs e)
		{
			if (console.TextLength == 0)
				return;

			try
			{
				Clipboard.SetText(console.Text);
			}
			catch (Exception)
			{
			}
		}

		private void clearConsole_Click(object sender, EventArgs e) {
			console.Clear();
			_logHistory.Clear();
			_consoleLineCount = 0;
		}

		/// <summary>
		/// Link clicked in console
		/// </summary>
		private void ConsoleLinkClicked(object sender, LinkClickedEventArgs e)
		{
			if (string.IsNullOrEmpty(e.LinkText))
				return;

			OpenUrl(e.LinkText);
		}

		#endregion

		#region Conditions and matches

		/// <summary>
		/// Updates the condition list
		/// </summary>
		private void UpdateConditionList() {
			conditionsGrid.Rows.Clear();

			foreach (var conditionId in _conditionManagerService.Conditions.Keys.OrderBy(x => x)) {
				var c = _conditionManagerService.Conditions[conditionId];

				var triggerSummary = string.Join(c.TriggersUseOrLogic ? "  or  " : "  and  ",
					c.Triggers.Values.Select(t => $"{t.Property.ToString().Replace('_', ' ')} {t.ComparisonType.ToLowerInvariant()} {t.Value}"));

				var rowIndex = conditionsGrid.Rows.Add(
					c.Enabled,
					c.Name,
					DescribeAlertType(c.AlertType),
					triggerSummary,
					c.EmailEnabled,
					c.AlertsThisSession);

				var row = conditionsGrid.Rows[rowIndex];
				row.Tag = conditionId;
				row.Cells[conditionTriggersColumn.Index].ToolTipText = triggerSummary.Replace("  ", " ");
				StyleConditionRow(row, c);
			}

			var hasConditions = conditionsGrid.Rows.Count > 0;
			conditionsGrid.Visible = hasConditions;
			emptyStatePanel.Visible = !hasConditions;
			conditionsHeaderLabel.Text = hasConditions ? $"Conditions ({conditionsGrid.Rows.Count})" : "Conditions";
		}

		#region Condition on/off switches

		private readonly DataGridViewColumn _conditionEnabledColumn = new DataGridViewTextBoxColumn
		{
			Name = "conditionEnabledColumn",
			HeaderText = "",
			AutoSizeMode = DataGridViewAutoSizeColumnMode.None,
			Resizable = DataGridViewTriState.False,
			SortMode = DataGridViewColumnSortMode.NotSortable,
			ReadOnly = true,
			ToolTipText = "Turn conditions on or off"
		};

		//Cell under the mouse when it is over a switch, column -1 for none
		private (int Column, int Row) _hoverToggle = (-1, -1);

		/// <summary>
		/// Add a column of on/off switches to the start of the condition list, and turn the
		/// email column into switches
		/// </summary>
		private void SetUpConditionToggles()
		{
			_conditionEnabledColumn.Width = S(52);
			conditionsGrid.Columns.Insert(0, _conditionEnabledColumn);

			conditionRecipientsColumn.AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
			conditionRecipientsColumn.Width = S(78);
			conditionRecipientsColumn.ToolTipText = "Turn email alerts on or off";

			conditionsGrid.CellPainting += ConditionsGrid_CellPainting;
			conditionsGrid.CellMouseClick += (_, e) =>
			{
				if (e.Button == MouseButtons.Left && e.RowIndex >= 0 && IsToggleColumn(e.ColumnIndex))
					Toggle(e.ColumnIndex, e.RowIndex);
			};
			conditionsGrid.CellMouseEnter += (_, e) =>
				SetHoverToggle(e.RowIndex >= 0 && IsToggleColumn(e.ColumnIndex) ? (e.ColumnIndex, e.RowIndex) : (-1, -1));
			conditionsGrid.MouseLeave += (_, _) => SetHoverToggle((-1, -1));
			conditionsGrid.KeyDown += (_, e) =>
			{
				if (e.KeyCode != Keys.Space || conditionsGrid.CurrentRow == null)
					return;
				Toggle(_conditionEnabledColumn.Index, conditionsGrid.CurrentRow.Index);
				e.Handled = true;
			};
		}

		private static int S(int pixels) => Ui.S(pixels);

		private bool IsToggleColumn(int column) =>
			column == _conditionEnabledColumn.Index || column == conditionRecipientsColumn.Index;

		private void SetHoverToggle((int Column, int Row) cell)
		{
			if (_hoverToggle == cell)
				return;

			var old = _hoverToggle;
			_hoverToggle = cell;
			conditionsGrid.Cursor = cell.Column >= 0 ? Cursors.Hand : Cursors.Default;
			if (old.Column >= 0 && old.Row < conditionsGrid.Rows.Count)
				conditionsGrid.InvalidateCell(old.Column, old.Row);
			if (cell.Column >= 0)
				conditionsGrid.InvalidateCell(cell.Column, cell.Row);
		}

		private bool TryGetCondition(int rowIndex, out DataGridViewRow row, out Condition condition)
		{
			row = null!;
			condition = null!;
			if (rowIndex < 0 || rowIndex >= conditionsGrid.Rows.Count)
				return false;

			row = conditionsGrid.Rows[rowIndex];
			return row.Tag is int conditionId &&
			       _conditionManagerService.Conditions.TryGetValue(conditionId, out condition!);
		}

		private void Toggle(int column, int rowIndex)
		{
			if (!TryGetCondition(rowIndex, out var row, out var condition))
				return;

			string message;
			if (column == conditionRecipientsColumn.Index)
			{
				condition.EmailEnabled = !condition.EmailEnabled;
				message = $"Email alerts for \"{condition.Name}\" turned {(condition.EmailEnabled ? "on" : "off")}";
			}
			else
			{
				condition.Enabled = !condition.Enabled;
				message = $"Condition \"{condition.Name}\" turned {(condition.Enabled ? "on" : "off")}";
			}

			row.Cells[_conditionEnabledColumn.Index].Value = condition.Enabled;
			row.Cells[conditionRecipientsColumn.Index].Value = condition.EmailEnabled;
			StyleConditionRow(row, condition);

			try
			{
				_conditionManagerService.SaveConditions();
			}
			catch (Exception ex)
			{
				_logger.Log($"Could not save conditions: {ex.Message}", Color.Red);
			}

			_logger.Log(message, Color.White);
		}

		private void StyleConditionRow(DataGridViewRow row, Condition condition)
		{
			row.DefaultCellStyle.ForeColor = condition.Enabled ? Color.Empty : Theme.TextMuted;
			row.Cells[_conditionEnabledColumn.Index].ToolTipText = condition.Enabled
				? "On: click to stop checking this condition"
				: "Off: click to start checking this condition";

			var recipients = condition.ReceiverEmails.Count == 0
				? "No recipients set"
				: string.Join(Environment.NewLine, condition.ReceiverEmails);
			row.Cells[conditionRecipientsColumn.Index].ToolTipText = condition.EmailEnabled
				? $"Emails on: click to stop emailing{Environment.NewLine}{recipients}"
				: $"Emails off: click to start emailing{Environment.NewLine}{recipients}";

			conditionsGrid.InvalidateRow(row.Index);
		}

		/// <summary>
		/// Draw the on/off switch cells, matching the switches in the condition editor. The email
		/// switch has the number of recipients beside it.
		/// </summary>
		private void ConditionsGrid_CellPainting(object? sender, DataGridViewCellPaintingEventArgs e)
		{
			if (e.RowIndex < 0 || !IsToggleColumn(e.ColumnIndex) || e.Graphics == null)
				return;

			var selected = (e.State & DataGridViewElementStates.Selected) != 0;
			e.PaintBackground(e.CellBounds, selected);

			var on = e.Value is true;
			var hover = _hoverToggle == (e.ColumnIndex, e.RowIndex);
			var trackWidth = S(34);
			var trackHeight = S(18);
			var isEmail = e.ColumnIndex == conditionRecipientsColumn.Index;
			var trackX = isEmail
				? e.CellBounds.X + S(8)
				: e.CellBounds.X + (e.CellBounds.Width - trackWidth) / 2;
			var track = new Rectangle(trackX,
				e.CellBounds.Y + (e.CellBounds.Height - trackHeight) / 2,
				trackWidth, trackHeight);

			if (isEmail && TryGetCondition(e.RowIndex, out _, out var condition))
			{
				var countRect = new Rectangle(track.Right + S(6), e.CellBounds.Y,
					e.CellBounds.Right - track.Right - S(8), e.CellBounds.Height);
				var countColor = selected ? Theme.TextOnNavy : on && condition.Enabled ? Theme.Text : Theme.TextMuted;
				TextRenderer.DrawText(e.Graphics, condition.ReceiverEmails.Count.ToString(), e.CellStyle?.Font ?? conditionsGrid.Font,
					countRect, countColor, TextFormatFlags.Left | TextFormatFlags.VerticalCenter | TextFormatFlags.SingleLine);
			}

			Color fill;
			if (selected)
				fill = on ? Theme.Navy : Color.FromArgb(140, Color.White);
			else
				fill = on ? (hover ? Theme.Navy : Theme.Sky) : (hover ? Theme.TextMuted : Theme.Border);

			var g = e.Graphics;
			var oldMode = g.SmoothingMode;
			g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;

			using (var path = Ui.Rounded(track, trackHeight / 2))
			using (var brush = new SolidBrush(fill))
				g.FillPath(brush, path);

			var knob = trackHeight - S(6);
			var knobX = on ? track.Right - knob - S(3) : track.X + S(3);
			g.FillEllipse(Brushes.White, knobX, track.Y + S(3), knob, knob);

			g.SmoothingMode = oldMode;
			e.Handled = true;
		}

		#endregion

		private static string DescribeAlertType(AlertType alertType)
		{
			return alertType switch
			{
				AlertType.Disabled => "Disabled",
				AlertType.First_Contact => "First contact",
				AlertType.Last_Contact => "Last contact",
				AlertType.First_and_Last_Contact => "First and last",
				_ => alertType.ToString()
			};
		}

		/// <summary>
		/// Update the UI with all the stats
		/// </summary>
		private void UpdateStats()
		{
			if (IsDisposed)
				return;

			try
			{
				alertsSentLabel.Text = $"Alerts sent: {_statsService.TotalAlertsSent}";

				foreach (DataGridViewRow row in conditionsGrid.Rows)
				{
					if (row.Tag is int conditionId &&
					    _conditionManagerService.Conditions.TryGetValue(conditionId, out var condition))
						row.Cells[conditionAlertsColumn.Index].Value = condition.AlertsThisSession;
				}

				UpdateActiveMatches();
			}
			catch (Exception)
			{
			}
		}

		/// <summary>
		/// Rebuild the active matches grid, keeping the selected aircraft selected
		/// </summary>
		private void UpdateActiveMatches()
		{
			var selectedIcao = activeMatchesGrid.CurrentRow?.Tag as string;
			var firstDisplayed = activeMatchesGrid.FirstDisplayedScrollingRowIndex;

			activeMatchesGrid.SuspendLayout();
			activeMatchesGrid.Rows.Clear();

			foreach (var match in _checkerService.ActiveMatches.Values.ToList().OrderByDescending(m => m.MatchedTime))
			{
				var status = match.SignalLost
					? $"Signal lost {FormatAge(DateTime.Now - match.SignalLostTime)} ago"
					: "Tracking";

				var rowIndex = activeMatchesGrid.Rows.Add(match.Icao,
					match.AircraftInfo.GetProperty("Reg"),
					match.AircraftInfo.GetProperty("Type"),
					match.AircraftInfo.GetProperty("Call"),
					string.Join(", ", match.Conditions.Select(c => c.Condition.Name)),
					match.MatchedTime.ToString("HH:mm:ss"),
					status);

				var row = activeMatchesGrid.Rows[rowIndex];
				row.Tag = match.Icao;
				if (match.SignalLost)
					row.DefaultCellStyle.ForeColor = Theme.TextMuted;
			}

			if (selectedIcao != null)
			{
				foreach (DataGridViewRow row in activeMatchesGrid.Rows)
				{
					if (row.Tag as string != selectedIcao)
						continue;

					row.Selected = true;
					activeMatchesGrid.CurrentCell = row.Cells[0];
					break;
				}
			}

			if (firstDisplayed >= 0 && firstDisplayed < activeMatchesGrid.Rows.Count)
				activeMatchesGrid.FirstDisplayedScrollingRowIndex = firstDisplayed;

			activeMatchesGrid.ResumeLayout();

			activeAlertsLabel.Text = $"Active alerts ({_checkerService.ActiveMatches.Count})";
		}

		private static string FormatAge(TimeSpan age)
		{
			if (age.TotalSeconds < 60) return $"{(int)age.TotalSeconds}s";
			if (age.TotalMinutes < 60) return $"{(int)age.TotalMinutes}m {age.Seconds:00}s";
			return $"{(int)age.TotalHours}h {age.Minutes:00}m";
		}

		private void conditionsGrid_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
		{
			if (e.RowIndex < 0 || IsToggleColumn(e.ColumnIndex))
				return;

			if (conditionsGrid.Rows[e.RowIndex].Tag is int conditionId)
				OpenConditionEditor(conditionId);
		}

		private void activeMatchesGrid_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
		{
			if (e.RowIndex < 0 || activeMatchesGrid.Rows[e.RowIndex].Tag is not string icao)
				return;

			var radarUrl = _settingsManagerService.Settings.RadarUrl;
			if (string.IsNullOrWhiteSpace(radarUrl))
				return;

			OpenUrl($"{radarUrl}?icao={icao}&movingMap=1");
		}

		#endregion

		#region Dialogs

		private void OpenConditionEditor(int? selectConditionId)
		{
			var editor = Program.ServiceProvider.GetRequiredService<ConditionEditorForm>();
			if (selectConditionId != null)
				editor.SelectCondition(selectConditionId.Value);

			if (editor.ShowDialog(this) != DialogResult.OK)
				return;

			_conditionManagerService.LoadConditions();
			_checkerService.ActiveMatches.Clear();
			UpdateConditionList();
			UpdateStats();
		}

		private void conditionEditorMenuItem_Click(object sender, EventArgs e) {
			OpenConditionEditor(null);
		}

		private void settingsMenuItem_Click(object sender, EventArgs e) {
			var settingsForm = Program.ServiceProvider.GetRequiredService<SettingsForm>();
			if (settingsForm.ShowDialog(this) != DialogResult.OK)
				return;

			_logger.Log("Settings saved, restarting checker...", Color.White);
			_threadManagerService.Restart();
		}

		private void emailContentConfigMenuItem_Click(object sender, EventArgs e) {
			var ecc = Program.ServiceProvider.GetRequiredService<EmailContentConfigForm>();
			ecc.ShowDialog(this);
		}

		private void propertyInfoMenuItem_Click(object sender, EventArgs e) {
			var propertyInfoForm = Program.ServiceProvider.GetRequiredService<PropertyInfoForm>();
			propertyInfoForm.Show(this);
		}

		private void showLogFileMenuItem_Click(object sender, EventArgs e) {
			if (!File.Exists("alerts.log"))
			{
				MessageBox.Show(this, "No alerts have been logged yet, so alerts.log does not exist.", "No log file",
					MessageBoxButtons.OK, MessageBoxIcon.Information);
				return;
			}

			Process.Start("explorer.exe", "/select, \"" + Path.GetFullPath("alerts.log") + "\"");
		}

		private void donateMenuItem_Click(object sender, EventArgs e) {
			OpenUrl("https://www.paypal.me/dom2364");
		}

		private void aboutMenuItem_Click(object sender, EventArgs e)
		{
			var version = BuildInfo.FromCommit ? BuildInfo.Version : BuildInfo.Version + " (no git history, build time)";
			MessageBox.Show(this,
				$"PlaneAlerter\nVersion: {version}\n\nEmail alerts for Virtual Radar Server.\n\nOriginal project by Dom2364: github.com/Dom2364/PlaneAlerter\nThis fork: github.com/egite/PlaneAlerter",
				"About PlaneAlerter", MessageBoxButtons.OK, MessageBoxIcon.Information);
		}

		private static void OpenUrl(string url)
		{
			try
			{
				Process.Start(new ProcessStartInfo(url) { UseShellExecute = true });
			}
			catch (Exception)
			{
			}
		}

		#endregion

		#region Start/stop, tray, layout

		private void startStopButton_Click(object sender, EventArgs e) {
			if (_threadManagerService.ThreadStatus == CheckerStatus.Running) {
				_threadManagerService.Stop();
			}
			else {
				_threadManagerService.Start();
			}
		}

		private void restartButton_Click(object sender, EventArgs e) {
			_threadManagerService.Restart();
		}

		private void exitMenuItem_Click(object sender, EventArgs e)
		{
			Close();
		}

		/// <summary>
		/// Bring the window back from the tray or to the front
		/// </summary>
		private void notifyIcon_Activate(object sender, EventArgs e) {
			if (_hiddenToTray)
			{
				_hiddenToTray = false;
				Show();
				WindowState = FormWindowState.Normal;
			}

			Activate();
		}

		private void MainForm_Resize(object sender, EventArgs e)
		{
			if (WindowState != FormWindowState.Minimized || _hiddenToTray)
				return;

			if (_settingsManagerService.Settings != null && _settingsManagerService.Settings.MinimizeToTray)
			{
				_hiddenToTray = true;
				Hide();
			}
		}

		private void MainForm_FormClosing(object sender, FormClosingEventArgs e) {
			uiTimer.Stop();
			SaveLayout();
			notifyIcon.Visible = false;

			if (_settingsManagerService.Settings != null)
				_settingsManagerService.Save();
		}

		private void RestoreLayout()
		{
			var layout = WindowLayout.Load();
			if (layout == null)
				return;

			try
			{
				if (layout.Width >= MinimumSize.Width && layout.Height >= MinimumSize.Height)
					Size = new Size(layout.Width, layout.Height);

				if (layout.Maximized)
					WindowState = FormWindowState.Maximized;

				if (layout.LeftPaneWidth > splitContainer1.Panel1MinSize &&
				    layout.LeftPaneWidth < splitContainer1.Width - splitContainer1.Panel2MinSize)
					splitContainer1.SplitterDistance = layout.LeftPaneWidth;

				if (layout.ConditionsPaneHeight > splitContainer2.Panel1MinSize &&
				    layout.ConditionsPaneHeight < splitContainer2.Height - splitContainer2.Panel2MinSize)
					splitContainer2.SplitterDistance = layout.ConditionsPaneHeight;
			}
			catch (Exception)
			{
				//Bad layout file, keep defaults
			}
		}

		private void SaveLayout()
		{
			var bounds = WindowState == FormWindowState.Normal ? Bounds : RestoreBounds;

			new WindowLayout
			{
				Width = bounds.Width,
				Height = bounds.Height,
				Maximized = WindowState == FormWindowState.Maximized,
				LeftPaneWidth = splitContainer1.SplitterDistance,
				ConditionsPaneHeight = splitContainer2.SplitterDistance
			}.Save();
		}

		#endregion
	}
}
