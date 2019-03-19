using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebsiteBanGiay.Models;

namespace WebsiteBanGiay.Controllers
{
    public class HomeController : Controller
    {
        DatabaseDataContext db = new DatabaseDataContext();
        // GET: Home
        public ActionResult Index()
        {
            return View();
        }

        public PartialViewResult SliderPartial()
        {
            return PartialView();
        }

        public PartialViewResult BoxesPartial()
        {
            return PartialView();
        }

        public PartialViewResult FeaturesParital()
        {
            return PartialView();
        }

        public PartialViewResult ProductsPartial()
        {
            return PartialView();
        }

        public PartialViewResult NavigationPartial()
        {
            var list = from s in db.Loais select s;
            return PartialView(list);
        }

        public PartialViewResult BranchPartial()
        {
            var list = from s in db.CuaHangs select s;
            return PartialView(list);
        }
    }
}