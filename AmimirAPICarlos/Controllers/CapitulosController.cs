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
    public class CapitulosController : ApiController
    {
        private AmimirEntities1 db = new AmimirEntities1();

        // GET: api/Capitulos
        public IQueryable<Capitulo> GetCapitulo()
        {
            return db.Capitulo;
        }

        // GET: api/Capitulos/5
        [ResponseType(typeof(Capitulo))]
        public IHttpActionResult GetCapitulo(int id)
        {
            Capitulo capitulo = db.Capitulo.Find(id);
            if (capitulo == null)
            {
                return NotFound();
            }

            return Ok(capitulo);
        }

        // PUT: api/Capitulos/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutCapitulo(int id, CapituloWrapper Req)
        {
            if (!AdminValidator())
            {
                return Unauthorized();
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var capitulo = Req.Capitulo;
            capitulo.ID = id;
            var URLAlternativos = Req.urlAlternativos;

            db.UrlAlternativo.RemoveRange(db.UrlAlternativo.Where(x => x.CapituloID == id));

            foreach ( UrlAlternativo urlAlternativo in URLAlternativos )
            {
                urlAlternativo.CapituloID = id;
                db.UrlAlternativo.Add(urlAlternativo);
            }

            db.Entry(capitulo).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CapituloExists(id))
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

        // POST: api/Capitulos
        [ResponseType(typeof(Capitulo))]
        public IHttpActionResult PostCapitulo(CapituloWrapper Req)
        {
            if (!AdminValidator())
            {
                return Unauthorized();
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var capitulo = Req.Capitulo;

            try
            {
                var URLAlternativos = Req.urlAlternativos;

                db.Capitulo.Add(capitulo);
                db.SaveChanges();

                foreach(UrlAlternativo urlAlternativo in URLAlternativos)
                {
                    urlAlternativo.CapituloID = capitulo.ID;
                    db.UrlAlternativo.Add(urlAlternativo);
                }

                db.SaveChanges();
            }
            catch
            {
                return Conflict();
            }


            return CreatedAtRoute("DefaultApi", new { id = capitulo.ID }, capitulo);
        }

        // DELETE: api/Capitulos/5
        [ResponseType(typeof(Capitulo))]
        public IHttpActionResult DeleteCapitulo(int id)
        {
            if (!AdminValidator())
            {
                return Unauthorized();
            }

            Capitulo capitulo = db.Capitulo.Find(id);
            if (capitulo == null)
            {
                return NotFound();
            }

            try
            {
                db.Capitulo.Remove(capitulo);
                db.UrlAlternativo.RemoveRange(db.UrlAlternativo.Where(x => x.CapituloID == id));
                db.SaveChanges();
            }
            catch
            {
                return Conflict();
            }
            
            return Ok(capitulo);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool CapituloExists(int id)
        {
            return db.Capitulo.Count(e => e.ID == id) > 0;
        }
    }
}