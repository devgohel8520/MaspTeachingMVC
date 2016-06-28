namespace MaspTeachingWebmvc.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class AdminClass
    {
        [Key]
        public long AdminClassesId { get; set; }

        public long? AdminId { get; set; }

        public long? ClassesId { get; set; }

        public virtual Admin Admin { get; set; }

        public virtual Class Class { get; set; }
    }
}
