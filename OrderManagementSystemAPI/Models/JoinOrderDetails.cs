using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OrderManagementSystemAPI.Models
{
    public class JoinOrderDetails
    {
      public string  BuyerName { get; set; }
      public string ShippingAddress { get; set; }
        public string OrderID { get; set; }
        public string BuyerEmail { get; set; }
        public string OrderItemID { get; set; }
        public Nullable<int> OrderQuantity { get; set; }
        public string OrderStatus { get; set; }
        public string ProductImage { get; set; }
        public string ProductName { get; set; }
        public string ProductWeight { get; set; }
        public string ProductHeight { get; set; }
    }
}