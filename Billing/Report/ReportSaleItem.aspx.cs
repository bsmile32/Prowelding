using Billing.AppData;
using Billing.Common;
using Billing.Model;
using iTextSharp.text;
using iTextSharp.text.pdf;
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

namespace Billing.Report
{
    public partial class ReportSaleItem : BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                Session["ReportSaleItem"] = null;
                Session["ItemList"] = null;
                //BindDDL();
                BindData();
            }
        }
        protected void BindDDL()
        {
            try
            {
                //List<MasItem> lst = new List<MasItem>();
                //using (BillingEntities cre = new BillingEntities())
                //{
                //    lst = cre.MasItems.ToList();
                //};

                //if (lst != null)
                //{
                //    Session["ItemList"] = lst;
                //    ddlItem.DataSource = lst;
                //    ddlItem.DataValueField = "ItemID";
                //    ddlItem.DataTextField = "ItemName";
                //}
                //else
                //{
                //    Session["ItemList"] = null;
                //    ddlItem.DataSource = null;
                //}

                //ddlItem.DataBind();
                //ddlItem.Items.Insert(0, "");
            }
            catch (Exception ex)
            {

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
                List<ReportSaleItemDTO> lst = new List<ReportSaleItemDTO>();
                DateTime dateFrom = string.IsNullOrEmpty(txtDateFrom.Text) ? DateTime.MinValue : DateTime.ParseExact(txtDateFrom.Text, "dd/MM/yyyy", new System.Globalization.CultureInfo("en-US"));
                DateTime dateTo = string.IsNullOrEmpty(txtDateTo.Text) ? DateTime.MaxValue : DateTime.ParseExact(txtDateTo.Text + " 235959", "dd/MM/yyyy HHmmss", new System.Globalization.CultureInfo("en-US"));                
                //Int32 itemID = ToInt32(ddlMItem.SelectedItem.Value);
                string ItemCode = "", Group = "";                          
                //ItemCode = ddlItem.SelectedItem.Value;
                ItemCode = txtItemCode.Text;
                Group = chkShow.Checked ? "1" : "";

                using (BillingEntities cre = new BillingEntities())
                {

                    lst = (from d in cre.GetReportSaleItem(dateFrom, dateTo, ItemCode, Group)
                           select new ReportSaleItemDTO()
                           {
                               //HeaderID = h.SaleHeaderID,
                               //CustomerName = h.CustomerName,
                               //ReceivedDate = h.ReceivedDate,
                               //SaleNumber = h.SaleNumber,
                               ItemCode = d.ItemCode,
                               ItemName = d.ItemName,
                               ItemPrice = d.ItemPrice.Value,
                               Amount = d.sumAmt.Value,
                           }).OrderBy(od => od.ItemCode).ToList();


                    //lst = (from h in cre.TransSaleHeaders
                    //       join d in cre.TransSaleDetails on h.SaleHeaderID equals d.SaleHeaderID
                    //       join i in cre.MasItems on d.ItemID equals i.ItemID
                    //       where i.ItemCode.Contains(ItemCode)
                    //       && h.ReceivedDate > dateFrom && h.ReceivedDate < dateTo
                    //       //&& dateFrom == DateTime.MinValue ? true : h.ReceivedDate.HasValue ? h.ReceivedDate.Value == date : true
                    //       select new ReportSaleItemDTO()
                    //       {
                    //           //HeaderID = h.SaleHeaderID,
                    //           //CustomerName = h.CustomerName,
                    //           //ReceivedDate = h.ReceivedDate,
                    //           //SaleNumber = h.SaleNumber,
                    //           ItemCode = i.ItemCode,
                    //           ItemName = i.ItemName,
                    //           ItemPrice = d.ItemPrice.Value,
                    //           Discount = d.Discount.Value,
                    //           Amount = d.Amount.Value,
                    //       }).OrderBy(od => od.ItemCode).ToList();
                };

                if (Group == "1")
                {
                    gv.Columns[3].Visible = true;
                    gv.Columns[4].Visible = true;
                }
                else
                {
                    gv.Columns[3].Visible = false;
                    gv.Columns[4].Visible = false;
                }

                if (lst != null && lst.Count > 0)
                {
                    //ModData(lst);
                    gv.DataSource = lst;
                    Session["ReportSaleItem"] = lst;
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
        
        protected void btnExport_Click(object sender, EventArgs e)
        {
            try
            {
                if (Session["ReportSaleItem"] == null)
                {
                    BindData();
                }

                List<ReportSaleItemDTO> lst = (List<ReportSaleItemDTO>)Session["ReportSaleItem"];
                if(lst != null && lst.Count > 0)
                {
                    ReportExportPDF(lst);
                }
            }
            catch (Exception)
            {
                
            }
        }

        protected void ReportExportPDF(List<ReportSaleItemDTO> lst)
        {
            MemoryStream ms = new MemoryStream();
            Document doc = new Document(PageSize.A4.Rotate(), 3, 3, 7, 3);
            //var output = new FileStream(Server.MapPath(filename), FileMode.Create);
            var writer = PdfWriter.GetInstance(doc, ms);
            doc.Open();
            try
            {
                #region Variable
                BaseColor bc = new BaseColor(255, 255, 255);
                float[] widths;
                string header = "";
                string fontFamily = "C:/Windows/Fonts/Tahoma.ttf";
                BaseFont bf = BaseFont.CreateFont(fontFamily, BaseFont.IDENTITY_H, BaseFont.NOT_EMBEDDED);
                Font fontTitle = new Font(bf, 12, iTextSharp.text.Font.BOLD);
                Font fontHeader = new Font(bf, 10);
                Font fontGrid = new Font(bf, 9);                
                PdfPTable table = new PdfPTable(1);
                PdfPTable tableGrid = new PdfPTable(5);
                PdfPCell[] cellAry;
                PdfPRow row;
                PdfPCell cell1 = new PdfPCell();
                PdfPCell cell2 = new PdfPCell();
                PdfPCell cell3 = new PdfPCell();
                PdfPCell cell4 = new PdfPCell();
                PdfPCell cell5 = new PdfPCell();
                table.WidthPercentage = 95;
                tableGrid.WidthPercentage = 95;
                DateTime dateFrom = string.IsNullOrEmpty(txtDateFrom.Text) ? DateTime.MinValue : DateTime.ParseExact(txtDateFrom.Text, "dd/MM/yyyy", new System.Globalization.CultureInfo("en-US"));
                DateTime dateTo = string.IsNullOrEmpty(txtDateTo.Text) ? DateTime.MaxValue : DateTime.ParseExact(txtDateTo.Text, "dd/MM/yyyy", new System.Globalization.CultureInfo("en-US")).AddDays(1);                
                
                #endregion

                header = "รายงานการขายตามสินค้า : ";
                if (dateFrom == DateTime.MinValue && dateTo == DateTime.MaxValue)
                {
                    header = header + "ทั้งหมด";
                }
                else
                {
                    header = header + "ตั้งแต่วันที่ " + txtDateFrom.Text + " ถึง " + txtDateTo.Text;
                }
                cell1 = GetNewCell(false);
                cell1.AddElement(GetNewParag(header, fontTitle));
                cellAry = new PdfPCell[] { cell1 };
                row = new PdfPRow(cellAry);
                table.Rows.Add(row);

                cell1 = GetNewCell(false);
                cell1.AddElement(GetNewParag(" ", fontHeader));
                cellAry = new PdfPCell[] { cell1 };
                row = new PdfPRow(cellAry);
                table.Rows.Add(row);
                doc.Add(table);

                cell1 = GetNewCell(true);
                cell2 = GetNewCell(true);
                cell3 = GetNewCell(true);
                cell4 = GetNewCell(true);
                cell5 = GetNewCell(true);

                cell1.AddElement(GetNewParag("รหัสสินค้า", fontHeader, 1));
                cell2.AddElement(GetNewParag("สินค้า", fontHeader, 1));
                cell3.AddElement(GetNewParag("จำนวน", fontHeader, 1));
                cell4.AddElement(GetNewParag("ราคา", fontHeader, 1));
                cell5.AddElement(GetNewParag("รวม", fontHeader, 1));
                cellAry = new PdfPCell[] { cell1, cell2, cell3, cell4, cell5 };
                row = new PdfPRow(cellAry);
                widths = new float[] { 30f, 65f, 15f, 20f, 20f };
                tableGrid.SetWidths(widths);
                tableGrid.Rows.Add(row);

                foreach (ReportSaleItemDTO item in lst)
                {
                    cell1 = GetNewCell(true);
                    cell2 = GetNewCell(true);
                    cell3 = GetNewCell(true);
                    cell4 = GetNewCell(true);
                    cell5 = GetNewCell(true);

                    cell1.AddElement(GetNewParag(item.ItemCode, fontGrid));
                    cell2.AddElement(GetNewParag(item.ItemName, fontGrid));
                    cell3.AddElement(GetNewParag(item.AmountStr, fontGrid, 1));
                    cell4.AddElement(GetNewParag(item.ItemPriceStr, fontGrid, 2));
                    cell5.AddElement(GetNewParag(item.TotalStr, fontGrid, 2));
                    cellAry = new PdfPCell[] { cell1, cell2, cell3, cell4, cell5 };
                    row = new PdfPRow(cellAry);
                    tableGrid.Rows.Add(row);                    
                }
                doc.Add(tableGrid);
            }
            catch (Exception ex)
            {

            }
            finally
            {
                doc.Close();
                writer.Close();
            }

            string filename = "ReportSaleItem_" + DateTime.Now.ToString("yyyyMMdd");
            Response.Clear();
            Response.Buffer = false;
            Response.ContentEncoding = Encoding.UTF8;
            Response.HeaderEncoding = Encoding.UTF8;
            Response.AddHeader("content-disposition", "attachment;filename=" + filename + ".pdf");
            Response.ContentType = "application/pdf";
            Response.BinaryWrite(ms.ToArray());
            Response.Flush();
            Response.SuppressContent = true;
        }

        protected void imgbtnView_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                ModalPopupExtender1.Show();
                ImageButton imb = new ImageButton();
                DateTime dateFrom = string.IsNullOrEmpty(txtDateFrom.Text) ? DateTime.MinValue : DateTime.ParseExact(txtDateFrom.Text, "dd/MM/yyyy", new System.Globalization.CultureInfo("en-US"));
                DateTime dateTo = string.IsNullOrEmpty(txtDateTo.Text) ? DateTime.MaxValue : DateTime.ParseExact(txtDateTo.Text + " 235959", "dd/MM/yyyy HHmmss", new System.Globalization.CultureInfo("en-US"));                                
                string ItemCode = "";
                imb = (ImageButton)sender;
                List<ReportSaleItemDTO> lst = new List<ReportSaleItemDTO>();
                if(imb != null)
                {
                    ItemCode = imb.CommandArgument;

                    using (BillingEntities cre = new BillingEntities())
                    {
                        lst = (from d in cre.GetReportSaleItemItemCode(dateFrom, dateTo, ItemCode)
                               select new ReportSaleItemDTO()
                               {
                                   SaleHeaderID = d.SaleHeaderID,
                                   SaleNumber = d.SaleNumber,
                                   ItemCode = d.ItemCode,
                               }).OrderBy(od => od.ItemCode).ToList();
                    }

                    if (lst != null && lst.Count > 0)
                    {
                        //ModData(lst);
                        gvSale.DataSource = lst;
                    }
                    else
                        gvSale.DataSource = null;

                    gvSale.DataBind();   
                }
            }
            catch (Exception ex)
            {
                
            }
        }

        protected void imgbtnChoose_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                Int32 SaleID = 0;
                ImageButton imb = new ImageButton();
                imb = (ImageButton)sender;
                if (imb != null)
                {
                    SaleID = ToInt32(imb.CommandArgument);
                    Response.Redirect("~/Transaction/TransactionSales.aspx?ID=" + SaleID);
                }
            }
            catch (Exception ex)
            {

            }
        }
    }
}