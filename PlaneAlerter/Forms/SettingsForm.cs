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

		/// <summary>
		/// Smtp host info
		/// </summary>
		private static Dictionary<string, object[]> HostInfo { get; } = new();

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
		public SettingsForm(ISettingsManagerService settingsManagerService, IVrsService vrsService) {
			_settingsManagerService = settingsManagerService;
			_vrsService = vrsService;

			//Initialise form elements
			InitializeComponent();
			Theme.Apply(this);
			Theme.MakePrimary(saveSettingsButton);

			//Add smtp host info
			HostInfo.Clear();
			HostInfo.Add("smtp.gmail.com", new object[] { 587, true });
			HostInfo.Add("smtp.live.com", new object[] { 587, true });
			HostInfo.Add("smtp.office365.com", new object[] { 587, true });
			HostInfo.Add("smtp.mail.yahoo.com", new object[] { 465, true });
			HostInfo.Add("plus.smtp.mail.yahoo.com", new object[] { 465, true });
			HostInfo.Add("smtp.mail.yahoo.co.uk", new object[] { 465, true });
			HostInfo.Add("smtp.mail.yahoo.com.au", new object[] { 465, true });
			HostInfo.Add("smtp.att.yahoo.com", new object[] { 465, true });
			HostInfo.Add("smtp.comcast.net", new object[] { 587, false });
			HostInfo.Add("outgoing.verizon.net", new object[] { 465, true });
			HostInfo.Add("smtp.mail.com", new object[] { 465, true });
		}

		private async void SettingsForm_Load(object sender, EventArgs e)
		{
			//Add smtp host info to combobox
			foreach (var smtpHost in HostInfo.Keys)
				smtpHostComboBox.Items.Add(smtpHost);

			await UpdateReceivers();

			//Set settings from current settings
			var settings = _settingsManagerService.Settings;
			senderEmailTextBox.Text = settings.SenderEmail;
			aircraftListTextBox.Text = settings.AircraftListUrl;
			radarURLTextBox.Text = settings.RadarUrl;
			VRSUsrTextBox.Text = settings.VRSUser;
			VRSPwdTextBox.Text = settings.VRSPassword;
			latTextBox.Value = settings.Lat;
			longTextBox.Value = settings.Long;
			removalTimeoutTextBox.Value = settings.RemovalTimeout;
			refreshTextBox.Value = settings.RefreshRate;
			startOnStartCheckBox.Checked = settings.StartOnStart;
			soundAlertsCheckBox.Checked = settings.SoundAlerts;
			flashWindowCheckBox.Checked = settings.FlashWindow;
			notificationsCheckBox.Checked = settings.ShowNotifications;
			centreAircraftRadioButton.Checked = settings.CentreMapOnAircraft;
			centreLatLngRadioButton.Checked = !settings.CentreMapOnAircraft;
			mapboxTokenTextBox.Text = settings.MapboxToken;
			smtpHostComboBox.Text = settings.SMTPHost;
			smtpHostPortTextBox.Value = settings.SMTPPort;
			smtpUsrTextBox.Text = settings.SMTPUser;
			smtpPwdTextBox.Text = settings.SMTPPassword;
			smtpSSLCheckBox.Checked = settings.SMTPSSL;
			timeoutTextBox.Value = settings.Timeout;
			filterDstCheckBox.Checked = settings.FilterDistance;
			filterDstCheckBox_CheckedChanged(this, EventArgs.Empty);
			filterAltCheckBox.Checked = settings.FilterAltitude;
			filterAltCheckBox_CheckedChanged(this, EventArgs.Empty);
			ignoreModeSCheckBox.Checked = settings.IgnoreModeS;
			ignoreDistTextBox.Value = Convert.ToDecimal(settings.IgnoreDistance);
			ignoreAltTextBox.Value = settings.IgnoreAltitude;
			if (filterReceiverCheckBox.Checked) filterReceiverCheckBox.Checked = settings.FilterReceiver; //Will be unchecked if there was an error getting receivers
			filterReceiverCheckBox_CheckedChanged(this, EventArgs.Empty);
			trailsAgeNumericUpDown.Value = settings.TrailsUpdateFrequency;

			_loadedSnapshot = Snapshot();
		}

		private async Task UpdateReceivers() {
			if (string.IsNullOrWhiteSpace(_settingsManagerService.Settings.AircraftListUrl)) {
				receiverComboBox.DataSource = null;
				receiverComboBox.Items.Clear();
				filterReceiverCheckBox.Checked = false;
				return;
			}

			var receivers = await Task.Run(_vrsService.UpdateReceivers);
			if (receivers != null) {
				receivers = receivers.OrderBy(x => x.Value, StringComparer.Ordinal).ToDictionary(x => x.Key, x => x.Value);

				receiverComboBox.DataSource = new BindingSource(receivers, null);
				receiverComboBox.DisplayMember = "Value";
				receiverComboBox.ValueMember = "Key";
				receiverComboBox.SelectedValue = _settingsManagerService.Settings.FilterReceiverId.ToString();
			}
			else {
				receiverComboBox.DataSource = null;
				receiverComboBox.Items.Clear();
				filterReceiverCheckBox.Checked = false;
			}
		}

		/// <summary>
		/// Build a Settings object from the current form values
		/// </summary>
		private Settings BuildSettingsFromForm() {
			return new Settings(
				senderEmailTextBox.Text,
				aircraftListTextBox.Text,
				VRSUsrTextBox.Text,
				VRSPwdTextBox.Text,
				latTextBox.Value,
				longTextBox.Value,
				radarURLTextBox.Text,
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
				Convert.ToInt32(receiverComboBox.SelectedValue),
				Convert.ToInt32(trailsAgeNumericUpDown.Value),
				smtpHostComboBox.Text,
				Convert.ToInt32(smtpHostPortTextBox.Value),
				smtpUsrTextBox.Text,
				smtpPwdTextBox.Text,
				smtpSSLCheckBox.Checked,
				mapboxTokenTextBox.Text.Trim());
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
			if (!string.IsNullOrWhiteSpace(senderEmailTextBox.Text)) {
				try {
					new System.Net.Mail.MailAddress(senderEmailTextBox.Text);
				}
				catch (FormatException) {
					if (MessageBox.Show("The sender email address does not appear to be valid. Save anyway?",
						"Invalid Email", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.No)
						return false;
				}
			}

			//Validate AircraftList.json URL
			if (!string.IsNullOrWhiteSpace(aircraftListTextBox.Text) &&
				!Uri.TryCreate(aircraftListTextBox.Text, UriKind.Absolute, out _)) {
				if (MessageBox.Show("The AircraftList.json URL does not appear to be valid. Save anyway?",
					"Invalid URL", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.No)
					return false;
			}

			//Mapbox public tokens start with pk.
			var token = mapboxTokenTextBox.Text.Trim();
			if (!string.IsNullOrEmpty(token) && !token.StartsWith("pk.", StringComparison.Ordinal)) {
				if (MessageBox.Show("The Mapbox access token does not look like a public token (they start with \"pk.\"). Save anyway?",
					"Unexpected Mapbox Token", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.No)
					return false;
			}

			return true;
		}

		/// <summary>
		/// Smtp combobox value changed
		/// </summary>
		/// <param name="sender">Sender</param>
		/// <param name="e">Event Args</param>
		private void smtpHostComboBox_SelectedValueChanged(object sender, EventArgs e)
		{
			//If exists in smtp host info, set port and ssl values
			if (!HostInfo.ContainsKey(smtpHostComboBox.Text))
				return;

			smtpHostPortTextBox.Value = Convert.ToDecimal(HostInfo[smtpHostComboBox.Text][0]);
			smtpSSLCheckBox.Checked = (bool)HostInfo[smtpHostComboBox.Text][1];
		}

		/// <summary>
		/// Settings form closing: persist on Save, otherwise confirm discarding any changes
		/// </summary>
		/// <param name="sender">Sender</param>
		/// <param name="e">Event Args</param>
		private void SettingsForm_FormClosing(object sender, FormClosingEventArgs e) {
			if (_saveRequested) {
				_settingsManagerService.Settings = BuildSettingsFromForm();
				_settingsManagerService.Save();
				DialogResult = DialogResult.OK;
				return;
			}

			//Closing without saving: warn if anything changed since load
			if (_loadedSnapshot != null && Snapshot() != _loadedSnapshot) {
				if (MessageBox.Show("Discard your unsaved changes to settings?", "Unsaved Changes",
					MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.No) {
					e.Cancel = true;
					return;
				}
			}

			DialogResult = DialogResult.Cancel;
		}

		/// <summary>
		/// Save settings button click
		/// </summary>
		/// <param name="sender">Sender</param>
		/// <param name="e">Event Args</param>
		private void saveSettingsButton_Click(object sender, EventArgs e) {
			if (!ValidateInputs())
				return;

			_saveRequested = true;
			Close();
		}

		/// <summary>
		/// Cancel button click
		/// </summary>
		private void cancelButton_Click(object sender, EventArgs e) {
			Close();
		}

		/// <summary>
		/// Gmail link clicked
		/// </summary>
		/// <param name="sender">Sender</param>
		/// <param name="e">Event Args</param>
		private void gmailLink_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
		{
			//Open Google app passwords page
			Process.Start(new ProcessStartInfo("https://myaccount.google.com/apppasswords")
			{
				UseShellExecute = true
			});
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
		}

		private async void refreshReceiversButton_Click(object sender, EventArgs e) {
			_settingsManagerService.Settings.AircraftListUrl = aircraftListTextBox.Text;
			Cursor = Cursors.WaitCursor;
			refreshReceiversButton.Enabled = false;
			try {
				await UpdateReceivers();
			}
			finally {
				Cursor = Cursors.Default;
				refreshReceiversButton.Enabled = true;
			}
		}
	}
}
