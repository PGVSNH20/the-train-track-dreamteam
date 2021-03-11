using System;
using System.Collections.Generic;
using System.Text;

namespace TrainEngine.Tracks
{
    public class Track
    {
        public Station StartStation { get; set; }
        public Station EndStation { get; set; }
        public int NumberOfLinkParts { get; set; }
    }
}