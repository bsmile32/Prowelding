using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Entities
{
    public class TransStock
    {
        public int StockID { get; set; }
        public int ItemID { get; set; }
        public string Serial { get; set; }
        public int SaleHeaderID { get; set; }
        public int SaleDetailID { get; set; }
        public string Active { get; set; }
    }
}