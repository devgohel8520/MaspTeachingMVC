namespace MaspTeachingWebmvc.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class ExamClass
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public ExamClass()
        {
            ExamCSubjects = new HashSet<ExamCSubject>();
        }

        [Key]
        public long ExamClassesId { get; set; }

        public long? ExamId { get; set; }

        public long? ClassesId { get; set; }

        public virtual Class Class { get; set; }

        public virtual Exam Exam { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ExamCSubject> ExamCSubjects { get; set; }
    }
}
