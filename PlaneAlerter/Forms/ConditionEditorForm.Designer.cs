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
		/// Form shell only. The editor's controls are built in code (ConditionEditorForm.Layout.cs)
		/// because most of them are custom drawn and created per condition.
		/// </summary>
		private void InitializeComponent() {
			this.components = new System.ComponentModel.Container();
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ConditionEditorForm));
			this.toolTip = new System.Windows.Forms.ToolTip(this.components);
			this.matchTimer = new System.Windows.Forms.Timer(this.components);
			this.radarRefreshTimer = new System.Windows.Forms.Timer(this.components);
			this.SuspendLayout();
			//
			// toolTip
			//
			this.toolTip.AutoPopDelay = 15000;
			this.toolTip.InitialDelay = 300;
			this.toolTip.ReshowDelay = 100;
			//
			// matchTimer
			//
			this.matchTimer.Interval = 400;
			this.matchTimer.Tick += new System.EventHandler(this.matchTimer_Tick);
			//
			// radarRefreshTimer
			//
			this.radarRefreshTimer.Interval = 5000;
			this.radarRefreshTimer.Tick += new System.EventHandler(this.radarRefreshTimer_Tick);
			//
			// ConditionEditorForm
			//
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.KeyPreview = true;
			this.Name = "ConditionEditorForm";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			this.Text = "Condition Editor";
			this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.ConditionEditorForm_FormClosing);
			this.Load += new System.EventHandler(this.ConditionEditorForm_Load);
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.ToolTip toolTip;
		private System.Windows.Forms.Timer matchTimer;
		private System.Windows.Forms.Timer radarRefreshTimer;
	}
}
