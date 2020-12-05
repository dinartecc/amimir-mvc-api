using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AmimirMVC_API.Models
{
    [Serializable()]
    public partial class Token
    {
        public string RefreshToken { get; set; }
        public string AccessToken { get; set; }
        public DateTime ExpiresAt { get; set; }
        public bool isAdmin { get; set; }
        public string Username { get; set; }
    }
}