using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ch16.Models;

namespace ch16.Controllers
{
    public class MenuController : Controller
    {
        ChuHou1120Entities db = new ChuHou1120Entities();        
        public ActionResult Index(string mcid)
        {            
            if (Session["Member"] == null)
            {
                return RedirectToAction("Login", "Home", "_Layout");//判斷是否登入
            }
            var suser = Session["Store"].ToString();//抓店家id
            if (mcid == null)//所有餐點
            {
                ViewBag.cate = "全部餐點";
                CVMmenucate mc = new CVMmenucate()
                {
                    menucate = db.MenuCategory.Where(m => m.suserid == suser && m.historycate == "0").ToList(),//顯示全部分類
                    menu = db.Menu.Where(m => m.suserid == suser && m.historymenu == "0").OrderByDescending(m => m.mcid).ToList()//顯示全部餐點
                };
                return View("Index", "_LayoutMember", mc);
            }
            ViewBag.cate = db.MenuCategory.Where(m => m.mcid == mcid).FirstOrDefault().mtype;//更改選取餐點
            CVMmenucate mcc = new CVMmenucate()//選擇分類後的餐點
            {
                menucate = db.MenuCategory.Where(m => m.suserid == suser && m.historycate == "0").ToList(),//顯示全部分類
                menu = db.Menu.Where(m => m.mcid == mcid && m.historymenu == "0").ToList()//顯示選取分類的餐點
            };
            return View("Index", "_LayoutMember", mcc);
        }

        public ActionResult IndexCate()//新增餐點分類
        {
            if (Session["Member"] == null)
            {
                return View("Login", "Home", "_Layout");
            }
            return View("IndexCate", "_LayoutMember");
        }
        [HttpPost]
        public ActionResult IndexCate(string mtype)//新增餐點分類
        {
            var suser = Session["Store"].ToString();            
            MenuCategory cate = new MenuCategory();
            cate.mtype = mtype;
            cate.suserid = suser;
            cate.historycate = "0";
            db.MenuCategory.Add(cate);
            db.SaveChanges();
            return RedirectToAction("Index");
        }
        public ActionResult CreateMenu()//新增餐點
        {
            if (Session["Member"] == null)
            {
                return RedirectToAction("Login", "Home", "_Layout");
            }
            return View("CreateMenu", "_LayoutMember");
        }
        [HttpPost]
        public ActionResult CreateMenu(string mname, int mprice, string mcid)//新增餐點
        {
            var suser = Session["Store"].ToString();
            Menu me = new Menu();
            me.mname = mname;
            me.mprice = mprice;
            me.suserid = suser;
            me.mcid = mcid;
            me.historymenu = "0";
            db.Menu.Add(me);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        public ActionResult DeleteMenu(string id)//刪除餐點
        {
            var me = db.Menu.Where(m => m.mid == id).FirstOrDefault();
            me.historymenu = "1";
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        public ActionResult DeleteCate(string mcid)//刪除餐點分類
        {
            var ca = db.MenuCategory.Where(m => m.mcid == mcid).FirstOrDefault();//找到是哪個分類
            var me = db.Menu.Where(m => m.mcid == mcid).ToList();//找到那個分類所有的餐點
            foreach (var item in me)
            {
                item.historymenu = "1";                
                db.SaveChanges();
            }
            ca.historycate = "1";
            db.SaveChanges();
            return RedirectToAction("Index");
        }
        
        public ActionResult EditMenu(string id)
        {
            if (Session["Member"] == null)
            {
                return RedirectToAction("Login", "Home", "_Layout");
            }
            var me = db.Menu.Where(m => m.mid == id).FirstOrDefault();
            return View("EditMenu", "_LayoutMember", me);
        }
        [HttpPost]
        public ActionResult EditMenu(string id,int mprice)
        {
            var me = db.Menu.Where(m => m.mid == id).FirstOrDefault();
            me.mprice = mprice;
            db.SaveChanges();
            return RedirectToAction("Index", "Menu");
        }
    }
}