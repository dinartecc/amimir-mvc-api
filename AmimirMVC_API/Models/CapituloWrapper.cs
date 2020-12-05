using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AmimirMVC_API.Models
{
    public class CapituloWrapper
    {
        public CapituloCLS Capitulo { get; set; }

        public List<UrlAlternativo> urlAlternativos { get; set; }
    }

    public class UrlAlternativo
    {

        public int ID { get; set; }
        public int CapituloID { get; set; }
        public string Descripcion { get; set; }
        public string URL { get; set; }

    }
}