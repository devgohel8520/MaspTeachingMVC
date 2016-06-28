using MaspTeachingWebmvc.Models;
using System.Web;
using System.Web.Mvc;

namespace MaspTeachingWebmvc.Controllers
{
    public class HomeController : Controller
    {
        MapsDbContext _db;
        public HttpCookie userCookie;
        public string AdminType;


        public HomeController()
        {
            _db = new MapsDbContext();
        }


        public ActionResult Index()
        {
            return View();
        }

    }
}