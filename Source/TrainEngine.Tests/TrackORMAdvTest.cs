using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrainEngine.Tracks;
using TrainEngine.Trains;
using Xunit;

namespace TrainEngine.Tests
{
    public class TrackORMAdvTest
    {
        public string advTrackMap =
            @"                           ---=[3]---                       ;" +
            @"              -----[2]-=--<          \                      ;" +
            @"             /             \          >--[6]                ;" +
            @"            /               \        /                      ;" +
            @"      -=---<                 >---=--<                       ;" +
            @"*[1]-<      \               /        ---[5]                 ;" +
            @"      \      \             /                                ;" +
            @"       \      ---=-[4]----<        ---=-----=------[9]      ;" +
            @"        \                  \      /                         ;" +
            @"         \                  \-=--<           ---[8]         ;" +
            @"          \                       \         /               ;" +
            @"           \  ---=----=---[10]     ---[7]--<                ;" +
            @"            -<                              \      ---[17]  ;" +
            @"              \           ---[12]            \    /         ;" +
            @"               \--[13]---<                    ---<          ;" +
            @"                          \                       \         ;" +
            @"                           ---[14]---[15]---[16]   ----[18] ;";

        [Fact]
        public void When_Two_Station_Provided()
        {
            // Arrange
            string track = "*[1]---[2]";
            var trackOrm = new TrackORMAdv(track, false);

            // Act
            var result = trackOrm.Tracks[0];

            // Assert
            Assert.IsType<Station>(result.StartStation);
            Station startStation = (Station)result.StartStation;
            Assert.Equal(1, startStation.Id);

            Assert.IsType<Station>(result.EndStation);
            Station endStation = (Station)result.StartStation;
            Assert.Equal(1, endStation.Id);
        }

        [Fact]
        public void When_Tre_Station_Provided_With_2digit_Id()
        {
            // Arrange
            string tracks = "*[11]---[22]----[33]";
            var trackOrm = new TrackORMAdv(tracks, false);

            // Act
            var result = trackOrm.Tracks;

            // Assert
            Assert.IsType<Station>(result[0].StartStation);
            Station startStation1 = (Station)result[0].StartStation;
            Assert.Equal(11, startStation1.Id);

            Assert.IsType<Station>(result[0].EndStation);
            Station endStation1 = (Station)result[0].EndStation;
            Assert.Equal(22, endStation1.Id);

            Assert.IsType<Station>(result[1].StartStation);
            Station startStation2 = (Station)result[1].StartStation;
            Assert.Equal(22, startStation2.Id);

            Assert.IsType<Station>(result[1].EndStation);
            Station endStation2 = (Station)result[1].EndStation;
            Assert.Equal(33, endStation2.Id);
        }

        [Fact]
        public void When_Twenty_Station_Provided()
        {
            // Arrange
            string tracks = "*[1]-[2]-[3]-[4]-[5]-[6]-[7]-[8]-[9]-[10]-" +
                "[11]-[12]-[13]-[14]-[15]-[16]-[17]-[18]-[19]-[20]";
            var trackOrm = new TrackORMAdv(tracks, false);

            // Act
            var result = trackOrm.Tracks;

            // Assert
            Assert.Equal(19, result.Count);
        }

        [Fact]
        public void When_Advanced_Track_Map_Provided_Get_19_Tracks()
        {
            // Arrange
            string tracks = advTrackMap;
            var trackOrm = new TrackORMAdv(tracks, false);

            // Act
            var result = trackOrm.Tracks;

            // Asserts
            //Track count
            Assert.Equal(19, result.Count);
        }

        [Fact]
        public void When_Advanced_Track_Map_Provided_Get_440km_Lenght_Station1to17()
        {
            // Arrange
            string tracks = advTrackMap;
            var trackOrm = new TrackORMAdv(tracks, false);

            //Trip length
            var lengh1to17km = trackOrm.GetTripLength(1, 17);
            Assert.Equal(440, lengh1to17km);
            var lengh17to1km = trackOrm.GetTripLength(17, 1);
            Assert.Equal(440, lengh17to1km);

            //Trip direction
            var direction17to1km = trackOrm.GetTripDirection(17, 1);
            Assert.Equal("to west", direction17to1km);
            var direction1to17km = trackOrm.GetTripDirection(1, 17);
            Assert.Equal("to east", direction1to17km);
        }

        [Fact]
        public void When_Advanced_Track_Map_Provided_Get_Directions()
        {
            // Arrange
            string tracks = advTrackMap;
            var trackOrm = new TrackORMAdv(tracks, false);

            //Trip direction
            var direction17to1km = trackOrm.GetTripDirection(17, 1);
            Assert.Equal("to west", direction17to1km);
            var direction1to17km = trackOrm.GetTripDirection(1, 17);
            Assert.Equal("to east", direction1to17km);
        }

        [Fact]
        public void When_Advanced_Track_Map_Provided_Get_TravelTime_Based_On_Given_Speed()
        {
            // Arrange
            string tracks = advTrackMap;
            var trackOrm = new TrackORMAdv(tracks, false);

            //Trip direction
            var travelTime = trackOrm.GetTripTravelTime(160, 4, 8);
            Assert.Equal(TimeSpan.Parse("1:26:15"), travelTime);
            var travelTime2 = trackOrm.GetTripTravelTime(160, 3, 1);
            Assert.Equal(TimeSpan.Parse("1:30:00"), travelTime2);
        }

        [Fact]
        public void When_Advanced_Track_Map_Provided_Get_TravelTime_Per_Link_Based_On_Given_Speed()
        {
            // Arrange
            string tracks = advTrackMap;
            var trackOrm = new TrackORMAdv(tracks, false);
            //Trip direction
            var travelTime = trackOrm.GetLinkTravelTimes(160, 1, 13);
            Dictionary<string, TimeSpan> links = new Dictionary<string, TimeSpan>();
            links.Add("1-RS:X5Y5", TimeSpan.Parse("00:07:30"));
            links.Add("RS:X5Y5-RS:X12Y13", TimeSpan.Parse("00:30:00"));
            links.Add("RS:X12Y13-13", TimeSpan.Parse("00:07:30"));
            Assert.Equal(links, travelTime);
        }
    }
}