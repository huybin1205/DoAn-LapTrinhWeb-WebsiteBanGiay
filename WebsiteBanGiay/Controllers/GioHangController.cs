using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebsiteBanGiay.Models;

namespace WebsiteBanGiay.Controllers
{
    public class GioHangController : Controller
    {
        dbQuanLyBanGiayDataContext db = new dbQuanLyBanGiayDataContext();
        // GET: GioHang
        public ActionResult Index()
        {
            
            return View();
        }

        public List<GioHang> layGioHang()
        {
            List<GioHang> list = Session["GioHang"] as List<GioHang>;
            if(list==null)
            {
                list = new List<GioHang>();
                Session["GioHang"] = list;
            }
            return list;
        }

        public ActionResult ThemGioHang(int ma, string url)
        {
            List<GioHang> lst = layGioHang();
            GioHang sp = lst.Find(n =>n.maGiay == ma);
            if(sp == null)
            {
                sp = new GioHang(ma);
                lst.Add(sp);
                return Redirect(url);
            }
            else
            {
                sp.soLuong++;
                return Redirect(url);
            }
        }

        private int tongSoluong()
        {
            int tongSL = 0;
            List<GioHang> lst = Session["GioHang"] as List<GioHang>;
            if (lst != null)
                tongSL = lst.Sum(n=>n.soLuong);
            return tongSL;
        }

        private double tongThanhTien()
        {
            double tongThanhTien = 0;
            List<GioHang> lst = Session["GioHang"] as List<GioHang>;
            if (lst != null)
                tongThanhTien = lst.Sum(n => n.thanhTien);
            return tongThanhTien;
        }

        public ActionResult GioHang()
        {
            List<GioHang> lst = layGioHang();
            if(lst.Count == 0)
            {
                return RedirectToAction("Index","Home");
            }
            ViewBag.TongSoLuong = tongSoluong();
            ViewBag.TongThanhTien = tongThanhTien();
            return View(lst);
        }

        public ActionResult XoaGioHang(int ma)
        {
            List<GioHang> lst = layGioHang();
            GioHang sp = lst.SingleOrDefault(n => n.maGiay == ma);
            if(sp!=null)
            {
                lst.RemoveAll(n => n.maGiay == ma);
                return RedirectToAction("GioHang");
            }
            if(lst.Count == 0)
            {
                return RedirectToAction("Index", "Home");
            }
            return RedirectToAction("GioHang");
        }

        public ActionResult CapNhatGioHang(int ma, FormCollection f)
        {
            List<GioHang> lst = layGioHang();
            GioHang sp = lst.SingleOrDefault(n => n.maGiay == ma);
            if (sp == null)
            {
                sp.soLuong = int.Parse(f["txtSL"].ToString());
            }
            return RedirectToAction("GioHang");
        }

        public ActionResult XoaTatCaGioHang()
        {
            List<GioHang> lst = layGioHang();
            lst.Clear();
            return RedirectToAction("Index","Home");
        }
    }
}