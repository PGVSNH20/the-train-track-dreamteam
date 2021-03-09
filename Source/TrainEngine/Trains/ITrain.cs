using System;
using System.Collections.Generic;
using System.Text;

namespace TrainEngine.Trains
{
    internal interface ITrain
    {
        public int Id { set; }
        string Name { set; }
        int MaxSpeed { set; }
        bool Operated { set; }
        TrainPosition Position { set; }
    }
}