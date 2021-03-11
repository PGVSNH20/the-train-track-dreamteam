using System;
using System.Collections.Generic;
using System.Text;
using TrainEngine.Tracks;
using System.IO;

namespace TrainEngine.ORM
{
    class StationsOrm
    {
        public List<Station> Stations { get; set; }

        public StationsOrm()
        {
            Read();
        }

        private void Read()
        {
            string[] line = File.ReadAllLines("Data/stations.txt");         
        }
    }
}
