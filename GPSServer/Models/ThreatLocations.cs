using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace GPSServer.Models
{
    public class ThreatLocations
    {
        public int Id { get; set; }
        public int ThreatId { get; set; }
        public int LocationId { get; set; }
        public int Frequency { get; set; }

        [ForeignKey("ThreatId")]
        public virtual Threat Threat { get; set; }
        [ForeignKey("LocationId")]
        public virtual Location Location { get; set; }
    }
}