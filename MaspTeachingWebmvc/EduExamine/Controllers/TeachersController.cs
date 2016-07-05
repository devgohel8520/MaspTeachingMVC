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

        public TeachersController()
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

        [HttpGet]
        public ActionResult TIndex(int? menuid, int? page)
        {
            if (!LoginStatus())
                return RedirectToAction("Login", "Admins", null);

            if (menuid != null)
            {
                var model = _db.Teachers.Where(d => d.Types == (TeacherType)menuid).ToList();
                if (model.Count == 0)
                {
                    Teacher item = new Teacher() { Types = (TeacherType)menuid, TeacherId = 0, FullName = "" };
                    List<Teacher> teacher = new List<Teacher>();
                    teacher.Add(item);
                    return View(teacher.ToPagedList(page ?? 1, 10));
                }
                return View(model.ToPagedList(page ?? 1, 10));
            }
            else
            {
                var model = _db.Teachers.Where(d => d.Types == TeacherType.Principal).ToList();
                if (model.Count == 0)
                {
                    Teacher item = new Teacher() { Types = TeacherType.Principal, TeacherId = 0, FullName = "" };
                    List<Teacher> teacher = new List<Teacher>();
                    teacher.Add(item);
                    return View(teacher.ToPagedList(page ?? 1, 10));
                }
                return View(model.ToPagedList(page ?? 1, 10));
            }
        }

        [HttpGet]
        public ActionResult Create(int? menuid)
        {
            if (!LoginStatus())
                return RedirectToAction("Login", "Admins", null);

            Teacher teacher = new Teacher() { Types = (TeacherType)menuid, TeacherId = 0, FullName = "" };
            ViewBag.EduYearId = new SelectList(_db.EduYears, "EduYearId", "EduYearName");
            return View(teacher);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "FullName,LoginName,Password,Types,EduYearId")] Teacher model)
        {
            if (!LoginStatus())
                return RedirectToAction("Login", "Admins", null);

            bool Exists = _db.Teachers.Any(d => d.LoginName.Equals(model.LoginName));
            if (!Exists)
            {
                if (ModelState.IsValid)
                {
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
        public ActionResult Edit(int? menuid, long? teacherid)
        {
            if (!LoginStatus())
                return RedirectToAction("Login", "Admins", null);

            if (teacherid == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Teacher model = _db.Teachers.Find(teacherid);

            if (model == null)
            {
                return HttpNotFound();
            }
            else
            {
                ViewBag.EduYearId = new SelectList(_db.EduYears, "EduYearId", "EduYearName");
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
                _db.Entry(model).State = EntityState.Modified;
                _db.SaveChanges();
                return Json("");
            }
            return View(model);
        }


        public ActionResult Delete(int? menuid, long? teacherid)
        {
            if (!LoginStatus())
                return RedirectToAction("Login", "Admins", null);

            if (teacherid == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Teacher model = _db.Teachers.Find(teacherid);
            if (model == null)
            {
                return HttpNotFound();
            }
            return View(model);
        }


        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeletePConfirmed([Bind(Include = "TeacherId,Name")] Teacher model)
        {
            if (!LoginStatus())
                return RedirectToAction("Login", "Admins", null);

            Teacher admin = _db.Teachers.Find(model.TeacherId);
            _db.Teachers.Remove(admin);
            _db.SaveChanges();
            return Json("");
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