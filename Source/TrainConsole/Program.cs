using System;
using System.IO;
using TrainEngine.TrainTrack;

namespace TrainConsole
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            Console.WriteLine("Train track!");
            // Step 1:
            // Parse the traintrack (Data/traintrack.txt) using ORM (see suggested code)
            // Parse the trains (Data/trains.txt)

            // Step 2:
            // Make the trains run in treads
            string[] lines = System.IO.File.ReadAllLines(@"C:\Dev\Repos\dataankomst_kurs\the-train-track-dreamteam\Data\traintrack3.txt");
            var linkTrackService = new LinkTrackService(lines);

            linkTrackService.AddNewLink();

            Console.ReadKey();
        }
    }
}