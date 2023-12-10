using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Web;

namespace BanDienThoai.Models
{
    public class GioHang
    {
        public List<Item> dsSP;
        public GioHang()
        {
            dsSP = new List<Item>();
        }
        public Item Find(string id)
        {
            return dsSP.Find(t => t.DetailPhoneID == id);
        }
        public void Them(Item x)
        {
            dsSP.Add(x);
        }

        public void Xoa(Item x)
        {
            dsSP.Remove(x);
        }

        public int SLMatHang()
        {
            return dsSP.Count;
        }

        public int TongSL()
        {
            return dsSP.Sum(t => t.Quantity);
        }
        public double? TongTien()
        {
            return dsSP.Sum(t => t.ThanhTien);
        }
    }
}