﻿using System;
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

			if (radar.Longitude < minLongitude)
				return IsRadarReachIntersectsRelativeToExtremeLongitude(radar, radarReach,
							minLatitude, maxLatitude, minLongitude);

			if (radar.Longitude > maxLongitude)
				return IsRadarReachIntersectsRelativeToExtremeLongitude(radar, radarReach,
							minLatitude, maxLatitude, maxLongitude);

			if (radar.Latitude < minLatitude)
				return IsRadarReachIntersectsRelativeToExtremeLatitude(radar, radarReach, minLatitude);

			if (radar.Latitude > maxLatitude)
				return IsRadarReachIntersectsRelativeToExtremeLatitude(radar, radarReach, maxLatitude);

			return IsRadarInsideViewArea(radar, minLongitude, maxLongitude, minLatitude, maxLatitude);
		}

		private static bool IsRadarReachIntersectsRelativeToExtremeLongitude(Location radar, double radarReach,
			double minLatitude, double maxLatitude, double longitude)
		{
			double distance;

			if (radar.Latitude < minLatitude)
			{
				distance = CalcDistance(radar.Latitude, radar.Longitude,
					minLatitude, longitude);

				return radarReach - distance > 0;
			}

			if (radar.Latitude > maxLatitude)
			{
				distance = CalcDistance(radar.Latitude, radar.Longitude,
					maxLatitude, longitude);

				return radarReach - distance > 0;
			}

			distance = CalcDistance(radar.Latitude, radar.Longitude,
				radar.Latitude, longitude);

			return radarReach - distance > 0;
		}

		private static bool IsRadarReachIntersectsRelativeToExtremeLatitude(Location radar, double radarReach,
			double latitude)
		{
			var distance = CalcDistance(radar.Latitude, radar.Longitude,
				latitude, radar.Longitude);

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
			var longitudeProjectionLength = Math.Abs(Math.Abs(lonPoint1) - Math.Abs(lonPoint2));
			var latitudeProjectionLength = Math.Abs(Math.Abs(latPoint1) - Math.Abs(latPoint2));
			var distance = 
				Math.Sqrt(Math.Pow(longitudeProjectionLength, 2) + Math.Pow(latitudeProjectionLength , 2));

			return distance;
		}
	}
}
