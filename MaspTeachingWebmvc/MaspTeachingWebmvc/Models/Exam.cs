namespace MaspTeachingWebmvc.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Exam")]
    public partial class Exam
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Exam()
        {
            ExamClasses = new HashSet<ExamClass>();
        }

        public long ExamId { get; set; }

        [Required]
        [StringLength(250)]
        public string ExamName { get; set; }

        public long? ExamTypeId { get; set; }

        public int Marks { get; set; }

        public DateTime Created { get; set; }

        public bool Status { get; set; }

        public virtual ExamType ExamType { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ExamClass> ExamClasses { get; set; }
    }
}
