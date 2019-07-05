using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebsiteBanDoThoiTrang.Models;

namespace WebsiteBanDoThoiTrang.Controllers
{
    public class GioHangController : Controller
    {
        dbQuanLyBanDoThoiTrangDataContext db = new dbQuanLyBanDoThoiTrangDataContext();
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
            GioHang sp = lst.Find(n =>n.maHang == ma);
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
            GioHang sp = lst.SingleOrDefault(n => n.maHang == ma);
            if(sp!=null)
            {
                lst.RemoveAll(n => n.maHang == ma);
                return RedirectToAction("GioHang");
            }
            if(lst.Count == 0)
            {
                return RedirectToAction("Index", "Home");
            }
            return RedirectToAction("GioHang");
        }

        public ActionResult CapNhatGioHang(int idHang, FormCollection f)
        {
            List<GioHang> lst = layGioHang();
            GioHang sp = lst.SingleOrDefault(n => n.maHang == idHang);
            if (sp != null)
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

        public ActionResult DatHang()
        {
            if(Session["Taikhoan"]==null || Session["Taikhoan"].ToString()=="")
            {
                return RedirectToAction("Login","User");
            }
            if(Session["GioHang"]==null)
            {
                return RedirectToAction("Index","Home");
            }
            List<GioHang> list = layGioHang();
            ViewBag.TongSoLuong = tongSoluong();
            ViewBag.TongTien = tongThanhTien();
            return View(list);
        }

        [HttpPost]
        public ActionResult DatHang(FormCollection f)
        {
            string ten = "";
            int gia = 0;
            int sl = 0;

            DonDatHang ddh = new DonDatHang();
            KhachHang kh = (KhachHang) Session["Taikhoan"];
            List<GioHang> gh = layGioHang();
            ddh.MaKH = kh.MaKH;
            ddh.NgayDat = DateTime.Now;
            var ngaygiao = String.Format("{0:MM/dd/yyyy}",f["NgayGiao"].ToString());
            ddh.NgayGiao = DateTime.Parse(ngaygiao);
            ddh.TinhTrangGiaoHang = false;
            ddh.DaThanhToan = false;
            db.DonDatHangs.InsertOnSubmit(ddh);
            db.SubmitChanges();

            foreach(var item in gh)
            {
                ChiTietDonHang ctdh = new ChiTietDonHang();
                ctdh.MaDonHang = ddh.MaDonHang;
                ctdh.MaHang = item.maHang;
                ctdh.Soluong = item.soLuong;
                ctdh.DonGia = (decimal)item.donGia;
                db.ChiTietDonHangs.InsertOnSubmit(ctdh);
                
                foreach(MatHang g in db.MatHangs.ToList())
                {
                    if(g.MaHang == ctdh.MaHang)
                    {
                        ten += g.TenHang+",";
                    }
                }
            }
            db.SubmitChanges();
            foreach(ChiTietDonHang a in db.ChiTietDonHangs.Where(n=>n.MaDonHang == ddh.MaDonHang).ToList())
            {
                sl += (int) a.Soluong;
                gia += (int) (a.DonGia * a.Soluong);
            }
            string url = "https://www.baokim.vn/payment/product/version11?business=huyprosoccer@gmail.com&id=&order_description=ABC" + "&product_name=" + ten.Substring(0,(ten.Length-1)) + "&product_price="+ gia + "&product_quantity="+sl + "&total_amount=" + gia + "&url_cancel=&url_detail=&url_success=" + Url.Action("XacNhanDonHang","GioHang",new {idDH = ddh.MaDonHang, kt = 1});

            Session["GioHang"] = null;
            return Redirect(url);
        }

        public ActionResult DatHangOnline()
        {
            if (Session["Taikhoan"] == null || Session["Taikhoan"].ToString() == "")
            {
                return RedirectToAction("Login", "User");
            }
            if (Session["GioHang"] == null)
            {
                return RedirectToAction("Index", "Home");
            }
            List<GioHang> list = layGioHang();
            ViewBag.TongSoLuong = tongSoluong();
            ViewBag.TongTien = tongThanhTien();
            return View(list);
        }

        [HttpPost]
        public ActionResult DatHangOnline(FormCollection f)
        {
            string ten = "";
            int gia = 0;
            int sl = 0;

            DonDatHang ddh = new DonDatHang();
            KhachHang kh = (KhachHang)Session["Taikhoan"];
            List<GioHang> gh = layGioHang();
            ddh.MaKH = kh.MaKH;
            ddh.NgayDat = DateTime.Now;
            var ngaygiao = String.Format("{0:MM/dd/yyyy}", f["NgayGiao"]);
            ddh.NgayGiao = DateTime.Parse(ngaygiao);
            ddh.TinhTrangGiaoHang = false;
            ddh.DaThanhToan = false;
            db.DonDatHangs.InsertOnSubmit(ddh);
            db.SubmitChanges();
            foreach (var item in gh)
            {
                ChiTietDonHang ctdh = new ChiTietDonHang();
                ctdh.MaDonHang = ddh.MaDonHang;
                ctdh.MaHang = item.maHang;
                ctdh.Soluong = item.soLuong;
                ctdh.DonGia = (decimal)item.donGia;
                db.ChiTietDonHangs.InsertOnSubmit(ctdh);
            }
            db.SubmitChanges();
            Session["GioHang"] = null;
            return RedirectToAction("XacNhanDonHang", "GioHang", new { idDH = ddh.MaDonHang, kt = 0});
        }

        public ActionResult XacNhanDonHang(int idDH,int kt)
        {
            if(kt == 1)
            {
                var obj = db.DonDatHangs.SingleOrDefault(p=>p.MaDonHang == idDH);
                obj.DaThanhToan = true;
                db.SubmitChanges();
            }
            return View();
        }
    }
}