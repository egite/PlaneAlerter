namespace PlaneAlerter.Forms {
	partial class SettingsForm {
		/// <summary>
		/// Designer variable used to keep track of non-visual components.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary>
		/// Disposes resources used by the form.
		/// </summary>
		/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
		protected override void Dispose(bool disposing)
		{
			if (disposing) {
				if (components != null) {
					components.Dispose();
				}
			}
			base.Dispose(disposing);
		}

		/// <summary>
		/// This method is required for Windows Forms designer support.
		/// Do not change the method contents inside the source code editor. The Forms designer might
		/// not be able to load this method if it was changed manually.
		/// </summary>
		private void InitializeComponent()
		{
			this.components = new System.ComponentModel.Container();
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SettingsForm));
			this.toolTip = new System.Windows.Forms.ToolTip(this.components);
			this.tabControl = new System.Windows.Forms.TabControl();
			this.radarTabPage = new System.Windows.Forms.TabPage();
			this.emailTabPage = new System.Windows.Forms.TabPage();
			this.alertsTabPage = new System.Windows.Forms.TabPage();
			this.mapsTabPage = new System.Windows.Forms.TabPage();
			this.saveSettingsButton = new System.Windows.Forms.Button();
			this.cancelButton = new System.Windows.Forms.Button();
			// Radar tab
			this.aircraftListLabel = new System.Windows.Forms.Label();
			this.aircraftListTextBox = new System.Windows.Forms.TextBox();
			this.radarUrlLabel = new System.Windows.Forms.Label();
			this.radarURLTextBox = new System.Windows.Forms.TextBox();
			this.vrsUserLabel = new System.Windows.Forms.Label();
			this.VRSUsrTextBox = new System.Windows.Forms.TextBox();
			this.vrsPasswordLabel = new System.Windows.Forms.Label();
			this.VRSPwdTextBox = new System.Windows.Forms.MaskedTextBox();
			this.testConnectionButton = new System.Windows.Forms.Button();
			this.testResultLabel = new System.Windows.Forms.Label();
			this.timeoutLabel = new System.Windows.Forms.Label();
			this.timeoutTextBox = new System.Windows.Forms.NumericUpDown();
			this.filterReceiverCheckBox = new System.Windows.Forms.CheckBox();
			this.receiverComboBox = new System.Windows.Forms.ComboBox();
			this.refreshReceiversButton = new System.Windows.Forms.Button();
			this.latLabel = new System.Windows.Forms.Label();
			this.latTextBox = new System.Windows.Forms.NumericUpDown();
			this.longLabel = new System.Windows.Forms.Label();
			this.longTextBox = new System.Windows.Forms.NumericUpDown();
			this.useReceiverLocationButton = new System.Windows.Forms.Button();
			this.filterDstCheckBox = new System.Windows.Forms.CheckBox();
			this.ignoreDistTextBox = new System.Windows.Forms.NumericUpDown();
			this.ignoreModeSCheckBox = new System.Windows.Forms.CheckBox();
			this.filterAltCheckBox = new System.Windows.Forms.CheckBox();
			this.ignoreAltTextBox = new System.Windows.Forms.NumericUpDown();
			this.trailsLabel = new System.Windows.Forms.Label();
			this.trailsAgeNumericUpDown = new System.Windows.Forms.NumericUpDown();
			// Email tab
			this.senderLabel = new System.Windows.Forms.Label();
			this.senderEmailTextBox = new System.Windows.Forms.TextBox();
			this.smtpHostLabel = new System.Windows.Forms.Label();
			this.smtpHostComboBox = new System.Windows.Forms.ComboBox();
			this.smtpPortLabel = new System.Windows.Forms.Label();
			this.smtpHostPortTextBox = new System.Windows.Forms.NumericUpDown();
			this.smtpUserLabel = new System.Windows.Forms.Label();
			this.smtpUsrTextBox = new System.Windows.Forms.TextBox();
			this.smtpPasswordLabel = new System.Windows.Forms.Label();
			this.smtpPwdTextBox = new System.Windows.Forms.MaskedTextBox();
			this.smtpSSLCheckBox = new System.Windows.Forms.CheckBox();
			this.gmailLink = new System.Windows.Forms.LinkLabel();
			this.testEmailLabel = new System.Windows.Forms.Label();
			this.testEmailAddressTextBox = new System.Windows.Forms.TextBox();
			this.sendTestEmailButton = new System.Windows.Forms.Button();
			this.testEmailResultLabel = new System.Windows.Forms.Label();
			// Alerts tab
			this.refreshLabel = new System.Windows.Forms.Label();
			this.refreshTextBox = new System.Windows.Forms.NumericUpDown();
			this.removalTimeoutLabel = new System.Windows.Forms.Label();
			this.removalTimeoutTextBox = new System.Windows.Forms.NumericUpDown();
			this.startOnStartCheckBox = new System.Windows.Forms.CheckBox();
			this.minimizeToTrayCheckBox = new System.Windows.Forms.CheckBox();
			this.alertActionsLabel = new System.Windows.Forms.Label();
			this.soundAlertsCheckBox = new System.Windows.Forms.CheckBox();
			this.notificationsCheckBox = new System.Windows.Forms.CheckBox();
			this.flashWindowCheckBox = new System.Windows.Forms.CheckBox();
			// Maps tab
			this.centreLabel = new System.Windows.Forms.Label();
			this.centreAircraftRadioButton = new System.Windows.Forms.RadioButton();
			this.centreLatLngRadioButton = new System.Windows.Forms.RadioButton();
			this.mapboxTokenLabel = new System.Windows.Forms.Label();
			this.mapboxTokenTextBox = new System.Windows.Forms.TextBox();
			this.mapboxHelpLabel = new System.Windows.Forms.Label();
			this.mapboxLink = new System.Windows.Forms.LinkLabel();
			this.tabControl.SuspendLayout();
			this.radarTabPage.SuspendLayout();
			this.emailTabPage.SuspendLayout();
			this.alertsTabPage.SuspendLayout();
			this.mapsTabPage.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.timeoutTextBox)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.latTextBox)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.longTextBox)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.ignoreDistTextBox)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.ignoreAltTextBox)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.trailsAgeNumericUpDown)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.smtpHostPortTextBox)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.refreshTextBox)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.removalTimeoutTextBox)).BeginInit();
			this.SuspendLayout();
			//
			// toolTip
			//
			this.toolTip.AutoPopDelay = 10000;
			this.toolTip.InitialDelay = 300;
			this.toolTip.ReshowDelay = 100;
			//
			// tabControl
			//
			this.tabControl.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
            | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
			this.tabControl.Controls.Add(this.radarTabPage);
			this.tabControl.Controls.Add(this.emailTabPage);
			this.tabControl.Controls.Add(this.alertsTabPage);
			this.tabControl.Controls.Add(this.mapsTabPage);
			this.tabControl.Location = new System.Drawing.Point(12, 12);
			this.tabControl.Name = "tabControl";
			this.tabControl.Padding = new System.Drawing.Point(14, 5);
			this.tabControl.SelectedIndex = 0;
			this.tabControl.Size = new System.Drawing.Size(736, 440);
			this.tabControl.TabIndex = 0;
			//
			// radarTabPage
			//
			this.radarTabPage.Controls.Add(this.aircraftListLabel);
			this.radarTabPage.Controls.Add(this.aircraftListTextBox);
			this.radarTabPage.Controls.Add(this.radarUrlLabel);
			this.radarTabPage.Controls.Add(this.radarURLTextBox);
			this.radarTabPage.Controls.Add(this.vrsUserLabel);
			this.radarTabPage.Controls.Add(this.VRSUsrTextBox);
			this.radarTabPage.Controls.Add(this.vrsPasswordLabel);
			this.radarTabPage.Controls.Add(this.VRSPwdTextBox);
			this.radarTabPage.Controls.Add(this.testConnectionButton);
			this.radarTabPage.Controls.Add(this.testResultLabel);
			this.radarTabPage.Controls.Add(this.timeoutLabel);
			this.radarTabPage.Controls.Add(this.timeoutTextBox);
			this.radarTabPage.Controls.Add(this.filterReceiverCheckBox);
			this.radarTabPage.Controls.Add(this.receiverComboBox);
			this.radarTabPage.Controls.Add(this.refreshReceiversButton);
			this.radarTabPage.Controls.Add(this.latLabel);
			this.radarTabPage.Controls.Add(this.latTextBox);
			this.radarTabPage.Controls.Add(this.longLabel);
			this.radarTabPage.Controls.Add(this.longTextBox);
			this.radarTabPage.Controls.Add(this.useReceiverLocationButton);
			this.radarTabPage.Controls.Add(this.filterDstCheckBox);
			this.radarTabPage.Controls.Add(this.ignoreDistTextBox);
			this.radarTabPage.Controls.Add(this.ignoreModeSCheckBox);
			this.radarTabPage.Controls.Add(this.filterAltCheckBox);
			this.radarTabPage.Controls.Add(this.ignoreAltTextBox);
			this.radarTabPage.Controls.Add(this.trailsLabel);
			this.radarTabPage.Controls.Add(this.trailsAgeNumericUpDown);
			this.radarTabPage.Location = new System.Drawing.Point(4, 24);
			this.radarTabPage.Name = "radarTabPage";
			this.radarTabPage.Padding = new System.Windows.Forms.Padding(3);
			this.radarTabPage.Size = new System.Drawing.Size(728, 412);
			this.radarTabPage.TabIndex = 0;
			this.radarTabPage.Text = "Radar";
			//
			// aircraftListLabel
			//
			this.aircraftListLabel.AutoSize = true;
			this.aircraftListLabel.Location = new System.Drawing.Point(16, 20);
			this.aircraftListLabel.Name = "aircraftListLabel";
			this.aircraftListLabel.Text = "AircraftList.json URL";
			this.toolTip.SetToolTip(this.aircraftListLabel, "The VRS AircraftList.json endpoint, e.g. http://127.0.0.1:8080/VirtualRadar/AircraftList.json");
			//
			// aircraftListTextBox
			//
			this.aircraftListTextBox.Location = new System.Drawing.Point(200, 16);
			this.aircraftListTextBox.Name = "aircraftListTextBox";
			this.aircraftListTextBox.PlaceholderText = "http://127.0.0.1:8080/VirtualRadar/AircraftList.json";
			this.aircraftListTextBox.Size = new System.Drawing.Size(400, 23);
			this.aircraftListTextBox.TabIndex = 0;
			//
			// radarUrlLabel
			//
			this.radarUrlLabel.AutoSize = true;
			this.radarUrlLabel.Location = new System.Drawing.Point(16, 52);
			this.radarUrlLabel.Name = "radarUrlLabel";
			this.radarUrlLabel.Text = "Radar URL";
			this.toolTip.SetToolTip(this.radarUrlLabel, "The VRS web page, used for links in emails. Must end with /VirtualRadar/ for report links to work.");
			//
			// radarURLTextBox
			//
			this.radarURLTextBox.Location = new System.Drawing.Point(200, 48);
			this.radarURLTextBox.Name = "radarURLTextBox";
			this.radarURLTextBox.PlaceholderText = "http://127.0.0.1:8080/VirtualRadar/";
			this.radarURLTextBox.Size = new System.Drawing.Size(400, 23);
			this.radarURLTextBox.TabIndex = 1;
			//
			// vrsUserLabel
			//
			this.vrsUserLabel.AutoSize = true;
			this.vrsUserLabel.Location = new System.Drawing.Point(16, 84);
			this.vrsUserLabel.Name = "vrsUserLabel";
			this.vrsUserLabel.Text = "VRS username (optional)";
			//
			// VRSUsrTextBox
			//
			this.VRSUsrTextBox.Location = new System.Drawing.Point(200, 80);
			this.VRSUsrTextBox.Name = "VRSUsrTextBox";
			this.VRSUsrTextBox.Size = new System.Drawing.Size(200, 23);
			this.VRSUsrTextBox.TabIndex = 2;
			//
			// vrsPasswordLabel
			//
			this.vrsPasswordLabel.AutoSize = true;
			this.vrsPasswordLabel.Location = new System.Drawing.Point(16, 116);
			this.vrsPasswordLabel.Name = "vrsPasswordLabel";
			this.vrsPasswordLabel.Text = "VRS password";
			//
			// VRSPwdTextBox
			//
			this.VRSPwdTextBox.Location = new System.Drawing.Point(200, 112);
			this.VRSPwdTextBox.Name = "VRSPwdTextBox";
			this.VRSPwdTextBox.PasswordChar = '●';
			this.VRSPwdTextBox.Size = new System.Drawing.Size(200, 23);
			this.VRSPwdTextBox.TabIndex = 3;
			//
			// testConnectionButton
			//
			this.testConnectionButton.Location = new System.Drawing.Point(420, 79);
			this.testConnectionButton.Name = "testConnectionButton";
			this.testConnectionButton.Size = new System.Drawing.Size(180, 28);
			this.testConnectionButton.TabIndex = 4;
			this.testConnectionButton.Text = "Test connection";
			this.testConnectionButton.UseVisualStyleBackColor = true;
			this.testConnectionButton.Click += new System.EventHandler(this.testConnectionButton_Click);
			//
			// testResultLabel
			//
			this.testResultLabel.Location = new System.Drawing.Point(200, 140);
			this.testResultLabel.Name = "testResultLabel";
			this.testResultLabel.Size = new System.Drawing.Size(510, 34);
			this.testResultLabel.TabIndex = 5;
			//
			// timeoutLabel
			//
			this.timeoutLabel.AutoSize = true;
			this.timeoutLabel.Location = new System.Drawing.Point(16, 184);
			this.timeoutLabel.Name = "timeoutLabel";
			this.timeoutLabel.Text = "Request timeout (secs)";
			//
			// timeoutTextBox
			//
			this.timeoutTextBox.Location = new System.Drawing.Point(200, 180);
			this.timeoutTextBox.Maximum = new decimal(new int[] { 600, 0, 0, 0 });
			this.timeoutTextBox.Minimum = new decimal(new int[] { 1, 0, 0, 0 });
			this.timeoutTextBox.Name = "timeoutTextBox";
			this.timeoutTextBox.Size = new System.Drawing.Size(90, 23);
			this.timeoutTextBox.TabIndex = 6;
			this.timeoutTextBox.Value = new decimal(new int[] { 5, 0, 0, 0 });
			//
			// filterReceiverCheckBox
			//
			this.filterReceiverCheckBox.AutoSize = true;
			this.filterReceiverCheckBox.Location = new System.Drawing.Point(16, 218);
			this.filterReceiverCheckBox.Name = "filterReceiverCheckBox";
			this.filterReceiverCheckBox.TabIndex = 7;
			this.filterReceiverCheckBox.Text = "Only check aircraft from receiver:";
			this.filterReceiverCheckBox.UseVisualStyleBackColor = true;
			this.filterReceiverCheckBox.CheckedChanged += new System.EventHandler(this.filterReceiverCheckBox_CheckedChanged);
			//
			// receiverComboBox
			//
			this.receiverComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.receiverComboBox.Location = new System.Drawing.Point(240, 215);
			this.receiverComboBox.Name = "receiverComboBox";
			this.receiverComboBox.Size = new System.Drawing.Size(220, 23);
			this.receiverComboBox.TabIndex = 8;
			//
			// refreshReceiversButton
			//
			this.refreshReceiversButton.Location = new System.Drawing.Point(470, 213);
			this.refreshReceiversButton.Name = "refreshReceiversButton";
			this.refreshReceiversButton.Size = new System.Drawing.Size(90, 27);
			this.refreshReceiversButton.TabIndex = 9;
			this.refreshReceiversButton.Text = "Refresh list";
			this.refreshReceiversButton.UseVisualStyleBackColor = true;
			this.refreshReceiversButton.Click += new System.EventHandler(this.refreshReceiversButton_Click);
			//
			// latLabel
			//
			this.latLabel.AutoSize = true;
			this.latLabel.Location = new System.Drawing.Point(16, 258);
			this.latLabel.Name = "latLabel";
			this.latLabel.Text = "Your latitude";
			this.toolTip.SetToolTip(this.latLabel, "Used for distance filtering and as the map centre option");
			//
			// latTextBox
			//
			this.latTextBox.DecimalPlaces = 4;
			this.latTextBox.Location = new System.Drawing.Point(200, 254);
			this.latTextBox.Maximum = new decimal(new int[] { 90, 0, 0, 0 });
			this.latTextBox.Minimum = new decimal(new int[] { 90, 0, 0, -2147483648 });
			this.latTextBox.Name = "latTextBox";
			this.latTextBox.Size = new System.Drawing.Size(120, 23);
			this.latTextBox.TabIndex = 10;
			//
			// longLabel
			//
			this.longLabel.AutoSize = true;
			this.longLabel.Location = new System.Drawing.Point(340, 258);
			this.longLabel.Name = "longLabel";
			this.longLabel.Text = "Longitude";
			//
			// longTextBox
			//
			this.longTextBox.DecimalPlaces = 4;
			this.longTextBox.Location = new System.Drawing.Point(410, 254);
			this.longTextBox.Maximum = new decimal(new int[] { 180, 0, 0, 0 });
			this.longTextBox.Minimum = new decimal(new int[] { 180, 0, 0, -2147483648 });
			this.longTextBox.Name = "longTextBox";
			this.longTextBox.Size = new System.Drawing.Size(120, 23);
			this.longTextBox.TabIndex = 11;
			//
			// useReceiverLocationButton
			//
			this.useReceiverLocationButton.Location = new System.Drawing.Point(546, 252);
			this.useReceiverLocationButton.Name = "useReceiverLocationButton";
			this.useReceiverLocationButton.Size = new System.Drawing.Size(164, 27);
			this.useReceiverLocationButton.TabIndex = 12;
			this.useReceiverLocationButton.Text = "Use VRS location";
			this.toolTip.SetToolTip(this.useReceiverLocationButton, "Read the initial map position from the VRS server configuration");
			this.useReceiverLocationButton.UseVisualStyleBackColor = true;
			this.useReceiverLocationButton.Click += new System.EventHandler(this.useReceiverLocationButton_Click);
			//
			// filterDstCheckBox
			//
			this.filterDstCheckBox.AutoSize = true;
			this.filterDstCheckBox.Location = new System.Drawing.Point(16, 296);
			this.filterDstCheckBox.Name = "filterDstCheckBox";
			this.filterDstCheckBox.TabIndex = 13;
			this.filterDstCheckBox.Text = "Ignore aircraft further than (km)";
			this.toolTip.SetToolTip(this.filterDstCheckBox, "Saves bandwidth by asking VRS to leave out distant aircraft");
			this.filterDstCheckBox.UseVisualStyleBackColor = true;
			this.filterDstCheckBox.CheckedChanged += new System.EventHandler(this.filterDstCheckBox_CheckedChanged);
			//
			// ignoreDistTextBox
			//
			this.ignoreDistTextBox.DecimalPlaces = 1;
			this.ignoreDistTextBox.Location = new System.Drawing.Point(240, 293);
			this.ignoreDistTextBox.Maximum = new decimal(new int[] { 100000, 0, 0, 0 });
			this.ignoreDistTextBox.Name = "ignoreDistTextBox";
			this.ignoreDistTextBox.Size = new System.Drawing.Size(100, 23);
			this.ignoreDistTextBox.TabIndex = 14;
			//
			// ignoreModeSCheckBox
			//
			this.ignoreModeSCheckBox.AutoSize = true;
			this.ignoreModeSCheckBox.Location = new System.Drawing.Point(360, 296);
			this.ignoreModeSCheckBox.Name = "ignoreModeSCheckBox";
			this.ignoreModeSCheckBox.TabIndex = 15;
			this.ignoreModeSCheckBox.Text = "Also ignore aircraft without a position";
			this.toolTip.SetToolTip(this.ignoreModeSCheckBox, "Mode-S only aircraft have no position, so their distance is unknown");
			this.ignoreModeSCheckBox.UseVisualStyleBackColor = true;
			//
			// filterAltCheckBox
			//
			this.filterAltCheckBox.AutoSize = true;
			this.filterAltCheckBox.Location = new System.Drawing.Point(16, 328);
			this.filterAltCheckBox.Name = "filterAltCheckBox";
			this.filterAltCheckBox.TabIndex = 16;
			this.filterAltCheckBox.Text = "Ignore aircraft higher than (ft)";
			this.filterAltCheckBox.UseVisualStyleBackColor = true;
			this.filterAltCheckBox.CheckedChanged += new System.EventHandler(this.filterAltCheckBox_CheckedChanged);
			//
			// ignoreAltTextBox
			//
			this.ignoreAltTextBox.Location = new System.Drawing.Point(240, 325);
			this.ignoreAltTextBox.Maximum = new decimal(new int[] { 200000, 0, 0, 0 });
			this.ignoreAltTextBox.Name = "ignoreAltTextBox";
			this.ignoreAltTextBox.Size = new System.Drawing.Size(100, 23);
			this.ignoreAltTextBox.TabIndex = 17;
			//
			// trailsLabel
			//
			this.trailsLabel.AutoSize = true;
			this.trailsLabel.Location = new System.Drawing.Point(16, 364);
			this.trailsLabel.Name = "trailsLabel";
			this.trailsLabel.Text = "Fetch flight trails every N checks";
			this.toolTip.SetToolTip(this.trailsLabel, "0 = never fetch trails, 1 = every check, 2 or more = every N checks. Trails are only fetched while there are active alerts.");
			//
			// trailsAgeNumericUpDown
			//
			this.trailsAgeNumericUpDown.Location = new System.Drawing.Point(240, 360);
			this.trailsAgeNumericUpDown.Maximum = new decimal(new int[] { 100, 0, 0, 0 });
			this.trailsAgeNumericUpDown.Name = "trailsAgeNumericUpDown";
			this.trailsAgeNumericUpDown.Size = new System.Drawing.Size(100, 23);
			this.trailsAgeNumericUpDown.TabIndex = 18;
			this.trailsAgeNumericUpDown.Value = new decimal(new int[] { 1, 0, 0, 0 });
			//
			// emailTabPage
			//
			this.emailTabPage.Controls.Add(this.senderLabel);
			this.emailTabPage.Controls.Add(this.senderEmailTextBox);
			this.emailTabPage.Controls.Add(this.smtpHostLabel);
			this.emailTabPage.Controls.Add(this.smtpHostComboBox);
			this.emailTabPage.Controls.Add(this.smtpPortLabel);
			this.emailTabPage.Controls.Add(this.smtpHostPortTextBox);
			this.emailTabPage.Controls.Add(this.smtpUserLabel);
			this.emailTabPage.Controls.Add(this.smtpUsrTextBox);
			this.emailTabPage.Controls.Add(this.smtpPasswordLabel);
			this.emailTabPage.Controls.Add(this.smtpPwdTextBox);
			this.emailTabPage.Controls.Add(this.smtpSSLCheckBox);
			this.emailTabPage.Controls.Add(this.gmailLink);
			this.emailTabPage.Controls.Add(this.testEmailLabel);
			this.emailTabPage.Controls.Add(this.testEmailAddressTextBox);
			this.emailTabPage.Controls.Add(this.sendTestEmailButton);
			this.emailTabPage.Controls.Add(this.testEmailResultLabel);
			this.emailTabPage.Location = new System.Drawing.Point(4, 24);
			this.emailTabPage.Name = "emailTabPage";
			this.emailTabPage.Padding = new System.Windows.Forms.Padding(3);
			this.emailTabPage.Size = new System.Drawing.Size(728, 412);
			this.emailTabPage.TabIndex = 1;
			this.emailTabPage.Text = "Email";
			//
			// senderLabel
			//
			this.senderLabel.AutoSize = true;
			this.senderLabel.Location = new System.Drawing.Point(16, 20);
			this.senderLabel.Name = "senderLabel";
			this.senderLabel.Text = "Send alerts from";
			//
			// senderEmailTextBox
			//
			this.senderEmailTextBox.Location = new System.Drawing.Point(200, 16);
			this.senderEmailTextBox.Name = "senderEmailTextBox";
			this.senderEmailTextBox.PlaceholderText = "you@example.com";
			this.senderEmailTextBox.Size = new System.Drawing.Size(300, 23);
			this.senderEmailTextBox.TabIndex = 0;
			//
			// smtpHostLabel
			//
			this.smtpHostLabel.AutoSize = true;
			this.smtpHostLabel.Location = new System.Drawing.Point(16, 52);
			this.smtpHostLabel.Name = "smtpHostLabel";
			this.smtpHostLabel.Text = "SMTP host";
			//
			// smtpHostComboBox
			//
			this.smtpHostComboBox.FormattingEnabled = true;
			this.smtpHostComboBox.Location = new System.Drawing.Point(200, 48);
			this.smtpHostComboBox.Name = "smtpHostComboBox";
			this.smtpHostComboBox.Size = new System.Drawing.Size(300, 23);
			this.smtpHostComboBox.TabIndex = 1;
			this.toolTip.SetToolTip(this.smtpHostComboBox, "Pick a common provider or type your own. Picking one fills in the port and SSL setting.");
			this.smtpHostComboBox.SelectedValueChanged += new System.EventHandler(this.smtpHostComboBox_SelectedValueChanged);
			//
			// smtpPortLabel
			//
			this.smtpPortLabel.AutoSize = true;
			this.smtpPortLabel.Location = new System.Drawing.Point(520, 52);
			this.smtpPortLabel.Name = "smtpPortLabel";
			this.smtpPortLabel.Text = "Port";
			//
			// smtpHostPortTextBox
			//
			this.smtpHostPortTextBox.Location = new System.Drawing.Point(556, 48);
			this.smtpHostPortTextBox.Maximum = new decimal(new int[] { 65535, 0, 0, 0 });
			this.smtpHostPortTextBox.Minimum = new decimal(new int[] { 1, 0, 0, 0 });
			this.smtpHostPortTextBox.Name = "smtpHostPortTextBox";
			this.smtpHostPortTextBox.Size = new System.Drawing.Size(80, 23);
			this.smtpHostPortTextBox.TabIndex = 2;
			this.smtpHostPortTextBox.Value = new decimal(new int[] { 587, 0, 0, 0 });
			//
			// smtpUserLabel
			//
			this.smtpUserLabel.AutoSize = true;
			this.smtpUserLabel.Location = new System.Drawing.Point(16, 84);
			this.smtpUserLabel.Name = "smtpUserLabel";
			this.smtpUserLabel.Text = "SMTP username";
			//
			// smtpUsrTextBox
			//
			this.smtpUsrTextBox.Location = new System.Drawing.Point(200, 80);
			this.smtpUsrTextBox.Name = "smtpUsrTextBox";
			this.smtpUsrTextBox.Size = new System.Drawing.Size(300, 23);
			this.smtpUsrTextBox.TabIndex = 3;
			//
			// smtpPasswordLabel
			//
			this.smtpPasswordLabel.AutoSize = true;
			this.smtpPasswordLabel.Location = new System.Drawing.Point(16, 116);
			this.smtpPasswordLabel.Name = "smtpPasswordLabel";
			this.smtpPasswordLabel.Text = "SMTP password";
			//
			// smtpPwdTextBox
			//
			this.smtpPwdTextBox.Location = new System.Drawing.Point(200, 112);
			this.smtpPwdTextBox.Name = "smtpPwdTextBox";
			this.smtpPwdTextBox.PasswordChar = '●';
			this.smtpPwdTextBox.Size = new System.Drawing.Size(300, 23);
			this.smtpPwdTextBox.TabIndex = 4;
			//
			// smtpSSLCheckBox
			//
			this.smtpSSLCheckBox.AutoSize = true;
			this.smtpSSLCheckBox.Location = new System.Drawing.Point(520, 114);
			this.smtpSSLCheckBox.Name = "smtpSSLCheckBox";
			this.smtpSSLCheckBox.TabIndex = 5;
			this.smtpSSLCheckBox.Text = "Use SSL/TLS";
			this.smtpSSLCheckBox.UseVisualStyleBackColor = true;
			//
			// gmailLink
			//
			this.gmailLink.AutoSize = true;
			this.gmailLink.Location = new System.Drawing.Point(200, 144);
			this.gmailLink.Name = "gmailLink";
			this.gmailLink.TabIndex = 6;
			this.gmailLink.TabStop = true;
			this.gmailLink.Text = "Using Gmail? Create an App Password and use it as the SMTP password";
			this.toolTip.SetToolTip(this.gmailLink, "Gmail no longer accepts your normal account password over SMTP.\r\nTurn on 2-Step Verification, create an App Password at myaccount.google.com/apppasswords, and enter it here.");
			this.gmailLink.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.gmailLink_LinkClicked);
			//
			// testEmailLabel
			//
			this.testEmailLabel.AutoSize = true;
			this.testEmailLabel.Location = new System.Drawing.Point(16, 196);
			this.testEmailLabel.Name = "testEmailLabel";
			this.testEmailLabel.Text = "Send a test email to";
			//
			// testEmailAddressTextBox
			//
			this.testEmailAddressTextBox.Location = new System.Drawing.Point(200, 192);
			this.testEmailAddressTextBox.Name = "testEmailAddressTextBox";
			this.testEmailAddressTextBox.PlaceholderText = "recipient@example.com";
			this.testEmailAddressTextBox.Size = new System.Drawing.Size(300, 23);
			this.testEmailAddressTextBox.TabIndex = 7;
			//
			// sendTestEmailButton
			//
			this.sendTestEmailButton.Location = new System.Drawing.Point(510, 190);
			this.sendTestEmailButton.Name = "sendTestEmailButton";
			this.sendTestEmailButton.Size = new System.Drawing.Size(126, 27);
			this.sendTestEmailButton.TabIndex = 8;
			this.sendTestEmailButton.Text = "Send test";
			this.toolTip.SetToolTip(this.sendTestEmailButton, "Sends a short message using the settings on this tab as they are now, saved or not");
			this.sendTestEmailButton.UseVisualStyleBackColor = true;
			this.sendTestEmailButton.Click += new System.EventHandler(this.sendTestEmailButton_Click);
			//
			// testEmailResultLabel
			//
			this.testEmailResultLabel.Location = new System.Drawing.Point(200, 222);
			this.testEmailResultLabel.Name = "testEmailResultLabel";
			this.testEmailResultLabel.Size = new System.Drawing.Size(510, 50);
			this.testEmailResultLabel.TabIndex = 9;
			//
			// alertsTabPage
			//
			this.alertsTabPage.Controls.Add(this.refreshLabel);
			this.alertsTabPage.Controls.Add(this.refreshTextBox);
			this.alertsTabPage.Controls.Add(this.removalTimeoutLabel);
			this.alertsTabPage.Controls.Add(this.removalTimeoutTextBox);
			this.alertsTabPage.Controls.Add(this.startOnStartCheckBox);
			this.alertsTabPage.Controls.Add(this.minimizeToTrayCheckBox);
			this.alertsTabPage.Controls.Add(this.alertActionsLabel);
			this.alertsTabPage.Controls.Add(this.soundAlertsCheckBox);
			this.alertsTabPage.Controls.Add(this.notificationsCheckBox);
			this.alertsTabPage.Controls.Add(this.flashWindowCheckBox);
			this.alertsTabPage.Location = new System.Drawing.Point(4, 24);
			this.alertsTabPage.Name = "alertsTabPage";
			this.alertsTabPage.Padding = new System.Windows.Forms.Padding(3);
			this.alertsTabPage.Size = new System.Drawing.Size(728, 412);
			this.alertsTabPage.TabIndex = 2;
			this.alertsTabPage.Text = "Alerts";
			//
			// refreshLabel
			//
			this.refreshLabel.AutoSize = true;
			this.refreshLabel.Location = new System.Drawing.Point(16, 20);
			this.refreshLabel.Name = "refreshLabel";
			this.refreshLabel.Text = "Check the radar every (secs)";
			//
			// refreshTextBox
			//
			this.refreshTextBox.Location = new System.Drawing.Point(260, 16);
			this.refreshTextBox.Maximum = new decimal(new int[] { 600, 0, 0, 0 });
			this.refreshTextBox.Minimum = new decimal(new int[] { 1, 0, 0, 0 });
			this.refreshTextBox.Name = "refreshTextBox";
			this.refreshTextBox.Size = new System.Drawing.Size(90, 23);
			this.refreshTextBox.TabIndex = 0;
			this.refreshTextBox.Value = new decimal(new int[] { 60, 0, 0, 0 });
			//
			// removalTimeoutLabel
			//
			this.removalTimeoutLabel.AutoSize = true;
			this.removalTimeoutLabel.Location = new System.Drawing.Point(16, 52);
			this.removalTimeoutLabel.Name = "removalTimeoutLabel";
			this.removalTimeoutLabel.Text = "Treat an aircraft as gone after (secs)";
			this.toolTip.SetToolTip(this.removalTimeoutLabel, "How long a matched aircraft can be missing from the radar before a last contact alert is sent");
			//
			// removalTimeoutTextBox
			//
			this.removalTimeoutTextBox.Location = new System.Drawing.Point(260, 48);
			this.removalTimeoutTextBox.Maximum = new decimal(new int[] { 86400, 0, 0, 0 });
			this.removalTimeoutTextBox.Minimum = new decimal(new int[] { 1, 0, 0, 0 });
			this.removalTimeoutTextBox.Name = "removalTimeoutTextBox";
			this.removalTimeoutTextBox.Size = new System.Drawing.Size(90, 23);
			this.removalTimeoutTextBox.TabIndex = 1;
			this.removalTimeoutTextBox.Value = new decimal(new int[] { 60, 0, 0, 0 });
			//
			// startOnStartCheckBox
			//
			this.startOnStartCheckBox.AutoSize = true;
			this.startOnStartCheckBox.Location = new System.Drawing.Point(16, 90);
			this.startOnStartCheckBox.Name = "startOnStartCheckBox";
			this.startOnStartCheckBox.TabIndex = 2;
			this.startOnStartCheckBox.Text = "Start checking as soon as PlaneAlerter opens";
			this.startOnStartCheckBox.UseVisualStyleBackColor = true;
			//
			// minimizeToTrayCheckBox
			//
			this.minimizeToTrayCheckBox.AutoSize = true;
			this.minimizeToTrayCheckBox.Location = new System.Drawing.Point(16, 116);
			this.minimizeToTrayCheckBox.Name = "minimizeToTrayCheckBox";
			this.minimizeToTrayCheckBox.TabIndex = 3;
			this.minimizeToTrayCheckBox.Text = "Hide to the notification area when minimised";
			this.minimizeToTrayCheckBox.UseVisualStyleBackColor = true;
			//
			// alertActionsLabel
			//
			this.alertActionsLabel.AutoSize = true;
			this.alertActionsLabel.Location = new System.Drawing.Point(16, 160);
			this.alertActionsLabel.Name = "alertActionsLabel";
			this.alertActionsLabel.Text = "When an alert is sent";
			//
			// soundAlertsCheckBox
			//
			this.soundAlertsCheckBox.AutoSize = true;
			this.soundAlertsCheckBox.Location = new System.Drawing.Point(32, 184);
			this.soundAlertsCheckBox.Name = "soundAlertsCheckBox";
			this.soundAlertsCheckBox.TabIndex = 4;
			this.soundAlertsCheckBox.Text = "Play a sound";
			this.soundAlertsCheckBox.UseVisualStyleBackColor = true;
			//
			// notificationsCheckBox
			//
			this.notificationsCheckBox.AutoSize = true;
			this.notificationsCheckBox.Location = new System.Drawing.Point(32, 208);
			this.notificationsCheckBox.Name = "notificationsCheckBox";
			this.notificationsCheckBox.TabIndex = 5;
			this.notificationsCheckBox.Text = "Show a notification";
			this.notificationsCheckBox.UseVisualStyleBackColor = true;
			//
			// flashWindowCheckBox
			//
			this.flashWindowCheckBox.AutoSize = true;
			this.flashWindowCheckBox.Location = new System.Drawing.Point(32, 232);
			this.flashWindowCheckBox.Name = "flashWindowCheckBox";
			this.flashWindowCheckBox.TabIndex = 6;
			this.flashWindowCheckBox.Text = "Flash the window";
			this.flashWindowCheckBox.UseVisualStyleBackColor = true;
			//
			// mapsTabPage
			//
			this.mapsTabPage.Controls.Add(this.centreLabel);
			this.mapsTabPage.Controls.Add(this.centreAircraftRadioButton);
			this.mapsTabPage.Controls.Add(this.centreLatLngRadioButton);
			this.mapsTabPage.Controls.Add(this.mapboxTokenLabel);
			this.mapsTabPage.Controls.Add(this.mapboxTokenTextBox);
			this.mapsTabPage.Controls.Add(this.mapboxHelpLabel);
			this.mapsTabPage.Controls.Add(this.mapboxLink);
			this.mapsTabPage.Location = new System.Drawing.Point(4, 24);
			this.mapsTabPage.Name = "mapsTabPage";
			this.mapsTabPage.Padding = new System.Windows.Forms.Padding(3);
			this.mapsTabPage.Size = new System.Drawing.Size(728, 412);
			this.mapsTabPage.TabIndex = 3;
			this.mapsTabPage.Text = "Maps";
			//
			// centreLabel
			//
			this.centreLabel.AutoSize = true;
			this.centreLabel.Location = new System.Drawing.Point(16, 20);
			this.centreLabel.Name = "centreLabel";
			this.centreLabel.Text = "Centre email maps on";
			//
			// centreAircraftRadioButton
			//
			this.centreAircraftRadioButton.AutoSize = true;
			this.centreAircraftRadioButton.Checked = true;
			this.centreAircraftRadioButton.Location = new System.Drawing.Point(200, 18);
			this.centreAircraftRadioButton.Name = "centreAircraftRadioButton";
			this.centreAircraftRadioButton.TabIndex = 0;
			this.centreAircraftRadioButton.TabStop = true;
			this.centreAircraftRadioButton.Text = "The aircraft";
			this.centreAircraftRadioButton.UseVisualStyleBackColor = true;
			//
			// centreLatLngRadioButton
			//
			this.centreLatLngRadioButton.AutoSize = true;
			this.centreLatLngRadioButton.Location = new System.Drawing.Point(310, 18);
			this.centreLatLngRadioButton.Name = "centreLatLngRadioButton";
			this.centreLatLngRadioButton.TabIndex = 1;
			this.centreLatLngRadioButton.Text = "My location (set on the Radar tab)";
			this.centreLatLngRadioButton.UseVisualStyleBackColor = true;
			//
			// mapboxTokenLabel
			//
			this.mapboxTokenLabel.AutoSize = true;
			this.mapboxTokenLabel.Location = new System.Drawing.Point(16, 60);
			this.mapboxTokenLabel.Name = "mapboxTokenLabel";
			this.mapboxTokenLabel.Text = "Mapbox access token";
			//
			// mapboxTokenTextBox
			//
			this.mapboxTokenTextBox.Location = new System.Drawing.Point(200, 56);
			this.mapboxTokenTextBox.Name = "mapboxTokenTextBox";
			this.mapboxTokenTextBox.PlaceholderText = "pk.…";
			this.mapboxTokenTextBox.Size = new System.Drawing.Size(450, 23);
			this.mapboxTokenTextBox.TabIndex = 2;
			//
			// mapboxHelpLabel
			//
			this.mapboxHelpLabel.Location = new System.Drawing.Point(200, 86);
			this.mapboxHelpLabel.Name = "mapboxHelpLabel";
			this.mapboxHelpLabel.Size = new System.Drawing.Size(450, 48);
			this.mapboxHelpLabel.TabIndex = 3;
			this.mapboxHelpLabel.Text = "The static map in each alert email is rendered by Mapbox. Public tokens start with \"pk.\" and the free tier is ample for alerts. Leave blank to use the built-in token, which belongs to the upstream author and may stop working at any time.";
			//
			// mapboxLink
			//
			this.mapboxLink.AutoSize = true;
			this.mapboxLink.Location = new System.Drawing.Point(200, 140);
			this.mapboxLink.Name = "mapboxLink";
			this.mapboxLink.TabIndex = 4;
			this.mapboxLink.TabStop = true;
			this.mapboxLink.Text = "Get a free Mapbox token";
			this.mapboxLink.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.mapboxLink_LinkClicked);
			//
			// saveSettingsButton
			//
			this.saveSettingsButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.saveSettingsButton.Location = new System.Drawing.Point(628, 466);
			this.saveSettingsButton.Name = "saveSettingsButton";
			this.saveSettingsButton.Size = new System.Drawing.Size(120, 30);
			this.saveSettingsButton.TabIndex = 1;
			this.saveSettingsButton.Text = "Save";
			this.saveSettingsButton.UseVisualStyleBackColor = true;
			this.saveSettingsButton.Click += new System.EventHandler(this.saveSettingsButton_Click);
			//
			// cancelButton
			//
			this.cancelButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.cancelButton.Location = new System.Drawing.Point(522, 466);
			this.cancelButton.Name = "cancelButton";
			this.cancelButton.Size = new System.Drawing.Size(100, 30);
			this.cancelButton.TabIndex = 2;
			this.cancelButton.Text = "Cancel";
			this.cancelButton.UseVisualStyleBackColor = true;
			this.cancelButton.Click += new System.EventHandler(this.cancelButton_Click);
			//
			// SettingsForm
			//
			this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.CancelButton = this.cancelButton;
			this.ClientSize = new System.Drawing.Size(760, 508);
			this.Controls.Add(this.cancelButton);
			this.Controls.Add(this.saveSettingsButton);
			this.Controls.Add(this.tabControl);
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.MinimumSize = new System.Drawing.Size(776, 547);
			this.Name = "SettingsForm";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			this.Text = "Settings";
			this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.SettingsForm_FormClosing);
			this.Load += new System.EventHandler(this.SettingsForm_Load);
			this.tabControl.ResumeLayout(false);
			this.radarTabPage.ResumeLayout(false);
			this.radarTabPage.PerformLayout();
			this.emailTabPage.ResumeLayout(false);
			this.emailTabPage.PerformLayout();
			this.alertsTabPage.ResumeLayout(false);
			this.alertsTabPage.PerformLayout();
			this.mapsTabPage.ResumeLayout(false);
			this.mapsTabPage.PerformLayout();
			((System.ComponentModel.ISupportInitialize)(this.timeoutTextBox)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.latTextBox)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.longTextBox)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.ignoreDistTextBox)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.ignoreAltTextBox)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.trailsAgeNumericUpDown)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.smtpHostPortTextBox)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.refreshTextBox)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.removalTimeoutTextBox)).EndInit();
			this.ResumeLayout(false);

		}

		private System.Windows.Forms.ToolTip toolTip;
		private System.Windows.Forms.TabControl tabControl;
		private System.Windows.Forms.TabPage radarTabPage;
		private System.Windows.Forms.TabPage emailTabPage;
		private System.Windows.Forms.TabPage alertsTabPage;
		private System.Windows.Forms.TabPage mapsTabPage;
		private System.Windows.Forms.Button saveSettingsButton;
		private System.Windows.Forms.Button cancelButton;
		private System.Windows.Forms.Label aircraftListLabel;
		private System.Windows.Forms.TextBox aircraftListTextBox;
		private System.Windows.Forms.Label radarUrlLabel;
		private System.Windows.Forms.TextBox radarURLTextBox;
		private System.Windows.Forms.Label vrsUserLabel;
		private System.Windows.Forms.TextBox VRSUsrTextBox;
		private System.Windows.Forms.Label vrsPasswordLabel;
		private System.Windows.Forms.MaskedTextBox VRSPwdTextBox;
		private System.Windows.Forms.Button testConnectionButton;
		private System.Windows.Forms.Label testResultLabel;
		private System.Windows.Forms.Label timeoutLabel;
		private System.Windows.Forms.NumericUpDown timeoutTextBox;
		private System.Windows.Forms.CheckBox filterReceiverCheckBox;
		private System.Windows.Forms.ComboBox receiverComboBox;
		private System.Windows.Forms.Button refreshReceiversButton;
		private System.Windows.Forms.Label latLabel;
		private System.Windows.Forms.NumericUpDown latTextBox;
		private System.Windows.Forms.Label longLabel;
		private System.Windows.Forms.NumericUpDown longTextBox;
		private System.Windows.Forms.Button useReceiverLocationButton;
		private System.Windows.Forms.CheckBox filterDstCheckBox;
		private System.Windows.Forms.NumericUpDown ignoreDistTextBox;
		private System.Windows.Forms.CheckBox ignoreModeSCheckBox;
		private System.Windows.Forms.CheckBox filterAltCheckBox;
		private System.Windows.Forms.NumericUpDown ignoreAltTextBox;
		private System.Windows.Forms.Label trailsLabel;
		private System.Windows.Forms.NumericUpDown trailsAgeNumericUpDown;
		private System.Windows.Forms.Label senderLabel;
		private System.Windows.Forms.TextBox senderEmailTextBox;
		private System.Windows.Forms.Label smtpHostLabel;
		private System.Windows.Forms.ComboBox smtpHostComboBox;
		private System.Windows.Forms.Label smtpPortLabel;
		private System.Windows.Forms.NumericUpDown smtpHostPortTextBox;
		private System.Windows.Forms.Label smtpUserLabel;
		private System.Windows.Forms.TextBox smtpUsrTextBox;
		private System.Windows.Forms.Label smtpPasswordLabel;
		private System.Windows.Forms.MaskedTextBox smtpPwdTextBox;
		private System.Windows.Forms.CheckBox smtpSSLCheckBox;
		private System.Windows.Forms.LinkLabel gmailLink;
		private System.Windows.Forms.Label testEmailLabel;
		private System.Windows.Forms.TextBox testEmailAddressTextBox;
		private System.Windows.Forms.Button sendTestEmailButton;
		private System.Windows.Forms.Label testEmailResultLabel;
		private System.Windows.Forms.Label refreshLabel;
		private System.Windows.Forms.NumericUpDown refreshTextBox;
		private System.Windows.Forms.Label removalTimeoutLabel;
		private System.Windows.Forms.NumericUpDown removalTimeoutTextBox;
		private System.Windows.Forms.CheckBox startOnStartCheckBox;
		private System.Windows.Forms.CheckBox minimizeToTrayCheckBox;
		private System.Windows.Forms.Label alertActionsLabel;
		private System.Windows.Forms.CheckBox soundAlertsCheckBox;
		private System.Windows.Forms.CheckBox notificationsCheckBox;
		private System.Windows.Forms.CheckBox flashWindowCheckBox;
		private System.Windows.Forms.Label centreLabel;
		private System.Windows.Forms.RadioButton centreAircraftRadioButton;
		private System.Windows.Forms.RadioButton centreLatLngRadioButton;
		private System.Windows.Forms.Label mapboxTokenLabel;
		private System.Windows.Forms.TextBox mapboxTokenTextBox;
		private System.Windows.Forms.Label mapboxHelpLabel;
		private System.Windows.Forms.LinkLabel mapboxLink;
	}
}
