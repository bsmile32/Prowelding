using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Entities.DTO
{
    public class ReportSaleItemDTO
    {
        public Int32 SaleHeaderID { get; set; }
        public string SaleNumber { get; set; }
        public string ItemCode { get; set; }
        public string ItemName { get; set; }
        public double ItemPrice { get; set; }
        public double Amount { get; set; }
        public double Total
        {
            get
            {
                double d = 0, result = 0;
                result = ItemPrice * Amount;
                if (Discount > 0)
                {
                    d = Discount;
                    result = result - d;
                }
                return result;
            }
        }
        public double VATAmount
        {
            get
            {
                double result = 0;
                if (Total > 0)
                {
                    result = Total * 7 / 100;
                }
                return result;
            }
        }
        public double Discount { get; set; }
        //public string ReceivedDateStr 
        //{
        //    get
        //    {
        //        return ReceivedDate.HasValue ? ReceivedDate.Value.ToString("dd/MM/yyyy", new System.Globalization.CultureInfo("en-US")) : "";
        //    }
        //}
        public string TotalStr
        {
            get
            {
                return Total.ToString("###,##0.00");
            }
        }
        public string DiscountStr
        {
            get
            {
                return Discount.ToString("###,##0.00");
            }
        }
        public string ItemPriceStr
        {
            get
            {
                return ItemPrice.ToString("###,##0.00");
            }
        }
        public string AmountStr
        {
            get
            {
                return Amount.ToString("###,##0");
            }
        }
    }
}