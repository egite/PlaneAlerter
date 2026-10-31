using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Net.Mime;
using System.Text;
using System.Threading.Tasks;
using PlaneAlerter.Enums;
using PlaneAlerter.Models;

namespace PlaneAlerter.Services
{
	internal interface IEmailBuilderService
	{
		Task<MailMessage> Build(Condition condition, Aircraft aircraft, string receiverName,
			bool isDetection);
	}

	internal class EmailBuilderService : IEmailBuilderService
	{
		//Palette shared with the desktop UI (see Theme.cs)
		private const string Navy = "#1B3A5C";
		private const string Sky = "#2E78C2";
		private const string Amber = "#F2A71B";
		private const string Background = "#EEF2F6";
		private const string Surface = "#FFFFFF";
		private const string SurfaceAlt = "#F6F9FC";
		private const string Border = "#C9D3DE";
		private const string Text = "#1E2A36";
		private const string TextMuted = "#5B6B7B";

		private const string FontStack = "'Segoe UI',Helvetica,Arial,sans-serif";

		private readonly ISettingsManagerService _settingsManagerService;
		private readonly IUrlBuilderService _urlBuilderService;
		private readonly IStringFormatterService _stringFormatterService;
		private readonly IKmlService _kmlService;
		private readonly IVrsEnumService _vrsEnumService;
		private readonly IVrsService _vrsService;

		public EmailBuilderService(ISettingsManagerService settingsManagerService, IUrlBuilderService urlBuilderService,
			IStringFormatterService stringFormatterService, IKmlService kmlService,
			IVrsEnumService vrsEnumService, IVrsService vrsService)
		{
			_settingsManagerService = settingsManagerService;
			_urlBuilderService = urlBuilderService;
			_stringFormatterService = stringFormatterService;
			_kmlService = kmlService;
			_vrsEnumService = vrsEnumService;
			_vrsService = vrsService;
		}

		public async Task<MailMessage> Build(Condition condition, Aircraft aircraft, string receiverName,
			bool isDetection)
		{
			var message = new MailMessage();

			//Set subject
			message.Subject = _stringFormatterService.Format(isDetection ? condition.EmailFirstFormat : condition.EmailLastFormat, aircraft, condition);

			//Gather the pieces once so the HTML and plain-text views agree
			var links = BuildLinks(aircraft);
			var photos = _settingsManagerService.EmailContentConfig.AircraftPhotos
				? await _vrsService.GetAircraftThumbnails(aircraft.Icao)
				: Array.Empty<string>();
			var mapUrl = _settingsManagerService.EmailContentConfig.Map && aircraft.GetProperty("Lat") != null
				? _urlBuilderService.GenerateMapboxStaticMapUrl(aircraft)
				: "";

			//HTML body plus a plain-text alternative for notification previews and text-only clients
			var html = BuildHtmlBody(condition, aircraft, receiverName, isDetection, links, photos, mapUrl);
			var text = BuildTextBody(condition, aircraft, receiverName, isDetection, links);

			message.AlternateViews.Add(AlternateView.CreateAlternateViewFromString(text, Encoding.UTF8, MediaTypeNames.Text.Plain));
			message.AlternateViews.Add(AlternateView.CreateAlternateViewFromString(html, Encoding.UTF8, MediaTypeNames.Text.Html));

			//Body is a fallback for clients that ignore alternate views
			message.IsBodyHtml = true;
			message.Body = html;

			//Attach KML
			if (_settingsManagerService.EmailContentConfig.KmlFile && aircraft.GetProperty("Lat") != null)
			{
				var kml = _kmlService.GenerateTrailKml(aircraft);

				message.Attachments.Add(Attachment.CreateAttachmentFromString(kml, aircraft.Icao + ".kml"));
			}

			return message;
		}

		/// <summary>
		/// A link shown in the email
		/// </summary>
		private record EmailLink(string Label, string Url);

		private List<EmailLink> BuildLinks(Aircraft aircraft)
		{
			var config = _settingsManagerService.EmailContentConfig;
			var links = new List<EmailLink>();

			if (config.RadarLink)
			{
				if (!string.IsNullOrWhiteSpace(_settingsManagerService.Settings.RadarUrl))
					links.Add(new EmailLink("Track on VRS", $"{_settingsManagerService.Settings.RadarUrl}?icao={aircraft.Icao}&movingMap=1"));
				links.Add(new EmailLink("airplanes.live", $"https://globe.airplanes.live/?icao={aircraft.Icao}"));
				links.Add(new EmailLink("ADS-B Exchange", $"https://globe.adsbexchange.com/?icao={aircraft.Icao}"));
				links.Add(new EmailLink("OpenSky", $"https://map.opensky-network.org/?icao={aircraft.Icao.ToLower()}"));
			}

			if (config.ReportLink)
			{
				var desktopReportUrl = _urlBuilderService.GenerateReportUrl(aircraft.Icao, false);
				var mobileReportUrl = _urlBuilderService.GenerateReportUrl(aircraft.Icao, true);
				if (!string.IsNullOrEmpty(desktopReportUrl))
				{
					links.Add(new EmailLink("VRS report", desktopReportUrl));
					links.Add(new EmailLink("VRS report (mobile)", mobileReportUrl));
				}
			}

			if (config.AfLookup && !string.IsNullOrEmpty(aircraft.GetProperty("Reg")))
				links.Add(new EmailLink("Airframes.org", _urlBuilderService.GenerateAirframesOrgUrl(aircraft.GetProperty("Reg")!)));

			if (config.Map && aircraft.GetProperty("Lat") != null)
				links.Add(new EmailLink("Google Maps", $"https://www.google.com/maps/search/{aircraft.GetProperty("Lat")},{aircraft.GetProperty("Long")}"));

			return links;
		}

		/// <summary>
		/// The handful of facts shown prominently under the header
		/// </summary>
		private List<(string Label, string Value)> BuildKeyFacts(Aircraft aircraft)
		{
			var facts = new List<(string, string)>();

			void Add(string label, string? value)
			{
				if (!string.IsNullOrWhiteSpace(value))
					facts.Add((label, value));
			}

			Add("Registration", aircraft.GetProperty("Reg"));
			Add("Callsign", aircraft.GetProperty("Call"));
			Add("Type", aircraft.GetProperty("Type"));
			Add("Model", aircraft.GetProperty("Mdl"));
			Add("Operator", aircraft.GetProperty("Op"));

			var altitude = aircraft.GetProperty("Alt");
			if (!string.IsNullOrWhiteSpace(altitude))
				Add("Altitude", altitude + " ft");

			var speed = aircraft.GetProperty("Spd");
			if (!string.IsNullOrWhiteSpace(speed))
				Add("Speed", speed + " kt");

			var squawk = aircraft.GetProperty("Sqk");
			Add("Squawk", squawk);

			var route = string.Join(" → ", new[] { aircraft.GetProperty("From"), aircraft.GetProperty("To") }
				.Where(x => !string.IsNullOrWhiteSpace(x)).Select(x => x!.Split(' ')[0]));
			Add("Route", route);

			return facts;
		}

		private string BuildHtmlBody(Condition condition, Aircraft aircraft, string receiverName, bool isDetection,
			List<EmailLink> links, string[] photos, string mapUrl)
		{
			var config = _settingsManagerService.EmailContentConfig;
			var body = new StringBuilder();

			var title = isDetection ? "First Contact" : "Last Contact";
			var headline = string.Join("  ·  ", new[] { aircraft.GetProperty("Reg"), aircraft.GetProperty("Type"), aircraft.GetProperty("Call") }
				.Where(x => !string.IsNullOrWhiteSpace(x)).Select(E));
			if (headline.Length == 0)
				headline = aircraft.Icao;

			body.Append("<!DOCTYPE html><html><head><meta charset='utf-8'><meta name='viewport' content='width=device-width, initial-scale=1'>");
			body.Append($"<title>{E(title)}: {headline}</title></head>");
			body.Append($"<body style='margin:0;padding:0;background:{Background};font-family:{FontStack};color:{Text};'>");
			body.Append($"<div style='background:{Background};padding:16px 8px;'>");
			body.Append($"<div style='max-width:600px;margin:0 auto;background:{Surface};border:1px solid {Border};border-radius:6px;overflow:hidden;'>");

			//Header band
			body.Append($"<div style='background:{Navy};border-bottom:4px solid {Amber};padding:16px 20px;color:#ffffff;'>");
			body.Append("<div style='font-size:11px;letter-spacing:1.5px;text-transform:uppercase;opacity:0.75;'>Plane Alert &middot; " + E(title) + "</div>");
			body.Append($"<div style='margin-top:4px;font-size:22px;font-weight:600;line-height:1.2;'>{headline}</div>");
			body.Append($"<div style='margin-top:6px;font-size:13px;opacity:0.85;'>Condition: {E(condition.Name)}</div>");
			body.Append("</div>");

			body.Append("<div style='padding:16px 20px;'>");

			//Context line
			var context = new List<string>();
			if (config.ReceiverName)
				context.Add("Receiver: " + E(receiverName));
			if (config.TransponderType)
			{
				var transponderName = _vrsEnumService.TryToString("Trt", aircraft.GetProperty("Trt"), out var convertedTrtValue)
					? convertedTrtValue
					: "Unknown";
				context.Add("Transponder: " + E(transponderName));
			}
			context.Add(DateTime.Now.ToString("ddd d MMM yyyy HH:mm"));
			body.Append($"<div style='color:{TextMuted};font-size:12px;margin-bottom:14px;'>{string.Join(" &nbsp;&middot;&nbsp; ", context)}</div>");

			//Key facts
			var facts = BuildKeyFacts(aircraft);
			if (facts.Count > 0)
			{
				body.Append("<table role='presentation' cellpadding='0' cellspacing='0' style='width:100%;border-collapse:separate;border-spacing:0 6px;margin-bottom:8px;'>");
				for (var i = 0; i < facts.Count; i += 2)
				{
					body.Append("<tr>");
					for (var j = i; j < i + 2; j++)
					{
						if (j < facts.Count)
						{
							body.Append($"<td style='width:50%;padding:8px 10px;background:{SurfaceAlt};border-left:3px solid {Sky};vertical-align:top;'>");
							body.Append($"<div style='font-size:11px;text-transform:uppercase;letter-spacing:0.5px;color:{TextMuted};'>{E(facts[j].Label)}</div>");
							body.Append($"<div style='font-size:15px;font-weight:600;color:{Navy};'>{E(facts[j].Value)}</div>");
							body.Append("</td>");
						}
						else
							body.Append("<td style='width:50%;'></td>");

						if (j == i)
							body.Append("<td style='width:8px;'></td>");
					}
					body.Append("</tr>");
				}
				body.Append("</table>");
			}

			//Links as buttons
			if (links.Count > 0)
			{
				body.Append("<div style='margin:10px 0 16px;'>");
				foreach (var link in links)
				{
					body.Append($"<a href='{E(link.Url)}' style='display:inline-block;margin:0 6px 6px 0;padding:7px 12px;background:{Surface};color:{Sky};border:1px solid {Sky};border-radius:4px;font-size:13px;font-weight:600;text-decoration:none;'>{E(link.Label)}</a>");
				}
				body.Append("</div>");
			}

			//Map
			if (!string.IsNullOrEmpty(mapUrl))
			{
				body.Append($"<img src='{E(mapUrl)}' alt='Map of aircraft position and track' width='560' style='display:block;width:100%;max-width:560px;height:auto;border:1px solid {Border};border-radius:4px;margin-bottom:14px;' />");
			}

			//Photos
			if (photos.Length > 0)
			{
				body.Append("<div style='margin-bottom:14px;'>");
				foreach (var image in photos)
					body.Append($"<img src='{E(image)}' alt='Aircraft photo' style='display:inline-block;max-width:48%;height:auto;margin:0 2% 6px 0;border:1px solid {Border};border-radius:4px;vertical-align:top;' />");
				body.Append("</div>");
			}

			//Property list
			if (config.PropertyList != PropertyListType.Hidden)
				body.Append(BuildPropertiesTable(aircraft, receiverName));

			body.Append("</div>");

			//Footer
			body.Append($"<div style='padding:10px 20px;background:{SurfaceAlt};border-top:1px solid {Border};color:{TextMuted};font-size:11px;'>Sent by PlaneAlerter for Virtual Radar Server</div>");

			body.Append("</div></div></body></html>");

			return body.ToString();
		}

		private string BuildPropertiesTable(Aircraft aircraft, string receiverName)
		{
			var table = new StringBuilder();
			table.Append($"<table id='acTable' cellpadding='0' cellspacing='0' style='width:100%;border:1px solid {Border};border-collapse:collapse;font-size:13px;'>");
			table.Append($"<tr><th colspan='2' style='background:{Navy};color:#ffffff;text-align:left;padding:7px 10px;font-size:12px;letter-spacing:0.5px;text-transform:uppercase;'>Aircraft details</th></tr>");

			var rowIndex = 0;
			foreach (var (name, value) in EnumerateProperties(aircraft, receiverName))
			{
				var rowBackground = rowIndex % 2 == 0 ? Surface : SurfaceAlt;
				table.Append($"<tr style='background:{rowBackground};'>");
				table.Append($"<td style='padding:5px 10px;font-weight:600;color:{Navy};white-space:nowrap;border-bottom:1px solid {Border};width:40%;'>{E(name)}</td>");
				table.Append($"<td style='padding:5px 10px;border-bottom:1px solid {Border};word-break:break-word;'>{E(value)}</td>");
				table.Append("</tr>");
				rowIndex++;
			}
			table.Append("</table>");

			return table.ToString();
		}

		/// <summary>
		/// The properties to list, honouring the All / Essentials setting, as display name and value
		/// </summary>
		private IEnumerable<(string Name, string Value)> EnumerateProperties(Aircraft aircraft, string receiverName)
		{
			var essentialsOnly = _settingsManagerService.EmailContentConfig.PropertyList == PropertyListType.Essentials;

			foreach (var propertyKey in aircraft.GetPropertyKeys())
			{
				var property = VrsProperties.VrsPropertyData.Where(x => x.Value[2] == propertyKey)
					.Select(x => (VrsProperty?)x.Key).FirstOrDefault();

				string propertyName;

				if (property != null)
				{
					if (essentialsOnly && !VrsProperties.EssentialProperties.Contains(property.Value))
						continue;

					propertyName = property.Value.ToString();
				}
				else
				{
					if (essentialsOnly)
						continue;

					//Set parameter to a readable name if it's not included in vrs property info
					switch (propertyKey)
					{
						case "FSeen":
							propertyName = "First_Seen";
							break;
						case "HasPic":
							propertyName = "Has_Picture";
							break;
						case "Id":
							propertyName = "Id";
							break;
						case "PosTime":
							propertyName = "Position_Time";
							break;
						case "TT":
						case "ResetTrail":
							continue;
						default:
							propertyName = propertyKey;
							break;
					}
				}

				var value = aircraft.GetProperty(propertyKey) ?? "";

				//Add string conversions of enum values
				if (_vrsEnumService.TryToString(propertyKey, value, out var convertedValue))
					value += " (" + convertedValue + ")";

				//If rcvr, add receiver name
				if (propertyKey == "Rcvr")
					value += " (" + receiverName + ")";

				yield return (propertyName.Replace('_', ' '), value);
			}
		}

		private string BuildTextBody(Condition condition, Aircraft aircraft, string receiverName, bool isDetection, List<EmailLink> links)
		{
			var text = new StringBuilder();
			text.AppendLine($"PLANE ALERT - {(isDetection ? "FIRST CONTACT" : "LAST CONTACT")}");
			text.AppendLine($"Condition: {condition.Name}");
			if (_settingsManagerService.EmailContentConfig.ReceiverName)
				text.AppendLine($"Receiver: {receiverName}");
			text.AppendLine();

			foreach (var (label, value) in BuildKeyFacts(aircraft))
				text.AppendLine($"{label}: {value}");

			if (links.Count > 0)
			{
				text.AppendLine();
				foreach (var link in links)
					text.AppendLine($"{link.Label}: {link.Url}");
			}

			if (_settingsManagerService.EmailContentConfig.PropertyList != PropertyListType.Hidden)
			{
				text.AppendLine();
				text.AppendLine("Aircraft details");
				text.AppendLine("----------------");
				foreach (var (name, value) in EnumerateProperties(aircraft, receiverName))
					text.AppendLine($"{name}: {value}");
			}

			text.AppendLine();
			text.AppendLine("Sent by PlaneAlerter for Virtual Radar Server");
			return text.ToString();
		}

		/// <summary>
		/// HTML-encode a value from VRS before placing it in the email
		/// </summary>
		private static string E(string? value) => WebUtility.HtmlEncode(value ?? "");
	}
}
