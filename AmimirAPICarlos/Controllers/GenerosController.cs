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
using static AmimirAPICarlos.Controllers.Utils;

namespace AmimirAPICarlos.Controllers
{
    [Authorize]
    public class GenerosController : ApiController
    {
        private AmimirEntities1 db = new AmimirEntities1();

        // GET: api/Generos
        public IQueryable<Genero> GetGenero()
        {
            return db.Genero;
        }

        // GET: api/Generos/5
        [ResponseType(typeof(Genero))]
        public IHttpActionResult GetGenero(int id)
        {
            Genero genero = db.Genero.Find(id);
            if (genero == null)
            {
                return NotFound();
            }

            return Ok(genero);
        }

        // PUT: api/Generos/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutGenero(int id, Genero genero)
        {
            if (!AdminValidator())
            {
                return Unauthorized();
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != genero.ID)
            {
                return BadRequest();
            }

            db.Entry(genero).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!GeneroExists(id))
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

        // POST: api/Generos
        [ResponseType(typeof(Genero))]
        public IHttpActionResult PostGenero(Genero genero)
        {
            if (!AdminValidator())
            {
                return Unauthorized();
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Genero.Add(genero);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = genero.ID }, genero);
        }

        // DELETE: api/Generos/5
        [ResponseType(typeof(Genero))]
        public IHttpActionResult DeleteGenero(int id)
        {
            if (!AdminValidator())
            {
                return Unauthorized();
            }

            Genero genero = db.Genero.Find(id);
            if (genero == null)
            {
                return NotFound();
            }


            try
            {
                db.Genero.Remove(genero);
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

            return Ok(genero);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool GeneroExists(int id)
        {
            return db.Genero.Count(e => e.ID == id) > 0;
        }
    }
}