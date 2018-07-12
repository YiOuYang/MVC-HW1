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
    public class 客戶資料Controller : BaseController
    {
        //private 客戶資料Entities db = new 客戶資料Entities();
        客戶資料Repository repo = RepositoryHelper.Get客戶資料Repository();


        public ActionResult FieldSort(string field, string type)
        {
            var Client = repo.Sort(field, type);
            var ClientFilter = RepositoryHelper.Get客戶資料Repository();
            ViewBag.客戶分類 = new SelectList(ClientFilter.All().Distinct(), "客戶分類", "客戶分類");
            return View("Index", Client);
        }

        // GET: 客戶資料
        public ActionResult Index()
        {
          
            var client = repo.All();
            var ClientFilter = RepositoryHelper.Get客戶資料Repository();
             //ViewBag.客戶分類 = new SelectList(ClientFilter.All().Select(p => p.客戶分類 ).Distinct(), "客戶分類", "客戶分類");
            ViewBag.客戶分類 = new SelectList(ClientFilter.All().Distinct(), "客戶分類", "客戶分類");
            return View(client.Take(50).ToList());
        }

        public ActionResult ExportExcel()
        {
           // var workbook = new XLWorkbook();
            DataTable DT = new DataTable("客戶資料");
            DT.Columns.AddRange(new DataColumn[9] {
                 new DataColumn("Id"),
                new DataColumn("客戶名稱"),
                new DataColumn("統一編號"),
                new DataColumn("電話"),
                new DataColumn("傳真"),
                new DataColumn("地址"),
                new DataColumn("Email"),
                new DataColumn("是否已刪除"),
                new DataColumn("客戶分類")});
          
            var Client = repo.All();
            foreach (var Clients in Client)
            {
                DT.Rows.Add(Clients.Id,Clients.客戶名稱, Clients.統一編號, Clients.電話, Clients.傳真 , Clients.地址 , Clients.Email , Clients.是否已刪除 , Clients.客戶分類);
            }
            

            var workbook = new XLWorkbook();
            var ws = workbook.Worksheets.Add(DT); //新增頁籤
            workbook.SaveAs(Server.MapPath("~/App_Data/Export.xlsx"));
            return File(Server.MapPath("~/App_Data/Export.xlsx"), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet");
            //return null;
            //ReturnToExcel(DT);
        }

        public ActionResult Search(string KeyWord)
        {
            //var client = db.客戶資料.AsQueryable();
            //var client = repo.All();
            //if (!string.IsNullOrEmpty(KeyWord))
            //{
            //    client = client.Where(P => P.客戶名稱.Contains(KeyWord));

            //}
            var client = repo.搜尋名稱(KeyWord);
            var ClientFilter = RepositoryHelper.Get客戶資料Repository();
            ViewBag.客戶分類 = new SelectList(ClientFilter.All().Distinct(), "客戶分類", "客戶分類");
            return View("Index", client);
        }

        public ActionResult Filter(string 客戶分類)
        {
            var client = repo.客戶分類 (客戶分類);
            var ClientFilter = RepositoryHelper.Get客戶資料Repository();
            ViewBag.客戶分類 = new SelectList(ClientFilter.All().Distinct(), "客戶分類", "客戶分類");
            return View("Index", client);
        }
        // GET: 客戶資料/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var client = repo.Find(id);
            
            if (client == null)
            {
                return HttpNotFound();
            }
            return View(client);
        }

        // GET: 客戶資料/Create
        public ActionResult Create()
        {
            var Client = RepositoryHelper.Get客戶資料Repository();
            ViewBag.客戶分類 = new SelectList(Client.All().Distinct() , "客戶分類", "客戶分類");
            return View();
        }

        // POST: 客戶資料/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,客戶名稱,統一編號,電話,傳真,地址,Email,是否已刪除,客戶分類")] 客戶資料 客戶資料)
        {
             repo.Add(客戶資料);
            repo.UnitOfWork.Commit();
            return View("Index");
        }

        // GET: 客戶資料/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            //客戶資料 客戶資料 = db.客戶資料.Find(id);
            var client = repo.Find(id);
            if (client == null)
            {
                return HttpNotFound();
            }
            var Client = RepositoryHelper.Get客戶資料Repository();
            ViewBag.客戶分類 = new SelectList(Client.All().Distinct( ), "客戶分類", "客戶分類", client.客戶分類  );
            return View(client);
        }

        // POST: 客戶資料/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,客戶名稱,統一編號,電話,傳真,地址,Email,是否已刪除,客戶分類")] 客戶資料 客戶資料)
        {
            if (ModelState.IsValid)
            {
                //db.Entry(客戶資料).State = EntityState.Modified;
                //db.SaveChanges();
                //return RedirectToAction("Index");
 
                repo.UnitOfWork.Context.Entry(客戶資料).State = EntityState.Modified;
                repo.UnitOfWork.Commit();
                return RedirectToAction("Index");

            }
            return View(客戶資料);
        }

        // GET: 客戶資料/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var client = repo.Find(id.Value);
            if (client == null)
            {
                return HttpNotFound();
            }
            return View(client);
        }

        // POST: 客戶資料/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            //客戶資料 客戶資料 = db.客戶資料.Find(id);
            //db.客戶資料.Remove(客戶資料);
            //db.SaveChanges();

            客戶資料 client = repo.Find(id);
            repo.Delete(client);
            repo.UnitOfWork.Commit();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                repo.UnitOfWork .Context .Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
