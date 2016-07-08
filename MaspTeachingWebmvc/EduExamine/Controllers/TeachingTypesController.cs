using EduExamine.Models;
using System;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace EduExamine.Controllers
{
    public class TeachingTypesController : Controller
    {
        private EduExamineContext _db;
        public HttpCookie userCookie;
        public AdminType AdminType;

        public TeachingTypesController()
        {
            _db = new EduExamineContext();
        }

        public bool LoginStatus()
        {
            if (Request.Cookies["MapsUser"] != null)
            {
                userCookie = HttpContext.Request.Cookies["MapsUser"];
                AdminType = (AdminType)Enum.Parse(typeof(AdminType), userCookie["Type"], true);
                return true;
            }
            return false;
        }

        public ActionResult TTIndex()
        {
            if (!LoginStatus())
                return RedirectToAction("Login", "Admins", null);

            return View(_db.TeachingTypes.ToList());
        }


        public ActionResult Create()
        {
            if (!LoginStatus())
                return RedirectToAction("Login", "Admins", null);
            TeachingType model = new TeachingType() { Name = "", OrderId = (_db.TeachingTypes.Count() + 1) };
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Name,OrderId")] TeachingType model)
        {
            if (!LoginStatus())
                return RedirectToAction("Login", "Admins", null);

            bool Exists = _db.TeachingTypes.Any(d => d.Name.Equals(model.Name));
            if (!Exists)
            {
                if (ModelState.IsValid)
                {
                    _db.TeachingTypes.Add(model);
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

        public ActionResult Edit(int? id)
        {
            if (!LoginStatus())
                return RedirectToAction("Login", "Admins", null);

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TeachingType teachType = _db.TeachingTypes.Find(id);
            if (teachType == null)
            {
                return HttpNotFound();
            }
            return View(teachType);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "TeachingTypeId,Name,OrderId")] TeachingType model)
        {
            if (!LoginStatus())
                return RedirectToAction("Login", "Admins", null);

            bool Exists = _db.TeachingTypes.Any(d => d.Name.Equals(model.Name));
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

        public ActionResult Delete(int? id)
        {
            if (!LoginStatus())
                return RedirectToAction("Login", "Admins", null);


            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TeachingType teachingType = _db.TeachingTypes.Find(id);
            if (teachingType == null)
            {
                return HttpNotFound();
            }
            return View(teachingType);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            if (!LoginStatus())
                return RedirectToAction("Login", "Admins", null);

            TeachingType teachingType = _db.TeachingTypes.Find(id);
            _db.TeachingTypes.Remove(teachingType);
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
