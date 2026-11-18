using System;
using System.Collections.Generic;
using System.Linq;
using PlaneAlerter.Enums;

namespace PlaneAlerter
{
	/// <summary>
	/// Friendly labels, categories and units for VRS properties, used by the condition editor
	/// </summary>
	internal static class PropertyMeta
	{
		public const string Identity = "Identity";
		public const string AircraftCategory = "Aircraft";
		public const string Flight = "Flight";
		public const string Position = "Position and movement";
		public const string Reception = "Reception";
		public const string Database = "Your database";

		/// <summary>
		/// Categories in the order they are listed
		/// </summary>
		public static readonly string[] Categories = { Identity, AircraftCategory, Flight, Position, Reception, Database };

		private static readonly Dictionary<VrsProperty, (string Category, string Label, string? Unit)> Data = new()
		{
			{ VrsProperty.Icao, (Identity, "ICAO hex", null) },
			{ VrsProperty.Invalid_Icao, (Identity, "Invalid ICAO", null) },
			{ VrsProperty.Registration, (Identity, "Registration", null) },
			{ VrsProperty.Callsign, (Identity, "Callsign", null) },
			{ VrsProperty.Callsign_Inaccurate, (Identity, "Callsign may be wrong", null) },
			{ VrsProperty.Squawk, (Identity, "Squawk", null) },
			{ VrsProperty.Ident, (Identity, "Squawking ident", null) },
			{ VrsProperty.Is_In_Emergency, (Identity, "Emergency", null) },
			{ VrsProperty.Transponder_Type, (Identity, "Transponder type", null) },
			{ VrsProperty.Registered_Country, (Identity, "Country", null) },

			{ VrsProperty.Aircraft_Model_Icao, (AircraftCategory, "Type code", null) },
			{ VrsProperty.Aircraft_Model, (AircraftCategory, "Model", null) },
			{ VrsProperty.Manufacturer, (AircraftCategory, "Manufacturer", null) },
			{ VrsProperty.Serial, (AircraftCategory, "Serial number", null) },
			{ VrsProperty.Year, (AircraftCategory, "Year built", null) },
			{ VrsProperty.Species, (AircraftCategory, "Species", null) },
			{ VrsProperty.Engines, (AircraftCategory, "Engine count", null) },
			{ VrsProperty.Engine_Type, (AircraftCategory, "Engine type", null) },
			{ VrsProperty.Engine_Mount, (AircraftCategory, "Engine placement", null) },
			{ VrsProperty.Wake_Turbulence_Category, (AircraftCategory, "Wake category", null) },
			{ VrsProperty.Is_Military, (AircraftCategory, "Military", null) },

			{ VrsProperty.Operator, (Flight, "Operator", null) },
			{ VrsProperty.Operator_Icao_Code, (Flight, "Operator code", null) },
			{ VrsProperty.Departure_Airport, (Flight, "From airport", null) },
			{ VrsProperty.Arrival_Airport, (Flight, "To airport", null) },
			{ VrsProperty.Is_Ferry_Flight, (Flight, "Ferry flight", null) },
			{ VrsProperty.Is_Charter_Flight, (Flight, "Charter flight", null) },

			{ VrsProperty.Altitude, (Position, "Altitude", "ft") },
			{ VrsProperty.Altitude_AMSL, (Position, "Altitude (AMSL)", "ft") },
			{ VrsProperty.Altitude_Type, (Position, "Altitude type", null) },
			{ VrsProperty.Air_Pressure, (Position, "Air pressure", "inHg") },
			{ VrsProperty.Target_Altitude, (Position, "Target altitude", "ft") },
			{ VrsProperty.Latitude, (Position, "Latitude", "°") },
			{ VrsProperty.Longitude, (Position, "Longitude", "°") },
			{ VrsProperty.Speed, (Position, "Speed", "kts") },
			{ VrsProperty.Speed_Type, (Position, "Speed type", null) },
			{ VrsProperty.Vertical_Speed, (Position, "Vertical speed", "ft/min") },
			{ VrsProperty.Vertical_Speed_Type, (Position, "Vertical speed type", null) },
			{ VrsProperty.Track, (Position, "Track", "°") },
			{ VrsProperty.Is_Track_Heading, (Position, "Track is heading", null) },
			{ VrsProperty.Target_Heading, (Position, "Target heading", "°") },
			{ VrsProperty.Is_On_Ground, (Position, "On the ground", null) },
			{ VrsProperty.Distance, (Position, "Distance", "km") },
			{ VrsProperty.Bearing, (Position, "Bearing", "°") },

			{ VrsProperty.Receiver, (Reception, "Receiver", null) },
			{ VrsProperty.Time_Tracked, (Reception, "Time tracked", "s") },
			{ VrsProperty.Message_Count, (Reception, "Messages received", null) },
			{ VrsProperty.Mlat, (Reception, "Position from MLAT", null) },
			{ VrsProperty.Is_Tisb, (Reception, "TIS-B source", null) },
			{ VrsProperty.Has_Signal_Level, (Reception, "Has signal level", null) },
			{ VrsProperty.Signal_Level, (Reception, "Signal level", null) },

			{ VrsProperty.User_Tag, (Database, "User tag", null) },
			{ VrsProperty.Notes, (Database, "Notes", null) },
			{ VrsProperty.Is_Interesting, (Database, "Interesting", null) },
			{ VrsProperty.Flight_Count, (Database, "Flights in database", null) },
		};

		public static string Label(VrsProperty property) =>
			Data.TryGetValue(property, out var d) ? d.Label : property.ToString().Replace('_', ' ');

		public static string Category(VrsProperty property) =>
			Data.TryGetValue(property, out var d) ? d.Category : Reception;

		public static string? Unit(VrsProperty property) =>
			Data.TryGetValue(property, out var d) ? d.Unit : null;

		/// <summary>
		/// "Number", "String" or "Boolean"
		/// </summary>
		public static string ValueType(VrsProperty property) => VrsProperties.VrsPropertyData[property][0];

		public static string InternalName(VrsProperty property) => VrsProperties.VrsPropertyData[property][2];

		/// <summary>
		/// Description without the "0 = none, 1 = ..." code list, since the editor shows named values instead
		/// </summary>
		public static string Description(VrsProperty property)
		{
			var full = VrsProperties.VrsPropertyData[property][3];
			var trimmed = System.Text.RegularExpressions.Regex.Replace(full, @"\s*\d+(/missing)?\s*=.*$", "",
				System.Text.RegularExpressions.RegexOptions.Singleline).TrimEnd(' ', ',', '.');
			return trimmed.Length == 0 ? full : trimmed + ".";
		}

		public static bool IsBoolean(VrsProperty property) => ValueType(property) == "Boolean";

		/// <summary>
		/// Stored comparison names the property supports, in display order
		/// </summary>
		public static List<string> Comparisons(VrsProperty property)
		{
			var supported = VrsProperties.VrsPropertyData[property][1];
			var list = new List<string>();
			foreach (var group in new[] { "A", "B", "C", "D", "E" })
			{
				if (!supported.Contains(group))
					continue;
				foreach (var comparison in VrsProperties.ComparisonTypes[group])
					if (!list.Contains(comparison))
						list.Add(comparison);
			}
			return list;
		}

		/// <summary>
		/// Plain-language wording for a stored comparison name
		/// </summary>
		public static string ComparisonLabel(string comparison) => comparison switch
		{
			"Equals" => "is",
			"Not Equals" => "is not",
			"Higher Than" => "is above",
			"Lower Than" => "is below",
			"Starts With" => "starts with",
			"Ends With" => "ends with",
			"Contains" => "contains",
			_ => comparison.ToLowerInvariant()
		};

		/// <summary>
		/// Properties grouped by category, each group sorted by label
		/// </summary>
		public static IEnumerable<(string Category, List<VrsProperty> Properties)> Grouped()
		{
			var all = Enum.GetValues(typeof(VrsProperty)).Cast<VrsProperty>().ToList();
			foreach (var category in Categories)
			{
				var props = all.Where(p => Category(p) == category)
					.OrderBy(Label, StringComparer.OrdinalIgnoreCase).ToList();
				if (props.Count > 0)
					yield return (category, props);
			}
		}
	}
}
