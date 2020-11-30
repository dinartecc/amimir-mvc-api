using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AmimirMVC_API.Models
{
    public class CapituloCLS
    {
        public int ID { get; set; }
        public int AnimeID { get; set; }
        public string Titulo { get; set; }
        public int Duracion { get; set; }
        public string Sinopsis { get; set; }
        public System.DateTime FechaPublicado { get; set; }
        public string URL { get; set; }
    }
}