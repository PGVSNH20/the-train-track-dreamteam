using System;
using System.Collections.Generic;
using TrainEngine.ORM;
using TrainEngine.Tracks;
using TrainEngine.Trains;
using TrainEngine.Travel;

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

            var trainTracks = new TrackORMAdv();
            trainTracks.PrintTrackMap();

            // Step 2:
            // Make the trains run in treads

            //stations.Write();
            Console.ReadKey();
        }
    }
}