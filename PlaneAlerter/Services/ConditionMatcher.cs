using System;
using PlaneAlerter.Models;

namespace PlaneAlerter.Services
{
	/// <summary>
	/// Decides whether an aircraft matches a condition's triggers. Shared by the checker and the editor's live preview.
	/// </summary>
	internal static class ConditionMatcher
	{
		/// <summary>
		/// The checker skips aircraft that look like they have no VRS database info
		/// </summary>
		public static bool HasDatabaseInfo(Aircraft aircraft)
		{
			return !(aircraft.GetProperty("Reg") == null && aircraft.GetProperty("Type") == null &&
			         (aircraft.GetProperty("FlightsCount") == null ||
			          Convert.ToInt32(aircraft.GetProperty("FlightsCount")) == 0));
		}

		public static bool Matches(Condition condition, Aircraft aircraft)
		{
			var triggersMatching = 0;

			//Iterate triggers for condition
			foreach (var trigger in condition.Triggers.Values)
			{
				//Get internal name for property to compare
				var propertyInternalName = VrsProperties.VrsPropertyData[trigger.Property][2];
				var propertyValue = aircraft.GetProperty(propertyInternalName);

				//Check property against value
				if (trigger.ComparisonType == "Equals")
				{
					if (propertyValue == null)
					{
						if (trigger.Value == "") triggersMatching++;
					}
					else
					{
						if (propertyValue == trigger.Value) triggersMatching++;
					}
				}
				else if (trigger.ComparisonType == "Not Equals")
				{
					if (propertyValue == null)
					{
						if (trigger.Value != "") triggersMatching++;
					}
					else
					{
						if (propertyValue != trigger.Value) triggersMatching++;
					}
				}
				//These comparisons must have a non-empty value to compare with
				else if (!string.IsNullOrEmpty(propertyValue))
				{
					switch (trigger.ComparisonType)
					{
						case "Contains" when propertyValue.Contains(trigger.Value):
						case "Higher Than" when Convert.ToDouble(propertyValue) > Convert.ToDouble(trigger.Value):
						case "Lower Than" when Convert.ToDouble(propertyValue) < Convert.ToDouble(trigger.Value):
						case "Starts With" when (propertyValue.Length > trigger.Value.Length && propertyValue.Substring(0, trigger.Value.Length) == trigger.Value):
						case "Ends With" when (propertyValue.Length > trigger.Value.Length && propertyValue.Substring(propertyValue.Length - trigger.Value.Length) == trigger.Value):
							triggersMatching++;
							break;
					}
				}

				if (condition.TriggersUseOrLogic && triggersMatching > 0)
					return true;
			}

			return triggersMatching == condition.Triggers.Count;
		}
	}
}
