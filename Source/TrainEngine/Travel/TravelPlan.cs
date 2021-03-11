using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.Json;
using TrainEngine.Trains;

namespace TrainEngine.Travel
{
    public class TravelPlan : ITravelPlan
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

        public ITravelPlan GeneratePlan()
        {
            Write();
            return this;
        }

        public void Read()
        {
            var jsonString = File.ReadAllText("Data/timetable.json");
            TimeTable = JsonSerializer.Deserialize<List<TripStop>>(jsonString);
        }

        public void Write()
        {
            string jsonString = JsonSerializer.Serialize(TimeTable);
            File.WriteAllText("Data/timetable.json", jsonString);
        }

    }
}