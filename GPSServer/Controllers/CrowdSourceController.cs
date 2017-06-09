using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using GPSServer.Models;

namespace GPSServer.Controllers
{
    public class CrowdSourceController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: CrowdSource/Create
        public ActionResult Index()
        {
            ViewBag.ThreatId = new SelectList(db.Threats.Take(5).OrderBy(s => s.Type == "Others"), "Id", "Type");
            return View();
        }

        // POST: CrowdSource/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Index(CrowdSource crowdSource)
        {
            if (ModelState.IsValid)
            {
                Location location = db.Locations.Add(new Location()
                {
                    Latitude = crowdSource.Latitude,
                    Longitude = crowdSource.Longitude,
                    GpsTime = crowdSource.Date.ToUniversalTime(),
                    LastUpdate = DateTime.UtcNow,
                    LocationMethod = "Crowd Source",
                });   
                if(crowdSource.ThreatId == 5)
                {
                    if (crowdSource.Threat.Type != null)
                    {
                        if (db.Threats.Where(t => t.Type.ToLower() == crowdSource.Threat.Type).Count() == 0)
                        {
                            Threat threat = db.Threats.Add(crowdSource.Threat);
                            db.ThreatLocations.Add(new ThreatLocations()
                            {
                                LocationId = location.Id,
                                ThreatId = threat.Id
                            });
                        }
                        else
                        {
                            db.ThreatLocations.Add(new ThreatLocations()
                            {
                                LocationId = location.Id,
                                ThreatId = db.Threats.Where(t => t.Type.ToLower() == crowdSource.Threat.Type).FirstOrDefault().Id
                            });
                        }
                    }
                    else
                    {
                        ModelState.AddModelError("Others", "The incident field is required.");
                        ViewBag.ThreatId = new SelectList(db.Threats.Take(5).OrderBy(s => s.Type == "Others"), "Id", "Type");
                        return View(crowdSource);
                    }
                }
                
                else
                {
                    db.ThreatLocations.Add(new ThreatLocations()
                    {
                        LocationId = location.Id,
                        ThreatId = crowdSource.ThreatId
                    });
                }               
                db.SaveChanges();
                return RedirectToAction("Thanks");
            }
            ViewBag.ThreatId = new SelectList(db.Threats.Take(5).OrderBy(s => s.Type == "Others"), "Id", "Type");
            return View(crowdSource);
        }

        // GET: CrowdSource/Thanks
        public ActionResult Thanks()
        {
            return View();
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
