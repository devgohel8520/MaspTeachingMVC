using EduExamine.Models;
using System;
using System.Linq;
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
        public ActionResult AdIndexAdmin(int? menuId)
        {
            if (!LoginStatus())
                return RedirectToAction("Login", "Admins", null);

            if (menuId != null)
            {
                var model = _db.Admins.Where(d => d.Types == (AdminType)menuId).ToList();
                if (model.Count == 0)
                {
                    Admin admin = new Admin() { Types = (AdminType)menuId, AdminId = 0, Name = "" };
                    model.Add(admin);
                }
                return View(model);
            }
            else
            {
                return View(_db.Admins.Where(d => d.Types == 0).ToList());
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
    }
}