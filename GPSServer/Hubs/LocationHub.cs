using GPSServer.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.AspNet.SignalR;
using System;
using System.Web;
using System.Web.Http;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using System.Device.Location;

namespace GPSServer.Hubs
{
    [HostAuthentication(DefaultAuthenticationTypes.ExternalBearer)]
    [Microsoft.AspNet.SignalR.Authorize]
    public class LocationHub : Hub
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        private ApplicationUserManager _userManager;
        public List<Threat> SendLocation(Location location, string phoneNumber)
        {
            Location threatLocation = new Location();
            if (location != null)
            {
                location.LastUpdate = DateTime.UtcNow;
                Clients.Group(phoneNumber).newLocation(location);
                location.UserId = Context.User.Identity.GetUserId();
                db.Locations.Add(location);
                threatLocation = db.Locations.AsEnumerable().Where(l => new GeoCoordinate(l.Latitude, l.Longitude).GetDistanceTo(new GeoCoordinate(location.Latitude, location.Longitude)) <= 200).FirstOrDefault();
                try
                {
                    db.SaveChanges();
                }
                catch (Exception)
                { }
            }
            if (threatLocation != null)
                return db.ThreatLocations.Where(t => t.LocationId == threatLocation.Id).Select(s => s.Threat).ToList();
            else
                return new List<Threat>();
        }

        public void JoinGroup(string phoneNumber)
        {
            ApplicationUser user = UserManager.FindByPhone(phoneNumber);
            if (!user.BlockedList.Exists(i => i.PhoneNumber == phoneNumber))
                Groups.Add(Context.ConnectionId, phoneNumber);
        }

        public void UnJoinGroup(string phoneNumber)
        {
            Groups.Remove(Context.ConnectionId, phoneNumber);
        }

        public ApplicationUserManager UserManager
        {
            get
            {
                return _userManager ?? HttpContext.Current.Request.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            private set
            {
                _userManager = value;
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

    }
}