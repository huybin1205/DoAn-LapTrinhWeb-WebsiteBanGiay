using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebsiteBanGiay.Models;

namespace WebsiteBanGiay.Controllers
{
    public class ProductController : Controller
    {
        dbQuanLyBanGiayDataContext db = new dbQuanLyBanGiayDataContext();
        // GET: Product
        public ActionResult SanPham(int id)
        {
            var list = from s in db.Giays where s.MaDM == id select s;
            DanhMuc dm = db.DanhMucs.SingleOrDefault(m => m.MaDM == id);
            ViewBag.TieuDe = dm.TenDM;
            return View(list);
        }

        public ActionResult ChiTietSanPham(int id)
        {
            return View();
        }
    }
}