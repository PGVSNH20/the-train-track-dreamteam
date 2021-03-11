using System;

namespace TrainEngine.Travel
{
    public class TripStop
    {
        public int TrainId { set; get; }
        public int StationId { set; get; }
        public TimeSpan? DepartureTime { set; get; }
        public TimeSpan? ArrivalTime { get; set; }
    }
}