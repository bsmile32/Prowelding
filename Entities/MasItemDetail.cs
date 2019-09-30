using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Entities
{
    public class MasItemDetail
    {
        public int TID { get; set; }
        public int ItemID { get; set; }
        public int OrderDetail { get; set; }
        public string ItemDesc { get; set; }
        public string CanChange { get; set; }
        public string Active { get; set; }
        
    }
}