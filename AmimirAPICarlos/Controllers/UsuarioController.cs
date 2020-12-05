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
    public class UsuarioController : ApiController
    {
        private AmimirEntities1 db = new AmimirEntities1();

        // GET: api/Usuario
        public IHttpActionResult GetUsuarios()
        {
             if (!AdminValidator())
            {
                return Unauthorized();
            }

            return Ok(db.Usuario);
        }

        // GET: api/Usuario/5
        [ResponseType(typeof(Usuario))]
        public IHttpActionResult GetUsuario(int id)
        {
            if (!AdminValidator() && !isOwnUsername(id))
            {
                return Unauthorized();
            }

            Usuario usuario = db.Usuario.Find(id);
            if (usuario == null)
            {
                return NotFound();
            }

            return Ok(usuario);
        }

        // PUT: api/Usuario/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutUsuario(int id, Usuario usuario)
        {
            if (!AdminValidator())
            {
                return Unauthorized();
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != usuario.ID)
            {
                return BadRequest();
            }

           

            if (usuario.Contrasena == null || usuario.Contrasena.Length == 0 )
            {
                var original = db.Usuario.AsNoTracking().Where(x=> x.ID == usuario.ID).FirstOrDefault();
                usuario.Contrasena = original.Contrasena;
            }
            else
            {
                usuario.Contrasena = sha256(usuario.Contrasena);
            }

            db.Entry(usuario).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateException)
            {
                if (!UsuarioExists(id))
                {
                    return NotFound();
                }
                else
                {
                    return Conflict();
                }
            }
            catch
            {
                return InternalServerError();
            }

            return StatusCode(HttpStatusCode.OK);
        }

        // POST: api/Usuario
        [ResponseType(typeof(Usuario))]
        public IHttpActionResult PostUsuario(Usuario usuario)
        {
            if (!AdminValidator())
            {
                return Unauthorized();
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Usuario.Add(usuario);
            try
            {
                db.SaveChanges();
            }
            catch(DbUpdateException)
            {
                return Conflict();
            }
            catch
            {
                return InternalServerError();
            }
            

            return CreatedAtRoute("DefaultApi", new { id = usuario.ID }, usuario);
        }

        // DELETE: api/Usuario/5
        [ResponseType(typeof(Usuario))]
        public IHttpActionResult DeleteUsuario(int id)
        {
            if (!AdminValidator())
            {
                return Unauthorized();
            }

            Usuario usuario = db.Usuario.Find(id);
            if (usuario == null)
            {
                return NotFound();
            }

            try
            {
                db.Usuario.Remove(usuario);
                db.SaveChanges();
            }
            catch
            {
                return Conflict();
            }

            return Ok(usuario);
        }

        [HttpPost]
        [Route("api/ResetUser")]
        public IHttpActionResult Reset(int id)
        {
            if(!AdminValidator())
            {
                return Unauthorized();
            }

            Usuario usuario = db.Usuario.Find(id);
            
            if (usuario == null)
            {
                return NotFound();
            }

            usuario.Contrasena = sha256("password");
            db.Entry(usuario).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (Exception e)
            {
                return InternalServerError(e);
            }

            return Ok();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool UsuarioExists(int id)
        {
            return db.Usuario.Count(e => e.ID == id) > 0;
        }
    }
}