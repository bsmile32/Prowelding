using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Entities.DTO
{
    public class MasItemDTO : MasItem
    {
        public int Amount { get; set; }
        public string UnitName { get; set; }
        public string DistributorName { get; set; }
        public string ItemTypeName { get; set; }
        public int TID { get; set; }
        public int DetailOrder { get; set; }
        public string CanChange { get; set; }
        public string ItemDetail { get; set; }
    }
}