using System;
using System.Collections.Generic;
using System.Text;

namespace TrainEngine.Travel
{
    class TravelPlan : ITravelPlan
    {
        public List<TripStop> TimeTable => throw new NotImplementedException();

        public object Train => throw new NotImplementedException();

        public void Load(string path)
        {
            throw new NotImplementedException();
        }

        public void Save(string path)
        {
            throw new NotImplementedException();
        }
    }
}
