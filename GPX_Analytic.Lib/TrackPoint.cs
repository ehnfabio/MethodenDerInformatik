using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GPX_Analytic.Lib
{
    public class TrackPoint
    {
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public DateTime? Time { get; set; }
    }
}
