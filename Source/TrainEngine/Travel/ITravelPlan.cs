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

        ITravelPlan StartAt(int stationId, string departureTime);

        ITravelPlan ArriveAt(int stationId, string ariveTime);

        ITravelPlan GeneratePlan(string fileName);

        ITravelPlan LoadPlan(string fileName);
    }
}