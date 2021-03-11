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
            Links = new List<Link>();
            CreateLinks();
        }

        private void Read()
        {
            string[] line = File.ReadAllLines("Data/traintrack2.txt");
            TrackMap = new char[line[0].Length];
            for (var i = 0; i < TrackMap.Length; i++)
            {
                TrackMap[i] = Convert.ToChar(line[0][i]);
            }
        }

        private void CreateLinks()
        {
            Link link = new Link();

            foreach (char symbol in TrackMap)
            {
                if (symbol > 47 && symbol < 59)
                {
                    ILinkNode node = new Station();
                    double number = char.GetNumericValue(symbol);
                    node.Id = Convert.ToInt32(number);

                    if (link.StartNode == null)
                        link.StartNode = node;
                    else
                        link.EndNode = node;
                }
                if (symbol == '-')
                    link.NumberOfLinkParts++;
            }
            Links.Add(link);
        }
    }
}