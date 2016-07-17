using AttributeRouting;
using AttributeRouting.Web.Mvc;
using Ecommerce.data;
using Ecommerce.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace Ecommerce.Controllers
{
    [RoutePrefix("/AandPSales")]
    public class HomeController : Controller
    {
        [Route("Home")]
        public ActionResult Index(int? categoryId)
        {

            var repo = new HomeRepository(Properties.Settings.Default.constr);
            HomeIndexViewModel hivm = new HomeIndexViewModel();
            hivm.Allcategories = repo.AllCategories();
            if (categoryId == null)
            {
                categoryId = hivm.Allcategories.ElementAt(0).Id;
            }
            hivm.ProductsWithImages = repo.AllProductsWithImagesForCategory(categoryId.Value);
            if (TempData["OrderPlaced"] != null)
            {
                hivm.Message = (string)TempData["OrderPlaced"];
                Session.Abandon();
            }
            return View(hivm);
        }

        [Route("Product-{productName}/{productId}")]
        public ActionResult ProductDetails(int productId,string productName)
        {
            var repo = new HomeRepository(Properties.Settings.Default.constr);
            ProductDetailsViewModel pdvm = new ProductDetailsViewModel();
            pdvm.Product = repo.ProductById(productId);
            return View(pdvm);
        }

        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Login(string username, string password)
        {
            AdministratorRepository repo = new AdministratorRepository(Properties.Settings.Default.constr);
            Administrator admin = repo.SignIn(username, password);
            if (admin == null)
            {
                return Redirect("/Home/Login");
            }

            FormsAuthentication.SetAuthCookie(username, true);
            return Redirect("/Admin/Index");
        }

        [HttpPost]
        public ActionResult AddToCart(ShoppingCartItem item)
        {
            var repo = new HomeRepository(Properties.Settings.Default.constr);
            if (Session["cartId"] == null)
            {
                ShoppingCart cart = new ShoppingCart
                {
                    DateCreated = DateTime.Now
                };
                repo.CreatShoppingCart(cart);
                Session["cartId"] = cart.Id;
            }
            item.ShoppingCartId = (int)Session["cartId"];
            AddToCartViewModel acvm = new AddToCartViewModel();
            acvm.Message = item.Quantity + " items successfully added to cart";
            if (repo.DoesShoppingCartItemExist(item))
            {
                item.Quantity += repo.CurrentShoppingCartItem(item).Quantity;
                item.Id = repo.CurrentShoppingCartItem(item).Id;
                repo.UpdateQuantityOfItemInShoppingCart(item);
            }
            else
            {
                repo.AddItemToShoppingCart(item);
            }
            acvm.CartQuantity = repo.NumberOfItemsInCart((int)Session["cartId"]);
            return Json(acvm);
        }

        public ActionResult Cart(int? id)
        {
            var repo = new HomeRepository(Properties.Settings.Default.constr);
            CartViewModel cvm = new CartViewModel();
            cvm.ProductsInCart = new List<CartModel>();
            if (Session["cartId"] == null || repo.AllItemsInShoppingCart((int)Session["cartId"]).Count() == 0)
            {
                return Redirect("/Home/EmptyCart");
            }
            foreach (ShoppingCartItem item in repo.AllItemsInShoppingCart((int)Session["cartId"]))
            {
                cvm.ProductsInCart.Add(new CartModel
                {
                    CartItem = item,
                    TotalPerItem = item.Quantity * item.Product.Price
                });
            }
            cvm.TotalForCart = cvm.ProductsInCart.Sum(c => c.TotalPerItem);
            if (id != null)
            {
                cvm.Customer = repo.Customer(id.Value);
            }
            if(Session["CustomerId"] != null)
            {
                cvm.Customer = repo.Customer((int)Session["CustomerId"]);
            }
            return View(cvm);
        }

        public ActionResult EmptyCart()
        {
            return View();
        }

        [HttpPost]
        public ActionResult UpdateShoppingCartItem(ShoppingCartItem itemToUpdate)
        {
            var repo = new HomeRepository(Properties.Settings.Default.constr);
            if (itemToUpdate.Quantity > 0)
            {
                repo.UpdateQuantityOfItemInShoppingCart(itemToUpdate.Id, itemToUpdate.Quantity);
            }
            else
            {
                repo.DeleteItemFromCart(itemToUpdate.Id);
            }
            return Redirect("/Home/Cart");
        }

        public ActionResult Checkout()
        {
            var repo = new HomeRepository(Properties.Settings.Default.constr);
            CheckoutViewModel cvm = new CheckoutViewModel();
            if (TempData["missingInfo"] != null)
            {
                cvm.Message = (string)TempData["missinginfo"];
            }

            if(Session["CustomerId"] != null)
            {
                cvm.Customer = repo.Customer((int)Session["CustomerId"]);
            }
            return View(cvm);
        }

        [HttpPost]
        public ActionResult Checkout(Customer c)
        {
            if (c.FirstName == null || c.LastName == null || c.Address == null || c.PhoneNumber == null || c.EmailAddress == null)
            {
                TempData["missingInfo"] = "You left out some of your info";
                return Redirect("/Home/Checkout");
            }
            //Type type = c.GetType();
            //PropertyInfo[] properties = type.GetProperties();
            //foreach (PropertyInfo property in properties)
            //{
            //    //if (property.Equals(null))
            //    if (property.GetValue(c) == null)
            //    {
            //        TempData["missingInfo"] = "You left out your " + property.Name;
            //        return Redirect("/Home/Checkout");
            //    }
            //}
            var repo = new HomeRepository(Properties.Settings.Default.constr);
            repo.AddCustomer(c);
            Session["CustomerId"] = c.Id;
            return Redirect("/Home/Cart?id=" + c.Id);
        }

        [HttpPost]
        public ActionResult UpdateCustomer(Customer c)
        {
            var repo = new HomeRepository(Properties.Settings.Default.constr);
            repo.UpdateCustomer(c);
            return Redirect("/Home/Cart?id=" + c.Id);
        }

        [HttpPost]
        public ActionResult PlaceOrder(Order o, List<OrderDetail> details)
        {
            var repo = new HomeRepository(Properties.Settings.Default.constr);
            repo.CreateOrder(o);
            foreach (OrderDetail od in details)
            {
                od.OrderId = o.OrderId;
            }
            repo.AddOrderDetails(details);
            //Session.Abandon();
            TempData["OrderPlaced"] = "Order Submitted successfully Your Order Number is " + o.OrderId;            
            return Redirect("/Home/Index");
        }

        [Route("Status/Order-{orderId}")]
        public ActionResult CheckStatus(int orderId)
        {
            var repo = new HomeRepository(Properties.Settings.Default.constr);
            Order order = repo.GetOrderById(orderId);
            CheckStatusViewModel covm;
            if (order != null)
            {
               covm = new CheckStatusViewModel(order);
            }
            else
            {
                covm = new CheckStatusViewModel();
            }
            return View(covm);
        }
    }
}
