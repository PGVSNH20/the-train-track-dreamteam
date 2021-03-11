using System;
using System.IO;
using System.Collections.Generic;
using System.Text;

namespace TrainEngine.Tracks
{
    public class TrackLinkOrm
    {
        public char[] TrackMap { get; set; }
        public List<Link> Links { get; set; }
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
                TrackMap[i] = Convert.ToChar(line[0][i]);
            }
        }

        public void CreateLinks()
        {
            Link link = new Link();
            foreach (char symbol in TrackMap)
            {
                if (symbol > '0' && symbol < '9')
                {
                    link.StartNode = Convert.ToInt32(symbol);
                }
            }
        }
    }
}