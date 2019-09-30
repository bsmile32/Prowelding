using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Entities
{
    public class MasItemType
    {
        public int ItemTypeID { get; set; }
        public string ItemTypeCode { get; set; }
        public string ItemTypeName { get; set; }
        public string Active { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public string UpdatedBy { get; set; }
        public DateTime UpdatedDate { get; set; }
    }
}