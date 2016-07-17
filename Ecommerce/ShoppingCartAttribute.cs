using Ecommerce.data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Providers.Entities;

namespace Ecommerce
{
    public class ShoppingCartAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            var repo = new HomeRepository(Properties.Settings.Default.constr);
            if (filterContext.HttpContext.Session["cartId"] != null)
            {
                filterContext.Controller.ViewBag.CartQuantity = repo.NumberOfItemsInCart((int)filterContext.HttpContext.Session["cartId"]);
            }
            else
            {
                filterContext.Controller.ViewBag.CartQuantity = 0;
            }
            base.OnActionExecuting(filterContext);
        }
    }
}