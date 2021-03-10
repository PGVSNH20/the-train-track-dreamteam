using System;
using System.Collections.Generic;
using System.Text;

namespace TrainEngine.Tracks
{
    internal class TrackLinkService
    {
        public char[] TrackMap { get; set; }

        public TrackLinkService(string[] data)
        {
            TrackMap = new char[data[0].Length];
            for (var i = 0; i < TrackMap.Length; i++)
            {
                TrackMap[i] = Convert.ToChar(data[i]);
            }
        }
    }
}