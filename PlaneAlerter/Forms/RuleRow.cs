using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using PlaneAlerter.Controls;
using PlaneAlerter.Services;

namespace PlaneAlerter.Forms
{
	/// <summary>
	/// Editor for one rule: [property] [comparison] [value] [remove], with the property's description underneath
	/// </summary>
	internal sealed class RuleRow : Control
	{
		private readonly IVrsEnumService _enums;
		private readonly PropertyField _propertyField = new();
		private readonly ChoiceField _comparisonField = new();
		private readonly Label _isLabel;
		private readonly FieldBox _valueBox = new();
		private readonly ChoiceField _enumField = new();
		private readonly SegmentedControl _yesNo = new() { Options = new[] { "Yes", "No" } };
		private readonly Button _removeButton;
		private readonly Label _hintLabel;
		private readonly Label _errorLabel;
		private readonly ToolTip _toolTip;
		private bool _loading;
		private bool _laying;

		public RuleDraft Rule { get; }

		/// <summary>
		/// Raised after the user changes the rule
		/// </summary>
		public event EventHandler? Changed;

		public event EventHandler? RemoveRequested;

		public RuleRow(RuleDraft rule, IVrsEnumService enums, ToolTip toolTip, Font font)
		{
			Rule = rule;
			_enums = enums;
			_toolTip = toolTip;
			Font = font;
			BackColor = Theme.Surface;
			SetStyle(ControlStyles.OptimizedDoubleBuffer | ControlStyles.AllPaintingInWmPaint, true);

			_isLabel = new Label
			{
				Text = "is",
				TextAlign = ContentAlignment.MiddleLeft,
				ForeColor = Theme.Text,
				BackColor = Theme.Surface,
				Padding = new Padding(Ui.S(8), 0, 0, 0)
			};

			_removeButton = new Button
			{
				Text = "×",
				Font = new Font(font.FontFamily, font.Size + 3f),
				FlatStyle = FlatStyle.Flat,
				BackColor = Theme.Surface,
				ForeColor = Theme.TextMuted,
				Cursor = Cursors.Hand,
				TabStop = false
			};
			_removeButton.FlatAppearance.BorderSize = 0;
			_removeButton.FlatAppearance.MouseOverBackColor = Theme.ErrorBackground;
			_removeButton.FlatAppearance.MouseDownBackColor = Theme.ErrorBackground;
			_removeButton.Click += (_, _) => RemoveRequested?.Invoke(this, EventArgs.Empty);
			_toolTip.SetToolTip(_removeButton, "Remove this rule");

			_hintLabel = new Label { ForeColor = Theme.TextMuted, BackColor = Theme.Surface, AutoEllipsis = true };
			_errorLabel = new Label { ForeColor = Theme.ErrorText, BackColor = Theme.Surface, AutoEllipsis = true, Visible = false };

			_comparisonField.Placeholder = "Comparison";
			_enumField.Placeholder = "Choose a value";
			_valueBox.Placeholder = "Value";
			_yesNo.MinSegmentWidth = Ui.S(56);

			Controls.AddRange(new Control[]
			{
				_propertyField, _comparisonField, _isLabel, _valueBox, _enumField, _yesNo, _removeButton, _hintLabel, _errorLabel
			});

			_propertyField.PropertyChanged += (_, _) => OnPropertyPicked();
			_comparisonField.SelectionChanged += (_, _) =>
			{
				Rule.Comparison = (string)_comparisonField.SelectedValue!;
				RaiseChanged();
			};
			_valueBox.Inner.TextChanged += (_, _) =>
			{
				if (_loading)
					return;
				Rule.Value = _valueBox.Text;
				RaiseChanged();
			};
			_enumField.SelectionChanged += (_, _) =>
			{
				Rule.Value = Convert.ToString(_enumField.SelectedValue) ?? "";
				RaiseChanged();
			};
			_yesNo.SelectedIndexChanged += (_, _) =>
			{
				if (_loading)
					return;
				Rule.Comparison = _yesNo.SelectedIndex == 0 ? "Equals" : "Not Equals";
				Rule.Value = "True";
				RaiseChanged();
			};

			_propertyField.Property = rule.Property;
			ConfigureForProperty(keepValue: true);
		}

		/// <summary>
		/// Open the property picker, used when a rule is first added
		/// </summary>
		public void OpenPropertyPicker()
		{
			_propertyField.Focus();
			_propertyField.Open();
		}

		public void ShowError(string? message)
		{
			var visible = !string.IsNullOrEmpty(message);
			_errorLabel.Text = message ?? "";
			_propertyField.HasError = visible && Rule.Property == null;
			_valueBox.HasError = visible && Rule.Property != null;
			//The error takes the description's place under the row
			_errorLabel.Visible = visible;
			_hintLabel.Visible = !visible;
		}

		private void RaiseChanged()
		{
			if (!_loading)
				Changed?.Invoke(this, EventArgs.Empty);
		}

		private void OnPropertyPicked()
		{
			var previous = Rule.Property;
			Rule.Property = _propertyField.Property;

			//Keep a typed value when switching between two properties of the same kind
			var keep = previous != null && Rule.Property != null &&
			           PropertyMeta.ValueType(previous.Value) == PropertyMeta.ValueType(Rule.Property.Value) &&
			           _enums.GetValues(PropertyMeta.InternalName(previous.Value)) == null &&
			           _enums.GetValues(PropertyMeta.InternalName(Rule.Property.Value)) == null;

			ConfigureForProperty(keep);
			RaiseChanged();

			//Move straight on to the value
			if (_valueBox.Visible) _valueBox.Inner.Focus();
			else if (_enumField.Visible) _enumField.Focus();
			else if (_yesNo.Visible) _yesNo.Focus();
		}

		/// <summary>
		/// Show the comparison and value editors that suit the rule's property
		/// </summary>
		private void ConfigureForProperty(bool keepValue)
		{
			_loading = true;
			var property = Rule.Property;

			_comparisonField.Visible = false;
			_isLabel.Visible = false;
			_valueBox.Visible = false;
			_enumField.Visible = false;
			_yesNo.Visible = false;

			if (property == null)
			{
				_comparisonField.SetItems(Array.Empty<(string, object)>(), null);
				_comparisonField.Enabled = false;
				_comparisonField.Visible = true;
				_valueBox.Text = "";
				_valueBox.Unit = null;
				_valueBox.Enabled = false;
				_valueBox.Visible = true;
				_hintLabel.Text = "";
			}
			else
			{
				var p = property.Value;
				_hintLabel.Text = PropertyMeta.Description(p);
				_toolTip.SetToolTip(_hintLabel, PropertyMeta.Description(p));

				if (PropertyMeta.IsBoolean(p))
				{
					if (!keepValue || Rule.Comparison is not ("Equals" or "Not Equals"))
					{
						Rule.Comparison = "Equals";
						Rule.Value = "True";
					}
					_isLabel.Visible = true;
					_yesNo.SelectedIndex = Rule.BooleanIsYes ? 0 : 1;
					_yesNo.Visible = true;
				}
				else
				{
					var comparisons = PropertyMeta.Comparisons(p);
					if (!comparisons.Contains(Rule.Comparison))
						Rule.Comparison = comparisons[0];
					_comparisonField.SetItems(comparisons.Select(c => (PropertyMeta.ComparisonLabel(c), (object)c)), Rule.Comparison);
					_comparisonField.Enabled = true;
					_comparisonField.Visible = true;

					var named = _enums.GetValues(PropertyMeta.InternalName(p));
					if (named != null)
					{
						if (!int.TryParse(Rule.Value, out var key) || !named.ContainsKey(key))
							key = named.Keys.Min();
						Rule.Value = key.ToString();
						_enumField.SetItems(named.OrderBy(n => n.Key).Select(n => (n.Value, (object)n.Key.ToString())), Rule.Value);
						_enumField.Visible = true;
					}
					else
					{
						if (!keepValue)
							Rule.Value = "";
						_valueBox.Text = Rule.Value;
						_valueBox.Unit = PropertyMeta.Unit(p);
						_valueBox.Placeholder = PropertyMeta.ValueType(p) == "Number" ? "Number" : "Text";
						_valueBox.Enabled = true;
						_valueBox.Visible = true;
					}
				}
			}

			_loading = false;
			PerformLayout();
		}

		public override Size GetPreferredSize(Size proposedSize) => new(Width, MeasureHeight());

		private int MeasureHeight()
		{
			var line = Ui.S(30);
			var small = Font.Height + Ui.S(2);
			return line + Ui.S(2) + small + Ui.S(8);
		}

		protected override void OnSizeChanged(EventArgs e)
		{
			base.OnSizeChanged(e);
			PerformLayout();
		}

		protected override void OnLayout(LayoutEventArgs levent)
		{
			base.OnLayout(levent);
			if (_laying)
				return;
			_laying = true;

			var gap = Ui.S(6);
			var line = Ui.S(30);
			var removeWidth = Ui.S(28);
			var comparisonWidth = Ui.S(112);
			var flexible = Math.Max(Ui.S(200), Width - comparisonWidth - removeWidth - gap * 3);
			var propertyWidth = (int)(flexible * 0.48);
			var valueWidth = flexible - propertyWidth;

			var x = 0;
			_propertyField.SetBounds(x, 0, propertyWidth, line);
			x += propertyWidth + gap;
			_comparisonField.SetBounds(x, 0, comparisonWidth, line);
			_isLabel.SetBounds(x, 0, comparisonWidth, line);
			x += comparisonWidth + gap;
			_valueBox.SetBounds(x, 0, valueWidth, line);
			_enumField.SetBounds(x, 0, valueWidth, line);
			_yesNo.Location = new Point(x, 0);
			x += valueWidth + gap;
			_removeButton.SetBounds(x, 0, removeWidth, line);

			var small = Font.Height + Ui.S(2);
			var y = line + Ui.S(2);
			_hintLabel.SetBounds(Ui.S(2), y, Width - removeWidth - gap, small);
			_errorLabel.SetBounds(Ui.S(2), y, Width - removeWidth - gap, small);

			var needed = MeasureHeight();
			if (Height != needed)
				Height = needed;

			_laying = false;
		}
	}
}
