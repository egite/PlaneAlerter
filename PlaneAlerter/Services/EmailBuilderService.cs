using System.Linq;
using System.Net.Mail;
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

		private const string LinkStyle = "color:" + Sky + ";font-weight:600;text-decoration:none;";

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

			//Set message type to html
			message.IsBodyHtml = true;

			//Set subject
			message.Subject = _stringFormatterService.Format(isDetection ? condition.EmailFirstFormat : condition.EmailLastFormat, aircraft, condition);

			//Build body
			message.Body = await BuildBody(condition, aircraft, receiverName, isDetection);

			//Attach KML
			if (_settingsManagerService.EmailContentConfig.KmlFile && aircraft.GetProperty("Lat") != null)
			{
				var kml = _kmlService.GenerateTrailKml(aircraft);

				message.Attachments.Add(Attachment.CreateAttachmentFromString(kml, aircraft.Icao + ".kml"));
			}

			return message;
		}

		private async Task<string> BuildBody(Condition condition, Aircraft aircraft, string receiverName,
			bool isDetection)
		{
			var config = _settingsManagerService.EmailContentConfig;
			var body = new StringBuilder();

			body.Append("<!DOCTYPE html><html><head><meta charset='utf-8'><meta name='viewport' content='width=device-width, initial-scale=1'></head>");
			body.Append($"<body style='margin:0;padding:16px;background:{Background};font-family:Segoe UI,Helvetica,Arial,sans-serif;color:{Text};'>");
			body.Append($"<div style='max-width:960px;margin:0 auto;background:{Surface};border:1px solid {Border};'>");

			//Header band
			body.Append($"<div style='background:{Navy};border-bottom:4px solid {Amber};padding:14px 18px;color:#ffffff;'>");
			body.Append("<div style='font-size:12px;letter-spacing:1px;text-transform:uppercase;opacity:0.8;'>Plane Alert</div>");
			body.Append($"<h1 style='margin:2px 0 0;font-size:22px;font-weight:600;'>{(isDetection ? "First Contact" : "Last Contact")}</h1>");
			body.Append("</div>");

			body.Append("<div style='padding:14px 18px;'>");

			//Condition name
			body.Append($"<h2 style='margin:0 0 6px;font-size:17px;color:{Navy};'>Condition: {condition.Name}</h2>");

			//Receiver name
			if (config.ReceiverName)
				body.Append($"<div style='color:{TextMuted};font-size:13px;margin-bottom:2px;'>Receiver: {receiverName}</div>");

			//Transponder type
			if (config.TransponderType)
			{
				var transponderName = _vrsEnumService.TryToString("Trt",
					aircraft.GetProperty("Trt"), out var convertedTrtValue)
					? convertedTrtValue
					: "Unknown";

				body.Append($"<div style='color:{TextMuted};font-size:13px;margin-bottom:2px;'>Transponder: {transponderName}</div>");
			}

			//Radar urls
			if (config.RadarLink)
			{
				body.Append($"<p style='margin:12px 0 4px;font-size:14px;'><b>Track live on:</b> " +
					$"<a style='{LinkStyle}' href='{_settingsManagerService.Settings.RadarUrl}?icao={aircraft.Icao}&movingMap=1'>VRS</a> &middot; " +
					$"<a style='{LinkStyle}' href='https://globe.airplanes.live/?icao={aircraft.Icao}'>airplanes.live</a> &middot; " +
					$"<a style='{LinkStyle}' href='https://globe.adsbexchange.com/?icao={aircraft.Icao}'>ADS-B Exchange</a> &middot; " +
					$"<a style='{LinkStyle}' href='https://map.opensky-network.org/?icao={aircraft.Icao.ToLower()}'>OpenSky</a></p>");
			}

			//Report url
			if (config.ReportLink)
			{
				var desktopReportUrl = _urlBuilderService.GenerateReportUrl(aircraft.Icao, false);
				var mobileReportUrl = _urlBuilderService.GenerateReportUrl(aircraft.Icao, true);

				if (!string.IsNullOrEmpty(desktopReportUrl))
					body.Append($"<p style='margin:4px 0;font-size:14px;'><b>VRS Report:</b> " +
						$"<a style='{LinkStyle}' href='{desktopReportUrl}'>Desktop</a> &middot; " +
						$"<a style='{LinkStyle}' href='{mobileReportUrl}'>Mobile</a></p>");
			}

			//Airframes.org url
			if (config.AfLookup && !string.IsNullOrEmpty(aircraft.GetProperty("Reg")))
				body.Append($"<p style='margin:4px 0;font-size:14px;'><a style='{LinkStyle}' href='{_urlBuilderService.GenerateAirframesOrgUrl(aircraft.GetProperty("Reg")!)}'>Airframes.org Lookup</a></p>");

			body.Append("<table style='border-collapse:collapse;margin-top:10px;' cellpadding='0' cellspacing='0'><tr><td style='vertical-align:top;'>");

			//Property list
			if (config.PropertyList != PropertyListType.Hidden)
				body.Append(BuildPropertiesTable(aircraft, receiverName));

			body.Append("</td><td style='padding-left:14px;vertical-align:top;'>");

			//Aircraft photos
			if (config.AircraftPhotos)
			{
				var aircraftImages = await _vrsService.GetAircraftThumbnails(aircraft.Icao);

				foreach (var image in aircraftImages)
					body.Append($"<img style='margin:0 6px 6px 0;border:1px solid {Border};max-width:100%;height:auto;' src='{image}' alt='Aircraft photo' />");

				if (aircraftImages.Any())
					body.Append("<br>");
			}

			//Map
			if (config.Map && aircraft.GetProperty("Lat") != null)
			{
				var googleMapsUrl =
					$"https://www.google.com/maps/search/{aircraft.GetProperty("Lat")},{aircraft.GetProperty("Long")}";

				var mapUrl = _urlBuilderService.GenerateMapboxStaticMapUrl(aircraft);

				body.Append($"<p style='margin:6px 0;font-size:14px;'><a style='{LinkStyle}' href='{googleMapsUrl}'>Open in Google Maps</a></p>");

				if (!string.IsNullOrEmpty(mapUrl))
					body.Append($"<img style='border:1px solid {Border};max-width:100%;height:auto;' alt='Map of aircraft position and track' src='{mapUrl}' />");
			}

			body.Append("</td></tr></table>");
			body.Append("</div></div></body></html>");

			return body.ToString();
		}

		private string BuildPropertiesTable(Aircraft aircraft, string receiverName)
		{
			var table = new StringBuilder();
			table.Append($"<table id='acTable' style='border:1px solid {Border};border-collapse:collapse;font-size:13px;' cellpadding='0' cellspacing='0'>");
			table.Append($"<tr><th colspan='2' style='background:{Navy};color:#ffffff;text-align:left;padding:6px 10px;font-size:13px;letter-spacing:0.5px;'>Aircraft Details</th></tr>");

			var aircraftPropertyKeys = aircraft.GetPropertyKeys();
			var rowIndex = 0;
			foreach (var propertyKey in aircraftPropertyKeys)
			{
				var property = VrsProperties.VrsPropertyData.Where(x => x.Value[2] == propertyKey)
					.Select(x => (VrsProperty?)x.Key).FirstOrDefault();

				string propertyName;

				if (property != null)
				{
					//If property list type is essentials and this property is not in the list of essentials then skip
					if (_settingsManagerService.EmailContentConfig.PropertyList == PropertyListType.Essentials &&
					    !VrsProperties.EssentialProperties.Contains(property.Value))
						continue;

					propertyName = property.Value.ToString();
				}
				else
				{
					//If property list type is essentials then skip
					if (_settingsManagerService.EmailContentConfig.PropertyList == PropertyListType.Essentials)
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

				//Get value
				var value = aircraft.GetProperty(propertyKey);

				//Add string conversions of enum values
				if (_vrsEnumService.TryToString(propertyKey, value, out var convertedValue))
				{
					value += " (" + convertedValue + ")";
				}

				//If rcvr, add receiver name
				if (propertyKey == "Rcvr")
				{
					value += " (" + receiverName + ")";
				}

				//Add html for property
				var rowBackground = rowIndex % 2 == 0 ? Surface : SurfaceAlt;
				table.Append($"<tr style='background:{rowBackground};border-bottom:1px solid {Border};'>");
				table.Append($"<td style='padding:4px 10px;font-weight:600;color:{Navy};white-space:nowrap;'>{propertyName.Replace('_', ' ')}</td>");
				table.Append($"<td style='padding:4px 10px;'>{value}</td>");
				table.Append("</tr>");
				rowIndex++;
			}
			table.Append("</table>");

			return table.ToString();
		}
	}
}
