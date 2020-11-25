using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AmimirAPICarlos.Models
{
    public class Token
    {
        public string AccessToken { get; set; }
        public int ExpiresIn { get; set; }
    }
}