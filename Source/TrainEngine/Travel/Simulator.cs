using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using TrainEngine.Tracks;

namespace TrainEngine.Travel
{
    internal class Simulator
    {
        public List<TripStop> TimeTable { get; set; }
        private TimeSpan Time { get; set; }
        public int TimeFastForward { get; set; }
        public TrackORMAdv TrackORMadv { get; set; }

        public Simulator(List<TripStop> timeTable, string fakeClock, int timeFastForward, TrackORMAdv trackORMadv)
        {
            TimeTable = timeTable;
            Time = TimeSpan.Parse(fakeClock);
            TimeFastForward = timeFastForward;
            TrackORMadv = trackORMadv;
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
                var trainTimeTable = new List<TripStop>();
                trainTimeTable = TimeTable
                    .FindAll(t => t.TrainId == trainId)
                    .OrderBy(t => t.DepartureTime)
                    .ToList();

                var runTrainTask = Task.Run(() => RunTrain(trainId, trainTimeTable, time, timeFastForward));
                trainTasks.Add(runTrainTask);


                var beginStation = trainTimeTable[0].StationId;
                var finishStation = trainTimeTable[trainTimeTable.Count() - 1].StationId;

                //var linkTravelTimes = new Dictionary<string, TimeSpan>(TrackORMadv.GetLinkTravelTimes(100, beginStation, finishStation));

                //var uppdateLinksInUseTasks = Task.Run(() => UppdateLinksInUse(
                //    trainId,
                //    linkTravelTimes,
                //    time,
                //    timeFastForward
                //    ));
                //trainTasks.Add(uppdateLinksInUseTasks);
            }



            Task.WaitAll(trainTasks.ToArray());
        }
        private void RunTrain(int trainId, List<TripStop> trainTimeTable, TimeSpan fakeClock, int timeFastForward)
        {

 

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
        private void UppdateLinksInUse(int trainId, Dictionary<string, TimeSpan> linkTravelTimes, TimeSpan fakeClock, int timeFastForward)
        {
            string previusLink = null;
            foreach (var link in linkTravelTimes)
            {
                if (previusLink != null)
                {
                    Console.WriteLine($"Train {trainId} leaving {previusLink}");
                }
                
                int waitTime = Convert.ToInt32((link.Value - fakeClock).TotalMilliseconds);
                fakeClock += link.Value - fakeClock;

                Console.WriteLine($"Link {link.Key} in use by {trainId}");
                if (waitTime > 0)
                {
                    Thread.Sleep(waitTime / timeFastForward);
                }
                
                previusLink = link.Key;
            }
        }
    }
}
