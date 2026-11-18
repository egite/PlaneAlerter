using System.Drawing;
using System.Windows.Forms;

namespace PlaneAlerter
{
	/// <summary>
	/// Colour palette and control styling shared by every form.
	/// Deep "flight deck" navy, a sky-blue accent for interaction, and an amber highlight.
	/// </summary>
	internal static class Theme
	{
		public static readonly Color Navy = ColorTranslator.FromHtml("#1B3A5C");
		public static readonly Color NavyDark = ColorTranslator.FromHtml("#12263D");
		public static readonly Color Sky = ColorTranslator.FromHtml("#2E78C2");
		public static readonly Color SkyLight = ColorTranslator.FromHtml("#DCE8F5");
		public static readonly Color Amber = ColorTranslator.FromHtml("#F2A71B");
		public static readonly Color Background = ColorTranslator.FromHtml("#EEF2F6");
		public static readonly Color Surface = Color.White;
		public static readonly Color SurfaceAlt = ColorTranslator.FromHtml("#F6F9FC");
		public static readonly Color Border = ColorTranslator.FromHtml("#C9D3DE");
		public static readonly Color Text = ColorTranslator.FromHtml("#1E2A36");
		public static readonly Color TextMuted = ColorTranslator.FromHtml("#5B6B7B");
		public static readonly Color TextOnNavy = Color.White;
		public static readonly Color TextDisabledOnNavy = ColorTranslator.FromHtml("#8FA3B8");
		public static readonly Color ErrorBackground = ColorTranslator.FromHtml("#FDE2E2");
		public static readonly Color ErrorText = ColorTranslator.FromHtml("#C0392B");
		public static readonly Color WarningText = ColorTranslator.FromHtml("#B9770E");
		public static readonly Color Success = ColorTranslator.FromHtml("#2E9E5B");

		//Console
		public static readonly Color ConsoleBackground = NavyDark;
		public static readonly Color ConsoleText = ColorTranslator.FromHtml("#D5DEE8");
		public static readonly Color LogError = ColorTranslator.FromHtml("#FF6B6B");
		public static readonly Color LogWarning = ColorTranslator.FromHtml("#FFB454");
		public static readonly Color LogInfo = ColorTranslator.FromHtml("#6CB4F7");
		public static readonly Color LogSuccess = ColorTranslator.FromHtml("#6EDC8C");
		public static readonly Color LogNotice = ColorTranslator.FromHtml("#F5E27A");

		private static readonly ToolStripRenderer MenuRenderer = new ThemeRenderer();

		/// <summary>
		/// Renderer for themed menus and dropdowns
		/// </summary>
		public static ToolStripRenderer Renderer => MenuRenderer;

		/// <summary>
		/// Apply the theme to a form and every control on it
		/// </summary>
		public static void Apply(Form form)
		{
			form.BackColor = Background;
			form.ForeColor = Text;
			ApplyToChildren(form);
		}

		/// <summary>
		/// Style a button as the primary action (filled navy)
		/// </summary>
		public static void MakePrimary(Button button)
		{
			StyleButton(button, true);
		}

		/// <summary>
		/// Style a button as a secondary action (white with a border)
		/// </summary>
		public static void MakeSecondary(Button button)
		{
			StyleButton(button, false);
		}

		/// <summary>
		/// Style a label as a section header (bold navy)
		/// </summary>
		public static void MakeHeader(Label label)
		{
			label.ForeColor = Navy;
			label.Font = new Font(label.Font, FontStyle.Bold);
		}

		/// <summary>
		/// Map the colours the services log with onto the console palette
		/// </summary>
		public static Color LogColor(Color? color)
		{
			if (color == null)
				return ConsoleText;

			var argb = color.Value.ToArgb();
			if (argb == Color.Red.ToArgb()) return LogError;
			if (argb == Color.Orange.ToArgb()) return LogWarning;
			if (argb == Color.LightBlue.ToArgb()) return LogInfo;
			if (argb == Color.LightGreen.ToArgb()) return LogSuccess;
			if (argb == Color.LightGoldenrodYellow.ToArgb()) return LogNotice;
			if (argb == Color.White.ToArgb()) return ConsoleText;

			return color.Value;
		}

		private static void ApplyToChildren(Control parent)
		{
			foreach (Control control in parent.Controls)
			{
				Style(control);
				ApplyToChildren(control);
			}
		}

		private static void Style(Control control)
		{
			switch (control)
			{
				case Button button:
					StyleButton(button, false);
					break;
				case LinkLabel link:
					link.LinkColor = Sky;
					link.ActiveLinkColor = Amber;
					link.VisitedLinkColor = Sky;
					link.LinkBehavior = LinkBehavior.HoverUnderline;
					break;
				case Label label:
					label.ForeColor = Text;
					break;
				case GroupBox groupBox:
					//Caption colour; child labels are set explicitly as they are visited
					groupBox.ForeColor = Navy;
					groupBox.BackColor = Background;
					break;
				case CheckBox checkBox:
					checkBox.ForeColor = Text;
					break;
				case RadioButton radioButton:
					radioButton.ForeColor = Text;
					break;
				case RichTextBox richTextBox:
					richTextBox.BackColor = ConsoleBackground;
					richTextBox.ForeColor = ConsoleText;
					richTextBox.BorderStyle = BorderStyle.None;
					break;
				case TextBoxBase textBox:
					textBox.BorderStyle = BorderStyle.FixedSingle;
					textBox.BackColor = Surface;
					textBox.ForeColor = Text;
					break;
				case NumericUpDown numeric:
					numeric.BorderStyle = BorderStyle.FixedSingle;
					numeric.BackColor = Surface;
					numeric.ForeColor = Text;
					break;
				case ComboBox comboBox:
					comboBox.BackColor = Surface;
					comboBox.ForeColor = Text;
					break;
				case TreeView treeView:
					treeView.BackColor = Surface;
					treeView.ForeColor = Text;
					treeView.BorderStyle = BorderStyle.FixedSingle;
					treeView.LineColor = Border;
					break;
				case DataGridView grid:
					StyleGrid(grid);
					break;
				case MenuStrip menuStrip:
					menuStrip.Renderer = MenuRenderer;
					menuStrip.BackColor = Navy;
					menuStrip.ForeColor = TextOnNavy;
					menuStrip.Padding = new Padding(8, 4, 0, 4);
					break;
				case StatusStrip statusStrip:
					statusStrip.Renderer = MenuRenderer;
					statusStrip.BackColor = Navy;
					statusStrip.ForeColor = TextOnNavy;
					statusStrip.SizingGrip = false;
					break;
				case SplitContainer splitContainer:
					//The container colour shows through as the splitter bar
					splitContainer.BackColor = Border;
					splitContainer.Panel1.BackColor = Background;
					splitContainer.Panel2.BackColor = Background;
					break;
				case Panel panel:
					panel.BackColor = Background;
					break;
			}
		}

		private static void StyleButton(Button button, bool primary)
		{
			button.FlatStyle = FlatStyle.Flat;
			button.FlatAppearance.BorderSize = 1;
			button.UseVisualStyleBackColor = false;
			button.Cursor = Cursors.Hand;

			if (primary)
			{
				button.BackColor = Navy;
				button.ForeColor = TextOnNavy;
				button.FlatAppearance.BorderColor = Navy;
				button.FlatAppearance.MouseOverBackColor = Sky;
				button.FlatAppearance.MouseDownBackColor = NavyDark;
			}
			else
			{
				button.BackColor = Surface;
				button.ForeColor = Navy;
				button.FlatAppearance.BorderColor = Border;
				button.FlatAppearance.MouseOverBackColor = SkyLight;
				button.FlatAppearance.MouseDownBackColor = Border;
			}
		}

		private static void StyleGrid(DataGridView grid)
		{
			grid.BackgroundColor = Surface;
			grid.BorderStyle = BorderStyle.None;
			grid.GridColor = Border;
			grid.EnableHeadersVisualStyles = false;
			grid.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.Single;
			grid.CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal;

			grid.ColumnHeadersDefaultCellStyle.BackColor = Navy;
			grid.ColumnHeadersDefaultCellStyle.ForeColor = TextOnNavy;
			grid.ColumnHeadersDefaultCellStyle.SelectionBackColor = Navy;
			grid.ColumnHeadersDefaultCellStyle.SelectionForeColor = TextOnNavy;
			grid.ColumnHeadersDefaultCellStyle.Font = new Font(grid.Font, FontStyle.Bold);
			grid.ColumnHeadersDefaultCellStyle.Padding = new Padding(4, 3, 4, 3);

			grid.DefaultCellStyle.Font = grid.Font;
			grid.DefaultCellStyle.BackColor = Surface;
			grid.DefaultCellStyle.ForeColor = Text;
			grid.DefaultCellStyle.SelectionBackColor = Sky;
			grid.DefaultCellStyle.SelectionForeColor = TextOnNavy;
			grid.DefaultCellStyle.Padding = new Padding(3, 0, 3, 0);

			grid.AlternatingRowsDefaultCellStyle.BackColor = SurfaceAlt;
		}

		/// <summary>
		/// Colour table for menu and status strips
		/// </summary>
		private sealed class ThemeColorTable : ProfessionalColorTable
		{
			public override Color MenuStripGradientBegin => Navy;
			public override Color MenuStripGradientEnd => Navy;
			public override Color StatusStripGradientBegin => Navy;
			public override Color StatusStripGradientEnd => Navy;
			public override Color ToolStripBorder => Navy;

			public override Color MenuItemSelected => Sky;
			public override Color MenuItemSelectedGradientBegin => Sky;
			public override Color MenuItemSelectedGradientEnd => Sky;
			public override Color MenuItemBorder => Sky;

			//Top-level item while its dropdown is open
			public override Color MenuItemPressedGradientBegin => Surface;
			public override Color MenuItemPressedGradientMiddle => Surface;
			public override Color MenuItemPressedGradientEnd => Surface;

			public override Color MenuBorder => Border;
			public override Color ToolStripDropDownBackground => Surface;
			public override Color ImageMarginGradientBegin => Surface;
			public override Color ImageMarginGradientMiddle => Surface;
			public override Color ImageMarginGradientEnd => Surface;
			public override Color SeparatorDark => Border;
			public override Color SeparatorLight => Surface;
		}

		/// <summary>
		/// Renderer that keeps menu text readable on the navy bar and white dropdowns
		/// </summary>
		private sealed class ThemeRenderer : ToolStripProfessionalRenderer
		{
			public ThemeRenderer() : base(new ThemeColorTable())
			{
				RoundedEdges = false;
			}

			protected override void OnRenderItemText(ToolStripItemTextRenderEventArgs e)
			{
				var onBar = !e.Item.IsOnDropDown;

				if (!e.Item.Enabled)
					e.TextColor = onBar ? TextDisabledOnNavy : TextMuted;
				else if (onBar)
					e.TextColor = e.Item.Pressed ? Text : TextOnNavy;
				else
					e.TextColor = e.Item.Selected ? TextOnNavy : Text;

				base.OnRenderItemText(e);
			}

			protected override void OnRenderToolStripBorder(ToolStripRenderEventArgs e)
			{
				//No bevel line under the menu bar or above the status bar
				if (e.ToolStrip is MenuStrip || e.ToolStrip is StatusStrip)
					return;

				base.OnRenderToolStripBorder(e);
			}
		}
	}
}
