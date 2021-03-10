using System;
using System.IO;

namespace TrainConsole
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Train track!");
            // Step 1:
            // Parse the traintrack (Data/traintrack.txt) using ORM (see suggested code)
            // Parse the trains (Data/trains.txt)

            // Step 2:
            // Make the trains run in treads

            string[] line = File.ReadAllLines("Data/traintrack1.txt");
            Console.WriteLine(line[0]);


        }
    }
}
