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
    }
}