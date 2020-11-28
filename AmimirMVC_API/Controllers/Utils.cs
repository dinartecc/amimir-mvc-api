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

namespace AmimirMVC_API.Controllers
{
    public class Utils
    {
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
    }
}