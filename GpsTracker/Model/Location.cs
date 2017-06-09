using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GPSTracker.Model
{
    class Location
    {
        public int GPSLocationID { get; set; }
        public string UserId { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }

        private DateTime _lastUpdate;
        public DateTime LastUpdate
        {
            get { return _lastUpdate; }
            set
            {
                _lastUpdate = value.ToLocalTime();
            }
        }

        public float? Speed { get; set; }
        public int? Direction { get; set; }
        public float? Distance { get; set; }
        public DateTime GpsTime { get; set; }
        public string LocationMethod { get; set; }
        public int? Accuracy { get; set; }
        public ICollection<Threat> Threats { get; set; }
    }
}
