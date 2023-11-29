using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BanDienThoai.Models
{
    public class CTHoaDon
    {
        public string DetailImage { get; set; }
        public string PhoneName { get; set; }
        public int? Price { get; set; }
        public int QuantityPurchased { get; set; }
        public string ColorName { get; set; }
        public int Capacity { get; set; }
        public string Unit { get; set; }
    }
}