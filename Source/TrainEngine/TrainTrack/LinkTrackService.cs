using System;
using System.Collections.Generic;
using System.Text;

namespace TrainEngine.TrainTrack
{
    public class LinkTrackService
    {
        public List<Link> Links { set; get; } = new List<Link>();
        private char[,] _trainTrackMap;
        private (int x, int y) _prevPos = (0, 0);
        private (int x, int y) _currentPos = (0, 0);

        public LinkTrackService(string[] fileLines)
        {
            int longestLineLength = 0;
            foreach (string line in fileLines)
                if (line.Length > longestLineLength)
                    longestLineLength = line.Length;

            _trainTrackMap = new char[fileLines.Length, longestLineLength];

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

        public void AddNewLink()
        {
            var link = new Link();
            while (link.EndStationId == null)
            {
                char? scanResult = ScanPointNeibours();
                while (link.StartStationId == null)
                {
                    link.StartStationId = AddStation(scanResult);
                }
                while ((scanResult = ScanPointNeibours()) != null)
                {
                    if (scanResult == '[')
                    {
                        link.EndStationId = AddStation(scanResult);
                        break;
                    }
                    else link.LinkUnitsCount++;
                    if (scanResult == '<')
                    {
                        var savedPrevPos = _prevPos;
                        var savedCurrentPos = _currentPos;
                        ScanPointNeibours();
                        var ignorPos = _currentPos;
                        _prevPos = savedCurrentPos;
                        _currentPos = savedCurrentPos;

                        var newLink = new Link()
                        {
                            StartStationId = link.StartStationId,
                            LinkUnitsCount = link.LinkUnitsCount
                        };

                        while (newLink.EndStationId == null)
                        {
                            char? scanResult2 = ScanPointNeibours(ignorPos);
                            while ((scanResult2 = ScanPointNeibours()) != null)
                            {
                                if (scanResult2 == '[')
                                {
                                    newLink.EndStationId = AddStation(scanResult2);
                                    break;
                                }
                                else newLink.LinkUnitsCount++;
                            }
                        }

                        Links.Add(newLink);
                        _prevPos = savedCurrentPos;
                        _currentPos = savedCurrentPos;
                    }
                    if (scanResult == '[')
                    {
                        link.EndStationId = AddStation(scanResult);
                        break;
                    }
                }
            }
            Links.Add(link);
        }

        private int AddStation(char? scanResult)
        {
            string stationId = string.Empty;
            while (scanResult != ']')
            {
                scanResult = ScanPointNeibours();
                if (scanResult == ']') break;
                stationId += scanResult.ToString();
            }
            return Convert.ToInt32(stationId);
        }

        private char? ScanPointNeibours((int x, int y)? ignorPosition = null)
        {
            (int x, int y) pOld = _prevPos;
            (int x, int y) p = _currentPos;
            _prevPos = _currentPos;
            int maxX = _trainTrackMap.GetLength(0);
            int maxY = _trainTrackMap.GetLength(1);
            (int x, int y)[] neighbors = new (int x, int y)[8];
            neighbors[0] = (p.x - 1, p.y - 1);
            neighbors[1] = (p.x - 1, p.y);
            neighbors[2] = (p.x - 1, p.y + 1);
            neighbors[3] = (p.x, p.y + 1);
            neighbors[4] = (p.x + 1, p.y + 1);
            neighbors[5] = (p.x + 1, p.y);
            neighbors[6] = (p.x + 1, p.y - 1);
            neighbors[7] = (p.x, p.y - 1);
            char? returnValue;

            foreach (var neighbor in neighbors)
                if (CheckNeibhorAvailability(neighbor))
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

            bool CheckNeibhorAvailability((int x, int y) p)
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