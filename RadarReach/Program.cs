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
		public static void Main(string[] args)
		{
			#region ForQuickTestPurpose

			Location radarLocation = new Location() { Latitude = 0, Longitude = -70 };
			Location topLeftLocation = new Location() { Latitude = 30, Longitude = -50 };
			Location topRightLocation = new Location() { Latitude = 30, Longitude = 50 };
			Location bottomLeftLocation = new Location() { Latitude = -30, Longitude = -50 };
			Location bottomRightLocation = new Location() { Latitude = -30, Longitude = 50 };
			double radarReachInDegrees = 10;

			bool result = IsRadarReachIntersectViewArea(radarLocation, radarReachInDegrees,
				topLeftLocation, topRightLocation,
				bottomLeftLocation, bottomRightLocation);

			Console.WriteLine(result);

			#endregion
		}

		public static bool IsRadarReachIntersectViewArea(Location radar, double radarReach,
			Location topLeft, Location topRight,
			Location bottomLeft, Location bottomRight)
		{
			if (IsRadarInsideViewArea(radar, topLeft.Longitude, topRight.Longitude,
					bottomLeft.Latitude, topLeft.Latitude))
			{
				return true;
			}

			bool? result;

			result = IsRadarReachIntersectsWhenAboveMaxLatitude(radar, radarReach,
				    topLeft.Latitude, topLeft.Longitude, topRight.Longitude);
			if (result.HasValue)
				return result.Value;

			result = IsRadarReachIntersectsWhenBelowMinLatitude(radar, radarReach,
				    bottomLeft.Latitude, bottomLeft.Longitude, bottomRight.Longitude);
			if (result.HasValue)
				return result.Value;

			result = IsRadarReachIntersectsWhenBelowMinLongitude(radar, radarReach,
				    topLeft.Longitude);
			if (result.HasValue)
				return result.Value;

			result = IsRadarReachIntersectsWhenAboveMaxLongitude(radar, radarReach,
					topRight.Longitude);
			if (result.HasValue)
				return result.Value;

			return false;
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

		#region Refactoring will reduce the code by 2 times
		private static bool? IsRadarReachIntersectsWhenAboveMaxLatitude(Location radar, double radarReach,
			double maxLatitude, double minLongitude, double maxLongitude)
		{
			if (radar.Latitude < maxLatitude)
				return null;

			double distance;

			if (radar.Longitude < minLongitude)
			{
				distance = CalcDistance(radar.Latitude, radar.Longitude,
					maxLatitude, minLongitude);

				return radarReach - distance > 0;
			}

			if (radar.Longitude > maxLongitude)
			{
				distance = CalcDistance(radar.Latitude, radar.Longitude,
					maxLatitude, maxLongitude);

				return radarReach - distance > 0;
			}

			distance = CalcDistance(radar.Latitude, radar.Longitude,
				maxLatitude, radar.Longitude);

			return radarReach - distance > 0;
		}

		private static bool? IsRadarReachIntersectsWhenBelowMinLatitude(Location radar, double radarReach,
			double minLatitude, double minLongitude, double maxLongitude)
		{
			if (radar.Latitude > minLatitude)
				return null;

			double distance;

			if (radar.Longitude < minLongitude)
			{
				distance = CalcDistance(radar.Latitude, radar.Longitude,
					minLatitude, minLongitude);

				return radarReach - distance > 0;
			}

			if (radar.Longitude > maxLongitude)
			{
				distance = CalcDistance(radar.Latitude, radar.Longitude,
					minLatitude, maxLongitude);

				return radarReach - distance > 0;
			}

			distance = CalcDistance(radar.Latitude, radar.Longitude,
				minLatitude, radar.Longitude);

			return radarReach - distance > 0;
		}

		private static bool? IsRadarReachIntersectsWhenBelowMinLongitude(Location radar, double radarReach,
			double minLongitude)
		{
			if (radar.Longitude > minLongitude)
				return null;

			var distance = CalcDistance(radar.Latitude, radar.Longitude,
				radar.Latitude, minLongitude);

			return radarReach - distance > 0;
		}

		private static bool? IsRadarReachIntersectsWhenAboveMaxLongitude(Location radar, double radarReach,
			double maxLongitude)
		{
			if (radar.Longitude < maxLongitude)
				return null;

			var distance = CalcDistance(radar.Latitude, radar.Longitude,
				radar.Latitude, maxLongitude);

			return radarReach - distance > 0;
		}
		#endregion

		// GeoHelper class has this kind of calculation.
		// This method calculate distance in degree units only for the testing purposes.
		public static double CalcDistance(double latPoint1, double lonPoint1,
			double latPoint2, double lonPoint2)
		{
			var longitudeProjectionLength = Math.Abs(Math.Abs(lonPoint1) - Math.Abs(lonPoint2));
			var latitudeProjectionLength = Math.Abs(Math.Abs(latPoint1) - Math.Abs(latPoint2));
			var distance = Math.Sqrt(
				Math.Pow(longitudeProjectionLength, 2) + Math.Pow(latitudeProjectionLength , 2));

			return distance;
		}
	}
}
