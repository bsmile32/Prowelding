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
    public partial class ReportSale : BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                Session["ReportSale"] = null;
                DateTime dt = DateTime.Now;
                txtDateTo.Text = dt.ToString("dd/MM/yyyy");
                txtDateFrom.Text = dt.ToString("dd/MM/yyyy");
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
                //List<TransSaleHeader> lstH = new List<TransSaleHeader>();
                List<ReportSaleDTO> lst = new List<ReportSaleDTO>();
                List<MasValueList> lstVal = new List<MasValueList>();
                DateTime dateFrom = string.IsNullOrEmpty(txtDateFrom.Text) ? DateTime.MinValue : DateTime.ParseExact(txtDateFrom.Text, "dd/MM/yyyy", new System.Globalization.CultureInfo("en-US"));
                DateTime dateTo = string.IsNullOrEmpty(txtDateTo.Text) ? DateTime.MaxValue : DateTime.ParseExact(txtDateTo.Text, "dd/MM/yyyy", new System.Globalization.CultureInfo("en-US")).AddDays(1);
                string AccName = txtCustName.Text;
                string Tel = txtTel.Text;

                using (BillingEntities cre = new BillingEntities())
                {
                    //lstH = cre.TransSaleHeaders.Where(w => w.SaleHeaderID > 3600).ToList();
                    lstVal = cre.MasValueLists.Where(w => w.FUNC.Equals("PAYMENT")).ToList();
                    lst = (from h in cre.TransSaleHeaders
                           join d in cre.TransSaleDetails on h.SaleHeaderID equals d.SaleHeaderID
                           join i in cre.MasItems on d.ItemID equals i.ItemID                           
                           where h.ReceivedDate >= dateFrom && h.ReceivedDate < dateTo
                           //&& dateFrom == DateTime.MinValue ? true : h.ReceivedDate.HasValue ? h.ReceivedDate.Value == date : true
                           select new ReportSaleDTO()
                           {
                               HeaderID = h.SaleHeaderID,
                               CustomerName = h.CustomerName,
                               ReceivedDate = h.ReceivedDate,
                               WarrantyDate = h.WarrantyDate,
                               SaleNumber = h.SaleNumber,
                               ItemCode = i.ItemCode,
                               ItemName = i.ItemName,
                               ItemPrice = d.ItemPrice.Value,
                               Discount = d.Discount.Value,
                               Amount = d.Amount.Value,
                               Tel = h.Tel,
                               Address = h.CustomerAddress,
                               District = h.CustomerDistrict,
                               Country = h.CustomerCountry,
                               Province = h.CustomerProvince,
                               PostalCode = h.CustomerPostalCode,
                               PayTypeID = h.PayType,
                               BillTypeID = h.BillType,
                               ConsignmentNo = h.ConsignmentNo,
                               SaleName = h.SaleName,
                               AccountTransfer = h.AccountTransfer,
                               Installment = h.Installment,
                           }).OrderBy(od => od.ReceivedDate).ThenBy(od => od.SaleNumber).ToList();
                };

                if (lst != null && lst.Count > 0)
                {
                    if(!string.IsNullOrEmpty(AccName))
                    {
                        lst = lst.Where(w => w.CustomerName.Contains(AccName)).ToList();
                    }
                    if (!string.IsNullOrEmpty(Tel))
                    {
                        lst = lst.Where(w => w.Tel.Contains(Tel)).ToList();
                    }
                    
                    ModData(lst, lstVal);
                    gv.DataSource = lst;
                    Session["ReportSale"] = lst;
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

        protected void ModData(List<ReportSaleDTO> lst, List<MasValueList> lstVal)
        {
            try
            {
                int OldHeader = 0, CurrHeader = 0;
                int i = 0;
                double Summary = 0;
                foreach (ReportSaleDTO item in lst)
                {
                    if (!string.IsNullOrEmpty(item.BillTypeID))
                        item.BillType = item.BillTypeID.Equals("Vat") ? "Vat" : "Cash";

                    if (!string.IsNullOrEmpty(item.PayTypeID))
                        item.PayType = lstVal.FirstOrDefault(w => w.CODE.Equals(item.PayTypeID)).DESCRIPTION;
                    CurrHeader = item.HeaderID;
                    //if (!string.IsNullOrEmpty(item.Tel))
                    //    item.CustomerName = item.CustomerName + "(" + item.Tel + ")";

                    if (i > 0 && CurrHeader == OldHeader)
                    {
                        item.CustomerName = "";                        
                        item.ReceivedDate = null;
                        item.SaleNumber = "";
                        item.Tel = "";   
                    }
                    OldHeader = CurrHeader;
                    Summary = Summary + item.Total;
                    i++;
                }

                lbSummary.Text = Summary.ToString("###,##0.00");
            }
            catch (Exception ex)
            {
                
            }
        }
        
        protected void btnExport_Click(object sender, EventArgs e)
        {
            try
            {
                if(Session["ReportSale"] == null)
                {
                    BindData();
                }

                List<ReportSaleDTO> lst = (List<ReportSaleDTO>)Session["ReportSale"];
                if(lst != null && lst.Count > 0)
                {
                    ReportExportPDF(lst);
                }
            }
            catch (Exception)
            {
                
            }
        }

        protected void ReportExportPDF(List<ReportSaleDTO> lst)
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
                string fontFamily = "C:/Windows/Fonts/Tahoma.ttf";
                BaseFont bf = BaseFont.CreateFont(fontFamily, BaseFont.IDENTITY_H, BaseFont.NOT_EMBEDDED);
                Font fontTitle = new Font(bf, 12, iTextSharp.text.Font.BOLD);
                Font fontHeader = new Font(bf, 10);
                Font fontGrid = new Font(bf, 9);                
                PdfPTable table = new PdfPTable(1);
                PdfPTable tableGrid = new PdfPTable(8);
                PdfPCell[] cellAry;
                PdfPRow row;
                PdfPCell cell1 = new PdfPCell();
                PdfPCell cell2 = new PdfPCell();
                PdfPCell cell3 = new PdfPCell();
                PdfPCell cell4 = new PdfPCell();
                PdfPCell cell5 = new PdfPCell();
                PdfPCell cell6 = new PdfPCell();
                PdfPCell cell7 = new PdfPCell();
                PdfPCell cell8 = new PdfPCell();
                table.WidthPercentage = 95;
                tableGrid.WidthPercentage = 95;
                #endregion

                cell1 = GetNewCell(false);
                cell1.AddElement(GetNewParag("รายงานการขาย", fontTitle));
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
                cell6 = GetNewCell(true);
                cell7 = GetNewCell(true);
                cell8 = GetNewCell(true);

                cell1.AddElement(GetNewParag("ชื่อลูกค้า", fontHeader, 1));
                cell2.AddElement(GetNewParag("วันที่", fontHeader, 1));
                cell3.AddElement(GetNewParag("เลขที่", fontHeader, 1));
                cell4.AddElement(GetNewParag("สินค้า", fontHeader, 1));
                cell5.AddElement(GetNewParag("จำนวน", fontHeader, 1));
                cell6.AddElement(GetNewParag("ราคา", fontHeader, 1));
                cell7.AddElement(GetNewParag("ส่วนลด", fontHeader, 1));
                cell8.AddElement(GetNewParag("รวม", fontHeader, 1));
                cellAry = new PdfPCell[] { cell1, cell2, cell3, cell4, cell5, cell6, cell7, cell8 };
                row = new PdfPRow(cellAry);
                widths = new float[] { 50f, 20f, 20f, 65f, 15f, 25f, 25f, 30f };
                tableGrid.SetWidths(widths);
                tableGrid.Rows.Add(row);

                foreach (ReportSaleDTO item in lst)
                {
                    cell1 = GetNewCell(true);
                    cell2 = GetNewCell(true);
                    cell3 = GetNewCell(true);
                    cell4 = GetNewCell(true);
                    cell5 = GetNewCell(true);
                    cell6 = GetNewCell(true);
                    cell7 = GetNewCell(true);
                    cell8 = GetNewCell(true);

                    cell1.AddElement(GetNewParag(item.CustomerName, fontGrid));
                    cell2.AddElement(GetNewParag(item.ReceivedDateStr, fontGrid, 1));
                    cell3.AddElement(GetNewParag(item.SaleNumber, fontGrid, 1));
                    cell4.AddElement(GetNewParag(item.ItemName, fontGrid));
                    cell5.AddElement(GetNewParag(item.AmountStr, fontGrid, 1));
                    cell6.AddElement(GetNewParag(item.ItemPriceStr, fontGrid, 2));
                    cell7.AddElement(GetNewParag(item.DiscountStr, fontGrid, 2));
                    cell8.AddElement(GetNewParag(item.TotalStr, fontGrid, 2));
                    cellAry = new PdfPCell[] { cell1, cell2, cell3, cell4, cell5, cell6, cell7, cell8 };
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

            string filename = "ReportSale_" + DateTime.Now.ToString("yyyyMMdd");
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

        protected void btnExportExcel_Click(object sender, EventArgs e)
        {
            try
            {
                if (Session["ReportSale"] == null)
                {
                    BindData();
                }

                List<ReportSaleDTO> lst = (List<ReportSaleDTO>)Session["ReportSale"];
                if (lst != null && lst.Count > 0)
                {
                    ExportExcel(lst);
                }
            }
            catch (Exception)
            {

            }
        }

        protected void ExportExcel(List<ReportSaleDTO> lst)
        {
            try
            {
                int row = 3, i = 1;
                using (ExcelPackage package = new ExcelPackage())
                {
                    ExcelWorksheet ws = package.Workbook.Worksheets.Add("Report Sale");
                    //Width
                    ws.Column(1).Width = 7;
                    ws.Column(2).Width = 20;
                    ws.Column(3).Width = 15;
                    ws.Column(4).Width = 10;
                    ws.Column(5).Width = 10;
                    ws.Column(6).Width = 10;
                    ws.Column(7).Width = 35;
                    ws.Column(8).Width = 20;
                    ws.Column(9).Width = 10;
                    ws.Column(10).Width = 15;
                    ws.Column(11).Width = 15;
                    ws.Column(12).Width = 20;
                    ws.Column(13).Width = 15;
                    ws.Column(14).Width = 15;
                    ws.Column(15).Width = 15;
                    ws.Column(16).Width = 15;
                    ws.Column(17).Width = 15;
                    ws.Column(18).Width = 15;
                    ws.Column(19).Width = 15;
                    ws.Column(20).Width = 15;
                    
                    ws.Column(1).Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                    ws.Column(8).Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Right;
                    ws.Column(9).Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Right;
                    ws.Column(10).Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Right;
                    ws.Column(11).Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Right;
                    ws.Column(12).Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Right;
                    ws.Column(13).Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                    ws.Column(14).Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                    ws.Column(15).Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                    ws.Column(16).Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                    ws.Row(1).Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;

                    ws.Column(14).Style.Numberformat.Format = "#,##0";
                    ws.Column(15).Style.Numberformat.Format = "#,##0.00";
                    ws.Column(16).Style.Numberformat.Format = "#,##0.00";
                    ws.Column(17).Style.Numberformat.Format = "#,##0.00";

                    //Header
                    ws.Row(1).Height = 20;
                    ws.Cells["A1:R1"].Merge = true;
                    ws.Cells["A1"].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                    ws.Cells["A1"].Style.Font.Size = 16f;
                    ws.Cells["A1"].Style.Font.Bold = true;
                    ws.Cells["A1"].Value = "รายงานการขาย";

                    ws.Row(2).Height = 18;
                    ws.Row(2).Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                    ws.Cells["A2:P2"].Style.Font.Size = 11f;
                    ws.Cells["A2"].Value = "No.";
                    ws.Cells["B2"].Value = "ชื่อลูกค้า";
                    ws.Cells["C2"].Value = "โทร";
                    ws.Cells["D2"].Value = "วันที่";
                    ws.Cells["E2"].Value = "วันที่รับประกัน";
                    ws.Cells["F2"].Value = "เลขที่";
                    ws.Cells["G2"].Value = "สินค้า";
                    ws.Cells["H2"].Value = "รหัส";
                    ws.Cells["I2"].Value = "จำนวน";
                    ws.Cells["J2"].Value = "ราคา";
                    ws.Cells["K2"].Value = "ส่วนลด";
                    ws.Cells["L2"].Value = "รวม";
                    ws.Cells["M2"].Value = "ประเภทบิล";
                    ws.Cells["N2"].Value = "ช่องทางการชำระเงิน";
                    ws.Cells["O2"].Value = "บัญชีโอน";
                    ws.Cells["P2"].Value = "ผ่อน";
                    ws.Cells["Q2"].Value = "Consignment No.";
                    ws.Cells["R2"].Value = "ผู้ขาย";
                    ws.Cells["A1:R1"].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                    ws.Cells["A2:R2"].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;

                    foreach (ReportSaleDTO item in lst)
                    {
                        ws.Row(row).Height = 18;
                        ws.Cells["A" + row.ToString() + ":" + "S" + row.ToString()].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                        ws.Cells["A" + row.ToString()].Value = i.ToString();
                        ws.Cells["B" + row.ToString()].Value = item.CustomerName;
                        ws.Cells["C" + row.ToString()].Value = item.Tel;
                        ws.Cells["D" + row.ToString()].Value = item.ReceivedDateStr;
                        ws.Cells["E" + row.ToString()].Value = item.WarrantyDateStr;
                        ws.Cells["F" + row.ToString()].Value = item.SaleNumber;
                        ws.Cells["G" + row.ToString()].Value = item.ItemName;
                        ws.Cells["H" + row.ToString()].Value = item.ItemCode;
                        ws.Cells["I" + row.ToString()].Value = item.Amount;
                        ws.Cells["J" + row.ToString()].Value = item.ItemPrice;
                        ws.Cells["K" + row.ToString()].Value = item.Discount;
                        ws.Cells["L" + row.ToString()].Value = item.Total;
                        ws.Cells["M" + row.ToString()].Value = item.BillType;
                        ws.Cells["N" + row.ToString()].Value = item.PayType;
                        ws.Cells["O" + row.ToString()].Value = item.AccountTransfer;
                        ws.Cells["P" + row.ToString()].Value = item.Installment;
                        ws.Cells["Q" + row.ToString()].Value = item.ConsignmentNo;
                        ws.Cells["R" + row.ToString()].Value = item.SaleName;
                        i++;
                        row++;
                    }

                    ws.Cells["S3:S" + (row - 1).ToString()].Style.Border.Left.Style = ExcelBorderStyle.Thin;

                    package.SaveAs(Response.OutputStream);
                    string FileName = "ReportSale_" + DateTime.Now.ToString("yyyyMMdd", new System.Globalization.CultureInfo("en-US"));
                    Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                    Response.AddHeader("content-disposition", "attachment;  filename=" + FileName + ".xlsx");
                    Response.Flush();
                    Response.SuppressContent = true;
                    HttpContext.Current.ApplicationInstance.CompleteRequest();
                };
            }
            catch (Exception ex)
            {

            }
        }
    }
}