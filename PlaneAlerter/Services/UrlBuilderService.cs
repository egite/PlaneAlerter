using System.Collections.Generic;
using System.Drawing;
using System.Net;
using PlaneAlerter.Helpers;
using PlaneAlerter.Models;

namespace PlaneAlerter.Services
{
	internal interface IUrlBuilderService
	{
		/// <summary>
		/// Generate the report url for a specific icao
		/// </summary>
		string GenerateReportUrl(string icao, bool mobile);
		string GenerateMapboxStaticMapUrl(Aircraft aircraft);

		/// <summary>
		/// Generate a link to airframes.org
		/// </summary>
		/// <param name="reg">Aircraft registration</param>
		string GenerateAirframesOrgUrl(string reg);
	}

	internal class UrlBuilderService : IUrlBuilderService
	{
		private readonly ISettingsManagerService _settingsManagerService;
		private readonly ILoggerWithQueue _logger;
		
		private const string MapboxAccessToken = "pk.eyJ1IjoiZG9tMjM2NCIsImEiOiJjbTllNHMwdzAxOGI4MmtvZTZhMGpmMnp2In0.jmjmGSwa0p9bM4YCG_xewQ";

		public UrlBuilderService(ISettingsManagerService settingsManagerService, ILoggerWithQueue logger)
		{
			_settingsManagerService = settingsManagerService;
			_logger = logger;
		}

		/// <summary>
		/// Generate the report url for a specific icao
		/// </summary>
		public string GenerateReportUrl(string icao, bool mobile)
		{
			var reportUrl = "";
			if (string.IsNullOrWhiteSpace(_settingsManagerService.Settings.RadarUrl))
			{
				_logger.Log("ERROR: Please enter radar URL in settings", Color.Red);
				return "";
			}
			if (!_settingsManagerService.Settings.RadarUrl.ToLower().Contains("virtualradar"))
			{
				_logger.Log("WARNING: Radar URL must end with /VirtualRadar/ for report links to work", Color.Orange);
				return "";
			}


			reportUrl += _settingsManagerService.Settings.RadarUrl;
			if (_settingsManagerService.Settings.RadarUrl[_settingsManagerService.Settings.RadarUrl.Length - 1] != '/') reportUrl += "/";

			if (mobile) reportUrl += "mobileReport.html?sort1=date&sortAsc1=0&icao-Q=" + icao;
			else reportUrl += "desktopReport.html?sort1=date&sortAsc1=0&icao-Q=" + icao;

			return reportUrl;
		}

		public string GenerateMapboxStaticMapUrl(Aircraft aircraft)
		{
			var aircraftLat = aircraft.GetProperty("Lat");
			var aircraftLong = aircraft.GetProperty("Long");
            
			if (aircraftLat == null || aircraftLong == null)
				return string.Empty;
            
			var showTrail = aircraft.Trail.Length > 4;
			
			var centreLat = _settingsManagerService.Settings.CentreMapOnAircraft
				? aircraftLat
				: _settingsManagerService.Settings.Lat.ToString("#.####");
			
			var centreLong = _settingsManagerService.Settings.CentreMapOnAircraft
				? aircraftLong
				: _settingsManagerService.Settings.Long.ToString("#.####");
			
			var bounds = showTrail || !_settingsManagerService.Settings.CentreMapOnAircraft
				? "auto"
				: $"{centreLong},{centreLat},8,0";

			var overlays = new List<string>
			{
				//pin-s marker
				//#555555 fill colour
				$"pin-s+555555({aircraftLong},{aircraftLat})"
			};

			if (!_settingsManagerService.Settings.CentreMapOnAircraft)
			{
				//pin-s marker
				//#4275f5 fill colour
				overlays.Add($"pin-s+4275f5({centreLong},{centreLat})");
			}
			
			if (showTrail)
			{
				var trailPoints = new List<double[]>();

				//Process entire aircraft trail (oldest to newest)
				for (var i = 0; i < aircraft.Trail.Length / 4; i++)
				{
					var lat = aircraft.Trail[i * 4] ?? 0;
					var lon = aircraft.Trail[i * 4 + 1] ?? 0;

					trailPoints.Add(new[]{lat,lon});
				}

				//Mapbox static map URL limit is 8192 bytes. Trim oldest points if the URL would be too long.
				const int maxUrlLength = 8100;
				string encodedPolyline;
				while (true)
				{
					encodedPolyline = WebUtility.UrlEncode(GooglePolylineEncodingHelper.Encode(trailPoints));
					var estimatedUrlLength = $"https://api.mapbox.com/styles/v1/mapbox/streets-v12/static/pin-s+555555({aircraftLong},{aircraftLat}),path({encodedPolyline})/{bounds}/800x800?access_token={MapboxAccessToken}&padding=100".Length;

					if (estimatedUrlLength <= maxUrlLength || trailPoints.Count <= 2)
						break;

					//Remove oldest points (from the start) to shorten the URL
					var removeCount = trailPoints.Count / 10;
					if (removeCount < 1) removeCount = 1;
					trailPoints.RemoveRange(0, removeCount);
				}

				overlays.Add($"path({encodedPolyline})");
			}
            
			var staticMapUrl = $"https://api.mapbox.com/styles/v1/mapbox/streets-v12/static/{string.Join(',', overlays)}/{bounds}/800x800?access_token={MapboxAccessToken}";

			if (bounds == "auto")
				staticMapUrl += "&padding=100";
			
			return staticMapUrl;
		}

		public string GenerateAirframesOrgUrl(string reg)
		{
			return $"http://www.airframes.org/reg/{reg.Replace("-", "").ToUpper()}";
		}
	}
}
