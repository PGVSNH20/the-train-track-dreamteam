using System;
using System.Collections.Generic;
using System.Text;
using TrainEngine.Tracks;
using System.IO;
using System.Text.Json;
using System.Text.Encodings.Web;
using System.Text.Unicode;

namespace TrainEngine.ORM
{
    public class StationsORM
    {
        public List<Station> Stations { get; set; }

        public StationsORM()
        {
            Read();
        }

        public StationsORM Read()
        {
            var jsonString = File.ReadAllText("Data/stations.json");
            Stations = JsonSerializer.Deserialize<List<Station>>(jsonString);
            return this;
        }

        public StationsORM Write()
        {
            var options = new JsonSerializerOptions
            {
                Encoder = JavaScriptEncoder.Create(UnicodeRanges.BasicLatin),
                WriteIndented = true
            };
            string jsonString = JsonSerializer.Serialize(Stations, options);
            File.WriteAllText("Data/stations.json", jsonString);
            return this;
        }
    }
}