//------------------------------------------------------------------------------
// <auto-generated>
//    This code was generated from a template.
//
//    Manual changes to this file may cause unexpected behavior in your application.
//    Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace OMSDataAccessLayer
{
    using System;
    using System.Collections.Generic;
    
    public partial class Table_OrderDetails
    {
        public int ID { get; set; }
        public string OrderID { get; set; }
        public string BuyerID { get; set; }
        public string BuyerEmail { get; set; }
        public string OrderItemID { get; set; }
        public string ProductID { get; set; }
        public Nullable<int> OrderQuantity { get; set; }
        public string OrderStatus { get; set; }
    }
}
