using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AmimirMVC_API.Models
{
    public class PaqueteCLS
    {
        public int ID { get; set; }
        public string Nombre { get; set; }
        public int Duracion { get; set; }
        public decimal Precio { get; set; }
    }
}