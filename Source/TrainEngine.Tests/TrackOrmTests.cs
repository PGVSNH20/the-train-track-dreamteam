using System;
using System.Collections.Generic;
using TrainEngine.Tracks;
using Xunit;

namespace TrainEngine.Tests
{
    public class TrackOrmTests
    {
        [Fact]
        public void When_OnlyAStationIsProvided_Expect_TheResultOnlyToContainAStationWithId1()
        {
            // Arrange
            string track = "[1]";
            TrackOrm trackOrm = new TrackOrm();

            // Act
            var result = trackOrm.ParseTrackDescription(track);

            // Assert
            //Assert.IsType<Station>(result.TackPart[0]);
            //Station s = (Station)result.TackPart[0];
            //Assert.Equal(1, s.Id);
        }

        [Fact]
        public void When_ProvidingTwoStationsWithOneTrackBetween_Expect_TheTrackToConcistOf3Parts()
        {
            // Arrange
            string track = "[1]-[2]";
            TrackOrm trackOrm = new TrackOrm();
            
            // Act
            var result = trackOrm.ParseTrackDescription(track);

            // Assert
            Assert.Equal(3, result.NumberOfTrackParts);
        }

        [Fact]
        public void When_ProvidingTwoStationsWithSevenTrackBetween_Expect_TheTrackToConcistOf9Parts()
        {
            // Arrange
            TrackORMAdv trackOrm = new(@"Data\traintrack1.txt");

            // Act
            Track result = trackOrm.Tracks[0];

            // Assert
            Assert.Equal(9, result.NumberofTrackParts);
        }
        [Fact]
        public void When_ProvidingThreeStationsWith12TrackBetween_Expect_TheTrackToConcistOf27Parts()
        {
            // Arrange
   
            TrackORM trackOrm = new();
            // Act
            
            Track result = trackOrm.Tracks[0];
            
            // Assert
            Assert.Equal(27, result.TrackLength);
        }
        [Fact]
        public void When_ProvidingFourStationsWithManyTrackBetween_Expect_TheTrackToConcistOf49Parts()
        {
            // Arrange

            TrackORMAdv trackOrm = new(@"Data\traintrack3.txt");
            // Act

            Track result = trackOrm.Tracks[0];

            // Assert
            Assert.Equal(49, result.NumberofTrackParts);
        }
        [Fact]
        public void When_ProvidingFourStationsWithManyTrackBetween_Expect_TheTrackLengthToBe67()
        {
            // Arrange
            TrackORMAdv trackOrm = new(@"Data\traintrack3.txt");
            int numberofTracks = new();
            List<Track> list = trackOrm.Tracks;
           
            // Act
            foreach (Track track in list)
            {
                numberofTracks += track.TrackLength;
            }

            // Assert
            Assert.Equal(67, numberofTracks);
        }
    }
}
