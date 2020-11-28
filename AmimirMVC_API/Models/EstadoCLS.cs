using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AmimirMVC_API.Models
{
    public class EstadoCLS
    {
        public int ID { get; set; }
        public string Nombre { get; set; }
        public virtual ICollection<AnimeCLS> Anime { get; set; }
    }
}