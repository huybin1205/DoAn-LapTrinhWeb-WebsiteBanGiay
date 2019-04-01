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
            if(fileUpload==null)
            {
                ViewBag.Thongbao = "Vui lòng chọn ảnh";
                return View();
            }
            else
            {
                if(ModelState.IsValid)
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
                    g.Anhbia = "/Product/"+fileName;
                    //Lưu vào CSDL
                    db.Giays.InsertOnSubmit(g);
                    db.SubmitChanges();
                }
                return RedirectToAction("QuanLySanPham");
            }
        }
    }
}