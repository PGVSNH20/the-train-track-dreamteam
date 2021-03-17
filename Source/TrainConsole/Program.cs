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
            //    .ArriveAt(2, "11:47")
            //    .ArriveAt(3, "13:43")
            //    .SettActualTrain(2)
            //    .StartAt(1, "11:14")
            //    .ArriveAt(13, "12:42")
            //    .ArriveAt(14, "14:48")
            //    .SettActualTrain(3)
            //    .StartAt(4, "11:14")
            //    .ArriveAt(7, "12:32")
            //    .ArriveAt(17, "14:56")
            //    .SettActualTrain(4)
            //    .StartAt(16, "11:12")
            //    .ArriveAt(14, "12:35")
            //    .ArriveAt(13, "14:33")
            //     .SettActualTrain(5)
            //    .StartAt(8, "11:12")
            //    .ArriveAt(7, "12:43")
            //    .ArriveAt(4, "13:42")
            //    .ArriveAt(1, "16:12")
            //    .GenerateNewPlan("5trains_20210317");

            //var travelPlanAdv = new TravelPlanAdv();

            // Step 1:
            // Parse the traintrack (Data/traintrack.txt) using ORM (see suggested code)
            // Parse the trains (Data/trains.txt)
            //var espresso = new CoffeeMachine().AddEspresso().AddBean("Robusta", 100).ToBeverage();

            //var train = new Train();

            //var trainTracks = new TrackORM(@"Data\traintrack3.txt");

            //travelPlan.Simulate("10:14:00", 1000);

            var trainTracks = new TrackORMAdv(@"Data\traintrack4.txt");
            trainTracks._PrintTrackMap();

            var travelPlan = new TravelPlanAdv(trainTracks).LoadPlan("5trains_20210317").Simulate("10:00", 1000);

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