using System;
using System.Collections;
using TrainEngine.Tracks;
using Xunit;

namespace TrainEngine.Tests
{
    public class TrackOrmTests
    {
        
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

        [Fact]
        public void When_One_Crossing_Provided()
        {
            //Arrange
            var track = new TrackORM("*[1]-=-[3]");

            Assert.Single(track.Tracks[0].CrossingsAtTrackPart);
        }

        [Fact]
        public void When_Two_Crossing_Provided()
        {
            //Arrange
            var track = new TrackORM("*[1]-=-=-[3]");

            Assert.Equal(2, track.Tracks[0].CrossingsAtTrackPart.Count);
        }

        [Fact]
        public void When_Crossing_at_TrackPart_Two_Provided()
        {
            //Arrange
            var track = new TrackORM("*[1]-=-[3]");

            Assert.Equal(2, track.Tracks[0].CrossingsAtTrackPart[0]);
        }
    }
}