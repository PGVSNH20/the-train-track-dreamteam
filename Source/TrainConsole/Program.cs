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

            var trainTracks = new TrackORMAdv(@"Data\traintrack4.txt");
            //trainTracks.PrintTrackMap();

            var foo = trainTracks.GetTravelTime(60, 4, 8);
            var foo2 = trainTracks.GetLinkTravelTimes(60, 4, 8);


            //var foo = new TravelPlan().SettActualTrain(2).StartAt(3, "11:03").ArriveAt(5, "12:00");
            //foo.GenerateNewPlan("timetable_20210312");

            //var foo = new TrainsOrm();
            //foo.Trains.Add(new Train(2)
            //{
            //    Name = "Snabbtåg",
            //    MaxSpeed = 120
            //});
            //foo.Write();



            var travelPlan = new TravelPlan().SettActualTrain(1).StartAt(1, "10:14").ArriveAt(13, "11:45").ArriveAt(16, "13:45");
            travelPlan.SettActualTrain(2).StartAt(1, "10:25").ArriveAt(3, "11:55").ArriveAt(6, "12:35");
            travelPlan.SettActualTrain(3).StartAt(3, "11:45").ArriveAt(6, "12:35");
            travelPlan.Simulate("10:00", 300);



            // Step 2:
            // Make the trains run in treads

            //stations.Write();
            Console.ReadKey();
        }
    }
}