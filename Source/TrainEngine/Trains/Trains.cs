using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.Json;

namespace TrainEngine.Trains
{
    internal class Trains
    {
        public List<Train> TrainsList { get; set; }

        public Trains()
        {
            TrainsList = new List<Train>();
        }

        public void Read()
        {
            var jsonString = File.ReadAllText("Data/trains.json");
            TrainsList = JsonSerializer.Deserialize<List<Train>>(jsonString);
        }

        public void Write()
        {
            string jsonString = JsonSerializer.Serialize(TrainsList);
            File.WriteAllText("Data/trains.json", jsonString);
        }
    }
}