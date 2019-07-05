using Facebook;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebsiteBanDoThoiTrang.Models;

namespace WebsiteBanDoThoiTrang.Controllers
{
    public class UserController : Controller
    {
        private Uri RedirectUri
        {
            get
            {
                var uriBuilder = new UriBuilder(Request.Url);
                uriBuilder.Query = null;
                uriBuilder.Fragment = null;
                uriBuilder.Path = Url.Action("FacebookCallback");
                return uriBuilder.Uri;
            }
        }
        dbQuanLyBanDoThoiTrangDataContext db = new dbQuanLyBanDoThoiTrangDataContext();
        // GET: User
        [HttpGet]
        public ActionResult Login()
        {
            if (Session["Taikhoan"]!=null)
            {
                return RedirectToAction("Profile","User");
            }
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
            KhachHang kh = db.KhachHangs.SingleOrDefault(n => n.Matkhau.CompareTo(matkhau)==0 && n.Taikhoan.CompareTo(ten)==0);
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
        
        public ActionResult LoginFacebook()
        {
            var fb = new FacebookClient();
            var loginUrl = fb.GetLoginUrl(new
            {
                client_id = ConfigurationManager.AppSettings["FbAppId"],
                client_secret = ConfigurationManager.AppSettings["FbAppSecret"],
                redirect_uri = RedirectUri.AbsoluteUri,
                response_type = "code",
                scope = "email",

            });
            return Redirect(loginUrl.AbsoluteUri);
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
        public ActionResult Profile()
        {
            KhachHang kh = (KhachHang) Session["Taikhoan"];
            if (kh == null)
                return View();
            return View(kh);
        }

        [HttpPost]
        public ActionResult Profile(FormCollection f)
        {
            Session["Taikhoan"] = null;
            return RedirectToAction("Login","User");
        }

    }
}