//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace AmimirAPICarlos.Models
{
    using Newtonsoft.Json;
    using System;
    using System.Collections.Generic;
    
    public partial class Usuario
    {
        public int ID { get; set; }
        public string Nombre { get; set; }
        public Nullable<System.DateTime> FechaNacimiento { get; set; }
        public string Username { get; set; }
        public string CorreoElectronico { get; set; }
        public string Contrasena { get; set; }
        public bool isAdmin { get; set; }

        public bool ShouldSerializeContrasena()
        {
            return false;
        }
    }
}
