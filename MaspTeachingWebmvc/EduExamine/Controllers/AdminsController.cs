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
    public class AdminsController : Controller
    {
        EduExamineContext _db;
        public HttpCookie userCookie;
        public AdminType AdminType;

        public AdminsController()
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

        public ActionResult AdIndex()
        {
            if (!LoginStatus())
                return RedirectToAction("Login", "Admins", null);

            return View();
        }

        public ActionResult Login()
        {
            if (LoginStatus())
            {
                return Redirect("AdIndex");
            }
            else
            {
                return View();
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login([Bind(Include = "Name,Password,Types")] Admin model)
        {
            try
            {
                int adminTypeValue = (int)model.Types;
                if (adminTypeValue != 2 && adminTypeValue != 3)
                {
                    if (ModelState.IsValid)
                    {
                        var query = _db.Admins.FirstOrDefault(s => s.Name == model.Name && s.Password == model.Password);

                        if (query != null)
                        {
                            if (query.Types.Equals(model.Types))
                            {
                                HttpCookie userInfo = new HttpCookie("MapsUser");
                                userInfo.Expires = DateTime.Now.AddDays(1);
                                userInfo.Values["UserId"] = query.AdminId.ToString();
                                userInfo.Values["Email"] = query.Name;
                                //userInfo.Values["Name"] = query.AdminProfile.FirstName.ToUpper() + " " + query.AdminProfile.LastName.ToUpper();
                                userInfo.Values["Type"] = query.Types.ToString();
                                Response.Cookies.Add(userInfo);
                                return Json("");
                            }
                            else
                            {
                                return Json("Incorrect information.");
                            }
                        }
                        else
                        {
                            return Json("Incorrect email id or password.");
                        }

                    }
                    else
                    {
                        return Json("Please fill login information.");
                    }
                }
                else
                {
                    return Json("You are not authenticated.");
                }

            }
            catch (Exception ex)
            {
                return Json("Error: " + ex.Message.ToString());
            }
        }

        [HttpGet]
        public ActionResult AdIndexAdmin(int? menuId, int? page)
        {
            if (!LoginStatus())
                return RedirectToAction("Login", "Admins", null);

            if (menuId != null)
            {
                var model = _db.Admins.Where(d => d.Types == (AdminType)menuId).ToList();
                if (model.Count == 0)
                {
                    Admin item = new Admin() { Types = (AdminType)menuId, AdminId = 0, Name = "" };
                    List<Admin> teacher = new List<Admin>();
                    teacher.Add(item);
                    return View(teacher.ToPagedList(page ?? 1, 10));
                }
                return View(model.ToPagedList(page ?? 1, 10));
            }
            else
            {
                var model = _db.Admins.Where(d => d.Types == AdminType.Admin).ToList();
                if (model.Count == 0)
                {
                    Admin item = new Admin() { Types = AdminType.Admin, AdminId = 0, Name = "" };
                    List<Admin> teacher = new List<Admin>();
                    teacher.Add(item);
                    return View(teacher.ToPagedList(page ?? 1, 10));
                }
                return View(model.ToPagedList(page ?? 1, 10));
            }
        }


        public ActionResult Logout()
        {
            if (Request.Cookies["MapsUser"] != null)
            {
                HttpCookie mybookUser = new HttpCookie("MapsUser");
                mybookUser.Expires = DateTime.Now.AddDays(-1d);
                Response.Cookies.Add(mybookUser);
                return RedirectToAction("Login", "Admins", null);
            }
            return View();
        }

        [HttpGet]
        public ActionResult AdCreate(int? menuId)
        {
            if (!LoginStatus())
                return RedirectToAction("Login", "Admins", null);

            Admin admin = new Admin() { Types = (AdminType)menuId, AdminId = 0, Name = "" };
            return View(admin);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AdCreate([Bind(Include = "Name,Password,Types")] Admin model)
        {
            if (!LoginStatus())
                return RedirectToAction("Login", "Admins", null);

            bool Exists = _db.Admins.Any(d => d.Name.Equals(model.Name));
            if (!Exists)
            {
                if (ModelState.IsValid)
                {
                    _db.Admins.Add(model);
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
        public ActionResult AdEdit(int? menuId, long? Adminid)
        {
            if (!LoginStatus())
                return RedirectToAction("Login", "Admins", null);

            if (AdminType != 0)
            {
                return RedirectToAction("AdIndexAdmin", "Admins", new { menuId = menuId });
            }
            if (Adminid == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Admin model = _db.Admins.Find(Adminid);

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
        public ActionResult AdEdit([Bind(Include = "AdminId,Name,Password,Types")] Admin model)
        {
            if (!LoginStatus())
                return RedirectToAction("Login", "Admins", null);

            if (AdminType != 0)
            {
                return RedirectToAction("AdIndex", "Admins", null);
            }
            if (ModelState.IsValid)
            {
                _db.Entry(model).State = EntityState.Modified;
                _db.SaveChanges();
                return Json("");
            }
            return View(model);
        }

        public ActionResult AdDelete(int? menuId, long? Adminid)
        {
            if (!LoginStatus())
                return RedirectToAction("Login", "Admins", null);

            if (AdminType != 0)
            {
                return RedirectToAction("AdIndex", "Admins", null);
            }

            if (Adminid == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Admin model = _db.Admins.Find(Adminid);
            if (model == null)
            {
                return HttpNotFound();
            }
            return View(model);
        }


        [HttpPost, ActionName("AdDelete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeletePConfirmed([Bind(Include = "AdminId,Name")] Admin model)
        {
            if (!LoginStatus())
                return RedirectToAction("Login", "Admins", null);

            if (AdminType != 0)
            {
                return RedirectToAction("AdIndex", "Admins", null);
            }

            Admin admin = _db.Admins.Find(model.AdminId);
            _db.Admins.Remove(admin);
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