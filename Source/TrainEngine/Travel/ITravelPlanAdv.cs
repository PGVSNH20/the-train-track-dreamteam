using System;
using System.Collections.Generic;
using System.Text;
using TrainEngine.Tracks;
using TrainEngine.Trains;

namespace TrainEngine.Travel
{
    public interface ITravelPlanAdv
    {
        List<TripStop> TimeTable { get; }

        Train Train { get; }

        ITravelPlanAdv SettActualTrain(int trainId);

        ITravelPlanAdv StartAt(int stationId, string departureTime);

        ITravelPlanAdv ArriveAt(int stationId, string ariveTime);

        ITravelPlanAdv GenerateNewPlan(string fileName);

        public ITravelPlanAdv AddToExistingPlan(string fileName);

        ITravelPlanAdv LoadPlan(string fileName);

        void Simulate(string fakeClock, int timeFastForward);
    }
}