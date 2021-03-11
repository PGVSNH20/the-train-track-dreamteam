using System;
using System.Collections.Generic;
using System.Text;

namespace TrainEngine.TrainTrack
{
    public interface ILinkNode
    {
        public int Id { set; get; }
        public bool IsLastStation { set; get; }
    }
}