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
    
    public partial class Estudio
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Estudio()
        {
            this.AnimeEstudio = new HashSet<AnimeEstudio>();
        }
    
        public int ID { get; set; }
        public string Nombre { get; set; }
        public string Imagen { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<AnimeEstudio> AnimeEstudio { get; set; }
    }
}
