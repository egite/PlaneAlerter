using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using PlaneAlerter.Controls;

namespace PlaneAlerter.Forms
{
	/// <summary>
	/// Condition list that shows each condition's state, name and a one-line summary of its rules
	/// </summary>
	internal sealed class ConditionListBox : ListBox
	{
		private int _hover = -1;
		private int _dropIndex = -1;

		/// <summary>
		/// Second line of text for a condition, and whether it describes a problem
		/// </summary>
		public Func<ConditionDraft, (string Text, bool IsProblem)>? Describe { get; set; }

		public ConditionListBox()
		{
			DrawMode = DrawMode.OwnerDrawFixed;
			ItemHeight = Ui.S(52);
			IntegralHeight = false;
			BorderStyle = BorderStyle.None;
			BackColor = Theme.SurfaceAlt;
			SetStyle(ControlStyles.OptimizedDoubleBuffer | ControlStyles.ResizeRedraw, true);
		}

		/// <summary>
		/// Insertion point shown while dragging (index to insert before), -1 for none
		/// </summary>
		public int DropIndex
		{
			get => _dropIndex;
			set
			{
				if (_dropIndex == value)
					return;
				_dropIndex = value;
				Invalidate();
			}
		}

		protected override void OnMouseMove(MouseEventArgs e)
		{
			var index = IndexFromPoint(e.Location);
			if (index != _hover)
			{
				var old = _hover;
				_hover = index;
				InvalidateItem(old);
				InvalidateItem(index);
			}
			base.OnMouseMove(e);
		}

		protected override void OnMouseLeave(EventArgs e)
		{
			var old = _hover;
			_hover = -1;
			InvalidateItem(old);
			base.OnMouseLeave(e);
		}

		public void InvalidateItem(int index)
		{
			if (index >= 0 && index < Items.Count)
				Invalidate(GetItemRectangle(index));
		}

		public void InvalidateItem(ConditionDraft draft) => InvalidateItem(Items.IndexOf(draft));

		protected override void OnPaintBackground(PaintEventArgs pevent)
		{
			using var brush = new SolidBrush(BackColor);
			pevent.Graphics.FillRectangle(brush, ClientRectangle);
		}

		protected override void OnDrawItem(DrawItemEventArgs e)
		{
			if (e.Index < 0 || e.Index >= Items.Count || Items[e.Index] is not ConditionDraft draft)
				return;

			var g = e.Graphics;
			var b = e.Bounds;
			var selected = (e.State & DrawItemState.Selected) != 0;
			var hover = e.Index == _hover;

			var background = selected ? Theme.Surface : hover ? Theme.Background : Theme.SurfaceAlt;
			using (var brush = new SolidBrush(background))
				g.FillRectangle(brush, b);

			if (selected)
			{
				using var accent = new SolidBrush(Theme.Sky);
				g.FillRectangle(accent, b.X, b.Y, Ui.S(3), b.Height);
			}

			using (var line = new Pen(Theme.Border))
				g.DrawLine(line, b.X, b.Bottom - 1, b.Right, b.Bottom - 1);

			//Status dot: filled when enabled, hollow when off
			g.SmoothingMode = SmoothingMode.AntiAlias;
			var dot = Ui.S(8);
			var dotRect = new Rectangle(b.X + Ui.S(14), b.Y + Ui.S(13), dot, dot);
			if (draft.Enabled)
			{
				using var dotBrush = new SolidBrush(Theme.Success);
				g.FillEllipse(dotBrush, dotRect);
			}
			else
			{
				using var dotPen = new Pen(Theme.TextMuted, 1.2f);
				g.DrawEllipse(dotPen, dotRect);
			}
			g.SmoothingMode = SmoothingMode.Default;

			var (detail, isProblem) = Describe?.Invoke(draft) ?? ("", false);
			var textX = b.X + Ui.S(30);
			var warningWidth = isProblem ? Ui.S(20) : 0;

			var name = string.IsNullOrWhiteSpace(draft.Name) ? "Untitled condition" : draft.Name;
			using (var bold = new Font(Font, FontStyle.Bold))
			{
				var nameColor = !draft.Enabled ? Theme.TextMuted : selected ? Theme.Navy : Theme.Text;
				TextRenderer.DrawText(g, draft.Enabled ? name : name + "  (off)", bold,
					new Rectangle(textX, b.Y + Ui.S(6), b.Right - textX - Ui.S(8) - warningWidth, Ui.S(20)), nameColor,
					TextFormatFlags.Left | TextFormatFlags.VerticalCenter | TextFormatFlags.SingleLine | TextFormatFlags.EndEllipsis);
			}

			if (isProblem)
			{
				TextRenderer.DrawText(g, "⚠", Font, new Rectangle(b.Right - Ui.S(26), b.Y + Ui.S(6), Ui.S(20), Ui.S(20)),
					Theme.WarningText, TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter);
			}

			TextRenderer.DrawText(g, detail, Font,
				new Rectangle(textX, b.Y + Ui.S(26), b.Right - textX - Ui.S(8), Ui.S(20)),
				isProblem ? Theme.WarningText : Theme.TextMuted,
				TextFormatFlags.Left | TextFormatFlags.VerticalCenter | TextFormatFlags.SingleLine | TextFormatFlags.EndEllipsis);

			//Insertion line while dragging
			if (_dropIndex == e.Index || (_dropIndex == Items.Count && e.Index == Items.Count - 1))
			{
				var y = _dropIndex == e.Index ? b.Y + 1 : b.Bottom - 2;
				using var pen = new Pen(Theme.Sky, Ui.S(2));
				g.DrawLine(pen, b.X + Ui.S(4), y, b.Right - Ui.S(4), y);
			}
		}
	}
}
