using System;
using System.Collections.Generic;
using System.Text;
using TrainEngine.Trains;

namespace TrainEngine.Travel
{
    internal class TravelPlan : ITravelPlan
    {
        public List<TripStop> TimeTable { get; set; }

        public Train Train { get; set; }

        public TravelPlan(int trainId)
        {
            var train = new Train(trainId);
            Train = train;
        }

        public ITravelPlan StartAt(int stationId, DateTime departureTime)
        {
            var tripStop = new TripStop();
            tripStop.StationId = stationId;
            tripStop.DepartureTime = departureTime;
            tripStop.TrainId = Train.Id;
            return this;
        }

        public ITravelPlan ArriveAt(int stationId, DateTime ariveTime)
        {
            var tripStop = new TripStop();
            tripStop.StationId = stationId;
            tripStop.ArrivalTime = ariveTime;
            DateTime departureTime = ariveTime.AddMinutes(5);
            tripStop.DepartureTime = departureTime;
            tripStop.TrainId = Train.Id;
            return this;
        }

        public ITravelPlan GeneratePlan(string path)
        {
            return this;
        }

        public void Load(string path)
        {
            throw new NotImplementedException();
        }

        public void Save(string path)
        {
            throw new NotImplementedException();
        }

    }
}