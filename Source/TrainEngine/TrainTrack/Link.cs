using System;
using System.Collections.Generic;
using System.Text;

namespace TrainEngine.TrainTrack
{
    public class Link
    {
        public Station StartStation { set; get; }
        public Station EndStation { set; get; }
        public int LinkUnitsCount { set; get; }
        public List<int> CrossingsAtUnit { set; get; }

        public Link()
        {
            LinkUnitsCount = 0;
            CrossingsAtUnit = new List<int>();
        }

        public override string ToString()
        {
            if (CrossingsAtUnit != null && CrossingsAtUnit.Count > 0)
            {
                string crossings = $"has {CrossingsAtUnit.Count} crossings";
                return ($"Track from {StartStation.Id} to {EndStation.Id} is {LinkUnitsCount} units long and {crossings}");
            }
            return ($"Track from {StartStation.Id} to {EndStation.Id} is {LinkUnitsCount} units long");
        }
    }
}