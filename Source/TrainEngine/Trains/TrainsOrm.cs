using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Unicode;

namespace TrainEngine.Trains
{
    public class TrainsOrm
    {
        public List<Train> TrainsList { get; set; }

        public TrainsOrm()
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
            var options = new JsonSerializerOptions
            {
                Encoder = JavaScriptEncoder.Create(UnicodeRanges.BasicLatin),
                WriteIndented = true
            };
            string jsonString = JsonSerializer.Serialize(TrainsList, options);
            File.WriteAllText("Data/trains.json", jsonString);
        }
    }
}