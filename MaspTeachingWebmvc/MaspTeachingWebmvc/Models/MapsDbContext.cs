namespace MaspTeachingWebmvc.Models
{
    using System.Data.Entity;

    public partial class MapsDbContext : DbContext
    {
        public MapsDbContext()
            : base("name=MapsDbContext")
        {
        }

        public virtual DbSet<Admin> Admins { get; set; }
        public virtual DbSet<AdminClass> AdminClasses { get; set; }
        public virtual DbSet<AdminSubject> AdminSubjects { get; set; }
        public virtual DbSet<Chapter> Chapters { get; set; }
        public virtual DbSet<Class> Classes { get; set; }
        public virtual DbSet<Exam> Exams { get; set; }
        public virtual DbSet<ExamClass> ExamClasses { get; set; }
        public virtual DbSet<ExamCSubject> ExamCSubjects { get; set; }
        public virtual DbSet<ExamType> ExamTypes { get; set; }
        public virtual DbSet<Subject> Subjects { get; set; }
        public virtual DbSet<sysdiagram> sysdiagrams { get; set; }
        public virtual DbSet<Teaching> Teachings { get; set; }
        public virtual DbSet<TeachingComplet> TeachingComplets { get; set; }
        public virtual DbSet<TeachingType> TeachingTypes { get; set; }
        public virtual DbSet<AdminProfile> AdminProfiles { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Admin>()
                .Property(e => e.Name)
                .IsUnicode(false);

            modelBuilder.Entity<Admin>()
                .Property(e => e.Password)
                .IsUnicode(false);

            modelBuilder.Entity<Admin>()
                .HasOptional(e => e.AdminProfile)
                .WithRequired(e => e.Admin)
                .WillCascadeOnDelete();

            modelBuilder.Entity<Class>()
                .HasMany(e => e.Subjects)
                .WithRequired(e => e.Class)
                .WillCascadeOnDelete(false);
        }
    }
    public class UserStatus
    {
        public string FName { get; set; }
        public string LName { get; set; }
        public string Name { get; set; }
        public string Types { get; set; }
        public string Email { get; set; }
        public bool Status { get; set; }
    }

    public enum AdminType
    {
        Admin,
        Supervisor,
        Principal,
        Teacher
    }
}
