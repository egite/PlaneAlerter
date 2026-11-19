using System.Collections.Generic;
using Newtonsoft.Json;
using PlaneAlerter.Enums;

namespace PlaneAlerter.Models
{
	/// <summary>
	/// Condition information
	/// </summary>
	internal class Condition
	{
		/// <summary>
		/// Name of condition
		/// </summary>
		public string Name { get; }

		/// <summary>
		/// Whether the condition is checked. Held separately from <see cref="AlertType"/> so turning
		/// a condition off and on again keeps its alert timing.
		/// </summary>
		public bool Enabled { get; set; } = true;

		/// <summary>
		/// Ignore following conditions
		/// </summary>
		public bool IgnoreFollowing { get; }

		/// <summary>
		/// Alerts sent for this condition
		/// </summary>
		[JsonIgnore]
		public int AlertsThisSession { get; set; } = 0;

		/// <summary>
		/// Send emails?
		/// </summary>
		public bool EmailEnabled { get; set; }

		/// <summary>
		/// Emails to send alert to
		/// </summary>
		public List<string> ReceiverEmails { get; }

		/// <summary>
		/// Email first contact alert format
		/// </summary>
		public string EmailFirstFormat { get; set; }

		/// <summary>
		/// Email last contact alert format
		/// </summary>
		public string EmailLastFormat { get; set; }

		/// <summary>
		/// Type of alert
		/// </summary>
		public AlertType AlertType { get; }

		/// <summary>
		/// Use OR logic instead of AND logic for triggers, only one trigger needs to match instead of all
		/// </summary>
		public bool TriggersUseOrLogic { get; set; }

		/// <summary>
		/// List of triggers
		/// </summary>
		public Dictionary<int, Trigger> Triggers { get; init; } = new();

		public Condition(
			string name,
			AlertType alertType,
			bool ignoreFollowing,
			bool triggersUseOrLogic,
			bool emailEnabled,
			string emailFirstFormat,
			string emailLastFormat,
			List<string> receiverEmails)
		{
			Name = name;
			AlertType = alertType;
			IgnoreFollowing = ignoreFollowing;
			TriggersUseOrLogic = triggersUseOrLogic;
			EmailEnabled = emailEnabled;
			EmailFirstFormat = emailFirstFormat;
			EmailLastFormat = emailLastFormat;
			ReceiverEmails = receiverEmails;
		}
	}
}
