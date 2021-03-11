﻿using System;
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
            //trainTracks.PrintTrackMap();

            var travelPlan = new TravelPlan().SettActualTrain(1).StartAt(1, "10:14").ArriveAt(2, "11:45");
            travelPlan.SettActualTrain(2).StartAt(3, "10:14").ArriveAt(4, "11:45").ArriveAt(4, "12:35").ArriveAt(4, "12:59");
            travelPlan.Simulate("10:00", 1000);

            // Step 2:
            // Make the trains run in treads

            //stations.Write();
            Console.ReadKey();
        }
    }
}