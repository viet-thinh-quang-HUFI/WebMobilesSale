using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BanDienThoai.Models
{
    public class LSHoaDon
    {
        public string BillID { get; set; }
        public string DateTime { get; set; }
        public string AparmentNo { get; set; }
        public string Street { get; set; }
        public string Ward { get; set; }
        public string District { get; set; }
        public string City { get; set; }
        public long? Total { get; set; }
        public bool? State { get; set; }
        public bool? Payment { get; set; }
    }
}