namespace MaspTeachingWebmvc.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("AdminSubject")]
    public partial class AdminSubject
    {
        public long AdminSubjectId { get; set; }

        public long? AdminId { get; set; }

        public long? SubjectId { get; set; }

        public virtual Admin Admin { get; set; }

        public virtual Subject Subject { get; set; }
    }
}
