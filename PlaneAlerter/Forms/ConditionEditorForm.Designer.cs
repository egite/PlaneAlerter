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
			this.leftPanel = new System.Windows.Forms.Panel();
			this.conditionsListBox = new System.Windows.Forms.ListBox();
			this.listButtonsPanel = new System.Windows.Forms.Panel();
			this.addButton = new System.Windows.Forms.Button();
			this.duplicateButton = new System.Windows.Forms.Button();
			this.removeButton = new System.Windows.Forms.Button();
			this.moveUpButton = new System.Windows.Forms.Button();
			this.moveDownButton = new System.Windows.Forms.Button();
			this.conditionsHeaderLabel = new System.Windows.Forms.Label();
			this.editorPanel = new System.Windows.Forms.Panel();
			this.conditionNameLabel = new System.Windows.Forms.Label();
			this.conditionNameTextBox = new System.Windows.Forms.TextBox();
			this.ignoreFollowingCheckbox = new System.Windows.Forms.CheckBox();
			this.alertTypeLabel = new System.Windows.Forms.Label();
			this.alertTypeComboBox = new System.Windows.Forms.ComboBox();
			this.alertWhenLabel = new System.Windows.Forms.Label();
			this.triggerLogicPanel = new System.Windows.Forms.Panel();
			this.triggersLogicAnyRadioButton = new System.Windows.Forms.RadioButton();
			this.triggersLogicAllRadioButton = new System.Windows.Forms.RadioButton();
			this.triggersHeaderLabel = new System.Windows.Forms.Label();
			this.triggerDataGridView = new System.Windows.Forms.DataGridView();
			this.propertyColumn = new System.Windows.Forms.DataGridViewComboBoxColumn();
			this.comparisonTypeColumn = new System.Windows.Forms.DataGridViewComboBoxColumn();
			this.valueColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.typeColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.deleteColumn = new System.Windows.Forms.DataGridViewButtonColumn();
			this.emailGroupBox = new System.Windows.Forms.GroupBox();
			this.emailCheckBox = new System.Windows.Forms.CheckBox();
			this.firstSubjectLabel = new System.Windows.Forms.Label();
			this.emailFirstFormatTextBox = new System.Windows.Forms.TextBox();
			this.insertFirstButton = new System.Windows.Forms.Button();
			this.firstPreviewLabel = new System.Windows.Forms.Label();
			this.lastSubjectLabel = new System.Windows.Forms.Label();
			this.emailLastFormatTextBox = new System.Windows.Forms.TextBox();
			this.insertLastButton = new System.Windows.Forms.Button();
			this.lastPreviewLabel = new System.Windows.Forms.Label();
			this.emailToSendToLabel = new System.Windows.Forms.Label();
			this.receiverEmailTextBox = new System.Windows.Forms.TextBox();
			this.propertyInfoButton = new System.Windows.Forms.Button();
			this.errorLabel = new System.Windows.Forms.Label();
			this.cancelButton = new System.Windows.Forms.Button();
			this.saveButton = new System.Windows.Forms.Button();
			this.toolTip = new System.Windows.Forms.ToolTip(this.components);
			this.errorProvider = new System.Windows.Forms.ErrorProvider(this.components);
			this.leftPanel.SuspendLayout();
			this.listButtonsPanel.SuspendLayout();
			this.editorPanel.SuspendLayout();
			this.triggerLogicPanel.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.triggerDataGridView)).BeginInit();
			this.emailGroupBox.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.errorProvider)).BeginInit();
			this.SuspendLayout();
			//
			// leftPanel
			//
			this.leftPanel.Controls.Add(this.conditionsListBox);
			this.leftPanel.Controls.Add(this.listButtonsPanel);
			this.leftPanel.Controls.Add(this.conditionsHeaderLabel);
			this.leftPanel.Dock = System.Windows.Forms.DockStyle.Left;
			this.leftPanel.Location = new System.Drawing.Point(0, 0);
			this.leftPanel.Name = "leftPanel";
			this.leftPanel.Padding = new System.Windows.Forms.Padding(10, 10, 6, 10);
			this.leftPanel.Size = new System.Drawing.Size(300, 560);
			this.leftPanel.TabIndex = 0;
			//
			// conditionsListBox
			//
			this.conditionsListBox.AllowDrop = true;
			this.conditionsListBox.Dock = System.Windows.Forms.DockStyle.Fill;
			this.conditionsListBox.FormattingEnabled = true;
			this.conditionsListBox.IntegralHeight = false;
			this.conditionsListBox.ItemHeight = 15;
			this.conditionsListBox.Location = new System.Drawing.Point(10, 36);
			this.conditionsListBox.Name = "conditionsListBox";
			this.conditionsListBox.Size = new System.Drawing.Size(284, 444);
			this.conditionsListBox.TabIndex = 1;
			this.conditionsListBox.DragDrop += new System.Windows.Forms.DragEventHandler(this.conditionsListBox_DragDrop);
			this.conditionsListBox.DragOver += new System.Windows.Forms.DragEventHandler(this.conditionsListBox_DragOver);
			this.conditionsListBox.SelectedIndexChanged += new System.EventHandler(this.conditionsListBox_SelectedIndexChanged);
			this.conditionsListBox.MouseDown += new System.Windows.Forms.MouseEventHandler(this.conditionsListBox_MouseDown);
			this.conditionsListBox.MouseMove += new System.Windows.Forms.MouseEventHandler(this.conditionsListBox_MouseMove);
			//
			// listButtonsPanel
			//
			this.listButtonsPanel.Controls.Add(this.moveDownButton);
			this.listButtonsPanel.Controls.Add(this.moveUpButton);
			this.listButtonsPanel.Controls.Add(this.removeButton);
			this.listButtonsPanel.Controls.Add(this.duplicateButton);
			this.listButtonsPanel.Controls.Add(this.addButton);
			this.listButtonsPanel.Dock = System.Windows.Forms.DockStyle.Bottom;
			this.listButtonsPanel.Location = new System.Drawing.Point(10, 480);
			this.listButtonsPanel.Name = "listButtonsPanel";
			this.listButtonsPanel.Size = new System.Drawing.Size(284, 70);
			this.listButtonsPanel.TabIndex = 2;
			//
			// addButton
			//
			this.addButton.Location = new System.Drawing.Point(0, 6);
			this.addButton.Name = "addButton";
			this.addButton.Size = new System.Drawing.Size(88, 28);
			this.addButton.TabIndex = 0;
			this.addButton.Text = "Add";
			this.addButton.UseVisualStyleBackColor = true;
			this.addButton.Click += new System.EventHandler(this.addButton_Click);
			//
			// duplicateButton
			//
			this.duplicateButton.Location = new System.Drawing.Point(94, 6);
			this.duplicateButton.Name = "duplicateButton";
			this.duplicateButton.Size = new System.Drawing.Size(96, 28);
			this.duplicateButton.TabIndex = 1;
			this.duplicateButton.Text = "Duplicate";
			this.duplicateButton.UseVisualStyleBackColor = true;
			this.duplicateButton.Click += new System.EventHandler(this.duplicateButton_Click);
			//
			// removeButton
			//
			this.removeButton.Location = new System.Drawing.Point(196, 6);
			this.removeButton.Name = "removeButton";
			this.removeButton.Size = new System.Drawing.Size(88, 28);
			this.removeButton.TabIndex = 2;
			this.removeButton.Text = "Remove";
			this.removeButton.UseVisualStyleBackColor = true;
			this.removeButton.Click += new System.EventHandler(this.removeButton_Click);
			//
			// moveUpButton
			//
			this.moveUpButton.Location = new System.Drawing.Point(0, 38);
			this.moveUpButton.Name = "moveUpButton";
			this.moveUpButton.Size = new System.Drawing.Size(88, 28);
			this.moveUpButton.TabIndex = 3;
			this.moveUpButton.Text = "Move up";
			this.moveUpButton.UseVisualStyleBackColor = true;
			this.moveUpButton.Click += new System.EventHandler(this.moveUpButton_Click);
			//
			// moveDownButton
			//
			this.moveDownButton.Location = new System.Drawing.Point(94, 38);
			this.moveDownButton.Name = "moveDownButton";
			this.moveDownButton.Size = new System.Drawing.Size(96, 28);
			this.moveDownButton.TabIndex = 4;
			this.moveDownButton.Text = "Move down";
			this.moveDownButton.UseVisualStyleBackColor = true;
			this.moveDownButton.Click += new System.EventHandler(this.moveDownButton_Click);
			//
			// conditionsHeaderLabel
			//
			this.conditionsHeaderLabel.Dock = System.Windows.Forms.DockStyle.Top;
			this.conditionsHeaderLabel.Location = new System.Drawing.Point(10, 10);
			this.conditionsHeaderLabel.Name = "conditionsHeaderLabel";
			this.conditionsHeaderLabel.Size = new System.Drawing.Size(284, 26);
			this.conditionsHeaderLabel.TabIndex = 0;
			this.conditionsHeaderLabel.Text = "Conditions";
			this.conditionsHeaderLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.toolTip.SetToolTip(this.conditionsHeaderLabel, "Conditions are checked in this order. Drag to reorder.");
			//
			// editorPanel
			//
			this.editorPanel.Controls.Add(this.saveButton);
			this.editorPanel.Controls.Add(this.cancelButton);
			this.editorPanel.Controls.Add(this.errorLabel);
			this.editorPanel.Controls.Add(this.propertyInfoButton);
			this.editorPanel.Controls.Add(this.emailGroupBox);
			this.editorPanel.Controls.Add(this.triggerDataGridView);
			this.editorPanel.Controls.Add(this.triggersHeaderLabel);
			this.editorPanel.Controls.Add(this.triggerLogicPanel);
			this.editorPanel.Controls.Add(this.alertWhenLabel);
			this.editorPanel.Controls.Add(this.alertTypeComboBox);
			this.editorPanel.Controls.Add(this.alertTypeLabel);
			this.editorPanel.Controls.Add(this.ignoreFollowingCheckbox);
			this.editorPanel.Controls.Add(this.conditionNameTextBox);
			this.editorPanel.Controls.Add(this.conditionNameLabel);
			this.editorPanel.Dock = System.Windows.Forms.DockStyle.Fill;
			this.editorPanel.Location = new System.Drawing.Point(300, 0);
			this.editorPanel.Name = "editorPanel";
			this.editorPanel.Size = new System.Drawing.Size(780, 560);
			this.editorPanel.TabIndex = 1;
			//
			// conditionNameLabel
			//
			this.conditionNameLabel.AutoSize = true;
			this.conditionNameLabel.Location = new System.Drawing.Point(12, 16);
			this.conditionNameLabel.Name = "conditionNameLabel";
			this.conditionNameLabel.Size = new System.Drawing.Size(98, 15);
			this.conditionNameLabel.TabIndex = 0;
			this.conditionNameLabel.Text = "Condition name:";
			//
			// conditionNameTextBox
			//
			this.conditionNameTextBox.Location = new System.Drawing.Point(120, 12);
			this.conditionNameTextBox.Name = "conditionNameTextBox";
			this.conditionNameTextBox.Size = new System.Drawing.Size(300, 23);
			this.conditionNameTextBox.TabIndex = 1;
			this.conditionNameTextBox.TextChanged += new System.EventHandler(this.conditionNameTextBox_TextChanged);
			//
			// ignoreFollowingCheckbox
			//
			this.ignoreFollowingCheckbox.AutoSize = true;
			this.ignoreFollowingCheckbox.Location = new System.Drawing.Point(440, 14);
			this.ignoreFollowingCheckbox.Name = "ignoreFollowingCheckbox";
			this.ignoreFollowingCheckbox.Size = new System.Drawing.Size(248, 19);
			this.ignoreFollowingCheckbox.TabIndex = 2;
			this.ignoreFollowingCheckbox.Text = "Stop checking later conditions on a match";
			this.toolTip.SetToolTip(this.ignoreFollowingCheckbox, "When this condition matches an aircraft, conditions further down the list are not checked for it");
			this.ignoreFollowingCheckbox.UseVisualStyleBackColor = true;
			this.ignoreFollowingCheckbox.CheckedChanged += new System.EventHandler(this.editorValueChanged);
			//
			// alertTypeLabel
			//
			this.alertTypeLabel.AutoSize = true;
			this.alertTypeLabel.Location = new System.Drawing.Point(12, 48);
			this.alertTypeLabel.Name = "alertTypeLabel";
			this.alertTypeLabel.Size = new System.Drawing.Size(53, 15);
			this.alertTypeLabel.TabIndex = 3;
			this.alertTypeLabel.Text = "Alert on:";
			this.toolTip.SetToolTip(this.alertTypeLabel, "Disabled: never alert\r\nFirst contact: when the aircraft first matches\r\nLast contact: when a matched aircraft leaves the radar\r\nBoth: first and last contact");
			//
			// alertTypeComboBox
			//
			this.alertTypeComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.alertTypeComboBox.FormattingEnabled = true;
			this.alertTypeComboBox.Location = new System.Drawing.Point(120, 44);
			this.alertTypeComboBox.Name = "alertTypeComboBox";
			this.alertTypeComboBox.Size = new System.Drawing.Size(200, 23);
			this.alertTypeComboBox.TabIndex = 4;
			this.alertTypeComboBox.SelectedIndexChanged += new System.EventHandler(this.alertTypeComboBox_SelectedIndexChanged);
			//
			// alertWhenLabel
			//
			this.alertWhenLabel.AutoSize = true;
			this.alertWhenLabel.Location = new System.Drawing.Point(340, 48);
			this.alertWhenLabel.Name = "alertWhenLabel";
			this.alertWhenLabel.Size = new System.Drawing.Size(82, 15);
			this.alertWhenLabel.TabIndex = 5;
			this.alertWhenLabel.Text = "Match when:";
			//
			// triggerLogicPanel
			//
			this.triggerLogicPanel.Controls.Add(this.triggersLogicAnyRadioButton);
			this.triggerLogicPanel.Controls.Add(this.triggersLogicAllRadioButton);
			this.triggerLogicPanel.Location = new System.Drawing.Point(428, 40);
			this.triggerLogicPanel.Name = "triggerLogicPanel";
			this.triggerLogicPanel.Size = new System.Drawing.Size(260, 30);
			this.triggerLogicPanel.TabIndex = 6;
			//
			// triggersLogicAllRadioButton
			//
			this.triggersLogicAllRadioButton.AutoSize = true;
			this.triggersLogicAllRadioButton.Checked = true;
			this.triggersLogicAllRadioButton.Location = new System.Drawing.Point(3, 6);
			this.triggersLogicAllRadioButton.Name = "triggersLogicAllRadioButton";
			this.triggersLogicAllRadioButton.Size = new System.Drawing.Size(116, 19);
			this.triggersLogicAllRadioButton.TabIndex = 0;
			this.triggersLogicAllRadioButton.TabStop = true;
			this.triggersLogicAllRadioButton.Text = "all triggers match";
			this.triggersLogicAllRadioButton.UseVisualStyleBackColor = true;
			this.triggersLogicAllRadioButton.CheckedChanged += new System.EventHandler(this.editorValueChanged);
			//
			// triggersLogicAnyRadioButton
			//
			this.triggersLogicAnyRadioButton.AutoSize = true;
			this.triggersLogicAnyRadioButton.Location = new System.Drawing.Point(130, 6);
			this.triggersLogicAnyRadioButton.Name = "triggersLogicAnyRadioButton";
			this.triggersLogicAnyRadioButton.Size = new System.Drawing.Size(122, 19);
			this.triggersLogicAnyRadioButton.TabIndex = 1;
			this.triggersLogicAnyRadioButton.Text = "any trigger matches";
			this.triggersLogicAnyRadioButton.UseVisualStyleBackColor = true;
			//
			// triggersHeaderLabel
			//
			this.triggersHeaderLabel.AutoSize = true;
			this.triggersHeaderLabel.Location = new System.Drawing.Point(12, 84);
			this.triggersHeaderLabel.Name = "triggersHeaderLabel";
			this.triggersHeaderLabel.Size = new System.Drawing.Size(50, 15);
			this.triggersHeaderLabel.TabIndex = 7;
			this.triggersHeaderLabel.Text = "Triggers";
			//
			// triggerDataGridView
			//
			this.triggerDataGridView.AllowUserToResizeRows = false;
			this.triggerDataGridView.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
            | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
			this.triggerDataGridView.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
			this.triggerDataGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
			this.triggerDataGridView.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.propertyColumn,
            this.comparisonTypeColumn,
            this.valueColumn,
            this.typeColumn,
            this.deleteColumn});
			this.triggerDataGridView.Location = new System.Drawing.Point(12, 106);
			this.triggerDataGridView.MultiSelect = false;
			this.triggerDataGridView.Name = "triggerDataGridView";
			this.triggerDataGridView.RowHeadersVisible = false;
			this.triggerDataGridView.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.CellSelect;
			this.triggerDataGridView.Size = new System.Drawing.Size(428, 394);
			this.triggerDataGridView.TabIndex = 8;
			this.triggerDataGridView.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.triggerDataGridView_CellContentClick);
			this.triggerDataGridView.CellMouseClick += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.triggerDataGridView_CellMouseClick);
			this.triggerDataGridView.CellValueChanged += new System.Windows.Forms.DataGridViewCellEventHandler(this.triggerDataGridView_CellValueChanged);
			this.triggerDataGridView.CurrentCellDirtyStateChanged += new System.EventHandler(this.triggerDataGridView_CurrentCellDirtyStateChanged);
			this.triggerDataGridView.DataError += new System.Windows.Forms.DataGridViewDataErrorEventHandler(this.triggerDataGridView_DataError);
			this.triggerDataGridView.RowsRemoved += new System.Windows.Forms.DataGridViewRowsRemovedEventHandler(this.triggerDataGridView_RowsRemoved);
			this.triggerDataGridView.UserAddedRow += new System.Windows.Forms.DataGridViewRowEventHandler(this.TriggerDataGridViewUserAddedRow);
			//
			// propertyColumn
			//
			this.propertyColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
			this.propertyColumn.HeaderText = "Property";
			this.propertyColumn.Name = "propertyColumn";
			this.propertyColumn.Resizable = System.Windows.Forms.DataGridViewTriState.False;
			this.propertyColumn.Width = 165;
			//
			// comparisonTypeColumn
			//
			this.comparisonTypeColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
			this.comparisonTypeColumn.HeaderText = "Comparison";
			this.comparisonTypeColumn.Name = "comparisonTypeColumn";
			this.comparisonTypeColumn.Resizable = System.Windows.Forms.DataGridViewTriState.False;
			this.comparisonTypeColumn.Width = 100;
			//
			// valueColumn
			//
			this.valueColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
			this.valueColumn.HeaderText = "Value";
			this.valueColumn.Name = "valueColumn";
			this.valueColumn.Resizable = System.Windows.Forms.DataGridViewTriState.False;
			this.valueColumn.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
			//
			// typeColumn
			//
			this.typeColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
			this.typeColumn.HeaderText = "Type";
			this.typeColumn.Name = "typeColumn";
			this.typeColumn.ReadOnly = true;
			this.typeColumn.Resizable = System.Windows.Forms.DataGridViewTriState.False;
			this.typeColumn.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
			this.typeColumn.Width = 60;
			//
			// deleteColumn
			//
			this.deleteColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
			this.deleteColumn.HeaderText = "";
			this.deleteColumn.Name = "deleteColumn";
			this.deleteColumn.Text = "✕";
			this.deleteColumn.UseColumnTextForButtonValue = true;
			this.deleteColumn.Width = 26;
			//
			// emailGroupBox
			//
			this.emailGroupBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
            | System.Windows.Forms.AnchorStyles.Right)));
			this.emailGroupBox.Controls.Add(this.receiverEmailTextBox);
			this.emailGroupBox.Controls.Add(this.emailToSendToLabel);
			this.emailGroupBox.Controls.Add(this.lastPreviewLabel);
			this.emailGroupBox.Controls.Add(this.insertLastButton);
			this.emailGroupBox.Controls.Add(this.emailLastFormatTextBox);
			this.emailGroupBox.Controls.Add(this.lastSubjectLabel);
			this.emailGroupBox.Controls.Add(this.firstPreviewLabel);
			this.emailGroupBox.Controls.Add(this.insertFirstButton);
			this.emailGroupBox.Controls.Add(this.emailFirstFormatTextBox);
			this.emailGroupBox.Controls.Add(this.firstSubjectLabel);
			this.emailGroupBox.Controls.Add(this.emailCheckBox);
			this.emailGroupBox.Location = new System.Drawing.Point(460, 84);
			this.emailGroupBox.Name = "emailGroupBox";
			this.emailGroupBox.Size = new System.Drawing.Size(308, 416);
			this.emailGroupBox.TabIndex = 9;
			this.emailGroupBox.TabStop = false;
			this.emailGroupBox.Text = "Email Alerts";
			//
			// emailCheckBox
			//
			this.emailCheckBox.AutoSize = true;
			this.emailCheckBox.Location = new System.Drawing.Point(12, 24);
			this.emailCheckBox.Name = "emailCheckBox";
			this.emailCheckBox.Size = new System.Drawing.Size(144, 19);
			this.emailCheckBox.TabIndex = 0;
			this.emailCheckBox.Text = "Send email on alert";
			this.emailCheckBox.UseVisualStyleBackColor = true;
			this.emailCheckBox.CheckedChanged += new System.EventHandler(this.emailCheckBox_CheckedChanged);
			//
			// firstSubjectLabel
			//
			this.firstSubjectLabel.AutoSize = true;
			this.firstSubjectLabel.Location = new System.Drawing.Point(12, 52);
			this.firstSubjectLabel.Name = "firstSubjectLabel";
			this.firstSubjectLabel.Size = new System.Drawing.Size(123, 15);
			this.firstSubjectLabel.TabIndex = 1;
			this.firstSubjectLabel.Text = "First contact subject:";
			//
			// emailFirstFormatTextBox
			//
			this.emailFirstFormatTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
			this.emailFirstFormatTextBox.Location = new System.Drawing.Point(12, 70);
			this.emailFirstFormatTextBox.Name = "emailFirstFormatTextBox";
			this.emailFirstFormatTextBox.Size = new System.Drawing.Size(216, 23);
			this.emailFirstFormatTextBox.TabIndex = 2;
			this.toolTip.SetToolTip(this.emailFirstFormatTextBox, resources.GetString("emailFirstFormatTextBox.ToolTip"));
			this.emailFirstFormatTextBox.TextChanged += new System.EventHandler(this.subjectFormat_TextChanged);
			//
			// insertFirstButton
			//
			this.insertFirstButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.insertFirstButton.Location = new System.Drawing.Point(234, 69);
			this.insertFirstButton.Name = "insertFirstButton";
			this.insertFirstButton.Size = new System.Drawing.Size(62, 25);
			this.insertFirstButton.TabIndex = 3;
			this.insertFirstButton.Text = "Insert ▾";
			this.toolTip.SetToolTip(this.insertFirstButton, "Insert an aircraft property placeholder");
			this.insertFirstButton.UseVisualStyleBackColor = true;
			this.insertFirstButton.Click += new System.EventHandler(this.insertFirstButton_Click);
			//
			// firstPreviewLabel
			//
			this.firstPreviewLabel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
			this.firstPreviewLabel.AutoEllipsis = true;
			this.firstPreviewLabel.Location = new System.Drawing.Point(12, 95);
			this.firstPreviewLabel.Name = "firstPreviewLabel";
			this.firstPreviewLabel.Size = new System.Drawing.Size(284, 17);
			this.firstPreviewLabel.TabIndex = 4;
			this.firstPreviewLabel.Text = "Preview";
			//
			// lastSubjectLabel
			//
			this.lastSubjectLabel.AutoSize = true;
			this.lastSubjectLabel.Location = new System.Drawing.Point(12, 118);
			this.lastSubjectLabel.Name = "lastSubjectLabel";
			this.lastSubjectLabel.Size = new System.Drawing.Size(122, 15);
			this.lastSubjectLabel.TabIndex = 5;
			this.lastSubjectLabel.Text = "Last contact subject:";
			//
			// emailLastFormatTextBox
			//
			this.emailLastFormatTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
			this.emailLastFormatTextBox.Location = new System.Drawing.Point(12, 136);
			this.emailLastFormatTextBox.Name = "emailLastFormatTextBox";
			this.emailLastFormatTextBox.Size = new System.Drawing.Size(216, 23);
			this.emailLastFormatTextBox.TabIndex = 6;
			this.toolTip.SetToolTip(this.emailLastFormatTextBox, resources.GetString("emailLastFormatTextBox.ToolTip"));
			this.emailLastFormatTextBox.TextChanged += new System.EventHandler(this.subjectFormat_TextChanged);
			//
			// insertLastButton
			//
			this.insertLastButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.insertLastButton.Location = new System.Drawing.Point(234, 135);
			this.insertLastButton.Name = "insertLastButton";
			this.insertLastButton.Size = new System.Drawing.Size(62, 25);
			this.insertLastButton.TabIndex = 7;
			this.insertLastButton.Text = "Insert ▾";
			this.toolTip.SetToolTip(this.insertLastButton, "Insert an aircraft property placeholder");
			this.insertLastButton.UseVisualStyleBackColor = true;
			this.insertLastButton.Click += new System.EventHandler(this.insertLastButton_Click);
			//
			// lastPreviewLabel
			//
			this.lastPreviewLabel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
			this.lastPreviewLabel.AutoEllipsis = true;
			this.lastPreviewLabel.Location = new System.Drawing.Point(12, 161);
			this.lastPreviewLabel.Name = "lastPreviewLabel";
			this.lastPreviewLabel.Size = new System.Drawing.Size(284, 17);
			this.lastPreviewLabel.TabIndex = 8;
			this.lastPreviewLabel.Text = "Preview";
			//
			// emailToSendToLabel
			//
			this.emailToSendToLabel.AutoSize = true;
			this.emailToSendToLabel.Location = new System.Drawing.Point(12, 186);
			this.emailToSendToLabel.Name = "emailToSendToLabel";
			this.emailToSendToLabel.Size = new System.Drawing.Size(160, 15);
			this.emailToSendToLabel.TabIndex = 9;
			this.emailToSendToLabel.Text = "Send to (one address per line):";
			//
			// receiverEmailTextBox
			//
			this.receiverEmailTextBox.AcceptsReturn = true;
			this.receiverEmailTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
            | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
			this.receiverEmailTextBox.Location = new System.Drawing.Point(12, 204);
			this.receiverEmailTextBox.Multiline = true;
			this.receiverEmailTextBox.Name = "receiverEmailTextBox";
			this.receiverEmailTextBox.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
			this.receiverEmailTextBox.Size = new System.Drawing.Size(284, 200);
			this.receiverEmailTextBox.TabIndex = 10;
			this.receiverEmailTextBox.TextChanged += new System.EventHandler(this.editorValueChanged);
			//
			// propertyInfoButton
			//
			this.propertyInfoButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.propertyInfoButton.Location = new System.Drawing.Point(12, 518);
			this.propertyInfoButton.Name = "propertyInfoButton";
			this.propertyInfoButton.Size = new System.Drawing.Size(140, 30);
			this.propertyInfoButton.TabIndex = 10;
			this.propertyInfoButton.Text = "Property Reference";
			this.propertyInfoButton.UseVisualStyleBackColor = true;
			this.propertyInfoButton.Click += new System.EventHandler(this.propertyInfoButton_Click);
			//
			// errorLabel
			//
			this.errorLabel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
			this.errorLabel.AutoEllipsis = true;
			this.errorLabel.Location = new System.Drawing.Point(160, 516);
			this.errorLabel.Name = "errorLabel";
			this.errorLabel.Size = new System.Drawing.Size(360, 34);
			this.errorLabel.TabIndex = 11;
			this.errorLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			//
			// cancelButton
			//
			this.cancelButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.cancelButton.Location = new System.Drawing.Point(530, 518);
			this.cancelButton.Name = "cancelButton";
			this.cancelButton.Size = new System.Drawing.Size(90, 30);
			this.cancelButton.TabIndex = 12;
			this.cancelButton.Text = "Cancel";
			this.cancelButton.UseVisualStyleBackColor = true;
			this.cancelButton.Click += new System.EventHandler(this.cancelButton_Click);
			//
			// saveButton
			//
			this.saveButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.saveButton.Location = new System.Drawing.Point(628, 518);
			this.saveButton.Name = "saveButton";
			this.saveButton.Size = new System.Drawing.Size(140, 30);
			this.saveButton.TabIndex = 13;
			this.saveButton.Text = "Save and Close";
			this.saveButton.UseVisualStyleBackColor = true;
			this.saveButton.Click += new System.EventHandler(this.saveButton_Click);
			//
			// toolTip
			//
			this.toolTip.AutomaticDelay = 0;
			this.toolTip.AutoPopDelay = 8000;
			this.toolTip.InitialDelay = 10;
			this.toolTip.ReshowDelay = 100;
			this.toolTip.UseAnimation = false;
			this.toolTip.UseFading = false;
			//
			// errorProvider
			//
			this.errorProvider.BlinkStyle = System.Windows.Forms.ErrorBlinkStyle.NeverBlink;
			this.errorProvider.ContainerControl = this;
			//
			// ConditionEditorForm
			//
			this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(1080, 560);
			this.Controls.Add(this.editorPanel);
			this.Controls.Add(this.leftPanel);
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.MinimumSize = new System.Drawing.Size(1000, 560);
			this.Name = "ConditionEditorForm";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			this.Text = "Condition Editor";
			this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.ConditionEditorForm_FormClosing);
			this.Load += new System.EventHandler(this.ConditionEditorForm_Load);
			this.leftPanel.ResumeLayout(false);
			this.listButtonsPanel.ResumeLayout(false);
			this.editorPanel.ResumeLayout(false);
			this.editorPanel.PerformLayout();
			this.triggerLogicPanel.ResumeLayout(false);
			this.triggerLogicPanel.PerformLayout();
			((System.ComponentModel.ISupportInitialize)(this.triggerDataGridView)).EndInit();
			this.emailGroupBox.ResumeLayout(false);
			this.emailGroupBox.PerformLayout();
			((System.ComponentModel.ISupportInitialize)(this.errorProvider)).EndInit();
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.Panel leftPanel;
		private System.Windows.Forms.ListBox conditionsListBox;
		private System.Windows.Forms.Panel listButtonsPanel;
		private System.Windows.Forms.Button addButton;
		private System.Windows.Forms.Button duplicateButton;
		private System.Windows.Forms.Button removeButton;
		private System.Windows.Forms.Button moveUpButton;
		private System.Windows.Forms.Button moveDownButton;
		private System.Windows.Forms.Label conditionsHeaderLabel;
		private System.Windows.Forms.Panel editorPanel;
		private System.Windows.Forms.Label conditionNameLabel;
		private System.Windows.Forms.TextBox conditionNameTextBox;
		private System.Windows.Forms.CheckBox ignoreFollowingCheckbox;
		private System.Windows.Forms.Label alertTypeLabel;
		private System.Windows.Forms.ComboBox alertTypeComboBox;
		private System.Windows.Forms.Label alertWhenLabel;
		private System.Windows.Forms.Panel triggerLogicPanel;
		private System.Windows.Forms.RadioButton triggersLogicAnyRadioButton;
		private System.Windows.Forms.RadioButton triggersLogicAllRadioButton;
		private System.Windows.Forms.Label triggersHeaderLabel;
		private System.Windows.Forms.DataGridView triggerDataGridView;
		private System.Windows.Forms.DataGridViewComboBoxColumn propertyColumn;
		private System.Windows.Forms.DataGridViewComboBoxColumn comparisonTypeColumn;
		private System.Windows.Forms.DataGridViewTextBoxColumn valueColumn;
		private System.Windows.Forms.DataGridViewTextBoxColumn typeColumn;
		private System.Windows.Forms.DataGridViewButtonColumn deleteColumn;
		private System.Windows.Forms.GroupBox emailGroupBox;
		private System.Windows.Forms.CheckBox emailCheckBox;
		private System.Windows.Forms.Label firstSubjectLabel;
		private System.Windows.Forms.TextBox emailFirstFormatTextBox;
		private System.Windows.Forms.Button insertFirstButton;
		private System.Windows.Forms.Label firstPreviewLabel;
		private System.Windows.Forms.Label lastSubjectLabel;
		private System.Windows.Forms.TextBox emailLastFormatTextBox;
		private System.Windows.Forms.Button insertLastButton;
		private System.Windows.Forms.Label lastPreviewLabel;
		private System.Windows.Forms.Label emailToSendToLabel;
		private System.Windows.Forms.TextBox receiverEmailTextBox;
		private System.Windows.Forms.Button propertyInfoButton;
		private System.Windows.Forms.Label errorLabel;
		private System.Windows.Forms.Button cancelButton;
		private System.Windows.Forms.Button saveButton;
		private System.Windows.Forms.ToolTip toolTip;
		private System.Windows.Forms.ErrorProvider errorProvider;
	}
}
