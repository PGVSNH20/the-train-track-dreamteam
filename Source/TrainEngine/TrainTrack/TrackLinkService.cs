using System;
using System.Collections.Generic;
using System.Text;

namespace TrainEngine.TrainTrack
{
    public class TrackLinkService
    {
        public List<Link> Links { get; } = new List<Link>();
        private char[,] _trainTrackMap;
        private (int x, int y) _prevPos = (0, 0);
        private (int x, int y) _currentPos = (0, 0);

        public TrackLinkService(string[] fileLines)
        {
            int longestLineLength = 0;
            foreach (string line in fileLines)
                if (line.Length > longestLineLength)
                    longestLineLength = line.Length;

            _trainTrackMap = new char[fileLines.Length, longestLineLength];
            for (var i = 0; i < _trainTrackMap.GetLength(0); i++)
                for (var n = 0; n < _trainTrackMap.GetLength(1); n++)
                    _trainTrackMap[i, n] = ' ';

            for (int x = 0; x < fileLines.Length; x++)
            {
                string line = fileLines[x];
                for (int y = 0; y < fileLines[x].Length; y++)
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

        public void AddLinks()
        {
            var link = new Link();
            while (link.EndStation == null)
            {
                char? scanResult = ScanNeighbours();

                if (IsLinkUnit(scanResult))
                    link.LinkUnitsCount++;
                if (scanResult == '=')
                    link.CrossingsAtUnit.Add(link.LinkUnitsCount);

                while (link.StartStation == null)
                {
                    link.StartStation = AddStartStation(scanResult);
                }
                while (scanResult != null)
                {
                    scanResult = ScanNeighbours();
                    if (IsLinkUnit(scanResult))
                        link.LinkUnitsCount++;
                    if (scanResult == '=')
                        link.CrossingsAtUnit.Add(link.LinkUnitsCount);

                    if (scanResult == '[')
                    {
                        var startPos = _prevPos;
                        link.EndStation = AddEndStation(scanResult);
                        if (!link.EndStation.IsLastStation)
                        {
                            _currentPos = startPos;
                            AddLinks();
                        }
                        break;
                    }
                    if (scanResult == '<')
                    {
                        AddBranchLink(link);
                    }
                }
            }
            Links.Add(link);
        }

        private void AddBranchLink(Link mainLink)
        {
            var savedPrevPos = _prevPos;
            var savedCurrentPos = _currentPos;
            ScanNeighbours();
            var ignorPos = _currentPos;
            _prevPos = savedCurrentPos;
            _currentPos = savedCurrentPos;

            var branchLink = new Link()
            {
                StartStation = mainLink.StartStation,
                LinkUnitsCount = mainLink.LinkUnitsCount + 1
            };
            foreach (var crossing in mainLink.CrossingsAtUnit)
                branchLink.CrossingsAtUnit.Add(crossing);

            while (branchLink.EndStation == null)
            {
                char? scanResult2 = ScanNeighbours(ignorPos);
                while (scanResult2 != null)
                {
                    scanResult2 = ScanNeighbours();
                    if (IsLinkUnit(scanResult2))
                        branchLink.LinkUnitsCount++;
                    if (scanResult2 == '=')
                        branchLink.CrossingsAtUnit.Add(branchLink.LinkUnitsCount);

                    if (scanResult2 == '[')
                    {
                        var startPos = _prevPos;
                        var endPos = _currentPos;
                        branchLink.EndStation = AddEndStation(scanResult2);
                        if (!branchLink.EndStation.IsLastStation)
                        {
                            _currentPos = startPos;
                            AddLinks();
                        }
                        _currentPos = endPos;
                        break;
                    }
                    if (scanResult2 == '<')
                    {
                        AddBranchLink(branchLink);
                    }
                }
            }
            Links.Add(branchLink);
            _prevPos = savedPrevPos;
            _currentPos = savedCurrentPos;
        }

        private bool IsLinkUnit(char? scanResult)
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
                scanResult = ScanNeighbours();
                if (scanResult == ']')
                {
                    var savedPrevPos = _prevPos;
                    var savedCurrentPos = _currentPos;
                    scanResult = ScanNeighbours();
                    if (scanResult == null)
                    {
                        station.IsLastStation = true;
                    }
                    else station.IsLastStation = false;
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
                station.IsLastStation = false;

            while (scanResult != ']')
            {
                scanResult = ScanNeighbours();
                if (scanResult == ']')
                    break;
                stationId += scanResult.ToString();
            }
            station.Id = Convert.ToInt32(stationId);
            return station;
        }

        private char? ScanNeighbours((int x, int y)? ignorPosition = null)
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
                if (NeighbourExists(neighbor))
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

            bool NeighbourExists((int x, int y) p)
            {
                if (p.x >= 0 &&
                    p.x < maxX &&
                    p.y >= 0 &&
                    p.y < maxY &&
                    p != pOld &&
                    p != ignorPosition)
                    return true;
                return false;
            }
        }
    }
}