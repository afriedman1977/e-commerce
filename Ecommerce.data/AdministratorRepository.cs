using System;
using System.Collections.Generic;
using System.Data.Linq;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Ecommerce.data
{
    public class AdministratorRepository
    {
        private string _connectionString;
        public AdministratorRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

        public void SignUp(Administrator admin)
        {
            using (EcommerceDbDataContext context = new EcommerceDbDataContext(_connectionString))
            {
                context.Administrators.InsertOnSubmit(admin);
                context.SubmitChanges();
            }
        }

        public Administrator SignIn(string username, string password)
        {
            using (EcommerceDbDataContext context = new EcommerceDbDataContext(_connectionString))
            {
                Administrator admin = context.Administrators.FirstOrDefault(a => a.UserName == username);
                if (admin == null)
                {
                    return null;
                }
                if (!PasswordHelper.PasswordMatch(password, admin.PasswordSalt, admin.PasswordHash))
                {
                    return null;
                }
                return admin;
            }
        }

        public void AddCategory(Category c)
        {
            using (var context = new EcommerceDbDataContext(_connectionString))
            {
                context.Categories.InsertOnSubmit(c);
                context.SubmitChanges();
            }
        }

        public void AddProduct(Product p)
        {
            using (var context = new EcommerceDbDataContext(_connectionString))
            {
                context.Products.InsertOnSubmit(p);
                context.SubmitChanges();
            }
        }

        public void AddImages(List<Image> images)
        {
            using (EcommerceDbDataContext context = new EcommerceDbDataContext(_connectionString))
            {
                context.Images.InsertAllOnSubmit(images);
                context.SubmitChanges();
            }
        }

        public IEnumerable<Category> AllCategories()
        {
            using (var context = new EcommerceDbDataContext(_connectionString))
            {
                return context.Categories.ToList();
            }
        }

        public IEnumerable<Order> PendingOrders()
        {
            using (var context = new EcommerceDbDataContext(_connectionString))
            {
                return context.Orders.Where(o => o.Fulfillled == false).ToList();
            }
        }

        public IEnumerable<Order> ProcessedOrders()
        {
            using (var context = new EcommerceDbDataContext(_connectionString))
            {
                return context.Orders.Where(o => o.Status == Status.Proccesing && o.Fulfillled == true).ToList();
            }
        }

        public IEnumerable<Order> OutForDeliveryOrders()
        {
            using(var context = new EcommerceDbDataContext(_connectionString))
            {
                return context.Orders.Where(o => o.Status == Status.OutForDelivery).ToList();
            }
        }

        public Order OrderDetails(int orderId)
        {
            using (var context = new EcommerceDbDataContext(_connectionString))
            {
                var loadoptions = new DataLoadOptions();
                loadoptions.LoadWith<Order>(o => o.OrderDetails);
                loadoptions.LoadWith<Order>(o => o.Customer);
                context.LoadOptions = loadoptions;
                return context.Orders.First(o => o.OrderId == orderId);
            }
        }

        public void Detailfulfilled(int odId, int orderId)
        {
            using (var context = new EcommerceDbDataContext(_connectionString))
            {
                context.ExecuteCommand("UPDATE OrderDetails SET Fulfilled = 'true' WHERE Id = {0}", odId);

                if (OrdrerFullfilled(orderId))
                {
                    context.ExecuteCommand("UPDATE Orders SET Fulfillled = 'true' WHERE OrderId = {0}", orderId);
                }
            }
        }

        public void UpdateOrderStatus(int orderId, Status status)
        {
            using (var context = new EcommerceDbDataContext(_connectionString))
            {
                Order o = context.Orders.First(or => or.OrderId == orderId);
                o.Status = status;
                context.SubmitChanges();
            }
        }

        private bool OrdrerFullfilled(int orderId)
        {
            using (var context = new EcommerceDbDataContext(_connectionString))
            {
                return context.OrderDetails.Where(od => od.OrderId == orderId).All(od => od.Fulfilled == true);
            }
        }
    }
}
