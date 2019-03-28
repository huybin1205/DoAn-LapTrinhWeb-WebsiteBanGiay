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
        dbQuanLyBanGiayDataContext db = new dbQuanLyBanGiayDataContext();
        // GET: Home
        public ActionResult Index()
        {
            return View();
        }

        public PartialViewResult SliderPartial()
        {
            var list = from s in db.Banners orderby s.MaBanner descending select s;
            return PartialView(list.Take(4));
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
            var list = db.Giays.Take(9).ToList();
            return PartialView(list);
        }

        public PartialViewResult ProductNewPartial()
        {
            var list = from s in db.Giays orderby s.MaGiay descending select s;
            return PartialView(list.Take(9).ToList());
        }

        public PartialViewResult BestProductPartial()
        {
            var list = from s in db.Giays orderby s.SoLuongBan descending select s;
            return PartialView(list.Take(9).ToList());
        }

        public PartialViewResult NavigationPartial()
        {
            var list = from s in db.DanhMucs select s;
            return PartialView(list);
        }

        public PartialViewResult BranchPartial()
        {
            var list = from s in db.CuaHangs select s;
            return PartialView(list);
        }

        public PartialViewResult GioHangIcon()
        {
            List<GioHang> list = Session["GioHang"] as List<GioHang>;
            if (list == null)
            {
                list = new List<GioHang>();
                Session["GioHang"] = list;
            }
            return PartialView(list);
        }
    }
}