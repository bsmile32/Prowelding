using Billing.AppData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Billing.Model
{
    public class SaleHeaderDTO : TransSaleHeader, ICloneable
    {
        public string ReceivedDateStr 
        {
            get
            {
                return ReceivedDate.HasValue ? ReceivedDate.Value.ToString("dd/MM/yyyy", new System.Globalization.CultureInfo("en-US")) : "";
            }
        }
        public string SerialNumber { get; set; }
        public Int32 ItemID { get; set; }
        public string ItemCode { get; set; }
        public string ItemName { get; set; }
        public double? dAmount { get; set; }
        public double? ItemPrice { get; set; }
        public double? Discount { get; set; }
        public double DetailPrice
        {
            get
            {
                double d = 0, result = 0, amt = 0, iPrice = 0;
                amt = dAmount.HasValue ? dAmount.Value : 0;
                iPrice = ItemPrice.HasValue ? ItemPrice.Value : 0;
                result = iPrice * amt;
                if (Discount.HasValue && Discount.Value > 0)
                {
                    d = Discount.Value;
                    result = result - d;
                }
                return result;
            }
        }

        public string DetailPriceStr
        {
            get
            {
                return DetailPrice.ToString("###,##0.00");
            }
        }

        public double SaleAmount { get; set; }

        public string SaleAmountStr
        {
            get
            {
                return SaleAmount.ToString("###,##0.00");
            }
        }

        #region Delivery
        public string Name
        {
            get
            {
                string result = "";
                try
                {
                    if (!string.IsNullOrEmpty(CustomerName))
                    {
                        string[] spl = CustomerName.Split('(');
                        if (spl != null)
                        {
                            result = spl[0];
                        }
                    }
                }
                catch (Exception ex)
                {

                }
                return result;
            }
        }
        public string Tell
        {            
            get
            {
                string result = "";
                try 
	            {	        
		            if(!string.IsNullOrEmpty(CustomerName))
                    {
                        string[] spl = CustomerName.Split('(');
                        if(spl != null)
                        {
                            result = spl[1].Substring(0 ,spl[1].IndexOf(")"));
                        }
                    }
	            }
	            catch (Exception ex)
	            {

	            }
                return result;
            }
        }

        public string Add1
        {
            get
            {
                string result = "";
                try
                {
                    if (!string.IsNullOrEmpty(CustomerAddress))
                    {
                        int idx1 = CustomerAddress.IndexOf("ตำบล");
                        if (idx1 <= 0)
                        {
                            idx1 = CustomerAddress.IndexOf("แขวง");
                        }

                        if (idx1 <= 0)
                        {
                            idx1 = CustomerAddress.IndexOf("อำเภอ");
                        }

                        if (idx1 <= 0)
                        {
                            idx1 = CustomerAddress.IndexOf("เขต");
                        }

                        result = CustomerAddress.Substring(0, idx1);
                        result = result.Replace("\\r", "").Replace("\\n", "");
                    }
                }
                catch (Exception ex)
                {

                }
                return result;
            }
        }

        public string Add2
        {
            get
            {
                string result = "";
                try
                {
                    if (!string.IsNullOrEmpty(CustomerAddress))
                    {
                        int idx1 = CustomerAddress.IndexOf("ตำบล");
                        if (idx1 <= 0)
                        {
                            idx1 = CustomerAddress.IndexOf("แขวง");
                        }

                        int idx2 = CustomerAddress.IndexOf("จังหวัด");
                        if (idx2 <= 0)
                        {
                            idx2 = CustomerAddress.IndexOf("กรุงเทพ");
                        }
                        result = CustomerAddress.Substring(idx1, idx2 - idx1);
                        result = result.Replace("\\r", "").Replace("\\n", "");
                    }
                }
                catch (Exception ex)
                {

                }
                return result;
            }
        }

        public string Add3
        {
            get
            {
                string result = "";
                try
                {
                    if (!string.IsNullOrEmpty(CustomerAddress))
                    {
                        int idx1 = CustomerAddress.IndexOf("จังหวัด");
                        if (idx1 <= 0)
                        {
                            idx1 = CustomerAddress.IndexOf("กรุงเทพ");
                        }
                        result = CustomerAddress.Substring(idx1);
                        result = result.Replace("\r", "").Replace("\n", "");
                    }
                }
                catch (Exception ex)
                {

                }
                return result;
            }
        }

        public string AddressAll
        {
            get
            {
                string result = "";
                try
                {
                    //result = CustomerAddress + " " + CustomerAddress2 + " " + CustomerAddress3;
                    result = CustomerAddress + " " + CustomerDistrict + " " + CustomerCountry + " " + CustomerProvince + " " + CustomerPostalCode;
                }
                catch (Exception ex)
                {

                }
                return result;
            }
        }

        public object Clone()
        {
            return this.MemberwiseClone();
        }
        #endregion
    }
}