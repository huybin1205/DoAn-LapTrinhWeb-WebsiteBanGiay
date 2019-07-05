using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebsiteBanDoThoiTrang.Models;
using PagedList;
using PagedList.Mvc;

namespace WebsiteBanDoThoiTrang.Controllers
{
    public class ProductController : Controller
    {
        dbQuanLyBanDoThoiTrangDataContext db = new dbQuanLyBanDoThoiTrangDataContext();
        // GET: Product
        public ActionResult SanPham(int id, int ? page)
        {
            //Số sản phẩm 1 trang
            int pageSize = 21;
            //Số trang
            int pageNum = (page ?? 1);
            var list = from s in db.MatHangs where s.MaDM == id select s;
            DanhMuc dm = db.DanhMucs.SingleOrDefault(m => m.MaDM == id);
            ViewBag.TieuDe = dm.TenDM;
            ViewBag.MaDM = dm.MaDM;
            return View(list.ToPagedList(pageNum, pageSize));
        }

        public ActionResult ChiTietSanPham(int id)
        {
            var DoThoiTrang = from s in db.MatHangs where s.MaHang ==id select s;
            List<DanhMuc> list = db.DanhMucs.ToList();
            List<MatHang> listHang = db.MatHangs.ToList();
            MatHang itemhang = new MatHang();
            foreach(MatHang item in listHang)
            {
                if(item.MaHang == id)
                {
                    itemhang = item;
                }
            }
            foreach(DanhMuc item in list)
            {
                if(item.MaDM == itemhang.MaDM)
                {
                    ViewBag.DanhMuc = item.TenDM;
                    ViewBag.Ma = item.MaDM;
                }
            }
            return View(DoThoiTrang.SingleOrDefault());
        }
    }
}