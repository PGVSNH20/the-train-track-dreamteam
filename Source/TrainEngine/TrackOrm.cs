using System;
using System.IO;

namespace TrainEngine
{
    public class TrackOrm
    {
        public TrackDescription ParseTrackDescription(string track)
        {
            string aLine;
            TrackDescription trackNumber = new TrackDescription();
            char[][] trackMap = new char[7][];
            for (int i = 0; i < 7; i++)
            {
                trackMap[i] = new char[50];
            }

            StringReader strReader = new StringReader(track);
            while (true)
            {
                int i = new int();
                int line = new int();
                aLine = strReader.ReadLine();
                if (aLine != null)
                {
                    foreach (var c in aLine)
                    {
                        Console.WriteLine(line);
                        trackMap[line][i] = c;
                        i++;
                    }
                    line++;
                    i = 0;
                }
                else
                {
                    break;
                }
                Console.WriteLine(trackMap[0][20]);
            }

            for (int i = 0; i < 7; i++)
            {
                int length = trackMap[i].Length;

                for (int a = 0; a < length; a++)
                {
                    var trackChar = trackMap[i][a];
                    Console.Write(i + " " + a);
                    Console.Write(trackChar);
                    Console.ReadKey();
                }
                
                Console.WriteLine();
            }
            
            trackNumber.NumberOfTrackParts++;
            return trackNumber;
            
        }
    }
}