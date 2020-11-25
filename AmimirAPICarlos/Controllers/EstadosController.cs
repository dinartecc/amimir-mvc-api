using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.SqlServer;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using AmimirAPICarlos.Models;

namespace AmimirAPICarlos.Controllers
{
    public class EstadosController : ApiController
    {
        private AmimirEntities1 db = new AmimirEntities1();

        int numberPage = 10;

        // GET: api/Estados
        public IQueryable<Estado> GetEstado(int page = 1, string orderBy = "ID", string search = null, string searchBy = "Nombre")
        {
            page = page - 1;

            if (page < 0)
            {
                page = 0;
            }

            var results = db.Estado.OrderBy(orderBy).Skip(numberPage * page).Take(numberPage);
           

            if (search != null )
            {
               switch(searchBy)
                {
                    case "ID": results = results.Where(x => SqlFunctions.StringConvert((double)x.ID).Contains(searchBy));
                        break;
                    default: results = results.Where(x => x.Nombre.Contains(searchBy));
                        break;
                }
            }

            return results;

        }

        // GET: api/Estados/5
        [ResponseType(typeof(Estado))]
        public IHttpActionResult GetEstadoDetail(int id)
        {
            Estado estado = db.Estado.Find(id);
            if (estado == null)
            {
                return NotFound();
            }

            return Ok(estado);
        }

        // PUT: api/Estados/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutEstado(int id, Estado estado)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != estado.ID)
            {
                return BadRequest();
            }

            db.Entry(estado).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!EstadoExists(id))
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

        // POST: api/Estados
        [ResponseType(typeof(Estado))]
        public IHttpActionResult PostEstado(Estado estado)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Estado.Add(estado);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = estado.ID }, estado);
        }

        // DELETE: api/Estados/5
        [ResponseType(typeof(Estado))]
        public IHttpActionResult DeleteEstado(int id)
        {
            Estado estado = db.Estado.Find(id);
            if (estado == null)
            {
                return NotFound();
            }

            db.Estado.Remove(estado);
            db.SaveChanges();

            return Ok(estado);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool EstadoExists(int id)
        {
            return db.Estado.Count(e => e.ID == id) > 0;
        }
    }
}