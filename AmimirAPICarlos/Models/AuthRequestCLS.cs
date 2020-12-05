using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AmimirAPICarlos.Models
{
    public class AuthRequestCLS
    {
        public User user { get; set; }
        public string ClientSecret { get; set; }
    }

    public class RefRequestCLS
    {
        public string RefreshToken { get; set; }
        public string ClientSecret { get; set; }
    }
}