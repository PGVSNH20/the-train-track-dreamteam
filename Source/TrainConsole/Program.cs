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
            //var track = new TrackORM();
            //Console.WriteLine("Train track!");

            //var travelPlan = new TravelPlan()
            //    .SettActualTrain(1)
            //    .StartAt(1, "10:14")
            //    .ArriveAt(2, "11:45")
            //    .ArriveAt(3, "13:45")
            //    .SettActualTrain(2)
            //    .StartAt(1, "11:14")
            //    .ArriveAt(2, "12:45")
            //    .ArriveAt(3, "14:45")
            //    .GenerateNewPlan("5trains_20210316");

            //var travelPlanAdv = new TravelPlanAdv();


            // Step 1:
            // Parse the traintrack (Data/traintrack.txt) using ORM (see suggested code)
            // Parse the trains (Data/trains.txt)
            //var espresso = new CoffeeMachine().AddEspresso().AddBean("Robusta", 100).ToBeverage();
            

            var train = new Train();

            var trainTracks = new TrackORM(@"Data\traintrack3.txt");

            travelPlan.Simulate("10:14:00", 1000);

            var trainTracks = new TrackORMAdv(@"Data\traintrack4.txt");

            var travelPlan = new TravelPlanAdv(trainTracks).LoadPlan("timetable_5trains_20210315").Simulate("10:00", 1000);

            //trainTracks.PrintTrackMap();

            //var fo = trainTracks.GetTripDirection(3, 4);

            //var foo = trainTracks.GetTravelTime(60, 4, 8);
            //var foo2 = trainTracks.GetLinkTravelTimes(60, 4, 8);

            //var foo = new TravelPlan().SettActualTrain(2).StartAt(3, "11:03").ArriveAt(5, "12:00");
            //foo.GenerateNewPlan("timetable_20210312");

            //var foo = new TrainsOrm();
            //foo.Trains.Add(new Train(2)
            //{
            //    Name = "Snabbtåg",
            //    MaxSpeed = 120
            //});
            //foo.Write();

            // Step 2:
            // Make the trains run in treads

            //stations.Write();
            Console.ReadKey();
        }
    }
}