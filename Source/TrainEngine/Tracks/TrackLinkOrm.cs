using System;
using System.IO;
using System.Collections.Generic;
using System.Text;

namespace TrainEngine.Tracks
{
    public class TrackLinkOrm
    {
        public char[] TrackMap { get; set; }

        public TrackLinkOrm()
        {
            Read();
        }
        private void Read()
        {
            string[] line = File.ReadAllLines("Data/traintrack1.txt");
            TrackMap = new char[line[0].Length];
            for (var i = 0; i < TrackMap.Length; i++)
            {
                TrackMap[i] = Convert.ToChar(line[i]);
            }
        }
    }
}