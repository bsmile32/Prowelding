using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Entities.DTO
{
    public class InventoryDTO //: Inventory
    {
        public int StockID { get; set; }
        public int ItemID { get; set; }
        public string ItemName { get; set; }
        public string ItemCode { get; set; }
        public string ItemDesc { get; set; }
        public double ItemPrice { get; set; }
        public int Amount { get; set; }
        public string AmountStr 
        {
            get
            {
                return Amount.ToString();
            }
        }
        public int TempID { get; set; }
        public string UpdatedBy { get; set; }
        public DateTime UpdatedDate { get; set; }
        public string UpdatedDateStr 
        {            
            get
            {
                try
                {
                    return UpdatedDate.ToString("dd/MM/yyyy", new System.Globalization.CultureInfo("en-US"));
                }
                catch (Exception) { return ""; }
                
            }
        }
        public string Serial { get; set; }
        public string Remark { get; set; }
        public string UnitName { get; set; }
        public string ItemTypeName { get; set; }
        public int Remaining { get; set; }
        public string RemainingStr
        {
            get
            {
                return Remaining.ToString();
            }
        }
    }
}