using System;
using System.Collections.Generic;
using System.Windows.Forms;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using PlaneAlerter.Models;
using PlaneAlerter.Services;

namespace PlaneAlerter.Forms {
	/// <summary>
	/// Form for editing conditions
	/// </summary>
	internal partial class ConditionListForm :Form {
		private readonly IConditionManagerService _conditionManagerService;

		/// <summary>
		/// Snapshot of the conditions when the editor was opened, used to detect unsaved changes
		/// </summary>
		private readonly string _openedSnapshot;

		/// <summary>
		/// Set once the conditions have been written to disk
		/// </summary>
		private bool _saved;

		public ConditionListForm(IConditionManagerService conditionManagerService) {
			_conditionManagerService = conditionManagerService;

			//Initialise form elements
			InitializeComponent();
			Theme.Apply(this);
			Theme.MakePrimary(exitButton);
			
			//Load conditions
			_conditionManagerService.EditorConditions = new SortedDictionary<int, Condition>(_conditionManagerService.Conditions);
			_openedSnapshot = Snapshot();
			UpdateConditionList();
		}

		/// <summary>
		/// Serialise the conditions being edited so they can be compared for changes
		/// </summary>
		private string Snapshot() {
			return JsonConvert.SerializeObject(_conditionManagerService.EditorConditions);
		}

		/// <summary>
		/// Prompt to save if the window is closed with unsaved changes
		/// </summary>
		private void ConditionListForm_FormClosing(object sender, FormClosingEventArgs e) {
			if (_saved || Snapshot() == _openedSnapshot)
				return;

			var result = MessageBox.Show("You have unsaved changes to your conditions. Save them before closing?",
				"Unsaved Changes", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Warning);

			switch (result) {
				case DialogResult.Yes:
					_conditionManagerService.SaveEditorConditions();
					_saved = true;
					break;
				case DialogResult.Cancel:
					e.Cancel = true;
					break;
			}
		}

		/// <summary>
		/// Update condition list
		/// </summary>
		private void UpdateConditionList() {
			conditionEditorTreeView.Nodes.Clear();

			foreach (var conditionId in _conditionManagerService.EditorConditions.Keys) {
				var condition = _conditionManagerService.EditorConditions[conditionId];

				var conditionNode = conditionEditorTreeView.Nodes.Add(conditionId + ": " + condition.Name);
				conditionNode.Tag = conditionId;
				conditionNode.Nodes.Add("Id: " + conditionId);
				conditionNode.Nodes.Add("Alert Type: " + condition.AlertType.ToString().Replace("_", " "));
				conditionNode.Nodes.Add("Email Enabled: " + condition.EmailEnabled);
				conditionNode.Nodes.Add("Emails: " + string.Join(", ", condition.ReceiverEmails));

				var triggersNode = conditionNode.Nodes.Add("Condition Triggers");
				foreach (var trigger in condition.Triggers.Values)
					triggersNode.Nodes.Add(trigger.Property + " " + trigger.ComparisonType + " " + trigger.Value);
			}
			
			if (conditionEditorTreeView.Nodes.Count != 0)
				conditionEditorTreeView.SelectedNode = conditionEditorTreeView.Nodes[0];
			
			UpdateUiState();
		}

		/// <summary>
		/// Add a condition
		/// </summary>
		/// <param name="sender">Sender</param>
		/// <param name="e">Event Args</param>
		private void addConditionButton_Click(object sender, EventArgs e) {
			//Show editor dialog then update condition list
			var editor = Program.ServiceProvider.GetRequiredService<ConditionEditorForm>();
			editor.ShowDialog();
			UpdateConditionList();
		}
		
		/// <summary>
		/// Exit button click
		/// </summary>
		/// <param name="sender">Sender</param>
		/// <param name="e">Event Args</param>
		private void ExitButtonClick(object sender, EventArgs e) {
			_conditionManagerService.SaveEditorConditions();
			_saved = true;
			Close();
		}
		
		/// <summary>
		/// Remove condition button click
		/// </summary>
		/// <param name="sender">Sender</param>
		/// <param name="e">Event viewer</param>
		private void RemoveConditionButtonClick(object sender, EventArgs e) {
			//Cancel if selected node is invalid
			if (conditionEditorTreeView.SelectedNode == null || conditionEditorTreeView.SelectedNode.Tag == null || conditionEditorTreeView.SelectedNode.Tag.ToString() == "")
				return;
			//Remove condition from condition list
			_conditionManagerService.EditorConditions.Remove(Convert.ToInt32(conditionEditorTreeView.SelectedNode.Tag));
			//Sort conditions
			var sortedConditions = new SortedDictionary<int, Condition>();
			var id = 0;
			foreach (var c in _conditionManagerService.EditorConditions.Values) {
				sortedConditions.Add(id,c);
				id++;
			}
			_conditionManagerService.EditorConditions = sortedConditions;
			//Update condition list
			UpdateConditionList();
		}

		/// <summary>
		/// Move up button
		/// </summary>
		/// <param name="sender">Sender</param>
		/// <param name="e">Event Args</param>
		private void moveUpButton_Click(object sender, EventArgs e) {
			//Cancel if selected node is invalid
			if (conditionEditorTreeView.SelectedNode == null || conditionEditorTreeView.SelectedNode.Tag == null || conditionEditorTreeView.SelectedNode.Tag.ToString() == "")
				return;
			//Swap conditions then update condition list
			var conditionId = Convert.ToInt32(conditionEditorTreeView.SelectedNode.Tag);
			if (conditionId == 0) return;
			var c1 = _conditionManagerService.EditorConditions[conditionId];
			var c2 = _conditionManagerService.EditorConditions[conditionId - 1];
			_conditionManagerService.EditorConditions.Remove(conditionId - 1);
			_conditionManagerService.EditorConditions.Remove(conditionId);
			_conditionManagerService.EditorConditions.Add(conditionId - 1, c1);
			_conditionManagerService.EditorConditions.Add(conditionId, c2);
			UpdateConditionList();
		}

		/// <summary>
		/// Move down button
		/// </summary>
		/// <param name="sender">Sender</param>
		/// <param name="e">Event Args</param>
		private void moveDownButton_Click(object sender, EventArgs e) {
			//Cancel if selected node is invalid
			if (conditionEditorTreeView.SelectedNode == null || conditionEditorTreeView.SelectedNode.Tag == null || conditionEditorTreeView.SelectedNode.Tag.ToString() == "")
				return;
			//Swap conditions then update condition list
			var conditionId = Convert.ToInt32(conditionEditorTreeView.SelectedNode.Tag);
			if (conditionId == _conditionManagerService.EditorConditions.Count - 1) return;
			var c1 = _conditionManagerService.EditorConditions[conditionId];
			var c2 = _conditionManagerService.EditorConditions[conditionId + 1];
			_conditionManagerService.EditorConditions.Remove(conditionId + 1);
			_conditionManagerService.EditorConditions.Remove(conditionId);
			_conditionManagerService.EditorConditions.Add(conditionId + 1, c1);
			_conditionManagerService.EditorConditions.Add(conditionId, c2);
			UpdateConditionList();
		}

		/// <summary>
		/// Edit button click
		/// </summary>
		private void editButton_Click(object sender, EventArgs e) {
			var node = conditionEditorTreeView.SelectedNode;
			
			//Check if node is valid
			if (node?.Tag == null || node.Tag.ToString() == "")
				return;

			//Open editor, update list once closed
			var editor = Program.ServiceProvider.GetRequiredService<ConditionEditorForm>();
			editor.LoadCondition(Convert.ToInt32(conditionEditorTreeView.SelectedNode.Tag));
			editor.ShowDialog();
			UpdateConditionList();
		}

		/// <summary>
		/// Update buttons enabled state
		/// </summary>
		private void UpdateUiState() {
			//If node is valid, enable buttons
			var node = conditionEditorTreeView.SelectedNode;
			var conditionSelected = !string.IsNullOrEmpty(node?.Tag?.ToString());
			removeConditionButton.Enabled = conditionSelected;
			editButton.Enabled = conditionSelected;
			moveUpButton.Enabled = conditionSelected;
			moveDownButton.Enabled = conditionSelected;
		}

		private void conditionEditorTreeView_AfterSelect(object sender, TreeViewEventArgs e) {
			UpdateUiState();
		}
	}
}
