using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebsiteBanDoThoiTrang.Models;
using PagedList;
using PagedList.Mvc;
using System.IO;
using System.Web.UI;

namespace WebsiteBanDoThoiTrang.Controllers
{
    public class AdminController : Controller
    {
        dbQuanLyBanDoThoiTrangDataContext db = new dbQuanLyBanDoThoiTrangDataContext();
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
            if (pq.PQ_MatHang == false)
            {
                return Content("<script language='javascript' type='text/javascript'>alert('Bạn không được phép truy cập vào mục này!');</script>");
            }
            //Số sản phẩm 1 trang
            int pageSize = 21;
            //Số trang
            int pageNum = (page ?? 1);
            var lst = from s in db.MatHangs orderby s.MaHang descending select s;
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
            if (pq.PQ_MatHang == false)
                return RedirectToAction("Index", "Admin");
            ViewBag.MaDM = new SelectList(db.DanhMucs.OrderBy(n => n.TenDM), "MaDM", "TenDM"); ;
            ViewBag.MaThuongHieu = new SelectList(db.ThuongHieus.OrderBy(n => n.TenThuongHieu), "MaThuongHieu", "TenThuongHieu");
            return View();
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult ThemMoiSP(MatHang g, HttpPostedFileBase fileUpload)
        {
            //Đưa dữ liệu vào dropdown
            ViewBag.MaDM = new SelectList(db.DanhMucs.OrderBy(n => n.TenDM), "MaDM", "TenDM"); ;
            ViewBag.MaThuongHieu = new SelectList(db.ThuongHieus.OrderBy(n => n.TenThuongHieu), "MaThuongHieu", "TenThuongHieu");
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
                    db.MatHangs.InsertOnSubmit(g);
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
            if (pq.PQ_MatHang == false)
                return RedirectToAction("Index", "Admin");
            MatHang sp = db.MatHangs.SingleOrDefault(n => n.MaHang == id);
            ViewBag.MaSach = sp.MaHang;
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
            if (pq.PQ_MatHang == false)
                return RedirectToAction("Index", "Admin");
            MatHang sp = db.MatHangs.SingleOrDefault(n => n.MaHang == id);
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
            MatHang sp = db.MatHangs.SingleOrDefault(n => n.MaHang == id);
            if (sp == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            db.MatHangs.DeleteOnSubmit(sp);
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
            if (pq.PQ_MatHang == false)
                return RedirectToAction("Index", "Admin");
            MatHang g = db.MatHangs.SingleOrDefault(n => n.MaHang == id);
            if (g == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            ViewBag.MaDM = new SelectList(db.DanhMucs.OrderBy(n => n.TenDM), "MaDM", "TenDM", g.MaDM); ;
            ViewBag.MaThuongHieu = new SelectList(db.ThuongHieus.OrderBy(n => n.TenThuongHieu), "MaThuongHieu", "TenThuongHieu", g.MaThuongHieu);
            return View(g);
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult SuaSP(MatHang g, HttpPostedFileBase fileUpload)
        {
            //Kiểm tra đường dẫn
            if (ModelState.IsValid)
            {
                if (fileUpload != null)
                {
                    var fileName = Path.GetFileName(fileUpload.FileName);
                    var path = Path.Combine(Server.MapPath("~/Images/Product/"), fileName);
                    //Lưu hình
                    fileUpload.SaveAs(path);
                    g.Anhbia = "/Product/" + fileName;
                }
                //Lưu vào CSDL
                var obj = db.MatHangs.SingleOrDefault(p => p.MaHang == g.MaHang);
                obj.MaDM = g.MaDM;
                obj.MaThuongHieu = g.MaThuongHieu;
                obj.TenHang = g.TenHang;
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
            ViewBag.MaNXB = new SelectList(db.ThuongHieus.OrderBy(n => n.TenThuongHieu), "MaThuongHieu", "TenThuongHieu");
            return View(g);
        }

        public ActionResult QuanLyThuongHieu(int? page)
        {
            if (Session["Admin"] == null)
            {
                return RedirectToAction("LoginAdmin", "Admin");
            }
            Admin ad = (Admin)Session["Admin"];
            PhanQuyen_Admin pq = db.PhanQuyen_Admins.SingleOrDefault(n => n.MaAdmin == ad.MaAdmin);
            if (pq.PQ_ThuongHieu == false)
                return RedirectToAction("Index", "Admin");
            //Số sản phẩm 1 trang
            int pageSize = 21;
            //Số trang
            int pageNum = (page ?? 1);
            var lst = from s in db.ThuongHieus select s;
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
            if (pq.PQ_ThuongHieu == false)
                return RedirectToAction("Index", "Admin");
            return View();
        }

        [HttpPost]
        public ActionResult ThemMoiNSX(ThuongHieu nsx)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    db.ThuongHieus.InsertOnSubmit(nsx);
                    db.SubmitChanges();
                    return RedirectToAction("QuanLyThuongHieu", "Admin");
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
            if (pq.PQ_ThuongHieu == false)
                return RedirectToAction("Index", "Admin");
            var thuongHieu = db.ThuongHieus.SingleOrDefault(n => n.MaThuongHieu == id);
            return View(thuongHieu);
        }

        [HttpPost, ActionName("XoaNSX")]
        public ActionResult XoaThuongHieu(int id)
        {
            var lst = db.MatHangs.ToList();
            foreach (MatHang g in lst)
            {
                if (g.MaThuongHieu == id)
                {
                    db.MatHangs.DeleteOnSubmit(g);
                }
            }
            ThuongHieu nsx = db.ThuongHieus.SingleOrDefault(n => n.MaThuongHieu == id);
            if (nsx == null)
            {
                Response.StatusCode = 404;
                ViewBag.ThongBao = "Lỗi";
                return null;
            }
            db.ThuongHieus.DeleteOnSubmit(nsx);
            db.SubmitChanges();
            return RedirectToAction("QuanLyThuongHieu", "Admin");
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
            if (pq.PQ_ThuongHieu == false)
                return RedirectToAction("Index", "Admin");
            var model = db.ThuongHieus.SingleOrDefault(n => n.MaThuongHieu == id);
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
            if (pq.PQ_ThuongHieu == false)
                return RedirectToAction("Index", "Admin");
            var model = db.ThuongHieus.SingleOrDefault(n => n.MaThuongHieu == id);
            return View(model);//cai nay la httpget
        }

        [HttpPost, ActionName("SuaNXS")]
        [ValidateAntiForgeryToken]
        public ActionResult SuaNXS(ThuongHieu nsx)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var obj = db.ThuongHieus.SingleOrDefault(p => p.MaThuongHieu == nsx.MaThuongHieu);
                    obj.TenThuongHieu = nsx.TenThuongHieu;
                    obj.Diachi = nsx.Diachi;
                    obj.DienThoai = nsx.DienThoai;
                    db.SubmitChanges();
                    return RedirectToAction("QuanLyThuongHieu", "Admin");
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
                foreach(MatHang g in db.MatHangs.ToList())
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
                "var options = { 'title': 'THỐNG KÊ HÀNG TỒN KHO', 'width': 434, 'height': 320 };" +
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
            foreach (DanhMuc dm in db.DanhMucs.ToList())
            {
                foreach (MatHang g in db.MatHangs.ToList())
                {
                    if (g.MaDM == dm.MaDM)
                    {
                        total += (int)(g.Giaban * g.SoLuongBan);
                    }
                }
                TopTotal tt = new TopTotal();
                tt.ten = dm.TenDM;
                tt.tong = total;
                lst.Add(tt);
                total = 0;
            }
            return View(lst.OrderByDescending(n => n.tong));
        }

        public PartialViewResult SPHienCo()
        {
            List<MatHang> lst = db.MatHangs.ToList();
            ViewBag.TongSoMatHang = lst.Count;
            return PartialView(lst);
        }

        public PartialViewResult TongDoanhThu()
        {
            int gia= 0;
            List<ChiTietDonHang> lst = db.ChiTietDonHangs.ToList();
            foreach(ChiTietDonHang ct in lst)
            {
                gia += (int)(ct.DonGia * ct.Soluong);
            }
            ViewBag.TongDoanhThu = gia;
            return PartialView(lst);
        }

        public PartialViewResult SoDonHang()
        {
            List<DonDatHang> lst = db.DonDatHangs.ToList();
            ViewBag.SoDonHang = lst.Count;
            return PartialView(lst);
        }

        public PartialViewResult SoKhachHang()
        {
            List<KhachHang> lst = db.KhachHangs.ToList();
            ViewBag.SoKhachHang = lst.Count;
            return PartialView(lst);
        }
    }
}