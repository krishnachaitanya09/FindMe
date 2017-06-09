using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace GPSServer.Controllers
{
    public class FindMeController : Controller
    {
        // GET: FindMe
        public ActionResult Index(string phonenumber, string name)
        {
            ViewBag.PhoneNumber = phonenumber;
            ViewBag.Name = name;
            return View();
        }
    }
}