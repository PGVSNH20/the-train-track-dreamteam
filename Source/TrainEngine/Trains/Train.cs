using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TrainEngine.Trains
{
    class Train : ITrain
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int MaxSpeed { get; set; }
        public bool Operated { get; set; }
        public TrainPosition Position { get; set; }
        public List<Passanger> CurrentPassangers { get; set; }
        public int MaxPassengersCount { get; set; }

        public Train (int id, string name, int maxSpeed, bool operated)
        {
            Id = id;
            Name = name;
            MaxSpeed = maxSpeed;
            Operated = operated;
            CurrentPassangers = new List<Passanger>();

        }

        public void UpdatePassengers(List<Passanger> GettingOnPassengers, List<Passanger> GettingOffPassengers)
        {

            foreach (Passanger passenger in GettingOnPassengers)
            {
                if (CurrentPassangers.Count < MaxPassengersCount)
                    CurrentPassangers.Add(passenger);

                else throw new Exception();
            }

            foreach (Passanger passenger in GettingOffPassengers)
            {
                if (CurrentPassangers.Any(p => p.Id == passenger.Id))
                    CurrentPassangers.Remove(p => p.Id == passenger.Id);
            }
        }

        public void UpdateTrainPosition(TrainPosition trainPosition)
        {
            throw new NotImplementedException();
        }
    }
}
