using AmimirAPICarlos.Models;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Claims;
using System.Text;
using System.Web.Http;
using static AmimirAPICarlos.Controllers.Utils;

namespace AmimirAPICarlos.Controllers
{

    public class TokenController : ApiController
    {
        private AmimirEntities1 db = new AmimirEntities1();
        [HttpPost]
        public IHttpActionResult Authenticate(AuthRequestCLS req)
        {
            if (req == null || req.user == null || req.ClientID == null )
            {
                throw new HttpResponseException(HttpStatusCode.BadRequest);
            }

            var user = req.user;

            var encrypted = sha256(user.Contrasena);

            var usuario = (from s in db.Usuario where s.Contrasena == encrypted && s.Username == user.Username select s).FirstOrDefault<Usuario>();

            if (usuario != null)
            {
                var token = GenerateJWT(usuario, req.ClientID, req.ClientSecret);

                return Ok(token);
            }
            else
            {
                return Unauthorized();
            }
        }



        private Token GenerateJWT(Usuario user, string ClientID, string ClientSecret)
        {
            var now = DateTime.Now;


            var secretKey = ConfigurationManager.AppSettings["JWT_SECRET_KEY"];
            var audienceToken = ConfigurationManager.AppSettings["JWT_AUDIENCE_TOKEN"];
            var issuerToken = ConfigurationManager.AppSettings["JWT_ISSUER_TOKEN"];
            var expireTime = ConfigurationManager.AppSettings["JWT_EXPIRE_MINUTES"];
            var rtExpireTime = ConfigurationManager.AppSettings["RT_EXPIRE_DAYS"];

            var oldRefresh = db.RefreshToken.Find(ClientID);

            if (oldRefresh != null)
            {
                try
                {
                    db.RefreshToken.Remove(oldRefresh);
                    db.SaveChanges();
                }
                catch
                {
                    Console.WriteLine("Error al borrar refresh token");
                }
                
            }

            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));
            var signingCredentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var tokenHandler = new JwtSecurityTokenHandler();

            var claims = new ClaimsIdentity(new[]
            {
                new Claim("isAdmin", user.isAdmin.ToString()),
                new Claim("userID", user.ID.ToString()),
            });

            var jwtSecurityToken = tokenHandler.CreateJwtSecurityToken(
                audience: audienceToken,
                issuer: issuerToken,
                notBefore: now,
                subject: claims,
                expires: now.AddMinutes(Convert.ToInt32(expireTime)),
                signingCredentials: signingCredentials);

            var jwtTokenSerialized = tokenHandler.WriteToken(jwtSecurityToken);

            var refreshToken = sha256($"{jwtTokenSerialized}{now}{secretKey}");

            RefreshTokenCLS refreshTokenCLS = new RefreshTokenCLS();
            refreshTokenCLS.RefreshToken = refreshToken;
            refreshTokenCLS.ClientID = ClientID;
            refreshTokenCLS.ClientSecret = ClientSecret;
            refreshTokenCLS.ExpiresAt = now.AddDays(Convert.ToInt32(rtExpireTime));

            db.RefreshToken.Add(refreshTokenCLS);
            try
            {
                db.SaveChanges();
            }
            catch
            {
                Console.WriteLine("Error al guardar refresh token");
            }

            Token token = new Token();
            token.AccessToken = jwtTokenSerialized;
            token.ExpiresAt =  now.AddMinutes(Convert.ToInt32(expireTime));
            token.isAdmin = user.isAdmin;
            token.RefreshToken = refreshToken;

            return token;
        }
    }
}
