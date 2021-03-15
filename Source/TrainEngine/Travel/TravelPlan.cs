using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Unicode;
using System.Threading;
using System.Threading.Tasks;
using TrainEngine.Tracks;
using TrainEngine.Trains;

namespace TrainEngine.Travel
{
    public class TravelPlan : ITravelPlan
    {
        public List<TripStop> TimeTable { get; set; }

        public Train Train { get; set; }

        private TrackORMAdv trackORMAdv { get; set; }

        public TravelPlan()
        {
            TimeTable = new List<TripStop>();
        }

        public ITravelPlan SettActualTrain(int trainId)
        {
            var train = new Train(trainId);
            Train = train;
            return this;
        }

        public ITravelPlan StartAt(int stationId, string departureTime)
        {
            var tripStop = new TripStop();
            tripStop.StationId = stationId;
            tripStop.DepartureTime = TimeSpan.Parse(departureTime);
            tripStop.TrainId = Train.Id;
            TimeTable.Add(tripStop);
            return this;
        }

        public ITravelPlan ArriveAt(int stationId, string ariveTime)
        {
            var tripStop = new TripStop();
            tripStop.StationId = stationId;
            tripStop.ArrivalTime = TimeSpan.Parse(ariveTime);
            TimeSpan? departureTime = tripStop.ArrivalTime + TimeSpan.Parse("0:05");
            tripStop.DepartureTime = departureTime;
            tripStop.TrainId = Train.Id;
            TimeTable.Add(tripStop);
            return this;
        }

        public ITravelPlan DepartureFrom(int stationId, string departureTime)
        {
            var tripStop = new TripStop();
            tripStop.StationId = stationId;
            tripStop.ArrivalTime = null;
            tripStop.DepartureTime = TimeSpan.Parse(departureTime);
            tripStop.TrainId = Train.Id;
            TimeTable.Add(tripStop);
            return this;
        }

        public ITravelPlan GenerateNewPlan(string fileName = "timetable")
        {
            Write(fileName);
            return this;
        }

        public ITravelPlan AddToExistingPlan(string fileName = "timetable")
        {
            var tmpTimeTable = new List<TripStop>();
            tmpTimeTable.AddRange(TimeTable);
            Read(fileName);
            TimeTable.AddRange(tmpTimeTable);
            Write(fileName);
            return this;
        }

        public ITravelPlan LoadPlan(string fileName = "timetable")
        {
            Read(fileName);
            return this;
        }

        private void Read(string fileName)
        {
            var jsonString = File.ReadAllText($"Data/{fileName}.json");
            TimeTable = JsonSerializer.Deserialize<List<TripStop>>(jsonString);
        }

        private void Write(string fileName)
        {
            var options = new JsonSerializerOptions
            {
                Encoder = JavaScriptEncoder.Create(UnicodeRanges.BasicLatin),
                WriteIndented = true
            };
            string jsonString = JsonSerializer.Serialize(TimeTable, options);
            File.WriteAllText($"Data/{fileName}.json", jsonString);
        }

        public void Simulate(string fakeClock, int timeFastForward)
        {
            trackORMAdv = new TrackORMAdv();
            var simulator = new Simulator(TimeTable, fakeClock, timeFastForward, trackORMAdv);
            simulator.Simulate(timeFastForward);
        }
    }
}