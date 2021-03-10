using System;
using TrainEngine;
using System.IO;
using System.Threading;

namespace TrainConsole
{
    class Program
    {
        static void Main(string[] args)
        {
            // Step 1:
            // Parse the traintrack (Data/traintrack.txt) using ORM (see suggested code)
            // Parse the trains (Data/trains.txt)

            // Step 2:
            // Make the trains run in treads


            // Read the file and display it line by line.  

            /*TrackOrm trackOrm = new TrackOrm();
            var track = trackOrm.ParseTrackDescription(Properties.Resources.traintrack1);
            Console.WriteLine(track.NumberOfTrackParts);
            Console.ReadKey();*/
            
            TrackOrm newTrack = new TrackOrm();
            newTrack.ParseTrackDescription(Properties.Resources.traintrack3);

            
            Console.ReadKey();

        }
    }
}
