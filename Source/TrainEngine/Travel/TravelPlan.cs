using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
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

        private TrackORM _trackORM { get; set; }

        public TravelPlan()
        {
            _trackORM = new TrackORM();
            TimeTable = new List<TripStop>();
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
            TimeSpan clock = TimeSpan.Parse(fakeClock);

            int[] trains = TimeTable
                .GroupBy(t => t.TrainId)
                .Select(grp => grp.First())
                .Select(t => t.TrainId)
                .ToArray();

            List<Task> trainTasks = new List<Task>();

            foreach (int trainId in trains)
            {
                var runTrainTask = Task.Run(() => RunTrain(trainId, clock, timeFastForward));
                trainTasks.Add(runTrainTask);
            }

            Task.WaitAll(trainTasks.ToArray());

            void RunTrain(int trainId, TimeSpan fakeClock, int timeFastForward)
            {
                var trainTimeTable = TimeTable.FindAll(t => t.TrainId == trainId).OrderBy(t => t.DepartureTime).ToList();

                var waitTime = trainTimeTable[0].DepartureTime - fakeClock;

                Thread.Sleep(Convert.ToInt32(waitTime.Value.TotalMilliseconds) / timeFastForward);

                for (int i = 0; i < trainTimeTable.Count; i++)
                { 
                    if (i > 0)
                    { 
                        Console.WriteLine($"Train {trainId} arrived at station {trainTimeTable[i].StationId} at {trainTimeTable[i].ArrivalTime}");
                        if (i != (trainTimeTable.Count - 1))                       
                        Thread.Sleep(5);                       
                    }       
                    if (i < (trainTimeTable.Count - 1))
                    {
                        Thread.Sleep(5);
                        Console.WriteLine($"Train {trainId} left station {trainTimeTable[i].StationId} at {trainTimeTable[i].DepartureTime}");
                        waitTime = trainTimeTable[i + 1].ArrivalTime - trainTimeTable[i].DepartureTime;
                        Thread.Sleep(Convert.ToInt32(waitTime.Value.TotalMilliseconds) / timeFastForward);              
                    }               
                }
            }
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