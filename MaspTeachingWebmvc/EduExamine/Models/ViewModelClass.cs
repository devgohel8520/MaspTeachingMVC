using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace EduExamine.Models
{
    public class ViewModelClass
    {
    }

    public class AdminViewModel
    {
        public string FullName { get; set; }
        public string Name { get; set; }
        public string Password { get; set; }
        public AdminType Types { get; set; }
        public int EduYearId { get; set; }
        public bool Status { get; set; }
    }


    public class ChapterDateAndTeaching
    {
        public long ChapterId { get; set; }
        [Required(ErrorMessage = "*")]
        [DataType(DataType.Date)]
        public DateTime CStartDate { get; set; }
        [Required(ErrorMessage = "*")]
        [DataType(DataType.Date)]
        public DateTime CEndDate { get; set; }
        public Chapter Chapter { get; set; }
        [Required(ErrorMessage = "*")]
        [DataType(DataType.Date)]
        public int EduYearId { get; set; }
        public EduYear EduYear { get; set; }
        public TeacherChapterDate TeacherChapterDate { get; set; }
        public List<ChapterTeaching> chapterTeachings { get; set; }
    }

    public class TeacherDateAndTeaching
    {
        public long TeacherChapterDateId { get; set; }
        [Required(ErrorMessage = "*")]
        public long TeacherId { get; set; }
        public Teacher Teacher { get; set; }

        public ChapterDate ChapterDate { get; set; }
        [Required(ErrorMessage = "*")]
        [DataType(DataType.Date)]
        public DateTime TCStartDate { get; set; }
        [Required(ErrorMessage = "*")]
        [DataType(DataType.Date)]
        public DateTime TCEndDate { get; set; }

        public TeacherSubject teachersubject { get; set; }
        public long SubjectId { get; set; }
        public List<TeacherTeaching> teacherTeachings { get; set; }
    }

    public class TeacherChapterViewModel
    {
        public Teacher teacher { get; set; }
        public Subject subject { get; set; }
        public IEnumerable<Chapter> Chapters { get; set; }
    }

    public class ExamsClassViewModel
    {
        public Exam Exam { get; set; }
        public List<Classes> Classess { get; set; }
    }

    public class ExamsClassesSubjectViewModel
    {
        public Exam Exam { get; set; }
        public Classes Classess { get; set; }
        public List<ExamSubject> examSubjects { get; set; }

        public int TotalAvg { get; set; }
        public int TotalMarks { get; set; }
        public int TotalPercentage { get; set; }
    }

    public class TeachersGraph
    {
        public int RankId { get; set; }
        public Teacher Teacher { get; set; }
        public int TotalAvg { get; set; }
        public int TeacheringAvg { get; set; }
        public int ExamAvg { get; set; }
        public SpeedTest Speed { get; set; }
        public List<TeacherTeaching> WeakNess { get; set; }

        public int EduYearId { get; set; }
        public EduYear EduYears { get; set; }
    }

    public enum SpeedTest
    {
        Perfect,
        Slow,
        Fast,
    }

}