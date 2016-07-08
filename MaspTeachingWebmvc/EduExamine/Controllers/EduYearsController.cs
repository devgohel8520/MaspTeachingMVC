using EduExamine.Models;
using PagedList;
using System;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace EduExamine.Controllers
{
    public class EduYearsController : Controller
    {
        EduExamineContext _db;
        public HttpCookie userCookie;
        public AdminType AdminType;

        public EduYearsController()
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


        public ActionResult EdIndex(int? page, string Search = null)
        {
            if (!LoginStatus())
                return RedirectToAction("Login", "Admins", null);

            if (Search != null)
            {
                var model = _db.EduYears.Where(d => d.EduYearName.Contains(Search)).OrderByDescending(o => o.EduYearName).ToList();
                return View(model.ToPagedList(page ?? 1, 10));
            }
            else
            {
                var model = _db.EduYears.OrderByDescending(o => o.EduYearName).ToList();
                return View(model.ToPagedList(page ?? 1, 10));
            }
        }


        public ActionResult Create()
        {
            if (!LoginStatus())
                return RedirectToAction("Login", "Admins", null);

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "EduYearName,EduStart,EduEnd")] EduYear model)
        {
            if (!LoginStatus())
                return RedirectToAction("Login", "Admins", null);
            if (model.EduStart >= model.EduEnd)
                return Json("start date should be before end date");

            bool Exists = _db.EduYears.Any(d => d.EduYearName.Equals(model.EduYearName));
            if (!Exists)
            {
                if (ModelState.IsValid)
                {
                    _db.EduYears.Add(model);
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
            EduYear eduYear = _db.EduYears.Find(id);
            if (eduYear == null)
            {
                return HttpNotFound();
            }
            return View(eduYear);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "EduYearId,EduYearName,EduStart,EduEnd")] EduYear model)
        {
            if (!LoginStatus())
                return RedirectToAction("Login", "Admins", null);
            if (model.EduStart >= model.EduEnd)
                return Json("start date should be before end date");

            bool Exists = _db.EduYears.Any(d => d.EduYearName.Equals(model.EduYearName));
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
            EduYear eduYear = _db.EduYears.Find(id);
            if (eduYear == null)
            {
                return HttpNotFound();
            }
            return View(eduYear);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            if (!LoginStatus())
                return RedirectToAction("Login", "Admins", null);

            EduYear eduYear = _db.EduYears.Find(id);
            _db.EduYears.Remove(eduYear);
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
