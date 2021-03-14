using System;
using System.Collections.Generic;
using System.Text;

namespace TrainEngine.Tracks
{
    public class Track
    {
        public Station StartStation { get; set; }
        public Station EndStation { get; set; }
        public int NumberOfTrackParts { get; set; }
        public List<int> CrossingsAtTrackPart { set; get; }
        public List<RailroudSwitch> SwitchesAtTrackPart { set; get; }
        public List<Link> TrackLinks { set; get; }

        public Track()
        {
            CrossingsAtTrackPart = new List<int>();
            SwitchesAtTrackPart = new List<RailroudSwitch>();
        }
        public override string ToString()
        {
            var crossings = string.Empty;
            var switches = string.Empty;
            if (CrossingsAtTrackPart != null && CrossingsAtTrackPart.Count > 0)
            {
                crossings = $"and has {CrossingsAtTrackPart.Count} crossings";
            }
            if (SwitchesAtTrackPart != null && SwitchesAtTrackPart.Count > 0)
            {
                switches = $"and has {SwitchesAtTrackPart.Count} Railroad switches";
            }
            return ($"Track from {StartStation.Id} to {EndStation.Id} is {NumberOfTrackParts} units long {switches} {crossings}");
        }
    }
}