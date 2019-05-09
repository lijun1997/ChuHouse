using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ch16.Models;

namespace ch16.Controllers
{
    public class RegisterController : Controller
    {
        ChuHou1120Entities db = new ChuHou1120Entities();
        // GET: Register
        public ActionResult Register()
        {
            if(Session["Member"] == null)
            {
                return View("Register", "_Layout");
            }
            return View("Register", "_Layout");
        }
        [HttpPost]
        public ActionResult Register(StoreMember NewUser)
        {
            if (ModelState.IsValid == false)//沒通過驗證
            {
                return View();
            }
            var member = (from s in db.StoreMember where s.suserid == NewUser.suserid select s).FirstOrDefault();//判斷是否重複帳號
            if (member == null)
            {
                db.StoreMember.Add(NewUser);
                db.SaveChanges();
                var sto = db.StoreMember.Where(m => m.suserid == NewUser.suserid).FirstOrDefault();
                sto.sstatus = "0";
                sto.sreport = "0";
                sto.sdelivery = "0";
                sto.sopen = "0";
                db.SaveChanges();
                Session["reg"] = NewUser.suserid;//紀錄註冊的帳號
                //Session["Welcome"]=NewUser.sname + "歡迎光臨~";
                return RedirectToAction("SecondStep");
            }
            ViewBag.Mes = "此帳號已有人使用!";
            return View();
        }

        public ActionResult SecondStep()//第二步，選擇店家類型
        {
            if (Session["reg"] == null)
            {
                return RedirectToAction("Register");
            }
            var user = Session["reg"].ToString();//剛剛註冊的帳號
            var cate = db.Category.ToList();//列出所有分類
            return View("SecondStep", "_Layout", cate);
        }
        [HttpPost]
        public ActionResult SecondStep(string cateid,string del)//選擇外送員
        {
            var user = Session["reg"].ToString();//剛剛註冊的帳號            
            var sto = db.StoreMember.Where(m => m.suserid == user).FirstOrDefault();
            sto.sdelivery = del;
            sto.cateid = cateid;
            db.SaveChanges();
            return RedirectToAction("ThirdStep");
        }

        public ActionResult ThirdStep()//上傳照片
        {
            if (Session["reg"] == null)
            {
                return RedirectToAction("Register");
            }
            var user = Session["reg"].ToString();//剛剛註冊的帳號
            var sto = db.StoreMember.Where(m => m.suserid == user).FirstOrDefault();
            
            return View("ThirdStep", "_LayoutEdit", sto);
        }
        [HttpPost]
        public ActionResult FileUploadLogo(HttpPostedFileBase file)//店家圖片
        {
            var user = Session["reg"].ToString();//剛剛註冊的帳號            
            var sto = db.StoreMember.Where(m => m.suserid == user).FirstOrDefault();

            if (file != null)
            {
                string pic = System.IO.Path.GetFileName(file.FileName);
                string path = System.IO.Path.Combine(Server.MapPath("~/images"), pic);
                // file is uploaded
                file.SaveAs(path);
                // save the image path path to the database or you can send image
                // directly to database
                // in-case if you want to store byte[] ie. for DB
                using (MemoryStream ms = new MemoryStream())
                {
                    file.InputStream.CopyTo(ms);
                    byte[] array = ms.GetBuffer();
                }

                sto.sphotoname = pic;
                sto.sphotopath = path;
                db.SaveChanges();
            }
            return RedirectToAction("ThirdStep");
        }

        public ActionResult FileUploadMenu(HttpPostedFileBase file)//店家圖片
        {
            var user = Session["reg"].ToString();//剛剛註冊的帳號            
            var sto = db.StoreMember.Where(m => m.suserid == user).FirstOrDefault();

            if (file != null)
            {
                string pic = System.IO.Path.GetFileName(file.FileName);
                string path = System.IO.Path.Combine(Server.MapPath("~/images"), pic);
                // file is uploaded
                file.SaveAs(path);
                // save the image path path to the database or you can send image
                // directly to database
                // in-case if you want to store byte[] ie. for DB
                using (MemoryStream ms = new MemoryStream())
                {
                    file.InputStream.CopyTo(ms);
                    byte[] array = ms.GetBuffer();
                }

                sto.mephotoname = pic;
                sto.mephotopath = path;
                db.SaveChanges();
            }
            return RedirectToAction("ThirdStep");
        }

        public ActionResult Done()
        {
            if (Session["reg"] == null)
            {
                return RedirectToAction("Register");
            }
            var user = Session["reg"].ToString();//剛剛註冊的帳號
            var sto = db.StoreMember.Where(m => m.suserid == user).FirstOrDefault();
            if (sto.sphotoname != null && sto.mephotoname != null)
            {
                return RedirectToAction("Visitor", "Home");
            }
            return RedirectToAction("ThirdStep");
        }

    }
}