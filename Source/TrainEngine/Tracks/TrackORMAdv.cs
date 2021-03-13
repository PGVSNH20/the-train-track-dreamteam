using System;
using System.IO;
using System.Collections.Generic;
using System.Text;
using TrainEngine.Trains;
using System.Linq;

namespace TrainEngine.Tracks
{
    public class TrackORMAdv
    {
        public char[] TrackMap { get; set; }
        public List<Track> Tracks { get; } = new List<Track>();
        private char[,] _trainTrackMap;
        private (int x, int y) _prevPos = (0, 0);
        private (int x, int y) _currentPos = (0, 0);
        public string SourceFile { get; set; }

        public TrackORMAdv()
        {
            SourceFile = @"Data\traintrack4.txt";
            Read();
            AddTracks();
        }

        public TrackORMAdv(string sourceFile)
        {
            SourceFile = sourceFile;
            Read();
            AddTracks();
        }

        private void Read()
        {
            string[] dataString = File.ReadAllLines(SourceFile);
            int longestLineLength = 0;
            foreach (string line in dataString)
                if (line.Length > longestLineLength)
                    longestLineLength = line.Length;

            _trainTrackMap = new char[dataString.Length, longestLineLength];
            for (var i = 0; i < _trainTrackMap.GetLength(0); i++)
                for (var n = 0; n < _trainTrackMap.GetLength(1); n++)
                    _trainTrackMap[i, n] = ' ';

            for (int x = 0; x < dataString.Length; x++)
            {
                string line = dataString[x];
                for (int y = 0; y < dataString[x].Length; y++)
                {
                    if (line[y] == '*') { _currentPos.x = x; _currentPos.y = y; }
                    _trainTrackMap[x, y] = line[y];
                }
            }
        }

        public void PrintTrackMap()
        {
            Console.Write($"|x,y");
            for (var y = 0; y < _trainTrackMap.GetLength(1); y++)
                Console.Write($"|{y,-3}");
            Console.WriteLine($"|");

            RenderLineSeparator();

            for (var x = 0; x < _trainTrackMap.GetLength(0); x++)
            {
                Console.Write($"{x,4}");
                for (var y = 0; y < _trainTrackMap.GetLength(1); y++)
                {
                    Console.Write($"| {_trainTrackMap[x, y]} ");
                }
                Console.WriteLine($"|");
                RenderLineSeparator();
            }

            void RenderLineSeparator()
            {
                Console.Write($"{' ',4}");
                for (var y = 0; y < _trainTrackMap.GetLength(1); y++)
                    Console.Write($"|{"–––"}");
                Console.WriteLine($"|");
            }
        }

        private void AddTracks()
        {
            var track = new Track();
            while (track.EndStation == null)
            {
                char? scanResult = FindNext();

                if (IsTrackPart(scanResult))
                    track.NumberOfTrackParts++;
                if (scanResult == '=')
                    track.CrossingsAtTrackPart.Add(track.NumberOfTrackParts);

                while (track.StartStation == null)
                {
                    track.StartStation = AddStartStation(scanResult);
                }
                while (scanResult != null)
                {
                    scanResult = FindNext();
                    if (IsTrackPart(scanResult))
                        track.NumberOfTrackParts++;
                    if (scanResult == '=')
                        track.CrossingsAtTrackPart.Add(track.NumberOfTrackParts);

                    if (scanResult == '[')
                    {
                        var startPos = _prevPos;
                        track.EndStation = AddEndStation(scanResult);
                        if (!track.EndStation.IsEndStation)
                        {
                            _currentPos = startPos;
                            AddTracks();
                        }
                        break;
                    }
                    if (scanResult == '<')
                    {
                        AddBranchTrack(track);
                    }
                }
            }
            Tracks.Add(track);
        }

        private void AddBranchTrack(Track mainTrack)
        {
            var savedPrevPos = _prevPos;
            var savedCurrentPos = _currentPos;
            FindNext();
            var ignorPos = _currentPos;
            _prevPos = savedCurrentPos;
            _currentPos = savedCurrentPos;

            var branchTrack = new Track()
            {
                StartStation = mainTrack.StartStation,
                NumberOfTrackParts = mainTrack.NumberOfTrackParts + 1
            };
            foreach (var crossing in mainTrack.CrossingsAtTrackPart)
                branchTrack.CrossingsAtTrackPart.Add(crossing);

            while (branchTrack.EndStation == null)
            {
                char? scanResult2 = FindNext(ignorPos);
                while (scanResult2 != null)
                {
                    scanResult2 = FindNext();
                    if (IsTrackPart(scanResult2))
                        branchTrack.NumberOfTrackParts++;
                    if (scanResult2 == '=')
                        branchTrack.CrossingsAtTrackPart.Add(branchTrack.NumberOfTrackParts);

                    if (scanResult2 == '[')
                    {
                        var startPos = _prevPos;
                        var endPos = _currentPos;
                        branchTrack.EndStation = AddEndStation(scanResult2);
                        if (!branchTrack.EndStation.IsEndStation)
                        {
                            _currentPos = startPos;
                            AddTracks();
                        }
                        _currentPos = endPos;
                        break;
                    }
                    if (scanResult2 == '<')
                    {
                        AddBranchTrack(branchTrack);
                    }
                }
            }
            Tracks.Add(branchTrack);
            _prevPos = savedPrevPos;
            _currentPos = savedCurrentPos;
        }

        private bool IsTrackPart(char? scanResult)
        {
            if (scanResult == '-' ||
                scanResult == '>' ||
                scanResult == '<' ||
                scanResult == '/' ||
                scanResult == '=' ||
                scanResult == '\\')
                return true;
            return false;
        }

        private Station AddEndStation(char? scanResult)
        {
            var station = new Station();
            string stationId = string.Empty;
            while (scanResult != ']')
            {
                scanResult = FindNext();
                if (scanResult == ']')
                {
                    var savedPrevPos = _prevPos;
                    var savedCurrentPos = _currentPos;
                    scanResult = FindNext();
                    if (scanResult == null)
                    {
                        station.IsEndStation = true;
                    }
                    else station.IsEndStation = false;
                    _prevPos = savedPrevPos;
                    _currentPos = savedCurrentPos;
                    break;
                }
                stationId += scanResult.ToString();
            }
            station.Id = Convert.ToInt32(stationId);
            return station;
        }

        private Station AddStartStation(char? scanResult)
        {
            var station = new Station();
            string stationId = string.Empty;

            if (_trainTrackMap[_currentPos.x, _currentPos.y - 1] == '-')
                station.IsEndStation = false;

            while (scanResult != ']')
            {
                scanResult = FindNext();
                if (scanResult == ']')
                    break;
                stationId += scanResult.ToString();
            }
            station.Id = Convert.ToInt32(stationId);
            return station;
        }

        private char? FindNext((int x, int y)? ignorPosition = null)
        {
            (int x, int y) pOld = _prevPos;
            (int x, int y) p = _currentPos;
            _prevPos = _currentPos;
            int maxX = _trainTrackMap.GetLength(0);
            int maxY = _trainTrackMap.GetLength(1);
            (int x, int y)[] neighbors = new (int x, int y)[8];
            neighbors[0] = (p.x, p.y + 1);
            neighbors[1] = (p.x - 1, p.y);
            neighbors[2] = (p.x - 1, p.y + 1);
            neighbors[3] = (p.x - 1, p.y - 1);
            neighbors[4] = (p.x + 1, p.y + 1);
            neighbors[5] = (p.x + 1, p.y);
            neighbors[6] = (p.x + 1, p.y - 1);
            neighbors[7] = (p.x, p.y - 1);
            char? returnValue;

            foreach (var neighbor in neighbors)
                if (IgnoreThis(neighbor))
                    continue;
                else
                {
                    returnValue = CheckReturnValue(neighbor);
                    if (returnValue != ' ' && returnValue != null)
                        return returnValue;
                }

            return null;
            char? CheckReturnValue((int x, int y) p)
            {
                if ((_trainTrackMap[p.x, p.y]) != ' ')
                {
                    _currentPos = p;
                    return _trainTrackMap[p.x, p.y];
                }
                return null;
            }

            bool IgnoreThis((int x, int y) p)
            {
                if (p.x >= 0 &&
                    p.x < maxX &&
                    p.y >= 0 &&
                    p.y < maxY &&
                    p != pOld &&
                    p != ignorPosition)
                    return false;
                return true;
            }
        }

        public void GetMinTravelTime(int trainId, int startStationId, int endStationId)
        {
            List<Track> tripTracks = new List<Track>();
            
            tripTracks = FindTripTracks(startStationId, endStationId);

            var trains = new TrainsOrm();
            var train = trains.GetTrainById(trainId);
            double tripLengh = tripTracks.Sum(t => t.NumberOfTrackParts) * 10;
            double hours = tripLengh / train.MaxSpeed;
            var minTravelTime = TimeSpan.FromHours(hours);

            foreach (Track tmpTrack in tripTracks) Console.WriteLine(tmpTrack.ToString());

            Console.WriteLine($"travel time {minTravelTime}");


        }
        private List<Track> FindTripTracks(int startStationId, int endStationId)
        {
            List<Track> currentTracks = Tracks.FindAll(t => t.StartStation.Id == startStationId);
            foreach (Track currentTrack in currentTracks)
            {
                var tripTracks = new List<Track>();
                tripTracks.Add(currentTrack);
                if (currentTrack.EndStation.Id == endStationId)
                {
                    return tripTracks;
                }
                else
                {
                    var tmpResult = FindTripTracks(currentTrack.EndStation.Id, endStationId);
                    if (tmpResult != null)
                    {
                        tripTracks.AddRange(tmpResult);
                        return tripTracks;
                    }
                }
            }
            return null;
        }
    }
}