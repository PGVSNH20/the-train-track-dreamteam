using System;
using System.Collections.Generic;
using System.Text;

namespace TrainEngine.Tracks
{
    public class Station
    {
        public int Id { set; get; }
        public string StationName { set; get; }
        public bool IsEndstation { set; get; }
    }
}