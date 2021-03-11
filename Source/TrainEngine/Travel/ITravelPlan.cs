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

        void Save(string path);
        void Load(string path);

    }
}
