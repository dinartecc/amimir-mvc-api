using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AmimirMVC_API.Models
{
    public class UserCLS
    {
        public int ID { get; set; }
        public string Nombre { get; set; }
        public DateTime? FechaNacimiento { get; set; }
        public string Username { get; set; }
        public string CorreoElectronico { get; set; }
        public string Contrasena { get; set; }
        public bool isAdmin { get; set; }
    }
}