using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebsiteBanGiay.Models;
using PagedList;
using PagedList.Mvc;
using System.IO;

namespace WebsiteBanGiay.Controllers
{
    public class AdminController : Controller
    {
        dbQuanLyBanGiayDataContext db = new dbQuanLyBanGiayDataContext();
        // GET: Admin
        public ActionResult LoginAdmin()
        {
            return View();
        }

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult QuanLySanPham(int? page)
        {
            //Số sản phẩm 1 trang
            int pageSize = 21;
            //Số trang
            int pageNum = (page ?? 1);
            var lst = from s in db.Giays select s;
            return View(lst.ToPagedList(pageNum, pageSize));
        }

        public ActionResult ThemMoiSP()
        {
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
            //Số sản phẩm 1 trang
            int pageSize = 21;
            //Số trang
            int pageNum = (page ?? 1);
            var lst = from s in db.NhaXuatBans select s;
            return View(lst.ToPagedList(pageNum, pageSize));
        }

        public ActionResult ThemMoiNSX()
        {
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
            var model = db.NhaXuatBans.SingleOrDefault(n => n.MaNXB == id);
            return View(model);
        }

        public ActionResult SuaNXS(int id)
        {
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
            //Số sản phẩm 1 trang
            int pageSize = 21;
            //Số trang
            int pageNum = (page ?? 1);
            var lst = from s in db.CuaHangs select s;
            return View(lst.ToPagedList(pageNum, pageSize));
        }
    }
}