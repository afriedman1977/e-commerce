using Ecommerce.data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Ecommerce.Models
{
    public class CartModel
    {
        public ShoppingCartItem CartItem { get; set; }
        public decimal TotalPerItem { get; set; } 
    }
}