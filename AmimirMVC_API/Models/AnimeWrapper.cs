using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AmimirMVC_API.Models
{
    public class AnimeWrapper
    {
        public AnimeCLS Anime { get; set; }
        public List<int> Generos { get; set; }
        public List<int> Estudios { get; set; }
        public List<PersonajeCLS> Personajes { get; set; }
        public List<string> NombresAlternativos { get; set; }

    }
}