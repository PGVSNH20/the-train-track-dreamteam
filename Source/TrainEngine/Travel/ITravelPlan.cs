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

        ITravelPlan SettActualTrain(int trainId);

        ITravelPlan StartAt(int stationId, string departureTime);

        ITravelPlan ArriveAt(int stationId, string ariveTime);

        ITravelPlan GenerateNewPlan(string fileName);

        public ITravelPlan AddToExistingPlan(string fileName);

        void Simulate(string fakeClock, int timeFastForward);
    }
}
