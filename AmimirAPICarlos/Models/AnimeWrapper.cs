using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AmimirAPICarlos.Models
{
    public class AnimeWrapper
    {
        public Anime Anime { get; set; }
        public List<int> Generos { get; set; }
        public List<int> Estudios { get; set; }
        public List<Personajes> Personajes { get; set; }
        public List<string> NombresAlternativos { get; set; }

    }
}