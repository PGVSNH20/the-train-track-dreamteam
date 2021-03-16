using System;
using System.Collections;
using TrainEngine.Tracks;
using Xunit;

namespace TrainEngine.Tests
{
    public class TrackOrmTests
    {
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
            string track = "*[11]---[22]----[33]";
            var trackOrm = new TrackORMAdv(track, false);

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
            string track = "*[1]-[2]-[3]-[4]-[5]-[6]-[7]-[8]-[9]-[10]-[11]-[12]-[13]-[14]-[15]-[16]-[17]-[18]-[19]-[20]";
            var trackOrm = new TrackORMAdv(track, false);

            // Act
            var result = trackOrm.Tracks;

            // Assert
            Assert.Equal(19, result.Count);
        }
        [Fact]
        public void When_Two_Stations_Provided()
        {
            //Arrange
            var track = new TrackORM("*[1]-------[3]");

            Assert.Single(track.Tracks);

        }
        [Fact]
        public void When_Three_Stations_Provided()
        {
            //Arrange
            var track = new TrackORM("*[1]---[2]----[3]");

            Assert.Equal(2, track.Tracks.Count);
        }
        [Fact]
        public void When_Three_Track_Parts_Provided()
        {
            //Arrange
            var track = new TrackORM("*[1]---[3]");

            Assert.Equal(3, track.Tracks[0].NumberOfTrackParts);
        }
    }
}