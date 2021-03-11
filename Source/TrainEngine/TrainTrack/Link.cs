using System;
using System.Collections.Generic;
using System.Text;

namespace TrainEngine.TrainTrack
{
    public class Link
    {
        public ILinkNode StartNode { set; get; }
        public ILinkNode EndNode { set; get; }
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
                return ($"Track from {StartNode.Id} to {EndNode.Id} is {LinkUnitsCount} units long and {crossings}");
            }
            return ($"Track from {StartNode.Id} to {EndNode.Id} is {LinkUnitsCount} units long");
        }
    }
}