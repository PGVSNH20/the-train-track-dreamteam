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

        private TrackORMAdv _trackORMAdv { get; set; }

        public TravelPlan()
        {
            TimeTable = new List<TripStop>();
            _trackORMAdv = new TrackORMAdv();
        }

        public TravelPlan(TrackORMAdv trackORMAdv)
        {
            TimeTable = new List<TripStop>();
            _trackORMAdv = trackORMAdv;
        }

        public TravelPlan(string sringInput, bool file = true)
        {
            TimeTable = new List<TripStop>();
            _trackORMAdv = new TrackORMAdv(sringInput, file);
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
            var lastRecord = TimeTable
                .FindAll(t => t.TrainId == Train.Id)
                .OrderBy(t => t.ArrivalTime)
                .ToList()
                .Last();

            var minTravelTime = _trackORMAdv.GetMinTravelTime(Train.Id, lastRecord.StationId, stationId);

            if ((TimeSpan.Parse(ariveTime) - lastRecord.DepartureTime) > minTravelTime)
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
            throw new ArgumentOutOfRangeException(
                $"Mininum travel time for train {Train.Id} from station {lastRecord.StationId} " +
                $"to station {stationId} is {minTravelTime}");
        }

        public ITravelPlan DepartureFrom(int stationId, string departureTime)
        {
            var lastRecord = TimeTable
                .FindAll(t => t.TrainId == Train.Id)
                .OrderBy(t => t.ArrivalTime)
                .ToList()
                .Last();

            var minTravelTime = _trackORMAdv.GetMinTravelTime(Train.Id, lastRecord.StationId, stationId);
            var ariveTime = TimeSpan.Parse(departureTime) - TimeSpan.Parse("0:05");

            if ((ariveTime - lastRecord.DepartureTime) > minTravelTime)
            {
                var tripStop = new TripStop();
                tripStop.StationId = stationId;
                tripStop.ArrivalTime = ariveTime;
                tripStop.DepartureTime = TimeSpan.Parse(departureTime);
                tripStop.TrainId = Train.Id;
                TimeTable.Add(tripStop);
                return this;
            }
            throw new ArgumentOutOfRangeException(
              $"Mininum travel time for train {Train.Id} from station {lastRecord.StationId} " +
              $"to station {stationId} is {minTravelTime}");
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
            var simulator = new Simulator(TimeTable, fakeClock, timeFastForward, _trackORMAdv);
            simulator.Simulate(timeFastForward);
        }
    }
}