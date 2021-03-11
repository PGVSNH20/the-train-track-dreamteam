using System;
using System.Collections.Generic;
using System.Text;
using TrainEngine.Tracks;
using System.IO;
using System.Text.Json;

namespace TrainEngine.ORM
{
    public class StationsOrm
    {
        public List<Station> Stations { get; set; }

        public StationsOrm()
        {
            Read();
        }

        public StationsOrm Read()
        {
            var jsonString = File.ReadAllText("Data/stations_test.json");
            Stations = JsonSerializer.Deserialize<List<Station>>(jsonString);
            return this;
        }

        public StationsOrm Write()
        {
            string jsonString = JsonSerializer.Serialize(Stations);
            File.WriteAllText("Data/stations_test.json", jsonString);
            return this;
        }
    }
}
