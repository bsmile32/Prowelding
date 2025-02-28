﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Entities
{
    public class TransSaleHeader
    {
        public int SaleHeaderID { get; set; }
        public string CustomerName { get; set; }
        public string CustomerAddress { get; set; }
        public DateTime? ReceivedDate { get; set; }
        public string ReceivedBy { get; set; }
        public string SaleNumber { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public string UpdatedBy { get; set; }
        public DateTime UpdatedDate { get; set; }
        public string Active { get; set; }
        public string Remark { get; set; }
        public string COD { get; set; }
        public string CustomerAddress2 { get; set; }
        public string CustomerAddress3 { get; set; }
        public DateTime WarrantyDate { get; set; }
        public string CustomerAddressTemp { get; set; }
        public string Tel { get; set; }
        public string CustomerNameTemp { get; set; }
        public string DeliverAdd { get; set; }
        public string DeliverAdd2 { get; set; }
        public string DeliverAdd3 { get; set; }
        public string DeliveryName { get; set; }
        public string CustomerDistrict { get; set; }
        public string CustomerCountry { get; set; }
        public string CustomerProvince { get; set; }
        public string CustomerPostalCode { get; set; }
        public string DeliverDistrict { get; set; }
        public string DeliverCountry { get; set; }
        public string DeliverProvince { get; set; }
        public string DeliverPostalCode { get; set; }
        public string BillType { get; set; }
        public string PayType { get; set; }
        public string ConsignmentNo { get; set; }
        public string SaleName { get; set; }
        public string AccountTransfer { get; set; }
        public string TimeTransfer { get; set; }
    }
}