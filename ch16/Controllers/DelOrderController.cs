using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ch16.Models;

namespace ch16.Controllers
{
    public class DelOrderController : Controller
    {
        ChuHou1120Entities db = new ChuHou1120Entities();
        // GET: DelOrder
        public ActionResult DelIndex()//外送員主頁所有訂單
        {
            if (Session["Member"] == null)
            {
                return RedirectToAction("Login", "Home", "_Layout");
            }
            var del = Session["Del"].ToString();
            var show = db.Order.Where(m => m.duserid == del && m.ostatus != "已送達" ).ToList();//指派給外送員的訂單
            return View("DelIndex", "_LayoutDel", show);
        }

        public ActionResult Detail1(string id)//出發頁面
        {            
            if (Session["Member"] == null)
            {
                return RedirectToAction("Login", "Home", "_Layout");
            }
            Session["id"] = id;
            var ordd = db.Order.Where(m => m.orderguid == id).FirstOrDefault();           
            if(ordd.ostatus == "出發外送")//判斷狀態跳頁
            {
                return RedirectToAction("Detail2");
            }

            var mu = db.DetailMenu.Where(m => m.orderguid == id).ToList();
            List<int> b = new List<int>();
            int a = 0;
            int sum = 0;
            foreach (var item in mu)
            {
                //b.Add(int.Parse(item.mqty.ToString()));
                int c = int.Parse(item.mprice.ToString()) * int.Parse(item.mqty.ToString());
                b.Add(c);
                a = a + 1;
                sum = sum + c;
            }
            ViewBag.price = b;
            ViewBag.sum = sum;

            CVMdemu od = new CVMdemu()
            {
                order = db.Order.Where(m => m.orderguid == id).ToList(),
                menu = db.DetailMenu.Where(m => m.orderguid == id).ToList()
            };
            return View("Detail1", "_LayoutDel", od);
        }
        
        public ActionResult Detail11(string id)//出發外送按鈕
        {
            var od = db.Order.Where(m => m.orderguid == id).FirstOrDefault();
            od.ostatus = "出發外送";            
            db.SaveChanges();
            return RedirectToAction("Detail2");
        }
        public ActionResult Detail2()//出發外送頁面
        {
            if (Session["Member"] == null)
            {
                return RedirectToAction("Login", "Home", "_Layout");
            }
            var id=Session["id"].ToString();

            var mu = db.DetailMenu.Where(m => m.orderguid == id).ToList();
            List<int> b = new List<int>();
            int a = 0;
            int sum = 0;
            foreach (var item in mu)
            {
                //b.Add(int.Parse(item.mqty.ToString()));
                int c = int.Parse(item.mprice.ToString()) * int.Parse(item.mqty.ToString());
                b.Add(c);
                a = a + 1;
                sum = sum + c;
            }
            ViewBag.price = b;
            ViewBag.sum = sum;

            CVMdemu od = new CVMdemu()
            {
                order = db.Order.Where(m => m.orderguid == id).ToList(),
                menu = db.DetailMenu.Where(m => m.orderguid == id).ToList()
            };
            return View("Detail2", "_LayoutDel", od);
        }

        public ActionResult Detail22(string id)//已送達按鈕
        {
            var od = db.Order.Where(m => m.orderguid == id).FirstOrDefault();
            od.isapproved = "2";
            od.ostatus = "已送達";
            od.odate = DateTime.Now;
            db.SaveChanges();
            return RedirectToAction("DelIndex");
        }
        public ActionResult DelAbout()//外送員查看自己資料
        {            
            if (Session["Member"] == null)
            {
                return RedirectToAction("Login", "Home", "_Layout");
            }
            var del = Session["Del"].ToString();
            var show = db.Delivery.Where(m => m.duserid == del).FirstOrDefault();
            return View("DelAbout", "_LayoutDel", show);
        }

        public ActionResult Edit(string id)//外送員修改自己資料
        {
            if (Session["Member"] == null)
            {
                return RedirectToAction("Login", "Home", "_Layout");
            }
            var de = db.Delivery.Where(m => m.duserid == id).FirstOrDefault();
            return View("Edit", "_LayoutDel", de);
        }
        [HttpPost]
        public ActionResult Edit(string id, string dpwd, string dname, string dphone)
        {
            var de = db.Delivery.Where(m => m.duserid == id).FirstOrDefault();
            de.dpwd = dpwd;
            de.dname = dname;
            de.dphone = dphone;
            db.SaveChanges();
            return RedirectToAction("DelAbout");
        }
        public ActionResult DelOrder()//曾經外送的訂單
        {
            if (Session["Member"] == null)
            {
                return RedirectToAction("Login","Home","_Layout");
            }
            var del = Session["Del"].ToString();
            var show = (from s in db.Order
                        where s.duserid == del && s.ostatus == "已送達"
                        orderby s.odate descending
                        select s).ToList();

            return View("DelOrder","_LayoutDel",show);
        }

        public ActionResult DelOrderDetail(string id)//已完成外送訂單明細
        {
            if (Session["Member"] == null)
            {
                return RedirectToAction("Login", "Home", "_Layout");
            }

            var mu = db.DetailMenu.Where(m => m.orderguid == id).ToList();
            List<int> b = new List<int>();
            int a = 0;
            int sum = 0;
            foreach (var item in mu)
            {
                //b.Add(int.Parse(item.mqty.ToString()));
                int c = int.Parse(item.mprice.ToString()) * int.Parse(item.mqty.ToString());
                b.Add(c);
                a = a + 1;
                sum = sum + c;
            }
            ViewBag.price = b;
            ViewBag.sum = sum;

            CVMdemu od = new CVMdemu()
            {
                order = db.Order.Where(m => m.orderguid == id).ToList(),
                menu = db.DetailMenu.Where(m => m.orderguid == id).OrderBy(m => m.mid).ToList()
            };
            
            return View("DelOrderDetail", "_LayoutDel", od);
        }
    }
}