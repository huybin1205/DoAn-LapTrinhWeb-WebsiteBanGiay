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
            var giay = from s in db.Giays where s.MaGiay ==id select s;
            List<DanhMuc> list = db.DanhMucs.ToList();
            List<Giay> listGiay = db.Giays.ToList();
            Giay itemgiay = new Giay();
            foreach(Giay item in listGiay)
            {
                if(item.MaGiay == id)
                {
                    itemgiay = item;
                }
            }
            foreach(DanhMuc item in list)
            {
                if(item.MaDM == itemgiay.MaDM)
                {
                    ViewBag.DanhMuc = item.TenDM;
                    ViewBag.Ma = item.MaDM;
                }
            }
            return View(giay.SingleOrDefault());
        }
    }
}