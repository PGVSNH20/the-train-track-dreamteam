using System;
using System.Collections.Generic;
using System.Text;

namespace TrainEngine.TrainTrack
{
    public class Link
    {
        public int? StartStationId { set; get; } = null;
        public int? EndStationId { set; get; } = null;
        public int LinkUnitsCount { set; get; }

        public Link()
        {
            LinkUnitsCount = 0;
        }
    }
}