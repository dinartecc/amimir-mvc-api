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
    public class AnimesController : ApiController
    {
        private AmimirEntities1 db = new AmimirEntities1();

        // GET: api/Animes
        public IQueryable<Anime> GetAnimes()
        {
            return db.Anime;
        }

        // GET: api/Animes/5
        [ResponseType(typeof(Anime))]
        public IHttpActionResult GetAnime(int id)
        {
            Anime anime = db.Anime.Find(id);
            if (anime == null)
            {
                return NotFound();
            }

            return Ok(anime);
        }

        // PUT: api/Animes/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutAnime(int id, Anime anime)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != anime.ID)
            {
                return BadRequest();
            }

            db.Entry(anime).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!AnimeExists(id))
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

        // POST: api/Animes
        [ResponseType(typeof(Anime))]
        public IHttpActionResult PostAnime(Anime anime)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                db.Anime.Add(anime);
                db.SaveChanges();
            }
            catch
            {
                return Conflict();
            }

            return CreatedAtRoute("DefaultApi", new { id = anime.ID }, anime);
        }

        // DELETE: api/Animes/5
        [ResponseType(typeof(Anime))]
        public IHttpActionResult DeleteAnime(int id)
        {
            Anime anime = db.Anime.Find(id);
            if (anime == null)
            {
                return NotFound();
            }

            try
            {
                db.Anime.Remove(anime);
                db.SaveChanges();
            }
            catch
            {
                return Conflict();
            }

            return Ok(anime);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool AnimeExists(int id)
        {
            return db.Anime.Count(e => e.ID == id) > 0;
        }
    }
}