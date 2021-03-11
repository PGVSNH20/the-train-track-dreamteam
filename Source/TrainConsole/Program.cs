using System;
using TrainEngine.ORM;
using TrainEngine.Tracks;

namespace TrainConsole
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            Console.WriteLine("Train track!");
            // Step 1:
            // Parse the traintrack (Data/traintrack.txt) using ORM (see suggested code)
            // Parse the trains (Data/trains.txt)

            // Step 2:
            // Make the trains run in treads

            TrackLinkOrm track = new TrackLinkOrm();

            Console.WriteLine(track.TrackMap);

            StationsOrm stations = new StationsOrm();
            stations.Stations.Add(new Station() { 
            });
        }
    }
}