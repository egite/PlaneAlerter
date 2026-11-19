using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Newtonsoft.Json;
using PlaneAlerter.Enums;
using PlaneAlerter.Models;
using PlaneAlerter.Services;

namespace PlaneAlerter.Forms
{
	/// <summary>
	/// One editable trigger. Holds whatever the user has entered, valid or not.
	/// </summary>
	internal sealed class RuleDraft
	{
		public VrsProperty? Property { get; set; }
		public string Comparison { get; set; } = "";
		public string Value { get; set; } = "";

		public RuleDraft Clone() => new() { Property = Property, Comparison = Comparison, Value = Value };

		/// <summary>
		/// Yes/no rules store "Equals True" for yes and "Not Equals True" for no
		/// </summary>
		public bool BooleanIsYes =>
			(Comparison == "Equals" && !string.Equals(Value, "False", StringComparison.OrdinalIgnoreCase)) ||
			(Comparison == "Not Equals" && string.Equals(Value, "False", StringComparison.OrdinalIgnoreCase));

		/// <summary>
		/// Problem with this rule, or null if it is complete
		/// </summary>
		public string? Problem()
		{
			if (Property == null)
				return "Choose a property";

			var property = Property.Value;
			if (PropertyMeta.IsBoolean(property))
				return null;

			if (!PropertyMeta.Comparisons(property).Contains(Comparison))
				return "Choose a comparison";

			var value = Value.Trim();
			var equality = Comparison is "Equals" or "Not Equals";
			if (value.Length == 0)
				return equality ? null : "Enter a value";

			if (PropertyMeta.ValueType(property) == "Number" &&
			    !double.TryParse(value, NumberStyles.Float, CultureInfo.CurrentCulture, out _))
				return "Enter a number";

			return null;
		}

		public Trigger ToTrigger() => new(Property!.Value, Value.Trim(), Comparison);
	}

	/// <summary>
	/// Editable copy of a condition. Enabled and alert timing are held separately so switching
	/// a condition off and on again keeps its timing.
	/// </summary>
	internal sealed class ConditionDraft
	{
		public string Name { get; set; } = "";
		public bool Enabled { get; set; } = true;
		public AlertType Timing { get; set; } = AlertType.First_and_Last_Contact;
		public bool IgnoreFollowing { get; set; }
		public bool UseOrLogic { get; set; }
		public bool EmailEnabled { get; set; } = true;
		public string FirstSubject { get; set; } = "";
		public string LastSubject { get; set; } = "";
		public List<string> Recipients { get; set; } = new();
		public List<RuleDraft> Rules { get; set; } = new();

		/// <summary>
		/// Show problems inline. Set once the user has left the condition or tried to save.
		/// </summary>
		[JsonIgnore]
		public bool ShowProblems { get; set; }

		public bool SendsFirst => Timing != AlertType.Last_Contact;
		public bool SendsLast => Timing != AlertType.First_Contact;

		public static ConditionDraft From(Condition c) => new()
		{
			Name = c.Name,
			Enabled = c.Enabled && c.AlertType != AlertType.Disabled,
			Timing = c.AlertType == AlertType.Disabled ? AlertType.First_and_Last_Contact : c.AlertType,
			IgnoreFollowing = c.IgnoreFollowing,
			UseOrLogic = c.TriggersUseOrLogic,
			EmailEnabled = c.EmailEnabled,
			FirstSubject = c.EmailFirstFormat ?? "",
			LastSubject = c.EmailLastFormat ?? "",
			Recipients = c.ReceiverEmails?.ToList() ?? new List<string>(),
			Rules = c.Triggers.OrderBy(t => t.Key).Select(t => new RuleDraft
			{
				Property = t.Value.Property,
				Comparison = t.Value.ComparisonType,
				Value = t.Value.Value
			}).ToList()
		};

		public ConditionDraft Clone() => new()
		{
			Name = Name,
			Enabled = Enabled,
			Timing = Timing,
			IgnoreFollowing = IgnoreFollowing,
			UseOrLogic = UseOrLogic,
			EmailEnabled = EmailEnabled,
			FirstSubject = FirstSubject,
			LastSubject = LastSubject,
			Recipients = Recipients.ToList(),
			Rules = Rules.Select(r => r.Clone()).ToList()
		};

		/// <summary>
		/// Build the stored condition. Only valid once <see cref="Problems"/> is empty.
		/// </summary>
		public Condition ToCondition()
		{
			var triggers = new Dictionary<int, Trigger>();
			foreach (var rule in Rules)
				triggers.Add(triggers.Count, rule.ToTrigger());

			return new Condition(Name.Trim(), Timing, IgnoreFollowing, UseOrLogic,
				EmailEnabled, FirstSubject, LastSubject, Recipients.ToList())
			{
				Enabled = Enabled,
				Triggers = triggers
			};
		}

		/// <summary>
		/// A condition with only the complete rules, for previewing matches while editing
		/// </summary>
		public Condition? ToPreviewCondition()
		{
			var rules = Rules.Where(r => r.Problem() == null).ToList();
			if (rules.Count == 0)
				return null;

			var triggers = new Dictionary<int, Trigger>();
			foreach (var rule in rules)
				triggers.Add(triggers.Count, rule.ToTrigger());

			return new Condition(Name, Timing, false, UseOrLogic, false, "", "", new List<string>()) { Triggers = triggers };
		}

		public enum Field { Name, Rules, Recipients, FirstSubject, LastSubject }

		public sealed record Problem(Field Field, string Message, RuleDraft? Rule = null);

		public List<Problem> Problems()
		{
			var problems = new List<Problem>();

			if (string.IsNullOrWhiteSpace(Name))
				problems.Add(new Problem(Field.Name, "Give the condition a name"));

			if (Rules.Count == 0)
				problems.Add(new Problem(Field.Rules, "Add at least one rule"));

			foreach (var rule in Rules)
			{
				var problem = rule.Problem();
				if (problem != null)
					problems.Add(new Problem(Field.Rules, problem, rule));
			}

			if (EmailEnabled)
			{
				if (Recipients.Count == 0)
					problems.Add(new Problem(Field.Recipients, "Add an email address to send to"));
				else
				{
					var invalid = Recipients.Where(r => !IsValidEmail(r)).ToList();
					if (invalid.Count > 0)
						problems.Add(new Problem(Field.Recipients,
							invalid.Count == 1 ? $"\"{invalid[0]}\" isn't a valid email address" : $"{invalid.Count} addresses aren't valid"));
				}

				if (SendsFirst && string.IsNullOrWhiteSpace(FirstSubject))
					problems.Add(new Problem(Field.FirstSubject, "Enter a subject for first contact emails"));
				if (SendsLast && string.IsNullOrWhiteSpace(LastSubject))
					problems.Add(new Problem(Field.LastSubject, "Enter a subject for last contact emails"));
			}

			return problems;
		}

		/// <summary>
		/// One-line description of the rules, e.g. "Military is yes · Altitude is below 5000 ft"
		/// </summary>
		public string Summary(IVrsEnumService enums)
		{
			var parts = Rules.Where(r => r.Property != null).Select(r => DescribeRule(r, enums)).ToList();
			if (parts.Count == 0)
				return "No rules yet";
			return string.Join(UseOrLogic ? " or " : " · ", parts);
		}

		public static string DescribeRule(RuleDraft rule, IVrsEnumService enums)
		{
			var property = rule.Property!.Value;
			var label = PropertyMeta.Label(property);

			if (PropertyMeta.IsBoolean(property))
				return $"{label} is {(rule.BooleanIsYes ? "yes" : "no")}";

			var value = rule.Value.Trim();
			var named = enums.GetValues(PropertyMeta.InternalName(property));
			if (named != null && int.TryParse(value, out var key) && named.TryGetValue(key, out var name))
				value = name;
			else if (value.Length == 0)
				value = "blank";
			else if (PropertyMeta.Unit(property) is { } unit)
				value = unit == "°" ? value + unit : $"{value} {unit}";

			return $"{label} {PropertyMeta.ComparisonLabel(rule.Comparison)} {value}";
		}

		public static bool IsValidEmail(string address)
		{
			try
			{
				var parsed = new System.Net.Mail.MailAddress(address);
				return parsed.Address == address && address.Contains('.');
			}
			catch (FormatException)
			{
				return false;
			}
		}
	}
}
