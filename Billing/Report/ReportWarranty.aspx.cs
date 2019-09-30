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
    public partial class ReportWarranty : BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                Session["ReportWarranty"] = null;
                DateTime dt = DateTime.Now;
                txtDateTo.Text = dt.ToString("dd/MM/yyyy");
                txtDateFrom.Text = dt.ToString("dd/MM/yyyy");
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
                List<ReportSaleDTO> lst = new List<ReportSaleDTO>();
                DateTime dateFrom = string.IsNullOrEmpty(txtDateFrom.Text) ? DateTime.MinValue : DateTime.ParseExact(txtDateFrom.Text, "dd/MM/yyyy", new System.Globalization.CultureInfo("en-US"));
                DateTime dateTo = string.IsNullOrEmpty(txtDateTo.Text) ? DateTime.MaxValue : DateTime.ParseExact(txtDateTo.Text, "dd/MM/yyyy", new System.Globalization.CultureInfo("en-US")).AddDays(1);
                string SaleNo = txtSaleNo.Text;
                string Serial = txtSn.Text;

                using (BillingEntities cre = new BillingEntities())
                {
                    lst = (from h in cre.TransSaleHeaders
                           join d in cre.TransSaleDetails on h.SaleHeaderID equals d.SaleHeaderID
                           join i in cre.MasItems on d.ItemID equals i.ItemID
                           where h.ReceivedDate >= dateFrom && h.ReceivedDate < dateTo
                           //date == DateTime.MinValue ? true : h.ReceivedDate.HasValue ? h.ReceivedDate.Value == date : true
                           select new ReportSaleDTO()
                           {
                               HeaderID = h.SaleHeaderID,
                               DetailID = d.SaleDetailID,
                               Tel = h.Tel,
                               CustomerName = h.DeliveryName,
                               Address = h.DeliverCountry + " " + h.DeliverProvince,
                               District = h.DeliverDistrict,
                               Country = h.DeliverCountry,
                               Province = h.DeliverProvince,
                               PostalCode = h.DeliverPostalCode,
                               SaleNumber = h.SaleNumber,
                               ItemCode = i.ItemCode,
                               ItemName = i.ItemName,
                               WarrantyDate = h.WarrantyDate,
                               SerialNumber = d.SerialNumber,
                           }).OrderBy(od => od.HeaderID).ToList();

                };

                if (lst != null && lst.Count > 0)
                {
                    //lstMod = ModDataFromSale(lst);
                    gv.DataSource = lst;
                    Session["ReportWarranty"] = lst;
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
                if (Session["ReportWarranty"] == null)
                {
                    BindData();
                }
                #region Variable
                List<ReportSaleDTO> lst = (List<ReportSaleDTO>)Session["ReportWarranty"];
                List<ReportSaleDTO> lstChoose = new List<ReportSaleDTO>();
                ReportSaleDTO o = new ReportSaleDTO();
                CheckBox cb;
                HiddenField hdd;
                Int32 ID = 0;
                #endregion
                foreach (GridViewRow item in gv.Rows)
                {
                    cb = (CheckBox)item.FindControl("chk");
                    if(cb != null && cb.Checked)
                    {
                        hdd = (HiddenField)item.FindControl("hddID");
                        if(hdd != null && !string.IsNullOrEmpty(hdd.Value))
                        {
                            ID = ToInt32(hdd.Value);
                            o = lst.FirstOrDefault(w => w.DetailID.Equals(ID));
                            if (o != null)
                            {
                                lstChoose.Add(o);
                            }
                        }
                    }
                }

                //เพิ่มเก็บเงินปลายทาง -- เพิ่มช่องโทรสับ
                if (lstChoose != null && lstChoose.Count > 0)
                {
                    ReportDeliveryExportPDF(lstChoose);
                }
                else
                {
                    ShowMessageBox("กรุณาเลือกรายการ.");
                    return;
                }
            }
            catch (Exception)
            {
                
            }
        }

        protected void ReportDeliveryExportPDF(List<ReportSaleDTO> lst)
        {
            MemoryStream ms = new MemoryStream();
            Document doc = new Document(new RectangleReadOnly(283.0f, 212.0f).Rotate(), 0.1f, 0.1f, 0.3f, 0.3f);
            //var output = new FileStream(Server.MapPath(filename), FileMode.Create);
            var writer = PdfWriter.GetInstance(doc, ms);
            doc.Open();
            try
            {
                #region Variable
                int FontSize = 11;
                BaseColor bc = new BaseColor(255, 255, 255);
                //string fontFamily = "C:/Windows/Fonts/Tahoma.ttf";
                string fontFamily = Server.MapPath("~") + "Fonts\\CSChatThai.ttf";
                BaseFont bf = BaseFont.CreateFont(fontFamily, BaseFont.IDENTITY_H, BaseFont.NOT_EMBEDDED);
                Font fontBold = new Font(bf, FontSize, iTextSharp.text.Font.BOLD);
                Font fontNormal = new Font(bf, FontSize);              
                PdfPTable table = new PdfPTable(2);
                table.WidthPercentage = 99;
                #endregion

                if (lst != null && lst.Count > 0)
                {
                    foreach (ReportSaleDTO item in lst)
                    {
                        AddTextToRow("  รุ่น/Model", item.ItemCode, fontNormal, fontBold, table);
                        AddTextToRow("  เลขเครื่อง/S/N", item.SerialNumber, fontNormal, fontBold, table);
                        AddTextToRow("  ชื่อ/Name", item.CustomerName, fontNormal, fontBold, table);
                        AddTextToRow("  ที่อยู่/Address", item.Address, fontNormal, fontBold, table);
                        AddTextToRow("  โทรศัพท์/Tel.", item.Tel, fontNormal, fontBold, table);
                        AddTextToRow("  วันที่ซื้อ/Date", item.WarrantyDateStr, fontNormal, fontBold, table);
                        AddTextToRow("   ---------------------------------------------------  ", "", fontNormal, fontNormal, table);

                        AddTextToRow("  รุ่น/Model", item.ItemCode, fontNormal, fontBold, table);
                        AddTextToRow("  เลขเครื่อง/S/N", item.SerialNumber, fontNormal, fontBold, table);
                        AddTextToRow("  ชื่อ/Name", item.CustomerName, fontNormal, fontBold, table);
                        AddTextToRow("  ที่อยู่/Address", item.Address, fontNormal, fontBold, table);
                        AddTextToRow("  โทรศัพท์/Tel.", item.Tel, fontNormal, fontBold, table);
                        AddTextToRow("  วันที่ซื้อ/Date", item.WarrantyDateStr, fontNormal, fontBold, table);
                        
                        doc.Add(table);
                        doc.NewPage();
                        table = new PdfPTable(2);
                        table.WidthPercentage = 99;
                    }

                    //doc.SetPageSize(new RectangleReadOnly(283.0f, 212.0f).Rotate());
                    //stamper.setRotateContents(false);
                    //pdfPCell.Rotation = 90;
                }
            }
            catch (Exception ex)
            {

            }
            finally
            {
                doc.Close();
                writer.Close();
            }

            string filename = "ReportWarranty_" + DateTime.Now.ToString("yyyyMMdd_HHmm");
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

        protected void AddTextToRow(string Text, string Text2, Font fontUse, Font fontUse2, PdfPTable table)
        {
            try
            {
                float[] widths = new float[] { 30f, 70f };
                PdfPCell cell1 = GetNewCell(false);
                PdfPCell cell2 = GetNewCell(false);
                cell1.AddElement(GetNewParag(Text, fontUse));
                if (string.IsNullOrEmpty(Text2))
                    cell1.Colspan = 2;
                else
                    cell2.AddElement(GetNewParag(Text2.Length > 40 ? Text2.Substring(0, 38) : Text2, fontUse2));

                PdfPCell[] cellAry = new PdfPCell[] { cell1, cell2 };
                PdfPRow row = new PdfPRow(cellAry);
                table.SetWidths(widths);
                table.Rows.Add(row);
            }
            catch (Exception ex)
            {
                
            }
        }
    }
}