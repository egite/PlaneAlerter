using System;
using System.IO;
using Newtonsoft.Json;

namespace PlaneAlerter.Models
{
	/// <summary>
	/// Persisted size and pane positions of the main window
	/// </summary>
	internal class WindowLayout
	{
		private const string FileName = "windowlayout.json";

		public int Width { get; set; }
		public int Height { get; set; }
		public bool Maximized { get; set; }
		public int LeftPaneWidth { get; set; }
		public int ConditionsPaneHeight { get; set; }

		public static WindowLayout? Load()
		{
			try
			{
				if (!File.Exists(FileName))
					return null;

				return JsonConvert.DeserializeObject<WindowLayout>(File.ReadAllText(FileName));
			}
			catch (Exception)
			{
				return null;
			}
		}

		public void Save()
		{
			try
			{
				File.WriteAllText(FileName, JsonConvert.SerializeObject(this, Formatting.Indented));
			}
			catch (Exception)
			{
				//Layout persistence is best-effort
			}
		}
	}
}
