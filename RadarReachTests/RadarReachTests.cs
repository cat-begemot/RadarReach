using Microsoft.VisualStudio.TestTools.UnitTesting;
using RadarReach;
using System;

namespace RadarReachTests
{
	[TestClass]
    public class RadarReachTests
    {
        [DataTestMethod]
        [DataRow(0, 0, 0, 0, 0)]
		[DataRow(-70, -50, 0, 0, 20)]
		[DataRow(-50, -70, 0, 0, 20)]
        [DataRow(70, 50, -70, -30, 45)]
        [DataRow(50, 0, 30, 0, 58)]
        [DataRow(-70, 50, 0, 0, 120)]
        public void TestCalcDistanceMethod_UsingRoundedResult(double lon1, double lon2,
	        double lat1, double lat2, double expectedResult)
        {
			var actualResult = Math.Round(Program.CalcDistance(lat1, lon1, lat2, lon2));

			Assert.AreEqual(expectedResult, actualResult);
		}

		[DataTestMethod]
		// Above maxLatitude
		[DataRow(-70, 45, 25.1)] 
        [DataRow(0, 45, 15.1)]
        [DataRow(70, 45, 25.1)]
		// Above maxLongitude
		[DataRow(70, 0, 20.1)]
		// Below minLatitude
		[DataRow(70, -45, 25.1)] 
		[DataRow(0, -45, 15.1)]
		[DataRow(-70, 45, 25.1)]
		// Below minLongitude
		[DataRow(-70, 0, 20.1)] 
		public void TestIntersection_RadarReachIntersectsViewArea_ReturnsTrue(
			double radar_lon, double radar_lat, double radarReachInDegrees)
        {
	        var radar = new Location() { Latitude = radar_lat, Longitude = radar_lon };
	        var topLeft = new Location() { Latitude = 30, Longitude = -50 };
	        var topRight = new Location() { Latitude = 30, Longitude = 50 };
	        var bottomLeft = new Location() { Latitude = -30, Longitude = -50 };

			var actualResult = Program.IsRadarReachIntersectViewArea(radar, radarReachInDegrees,
				topLeft, topRight, bottomLeft);

            Assert.IsTrue(actualResult);
        }

		[DataTestMethod]
		// Above maxLatitude
		[DataRow(-70, 45, 24.9)]
		[DataRow(0, 45, 14.9)]
		[DataRow(70, 45, 24.9)]
		// Above maxLongitude
		[DataRow(70, 0, 19.9)]
		// Below minLatitude
		[DataRow(70, -45, 24.9)]
		[DataRow(0, -45, 14.9)]
		[DataRow(-70, 45, 24.9)]
		// Below minLongitude
		[DataRow(-70, 0, 19.9)]
		public void TestIntersection_RadarReachDoesNotIntersectViewArea_ReturnsFalse(
			double radar_lon, double radar_lat, double radarReachInDegrees)
		{
			var radar = new Location() { Latitude = radar_lat, Longitude = radar_lon };
			var topLeft = new Location() { Latitude = 30, Longitude = -50 };
			var topRight = new Location() { Latitude = 30, Longitude = 50 };
			var bottomLeft = new Location() { Latitude = -30, Longitude = -50 };

			var actualResult = Program.IsRadarReachIntersectViewArea(radar, radarReachInDegrees,
				topLeft, topRight, bottomLeft);

			Assert.IsFalse(actualResult);
		}
	}
}
