using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AmimirAPICarlos.Models
{
    public class CapituloWrapper
    {
        public Capitulo Capitulo { get; set; }
        public List<UrlAlternativo> urlAlternativos { get; set; }
    }
}