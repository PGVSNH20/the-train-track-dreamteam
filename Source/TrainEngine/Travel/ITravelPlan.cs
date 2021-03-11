using System;
using System.Collections.Generic;
using System.Text;
using TrainEngine.Trains;

namespace TrainEngine.Travel
{
    public interface ITravelPlan
    {
        List<TripStop> TimeTable { get; }

        Train Train { get; }

        ITravelPlan StartAt(int stationId, DateTime departureTime);

        ITravelPlan ArriveAt(int stationId, DateTime ariveTime);

        ITravelPlan GeneratePlan();

        void Read();

        void Write();
    }
}