using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebsiteBanGiay.Models;

namespace WebsiteBanGiay.Controllers
{
    public class UserController : Controller
    {
        dbQuanLyBanGiayDataContext db = new dbQuanLyBanGiayDataContext();
        // GET: User
        [HttpGet]
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Login(FormCollection f)
        {
            var ten = f["txtTenDN"].ToString();
            var matkhau = f["txtMatKhau"].ToString();
            if(String.IsNullOrEmpty(ten))
            {
                ViewData["Loi1"] = "Phải nhập tên đăng nhập";
            }
            if(String.IsNullOrEmpty(matkhau))
            { 
                    ViewData["Loi2"] = "Phải nhập mật khẩu";
                return View();
            }
                KhachHang kh = db.KhachHangs.SingleOrDefault(n => n.Matkhau == matkhau || n.Taikhoan == ten);
                if (kh != null)
                {
                    ViewBag.Thongbao = "Chúc mừng đăng nhập thành công";
                    Session["Taikhoan"] = kh;
                    return RedirectToAction("Index", "Home");
                }
                else
                    ViewBag.Thongbao = "Tên đăng nhập hoặc mật khẩu không đúng";

            return View();
        }

        public ActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Register(KhachHang kh)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    //Thêm khách hàng
                    db.KhachHangs.InsertOnSubmit(kh);
                    //Lưu lên csdl
                    db.SubmitChanges();
                    return RedirectToAction("Login");
                }
            }
            catch (Exception ex)
            {
                ViewBag.Loi = ex.ToString();
            }
            return View();
        }
    }
}