using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.Spatial;
using System.Linq;
using System.Web;

namespace GPSServer.Models
{
    public class Location
    {
        [Key]
        public int Id { get; set; }
        public string UserId { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public DateTime LastUpdate { get; set; }
        public float? Speed { get; set; }
        public int? Direction { get; set; }
        public float? Distance { get; set; }
        public DateTime GpsTime { get; set; }
        public string LocationMethod { get; set; }
        public int? Accuracy { get; set; }
        public bool Safe { get; set; }

        [JsonIgnore]
        [ForeignKey("UserId")]
        public virtual ApplicationUser User { get; set; }

        public bool ShouldSerializeId()
        {
            return false;
        }

        public bool ShouldSerializeUserId()
        {
            return false;
        }
        public bool ShouldSerializeSpeed()
        {
            return false;
        }
        public bool ShouldSerializeDirection()
        {
            return false;
        }
        public bool ShouldSerializeDistance()
        {
            return false;
        }
        public bool ShouldSerializeGpsTime()
        {
            return false;
        }
        public bool ShouldSerializeLocationMethod()
        {
            return false;
        }
        public bool ShouldSerializeAccuracy()
        {
            return false;
        }
    }
}