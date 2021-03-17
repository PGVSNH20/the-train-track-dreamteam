using System;
using System.Text.Json.Serialization;

namespace TrainEngine.Travel
{
    public class TripStop
    {
        public int TrainId { set; get; }
        public int StationId { set; get; }

        [JsonConverter(typeof(JsonTimeSpanConverter))]
        public TimeSpan? DepartureTime { set; get; }

        [JsonConverter(typeof(JsonTimeSpanConverter))]
        public TimeSpan? ArrivalTime { get; set; }
    }
}