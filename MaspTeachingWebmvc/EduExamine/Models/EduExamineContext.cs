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
        SuperVisor,
        Principal,
        Teacher
    }

    public class EduYear
    {
        public int EduYearId { get; set; }
        public string EduYearName { get; set; }
        public DateTime EduStart { get; set; }
        public DateTime EduEnd { get; set; }

        public ICollection<ChapterDate> ChapterDates { get; set; }
        public ICollection<Admin> Admins { get; set; }
        public ICollection<Exam> Exams { get; set; }
        public ICollection<ChapterTeaching> ChapterTeachings { get; set; }
    }

    public class Admin
    {
        public long AdminId { get; set; }
        public string Name { get; set; }
        public string Password { get; set; }
        public AdminType Types { get; set; }

        [ForeignKey("EduYear")]
        public int EduYearId { get; set; }
        public EduYear EduYear { get; set; }

        public AdminProfile AdminProfile { get; set; }
        public ICollection<TeacherSubject> TeacherSubjects { get; set; }
    }

    public class AdminProfile
    {
        [Key, ForeignKey("Admin")]
        public long AdminId { get; set; }

        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string MiddleName { get; set; }
        public string Mobile { get; set; }
        public string Phone { get; set; }
        public string Address1 { get; set; }
        public string Address2 { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Country { get; set; }
        public string PinCode { get; set; }

        public virtual Admin Admin { get; set; }
    }

    public class TeachingType
    {
        public int TeachingTypeId { get; set; }
        public string Name { get; set; }
        public int OrderId { get; set; }

        public ICollection<ChapterTeaching> ChapterTeachings { get; set; }
        public ICollection<TeacherTeaching> TeacherTeachings { get; set; }
    }

    public class Classes
    {
        public long ClassesId { get; set; }
        public string ClassName { get; set; }

        public ICollection<Subject> Subjects { get; set; }
    }

    public class Subject
    {
        public long SubjectId { get; set; }
        public string SubjectName { get; set; }

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
        public string ChapterName { get; set; }

        [ForeignKey("Subject")]
        public long SubjectId { get; set; }
        public Subject Subject { get; set; }

        public ChapterDate ChapterDate { get; set; }

        public ICollection<ChapterTeaching> ChapterTeachings { get; set; }
        public ICollection<TeacherTeaching> TeacherTeachings { get; set; }
    }

    public class ChapterDate
    {
        [ForeignKey("Chapter")]
        public long ChapterDateId { get; set; }

        public DateTime CStartDate { get; set; }
        public DateTime CEndDate { get; set; }

        public Chapter Chapter { get; set; }

        [ForeignKey("EduYear")]
        public int EduYearId { get; set; }
        public EduYear EduYear { get; set; }

        public TeacherChapterDate TeacherChapterDate { get; set; }
    }

    public class ChapterTeaching
    {
        public long ChapterTeachingId { get; set; }

        public int OrderId { get; set; }

        [ForeignKey("TeachingType")]
        public int TeachingTypeId { get; set; }
        public TeachingType TeachingType { get; set; }

        public int MaxVal { get; set; }
        public int MinVal { get; set; }

        [ForeignKey("EduYear")]
        public int EduYearId { get; set; }
        public EduYear EduYear { get; set; }

        [ForeignKey("Chapter")]
        public long ChapterId { get; set; }
        public Chapter Chapter { get; set; }
    }

    public class TeacherSubject
    {
        public long TeacherSubjectId { get; set; }

        [ForeignKey("Admin")]
        public long AdminId { get; set; }
        public Admin Admin { get; set; }

        [ForeignKey("Subject")]
        public long SubjectId { get; set; }
        public Subject Subject { get; set; }
    }

    public class TeacherChapterDate
    {
        [ForeignKey("ChapterDate")]
        public long TeacherChapterDateId { get; set; }

        [ForeignKey("Admin")]
        public long AdminId { get; set; }
        public Admin Admin { get; set; }

        public ChapterDate ChapterDate { get; set; }

        public DateTime TCStartDate { get; set; }
        public DateTime TCEndDate { get; set; }
    }

    public class TeacherTeaching
    {
        public long TeacherTeachingId { get; set; }

        public int OrderId { get; set; }

        [ForeignKey("TeachingType")]
        public int TeachingTypeId { get; set; }
        public TeachingType TeachingType { get; set; }

        public int MaxVal { get; set; }
        public int MinVal { get; set; }
        public int Marks { get; set; }

        [ForeignKey("Chapter")]
        public long ChapterId { get; set; }
        public Chapter Chapter { get; set; }
    }

    public class Exam
    {
        public long ExamId { get; set; }
        public string ExamName { get; set; }

        [ForeignKey("EduYear")]
        public int EduYearId { get; set; }
        public EduYear EduYear { get; set; }

        public ICollection<ExamSubject> ExamSubjects { get; set; }
    }

    public class ExamSubject
    {
        public long ExamSubjectId { get; set; }
        [ForeignKey("Subject")]
        public long SubjectId { get; set; }
        public Subject Subject { get; set; }

        public int AvgMarks { get; set; }
        public int ExamMarks { get; set; }

        [ForeignKey("Exam")]
        public long ExamId { get; set; }
        public Exam Exam { get; set; }
    }

}