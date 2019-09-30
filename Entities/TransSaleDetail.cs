using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Entities
{
    public class TransSaleDetail
    {
        public int SaleDetailID { get; set; }
        public int SaleHeaderID { get; set; }
        public int ItemID { get; set; }
        public string SerialNumber { get; set; }
        public double? Amount { get; set; }
        public double? Discount { get; set; }
        public double? DiscountPer { get; set; }
        public double? ItemPrice { get; set; }
        public string ItemDetail { get; set; }
    }
}