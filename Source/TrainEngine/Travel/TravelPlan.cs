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
using TrainEngine.Trains;

namespace TrainEngine.Travel
{
    public class TravelPlan : ITravelPlan
    {
        public List<TripStop> TimeTable { get; set; }

        public Train Train { get; set; }

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
            var time = TimeSpan.Parse(fakeClock);

            int[] trains = TimeTable
                .GroupBy(t => t.TrainId)
                .Select(grp => grp.First())
                .Select(t => t.TrainId)
                .ToArray();

            List<Task> trainTasks = new List<Task>();

            foreach (int trainId in trains)
            {
                var trainTask = Task.Run(() => RunTrain(trainId, time));
                trainTasks.Add(trainTask);
            }

            Task.WaitAll(trainTasks.ToArray());

            void RunTrain(int trainId, TimeSpan fakeClock)
            {
                List<TripStop> trainTimeTable = TimeTable
                    .FindAll(t => t.TrainId == trainId)
                    .OrderBy(t => t.DepartureTime)
                    .ToList();
                int waitTime = Convert.ToInt32(((TimeSpan)trainTimeTable[0].DepartureTime - fakeClock).TotalMilliseconds);
                fakeClock += (TimeSpan)trainTimeTable[0].DepartureTime - fakeClock;
                Console.WriteLine($"Train {trainId} is ready for departure");
                Thread.Sleep(waitTime / timeFastForward);

                foreach (var tripStop in trainTimeTable)
                {
                    if (tripStop.ArrivalTime != null)
                    {
                        waitTime = Convert.ToInt32(((TimeSpan)tripStop.DepartureTime - fakeClock).TotalMilliseconds);
                        fakeClock += (TimeSpan)tripStop.ArrivalTime - fakeClock;
                        Thread.Sleep(waitTime / timeFastForward);
                        Console.WriteLine($"Train {tripStop.TrainId} arrived att station {tripStop.StationId} at {fakeClock} o´clock");
                    }
                    if (tripStop.DepartureTime != null)
                    {
                        waitTime = Convert.ToInt32(((TimeSpan)tripStop.DepartureTime - fakeClock).TotalMilliseconds);
                        fakeClock += (TimeSpan)tripStop.DepartureTime - fakeClock;
                        Thread.Sleep(waitTime / timeFastForward);
                        Console.WriteLine($"Train {tripStop.TrainId} left station {tripStop.StationId} at {fakeClock} o´clock");
                    }
                }
            }
        }
    }
}