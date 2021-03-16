﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.Json;
using TrainEngine.Tracks;
using TrainEngine.Trains;

namespace TrainEngine.Travel
{
    class TravelPlan : ITravelPlan
    {
        public List<TripStop> TimeTable { get; set; }

        public Train Train { get; set; }

        private TrackORM _trackORM { get; set; }

        public TravelPlan()
        {
            _trackORM = new TrackORM();

        }


        public ITravelPlan AddToExistingPlan(string fileName)
        {
            throw new NotImplementedException();
        }

        public ITravelPlan ArriveAt(int stationId, string ariveTime)
        {
            TripStop tripstop = new TripStop();
            tripstop.StationId = stationId;
            tripstop.ArrivalTime = TimeSpan.Parse(ariveTime);
            tripstop.TrainId = Train.Id;
            TimeSpan? departureTime = tripstop.ArrivalTime + TimeSpan.Parse("0:05");
            tripstop.DepartureTime = departureTime;
            TimeTable.Add(tripstop);
            return this;
        }

        public ITravelPlan GenerateNewPlan(string fileName)
        {
            string jsonString = JsonSerializer.Serialize(TimeTable);
            File.WriteAllText($"Data/{fileName}.json", jsonString);
            return this;
        }

        public ITravelPlan SettActualTrain(int trainId)
        {
            TrainsOrm trainsOrm = new TrainsOrm();
            Train = trainsOrm.Trains.Find(t => t.Id == trainId);
            return this;
        }

        public void Simulate(string fakeClock, int timeFastForward)
        {
            throw new NotImplementedException();
        }

        public ITravelPlan StartAt(int stationId, string departureTime)
        {
            TripStop tripstop = new TripStop();
            tripstop.StationId = stationId;
            tripstop.DepartureTime = TimeSpan.Parse(departureTime);
            tripstop.TrainId = Train.Id;
            TimeSpan? arriveTime = tripstop.DepartureTime - TimeSpan.Parse("0:05");
            tripstop.ArrivalTime = arriveTime;
            TimeTable.Add(tripstop);
            return this;
        }
    }
}
