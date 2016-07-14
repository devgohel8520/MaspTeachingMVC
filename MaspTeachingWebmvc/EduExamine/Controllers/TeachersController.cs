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
    public class TeachersController : Controller
    {
        EduExamineContext _db;
        public HttpCookie userCookie;
        public AdminType AdminType;
        public int? GetEduYearId;

        public TeachersController()
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

        [HttpGet]
        public ActionResult TIndex(int? page)
        {
            if (!LoginStatus())
                return RedirectToAction("Login", "Admins", null);

            var model = _db.Teachers.Include(d => d.TeacherSubjects).Where(d => d.Types == TeacherType.Teacher && d.EduYearId == GetEduYearId).ToList();
            if (model.Count == 0)
            {
                Teacher item = new Teacher() { Types = TeacherType.Teacher, TeacherId = 0, FullName = "" };
                List<Teacher> teacher = new List<Teacher>();
                teacher.Add(item);
                return View(teacher.ToPagedList(page ?? 1, 10));
            }
            return View(model.ToPagedList(page ?? 1, 10));

        }

        [HttpGet]
        public ActionResult Create()
        {
            if (!LoginStatus())
                return RedirectToAction("Login", "Admins", null);

            Teacher teacher = new Teacher() { Types = TeacherType.Teacher, TeacherId = 0, FullName = "" };
            ViewBag.EduYearId = new SelectList(_db.EduYears, "EduYearId", "EduYearName");
            return View(teacher);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "FullName,LoginName,Password,Types")] Teacher model)
        {
            if (!LoginStatus())
                return RedirectToAction("Login", "Admins", null);

            bool Exists = _db.Teachers.Any(d => d.LoginName.Equals(model.LoginName));
            if (!Exists)
            {
                if (ModelState.IsValid)
                {
                    model.EduYear = _db.EduYears.Find((int)GetEduYearId);
                    model.EduYearId = (int)GetEduYearId;
                    _db.Teachers.Add(model);
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

        [HttpGet]
        public ActionResult Edit(long? id)
        {
            if (!LoginStatus())
                return RedirectToAction("Login", "Admins", null);

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Teacher model = _db.Teachers.Find(id);

            if (model == null)
            {
                return HttpNotFound();
            }
            else
            {
                return View(model);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "TeacherId,FullName,LoginName,Password,Types,EduYearId")] Teacher model)
        {
            if (!LoginStatus())
                return RedirectToAction("Login", "Admins", null);

            if (ModelState.IsValid)
            {
                model.EduYear = _db.EduYears.Find((int)GetEduYearId);
                model.EduYearId = (int)GetEduYearId;
                _db.Entry(model).State = EntityState.Modified;
                _db.SaveChanges();
                return Json("");
            }
            return View(model);
        }

        public ActionResult Delete(long? id)
        {
            if (!LoginStatus())
                return RedirectToAction("Login", "Admins", null);

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Teacher model = _db.Teachers.Find(id);
            if (model == null)
            {
                return HttpNotFound();
            }
            return View(model);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeletePConfirmed(long? id)
        {
            if (!LoginStatus())
                return RedirectToAction("Login", "Admins", null);

            Teacher admin = _db.Teachers.Find(id);
            _db.Teachers.Remove(admin);
            _db.SaveChanges();
            return Json("");
        }

        //Subject list of teacher
        public ActionResult SubjectDisplay(int? page, long? id)
        {
            if (!LoginStatus())
                return RedirectToAction("Login", "Admins", null);

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var model = _db.TeacherSubjects.Include(d => d.Subject).Include(d => d.Subject.Classes).Include(d => d.Teacher).Where(d => d.TeacherId == id).OrderBy(d => d.Subject.SubjectName);
            if (model.Count() == 0)
            {
                List<TeacherSubject> subList = new List<TeacherSubject>();
                subList.Add(new TeacherSubject() { Teacher = _db.Teachers.Find(id) });
                return View(subList.ToPagedList(page ?? 1, 20));
            }
            return View(model.ToPagedList(page ?? 1, 20));
        }

        public ActionResult SubjectCreate(long? id)
        {
            if (!LoginStatus())
                return RedirectToAction("Login", "Admins", null);

            Teacher teacher = _db.Teachers.Find(id);

            List<SubjectList> lstSubject = new List<SubjectList>();

            var subjects = _db.Subjects.Include(d => d.Classes).ToList();

            foreach (var item in subjects)
            {
                SubjectList subject = new SubjectList()
                {
                    SubjectId = item.SubjectId,
                    SubjectName = item.SubjectName,
                    ClassName = item.Classes.ClassName,
                    Status = false

                };
                lstSubject.Add(subject);
            }

            TeacherSubjectViewModel model = new TeacherSubjectViewModel()
            {
                TeacherId = teacher.TeacherId,
                Teacher = teacher,
                Subjects = lstSubject.OrderBy(x => x.SubjectName).ToList()
            };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult SubjectCreate([Bind(Include = "TeacherId,subjects")] TeacherSubjectViewModel model)
        {
            if (!LoginStatus())
                return RedirectToAction("Login", "Admins", null);

            bool successed = false;
            bool exists = false;

            foreach (var subject in model.Subjects)
            {
                if (subject.Status)
                {
                    bool Exists = _db.TeacherSubjects.Any(d => d.SubjectId == subject.SubjectId);
                    if (Exists)
                    {
                        List<TeacherSubject> findteachersubject = _db.TeacherSubjects.Where(d => d.SubjectId == subject.SubjectId).ToList();

                        if (findteachersubject != null)
                        {
                            foreach (var teacherSubject in findteachersubject)
                            {
                                Teacher teacher = _db.Teachers.Find(teacherSubject.TeacherId);
                                if (teacher.EduYearId == GetEduYearId)
                                {
                                    exists = true;
                                }
                            }
                        }
                    }

                    if (!exists)
                    {
                        if (ModelState.IsValid)
                        {
                            TeacherSubject sbmodel = new TeacherSubject()
                            {
                                SubjectId = subject.SubjectId,
                                TeacherId = model.TeacherId,
                            };

                            _db.TeacherSubjects.Add(sbmodel);
                            _db.SaveChanges();
                            successed = true;
                        }
                        else
                        {
                            successed = false;
                        }
                    }
                    exists = false;
                }
            }
            if (successed)
            {
                return Json("");
            }
            else
            {
                return Json("This subject has already been assigned.");
            }
        }

        public ActionResult SubjectDelete(long? id)
        {
            if (!LoginStatus())
                return RedirectToAction("Login", "Admins", null);

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TeacherSubject subject = _db.TeacherSubjects.Find(id);
            subject.Subject = _db.Subjects.Find(subject.SubjectId);
            if (subject == null)
            {
                return HttpNotFound();
            }
            return View(subject);
        }

        [HttpPost, ActionName("SubjectDelete")]
        [ValidateAntiForgeryToken]
        public ActionResult SubjectDeleteConfirmed([Bind(Include = "TeacherSubjectId")] TeacherSubject model)
        {
            if (!LoginStatus())
                return RedirectToAction("Login", "Admins", null);

            TeacherSubject subject = _db.TeacherSubjects.Find(model.TeacherSubjectId);

            if (subject != null)
            {
                _db.TeacherSubjects.Remove(subject);
                _db.SaveChanges();
                return Json("");
            }
            else
            {
                return Json("No record found.");
            }
        }

        //Chapter Control of subject
        public ActionResult ChapterDisplay(int? id)
        {
            if (!LoginStatus())
                return RedirectToAction("Login", "Admins", null);

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            TeacherChapterViewModel model = new TeacherChapterViewModel()
            {
                teacher = new Teacher(),
                subject = new Subject(),
                Chapters = new List<Chapter>()
            };

            TeacherSubject teacherSubject = _db.TeacherSubjects.Find(id);

            model.subject = _db.Subjects.Find(teacherSubject.SubjectId);
            model.teacher = _db.Teachers.Find(teacherSubject.TeacherId);

            model.Chapters = _db.Chapters.Where(d => d.SubjectId == teacherSubject.SubjectId && d.EduYearId == GetEduYearId).OrderBy(d => d.ChapterId);

            return View(model);
        }

        //Teacher teaching performance
        public ActionResult SettingsDisplay(int? id, int? eduyearid, int? subjectid, int? teacherid)
        {
            if (!LoginStatus())
                return RedirectToAction("Login", "Admins", null);

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            TeacherChapterDate teacherchapterDate = _db.TeacherChapterDates.Where(d => d.ChapterId == id && d.TeacherId == teacherid).FirstOrDefault();

            TeacherSubject teacherSubject = _db.TeacherSubjects.Where(d => d.SubjectId == subjectid && d.TeacherId == teacherid).First();

            Chapter chapter = _db.Chapters.Find(id);
            ChapterDate chapterDateExist = _db.ChapterDates.Find(id);

            Subject subjects = _db.Subjects.Find(chapter.SubjectId);

            if (teacherSubject != null)
            {
                TeacherDateAndTeaching model = new TeacherDateAndTeaching()
                {
                    TCStartDate = DateTime.Now,
                    TCEndDate = DateTime.Now,
                    SubjectId = teacherSubject.SubjectId,
                    TeacherId = teacherSubject.TeacherId,
                    Teacher = _db.Teachers.Find(teacherSubject.TeacherId),
                    ChapterDate = chapterDateExist,
                    teachersubject = teacherSubject,
                    Chapter = chapter,
                    ClassId = subjects.ClassesId,
                    teacherTeachings = new List<TeacherTeaching>()
                };

                if (teacherchapterDate != null)
                {
                    model.ChapterId = eduyearid != null ? 0 : teacherchapterDate.ChapterId;
                    model.Chapter = chapter;
                    model.TCStartDate = teacherchapterDate.TCStartDate;
                    model.TCEndDate = teacherchapterDate.TCEndDate;
                    model.ChapterDate = chapterDateExist;
                    model.Teacher = teacherchapterDate.Teacher;
                    model.TeacherId = teacherchapterDate.TeacherId;
                    model.ClassId = subjects.ClassesId;
                }

                List<TeacherTeaching> lstTeacherTeaching = new List<TeacherTeaching>();

                lstTeacherTeaching = _db.TeacherTeachings.Where(d => d.ChapterId == id).ToList();

                var teachingType = _db.ChapterTeachings.Where(d => d.ChapterId == id).OrderBy(d => d.OrderId).ToList();

                foreach (var item in teachingType)
                {
                    if (item.Status)
                    {
                        TeacherTeaching findVal = new TeacherTeaching();
                        findVal = lstTeacherTeaching.Find(d => d.TeachingTypeId == item.TeachingTypeId);

                        TeacherTeaching chapterTeaching = new TeacherTeaching()
                        {
                            OrderId = item.OrderId,
                            MinVal = item.MinVal,
                            MaxVal = item.MaxVal,
                            Marks = findVal != null ? findVal.Marks : 0,
                            ChapterId = item.ChapterId,
                            TeachingTypeId = item.TeachingTypeId,
                            TeacherTeachingId = findVal != null ? findVal.TeacherTeachingId : 0,
                            TeachingType = _db.TeachingTypes.Find(item.TeachingTypeId)
                        };
                        model.teacherTeachings.Add(chapterTeaching);
                    }
                }


                return View(model);
            }

            return View("");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult SettingsDisplay([Bind(Include = "TeacherId,TCStartDate,TCEndDate,ChapterDate")] TeacherDateAndTeaching model)
        {
            if (!LoginStatus())
                return RedirectToAction("Login", "Admins", null);

            Chapter chapter = _db.Chapters.Find(model.ChapterDate.ChapterId);

            if (ModelState.IsValid)
            {
                if (model.TCStartDate >= model.TCEndDate)
                    return Json("start date should be before end date");


                TeacherChapterDate chapterDate = new TeacherChapterDate()
                {
                    Chapter = chapter,
                    Teacher = _db.Teachers.Find(model.TeacherId),
                    TeacherId = model.TeacherId,
                    TCStartDate = model.TCStartDate,
                    TCEndDate = model.TCEndDate,
                    ChapterId = model.ChapterDate.ChapterId
                };

                bool Exists = _db.TeacherChapterDates.Any(d => d.ChapterId == model.ChapterDate.ChapterId && d.TeacherId == model.TeacherId);
                if (!Exists)
                {
                    _db.TeacherChapterDates.Add(chapterDate);
                    _db.SaveChanges();
                }
                else
                {
                    _db.Entry(chapterDate).State = EntityState.Modified;
                    _db.SaveChanges();
                }
                return Json("");
            }
            else
            {
                return Json("Model is not valid.");
            }

        }

        //SubmitChapterTeaching
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult SubmitTeacherTeaching([Bind(Include = "teacherTeachings")] TeacherDateAndTeaching model)
        {
            if (!LoginStatus())
                return RedirectToAction("Login", "Admins", null);

            if (ModelState.IsValid)
            {

                if (model.teacherTeachings != null)
                {
                    foreach (var item in model.teacherTeachings)
                    {
                        bool Exists = _db.TeacherTeachings.Any(d => d.ChapterId == item.ChapterId && d.TeachingTypeId == item.TeachingTypeId);

                        TeacherTeaching model2 = new TeacherTeaching()
                        {
                            Chapter = _db.Chapters.Find(item.ChapterId),
                            ChapterId = item.ChapterId,
                            MaxVal = item.MaxVal,
                            MinVal = item.MinVal,
                            Marks = item.Marks,
                            OrderId = item.OrderId,
                            TeacherTeachingId = item.TeacherTeachingId,
                            TeachingType = _db.TeachingTypes.Find(item.TeachingTypeId),
                            TeachingTypeId = item.TeachingTypeId
                        };

                        if (model2.Marks <= model2.MaxVal)
                        {

                            if (!Exists)
                            {
                                _db.TeacherTeachings.Add(model2);
                            }
                            else
                            {
                                _db.Entry(model2).State = EntityState.Modified;
                            }
                            _db.SaveChanges();
                        }
                        else
                        {
                            return Json("It should be less than to max value.");
                        }
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