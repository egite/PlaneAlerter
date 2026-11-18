using System.Drawing;
using System.Windows.Forms;
using PlaneAlerter.Controls;

namespace PlaneAlerter.Forms
{
	/// <summary>
	/// Builds the condition editor's controls
	/// </summary>
	internal partial class ConditionEditorForm
	{
		//Left: condition list
		private ConditionListBox conditionList = null!;
		private FieldBox filterBox = null!;
		private Label countLabel = null!;
		private Button newButton = null!;

		//Right: editor
		private Panel editorHost = null!;
		private TableLayoutPanel content = null!;
		private Panel emptyPanel = null!;
		private FieldBox nameBox = null!;
		private ToggleSwitch enabledToggle = null!;
		private Button moreButton = null!;
		private Label nameError = null!;
		private Label matchLabel = null!;
		private SegmentedControl logicSegment = null!;
		private TableLayoutPanel rulesTable = null!;
		private Button addRuleButton = null!;
		private Label rulesError = null!;
		private SegmentedControl timingSegment = null!;
		private Label timingHint = null!;
		private ToggleSwitch emailToggle = null!;
		private TableLayoutPanel emailTable = null!;
		private ChipInput recipientsInput = null!;
		private Label recipientsError = null!;
		private Label firstSubjectLabel = null!;
		private TableLayoutPanel firstSubjectRow = null!;
		private FieldBox firstSubjectBox = null!;
		private Label firstPreview = null!;
		private Label lastSubjectLabel = null!;
		private TableLayoutPanel lastSubjectRow = null!;
		private FieldBox lastSubjectBox = null!;
		private Label lastPreview = null!;
		private Button advancedButton = null!;
		private Panel advancedPanel = null!;
		private CheckBox ignoreFollowingCheck = null!;

		//Bottom bar
		private Label statusLabel = null!;
		private LinkLabel undoLink = null!;
		private Button cancelButton = null!;
		private Button saveButton = null!;

		private ContextMenuStrip conditionMenu = null!;
		private ToolStripMenuItem duplicateMenuItem = null!;
		private ToolStripMenuItem moveUpMenuItem = null!;
		private ToolStripMenuItem moveDownMenuItem = null!;
		private ToolStripMenuItem deleteMenuItem = null!;

		private static int S(int px) => Ui.S(px);

		private void BuildLayout()
		{
			SuspendLayout();
			Font = new Font("Segoe UI", 9F);
			BackColor = Theme.Background;
			ForeColor = Theme.Text;
			ClientSize = new Size(S(1120), S(720));
			MinimumSize = new Size(S(900), S(560));

			BuildMenu();
			var bottom = BuildBottomBar();
			var left = BuildLeftPanel();
			BuildEditor();
			BuildEmptyPanel();

			//Fill first, then the docked edges outermost last
			Controls.Add(editorHost);
			Controls.Add(emptyPanel);
			Controls.Add(left);
			Controls.Add(bottom);
			ResumeLayout();
		}

		private void BuildMenu()
		{
			conditionMenu = new ContextMenuStrip { Renderer = Theme.Renderer };
			duplicateMenuItem = new ToolStripMenuItem("Duplicate", null, (_, _) => DuplicateCurrent()) { ShortcutKeyDisplayString = "Ctrl+D" };
			moveUpMenuItem = new ToolStripMenuItem("Move up", null, (_, _) => MoveCurrent(-1)) { ShortcutKeyDisplayString = "Alt+Up" };
			moveDownMenuItem = new ToolStripMenuItem("Move down", null, (_, _) => MoveCurrent(1)) { ShortcutKeyDisplayString = "Alt+Down" };
			deleteMenuItem = new ToolStripMenuItem("Delete", null, (_, _) => DeleteCurrent()) { ShortcutKeyDisplayString = "Del" };
			conditionMenu.Items.AddRange(new ToolStripItem[]
			{
				duplicateMenuItem, moveUpMenuItem, moveDownMenuItem, new ToolStripSeparator(), deleteMenuItem
			});
			conditionMenu.Opening += (_, _) => UpdateMenuState();
		}

		private Panel BuildBottomBar()
		{
			var bar = new Panel { Dock = DockStyle.Bottom, Height = S(56), BackColor = Theme.Surface };
			bar.Paint += (_, e) =>
			{
				using var pen = new Pen(Theme.Border);
				e.Graphics.DrawLine(pen, 0, 0, bar.Width, 0);
			};

			var layout = new TableLayoutPanel
			{
				Dock = DockStyle.Fill,
				ColumnCount = 4,
				RowCount = 1,
				Padding = new Padding(S(16), S(12), S(16), S(10)),
				BackColor = Theme.Surface
			};
			layout.ColumnStyles.Add(new ColumnStyle(SizeType.AutoSize));
			layout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100));
			layout.ColumnStyles.Add(new ColumnStyle(SizeType.AutoSize));
			layout.ColumnStyles.Add(new ColumnStyle(SizeType.AutoSize));

			var referenceLink = new LinkLabel
			{
				Text = "Property reference",
				AutoSize = true,
				Anchor = AnchorStyles.Left,
				LinkColor = Theme.Sky,
				ActiveLinkColor = Theme.Amber,
				VisitedLinkColor = Theme.Sky,
				LinkBehavior = LinkBehavior.HoverUnderline
			};
			referenceLink.LinkClicked += (_, _) => ShowPropertyReference();

			var status = new FlowLayoutPanel
			{
				AutoSize = true,
				WrapContents = false,
				Anchor = AnchorStyles.Left,
				Margin = new Padding(S(20), 0, 0, 0),
				BackColor = Theme.Surface
			};
			statusLabel = new Label { AutoSize = true, ForeColor = Theme.TextMuted, Margin = new Padding(0, S(2), 0, 0) };
			undoLink = new LinkLabel
			{
				Text = "Undo",
				AutoSize = true,
				Visible = false,
				LinkColor = Theme.Sky,
				ActiveLinkColor = Theme.Amber,
				LinkBehavior = LinkBehavior.HoverUnderline,
				Margin = new Padding(S(6), S(2), 0, 0)
			};
			undoLink.LinkClicked += (_, _) => UndoDelete();
			status.Controls.Add(statusLabel);
			status.Controls.Add(undoLink);

			cancelButton = new Button { Text = "Cancel", Size = new Size(S(96), S(32)), Margin = new Padding(S(8), 0, 0, 0) };
			Theme.MakeSecondary(cancelButton);
			cancelButton.Click += (_, _) => Close();

			saveButton = new Button { Text = "Save and close", Size = new Size(S(132), S(32)), Margin = new Padding(S(8), 0, 0, 0) };
			Theme.MakePrimary(saveButton);
			saveButton.Click += (_, _) => SaveAndClose();
			toolTip.SetToolTip(saveButton, "Save all conditions (Ctrl+S)");

			layout.Controls.Add(referenceLink, 0, 0);
			layout.Controls.Add(status, 1, 0);
			layout.Controls.Add(cancelButton, 2, 0);
			layout.Controls.Add(saveButton, 3, 0);
			bar.Controls.Add(layout);

			CancelButton = cancelButton;
			return bar;
		}

		private Panel BuildLeftPanel()
		{
			var panel = new Panel { Dock = DockStyle.Left, Width = S(280), BackColor = Theme.SurfaceAlt };
			panel.Paint += (_, e) =>
			{
				using var pen = new Pen(Theme.Border);
				e.Graphics.DrawLine(pen, panel.Width - 1, 0, panel.Width - 1, panel.Height);
			};

			//Header: title, count and New button
			var header = new TableLayoutPanel
			{
				Dock = DockStyle.Top,
				Height = S(52),
				ColumnCount = 3,
				RowCount = 1,
				Padding = new Padding(S(14), S(12), S(12), S(4)),
				BackColor = Theme.SurfaceAlt
			};
			header.ColumnStyles.Add(new ColumnStyle(SizeType.AutoSize));
			header.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100));
			header.ColumnStyles.Add(new ColumnStyle(SizeType.AutoSize));

			var title = new Label
			{
				Text = "Conditions",
				AutoSize = true,
				Anchor = AnchorStyles.Left,
				Font = new Font("Segoe UI Semibold", 11F),
				ForeColor = Theme.Navy
			};
			countLabel = new Label { AutoSize = true, Anchor = AnchorStyles.Left, ForeColor = Theme.TextMuted, Margin = new Padding(S(2), S(3), 0, 0) };
			newButton = new Button { Text = "+ New", Size = new Size(S(72), S(30)), Anchor = AnchorStyles.Right };
			Theme.MakePrimary(newButton);
			newButton.Click += (_, _) => AddCondition();
			toolTip.SetToolTip(newButton, "New condition (Ctrl+N)");
			header.Controls.Add(title, 0, 0);
			header.Controls.Add(countLabel, 1, 0);
			header.Controls.Add(newButton, 2, 0);

			var filterWrap = new Panel
			{
				Dock = DockStyle.Top,
				Height = S(42),
				Padding = new Padding(S(12), S(2), S(12), S(10)),
				BackColor = Theme.SurfaceAlt
			};
			filterBox = new FieldBox { Dock = DockStyle.Fill, Placeholder = "Filter conditions" };
			filterBox.Inner.TextChanged += (_, _) => RefreshList();
			filterBox.Inner.KeyDown += (_, e) =>
			{
				if (e.KeyCode == Keys.Down)
				{
					conditionList.Focus();
					e.Handled = true;
				}
			};
			filterWrap.Controls.Add(filterBox);

			var hint = new Label
			{
				Dock = DockStyle.Bottom,
				Height = S(40),
				Text = "Checked top to bottom. Drag to reorder.",
				ForeColor = Theme.TextMuted,
				BackColor = Theme.SurfaceAlt,
				Padding = new Padding(S(14), 0, S(10), 0),
				TextAlign = ContentAlignment.MiddleLeft
			};

			var listTop = new Panel { Dock = DockStyle.Top, Height = 1, BackColor = Theme.Border };

			conditionList = new ConditionListBox
			{
				Dock = DockStyle.Fill,
				AllowDrop = true,
				ContextMenuStrip = conditionMenu,
				Describe = DescribeForList
			};
			conditionList.SelectedIndexChanged += conditionList_SelectedIndexChanged;
			conditionList.MouseDown += conditionList_MouseDown;
			conditionList.MouseMove += conditionList_MouseMove;
			conditionList.DragOver += conditionList_DragOver;
			conditionList.DragDrop += conditionList_DragDrop;
			conditionList.DragLeave += (_, _) => conditionList.DropIndex = -1;
			conditionList.KeyDown += conditionList_KeyDown;

			panel.Controls.Add(conditionList);
			panel.Controls.Add(listTop);
			panel.Controls.Add(hint);
			panel.Controls.Add(filterWrap);
			panel.Controls.Add(header);
			return panel;
		}

		private Label SectionLabel(string text, int top = 22) => new()
		{
			Text = text,
			AutoSize = true,
			Font = new Font(Font, FontStyle.Bold),
			ForeColor = Theme.Navy,
			Margin = new Padding(0, S(top), 0, S(8))
		};

		private Label ErrorLabel() => new()
		{
			AutoSize = true,
			ForeColor = Theme.ErrorText,
			Visible = false,
			Margin = new Padding(0, S(3), 0, 0)
		};

		private static Label InlineLabel(string text, Font font, Color color) => new()
		{
			Text = text,
			AutoSize = true,
			Font = font,
			ForeColor = color,
			Margin = new Padding(0, S(7), S(8), 0)
		};

		private void BuildEditor()
		{
			editorHost = new Panel
			{
				Dock = DockStyle.Fill,
				AutoScroll = true,
				BackColor = Theme.Surface,
				Padding = new Padding(S(32), S(20), S(32), S(28))
			};
			editorHost.Resize += (_, _) =>
			{
				//Keep lines readable on wide windows
				var right = System.Math.Max(S(32), editorHost.ClientSize.Width - S(32) - S(860));
				if (editorHost.Padding.Right != right)
					editorHost.Padding = new Padding(S(32), S(20), right, S(28));
			};

			content = new TableLayoutPanel
			{
				Dock = DockStyle.Top,
				AutoSize = true,
				AutoSizeMode = AutoSizeMode.GrowAndShrink,
				ColumnCount = 1,
				BackColor = Theme.Surface,
				GrowStyle = TableLayoutPanelGrowStyle.AddRows
			};
			content.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100));

			//Name, enabled switch and menu
			var header = new TableLayoutPanel
			{
				AutoSize = true,
				ColumnCount = 3,
				RowCount = 1,
				Anchor = AnchorStyles.Left | AnchorStyles.Right,
				Margin = Padding.Empty,
				BackColor = Theme.Surface
			};
			header.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100));
			header.ColumnStyles.Add(new ColumnStyle(SizeType.AutoSize));
			header.ColumnStyles.Add(new ColumnStyle(SizeType.AutoSize));

			nameBox = new FieldBox
			{
				Underline = true,
				Font = new Font("Segoe UI Semibold", 15F),
				Height = S(42),
				Anchor = AnchorStyles.Left | AnchorStyles.Right,
				Placeholder = "Name this condition",
				Margin = new Padding(0, 0, S(16), 0)
			};
			nameBox.Inner.TextChanged += (_, _) => Edit(d => d.Name = nameBox.Text);
			toolTip.SetToolTip(nameBox.Inner, "Condition name, also available in subjects as [ConditionName]");

			enabledToggle = new ToggleSwitch { Text = "Enabled", Anchor = AnchorStyles.Right, Margin = new Padding(0, S(9), S(8), 0) };
			enabledToggle.CheckedChanged += (_, _) =>
			{
				Edit(d => d.Enabled = enabledToggle.Checked);
				enabledToggle.Text = enabledToggle.Checked ? "Enabled" : "Off";
			};
			toolTip.SetToolTip(enabledToggle, "Turn this condition off without losing its settings");

			moreButton = new Button
			{
				Text = "⋯",
				Size = new Size(S(34), S(30)),
				Anchor = AnchorStyles.Right,
				Margin = new Padding(0, S(6), 0, 0),
				Font = new Font("Segoe UI", 11F, FontStyle.Bold)
			};
			Theme.MakeSecondary(moreButton);
			moreButton.Click += (_, _) => conditionMenu.Show(moreButton, new Point(moreButton.Width - conditionMenu.PreferredSize.Width, moreButton.Height));
			toolTip.SetToolTip(moreButton, "Duplicate, move or delete");

			header.Controls.Add(nameBox, 0, 0);
			header.Controls.Add(enabledToggle, 1, 0);
			header.Controls.Add(moreButton, 2, 0);
			content.Controls.Add(header);

			nameError = ErrorLabel();
			content.Controls.Add(nameError);

			matchLabel = new Label { AutoSize = true, ForeColor = Theme.TextMuted, Margin = new Padding(0, S(8), 0, 0), Cursor = Cursors.Help };
			content.Controls.Add(matchLabel);

			//Rules
			var sentence = new FlowLayoutPanel
			{
				AutoSize = true,
				WrapContents = false,
				Margin = new Padding(0, S(18), 0, S(8)),
				BackColor = Theme.Surface
			};
			var bold = new Font(Font, FontStyle.Bold);
			logicSegment = new SegmentedControl { Options = new[] { "all", "any" }, Margin = new Padding(0, 0, S(8), 0) };
			logicSegment.SelectedIndexChanged += (_, _) => Edit(d => d.UseOrLogic = logicSegment.SelectedIndex == 1);
			toolTip.SetToolTip(logicSegment, "All: every rule must match. Any: one matching rule is enough.");
			sentence.Controls.Add(InlineLabel("Match aircraft where", bold, Theme.Navy));
			sentence.Controls.Add(logicSegment);
			sentence.Controls.Add(InlineLabel("of these rules are true", bold, Theme.Navy));
			content.Controls.Add(sentence);

			rulesTable = new TableLayoutPanel
			{
				AutoSize = true,
				AutoSizeMode = AutoSizeMode.GrowAndShrink,
				ColumnCount = 1,
				Anchor = AnchorStyles.Left | AnchorStyles.Right,
				Margin = Padding.Empty,
				BackColor = Theme.Surface,
				GrowStyle = TableLayoutPanelGrowStyle.AddRows
			};
			rulesTable.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100));
			content.Controls.Add(rulesTable);

			rulesError = ErrorLabel();
			content.Controls.Add(rulesError);

			addRuleButton = new Button
			{
				Text = "+ Add rule",
				AutoSize = true,
				FlatStyle = FlatStyle.Flat,
				ForeColor = Theme.Sky,
				BackColor = Theme.Surface,
				Cursor = Cursors.Hand,
				Margin = new Padding(0, S(2), 0, 0),
				Font = new Font(Font, FontStyle.Bold)
			};
			addRuleButton.FlatAppearance.BorderSize = 0;
			addRuleButton.FlatAppearance.MouseOverBackColor = Theme.SkyLight;
			addRuleButton.FlatAppearance.MouseDownBackColor = Theme.SkyLight;
			addRuleButton.Click += (_, _) => AddRule();
			content.Controls.Add(addRuleButton);

			//Timing
			content.Controls.Add(SectionLabel("Alert me on"));
			timingSegment = new SegmentedControl { Options = new[] { "First contact", "Last contact", "Both" }, Margin = Padding.Empty };
			timingSegment.SelectedIndexChanged += (_, _) =>
			{
				Edit(d => d.Timing = TimingFromIndex(timingSegment.SelectedIndex));
				UpdateVisibility();
			};
			timingHint = new Label { AutoSize = true, ForeColor = Theme.TextMuted, Margin = new Padding(0, S(5), 0, 0) };
			content.Controls.Add(timingSegment);
			content.Controls.Add(timingHint);

			//Email
			emailToggle = new ToggleSwitch
			{
				Text = "Send an email",
				Font = new Font(Font, FontStyle.Bold),
				ForeColor = Theme.Navy,
				Margin = new Padding(0, S(24), 0, S(8))
			};
			emailToggle.CheckedChanged += (_, _) =>
			{
				Edit(d => d.EmailEnabled = emailToggle.Checked);
				UpdateVisibility();
			};
			content.Controls.Add(emailToggle);

			emailTable = new TableLayoutPanel
			{
				AutoSize = true,
				AutoSizeMode = AutoSizeMode.GrowAndShrink,
				ColumnCount = 2,
				Anchor = AnchorStyles.Left | AnchorStyles.Right,
				Margin = Padding.Empty,
				BackColor = Theme.Surface,
				GrowStyle = TableLayoutPanelGrowStyle.AddRows
			};
			emailTable.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, S(150)));
			emailTable.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100));

			Label FieldLabel(string text) => new()
			{
				Text = text,
				AutoSize = true,
				ForeColor = Theme.Text,
				Anchor = AnchorStyles.Left | AnchorStyles.Top,
				Margin = new Padding(0, S(8), S(8), 0)
			};

			recipientsInput = new ChipInput
			{
				Anchor = AnchorStyles.Left | AnchorStyles.Right,
				Placeholder = "name@example.com",
				Validator = ConditionDraft.IsValidEmail,
				Margin = Padding.Empty
			};
			recipientsInput.ValuesChanged += (_, _) => Edit(d => d.Recipients = recipientsInput.Values);
			recipientsError = ErrorLabel();

			firstSubjectLabel = FieldLabel("First contact subject");
			firstSubjectBox = SubjectBox();
			firstSubjectBox.Inner.TextChanged += (_, _) => Edit(d => d.FirstSubject = firstSubjectBox.Text);
			firstSubjectRow = SubjectRow(firstSubjectBox);
			firstPreview = new Label { AutoSize = true, ForeColor = Theme.TextMuted, Margin = new Padding(0, S(4), 0, S(10)) };

			lastSubjectLabel = FieldLabel("Last contact subject");
			lastSubjectBox = SubjectBox();
			lastSubjectBox.Inner.TextChanged += (_, _) => Edit(d => d.LastSubject = lastSubjectBox.Text);
			lastSubjectRow = SubjectRow(lastSubjectBox);
			lastPreview = new Label { AutoSize = true, ForeColor = Theme.TextMuted, Margin = new Padding(0, S(4), 0, S(10)) };

			emailTable.Controls.Add(FieldLabel("Send to"), 0, 0);
			emailTable.Controls.Add(recipientsInput, 1, 0);
			emailTable.Controls.Add(recipientsError, 1, 1);
			var recipientsHint = new Label
			{
				AutoSize = true,
				ForeColor = Theme.TextMuted,
				Text = "Press Enter or comma after each address. Double-click one to edit it.",
				Margin = new Padding(0, S(4), 0, S(14))
			};
			emailTable.Controls.Add(recipientsHint, 1, 2);
			emailTable.Controls.Add(firstSubjectLabel, 0, 3);
			emailTable.Controls.Add(firstSubjectRow, 1, 3);
			emailTable.Controls.Add(firstPreview, 1, 4);
			emailTable.Controls.Add(lastSubjectLabel, 0, 5);
			emailTable.Controls.Add(lastSubjectRow, 1, 5);
			emailTable.Controls.Add(lastPreview, 1, 6);
			content.Controls.Add(emailTable);

			//More options
			advancedButton = new Button
			{
				Text = "▸  More options",
				AutoSize = true,
				FlatStyle = FlatStyle.Flat,
				ForeColor = Theme.TextMuted,
				BackColor = Theme.Surface,
				Cursor = Cursors.Hand,
				Margin = new Padding(0, S(16), 0, 0)
			};
			advancedButton.FlatAppearance.BorderSize = 0;
			advancedButton.FlatAppearance.MouseOverBackColor = Theme.SurfaceAlt;
			advancedButton.FlatAppearance.MouseDownBackColor = Theme.SurfaceAlt;
			advancedButton.Click += (_, _) => SetAdvancedOpen(!advancedPanel.Visible);
			content.Controls.Add(advancedButton);

			advancedPanel = new FlowLayoutPanel
			{
				AutoSize = true,
				FlowDirection = FlowDirection.TopDown,
				WrapContents = false,
				Visible = false,
				Margin = new Padding(S(18), S(6), 0, 0),
				BackColor = Theme.Surface
			};
			ignoreFollowingCheck = new CheckBox
			{
				Text = "Stop checking later conditions when this one matches",
				AutoSize = true,
				ForeColor = Theme.Text
			};
			ignoreFollowingCheck.CheckedChanged += (_, _) => Edit(d => d.IgnoreFollowing = ignoreFollowingCheck.Checked);
			var ignoreHint = new Label
			{
				AutoSize = true,
				ForeColor = Theme.TextMuted,
				MaximumSize = new Size(S(560), 0),
				Text = "Conditions are checked from the top of the list down. With this on, an aircraft that matches here isn't checked against the conditions below it.",
				Margin = new Padding(S(18), S(2), 0, 0)
			};
			advancedPanel.Controls.Add(ignoreFollowingCheck);
			advancedPanel.Controls.Add(ignoreHint);
			content.Controls.Add(advancedPanel);

			editorHost.Controls.Add(content);
		}

		private FieldBox SubjectBox()
		{
			var box = new FieldBox { Anchor = AnchorStyles.Left | AnchorStyles.Right, Margin = Padding.Empty, Placeholder = "Subject line" };
			box.Inner.KeyPress += (_, e) =>
			{
				if (e.KeyChar == '[')
					BeginInvoke(new System.Action(() => ShowPlaceholderPopup(box, box.Inner.SelectionStart - 1)));
			};
			toolTip.SetToolTip(box.Inner, "Type [ to insert an aircraft field");
			return box;
		}

		private TableLayoutPanel SubjectRow(FieldBox box)
		{
			var row = new TableLayoutPanel
			{
				AutoSize = true,
				ColumnCount = 2,
				RowCount = 1,
				Anchor = AnchorStyles.Left | AnchorStyles.Right,
				Margin = Padding.Empty,
				BackColor = Theme.Surface
			};
			row.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100));
			row.ColumnStyles.Add(new ColumnStyle(SizeType.AutoSize));

			var insert = new Button { Text = "Insert field ▾", Size = new Size(S(108), S(30)), Margin = new Padding(S(6), 0, 0, 0), TabStop = false };
			Theme.MakeSecondary(insert);
			insert.Click += (_, _) => ShowPlaceholderPopup(box, -1);

			row.Controls.Add(box, 0, 0);
			row.Controls.Add(insert, 1, 0);
			return row;
		}

		private void BuildEmptyPanel()
		{
			emptyPanel = new Panel { Dock = DockStyle.Fill, BackColor = Theme.Surface, Visible = false };
			var stack = new FlowLayoutPanel
			{
				AutoSize = true,
				FlowDirection = FlowDirection.TopDown,
				WrapContents = false,
				BackColor = Theme.Surface
			};
			stack.Controls.Add(new Label
			{
				Text = "Create your first condition",
				AutoSize = true,
				Font = new Font("Segoe UI Semibold", 15F),
				ForeColor = Theme.Navy
			});
			stack.Controls.Add(new Label
			{
				Text = "A condition describes the aircraft you want to hear about and who gets the email.",
				AutoSize = true,
				MaximumSize = new Size(S(420), 0),
				ForeColor = Theme.TextMuted,
				Margin = new Padding(S(2), S(6), 0, S(16))
			});
			var create = new Button { Text = "New condition", Size = new Size(S(140), S(34)) };
			Theme.MakePrimary(create);
			create.Click += (_, _) => AddCondition();
			stack.Controls.Add(create);
			emptyPanel.Controls.Add(stack);

			void Center()
			{
				stack.Location = new Point(System.Math.Max(S(24), (emptyPanel.Width - stack.Width) / 2),
					System.Math.Max(S(24), (emptyPanel.Height - stack.Height) / 2 - S(30)));
			}
			emptyPanel.Resize += (_, _) => Center();
			stack.SizeChanged += (_, _) => Center();
		}
	}
}
