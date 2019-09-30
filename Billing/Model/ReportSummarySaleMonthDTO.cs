using Billing.AppData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Billing.Model
{
    public class ReportSummarySaleMonthDTO
    {
        public Int32 HeaderID { get; set; }
        public Int32 DetailID { get; set; }
        public string CustomerName { get; set; }
        public string Remark { get; set; }
        public string CustomerAddress { get; set; }
        public string CustomerDistrict { get; set; }
        public string CustomerCountry { get; set; }
        public string CustomerProvince { get; set; }
        public string CustomerPostalCode { get; set; }
        public string Tel { get; set; }
        public DateTime? ReceivedDate { get; set; }
        public DateTime? WarrantyDate { get; set; }
        public string SaleNumber { get; set; }
        public string SaleName { get; set; }
        public string ItemCode { get; set; }
        public string ItemName { get; set; }
        public string ItemDescription { get; set; }
        public string SerialNumber { get; set; }
        public double ItemPrice { get; set; }
        public double Amount { get; set; }
        public double Total { get; set; }
        public double VATAmount { get; set; }
        public double TotalPrice { get; set; }
        public double TotalExVat { get; set; }
        public double TotalExVatAmount { get; set; }
        public double Discount { get; set; }
        public string ReceivedDateStr 
        {
            get
            {
                return ReceivedDate.HasValue ? ReceivedDate.Value.ToString("dd/MM/yyyy", new System.Globalization.CultureInfo("en-US")) : "";
            }
        }
        public string WarrantyDateStr
        {
            get
            {
                return WarrantyDate.HasValue ? WarrantyDate.Value.ToString("dd/MM/yyyy", new System.Globalization.CultureInfo("en-US")) : "";
            }
        }
        public string TotalStr
        {
            get
            {
                return Total.ToString("###,##0.00");
            }
        }
        public string VATAmountStr
        {
            get
            {
                return VATAmount.ToString("###,##0.00");
            }
        }
        public string TotalExVatStr
        {
            get
            {
                return TotalExVat.ToString("###,##0.00");
            }
        }
        public string TotalExVatAmountStr
        {
            get
            {
                return TotalExVatAmount.ToString("###,##0.00");
            }
        }
        public string TotalPriceStr
        {
            get
            {
                return TotalPrice.ToString("###,##0.00");
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

        public string Address { get; set; }
        public string District { get; set; }
        public string Country { get; set; }
        public string Province { get; set; }
        public string PostalCode { get; set; }
        public string BillType { get; set; }
        public string BillTypeID { get; set; }
        public string PayType { get; set; }
        public string PayTypeID { get; set; }
        public double VAT { get; set; }
        public string VisImgBtn { get; set; }
        public double MAXMIG200 { get; set; }
        public double MAXMIG225 { get; set; }
        public string ConsignmentNo { get; set; }
        public string AccountTransfer { get; set; }
        public string Installment { get; set; }
    }
}