using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebsiteBanDoThoiTrang.Models;

namespace WebsiteBanDoThoiTrang.Models
{
    public class GioHang
    {
        dbQuanLyBanDoThoiTrangDataContext db = new dbQuanLyBanDoThoiTrangDataContext();
        public int maHang { get; set; }
        public string tenHang { get; set; }
        public string anhBia { get; set; }
        public double donGia { get; set; }
        public int soLuong { get; set; }
        public string size { get; set; }
        public string mau { get; set; }
        public double thanhTien
        {
            get { return donGia* soLuong; }
        }

        public GioHang(int maHang)
        {
            this.maHang = maHang;
            MatHang hang = db.MatHangs.SingleOrDefault(n => n.MaHang == maHang);
            tenHang = hang.TenHang;
            anhBia = hang.Anhbia;
            donGia = double.Parse(hang.Giaban.ToString());
            size = hang.Size;
            mau = hang.Mau;
            soLuong = 1;
        }
    }
}