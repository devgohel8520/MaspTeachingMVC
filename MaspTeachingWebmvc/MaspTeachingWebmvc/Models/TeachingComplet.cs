namespace MaspTeachingWebmvc.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("TeachingComplet")]
    public partial class TeachingComplet
    {
        public long TeachingCompletId { get; set; }

        public long? ChapterId { get; set; }

        public DateTime? Started { get; set; }

        public DateTime? Ended { get; set; }

        public virtual Chapter Chapter { get; set; }
    }
}
