using System;
using System.Globalization;
using System.Transactions;

namespace RadarReach
{
	public struct Location
	{
		public double Latitude;
		public double Longitude;
	}
	
	public class Program
	{
		public static void Main(string[] args) { }

		public static bool IsRadarReachIntersectViewArea(Location radar, double radarReach,
			Location topLeft, Location topRight, Location bottomLeft)
		{
			var minLongitude = topLeft.Longitude;
			var maxLongitude = topRight.Longitude;
			
			var minLatitude = bottomLeft.Latitude;
			var maxLatitude = topLeft.Latitude;

			// Checks radar position relative to the left side of the view area.
			if (radar.Longitude < minLongitude)
				return IsRadarReachIntersectsRelativeToExtremeLongitude(radar, radarReach,
							minLatitude, maxLatitude, minLongitude);

			// Checks radar position relative to the right side of the view area.
			if (radar.Longitude > maxLongitude)
				return IsRadarReachIntersectsRelativeToExtremeLongitude(radar, radarReach,
							minLatitude, maxLatitude, maxLongitude);

			// Checks radar position relative to the bottom side of the view area.
			if (radar.Latitude < minLatitude)
				return IsRadarReachIntersectsRelativeToExtremeLatitude(radar, radarReach, minLatitude);

			// Checks radar position relative to the top side of the view area.
			if (radar.Latitude > maxLatitude)
				return IsRadarReachIntersectsRelativeToExtremeLatitude(radar, radarReach, maxLatitude);

			// Checks whether radar locates in side ViewArea.
			return IsRadarInsideViewArea(radar, minLongitude, maxLongitude, minLatitude, maxLatitude);
		}

		private static bool IsRadarReachIntersectsRelativeToExtremeLongitude(Location radar, double radarReach,
			double minLatitude, double maxLatitude, double longitude)
		{
			if (radar.Latitude < minLatitude)
				return IsRadarReachGreaterThanDistanceToViewArea(radar, radarReach, minLatitude, longitude);

			if (radar.Latitude > maxLatitude)
				return IsRadarReachGreaterThanDistanceToViewArea(radar, radarReach, maxLatitude, longitude);

			return IsRadarReachGreaterThanDistanceToViewArea(radar, radarReach, radar.Latitude, longitude);
		}

		private static bool IsRadarReachIntersectsRelativeToExtremeLatitude(Location radar,
			double radarReach, double latitude) =>
				IsRadarReachGreaterThanDistanceToViewArea(radar, radarReach, latitude, radar.Longitude);
		
		private static bool IsRadarReachGreaterThanDistanceToViewArea(Location radar, double radarReach,
			double latitude, double longitude)
		{
			var distance = CalcDistance(radar.Latitude, radar.Longitude,
				latitude, longitude);

			return radarReach - distance > 0;
		}

		private static bool IsRadarInsideViewArea(Location radar,
			double minLongitude, double maxLongitude,
			double minLatitude, double maxLatitude)
		{
			return radar.Longitude > minLongitude &&
			       radar.Longitude < maxLongitude &&
			       radar.Latitude < maxLatitude &&
			       radar.Latitude > minLatitude;
		}

		// GeoHelper class has this kind of calculation.
		// This method calculate distance in degree units only for the testing purposes.
		public static double CalcDistance(double latPoint1, double lonPoint1,
			double latPoint2, double lonPoint2)
		{
			var longitudeProjectionLength = Math.Abs(lonPoint1 - lonPoint2);
			var latitudeProjectionLength = Math.Abs(latPoint1 - latPoint2);
			var distance = 
				Math.Sqrt(Math.Pow(longitudeProjectionLength, 2) + Math.Pow(latitudeProjectionLength , 2));

			return distance;
		}
	}
}
