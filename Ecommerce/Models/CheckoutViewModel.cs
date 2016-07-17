using Ecommerce.data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Ecommerce.Models
{
    public class CheckoutViewModel
    {
        public string Message { get; set; }
        public Customer Customer { get; set; }
    }
}