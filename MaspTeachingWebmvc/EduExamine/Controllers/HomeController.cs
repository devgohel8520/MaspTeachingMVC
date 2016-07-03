using EduExamine.Models;
using System.Web;
using System.Web.Mvc;

namespace EduExamine.Controllers
{
    public class HomeController : Controller
    {
        EduExamineContext _db;
        public HttpCookie userCookie;
        public string AdminType;

        public HomeController()
        {
            _db = new EduExamineContext();
        }
        public ActionResult Index()
        {
            return View();
        }
    }
}