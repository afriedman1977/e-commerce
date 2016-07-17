using Ecommerce.data;
using Ecommerce.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Ecommerce.Controllers
{
    [Authorize]
    public class AdminController : Controller
    {
        public ActionResult Index()
        {
            var repo = new AdministratorRepository(Properties.Settings.Default.constr);
            AdminIndexViewModel aivm = new AdminIndexViewModel();
            aivm.PendingOrders = repo.PendingOrders();
            aivm.ProcessedOrders = repo.ProcessedOrders();
            aivm.OutForDeliveryOrders = repo.OutForDeliveryOrders();
            return View(aivm);
        }

        public ActionResult OrderDetails(int orderId)
        {
            var repo = new AdministratorRepository(Properties.Settings.Default.constr);            
            var odvm = new OrderDetailsViewModel();
            odvm.OrderDetails = repo.OrderDetails(orderId);          
            if(odvm.OrderDetails.Status == Status.Submitted)
            {
                repo.UpdateOrderStatus(orderId, Status.Viewed);
            }
            return View(odvm);
        }

        public ActionResult AddCategory()
        {
            return View();
        }

        [HttpPost]
        public ActionResult AddCategory(Category category)
        {
            AdministratorRepository repo = new AdministratorRepository(Properties.Settings.Default.constr);
            repo.AddCategory(category);
            TempData["CategoryAdded"] = "Category " + category.CategoryName + " Successfully Added";
            return Redirect("/Admin/Index");
        }

        public ActionResult AddProduct()
        {
            var repo = new AdministratorRepository(Properties.Settings.Default.constr);
            var apvm = new AddProductViewModel();
            apvm.AllCategories = repo.AllCategories();
            return View(apvm);
        }

        [HttpPost]
        public ActionResult AddProduct(Product product, List<HttpPostedFileBase> imageFiles)
        {
            var repo = new AdministratorRepository(Properties.Settings.Default.constr);
            repo.AddProduct(product);
            List<Image> images = new List<Image>();
            foreach (HttpPostedFileBase f in imageFiles)
            {
                string fileName = Guid.NewGuid() + Path.GetExtension(f.FileName);
                f.SaveAs(Server.MapPath("~/Images/" + fileName));
                Image image = new Image
                {
                    ImagePath = fileName,
                    ProductId = product.Id
                };
                images.Add(image);
            }
            repo.AddImages(images);
            return Redirect("/Admin/Index");
        }

        [HttpPost]
        public ActionResult DetailFulfilled(int odId, int orderId)
        {
            var repo = new AdministratorRepository(Properties.Settings.Default.constr);
            repo.UpdateOrderStatus(orderId, Status.Proccesing);
            repo.Detailfulfilled(odId, orderId);
            return Json(odId);
        }

        [HttpPost]
        public ActionResult UpdateStatus(int orderId, Status status)
        {
            var repo = new AdministratorRepository(Properties.Settings.Default.constr);
            repo.UpdateOrderStatus(orderId, status);
            return Json(orderId);
        }
    }
}
