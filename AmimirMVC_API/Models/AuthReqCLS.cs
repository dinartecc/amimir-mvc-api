using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AmimirMVC_API.Models
{
    public class AuthReqCLS
    {
        public UserCLS user { get; set; }
        public string ClientSecret { get; set; }
    }
}