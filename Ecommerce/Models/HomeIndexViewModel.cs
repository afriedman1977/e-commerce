using Ecommerce.data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Ecommerce.Models
{
    public class HomeIndexViewModel
    {
        public IEnumerable<Category> Allcategories { get; set; }
        public IEnumerable<Product> ProductsWithImages { get; set; }
        public string Message { get; set; }
    }
}