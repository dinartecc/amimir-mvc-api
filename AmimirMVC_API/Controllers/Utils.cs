using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Net.Http.Headers;
using System.Net.Http;
using System.Web;
using System.Web.Mvc;
using AmimirMVC_API.Models;
using System.Configuration;
using Newtonsoft.Json;

namespace AmimirMVC_API.Controllers
{
    public class Utils
    {
        private static string baseURL = "https://localhost:44300";
        private static string basePath = "/";
        public static string sha256(string randomString)
        {
            var crypt = new SHA256Managed();
            string hash = String.Empty;
            byte[] crypto = crypt.ComputeHash(Encoding.ASCII.GetBytes(randomString));
            foreach (byte theByte in crypto)
            {
                hash += theByte.ToString("x2");
            }
            return hash;
        }

        public static int ValidarLogin (Token token)
        {   
            if (token == null )
            {
                return 0;
            }

            if (token.isAdmin)
            {
                return 2;
            }

            return 1;
        }

        public static Token RefrescarToken ( Token token )
        {
            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri(baseURL);
            var contentType = new MediaTypeWithQualityHeaderValue("application/json");
            client.DefaultRequestHeaders.Accept.Add(contentType);

            var req = new RefRequestCLS();
            req.ClientSecret = ConfigurationManager.AppSettings["CLIENT_SECRET"];
            req.RefreshToken = token.RefreshToken;

            string stringData = JsonConvert.SerializeObject(req);
            var contentData = new StringContent(stringData, Encoding.UTF8, "application/json");

            HttpResponseMessage response = client.PostAsync(basePath + "api/RefreshToken", contentData).Result;

            string stringJWT = response.Content.ReadAsStringAsync().Result;

            if (!response.IsSuccessStatusCode)
            {
                return token;
            }

            var newToken = JsonConvert.DeserializeObject<Token>(stringJWT);

            newToken.Username = token.Username;

            return newToken;
        }
    }
}