using Ecommerce.data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Ecommerce
{
    public class LoggedinAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            var repo = new AdministratorRepository(Properties.Settings.Default.constr);
            if(HttpContext.Current.User.Identity.IsAuthenticated)
            {
                filterContext.Controller.ViewBag.isLoggedin = true;
            }
            else
            {
                filterContext.Controller.ViewBag.isLoggedin = false;
            }
            base.OnActionExecuting(filterContext);
        }
    }
}