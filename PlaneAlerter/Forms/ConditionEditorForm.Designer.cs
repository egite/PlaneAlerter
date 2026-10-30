namespace PlaneAlerter.Forms {
	partial class ConditionEditorForm {
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
		protected override void Dispose(bool disposing) {
			if(disposing && (components != null)) {
				components.Dispose();
			}
			base.Dispose(disposing);
		}

		#region Windows Form Designer generated code

		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent() {
			this.components = new System.ComponentModel.Container();
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ConditionEditorForm));
			this.conditionNameTextBox = new System.Windows.Forms.TextBox();
			this.conditionNameLabel = new System.Windows.Forms.Label();
			this.triggerDataGridView = new System.Windows.Forms.DataGridView();
			this.alertTypeLabel = new System.Windows.Forms.Label();
			this.saveButton = new System.Windows.Forms.Button();
			this.cancelButton = new System.Windows.Forms.Button();
			this.alertTypeComboBox = new System.Windows.Forms.ComboBox();
			this.emailToSendToLabel = new System.Windows.Forms.Label();
			this.receiverEmailTextBox = new System.Windows.Forms.TextBox();
			this.propertyInfoButton = new System.Windows.Forms.Button();
			this.toolTip = new System.Windows.Forms.ToolTip(this.components);
			this.emailFirstFormatTextBox = new System.Windows.Forms.TextBox();
			this.label1 = new System.Windows.Forms.Label();
			this.label2 = new System.Windows.Forms.Label();
			this.emailLastFormatTextBox = new System.Windows.Forms.TextBox();
			this.ignoreFollowingCheckbox = new System.Windows.Forms.CheckBox();
			this.emailCheckBox = new System.Windows.Forms.CheckBox();
			this.emailGroupBox = new System.Windows.Forms.GroupBox();
			this.label4 = new System.Windows.Forms.Label();
			this.triggerLogicPanel = new System.Windows.Forms.Panel();
			this.triggersLogicAnyRadioButton = new System.Windows.Forms.RadioButton();
			this.triggersLogicAllRadioButton = new System.Windows.Forms.RadioButton();
			this.propertyColumn = new System.Windows.Forms.DataGridViewComboBoxColumn();
			this.comparisonTypeColumn = new System.Windows.Forms.DataGridViewComboBoxColumn();
			this.valueColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.deleteColumn = new System.Windows.Forms.DataGridViewButtonColumn();
			((System.ComponentModel.ISupportInitialize)(this.triggerDataGridView)).BeginInit();
			this.emailGroupBox.SuspendLayout();
			this.triggerLogicPanel.SuspendLayout();
			this.SuspendLayout();
			//
			// conditionNameTextBox
			//
			this.conditionNameTextBox.Location = new System.Drawing.Point(109, 14);
			this.conditionNameTextBox.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
			this.conditionNameTextBox.Name = "conditionNameTextBox";
			this.conditionNameTextBox.Size = new System.Drawing.Size(238, 23);
			this.conditionNameTextBox.TabIndex = 0;
			//
			// conditionNameLabel
			//
			this.conditionNameLabel.AutoSize = true;
			this.conditionNameLabel.Location = new System.Drawing.Point(8, 17);
			this.conditionNameLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
			this.conditionNameLabel.Name = "conditionNameLabel";
			this.conditionNameLabel.Size = new System.Drawing.Size(98, 15);
			this.conditionNameLabel.TabIndex = 1;
			this.conditionNameLabel.Text = "Condition Name:";
			this.toolTip.SetToolTip(this.conditionNameLabel, "The name for the condition");
			//
			// triggerDataGridView
			//
			this.triggerDataGridView.AllowUserToResizeRows = false;
			this.triggerDataGridView.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
            | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
			this.triggerDataGridView.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
			this.triggerDataGridView.BackgroundColor = System.Drawing.SystemColors.ControlLight;
			this.triggerDataGridView.BorderStyle = System.Windows.Forms.BorderStyle.None;
			this.triggerDataGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
			this.triggerDataGridView.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.propertyColumn,
            this.comparisonTypeColumn,
            this.valueColumn,
            this.deleteColumn});
			this.triggerDataGridView.Location = new System.Drawing.Point(9, 73);
			this.triggerDataGridView.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
			this.triggerDataGridView.MultiSelect = false;
			this.triggerDataGridView.Name = "triggerDataGridView";
			this.triggerDataGridView.RowHeadersVisible = false;
			this.triggerDataGridView.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.CellSelect;
			this.triggerDataGridView.Size = new System.Drawing.Size(572, 376);
			this.triggerDataGridView.TabIndex = 7;
			this.triggerDataGridView.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.triggerDataGridView_CellContentClick);
			this.triggerDataGridView.CellMouseClick += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.triggerDataGridView_CellMouseClick);
			this.triggerDataGridView.CellValueChanged += new System.Windows.Forms.DataGridViewCellEventHandler(this.triggerDataGridView_CellValueChanged);
			this.triggerDataGridView.UserAddedRow += new System.Windows.Forms.DataGridViewRowEventHandler(this.TriggerDataGridViewUserAddedRow);
			//
			// alertTypeLabel
			//
			this.alertTypeLabel.AutoSize = true;
			this.alertTypeLabel.Location = new System.Drawing.Point(45, 47);
			this.alertTypeLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
			this.alertTypeLabel.Name = "alertTypeLabel";
			this.alertTypeLabel.Size = new System.Drawing.Size(62, 15);
			this.alertTypeLabel.TabIndex = 5;
			this.alertTypeLabel.Text = "Alert Type:";
			this.toolTip.SetToolTip(this.alertTypeLabel, "Disabled: Never alert\r\nFirst: Alert on first contact\r\nLast: Alert on last contact" +
        "\r\nBoth: Alert on first and last contact");
			//
			// saveButton
			//
			this.saveButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.saveButton.Location = new System.Drawing.Point(735, 426);
			this.saveButton.Margin = new System.Windows.Forms.Padding(2);
			this.saveButton.Name = "saveButton";
			this.saveButton.Size = new System.Drawing.Size(141, 26);
			this.saveButton.TabIndex = 5;
			this.saveButton.Text = "Save Condition";
			this.saveButton.UseVisualStyleBackColor = true;
			this.saveButton.Click += new System.EventHandler(this.SaveButtonClick);
			//
			// cancelButton
			//
			this.cancelButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.cancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.cancelButton.Location = new System.Drawing.Point(640, 426);
			this.cancelButton.Margin = new System.Windows.Forms.Padding(2);
			this.cancelButton.Name = "cancelButton";
			this.cancelButton.Size = new System.Drawing.Size(90, 26);
			this.cancelButton.TabIndex = 6;
			this.cancelButton.Text = "Cancel";
			this.cancelButton.UseVisualStyleBackColor = true;
			//
			// alertTypeComboBox
			//
			this.alertTypeComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.alertTypeComboBox.FormattingEnabled = true;
			this.alertTypeComboBox.Location = new System.Drawing.Point(109, 43);
			this.alertTypeComboBox.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
			this.alertTypeComboBox.Name = "alertTypeComboBox";
			this.alertTypeComboBox.Size = new System.Drawing.Size(238, 23);
			this.alertTypeComboBox.TabIndex = 3;
			this.alertTypeComboBox.SelectedIndexChanged += new System.EventHandler(this.alertTypeComboBox_SelectedIndexChanged);
			//
			// emailToSendToLabel
			//
			this.emailToSendToLabel.AutoSize = true;
			this.emailToSendToLabel.Location = new System.Drawing.Point(9, 150);
			this.emailToSendToLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
			this.emailToSendToLabel.Name = "emailToSendToLabel";
			this.emailToSendToLabel.Size = new System.Drawing.Size(186, 15);
			this.emailToSendToLabel.TabIndex = 9;
			this.emailToSendToLabel.Text = "Emails to send to (One each line): ";
			this.toolTip.SetToolTip(this.emailToSendToLabel, "The emails that will receive alert messages (one per line)");
			//
			// receiverEmailTextBox
			//
			this.receiverEmailTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
            | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
			this.receiverEmailTextBox.Location = new System.Drawing.Point(11, 169);
			this.receiverEmailTextBox.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
			this.receiverEmailTextBox.Multiline = true;
			this.receiverEmailTextBox.Name = "receiverEmailTextBox";
			this.receiverEmailTextBox.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
			this.receiverEmailTextBox.Size = new System.Drawing.Size(262, 224);
			this.receiverEmailTextBox.TabIndex = 1;
			//
			// propertyInfoButton
			//
			this.propertyInfoButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.propertyInfoButton.Location = new System.Drawing.Point(9, 426);
			this.propertyInfoButton.Margin = new System.Windows.Forms.Padding(2);
			this.propertyInfoButton.Name = "propertyInfoButton";
			this.propertyInfoButton.Size = new System.Drawing.Size(141, 26);
			this.propertyInfoButton.TabIndex = 4;
			this.propertyInfoButton.Text = "Property Info";
			this.propertyInfoButton.UseVisualStyleBackColor = true;
			this.propertyInfoButton.Click += new System.EventHandler(this.propertyInfoButton_Click);
			//
			// toolTip
			//
			this.toolTip.AutomaticDelay = 0;
			this.toolTip.AutoPopDelay = 5000;
			this.toolTip.InitialDelay = 10;
			this.toolTip.ReshowDelay = 100;
			this.toolTip.UseAnimation = false;
			this.toolTip.UseFading = false;
			//
			// emailFirstFormatTextBox
			//
			this.emailFirstFormatTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
			this.emailFirstFormatTextBox.Location = new System.Drawing.Point(11, 70);
			this.emailFirstFormatTextBox.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
			this.emailFirstFormatTextBox.Name = "emailFirstFormatTextBox";
			this.emailFirstFormatTextBox.Size = new System.Drawing.Size(262, 23);
			this.emailFirstFormatTextBox.TabIndex = 11;
			this.toolTip.SetToolTip(this.emailFirstFormatTextBox, resources.GetString("emailFirstFormatTextBox.ToolTip"));
			//
			// label1
			//
			this.label1.AutoSize = true;
			this.label1.Location = new System.Drawing.Point(9, 51);
			this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(160, 15);
			this.label1.TabIndex = 12;
			this.label1.Text = "First Contact Subject Format:";
			this.toolTip.SetToolTip(this.label1, "Format for first contact alert emails");
			//
			// label2
			//
			this.label2.AutoSize = true;
			this.label2.Location = new System.Drawing.Point(9, 100);
			this.label2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(159, 15);
			this.label2.TabIndex = 14;
			this.label2.Text = "Last Contact Subject Format:";
			this.toolTip.SetToolTip(this.label2, "Format for last contact alert emails");
			//
			// emailLastFormatTextBox
			//
			this.emailLastFormatTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
			this.emailLastFormatTextBox.Location = new System.Drawing.Point(11, 118);
			this.emailLastFormatTextBox.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
			this.emailLastFormatTextBox.Name = "emailLastFormatTextBox";
			this.emailLastFormatTextBox.Size = new System.Drawing.Size(262, 23);
			this.emailLastFormatTextBox.TabIndex = 13;
			this.toolTip.SetToolTip(this.emailLastFormatTextBox, resources.GetString("emailLastFormatTextBox.ToolTip"));
			//
			// ignoreFollowingCheckbox
			//
			this.ignoreFollowingCheckbox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.ignoreFollowingCheckbox.AutoSize = true;
			this.ignoreFollowingCheckbox.Location = new System.Drawing.Point(355, 16);
			this.ignoreFollowingCheckbox.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
			this.ignoreFollowingCheckbox.Name = "ignoreFollowingCheckbox";
			this.ignoreFollowingCheckbox.Size = new System.Drawing.Size(172, 19);
			this.ignoreFollowingCheckbox.TabIndex = 6;
			this.ignoreFollowingCheckbox.Text = "Ignore following conditions";
			this.ignoreFollowingCheckbox.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.ignoreFollowingCheckbox.UseVisualStyleBackColor = true;
			//
			// emailCheckBox
			//
			this.emailCheckBox.AutoSize = true;
			this.emailCheckBox.Location = new System.Drawing.Point(12, 25);
			this.emailCheckBox.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
			this.emailCheckBox.Name = "emailCheckBox";
			this.emailCheckBox.Size = new System.Drawing.Size(133, 19);
			this.emailCheckBox.TabIndex = 10;
			this.emailCheckBox.Text = "Email Alerts Enabled";
			this.emailCheckBox.UseVisualStyleBackColor = true;
			this.emailCheckBox.CheckedChanged += new System.EventHandler(this.emailCheckBox_CheckedChanged);
			//
			// emailGroupBox
			//
			this.emailGroupBox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
            | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
			this.emailGroupBox.Controls.Add(this.emailCheckBox);
			this.emailGroupBox.Controls.Add(this.label1);
			this.emailGroupBox.Controls.Add(this.emailFirstFormatTextBox);
			this.emailGroupBox.Controls.Add(this.label2);
			this.emailGroupBox.Controls.Add(this.emailLastFormatTextBox);
			this.emailGroupBox.Controls.Add(this.emailToSendToLabel);
			this.emailGroupBox.Controls.Add(this.receiverEmailTextBox);
			this.emailGroupBox.Location = new System.Drawing.Point(589, 14);
			this.emailGroupBox.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
			this.emailGroupBox.Name = "emailGroupBox";
			this.emailGroupBox.Padding = new System.Windows.Forms.Padding(4, 3, 4, 3);
			this.emailGroupBox.Size = new System.Drawing.Size(287, 407);
			this.emailGroupBox.TabIndex = 12;
			this.emailGroupBox.TabStop = false;
			this.emailGroupBox.Text = "Email Alerts";
			//
			// label4
			//
			this.label4.AutoSize = true;
			this.label4.Location = new System.Drawing.Point(354, 47);
			this.label4.Name = "label4";
			this.label4.Size = new System.Drawing.Size(64, 15);
			this.label4.TabIndex = 13;
			this.label4.Text = "Alert when";
			//
			// triggerLogicPanel
			//
			this.triggerLogicPanel.Controls.Add(this.triggersLogicAnyRadioButton);
			this.triggerLogicPanel.Controls.Add(this.triggersLogicAllRadioButton);
			this.triggerLogicPanel.Location = new System.Drawing.Point(418, 38);
			this.triggerLogicPanel.Name = "triggerLogicPanel";
			this.triggerLogicPanel.Size = new System.Drawing.Size(163, 34);
			this.triggerLogicPanel.TabIndex = 14;
			//
			// triggersLogicAnyRadioButton
			//
			this.triggersLogicAnyRadioButton.AutoSize = true;
			this.triggersLogicAnyRadioButton.Location = new System.Drawing.Point(78, 7);
			this.triggersLogicAnyRadioButton.Name = "triggersLogicAnyRadioButton";
			this.triggersLogicAnyRadioButton.Size = new System.Drawing.Size(81, 19);
			this.triggersLogicAnyRadioButton.TabIndex = 1;
			this.triggersLogicAnyRadioButton.TabStop = true;
			this.triggersLogicAnyRadioButton.Text = "any match";
			this.triggersLogicAnyRadioButton.UseVisualStyleBackColor = true;
			//
			// triggersLogicAllRadioButton
			//
			this.triggersLogicAllRadioButton.AutoSize = true;
			this.triggersLogicAllRadioButton.Location = new System.Drawing.Point(3, 7);
			this.triggersLogicAllRadioButton.Name = "triggersLogicAllRadioButton";
			this.triggersLogicAllRadioButton.Size = new System.Drawing.Size(74, 19);
			this.triggersLogicAllRadioButton.TabIndex = 0;
			this.triggersLogicAllRadioButton.TabStop = true;
			this.triggersLogicAllRadioButton.Text = "all match";
			this.triggersLogicAllRadioButton.UseVisualStyleBackColor = true;
			//
			// propertyColumn
			//
			this.propertyColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
			this.propertyColumn.FillWeight = 6.497205F;
			this.propertyColumn.HeaderText = "Property";
			this.propertyColumn.Name = "propertyColumn";
			this.propertyColumn.Resizable = System.Windows.Forms.DataGridViewTriState.False;
			this.propertyColumn.Width = 175;
			//
			// comparisonTypeColumn
			//
			this.comparisonTypeColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
			this.comparisonTypeColumn.FillWeight = 8.731219F;
			this.comparisonTypeColumn.HeaderText = "Comparison Type";
			this.comparisonTypeColumn.Name = "comparisonTypeColumn";
			this.comparisonTypeColumn.Resizable = System.Windows.Forms.DataGridViewTriState.False;
			this.comparisonTypeColumn.Width = 110;
			//
			// valueColumn
			//
			this.valueColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
			this.valueColumn.FillWeight = 284.7716F;
			this.valueColumn.HeaderText = "Value";
			this.valueColumn.Name = "valueColumn";
			this.valueColumn.Resizable = System.Windows.Forms.DataGridViewTriState.False;
			this.valueColumn.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
			//
			// deleteColumn
			//
			this.deleteColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
			this.deleteColumn.HeaderText = "";
			this.deleteColumn.Name = "deleteColumn";
			this.deleteColumn.Text = "X";
			this.deleteColumn.UseColumnTextForButtonValue = true;
			this.deleteColumn.Width = 23;
			//
			// ConditionEditorForm
			//
			this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.CancelButton = this.cancelButton;
			this.ClientSize = new System.Drawing.Size(884, 461);
			this.Controls.Add(this.triggerLogicPanel);
			this.Controls.Add(this.label4);
			this.Controls.Add(this.emailGroupBox);
			this.Controls.Add(this.triggerDataGridView);
			this.Controls.Add(this.ignoreFollowingCheckbox);
			this.Controls.Add(this.propertyInfoButton);
			this.Controls.Add(this.alertTypeComboBox);
			this.Controls.Add(this.saveButton);
			this.Controls.Add(this.cancelButton);
			this.Controls.Add(this.alertTypeLabel);
			this.Controls.Add(this.conditionNameLabel);
			this.Controls.Add(this.conditionNameTextBox);
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
			this.MaximizeBox = false;
			this.MaximumSize = new System.Drawing.Size(900, 11532);
			this.MinimumSize = new System.Drawing.Size(900, 500);
			this.Name = "ConditionEditorForm";
			this.Text = "PlaneAlerter Condition Editor";
			((System.ComponentModel.ISupportInitialize)(this.triggerDataGridView)).EndInit();
			this.emailGroupBox.ResumeLayout(false);
			this.emailGroupBox.PerformLayout();
			this.triggerLogicPanel.ResumeLayout(false);
			this.triggerLogicPanel.PerformLayout();
			this.ResumeLayout(false);
			this.PerformLayout();

		}
		private System.Windows.Forms.Button saveButton;
		private System.Windows.Forms.Button cancelButton;

		#endregion

		private System.Windows.Forms.TextBox conditionNameTextBox;
		private System.Windows.Forms.Label conditionNameLabel;
		private System.Windows.Forms.DataGridView triggerDataGridView;
		private System.Windows.Forms.Label alertTypeLabel;
		private System.Windows.Forms.ComboBox alertTypeComboBox;
		private System.Windows.Forms.Label emailToSendToLabel;
		private System.Windows.Forms.TextBox receiverEmailTextBox;
		private System.Windows.Forms.Button propertyInfoButton;
		private System.Windows.Forms.ToolTip toolTip;
		private System.Windows.Forms.CheckBox ignoreFollowingCheckbox;
		private System.Windows.Forms.CheckBox emailCheckBox;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.TextBox emailLastFormatTextBox;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.TextBox emailFirstFormatTextBox;
		private System.Windows.Forms.GroupBox emailGroupBox;
		private System.Windows.Forms.Label label4;
		private System.Windows.Forms.Panel triggerLogicPanel;
		private System.Windows.Forms.RadioButton triggersLogicAnyRadioButton;
		private System.Windows.Forms.RadioButton triggersLogicAllRadioButton;
		private System.Windows.Forms.DataGridViewComboBoxColumn propertyColumn;
		private System.Windows.Forms.DataGridViewComboBoxColumn comparisonTypeColumn;
		private System.Windows.Forms.DataGridViewTextBoxColumn valueColumn;
		private System.Windows.Forms.DataGridViewButtonColumn deleteColumn;
	}
}
