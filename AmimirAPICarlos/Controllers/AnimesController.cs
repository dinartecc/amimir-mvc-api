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
        public IHttpActionResult PutAnime(int id, AnimeWrapper Req)
        {
            if (!AdminValidator())
            {
                return Unauthorized();
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }


            var anime = Req.Anime;
            anime.ID = id;
            var Generos = Req.Generos;
            var Estudios = Req.Estudios;
            var NombresAlternativos = Req.NombresAlternativos;
            var Personajes = Req.Personajes;

            db.Personajes.RemoveRange(db.Personajes.Where(x => x.AnimeID == id));
            db.AnimeGenero.RemoveRange(db.AnimeGenero.Where(x => x.AnimeID == id));
            db.AnimeEstudio.RemoveRange(db.AnimeEstudio.Where(x => x.AnimeID == id));
            db.NombreAlternativo.RemoveRange(db.NombreAlternativo.Where(x => x.AnimeID == id));

            foreach ( int GeneroID in Generos )
            {
                AnimeGenero animeGenero = new AnimeGenero();
                animeGenero.AnimeID = id;
                animeGenero.GeneroID = GeneroID;

                db.AnimeGenero.Add(animeGenero);
            }

            foreach (int EstudioID in Estudios)
            {
                AnimeEstudio animeEstudio = new AnimeEstudio();
                animeEstudio.AnimeID = id;
                animeEstudio.EstudioID = EstudioID;

                db.AnimeEstudio.Add(animeEstudio);
            }

            foreach (string NombreAlternativo in NombresAlternativos)
            {
                NombreAlternativo nombreAlternativo = new NombreAlternativo();
                nombreAlternativo.AnimeID = id;
                nombreAlternativo.Nombre = NombreAlternativo;

                db.NombreAlternativo.Add(nombreAlternativo);
            }

            foreach (Personajes Personaje in Personajes)
            {
                Personaje.AnimeID = id;

                db.Personajes.Add(Personaje);
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
        public IHttpActionResult PostAnime(AnimeWrapper Req)
        {
            if (!AdminValidator())
            {
                return Unauthorized();
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var anime = Req.Anime;

            try
            {
                var Generos = Req.Generos;
                var Estudios = Req.Estudios;
                var NombresAlternativos = Req.NombresAlternativos;
                var Personajes = Req.Personajes;

                db.Anime.Add(anime);
                db.SaveChanges();

                foreach (int GeneroID in Generos)
                {
                    AnimeGenero animeGenero = new AnimeGenero();
                    animeGenero.AnimeID = anime.ID;
                    animeGenero.GeneroID = GeneroID;

                    db.AnimeGenero.Add(animeGenero);
                }

                foreach (int EstudioID in Estudios)
                {
                    AnimeEstudio animeEstudio = new AnimeEstudio();
                    animeEstudio.AnimeID = anime.ID;
                    animeEstudio.EstudioID = EstudioID;

                    db.AnimeEstudio.Add(animeEstudio);
                }

                foreach (string NombreAlternativo in NombresAlternativos)
                {
                    NombreAlternativo nombreAlternativo = new NombreAlternativo();
                    nombreAlternativo.AnimeID = anime.ID;
                    nombreAlternativo.Nombre = NombreAlternativo;

                    db.NombreAlternativo.Add(nombreAlternativo);
                }

                foreach (Personajes Personaje in Personajes)
                {
                    Personaje.AnimeID = anime.ID;

                    db.Personajes.Add(Personaje);
                }

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
            if (!AdminValidator())
            {
                return Unauthorized();
            }

            Anime anime = db.Anime.Find(id);
            if (anime == null)
            {
                return NotFound();
            }

            try
            {
                db.Personajes.RemoveRange(db.Personajes.Where(x => x.AnimeID == id));
                db.AnimeGenero.RemoveRange(db.AnimeGenero.Where(x => x.AnimeID == id));
                db.AnimeEstudio.RemoveRange(db.AnimeEstudio.Where(x => x.AnimeID == id));
                db.NombreAlternativo.RemoveRange(db.NombreAlternativo.Where(x => x.AnimeID == id));
                db.Anime.Remove(anime);
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