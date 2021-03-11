using System;
using System.Collections.Generic;
using System.Text;

namespace TrainEngine.Tracks
{
    public class Link
    {
        public ILinkNode StartNode { get; set; }
        public ILinkNode EndNode { get; set; }
        public int NumberOfLinkParts { get; set; }
    }
}
