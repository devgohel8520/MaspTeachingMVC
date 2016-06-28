namespace MaspTeachingWebmvc.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("TeachingType")]
    public partial class TeachingType
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public TeachingType()
        {
            Teachings = new HashSet<Teaching>();
        }

        public long TeachingTypeId { get; set; }

        [Required]
        [StringLength(50)]
        public string TName { get; set; }

        public int TMarks { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Teaching> Teachings { get; set; }
    }
}
