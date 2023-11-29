using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BanDienThoai.Models
{
    public class CTPhone
    {
        public string PhoneID { get; set; }
        public string PhoneName { get; set; }
        public string MainImage { get; set; }
        public string ScreenTechnology { get; set; }
        public int? PhysicalWidth { get; set; }
        public int? PhysicalHeight { get; set; }
        public decimal? ScreenDiagonal { get; set; }
        public string Chip { get; set; }
        public string OperatingSystem { get; set; }
        public string Sim { get; set; }
        public string Wifi { get; set; }
        public string Bluetooth { get; set; }
        public int? BatteryCapacity { get; set; }
        public string TypeOfPin { get; set; }
        public string BrandID { get; set; }
        public int? pri { get; set; }
    }

}