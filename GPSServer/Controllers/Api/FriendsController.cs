using GPSServer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.Cookies;
using Microsoft.Owin.Security.OAuth;
using System.Web.Http.Description;

namespace GPSServer.Controllers.Api
{
    [HostAuthentication(DefaultAuthenticationTypes.ExternalBearer)]
    [Authorize]
    public class FriendsController : ApiController
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        private ApplicationUserManager _userManager;

        public IHttpActionResult PostFriends(List<Friends> friendsList)
        {
            List<Friends> friends = new List<Models.Friends>();
            foreach (Friends friend in friendsList)
            {
                ApplicationUser user = db.Users.Where(u => u.PhoneNumber.Contains(friend.PhoneNumber)).FirstOrDefault();
                if (user != null)
                {
                    friends.Add(new Friends { Name = user.Name, Email = user.Email, PhoneNumber = user.PhoneNumber, ProfilePicUrl = user.ProfilePicUrl });
                }
            }
            return Ok(friends);
        }

        public IHttpActionResult BlockFriends(List<Block> blockList)
        {
            foreach (Block friend in blockList)
            {
                ApplicationUser user = db.Users.Where(u => u.PhoneNumber.Contains(friend.PhoneNumber)).FirstOrDefault();
                if (user != null)
                {
                    friend.UserId = user.Id;
                    db.Block.Add(friend);
                    try
                    {
                        db.SaveChanges();
                    }
                    catch (Exception)
                    { }
                }
            }
            return Ok();
        }

        public IHttpActionResult RemoveFromBlockList(List<Block> blockList)
        {
            foreach (Block friend in blockList)
            {
                ApplicationUser user = db.Users.Where(u => u.PhoneNumber.Contains(friend.PhoneNumber)).FirstOrDefault();
                if (user != null)
                {
                    friend.UserId = user.Id;
                    //db.Block.Attach(friend);
                    db.Entry(friend).State = System.Data.Entity.EntityState.Deleted;
                    //db.Block.Remove(friend);
                }
            }
            try
            {
                db.SaveChanges();
            }
            catch (Exception)
            { }
            return Ok();
        }

        [ResponseType(typeof(Location))]
        public IHttpActionResult GetRecentLocation(string phoneNumber)
        {
            ApplicationUser user = db.Users.Where(u => u.PhoneNumber.Contains(phoneNumber)).FirstOrDefault();
            Location location = db.Locations.Where(u => u.User.PhoneNumber == user.PhoneNumber).OrderByDescending(d => d.LastUpdate).FirstOrDefault();
            return Ok(location);
        }


        public ApplicationUserManager UserManager
        {
            get
            {
                return _userManager ?? Request.GetOwinContext().GetUserManager<ApplicationUserManager>();
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
                UserManager.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
