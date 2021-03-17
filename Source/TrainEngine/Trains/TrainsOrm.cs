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
        public List<Train> Trains { get; set; }
        public string SourceFile { get; set; }

        public TrainsOrm()
        {
            SourceFile = @"Data\trains.json";
            Trains = new List<Train>();
            _Read();
        }

        public TrainsOrm(string sourceFile)
        {
            SourceFile = sourceFile;
            Trains = new List<Train>();
            _Read();
        }

        private void _Read()
        {
            var jsonString = File.ReadAllText(SourceFile);
            Trains = JsonSerializer.Deserialize<List<Train>>(jsonString);
        }

        public void SaveToFile()
        {
            var options = new JsonSerializerOptions
            {
                Encoder = JavaScriptEncoder.Create(UnicodeRanges.BasicLatin),
                WriteIndented = true
            };
            string jsonString = JsonSerializer.Serialize(Trains, options);
            File.WriteAllText(SourceFile, jsonString);
        }

        public Train GetTrainById(int traindId)
        {
            return Trains.Find(t => t.Id == traindId);
        }
    }
}