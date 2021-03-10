using System;
using System.Collections.Generic;
using System.Text;

namespace TrainEngine.Tracks
{
    class Station : ILinkNode
    {
        public int ID { set; get; }
        public string StationName { set; get; }
        public bool IsEndstation { set; get; }
    }

}
