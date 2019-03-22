using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebsiteBanGiay.Models;

namespace WebsiteBanGiay.Models
{
    public class GioHang
    {
        dbQuanLyBanGiayDataContext db = new dbQuanLyBanGiayDataContext();
        public int maGiay { get; set; }
        public string tenGiay { get; set; }
        public string anhBia { get; set; }
        public double donGia { get; set; }
        public int soLuong { get; set; }
        public string size { get; set; }
        public string mau { get; set; }
        public double thanhTien
        {
            get { return donGia* soLuong; }
        }

        public GioHang(int maGiay)
        {
            this.maGiay = maGiay;
            Giay giay = db.Giays.SingleOrDefault(n => n.MaGiay == maGiay);
            tenGiay = giay.TenGiay;
            anhBia = giay.Anhbia;
            donGia = double.Parse(giay.Giaban.ToString());
            size = giay.Size;
            mau = giay.Mau;
            soLuong = 1;
        }
    }
}