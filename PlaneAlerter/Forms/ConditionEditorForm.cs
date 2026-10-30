using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using Microsoft.Extensions.DependencyInjection;
using PlaneAlerter.Enums;
using PlaneAlerter.Models;
using PlaneAlerter.Services;

namespace PlaneAlerter.Forms {
	/// <summary>
	/// Form for editing conditions
	/// </summary>
	internal partial class ConditionEditorForm :Form {
		private readonly IConditionManagerService _conditionManagerService;

		/// <summary>
		/// Id of condition to update
		/// </summary>
		private int _conditionToUpdate;

		/// <summary>
		/// Is this updating the condition?
		/// </summary>
		private bool _isUpdating;

		/// <summary>
		/// Initialise form with no existing condition
		/// </summary>
		public ConditionEditorForm(IConditionManagerService conditionManagerService) {
			_conditionManagerService = conditionManagerService;

			//Initialise form elements
			InitializeComponent();
			Theme.Apply(this);
			Theme.MakePrimary(saveButton);

			//Add vrs properties to triggers table
			var rows = triggerDataGridView.Rows.Cast<DataGridViewRow>();
			foreach (var row in rows)
			{
				InitNewRow(row);
			}

			//Add alert types to combobox
			foreach (AlertType property in Enum.GetValues(typeof(AlertType)))
				alertTypeComboBox.Items.Add(property.ToString().Replace('_', ' '));

			//Set Defaults
			conditionNameTextBox.Text = "New Condition";
			emailCheckBox.Checked = true;
			emailFirstFormatTextBox.Text = "First Contact Alert! [conditionname]: [reg]";
			emailLastFormatTextBox.Text = "Last Contact Alert! [conditionname]: [reg]";
			alertTypeComboBox.SelectedIndex = 1;
			triggersLogicAnyRadioButton.Checked = false;
			triggersLogicAllRadioButton.Checked = true;
		}

		public void LoadCondition(int conditionId)
		{
			_isUpdating = true;
			_conditionToUpdate = conditionId;

			//Set form element values from condition info
			var c = _conditionManagerService.EditorConditions[_conditionToUpdate];
			conditionNameTextBox.Text = c.Name;
			ignoreFollowingCheckbox.Checked = c.IgnoreFollowing;
			triggersLogicAnyRadioButton.Checked = c.TriggersUseOrLogic;
			emailCheckBox.Checked = c.EmailEnabled;
			emailFirstFormatTextBox.Text = c.EmailFirstFormat;
			emailLastFormatTextBox.Text = c.EmailLastFormat;
			receiverEmailTextBox.Text = string.Join(Environment.NewLine, c.ReceiverEmails.ToArray());
			alertTypeComboBox.Text = c.AlertType.ToString().Replace('_', ' ');

			foreach (var trigger in c.Triggers.Values)
			{
				triggerDataGridView.Rows.Add();
				var newRow = triggerDataGridView.Rows[triggerDataGridView.Rows.Count - 2];

				InitNewRow(newRow);

				newRow.Cells[0].Value = trigger.Property.ToString().Replace('_', ' ');
				newRow.Cells[1].Value = trigger.ComparisonType;
				newRow.Cells[2].Value = trigger.Value;
			}
		}

		/// <summary>
		/// Trigger table cell value changed
		/// </summary>
		/// <param name="sender">Sender</param>
		/// <param name="e">Event Args</param>
		private void triggerDataGridView_CellValueChanged(object sender, DataGridViewCellEventArgs e)
		{
			if (triggerDataGridView.Rows.Count == 1)
				return;

			var selectedPropertyKey = triggerDataGridView.Rows[e.RowIndex].Cells[0].Value?.ToString();
			var selectedProperty = !string.IsNullOrEmpty(selectedPropertyKey)
				? (VrsProperty?)Enum.Parse(typeof(VrsProperty), selectedPropertyKey.Replace(' ', '_'))
				: null;

			var comparisonTypeComboBoxCell = (DataGridViewComboBoxCell)(triggerDataGridView.Rows[e.RowIndex].Cells[1]);
			var valueCell = triggerDataGridView.Rows[e.RowIndex].Cells[2];

			if (selectedProperty == null)
			{
				comparisonTypeComboBoxCell.Items.Clear();
				comparisonTypeComboBoxCell.Value = "";
				valueCell.Value = "";
				return;
			}

			var selectedPropertyData = VrsProperties.VrsPropertyData[selectedProperty.Value];

			//Value changed
			if (e.ColumnIndex == 2)
			{
				//Clear value if property is number and value is not a number
				if (selectedPropertyData[0] == "Number" &&
				    !string.IsNullOrEmpty(valueCell.Value?.ToString()) &&
				    !double.TryParse(valueCell.Value.ToString(), out _))
				{
					valueCell.Value = "";
				}
			}

			//Property changed
			if (e.ColumnIndex == 0) {
				//Clear combobox
				comparisonTypeComboBoxCell.Items.Clear();
				comparisonTypeComboBoxCell.Value = "";
				comparisonTypeComboBoxCell.ReadOnly = false;

				//Clear value
				valueCell.Value = "";
				valueCell.ReadOnly = false;

				//Get comparison types supported by property
				var supportedComparisonTypes = selectedPropertyData[1];

				//Add comparison types to combobox from supported comparison types
				if (supportedComparisonTypes.Contains("A")) {
					foreach (var comparisonType in VrsProperties.ComparisonTypes["A"])
						comparisonTypeComboBoxCell.Items.Add(comparisonType);
				}

				if (supportedComparisonTypes.Contains("B")) {
					foreach (var comparisonType in VrsProperties.ComparisonTypes["B"])
						comparisonTypeComboBoxCell.Items.Add(comparisonType);
				}

				if (supportedComparisonTypes.Contains("C")) {
					foreach (var comparisonType in VrsProperties.ComparisonTypes["C"])
						comparisonTypeComboBoxCell.Items.Add(comparisonType);

					valueCell.Value = "True";
					valueCell.ReadOnly = true;
				}

				if (supportedComparisonTypes.Contains("D")) {
					foreach (var comparisonType in VrsProperties.ComparisonTypes["D"])
						comparisonTypeComboBoxCell.Items.Add(comparisonType);
				}

				if (supportedComparisonTypes.Contains("E")) {
					foreach (var comparisonType in VrsProperties.ComparisonTypes["E"])
						comparisonTypeComboBoxCell.Items.Add(comparisonType);
				}

				comparisonTypeComboBoxCell.Value = comparisonTypeComboBoxCell.Items[0].ToString();
			}
		}

		/// <summary>
		/// Trigger table row added
		/// </summary>
		/// <param name="sender">Sender</param>
		/// <param name="e">Event Args</param>
		private void TriggerDataGridViewUserAddedRow(object sender, DataGridViewRowEventArgs e)
		{
			InitNewRow(e.Row);
		}

		private void InitNewRow(DataGridViewRow row)
		{
			var propertyComboBoxCell = (DataGridViewComboBoxCell)(row.Cells[0]);
			var comparisonComboBoxCell = (DataGridViewComboBoxCell)(row.Cells[1]);
			var valueCell = row.Cells[2];

			//Add vrs properties to combobox
			propertyComboBoxCell.Items.Clear();
			foreach (VrsProperty property in Enum.GetValues(typeof(VrsProperty)))
				propertyComboBoxCell.Items.Add(property.ToString().Replace('_', ' '));

			comparisonComboBoxCell.ReadOnly = true;
			valueCell.ReadOnly = true;
		}

		/// <summary>
		/// Save button click
		/// </summary>
		/// <param name="sender">Sender</param>
		/// <param name="e">Event Args</param>
		private void SaveButtonClick(object sender, EventArgs e) {
			var cancelSave = false;
			var validationErrors = new List<string>();

			//Check if values are empty/invalid
			if (conditionNameTextBox.Text == "") {
				conditionNameLabel.ForeColor = Color.Red;
				conditionNameLabel.Text = "Condition Name: *";
				validationErrors.Add("Condition name is required");
				cancelSave = true;
			}
			else {
				conditionNameLabel.ForeColor = Theme.Text;
				conditionNameLabel.Text = "Condition Name:";
			}

			if (emailCheckBox.Checked) {
				if (string.IsNullOrWhiteSpace(emailFirstFormatTextBox.Text)) {
					emailFirstFormatTextBox.ForeColor = Color.Red;
					validationErrors.Add("First contact email subject is required");
					cancelSave = true;
				}
				else {
					emailFirstFormatTextBox.ForeColor = Theme.Text;
				}

				if (string.IsNullOrWhiteSpace(emailLastFormatTextBox.Text)) {
					emailLastFormatTextBox.ForeColor = Color.Red;
					validationErrors.Add("Last contact email subject is required");
					cancelSave = true;
				}
				else {
					emailLastFormatTextBox.ForeColor = Theme.Text;
				}

				if (string.IsNullOrWhiteSpace(receiverEmailTextBox.Text)) {
					emailToSendToLabel.ForeColor = Color.Red;
					emailToSendToLabel.Text = "Emails to send to (One each line): *";
					validationErrors.Add("At least one receiver email is required");
					cancelSave = true;
				}
				else {
					emailToSendToLabel.ForeColor = Theme.Text;
					emailToSendToLabel.Text = "Emails to send to (One each line): ";
				}
			}

			if (string.IsNullOrWhiteSpace(alertTypeComboBox.Text)) {
				alertTypeLabel.ForeColor = Color.Red;
				alertTypeLabel.Text = "Alert Type: *";
				validationErrors.Add("Alert type is required");
				cancelSave = true;
			}
			else {
				alertTypeLabel.ForeColor = Theme.Text;
				alertTypeLabel.Text = "Alert Type:";
			}

			if (triggerDataGridView.Rows.Count == 1) {
				triggerDataGridView.BackgroundColor = Theme.ErrorBackground;
				validationErrors.Add("At least one trigger is required");
				cancelSave = true;
			}
			else {
				triggerDataGridView.BackgroundColor = Theme.Surface;
			}

			//Trim empty lines from email textbox
			receiverEmailTextBox.Text = receiverEmailTextBox.Text.TrimEnd('\r', '\n');

			//Check if emails are valid
			if (emailCheckBox.Checked) {
				foreach (var line in receiverEmailTextBox.Lines) {
					try {
						new System.Net.Mail.MailAddress(line);
					}
					catch (FormatException) {
						emailToSendToLabel.ForeColor = Color.Red;
						emailToSendToLabel.Text = "Emails to send to (One each line): *";
						validationErrors.Add("One or more receiver email addresses are invalid");
						cancelSave = true;
						break;
					}
				}
			}

			//Cancel if values are invalid
			if (cancelSave) {
				MessageBox.Show(string.Join(Environment.NewLine, validationErrors),
					"Validation Errors", MessageBoxButtons.OK, MessageBoxIcon.Warning);
				return;
			}

			//If condition is being updated, remove the old one
			if (_isUpdating)
				_conditionManagerService.EditorConditions.Remove(_conditionToUpdate);

			//Sort conditions
			var list = _conditionManagerService.EditorConditions.Keys.ToList();
			var sortedConditions = new SortedDictionary<int, Condition>();
			list.Sort();
			foreach(var key in list)
				sortedConditions.Add(key, _conditionManagerService.EditorConditions[key]);
			_conditionManagerService.EditorConditions = sortedConditions;

			//Create new condition
			var receiverEmails = receiverEmailTextBox.Text
				.Split(new[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries)
				.Select(x => x.Trim())
				.ToList();

			var newCondition = new Condition(
				conditionNameTextBox.Text,
				(AlertType)Enum.Parse(typeof(AlertType), alertTypeComboBox.Text.Replace(' ', '_')),
				ignoreFollowingCheckbox.Checked,
				triggersLogicAnyRadioButton.Checked,
				emailCheckBox.Checked,
				emailFirstFormatTextBox.Text,
				emailLastFormatTextBox.Text,
				receiverEmails);

			if (triggerDataGridView.Rows.Count != 0)
			{
				foreach (DataGridViewRow row in triggerDataGridView.Rows)
				{
					if (row.Index == triggerDataGridView.Rows.Count - 1)
						continue;

					var selectedPropertyKey = row.Cells[0].Value?.ToString();
					var selectedProperty = !string.IsNullOrEmpty(selectedPropertyKey)
						? (VrsProperty?)Enum.Parse(typeof(VrsProperty), selectedPropertyKey.Replace(' ', '_'))
						: null;

					var comparisonType = row.Cells[1].Value?.ToString();

					if (selectedProperty == null || string.IsNullOrEmpty(comparisonType))
						continue;

					newCondition.Triggers.Add(newCondition.Triggers.Count,
						new Trigger(
							selectedProperty.Value,
							row.Cells[2].Value?.ToString() ?? "",
							comparisonType));
				}
			}

			//Add condition to condition list
			_conditionManagerService.EditorConditions.Add(_isUpdating ?
				_conditionToUpdate :
				_conditionManagerService.EditorConditions.Count, newCondition);

			//Close form
			DialogResult = DialogResult.OK;
			Close();
		}

		/// <summary>
		/// Property info button click
		/// </summary>
		private void propertyInfoButton_Click(object sender, EventArgs e) {
			//Show property info form
			var propertyInfoForm = Program.ServiceProvider.GetRequiredService<PropertyInfoForm>();
			propertyInfoForm.Show();
		}

		/// <summary>
		/// Update controls when email checkbox is toggled
		/// </summary>
		private void emailCheckBox_CheckedChanged(object sender, EventArgs e) {
			UpdateUiState();
		}

		private void triggerDataGridView_CellMouseClick(object sender, DataGridViewCellMouseEventArgs e) {
			//Ignore if clicking column header
			if (e.RowIndex == -1)
				return;

			//Edit combobox on click
			var column = triggerDataGridView.Columns[e.ColumnIndex];
			if (column is DataGridViewComboBoxColumn) {
				var cell = (DataGridViewComboBoxCell)triggerDataGridView[e.ColumnIndex, e.RowIndex];
				triggerDataGridView.CurrentCell = cell;
				triggerDataGridView.BeginEdit(true);

				var editingControl = (DataGridViewComboBoxEditingControl)triggerDataGridView.EditingControl;
				if (editingControl != null)
					editingControl.DroppedDown = true;
			}
		}

		/// <summary>
		/// Update enabled state and styling of UI based on settings
		/// </summary>
		private void UpdateUiState() {
			var alertType = AlertType.First_and_Last_Contact;

			if (!string.IsNullOrEmpty(alertTypeComboBox.Text)) {
				alertType = (AlertType)Enum.Parse(typeof(AlertType), alertTypeComboBox.Text.Replace(' ', '_'));
			}

			emailFirstFormatTextBox.Enabled = emailCheckBox.Checked && (alertType != AlertType.Last_Contact);
			emailLastFormatTextBox.Enabled = emailCheckBox.Checked && (alertType != AlertType.First_Contact);
			receiverEmailTextBox.Enabled = emailCheckBox.Checked;
		}

		private void alertTypeComboBox_SelectedIndexChanged(object sender, EventArgs e) {
			UpdateUiState();
		}

		private void triggerDataGridView_CellContentClick(object sender, DataGridViewCellEventArgs e)
		{
			var dataGridView = (DataGridView)sender;

			//Delete row on delete button cell click
			if (dataGridView.Columns[e.ColumnIndex] is DataGridViewButtonColumn &&
				e.ColumnIndex == 3 &&
				e.RowIndex >= 0 &&
				!dataGridView.Rows[e.RowIndex].IsNewRow)
			{
				dataGridView.Rows.RemoveAt(e.RowIndex);
			}
		}
	}
}
