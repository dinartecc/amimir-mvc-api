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
    [Authorize]
    public class EstudiosController : ApiController
    {
        private AmimirEntities1 db = new AmimirEntities1();

        // GET: api/Estudios
        public IQueryable<Estudio> GetEstudio()
        {
            return db.Estudio;
        }

        // GET: api/Estudios/5
        [ResponseType(typeof(Estudio))]
        public IHttpActionResult GetEstudio(int id)
        {
            Estudio estudio = db.Estudio.Find(id);
            if (estudio == null)
            {
                return NotFound();
            }

            return Ok(estudio);
        }

        // PUT: api/Estudios/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutEstudio(int id, Estudio estudio)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != estudio.ID)
            {
                return BadRequest();
            }

            db.Entry(estudio).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!EstudioExists(id))
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

        // POST: api/Estudios
        [ResponseType(typeof(Estudio))]
        public IHttpActionResult PostEstudio(Estudio estudio)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Estudio.Add(estudio);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = estudio.ID }, estudio);
        }

        // DELETE: api/Estudios/5
        [ResponseType(typeof(Estudio))]
        public IHttpActionResult DeleteEstudio(int id)
        {
            Estudio estudio = db.Estudio.Find(id);
            if (estudio == null)
            {
                return NotFound();
            }

            db.Estudio.Remove(estudio);
            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateException)
            {
                return Conflict();
            }
            catch (Exception)
            {
                return InternalServerError();
            }

            return Ok(estudio);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool EstudioExists(int id)
        {
            return db.Estudio.Count(e => e.ID == id) > 0;
        }
    }
}