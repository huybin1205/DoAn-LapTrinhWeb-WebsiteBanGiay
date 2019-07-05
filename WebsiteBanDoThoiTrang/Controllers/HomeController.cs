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
    public class HomeController : Controller
    {
        dbQuanLyBanDoThoiTrangDataContext db = new dbQuanLyBanDoThoiTrangDataContext();
        // GET: Home
        public ActionResult Index()
        {
            return View();
        }

        public PartialViewResult SliderPartial()
        {
            var list = from s in db.Banners orderby s.MaBanner descending select s;
            return PartialView(list.Take(4));
        }

        public PartialViewResult QCThuongHieu()
        {
            //var list = db.QuangCaos.Take(3).ToList();
            var list = from s in db.QuangCaos orderby s.MaQC descending select s;
            return PartialView(list.Take(3));
        }

        public PartialViewResult QuangCao()
        {
            return PartialView();
        }

        public PartialViewResult PhuKien()
        {
            //var list = db.DoThoiTrangs.Take(9).Where(n=>n.MaDM==5).ToList();
            var list = from s in db.MatHangs where s.MaDM == 5 select s;
            return PartialView(list.Take(9));
        }

        public PartialViewResult SanPhamMoi()
        {
            var list = from s in db.MatHangs orderby s.MaHang descending select s;
            return PartialView(list.Take(9).ToList());
        }

        public PartialViewResult SPBanChayNhat()
        {
            var list = from s in db.MatHangs orderby s.SoLuongBan descending select s;
            return PartialView(list.Take(9).ToList());
        }

        public PartialViewResult NavigationPartial()
        {
            var list = from s in db.DanhMucs select s;
            return PartialView(list);
        }

        public PartialViewResult HeThongCH()
        {
            var list = from s in db.CuaHangs select s;
            return PartialView(list);
        }

        public PartialViewResult GioHangIcon()
        {
            List<GioHang> list = Session["GioHang"] as List<GioHang>;
            if (list == null)
            {
                list = new List<GioHang>();
                Session["GioHang"] = list;
            }
            return PartialView(list);
        }

        public ActionResult LoadMorePartial(int ? page)
        {
            //Số sản phẩm 1 trang
            int pageSize = 21;
            //Số trang
            int pageNum = (page ?? 1);
            var list = from s in db.MatHangs select s;
            return View(list.ToPagedList(pageNum,pageSize));
        }

        [HttpPost]
        public ActionResult Search(FormCollection f,int ? page)
        {
            string chuoi = f["timkiem"].ToString();
            //Số sản phẩm 1 trang
            int pageSize = 21;
            //Số trang
            int pageNum = (page ?? 1);
            var lst = from s in db.MatHangs where s.TenHang.Contains(chuoi.Trim()) select s;
            return View(lst.ToPagedList(pageNum, pageSize));
        }
        public ActionResult LienHe(int id)
        {
            CuaHang ch = db.CuaHangs.SingleOrDefault(n => n.MaCH == id);
            switch(ch.MaCH)
            {
                case 1:
                    ViewBag.BanDo = "https://www.google.com/maps/embed?pb=!1m18!1m12!1m3!1d3919.120335130718!2d106.71240001405666!3d10.802094592304117!2m3!1f0!2f0!3f0!3m2!1i1024!2i768!4f13.1!3m3!1m2!1s0x317528a459cb43ab%3A0x6c3d29d370b52a7e!2zVHLGsOG7nW5nIMSQ4bqhaSBo4buNYyBIdXRlY2g!5e0!3m2!1svi!2s!4v1554117234885!5m2!1svi!2s";
                    break;
                case 2:
                    ViewBag.BanDo = "https://www.google.com/maps/embed?pb=!1m18!1m12!1m3!1d1694.7167536929737!2d107.28410175496623!3d11.039294548374249!2m3!1f0!2f0!3f0!3m2!1i1024!2i768!4f13.1!3m3!1m2!1s0x3174f7de01459107%3A0x9355b1402cbc3999!2zQsOhbmgga2VtIFRodSBI4bqjbw!5e0!3m2!1svi!2s!4v1554119844179!5m2!1svi!2s";
                    break;
                case 3:
                    ViewBag.BanDo = "https://www.google.com/maps/embed?pb=!1m18!1m12!1m3!1d3886.2636038537967!2d109.2993662143054!3d13.082473015976632!2m3!1f0!2f0!3f0!3m2!1i1024!2i768!4f13.1!3m3!1m2!1s0x316fec19f1b07da9%3A0x112f6f62ae09906f!2zVGjDoXAgTmjhuqFuIFBow7ogWcOqbg!5e0!3m2!1svi!2s!4v1554119145668!5m2!1svi!2s";
                    break;
                case 4:
                    ViewBag.BanDo = "https://www.google.com/maps/embed?pb=!1m18!1m12!1m3!1d2475.1142781218255!2d105.08431714673522!3d10.016948498900248!2m3!1f0!2f0!3f0!3m2!1i1024!2i768!4f13.1!3m3!1m2!1s0x31a0b399875b2c4b%3A0x72dd70267a4b7aa7!2sKien+Giang+Water+Supply+%26+Drainage+Co.!5e0!3m2!1svi!2s!4v1554119560724!5m2!1svi!2s";
                    break;
                case 5:
                    ViewBag.BanDo = "https://www.google.com/maps/embed?pb=!1m18!1m12!1m3!1d958.3400031873244!2d105.8407817548716!3d21.023415439190313!2m3!1f0!2f0!3f0!3m2!1i1024!2i768!4f13.1!3m3!1m2!1s0x3135ab90949fd5f7%3A0x25802ec5e94a3668!2sT%C3%B2a+nh%C3%A0+Capital+Tower!5e0!3m2!1svi!2s!4v1554119612218!5m2!1svi!2s";
                    break;
                case 6:
                    ViewBag.BanDo = "https://www.google.com/maps/embed?pb=!1m18!1m12!1m3!1d338.9123994889018!2d108.22223380885225!3d16.04700494279888!2m3!1f0!2f0!3f0!3m2!1i1024!2i768!4f13.1!3m3!1m2!1s0x314219c387f619e1%3A0xf5aadb68a31eeea3!2zQ8O0bmcgVHkgVE5ISCBNaW5oIMSQw7RuZw!5e0!3m2!1svi!2s!4v1554119672187!5m2!1svi!2s";
                    break;



            }
            return View(ch);
        }
    }
}