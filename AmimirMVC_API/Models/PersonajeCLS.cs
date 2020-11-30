using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AmimirMVC_API.Models
{
    public class PersonajeCLS
    {
        public int ID { get; set; }
        public int AnimeID { get; set; }
        public int ActorID { get; set; }
        public string Nombre { get; set; }
        public Nullable<int>  Rol { get; set; }
    }
}