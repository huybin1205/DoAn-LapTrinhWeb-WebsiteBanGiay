using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WebsiteBanGiay.Controllers
{
    public class ProductController : Controller
    {
        // GET: Product
        public ActionResult SanPham(int id)
        {
            return View();
        }

        public ActionResult ChiTietSanPham(int id)
        {
            return View();
        }
    }
}