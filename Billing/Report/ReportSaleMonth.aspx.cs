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
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Billing.Report
{
    public partial class ReportSaleMonth : BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                Session["ReportSaleMonth"] = null;
                BindDDL();
                txtXX.Text = "7";
                txtMM200.Text = "0";
                txtMM225.Text = "0";
            }
        }

        protected void BindDDL()
        {
            try
            {
                DataTable dt = new DataTable();
                dt.Columns.Add("Text");
                dt.Columns.Add("Val");
                DataRow dr;

                #region Month
                DateTimeFormatInfo monthsInfo = DateTimeFormatInfo.GetInstance(null);
                for (int i = 1; i < 13; i++)
                {
                    dr = dt.NewRow();
                    dr["Text"] = monthsInfo.GetMonthName(i);
                    dr["Val"] = i.ToString();
                    dt.Rows.Add(dr);                    
                }
                if (dt != null && dt.Rows.Count > 0)
                {
                    ddlMonth.DataSource = dt;
                    ddlMonth.DataTextField = "Text";
                    ddlMonth.DataValueField = "Val";
                }
                else
                {
                    ddlMonth.DataSource = null;
                }
                                    
                ddlMonth.DataBind();
                ddlMonth.SelectedIndex = ddlMonth.Items.IndexOf(ddlMonth.Items.FindByValue(DateTime.Now.Month.ToString())); //ToInt32(obj.PayType);
                
                #endregion

                #region Year
                List<Int32> lstYear = new List<Int32>();
                using (BillingEntities cre = new BillingEntities())
                {
                    lstYear = cre.TransSaleHeaders.Select(s => s.ReceivedDate.HasValue ? s.ReceivedDate.Value.Year : 0).Distinct().OrderByDescending(od => od).ToList();                    
                }
                if (lstYear != null)
                {
                    ddlYear.DataSource = lstYear;
                    ddlYear.SelectedIndex = ddlYear.Items.IndexOf(ddlYear.Items.FindByValue(DateTime.Now.Year.ToString())); //ToInt32(obj.PayType);
                }
                else
                {
                    ddlYear.DataSource = null;
                }

                ddlYear.DataBind();
                #endregion
            }
            catch (Exception ex)
            {
                SendMailError(ex.Message, System.Reflection.MethodBase.GetCurrentMethod());
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
                List<ReportSaleMonthDTO> lst = new List<ReportSaleMonthDTO>();
                List<MasValueList> lstVal = new List<MasValueList>();
                double xx = string.IsNullOrEmpty(txtXX.Text) ? 0 : Convert.ToDouble(txtXX.Text);
                double MM200 = string.IsNullOrEmpty(txtMM200.Text) ? 0 : Convert.ToDouble(txtMM200.Text);
                double MM225 = string.IsNullOrEmpty(txtMM225.Text) ? 0 : Convert.ToDouble(txtMM225.Text);
                string Month = ddlMonth.SelectedValue;
                string Year = ddlYear.SelectedValue;
                string date = "01/" + Month.PadLeft(2, '0') + "/" + Year;
                DateTime dateFrom = DateTime.ParseExact(date, "dd/MM/yyyy", new System.Globalization.CultureInfo("en-US"));
                DateTime dateTo = dateFrom.AddMonths(1).AddSeconds(-1);
                xx = xx / 100;
                MM200 = MM200 / 100;
                MM225 = MM225 / 100;

                using (BillingEntities cre = new BillingEntities())
                {
                    lstVal = cre.MasValueLists.Where(w => w.FUNC.Equals("PAYMENT")).ToList();
                    lst = (from h in cre.TransSaleHeaders
                           join d in cre.TransSaleDetails on h.SaleHeaderID equals d.SaleHeaderID
                           join i in cre.MasItems on d.ItemID equals i.ItemID
                           where h.ReceivedDate >= dateFrom && h.ReceivedDate < dateTo
                           
                           //&& dateFrom == DateTime.MinValue ? true : h.ReceivedDate.HasValue ? h.ReceivedDate.Value == date : true
                           select new ReportSaleMonthDTO()
                           {
                               HeaderID = h.SaleHeaderID,
                               DetailID = d.SaleDetailID,
                               Remark = h.Remark,
                               CustomerName = h.CustomerName,
                               CustomerAddress = h.CustomerAddress,
                               CustomerDistrict = h.CustomerDistrict,
                               CustomerCountry = h.CustomerCountry,
                               CustomerProvince = h.CustomerProvince,
                               CustomerPostalCode = h.CustomerPostalCode,
                               ReceivedDate = h.ReceivedDate,
                               WarrantyDate = h.WarrantyDate,
                               SaleNumber = h.SaleNumber,
                               ItemCode = i.ItemCode,
                               ItemName = i.ItemName,
                               SerialNumber = d.SerialNumber,
                               ItemDescription = i.ItemDesc,
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
                               VAT = xx,
                               MAXMIG200 = MM200,
                               MAXMIG225 = MM225,
                               VisImgBtn = "true",
                               ConsignmentNo = h.ConsignmentNo,
                               AccountTransfer = h.AccountTransfer,
                               Installment = h.Installment,
                           }).OrderBy(od => od.ReceivedDate).ThenBy(od => od.SaleNumber).ThenBy(od => od.DetailID).ToList();
                };

                if (lst != null && lst.Count > 0)
                {                    
                    ModData(lst, lstVal);
                    gv.DataSource = lst;
                    Session["ReportSaleMonth"] = lst;
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

        protected void ModData(List<ReportSaleMonthDTO> lst, List<MasValueList> lstVal)
        {
            try
            {
                MasValueList o = new MasValueList();
                string bill = "", pay = "";
                int OldHeader = 0, CurrHeader = 0;
                int i = 0;
                double Summary = 0;
                foreach (ReportSaleMonthDTO item in lst)
                {
                    bill = !string.IsNullOrEmpty(item.BillTypeID) ? item.BillTypeID : "";
                    pay = !string.IsNullOrEmpty(item.PayTypeID) ? item.PayTypeID : "";
                    item.BillType = bill.Equals("Vat") ? "Vat" : "Cash";
                    o = lstVal.FirstOrDefault(w => w.CODE.Equals(pay));
                    if(o != null)
                        item.PayType = o.DESCRIPTION;
                    CurrHeader = item.HeaderID;
                    //if (!string.IsNullOrEmpty(item.Tel))
                    //    item.CustomerName = item.CustomerName + "(" + item.Tel + ")";

                    if (i > 0 && CurrHeader == OldHeader)
                    {
                        item.CustomerName = "";                        
                        item.ReceivedDate = null;
                        item.SaleNumber = "";
                        item.Tel = "";
                        item.VisImgBtn = "false";   
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
                if (Session["ReportSaleMonth"] == null)
                {
                    BindData();
                }

                List<ReportSaleMonthDTO> lst = (List<ReportSaleMonthDTO>)Session["ReportSaleMonth"];
                if(lst != null && lst.Count > 0)
                {
                    System.GC.Collect();
                    ReportExportPDF(lst);
                }
            }
            catch (Exception)
            {
                
            }
        }

        protected void ReportExportPDF(List<ReportSaleMonthDTO> lst)
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
                cell6.AddElement(GetNewParag("รวม", fontHeader, 1));
                cell7.AddElement(GetNewParag("ราคา", fontHeader, 1));
                cell8.AddElement(GetNewParag("ภาษีมูลค่าเพิ่ม", fontHeader, 1));
                cellAry = new PdfPCell[] { cell1, cell2, cell3, cell4, cell5, cell6, cell7, cell8 };
                row = new PdfPRow(cellAry);
                widths = new float[] { 50f, 20f, 20f, 65f, 15f, 25f, 25f, 30f };
                tableGrid.SetWidths(widths);
                tableGrid.Rows.Add(row);

                foreach (ReportSaleMonthDTO item in lst)
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
                    cell6.AddElement(GetNewParag(item.TotalStr, fontGrid, 2));
                    cell7.AddElement(GetNewParag(item.TotalExVatStr, fontGrid, 2));
                    cell8.AddElement(GetNewParag(item.VATAmountStr, fontGrid, 2));
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
                if (Session["ReportSaleMonth"] == null)
                {
                    BindData();
                }

                List<ReportSaleMonthDTO> lst = (List<ReportSaleMonthDTO>)Session["ReportSaleMonth"];
                if (lst != null && lst.Count > 0)
                {
                    System.GC.Collect();
                    ExportExcel(lst);
                }
            }
            catch (Exception)
            {

            }
        }

        protected void ExportExcel(List<ReportSaleMonthDTO> lst)
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
                    ws.Column(4).Width = 20;
                    ws.Column(5).Width = 15;
                    ws.Column(6).Width = 15;
                    ws.Column(7).Width = 15;
                    ws.Column(8).Width = 10;
                    ws.Column(9).Width = 10;
                    ws.Column(10).Width = 10;
                    ws.Column(11).Width = 10;
                    ws.Column(12).Width = 35;
                    ws.Column(13).Width = 20;
                    ws.Column(14).Width = 10;
                    ws.Column(15).Width = 15;
                    ws.Column(16).Width = 15;
                    ws.Column(17).Width = 15;
                    ws.Column(18).Width = 20;
                    ws.Column(19).Width = 15;
                    ws.Column(20).Width = 15;
                    ws.Column(21).Width = 15;
                    //ws.Column(21).Width = 15;

                    ws.Column(1).Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                    ws.Column(13).Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Right;
                    ws.Column(14).Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Right;
                    ws.Column(15).Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Right;
                    ws.Column(16).Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Right;
                    ws.Column(17).Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Right;
                    ws.Column(18).Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                    ws.Column(19).Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                    ws.Column(20).Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                    ws.Column(21).Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                    ws.Row(1).Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;

                    ws.Column(13).Style.Numberformat.Format = "#,##0";
                    ws.Column(14).Style.Numberformat.Format = "#,##0.00";
                    ws.Column(15).Style.Numberformat.Format = "#,##0.00";
                    ws.Column(16).Style.Numberformat.Format = "#,##0.00";
                    ws.Column(17).Style.Numberformat.Format = "#,##0.00";

                    //Header
                    ws.Row(1).Height = 20;
                    ws.Cells["A1:U1"].Merge = true;
                    ws.Cells["A1"].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                    ws.Cells["A1"].Style.Font.Size = 16f;
                    ws.Cells["A1"].Style.Font.Bold = true;
                    ws.Cells["A1"].Value = "รายงานการขาย";

                    ws.Row(2).Height = 18;
                    ws.Row(2).Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                    ws.Cells["A2:U2"].Style.Font.Size = 11f;
                    ws.Cells["A2"].Value = "No.";
                    ws.Cells["B2"].Value = "ชื่อลูกค้า";
                    ws.Cells["C2"].Value = "โทร";
                    ws.Cells["D2"].Value = "ที่อยู่";
                    ws.Cells["E2"].Value = "ตำบล";
                    ws.Cells["F2"].Value = "อำเภอ";
                    ws.Cells["G2"].Value = "จังหวัด";
                    ws.Cells["H2"].Value = "รหัสไปรษณีย์";
                    ws.Cells["I2"].Value = "วันที่";
                    ws.Cells["j2"].Value = "วันที่รับประกัน";
                    ws.Cells["K2"].Value = "เลขที่";
                    ws.Cells["L2"].Value = "สินค้า";
                    ws.Cells["M2"].Value = "รหัส";
                    ws.Cells["N2"].Value = "จำนวน";
                    ws.Cells["O2"].Value = "รวม";
                    ws.Cells["P2"].Value = "ราคา";
                    ws.Cells["Q2"].Value = "ราคาก่อน VAT";
                    ws.Cells["R2"].Value = "ภาษีมูลค่าเพิ่ม";
                    ws.Cells["S2"].Value = "ประเภทบิล";
                    ws.Cells["T2"].Value = "ช่องทางการชำระเงิน";
                    ws.Cells["U2"].Value = "บัญชีโอน";
                    ws.Cells["V2"].Value = "ผ่อน";
                    ws.Cells["W2"].Value = "Consignment No.";
                    ws.Cells["A1:W1"].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                    ws.Cells["A2:W2"].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;                    

                    foreach (ReportSaleMonthDTO item in lst)
                    {
                        ws.Row(row).Height = 18;
                        ws.Cells["A" + row.ToString() + ":" + "X" + row.ToString()].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                        ws.Cells["A" + row.ToString()].Value = i.ToString();
                        ws.Cells["B" + row.ToString()].Value = item.CustomerName;
                        ws.Cells["C" + row.ToString()].Value = item.Tel;
                        ws.Cells["D" + row.ToString()].Value = item.Address;
                        ws.Cells["E" + row.ToString()].Value = item.District;
                        ws.Cells["F" + row.ToString()].Value = item.Country;
                        ws.Cells["G" + row.ToString()].Value = item.Province;
                        ws.Cells["H" + row.ToString()].Value = item.PostalCode;
                        ws.Cells["I" + row.ToString()].Value = item.ReceivedDateStr;
                        ws.Cells["J" + row.ToString()].Value = item.WarrantyDateStr;
                        ws.Cells["K" + row.ToString()].Value = item.SaleNumber;
                        ws.Cells["L" + row.ToString()].Value = item.ItemName;
                        ws.Cells["M" + row.ToString()].Value = item.ItemCode;
                        ws.Cells["N" + row.ToString()].Value = item.Amount;
                        ws.Cells["O" + row.ToString()].Value = item.Total;
                        ws.Cells["P" + row.ToString()].Value = item.TotalPrice;
                        ws.Cells["Q" + row.ToString()].Value = item.TotalExVat;
                        ws.Cells["R" + row.ToString()].Value = item.VATAmount;
                        ws.Cells["S" + row.ToString()].Value = item.BillType;
                        ws.Cells["T" + row.ToString()].Value = item.PayType;
                        ws.Cells["U" + row.ToString()].Value = item.AccountTransfer;
                        ws.Cells["V" + row.ToString()].Value = item.Installment;
                        ws.Cells["W" + row.ToString()].Value = item.ConsignmentNo;
                        i++;
                        row++;
                    }

                    ws.Cells["X3:X" + (row - 1).ToString()].Style.Border.Left.Style = ExcelBorderStyle.Thin;

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
                    if (Header != null)
                    {
                        if (Session["ReportSaleMonth"] == null)
                        {
                            BindData();
                        }

                        List<ReportSaleMonthDTO> lst = (List<ReportSaleMonthDTO>)Session["ReportSaleMonth"];
                        if (lst != null && lst.Count > 0)
                        {
                            lst = lst.Where(w => w.HeaderID.Equals(headerID)).ToList();
                            System.GC.Collect();
                            msg = ExportPDFManyBillVatFromMonthly(lst);
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

        protected void btnPrint_Click(object sender, EventArgs e)
        {
            try
            {
                ImageButton imb = new ImageButton();
                CheckBox chk;
                List<ReportSaleMonthDTO> lst = new List<ReportSaleMonthDTO>();//(List<ReportSaleMonthDTO>)Session["ReportSaleMonth"];
                List<ReportSaleMonthDTO> temp = new List<ReportSaleMonthDTO>();
                List<ReportSaleMonthDTO> lstSelect = new List<ReportSaleMonthDTO>();
                Int32 headerID = 0;
                if (Session["ReportSaleMonth"] == null)
                {
                    BindData();
                }

                lst = (List<ReportSaleMonthDTO>)Session["ReportSaleMonth"];
                foreach (GridViewRow item in gv.Rows)
                {
                    chk = (CheckBox)item.FindControl("chk");
                    imb = (ImageButton)item.FindControl("imgbtnPrint");
                    if (chk != null && imb != null && chk.Checked)
                    {
                        headerID = ToInt32(imb.CommandArgument);
                        temp = lst.Where(w => w.HeaderID.Equals(headerID)).ToList();
                        if (temp != null)
                            lstSelect.AddRange(temp);
                    }
                }

                if (lstSelect != null && lstSelect.Count > 0)
                {
                    ExportPDFManyBillVatFromMonthly(lstSelect);
                }
                else
                {
                    ShowMessageBox("กรุณาเลือกรายการที่ต้องการ !!!");
                }
                
            }
            catch (Exception ex)
            {
                ShowMessageBox(ex.Message);
            }
        }
    }
}