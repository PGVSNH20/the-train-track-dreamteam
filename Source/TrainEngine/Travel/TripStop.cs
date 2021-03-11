using System;

namespace TrainEngine.Travel
{
    public class TripStop
    {
        public int TrainId { set; get; }
        public int StationId { set; get; }
        public DateTime? DepartureTime { set; get; }
        public DateTime? ArrivalTime { get; set; }
    }
}