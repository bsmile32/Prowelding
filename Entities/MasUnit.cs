using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Entities
{
    public class MasUnit
    {
        public int UnitID { get; set; }
        public string UnitCode { get; set; }
        public string UnitName { get; set; }
        public string Active { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public string UpdatedBy { get; set; }
        public DateTime UpdatedDate { get; set; }
    }
}