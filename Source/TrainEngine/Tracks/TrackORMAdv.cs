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
                char? trackSymbol = FindNextSymbol();

                if (IsTrackPart(trackSymbol))
                    track.NumberOfTrackParts++;

                if (trackSymbol == '=')
                    track.CrossingsAtTrackPart.Add(track.NumberOfTrackParts);

                while (track.StartStation == null)
                {
                    track.StartStation = AddStartStation(trackSymbol);
                }
                while (trackSymbol != null)
                {
                    trackSymbol = FindNextSymbol();
                    if (IsTrackPart(trackSymbol))
                        track.NumberOfTrackParts++;

                    if (trackSymbol == '=')
                        track.CrossingsAtTrackPart.Add(track.NumberOfTrackParts);

                    if (trackSymbol == '>' || trackSymbol == '<')
                    {
                        var railroadSwitch = new RailroudSwitch()
                        {
                            Id = $"RS:X{_currentPos.x}Y{_currentPos.y}",
                            AttTrackPart = track.NumberOfTrackParts
                        };
                        track.SwitchesAtTrackPart.Add(railroadSwitch);
                        Console.WriteLine($"added {railroadSwitch.Id} to {track.StartStation.Id}");
                    }

                    if (trackSymbol == '[')
                    {
                        var startPos = _prevPos;
                        track.EndStation = AddEndStation(trackSymbol);
                        if (!track.EndStation.IsEndStation)
                        {
                            _currentPos = startPos;
                            AddTracks();
                        }
                        break;
                    }
                    
                    if (trackSymbol == '<')
                    {
                        AddBranchTrack(track);
                    }
                }
            }
            track.TrackLinks = FindTrackLinks(track);
            Tracks.Add(track);
        }

        private void AddBranchTrack(Track mainTrack)
        {
            var savedPrevPos = _prevPos;
            var savedCurrentPos = _currentPos;
            FindNextSymbol();
            var ignorPos = _currentPos;
            _prevPos = savedCurrentPos;
            _currentPos = savedCurrentPos;

            var branchTrack = new Track()
            {
                StartStation = mainTrack.StartStation,
                NumberOfTrackParts = mainTrack.NumberOfTrackParts + 1,

            };
            foreach (var crossing in mainTrack.CrossingsAtTrackPart)
                branchTrack.CrossingsAtTrackPart.Add(crossing);
            foreach (var railroudSwitch in mainTrack.SwitchesAtTrackPart)
            {
                var newRailroudSwitch = new RailroudSwitch()
                {
                    Id = railroudSwitch.Id,
                    AttTrackPart = railroudSwitch.AttTrackPart
                };
                branchTrack.SwitchesAtTrackPart.Add(newRailroudSwitch);
            }
                
            while (branchTrack.EndStation == null)
            {
                char? trackSymbol = FindNextSymbol(ignorPos);
                while (trackSymbol != null)
                {
                    trackSymbol = FindNextSymbol();
                    if (IsTrackPart(trackSymbol))
                        branchTrack.NumberOfTrackParts++;
                    if (trackSymbol == '=')
                        branchTrack.CrossingsAtTrackPart.Add(branchTrack.NumberOfTrackParts);
                    if (trackSymbol == '>' || trackSymbol == '<')
                    {
                        var railroadSwitch = new RailroudSwitch()
                        {
                            Id = $"RS:X{_currentPos.x}Y{_currentPos.y}",
                            AttTrackPart = branchTrack.NumberOfTrackParts
                        };
                        branchTrack.SwitchesAtTrackPart.Add(railroadSwitch);
                    }

                    if (trackSymbol == '[')
                    {
                        var startPos = _prevPos;
                        var endPos = _currentPos;
                        branchTrack.EndStation = AddEndStation(trackSymbol);
                        if (!branchTrack.EndStation.IsEndStation)
                        {
                            _currentPos = startPos;
                            AddTracks();
                        }
                        _currentPos = endPos;
                        break;
                    }
                    if (trackSymbol == '<')
                    {
                        AddBranchTrack(branchTrack);
                    }
                }
            }
            branchTrack.TrackLinks = FindTrackLinks(branchTrack);
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
                scanResult = FindNextSymbol();
                if (scanResult == ']')
                {
                    var savedPrevPos = _prevPos;
                    var savedCurrentPos = _currentPos;
                    scanResult = FindNextSymbol();
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
                scanResult = FindNextSymbol();
                if (scanResult == ']')
                    break;
                stationId += scanResult.ToString();
            }
            station.Id = Convert.ToInt32(stationId);
            return station;
        }

        private char? FindNextSymbol((int x, int y)? ignorPosition = null)
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

        public TimeSpan GetMinTravelTime(int trainId, int startStationId, int endStationId)
        {            
            var tripTracks = FindTripTracks(startStationId, endStationId);

            var trains = new TrainsOrm();
            var maxSpeed = trains.GetTrainById(trainId).MaxSpeed;
            double tripLengh = tripTracks.Sum(t => t.NumberOfTrackParts) * 10;
            double hours = tripLengh / maxSpeed;
            var minTravelTime = TimeSpan.FromHours(hours);

            return minTravelTime;
        }

        public Dictionary<string, TimeSpan> GetLinkMinTravelTimes(
            int trainId, 
            int startStationId, 
            int endStationId)
        {
            Dictionary<string, TimeSpan> linkTravelTimes = new Dictionary<string, TimeSpan>();
            var trains = new TrainsOrm();
            var maxSpeed = trains.GetTrainById(trainId).MaxSpeed;

            var tripTracks = FindTripTracks(startStationId, endStationId);

            TimeSpan timeTrack = new TimeSpan(0);

            foreach (var tripTrack in tripTracks)
            {
                foreach (var link in tripTrack.TrackLinks)
                {
                    double linkLenghs = link.NumberOfTrackParts * 10;
                    double travelTimeInHours = linkLenghs / maxSpeed;
                    var minTravelTime = TimeSpan.FromHours(travelTimeInHours);
                    linkTravelTimes.Add(link.LinkId, minTravelTime);
                }
            }

            return linkTravelTimes;
        }

        public TimeSpan GetTravelTime(int speed, int startStationId, int endStationId)
        {
            var tripTracks = FindTripTracks(startStationId, endStationId);

            var trains = new TrainsOrm();
            double tripLengh = tripTracks.Sum(t => t.NumberOfTrackParts) * 10;
            double hours = tripLengh / speed;
            var minTravelTime = TimeSpan.FromHours(hours);

            return minTravelTime;
        }

        public Dictionary<string, TimeSpan> GetLinkTravelTimes(
            int speed,
            int startStationId,
            int endStationId)
        {
            Dictionary<string, TimeSpan> linkTravelTimes = new Dictionary<string, TimeSpan>();
            var trains = new TrainsOrm();

            var tripTracks = FindTripTracks(startStationId, endStationId);

            foreach (var tripTrack in tripTracks)
            {
                foreach (var link in tripTrack.TrackLinks)
                {
                    double linkLenghs = link.NumberOfTrackParts * 10;
                    double travelTimeInHours = linkLenghs / speed;
                    var minTravelTime = TimeSpan.FromHours(travelTimeInHours);
                    linkTravelTimes.Add(link.LinkId, minTravelTime);
                }
            }

            return linkTravelTimes;
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

        private List<Link> FindTrackLinks(Track track)
        {
            var trackLinks = new List<Link>();
            var trackPartCounter = 0;
            string idFirstPart = Convert.ToString(track.StartStation.Id);
            foreach (var railroadSwitch in track.SwitchesAtTrackPart)
            {
                trackLinks.Add(CreateLink(idFirstPart, railroadSwitch.Id, railroadSwitch.AttTrackPart));
                idFirstPart = railroadSwitch.Id;
            }
            trackLinks.Add(CreateLink(idFirstPart, Convert.ToString(track.EndStation.Id), track.NumberOfTrackParts));
            return trackLinks;

            Link CreateLink(string idFirstpart, string idSecondPart, int switchAttTrackpart)
            {
                var id = $"{idFirstpart}-{idSecondPart}";

                var numberOfTrackParts = switchAttTrackpart - trackPartCounter;
                trackPartCounter += switchAttTrackpart;

                var newLink = new Link();
                newLink.LinkId = id;
                newLink.NumberOfTrackParts = numberOfTrackParts;
                return newLink;
            }
        }
    }
}