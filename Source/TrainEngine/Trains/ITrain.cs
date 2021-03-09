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
        TrainPosition Position { set; }
        List<Passanger> CurrentPassangers { set; }
    }
}