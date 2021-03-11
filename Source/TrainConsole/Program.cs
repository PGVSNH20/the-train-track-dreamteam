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

            // Step 2:
            // Make the trains run in treads
            TrainsOrm train = new TrainsOrm();

            train.TrainsList.Add(new Train(0) {
                Name = "Loket",
                MaxSpeed = 100,
                Operated = true,
                CurrentPassangers = new List<Passanger>(),
                MaxPassengersCount = 15,
                IsMoving = false,
                CurrentDestination = "",
                IsAtStation = false });

            TrackLinkOrm track = new TrackLinkOrm();

            Console.WriteLine(track.TrackMap);

            StationsOrm stations = new StationsOrm();

            stations.Read();

            ITravelPlan travelPlan = new TravelPlan(1)
                .StartAt(1, new DateTime(2021, 03, 21, 10, 00, 00))
                .ArriveAt(2, new DateTime(2021, 03, 21, 11, 00, 00))
                .ArriveAt(3, new DateTime(2021, 03, 21, 11, 00, 00))
                .GeneratePlan();

            //stations.Write();
        }
    }
}