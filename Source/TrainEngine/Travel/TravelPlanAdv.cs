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
    public class TravelPlanAdv : ITravelPlanAdv
    {
        public List<TripStop> TimeTable { get => _timeTable; }

        private List<TripStop> _timeTable { get; set; }

        public Train Train { get; set; }

        private TrackORMAdv _trackORMAdv { get; set; }

        public TravelPlanAdv()
        {
            _timeTable = new List<TripStop>();
            _trackORMAdv = new TrackORMAdv();
        }

        public TravelPlanAdv(TrackORMAdv trackORMAdv)
        {
            _timeTable = new List<TripStop>();
            _trackORMAdv = trackORMAdv;
        }

        public TravelPlanAdv(string sringInput, bool file = true)
        {
            _timeTable = new List<TripStop>();
            _trackORMAdv = new TrackORMAdv(sringInput, file);
        }

        public ITravelPlanAdv SettActualTrain(int trainId)
        {
            var train = new Train(trainId);
            Train = train;
            return this;
        }

        public ITravelPlanAdv StartAt(int stationId, string departureTime)
        {
            var tripStop = new TripStop();
            tripStop.StationId = stationId;
            tripStop.DepartureTime = TimeSpan.Parse(departureTime);
            tripStop.TrainId = Train.Id;
            _timeTable.Add(tripStop);
            return this;
        }

        public ITravelPlanAdv ArriveAt(int stationId, string ariveTime)
        {
            var lastRecord = _timeTable
                .FindAll(t => t.TrainId == Train.Id)
                .OrderBy(t => t.ArrivalTime)
                .ToList()
                .Last();

            var minTravelTime = _trackORMAdv.GetMinTripTravelTime(Train.Id, lastRecord.StationId, stationId);

            if ((TimeSpan.Parse(ariveTime) - lastRecord.DepartureTime) > minTravelTime)
            {
                var tripStop = new TripStop();
                tripStop.StationId = stationId;
                tripStop.ArrivalTime = TimeSpan.Parse(ariveTime);
                TimeSpan? departureTime = tripStop.ArrivalTime + TimeSpan.Parse("0:05");
                tripStop.DepartureTime = departureTime;
                tripStop.TrainId = Train.Id;
                _timeTable.Add(tripStop);
                return this;
            }
            throw new ArgumentOutOfRangeException(
                $"Minimum travel time for train {Train.Id} from station {lastRecord.StationId} " +
                $"to station {stationId} is {minTravelTime}");
        }

        public ITravelPlanAdv DepartureFrom(int stationId, string departureTime)
        {
            var lastRecord = _timeTable
                .FindAll(t => t.TrainId == Train.Id)
                .OrderBy(t => t.ArrivalTime)
                .ToList()
                .Last();

            var minTravelTime = _trackORMAdv.GetMinTripTravelTime(Train.Id, lastRecord.StationId, stationId);
            var ariveTime = TimeSpan.Parse(departureTime) - TimeSpan.Parse("0:05");

            if ((ariveTime - lastRecord.DepartureTime) > minTravelTime)
            {
                var tripStop = new TripStop();
                tripStop.StationId = stationId;
                tripStop.ArrivalTime = ariveTime;
                tripStop.DepartureTime = TimeSpan.Parse(departureTime);
                tripStop.TrainId = Train.Id;
                _timeTable.Add(tripStop);
                return this;
            }
            throw new ArgumentOutOfRangeException(
              $"Minimum travel time for train {Train.Id} from station {lastRecord.StationId} " +
              $"to station {stationId} is {minTravelTime}");
        }

        public ITravelPlanAdv GenerateNewPlan(string fileName = "timetable")
        {
            Write(fileName);
            return this;
        }

        public ITravelPlanAdv AddToExistingPlan(string fileName = "timetable")
        {
            var tmpTimeTable = new List<TripStop>();
            tmpTimeTable.AddRange(_timeTable);
            Read(fileName);

            foreach (var tripStop in tmpTimeTable)
                if (_timeTable.Exists(t => t.TrainId == tripStop.TrainId))
                    _timeTable.RemoveAll(t => t.TrainId == tripStop.TrainId);

            _timeTable.AddRange(tmpTimeTable);
            Write(fileName);
            return this;
        }

        public ITravelPlanAdv LoadPlan(string fileName = "timetable")
        {
            Read(fileName);
            return this;
        }

        private void Read(string fileName)
        {
            var jsonString = File.ReadAllText($"Data/{fileName}.json");
            _timeTable = JsonSerializer.Deserialize<List<TripStop>>(jsonString);
        }

        private void Write(string fileName)
        {
            var options = new JsonSerializerOptions
            {
                Encoder = JavaScriptEncoder.Create(UnicodeRanges.BasicLatin),
                WriteIndented = true
            };
            string jsonString = JsonSerializer.Serialize(_timeTable, options);
            File.WriteAllText($"Data/{fileName}.json", jsonString);
        }

        public ITravelPlanAdv Simulate(string fakeClock, int timeFastForward)
        {
            var simulator = new Simulator(_timeTable, fakeClock, timeFastForward, _trackORMAdv);
            simulator.Simulate(timeFastForward);
            return this;
        }
    }
}