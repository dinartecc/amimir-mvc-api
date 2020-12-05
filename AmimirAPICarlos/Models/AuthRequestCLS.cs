using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AmimirAPICarlos.Models
{
    public class AuthRequestCLS
    {
        public User user { get; set; }
        public string ClientID { get; set; }
        public string ClientSecret { get; set; }
    }
}