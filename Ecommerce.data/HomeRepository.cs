using System;
using System.Collections.Generic;
using System.Data.Linq;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.data
{
    public class HomeRepository
    {
        private string _connectionString;
        public HomeRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

        public IEnumerable<Category> AllCategories()
        {
            using (var context = new EcommerceDbDataContext(_connectionString))
            {
                return context.Categories.ToList();
            }
        }

        public IEnumerable<Product> AllProductsWithImagesForCategory(int categoryId)
        {
            using (var context = new EcommerceDbDataContext(_connectionString))
            {
                var loadOptions = new DataLoadOptions();
                loadOptions.LoadWith<Product>(p => p.Images);
                context.LoadOptions = loadOptions;

                return context.Products.Where(p => p.CategoryId == categoryId).ToList();
            }
        }

        public Product ProductById(int productId)
        {
            using (var context = new EcommerceDbDataContext(_connectionString))
            {
                var loadOptions = new DataLoadOptions();
                loadOptions.LoadWith<Product>(p => p.Images);
                context.LoadOptions = loadOptions;

                return context.Products.First(p => p.Id == productId);
            }
        }

        public Customer Customer(int id)
        {
            using (var context = new EcommerceDbDataContext(_connectionString))
            {
                return context.Customers.First(c => c.Id == id);
            }
        }

        public void CreatShoppingCart(ShoppingCart cart)
        {
            using (var context = new EcommerceDbDataContext(_connectionString))
            {
                context.ShoppingCarts.InsertOnSubmit(cart);
                context.SubmitChanges();
            }
        }

        public void AddItemToShoppingCart(ShoppingCartItem item)
        {
            using (EcommerceDbDataContext context = new EcommerceDbDataContext(_connectionString))
            {
                context.ShoppingCartItems.InsertOnSubmit(item);
                context.SubmitChanges();
            }
        }

        public void UpdateQuantityOfItemInShoppingCart(ShoppingCartItem item)
        {
            using (var context = new EcommerceDbDataContext(_connectionString))
            {
                context.ShoppingCartItems.Attach(item);
                context.Refresh(RefreshMode.KeepCurrentValues, item);
                context.SubmitChanges();
            }
        }

        public void UpdateQuantityOfItemInShoppingCart(int id, int quantity)
        {
            using (var context = new EcommerceDbDataContext(_connectionString))
            {
                context.ExecuteCommand("UPDATE ShoppingCartItems SET Quantity ={0} WHERE Id = {1}", quantity, id);
            }
        }

        public bool DoesShoppingCartItemExist(ShoppingCartItem item)
        {
            using (EcommerceDbDataContext context = new EcommerceDbDataContext(_connectionString))
            {
                return context.ShoppingCartItems.Any(i => i.ShoppingCartId == item.ShoppingCartId && i.ProductId == item.ProductId);
            }
        }

        public ShoppingCartItem CurrentShoppingCartItem(ShoppingCartItem item)
        {
            using (var context = new EcommerceDbDataContext(_connectionString))
            {
                return context.ShoppingCartItems.First(i => i.ShoppingCartId == item.ShoppingCartId && i.ProductId == item.ProductId);
            }
        }

        public int NumberOfItemsInCart(int shoppingCartId)
        {
            using (var context = new EcommerceDbDataContext(_connectionString))
            {
                if (context.ShoppingCartItems.Where(i => i.ShoppingCartId == shoppingCartId).Count() == 0)
                {
                    return 0;
                }
                return context.ShoppingCartItems.Where(i => i.ShoppingCartId == shoppingCartId).Sum(i => i.Quantity);
            }
        }

        public IEnumerable<ShoppingCartItem> AllItemsInShoppingCart(int shoppingCartId)
        {
            using (EcommerceDbDataContext context = new EcommerceDbDataContext(_connectionString))
            {
                DataLoadOptions loadOptions = new DataLoadOptions();
                loadOptions.LoadWith<ShoppingCartItem>(i => i.Product);
                context.LoadOptions = loadOptions;
                return context.ShoppingCartItems.Where(i => i.ShoppingCartId == shoppingCartId).ToList();
            }
        }

        public void DeleteItemFromCart(int id)
        {
            using (EcommerceDbDataContext context = new EcommerceDbDataContext(_connectionString))
            {
                context.ExecuteCommand("DELETE FROM ShoppingCartItems WHERE Id = {0}", id);
            }
        }

        public void AddCustomer(Customer c)
        {
            using (var context = new EcommerceDbDataContext(_connectionString))
            {
                context.Customers.InsertOnSubmit(c);
                context.SubmitChanges();
            }
        }

        public void UpdateCustomer(Customer c)
        {
            using (var context = new EcommerceDbDataContext(_connectionString))
            {
                context.Customers.Attach(c);
                context.Refresh(RefreshMode.KeepCurrentValues, c);
                context.SubmitChanges();
            }
        }

        public void CreateOrder(Order o)
        {
            using (var context = new EcommerceDbDataContext(_connectionString))
            {
                context.Orders.InsertOnSubmit(o);
                context.SubmitChanges();
            }
        }

        public Order GetOrderById(int orderId)
        {
            using (var context = new EcommerceDbDataContext(_connectionString))
            {

                return context.Orders.FirstOrDefault(o => o.OrderId == orderId);
            }
        }

        public void AddOrderDetails(List<OrderDetail> od)
        {
            using (var context = new EcommerceDbDataContext(_connectionString))
            {
                context.OrderDetails.InsertAllOnSubmit(od);
                context.SubmitChanges();
            }
        }
    }
}
