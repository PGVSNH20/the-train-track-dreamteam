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
        private List<LinkInUse> LinksInUse { get; set; }

        public Simulator(List<TripStop> timeTable, string fakeClock, int timeFastForward, TrackORMAdv trackORMadv)
        {
            TimeTable = timeTable;
            Time = TimeSpan.Parse(fakeClock);
            TimeFastForward = timeFastForward;
            TrackORMadv = trackORMadv;
            LinksInUse = new List<LinkInUse>();
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
                var runTrainTask = Task.Run(() => RunTrain(trainId, time, timeFastForward));
                trainTasks.Add(runTrainTask);

                var uppdateLinksInUseTasks = Task.Run(() => UppdateLinksInUse(
                    trainId,
                    time,
                    timeFastForward
                    ));
                trainTasks.Add(uppdateLinksInUseTasks);
            }

            Task.WaitAll(trainTasks.ToArray());
        }

        private void RunTrain(int trainId, TimeSpan fakeClock, int timeFastForward)
        {
            var trainTimeTable = new List<TripStop>();
            trainTimeTable = TimeTable
                .FindAll(t => t.TrainId == trainId)
                .OrderBy(t => t.DepartureTime)
                .ToList();

            int waitTime = Convert.ToInt32(((TimeSpan)trainTimeTable[0].DepartureTime - fakeClock).TotalMilliseconds);
            fakeClock += (TimeSpan)trainTimeTable[0].DepartureTime - fakeClock;
            Console.WriteLine($"Train {trainId} is ready for departure");
            Thread.Sleep(waitTime / timeFastForward);

            for (var i = 0; i < trainTimeTable.Count; i++)
            {
                if (trainTimeTable[i].ArrivalTime != null)
                {
                    waitTime = Convert.ToInt32(((TimeSpan)trainTimeTable[i].DepartureTime - fakeClock).TotalMilliseconds);
                    fakeClock += (TimeSpan)trainTimeTable[i].ArrivalTime - fakeClock;
                    Thread.Sleep(waitTime / timeFastForward);
                    Console.WriteLine($"Train {trainTimeTable[i].TrainId} arrived att station {trainTimeTable[i].StationId} at {fakeClock} o´clock");
                }

                if (trainTimeTable[i].DepartureTime != null)
                {
                    waitTime = Convert.ToInt32(((TimeSpan)trainTimeTable[i].DepartureTime - fakeClock).TotalMilliseconds);
                    fakeClock += (TimeSpan)trainTimeTable[i].DepartureTime - fakeClock;

                    if (trainTimeTable[i] == trainTimeTable.Last())
                        Console.WriteLine($"Train {trainTimeTable[i].TrainId} reached final destination");
                    else
                    {
                        Thread.Sleep(waitTime / timeFastForward);
                        Console.WriteLine($"Train {trainTimeTable[i].TrainId} left station {trainTimeTable[i].StationId} at {fakeClock} o´clock");
                    }
                }
            }
        }

        private void UppdateLinksInUse(int trainId, TimeSpan fakeClock, int timeFastForward)
        {
            var trainTimeTable = new List<TripStop>();
            trainTimeTable = TimeTable
                .FindAll(t => t.TrainId == trainId)
                .OrderBy(t => t.DepartureTime)
                .ToList();
            LinkInUse previusLink = null;

            var beginStation = trainTimeTable[0].StationId;
            var finishStation = trainTimeTable[trainTimeTable.Count() - 1].StationId;

            var tripDirection = TrackORMadv.GetTripDirection(beginStation, finishStation);

            for (var i = 0; i < trainTimeTable.Count() - 1; i++)
            {
                var trackLenght = TrackORMadv.GetTripLength(trainTimeTable[i].StationId, trainTimeTable[i + 1].StationId);
                var travelTime = trainTimeTable[i + 1].ArrivalTime - trainTimeTable[i].DepartureTime;
                var travelSpeed = Convert.ToInt32(trackLenght / travelTime.Value.TotalHours);

                var linkTravelTimes = new Dictionary<string, TimeSpan>(
                    TrackORMadv.GetLinkTravelTimes(
                        travelSpeed,
                        trainTimeTable[i].StationId,
                        trainTimeTable[i + 1].StationId));

                foreach (var link in linkTravelTimes)
                {
                    if (previusLink != null)
                    {
                        lock (LinksInUse)
                            LinksInUse.Remove(previusLink);

                        if (trainTimeTable[i + 1].DepartureTime != null)
                        {
                            var stopAtStationTime = Convert.ToInt32((trainTimeTable[i + 1].DepartureTime - trainTimeTable[i + 1].ArrivalTime).Value.Milliseconds);
                            if (stopAtStationTime > 0)
                                Thread.Sleep(stopAtStationTime / timeFastForward);
                        }
                    }

                    int waitTime = Convert.ToInt32((link.Value - fakeClock).TotalMilliseconds);
                    fakeClock += link.Value - fakeClock;

                    var currentLink = new LinkInUse()
                    {
                        LinkId = link.Key,
                        UsedByTrainId = trainId,
                        Direction = tripDirection
                    };

                    lock (LinksInUse)
                        if (LinksInUse.Find(l => (l.LinkId == link.Key && l.Direction != tripDirection)) != null)
                        {
                            Console.WriteLine($"TRAIN CRASH!!! " +
                                $"Train {trainId} and train " +
                                $"{LinksInUse.Find(l => l.LinkId == link.Key).UsedByTrainId} ON LINK {link.Key}");
                        }

                    lock (LinksInUse)
                        LinksInUse.Add(currentLink);

                    if (waitTime > 0)
                        Thread.Sleep(waitTime / timeFastForward);

                    previusLink = currentLink;
                }
            }
        }
    }
}