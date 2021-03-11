using System;
using System.IO;
using System.Collections.Generic;
using System.Text;

namespace TrainEngine.Tracks
{
    public class TrackORM
    {
        public char[] TrackMap { get; set; }
        public List<Track> Tracks { get; set; }

        public TrackORM()
        {
            Read();
            Tracks = new List<Track>();
            CreateTracks();
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

        private void CreateTracks()
        {
            Track link = new Track();

            foreach (char symbol in TrackMap)
            {
                if (symbol > 47 && symbol < 59)
                {
                    Station node = new Station();
                    double number = char.GetNumericValue(symbol);
                    node.Id = Convert.ToInt32(number);

                    if (link.StartStation == null)
                        link.StartStation = node;
                    else
                        link.EndStation = node;
                }
                if (symbol == '-')
                    link.NumberOfLinkParts++;
            }
            Tracks.Add(link);
        }
    }
}