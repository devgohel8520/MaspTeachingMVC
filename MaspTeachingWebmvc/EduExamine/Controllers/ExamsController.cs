using EduExamine.Models;
using PagedList;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace EduExamine.Controllers
{
    public class ExamsController : Controller
    {
        EduExamineContext _db;
        public HttpCookie userCookie;
        public AdminType AdminType;
        public int? GetEduYearId;

        public ExamsController()
        {
            _db = new EduExamineContext();
        }


        public bool LoginStatus()
        {
            if (Request.Cookies["MapsUser"] != null)
            {
                userCookie = HttpContext.Request.Cookies["MapsUser"];
                AdminType = (AdminType)Enum.Parse(typeof(AdminType), userCookie["Type"], true);
                GetEduYearId = Convert.ToInt32(userCookie["EduYearId"]);
                return true;
            }
            return false;
        }


        public ActionResult EIndex(int? page, string search = null)
        {
            if (!LoginStatus())
                return RedirectToAction("Login", "Admins", null);

            if (search != null)
            {
                var model = _db.Exams.Where(d => d.EduYearId == GetEduYearId && d.ExamName.Contains(search)).OrderBy(d => d.ExamId).ToList();
                return View(model.ToPagedList(page ?? 1, 10));
            }
            var exams = _db.Exams.Where(d => d.EduYearId == GetEduYearId).OrderBy(d => d.ExamId).ToList();
            return View(exams.ToPagedList(page ?? 1, 10));
        }

        public ActionResult Create()
        {
            if (!LoginStatus())
                return RedirectToAction("Login", "Admins", null);

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ExamId,ExamName,EduYearId")] Exam model)
        {
            if (!LoginStatus())
                return RedirectToAction("Login", "Admins", null);

            bool Exists = _db.Exams.Any(d => d.ExamName.Equals(model.ExamName));
            if (!Exists)
            {
                if (ModelState.IsValid)
                {
                    model.EduYear = _db.EduYears.Find((int)GetEduYearId);
                    model.EduYearId = (int)GetEduYearId;
                    _db.Exams.Add(model);
                    _db.SaveChanges();
                    return Json("");
                }
                else
                {
                    return Json("Model is not valid.");
                }
            }
            else
            {
                return Json("Try another label name");
            }
        }

        public ActionResult Edit(long? id)
        {
            if (!LoginStatus())
                return RedirectToAction("Login", "Admins", null);

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Exam exam = _db.Exams.Find(id);
            if (exam == null)
            {
                return HttpNotFound();
            }
            ViewBag.EduYearId = new SelectList(_db.EduYears, "EduYearId", "EduYearName", exam.EduYearId);
            return View(exam);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ExamId,ExamName,EduYearId")] Exam model)
        {
            if (!LoginStatus())
                return RedirectToAction("Login", "Admins", null);

            if (ModelState.IsValid)
            {
                model.EduYear = _db.EduYears.Find(model.EduYearId);
                model.EduYearId = model.EduYearId;
                _db.Entry(model).State = EntityState.Modified;
                _db.SaveChanges();
                return Json("");
            }
            else
            {
                return Json("Model is not valid.");
            }
        }

        public ActionResult Delete(long? id)
        {
            if (!LoginStatus())
                return RedirectToAction("Login", "Admins", null);


            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Exam exam = _db.Exams.Find(id);
            if (exam == null)
            {
                return HttpNotFound();
            }
            return View(exam);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed([Bind(Include = "ExamId")] Exam model)
        {
            if (!LoginStatus())
                return RedirectToAction("Login", "Admins", null);

            Exam exam = _db.Exams.Find(model.ExamId);
            _db.Exams.Remove(exam);
            _db.SaveChanges();
            return Json("");
        }

        public ActionResult ClassDisplay(int? id)
        {
            if (!LoginStatus())
                return RedirectToAction("Login", "Admins", null);

            ExamsClassViewModel model = new ExamsClassViewModel()
            {
                Exam = _db.Exams.Find(id),
                Classess = _db.Classess.OrderBy(d => d.ClassesId).ToList()
            };


            return View(model);
        }

        public ActionResult SubjectDisplay(long? id, long? examid)
        {
            if (!LoginStatus())
                return RedirectToAction("Login", "Admins", null);

            ExamsClassesSubjectViewModel model = new ExamsClassesSubjectViewModel()
            {
                Exam = _db.Exams.Find(examid),
                Classess = _db.Classess.Find(id),
                examSubjects = new List<ExamSubject>()
            };

            List<ExamSubject> examSubjects = _db.ExamSubjects.Where(d => d.ExamId == examid).ToList();

            List<Subject> subjectList = _db.Subjects.Where(d => d.ClassesId == id).ToList();

            int totalpercentage, totalavgmarks, totalmarks;
            totalpercentage = totalavgmarks = totalmarks = 0;

            foreach (var subject in subjectList)
            {
                ExamSubject findVal = examSubjects.Where(d => d.SubjectId == subject.SubjectId && d.ExamId == examid).FirstOrDefault();

                ExamSubject addModel = new ExamSubject()
                {
                    AvgMarks = findVal != null ? findVal.AvgMarks : 0,
                    Exam = model.Exam,
                    ExamId = (long)examid,
                    ExamMarks = findVal != null ? findVal.ExamMarks : 0,
                    ExamSubjectId = findVal != null ? findVal.ExamSubjectId : 0,
                    Percentages = findVal != null ? findVal.Percentages : 0,
                    Subject = subject,
                    SubjectId = subject.SubjectId
                };

                totalavgmarks += addModel.AvgMarks;
                totalmarks += addModel.ExamMarks;

                model.examSubjects.Add(addModel);
            }
            if (totalavgmarks != 0 && totalmarks != 0)
            {
                totalpercentage = ((totalavgmarks * 100) / totalmarks);
            }
            model.TotalAvg = totalavgmarks;
            model.TotalMarks = totalmarks;
            model.TotalPercentage = totalpercentage;

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult SubmitExamSubjectMarks([Bind(Include = "examSubjects")] ExamsClassesSubjectViewModel model)
        {
            if (!LoginStatus())
                return RedirectToAction("Login", "Admins", null);

            if (ModelState.IsValid)
            {
                if (model.examSubjects != null)
                {
                    foreach (var item in model.examSubjects)
                    {
                        Exam exam = _db.Exams.Find(item.ExamId);
                        Subject subject = _db.Subjects.Find(item.SubjectId);

                        bool Exists = _db.ExamSubjects.Any(d => d.ExamId == item.ExamId && d.ExamSubjectId == item.ExamSubjectId);

                        ExamSubject addModel = new ExamSubject()
                        {
                            AvgMarks = item.AvgMarks,
                            ExamMarks = item.ExamMarks,
                            ExamSubjectId = item.ExamSubjectId,
                            Percentages = item.Percentages,
                            Exam = exam,
                            ExamId = item.ExamId,
                            Subject = subject,
                            SubjectId = item.SubjectId
                        };

                        if (addModel.AvgMarks <= addModel.ExamMarks)
                        {
                            if (!Exists)
                            {
                                _db.ExamSubjects.Add(addModel);
                            }
                            else
                            {
                                _db.Entry(addModel).State = EntityState.Modified;
                            }
                        }
                        else
                        {
                            return Json("Error: cause of some wrong information feeded.");
                        }
                        _db.SaveChanges();
                    }
                    return Json("");
                }
                else
                {
                    return Json("Model is not valid.");
                }
            }
            else
            {
                return Json("Model is not valid.");
            }

        }



        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
