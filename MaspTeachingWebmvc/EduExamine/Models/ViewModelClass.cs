using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace EduExamine.Models
{
    public class ViewModelClass
    {
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

}