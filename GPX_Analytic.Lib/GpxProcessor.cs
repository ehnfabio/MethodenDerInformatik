using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GPX_Analytic.Lib
{
    public class GpxProcessor
    {
        public static List<TrackInfo> GetTrackInfosFromDirectory(string directoryPath)
        {
            var trackInfos = new List<TrackInfo>();

            if (!Directory.Exists(directoryPath))
            {
                throw new DirectoryNotFoundException($"The directory '{directoryPath}' does not exist.");
            }

            var gpxFiles = Directory.GetFiles(directoryPath, "*.gpx");

            foreach (var gpxFile in gpxFiles)
            {
                var parser = new GpxParser(gpxFile);
                List<TrackPoint> trackPoints = parser.GetTrackPoints() ?? new List<TrackPoint>();

                double totalDistance = CalculateTotalDistance(trackPoints);
                TimeSpan duration = CalculateDuration(trackPoints);

                double distanceKm = totalDistance / 1000.0;
                double durationMinutes = duration.TotalMinutes;

                double paceMinPerKm = distanceKm > 0 ? durationMinutes / distanceKm : 0;
                double speedKmPerHour = duration.TotalHours > 0 ? distanceKm / duration.TotalHours : 0;

                trackInfos.Add(new TrackInfo
                {
                    FileName = Path.GetFileName(gpxFile),
                    TrackPoints = trackPoints,
                    DistanceInMeters = totalDistance,
                    Duration = duration,
                    PaceMinPerKm = paceMinPerKm,
                    SpeedKmPerHour = speedKmPerHour
                });
            }

            return trackInfos;
        }

        private static double CalculateTotalDistance(List<TrackPoint> points)
        {
            double total = 0.0;

            for (int i = 1; i < points.Count; i++)
            {
                var p1 = points[i - 1];
                var p2 = points[i];

                total += HaversineDistance(p1.Latitude, p1.Longitude, p2.Latitude, p2.Longitude);
            }

            return total;
        }

        private static TimeSpan CalculateDuration(List<TrackPoint> points)
        {
            DateTime? start = null;
            DateTime? end = null;

            foreach (var point in points)
            {
                if (point.Time.HasValue)
                {
                    if (!start.HasValue || point.Time.Value < start.Value)
                        start = point.Time.Value;

                    if (!end.HasValue || point.Time.Value > end.Value)
                        end = point.Time.Value;
                }
            }

            if (start.HasValue && end.HasValue)
                return end.Value - start.Value;
            else
                return TimeSpan.Zero;
        }

        //that's crazyyy
        private static double HaversineDistance(double lat1, double lon1, double lat2, double lon2)
        {
            const double R = 6371000; // radius der erde

            double dLat = ToRadians(lat2 - lat1);
            double dLon = ToRadians(lon2 - lon1);

            double a = Math.Sin(dLat / 2) * Math.Sin(dLat / 2) +
                       Math.Cos(ToRadians(lat1)) * Math.Cos(ToRadians(lat2)) *
                       Math.Sin(dLon / 2) * Math.Sin(dLon / 2);

            double c = 2 * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1 - a));

            return R * c;
        }

        private static double ToRadians(double degrees)
        {
            return degrees * Math.PI / 180.0;
        }
    }
}