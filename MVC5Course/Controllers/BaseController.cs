using ClosedXML.Excel;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MVC5Course.Controllers
{
    public class BaseController : Controller
    {
        // GET: Base
        public ActionResult ReturnToExcel(DataTable DT)
        {
            var workbook = new XLWorkbook();
            var ws = workbook.Worksheets.Add(DT); //新增頁籤
            workbook.SaveAs(Server.MapPath("~/App_Data/Export.xlsx"));
            return File(Server.MapPath("~/App_Data/Export.xlsx"), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet");
          
        }
       
    }
}