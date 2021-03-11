using System;
using System.Collections.Generic;
using System.Text;

namespace TrainEngine.Tracks
{
    public class Station : ILinkNode
    {
        public int Id { set; get; }
        public string StationName { set; get; }
        public bool IsEndstation { set; get; }
    }
}