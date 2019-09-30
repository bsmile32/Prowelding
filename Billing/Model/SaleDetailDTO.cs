using Billing.AppData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Billing.Model
{
    public class SaleDetailDTO : TransSaleDetail
    {
        public Int32 No { get; set; }
        public Int32 StockID { get; set; }
        public string AmountStr 
        {
            get
            {
                return Amount.HasValue ? Amount.Value.ToString("###,##0") : "0";
            }
        }
        public string DiscountStr
        {
            get
            {
                return Discount.HasValue ? Discount.Value > 0 ? Discount.Value.ToString("###,##0.00") : "" : "";
            }
        }
        //public string DiscountPerStr
        //{
        //    get
        //    {
        //        return DiscountPer.HasValue ? DiscountPer.Value > 0 ? DiscountPer.Value.ToString("###,##0.00") : "" : "";
        //    }
        //}
        public string ItemPriceStr
        {
            get
            {
                return ItemPrice.HasValue ? ItemPrice.Value.ToString("###,##0.00") : "0.00";
            }
        }
        public double Total
        {
            get
            {
                double result = 0 , d = 0;
                result = ((ItemPrice.HasValue ? ItemPrice.Value : 0) * Amount.Value);

                //if (DiscountPer.HasValue && DiscountPer.Value > 0)
                //{
                //    d = result * DiscountPer.Value / 100;
                //    result = result - d;
                //}
                //else //
                    if (Discount.HasValue && Discount.Value > 0)
                {
                    d = Discount.Value;
                    result = result - d;
                }
                 
                return result;
            }
        }
        public string TotalStr
        {
            get
            {
                return Total.ToString("###,##0.00");
            }
        }
        public string Status { get; set; }
        //public double ItemPrice { get; set; }
        public string ItemCode { get; set; }
        public string ItemName { get; set; }
        public string ItemDescription { get; set; }
        public double TotalItemPriceCalAmtDis
        {
            get
            {
                double result = 0;
                try
                {
                    if (ItemPrice > 0)
                    {
                        result = ((ItemPrice.HasValue ? ItemPrice.Value : 0) * (Amount.HasValue ? Amount.Value : 1)) - (Discount.HasValue ? Discount.Value : 0);
                    }
                }
                catch (Exception)
                {

                }
                return result;
            }
        }

        public double ItemPriceCalAmtDis
        {
            get
            {
                double result = 0;
                try
                {
                    result = TotalItemPriceCalAmtDis / (Amount.HasValue ? Amount.Value : 1);
                }
                catch (Exception)
                {

                }
                return result;
            }
        }
        public double VATAmount
        {
            get
            {
                double result = 0;
                try
                {
                    if (ItemPriceCalAmtDis > 0)
                    {
                        //result = (ItemPrice.HasValue ? ItemPrice.Value : 0) * 7 / 107;
                        result = ItemPriceCalAmtDis * 7 / 107;
                    }
                }
                catch (Exception)
                {
                    
                }
                return result;
            }
        }

        public double VATAmount3Digit
        {
            get
            {
                return Math.Round(VATAmount, 3);
            }
        }

        public double VATAmount2Digit
        {
            get
            {
                return Math.Round(VATAmount3Digit, 2);
            }
        }

        public double TotalVATAmount2Digit
        {
            get
            {
                return VATAmount2Digit * Amount.Value;
            }
        }

        public double ItemPriceExVAT
        {
            get
            {
                return ItemPriceCalAmtDis - VATAmount2Digit;
            }
        }

        public string ItemPriceExVATStr
        {
            get
            {
                return ItemPriceExVAT.ToString("###,##0.00");
            }
        }

        public double TotalExVat
        {
            get
            {
                double d = 0, result = 0;
                result = ItemPriceExVAT * Amount.Value;
                //if (Discount.HasValue && Discount.Value > 0)
                //{
                //    d = Discount.Value;
                //    result = result - d;
                //}
                return result;
            }
        }
        public string TotalExVatStr
        {
            get
            {
                return TotalExVat.ToString("###,##0.00");
            }
        }

        public double TotalIndVat
        {
            get
            {
                return (ItemPriceExVAT + VATAmount2Digit) * Amount.Value;                
            }
        }
        public string TotalIndVatStr
        {
            get
            {
                return TotalIndVat.ToString("###,##0.00");
            }
        }
    }
}