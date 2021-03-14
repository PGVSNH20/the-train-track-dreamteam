using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace TrainEngine.Travel
{
    internal class Simulator
    {
        public List<TripStop> TimeTable { get; set; }
        private TimeSpan Time { get; set; }
        public int TimeFastForward { get; set; }

        public Simulator(List<TripStop> timeTable, string fakeClock, int timeFastForward)
        {
            TimeTable = timeTable;
            Time = TimeSpan.Parse(fakeClock);
            TimeFastForward = timeFastForward;
        }

        public void Simulate(int timeFastForward)
        {
            var time = Time;

            int[] trains = TimeTable
                .GroupBy(t => t.TrainId)
                .Select(grp => grp.First())
                .Select(t => t.TrainId)
                .ToArray();

            List<Task> trainTasks = new List<Task>();

            foreach (int trainId in trains)
            {
                var trainTask = Task.Run(() => RunTrain(trainId, time, timeFastForward));
                trainTasks.Add(trainTask);
            }

            Task.WaitAll(trainTasks.ToArray());
        }
        private void RunTrain(int trainId, TimeSpan fakeClock, int timeFastForward)
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
