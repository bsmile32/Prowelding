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
    public partial class ReportSaleDelivery : BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                Session["ReportSaleDeli"] = null;
                DateTime dt = DateTime.Now;
                txtDateTo.Text = dt.ToString("dd/MM/yyyy");
                txtDateFrom.Text = dt.ToString("dd/MM/yyyy");
                BindData();
                BindDDL();
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

        protected void BindDDL()
        {
            try
            {
                List<MasSender> lst = new List<MasSender>();
                using (BillingEntities cre = new BillingEntities())
                {
                    lst = cre.MasSenders.ToList();
                };

                if (lst != null && lst.Count > 0)
                {
                    ddlSender.DataSource = lst;
                    ddlSender.DataValueField = "SenderID";
                    ddlSender.DataTextField = "SenderName";
                    Session["ReportSaleDeliDDL"] = lst;
                }
                else
                {
                    ddlSender.DataSource = null;
                    Session["ReportSaleDeliDDL"] = null;
                }                    

                ddlSender.DataBind();
            }
            catch (Exception ex)
            {
                
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
                
                using (BillingEntities cre = new BillingEntities())
                {
                    lst = (from h in cre.TransSaleHeaders
                           join d in cre.TransSaleDetails on h.SaleHeaderID equals d.SaleHeaderID
                           join i in cre.MasItems on d.ItemID equals i.ItemID
                           where h.ReceivedDate >= dateFrom && h.ReceivedDate < dateTo
                           //date == DateTime.MinValue ? true : h.ReceivedDate.HasValue ? h.ReceivedDate.Value == date : true
                           select new SaleHeaderDTO()
                           {
                               SaleHeaderID = h.SaleHeaderID,
                               Tel = h.Tel,
                               CustomerName = h.DeliveryName,
                               CustomerAddress = h.DeliverAdd,
                               CustomerDistrict = h.DeliverDistrict,
                               CustomerCountry = h.DeliverCountry,
                               CustomerProvince = h.DeliverProvince,
                               CustomerPostalCode = h.DeliverPostalCode,
                               //CustomerAddress2 = h.DeliverAdd2,
                               //CustomerAddress3 = h.DeliverAdd3,
                               //CustomerName = h.CustomerName,
                               //CustomerAddress = h.CustomerAddress,
                               //CustomerAddress2 = h.CustomerAddress2,
                               //CustomerAddress3 = h.CustomerAddress3,
                               //DeliverAdd = h.DeliverAdd,
                               //DeliverAdd2 = h.DeliverAdd2,
                               //DeliverAdd3 = h.DeliverAdd3,
                               SaleNumber = h.SaleNumber,
                               ItemCode = i.ItemCode,
                               ItemName = i.ItemName,
                               dAmount = d.Amount,
                               ItemPrice = d.ItemPrice.HasValue ? d.ItemPrice.Value : 0,
                               Discount = d.Discount.HasValue ? d.Discount.Value : 0,
                               COD = h.COD,
                           }).OrderBy(od => od.SaleHeaderID).ToList();

                };

                if (lst != null && lst.Count > 0)
                {
                    lstMod = ModDataFromSale(lst);
                    gv.DataSource = lstMod;
                    Session["ReportSaleDeli"] = lstMod;
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
                if (Session["ReportSaleDeli"] == null)
                {
                    BindData();
                }
                #region Variable
                List<SaleHeaderDTO> lst = (List<SaleHeaderDTO>)Session["ReportSaleDeli"];
                List<SaleHeaderDTO> lstChoose = new List<SaleHeaderDTO>();                
                SaleHeaderDTO o = new SaleHeaderDTO();
                SaleHeaderDTO oP2 = new SaleHeaderDTO();
                CheckBox cb, cbCOD, cbP2;
                HiddenField hdd;
                Int32 ID = 0;
                lst.ForEach(c => c.COD = "0");
                #endregion
                foreach (GridViewRow item in gv.Rows)
                {
                    cb = (CheckBox)item.FindControl("chk");
                    cbCOD = (CheckBox)item.FindControl("chkCOD");
                    cbP2 = (CheckBox)item.FindControl("chkP2");
                    if(cb != null && cb.Checked)
                    {
                        hdd = (HiddenField)item.FindControl("hddID");
                        if(hdd != null && !string.IsNullOrEmpty(hdd.Value))
                        {
                            ID = ToInt32(hdd.Value);
                            o = lst.FirstOrDefault(w => w.SaleHeaderID.Equals(ID));      
                            if (o != null)
                            {
                                oP2 = (SaleHeaderDTO)o.Clone();
                                if (cbCOD != null && cbCOD.Checked)
                                    o.COD = "1";

                                lstChoose.Add(o);

                                //P2
                                if (cbP2 != null && cbP2.Checked)
                                {                                    
                                    lstChoose.Add(oP2);
                                }
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

        protected void ReportDeliveryExportPDF(List<SaleHeaderDTO> lst)
        {
            MemoryStream ms = new MemoryStream();
            Document doc = new Document(PageSize.A4, 3, 3, 3, 3);
            //var output = new FileStream(Server.MapPath(filename), FileMode.Create);
            var writer = PdfWriter.GetInstance(doc, ms);
            doc.Open();
            try
            {
                #region Variable
                BaseColor bc = new BaseColor(255, 255, 255);
                //string fontFamily = "C:/Windows/Fonts/Tahoma.ttf";
                string fontFamily = Server.MapPath("~") + "Fonts\\CSChatThai.ttf";
                BaseFont bf = BaseFont.CreateFont(fontFamily, BaseFont.IDENTITY_H, BaseFont.NOT_EMBEDDED);
                Font fontBold = new Font(bf, 15, iTextSharp.text.Font.BOLD);
                Font fontNormal = new Font(bf, 15);              
                PdfPTable table = new PdfPTable(1);
                PdfPCell cell1 = new PdfPCell();
                table.WidthPercentage = 99;
                PdfContentByte pcbItemCode, pcbCOD;
                string CODTextForm = "เก็บเงินปลายทาง \r\nจำนวนเงิน ";
                string CODText = "", SenderName = "", SenderTel = "", ItemCode = "";
                int i = 0, senderID = 0;
                int[] lowerY = { 620, 350, 90 };
                int[] upperY = { 650, 380, 120 };
                Rectangle rect;
                List<MasSender> lstSen = new List<MasSender>();
                MasSender Sen = new MasSender();
                #endregion

                if (lst != null && lst.Count > 0)
                {
                    if(Session["ReportSaleDeliDDL"] != null)
                    {
                        lstSen = (List<MasSender>)Session["ReportSaleDeliDDL"];
                        senderID = ToInt32(ddlSender.SelectedItem.Value);
                        Sen = lstSen.FirstOrDefault(w => w.SenderID.Equals(senderID));
                        if(Sen != null)
                        {
                            SenderName = Sen.SenderName;
                            SenderTel = Sen.Tel;
                        }
                    }

                    foreach (SaleHeaderDTO item in lst)
                    {
                        if(i > 0 && i % 3 == 0)
                        {
                            doc.Add(table);
                            doc.NewPage();
                            table = new PdfPTable(1);
                            table.WidthPercentage = 99;
                            i = 0;
                        }

                        AddTextToRow("  ", fontNormal, table);
                        AddTextToRow("  จัดส่ง :  " + item.Name, fontNormal, table);
                        AddTextToRow("  โทร :  " + item.Tel, fontNormal, table);
                        AddTextToRow("  ที่อยู่1 :  " + item.CustomerAddress, fontNormal, table);
                        AddTextToRow("  ที่อยู่2 :  " + item.CustomerDistrict + " " + item.CustomerCountry, fontNormal, table);
                        AddTextToRow("  ที่อยู่3 :  " + item.CustomerProvince + " " + item.CustomerPostalCode, fontNormal, table);
                        //AddTextToRow("ที่อยู่1 :  " + item.Add1, fontNormal, table);
                        //AddTextToRow("ที่อยู่2 :  " + item.Add2, fontNormal, table);
                        //AddTextToRow("ที่อยู่3 :  " + item.Add3, fontNormal, table);
                        AddTextToRow("  ผู้ส่ง :  " + SenderName, fontNormal, table);
                        AddTextToRow("  โทร.  " + SenderTel, fontNormal, table);

                        AddTextToRow("  ", fontNormal, table);
                        AddTextToRow("   ----------------------------------------------------------------------------------------------------", fontNormal, table);

                        //pcbItemCode = writer.DirectContent;
                        //PlaceText(pcbItemCode, item.ItemCode, fontBold, 300, lowerY[i], 550, upperY[i], 20, Element.ALIGN_CENTER);

                        pcbItemCode = writer.DirectContent;
                        if(item.dAmount > 1)
                        {
                            //ItemCode = item.ItemCode + "(" + item.dAmount + ")";
                            ItemCode = item.ItemCode + " * " + item.dAmount;
                        }
                        else
                        {
                            ItemCode = item.ItemCode;
                        }
                        PlaceText(pcbItemCode, ItemCode, fontBold, 300, lowerY[i], 550, upperY[i], 20, Element.ALIGN_CENTER);
                        rect = new Rectangle(300, lowerY[i], 550, upperY[i]);
                        pcbItemCode.SetColorStroke(BaseColor.BLACK);
                        pcbItemCode.Rectangle(rect.Left, rect.Bottom, rect.Width, rect.Height);
                        pcbItemCode.Stroke();

                        if(!string.IsNullOrEmpty(item.COD) && item.COD == "1")
                        {
                            pcbCOD = writer.DirectContent;
                            CODText = CODTextForm + item.SaleAmount.ToString("###,##0") + " บาท";
                            PlaceText(pcbCOD, CODText, fontBold, 300, lowerY[i] + 130, 550, upperY[i] + 150, 20, Element.ALIGN_CENTER);
                            rect = new Rectangle(300, lowerY[i] + 130, 550, upperY[i] + 150);
                            pcbCOD.SetColorStroke(BaseColor.BLACK);
                            pcbCOD.Rectangle(rect.Left, rect.Bottom, rect.Width, rect.Height);
                            pcbCOD.Stroke();
                        }
                        
                        i++;
                    }
                    doc.Add(table);
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

            string filename = "ReportSaleDelivery_" + DateTime.Now.ToString("yyyyMMdd");
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

        protected void AddTextToRow(string Text, Font fontUse, PdfPTable table)
        {
            try
            {
                PdfPCell cell1 = GetNewCell(false);
                cell1.AddElement(GetNewParag(Text, fontUse));
                PdfPCell[] cellAry = new PdfPCell[] { cell1 };
                PdfPRow row = new PdfPRow(cellAry);
                table.Rows.Add(row);
            }
            catch (Exception ex)
            {
                
            }
        }
    }
}