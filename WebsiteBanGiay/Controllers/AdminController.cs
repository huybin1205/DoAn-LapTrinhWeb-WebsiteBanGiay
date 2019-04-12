using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebsiteBanGiay.Models;
using PagedList;
using PagedList.Mvc;
using System.IO;
using System.Web.UI;

namespace WebsiteBanGiay.Controllers
{
    public class AdminController : Controller
    {
        dbQuanLyBanGiayDataContext db = new dbQuanLyBanGiayDataContext();
        public string chung = "";
        // GET: Admin
        public ActionResult LoginAdmin()
        {
            return View();
        }

        [HttpPost]
        public ActionResult LoginAdmin(FormCollection f)
        {
            var user = f["user"];
            var pass = f["password"];
            foreach(Admin item in db.Admins.ToList())
            {
                if(item.Username.CompareTo(user)==0 && item.Password.CompareTo(pass)==0)
                {
                    Session["Admin"] = item;
                    return RedirectToAction("Index","Admin");
                }
            }
            ViewBag.ThongBao = "Sai tên đăng nhập hoặc mật khẩu";
            return View();
        }


        public ActionResult Index()
        {
            if (chung.CompareTo("")==0)
                chung = "<script>alert('Welcome to Admin');</script>";
            TempData["msg"] = chung;
            if (Session["Admin"]==null)
            {
                return RedirectToAction("LoginAdmin", "Admin");
            }
            return View();
        }

        public ActionResult QuanLySanPham(int? page)
        {
            if (Session["Admin"] == null)
            {
                return RedirectToAction("Index", "Admin");
            }
            Admin ad = (Admin)Session["Admin"];
            PhanQuyen_Admin pq = db.PhanQuyen_Admins.SingleOrDefault(n => n.MaAdmin == ad.MaAdmin);
            if (pq.PQ_Giay == false)
            {
                return Content("<script language='javascript' type='text/javascript'>alert('Bạn không được phép truy cập vào mục này!');</script>");
            }
            //Số sản phẩm 1 trang
            int pageSize = 21;
            //Số trang
            int pageNum = (page ?? 1);
            var lst = from s in db.Giays select s;
            return View(lst.ToPagedList(pageNum, pageSize));
        }

        public ActionResult ThemMoiSP()
        {
            if (Session["Admin"] == null)
            {
                return RedirectToAction("LoginAdmin", "Admin");
            }
            Admin ad = (Admin)Session["Admin"];
            PhanQuyen_Admin pq = db.PhanQuyen_Admins.SingleOrDefault(n => n.MaAdmin == ad.MaAdmin);
            if (pq.PQ_Giay == false)
                return RedirectToAction("Index", "Admin");
            ViewBag.MaDM = new SelectList(db.DanhMucs.OrderBy(n => n.TenDM), "MaDM", "TenDM"); ;
            ViewBag.MaNXB = new SelectList(db.NhaXuatBans.OrderBy(n => n.TenNXB), "MaNXB", "TenNXB");
            return View();
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult ThemMoiSP(Giay g, HttpPostedFileBase fileUpload)
        {
            //Đưa dữ liệu vào dropdown
            ViewBag.MaDM = new SelectList(db.DanhMucs.OrderBy(n => n.TenDM), "MaDM", "TenDM"); ;
            ViewBag.MaNXB = new SelectList(db.NhaXuatBans.OrderBy(n => n.TenNXB), "MaNXB", "TenNXB");
            //Kiểm tra đường dẫn
            if (fileUpload == null)
            {
                ViewBag.Thongbao = "Vui lòng chọn ảnh";
                return View();
            }
            else
            {
                if (ModelState.IsValid)
                {
                    //Lưu tên file
                    var fileName = Path.GetFileName(fileUpload.FileName);
                    //Lưu đường dẫn
                    var path = Path.Combine(Server.MapPath("~/Images/Product"), fileName);
                    if (System.IO.File.Exists(path))
                    {
                        ViewBag.Thongbao = "Hình ảnh đã tồn tại";
                    }
                    else
                    {
                        //Lưu hình
                        fileUpload.SaveAs(path);
                    }
                    g.Anhbia = "/Product/" + fileName;
                    //Lưu vào CSDL
                    db.Giays.InsertOnSubmit(g);
                    db.SubmitChanges();
                }
                return RedirectToAction("QuanLySanPham");
            }
        }

        public ActionResult ChiTietSP(int id)
        {
            if (Session["Admin"] == null)
            {
                return RedirectToAction("LoginAdmin", "Admin");
            }
            Admin ad = (Admin)Session["Admin"];
            PhanQuyen_Admin pq = db.PhanQuyen_Admins.SingleOrDefault(n => n.MaAdmin == ad.MaAdmin);
            if (pq.PQ_Giay == false)
                return RedirectToAction("Index", "Admin");
            Giay sp = db.Giays.SingleOrDefault(n => n.MaGiay == id);
            ViewBag.MaSach = sp.MaGiay;
            if (sp == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            return View(sp);
        }

        public ActionResult XoaSP(int id)
        {
            if (Session["Admin"] == null)
            {
                return RedirectToAction("LoginAdmin", "Admin");
            }
            Admin ad = (Admin)Session["Admin"];
            PhanQuyen_Admin pq = db.PhanQuyen_Admins.SingleOrDefault(n => n.MaAdmin == ad.MaAdmin);
            if (pq.PQ_Giay == false)
                return RedirectToAction("Index", "Admin");
            Giay sp = db.Giays.SingleOrDefault(n => n.MaGiay == id);
            if (sp == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            return View(sp);
        }

        [HttpPost, ActionName("XoaSP")]
        public ActionResult XacNhanXoaSP(int id)
        {
            Giay sp = db.Giays.SingleOrDefault(n => n.MaGiay == id);
            if (sp == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            db.Giays.DeleteOnSubmit(sp);
            db.SubmitChanges();
            return RedirectToAction("QuanLySanPham", "Admin");
        }

        [HttpGet]

        public ActionResult SuaSP(int id)
        {
            if (Session["Admin"] == null)
            {
                return RedirectToAction("LoginAdmin", "Admin");
            }
            Admin ad = (Admin)Session["Admin"];
            PhanQuyen_Admin pq = db.PhanQuyen_Admins.SingleOrDefault(n => n.MaAdmin == ad.MaAdmin);
            if (pq.PQ_Giay == false)
                return RedirectToAction("Index", "Admin");
            Giay g = db.Giays.SingleOrDefault(n => n.MaGiay == id);
            if (g == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            ViewBag.MaDM = new SelectList(db.DanhMucs.OrderBy(n => n.TenDM), "MaDM", "TenDM", g.MaDM); ;
            ViewBag.MaNXB = new SelectList(db.NhaXuatBans.OrderBy(n => n.TenNXB), "MaNXB", "TenNXB", g.MaNXB);
            return View(g);
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult SuaSP(Giay g, HttpPostedFileBase fileUpload)
        {
            //Kiểm tra đường dẫn
            if (ModelState.IsValid)
            {
                if (fileUpload != null)
                {
                    var fileName = Path.GetFileName(fileUpload.FileName);
                    var path = Path.Combine(Server.MapPath("~/Images/Product"), fileName);
                    g.Anhbia = "/Product/" + fileName;
                }
                //Lưu vào CSDL
                var obj = db.Giays.SingleOrDefault(p => p.MaGiay == g.MaGiay);
                obj.MaDM = g.MaDM;
                obj.MaNXB = g.MaNXB;
                obj.TenGiay = g.TenGiay;
                obj.SoLuongBan = g.SoLuongBan;
                obj.SoLuongTon = g.SoLuongTon;
                obj.NgayCapNhat = g.NgayCapNhat;
                obj.Mau = g.Mau;
                obj.Mota = g.Mota;
                obj.Anhbia = g.Anhbia;
                obj.Giaban = g.Giaban;
                obj.Size = g.Size;
                db.SubmitChanges();
                return RedirectToAction("QuanLySanPham");
            }
            //Đưa dữ liệu vào dropdown
            ViewBag.MaDM = new SelectList(db.DanhMucs.OrderBy(n => n.TenDM), "MaDM", "TenDM"); ;
            ViewBag.MaNXB = new SelectList(db.NhaXuatBans.OrderBy(n => n.TenNXB), "MaNXB", "TenNXB");
            return View(g);
        }

        public ActionResult QuanLyNhaSanXuat(int? page)
        {
            if (Session["Admin"] == null)
            {
                return RedirectToAction("LoginAdmin", "Admin");
            }
            Admin ad = (Admin)Session["Admin"];
            PhanQuyen_Admin pq = db.PhanQuyen_Admins.SingleOrDefault(n => n.MaAdmin == ad.MaAdmin);
            if (pq.PQ_NhaSanXuat == false)
                return RedirectToAction("Index", "Admin");
            //Số sản phẩm 1 trang
            int pageSize = 21;
            //Số trang
            int pageNum = (page ?? 1);
            var lst = from s in db.NhaXuatBans select s;
            return View(lst.ToPagedList(pageNum, pageSize));
        }

        public ActionResult ThemMoiNSX()
        {
            if (Session["Admin"] == null)
            {
                return RedirectToAction("LoginAdmin", "Admin");
            }
            //Kiểm tra phân quyền
            Admin ad = (Admin)Session["Admin"];
            PhanQuyen_Admin pq = db.PhanQuyen_Admins.SingleOrDefault(n => n.MaAdmin == ad.MaAdmin);
            if (pq.PQ_NhaSanXuat == false)
                return RedirectToAction("Index", "Admin");
            return View();
        }

        [HttpPost]
        public ActionResult ThemMoiNSX(NhaXuatBan nsx)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    db.NhaXuatBans.InsertOnSubmit(nsx);
                    db.SubmitChanges();
                    return RedirectToAction("QuanLyNhaSanXuat", "Admin");
                }
                else
                {
                    Response.StatusCode = 404;
                }
            }
            catch (Exception ex)
            {
                ViewBag.ThongBao = "Lỗi";
            }
            return View();
        }

        public ActionResult XoaNSX(int id)
        {
            if (Session["Admin"] == null)
            {
                return RedirectToAction("LoginAdmin", "Admin");
            }
            //Kiểm tra phân quyền
            Admin ad = (Admin)Session["Admin"];
            PhanQuyen_Admin pq = db.PhanQuyen_Admins.SingleOrDefault(n => n.MaAdmin == ad.MaAdmin);
            if (pq.PQ_NhaSanXuat == false)
                return RedirectToAction("Index", "Admin");
            var nsx = db.NhaXuatBans.SingleOrDefault(n => n.MaNXB == id);
            return View(nsx);
        }

        [HttpPost, ActionName("XoaNSX")]
        public ActionResult XoaNhaSanXuat(int id)
        {
            var lst = db.Giays.ToList();
            foreach (Giay g in lst)
            {
                if (g.MaNXB == id)
                {
                    db.Giays.DeleteOnSubmit(g);
                }
            }
            NhaXuatBan nsx = db.NhaXuatBans.SingleOrDefault(n => n.MaNXB == id);
            if (nsx == null)
            {
                Response.StatusCode = 404;
                ViewBag.ThongBao = "Lỗi";
                return null;
            }
            db.NhaXuatBans.DeleteOnSubmit(nsx);
            db.SubmitChanges();
            return RedirectToAction("QuanLyNhaSanXuat", "Admin");
        }

        public ActionResult ChiTietNSX(int id)
        {
            if (Session["Admin"] == null)
            {
                return RedirectToAction("LoginAdmin", "Admin");
            }
            //Kiểm tra phân quyền
            Admin ad = (Admin)Session["Admin"];
            PhanQuyen_Admin pq = db.PhanQuyen_Admins.SingleOrDefault(n => n.MaAdmin == ad.MaAdmin);
            if (pq.PQ_NhaSanXuat == false)
                return RedirectToAction("Index", "Admin");
            var model = db.NhaXuatBans.SingleOrDefault(n => n.MaNXB == id);
            return View(model);
        }

        public ActionResult SuaNXS(int id)
        {
            if (Session["Admin"] == null)
            {
                return RedirectToAction("LoginAdmin", "Admin");
            }
            //Kiểm tra phân quyền
            Admin ad = (Admin)Session["Admin"];
            PhanQuyen_Admin pq = db.PhanQuyen_Admins.SingleOrDefault(n => n.MaAdmin == ad.MaAdmin);
            if (pq.PQ_NhaSanXuat == false)
                return RedirectToAction("Index", "Admin");
            var model = db.NhaXuatBans.SingleOrDefault(n => n.MaNXB == id);
            return View(model);//cai nay la httpget
        }

        [HttpPost, ActionName("SuaNXS")]
        [ValidateAntiForgeryToken]
        public ActionResult SuaNXS(NhaXuatBan nsx)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var obj = db.NhaXuatBans.SingleOrDefault(p => p.MaNXB == nsx.MaNXB);
                    obj.TenNXB = nsx.TenNXB;
                    obj.Diachi = nsx.Diachi;
                    obj.DienThoai = nsx.Diachi;
                    db.SubmitChanges();
                    ViewBag.ThongBao = "Thành công";
                }
                else
                {
                    Response.StatusCode = 404;
                }
            }
            catch (Exception ex)
            {
                ViewBag.ThongBao = "Lỗi";
            }
            return View();
        }

        public ActionResult QuanLyCuaHang(int? page)
        {
            if (Session["Admin"] == null)
            {
                return RedirectToAction("LoginAdmin", "Admin");
            }
            //Số sản phẩm 1 trang
            int pageSize = 21;
            //Số trang
            int pageNum = (page ?? 1);
            var lst = from s in db.CuaHangs select s;
            return View(lst.ToPagedList(pageNum, pageSize));
        }

        public ActionResult BieuDoTron()
        {
            int total = 0;
            string abc = "";
            List<DanhMuc> lst = new List<DanhMuc>();
            foreach(DanhMuc dm in db.DanhMucs.ToList())
            {
                foreach(Giay g in db.Giays.ToList())
                {
                    if(g.MaDM == dm.MaDM)
                    {
                        total += (int) g.SoLuongTon;
                    }
                }
                abc += "['"+dm.TenDM+"', "+total+"],";
                total = 0;
            }
            string text = "<script type='text/javascript'>" +
                "google.charts.load('current', { 'packages': ['corechart'] });" +
                "google.charts.setOnLoadCallback(drawChart);" +
                "function drawChart() {" +
                "var data = google.visualization.arrayToDataTable([" +
                "['Task', 'Hours per Day']," + abc +
                "]);" +
                "var options = { 'title': 'My Average Day', 'width': 434, 'height': 320 };" +
                "var chart = new google.visualization.PieChart(document.getElementById('piechart'));" +
                "chart.draw(data, options);" +
                "}" +
                "</script>";

            return Content(text);
        }

        [HttpGet]
        public ActionResult ThemMoiCH()
        {
            if (Session["Admin"] == null)
            {
                return RedirectToAction("LoginAdmin", "Admin");
            }
            return View();
        }

        [HttpPost]
        public ActionResult ThemMoiCH(CuaHang ch)
        {
            try
            {
                if(ModelState.IsValid)
                {
                    var obj = ch;
                    db.CuaHangs.InsertOnSubmit(obj);
                    db.SubmitChanges();
                    return RedirectToAction("QuanLyCuaHang","Admin");
                }
                else
                {
                    Response.StatusCode = 404;
                }
            }catch(Exception ex)
            {
                ViewBag.ThongBao = "Lỗi";
            }                      
            return View();
        }

        public ActionResult Logout()
        {
            return PartialView();
        }

        [HttpPost]
        public ActionResult Logout(FormCollection f)
        {
            ViewBag.ThongBao = "ABC";
            Session["Admin"] = null;
            Response.Redirect(Request.Url.ToString());
            return View("LoginAdmin");
        }

        public ActionResult TopTotal()
        {
            List<TopTotal> lst = new List<TopTotal>();
            int total = 0;
            foreach(DanhMuc dm in db.DanhMucs.ToList())
            {
                foreach(Giay g in db.Giays.ToList())
                {
                    if(g.MaDM == dm.MaDM)
                    {
                        total += (int) (g.Giaban * g.SoLuongBan);
                    }
                }
                TopTotal tt = new TopTotal();
                tt.ten = dm.TenDM;
                tt.tong = total;
                lst.Add(tt);
                total = 0;
            }
            return View(lst.OrderByDescending(n=>n.tong));
        }
    }
}