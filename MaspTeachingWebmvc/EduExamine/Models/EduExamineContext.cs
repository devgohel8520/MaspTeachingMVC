using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;


namespace EduExamine.Models
{

    public class EduExamineContext : DbContext
    {
        public DbSet<EduYear> EduYears { get; set; }
        public DbSet<Admin> Admins { get; set; }
        public DbSet<Teacher> Teachers { get; set; }
        public DbSet<TeachingType> TeachingTypes { get; set; }
        public DbSet<Classes> Classess { get; set; }
        public DbSet<Subject> Subjects { get; set; }
        public DbSet<Chapter> Chapters { get; set; }
        public DbSet<ChapterDate> ChapterDates { get; set; }
        public DbSet<ChapterTeaching> ChapterTeachings { get; set; }
        public DbSet<TeacherSubject> TeacherSubjects { get; set; }
        public DbSet<TeacherChapterDate> TeacherChapterDates { get; set; }
        public DbSet<TeacherTeaching> TeacherTeachings { get; set; }
        public DbSet<Exam> Exams { get; set; }
        public DbSet<ExamSubject> ExamSubjects { get; set; }
    }


    public enum AdminType
    {
        Admin,
        SuperVisor,
    }
    public enum TeacherType
    {
        Principal,
        Teacher
    }

    public class EduYear
    {
        public int EduYearId { get; set; }
        [Required(ErrorMessage = "*")]
        public string EduYearName { get; set; }
        [Required(ErrorMessage = "*")]
        [DataType(DataType.Date)]
        public DateTime EduStart { get; set; }
        [Required(ErrorMessage = "*")]
        [DataType(DataType.Date)]
        public DateTime EduEnd { get; set; }

        public ICollection<Chapter> Chapters { get; set; }
        public ICollection<Teacher> Teachers { get; set; }
        public ICollection<Exam> Exams { get; set; }
    }

    public class Admin
    {
        public long AdminId { get; set; }
        [Required(ErrorMessage = "*")]
        public string Name { get; set; }
        [Required(ErrorMessage = "*")]
        public string Password { get; set; }
        [Required(ErrorMessage = "*")]
        public AdminType Types { get; set; }
    }

    public class Teacher
    {
        public long TeacherId { get; set; }
        [Required(ErrorMessage = "*")]
        public string FullName { get; set; }
        [Required(ErrorMessage = "*")]
        public string LoginName { get; set; }
        [Required(ErrorMessage = "*")]
        public string Password { get; set; }
        [Required(ErrorMessage = "*")]
        public TeacherType Types { get; set; }

        [Required(ErrorMessage = "*")]
        [ForeignKey("EduYear")]
        public int EduYearId { get; set; }
        public EduYear EduYear { get; set; }

        public ICollection<TeacherSubject> TeacherSubjects { get; set; }
    }

    public class TeachingType
    {
        public int TeachingTypeId { get; set; }
        [Required(ErrorMessage = "*")]
        public string Name { get; set; }
        [Required(ErrorMessage = "*")]
        public int OrderId { get; set; }

        public ICollection<ChapterTeaching> ChapterTeachings { get; set; }
        public ICollection<TeacherTeaching> TeacherTeachings { get; set; }
    }

    public class Classes
    {
        public long ClassesId { get; set; }
        [Required(ErrorMessage = "*")]
        public string ClassName { get; set; }

        public ICollection<Subject> Subjects { get; set; }
    }

    public class Subject
    {
        public long SubjectId { get; set; }
        [Required(ErrorMessage = "*")]
        public string SubjectName { get; set; }

        [Required(ErrorMessage = "*")]
        [ForeignKey("Classes")]
        public long ClassesId { get; set; }
        public Classes Classes { get; set; }

        public ICollection<TeacherSubject> TeacherSubjects { get; set; }
        public ICollection<ExamSubject> ExamSubjects { get; set; }
        public ICollection<Chapter> Chapters { get; set; }
    }

    public class Chapter
    {
        public long ChapterId { get; set; }
        [Required(ErrorMessage = "*")]
        public string ChapterName { get; set; }

        [Required(ErrorMessage = "*")]
        [ForeignKey("Subject")]
        public long SubjectId { get; set; }
        public Subject Subject { get; set; }

        public ChapterDate ChapterDate { get; set; }

        [ForeignKey("EduYear")]
        public int EduYearId { get; set; }
        public EduYear EduYear { get; set; }

        public ICollection<ChapterTeaching> ChapterTeachings { get; set; }
        public ICollection<TeacherTeaching> TeacherTeachings { get; set; }
    }

    public class ChapterDate
    {
        [Key, ForeignKey("Chapter")]
        public long ChapterId { get; set; }
        [Required(ErrorMessage = "*")]
        [DataType(DataType.Date)]
        public DateTime CStartDate { get; set; }
        [Required(ErrorMessage = "*")]
        [DataType(DataType.Date)]
        public DateTime CEndDate { get; set; }

        public Chapter Chapter { get; set; }

        //[Required(ErrorMessage = "*")]
        //[ForeignKey("EduYear")]
        //public int EduYearId { get; set; }
        //public EduYear EduYear { get; set; }

        //public TeacherChapterDate TeacherChapterDate { get; set; }
    }

    public class ChapterTeaching
    {
        public long ChapterTeachingId { get; set; }
        [Required(ErrorMessage = "*")]
        public int OrderId { get; set; }
        [Required(ErrorMessage = "*")]
        [ForeignKey("TeachingType")]
        public int TeachingTypeId { get; set; }
        public TeachingType TeachingType { get; set; }
        [Required(ErrorMessage = "*")]
        public int MaxVal { get; set; }
        [Required(ErrorMessage = "*")]
        public int MinVal { get; set; }

        //[Required(ErrorMessage = "*")]
        //[ForeignKey("EduYear")]
        //public int EduYearId { get; set; }
        //public EduYear EduYear { get; set; }

        [Required(ErrorMessage = "*")]
        [ForeignKey("Chapter")]
        public long ChapterId { get; set; }
        public Chapter Chapter { get; set; }
        public bool Status { get; set; }
    }

    public class TeacherSubject
    {
        public long TeacherSubjectId { get; set; }
        [Required(ErrorMessage = "*")]
        [ForeignKey("Teacher")]
        public long TeacherId { get; set; }
        public Teacher Teacher { get; set; }
        [Required(ErrorMessage = "*")]
        [ForeignKey("Subject")]
        public long SubjectId { get; set; }
        public Subject Subject { get; set; }
    }

    public class TeacherChapterDate
    {
        [Key, ForeignKey("Chapter")]
        public long ChapterId { get; set; }
        public Chapter Chapter { get; set; }

        [Required(ErrorMessage = "*")]
        [ForeignKey("Teacher")]
        public long TeacherId { get; set; }
        public Teacher Teacher { get; set; }

        [Required(ErrorMessage = "*")]
        [DataType(DataType.Date)]
        public DateTime TCStartDate { get; set; }

        [Required(ErrorMessage = "*")]
        [DataType(DataType.Date)]
        public DateTime TCEndDate { get; set; }
    }

    public class TeacherTeaching
    {
        public long TeacherTeachingId { get; set; }
        [Required(ErrorMessage = "*")]
        public int OrderId { get; set; }
        [Required(ErrorMessage = "*")]
        [ForeignKey("TeachingType")]
        public int TeachingTypeId { get; set; }
        public TeachingType TeachingType { get; set; }
        [Required(ErrorMessage = "*")]
        public int MaxVal { get; set; }
        [Required(ErrorMessage = "*")]
        public int MinVal { get; set; }
        [Required(ErrorMessage = "*")]
        public int Marks { get; set; }
        [Required(ErrorMessage = "*")]
        [ForeignKey("Chapter")]
        public long ChapterId { get; set; }
        public Chapter Chapter { get; set; }
    }

    public class Exam
    {
        public long ExamId { get; set; }
        [Required(ErrorMessage = "*")]
        public string ExamName { get; set; }

        [Required(ErrorMessage = "*")]
        [ForeignKey("EduYear")]
        public int EduYearId { get; set; }
        public EduYear EduYear { get; set; }

        public ICollection<ExamSubject> ExamSubjects { get; set; }
    }

    public class ExamSubject
    {
        public long ExamSubjectId { get; set; }
        [Required(ErrorMessage = "*")]
        [ForeignKey("Subject")]
        public long SubjectId { get; set; }
        public Subject Subject { get; set; }
        [Required(ErrorMessage = "*")]
        public int AvgMarks { get; set; }
        [Required(ErrorMessage = "*")]
        public int ExamMarks { get; set; }
        public int Percentages { get; set; }
        [Required(ErrorMessage = "*")]
        [ForeignKey("Exam")]
        public long ExamId { get; set; }
        public Exam Exam { get; set; }
        public int EduYearId { get; set; }
    }

}