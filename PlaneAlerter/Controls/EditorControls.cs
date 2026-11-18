using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Windows.Forms;

namespace PlaneAlerter.Controls
{
	/// <summary>
	/// DPI and drawing helpers for the hand-drawn controls
	/// </summary>
	internal static class Ui
	{
		private static float? _factor;

		/// <summary>
		/// Scale a 96-dpi pixel length to the system DPI (the app is system DPI aware)
		/// </summary>
		public static int S(int pixels)
		{
			if (_factor == null)
			{
				using var g = Graphics.FromHwnd(IntPtr.Zero);
				_factor = g.DpiX / 96f;
			}
			return (int)Math.Round(pixels * _factor.Value);
		}

		public static GraphicsPath Rounded(Rectangle r, int radius)
		{
			var path = new GraphicsPath();
			if (radius <= 0)
			{
				path.AddRectangle(r);
				return path;
			}

			var d = Math.Min(radius * 2, Math.Min(r.Width, r.Height));
			path.AddArc(r.X, r.Y, d, d, 180, 90);
			path.AddArc(r.Right - d, r.Y, d, d, 270, 90);
			path.AddArc(r.Right - d, r.Bottom - d, d, d, 0, 90);
			path.AddArc(r.X, r.Bottom - d, d, d, 90, 90);
			path.CloseFigure();
			return path;
		}

		public static void DrawChevron(Graphics g, Rectangle area, Color color)
		{
			var cx = area.X + area.Width / 2;
			var cy = area.Y + area.Height / 2;
			var w = S(4);
			using var pen = new Pen(color, Math.Max(1.5f, S(2) * 0.75f));
			var old = g.SmoothingMode;
			g.SmoothingMode = SmoothingMode.AntiAlias;
			g.DrawLines(pen, new[] { new Point(cx - w, cy - w / 2), new Point(cx, cy + w / 2), new Point(cx + w, cy - w / 2) });
			g.SmoothingMode = old;
		}
	}

	/// <summary>
	/// On/off switch with a label to its right
	/// </summary>
	internal sealed class ToggleSwitch : Control
	{
		private bool _checked;
		private bool _hover;

		public event EventHandler? CheckedChanged;

		public ToggleSwitch()
		{
			SetStyle(ControlStyles.UserPaint | ControlStyles.AllPaintingInWmPaint | ControlStyles.OptimizedDoubleBuffer |
			         ControlStyles.ResizeRedraw | ControlStyles.Selectable | ControlStyles.StandardClick, true);
			Cursor = Cursors.Hand;
			TabStop = true;
			BackColor = Theme.Surface;
			ForeColor = Theme.Text;
			UpdateSize();
		}

		public bool Checked
		{
			get => _checked;
			set
			{
				if (_checked == value)
					return;
				_checked = value;
				Invalidate();
				CheckedChanged?.Invoke(this, EventArgs.Empty);
			}
		}

		private int TrackWidth => Ui.S(34);
		private int TrackHeight => Ui.S(18);

		private void UpdateSize()
		{
			var textWidth = string.IsNullOrEmpty(Text) ? 0 : TextRenderer.MeasureText(Text, Font).Width + Ui.S(6);
			Size = new Size(TrackWidth + Ui.S(4) + textWidth, Math.Max(Ui.S(24), Font.Height + Ui.S(6)));
		}

		protected override void OnTextChanged(EventArgs e)
		{
			base.OnTextChanged(e);
			UpdateSize();
			Invalidate();
		}

		protected override void OnFontChanged(EventArgs e)
		{
			base.OnFontChanged(e);
			UpdateSize();
		}

		protected override void OnClick(EventArgs e)
		{
			Focus();
			Checked = !Checked;
			base.OnClick(e);
		}

		protected override void OnKeyDown(KeyEventArgs e)
		{
			if (e.KeyCode == Keys.Space)
			{
				Checked = !Checked;
				e.Handled = true;
			}
			base.OnKeyDown(e);
		}

		protected override void OnMouseEnter(EventArgs e) { _hover = true; Invalidate(); base.OnMouseEnter(e); }
		protected override void OnMouseLeave(EventArgs e) { _hover = false; Invalidate(); base.OnMouseLeave(e); }
		protected override void OnGotFocus(EventArgs e) { Invalidate(); base.OnGotFocus(e); }
		protected override void OnLostFocus(EventArgs e) { Invalidate(); base.OnLostFocus(e); }

		protected override void OnPaint(PaintEventArgs e)
		{
			var g = e.Graphics;
			g.Clear(BackColor);
			g.SmoothingMode = SmoothingMode.AntiAlias;

			var track = new Rectangle(1, (Height - TrackHeight) / 2, TrackWidth - 2, TrackHeight);
			var fill = _checked ? (_hover ? Theme.Navy : Theme.Sky) : (_hover ? Theme.TextMuted : Theme.Border);
			using (var path = Ui.Rounded(track, TrackHeight / 2))
			using (var brush = new SolidBrush(fill))
				g.FillPath(brush, path);

			if (Focused)
			{
				var ring = Rectangle.Inflate(track, 1, 1);
				using var path = Ui.Rounded(ring, ring.Height / 2);
				using var pen = new Pen(Theme.Navy, 1);
				g.DrawPath(pen, path);
			}

			var knobSize = TrackHeight - Ui.S(6);
			var knobX = _checked ? track.Right - knobSize - Ui.S(3) : track.X + Ui.S(3);
			g.FillEllipse(Brushes.White, knobX, track.Y + Ui.S(3), knobSize, knobSize);

			if (!string.IsNullOrEmpty(Text))
			{
				var textRect = new Rectangle(TrackWidth + Ui.S(6), 0, Width - TrackWidth - Ui.S(6), Height);
				TextRenderer.DrawText(g, Text, Font, textRect, Enabled ? ForeColor : Theme.TextMuted,
					TextFormatFlags.VerticalCenter | TextFormatFlags.Left | TextFormatFlags.SingleLine);
			}
		}
	}

	/// <summary>
	/// A row of mutually exclusive buttons
	/// </summary>
	internal sealed class SegmentedControl : Control
	{
		private string[] _options = Array.Empty<string>();
		private int _selected = -1;
		private int _hover = -1;

		public event EventHandler? SelectedIndexChanged;

		public SegmentedControl()
		{
			SetStyle(ControlStyles.UserPaint | ControlStyles.AllPaintingInWmPaint | ControlStyles.OptimizedDoubleBuffer |
			         ControlStyles.ResizeRedraw | ControlStyles.Selectable, true);
			Cursor = Cursors.Hand;
			TabStop = true;
			BackColor = Theme.Surface;
		}

		public string[] Options
		{
			get => _options;
			set
			{
				_options = value;
				UpdateSize();
				Invalidate();
			}
		}

		/// <summary>
		/// Fixed minimum width per segment, 0 to size to the longest option
		/// </summary>
		public int MinSegmentWidth { get; set; }

		public int SelectedIndex
		{
			get => _selected;
			set
			{
				if (_selected == value)
					return;
				_selected = value;
				Invalidate();
				SelectedIndexChanged?.Invoke(this, EventArgs.Empty);
			}
		}

		private int SegmentWidth
		{
			get
			{
				var widest = _options.Length == 0 ? 0 : _options.Max(o => TextRenderer.MeasureText(o, Font).Width);
				return Math.Max(MinSegmentWidth, widest + Ui.S(24));
			}
		}

		private void UpdateSize()
		{
			Size = new Size(Math.Max(1, SegmentWidth * _options.Length), Ui.S(30));
		}

		protected override void OnFontChanged(EventArgs e)
		{
			base.OnFontChanged(e);
			UpdateSize();
		}

		private int IndexAt(Point p)
		{
			if (_options.Length == 0)
				return -1;
			var index = p.X / Math.Max(1, Width / _options.Length);
			return Math.Clamp(index, 0, _options.Length - 1);
		}

		protected override void OnMouseMove(MouseEventArgs e)
		{
			var index = IndexAt(e.Location);
			if (index != _hover)
			{
				_hover = index;
				Invalidate();
			}
			base.OnMouseMove(e);
		}

		protected override void OnMouseLeave(EventArgs e)
		{
			_hover = -1;
			Invalidate();
			base.OnMouseLeave(e);
		}

		protected override void OnMouseDown(MouseEventArgs e)
		{
			if (e.Button == MouseButtons.Left)
			{
				Focus();
				SelectedIndex = IndexAt(e.Location);
			}
			base.OnMouseDown(e);
		}

		protected override bool IsInputKey(Keys keyData) =>
			keyData is Keys.Left or Keys.Right || base.IsInputKey(keyData);

		protected override void OnKeyDown(KeyEventArgs e)
		{
			if (e.KeyCode == Keys.Left && _selected > 0)
				SelectedIndex = _selected - 1;
			else if (e.KeyCode == Keys.Right && _selected < _options.Length - 1)
				SelectedIndex = _selected + 1;
			base.OnKeyDown(e);
		}

		protected override void OnGotFocus(EventArgs e) { Invalidate(); base.OnGotFocus(e); }
		protected override void OnLostFocus(EventArgs e) { Invalidate(); base.OnLostFocus(e); }

		protected override void OnPaint(PaintEventArgs e)
		{
			var g = e.Graphics;
			g.Clear(Parent?.BackColor ?? BackColor);
			g.SmoothingMode = SmoothingMode.AntiAlias;
			if (_options.Length == 0)
				return;

			var outer = new Rectangle(0, 0, Width - 1, Height - 1);
			var segW = Width / _options.Length;
			using var outerPath = Ui.Rounded(outer, Ui.S(5));
			using (var bg = new SolidBrush(Theme.Surface))
				g.FillPath(bg, outerPath);

			g.SetClip(outerPath);
			for (var i = 0; i < _options.Length; i++)
			{
				var rect = new Rectangle(i * segW, 0, i == _options.Length - 1 ? Width - i * segW : segW, Height);
				var selected = i == _selected;
				if (selected)
				{
					using var brush = new SolidBrush(Enabled ? Theme.SkyLight : Theme.SurfaceAlt);
					g.FillRectangle(brush, rect);
				}
				else if (i == _hover && Enabled)
				{
					using var brush = new SolidBrush(Theme.SurfaceAlt);
					g.FillRectangle(brush, rect);
				}

				if (i > 0)
				{
					using var pen = new Pen(Theme.Border);
					g.DrawLine(pen, rect.X, 0, rect.X, Height);
				}

				var color = !Enabled ? Theme.TextMuted : selected ? Theme.Navy : Theme.Text;
				using var font = selected ? new Font(Font, FontStyle.Bold) : null;
				TextRenderer.DrawText(g, _options[i], font ?? Font, rect, color,
					TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter | TextFormatFlags.SingleLine);
			}
			g.ResetClip();

			using (var pen = new Pen(Focused ? Theme.Sky : Theme.Border))
				g.DrawPath(pen, outerPath);
		}
	}

	/// <summary>
	/// Base for a field that shows a value and opens a popup to change it
	/// </summary>
	internal abstract class DropField : Control
	{
		private bool _hover;
		private long _lastClosed;
		private bool _hasError;

		protected DropField()
		{
			SetStyle(ControlStyles.UserPaint | ControlStyles.AllPaintingInWmPaint | ControlStyles.OptimizedDoubleBuffer |
			         ControlStyles.ResizeRedraw | ControlStyles.Selectable, true);
			Cursor = Cursors.Hand;
			TabStop = true;
			BackColor = Theme.Surface;
			ForeColor = Theme.Text;
			Height = Ui.S(30);
		}

		public string Placeholder { get; set; } = "";

		public bool HasError
		{
			get => _hasError;
			set { _hasError = value; Invalidate(); }
		}

		/// <summary>
		/// Text shown in the field
		/// </summary>
		protected abstract string DisplayText { get; }

		protected abstract void OpenPopup(string seed);

		/// <summary>
		/// The popup currently open for this field, closed if the field goes away
		/// </summary>
		protected ToolStripDropDown? ActivePopup { get; set; }

		protected override void Dispose(bool disposing)
		{
			if (disposing && ActivePopup is { IsDisposed: false } popup)
				popup.Close();
			base.Dispose(disposing);
		}

		/// <summary>
		/// Call when the popup closes, so the click that closed it does not reopen it
		/// </summary>
		protected void NotePopupClosed()
		{
			_lastClosed = Environment.TickCount64;
			Invalidate();
		}

		protected override void OnMouseDown(MouseEventArgs e)
		{
			base.OnMouseDown(e);
			if (e.Button != MouseButtons.Left)
				return;

			Focus();
			if (Environment.TickCount64 - _lastClosed > 200)
				OpenPopup("");
		}

		protected override bool IsInputKey(Keys keyData) =>
			keyData is Keys.Down or Keys.Up || base.IsInputKey(keyData);

		protected override void OnKeyDown(KeyEventArgs e)
		{
			if (e.KeyCode is Keys.Enter or Keys.Space or Keys.F4 or Keys.Down)
			{
				OpenPopup("");
				e.Handled = true;
				e.SuppressKeyPress = true;
			}
			base.OnKeyDown(e);
		}

		protected override void OnMouseEnter(EventArgs e) { _hover = true; Invalidate(); base.OnMouseEnter(e); }
		protected override void OnMouseLeave(EventArgs e) { _hover = false; Invalidate(); base.OnMouseLeave(e); }
		protected override void OnGotFocus(EventArgs e) { Invalidate(); base.OnGotFocus(e); }
		protected override void OnLostFocus(EventArgs e) { Invalidate(); base.OnLostFocus(e); }

		protected override void OnPaint(PaintEventArgs e)
		{
			var g = e.Graphics;
			g.Clear(Parent?.BackColor ?? Theme.Surface);
			g.SmoothingMode = SmoothingMode.AntiAlias;

			var rect = new Rectangle(0, 0, Width - 1, Height - 1);
			using (var path = Ui.Rounded(rect, Ui.S(4)))
			{
				using (var brush = new SolidBrush(Enabled ? Theme.Surface : Theme.SurfaceAlt))
					g.FillPath(brush, path);
				var border = _hasError ? Theme.ErrorText : Focused ? Theme.Sky : _hover ? Theme.TextMuted : Theme.Border;
				using var pen = new Pen(border);
				g.DrawPath(pen, path);
			}

			var chevronWidth = Ui.S(24);
			var text = DisplayText;
			var textRect = new Rectangle(Ui.S(8), 0, Width - Ui.S(8) - chevronWidth, Height);
			TextRenderer.DrawText(g, string.IsNullOrEmpty(text) ? Placeholder : text, Font, textRect,
				string.IsNullOrEmpty(text) || !Enabled ? Theme.TextMuted : ForeColor,
				TextFormatFlags.VerticalCenter | TextFormatFlags.Left | TextFormatFlags.SingleLine | TextFormatFlags.EndEllipsis);

			Ui.DrawChevron(g, new Rectangle(Width - chevronWidth, 0, chevronWidth - Ui.S(4), Height), Theme.TextMuted);
		}
	}

	/// <summary>
	/// Dropdown with a short fixed list of choices
	/// </summary>
	internal sealed class ChoiceField : DropField
	{
		private readonly List<(string Label, object Value)> _items = new();
		private object? _selected;

		public event EventHandler? SelectionChanged;

		public object? SelectedValue
		{
			get => _selected;
			set
			{
				if (Equals(_selected, value))
					return;
				_selected = value;
				Invalidate();
			}
		}

		public void SetItems(IEnumerable<(string Label, object Value)> items, object? selected)
		{
			_items.Clear();
			_items.AddRange(items);
			_selected = selected;
			Invalidate();
		}

		protected override string DisplayText => _items.FirstOrDefault(i => Equals(i.Value, _selected)).Label ?? "";

		protected override void OpenPopup(string seed)
		{
			if (_items.Count == 0)
				return;

			var menu = new ContextMenuStrip { Renderer = Theme.Renderer, ShowImageMargin = false, ShowCheckMargin = true, Font = Font };
			foreach (var item in _items)
			{
				var value = item.Value;
				var menuItem = new ToolStripMenuItem(item.Label) { Checked = Equals(value, _selected) };
				menuItem.Click += (_, _) =>
				{
					if (Equals(_selected, value))
						return;
					_selected = value;
					Invalidate();
					SelectionChanged?.Invoke(this, EventArgs.Empty);
				};
				menu.Items.Add(menuItem);
			}

			menu.MinimumSize = new Size(Width, 0);
			ActivePopup = menu;
			menu.Closed += (_, _) =>
			{
				ActivePopup = null;
				NotePopupClosed();
				menu.BeginInvoke(new Action(menu.Dispose));
			};
			menu.Show(this, new Point(0, Height));
		}
	}

	/// <summary>
	/// Popup with a search box above a grouped list. Typing filters the list; Enter or a click picks.
	/// </summary>
	internal sealed class FilterPopup : ToolStripDropDown
	{
		public sealed record Entry(string Group, string Label, string? Detail, object Value, string? SearchText = null);

		private sealed class SearchBox : TextBox
		{
			public Func<Keys, bool>? CommandKey;

			protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
			{
				if (CommandKey != null && CommandKey(keyData))
					return true;
				return base.ProcessCmdKey(ref msg, keyData);
			}
		}

		private readonly List<Entry> _entries;
		private readonly List<object> _rows = new();
		private readonly SearchBox _search;
		private readonly ListBox _list;
		private int _highlight = -1;

		public event Action<Entry>? Picked;

		public FilterPopup(IEnumerable<Entry> entries, string placeholder, int width, Font font)
		{
			_entries = entries.ToList();
			Padding = Padding.Empty;
			Margin = Padding.Empty;
			AutoClose = true;
			DropShadowEnabled = true;
			BackColor = Theme.Surface;

			var panel = new Panel
			{
				Size = new Size(width, Ui.S(330)),
				BackColor = Theme.Surface,
				Padding = new Padding(Ui.S(6)),
				Font = font
			};

			_search = new SearchBox
			{
				Dock = DockStyle.Top,
				BorderStyle = BorderStyle.FixedSingle,
				PlaceholderText = placeholder,
				BackColor = Theme.Surface,
				ForeColor = Theme.Text
			};
			_search.CommandKey = HandleKey;
			_search.TextChanged += (_, _) => Refilter();

			var spacer = new Panel { Dock = DockStyle.Top, Height = Ui.S(6), BackColor = Theme.Surface };

			_list = new ListBox
			{
				Dock = DockStyle.Fill,
				BorderStyle = BorderStyle.None,
				DrawMode = DrawMode.OwnerDrawFixed,
				ItemHeight = Ui.S(24),
				IntegralHeight = false,
				SelectionMode = SelectionMode.None,
				BackColor = Theme.Surface
			};
			_list.DrawItem += DrawRow;
			_list.MouseMove += (_, e) =>
			{
				var index = _list.IndexFromPoint(e.Location);
				if (index >= 0 && index < _rows.Count && _rows[index] is Entry && index != _highlight)
				{
					_highlight = index;
					_list.Invalidate();
				}
			};
			_list.MouseClick += (_, e) =>
			{
				var index = _list.IndexFromPoint(e.Location);
				if (index >= 0 && index < _rows.Count && _rows[index] is Entry entry)
					Pick(entry);
			};

			//Fill first, then top-docked controls outermost last
			panel.Controls.Add(_list);
			panel.Controls.Add(spacer);
			panel.Controls.Add(_search);

			var host = new ToolStripControlHost(panel)
			{
				Margin = Padding.Empty,
				Padding = Padding.Empty,
				AutoSize = false,
				Size = panel.Size
			};
			Items.Add(host);

			Opened += (_, _) =>
			{
				_search.Focus();
				_search.SelectionStart = _search.Text.Length;
			};
		}

		/// <summary>
		/// Show under a control, with optional text already typed into the search box
		/// </summary>
		public void ShowFor(Control anchor, Point location, string seed = "")
		{
			_search.Text = seed;
			Refilter();
			Show(anchor, location);
		}

		private void Refilter()
		{
			var tokens = _search.Text.Split(' ', StringSplitOptions.RemoveEmptyEntries);
			_rows.Clear();
			foreach (var group in _entries.GroupBy(e => e.Group))
			{
				var matches = group.Where(e => tokens.All(t =>
					(e.Label + " " + e.Detail + " " + e.Group + " " + e.SearchText)
					.Contains(t, StringComparison.OrdinalIgnoreCase))).ToList();
				if (matches.Count == 0)
					continue;
				if (!string.IsNullOrEmpty(group.Key))
					_rows.Add(group.Key);
				_rows.AddRange(matches);
			}

			if (_rows.Count == 0)
				_rows.Add("No matches");

			_list.BeginUpdate();
			_list.Items.Clear();
			foreach (var row in _rows)
				_list.Items.Add(row);
			_list.EndUpdate();

			_highlight = _rows.FindIndex(r => r is Entry);
			_list.TopIndex = 0;
			_list.Invalidate();
		}

		private void MoveHighlight(int delta)
		{
			if (_highlight < 0)
				return;

			var index = _highlight;
			var steps = Math.Abs(delta);
			var dir = Math.Sign(delta);
			while (steps > 0)
			{
				var next = index + dir;
				while (next >= 0 && next < _rows.Count && _rows[next] is not Entry)
					next += dir;
				if (next < 0 || next >= _rows.Count)
					break;
				index = next;
				steps--;
			}

			_highlight = index;
			var visible = Math.Max(1, _list.ClientSize.Height / _list.ItemHeight);
			if (_highlight < _list.TopIndex + 1)
				_list.TopIndex = Math.Max(0, _highlight - 1);
			else if (_highlight >= _list.TopIndex + visible)
				_list.TopIndex = _highlight - visible + 1;
			_list.Invalidate();
		}

		private bool HandleKey(Keys key)
		{
			switch (key)
			{
				case Keys.Down: MoveHighlight(1); return true;
				case Keys.Up: MoveHighlight(-1); return true;
				case Keys.PageDown: MoveHighlight(8); return true;
				case Keys.PageUp: MoveHighlight(-8); return true;
				case Keys.Enter:
				case Keys.Tab:
					if (_highlight >= 0 && _highlight < _rows.Count && _rows[_highlight] is Entry entry)
						Pick(entry);
					else
						Close(ToolStripDropDownCloseReason.Keyboard);
					return true;
				case Keys.Escape:
					Close(ToolStripDropDownCloseReason.Keyboard);
					return true;
			}
			return false;
		}

		private void Pick(Entry entry)
		{
			Close(ToolStripDropDownCloseReason.ItemClicked);
			Picked?.Invoke(entry);
		}

		private void DrawRow(object? sender, DrawItemEventArgs e)
		{
			if (e.Index < 0 || e.Index >= _rows.Count)
				return;

			var g = e.Graphics;
			var row = _rows[e.Index];
			var bounds = e.Bounds;

			if (row is Entry entry)
			{
				var highlighted = e.Index == _highlight;
				using (var brush = new SolidBrush(highlighted ? Theme.SkyLight : Theme.Surface))
					g.FillRectangle(brush, bounds);

				var detailWidth = string.IsNullOrEmpty(entry.Detail) ? 0 : TextRenderer.MeasureText(entry.Detail, _list.Font).Width + Ui.S(8);
				var labelRect = new Rectangle(bounds.X + Ui.S(14), bounds.Y, bounds.Width - Ui.S(18) - detailWidth, bounds.Height);
				TextRenderer.DrawText(g, entry.Label, _list.Font, labelRect, highlighted ? Theme.Navy : Theme.Text,
					TextFormatFlags.VerticalCenter | TextFormatFlags.Left | TextFormatFlags.SingleLine | TextFormatFlags.EndEllipsis);

				if (detailWidth > 0)
				{
					var detailRect = new Rectangle(bounds.Right - detailWidth - Ui.S(4), bounds.Y, detailWidth, bounds.Height);
					TextRenderer.DrawText(g, entry.Detail, _list.Font, detailRect, Theme.TextMuted,
						TextFormatFlags.VerticalCenter | TextFormatFlags.Right | TextFormatFlags.SingleLine);
				}
			}
			else
			{
				using (var brush = new SolidBrush(Theme.Surface))
					g.FillRectangle(brush, bounds);
				using var font = new Font(_list.Font.FontFamily, _list.Font.Size - 0.5f, FontStyle.Bold);
				TextRenderer.DrawText(g, (string)row, font, new Rectangle(bounds.X + Ui.S(4), bounds.Y + Ui.S(2), bounds.Width, bounds.Height),
					Theme.TextMuted, TextFormatFlags.VerticalCenter | TextFormatFlags.Left | TextFormatFlags.SingleLine);
			}
		}
	}

	/// <summary>
	/// Picks a VRS property from a searchable, grouped popup
	/// </summary>
	internal sealed class PropertyField : DropField
	{
		private Enums.VrsProperty? _property;

		public event EventHandler? PropertyChanged;

		public PropertyField()
		{
			Placeholder = "Choose a property";
		}

		public Enums.VrsProperty? Property
		{
			get => _property;
			set
			{
				_property = value;
				Invalidate();
			}
		}

		protected override string DisplayText => _property == null ? "" : PropertyMeta.Label(_property.Value);

		protected override void OnKeyPress(KeyPressEventArgs e)
		{
			//Typing on the closed field starts a search
			if (char.IsLetterOrDigit(e.KeyChar))
			{
				OpenPopup(e.KeyChar.ToString());
				e.Handled = true;
			}
			base.OnKeyPress(e);
		}

		public void Open() => OpenPopup("");

		protected override void OpenPopup(string seed)
		{
			var entries = new List<FilterPopup.Entry>();
			foreach (var (category, properties) in PropertyMeta.Grouped())
			{
				foreach (var p in properties)
				{
					var kind = PropertyMeta.ValueType(p) switch
					{
						"Boolean" => "yes/no",
						"Number" => PropertyMeta.Unit(p) ?? "number",
						_ => "text"
					};
					entries.Add(new FilterPopup.Entry(category, PropertyMeta.Label(p), kind, p,
						p.ToString().Replace('_', ' ') + " " + PropertyMeta.InternalName(p)));
				}
			}

			var popup = new FilterPopup(entries, "Search properties", Math.Max(Width, Ui.S(300)), Font);
			popup.Picked += entry =>
			{
				var picked = (Enums.VrsProperty)entry.Value;
				if (_property == picked)
					return;
				_property = picked;
				Invalidate();
				PropertyChanged?.Invoke(this, EventArgs.Empty);
			};
			ActivePopup = popup;
			popup.Closed += (_, _) =>
			{
				ActivePopup = null;
				NotePopupClosed();
				popup.BeginInvoke(new Action(popup.Dispose));
			};
			popup.ShowFor(this, new Point(0, Height), seed);
		}
	}

	/// <summary>
	/// Bordered single-line text box, optionally with a unit shown at the right
	/// </summary>
	internal sealed class FieldBox : Control
	{
		private string? _unit;
		private bool _hover;
		private bool _hasError;

		public TextBox Inner { get; }

		/// <summary>
		/// Draw only a bottom rule instead of a full border (used for the headline name)
		/// </summary>
		public bool Underline { get; set; }

		public FieldBox()
		{
			SetStyle(ControlStyles.UserPaint | ControlStyles.AllPaintingInWmPaint | ControlStyles.OptimizedDoubleBuffer |
			         ControlStyles.ResizeRedraw, true);
			BackColor = Theme.Surface;
			Cursor = Cursors.IBeam;

			Inner = new TextBox { BorderStyle = BorderStyle.None, BackColor = Theme.Surface, ForeColor = Theme.Text };
			Inner.GotFocus += (_, _) => Invalidate();
			Inner.LostFocus += (_, _) => Invalidate();
			Inner.MouseEnter += (_, _) => { _hover = true; Invalidate(); };
			Inner.MouseLeave += (_, _) => { _hover = false; Invalidate(); };
			Controls.Add(Inner);
			Height = Ui.S(30);
		}

		public string? Unit
		{
			get => _unit;
			set { _unit = value; PerformLayout(); Invalidate(); }
		}

		public bool HasError
		{
			get => _hasError;
			set { _hasError = value; Invalidate(); }
		}

		public string Placeholder
		{
			get => Inner.PlaceholderText;
			set => Inner.PlaceholderText = value;
		}

		public override string Text
		{
			get => Inner.Text;
#pragma warning disable CS8765
			set => Inner.Text = value ?? "";
#pragma warning restore CS8765
		}

		protected override void OnMouseEnter(EventArgs e) { _hover = true; Invalidate(); base.OnMouseEnter(e); }
		protected override void OnMouseLeave(EventArgs e) { _hover = false; Invalidate(); base.OnMouseLeave(e); }

		protected override void OnMouseDown(MouseEventArgs e)
		{
			Inner.Focus();
			base.OnMouseDown(e);
		}

		protected override void OnGotFocus(EventArgs e)
		{
			Inner.Focus();
			base.OnGotFocus(e);
		}

		protected override void OnEnabledChanged(EventArgs e)
		{
			base.OnEnabledChanged(e);
			Inner.BackColor = Enabled ? Theme.Surface : Theme.SurfaceAlt;
			Invalidate();
		}

		protected override void OnLayout(LayoutEventArgs e)
		{
			base.OnLayout(e);
			var unitWidth = string.IsNullOrEmpty(_unit) ? 0 : TextRenderer.MeasureText(_unit, Font).Width + Ui.S(6);
			var pad = Underline ? Ui.S(2) : Ui.S(8);
			Inner.SetBounds(pad, (Height - Inner.Height) / 2, Math.Max(10, Width - pad * 2 - unitWidth), Inner.Height);
		}

		protected override void OnPaint(PaintEventArgs e)
		{
			var g = e.Graphics;
			var focused = Inner.Focused;
			var border = _hasError ? Theme.ErrorText : focused ? Theme.Sky : _hover ? Theme.TextMuted : Theme.Border;

			if (Underline)
			{
				g.Clear(BackColor);
				using var pen = new Pen(focused || _hasError ? border : _hover ? Theme.Border : BackColor, focused ? Ui.S(2) : 1);
				var y = Height - (focused ? Ui.S(2) : 1);
				g.DrawLine(pen, 0, y, Width, y);
				return;
			}

			g.Clear(Parent?.BackColor ?? Theme.Surface);
			g.SmoothingMode = SmoothingMode.AntiAlias;
			var rect = new Rectangle(0, 0, Width - 1, Height - 1);
			using (var path = Ui.Rounded(rect, Ui.S(4)))
			{
				using (var brush = new SolidBrush(Enabled ? Theme.Surface : Theme.SurfaceAlt))
					g.FillPath(brush, path);
				using var pen = new Pen(border);
				g.DrawPath(pen, path);
			}

			if (!string.IsNullOrEmpty(_unit))
			{
				var unitRect = new Rectangle(0, 0, Width - Ui.S(8), Height);
				TextRenderer.DrawText(g, _unit, Font, unitRect, Theme.TextMuted,
					TextFormatFlags.Right | TextFormatFlags.VerticalCenter | TextFormatFlags.SingleLine);
			}
		}
	}

	/// <summary>
	/// One address in a <see cref="ChipInput"/>
	/// </summary>
	internal sealed class Chip : Control
	{
		private bool _hoverRemove;

		public bool Invalid { get; set; }

		public event EventHandler? RemoveClicked;
		public event EventHandler? EditRequested;

		public Chip(string text, Font font)
		{
			SetStyle(ControlStyles.UserPaint | ControlStyles.AllPaintingInWmPaint | ControlStyles.OptimizedDoubleBuffer |
			         ControlStyles.ResizeRedraw | ControlStyles.SupportsTransparentBackColor, true);
			Font = font;
			Text = text;
			Cursor = Cursors.Default;
			Size = new Size(TextRenderer.MeasureText(text, font).Width + Ui.S(32), Ui.S(24));
		}

		private Rectangle RemoveRect => new(Width - Ui.S(22), 0, Ui.S(20), Height);

		protected override void OnMouseMove(MouseEventArgs e)
		{
			var over = RemoveRect.Contains(e.Location);
			if (over != _hoverRemove)
			{
				_hoverRemove = over;
				Cursor = over ? Cursors.Hand : Cursors.Default;
				Invalidate();
			}
			base.OnMouseMove(e);
		}

		protected override void OnMouseLeave(EventArgs e)
		{
			_hoverRemove = false;
			Invalidate();
			base.OnMouseLeave(e);
		}

		protected override void OnMouseClick(MouseEventArgs e)
		{
			if (RemoveRect.Contains(e.Location))
				RemoveClicked?.Invoke(this, EventArgs.Empty);
			base.OnMouseClick(e);
		}

		protected override void OnMouseDoubleClick(MouseEventArgs e)
		{
			if (!RemoveRect.Contains(e.Location))
				EditRequested?.Invoke(this, EventArgs.Empty);
			base.OnMouseDoubleClick(e);
		}

		protected override void OnPaint(PaintEventArgs e)
		{
			var g = e.Graphics;
			g.Clear(Parent?.BackColor ?? Theme.Surface);
			g.SmoothingMode = SmoothingMode.AntiAlias;

			var rect = new Rectangle(0, 0, Width - 1, Height - 1);
			using (var path = Ui.Rounded(rect, rect.Height / 2))
			{
				using (var brush = new SolidBrush(Invalid ? Theme.ErrorBackground : Theme.SkyLight))
					g.FillPath(brush, path);
				using var pen = new Pen(Invalid ? Theme.ErrorText : Theme.SkyLight);
				g.DrawPath(pen, path);
			}

			var textColor = Invalid ? Theme.ErrorText : Theme.Navy;
			TextRenderer.DrawText(g, Text, Font, new Rectangle(Ui.S(10), 0, Width - Ui.S(32), Height), textColor,
				TextFormatFlags.VerticalCenter | TextFormatFlags.Left | TextFormatFlags.SingleLine | TextFormatFlags.EndEllipsis);

			var r = RemoveRect;
			var cx = r.X + r.Width / 2;
			var cy = r.Y + r.Height / 2;
			var s = Ui.S(3);
			using var xPen = new Pen(_hoverRemove ? Theme.ErrorText : textColor, 1.5f);
			g.DrawLine(xPen, cx - s, cy - s, cx + s, cy + s);
			g.DrawLine(xPen, cx - s, cy + s, cx + s, cy - s);
		}
	}

	/// <summary>
	/// Text box that turns each entry into a removable chip. Enter, comma, semicolon or space adds the entry.
	/// </summary>
	internal sealed class ChipInput : Control
	{
		private readonly List<Chip> _chips = new();
		private readonly TextBox _box;
		private readonly ToolTip _toolTip = new();
		private bool _hasError;
		private bool _laying;

		public Func<string, bool>? Validator { get; set; }

		public string Placeholder { get; set; } = "";

		public event EventHandler? ValuesChanged;

		public ChipInput()
		{
			SetStyle(ControlStyles.UserPaint | ControlStyles.AllPaintingInWmPaint | ControlStyles.OptimizedDoubleBuffer |
			         ControlStyles.ResizeRedraw, true);
			BackColor = Theme.Surface;
			Cursor = Cursors.IBeam;

			_box = new TextBox { BorderStyle = BorderStyle.None, BackColor = Theme.Surface, ForeColor = Theme.Text };
			_box.KeyDown += BoxKeyDown;
			_box.KeyPress += BoxKeyPress;
			_box.TextChanged += BoxTextChanged;
			_box.Leave += (_, _) => CommitText();
			_box.GotFocus += (_, _) => Invalidate();
			_box.LostFocus += (_, _) => Invalidate();
			Controls.Add(_box);
			Height = Ui.S(34);
		}

		public bool HasError
		{
			get => _hasError;
			set { _hasError = value; Invalidate(); }
		}

		/// <summary>
		/// Entries as chips, plus anything still typed in the box
		/// </summary>
		public List<string> Values
		{
			get
			{
				var values = _chips.Select(c => c.Text).ToList();
				var pending = _box.Text.Trim();
				if (pending.Length > 0 && !values.Contains(pending, StringComparer.OrdinalIgnoreCase))
					values.Add(pending);
				return values;
			}
		}

		public void SetValues(IEnumerable<string> values)
		{
			SuspendLayout();
			foreach (var chip in _chips)
			{
				Controls.Remove(chip);
				chip.Dispose();
			}
			_chips.Clear();
			_box.Text = "";
			foreach (var value in values)
				AddChip(value, false);
			ResumeLayout();
			UpdatePlaceholder();
			PerformLayout();
		}

		public void FocusInput() => _box.Focus();

		private void AddChip(string text, bool raise)
		{
			text = text.Trim();
			if (text.Length == 0 || _chips.Any(c => string.Equals(c.Text, text, StringComparison.OrdinalIgnoreCase)))
				return;

			var chip = new Chip(text, Font) { Invalid = Validator != null && !Validator(text) };
			if (chip.Invalid)
				_toolTip.SetToolTip(chip, "This doesn't look like an email address. Double-click to edit.");
			chip.RemoveClicked += (_, _) => RemoveChip(chip);
			chip.EditRequested += (_, _) =>
			{
				CommitText();
				RemoveChip(chip);
				_box.Text = chip.Text;
				_box.SelectionStart = _box.Text.Length;
				_box.Focus();
			};
			_chips.Add(chip);
			Controls.Add(chip);
			UpdatePlaceholder();
			PerformLayout();
			if (raise)
				ValuesChanged?.Invoke(this, EventArgs.Empty);
		}

		private void RemoveChip(Chip chip)
		{
			_chips.Remove(chip);
			Controls.Remove(chip);
			chip.Dispose();
			UpdatePlaceholder();
			PerformLayout();
			ValuesChanged?.Invoke(this, EventArgs.Empty);
		}

		private void UpdatePlaceholder()
		{
			_box.PlaceholderText = _chips.Count == 0 ? Placeholder : "";
		}

		private void CommitText()
		{
			var text = _box.Text.Trim();
			if (text.Length == 0)
				return;
			_box.Text = "";
			AddChip(text, true);
		}

		private void BoxKeyDown(object? sender, KeyEventArgs e)
		{
			if (e.KeyCode == Keys.Enter)
			{
				CommitText();
				e.Handled = true;
				e.SuppressKeyPress = true;
			}
			else if (e.KeyCode == Keys.Back && _box.TextLength == 0 && _chips.Count > 0)
			{
				//Pull the last chip back into the box for editing
				var last = _chips[^1];
				RemoveChip(last);
				_box.Text = last.Text;
				_box.SelectionStart = _box.Text.Length;
				e.Handled = true;
				e.SuppressKeyPress = true;
			}
		}

		private void BoxKeyPress(object? sender, KeyPressEventArgs e)
		{
			if (e.KeyChar is ',' or ';' or ' ')
			{
				CommitText();
				e.Handled = true;
			}
		}

		private void BoxTextChanged(object? sender, EventArgs e)
		{
			//Pasted lists: split on separators and keep the last fragment in the box
			var text = _box.Text;
			if (text.IndexOfAny(new[] { ',', ';', ' ', '\n', '\r', '\t' }) < 0)
				return;

			var parts = text.Split(new[] { ',', ';', ' ', '\n', '\r', '\t' });
			_box.TextChanged -= BoxTextChanged;
			_box.Text = "";
			_box.TextChanged += BoxTextChanged;
			for (var i = 0; i < parts.Length - 1; i++)
				AddChip(parts[i], true);
			_box.Text = parts[^1];
			_box.SelectionStart = _box.Text.Length;
		}

		protected override void OnMouseDown(MouseEventArgs e)
		{
			_box.Focus();
			base.OnMouseDown(e);
		}

		protected override void OnSizeChanged(EventArgs e)
		{
			base.OnSizeChanged(e);
			PerformLayout();
		}

		private int MeasureHeight(int width, bool apply)
		{
			var pad = Ui.S(5);
			var gap = Ui.S(4);
			var lineHeight = Ui.S(24);
			var x = pad;
			var y = pad;

			foreach (var chip in _chips)
			{
				var w = Math.Min(chip.Width, width - pad * 2);
				if (x > pad && x + w > width - pad)
				{
					x = pad;
					y += lineHeight + gap;
				}
				if (apply)
					chip.SetBounds(x, y, w, lineHeight);
				x += w + gap;
			}

			var minBox = Ui.S(140);
			if (x > pad && width - pad - x < minBox)
			{
				x = pad;
				y += lineHeight + gap;
			}
			if (apply)
				_box.SetBounds(x + Ui.S(3), y + (lineHeight - _box.Height) / 2, Math.Max(10, width - pad - x - Ui.S(3)), _box.Height);

			return y + lineHeight + pad;
		}

		public override Size GetPreferredSize(Size proposedSize) =>
			new(Width, MeasureHeight(Width, false));

		protected override void OnLayout(LayoutEventArgs e)
		{
			base.OnLayout(e);
			if (_laying)
				return;

			_laying = true;
			var needed = MeasureHeight(Width, true);
			if (Height != needed)
				Height = needed;
			_laying = false;
			Invalidate();
		}

		protected override void OnPaint(PaintEventArgs e)
		{
			var g = e.Graphics;
			g.Clear(Parent?.BackColor ?? Theme.Surface);
			g.SmoothingMode = SmoothingMode.AntiAlias;
			var rect = new Rectangle(0, 0, Width - 1, Height - 1);
			using var path = Ui.Rounded(rect, Ui.S(4));
			using (var brush = new SolidBrush(Theme.Surface))
				g.FillPath(brush, path);
			var border = _hasError ? Theme.ErrorText : _box.Focused ? Theme.Sky : Theme.Border;
			using var pen = new Pen(border);
			g.DrawPath(pen, path);
		}

		protected override void Dispose(bool disposing)
		{
			if (disposing)
				_toolTip.Dispose();
			base.Dispose(disposing);
		}
	}
}
