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
        public List<Station> Stations { get => _stations; }
        private List<Station> _stations;

        public StationsORM()
        {
            _Read();
        }

        public void AddStation(Station newStation)
        {
            if (_stations.Exists(s => s.Id == newStation.Id))
            {
                _stations.Remove(_stations.Find(s => s.Id == newStation.Id));
            };
            _stations.Add(newStation);
            _Write();
        }

        private StationsORM _Read()
        {
            var jsonString = File.ReadAllText("Data/stations.json");
            _stations = JsonSerializer.Deserialize<List<Station>>(jsonString);
            return this;
        }

        private StationsORM _Write()
        {
            var options = new JsonSerializerOptions
            {
                Encoder = JavaScriptEncoder.Create(UnicodeRanges.All),
                WriteIndented = true
            };
            string jsonString = JsonSerializer.Serialize(_stations, options);
            File.WriteAllText("Data/stations.json", jsonString);
            return this;
        }
    }
}