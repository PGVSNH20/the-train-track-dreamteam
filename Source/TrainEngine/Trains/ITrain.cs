using System;
using System.Collections.Generic;
using System.Text;

namespace TrainEngine.Trains
{
    internal interface ITrain
    {
        int Id { set; }
        string Name { set; }
        int MaxSpeed { set; }
        bool Operated { set; }
        int MaxPassengersCount { set; }
        List<Passanger> CurrentPassangers { set; }

        void UpdatePassengers(List<Passanger> GettingOnPassengers, List<Passanger> GettingOffPassengers);

        void UpdateTrainPosition(ITrainPosition trainPosition);
    }
}