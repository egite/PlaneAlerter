using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using Newtonsoft.Json;
using PlaneAlerter.Models;
using PlaneAlerter.Services;

namespace PlaneAlerter.Forms {
	/// <summary>
	/// Form for changing settings
	/// </summary>
	internal partial class SettingsForm :Form {
		private readonly ISettingsManagerService _settingsManagerService;
		private readonly IVrsService _vrsService;
		private readonly IEmailService _emailService;

		/// <summary>
		/// Smtp host info: port and whether SSL is used
		/// </summary>
		private static readonly Dictionary<string, (int Port, bool Ssl)> HostInfo = new()
		{
			{ "smtp.gmail.com", (587, true) },
			{ "smtp.live.com", (587, true) },
			{ "smtp.office365.com", (587, true) },
			{ "smtp.mail.yahoo.com", (465, true) },
			{ "plus.smtp.mail.yahoo.com", (465, true) },
			{ "smtp.mail.yahoo.co.uk", (465, true) },
			{ "smtp.mail.yahoo.com.au", (465, true) },
			{ "smtp.att.yahoo.com", (465, true) },
			{ "smtp.comcast.net", (587, false) },
			{ "outgoing.verizon.net", (465, true) },
			{ "smtp.mail.com", (465, true) },
		};

		/// <summary>
		/// Serialised settings as they were when the form finished loading, used to detect unsaved changes
		/// </summary>
		private string? _loadedSnapshot;

		/// <summary>
		/// True once the user has clicked Save and the inputs validated
		/// </summary>
		private bool _saveRequested;

		/// <summary>
		/// Constructor
		/// </summary>
		public SettingsForm(ISettingsManagerService settingsManagerService, IVrsService vrsService, IEmailService emailService) {
			_settingsManagerService = settingsManagerService;
			_vrsService = vrsService;
			_emailService = emailService;

			//Initialise form elements
			InitializeComponent();
			Theme.Apply(this);
			Theme.MakePrimary(saveSettingsButton);
			Theme.MakeHeader(alertActionsLabel);
			mapboxHelpLabel.ForeColor = Theme.TextMuted;
			testResultLabel.ForeColor = Theme.TextMuted;
			testEmailResultLabel.ForeColor = Theme.TextMuted;
		}

		private async void SettingsForm_Load(object sender, EventArgs e)
		{
			//Add smtp host info to combobox
			foreach (var smtpHost in HostInfo.Keys)
				smtpHostComboBox.Items.Add(smtpHost);

			//Set settings from current settings
			var settings = _settingsManagerService.Settings;
			senderEmailTextBox.Text = settings.SenderEmail;
			aircraftListTextBox.Text = settings.AircraftListUrl;
			radarURLTextBox.Text = settings.RadarUrl;
			VRSUsrTextBox.Text = settings.VRSUser;
			VRSPwdTextBox.Text = settings.VRSPassword;
			latTextBox.Value = Clamp(settings.Lat, latTextBox);
			longTextBox.Value = Clamp(settings.Long, longTextBox);
			removalTimeoutTextBox.Value = Clamp(settings.RemovalTimeout, removalTimeoutTextBox);
			refreshTextBox.Value = Clamp(settings.RefreshRate, refreshTextBox);
			startOnStartCheckBox.Checked = settings.StartOnStart;
			minimizeToTrayCheckBox.Checked = settings.MinimizeToTray;
			soundAlertsCheckBox.Checked = settings.SoundAlerts;
			flashWindowCheckBox.Checked = settings.FlashWindow;
			notificationsCheckBox.Checked = settings.ShowNotifications;
			centreAircraftRadioButton.Checked = settings.CentreMapOnAircraft;
			centreLatLngRadioButton.Checked = !settings.CentreMapOnAircraft;
			mapboxTokenTextBox.Text = settings.MapboxToken;
			smtpHostComboBox.Text = settings.SMTPHost;
			smtpHostPortTextBox.Value = Clamp(settings.SMTPPort, smtpHostPortTextBox);
			smtpUsrTextBox.Text = settings.SMTPUser;
			smtpPwdTextBox.Text = settings.SMTPPassword;
			smtpSSLCheckBox.Checked = settings.SMTPSSL;
			timeoutTextBox.Value = Clamp(settings.Timeout, timeoutTextBox);
			filterDstCheckBox.Checked = settings.FilterDistance;
			filterDstCheckBox_CheckedChanged(this, EventArgs.Empty);
			filterAltCheckBox.Checked = settings.FilterAltitude;
			filterAltCheckBox_CheckedChanged(this, EventArgs.Empty);
			ignoreModeSCheckBox.Checked = settings.IgnoreModeS;
			ignoreDistTextBox.Value = Clamp(Convert.ToDecimal(settings.IgnoreDistance), ignoreDistTextBox);
			ignoreAltTextBox.Value = Clamp(settings.IgnoreAltitude, ignoreAltTextBox);
			trailsAgeNumericUpDown.Value = Clamp(settings.TrailsUpdateFrequency, trailsAgeNumericUpDown);
			testEmailAddressTextBox.Text = settings.SenderEmail;

			filterReceiverCheckBox.Checked = settings.FilterReceiver;
			filterReceiverCheckBox_CheckedChanged(this, EventArgs.Empty);

			_loadedSnapshot = Snapshot();

			//Receivers come from the server, so load them after the snapshot to avoid a false "changed"
			await UpdateReceivers(settings.AircraftListUrl);
			_loadedSnapshot = Snapshot();
		}

		private static decimal Clamp(decimal value, NumericUpDown control)
		{
			return Math.Min(control.Maximum, Math.Max(control.Minimum, value));
		}

		private async Task UpdateReceivers(string aircraftListUrl) {
			if (string.IsNullOrWhiteSpace(aircraftListUrl)) {
				receiverComboBox.DataSource = null;
				receiverComboBox.Items.Clear();
				return;
			}

			//UpdateReceivers reads the URL from the saved settings, so apply the current text first
			var savedUrl = _settingsManagerService.Settings.AircraftListUrl;
			_settingsManagerService.Settings.AircraftListUrl = aircraftListUrl;
			IReadOnlyDictionary<string, string>? receivers;
			try
			{
				receivers = await Task.Run(_vrsService.UpdateReceivers);
			}
			finally
			{
				_settingsManagerService.Settings.AircraftListUrl = savedUrl;
			}

			if (receivers != null && receivers.Count > 0) {
				var sorted = receivers.OrderBy(x => x.Value, StringComparer.Ordinal).ToDictionary(x => x.Key, x => x.Value);

				receiverComboBox.DataSource = new BindingSource(sorted, null);
				receiverComboBox.DisplayMember = "Value";
				receiverComboBox.ValueMember = "Key";
				receiverComboBox.SelectedValue = _settingsManagerService.Settings.FilterReceiverId.ToString();
				if (receiverComboBox.SelectedIndex < 0)
					receiverComboBox.SelectedIndex = 0;
			}
			else {
				receiverComboBox.DataSource = null;
				receiverComboBox.Items.Clear();
			}
		}

		/// <summary>
		/// Build a Settings object from the current form values
		/// </summary>
		private Settings BuildSettingsFromForm() {
			var receiverId = _settingsManagerService.Settings.FilterReceiverId;
			if (receiverComboBox.SelectedValue != null && int.TryParse(receiverComboBox.SelectedValue.ToString(), out var selectedReceiver))
				receiverId = selectedReceiver;

			return new Settings(
				senderEmailTextBox.Text.Trim(),
				aircraftListTextBox.Text.Trim(),
				VRSUsrTextBox.Text,
				VRSPwdTextBox.Text,
				latTextBox.Value,
				longTextBox.Value,
				radarURLTextBox.Text.Trim(),
				centreAircraftRadioButton.Checked,
				Convert.ToInt32(removalTimeoutTextBox.Value),
				Convert.ToInt32(refreshTextBox.Value),
				startOnStartCheckBox.Checked,
				Convert.ToInt32(timeoutTextBox.Value),
				notificationsCheckBox.Checked,
				soundAlertsCheckBox.Checked,
				flashWindowCheckBox.Checked,
				filterDstCheckBox.Checked,
				filterAltCheckBox.Checked,
				ignoreModeSCheckBox.Checked,
				Convert.ToDouble(ignoreDistTextBox.Value),
				Convert.ToInt32(ignoreAltTextBox.Value),
				filterReceiverCheckBox.Checked,
				receiverId,
				Convert.ToInt32(trailsAgeNumericUpDown.Value),
				smtpHostComboBox.Text.Trim(),
				Convert.ToInt32(smtpHostPortTextBox.Value),
				smtpUsrTextBox.Text,
				smtpPwdTextBox.Text,
				smtpSSLCheckBox.Checked,
				mapboxTokenTextBox.Text.Trim(),
				minimizeToTrayCheckBox.Checked);
		}

		/// <summary>
		/// Serialise the form values so they can be compared for changes
		/// </summary>
		private string Snapshot() {
			return JsonConvert.SerializeObject(BuildSettingsFromForm());
		}

		/// <summary>
		/// Check the inputs, asking the user whether to continue when something looks wrong
		/// </summary>
		/// <returns>True if saving should go ahead</returns>
		private bool ValidateInputs() {
			//Validate email address if one is provided
			if (!string.IsNullOrWhiteSpace(senderEmailTextBox.Text) && !IsValidEmail(senderEmailTextBox.Text.Trim())) {
				tabControl.SelectedTab = emailTabPage;
				if (MessageBox.Show(this, "The sender email address does not appear to be valid. Save anyway?",
					"Invalid Email", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.No)
					return false;
			}

			//Validate AircraftList.json URL
			if (!string.IsNullOrWhiteSpace(aircraftListTextBox.Text) &&
				!Uri.TryCreate(aircraftListTextBox.Text.Trim(), UriKind.Absolute, out _)) {
				tabControl.SelectedTab = radarTabPage;
				if (MessageBox.Show(this, "The AircraftList.json URL does not appear to be valid. Save anyway?",
					"Invalid URL", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.No)
					return false;
			}

			//Mapbox public tokens start with pk.
			var token = mapboxTokenTextBox.Text.Trim();
			if (!string.IsNullOrEmpty(token) && !token.StartsWith("pk.", StringComparison.Ordinal)) {
				tabControl.SelectedTab = mapsTabPage;
				if (MessageBox.Show(this, "The Mapbox access token does not look like a public token (they start with \"pk.\"). Save anyway?",
					"Unexpected Mapbox Token", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.No)
					return false;
			}

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
		/// Smtp combobox value changed
		/// </summary>
		private void smtpHostComboBox_SelectedValueChanged(object sender, EventArgs e)
		{
			//If exists in smtp host info, set port and ssl values
			if (!HostInfo.TryGetValue(smtpHostComboBox.Text, out var info))
				return;

			smtpHostPortTextBox.Value = info.Port;
			smtpSSLCheckBox.Checked = info.Ssl;
		}

		/// <summary>
		/// Settings form closing: persist on Save, otherwise confirm discarding any changes
		/// </summary>
		private void SettingsForm_FormClosing(object sender, FormClosingEventArgs e) {
			if (_saveRequested) {
				_settingsManagerService.Settings = BuildSettingsFromForm();
				_settingsManagerService.Save();
				DialogResult = DialogResult.OK;
				return;
			}

			//Closing without saving: warn if anything changed since load
			if (_loadedSnapshot != null && Snapshot() != _loadedSnapshot) {
				if (MessageBox.Show(this, "Discard your unsaved changes to settings?", "Unsaved Changes",
					MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.No) {
					e.Cancel = true;
					return;
				}
			}

			DialogResult = DialogResult.Cancel;
		}

		private void saveSettingsButton_Click(object sender, EventArgs e) {
			if (!ValidateInputs())
				return;

			_saveRequested = true;
			Close();
		}

		private void cancelButton_Click(object sender, EventArgs e) {
			Close();
		}

		private void gmailLink_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
		{
			OpenUrl("https://myaccount.google.com/apppasswords");
		}

		private void mapboxLink_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
		{
			OpenUrl("https://account.mapbox.com/access-tokens/");
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

		private void filterDstCheckBox_CheckedChanged(object sender, EventArgs e) {
			ignoreDistTextBox.Enabled = filterDstCheckBox.Checked;
			ignoreModeSCheckBox.Enabled = filterDstCheckBox.Checked;
		}

		private void filterAltCheckBox_CheckedChanged(object sender, EventArgs e) {
			ignoreAltTextBox.Enabled = filterAltCheckBox.Checked;
		}

		private void filterReceiverCheckBox_CheckedChanged(object sender, EventArgs e) {
			receiverComboBox.Enabled = filterReceiverCheckBox.Checked;
			refreshReceiversButton.Enabled = filterReceiverCheckBox.Checked;
		}

		private async void refreshReceiversButton_Click(object sender, EventArgs e) {
			Cursor = Cursors.WaitCursor;
			refreshReceiversButton.Enabled = false;
			try {
				await UpdateReceivers(aircraftListTextBox.Text.Trim());
				if (receiverComboBox.Items.Count == 0)
					ShowResult(testResultLabel, false, "No receivers returned. Check the AircraftList.json URL and credentials, then use Test connection.");
			}
			finally {
				Cursor = Cursors.Default;
				refreshReceiversButton.Enabled = true;
			}
		}

		private async void testConnectionButton_Click(object sender, EventArgs e)
		{
			testConnectionButton.Enabled = false;
			testResultLabel.ForeColor = Theme.TextMuted;
			testResultLabel.Text = "Connecting...";
			try
			{
				var result = await _vrsService.TestConnection(aircraftListTextBox.Text.Trim(), VRSUsrTextBox.Text,
					VRSPwdTextBox.Text, Convert.ToInt32(timeoutTextBox.Value));
				ShowResult(testResultLabel, result.Success, result.Message);
			}
			finally
			{
				testConnectionButton.Enabled = true;
			}
		}

		private async void useReceiverLocationButton_Click(object sender, EventArgs e)
		{
			useReceiverLocationButton.Enabled = false;
			Cursor = Cursors.WaitCursor;
			try
			{
				var location = await _vrsService.GetServerLocation(radarURLTextBox.Text.Trim(), VRSUsrTextBox.Text,
					VRSPwdTextBox.Text, Convert.ToInt32(timeoutTextBox.Value));

				if (location == null)
				{
					ShowResult(testResultLabel, false, "Could not read a location from VRS. Check the Radar URL (it should end with /VirtualRadar/) and that VRS has an initial map position set.");
					return;
				}

				latTextBox.Value = Clamp(location.Value.Lat, latTextBox);
				longTextBox.Value = Clamp(location.Value.Lng, longTextBox);
				ShowResult(testResultLabel, true, $"Location set to {location.Value.Lat:0.####}, {location.Value.Lng:0.####} from the VRS server configuration.");
			}
			finally
			{
				Cursor = Cursors.Default;
				useReceiverLocationButton.Enabled = true;
			}
		}

		private async void sendTestEmailButton_Click(object sender, EventArgs e)
		{
			var to = testEmailAddressTextBox.Text.Trim();
			if (!IsValidEmail(to))
			{
				ShowResult(testEmailResultLabel, false, "Enter a valid address to send the test to.");
				return;
			}

			var settings = BuildSettingsFromForm();
			if (string.IsNullOrWhiteSpace(settings.SMTPHost) || !IsValidEmail(settings.SenderEmail))
			{
				ShowResult(testEmailResultLabel, false, "Fill in the sender address and SMTP host first.");
				return;
			}

			sendTestEmailButton.Enabled = false;
			testEmailResultLabel.ForeColor = Theme.TextMuted;
			testEmailResultLabel.Text = $"Sending via {settings.SMTPHost}:{settings.SMTPPort}...";
			try
			{
				var error = await _emailService.SendTestEmail(settings, to);
				ShowResult(testEmailResultLabel, error == null,
					error == null ? $"Sent. Check the inbox of {to} (and its spam folder)." : "Failed: " + error);
			}
			finally
			{
				sendTestEmailButton.Enabled = true;
			}
		}

		private static void ShowResult(Label label, bool success, string message)
		{
			label.ForeColor = success ? Theme.Navy : Theme.LogError;
			label.Text = message;
		}
	}
}
