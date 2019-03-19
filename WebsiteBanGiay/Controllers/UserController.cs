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
        DatabaseDataContext db = new DatabaseDataContext();
        // GET: User
        public ActionResult Login()
        {
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
                    RedirectToAction("Login");
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