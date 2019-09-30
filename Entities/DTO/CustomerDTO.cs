using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Entities.DTO
{
    public class CustomerDTO
    {
        public string CustomerName { get; set; }
        public string CustomerAddress { get; set; }
        public string CustomerAddress2 { get; set; }
        public string CustomerAddress3 { get; set; }

        public string CustomerAddressAll 
        {
            get
            {
                return CustomerAddress + " " + CustomerDistrict + " " + CustomerCountry + " " + CustomerProvince + " " + CustomerPostalCode;
            }
        }
        public string Tel { get; set; }
        public string CustomerDistrict { get; set; }
        public string CustomerCountry { get; set; }
        public string CustomerProvince { get; set; }
        public string CustomerPostalCode { get; set; }

        public string DeliveryName { get; set; }
        public string DeliverAdd { get; set; }
        public string DeliverDistrict { get; set; }
        public string DeliverCountry { get; set; }
        public string DeliverProvince { get; set; }
        public string DeliverPostalCode { get; set; }
        public string DeliverAddressAll
        {
            get
            {
                return DeliverAdd + " " + DeliverDistrict + " " + DeliverCountry + " " + DeliverProvince + " " + DeliverPostalCode;
            }
        }
    }
}