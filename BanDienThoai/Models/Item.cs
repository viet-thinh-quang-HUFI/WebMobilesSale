using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BanDienThoai.Models
{
    public class Item
    {
        DataClasses1DataContext database = new DataClasses1DataContext();
        public string DetailPhoneID { get; set; }
        public string PhoneID { get; set; }
        public string PhoneName { get; set; }
        public string CapacityID { get; set; }
        public Int16 Capacity { get; set; }
        public string Unit { get; set; }
        public string ColorID { get; set; }
        public string ColorName { get; set; }
        public string Img { get; set; }
        public int? Pri { get; set; }
        public int Quantity { get; set; }

        public Item(string phoneid, string capacityid, string colorid)
        {
            PhoneID = phoneid;
            PHONE t = database.PHONEs.FirstOrDefault(i => i.PhoneID == phoneid);
            PhoneName = t.PhoneName;
            CAPACITY m = database.CAPACITies.FirstOrDefault(i => i.CapacityID == capacityid);
            CapacityID = capacityid;
            Capacity = Int16.Parse(m.Capacity1.ToString());
            Unit = m.Unit;
            COLOR c = database.COLORs.FirstOrDefault(i => i.ColorID == colorid);
            ColorID = colorid;
            ColorName = c.ColorName;
            DETAILSPHONE d = database.DETAILSPHONEs.FirstOrDefault(i => i.PhoneID == phoneid && i.CapacityID == capacityid && i.ColorID == colorid);
            DetailPhoneID = d.DetailsPhoneID;
            Img = d.DetailImage;
            Pri = d.Price;
            Quantity = 1;
        }
        public double? ThanhTien
        {
            get { return Quantity * Pri; }
        }
    }
}