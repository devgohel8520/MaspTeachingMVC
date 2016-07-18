using EduExamine.Models;
using PagedList;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace EduExamine.Controllers
{
    public class HomeController : Controller
    {
        EduExamineContext _db;
        public HttpCookie userCookie;
        public int? GetEduYearId;

        public HomeController()
        {
            _db = new EduExamineContext();
            GetEduYearId = 0;
        }

        public ActionResult Error()
        {
            return View();
        }

        public ActionResult Index(int? page, int? GetEduYearId, string search = null)
        {
            if (GetEduYearId == null)
                GetEduYearId = 1;

            List<TeachersGraph> teacherGraphs = new List<TeachersGraph>();

            if (!_db.EduYears.Any())
                return RedirectToAction("Login", "Admins", null);


            List<TeachersGraph> lstTeacherGraphs = new List<TeachersGraph>();
            string connectionString = ConfigurationManager.ConnectionStrings["EduExamineContext"].ConnectionString;
            using (SqlConnection cn = new SqlConnection(connectionString))
            {
                string query = "Select tc.TeacherId,((ISNULL(SUM(ex.AvgMarks),0)*100)/ ISNULL(SUM(ex.ExamMarks),1)) as ExamMarks, tc.FullName,tc.EduYearId";
                query += " from TeacherSubjects ts";
                query += " left join Teachers tc";
                query += " on ts.TeacherId = tc.TeacherId";
                query += " left join ExamSubjects ex";
                query += " on ex.SubjectId = ts.SubjectId";
                query += " Group By tc.TeacherId, tc.FullName,tc.EduYearId";
                query += " having tc.EduYearId = " + (int)GetEduYearId + ";";

                SqlCommand command = new SqlCommand(query, cn);
                cn.Open();
                SqlDataReader dr = command.ExecuteReader();
                while (dr.Read())
                {
                    TeachersGraph graphModel = new TeachersGraph()
                    {
                        RankId = 0,
                        TeacherId = Convert.ToInt64(dr[0]),
                        EduYears = _db.EduYears.Find(1),
                        TeacherFullName = Convert.ToString(dr[2]),
                        EduYearId = 1,
                        TeacheringAvg = 0,
                        ExamAvg = Convert.ToInt32(dr[1]),
                        TotalAvg = 0,
                        Speed = 0,
                    };
                    teacherGraphs.Add(graphModel);
                }
            }

            var TeachersChapterDates = _db.TeacherChapterDates.ToList();
            var ChaptersDates = _db.ChapterDates.ToList();

            using (SqlConnection cn = new SqlConnection(connectionString))
            {
                string query = "Select tc.TeacherId,tc.FullName,((ISNULL(SUM(tt.Marks),0)*100)/ISNULL(SUM(tt.MaxVal),1))AS TeachingMarks";
                query += " from Teachers tc";
                query += " left join TeacherSubjects ts";
                query += " on ts.TeacherId = tc.TeacherId";
                query += " left join Chapters ch";
                query += " on ch.SubjectId = ts.SubjectId";
                query += " left join TeacherTeachings tt";
                query += " on tt.ChapterId = ch.ChapterId";
                query += " group by tc.TeacherId,tc.EduYearId,tc.FullName";
                query += " having tc.EduYearId = " + (int)GetEduYearId + ";";

                SqlCommand command = new SqlCommand(query, cn);
                cn.Open();
                SqlDataReader dr = command.ExecuteReader();
                while (dr.Read())
                {
                    TeachersGraph findResult = teacherGraphs.Find(d => d.TeacherId == Convert.ToInt64(dr[0]));

                    findResult.TeacheringAvg = Convert.ToInt32(dr[2]);
                    findResult.TotalAvg = (findResult.TeacheringAvg + findResult.ExamAvg) / 2;

                    //Get Teaching Speed
                    var findVal = TeachersChapterDates.Where(d => d.TeacherId == findResult.TeacherId).ToList().LastOrDefault();
                    if (findVal != null)
                    {
                        var chapterDate = ChaptersDates.Find(d => d.ChapterId == findVal.ChapterId);
                        int diffCD = chapterDate.CEndDate.Subtract(chapterDate.CStartDate).Days;
                        int diffTCD = findVal.TCEndDate.Subtract(findVal.TCStartDate).Days;
                        int diff = diffCD - diffTCD;


                        findResult.SpeedDays = Convert.ToString(diff);

                        if (diff <= -11)
                            findResult.Speed = SpeedTest.ExtremlySlow;
                        else if (diff >= -10 && diff <= -8)
                            findResult.Speed = SpeedTest.VerySlow;
                        else if (diff >= -7 && diff <= -4)
                            findResult.Speed = SpeedTest.Slow;
                        else if (diff >= -3 && diff <= 3)
                            findResult.Speed = SpeedTest.Perfect;
                        else if (diff >= 4 && diff <= 7)
                            findResult.Speed = SpeedTest.Fast;
                        else if (diff >= 8 && diff <= 10)
                            findResult.Speed = SpeedTest.VeryFast;
                        else if (diff >= 11)
                            findResult.Speed = SpeedTest.ExtremlyFast;
                        else
                            findResult.Speed = SpeedTest.NA;

                    }
                    else
                    {
                        var chapterDate = ChaptersDates.Where(c => c.CStartDate < DateTime.Now).OrderByDescending(c => c.CStartDate).Take(1).First();

                        if (chapterDate != null)
                        {
                            int diffCD = DateTime.Now.Subtract(chapterDate.CStartDate).Days;
                            int diff = diffCD;
                            findResult.Speed = SpeedTest.Waiting;
                            findResult.SpeedDays = Convert.ToString(diff);
                        }
                        else
                        {
                            findResult.Speed = SpeedTest.NA;
                            findResult.SpeedDays = string.Empty;
                        }
                    }
                }
            }

            int count = 1;
            teacherGraphs = teacherGraphs.OrderByDescending(d => d.TotalAvg).ToList();
            foreach (var item in teacherGraphs)
            {
                item.RankId = count;
                count += 1;
            }

            return View(teacherGraphs.ToPagedList(page ?? 1, 20));
        }

        public ActionResult Years()
        {
            if (!_db.EduYears.Any())
                return RedirectToAction("Login", "Admins", null);

            List<EduYear> model = _db.EduYears.ToList();
            return View(model);
        }

        public ActionResult TeacherPerform(int? page, int? GetEduYearId, string search = null)
        {
            if (GetEduYearId == null)
                GetEduYearId = 1;

            List<TeachersGraph> teacherGraphs = new List<TeachersGraph>();

            if (!_db.EduYears.Any())
                return RedirectToAction("Login", "Admins", null);

            List<TeachersGraph> lstTeacherGraphs = new List<TeachersGraph>();
            string connectionString = ConfigurationManager.ConnectionStrings["EduExamineContext"].ConnectionString;
            using (SqlConnection cn = new SqlConnection(connectionString))
            {
                string query = "Select tc.TeacherId,((ISNULL(SUM(ex.AvgMarks),0)*100)/ ISNULL(SUM(ex.ExamMarks),1)) as ExamMarks, tc.FullName,tc.EduYearId";
                query += " from TeacherSubjects ts";
                query += " left join Teachers tc";
                query += " on ts.TeacherId = tc.TeacherId";
                query += " left join ExamSubjects ex";
                query += " on ex.SubjectId = ts.SubjectId";
                query += " Group By tc.TeacherId, tc.FullName,tc.EduYearId";
                query += " having tc.EduYearId = " + (int)GetEduYearId + "";

                SqlCommand command = new SqlCommand(query, cn);
                cn.Open();
                SqlDataReader dr = command.ExecuteReader();
                while (dr.Read())
                {
                    TeachersGraph graphModel = new TeachersGraph()
                    {
                        RankId = 0,
                        TeacherId = Convert.ToInt64(dr[0]),
                        EduYears = _db.EduYears.Find(1),
                        TeacherFullName = Convert.ToString(dr[2]),
                        EduYearId = 1,
                        TeacheringAvg = 0,
                        ExamAvg = Convert.ToInt32(dr[1]),
                        TotalAvg = 0,
                        Speed = 0,
                    };
                    teacherGraphs.Add(graphModel);
                }
            }

            var TeachersChapterDates = _db.TeacherChapterDates.ToList();
            var ChaptersDates = _db.ChapterDates.ToList();

            using (SqlConnection cn = new SqlConnection(connectionString))
            {
                string query = "Select tc.TeacherId,tc.FullName,((ISNULL(SUM(tt.Marks),0)*100)/ISNULL(SUM(tt.MaxVal),1))AS TeachingMarks";
                query += " from Teachers tc";
                query += " left join TeacherSubjects ts";
                query += " on ts.TeacherId = tc.TeacherId";
                query += " left join Chapters ch";
                query += " on ch.SubjectId = ts.SubjectId";
                query += " left join TeacherTeachings tt";
                query += " on tt.ChapterId = ch.ChapterId";
                query += " group by tc.TeacherId,tc.EduYearId,tc.FullName";
                query += " having tc.EduYearId = " + (int)GetEduYearId + ";";

                SqlCommand command = new SqlCommand(query, cn);
                cn.Open();
                SqlDataReader dr = command.ExecuteReader();
                while (dr.Read())
                {
                    TeachersGraph findResult = teacherGraphs.Find(d => d.TeacherId == Convert.ToInt64(dr[0]));

                    findResult.TeacheringAvg = Convert.ToInt32(dr[2]);
                    findResult.TotalAvg = (findResult.TeacheringAvg + findResult.ExamAvg) / 2;

                    //Get Teaching Speed
                    var findVal = TeachersChapterDates.Where(d => d.TeacherId == findResult.TeacherId).ToList().LastOrDefault();
                    if (findVal != null)
                    {
                        var chapterDate = ChaptersDates.Find(d => d.ChapterId == findVal.ChapterId);
                        int diffCD = chapterDate.CEndDate.Subtract(chapterDate.CStartDate).Days;
                        int diffTCD = findVal.TCEndDate.Subtract(findVal.TCStartDate).Days;
                        int diff = diffCD - diffTCD;


                        findResult.SpeedDays = Convert.ToString(diff);

                        if (diff <= -11)
                            findResult.Speed = SpeedTest.ExtremlySlow;
                        else if (diff >= -10 && diff <= -8)
                            findResult.Speed = SpeedTest.VerySlow;
                        else if (diff >= -7 && diff <= -4)
                            findResult.Speed = SpeedTest.Slow;
                        else if (diff >= -3 && diff <= 3)
                            findResult.Speed = SpeedTest.Perfect;
                        else if (diff >= 4 && diff <= 7)
                            findResult.Speed = SpeedTest.Fast;
                        else if (diff >= 8 && diff <= 10)
                            findResult.Speed = SpeedTest.VeryFast;
                        else if (diff >= 11)
                            findResult.Speed = SpeedTest.ExtremlyFast;
                        else
                            findResult.Speed = SpeedTest.NA;

                    }
                    else
                    {
                        var chapterDate = ChaptersDates.Where(c => c.CStartDate < DateTime.Now).OrderByDescending(c => c.CStartDate).Take(1).First();

                        if (chapterDate != null)
                        {
                            int diffCD = DateTime.Now.Subtract(chapterDate.CStartDate).Days;
                            int diff = diffCD;
                            findResult.Speed = SpeedTest.Waiting;
                            findResult.SpeedDays = Convert.ToString(diff);
                        }
                        else
                        {
                            findResult.Speed = SpeedTest.NA;
                            findResult.SpeedDays = string.Empty;
                        }
                    }
                }
            }

            int count = 1;
            teacherGraphs = teacherGraphs.OrderByDescending(d => d.TotalAvg).ToList();
            foreach (var item in teacherGraphs)
            {
                item.RankId = count;
                count += 1;
            }

            return View(teacherGraphs.ToPagedList(page ?? 1, 20));
        }
    }
}