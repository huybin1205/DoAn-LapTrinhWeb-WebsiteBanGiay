using Facebook;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebsiteBanGiay.Models;

namespace WebsiteBanGiay.Controllers
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
        dbQuanLyBanGiayDataContext db = new dbQuanLyBanGiayDataContext();
        // GET: User
        [HttpGet]
        public ActionResult Login()
        {
            if(Session["Taikhoan"]!=null)
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
        public ActionResult FacebookCallback(string code)
        {
            var fb = new FacebookClient();
            dynamic result = fb.Post("oauth/access_token", new
            {
                client_id = ConfigurationManager.AppSettings["FbAppId"],
                client_secret = ConfigurationManager.AppSettings["FbAppSecret"],
                redirect_uri = RedirectUri.AbsoluteUri,
                code = code

            });
            var accessToken = result.access_token;
            if (!string.IsNullOrEmpty(accessToken))
            {
                fb.AccessToken = accessToken;
                dynamic me = fb.Get("me?fields=first_name,middle_name,last_name,id,email");
                string email = me.email;
                string username = me.email;
                string firstname = me.first_name;
                string middlename = me.middle_name;
                string lastname = me.last_name;

                var kh = new KhachHang();
                kh.Email = email;
                kh.Taikhoan = username;
                kh.HoTen = firstname + "" + middlename + "" + lastname;

                db.KhachHangs.InsertOnSubmit(kh);
                db.SubmitChanges();
                ViewBag.Thongbao = "Chúc mừng đăng nhập thành công";
                Session["Taikhoan"] = kh;
            }
            
            return Redirect("/");
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
            return View();
        }

        [HttpPost]
        public ActionResult Profile(FormCollection f)
        {
            Session["Taikhoan"] = null;
            return RedirectToAction("Login","User");
        }

    }
}