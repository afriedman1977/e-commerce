using Ecommerce.data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Ecommerce.Models
{
    public class AddProductViewModel
    {
        public IEnumerable<Category> AllCategories { get; set; }
    }
}