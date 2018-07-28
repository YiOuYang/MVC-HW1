using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MVC5Course.Controllers
{
    public class ActFilterAttribute: ActionFilterAttribute
    {

        DateTime S;
        DateTime E;
        TimeSpan Ts;
        public override void OnActionExecuting(ActionExecutingContext filterContext)
      {
            S = DateTime.Now;
            System.Diagnostics.Debug.WriteLine("Controler: "+filterContext.RouteData.Values["controller"].ToString()  +" Action: " + filterContext .RouteData.Values["action"].ToString() + " Action 開始執行:"+ S.ToString ("hh:mm:ss fff"));

          base.OnActionExecuting(filterContext);
      }

        public override void OnActionExecuted(ActionExecutedContext filterContext)
        {
            E = DateTime.Now;
            Ts = E - S;
            System.Diagnostics.Debug.WriteLine("Controler: " + filterContext.RouteData.Values["controller"].ToString() + " Action: " + filterContext.RouteData.Values["action"].ToString()+ " Action 執行完畢:" + E.ToString("hh:mm:ss fff"));
            System.Diagnostics.Debug.WriteLine("Controler: " + filterContext.RouteData.Values["controller"].ToString() + " Action: " + filterContext.RouteData.Values["action"].ToString() + " Action 執行耗時(秒):" + Ts.TotalSeconds );
            System.Diagnostics.Debug.WriteLine("");
            base.OnActionExecuted(filterContext);
        }

    }
}