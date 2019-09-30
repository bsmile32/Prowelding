
using OfficeOpenXml;
using OfficeOpenXml.Style;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DAL;
using Entities.DTO;

namespace Billing.Transaction
{
    public partial class TransactionCustomerList : Billing.Common.BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                DateTime dt = DateTime.Now;
                txtDateTo.Text = dt.ToString("dd/MM/yyyy");
                txtDateFrom.Text = dt.AddDays(-365).ToString("dd/MM/yyyy");
                BindData();
            }
        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            try
            {
                BindData();
            }
            catch (Exception ex)
            {
                SendMailError(ex.Message, System.Reflection.MethodBase.GetCurrentMethod());
            }
        }

        protected void BindData()
        {
            try
            {
                List<SaleHeaderDTO> lst = new List<SaleHeaderDTO>();
                List<SaleHeaderDTO> lstMod = new List<SaleHeaderDTO>();
                DateTime dateFrom = string.IsNullOrEmpty(txtDateFrom.Text) ? DateTime.MinValue : DateTime.ParseExact(txtDateFrom.Text, "dd/MM/yyyy", new System.Globalization.CultureInfo("en-US"));
                DateTime dateTo = string.IsNullOrEmpty(txtDateTo.Text) ? DateTime.MaxValue : DateTime.ParseExact(txtDateTo.Text, "dd/MM/yyyy", new System.Globalization.CultureInfo("en-US")).AddDays(1);
                //DateTime date = string.IsNullOrEmpty(txtDate.Text) ? DateTime.MinValue : DateTime.ParseExact(txtDate.Text, "dd/MM/yyyy", new System.Globalization.CultureInfo("en-US"));
                string AccName = txtCustName.Text;
                var dal = TransactionDal.Instance;
                lst = dal.GetSearchCustomer(dateFrom, dateTo, AccName);
                //using (BillingEntities cre = new BillingEntities())
                //{
                //    lst = (from d in cre.GetTransSaleList(dateFrom, dateTo, )
                //           select new SaleHeaderDTO()
                //           {
                //               SaleHeaderID = d.SaleHeaderID,
                //               CustomerName = d.CustomerName,                                  
                //               Tel = d.tel,
                //               ReceivedDate = d.ReceivedDate,
                //               ReceivedBy = d.ReceivedBy,
                //               SaleNumber = d.SaleNumber,
                //               ItemCode = d.ItemCode,
                //               ItemName = d.ItemName,
                //               ItemID = d.ItemID.HasValue ? d.ItemID.Value : 0,
                //               dAmount = d.Amount,
                //               ItemPrice = d.ItemPrice.HasValue ? d.ItemPrice.Value : 0,
                //               Discount = d.Discount.HasValue ? d.Discount.Value : 0,
                //               SerialNumber = d.SerialNumber,
                //               BillType = d.BillType,
                //           }).ToList();

                //};

                if (lst != null && lst.Count > 0)
                {
                    if (lst != null && lst.Count > 0)
                    {
                        lstMod = ModDataFromCustomerSale(lst);
                        lstMod = lstMod.OrderByDescending(od => od.ReceivedDate).ToList();
                        gv.DataSource = lstMod;
                    }

                }
                else
                    gv.DataSource = null;

                gv.DataBind();                

            }
            catch (Exception ex)
            {
                SendMailError(ex.Message, System.Reflection.MethodBase.GetCurrentMethod());
            }
        }

        protected List<SaleHeaderDTO> ModDataFromCustomerSale(List<SaleHeaderDTO> lst)
        {
            List<SaleHeaderDTO> result = new List<SaleHeaderDTO>();
            try
            {
                SaleHeaderDTO o = new SaleHeaderDTO();
                if (lst != null && lst.Count > 0)
                {
                    string ItemCode = "";
                    int i = 0, saleID = 0;
                    double SaleAmt = 0;
                    foreach (SaleHeaderDTO item in lst)
                    {
                        if (saleID != item.SaleHeaderID)
                        {
                            if (i > 0)
                            {
                                ItemCode = ItemCode.Substring(0, ItemCode.Length - 2);
                                o.ItemCode = ItemCode;
                                o.SaleAmount = SaleAmt;
                                result.Add(o);

                                //Reset
                                SaleAmt = 0;
                                ItemCode = "";
                            }

                            o = new SaleHeaderDTO();
                            o.SaleHeaderID = item.SaleHeaderID;
                            o.SaleNumber = item.SaleNumber;
                            o.CustomerName = item.CustomerName;
                            o.Tel = item.Tel;
                            o.CustomerAddress = item.CustomerAddress;
                            o.CustomerDistrict = item.CustomerDistrict;
                            o.CustomerCountry = item.CustomerCountry;
                            o.CustomerProvince = item.CustomerProvince;
                            o.CustomerPostalCode = item.CustomerPostalCode;
                            //o.CustomerAddress2 = item.CustomerAddress2;
                            //o.CustomerAddress3 = item.CustomerAddress3;
                            o.ReceivedDate = item.ReceivedDate;
                            o.ReceivedBy = item.ReceivedBy;
                            o.COD = item.COD;
                            o.dAmount = item.dAmount;
                            o.BillType = string.IsNullOrEmpty(item.BillType) ? "" : item.BillType;
                            ItemCode = ItemCode + item.ItemCode + ", ";
                            SaleAmt = SaleAmt + item.DetailPrice;
                        }
                        else
                        {
                            ItemCode = ItemCode + item.ItemCode + ", ";
                            SaleAmt = SaleAmt + item.DetailPrice;
                        }

                        saleID = item.SaleHeaderID;
                        i++;
                    }

                    ItemCode = ItemCode.Substring(0, ItemCode.Length - 2);
                    o.ItemCode = ItemCode;
                    o.SaleAmount = SaleAmt;
                    result.Add(o);
                    result = result.OrderBy(od => od.SaleNumber).ToList();
                }
            }
            catch (Exception ex)
            {

            }
            return result;
        }
    }
}