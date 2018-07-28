using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using ClosedXML.Excel;
using MVC5Course.Models;
using X.PagedList;

namespace MVC5Course.Controllers
{
    public class 客戶資料Controller : BaseController
    {
        private 客戶資料Entities db = new 客戶資料Entities();
        客戶資料Repository repo = RepositoryHelper.Get客戶資料Repository();
        private int pageSize = 5;

        public List<客戶資料> GetCata()
        {
            List<客戶資料> _Cata = new List<客戶資料>();
            var queryA =
                (from o in db.客戶資料
                 orderby o.客戶分類
                 select o.客戶分類).Distinct();

            foreach (string c in queryA)
            {
                _Cata.Add(new 客戶資料 { 客戶分類 = c.ToString() });
            }
            return _Cata;
        }



        public ActionResult LoginPage(ClientAuth clientAuth)
        {
            if (clientAuth.帳號 != null && clientAuth.密碼 != null && ModelState.IsValid)
            {
                var client = repo.ClientLogin(clientAuth);

                if (client != null)
                {
                    string strUsername = client.FirstOrDefault().Id .ToString (); 

                    FormsAuthentication.RedirectFromLoginPage(strUsername, false);
                    if (User.Identity.IsAuthenticated)
                    {
                        ViewBag.Msg = "您現在是已登入狀態。";
                                       }
                    else
                    {
                        ViewBag.Msg = "登入失敗。";
                                          }
                   
                }
                else
                {
                    ViewBag.Msg = "帳密錯誤";
                    return View("LoginPage");
                }
            }
            ViewBag.Msg = "請登入";
            return View("LoginPage");
        }

        public ActionResult LoginUser()
        {
            ViewBag.Msg=  User.Identity.Name;
            if (!string.IsNullOrEmpty(User.Identity.Name))
            {
                var client = repo.Find(Convert.ToInt32(User.Identity.Name));


                
                byte[] bytes = System.Text.Encoding.UTF8.GetBytes(client.密碼);
                string Base64Pass = System.Text.Encoding.UTF8.GetString(Convert.FromBase64String(client.密碼));
                client.密碼 = Base64Pass;
                return View("LoginUser", client);
            }
            else
            {
                ViewBag.Msg = "請登入";
                return View("LoginPage");
            }
        }

        public ActionResult UserLoginEdit(UserLoginEdit userLoginEdit)
        {
            if ( ModelState.IsValid)
            {
                var client = repo.Find(userLoginEdit.Id);

                byte[] bytes = System.Text.Encoding.UTF8.GetBytes(userLoginEdit.密碼);
                string Base64Pass = Convert.ToBase64String(bytes);

                client.密碼 = Base64Pass;
                client.電話 = userLoginEdit.電話;
                client.傳真 = userLoginEdit.傳真;
                client.地址 = userLoginEdit.地址;
                repo.UnitOfWork.Commit();
                return RedirectToAction("Index");
            }
                return View();
        }

        public ActionResult SignOut()
        {
            FormsAuthentication.SignOut();
            return View("LoginPage");
           
        }

        [ActFilter]
        [ViewFilter]
        public ActionResult FieldSort(string field, string type, int? Page)
        {
            //var Client = repo.Sort(field, type);
            //var ClientFilter = RepositoryHelper.Get客戶資料Repository();
            //ViewBag.客戶分類 = new SelectList(ClientFilter.All().Distinct(), "客戶分類", "客戶分類");

            List<客戶資料> Title = GetCata();
            ViewData["客戶分類"] = new SelectList(Title, "客戶分類", "客戶分類");
            //return View("Index", Client);
            var Client = repo.All();

            switch (field)
            {
                case "客戶名稱":
                    if (type == "Desc")
                    {
                        Client = Client.OrderByDescending(s => s.客戶名稱);
                    }
                    else if (type == "Asc")
                    {
                        Client = Client.OrderBy(s => s.客戶名稱);
                    }

                    break;
                case "統一編號":
                    if (type == "Desc")
                    {
                        Client = Client.OrderByDescending(s => s.統一編號);
                    }
                    else if (type == "Asc")
                    {
                        Client = Client.OrderBy(s => s.統一編號);
                    }

                    break;
                case "電話":
                    if (type == "Desc")
                    {
                        Client = Client.OrderByDescending(s => s.電話);
                    }
                    else if (type == "Asc")
                    {
                        Client = Client.OrderBy(s => s.電話);

                    }

                    break;
                case "傳真":
                    if (type == "Desc")
                    {
                        Client = Client.OrderByDescending(s => s.傳真);
                    }
                    else if (type == "Asc")
                    {
                        Client = Client.OrderBy(s => s.傳真);
                    }

                    break;
                case "地址":
                    if (type == "Desc")
                    {
                        Client = Client.OrderByDescending(s => s.地址);
                    }
                    else if (type == "Asc")
                    {
                        Client = Client.OrderBy(s => s.地址);
                    }

                    break;
                case "Email":
                    if (type == "Desc")
                    {
                        Client = Client.OrderByDescending(s => s.Email);
                    }
                    else if (type == "Asc")
                    {
                        Client = Client.OrderBy(s => s.Email);
                    }

                    break;
                default:
                    Client = Client.OrderBy(s => s.Id);
                    break;
            }
            var pageNumber = Page ?? 1;
            ViewBag.CurrPage = pageNumber;
            return View("Index", Client.ToPagedList(pageNumber, pageSize));
        }

        // GET: 客戶資料
        [ActFilter]
        [ViewFilter]
        public ActionResult Index(int? Page)
        {
            var pageNumber = Page ?? 1;
            ViewBag.CurrPage = pageNumber;
            var client = repo.All().OrderBy(P => P.Id ).ToPagedList(pageNumber, pageSize);
            //var ClientFilter = RepositoryHelper.Get客戶資料Repository();
            //ViewBag.客戶分類 = new SelectList(ClientFilter.All().Distinct(), "客戶分類", "客戶分類");

            List<客戶資料> Title = GetCata();
            ViewData["客戶分類"] = new SelectList(Title, "客戶分類", "客戶分類");
            return View(client);
        }

        [ActFilter]
        [ViewFilter]
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

        [ActFilter]
        [ViewFilter]
        public ActionResult Search(string KeyWord, int? Page)
        {
            //var client = db.客戶資料.AsQueryable();
            //var client = repo.All();
            //if (!string.IsNullOrEmpty(KeyWord))
            //{
            //    client = client.Where(P => P.客戶名稱.Contains(KeyWord));

            //}
            
            List<客戶資料> Title = GetCata();
            ViewData["客戶分類"] = new SelectList(Title, "客戶分類", "客戶分類");


            var pageNumber = Page ?? 1;
            ViewBag.CurrPage = pageNumber;
            var client = repo.搜尋名稱(KeyWord).ToPagedList(pageNumber, pageSize);
            return View("Index", client);
        }

        [ActFilter]
        [ViewFilter]
        public ActionResult Filter(string 客戶分類, int? Page)
        {
            
            List<客戶資料> Title = GetCata();
            ViewData["客戶分類"] = new SelectList(Title, "客戶分類", "客戶分類");

            var pageNumber = Page ?? 1;
            ViewBag.CurrPage = pageNumber;
            var client = repo.客戶分類(客戶分類).ToPagedList(pageNumber, pageSize);
            return View("Index", client);
        }
        // GET: 客戶資料/Details/5
        [ActFilter]
        [ViewFilter]
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
        [ActFilter]
        [ViewFilter]
        public ActionResult Create()
        {
            List<客戶資料> Title = GetCata();
            ViewData["客戶分類"] = new SelectList(Title, "客戶分類", "客戶分類");
            return View();
        }

        // POST: 客戶資料/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [ActFilter]
        [ViewFilter]
        public ActionResult Create([Bind(Include = "Id,客戶名稱,統一編號,電話,傳真,地址,Email,是否已刪除,客戶分類")] 客戶資料 客戶資料)
        {
             repo.Add(客戶資料);
            repo.UnitOfWork.Commit();
            return View("Index");
        }

        // GET: 客戶資料/Edit/5
        [ActFilter]
        [ViewFilter]
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
            List<客戶資料> Title = GetCata();
            ViewData["客戶分類"] = new SelectList(Title, "客戶分類", "客戶分類");
            return View(client);
        }

        // POST: 客戶資料/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [ActFilter]
        [ViewFilter]
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
        [ActFilter]
        [ViewFilter]
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
        [ActFilter]
        [ViewFilter]
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

        [ActFilter]
        [ViewFilter]
        public ActionResult Details_OrderList(int id)
        {
            ViewData.Model = repo.Find(id).客戶聯絡人.ToList();
            return PartialView("ClientContact");
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
