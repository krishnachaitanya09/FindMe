using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace GPSServer.Models
{
    public class Block
    {
        [Key]
        public string PhoneNumber { get; set; }
        public string UserId { get; set; }

        [JsonIgnore]
        [ForeignKey("UserId")]
        public virtual ApplicationUser User { get; set; }
    }
}