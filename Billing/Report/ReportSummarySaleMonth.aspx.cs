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
    public partial class ReportSummarySaleMonth : BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                Session["ReportSummarySaleMonth"] = null;
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

                #region Sale
                List<MasValueList> lst = new List<MasValueList>();
                using (BillingEntities cre = new BillingEntities())
                {
                    lst = cre.MasValueLists.Where(w => w.FUNC.Equals("SALENAME") && w.ISACTIVE.Equals("Y")).OrderBy(od => od.CODE).ToList();
                }
                if (lst != null && lst.Count > 0)
                {
                    ddlSaleName.DataSource = lst;
                    ddlSaleName.DataTextField = "Description";
                    ddlSaleName.DataValueField = "Code";
                }
                else
                {
                    ddlSaleName.DataSource = null;
                }

                ddlSaleName.Items.Insert(0, new System.Web.UI.WebControls.ListItem("", "0"));
                ddlSaleName.DataBind();
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
                List<ReportSummarySaleMonthDTO> lstResult = new List<ReportSummarySaleMonthDTO>();
                List<ReportSaleMonthDTO> lst = new List<ReportSaleMonthDTO>();
                List<MasValueList> lstVal = new List<MasValueList>();
                double xx = string.IsNullOrEmpty(txtXX.Text) ? 0 : Convert.ToDouble(txtXX.Text);
                double MM200 = string.IsNullOrEmpty(txtMM200.Text) ? 0 : Convert.ToDouble(txtMM200.Text);
                double MM225 = string.IsNullOrEmpty(txtMM225.Text) ? 0 : Convert.ToDouble(txtMM225.Text);
                string Month = ddlMonth.SelectedValue;
                string Year = ddlYear.SelectedValue;
                string date = "01/" + Month.PadLeft(2, '0') + "/" + Year;
                string SaleName = ddlSaleName.SelectedItem.Text;
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
                           && (string.IsNullOrEmpty(SaleName) ? true : h.SaleName.Equals(SaleName))
                           //&& dateFrom == DateTime.MinValue ? true : h.ReceivedDate.HasValue ? h.ReceivedDate.Value == date : true
                           select new ReportSaleMonthDTO()
                           {
                               HeaderID = h.SaleHeaderID,
                               DetailID = d.SaleDetailID,
                               Remark = h.Remark,
                               CustomerName = h.CustomerName,
                               //CustomerAddress = h.CustomerAddress,
                               //CustomerDistrict = h.CustomerDistrict,
                               //CustomerCountry = h.CustomerCountry,
                               //CustomerProvince = h.CustomerProvince,
                               //CustomerPostalCode = h.CustomerPostalCode,
                               ReceivedDate = h.ReceivedDate,
                               //WarrantyDate = h.WarrantyDate,
                               SaleNumber = h.SaleNumber,
                               SaleName = h.SaleName,
                               ItemCode = i.ItemCode,
                               //ItemName = i.ItemName,
                               //SerialNumber = d.SerialNumber,
                               //ItemDescription = i.ItemDesc,
                               ItemPrice = d.ItemPrice.Value,
                               Discount = d.Discount.Value,
                               Amount = d.Amount.Value,
                               //Tel = h.Tel,
                               //Address = h.CustomerAddress,
                               //District = h.CustomerDistrict,
                               //Country = h.CustomerCountry,
                               //Province = h.CustomerProvince,
                               //PostalCode = h.CustomerPostalCode,
                               PayTypeID = h.PayType,
                               BillTypeID = h.BillType,
                               VAT = xx,
                               MAXMIG200 = MM200,
                               MAXMIG225 = MM225,
                               //VisImgBtn = "true",
                               ConsignmentNo = h.ConsignmentNo,
                               AccountTransfer = h.AccountTransfer,
                               Installment = h.Installment,
                           }).OrderBy(od => od.ReceivedDate).ThenBy(od => od.SaleNumber).ThenBy(od => od.DetailID).ToList();
                };

                if (lst != null && lst.Count > 0)
                {
                    lstResult = ModData(lst, lstVal);
                    gv.DataSource = lstResult;
                    Session["ReportSummarySaleMonth"] = lstResult;
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

        protected List<ReportSummarySaleMonthDTO> ModData(List<ReportSaleMonthDTO> lstSource, List<MasValueList> lstVal)
        {
            List<ReportSummarySaleMonthDTO> lstResult = new List<ReportSummarySaleMonthDTO>();
            try
            {
                ReportSummarySaleMonthDTO oRPT = new ReportSummarySaleMonthDTO();
                MasValueList o = new MasValueList();
                int OldHeader = 0, CurrHeader = 0;
                int i = 0;
                string bill = "", pay = "";
                double Summary = 0;
                foreach (ReportSaleMonthDTO itemSource in lstSource)
                {
                    bill = !string.IsNullOrEmpty(itemSource.BillTypeID) ? itemSource.BillTypeID : "";
                    pay = !string.IsNullOrEmpty(itemSource.PayTypeID) ? itemSource.PayTypeID : "";
                    itemSource.BillType = bill.Equals("Vat") ? "Vat" : "Cash";
                    o = lstVal.FirstOrDefault(w => w.CODE.Equals(pay));
                    if(o != null)
                        itemSource.PayType = o.DESCRIPTION;
                    Summary = Summary + itemSource.Total;
                }
                lbSummary.Text = Summary.ToString("###,##0.00");

                foreach (ReportSaleMonthDTO itemSource in lstSource)
                {
                    CurrHeader = itemSource.HeaderID;
                    if (i > 0 && CurrHeader == OldHeader)
                    {
                        oRPT = lstResult.FirstOrDefault(w => w.HeaderID.Equals(CurrHeader));
                        if (oRPT != null)
                        {
                            oRPT.Total = oRPT.Total + itemSource.Total;
                            oRPT.TotalPrice = oRPT.TotalPrice + itemSource.TotalPrice;
                            oRPT.TotalExVat = oRPT.TotalExVat + itemSource.TotalExVat;
                            oRPT.VATAmount = oRPT.VATAmount + itemSource.VATAmount;
                        }
                    }
                    else
                    {
                        oRPT = new ReportSummarySaleMonthDTO();
                        oRPT.HeaderID = itemSource.HeaderID;
                        oRPT.SaleNumber = itemSource.SaleNumber;
                        oRPT.SaleName = itemSource.SaleName;
                        oRPT.ReceivedDate = itemSource.ReceivedDate;
                        oRPT.CustomerName = itemSource.CustomerName;
                        oRPT.Total = itemSource.Total;
                        oRPT.TotalPrice = itemSource.TotalPrice;
                        oRPT.TotalExVat = itemSource.TotalExVat;
                        oRPT.VATAmount = itemSource.VATAmount;
                        oRPT.BillType = itemSource.BillType;
                        oRPT.PayType = itemSource.PayType;
                        oRPT.AccountTransfer = itemSource.AccountTransfer;
                        oRPT.Installment = itemSource.Installment;
                        oRPT.ConsignmentNo = itemSource.ConsignmentNo;
                        oRPT.Remark = itemSource.Remark;
                        lstResult.Add(oRPT);
                    }
                    OldHeader = CurrHeader;
                    i++;
                }
                
            }
            catch (Exception ex)
            {
                
            }
            return lstResult;
        }

        #region Exporp Excel
        protected void btnExportExcel_Click(object sender, EventArgs e)
        {
            try
            {
                if (Session["ReportSummarySaleMonth"] == null)
                {
                    BindData();
                }

                List<ReportSummarySaleMonthDTO> lst = (List<ReportSummarySaleMonthDTO>)Session["ReportSummarySaleMonth"];
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

        protected void ExportExcel(List<ReportSummarySaleMonthDTO> lst)
        {
            try
            {
                int row = 3, i = 1;
                using (ExcelPackage package = new ExcelPackage())
                {
                    ExcelWorksheet ws = package.Workbook.Worksheets.Add("Report Summary Sale");
                    //Width
                    ws.Column(1).Width = 7;
                    ws.Column(2).Width = 10;
                    ws.Column(3).Width = 15;
                    ws.Column(4).Width = 30;
                    ws.Column(5).Width = 15;
                    ws.Column(6).Width = 15;
                    ws.Column(7).Width = 15;
                    ws.Column(8).Width = 15;
                    ws.Column(9).Width = 13;
                    ws.Column(10).Width = 20;
                    ws.Column(11).Width = 20;
                    ws.Column(12).Width = 15;
                    ws.Column(13).Width = 20;
                    ws.Column(14).Width = 15;
                    ws.Column(15).Width = 30;

                    ws.Column(1).Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                    ws.Column(5).Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Right;
                    ws.Column(6).Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Right;
                    ws.Column(7).Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Right;
                    ws.Column(8).Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Right;
                    //ws.Column(9).Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                    //ws.Column(10).Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                    //ws.Column(11).Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                    //ws.Column(12).Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                    //ws.Column(13).Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                    //ws.Row(1).Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;

                    ws.Column(5).Style.Numberformat.Format = "#,##0.00";
                    ws.Column(6).Style.Numberformat.Format = "#,##0.00";
                    ws.Column(7).Style.Numberformat.Format = "#,##0.00";
                    ws.Column(8).Style.Numberformat.Format = "#,##0.00";

                    //Header
                    ws.Row(1).Height = 20;
                    ws.Cells["A1:O1"].Merge = true;
                    ws.Cells["A1"].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                    ws.Cells["A1"].Style.Font.Size = 16f;
                    ws.Cells["A1"].Style.Font.Bold = true;
                    ws.Cells["A1"].Value = "รายงานสรุปยอดขาย";

                    ws.Row(2).Height = 18;
                    ws.Row(2).Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                    ws.Cells["A2:O2"].Style.Font.Size = 11f;
                    ws.Cells["A2"].Value = "No.";
                    ws.Cells["B2"].Value = "เลขที่";
                    ws.Cells["C2"].Value = "วันที่";
                    ws.Cells["D2"].Value = "ชื่อลูกค้า";
                    ws.Cells["E2"].Value = "รวม";
                    ws.Cells["F2"].Value = "ราคา";
                    ws.Cells["G2"].Value = "ราคาก่อน VAT";
                    ws.Cells["H2"].Value = "ภาษีมูลค่าเพิ่ม";
                    ws.Cells["I2"].Value = "ประเภทบิล";
                    ws.Cells["j2"].Value = "ช่องทางการชำระเงิน";
                    ws.Cells["K2"].Value = "บัญชีโอน";
                    ws.Cells["L2"].Value = "ผ่อน";
                    ws.Cells["M2"].Value = "Consignment No.";
                    ws.Cells["N2"].Value = "ผู้ขาย";
                    ws.Cells["O2"].Value = "หมายเหตุ";
                    ws.Cells["A1:O1"].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                    ws.Cells["A2:O2"].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;

                    foreach (ReportSummarySaleMonthDTO item in lst)
                    {
                        ws.Row(row).Height = 18;
                        ws.Cells["A" + row.ToString() + ":" + "O" + row.ToString()].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                        ws.Cells["A" + row.ToString()].Value = i.ToString();
                        ws.Cells["B" + row.ToString()].Value = item.SaleNumber;
                        ws.Cells["C" + row.ToString()].Value = item.ReceivedDateStr;
                        ws.Cells["D" + row.ToString()].Value = item.CustomerName;
                        ws.Cells["E" + row.ToString()].Value = item.Total;
                        ws.Cells["F" + row.ToString()].Value = item.TotalPrice;
                        ws.Cells["G" + row.ToString()].Value = item.TotalExVat;
                        ws.Cells["H" + row.ToString()].Value = item.VATAmount;
                        ws.Cells["I" + row.ToString()].Value = item.BillType;
                        ws.Cells["J" + row.ToString()].Value = item.PayType;
                        ws.Cells["K" + row.ToString()].Value = item.AccountTransfer;
                        ws.Cells["L" + row.ToString()].Value = item.Installment;
                        ws.Cells["M" + row.ToString()].Value = item.ConsignmentNo;
                        ws.Cells["N" + row.ToString()].Value = item.SaleName;
                        ws.Cells["O" + row.ToString()].Value = item.Remark;

                        i++;
                        row++;
                    }

                    ws.Cells["P3:P" + (row - 1).ToString()].Style.Border.Left.Style = ExcelBorderStyle.Thin;

                    package.SaveAs(Response.OutputStream);
                    string FileName = "ReportSummarySale_" + DateTime.Now.ToString("yyyyMMdd", new System.Globalization.CultureInfo("en-US"));
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
        #endregion

        #region Comment
        protected void btnExport_Click(object sender, EventArgs e)
        {
            try
            {
                //if (Session["ReportSummarySaleMonth"] == null)
                //{
                //    BindData();
                //}

                //List<ReportSaleMonthDTO> lst = (List<ReportSaleMonthDTO>)Session["ReportSummarySaleMonth"];
                //if(lst != null && lst.Count > 0)
                //{
                //    System.GC.Collect();
                //    ReportExportPDF(lst);
                //}
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

                //#region Variable
                //BaseColor bc = new BaseColor(255, 255, 255);
                //float[] widths;
                //string fontFamily = "C:/Windows/Fonts/Tahoma.ttf";
                //BaseFont bf = BaseFont.CreateFont(fontFamily, BaseFont.IDENTITY_H, BaseFont.NOT_EMBEDDED);
                //Font fontTitle = new Font(bf, 12, iTextSharp.text.Font.BOLD);
                //Font fontHeader = new Font(bf, 10);
                //Font fontGrid = new Font(bf, 9);                
                //PdfPTable table = new PdfPTable(1);
                //PdfPTable tableGrid = new PdfPTable(8);
                //PdfPCell[] cellAry;
                //PdfPRow row;
                //PdfPCell cell1 = new PdfPCell();
                //PdfPCell cell2 = new PdfPCell();
                //PdfPCell cell3 = new PdfPCell();
                //PdfPCell cell4 = new PdfPCell();
                //PdfPCell cell5 = new PdfPCell();
                //PdfPCell cell6 = new PdfPCell();
                //PdfPCell cell7 = new PdfPCell();
                //PdfPCell cell8 = new PdfPCell();
                //table.WidthPercentage = 95;
                //tableGrid.WidthPercentage = 95;
                //#endregion

                //cell1 = GetNewCell(false);
                //cell1.AddElement(GetNewParag("รายงานการขาย", fontTitle));
                //cellAry = new PdfPCell[] { cell1 };
                //row = new PdfPRow(cellAry);
                //table.Rows.Add(row);

                //cell1 = GetNewCell(false);
                //cell1.AddElement(GetNewParag(" ", fontHeader));
                //cellAry = new PdfPCell[] { cell1 };
                //row = new PdfPRow(cellAry);
                //table.Rows.Add(row);
                //doc.Add(table);

                //cell1 = GetNewCell(true);
                //cell2 = GetNewCell(true);
                //cell3 = GetNewCell(true);
                //cell4 = GetNewCell(true);
                //cell5 = GetNewCell(true);
                //cell6 = GetNewCell(true);
                //cell7 = GetNewCell(true);
                //cell8 = GetNewCell(true);

                //cell1.AddElement(GetNewParag("ชื่อลูกค้า", fontHeader, 1));
                //cell2.AddElement(GetNewParag("วันที่", fontHeader, 1));
                //cell3.AddElement(GetNewParag("เลขที่", fontHeader, 1));
                //cell4.AddElement(GetNewParag("สินค้า", fontHeader, 1));
                //cell5.AddElement(GetNewParag("จำนวน", fontHeader, 1));
                //cell6.AddElement(GetNewParag("รวม", fontHeader, 1));
                //cell7.AddElement(GetNewParag("ราคา", fontHeader, 1));
                //cell8.AddElement(GetNewParag("ภาษีมูลค่าเพิ่ม", fontHeader, 1));
                //cellAry = new PdfPCell[] { cell1, cell2, cell3, cell4, cell5, cell6, cell7, cell8 };
                //row = new PdfPRow(cellAry);
                //widths = new float[] { 50f, 20f, 20f, 65f, 15f, 25f, 25f, 30f };
                //tableGrid.SetWidths(widths);
                //tableGrid.Rows.Add(row);

                //foreach (ReportSaleMonthDTO item in lst)
                //{
                //    cell1 = GetNewCell(true);
                //    cell2 = GetNewCell(true);
                //    cell3 = GetNewCell(true);
                //    cell4 = GetNewCell(true);
                //    cell5 = GetNewCell(true);
                //    cell6 = GetNewCell(true);
                //    cell7 = GetNewCell(true);
                //    cell8 = GetNewCell(true);

                //    cell1.AddElement(GetNewParag(item.CustomerName, fontGrid));
                //    cell2.AddElement(GetNewParag(item.ReceivedDateStr, fontGrid, 1));
                //    cell3.AddElement(GetNewParag(item.SaleNumber, fontGrid, 1));
                //    cell4.AddElement(GetNewParag(item.ItemName, fontGrid));
                //    cell5.AddElement(GetNewParag(item.AmountStr, fontGrid, 1));
                //    cell6.AddElement(GetNewParag(item.TotalStr, fontGrid, 2));
                //    cell7.AddElement(GetNewParag(item.TotalExVatStr, fontGrid, 2));
                //    cell8.AddElement(GetNewParag(item.VATAmountStr, fontGrid, 2));
                //    cellAry = new PdfPCell[] { cell1, cell2, cell3, cell4, cell5, cell6, cell7, cell8 };
                //    row = new PdfPRow(cellAry);
                //    tableGrid.Rows.Add(row);                    
                //}
                //doc.Add(tableGrid);
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

        protected void imgbtnPrint_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                //string msg = "";
                //ImageButton imb = new ImageButton();
                //imb = (ImageButton)sender;
                //Int32 headerID = 0;
                //if (imb != null)
                //{
                //    headerID = ToInt32(imb.CommandArgument);   
                //    if (Header != null)
                //    {
                //        if (Session["ReportSummarySaleMonth"] == null)
                //        {
                //            BindData();
                //        }

                //        List<ReportSaleMonthDTO> lst = (List<ReportSaleMonthDTO>)Session["ReportSummarySaleMonth"];
                //        if (lst != null && lst.Count > 0)
                //        {
                //            lst = lst.Where(w => w.HeaderID.Equals(headerID)).ToList();
                //            System.GC.Collect();
                //            msg = ExportPDFManyBillVatFromMonthly(lst);
                //        }
                //        if (msg != "")
                //        {
                //            ShowMessageBox(msg);
                //        }
                //    }
                //}
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
                //ImageButton imb = new ImageButton();
                //CheckBox chk;
                //List<ReportSaleMonthDTO> lst = new List<ReportSaleMonthDTO>();
                //List<ReportSaleMonthDTO> temp = new List<ReportSaleMonthDTO>();
                //List<ReportSaleMonthDTO> lstSelect = new List<ReportSaleMonthDTO>();
                //Int32 headerID = 0;
                //if (Session["ReportSummarySaleMonth"] == null)
                //{
                //    BindData();
                //}

                //lst = (List<ReportSaleMonthDTO>)Session["ReportSummarySaleMonth"];
                //foreach (GridViewRow item in gv.Rows)
                //{
                //    chk = (CheckBox)item.FindControl("chk");
                //    imb = (ImageButton)item.FindControl("imgbtnPrint");
                //    if (chk != null && imb != null && chk.Checked)
                //    {
                //        headerID = ToInt32(imb.CommandArgument);
                //        temp = lst.Where(w => w.HeaderID.Equals(headerID)).ToList();
                //        if (temp != null)
                //            lstSelect.AddRange(temp);
                //    }
                //}

                //if (lstSelect != null && lstSelect.Count > 0)
                //{
                //    ExportPDFManyBillVatFromMonthly(lstSelect);
                //}
                //else
                //{
                //    ShowMessageBox("กรุณาเลือกรายการที่ต้องการ !!!");
                //}
                
            }
            catch (Exception ex)
            {
                ShowMessageBox(ex.Message);
            }
        }
        #endregion
    }
}