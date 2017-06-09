using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace GPSServer.Models
{
    public class CrowdSource
    {
        public int Id { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        [Required]
        public string Location { get; set; }

        [Required]
        [Display(Name = "Incident")]
        public int ThreatId { get; set; }

        [Display(Name="Incident")]
        public Threat Threat { get; set; }
        public DateTime Date { get; set; }
    }
}