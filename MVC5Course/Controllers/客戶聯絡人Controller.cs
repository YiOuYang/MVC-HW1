using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Validation;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using ClosedXML.Excel;
using MVC5Course.Models;
using X.PagedList;

namespace MVC5Course.Controllers
{
    public class 客戶聯絡人Controller : Controller
    {
        private 客戶資料Entities db = new 客戶資料Entities();
        客戶聯絡人Repository repo = RepositoryHelper.Get客戶聯絡人Repository();
        private int pageSize = 5;

        public List<客戶聯絡人> GetTitles()
        {
            List<客戶聯絡人> _Title = new List<客戶聯絡人>();
            var queryA =
                (from o in db.客戶聯絡人
                 orderby o.職稱
                 select o.職稱).Distinct();

            foreach (string c in queryA)
            {
                _Title.Add(new 客戶聯絡人 { 職稱 = c.ToString() });
            }
            return _Title;
        }

        [ActFilter]
        [ViewFilter]
        public ActionResult FieldSort(string field, string type, int? Page)
        {
            //var Contact = repo.Sort(field, type);
            var ClientFilter = RepositoryHelper.Get客戶聯絡人Repository();
            ViewBag.職稱 = new SelectList(ClientFilter.All().Distinct(), "職稱", "職稱");
           //return View("Index", Contact);


            var Contact = repo.All();

            switch (field)
            {
                case "職稱":
                    if (type == "Desc")
                    {
                        Contact = Contact.OrderByDescending(s => s.職稱);
                    }
                    else if (type == "Asc")
                    {
                        Contact = Contact.OrderBy(s => s.職稱);
                    }

                    break;
                case "姓名":
                    if (type == "Desc")
                    {
                        Contact = Contact.OrderByDescending(s => s.姓名);
                    }
                    else if (type == "Asc")
                    {
                        Contact = Contact.OrderBy(s => s.姓名);
                    }

                    break;
                case "Email":
                    if (type == "Desc")
                    {
                        Contact = Contact.OrderByDescending(s => s.Email);
                    }
                    else if (type == "Asc")
                    {
                        Contact = Contact.OrderBy(s => s.Email);

                    }

                    break;
                case "手機":
                    if (type == "Desc")
                    {
                        Contact = Contact.OrderByDescending(s => s.手機);
                    }
                    else if (type == "Asc")
                    {
                        Contact = Contact.OrderBy(s => s.手機);
                    }

                    break;
                case "電話":
                    if (type == "Desc")
                    {
                        Contact = Contact.OrderByDescending(s => s.電話);
                    }
                    else if (type == "Asc")
                    {
                        Contact = Contact.OrderBy(s => s.電話);
                    }

                    break;
              
                default:
                    Contact = Contact.OrderBy(s => s.Id);
                    break;
            }
            var pageNumber = Page ?? 1;
            ViewBag.CurrPage = pageNumber;
            return View("Index", Contact.ToPagedList(pageNumber, pageSize));
        }

        // GET: 客戶聯絡人
        [ActFilter]
        [ViewFilter]
        public ActionResult Index(int? Page)
        {
            //var 客戶聯絡人 = db.客戶聯絡人.Include(客 => 客.客戶資料);
            //return View(客戶聯絡人.ToList());
            var pageNumber = Page ?? 1;
            ViewBag.CurrPage = pageNumber;

            var Contact = repo.All().OrderBy(P => P.Id).ToPagedList(pageNumber, pageSize);

            List<客戶聯絡人> Title = this.GetTitles();
            ViewData["職稱"] = new SelectList(Title, "職稱", "職稱");


            //var ClientFilter = RepositoryHelper.Get客戶聯絡人Repository();
            //ViewBag.職稱 = new SelectList(ClientFilter.All().Distinct(), "職稱", "職稱"); 

            return View(Contact);
        }

        [ActFilter]
        [ViewFilter]
        public ActionResult ExportExcel()
        {
            // var workbook = new XLWorkbook();
            DataTable DT = new DataTable("客戶聯絡人");
            DT.Columns.AddRange(new DataColumn[8] {
                 new DataColumn("Id"),
                new DataColumn("客戶Id"),
                new DataColumn("職稱"),
                new DataColumn("姓名"),
                new DataColumn("Email"),
                new DataColumn("手機"),
                new DataColumn("電話"),
                new DataColumn("是否已刪除")});

            var Contact = repo.All();
            foreach (var Contacts in Contact)
            {
                DT.Rows.Add(Contacts.Id, Contacts.客戶Id, Contacts.職稱,  Contacts.姓名, Contacts.Email, Contacts.手機, Contacts.電話, Contacts.是否已刪除);
            }
            //ReturnExcel(DT);
            var workbook = new XLWorkbook();
            var ws = workbook.Worksheets.Add(DT); //新增頁籤
            workbook.SaveAs(Server.MapPath("~/App_Data/Export.xlsx"));
            return File(Server.MapPath("~/App_Data/Export.xlsx"), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet");
        }

        [ActFilter]
        [ViewFilter]
        public ActionResult Search(string KeyWord, int? Page)
        {
            //var client = db.客戶聯絡人.AsQueryable();

            //if (!string.IsNullOrEmpty(KeyWord))
            //{
            //    client = client.Where(P => P.姓名 .Contains(KeyWord));

            //}
            //return View("Index", client);
            List<客戶聯絡人> Title = this.GetTitles();
            ViewData["職稱"] = new SelectList(Title, "職稱", "職稱");

            var pageNumber = Page ?? 1;
            ViewBag.CurrPage = pageNumber;
            var Contact = repo.客戶名稱(KeyWord).ToPagedList(pageNumber, pageSize);
          
            return View("Index", Contact);
        }

        [ActFilter]
        [ViewFilter]
        public ActionResult Filter(string 職稱,int? Page)
        {

            List<客戶聯絡人> Title = this.GetTitles();
            ViewData["職稱"] = new SelectList(Title, "職稱", "職稱");

            var pageNumber = Page ?? 1;
            ViewBag.CurrPage = pageNumber;
            var Contact = repo.職稱(職稱).ToPagedList(pageNumber, pageSize);
            return View("Index", Contact);
        }

        // GET: 客戶聯絡人/Details/5
        [ActFilter]
        [ViewFilter]
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            //客戶聯絡人 客戶聯絡人 = db.客戶聯絡人.Find(id);
            //if (客戶聯絡人 == null)
            //{
            //    return HttpNotFound();
            //}
            //return View(客戶聯絡人);

            var Contact = repo.Find(id);
            return View(Contact);

        }

        // GET: 客戶聯絡人/Create
        [ActFilter]
        [ViewFilter]
        public ActionResult Create()
        {
           // ViewBag.客戶Id = new SelectList(db.客戶資料, "Id", "客戶名稱");

            var Client = RepositoryHelper.Get客戶資料Repository();
            ViewBag.客戶Id = new SelectList(Client.All (), "Id", "客戶名稱");
            return View();
        }

        // POST: 客戶聯絡人/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [ActFilter]
        [ViewFilter]
        public ActionResult Create([Bind(Include = "Id,客戶Id,職稱,姓名,Email,手機,電話,是否已刪除")] 客戶聯絡人 客戶聯絡人)
        {
            if (ModelState.IsValid)
            {
                //db.客戶聯絡人.Add(客戶聯絡人);
                //db.SaveChanges();
                repo.Add(客戶聯絡人);
                repo.UnitOfWork.Commit();
                return RedirectToAction("Index");
            }

            //ViewBag.客戶Id = new SelectList(db.客戶資料, "Id", "客戶名稱", 客戶聯絡人.客戶Id);
            var Client = RepositoryHelper.Get客戶資料Repository();
            ViewBag.客戶Id = new SelectList(Client.All(), "Id", "客戶名稱");
            return View(客戶聯絡人);
        }

        // GET: 客戶聯絡人/Edit/5
        [ActFilter]
        [ViewFilter]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            //客戶聯絡人 客戶聯絡人 = db.客戶聯絡人.Find(id);
            //if (客戶聯絡人 == null)
            //{
            //    return HttpNotFound();
            //}


            //ViewBag.客戶Id = new SelectList(db.客戶資料, "Id", "客戶名稱", 客戶聯絡人.客戶Id);
            var Contact = repo.Find(id);
            var Client = RepositoryHelper.Get客戶資料Repository();
            ViewBag.客戶Id = new SelectList(Client.All(), "Id", "客戶名稱", Contact.Id );
           // return View(客戶聯絡人);
            return View(Contact);
        }

        // POST: 客戶聯絡人/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [ActFilter]
        [ViewFilter]
        public ActionResult Edit([Bind(Include = "Id,客戶Id,職稱,姓名,Email,手機,電話,是否已刪除")] 客戶聯絡人 客戶聯絡人)
        {
            if (ModelState.IsValid)
            {
                //db.Entry(客戶聯絡人).State = EntityState.Modified;
                //db.SaveChanges();
                repo.UnitOfWork.Context.Entry (客戶聯絡人).State = EntityState.Modified;
                repo.UnitOfWork.Commit();
                return RedirectToAction("Index");
            }
            //ViewBag.客戶Id = new SelectList(db.客戶資料, "Id", "客戶名稱", 客戶聯絡人.客戶Id);

            var Client = RepositoryHelper.Get客戶資料Repository();
            ViewBag.客戶Id = new SelectList(Client.All(), "Id", "客戶名稱");
            return View(客戶聯絡人);
        }

        // GET: 客戶聯絡人/Delete/5
        [ActFilter]
        [ViewFilter]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            // 客戶聯絡人 客戶聯絡人 = db.客戶聯絡人.Find(id);
            //if (客戶聯絡人 == null)
            //{
            //    return HttpNotFound();
            //}
           // return View(客戶聯絡人);
            var Contact = repo.Find(id);
            return View(Contact);
        }

        // POST: 客戶聯絡人/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [ActFilter]
        [ViewFilter]
        public ActionResult DeleteConfirmed(int id)
        {
            //客戶聯絡人 客戶聯絡人 = db.客戶聯絡人.Find(id);
            //db.客戶聯絡人.Remove(客戶聯絡人);
            //db.SaveChanges();

            var Contact = repo.Find(id);
            repo.Delete(Contact);
            repo.UnitOfWork.Commit();
           return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                repo.UnitOfWork.Context.Dispose();
               // db.Dispose();
            }
            base.Dispose(disposing);
        }

        [HttpPost]
        [ActFilter]
        [ViewFilter ]
        [HandleError(ExceptionType = typeof(DbEntityValidationException), View = "ErrorCustom")]
        public ActionResult BatchUpdate(ContactBatch[]  Data)
        {
           

            List<客戶聯絡人> Title = this.GetTitles();
            ViewData["職稱"] = new SelectList(Title, "職稱", "職稱");

            if (ModelState.IsValid)
            {
                foreach (var Contact in Data)
                {
                    var ContactInfo = repo.Find(Contact.Id );
                    ContactInfo.職稱  = Contact.職稱;
                    ContactInfo.手機  = Contact.手機;
                    ContactInfo.電話  = Contact.電話;
                }

                repo.UnitOfWork.Commit();

                return RedirectToAction("Index");
            }

            ViewData.Model = repo.All();

            return View("Index");
        }
    }
}
