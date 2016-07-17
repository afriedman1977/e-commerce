using Ecommerce.data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Ecommerce.Models
{
    public class CheckStatusViewModel
    {
        private Order _order;
        public CheckStatusViewModel()
        {
            
        }
        public CheckStatusViewModel(Order order)
        {
            _order = order;
        }
        public string StatusMessage()
        {
            if(_order == null)
            {
                return "Sorry Order Not Found Ckeck Your Order number and try again";
            }
            if (_order.Status == Status.Submitted)
            {
                return "Your Order Was Received We Will Get To It Shortly";
            }
            else if (_order.Status == Status.Viewed)
            {
                return "We Are Checking Over Your order And Will Begin Processing It Shortly";
            }
            else if (_order.Status == Status.Proccesing)
            {
                return "We Are Proccesing Your Order";
            }
            else if (_order.Status == Status.OutForDelivery)
            {
                return "Your Order Is Out For Delivery";
            }            
            return "Your Order Has Been Delivered";
        }
    }
}