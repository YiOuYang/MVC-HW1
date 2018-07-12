using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using ClosedXML.Excel;
using MVC5Course.Models;

namespace MVC5Course.Controllers
{
    public class 客戶銀行資訊Controller : Controller
    {
       // private 客戶資料Entities db = new 客戶資料Entities();
        客戶銀行資訊Repository repo = RepositoryHelper.Get客戶銀行資訊Repository();

        public ActionResult FieldSort(string field, string type)
        {
            var Bank = repo.Sort(field, type);
            return View("Index", Bank);
        }

        // GET: 客戶銀行資訊
        public ActionResult Index()
        {
            //var 客戶銀行資訊 = db.客戶銀行資訊.Include(客 => 客.客戶資料);
            var Bank = repo.All(); 
            return View(Bank.ToList());
        }

        public ActionResult ExportExcel()
        {
            // var workbook = new XLWorkbook();
            DataTable DT = new DataTable("客戶銀行資訊");
            DT.Columns.AddRange(new DataColumn[8] {
                new DataColumn("Id"),
                new DataColumn("客戶Id"),
                new DataColumn("銀行名稱"),
                new DataColumn("銀行代碼"),
                new DataColumn("分行代碼"),
                new DataColumn("帳戶名稱"),
                new DataColumn("帳戶號碼"),
                new DataColumn("是否已刪除")});

            var Bank = repo.All();
            foreach (var BankUser in Bank)
            {
                DT.Rows.Add(BankUser.Id , BankUser.客戶Id , BankUser.銀行名稱 , BankUser.銀行代碼 , BankUser.分行代碼 , BankUser.帳戶名稱 , BankUser.帳戶號碼 , BankUser.是否已刪除 );
            }
            //ReturnExcel(DT);
            var workbook = new XLWorkbook();
            var ws = workbook.Worksheets.Add(DT); //新增頁籤
            workbook.SaveAs(Server.MapPath("~/App_Data/Export.xlsx"));
            return File(Server.MapPath("~/App_Data/Export.xlsx"), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet");
        }

        public ActionResult Search(string KeyWord)
        {
            //var client = db.客戶銀行資訊.AsQueryable();

            //if (!string.IsNullOrEmpty(KeyWord))
            //{
            //    client = client.Where(P => P.銀行名稱 .Contains(KeyWord));

            //}

            var Bank = repo.銀行名稱(KeyWord);
            return View("Index", Bank);
       
        }

        // GET: 客戶銀行資訊/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            //客戶銀行資訊 客戶銀行資訊 = db.客戶銀行資訊.Find(id);
            //if (客戶銀行資訊 == null)
            //{
            //    return HttpNotFound();
            //}
            //return View(客戶銀行資訊);

            var Bank = repo.Find(id);
            return View(Bank);
        }

        //GET: 客戶銀行資訊/Create
        public ActionResult Create()
        {
            //ViewBag.客戶Id = new SelectList(db.客戶資料, "Id", "客戶名稱");
            var Client = RepositoryHelper.Get客戶資料Repository();
            ViewBag.客戶Id = new SelectList(Client.All()  , "Id", "客戶名稱");
            return View();


        }

        // POST: 客戶銀行資訊/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,客戶Id,銀行名稱,銀行代碼,分行代碼,帳戶名稱,帳戶號碼,是否已刪除")] 客戶銀行資訊 客戶銀行資訊)
        {
            if (ModelState.IsValid)
            {
                //db.客戶銀行資訊.Add(客戶銀行資訊);
                //db.SaveChanges();

                repo.Add(客戶銀行資訊);
                repo.UnitOfWork.Commit();
            }

            //ViewBag.客戶Id = new SelectList(db.客戶資料, "Id", "客戶名稱", 客戶銀行資訊.客戶Id);
            //return View(客戶銀行資訊);

            //ViewBag.客戶Id = new SelectList(db.客戶資料, "Id", "客戶名稱", 客戶銀行資訊.客戶Id);
           
            var Client = RepositoryHelper.Get客戶資料Repository();
            ViewBag.客戶Id = new SelectList(Client.All(), "Id", "客戶名稱");
            return View("Index", 客戶銀行資訊);
        }

        // GET: 客戶銀行資訊/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            //客戶銀行資訊 客戶銀行資訊 = db.客戶銀行資訊.Find(id);

            //if (客戶銀行資訊 == null)
            //{
            //    return HttpNotFound();
            //}
            //ViewBag.客戶Id = new SelectList(db.客戶資料, "Id", "客戶名稱", 客戶銀行資訊.客戶Id);
            //return View(客戶銀行資訊);
            var Bank = repo.Find(id);
            var Client = RepositoryHelper.Get客戶資料Repository();
            ViewBag.客戶Id = new SelectList(Client.All(), "Id", "客戶名稱", Bank.Id );
            return View(Bank);

        }

        // POST: 客戶銀行資訊/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,客戶Id,銀行名稱,銀行代碼,分行代碼,帳戶名稱,帳戶號碼,是否已刪除")] 客戶銀行資訊 客戶銀行資訊)
        {
            if (ModelState.IsValid)
            {
                //db.Entry(客戶銀行資訊).State = EntityState.Modified;
                //db.SaveChanges();
                //return RedirectToAction("Index");

                repo.UnitOfWork.Context.Entry (客戶銀行資訊).State= EntityState.Modified;
                repo.UnitOfWork.Commit();
                return RedirectToAction("Index");
            }
            //ViewBag.客戶Id = new SelectList(db.客戶資料, "Id", "客戶名稱", 客戶銀行資訊.客戶Id);
            //return View(客戶銀行資訊);

            //ViewBag.客戶Id = new SelectList(db.客戶資料, "Id", "客戶名稱", 客戶銀行資訊.客戶Id);
            return View("Index");
        }

        // GET: 客戶銀行資訊/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            //客戶銀行資訊 客戶銀行資訊 = db.客戶銀行資訊.Find(id);
            // if (客戶銀行資訊 == null)
            //{
            //    return HttpNotFound();
            //}
            //return View(客戶銀行資訊);

            var Bank = repo.Find(id);
            return View(Bank);
        }

        // POST: 客戶銀行資訊/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            //客戶銀行資訊 客戶銀行資訊 = db.客戶銀行資訊.Find(id);
            //db.客戶銀行資訊.Remove(客戶銀行資訊);
            //db.SaveChanges();
            var Bank = repo.Find(id);
            repo.Delete(Bank);
            repo.UnitOfWork.Commit();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                repo.UnitOfWork.Context.Dispose();
                //db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
