using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace GPSServer.Models
{
    public class Session
    {
        public string SessionID { get; set; }
        public bool IsActive { get; set; }
        public virtual ICollection<Location> Locations { get; set; }
    }
}