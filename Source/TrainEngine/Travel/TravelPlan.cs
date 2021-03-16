using System;
using System.Collections.Generic;
using System.Text;
using TrainEngine.Tracks;
using TrainEngine.Trains;

namespace TrainEngine.Travel
{
    class TravelPlan : ITravelPlan
    {
        public List<TripStop> TimeTable { get; set; }

        public Train Train { get; set; }

        private TrackORM _trackORM { get; set; }

        public TravelPlan()
        {
            _trackORM = new TrackORM();

        }


        public ITravelPlan AddToExistingPlan(string fileName)
        {
            throw new NotImplementedException();
        }

        public ITravelPlan ArriveAt(int stationId, TimeSpan ariveTime)
        {
            TripStop tripstop = new TripStop();
            tripstop.StationId = stationId;
        }

        public ITravelPlan GenerateNewPlan(string fileName)
        {
            throw new NotImplementedException();
        }

        public ITravelPlan SettActualTrain(int trainId)
        {
            throw new NotImplementedException();
        }

        public void Simulate(string fakeClock, int timeFastForward)
        {
            throw new NotImplementedException();
        }

        public ITravelPlan StartAt(int stationId, string departureTime)
        {
            throw new NotImplementedException();
        }
    }
}
