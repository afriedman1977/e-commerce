using Ecommerce.data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Ecommerce.Models
{
    public class CartViewModel
    {
        public List<CartModel> ProductsInCart { get; set; }        
        public decimal TotalForCart { get; set; }
        public Customer Customer { get; set; }
        public string Message { get; set; }
    }
}