using System;
using System.Globalization;

namespace PlaneAlerter
{
	/// <summary>
	/// Version of this build: the time of the last pushed commit. CommitUnixTime and FromCommit are
	/// generated at build time by the StampCommitVersion target in PlaneAlerter.csproj.
	/// </summary>
	internal static partial class BuildInfo
	{
		/// <summary>
		/// Commit time in local time, e.g. "November 18, 2026  09:23"
		/// </summary>
		public static string Version
		{
			get
			{
				var time = DateTimeOffset.FromUnixTimeSeconds(CommitUnixTime).ToLocalTime();
				return time.ToString("MMMM d, yyyy  HH:mm", CultureInfo.InvariantCulture);
			}
		}
	}
}
