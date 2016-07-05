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
    public class ClassesController : Controller
    {
        EduExamineContext _db;
        public HttpCookie userCookie;
        public AdminType AdminType;

        public ClassesController()
        {
            _db = new EduExamineContext();
        }

        public bool LoginStatus()
        {
            UserStatus logInfo = new UserStatus();
            if (Request.Cookies["MapsUser"] != null)
            {
                userCookie = HttpContext.Request.Cookies["MapsUser"];
                AdminType = (AdminType)Enum.Parse(typeof(AdminType), userCookie["Type"], true);
                return true;
            }
            return false;
        }


        public ActionResult CIndex(int? page)
        {
            if (!LoginStatus())
                return RedirectToAction("Login", "Admins", null);

            var model = _db.Classess.OrderBy(o => o.ClassName).ToList();
            return View(model.ToPagedList(page ?? 1, 20));
        }

        public ActionResult Create()
        {
            if (!LoginStatus())
                return RedirectToAction("Login", "Admins", null);

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ClassName")] Classes model)
        {
            if (!LoginStatus())
                return RedirectToAction("Login", "Admins", null);

            bool Exists = _db.Classess.Any(d => d.ClassName.Equals(model.ClassName));
            if (!Exists)
            {
                if (ModelState.IsValid)
                {
                    _db.Classess.Add(model);
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
            Classes classes = _db.Classess.Find(id);
            if (classes == null)
            {
                return HttpNotFound();
            }
            return View(classes);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ClassesId,ClassName")] Classes model)
        {
            if (!LoginStatus())
                return RedirectToAction("Login", "Admins", null);

            bool Exists = _db.Classess.Any(d => d.ClassName.Equals(model.ClassName));
            if (!Exists)
            {

                if (ModelState.IsValid)
                {
                    _db.Entry(model).State = EntityState.Modified;
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

        public ActionResult Delete(long? id)
        {
            if (!LoginStatus())
                return RedirectToAction("Login", "Admins", null);

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Classes classes = _db.Classess.Find(id);
            if (classes == null)
            {
                return HttpNotFound();
            }
            return View(classes);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(long id)
        {
            if (!LoginStatus())
                return RedirectToAction("Login", "Admins", null);

            Classes classes = _db.Classess.Find(id);
            _db.Classess.Remove(classes);
            _db.SaveChanges();
            return Json("");
        }

        //Subject Control of class

        public ActionResult SubjectDisplay(int? page, int? id)
        {
            if (!LoginStatus())
                return RedirectToAction("Login", "Admins", null);

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var model = _db.Subjects.Include(d => d.Classes).Where(d => d.ClassesId == id).OrderBy(d => d.SubjectName);
            if (model.Count() == 0)
            {
                List<Subject> subList = new List<Subject>();
                subList.Add(new Subject() { Classes = _db.Classess.Find(id) });
                return View(subList.ToPagedList(page ?? 1, 20));
            }
            return View(model.ToPagedList(page ?? 1, 20));
        }

        public ActionResult SubjectCreate(int? ClassesId)
        {
            if (!LoginStatus())
                return RedirectToAction("Login", "Admins", null);
            Classes classes = _db.Classess.Find(ClassesId);
            Subject subject = new Subject() { ClassesId = classes.ClassesId, Classes = classes };

            return View(subject);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult SubjectCreate([Bind(Include = "SubjectName,ClassesId")] Subject model)
        {
            if (!LoginStatus())
                return RedirectToAction("Login", "Admins", null);

            bool Exists = _db.Subjects.Any(d => d.SubjectName.Equals(model.SubjectName) && d.ClassesId.Equals(model.ClassesId));
            if (!Exists)
            {
                if (ModelState.IsValid)
                {
                    _db.Subjects.Add(model);
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

        public ActionResult SubjectEdit(long? id)
        {
            if (!LoginStatus())
                return RedirectToAction("Login", "Admins", null);

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Subject subject = _db.Subjects.Find(id);
            if (subject == null)
            {
                return HttpNotFound();
            }
            return View(subject);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult SubjectEdit([Bind(Include = "SubjectId,SubjectName,ClassesId")] Subject model)
        {
            if (!LoginStatus())
                return RedirectToAction("Login", "Admins", null);

            bool Exists = _db.Subjects.Any(d => d.SubjectName.Equals(model.SubjectName) && d.ClassesId.Equals(model.ClassesId));
            if (!Exists)
            {
                if (ModelState.IsValid)
                {
                    _db.Entry(model).State = EntityState.Modified;
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

        public ActionResult SubjectDelete(long? id)
        {
            if (!LoginStatus())
                return RedirectToAction("Login", "Admins", null);

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Subject subject = _db.Subjects.Find(id);
            if (subject == null)
            {
                return HttpNotFound();
            }
            return View(subject);
        }

        [HttpPost, ActionName("SubjectDelete")]
        [ValidateAntiForgeryToken]
        public ActionResult SubjectDeleteConfirmed(long id)
        {
            if (!LoginStatus())
                return RedirectToAction("Login", "Admins", null);

            Subject subject = _db.Subjects.Find(id);
            _db.Subjects.Remove(subject);
            _db.SaveChanges();
            return Json("");
        }

        //Chapter Control of subject

        public ActionResult ChapterDisplay(int? page, int? id)
        {
            if (!LoginStatus())
                return RedirectToAction("Login", "Admins", null);

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var model = _db.Chapters.Include(d => d.Subject).Include(d => d.Subject.Classes).Where(d => d.SubjectId == id).OrderBy(d => d.ChapterId);
            if (model.Count() == 0)
            {
                List<Chapter> chapList = new List<Chapter>();
                Subject subject = _db.Subjects.Include(d => d.Classes).First(d => d.SubjectId == id);
                chapList.Add(new Chapter() { Subject = subject });
                return View(chapList.ToPagedList(page ?? 1, 20));
            }
            return View(model.ToPagedList(page ?? 1, 20));
        }

        public ActionResult ChapterCreate(int? SubjectId)
        {
            if (!LoginStatus())
                return RedirectToAction("Login", "Admins", null);

            Subject subject = _db.Subjects.Find(SubjectId);
            Chapter chapter = new Chapter() { SubjectId = subject.SubjectId, Subject = subject };

            return View(chapter);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ChapterCreate([Bind(Include = "ChapterName,SubjectId")] Chapter model)
        {
            if (!LoginStatus())
                return RedirectToAction("Login", "Admins", null);

            if (ModelState.IsValid)
            {
                int noofChapter = 0;
                if (int.TryParse(model.ChapterName, out noofChapter))
                {
                    for (int i = 1; i <= noofChapter; i++)
                    {
                        model.ChapterName = "Chapter " + Convert.ToString(i);
                        bool Exists = _db.Chapters.Any(d => d.ChapterName.Equals(model.ChapterName) && d.SubjectId.Equals(model.SubjectId));
                        if (!Exists)
                        {
                            _db.Chapters.Add(model);
                            _db.SaveChanges();
                        }
                    }
                }
            }
            else
            {
                return Json("Model is not valid.");
            }

            return Json("");
        }


        public ActionResult ChapterEdit(long? id)
        {
            if (!LoginStatus())
                return RedirectToAction("Login", "Admins", null);

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Chapter chapter = _db.Chapters.Find(id);
            if (chapter == null)
            {
                return HttpNotFound();
            }
            return View(chapter);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ChapterEdit([Bind(Include = "ChapterId,ChapterName,SubjectId")] Chapter model)
        {
            if (!LoginStatus())
                return RedirectToAction("Login", "Admins", null);

            bool Exists = _db.Chapters.Any(d => d.ChapterName.Equals(model.ChapterName) && d.SubjectId.Equals(model.SubjectId));
            if (!Exists)
            {
                if (ModelState.IsValid)
                {
                    _db.Entry(model).State = EntityState.Modified;
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

        public ActionResult ChapterDelete(long? id)
        {
            if (!LoginStatus())
                return RedirectToAction("Login", "Admins", null);

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Chapter chapter = _db.Chapters.Find(id);
            if (chapter == null)
            {
                return HttpNotFound();
            }
            return View(chapter);
        }

        [HttpPost, ActionName("ChapterDelete")]
        [ValidateAntiForgeryToken]
        public ActionResult ChapterDeleteConfirmed(long id)
        {
            if (!LoginStatus())
                return RedirectToAction("Login", "Admins", null);

            Chapter chapter = _db.Chapters.Find(id);
            if (chapter != null)
            {
                _db.Chapters.Remove(chapter);
                _db.SaveChanges();
                return Json("");
            }
            else
            {
                return Json("No record found.");
            }
        }

        //Chapter Subject
        public ActionResult SettingsDisplay(int? id)
        {
            if (!LoginStatus())
                return RedirectToAction("Login", "Admins", null);

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            ViewBag.EduYearId = new SelectList(_db.EduYears, "EduYearId", "EduYearName");
            ChapterDate model = _db.ChapterDates.Find(id);
            if (model == null)
            {
                Chapter chapter = _db.Chapters.Find(id);
                model = new ChapterDate() { ChapterId = (long)id, Chapter = chapter };
                return View(model);
            }
            else
            {
                model.Chapter = _db.Chapters.Find(id);
                return View(model);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult SettingsDisplay([Bind(Include = "ChapterId,CStartDate,CEndDate,EduYearId")] ChapterDate model)
        {
            if (!LoginStatus())
                return RedirectToAction("Login", "Admins", null);

            if (ModelState.IsValid)
            {
                bool Exists = _db.ChapterDates.Any(d => d.ChapterId == model.ChapterId);
                if (!Exists)
                {
                    _db.ChapterDates.Add(model);
                    _db.SaveChanges();
                }
                else
                {
                    _db.Entry(model).State = EntityState.Modified;
                    _db.SaveChanges();
                }
                return Json("");
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
