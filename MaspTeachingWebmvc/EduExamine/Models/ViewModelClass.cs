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

        public bool CheckChapterDate { get; set; }
        //[Required(ErrorMessage = "*")]
        //[DataType(DataType.Date)]
        //public int EduYearId { get; set; }
        //public EduYear EduYear { get; set; }
        //public TeacherChapterDate TeacherChapterDate { get; set; }
        public List<ChapterTeaching> chapterTeachings { get; set; }
    }

    public class TeacherDateAndTeaching
    {
        public long ChapterId { get; set; }
        public Chapter Chapter { get; set; }

        [Required(ErrorMessage = "*")]
        public long TeacherId { get; set; }
        public Teacher Teacher { get; set; }

        [Required(ErrorMessage = "*")]
        [DataType(DataType.Date)]
        public DateTime TCStartDate { get; set; }
        [Required(ErrorMessage = "*")]
        [DataType(DataType.Date)]
        public DateTime TCEndDate { get; set; }

        public TeacherSubject teachersubject { get; set; }
        public long SubjectId { get; set; }
        public List<TeacherTeaching> teacherTeachings { get; set; }

        public ChapterDate ChapterDate { get; set; }
        public int eduyearid { get; set; }
        public long ClassId { get; set; }
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
        public long TeacherId { get; set; }
        public string TeacherFullName { get; set; }
        public int TotalAvg { get; set; }
        public int TeacheringAvg { get; set; }
        public int ExamAvg { get; set; }
        public SpeedTest Speed { get; set; }
        public List<TeacherTeaching> WeakNess { get; set; }

        public int EduYearId { get; set; }
        public EduYear EduYears { get; set; }
    }

    public class TeacherSubjectViewModel
    {
        public long TeacherId { get; set; }
        public Teacher Teacher { get; set; }
        public List<SubjectList> Subjects { get; set; }

    }

    public class SubjectList
    {
        public long SubjectId { get; set; }
        public string SubjectName { get; set; }
        public string ClassName { get; set; }

        public long ClassesId { get; set; }
        public Classes Classes { get; set; }

        public ICollection<TeacherSubject> TeacherSubjects { get; set; }
        public ICollection<ExamSubject> ExamSubjects { get; set; }
        public ICollection<Chapter> Chapters { get; set; }

        public bool Status { get; set; }
    }

    public class ExamMarksGroupViewModel
    {
        public long TeacherId { get; set; }
        public string FullName { get; set; }
        public long ExamId { get; set; }
        public long SubjectId { get; set; }
        public int AvgMarks { get; set; }
        public int ExamMarks { get; set; }
    }

    public enum SpeedTest
    {
        ExtremlySlow,
        VerySlow,
        Slow,
        Perfect,
        Fast,
        VeryFast,
        ExtremlyFast,
        NA
    }

}