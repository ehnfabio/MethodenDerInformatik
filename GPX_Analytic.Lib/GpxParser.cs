using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace GPX_Analytic.Lib
{
    public class GpxParser
    {
        private readonly string _filePath;
        private static readonly XNamespace Ns = "http://www.topografix.com/GPX/1/1";

        public GpxParser(string filePath)
        {
            if (string.IsNullOrWhiteSpace(filePath))
                throw new ArgumentException("Dateipfad darf nicht leer sein.", nameof(filePath));

            _filePath = filePath;
        }

        public List<TrackPoint> GetTrackPoints()
        {
            var doc = XDocument.Load(_filePath);

            var points = new List<TrackPoint>();

            foreach (var elem in doc.Descendants(Ns + "trkpt"))
            {
                // Attribute lat/lon parsen
                var lat = double.Parse(elem.Attribute("lat").Value, CultureInfo.InvariantCulture);
                var lon = double.Parse(elem.Attribute("lon").Value, CultureInfo.InvariantCulture);

                // Optionales Zeit-Element
                DateTime? time = null;
                var timeElem = elem.Element(Ns + "time");
                if (timeElem != null)
                {
                    // ISO 8601, UTC
                    time = DateTime.Parse(timeElem.Value, null, DateTimeStyles.AdjustToUniversal);
                }

                points.Add(new TrackPoint
                {
                    Latitude = lat,
                    Longitude = lon,
                    Time = time
                });
            }

            return points;
        }
    }
}