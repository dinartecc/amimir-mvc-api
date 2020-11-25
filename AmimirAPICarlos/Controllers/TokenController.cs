using AmimirAPICarlos.Models;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Web.Http;
using static AmimirAPICarlos.Controllers.Utils;

namespace AmimirAPICarlos.Controllers
{
    public class TokenController : ApiController
    {
        private AmimirEntities1 db = new AmimirEntities1();
        [HttpPost]
        public IHttpActionResult Authenticate(User user)
        {
            if (user == null)
            {
                throw new HttpResponseException(HttpStatusCode.BadRequest);
            }

            var encrypted = sha256(user.Contrasena);

            var usuario = (from s in db.Usuario where s.Contrasena == encrypted && s.Username == user.Username select s).FirstOrDefault<Usuario>();

            if (usuario != null && usuario.isAdmin)
            {
                var token = GenerateJWT();

                return Ok(token);
            }
            else
            {
                return Unauthorized();
            }
        }



        private Token GenerateJWT()
        {
            var now = DateTime.Now;


            var secretKey = ConfigurationManager.AppSettings["JWT_SECRET_KEY"];
            var audienceToken = ConfigurationManager.AppSettings["JWT_AUDIENCE_TOKEN"];
            var issuerToken = ConfigurationManager.AppSettings["JWT_ISSUER_TOKEN"];
            var expireTime = ConfigurationManager.AppSettings["JWT_EXPIRE_MINUTES"];


            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));
            var signingCredentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var tokenHandler = new JwtSecurityTokenHandler();
            var jwtSecurityToken = tokenHandler.CreateJwtSecurityToken(
                audience: audienceToken,
                issuer: issuerToken,
                notBefore: now,
                expires: now.AddMinutes(Convert.ToInt32(expireTime)),
                signingCredentials: signingCredentials);

            var jwtTokenSerialized = tokenHandler.WriteToken(jwtSecurityToken);

            Token token = new Token();
            token.AccessToken = jwtTokenSerialized;
            token.ExpiresIn = Convert.ToInt32(expireTime) * 60;

            return token;
        }
    }
}
