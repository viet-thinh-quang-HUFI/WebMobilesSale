using BanDienThoai.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Linq;
using System.Linq;
using System.Text;
using System.Timers;
using System.Web;
using System.Web.Mvc;

namespace BanDienThoai.Controllers
{
    public class UserController : Controller
    {
        //
        // GET: /User/
        DataClasses1DataContext database = new DataClasses1DataContext();
        public ActionResult Index(string name = "")
        {
            List<HTSP> a;
            if (name == "")
            {
                a = (from phone in database.PHONEs
                     join detailsPhone in database.DETAILSPHONEs on phone.PhoneID equals detailsPhone.PhoneID
                     where detailsPhone.Price != 0
                     group new { phone, detailsPhone } by new { phone.PhoneID, phone.PhoneName, phone.MainImage } into grp
                     select new HTSP
                     {
                         Id = grp.Key.PhoneID,
                         Name = grp.Key.PhoneName,
                         Img = grp.Key.MainImage,
                         Pri = grp.Min(p => p.detailsPhone.Price)
                     }).ToList();
            }
            else
            {
                a = (from phone in database.PHONEs
                     join detailsPhone in database.DETAILSPHONEs on phone.PhoneID equals detailsPhone.PhoneID
                     where detailsPhone.Price != 0 && phone.PhoneName.Contains(name.TrimStart())
                     group new { phone, detailsPhone } by new { phone.PhoneID, phone.PhoneName, phone.MainImage } into grp
                     select new HTSP
                  {
                      Id = grp.Key.PhoneID,
                      Name = grp.Key.PhoneName,
                      Img = grp.Key.MainImage,
                      Pri = grp.Min(p => p.detailsPhone.Price)
                  }).ToList();
            }
            //List<PHONE> a = database.PHONEs.Where(t => t.PhoneName.Contains(name)).ToList();
            return View(a);
        }
        public ActionResult dsHang()
        {
            List<BRAND> a = database.BRANDs.ToList();
            return PartialView(a);
        }
        public ActionResult Hang(string maHang)
        {
            List<HTSP> a;
            a = (from phone in database.PHONEs
                 join detailsPhone in database.DETAILSPHONEs on phone.PhoneID equals detailsPhone.PhoneID
                 where detailsPhone.Price != 0 && phone.BrandID == maHang
                 group new { phone, detailsPhone } by new { phone.PhoneID, phone.PhoneName, phone.MainImage } into grp
                 select new HTSP
                 {
                     Id = grp.Key.PhoneID,
                     Name = grp.Key.PhoneName,
                     Img = grp.Key.MainImage,
                     Pri = grp.Min(p => p.detailsPhone.Price)
                 }).ToList();
            if(a.Count == 0)
            {
                a = null;
            }    
            return View("Index", a);
        }
        public ActionResult CTPhone(string maSP)
        {
            CTPhone ct = (from phone in database.PHONEs
                          join detailsPhone in database.DETAILSPHONEs on phone.PhoneID equals detailsPhone.PhoneID
                          where detailsPhone.Price != 0 && phone.PhoneID == maSP
                          group new { phone, detailsPhone } by new
                          {
                              phone.PhoneID,
                              phone.PhoneName,
                              phone.MainImage,
                              phone.ScreenTeachnology,
                              phone.PhysicalWidth,
                              phone.PhysicalHeight,
                              phone.ScreenDiagonal,
                              phone.Chip,
                              phone.OperatingSystem,
                              phone.Sim,
                              phone.Wifi,
                              phone.Bluetooth,
                              phone.BatteryCapacity,
                              phone.TypeOfPin,
                              phone.BrandID
                          } into grp
                          select new CTPhone
                          {
                              PhoneID = grp.Key.PhoneID,
                              PhoneName = grp.Key.PhoneName,
                              MainImage = grp.Key.MainImage,
                              ScreenTechnology = grp.Key.ScreenTeachnology,
                              PhysicalWidth = grp.Key.PhysicalWidth,
                              PhysicalHeight = grp.Key.PhysicalHeight,
                              ScreenDiagonal = grp.Key.ScreenDiagonal,
                              Chip = grp.Key.Chip,
                              OperatingSystem = grp.Key.OperatingSystem,
                              Sim = grp.Key.Sim,
                              Wifi = grp.Key.Wifi,
                              Bluetooth = grp.Key.Bluetooth,
                              BatteryCapacity = grp.Key.BatteryCapacity,
                              TypeOfPin = grp.Key.TypeOfPin,
                              BrandID = grp.Key.BrandID,
                              pri = grp.Min(p => p.detailsPhone.Price)
                          }).FirstOrDefault();
            List<DungLuong> dl = (from detail in database.DETAILSPHONEs
                                  join capacity in database.CAPACITies on detail.CapacityID equals capacity.CapacityID
                                  where detail.PhoneID == maSP
                                  group capacity by new { detail.PhoneID, capacity.CapacityID, capacity.Capacity1, capacity.Unit } into grp
                                  select new DungLuong
                                  {
                                      PhoneID = grp.Key.PhoneID,
                                      CapacityID = grp.Key.CapacityID,
                                      Capacity = grp.Key.Capacity1,
                                      Unit = grp.Key.Unit,
                                  }).ToList();

            var comments = database.COMMENTs
                    .Where(c => c.PhoneID == maSP)
                    .ToList();
            ViewBag.Comment = comments;
            ViewBag.Phone = ct;
            ViewBag.ListDL = dl;
            return View();
        }

        public ActionResult LoadColors(string maDL, string maSP)
        {
            var colors = GetColorsFromDatabase(maDL, maSP);
            var jsonResult = new JsonResult
            {
                Data = colors,
                JsonRequestBehavior = JsonRequestBehavior.AllowGet
            };

            return jsonResult;
        }

        private List<MauSac> GetColorsFromDatabase(string maDL, string maSP)
        {
            List<MauSac> colors = new List<MauSac>();

            var query = from d in database.DETAILSPHONEs
                        join c in database.COLORs on d.ColorID equals c.ColorID
                        where d.PhoneID == maSP && d.CapacityID == maDL && d.Price != 0
                        select new MauSac
                        {
                            ColorID = c.ColorID,
                            ColorName = c.ColorName
                        };

            colors = query.ToList();

            return colors;
        }

        public ActionResult LoadPri(string maDL, string maSP, string maMau)
        {
            var gia = GetPri(maDL, maSP, maMau);
            var jsonResult = new JsonResult
            {
                Data = gia,
                JsonRequestBehavior = JsonRequestBehavior.AllowGet
            };

            return jsonResult;
        }

        private Gia GetPri(string maDL, string maSP, string maMau)
        {
            Gia gia = new Gia();

            var query = from d in database.DETAILSPHONEs
                        where d.PhoneID == maSP && d.CapacityID == maDL && d.ColorID == maMau
                        select new Gia
                              {
                                  Pri = d.Price,
                                  Img = d.DetailImage
                              };
            gia = query.FirstOrDefault();
            return gia;
        }
        [HttpPost]
        public ActionResult AddComment(FormCollection col)
        {
            COMMENT c = new COMMENT();
            c.Body = col["Body"];
            c.Name = col["Name"];
            c.PhoneNO = col["Phone"];
            c.PhoneID = col["phoneid"];
            DateTime dateTime = DateTime.Now;
            long timestamp = (long)(dateTime - new DateTime(1970, 1, 1)).TotalSeconds;
            c.Datetimestamp = timestamp.ToString();
            Guid commentGuid = Guid.NewGuid();
            c.CommentID = commentGuid.ToString().Substring(0, 10);
            database.COMMENTs.InsertOnSubmit(c);
            database.SubmitChanges();
            return RedirectToAction("Index");
        }
        public ActionResult ThongTin()
        {
            string mail = Session["mail"].ToString();
            var customer = database.CUSTOMERs.FirstOrDefault(c => c.Email == mail);

            var addresses = database.ADDRESSOFCUSTOMERs
                    .Where(a => a.Email == mail)
                    .ToList();

            ViewBag.Cus = customer;
            ViewBag.Addres = addresses;
            return View();
        }
        [HttpGet]
        public ActionResult DangNhap()
        {
            return View();
        }
        [HttpPost]
        public ActionResult DangNhap(FormCollection col)
        {

            CUSTOMER kh = database.CUSTOMERs.FirstOrDefault
                (k => k.Email == col["txtMail"] && k.Password == col["txtPW"]);
            if (kh != null)
            {
                Session["kh"] = kh;
                Session["tenTK"] = kh.CustomerName;
                Session["mail"] = kh.Email;
                return RedirectToAction("Index");
            }


            return View();
        }
        public ActionResult DangXuat()
        {
            Session["gh"] = null;
            Session["tenTK"] = null;
            return RedirectToAction("Index");
        }

        [HttpGet]
        public ActionResult DangKy()
        {
            return View();
        }
        [HttpPost]
        public ActionResult DangKy(FormCollection col)
        {
            string email = col["Email"];

            if (CustomerExists(email))
            {
                ModelState.AddModelError("Email", "Email này đã được sử dụng.");
                return View("DangKy");
            }
            else
            {
                CUSTOMER kh = new CUSTOMER();
                kh.Email = col["Email"];
                kh.CustomerName = col["CustomerName"];
                kh.PhoneNO = col["PhoneNo"];
                kh.Password = col["Password"];
                database.CUSTOMERs.InsertOnSubmit(kh);
                database.SubmitChanges();
                return RedirectToAction("DangNhap");
            }
        }

        private bool CustomerExists(string email)
        {
            return database.CUSTOMERs.Any(c => c.Email == email);
        }


        [HttpPost]
        public ActionResult XL_ChonMua(FormCollection fc1)
        {
                GioHang g = LayGioHang();
                Item i = g.dsSP.FirstOrDefault(t => t.PhoneID == fc1["phoneid"].ToString() && t.CapacityID == fc1["capacityid"].ToString() && t.ColorID == fc1["colorid"].ToString());

                if (i == null)
                {
                    Item x = new Item(fc1["phoneid"].ToString(), fc1["capacityid"].ToString(), fc1["colorid"].ToString());
                    g.Them(x);
                }
                else
                {
                    i.Quantity += 1;
                }
                LuuGioHang(g);
                return RedirectToAction("Index");
        }

        //===========Giỏ hàng==================
        public GioHang LayGioHang()
        {
            GioHang gio = (GioHang)Session["gh"];
            if (gio == null)
            {
                gio = new GioHang();
            }
            return gio;
        }
        public void LuuGioHang(GioHang gio)
        {
            Session["gh"] = gio;
        }
        public ActionResult Xoa(string id)
        {
            GioHang g = LayGioHang();
            Item i = g.dsSP.FirstOrDefault(t => t.DetailPhoneID == id);
            g.Xoa(i);
            LuuGioHang(g);
            return RedirectToAction("XemGioHang");
        }
        public ActionResult XoaAll()
        {
            GioHang g = LayGioHang();
            g.dsSP.Clear();
            LuuGioHang(g);
            return RedirectToAction("XemGioHang");
        }

        public ActionResult XemGioHang()
        {
            GioHang g = LayGioHang();
            List<Item> dsTour = g.dsSP;
            return View(dsTour);
        }


        public ActionResult ThanhToan()
        {
            if (Session["tenTK"] != null)
            {
                string mail = Session["mail"].ToString();
                var addresses = database.ADDRESSOFCUSTOMERs
                        .Where(a => a.Email == mail)
                        .ToList();
                if(addresses.Count > 0)
                {
                    ViewBag.Addres = addresses;
                }
                else
                {
                    ViewBag.Addres = null;
                }
                ViewBag.KH = Session["kh"];
                return View();
            }
            else { return RedirectToAction("DangNhap"); }
        }
        [HttpPost]
        public ActionResult ThanhToan(FormCollection col)
        {
            GioHang g = LayGioHang();
            List<Item> ds = g.dsSP;

            Guid commentGuid = Guid.NewGuid();
            string billID = commentGuid.ToString().Substring(0, 10);

            string ad = col["addressDropdown"].ToString();
            string tt = col["paymentMethod"].ToString();
            bool httt = false;
            if(tt == "0")
            {
                httt = true;
            }

            for (int i = 0; i < ds.Count; i++)
            {
                InsertDetailsBill(billID, ds[i].DetailPhoneID, ds[i].Quantity, ad, httt);
            }
            Session["gh"] = null;
            return RedirectToAction("Index");
        }
        private string connectionString = "Data Source=DESKTOP-U2I3TCB\\ASUS;Initial Catalog=SalesPhoneManagement;Integrated Security=True";
        public void InsertDetailsBill(string billId, string detailsPhoneId, int quantityPurchased, string addressCustomerId, bool paymentMethod)
        {
            using (DataContext context = new DataContext(connectionString))
            {
                context.ExecuteCommand("EXEC [dbo].[PROC_INSERT_DETAILS_BILL] {0}, {1}, {2}, {3}, {4}",
                    billId, detailsPhoneId, quantityPurchased, addressCustomerId, paymentMethod ? 0 : 1);
            }
        }
    
        [HttpGet]
        public ActionResult ThemDiaChi()
        {
            return View();
        }
        [HttpPost]
        public ActionResult ThemDiaChi(FormCollection col)
        {
            ADDRESSOFCUSTOMER ad = new ADDRESSOFCUSTOMER();

            Guid commentGuid = Guid.NewGuid();

            ad.AddressOfCustomerID = commentGuid.ToString().Substring(0, 10);
            ad.AparmentNo = col["AparmentNo"].ToString();
            ad.Street = col["Street"].ToString();
            ad.Ward = col["Ward"].ToString();
            ad.District = col["District"].ToString(); 
            ad.City = col["City"].ToString();
            ad.Email = Session["mail"].ToString();

            database.ADDRESSOFCUSTOMERs.InsertOnSubmit(ad);
            database.SubmitChanges();
            return RedirectToAction("ThongTin"); 
        }
        public ActionResult LSMuaHang()
        {
            string mail = Session["mail"].ToString();
            List<LSHoaDon> query = (from ad in database.ADDRESSOFCUSTOMERs
                        join bill in database.BILLs on ad.AddressOfCustomerID equals bill.AddressOfCustomerID
                        where ad.Email == mail
                        select new LSHoaDon
                        {
                            AparmentNo = ad.AparmentNo,
                            Street = ad.Street,
                            Ward = ad.Ward,
                            District = ad.District,
                            City = ad.City,
                            BillID = bill.BillID,
                            DateTime = bill.Datetimestamp,
                            State = bill.State,
                            Payment = bill.Payment,
                            Total = bill.Total
                        }).ToList();
            return View("LSMuaHang", query);
        }
        public ActionResult CTHoaDon(string billID)
        {
            List<CTHoaDon> a = (from detailsBill in database.DETAILSBILLs
                                join detailsPhone in database.DETAILSPHONEs on detailsBill.DetailsPhoneID equals detailsPhone.DetailsPhoneID
                                join phone in database.PHONEs on detailsPhone.PhoneID equals phone.PhoneID
                                join color in database.COLORs on detailsPhone.ColorID equals color.ColorID
                                join cap in database.CAPACITies on detailsPhone.CapacityID equals cap.CapacityID
                                where detailsBill.BillID == billID
                                select new CTHoaDon
                                {
                                    DetailImage = detailsPhone.DetailImage,
                                    PhoneName = phone.PhoneName,
                                    Price = detailsPhone.Price,
                                    QuantityPurchased = detailsBill.QuantityPurchased,
                                    ColorName = color.ColorName,
                                    Capacity = cap.Capacity1,
                                    Unit = cap.Unit
                                }).ToList();
            return View("CTHoaDon", a);
        }
    }
}
