using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Threading;
using System.Security.Claims;


namespace AmimirAPICarlos.Controllers
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

        public static Boolean AdminValidator()
        {
            var principal = (ClaimsPrincipal)Thread.CurrentPrincipal;

            var isAdmin = principal.Claims.Where(c => c.Type == "isAdmin").Select(c => c.Value).SingleOrDefault();

            return isAdmin == "True";
        }

        public static Boolean isOwnUsername( int userID )
        {
            var principal = (ClaimsPrincipal)Thread.CurrentPrincipal;

            var claimUserID = principal.Claims.Where(c => c.Type == "userID").Select(c => c.Value).SingleOrDefault();

            return claimUserID == userID.ToString();
        }
    }
}