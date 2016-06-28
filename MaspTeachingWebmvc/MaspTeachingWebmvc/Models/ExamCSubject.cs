namespace MaspTeachingWebmvc.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("ExamCSubject")]
    public partial class ExamCSubject
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public ExamCSubject()
        {
            Teachings = new HashSet<Teaching>();
        }

        public long ExamCSubjectId { get; set; }

        public long? ExamClassesId { get; set; }

        public long? SubjectId { get; set; }

        public int GetMarks { get; set; }

        public int GetPercentage { get; set; }

        public int TotalMarks { get; set; }

        public DateTime Created { get; set; }

        public bool Status { get; set; }

        public virtual ExamClass ExamClass { get; set; }

        public virtual Subject Subject { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Teaching> Teachings { get; set; }
    }
}
