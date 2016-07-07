using MaspTeachingWebmvc.Models;
using MaspTeachingWebmvc.ViewModel;
using System;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace MaspTeachingWebmvc.Controllers
{
    public class AdminsController : Controller
    {
        MapsDbContext _db;
        public HttpCookie userCookie;
        public int? AdminType;

        public AdminsController()
        {
            _db = new MapsDbContext();
        }

        public bool LoginStatus()
        {
            UserStatus logInfo = new UserStatus();
            if (Request.Cookies["MapsUser"] != null)
            {
                userCookie = HttpContext.Request.Cookies["MapsUser"];
                AdminType = Convert.ToInt32(userCookie["Type"]);
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
        public ActionResult Login([Bind(Include = "Name,Password,Types")] VAdmin model)
        {
            try
            {
                int adminTypeValue = (int)model.Types;
                if (adminTypeValue != 2 && adminTypeValue != 3)
                {
                    if (ModelState.IsValid)
                    {
                        var query = _db.Admins.FirstOrDefault(s => s.Name == model.Name && s.Password == model.Password && s.Status == true);

                        if (query != null)
                        {
                            if (query.Types.Equals((int)model.Types))
                            {
                                //var userName = query.AdminProfile.FirstName + " " + query.AdminProfile.LastName;

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
        public ActionResult AdIndexAdmin(int? menuId)
        {
            if (!LoginStatus())
                return RedirectToAction("Login", "Admins", null);

            if (menuId != null)
            {
                var model = _db.Admins.Where(d => d.Types == menuId).ToList();
                if (model.Count == 0)
                {
                    Admin admin = new Admin() { Types = (int)menuId, AdminId = 0, Name = "" };
                    model.Add(admin);
                }
                return View(model);
            }
            else
            {
                return View(_db.Admins.Where(d => d.Types == 0).ToList());
            }
        }

        [HttpGet]
        public ActionResult AdCreate(int? menuId)
        {

            if (!LoginStatus())
                return RedirectToAction("Login", "Admins", null);

            Admin admin = new Admin() { Types = (int)menuId, AdminId = 0, Name = "" };
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
                    model.Created = DateTime.Now;
                    model.Status = true;

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
        public ActionResult AdEdit([Bind(Include = "AdminId,Name,Password,Types,Created,Status")] Admin model)
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

    }
}