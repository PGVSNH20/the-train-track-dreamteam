using System;
using System.Collections.Generic;
using System.Text;

namespace TrainEngine.Tracks
{
    public class Track
    {
        public Station StartStation { get; set; }
        public Station EndStation { get; set; }
        public int NumberOfTrackParts { get; set; }
        public List<int> CrossingsAtTrackPart { set; get; }

        public Track()
        {
            CrossingsAtTrackPart = new List<int>();
        }
        public override string ToString()
        {
            if (CrossingsAtTrackPart != null && CrossingsAtTrackPart.Count > 0)
            {
                string crossings = $"has {CrossingsAtTrackPart.Count} crossings";
                return ($"Track from {StartStation.Id} to {EndStation.Id} is {NumberOfTrackParts} units long and {crossings}");
            }
            return ($"Track from {StartStation.Id} to {EndStation.Id} is {NumberOfTrackParts} units long");
        }
    }
}