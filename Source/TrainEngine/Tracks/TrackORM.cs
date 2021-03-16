using System;
using System.IO;
using System.Collections.Generic;
using System.Text;
using TrainEngine.Trains;

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
            Track track = new Track();

            foreach (char symbol in TrackMap)
            {
                while (track.StartStation == null)
                {
                    if (symbol > 47 && symbol < 59)
                    {
                        track.StartStation = new Station() { Id = symbol };
                    }
                }

                while (track.EndStation == null)
                {
                    if (symbol == '-' || symbol == '=')
                    track.NumberOfTrackParts++;

                    if (symbol > 47 && symbol < 59)
                    {
                        track.EndStation = new Station() { Id = symbol };
                        track = new Track();
                        track.StartStation = new Station() { Id = symbol };
                    }

                }

            }
            Tracks.Add(track);
        }

 
    }
}