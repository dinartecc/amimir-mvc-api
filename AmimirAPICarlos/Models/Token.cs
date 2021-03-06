﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AmimirAPICarlos.Models
{
    public class Token
    {
        public string AccessToken { get; set; }
        public DateTime ExpiresAt { get; set; }
        public bool isAdmin { get; set; }
        public string RefreshToken { get; set; }
        public int UserID { get; set; }
    }
}