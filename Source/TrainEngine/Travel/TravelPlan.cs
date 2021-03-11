using System;
using System.Collections.Generic;
using System.Text;
using TrainEngine.Trains;

namespace TrainEngine.Travel
{
    class TravelPlan : ITravelPlan
    {
        public List<TripStop> TimeTable { get; set; }

         public Train Train { get; set; }

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
