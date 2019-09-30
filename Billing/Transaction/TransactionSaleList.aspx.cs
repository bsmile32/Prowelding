using Billing.AppData;
using Billing.Model;
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

namespace Billing.Transaction
{
    public partial class TransactionSaleList : Billing.Common.BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                Session["TransItemList"] = null;
                DateTime dt = DateTime.Now;
                txtDateTo.Text = dt.ToString("dd/MM/yyyy");
                txtDateFrom.Text = dt.ToString("dd/MM/yyyy");
                BindDDL();
                BindData();
            }
        }

        protected void BindDDL()
        {
            try
            {
                List<MasItem> lst = new List<MasItem>();
                using (BillingEntities cre = new BillingEntities())
                {
                    lst = cre.MasItems.ToList();
                };

                if (lst != null)
                {
                    Session["TransItemList"] = lst;
                    ddlItem.DataSource = lst;
                    ddlItem.DataValueField = "ItemID";
                    ddlItem.DataTextField = "ItemName";
                }
                else
                {
                    Session["TransItemList"] = null;
                    ddlItem.DataSource = null;
                }

                ddlItem.DataBind();
                ddlItem.Items.Insert(0, "");
            }
            catch (Exception ex)
            {

            }
        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            try
            {
                //if (string.IsNullOrEmpty(txtCustName.Text) || string.IsNullOrEmpty(txtSN.Text) ||
                //    string.IsNullOrEmpty(txtTel.Text) || string.IsNullOrEmpty(txtDateFrom.Text) || string.IsNullOrEmpty(txtDateTo.Text))
                //{
                //    ShowMessageBox("กรุณาระบุเงื่อนไขการค้นหาอย่างน้อย 1 อย่าง.");
                //    return;
                //}
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
                string SN = txtSN.Text;
                string Tel = txtTel.Text;
                Int32 ItemID = 0;
                ItemID = ToInt32(ddlItem.SelectedItem.Value);

                using (BillingEntities cre = new BillingEntities())
                {
                    //lst = (from h in cre.TransSaleHeaders
                    //       join d in cre.TransSaleDetails on h.SaleHeaderID equals d.SaleHeaderID into dnull
                    //       from d in dnull.DefaultIfEmpty()
                    //       join i in cre.MasItems on d.ItemID equals i.ItemID into inull
                    //       from i in inull.DefaultIfEmpty()
                    //       where h.CustomerName.Contains(AccName)
                    //       //&& d.SerialNumber.Contains(SN)
                    //       && h.ReceivedDate >= dateFrom && h.ReceivedDate < dateTo
                    //       //date == DateTime.MinValue ? true : h.ReceivedDate.HasValue ? h.ReceivedDate.Value == date : true
                    //       select new SaleHeaderDTO()
                    //       {
                    //           SaleHeaderID = h.SaleHeaderID,
                    //           CustomerName = h.CustomerName,
                    //           ReceivedDate = h.ReceivedDate,
                    //           ReceivedBy = h.ReceivedBy,
                    //           SaleNumber = h.SaleNumber,
                    //           ItemCode = i.ItemCode,
                    //           ItemName = i.ItemName,
                    //           ItemID = i.ItemID,
                    //           dAmount = d.Amount,
                    //           ItemPrice = d.ItemPrice.HasValue ? d.ItemPrice.Value : 0,
                    //           Discount = d.Discount.HasValue ? d.Discount.Value : 0,
                    //           SerialNumber = d.SerialNumber,
                    //       }).ToList();

                    lst = (from d in cre.GetTransSaleList(dateFrom, dateTo)
                           select new SaleHeaderDTO()
                           {
                               SaleHeaderID = d.SaleHeaderID,
                               CustomerName = d.CustomerName,                                  
                               Tel = d.tel,
                               ReceivedDate = d.ReceivedDate,
                               ReceivedBy = d.ReceivedBy,
                               SaleNumber = d.SaleNumber,
                               ItemCode = d.ItemCode,
                               ItemName = d.ItemName,
                               ItemID = d.ItemID.HasValue ? d.ItemID.Value : 0,
                               dAmount = d.Amount,
                               ItemPrice = d.ItemPrice.HasValue ? d.ItemPrice.Value : 0,
                               Discount = d.Discount.HasValue ? d.Discount.Value : 0,
                               SerialNumber = d.SerialNumber,
                               BillType = d.BillType,
                           }).ToList();

                };

                if (lst != null && lst.Count > 0)
                {
                    if (AccName != "")
                        lst = lst.Where(w => w.CustomerName.Contains(AccName)).ToList();

                    if (Tel != "")
                        lst = lst.Where(w => !string.IsNullOrEmpty(w.Tel) && w.Tel.Contains(Tel)).ToList();

                    if(SN != "")
                        lst = lst.Where(w => !string.IsNullOrEmpty(w.SerialNumber) && w.SerialNumber.Contains(SN)).ToList();

                    if (ItemID != 0)                        
                        lst = lst.Where(w => w.ItemID.Equals(ItemID)).ToList();

                    if (lst != null && lst.Count > 0)
                    {
                        lstMod = ModDataFromSale(lst);
                        gv.DataSource = lstMod;
                    }

                    if (ItemID != 0 || SN != "")
                        gv.Columns[5].Visible = false;
                    else
                        gv.Columns[5].Visible = true;
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

        //protected List<SaleHeaderDTO> ModData(List<SaleHeaderDTO> lst)
        //{
        //    List<SaleHeaderDTO> result = new List<SaleHeaderDTO>();
        //    try
        //    {
        //        SaleHeaderDTO o = new SaleHeaderDTO();
        //        if(lst != null && lst.Count > 0)
        //        {
        //            string saleNumber = "", ItemCode = "";
        //            int i = 0, saleID = 0;
        //            double SaleAmt = 0;
        //            foreach (SaleHeaderDTO item in lst)
        //            {                        
        //                if (saleID != item.SaleHeaderID)
        //                {
        //                    if (i > 0)
        //                    {
        //                        ItemCode = ItemCode.Substring(0, ItemCode.Length - 2);
        //                        o.ItemCode = ItemCode;
        //                        o.SaleAmount = SaleAmt;
        //                        result.Add(o);

        //                        //Reset
        //                        SaleAmt = 0;
        //                        ItemCode = "";
        //                    }
                  
        //                    o = new SaleHeaderDTO();
        //                    o.SaleHeaderID = item.SaleHeaderID;
        //                    o.SaleNumber = item.SaleNumber;
        //                    o.CustomerName = item.CustomerName;
        //                    o.ReceivedDate = item.ReceivedDate;
        //                    o.ReceivedBy = item.ReceivedBy;
        //                    ItemCode = ItemCode + item.ItemCode + ", ";
        //                    SaleAmt = SaleAmt + item.DetailPrice;
        //                }
        //                else
        //                {
        //                    ItemCode = ItemCode + item.ItemCode + ", ";
        //                    SaleAmt = SaleAmt + item.DetailPrice;
        //                }                        

        //                saleID = item.SaleHeaderID;
        //                i++;
        //            }

        //            ItemCode = ItemCode.Substring(0, ItemCode.Length - 2);
        //            o.ItemCode = ItemCode;
        //            o.SaleAmount = SaleAmt;
        //            result.Add(o);
        //            result = result.OrderBy(od => od.SaleNumber).ToList();
        //        }
        //    }
        //    catch (Exception ex)
        //    {
                
        //    }
        //    return result;
        //}

        #region Image Button
        protected void imgbtnEdit_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                ImageButton imb = new ImageButton();
                imb = (ImageButton)sender;
                if (imb != null)
                {
                    string ID = imb.CommandArgument;
                    Response.Redirect("TransactionSales.aspx?ID=" + ID);
                }
            }
            catch (Exception ex)
            {

            }
        }

        protected void imgbtnDelete_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                ImageButton imb = new ImageButton();
                imb = (ImageButton)sender;
                if (imb != null)
                {
                    Int32 ID = ToInt32(imb.CommandArgument);
                    TransSaleHeader o = new TransSaleHeader();
                    using (BillingEntities cre = new BillingEntities())
                    {
                        o = cre.TransSaleHeaders.FirstOrDefault(w => w.SaleHeaderID.Equals(ID));
                        if (o != null)
                        {
                            cre.TransSaleHeaders.Remove(o);
                            cre.SaveChanges();                        
                        }
                    };
                }
                ShowMessageBox("ลบรายการสำเร็จ.");
                BindData();
            }
            catch (Exception ex)
            {
                
            }
        }

        protected void imgbtnPrint_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                string msg = "";
                ImageButton imb = new ImageButton();
                imb = (ImageButton)sender;
                Int32 headerID = 0;
                if (imb != null)
                {
                   
                    headerID = ToInt32(imb.CommandArgument);
                    //ExportExcel(headerID);
                    SaleHeaderDTO Header = new SaleHeaderDTO();
                    List<SaleDetailDTO> Detail = new List<SaleDetailDTO>();
                    Header = GetSaleHeaderFromDB(headerID);
                    Detail = GetSaleDetailFromDB(headerID);

                    #region Comment
                    //using (BillingEntities cre = new BillingEntities())
                    //{
                    //    Header = (from h in cre.TransSaleHeaders
                    //                where h.SaleHeaderID.Equals(headerID)
                    //                select new SaleHeaderDTO()
                    //                {
                    //                    SaleHeaderID = h.SaleHeaderID,
                    //                    CustomerName = h.CustomerName,
                    //                    CustomerAddress = h.CustomerAddress,
                    //                    CustomerDistrict = h.CustomerDistrict,
                    //                    CustomerCountry = h.CustomerCountry,
                    //                    CustomerProvince = h.CustomerProvince,
                    //                    CustomerPostalCode = h.CustomerPostalCode,
                    //                    //CustomerAddress2 = h.CustomerAddress2,
                    //                    //CustomerAddress3 = h.CustomerAddress3,
                    //                    ReceivedDate = h.ReceivedDate,
                    //                    ReceivedBy = h.ReceivedBy,
                    //                    SaleNumber = h.SaleNumber,
                    //                    Remark = h.Remark,
                    //                }).FirstOrDefault();

                    //    Detail = (from d in cre.TransSaleDetails
                    //                join i in cre.MasItems on d.ItemID equals i.ItemID
                    //                where d.SaleHeaderID.Equals(headerID)
                    //                select new SaleDetailDTO()
                    //                {
                    //                    SaleHeaderID = d.SaleHeaderID,
                    //                    SaleDetailID = d.SaleDetailID,
                    //                    ItemID = d.ItemID,
                    //                    ItemCode = i.ItemCode,
                    //                    ItemName = i.ItemName,
                    //                    ItemDescription = d.ItemDetail,
                    //                    Amount = d.Amount,
                    //                    ItemPrice = d.ItemPrice.HasValue ? d.ItemPrice.Value : 0,
                    //                    Discount = d.Discount,
                    //                    SerialNumber = d.SerialNumber,
                    //                }).ToList();
                    //};

                    //System.GC.Collect();
                    #endregion

                    if(Header != null)
                    {
                        System.GC.Collect();
                        if (imb.CommandName == "Cash")
                        {
                            msg = ExportPDFBillCash(Header, Detail);
                        }
                        else
                        {
                            //msg = ExportPDFBillVat(Header, Detail);
                            msg = ExportPDFBillVat2(Header, Detail);
                        }
                        if (msg != "")
                        {
                            ShowMessageBox(msg);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ShowMessageBox(ex.Message);
            }
        }

        #region Export Excel
        
        //protected void ExportExcel(Int32 headerID)
        //{
        //    try
        //    {
        //        int row = 3, i = 1;
        //        using (ExcelPackage package = new ExcelPackage())
        //        {
        //            ExcelWorksheet ws = package.Workbook.Worksheets.Add("Report");
        //            //Width
        //            ws.Column(1).Width = 7;
        //            ws.Column(2).Width = 15;
        //            ws.Column(3).Width = 15;
        //            ws.Column(4).Width = 15;
        //            ws.Column(5).Width = 15;
        //            ws.Column(6).Width = 25;

        //            ws.Column(1).Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
        //            ws.Column(2).Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Left;
        //            ws.Column(3).Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Left;
        //            ws.Column(4).Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
        //            ws.Column(5).Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Right;
        //            ws.Column(6).Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Left;
        //            ws.Row(1).Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;

        //            #region Form
        //            ws.Row(1).Height = 20;
        //            ws.Cells["A1:F1"].Merge = true;
        //            ws.Cells["A1"].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
        //            ws.Cells["A1"].Style.Font.Size = 16f;
        //            ws.Cells["A1"].Style.Font.Bold = true;
        //            ws.Cells["A1"].Value = "บริษัท โปร เวลดิ้ง แอนด์ ทูลส์ จำกัด";

        //            ws.Row(2).Height = 20;
        //            ws.Cells["A2:F2"].Merge = true;
        //            ws.Cells["A2"].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
        //            ws.Cells["A2"].Style.Font.Size = 16f;
        //            ws.Cells["A2"].Style.Font.Bold = true;
        //            ws.Cells["A2"].Value = "PRO WELDING & TOOLS CO., LTD.";

        //            ws.Row(5).Height = 18;
        //            ws.Cells["A5:F5"].Merge = true;
        //            ws.Cells["A5"].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
        //            ws.Cells["A5"].Style.Font.Size = 14f;
        //            ws.Cells["A5"].Style.Font.Bold = true;
        //            ws.Cells["A5"].Value = "ใบสำคัญรับ";

        //            ws.Row(6).Height = 18;
        //            ws.Cells["A6:F6"].Merge = true;
        //            ws.Cells["A6"].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
        //            ws.Cells["A6"].Style.Font.Size = 14f;
        //            ws.Cells["A6"].Style.Font.Bold = true;
        //            ws.Cells["A6"].Value = "RECEIVED VOUCHER";
        //            #endregion

        //            TransSaleHeader h = new TransSaleHeader();
        //            using (BillingEntities cre = new BillingEntities())
        //            {
        //                h = cre.TransSaleHeaders.FirstOrDefault(w => w.SaleHeaderID.Equals(headerID));
        //                if(h == null)
        //                {
        //                    ShowMessageBox("เกิดข้อผิดพลาด กรุณาติดต่อผู้ดูแลระบบ.");
        //                    return;
        //                }
        //                #region Header

        //                #endregion


        //            };
                    
        //            //List<ReportPaymentDTO> lst = (List<ReportPaymentDTO>)Session["reportPay"];
        //            //foreach (ReportPaymentDTO item in lst)
        //            //{
        //            //    ws.Row(row).Height = 18;
        //            //    ws.Cells["A" + row.ToString() + ":" + "F" + row.ToString()].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
        //            //    ws.Cells["A" + row.ToString()].Value = i.ToString();
        //            //    ws.Cells["B" + row.ToString()].Value = item.AccountName;
        //            //    ws.Cells["C" + row.ToString()].Value = item.WarehouseName;
        //            //    ws.Cells["D" + row.ToString()].Value = item.PayDateSTR;
        //            //    ws.Cells["E" + row.ToString()].Value = item.PayAmountSTR;
        //            //    ws.Cells["F" + row.ToString()].Value = item.PayDescription;
        //            //    i++;
        //            //    row++;
        //            //}

        //            //ws.Cells["A3:A" + (row - 1).ToString()].Style.Border.Left.Style = ExcelBorderStyle.Thin;
        //            //ws.Cells["B3:B" + (row - 1).ToString()].Style.Border.Left.Style = ExcelBorderStyle.Thin;
        //            //ws.Cells["C3:C" + (row - 1).ToString()].Style.Border.Left.Style = ExcelBorderStyle.Thin;
        //            //ws.Cells["D3:D" + (row - 1).ToString()].Style.Border.Left.Style = ExcelBorderStyle.Thin;
        //            //ws.Cells["E3:E" + (row - 1).ToString()].Style.Border.Left.Style = ExcelBorderStyle.Thin;
        //            //ws.Cells["F3:F" + (row - 1).ToString()].Style.Border.Left.Style = ExcelBorderStyle.Thin;
        //            //ws.Cells["F3:F" + (row - 1).ToString()].Style.Border.Right.Style = ExcelBorderStyle.Thin;

        //            package.SaveAs(Response.OutputStream);
        //            string FileName = "Report_" + DateTime.Now.ToString("yyyyMMdd", new System.Globalization.CultureInfo("en-US"));
        //            Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
        //            Response.AddHeader("content-disposition", "attachment;  filename=" + FileName + ".xlsx");
        //            Response.Flush();
        //            Response.SuppressContent = true;
        //            HttpContext.Current.ApplicationInstance.CompleteRequest();
        //        };
        //    }
        //    catch (Exception ex)
        //    {
                
        //    }
        //}
        #endregion

        #region Print 2 Types  
        //protected void PrintBillCash(string HeaderIDStr)
        //{
        //    try
        //    {

        //    }
        //    catch (Exception ex)
        //    {
        //        ShowMessageBox(ex.Message);
        //    }
        //}
        //protected void PrintBillVat(string HeaderIDStr)
        //{
        //    try
        //    {
        //        Int32 headerID = 0;
        //        if (!string.IsNullOrEmpty(HeaderIDStr))
        //        {
        //            headerID = ToInt32(HeaderIDStr);
        //            //ExportExcel(headerID);
        //            SaleHeaderDTO Header = new SaleHeaderDTO();
        //            List<SaleDetailDTO> Detail = new List<SaleDetailDTO>();
        //            using (BillingEntities cre = new BillingEntities())
        //            {
        //                Header = (from h in cre.TransSaleHeaders
        //                          where h.SaleHeaderID.Equals(headerID)
        //                          select new SaleHeaderDTO()
        //                          {
        //                              SaleHeaderID = h.SaleHeaderID,
        //                              CustomerName = h.CustomerName,
        //                              CustomerAddress = h.CustomerAddress,
        //                              CustomerDistrict = h.CustomerDistrict,
        //                              CustomerCountry = h.CustomerCountry,
        //                              CustomerProvince = h.CustomerProvince,
        //                              CustomerPostalCode = h.CustomerPostalCode,
        //                              //CustomerAddress2 = h.CustomerAddress2,
        //                              //CustomerAddress3 = h.CustomerAddress3,
        //                              ReceivedDate = h.ReceivedDate,
        //                              ReceivedBy = h.ReceivedBy,
        //                              SaleNumber = h.SaleNumber,
        //                              Remark = h.Remark,
        //                          }).FirstOrDefault();

        //                Detail = (from d in cre.TransSaleDetails
        //                          join i in cre.MasItems on d.ItemID equals i.ItemID
        //                          where d.SaleHeaderID.Equals(headerID)
        //                          select new SaleDetailDTO()
        //                          {
        //                              SaleHeaderID = d.SaleHeaderID,
        //                              SaleDetailID = d.SaleDetailID,
        //                              ItemID = d.ItemID,
        //                              ItemCode = i.ItemCode,
        //                              ItemName = i.ItemName,
        //                              ItemDescription = d.ItemDetail,
        //                              Amount = d.Amount,
        //                              ItemPrice = d.ItemPrice.HasValue ? d.ItemPrice.Value : 0,
        //                              Discount = d.Discount,
        //                              SerialNumber = d.SerialNumber,
        //                          }).ToList();
        //            };

        //            System.GC.Collect();
        //            ExportPDF(Header, Detail);
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        ShowMessageBox(ex.Message);
        //    }
        //}
        
        #endregion

        #endregion

    }
}