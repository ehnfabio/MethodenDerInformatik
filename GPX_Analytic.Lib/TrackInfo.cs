using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GPX_Analytic.Lib
{
    public class TrackInfo
    {
        public string FileName { get; set; }
        public List<TrackPoint> TrackPoints { get; set; }
        public double DistanceInMeters { get; set; }
        public TimeSpan Duration { get; set; }
        public double PaceMinPerKm { get; set; }
        public double SpeedKmPerHour { get; set; }

        public override string ToString()
        {
            string formattedDuration = Duration.ToString(@"hh\:mm\:ss");

            return $"File: {FileName}\n" +
                   $"Points: {TrackPoints?.Count ?? 0}\n" +
                   $"Distance: {DistanceInMeters:F2} meters\n" +
                   $"Duration: {formattedDuration}\n" +
                   $"Pace: {PaceMinPerKm:F2} min/km\n" +
                   $"Speed: {SpeedKmPerHour:F2} km/h\n" +
                   $"----------------------------";
        }
    }

}