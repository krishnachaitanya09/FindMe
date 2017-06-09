using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace GPSServer.Models
{
    public class Threat
    {
        public int Id { get; set; }

        [Display(Name = "Incident")]
        public string Type { get; set; }
    }
}