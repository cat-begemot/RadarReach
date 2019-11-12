using System;
using System.Xml.XPath;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RadarReach;

namespace RadarReachTests
{
    [TestClass]
    public class UnitTest1
    {
        [DataTestMethod]
        [DataRow(0, 0, 0, 0, 0)]
		[DataRow(-70, -50, 0, 0, 20)]
		[DataRow(-50, -70, 0, 0, 20)]
        [DataRow(70, 50, -70, -30, 45)]
        [DataRow(50, 0, 30, 0, 58)]
        public void TestCalcDistanceMethod(double lon1, double lon2, double lat1, double lat2, double expectedResult)
        {
			double actualResult = Math.Round(Program.CalcDistance(lat1, lon1, lat2, lon2));

			Assert.AreEqual(expectedResult, actualResult);
		}

		[DataTestMethod]
//        [DataRow(0, 0, 10)]
//        [DataRow(0, 0, 100)]
//        [DataRow(-70, 29, 21)]
//        [DataRow(-70, 30, 21)]
        [DataRow(-70, 45, 25.1)]
        public void TestIntersection_General_ReturnsTrue(double radar_lon, double radar_lat, double radarReachInDegrees)
        {
            Location radarLocation = new Location() { Latitude = radar_lat, Longitude = radar_lon };
			Location topLeftLocation = new Location() { Latitude = 30, Longitude = -50 };
			Location topRightLocation = new Location() { Latitude = 30, Longitude = 50 };
			Location bottomLeftLocation = new Location() { Latitude = -30, Longitude = -50 };
			Location bottomRightLocation = new Location() { Latitude = -30, Longitude = 50 };

            bool actualResult = Program.IsRadarReachIntersectViewArea(radarLocation, radarReachInDegrees,
				topLeftLocation, topRightLocation,
				bottomLeftLocation, bottomRightLocation);

            Assert.IsTrue(actualResult);
        }

//        [DataTestMethod]
//        public void TestIntersection_RoundedLongitude(double radar_lon, double radar_lat, double radarReachInDegrees)
//        {
//
//        }
    }
}
