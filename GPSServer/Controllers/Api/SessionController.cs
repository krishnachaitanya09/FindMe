using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using GPSServer.Models;

namespace GPSServer.Controllers.Api
{
    public class SessionController : ApiController
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: api/Session
        public IQueryable<Session> GetSessions()
        {
            return db.Sessions;
        }

        // GET: api/Session/5
        [ResponseType(typeof(Session))]
        public IHttpActionResult GetSession(string id)
        {
            Session session = db.Sessions.Find(id);
            if (session == null)
            {
                return NotFound();
            }

            return Ok(session);
        }

        // PUT: api/Session/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutSession(string id, Session session)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != session.SessionID)
            {
                return BadRequest();
            }

            db.Entry(session).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!SessionExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return StatusCode(HttpStatusCode.NoContent);
        }

        // POST: api/Session
        [ResponseType(typeof(Session))]
        public IHttpActionResult PostSession(Session session)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Sessions.Add(session);

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateException)
            {
                if (SessionExists(session.SessionID))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtRoute("DefaultApi", new { id = session.SessionID }, session);
        }

        // DELETE: api/Session/5
        [ResponseType(typeof(Session))]
        public IHttpActionResult DeleteSession(string id)
        {
            Session session = db.Sessions.Find(id);
            if (session == null)
            {
                return NotFound();
            }

            db.Sessions.Remove(session);
            db.SaveChanges();

            return Ok(session);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool SessionExists(string id)
        {
            return db.Sessions.Count(e => e.SessionID == id) > 0;
        }
    }
}