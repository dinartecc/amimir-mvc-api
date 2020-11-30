//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace AmimirAPICarlos.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class Capitulo
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Capitulo()
        {
            this.UrlAlternativo = new HashSet<UrlAlternativo>();
        }
    
        public int ID { get; set; }
        public int AnimeID { get; set; }
        public string Titulo { get; set; }
        public int Duracion { get; set; }
        public string Sinopsis { get; set; }
        public System.DateTime FechaPublicado { get; set; }
        public string URL { get; set; }
        public string Imagen { get; set; }
    
        public virtual Anime Anime { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<UrlAlternativo> UrlAlternativo { get; set; }
    }
}
