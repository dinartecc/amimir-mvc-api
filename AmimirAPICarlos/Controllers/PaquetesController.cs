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
using AmimirAPICarlos.Models;

namespace AmimirAPICarlos.Controllers
{
    public class PaquetesController : ApiController
    {
        private AmimirEntities1 db = new AmimirEntities1();

        // GET: api/Paquetes
        public IQueryable<Paquete> GetPaquetes()
        {
            return db.Paquete;
        }

        // GET: api/Paquetes/5
        [ResponseType(typeof(Paquete))]
        public IHttpActionResult GetPaquete(int id)
        {
            Paquete paquete = db.Paquete.Find(id);
            if (paquete == null)
            {
                return NotFound();
            }

            return Ok(paquete);
        }

        // PUT: api/Paquetes/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutPaquete(int id, Paquete paquete)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != paquete.ID)
            {
                return BadRequest();
            }

            db.Entry(paquete).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PaqueteExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return StatusCode(HttpStatusCode.OK);
        }

        // POST: api/Paquetes
        [ResponseType(typeof(Paquete))]
        public IHttpActionResult PostPaquete(Paquete paquete)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Paquete.Add(paquete);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = paquete.ID }, paquete);
        }

        // DELETE: api/Paquetes/5
        [ResponseType(typeof(Paquete))]
        public IHttpActionResult DeletePaquete(int id)
        {
            Paquete paquete = db.Paquete.Find(id);
            if (paquete == null)
            {
                return NotFound();
            }

            try
            {
                db.Paquete.Remove(paquete);
                db.SaveChanges();
            }
            catch
            {
                return Conflict();
            }



            return Ok(paquete);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool PaqueteExists(int id)
        {
            return db.Paquete.Count(e => e.ID == id) > 0;
        }
    }
}