using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ch16.Models;

namespace ch16.Controllers
{
    public class DeliveryController : Controller
    {
        ChuHou1120Entities db = new ChuHou1120Entities();
        // GET: Delivery
        public ActionResult DemoDel()//列出外送員名單
        {            
            if (Session["Member"] == null)
            {
                return RedirectToAction("Index", "Home","_Layout");
            }
            var user = Session["Store"].ToString();
            var sto = db.StoreMember.Where(m => m.suserid == user ).FirstOrDefault();//找店
            if (sto.sdelivery == "1")
            {
                ViewBag.delopen = "開放中";
                ViewBag.openchoose = "關閉外送";
            }else if(sto.sdelivery == "2")
            {
                ViewBag.delopen = "關閉中";
                ViewBag.openchoose = "開放外送";
            }
            
            var del = db.Delivery.Where(m => m.suserid == user && m.historydel == "0").ToList();//找店的外送員
            return View("DemoDel","_LayoutMember",del);
        }
        [HttpPost]
        public ActionResult openchoose()
        {
            var user = Session["Store"].ToString();
            var sto = db.StoreMember.Where(m => m.suserid == user).FirstOrDefault();
            if (sto.sdelivery == "1")
            {
                sto.sdelivery = "2";
                db.SaveChanges();
                return RedirectToAction("DemoDel");
            }            
            sto.sdelivery = "1";
            db.SaveChanges();
            return RedirectToAction("DemoDel");
        }
        public ActionResult CreateDel()//新增外送員
        {
            if (Session["Member"] == null)
            {
                return RedirectToAction("Index", "Home", "_Layout");
            }
            return View("CreateDel", "_LayoutMember");
        }
        [HttpPost]
        public ActionResult CreateDel(Delivery NewUser,string duserid, string dpwd, string dname, string dphone,string suserid)
        {
            if (ModelState.IsValid == false)
            {
                return View("CreateDel");
            }
            var user = Session["Store"].ToString();
            var del = (from s in db.Delivery where s.duserid == NewUser.duserid  select s).FirstOrDefault();//新增的帳號
            Session["Del"] = NewUser.duserid;//存剛剛的帳號
            var d = Session["Del"].ToString();
            if(del == null)//帳號重複 
            {
                db.Delivery.Add(NewUser);
                db.SaveChanges();
                var dd = db.Delivery.Where(m => m.duserid == d).FirstOrDefault();//找剛剛的帳號資料
                dd.suserid = Session["Store"].ToString();
                dd.historydel = "0";
                db.SaveChanges();
                return RedirectToAction("DemoDel");
            }
            ViewBag.Del = "帳號重複";
            return View("CreateDel");
        }
        public ActionResult EditDel(int id)//修改外送員
        {
            if (Session["Member"] == null)
            {
                return RedirectToAction("Login", "Home", "_Layout");
            }
            var de = db.Delivery.Where(m => m.did == id).FirstOrDefault();
            return View("EditDel", de);
        }
        [HttpPost]
        public ActionResult EditDel(int id, string dname, string dphone)
        {
            var de = db.Delivery.Where(m => m.did == id).FirstOrDefault();            
            de.dname = dname;
            de.dphone = dphone;
            db.SaveChanges();
            return RedirectToAction("DemoDel");
        }
        public ActionResult DeleteDel(int id)//刪除外送員
        {
            var de = db.Delivery.Where(m => m.did == id).FirstOrDefault();
            de.historydel="1";
            db.SaveChanges();
            return RedirectToAction("DemoDel");
        }      
       
    }
}