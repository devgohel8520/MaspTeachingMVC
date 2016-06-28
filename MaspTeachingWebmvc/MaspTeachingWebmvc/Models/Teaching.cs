namespace MaspTeachingWebmvc.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Teaching")]
    public partial class Teaching
    {
        public long TeachingId { get; set; }

        public long? ChapterId { get; set; }

        public long? TeachingTypeId { get; set; }

        public int GetMarks { get; set; }

        public int GetPercentage { get; set; }

        public long? ExamCSubjectId { get; set; }

        public virtual Chapter Chapter { get; set; }

        public virtual ExamCSubject ExamCSubject { get; set; }

        public virtual TeachingType TeachingType { get; set; }
    }
}
