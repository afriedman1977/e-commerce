using Ecommerce.data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Ecommerce.Models
{
    public class AdminIndexViewModel
    {
        public IEnumerable<Order> PendingOrders { get; set; }
        public IEnumerable<Order> ProcessedOrders { get; set; }
        public IEnumerable<Order> OutForDeliveryOrders { get; set; }
    }
}