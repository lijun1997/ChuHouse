using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ch16.Models;

namespace ch16.Controllers
{
    public class OrderController : Controller
    {               
        ChuHou1120Entities db = new ChuHou1120Entities();
        // GET: Order
        public ActionResult HistoryOrder()//歷史訂單
        {
            if (Session["Member"] == null)
            {
                return RedirectToAction("Login", "Home", "_Layout");
            }
            var user = Session["Store"].ToString();
            var od = (from s in db.Order
                      where s.suserid == user && s.isapproved == "2"
                      orderby s.odate descending
                      select s).ToList();
            //var od = db.Order.Where(m => m.suserid == user && m.isapproved == 2.ToString()).ToList();
            return View("HistoryOrder", "_LayoutMember",od);
        }
        public ActionResult HistoryDetail(string id)//歷史訂單明細
        {
            if (Session["Member"] == null)
            {
                return RedirectToAction("Login", "Home", "_Layout");
            }
            //ViewBag.abc = db.DetailMenu.ToList();
            var mu = db.DetailMenu.Where(m => m.orderguid == id).ToList();
            List<int> b = new List<int>();
            int a = 0;
            int sum = 0;
            foreach (var item in mu)
            {               
                int c=int.Parse(item.mprice.ToString()) * int.Parse(item.mqty.ToString());
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
            var qty = db.DetailMenu.Where(m => m.orderguid == id).ToList();
            
            return View("HistoryDetail", "_LayoutMember", od);
        }
        //-----------------------------------
        public ActionResult DemoOrder()//目前訂單
        {
            if (Session["Member"] == null)
            {
                return RedirectToAction("Login", "Home", "_Layout");
            }
            var user = Session["Store"].ToString();
            var od = db.Order.Where(m => m.suserid == user && m.isapproved == 1.ToString()).ToList();
                                   
            return View("DemoOrder", "_LayoutMember", od);
        }        
        //-----------拒絕訂單-----------------
        public ActionResult RefuseOrder(string id)//拒絕訂單
        {            
            if (Session["Member"] == null)
            {
                return RedirectToAction("Login", "Home", "_Layout");
            }
            var od = db.Order.Where(m => m.orderguid == id).FirstOrDefault();
            return View("RefuseOrder","_LayoutMember",od);
        }
        [HttpPost]
        public ActionResult RefuseOrder(string id,string note)
        {
            var od = db.Order.Where(m => m.orderguid == id).FirstOrDefault();
            od.isapproved = "2";
            od.ostatus = "已拒絕";
            od.note = note;
            db.SaveChanges();
            return RedirectToAction("DemoOrder");
        }
        //-----------跳頁判斷-----------------
        public ActionResult Detail1(string id)//自取尚未接受頁面
        {
            if (Session["Member"] == null)
            {
                return RedirectToAction("Login", "Home", "_Layout");
            }
            Session["id"] = id;
            var ord = db.Order.Where(m => m.orderguid == id).FirstOrDefault();//抓訂單資料

            if (ord.ostatus == "尚未接受" && ord.oway == "2.png")//外送1
            {
                return RedirectToAction("DelDetail1");
            }else if (ord.ostatus == "準備中" && ord.oway == "1.png")//自取2
            {
                return RedirectToAction("Detail2");
            }else if (ord.ostatus == "餐點完成" && ord.oway == "1.png")//自取3
            {
                return RedirectToAction("Detail3");
            }else if (ord.ostatus == "準備中" && ord.oway == "2.png")//外送2
            {
                return RedirectToAction("DelDetail2");
            }else if (ord.ostatus == "餐點完成" && ord.oway == "2.png")
            {
                return RedirectToAction("DelDetail3");//外送指派頁面
            }else if (ord.ostatus == "指派外送員" && ord.oway == "2.png")
            {
                return RedirectToAction("DelDetail4");
            }else if (ord.ostatus == "出發外送" && ord.oway == "2.png")
            {
                return RedirectToAction("DelDetail4");
            }

            var mu = db.DetailMenu.Where(m => m.orderguid == id).ToList();
            List<int> b = new List<int>();
            int a = 0;
            int sum = 0;
            foreach (var item in mu)
            {
                int c = int.Parse(item.mprice.ToString()) * int.Parse(item.mqty.ToString());
                b.Add(c);
                a = a + 1;
                sum = sum + c;
            }
            ViewBag.price = b;
            ViewBag.sum = sum;
            
            //var ord = db.Order.Where(m => m.orderguid == id).FirstOrDefault();//抓訂單資料            
            CVMdemu od = new CVMdemu()
            {
                order = db.Order.Where(m => m.orderguid == id).ToList(),//列出訂單資料
                menu = db.DetailMenu.Where(m => m.orderguid == id).ToList()//列出訂單餐點
            }; 
            return View("Detail1", "_LayoutMember", od);
        }

        public ActionResult btn1(string id)//自取接受訂單按鈕
        {
            if (Session["Member"] == null)
            {
                return RedirectToAction("Login", "Home", "_Layout");
            }
            var od = db.Order.Where(m => m.orderguid == id).FirstOrDefault();
            od.ostatus = "準備中";
            db.SaveChanges();
            return RedirectToAction("Detail2");
        }

        public ActionResult Detail2()//自取準備中頁面
        {
            if (Session["Member"] == null)
            {
                return RedirectToAction("Login", "Home", "_Layout");
            }
            var id = Session["id"].ToString();//訂單編號
            var mu = db.DetailMenu.Where(m => m.orderguid == id).ToList();

            List<int> b = new List<int>();
            int a = 0;
            int sum = 0;
            foreach (var item in mu)
            {
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
            return View("Detail2", "_LayoutMember", od);
        }

        public ActionResult btn2(string id)//餐點完成按鈕
        {
            if (Session["Member"] == null)
            {
                return RedirectToAction("Login", "Home", "_Layout");
            }
            var od = db.Order.Where(m => m.orderguid == id).FirstOrDefault();
            od.ostatus = "餐點完成";
            db.SaveChanges();
            return RedirectToAction("Detail3");
        }

        public ActionResult Detail3()//自取餐點完成頁面
        {
            if (Session["Member"] == null)
            {
                return RedirectToAction("Login", "Home", "_Layout");
            }
            var id = Session["id"].ToString();//訂單編號
            var mu = db.DetailMenu.Where(m => m.orderguid == id).ToList();

            List<int> b = new List<int>();
            int a = 0;
            int sum = 0;
            foreach (var item in mu)
            {
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
            return View("Detail3", "_LayoutMember", od);
        }

        public ActionResult btn3(string id)//已取餐按鈕
        {
            if (Session["Member"] == null)
            {
                return RedirectToAction("Login", "Home", "_Layout");
            }
            var od = db.Order.Where(m => m.orderguid == id).FirstOrDefault();
            od.ostatus = "已取餐";
            od.odate = DateTime.Now;
            od.isapproved = "2";
            db.SaveChanges();
            return RedirectToAction("DemoOrder");//返回目前訂單頁面
        }

        //--------------外送判斷--------------
        public ActionResult DelDetail1()//外送1
        {
            if (Session["Member"] == null)
            {
                return RedirectToAction("Login", "Home", "_Layout");
            }
            var id = Session["id"].ToString();
            var mu = db.DetailMenu.Where(m => m.orderguid == id).ToList();

            List<int> b = new List<int>();
            int a = 0;
            int sum = 0;
            foreach (var item in mu)
            {
                int c = int.Parse(item.mprice.ToString()) * int.Parse(item.mqty.ToString());
                b.Add(c);
                a = a + 1;
                sum = sum + c;
            }
            ViewBag.price = b;
            ViewBag.sum = sum;

            CVMdemu od = new CVMdemu()
            {
                order = db.Order.Where(m => m.orderguid == id).ToList(),//列出訂單資料
                menu = db.DetailMenu.Where(m => m.orderguid == id).ToList()//列出訂單餐點
            };
            
            return View("DelDetail1", "_LayoutMember", od);
        }

        public ActionResult dbtn1(string id)//外送接受訂單按鈕
        {
            if (Session["Member"] == null)
            {
                return RedirectToAction("Login", "Home", "_Layout");
            }
            var od = db.Order.Where(m => m.orderguid == id).FirstOrDefault();
            od.ostatus = "準備中";
            db.SaveChanges();
            return RedirectToAction("DelDetail2");
        }

        public ActionResult DelDetail2()//外送2
        {
            if (Session["Member"] == null)
            {
                return RedirectToAction("Login", "Home", "_Layout");
            }
            var id = Session["id"].ToString();
            var mu = db.DetailMenu.Where(m => m.orderguid == id).ToList();

            List<int> b = new List<int>();
            int a = 0;
            int sum = 0;
            foreach (var item in mu)
            {
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
            return View("DelDetail2", "_LayoutMember", od);
        }

        public ActionResult dbtn2(string id)//餐點完成按鈕
        {
            if (Session["Member"] == null)
            {
                return RedirectToAction("Login", "Home", "_Layout");
            }
            var od = db.Order.Where(m => m.orderguid == id).FirstOrDefault();
            od.ostatus = "餐點完成";
            db.SaveChanges();
            return RedirectToAction("DelDetail3");
        }

        public ActionResult DelDetail3()//外送3指派頁面
        {
            if (Session["Member"] == null)
            {
                return RedirectToAction("Login", "Home", "_Layout");
            }
            var sto = Session["Store"].ToString();
            var del = db.Delivery.Where(m => m.suserid == sto && m.historydel == "0").ToList();
            return View("DelDetail3", "_LayoutMember", del);
        }
        [HttpPost]
        public ActionResult selectdel(string duserid)
        {
            var id = Session["id"].ToString();
            var od = db.Order.Where(m => m.orderguid == id).FirstOrDefault();
            od.duserid = duserid;
            od.ostatus = "指派外送員";
            db.SaveChanges();
            return RedirectToAction("DelDetail4");
        }

        public ActionResult DelDetail4()//外送4
        {
            if (Session["Member"] == null)
            {
                return RedirectToAction("Login", "Home", "_Layout");
            }
            var id = Session["id"].ToString();
            var mu = db.DetailMenu.Where(m => m.orderguid == id).ToList();

            List<int> b = new List<int>();
            int a = 0;
            int sum = 0;
            foreach (var item in mu)
            {
                int c = int.Parse(item.mprice.ToString()) * int.Parse(item.mqty.ToString());
                b.Add(c);
                a = a + 1;
                sum = sum + c;
            }
            ViewBag.price = b;
            ViewBag.sum = sum;

            CVMdemu od = new CVMdemu()
            {
                del = db.OrderDel.Where(m => m.orderguid == id).ToList(),
                menu = db.DetailMenu.Where(m => m.orderguid == id).ToList()
            };
            return View("DelDetail4", "_LayoutMember", od);
        }
        //-----------訂單評價-------------
        public ActionResult OrderScore(string id)
        {
            if (Session["Member"] == null)
            {
                return RedirectToAction("Login", "Home", "_Layout");
            }
            var sco = db.Score.Where(m => m.orderguid == id).FirstOrDefault();
            if(sco == null)
            {
                return RedirectToAction("NoScore");
            }
            if (sco.grade == "5")
            {
                ViewBag.count = 5;
            }else if (sco.grade == "4")
            {
                ViewBag.count = 4;
            }
            else if (sco.grade == "3")
            {
                ViewBag.count = 3;
            }else if (sco.grade == "2")
            {
                ViewBag.count = 2;
            }else if (sco.grade == "1")
            {
                ViewBag.count = 1;
            }

            return View("OrderScore", "_LayoutMember",sco);
        }
        //---------------空評價--------------------------
        public ActionResult NoScore()
        {
            if (Session["Member"] == null)
            {
                return RedirectToAction("Login", "Home", "_Layout");
            }            
            return View("NoScore", "_LayoutMember");
        }
        //--------------新增回復------------------------
        public ActionResult CreateReport(string id)
        {
            if(Session["Member"] == null)
            {
                return RedirectToAction("Login", "Home", "_Layout");
            }
            Session["orderid"] = id;
            var od = db.StoreReport.Where(m => m.orderguid == id).FirstOrDefault();
            if (od != null)
            {
                return RedirectToAction("DemoReport");
            }
            return View("CreateReport", "_LayoutMember", od);
        }
        [HttpPost]
        public ActionResult CreateReport(string id,string srcontent)
        {
            //var ord = Session["orderid"].ToString();//訂單編號
            var sto = Session["Store"].ToString();//店家帳號
            var cu = db.Order.Where(m => m.orderguid == id).FirstOrDefault();//找訂單
            StoreReport sreport = new StoreReport();
            sreport.cuserid = cu.cuserid;
            //sreport.srstatus = "1";
            sreport.suserid = sto;
            sreport.orderguid = id;
            sreport.srcontent = srcontent;
            sreport.stime = DateTime.Now;
            db.StoreReport.Add(sreport);
            db.SaveChanges();
            return RedirectToAction("DemoReport");
        }
        public ActionResult DemoReport()
        {
            if (Session["Member"] == null)
            {
                return RedirectToAction("Login", "Home", "_Layout");
            }
            var ord = Session["orderid"].ToString();
            var od = db.StoreReport.Where(m => m.orderguid == ord).FirstOrDefault();
            if (od.srreply == null)
            {
                ViewBag.report = "尚未回覆";
            }else { ViewBag.report = ""; }
            return View("DemoReport", "_LayoutMember", od);
        }
        //------------所有回報-------------
        public ActionResult Report()
        {
            if (Session["Member"] == null)
            {
                return RedirectToAction("Login", "Home", "_Layout");
            }
            var sto = Session["Store"].ToString();//店家帳號
            var re = db.StoreReport.Where(m => m.suserid == sto).ToList();//列出所有回報單
            return View("Report", "_LayoutMember", re); 
        }
        //------------回報單詳情------------
        public ActionResult ReportDetail(string id)
        {
            if (Session["Member"] == null)
            {
                return RedirectToAction("Login", "Home", "_Layout");
            }
            //var id = Session["id"].ToString();
            var mu = db.DetailMenu.Where(m => m.orderguid == id).ToList();

            List<int> b = new List<int>();
            int a = 0;
            int sum = 0;
            foreach (var item in mu)
            {
                int c = int.Parse(item.mprice.ToString()) * int.Parse(item.mqty.ToString());
                b.Add(c);
                a = a + 1;
                sum = sum + c;
            }
            ViewBag.price = b;
            ViewBag.sum = sum;
            var status = db.StoreReport.Where(m => m.orderguid == id).FirstOrDefault();
            if (status.srstatus == "0")
            {
                ViewBag.rep = "尚未處理";
            }else { ViewBag.rep = ""; }

            CVMdemu od = new CVMdemu()
            {
                sre=db.StoreReport.Where(m=>m.orderguid==id).ToList(),
                order = db.Order.Where(m => m.orderguid == id).ToList(),                
                menu = db.DetailMenu.Where(m => m.orderguid == id).ToList()           
            };


            return View("ReportDetail", "_LayoutMember",od);
        }
    }
}