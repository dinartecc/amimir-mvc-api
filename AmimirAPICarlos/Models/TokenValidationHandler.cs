using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web;

namespace AmimirAPICarlos.Models
{
    public class TokenValidationHandler : DelegatingHandler
    { 
        private static bool TryRetrieveToken(HttpRequestMessage request, out string token)
        {
            token = null;

            IEnumerable<string> authHeaders;

            if ( !request.Headers.TryGetValues("Authorization", out authHeaders) || authHeaders.Count() > 1)
            {
                return false;
            }

            var bearerToken = authHeaders.ElementAt(0);
            token = bearerToken.StartsWith("Bearer ") ? bearerToken.Substring(7) : bearerToken;

            return true;
        }

        protected override Task<HttpResponseMessage> SendAsync (HttpRequestMessage requestMessage, CancellationToken cancellationToken)
        {
            HttpStatusCode statusCode;
            string token;

            if(!TryRetrieveToken(requestMessage, out token))
            {
                statusCode = HttpStatusCode.Unauthorized;
                return base.SendAsync(requestMessage, cancellationToken);
            }

            try
            {
                var secretKey = ConfigurationManager.AppSettings["JWT_SECRET_KEY"];
                var audienceToken = ConfigurationManager.AppSettings["JWT_AUDIENCE_TOKEN"];
                var issuerToken = ConfigurationManager.AppSettings["JWT_ISSUER_TOKEN"];
                var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));

                SecurityToken securityToken;
                var tokenHandler = new JwtSecurityTokenHandler();
                TokenValidationParameters validationParameters = new TokenValidationParameters()
                {
                    ValidAudience = audienceToken,
                    ValidIssuer = issuerToken,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    LifetimeValidator = this.LifetimeValidator,
                    IssuerSigningKey = securityKey
                };

                Thread.CurrentPrincipal = tokenHandler.ValidateToken(token, validationParameters, out securityToken);
                HttpContext.Current.User = tokenHandler.ValidateToken(token, validationParameters, out securityToken);

                return base.SendAsync(requestMessage, cancellationToken);
            }
            catch (SecurityTokenValidationException)
            {
                statusCode = HttpStatusCode.Unauthorized;
            }
            catch (ArgumentException)
            {
                statusCode = HttpStatusCode.Unauthorized;
            }
            catch (Exception)
            {
                statusCode = HttpStatusCode.InternalServerError;
            }

            return Task<HttpResponseMessage>.Factory.StartNew(() => new HttpResponseMessage(statusCode) { });
        }

        public bool LifetimeValidator( DateTime? notBefore, DateTime? expires, SecurityToken securityToken, TokenValidationParameters validatorParameters)
        {
            if (expires != null)
            {
                return DateTime.UtcNow < expires ? true : false;
            }

            return false;
        }
    }
}