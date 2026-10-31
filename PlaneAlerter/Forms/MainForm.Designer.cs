namespace PlaneAlerter.Forms {
	partial class MainForm {
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
			this.menuStrip = new System.Windows.Forms.MenuStrip();
			this.fileMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.exitMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.toolsMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.conditionEditorMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.settingsMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.emailContentConfigMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.toolsSeparator = new System.Windows.Forms.ToolStripSeparator();
			this.propertyInfoMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.showLogFileMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.viewMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.clearConsoleMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.helpMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.donateMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.aboutMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.toolbarPanel = new System.Windows.Forms.Panel();
			this.startStopButton = new System.Windows.Forms.Button();
			this.restartButton = new System.Windows.Forms.Button();
			this.conditionsButton = new System.Windows.Forms.Button();
			this.settingsButton = new System.Windows.Forms.Button();
			this.statusStrip = new System.Windows.Forms.StatusStrip();
			this.statusLabel = new System.Windows.Forms.ToolStripStatusLabel();
			this.nextCheckLabel = new System.Windows.Forms.ToolStripStatusLabel();
			this.aircraftCountLabel = new System.Windows.Forms.ToolStripStatusLabel();
			this.alertsSentLabel = new System.Windows.Forms.ToolStripStatusLabel();
			this.uptimeLabel = new System.Windows.Forms.ToolStripStatusLabel();
			this.splitContainer1 = new System.Windows.Forms.SplitContainer();
			this.splitContainer2 = new System.Windows.Forms.SplitContainer();
			this.conditionsGrid = new System.Windows.Forms.DataGridView();
			this.conditionNameColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.conditionAlertTypeColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.conditionTriggersColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.conditionRecipientsColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.conditionAlertsColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.emptyStatePanel = new System.Windows.Forms.Panel();
			this.emptyStateLabel = new System.Windows.Forms.Label();
			this.addConditionButton = new System.Windows.Forms.Button();
			this.conditionsHeaderLabel = new System.Windows.Forms.Label();
			this.activeMatchesGrid = new System.Windows.Forms.DataGridView();
			this.matchIcaoColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.matchRegColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.matchTypeColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.matchCallsignColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.matchConditionColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.matchSinceColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.matchStatusColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.activeAlertsLabel = new System.Windows.Forms.Label();
			this.console = new System.Windows.Forms.RichTextBox();
			this.consoleHeaderPanel = new System.Windows.Forms.Panel();
			this.consoleLabel = new System.Windows.Forms.Label();
			this.consoleFilterComboBox = new System.Windows.Forms.ComboBox();
			this.autoScrollCheckBox = new System.Windows.Forms.CheckBox();
			this.copyConsoleButton = new System.Windows.Forms.Button();
			this.clearConsoleButton = new System.Windows.Forms.Button();
			this.notifyIcon = new System.Windows.Forms.NotifyIcon(this.components);
			this.trayMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
			this.trayShowMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.trayStartStopMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.traySeparator = new System.Windows.Forms.ToolStripSeparator();
			this.trayExitMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.uiTimer = new System.Windows.Forms.Timer(this.components);
			this.menuStrip.SuspendLayout();
			this.toolbarPanel.SuspendLayout();
			this.statusStrip.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
			this.splitContainer1.Panel1.SuspendLayout();
			this.splitContainer1.Panel2.SuspendLayout();
			this.splitContainer1.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).BeginInit();
			this.splitContainer2.Panel1.SuspendLayout();
			this.splitContainer2.Panel2.SuspendLayout();
			this.splitContainer2.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.conditionsGrid)).BeginInit();
			this.emptyStatePanel.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.activeMatchesGrid)).BeginInit();
			this.consoleHeaderPanel.SuspendLayout();
			this.trayMenu.SuspendLayout();
			this.SuspendLayout();
			//
			// menuStrip
			//
			this.menuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileMenuItem,
            this.toolsMenuItem,
            this.viewMenuItem,
            this.helpMenuItem});
			this.menuStrip.Location = new System.Drawing.Point(0, 0);
			this.menuStrip.Name = "menuStrip";
			this.menuStrip.Size = new System.Drawing.Size(1100, 24);
			this.menuStrip.TabIndex = 0;
			//
			// fileMenuItem
			//
			this.fileMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.exitMenuItem});
			this.fileMenuItem.Name = "fileMenuItem";
			this.fileMenuItem.Text = "File";
			//
			// exitMenuItem
			//
			this.exitMenuItem.Name = "exitMenuItem";
			this.exitMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Alt | System.Windows.Forms.Keys.F4)));
			this.exitMenuItem.Text = "Exit";
			this.exitMenuItem.Click += new System.EventHandler(this.exitMenuItem_Click);
			//
			// toolsMenuItem
			//
			this.toolsMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.conditionEditorMenuItem,
            this.settingsMenuItem,
            this.emailContentConfigMenuItem,
            this.toolsSeparator,
            this.propertyInfoMenuItem,
            this.showLogFileMenuItem});
			this.toolsMenuItem.Name = "toolsMenuItem";
			this.toolsMenuItem.Text = "Tools";
			//
			// conditionEditorMenuItem
			//
			this.conditionEditorMenuItem.Name = "conditionEditorMenuItem";
			this.conditionEditorMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.E)));
			this.conditionEditorMenuItem.Text = "Condition Editor...";
			this.conditionEditorMenuItem.Click += new System.EventHandler(this.conditionEditorMenuItem_Click);
			//
			// settingsMenuItem
			//
			this.settingsMenuItem.Name = "settingsMenuItem";
			this.settingsMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Oemcomma)));
			this.settingsMenuItem.Text = "Settings...";
			this.settingsMenuItem.Click += new System.EventHandler(this.settingsMenuItem_Click);
			//
			// emailContentConfigMenuItem
			//
			this.emailContentConfigMenuItem.Name = "emailContentConfigMenuItem";
			this.emailContentConfigMenuItem.Text = "Email Content...";
			this.emailContentConfigMenuItem.Click += new System.EventHandler(this.emailContentConfigMenuItem_Click);
			//
			// toolsSeparator
			//
			this.toolsSeparator.Name = "toolsSeparator";
			//
			// propertyInfoMenuItem
			//
			this.propertyInfoMenuItem.Name = "propertyInfoMenuItem";
			this.propertyInfoMenuItem.Text = "Property Reference";
			this.propertyInfoMenuItem.Click += new System.EventHandler(this.propertyInfoMenuItem_Click);
			//
			// showLogFileMenuItem
			//
			this.showLogFileMenuItem.Name = "showLogFileMenuItem";
			this.showLogFileMenuItem.Text = "Show alerts.log in Explorer";
			this.showLogFileMenuItem.Click += new System.EventHandler(this.showLogFileMenuItem_Click);
			//
			// viewMenuItem
			//
			this.viewMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.clearConsoleMenuItem});
			this.viewMenuItem.Name = "viewMenuItem";
			this.viewMenuItem.Text = "View";
			//
			// clearConsoleMenuItem
			//
			this.clearConsoleMenuItem.Name = "clearConsoleMenuItem";
			this.clearConsoleMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.L)));
			this.clearConsoleMenuItem.Text = "Clear Console";
			this.clearConsoleMenuItem.Click += new System.EventHandler(this.clearConsole_Click);
			//
			// helpMenuItem
			//
			this.helpMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.donateMenuItem,
            this.aboutMenuItem});
			this.helpMenuItem.Name = "helpMenuItem";
			this.helpMenuItem.Text = "Help";
			//
			// donateMenuItem
			//
			this.donateMenuItem.Name = "donateMenuItem";
			this.donateMenuItem.Text = "Donate to the original author";
			this.donateMenuItem.Click += new System.EventHandler(this.donateMenuItem_Click);
			//
			// aboutMenuItem
			//
			this.aboutMenuItem.Name = "aboutMenuItem";
			this.aboutMenuItem.Text = "About PlaneAlerter";
			this.aboutMenuItem.Click += new System.EventHandler(this.aboutMenuItem_Click);
			//
			// toolbarPanel
			//
			this.toolbarPanel.Controls.Add(this.settingsButton);
			this.toolbarPanel.Controls.Add(this.conditionsButton);
			this.toolbarPanel.Controls.Add(this.restartButton);
			this.toolbarPanel.Controls.Add(this.startStopButton);
			this.toolbarPanel.Dock = System.Windows.Forms.DockStyle.Top;
			this.toolbarPanel.Location = new System.Drawing.Point(0, 24);
			this.toolbarPanel.Name = "toolbarPanel";
			this.toolbarPanel.Padding = new System.Windows.Forms.Padding(8, 6, 8, 6);
			this.toolbarPanel.Size = new System.Drawing.Size(1100, 44);
			this.toolbarPanel.TabIndex = 1;
			//
			// startStopButton
			//
			this.startStopButton.Location = new System.Drawing.Point(10, 7);
			this.startStopButton.Name = "startStopButton";
			this.startStopButton.Size = new System.Drawing.Size(110, 30);
			this.startStopButton.TabIndex = 0;
			this.startStopButton.Text = "Start";
			this.startStopButton.UseVisualStyleBackColor = true;
			this.startStopButton.Click += new System.EventHandler(this.startStopButton_Click);
			//
			// restartButton
			//
			this.restartButton.Enabled = false;
			this.restartButton.Location = new System.Drawing.Point(126, 7);
			this.restartButton.Name = "restartButton";
			this.restartButton.Size = new System.Drawing.Size(90, 30);
			this.restartButton.TabIndex = 1;
			this.restartButton.Text = "Restart";
			this.restartButton.UseVisualStyleBackColor = true;
			this.restartButton.Click += new System.EventHandler(this.restartButton_Click);
			//
			// conditionsButton
			//
			this.conditionsButton.Location = new System.Drawing.Point(240, 7);
			this.conditionsButton.Name = "conditionsButton";
			this.conditionsButton.Size = new System.Drawing.Size(130, 30);
			this.conditionsButton.TabIndex = 2;
			this.conditionsButton.Text = "Edit Conditions...";
			this.conditionsButton.UseVisualStyleBackColor = true;
			this.conditionsButton.Click += new System.EventHandler(this.conditionEditorMenuItem_Click);
			//
			// settingsButton
			//
			this.settingsButton.Location = new System.Drawing.Point(376, 7);
			this.settingsButton.Name = "settingsButton";
			this.settingsButton.Size = new System.Drawing.Size(100, 30);
			this.settingsButton.TabIndex = 3;
			this.settingsButton.Text = "Settings...";
			this.settingsButton.UseVisualStyleBackColor = true;
			this.settingsButton.Click += new System.EventHandler(this.settingsMenuItem_Click);
			//
			// statusStrip
			//
			this.statusStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.statusLabel,
            this.nextCheckLabel,
            this.aircraftCountLabel,
            this.alertsSentLabel,
            this.uptimeLabel});
			this.statusStrip.Location = new System.Drawing.Point(0, 578);
			this.statusStrip.Name = "statusStrip";
			this.statusStrip.Padding = new System.Windows.Forms.Padding(8, 0, 16, 0);
			this.statusStrip.Size = new System.Drawing.Size(1100, 22);
			this.statusStrip.TabIndex = 3;
			//
			// statusLabel
			//
			this.statusLabel.Name = "statusLabel";
			this.statusLabel.Spring = true;
			this.statusLabel.Text = "Idle";
			this.statusLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			//
			// nextCheckLabel
			//
			this.nextCheckLabel.Name = "nextCheckLabel";
			this.nextCheckLabel.Text = "Next check: —";
			this.nextCheckLabel.Padding = new System.Windows.Forms.Padding(8, 0, 8, 0);
			//
			// aircraftCountLabel
			//
			this.aircraftCountLabel.Name = "aircraftCountLabel";
			this.aircraftCountLabel.Text = "Aircraft: —";
			this.aircraftCountLabel.Padding = new System.Windows.Forms.Padding(8, 0, 8, 0);
			//
			// alertsSentLabel
			//
			this.alertsSentLabel.Name = "alertsSentLabel";
			this.alertsSentLabel.Text = "Alerts sent: 0";
			this.alertsSentLabel.Padding = new System.Windows.Forms.Padding(8, 0, 8, 0);
			//
			// uptimeLabel
			//
			this.uptimeLabel.Name = "uptimeLabel";
			this.uptimeLabel.Text = "Up 0m";
			this.uptimeLabel.Padding = new System.Windows.Forms.Padding(8, 0, 0, 0);
			//
			// splitContainer1
			//
			this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.splitContainer1.Location = new System.Drawing.Point(0, 68);
			this.splitContainer1.Name = "splitContainer1";
			//
			// splitContainer1.Panel1
			//
			this.splitContainer1.Panel1.Controls.Add(this.splitContainer2);
			this.splitContainer1.Panel1MinSize = 300;
			//
			// splitContainer1.Panel2
			//
			this.splitContainer1.Panel2.Controls.Add(this.console);
			this.splitContainer1.Panel2.Controls.Add(this.consoleHeaderPanel);
			this.splitContainer1.Panel2MinSize = 300;
			this.splitContainer1.Size = new System.Drawing.Size(1100, 510);
			this.splitContainer1.SplitterDistance = 520;
			this.splitContainer1.SplitterWidth = 6;
			this.splitContainer1.TabIndex = 2;
			//
			// splitContainer2
			//
			this.splitContainer2.Dock = System.Windows.Forms.DockStyle.Fill;
			this.splitContainer2.Location = new System.Drawing.Point(0, 0);
			this.splitContainer2.Name = "splitContainer2";
			this.splitContainer2.Orientation = System.Windows.Forms.Orientation.Horizontal;
			//
			// splitContainer2.Panel1
			//
			this.splitContainer2.Panel1.Controls.Add(this.conditionsGrid);
			this.splitContainer2.Panel1.Controls.Add(this.emptyStatePanel);
			this.splitContainer2.Panel1.Controls.Add(this.conditionsHeaderLabel);
			this.splitContainer2.Panel1.Padding = new System.Windows.Forms.Padding(8, 6, 4, 0);
			this.splitContainer2.Panel1MinSize = 80;
			//
			// splitContainer2.Panel2
			//
			this.splitContainer2.Panel2.Controls.Add(this.activeMatchesGrid);
			this.splitContainer2.Panel2.Controls.Add(this.activeAlertsLabel);
			this.splitContainer2.Panel2.Padding = new System.Windows.Forms.Padding(8, 4, 4, 8);
			this.splitContainer2.Panel2MinSize = 80;
			this.splitContainer2.Size = new System.Drawing.Size(520, 510);
			this.splitContainer2.SplitterDistance = 250;
			this.splitContainer2.SplitterWidth = 6;
			this.splitContainer2.TabIndex = 0;
			//
			// conditionsGrid
			//
			this.conditionsGrid.AllowUserToAddRows = false;
			this.conditionsGrid.AllowUserToDeleteRows = false;
			this.conditionsGrid.AllowUserToResizeRows = false;
			this.conditionsGrid.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
			this.conditionsGrid.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
			this.conditionsGrid.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.conditionNameColumn,
            this.conditionAlertTypeColumn,
            this.conditionTriggersColumn,
            this.conditionRecipientsColumn,
            this.conditionAlertsColumn});
			this.conditionsGrid.Dock = System.Windows.Forms.DockStyle.Fill;
			this.conditionsGrid.Location = new System.Drawing.Point(8, 30);
			this.conditionsGrid.MultiSelect = false;
			this.conditionsGrid.Name = "conditionsGrid";
			this.conditionsGrid.ReadOnly = true;
			this.conditionsGrid.RowHeadersVisible = false;
			this.conditionsGrid.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
			this.conditionsGrid.ShowCellToolTips = true;
			this.conditionsGrid.Size = new System.Drawing.Size(508, 220);
			this.conditionsGrid.TabIndex = 1;
			this.conditionsGrid.CellDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.conditionsGrid_CellDoubleClick);
			//
			// conditionNameColumn
			//
			this.conditionNameColumn.FillWeight = 30F;
			this.conditionNameColumn.HeaderText = "Condition";
			this.conditionNameColumn.Name = "conditionNameColumn";
			this.conditionNameColumn.ReadOnly = true;
			//
			// conditionAlertTypeColumn
			//
			this.conditionAlertTypeColumn.FillWeight = 20F;
			this.conditionAlertTypeColumn.HeaderText = "Alert on";
			this.conditionAlertTypeColumn.Name = "conditionAlertTypeColumn";
			this.conditionAlertTypeColumn.ReadOnly = true;
			//
			// conditionTriggersColumn
			//
			this.conditionTriggersColumn.FillWeight = 32F;
			this.conditionTriggersColumn.HeaderText = "Triggers";
			this.conditionTriggersColumn.Name = "conditionTriggersColumn";
			this.conditionTriggersColumn.ReadOnly = true;
			//
			// conditionRecipientsColumn
			//
			this.conditionRecipientsColumn.FillWeight = 9F;
			this.conditionRecipientsColumn.HeaderText = "Emails";
			this.conditionRecipientsColumn.Name = "conditionRecipientsColumn";
			this.conditionRecipientsColumn.ReadOnly = true;
			//
			// conditionAlertsColumn
			//
			this.conditionAlertsColumn.FillWeight = 9F;
			this.conditionAlertsColumn.HeaderText = "Sent";
			this.conditionAlertsColumn.Name = "conditionAlertsColumn";
			this.conditionAlertsColumn.ReadOnly = true;
			//
			// emptyStatePanel
			//
			this.emptyStatePanel.Controls.Add(this.addConditionButton);
			this.emptyStatePanel.Controls.Add(this.emptyStateLabel);
			this.emptyStatePanel.Dock = System.Windows.Forms.DockStyle.Fill;
			this.emptyStatePanel.Location = new System.Drawing.Point(8, 30);
			this.emptyStatePanel.Name = "emptyStatePanel";
			this.emptyStatePanel.Size = new System.Drawing.Size(508, 220);
			this.emptyStatePanel.TabIndex = 2;
			this.emptyStatePanel.Visible = false;
			//
			// emptyStateLabel
			//
			this.emptyStateLabel.Anchor = System.Windows.Forms.AnchorStyles.None;
			this.emptyStateLabel.Location = new System.Drawing.Point(54, 60);
			this.emptyStateLabel.Name = "emptyStateLabel";
			this.emptyStateLabel.Size = new System.Drawing.Size(400, 50);
			this.emptyStateLabel.TabIndex = 0;
			this.emptyStateLabel.Text = "No conditions yet.\r\nA condition describes which aircraft should trigger an alert, and who gets the email.";
			this.emptyStateLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			//
			// addConditionButton
			//
			this.addConditionButton.Anchor = System.Windows.Forms.AnchorStyles.None;
			this.addConditionButton.Location = new System.Drawing.Point(179, 118);
			this.addConditionButton.Name = "addConditionButton";
			this.addConditionButton.Size = new System.Drawing.Size(150, 30);
			this.addConditionButton.TabIndex = 1;
			this.addConditionButton.Text = "Add a condition";
			this.addConditionButton.UseVisualStyleBackColor = true;
			this.addConditionButton.Click += new System.EventHandler(this.conditionEditorMenuItem_Click);
			//
			// conditionsHeaderLabel
			//
			this.conditionsHeaderLabel.Dock = System.Windows.Forms.DockStyle.Top;
			this.conditionsHeaderLabel.Location = new System.Drawing.Point(8, 6);
			this.conditionsHeaderLabel.Name = "conditionsHeaderLabel";
			this.conditionsHeaderLabel.Size = new System.Drawing.Size(508, 24);
			this.conditionsHeaderLabel.TabIndex = 0;
			this.conditionsHeaderLabel.Text = "Conditions";
			this.conditionsHeaderLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			//
			// activeMatchesGrid
			//
			this.activeMatchesGrid.AllowUserToAddRows = false;
			this.activeMatchesGrid.AllowUserToDeleteRows = false;
			this.activeMatchesGrid.AllowUserToResizeRows = false;
			this.activeMatchesGrid.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
			this.activeMatchesGrid.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
			this.activeMatchesGrid.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.matchIcaoColumn,
            this.matchRegColumn,
            this.matchTypeColumn,
            this.matchCallsignColumn,
            this.matchConditionColumn,
            this.matchSinceColumn,
            this.matchStatusColumn});
			this.activeMatchesGrid.Dock = System.Windows.Forms.DockStyle.Fill;
			this.activeMatchesGrid.Location = new System.Drawing.Point(8, 28);
			this.activeMatchesGrid.MultiSelect = false;
			this.activeMatchesGrid.Name = "activeMatchesGrid";
			this.activeMatchesGrid.ReadOnly = true;
			this.activeMatchesGrid.RowHeadersVisible = false;
			this.activeMatchesGrid.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
			this.activeMatchesGrid.Size = new System.Drawing.Size(508, 218);
			this.activeMatchesGrid.TabIndex = 1;
			this.activeMatchesGrid.CellDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.activeMatchesGrid_CellDoubleClick);
			//
			// matchIcaoColumn
			//
			this.matchIcaoColumn.FillWeight = 12F;
			this.matchIcaoColumn.HeaderText = "ICAO";
			this.matchIcaoColumn.Name = "matchIcaoColumn";
			this.matchIcaoColumn.ReadOnly = true;
			//
			// matchRegColumn
			//
			this.matchRegColumn.FillWeight = 12F;
			this.matchRegColumn.HeaderText = "Reg";
			this.matchRegColumn.Name = "matchRegColumn";
			this.matchRegColumn.ReadOnly = true;
			//
			// matchTypeColumn
			//
			this.matchTypeColumn.FillWeight = 10F;
			this.matchTypeColumn.HeaderText = "Type";
			this.matchTypeColumn.Name = "matchTypeColumn";
			this.matchTypeColumn.ReadOnly = true;
			//
			// matchCallsignColumn
			//
			this.matchCallsignColumn.FillWeight = 14F;
			this.matchCallsignColumn.HeaderText = "Callsign";
			this.matchCallsignColumn.Name = "matchCallsignColumn";
			this.matchCallsignColumn.ReadOnly = true;
			//
			// matchConditionColumn
			//
			this.matchConditionColumn.FillWeight = 24F;
			this.matchConditionColumn.HeaderText = "Condition";
			this.matchConditionColumn.Name = "matchConditionColumn";
			this.matchConditionColumn.ReadOnly = true;
			//
			// matchSinceColumn
			//
			this.matchSinceColumn.FillWeight = 12F;
			this.matchSinceColumn.HeaderText = "Since";
			this.matchSinceColumn.Name = "matchSinceColumn";
			this.matchSinceColumn.ReadOnly = true;
			//
			// matchStatusColumn
			//
			this.matchStatusColumn.FillWeight = 16F;
			this.matchStatusColumn.HeaderText = "Status";
			this.matchStatusColumn.Name = "matchStatusColumn";
			this.matchStatusColumn.ReadOnly = true;
			//
			// activeAlertsLabel
			//
			this.activeAlertsLabel.Dock = System.Windows.Forms.DockStyle.Top;
			this.activeAlertsLabel.Location = new System.Drawing.Point(8, 4);
			this.activeAlertsLabel.Name = "activeAlertsLabel";
			this.activeAlertsLabel.Size = new System.Drawing.Size(508, 24);
			this.activeAlertsLabel.TabIndex = 0;
			this.activeAlertsLabel.Text = "Active alerts (0)";
			this.activeAlertsLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			//
			// console
			//
			this.console.Dock = System.Windows.Forms.DockStyle.Fill;
			this.console.Font = new System.Drawing.Font("Consolas", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
			this.console.Location = new System.Drawing.Point(0, 32);
			this.console.Name = "console";
			this.console.ReadOnly = true;
			this.console.Size = new System.Drawing.Size(574, 478);
			this.console.TabIndex = 1;
			this.console.Text = "";
			this.console.WordWrap = false;
			this.console.LinkClicked += new System.Windows.Forms.LinkClickedEventHandler(this.ConsoleLinkClicked);
			//
			// consoleHeaderPanel
			//
			this.consoleHeaderPanel.Controls.Add(this.clearConsoleButton);
			this.consoleHeaderPanel.Controls.Add(this.copyConsoleButton);
			this.consoleHeaderPanel.Controls.Add(this.autoScrollCheckBox);
			this.consoleHeaderPanel.Controls.Add(this.consoleFilterComboBox);
			this.consoleHeaderPanel.Controls.Add(this.consoleLabel);
			this.consoleHeaderPanel.Dock = System.Windows.Forms.DockStyle.Top;
			this.consoleHeaderPanel.Location = new System.Drawing.Point(0, 0);
			this.consoleHeaderPanel.Name = "consoleHeaderPanel";
			this.consoleHeaderPanel.Size = new System.Drawing.Size(574, 32);
			this.consoleHeaderPanel.TabIndex = 0;
			//
			// consoleLabel
			//
			this.consoleLabel.Location = new System.Drawing.Point(6, 4);
			this.consoleLabel.Name = "consoleLabel";
			this.consoleLabel.Size = new System.Drawing.Size(70, 24);
			this.consoleLabel.TabIndex = 0;
			this.consoleLabel.Text = "Console";
			this.consoleLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			//
			// consoleFilterComboBox
			//
			this.consoleFilterComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.consoleFilterComboBox.Items.AddRange(new object[] {
            "All messages",
            "Alert activity",
            "Warnings and errors"});
			this.consoleFilterComboBox.Location = new System.Drawing.Point(80, 5);
			this.consoleFilterComboBox.Name = "consoleFilterComboBox";
			this.consoleFilterComboBox.Size = new System.Drawing.Size(160, 23);
			this.consoleFilterComboBox.TabIndex = 1;
			this.consoleFilterComboBox.SelectedIndexChanged += new System.EventHandler(this.consoleFilterComboBox_SelectedIndexChanged);
			//
			// autoScrollCheckBox
			//
			this.autoScrollCheckBox.AutoSize = true;
			this.autoScrollCheckBox.Checked = true;
			this.autoScrollCheckBox.CheckState = System.Windows.Forms.CheckState.Checked;
			this.autoScrollCheckBox.Location = new System.Drawing.Point(250, 7);
			this.autoScrollCheckBox.Name = "autoScrollCheckBox";
			this.autoScrollCheckBox.Size = new System.Drawing.Size(84, 19);
			this.autoScrollCheckBox.TabIndex = 2;
			this.autoScrollCheckBox.Text = "Auto-scroll";
			this.autoScrollCheckBox.UseVisualStyleBackColor = true;
			//
			// copyConsoleButton
			//
			this.copyConsoleButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.copyConsoleButton.Location = new System.Drawing.Point(428, 4);
			this.copyConsoleButton.Name = "copyConsoleButton";
			this.copyConsoleButton.Size = new System.Drawing.Size(64, 25);
			this.copyConsoleButton.TabIndex = 3;
			this.copyConsoleButton.Text = "Copy";
			this.copyConsoleButton.UseVisualStyleBackColor = true;
			this.copyConsoleButton.Click += new System.EventHandler(this.copyConsoleButton_Click);
			//
			// clearConsoleButton
			//
			this.clearConsoleButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.clearConsoleButton.Location = new System.Drawing.Point(498, 4);
			this.clearConsoleButton.Name = "clearConsoleButton";
			this.clearConsoleButton.Size = new System.Drawing.Size(64, 25);
			this.clearConsoleButton.TabIndex = 4;
			this.clearConsoleButton.Text = "Clear";
			this.clearConsoleButton.UseVisualStyleBackColor = true;
			this.clearConsoleButton.Click += new System.EventHandler(this.clearConsole_Click);
			//
			// notifyIcon
			//
			this.notifyIcon.ContextMenuStrip = this.trayMenu;
			this.notifyIcon.Icon = ((System.Drawing.Icon)(resources.GetObject("notifyIcon.Icon")));
			this.notifyIcon.Text = "PlaneAlerter";
			this.notifyIcon.Visible = true;
			this.notifyIcon.BalloonTipClicked += new System.EventHandler(this.notifyIcon_Activate);
			this.notifyIcon.DoubleClick += new System.EventHandler(this.notifyIcon_Activate);
			//
			// trayMenu
			//
			this.trayMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.trayShowMenuItem,
            this.trayStartStopMenuItem,
            this.traySeparator,
            this.trayExitMenuItem});
			this.trayMenu.Name = "trayMenu";
			this.trayMenu.Size = new System.Drawing.Size(181, 76);
			//
			// trayShowMenuItem
			//
			this.trayShowMenuItem.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
			this.trayShowMenuItem.Name = "trayShowMenuItem";
			this.trayShowMenuItem.Text = "Show PlaneAlerter";
			this.trayShowMenuItem.Click += new System.EventHandler(this.notifyIcon_Activate);
			//
			// trayStartStopMenuItem
			//
			this.trayStartStopMenuItem.Name = "trayStartStopMenuItem";
			this.trayStartStopMenuItem.Text = "Start";
			this.trayStartStopMenuItem.Click += new System.EventHandler(this.startStopButton_Click);
			//
			// traySeparator
			//
			this.traySeparator.Name = "traySeparator";
			//
			// trayExitMenuItem
			//
			this.trayExitMenuItem.Name = "trayExitMenuItem";
			this.trayExitMenuItem.Text = "Exit";
			this.trayExitMenuItem.Click += new System.EventHandler(this.exitMenuItem_Click);
			//
			// uiTimer
			//
			this.uiTimer.Interval = 1000;
			this.uiTimer.Tick += new System.EventHandler(this.uiTimer_Tick);
			//
			// MainForm
			//
			this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(1100, 600);
			this.Controls.Add(this.splitContainer1);
			this.Controls.Add(this.toolbarPanel);
			this.Controls.Add(this.statusStrip);
			this.Controls.Add(this.menuStrip);
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.MainMenuStrip = this.menuStrip;
			this.MinimumSize = new System.Drawing.Size(720, 400);
			this.Name = "MainForm";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "PlaneAlerter";
			this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MainForm_FormClosing);
			this.Resize += new System.EventHandler(this.MainForm_Resize);
			this.menuStrip.ResumeLayout(false);
			this.menuStrip.PerformLayout();
			this.toolbarPanel.ResumeLayout(false);
			this.statusStrip.ResumeLayout(false);
			this.statusStrip.PerformLayout();
			this.splitContainer1.Panel1.ResumeLayout(false);
			this.splitContainer1.Panel2.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
			this.splitContainer1.ResumeLayout(false);
			this.splitContainer2.Panel1.ResumeLayout(false);
			this.splitContainer2.Panel2.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).EndInit();
			this.splitContainer2.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.conditionsGrid)).EndInit();
			this.emptyStatePanel.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.activeMatchesGrid)).EndInit();
			this.consoleHeaderPanel.ResumeLayout(false);
			this.consoleHeaderPanel.PerformLayout();
			this.trayMenu.ResumeLayout(false);
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		private System.Windows.Forms.MenuStrip menuStrip;
		private System.Windows.Forms.ToolStripMenuItem fileMenuItem;
		private System.Windows.Forms.ToolStripMenuItem exitMenuItem;
		private System.Windows.Forms.ToolStripMenuItem toolsMenuItem;
		private System.Windows.Forms.ToolStripMenuItem conditionEditorMenuItem;
		private System.Windows.Forms.ToolStripMenuItem settingsMenuItem;
		private System.Windows.Forms.ToolStripMenuItem emailContentConfigMenuItem;
		private System.Windows.Forms.ToolStripSeparator toolsSeparator;
		private System.Windows.Forms.ToolStripMenuItem propertyInfoMenuItem;
		private System.Windows.Forms.ToolStripMenuItem showLogFileMenuItem;
		private System.Windows.Forms.ToolStripMenuItem viewMenuItem;
		private System.Windows.Forms.ToolStripMenuItem clearConsoleMenuItem;
		private System.Windows.Forms.ToolStripMenuItem helpMenuItem;
		private System.Windows.Forms.ToolStripMenuItem donateMenuItem;
		private System.Windows.Forms.ToolStripMenuItem aboutMenuItem;
		private System.Windows.Forms.Panel toolbarPanel;
		private System.Windows.Forms.Button startStopButton;
		private System.Windows.Forms.Button restartButton;
		private System.Windows.Forms.Button conditionsButton;
		private System.Windows.Forms.Button settingsButton;
		private System.Windows.Forms.StatusStrip statusStrip;
		private System.Windows.Forms.ToolStripStatusLabel statusLabel;
		private System.Windows.Forms.ToolStripStatusLabel nextCheckLabel;
		private System.Windows.Forms.ToolStripStatusLabel aircraftCountLabel;
		private System.Windows.Forms.ToolStripStatusLabel alertsSentLabel;
		private System.Windows.Forms.ToolStripStatusLabel uptimeLabel;
		private System.Windows.Forms.SplitContainer splitContainer1;
		private System.Windows.Forms.SplitContainer splitContainer2;
		private System.Windows.Forms.DataGridView conditionsGrid;
		private System.Windows.Forms.DataGridViewTextBoxColumn conditionNameColumn;
		private System.Windows.Forms.DataGridViewTextBoxColumn conditionAlertTypeColumn;
		private System.Windows.Forms.DataGridViewTextBoxColumn conditionTriggersColumn;
		private System.Windows.Forms.DataGridViewTextBoxColumn conditionRecipientsColumn;
		private System.Windows.Forms.DataGridViewTextBoxColumn conditionAlertsColumn;
		private System.Windows.Forms.Panel emptyStatePanel;
		private System.Windows.Forms.Label emptyStateLabel;
		private System.Windows.Forms.Button addConditionButton;
		private System.Windows.Forms.Label conditionsHeaderLabel;
		private System.Windows.Forms.DataGridView activeMatchesGrid;
		private System.Windows.Forms.DataGridViewTextBoxColumn matchIcaoColumn;
		private System.Windows.Forms.DataGridViewTextBoxColumn matchRegColumn;
		private System.Windows.Forms.DataGridViewTextBoxColumn matchTypeColumn;
		private System.Windows.Forms.DataGridViewTextBoxColumn matchCallsignColumn;
		private System.Windows.Forms.DataGridViewTextBoxColumn matchConditionColumn;
		private System.Windows.Forms.DataGridViewTextBoxColumn matchSinceColumn;
		private System.Windows.Forms.DataGridViewTextBoxColumn matchStatusColumn;
		private System.Windows.Forms.Label activeAlertsLabel;
		private System.Windows.Forms.RichTextBox console;
		private System.Windows.Forms.Panel consoleHeaderPanel;
		private System.Windows.Forms.Label consoleLabel;
		private System.Windows.Forms.ComboBox consoleFilterComboBox;
		private System.Windows.Forms.CheckBox autoScrollCheckBox;
		private System.Windows.Forms.Button copyConsoleButton;
		private System.Windows.Forms.Button clearConsoleButton;
		private System.Windows.Forms.NotifyIcon notifyIcon;
		private System.Windows.Forms.ContextMenuStrip trayMenu;
		private System.Windows.Forms.ToolStripMenuItem trayShowMenuItem;
		private System.Windows.Forms.ToolStripMenuItem trayStartStopMenuItem;
		private System.Windows.Forms.ToolStripSeparator traySeparator;
		private System.Windows.Forms.ToolStripMenuItem trayExitMenuItem;
		private System.Windows.Forms.Timer uiTimer;
	}
}
