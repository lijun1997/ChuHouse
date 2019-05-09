using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ch16.Models;

namespace ch16.Controllers
{
    public class HomeController : Controller
    {
        ChuHou1120Entities db = new ChuHou1120Entities();
        public ActionResult Visitor()//訪客畫面
        {
            Session.Clear();
            return View();
        }
        public ActionResult editstore()//審核未通過的主頁
        {
            if(Session["Member"] == null)
            {
                return RedirectToAction("Vistior", "_Layout");
            }
            var user = Session["Store"].ToString();
            var sto = db.StoreCategory.Where(m => m.suserid == user).FirstOrDefault();

            return View("editstore", "_LayoutEdit",sto);
        }

        public ActionResult edit(string id)//審核未通過修改頁面
        {           
            if (Session["Member"] == null)
            {
                return RedirectToAction("Login", "_Layout");
            }
            var userid = Session["Store"].ToString();
            var user = db.StoreMember.Where(m => m.suserid == userid).FirstOrDefault();
            return View("edit", "_LayoutEdit", user);
        }
        [HttpPost]
        public ActionResult edit(string spwd, string sname, string sphone, string semail, string saddress, string shour, string sinf)
        {
            var suser = Session["Store"].ToString();
            var userid = db.StoreMember.Where(m => m.suserid == suser).FirstOrDefault();
            userid.spwd = spwd;
            userid.sname = sname;
            userid.sphone = sphone;
            userid.semail = semail;
            userid.saddress = saddress;
            userid.shour = shour;
            userid.sinf = sinf;
            db.SaveChanges();
            return RedirectToAction("editstore");
        }

        public ActionResult editpho(string id)//審核未通過修改圖片
        {
            if (Session["Member"] == null)
            {
                return RedirectToAction("Login", "_Layout");
            }
            var pic = db.StoreMember.Where(m => m.suserid == id).FirstOrDefault();
            return View("editpho", "_LayoutEdit", pic);
        }
        [HttpPost]
        public ActionResult FileUpload2(HttpPostedFileBase file)
        {
            
            var user = Session["Store"].ToString();
            var userid = db.StoreMember.Where(m => m.suserid == user).FirstOrDefault();

            if (file != null)
            {
                string pic = System.IO.Path.GetFileName(file.FileName);//原圖檔名
                //string[] pic_part = pic.Split('.');
                //string time = DateTime.Now.ToString("yyyyMMdd_HHmmssms");
                //string photoname = time + "." + pic_part[1];  //更改黨名             
                string path = System.IO.Path.Combine(Server.MapPath("~/images"), pic);// file is uploaded

                file.SaveAs(path);
                // save the image path path to the database or you can send image
                // directly to database
                // in-case if you want to store byte[] ie. for DB
                using (MemoryStream ms = new MemoryStream())
                {
                    file.InputStream.CopyTo(ms);
                    byte[] array = ms.GetBuffer();
                }
                userid.sphotoname = pic;
                userid.sphotopath = path;
                db.SaveChanges();
                return RedirectToAction("editstore");
            }
            // after successfully uploading redirect the user
            return View("editpho", "_LayoutEdit");
        }
        public ActionResult Index()//首頁
        {
            if (Session["Member"] == null)//登入狀態
            {                
                return RedirectToAction("Visitor");
            }

            var user = Session["Store"].ToString();
            var od = db.Order.Where(m => m.suserid == user && m.isapproved == 1.ToString()).ToList();
            return View("Index", "_LayoutMember",od);
        }

        public ActionResult Login()//選擇登入身分
        {
            return View();
        }
        public ActionResult LoginSto()//店家登入畫面
        {
            if (Session["Member"] == null)
            {
                return View("LoginSto", "_Layout");
            }
            return View("LoginSto", "_Layout");
        }
        [HttpPost]
        public ActionResult LoginSto(string userid, string pwd)
        {
            var sto = db.StoreMember.Where(m => m.suserid == userid && m.spwd == pwd).FirstOrDefault();
            if (sto != null)//帳密正確
            {
                if (sto.sstatus == "1" && sto.sreport !="3")//申請通過帳號，警告小於三次，登入條件
                {
                    Session["Welcome"] = sto.sname + "歡迎光臨~";
                    Session["Member"] = sto;
                    Session["Store"] = sto.suserid;
                    return RedirectToAction("DemoOrder","Order");
                }else if(sto.sstatus == "2")//審核未通過
                {
                    Session["Welcome"] = sto.sname + "歡迎光臨~";
                    Session["Member"] = sto;
                    Session["Store"] = sto.suserid;
                    return RedirectToAction("editstore");
                }else if(sto.sstatus == "1" && sto.sreport == "3")//警告三次之店家
                {
                    ViewBag.Message = "警告三次，帳號已被封鎖!";
                    return View();
                }
                ViewBag.Message = "帳號尚未開啟";
                return View();
            }            
            ViewBag.Message = "帳號密碼錯誤，登入失敗!";
            return View();
        }

        public ActionResult LoginDel()//外送員登入
        {
            if (Session["Member"] == null)
            {
                return View("LoginDel", "_Layout");
            }
            return View("LoginDel", "_Layout");
        }
        [HttpPost]
        public ActionResult LoginDel(string userid, string pwd)
        {
            var de = db.Delivery.Where(m => m.duserid == userid && m.dpwd == pwd).FirstOrDefault();
            if (de == null)
            {
                ViewBag.Message = "帳號密碼錯誤，登入失敗!";
                return View();
            }
            Session["Welcome"] = de.dname + "歡迎光臨~";
            Session["Member"] = de;
            Session["Del"] = de.duserid;
            return RedirectToAction("DelIndex","DelOrder");
        }
        
        public ActionResult Logout()//登出
        {
            Session.Clear();
            return RedirectToAction("Visitor");
        }
                
        public ActionResult About()//店家資訊
        {            
            if (Session["Store"] == null)
            {
                return RedirectToAction("Login", "_Layout");
            }
            var user = Session["Store"].ToString();
            var sto = (from s in db.StoreCategory where s.suserid == user select s).FirstOrDefault();
            return View("About", "_LayoutMember", sto);
        }
        public ActionResult EditAbout(string id)
        {                       
            if (Session["Member"] == null)
            {
                return RedirectToAction("Login", "_Layout");
            }
            var userid = Session["Store"].ToString();
            var user = db.StoreMember.Where(m => m.suserid == userid).FirstOrDefault(); 
            return View("EditAbout", "_LayoutMember", user);
        }
        [HttpPost]
        public ActionResult EditAbout(string spwd,string sname,string sphone,string semail,string saddress,string shour,string sinf)
        {
            var suser = Session["Store"].ToString();
            var userid = db.StoreMember.Where(m => m.suserid == suser).FirstOrDefault();
            userid.spwd = spwd;
            userid.sname = sname;
            userid.sphone = sphone;
            userid.semail = semail;
            userid.saddress = saddress;
            userid.shour = shour;
            userid.sinf = sinf;
            db.SaveChanges();
            return RedirectToAction("About");
        }
        public ActionResult EditPhoto(string id)
        {
            if (Session["Member"] == null)
            {
                return RedirectToAction("Login", "_Layout");
            }
            var pic = db.StoreMember.Where(m => m.suserid == id).FirstOrDefault();
            return View("EditPhoto", "_LayoutMember",pic);
        }
        [HttpPost]
        public ActionResult FileUpload(HttpPostedFileBase file)
        {
            var user = Session["Store"].ToString();
            var userid = db.StoreMember.Where(m => m.suserid == user).FirstOrDefault();

            if (file != null)
            {
                string pic = System.IO.Path.GetFileName(file.FileName);//原圖檔名
                //string[] pic_part = pic.Split('.');
                //string time = DateTime.Now.ToString("yyyyMMdd_HHmmssms");
                //string photoname = time + "." + pic_part[1];  //更改黨名             
                string path = System.IO.Path.Combine(Server.MapPath("~/images"),pic);// file is uploaded
                
                file.SaveAs(path);
                // save the image path path to the database or you can send image
                // directly to database
                // in-case if you want to store byte[] ie. for DB
                using (MemoryStream ms = new MemoryStream())
                {
                    file.InputStream.CopyTo(ms);
                    byte[] array = ms.GetBuffer();
                }
                userid.sphotoname = pic;
                userid.sphotopath = path;
                db.SaveChanges();
                return RedirectToAction("About");
            }
            // after successfully uploading redirect the user
            return View("EditPhoto");
        }
        public ActionResult open()
        {
            var user = Session["Store"].ToString();
            var op = db.StoreMember.Where(m => m.suserid == user).FirstOrDefault();
            op.sstatus = "2";
            db.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}