using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AmimirMVC_API.Models
{
    public class AnimeCLS
    {
        public int ID { get; set; }
        public string Nombre { get; set; }
        public DateTime? FechaEstreno { get; set; }
        public string Sinopsis { get; set; }
        public decimal Puntuacion { get; set; }
        public decimal Popularidad { get; set; }
        public int EstadoID { get; set; }
    }
}