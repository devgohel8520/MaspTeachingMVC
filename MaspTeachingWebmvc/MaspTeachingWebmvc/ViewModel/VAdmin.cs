using MaspTeachingWebmvc.Models;
using System;
using System.Collections.Generic;


namespace MaspTeachingWebmvc.ViewModel
{
    public enum AdminType
    {
        Admin,
        Supervisor,
        Principal,
        Teacher
    }
    public class VAdmin
    {
        public long AdminId { get; set; }
        public string Name { get; set; }
        public string Password { get; set; }
        public AdminType Types { get; set; }
        public DateTime Created { get; set; }
        public bool Status { get; set; }
        public virtual ICollection<AdminClass> AdminClasses { get; set; }
        public virtual AdminProfile AdminProfile { get; set; }
        public virtual ICollection<AdminSubject> AdminSubjects { get; set; }
    }


}