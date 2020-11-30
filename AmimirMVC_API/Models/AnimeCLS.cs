using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AmimirMVC_API.Models
{
    public class AnimeCLS
    {
        public Nullable<int> ID { get; set; }
        public string Nombre { get; set; }
        public Nullable<System.DateTime> FechaEstreno { get; set; }
        public string Sinopsis { get; set; }

        public string Imagen { get; set; }
        public decimal Puntuacion { get; set; }
        public decimal Popularidad { get; set; }
        public int EstadoID { get; set; }

        public virtual EstadoCLS Estado { get; set; }
       
        public virtual List<EstudioCLS> Estudios { get; set; }
       
        public virtual List<GeneroCLS> Generos { get; set; }
       
        public virtual List<CapituloCLS> Capitulos { get; set; }
       
        public virtual List<NombreAlternativoCLS> NombreAlternativo { get; set; }
       
        public virtual List<PersonajeCLS> Personajes { get; set; }
    }
}