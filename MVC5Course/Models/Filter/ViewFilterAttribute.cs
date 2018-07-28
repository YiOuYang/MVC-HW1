using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MVC5Course.Controllers
{
    public class ViewFilterAttribute : ActionFilterAttribute
    {
        DateTime S;
        DateTime E;
        TimeSpan Ts;

        public override void OnResultExecuting(ResultExecutingContext filterContext)
        {
            S = DateTime.Now;

            var a=filterContext.Controller.ViewData["view"];
            System.Diagnostics.Debug.WriteLine("Controler: " + filterContext.RouteData.Values["controller"].ToString() + " Action: " + filterContext.RouteData.Values["action"].ToString() + " View 開始執行:" + S.ToString("hh:mm:ss fff"));

            base.OnResultExecuting(filterContext);
        }

        public override void OnResultExecuted(ResultExecutedContext filterContext)
        {
            E = DateTime.Now;
            Ts = E - S;
            System.Diagnostics.Debug.WriteLine("Controler: " + filterContext.RouteData.Values["controller"].ToString() + " Action: " + filterContext.RouteData.Values["action"].ToString() + " View 執行完畢:" + E.ToString("hh:mm:ss fff"));
            System.Diagnostics.Debug.WriteLine("Controler: " + filterContext.RouteData.Values["controller"].ToString() + " Action: " + filterContext.RouteData.Values["action"].ToString() + " View 執行耗時(秒):" + Ts.TotalSeconds);
            System.Diagnostics.Debug.WriteLine("");
            base.OnResultExecuted(filterContext);
        }
    }
}