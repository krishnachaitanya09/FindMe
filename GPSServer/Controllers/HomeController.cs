using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace GPSServer.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult GetRoutes()
        {
            DbJsonReader reader = new DbJsonReader();
            Response.AppendHeader("Content-Type", "application/json");
            Response.Write(reader.getJsonString("prcGetRoutes", "routes"));
            return View();
        }

        public ActionResult DeleteRoute()
        {
            string sessionID = Request.QueryString["sessionID"];
            string phoneNumber = Request.QueryString["phoneNumber"];

            // our helper class to get data
            //DbXmlReader reader = new DbXmlReader();
            DbJsonReader reader = new DbJsonReader();
            Response.AppendHeader("Content-Type", "text/xml");

            // actually we are getting a response back here, but the result will be a deleted route on the webpage
            Response.Write(reader.getJsonString("prcDeleteRoute", "routes",
                new SqlParameter("@sessionID", sessionID),
                new SqlParameter("@phoneNumber", phoneNumber)));
            return View();
        }

        public ActionResult GetRouteForMap()
        {
            string sessionID = Request.QueryString["sessionID"];
            string phoneNumber = Request.QueryString["phoneNumber"];

            // our helper class to get data
            DbJsonReader reader = new DbJsonReader();

            Response.AppendHeader("Content-Type", "application/json");

            Response.Write(reader.getJsonString("prcGetRouteForMap", "locations",
                new SqlParameter("@sessionID", sessionID),
                new SqlParameter("@phoneNumber", phoneNumber)));
            return View();
        }

        public ActionResult UpdateLocation()
        {
            string latitude = Request.Form["latitude"];
            string longitude = Request.Form["longitude"];
            string speed = Request.Form["speed"];
            string direction = Request.Form["direction"];
            string distance = Request.Form["distance"];
            string date = Server.UrlDecode(Request.Form["date"]);
            string locationMethod = Server.UrlDecode(Request.Form["locationmethod"]);
            string phoneNumber = Request.Form["phonenumber"];
            string sessionID = Request.Form["sessionid"];
            string accuracy = Request.Form["accuracy"];
            string eventType = Request.Form["eventtype"];
            string extraInfo = Request.Form["extrainfo"];

            // our helper class to update the database
            DbWriter dbw = new DbWriter();

            try
            {
                // update the database with our GPS data from the phone
                dbw.updateDB("prcSaveGPSLocation",
                    new SqlParameter("@latitude", latitude),
                    new SqlParameter("@longitude", longitude),
                    new SqlParameter("@speed", speed),
                    new SqlParameter("@direction", direction),
                    new SqlParameter("@distance", distance),
                    new SqlParameter("@date", date),
                    new SqlParameter("@locationMethod", locationMethod),
                    new SqlParameter("@phoneNumber", phoneNumber),
                    new SqlParameter("@sessionID", sessionID),
                    new SqlParameter("@accuracy", accuracy),
                    new SqlParameter("@eventType", eventType),
                    new SqlParameter("@extraInfo", extraInfo));
            }
            catch (Exception ex)
            {
                Response.Write(ex.Message);
            }
            return View();
        }

        public ActionResult DisplayMap()
        {
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}