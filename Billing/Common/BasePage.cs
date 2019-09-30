using Billing.AppData;
using Billing.Model;
using iTextSharp.text;
using iTextSharp.text.pdf;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Web;
using System.Web.UI;

namespace Billing.Common
{
    public class BasePage : System.Web.UI.Page
    {
        public void CheckUser()
        {
            if(Session["Username"] == null)
                HttpContext.Current.Response.Redirect("/Logout.aspx");            
        }

        #region Message Alert
        public void ShowMessageBox(string message)
        {
            string msgboxScript = "alert('" + message + "');";
            ScriptManager.RegisterClientScriptBlock(this.Page, this.Page.GetType(), "msgboxScriptAJAX", msgboxScript, true);
        }

        public void ShowMessageBox(string message, System.Web.UI.Page currentPage)
        {
            string msgboxScript = "alert('" + message + "');";

            if ((ScriptManager.GetCurrent(currentPage) != null))
            {
                ScriptManager.RegisterClientScriptBlock(currentPage, currentPage.GetType(), "msgboxScriptAJAX", msgboxScript, true);
            }
        }

        public void ShowMessageBox(string message, System.Web.UI.Page currentPage, string redirectNamePage = "")
        {
            string msgboxScript = "alert('" + message + "');";

            if (redirectNamePage == "" && message != "1")
            {
                if ((ScriptManager.GetCurrent(currentPage) != null))
                {
                    ScriptManager.RegisterClientScriptBlock(currentPage, currentPage.GetType(), "msgboxScriptAJAX", msgboxScript, true);
                }
            }
            else if (redirectNamePage != "" && message != "1")
            {
                string redirectPage = "window.location.href=\"" + redirectNamePage + "\";";

                if ((ScriptManager.GetCurrent(currentPage) != null))
                {
                    ScriptManager.RegisterClientScriptBlock(currentPage, currentPage.GetType(), "msgboxScriptAJAX", msgboxScript + redirectPage, true);
                }
            }
            else
            {
                string redirectPage = "window.location.href=\"" + redirectNamePage + "\";";

                if ((ScriptManager.GetCurrent(currentPage) != null))
                {
                    ScriptManager.RegisterClientScriptBlock(currentPage, currentPage.GetType(), "msgboxScriptAJAX", redirectPage, true);
                }
            }
        }
        #endregion

        #region Convert ... to ...
        public double ToDoudle(string Input)
        {
            try
            {
                return Convert.ToDouble(Input);
            }
            catch (Exception)
            {
                return 0;
            }
        }
        public Int32 ToInt32(object Input)
        {
            try
            {
                return Convert.ToInt32(Input);
            }
            catch (Exception)
            {
                return 0;
            }
        }
        #endregion

        #region Get....
        public string GetMonth(string input)
        {
            string Result = "";
            try
            {
                System.Globalization.CultureInfo cul = new System.Globalization.CultureInfo("en-US");
                input = input.PadLeft(2, '0');
                Result = DateTime.ParseExact(input, "MM", cul).ToString("MMMM", cul);
            }
            catch (Exception ex)
            {
                
            }
            return Result;
        }

        public string GetConfig(string key)
        {
            try
            {
                return System.Configuration.ConfigurationManager.AppSettings[key];
            }
            catch (Exception ex)
            {
                return "";
            }
        }
        #endregion

        #region User
        public string GetUsername()
        {            
            try
            {
                if (Session["Username"] != null)
                    return Session["Username"].ToString();
                else
                {
                    ShowMessageBox("Session Timeout. !!", this, "../Index.aspx");
                }
            }
            catch (Exception ex)
            {
                
            }
            return "Null";
        }
        #endregion

        #region SendMail
        public void SendMailError(string error, System.Reflection.MethodBase method)
        {
            try
            {
                string to = GetConfig("mailError");
                string from = "system@system.com";
                string subject = "System get something wrong!!!";
                StringBuilder sb = new StringBuilder();
                sb.AppendFormat("Classname : {0}", method.ReflectedType.FullName).AppendLine();
                sb.AppendFormat("Method : {0}", method.Name).AppendLine();
                sb.AppendFormat("Error : {0}", error).AppendLine();
                string body = sb.ToString();

                MailMessage message = new MailMessage(from, to, subject, body);
                //message.IsBodyHtml = true;

                SmtpClient client = new SmtpClient(GetConfig("Connection.SMTP.IP").ToString(), Convert.ToInt32(GetConfig("Connection.SMTP.Port").ToString()));
                client.Credentials = System.Net.CredentialCache.DefaultNetworkCredentials;
                client.Send(message);
            }
            catch (Exception ex) 
            {

            }
        }
        #endregion

        #region PDF Bill Vat
        // Version 1.0 กระดาษต่อเนื่อง
        public string ExportPDFBillVat(Model.SaleHeaderDTO Header, List<Model.SaleDetailDTO> Detail)
        {
            //string filename = "/ExportPDF/MyFirstPDF.pdf";            
            string error = "";
            MemoryStream ms = new MemoryStream();
            //Rectangle A5 = new RectangleReadOnly(595, 450);
            Document doc = new Document(new RectangleReadOnly(595, 545.0f), 3, 3, 3, 0);
            //doc.SetPageSize(PageSize.A5.Rotate());
            //var output = new FileStream(Server.MapPath(filename), FileMode.Create);
            var writer = PdfWriter.GetInstance(doc, ms);
            doc.Open();
            try
            {
                BaseColor bc = new BaseColor(255, 255, 255);
                float[] widths;                
                //string fontFamily = "C:/Windows/Fonts/Tahoma.ttf";
                string fontFamily = Server.MapPath("~") + "Fonts\\CSChatThai.ttf";
                BaseFont bf = BaseFont.CreateFont(fontFamily, BaseFont.IDENTITY_H, BaseFont.EMBEDDED);   
                //Font fontHeaderGrid = new Font(bf, 12, iTextSharp.text.Font.BOLD, BaseColor.WHITE);
                Font fontGrid = new Font(bf, 9);
                Font fontDetail = new Font(bf, 11);
                Font fontDetailOld = new Font(bf, 10);                
                Font fontAdd = new Font(bf, 9);
                Font fontAddB = new Font(bf, 9, 1);                
                PdfPTable tableHead = new PdfPTable(3);
                PdfPTable tableDetail = new PdfPTable(8);
                PdfPTable tableRemark = new PdfPTable(2);
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
                PdfPCell cell20 = new PdfPCell();
                PdfPCell cell21 = new PdfPCell();                
                tableHead.WidthPercentage = 99;
                tableDetail.WidthPercentage = 99;               

                #region Header
                //Row Blank
                cell1 = GetNewCell(false);
                cell2 = GetNewCell(false);
                cell3 = GetNewCell(false);
                cell1.AddElement(new Paragraph(" ", fontDetailOld));
                cellAry = new PdfPCell[] { cell1, cell2, cell3 };
                row = new PdfPRow(cellAry);
                tableHead.Rows.Add(row);
                tableHead.Rows.Add(row);
                tableHead.Rows.Add(row);
                tableHead.Rows.Add(row);
                tableHead.Rows.Add(row);
                tableHead.Rows.Add(row);
                tableHead.Rows.Add(row);
                tableHead.Rows.Add(row);
                tableHead.Rows.Add(row);
                doc.Add(tableHead);                                
                
                #endregion

                string Add1 = "", Add2 = "";
                Add1 = Header.CustomerAddress;
                Add2 = Header.CustomerDistrict + " " + Header.CustomerCountry + " " + Header.CustomerProvince + " " + Header.CustomerPostalCode;                

                PdfContentByte SaleNo = writer.DirectContent;
                PlaceText(SaleNo, Header.SaleNumber, fontAdd, 330, 373, 420, 454, 14, Element.ALIGN_LEFT);
                PdfContentByte Date = writer.DirectContent;
                PlaceText(Date, Header.ReceivedDateStr, fontAdd, 460, 373, 550, 454, 14, Element.ALIGN_LEFT);
                PdfContentByte CustName = writer.DirectContent;
                PlaceText(CustName, Header.CustomerName, fontAdd, 75, 369, 400, 434, 14, Element.ALIGN_LEFT);
                PdfContentByte CustAdd = writer.DirectContent;
                PlaceText(CustAdd, Add1, fontAdd, 75, 350, 400, 419, 14, Element.ALIGN_LEFT);
                PdfContentByte CustAdd2 = writer.DirectContent;
                PlaceText(CustAdd2, Add2, fontAdd, 75, 330, 400, 405, 14, Element.ALIGN_LEFT);

                #region Detail & Summary
                string[] spl, splAmt;
                string sn = "", RealSN = "", ItemDesc = "", ItemDescAmt = "";
                double pItem = 0, pVat = 0, pTotal = 0, Discount = 0;
                foreach (SaleDetailDTO item in Detail)
                {
                    ItemDesc = "";
                    ItemDescAmt = "";

                    cell1 = GetNewCell(false);
                    cell2 = GetNewCell(false);
                    cell3 = GetNewCell(false);
                    cell4 = GetNewCell(false);
                    cell5 = GetNewCell(false);
                    cell6 = GetNewCell(false);
                    cell7 = GetNewCell(false);
                    cell8 = GetNewCell(false);

                    cell2.AddElement(GetNewParag(item.ItemCode, fontAdd));
                    cell3.AddElement(GetNewParag(item.ItemName, fontAdd));
                    cell4.AddElement(GetNewParag(item.AmountStr, fontAdd, 1));
                    cell5.AddElement(GetNewParag(item.ItemPriceExVATStr, fontAdd, 2));
                    cell7.AddElement(GetNewParag(item.TotalExVatStr, fontAdd, 2));
                    cellAry = new PdfPCell[] { cell1, cell2, cell3, cell4, cell5, cell6, cell7, cell8 };
                    row = new PdfPRow(cellAry);
                    widths = new float[] { 5f, 34f, 115f, 26f, 30f, 20f, 30f, 12f };
                    
                    tableDetail.SetWidths(widths);
                    tableDetail.Rows.Add(row);

                    #region ItemDescription
                    if (item.ItemDescription != null)
                    {                        
                        spl = item.ItemDescription.Split(new string[] { "\r\n" }, StringSplitOptions.None);
                        if(spl != null && spl.Length > 0)
                        {
                            foreach (string s in spl)
                            {
                                cell1 = GetNewCell(false);
                                cell2 = GetNewCell(false);
                                cell3 = GetNewCell(false);
                                cell4 = GetNewCell(false);
                                cell5 = GetNewCell(false);
                                cell6 = GetNewCell(false);
                                cell7 = GetNewCell(false);
                                cell8 = GetNewCell(false);

                                splAmt = s.Split(new string[] { "จำนวน" }, StringSplitOptions.None);
                                if (splAmt != null && splAmt.Length > 0)
                                {
                                    ItemDesc = splAmt[0];
                                    if (splAmt.Length > 1)
                                        ItemDescAmt = splAmt[1].Trim();
                                    else
                                        ItemDescAmt = "";

                                    cell3.AddElement(GetNewParag(ItemDesc, fontAdd));
                                    cell4.AddElement(GetNewParag(ItemDescAmt, fontAdd, 1));
                                }

                                cellAry = new PdfPCell[] { cell1, cell2, cell3, cell4, cell5, cell6, cell7, cell8 };
                                row = new PdfPRow(cellAry);
                                widths = new float[] { 5f, 34f, 115f, 26f, 30f, 20f, 30f, 12f };
                                tableDetail.SetWidths(widths);
                                tableDetail.Rows.Add(row);
                            }                           
                        }
                    }
                    #endregion

                    RealSN = RealSN + item.SerialNumber;
                    sn = sn + item.SerialNumber + ", ";
                    pTotal = pTotal + item.TotalIndVat;
                    pVat = pVat + item.TotalVATAmount2Digit;
                    Discount = Discount + item.Discount.Value;
                }

                #region Discount
                //if(Discount > 0)
                //{
                //    cell1 = GetNewCell(false);
                //    cell2 = GetNewCell(false);
                //    cell3 = GetNewCell(false);
                //    cell4 = GetNewCell(false);
                //    cell5 = GetNewCell(false);
                //    cell6 = GetNewCell(false);
                //    cell7 = GetNewCell(false);
                //    cell8 = GetNewCell(false);

                //    cell3.AddElement(GetNewParag("ส่วนลด         ", fontAdd, 1));
                //    cell7.AddElement(GetNewParag(Discount.ToString("###,##0.00"), fontAdd, 2));
                //    cellAry = new PdfPCell[] { cell1, cell2, cell3, cell4, cell5, cell6, cell7, cell8 };
                //    row = new PdfPRow(cellAry);
                //    widths = new float[] { 7f, 34f, 115f, 26f, 30f, 20f, 30f, 12f };
                //    tableDetail.SetWidths(widths);
                //    tableDetail.Rows.Add(row);
                //}                
                #endregion

                #region Remark
                if (Header.Remark != "")
                {
                    cell1 = GetNewCell(false);
                    cell2 = GetNewCell(false);
                    cell3 = GetNewCell(false, 0);
                    cell4 = GetNewCell(false);
                    cell5 = GetNewCell(false);
                    cell6 = GetNewCell(false);
                    cell7 = GetNewCell(false);
                    cell8 = GetNewCell(false);

                    cell20 = GetNewCell(false, 0);
                    cell21 = GetNewCell(false, 0);
                    //cell3.HorizontalAlignment = PdfPCell.ALIGN_LEFT;
                    //cell20.HorizontalAlignment = PdfPCell.ALIGN_LEFT;

                    cell20.AddElement(new Chunk("หมายเหตุ : ", fontAddB));
                    //cell20.AddElement(GetNewParag("หมายเหตุ : ", fontAddB));
                    cell21.AddElement(GetNewParag(Header.Remark, fontAdd));
                    cellAry = new PdfPCell[] { cell20, cell21 };
                    row = new PdfPRow(cellAry);
                    widths = new float[] { 50f, 180f };
                    tableRemark.SetWidths(widths);
                    tableRemark.Rows.Add(row);
                    tableRemark.HorizontalAlignment = PdfPCell.ALIGN_LEFT;
                    cell3.AddElement(tableRemark);

                    //cell3.AddElement(GetNewParag("หมายเหตุ : ", fontAddB));
                    //cell3.AddElement(GetNewParag(Header.Remark, fontAdd));
                    cellAry = new PdfPCell[] { cell1, cell2, cell3, cell4, cell5, cell6, cell7, cell8 };
                    row = new PdfPRow(cellAry);
                    widths = new float[] { 5f, 34f, 115f, 26f, 30f, 20f, 30f, 12f };

                    tableDetail.SetWidths(widths);
                    tableDetail.Rows.Add(row);
                }
                #endregion     
           
                #region S/N
                if (!string.IsNullOrEmpty(RealSN))
                {
                    cell1 = GetNewCell(false);
                    cell2 = GetNewCell(false);
                    cell3 = GetNewCell(false);
                    cell4 = GetNewCell(false);
                    cell5 = GetNewCell(false);
                    cell6 = GetNewCell(false);
                    cell7 = GetNewCell(false);
                    cell8 = GetNewCell(false);

                    sn = sn.Substring(0, sn.Length - 2);
                    cell3.AddElement(GetNewParag("S/N : " + sn, fontAdd));
                    cellAry = new PdfPCell[] { cell1, cell2, cell3, cell4, cell5, cell6, cell7, cell8 };
                    row = new PdfPRow(cellAry);
                    widths = new float[] { 5f, 34f, 115f, 26f, 30f, 20f, 30f, 12f };

                    tableDetail.SetWidths(widths);
                    tableDetail.Rows.Add(row);
                }
                #endregion

                //pTotal = pTotal - Discount;
                pItem = pTotal - pVat;
                doc.Add(tableDetail);

                PdfContentByte PriceItem = writer.DirectContent;
                PlaceText(PriceItem, pItem.ToString("###,##0.00"), fontAdd, 480, 102, 563, 131, 0, Element.ALIGN_RIGHT);
                PdfContentByte PriceVat = writer.DirectContent;
                PlaceText(PriceVat, pVat.ToString("###,##0.00"), fontAdd, 480, 89, 563, 114, 0, Element.ALIGN_RIGHT);
                PdfContentByte PriceTotal = writer.DirectContent;
                PlaceText(PriceTotal, pTotal.ToString("###,##0.00"), fontAdd, 480, 76, 563, 97, 0, Element.ALIGN_RIGHT);
                PdfContentByte Blank = writer.DirectContent;
                PlaceText(Blank, "  ", fontDetail, 480, 5, 563, 12, 0, Element.ALIGN_RIGHT);
                #endregion

                //End Generate
            }
            catch (Exception ex)
            {                
                error = ex.Message;
            }
            finally
            {
                doc.Close();
                writer.Close();
            }

            string FileName = "Report_" + Header.SaleNumber;
            Response.Clear();
            Response.Buffer = false;
            Response.ContentEncoding = Encoding.UTF8;
            Response.HeaderEncoding = Encoding.UTF8;
            Response.AddHeader("content-disposition", "attachment;filename=" + FileName + ".pdf");
            Response.ContentType = "application/pdf";
            Response.BinaryWrite(ms.ToArray());
            Response.Flush();
            Response.SuppressContent = true;
            
            return error;
        }

        // Version 2.0 A4
        public string ExportPDFBillVat2(Model.SaleHeaderDTO Header, List<Model.SaleDetailDTO> Detail)
        {           
            string error = "";
            MemoryStream ms = new MemoryStream();
            Document doc = new Document();
            doc.SetPageSize(PageSize.A4);
            var writer = PdfWriter.GetInstance(doc, ms);
            doc.Open();
            doc.SetMargins(36f, 36f, 10f, 10f);
            try
            {
                #region Variable
                BaseColor bc = new BaseColor(255, 255, 255);
                float[] widths;
                string fontFamily = Server.MapPath("~") + "Fonts\\CSChatThai.ttf";
                BaseFont bf = BaseFont.CreateFont(fontFamily, BaseFont.IDENTITY_H, BaseFont.EMBEDDED);
                Font fontComName18 = new Font(bf, 17, iTextSharp.text.Font.BOLD);
                Font fontComName = new Font(bf, 16, iTextSharp.text.Font.BOLD);
                Font font5 = new Font(bf, 5);
                Font font12 = new Font(bf, 12);
                Font font12B = new Font(bf, 12, iTextSharp.text.Font.BOLD);
                Font font12BU = new Font(bf, 12, iTextSharp.text.Font.BOLD | iTextSharp.text.Font.UNDERLINE);
                Font font11 = new Font(bf, 11);
                Font font11B = new Font(bf, 11, iTextSharp.text.Font.BOLD);
                Font font8 = new Font(bf, 8);
                PdfPTable tableHead = new PdfPTable(3);
                PdfPTable tableHead2 = new PdfPTable(6);
                PdfPTable tableDetail = new PdfPTable(2);
                PdfPTable tableDetail2 = new PdfPTable(6);
                PdfPTable tableRemark = new PdfPTable(2);
                PdfPTable tableSummary = new PdfPTable(3);
                PdfPTable tableSign = new PdfPTable(3);
                PdfPCell[] cellAry;
                PdfPRow row;
                PdfPCell cell1 = new PdfPCell();
                PdfPCell cell2 = new PdfPCell();
                PdfPCell cell3 = new PdfPCell();
                PdfPCell cell4 = new PdfPCell();
                PdfPCell cell5 = new PdfPCell();
                PdfPCell cell6 = new PdfPCell();
                PdfPCell cell20 = new PdfPCell();
                PdfPCell cell21 = new PdfPCell();
                tableHead.WidthPercentage = 99;
                tableHead2.WidthPercentage = 99;
                tableDetail.WidthPercentage = 99;
                tableDetail2.WidthPercentage = 99;
                tableSummary.WidthPercentage = 99;
                tableSign.WidthPercentage = 99;
                int count = 0, countRow = 0;
                string Add1 = "", Add2 = "", SaleNum = "";
                string[] spl, splAmt;
                string sn = "", RealSN = "", ItemDesc = "", ItemDescAmt = "";
                double pItem = 0, pVat = 0, pTotal = 0;//, Discount = 0;
                #endregion

                #region Header

                #region Header
                cell1 = GetNewCell(false);
                cell3 = GetNewCell(false);
                cell2 = GetNewCell(false);

                cell2.AddElement(GetNewParag("บริษัท โปร เวลดิ้ง แอนด์ ทูลส์ จำกัด", fontComName18, 1, 18));
                cellAry = new PdfPCell[] { cell1, cell2, cell3 };
                row = new PdfPRow(cellAry);
                tableHead.Rows.Add(row);

                cell2 = GetNewCell(false);
                cell2.AddElement(GetNewParag("PRO WELDING & TOOLS CO., LTD.", fontComName, 1, 18));
                cellAry = new PdfPCell[] { cell1, cell2, cell3 };
                row = new PdfPRow(cellAry);
                tableHead.Rows.Add(row);

                cell2 = GetNewCell(false);
                cell2.AddElement(GetNewParag("9/1 ซอยบางบอน 4 (ซอย 14 แยก 1) ถนนบางบอน 4 แขวงบางบอนเหนือ เขตบางบอน กรุงเทพฯ", font12, 1, 10));
                cellAry = new PdfPCell[] { cell1, cell2, cell3 };
                row = new PdfPRow(cellAry);
                tableHead.Rows.Add(row);

                cell2 = GetNewCell(false);
                cell2.AddElement(GetNewParag("9/1 Soi Bangbon 4 (Soi 14 Yeak 1) Bangbon 4 Rd., Kweang Bangbon Nua, Khet Bangbon, Bangkok", font12, 1, 10));
                cellAry = new PdfPCell[] { cell1, cell2, cell3 };
                row = new PdfPRow(cellAry);
                tableHead.Rows.Add(row);

                cell2 = GetNewCell(false);
                cell2.AddElement(GetNewParag("TAX ID: 0105560093156 (สำนักงานใหญ่) Tel. 02-297-0744, 086-909-4777, 098-747-9932", font12, 1, 10));
                cellAry = new PdfPCell[] { cell1, cell2, cell3 };
                row = new PdfPRow(cellAry);
                tableHead.Rows.Add(row);

                cell1 = GetNewCell(false);
                cell2 = GetNewCell(false, 1);
                cell3 = GetNewCell(false);
                cell2.AddElement(GetNewParag(" ", font5, 1));
                cell1.Border = PdfPCell.BOTTOM_BORDER;
                cell2.Border = PdfPCell.BOTTOM_BORDER;
                cell3.Border = PdfPCell.BOTTOM_BORDER;
                cellAry = new PdfPCell[] { cell1, cell2, cell3 };
                row = new PdfPRow(cellAry);
                tableHead.Rows.Add(row);
                widths = new float[] { 20f, 160f, 20f };
                tableHead.SetWidths(widths);
                doc.Add(tableHead);
                #endregion

                #region Header2
                SaleNum = Header.SaleNumber;
                cell1 = GetNewCell(false);
                cell2 = GetNewCell(false);
                cell3 = GetNewCell(false);
                cell4 = GetNewCell(false);
                cell5 = GetNewCell(false);
                cell6 = GetNewCell(false);
                cell1.Rowspan = 2;
                cell2.Rowspan = 2;
                cell3.Rowspan = 2;
                cell4.Rowspan = 2;
                cell5.Rowspan = 2;
                cell1.VerticalAlignment = Element.ALIGN_MIDDLE;
                cell2.VerticalAlignment = Element.ALIGN_MIDDLE;
                cell3.VerticalAlignment = Element.ALIGN_MIDDLE;
                cell4.VerticalAlignment = Element.ALIGN_MIDDLE;
                cell5.VerticalAlignment = Element.ALIGN_MIDDLE;

                cell1.AddElement(GetNewParag("วันที่", font12B, 1, 10));
                cell2.AddElement(GetNewParag(Header.ReceivedDateStr, font12, 0, 10));
                cell3.AddElement(GetNewParag("เลขที่", font12B, 1, 10));
                cell4.AddElement(GetNewParag(SaleNum, font12, 0, 10));
                cell5.AddElement(GetNewParag("Original", font12B, 1, 10));
                cell6.AddElement(GetNewParag("ใบกำกับภาษี / ใบส่งของ / ใบเสร็จรับเงิน", font12BU, 1, 10));
                cellAry = new PdfPCell[] { cell1, cell2, cell3, cell4, cell5, cell6 };
                row = new PdfPRow(cellAry);
                tableHead2.Rows.Add(row);

                cell1 = GetNewCell(false);
                cell2 = GetNewCell(false);
                cell3 = GetNewCell(false);
                cell4 = GetNewCell(false);
                cell5 = GetNewCell(false);
                cell6 = GetNewCell(false);
                cell6.AddElement(GetNewParag("Tax Invoice / Delivery Order / Receipt", font12B, 1, 10));
                cellAry = new PdfPCell[] { cell1, cell2, cell3, cell4, cell5, cell6 };
                row = new PdfPRow(cellAry);
                tableHead2.Rows.Add(row);

                cell1 = GetNewCell(false);
                cell2 = GetNewCell(false);
                cell3 = GetNewCell(false);
                cell4 = GetNewCell(false);
                cell5 = GetNewCell(false);
                cell6 = GetNewCell(false);
                cell6.AddElement(GetNewParag(" ", font5, 0, 1));
                cellAry = new PdfPCell[] { cell1, cell2, cell3, cell4, cell5, cell6 };
                row = new PdfPRow(cellAry);
                tableHead2.Rows.Add(row);
                widths = new float[] { 10f, 40f, 10f, 35f, 20f, 50f };
                tableHead2.SetWidths(widths);
                doc.Add(tableHead2);
                #endregion

                #endregion

                #region Name & Address
                Add1 = Header.CustomerAddress;// +" " + header.CustomerDistrict;
                Add2 = Header.CustomerDistrict + " " + Header.CustomerCountry + " " + Header.CustomerProvince + " " + Header.CustomerPostalCode;

                cell1 = GetNewCell(false);
                cell2 = GetNewCell(false);
                cell3 = GetNewCell(false);
                cell4 = GetNewCell(false);
                cell5 = GetNewCell(false);
                cell6 = GetNewCell(false);

                cell1.Border = PdfPCell.LEFT_BORDER | PdfPCell.TOP_BORDER;
                cell2.Border = PdfPCell.RIGHT_BORDER | PdfPCell.TOP_BORDER;
                cell1.AddElement(GetNewParag("Buyer / ผู้ซื้อ", font11, 0, 10));
                cell2.AddElement(GetNewParag(Header.CustomerName, font11, 0, 10));
                cellAry = new PdfPCell[] { cell1, cell2 };
                row = new PdfPRow(cellAry);
                tableDetail.Rows.Add(row);

                cell1 = GetNewCell(false);
                cell2 = GetNewCell(false);
                cell1.Border = PdfPCell.LEFT_BORDER;
                cell2.Border = PdfPCell.RIGHT_BORDER;
                cell1.AddElement(GetNewParag("Address / ที่อยู่", font11, 0, 10));
                cell2.AddElement(GetNewParag(Add1, font11, 0, 10));
                cellAry = new PdfPCell[] { cell1, cell2 };
                row = new PdfPRow(cellAry);
                tableDetail.Rows.Add(row);

                cell1 = GetNewCell(false);
                cell2 = GetNewCell(false);
                cell1.Border = PdfPCell.LEFT_BORDER;
                cell2.Border = PdfPCell.RIGHT_BORDER;
                cell1.AddElement(GetNewParag("  ", font11));
                cell2.AddElement(GetNewParag(Add2, font11, 0, 10));
                cellAry = new PdfPCell[] { cell1, cell2 };
                row = new PdfPRow(cellAry);
                tableDetail.Rows.Add(row);
                widths = new float[] { 10f, 60f };
                tableDetail.SetWidths(widths);
                doc.Add(tableDetail);
                #endregion

                #region Table Header
                cell1 = GetNewCell(false);
                cell2 = GetNewCell(false);
                cell3 = GetNewCell(false);
                cell4 = GetNewCell(false);
                cell5 = GetNewCell(false);
                cell6 = GetNewCell(false);
                cell1.Border = PdfPCell.LEFT_BORDER | PdfPCell.TOP_BORDER | PdfPCell.BOTTOM_BORDER;
                cell2.Border = PdfPCell.LEFT_BORDER | PdfPCell.TOP_BORDER | PdfPCell.BOTTOM_BORDER;
                cell3.Border = PdfPCell.LEFT_BORDER | PdfPCell.TOP_BORDER | PdfPCell.BOTTOM_BORDER;
                cell4.Border = PdfPCell.LEFT_BORDER | PdfPCell.TOP_BORDER | PdfPCell.BOTTOM_BORDER;
                cell5.Border = PdfPCell.LEFT_BORDER | PdfPCell.TOP_BORDER | PdfPCell.BOTTOM_BORDER;
                cell6.Border = PdfPCell.LEFT_BORDER | PdfPCell.TOP_BORDER | PdfPCell.BOTTOM_BORDER | PdfPCell.RIGHT_BORDER;
                cell1.AddElement(GetNewParag("รหัสสินค้า", font12, 1));
                cell2.AddElement(GetNewParag("รายการ", font12, 1));
                cell3.AddElement(GetNewParag("จำนวน", font12, 1));
                cell4.AddElement(GetNewParag("หน่วยละ", font12, 1));
                cell5.AddElement(GetNewParag("ส่วนลด %", font12, 1));
                cell6.AddElement(GetNewParag("จำนวนเงิน", font12, 1));
                cellAry = new PdfPCell[] { cell1, cell2, cell3, cell4, cell5, cell6 };
                row = new PdfPRow(cellAry);
                tableDetail2.Rows.Add(row);
                #endregion

                #region Item Detail & Process Summary
                foreach (SaleDetailDTO item in Detail)
                {
                    #region Reset
                    ItemDesc = "";
                    ItemDescAmt = "";

                    cell1 = GetNewCell(false);
                    cell2 = GetNewCell(false);
                    cell3 = GetNewCell(false);
                    cell4 = GetNewCell(false);
                    cell5 = GetNewCell(false);
                    cell6 = GetNewCell(false);
                    cell1.Border = PdfPCell.LEFT_BORDER;
                    cell2.Border = PdfPCell.LEFT_BORDER;
                    cell3.Border = PdfPCell.LEFT_BORDER;
                    cell4.Border = PdfPCell.LEFT_BORDER;
                    cell5.Border = PdfPCell.LEFT_BORDER;
                    cell6.Border = PdfPCell.LEFT_BORDER | PdfPCell.RIGHT_BORDER;
                    #endregion

                    //Start
                    cell1.AddElement(GetNewParag(item.ItemCode, font11));
                    cell2.AddElement(GetNewParag(item.ItemName, font11));
                    cell3.AddElement(GetNewParag(item.AmountStr, font11, 1));
                    cell4.AddElement(GetNewParag(item.ItemPriceExVATStr, font11, 2));
                    cell6.AddElement(GetNewParag(item.TotalExVatStr, font11, 2));
                    cellAry = new PdfPCell[] { cell1, cell2, cell3, cell4, cell5, cell6 };
                    row = new PdfPRow(cellAry);
                    tableDetail2.Rows.Add(row);
                    countRow++;

                    #region Item Description
                    if (!string.IsNullOrEmpty(item.ItemDescription))
                    {
                        spl = item.ItemDescription.Split(new string[] { "\r\n" }, StringSplitOptions.None);
                        if (spl != null && spl.Length > 0)
                        {
                            foreach (string s in spl)
                            {
                                #region Reset
                                cell1 = GetNewCell(false);
                                cell2 = GetNewCell(false);
                                cell3 = GetNewCell(false);
                                cell4 = GetNewCell(false);
                                cell5 = GetNewCell(false);
                                cell6 = GetNewCell(false);
                                cell1.Border = PdfPCell.LEFT_BORDER;
                                cell2.Border = PdfPCell.LEFT_BORDER;
                                cell3.Border = PdfPCell.LEFT_BORDER;
                                cell4.Border = PdfPCell.LEFT_BORDER;
                                cell5.Border = PdfPCell.LEFT_BORDER;
                                cell6.Border = PdfPCell.LEFT_BORDER | PdfPCell.RIGHT_BORDER;
                                #endregion

                                splAmt = s.Split(new string[] { "จำนวน" }, StringSplitOptions.None);
                                if (splAmt != null && splAmt.Length > 0)
                                {
                                    ItemDesc = splAmt[0];
                                    if (splAmt.Length > 1)
                                        ItemDescAmt = splAmt[1].Trim();
                                    else
                                        ItemDescAmt = "";

                                    cell2.AddElement(GetNewParag(ItemDesc, font11));
                                    cell3.AddElement(GetNewParag(ItemDescAmt, font11, 1));
                                }

                                cellAry = new PdfPCell[] { cell1, cell2, cell3, cell4, cell5, cell6 };
                                row = new PdfPRow(cellAry);
                                tableDetail2.Rows.Add(row);
                                countRow++;
                            }
                        }
                    }
                    #endregion

                    RealSN = RealSN + item.SerialNumber;
                    sn = sn + item.SerialNumber + ", ";
                    pTotal = pTotal + item.TotalIndVat;
                    pVat = pVat + item.TotalVATAmount2Digit;
                    //Discount = Discount + item.Discount;

                }

                #region Remark
                //if (Header.Remark != "")
                //{
                //    #region Reset
                //    cell1 = GetNewCell(false);
                //    cell2 = GetNewCell(false);
                //    cell3 = GetNewCell(false);
                //    cell4 = GetNewCell(false);
                //    cell5 = GetNewCell(false);
                //    cell6 = GetNewCell(false);
                //    cell1.Border = PdfPCell.LEFT_BORDER;
                //    cell2.Border = PdfPCell.LEFT_BORDER;
                //    cell3.Border = PdfPCell.LEFT_BORDER;
                //    cell4.Border = PdfPCell.LEFT_BORDER;
                //    cell5.Border = PdfPCell.LEFT_BORDER;
                //    cell6.Border = PdfPCell.LEFT_BORDER | PdfPCell.RIGHT_BORDER;

                //    cell20 = GetNewCell(false);
                //    cell21 = GetNewCell(false);
                //    #endregion

                //    cell20.AddElement(new Chunk("หมายเหตุ : ", font11B));
                //    cell21.AddElement(GetNewParag(Header.Remark, font11));
                //    cellAry = new PdfPCell[] { cell20, cell21 };
                //    row = new PdfPRow(cellAry);
                //    widths = new float[] { 50f, 180f };
                //    tableRemark.SetWidths(widths);
                //    tableRemark.Rows.Add(row);
                //    tableRemark.HorizontalAlignment = PdfPCell.ALIGN_LEFT;
                //    cell2.AddElement(tableRemark);

                //    cellAry = new PdfPCell[] { cell1, cell2, cell3, cell4, cell5, cell6 };
                //    row = new PdfPRow(cellAry);
                //    tableDetail2.Rows.Add(row);
                //    countRow++;
                //}
                #endregion

                #region S/N
                if (!string.IsNullOrEmpty(RealSN))
                {
                    #region Reset
                    cell1 = GetNewCell(false);
                    cell2 = GetNewCell(false);
                    cell3 = GetNewCell(false);
                    cell4 = GetNewCell(false);
                    cell5 = GetNewCell(false);
                    cell6 = GetNewCell(false);
                    cell1.Border = PdfPCell.LEFT_BORDER;
                    cell2.Border = PdfPCell.LEFT_BORDER;
                    cell3.Border = PdfPCell.LEFT_BORDER;
                    cell4.Border = PdfPCell.LEFT_BORDER;
                    cell5.Border = PdfPCell.LEFT_BORDER;
                    cell6.Border = PdfPCell.LEFT_BORDER | PdfPCell.RIGHT_BORDER;
                    #endregion

                    sn = sn.Substring(0, sn.Length - 2);
                    cell2.AddElement(GetNewParag("S/N : " + sn, font11));
                    cellAry = new PdfPCell[] { cell1, cell2, cell3, cell4, cell5, cell6 };
                    row = new PdfPRow(cellAry);
                    tableDetail2.Rows.Add(row);
                    countRow++;
                }
                #endregion

                pItem = pTotal - pVat;
                
                #region Empty Row & Bottom
                for (int i = countRow; i < 19; i++)
                {
                    cell1 = GetNewCell(false);
                    cell2 = GetNewCell(false);
                    cell3 = GetNewCell(false);
                    cell4 = GetNewCell(false);
                    cell5 = GetNewCell(false);
                    cell6 = GetNewCell(false);
                    cell1.Border = PdfPCell.LEFT_BORDER;
                    cell2.Border = PdfPCell.LEFT_BORDER;
                    cell3.Border = PdfPCell.LEFT_BORDER;
                    cell4.Border = PdfPCell.LEFT_BORDER;
                    cell5.Border = PdfPCell.LEFT_BORDER;
                    cell6.Border = PdfPCell.LEFT_BORDER | PdfPCell.RIGHT_BORDER;
                    cell1.AddElement(GetNewParag(" ", font11));
                    cellAry = new PdfPCell[] { cell1, cell2, cell3, cell4, cell5, cell6 };
                    row = new PdfPRow(cellAry);
                    tableDetail2.Rows.Add(row);
                }

                cell1 = GetNewCell(false);
                cell2 = GetNewCell(false);
                cell3 = GetNewCell(false);
                cell4 = GetNewCell(false);
                cell5 = GetNewCell(false);
                cell6 = GetNewCell(false);
                cell1.Border = PdfPCell.LEFT_BORDER | PdfPCell.BOTTOM_BORDER;
                cell2.Border = PdfPCell.LEFT_BORDER | PdfPCell.BOTTOM_BORDER;
                cell3.Border = PdfPCell.LEFT_BORDER | PdfPCell.BOTTOM_BORDER;
                cell4.Border = PdfPCell.LEFT_BORDER | PdfPCell.BOTTOM_BORDER;
                cell5.Border = PdfPCell.LEFT_BORDER | PdfPCell.BOTTOM_BORDER;
                cell6.Border = PdfPCell.LEFT_BORDER | PdfPCell.RIGHT_BORDER | PdfPCell.BOTTOM_BORDER;
                cell1.AddElement(GetNewParag(" ", font11));
                cellAry = new PdfPCell[] { cell1, cell2, cell3, cell4, cell5, cell6 };
                row = new PdfPRow(cellAry);
                tableDetail2.Rows.Add(row);

                widths = new float[] { 15f, 40f, 10f, 10f, 9f, 12f };
                tableDetail2.SetWidths(widths);
                doc.Add(tableDetail2);
                #endregion
                #endregion

                #region Summary
                cell1 = GetNewCell(false);
                cell2 = GetNewCell(false);
                cell3 = GetNewCell(false);
                cell1.Border = PdfPCell.LEFT_BORDER;
                cell2.Border = PdfPCell.LEFT_BORDER;
                cell3.Border = PdfPCell.LEFT_BORDER | PdfPCell.RIGHT_BORDER | PdfPCell.BOTTOM_BORDER;
                cell1.AddElement(GetNewParag("สินค้าที่ออกใบกำกับภาษีเกิน 7 วัน แล้วไม่รับ", font8, 0, 10));
                cell2.AddElement(GetNewParag("มูลค่าสินค้า / Goods Value", font8, 0, 10));
                cell3.AddElement(GetNewParag(pItem.ToString("###,##0.00"), font11, 2, 10));
                cellAry = new PdfPCell[] { cell1, cell2, cell3 };
                row = new PdfPRow(cellAry);
                tableSummary.Rows.Add(row);

                cell1 = GetNewCell(false);
                cell2 = GetNewCell(false);
                cell3 = GetNewCell(false);
                cell1.Border = PdfPCell.LEFT_BORDER;
                cell2.Border = PdfPCell.LEFT_BORDER;
                cell3.Border = PdfPCell.LEFT_BORDER | PdfPCell.RIGHT_BORDER | PdfPCell.BOTTOM_BORDER;
                cell1.AddElement(GetNewParag("ใบเสร็จรับเงินจะสมบูรณ์เมื่อมีลายเซ็นผู้รับเงิน การชำระเงินจะสมบูรณ์เมื่อบริษัทฯ ได้เรียเก็บเงินตามเช็คเรียบร้อยแล้ว", font8, 0, 10));
                cell2.AddElement(GetNewParag("ภาษีมูลค่าเพิ่ม 7% / VAT 7%", font8, 0, 10));
                cell3.AddElement(GetNewParag(pVat.ToString("###,##0.00"), font11, 2, 10));
                cellAry = new PdfPCell[] { cell1, cell2, cell3 };
                row = new PdfPRow(cellAry);
                tableSummary.Rows.Add(row);

                cell1 = GetNewCell(false);
                cell2 = GetNewCell(false);
                cell3 = GetNewCell(false);
                cell1.Border = PdfPCell.LEFT_BORDER | PdfPCell.BOTTOM_BORDER;
                cell2.Border = PdfPCell.LEFT_BORDER | PdfPCell.BOTTOM_BORDER;
                cell3.Border = PdfPCell.LEFT_BORDER | PdfPCell.RIGHT_BORDER | PdfPCell.BOTTOM_BORDER;
                cell1.AddElement(GetNewParag("สินค้าตามใบกำกับภาษียังคงเป็นกรรมสิทธิ์ของทางบริษัทฯ จนกว่าจะได้รับการชำระเงินเป็นที่เรียบร้อยแล้ว                ผิดตกยกเว้น E.&O.E.", font8, 0, 10));
                cell2.AddElement(GetNewParag("รวมมูลค่า / Total Amount", font8, 0, 10));
                cell3.AddElement(GetNewParag(pTotal.ToString("###,##0.00"), font11B, 2, 10));
                cellAry = new PdfPCell[] { cell1, cell2, cell3 };
                row = new PdfPRow(cellAry);
                tableSummary.Rows.Add(row);

                widths = new float[] { 65f, 19f, 12f };
                tableSummary.SetWidths(widths);
                doc.Add(tableSummary);
                #endregion

                #region Sign
                //PdfContentByte Box1 = writer.DirectContent;
                //Box1.Rectangle(52f, 155f, 10f, 15f);
                //Box1.Stroke();

                //PdfContentByte Box2 = writer.DirectContent;
                //Box2.Rectangle(140f, 155f, 10f, 15f);
                //Box2.Stroke();

                cell1 = GetNewCell(false);
                cell2 = GetNewCell(false);
                cell3 = GetNewCell(false);
                cell1.Border = PdfPCell.LEFT_BORDER;
                cell2.Border = PdfPCell.LEFT_BORDER;
                cell3.Border = PdfPCell.LEFT_BORDER | PdfPCell.RIGHT_BORDER;
                cell1.AddElement(GetNewParag(" ", font8, 0, 10));
                cell2.AddElement(GetNewParag(" ", font8, 0, 10));
                cell3.AddElement(GetNewParag("บริษัท โปร เวลดิ้ง แอนด์ ทูลส์ จำกัด", font8, 1, 10));
                cellAry = new PdfPCell[] { cell1, cell2, cell3 };
                row = new PdfPRow(cellAry);
                tableSign.Rows.Add(row);

                cell1 = GetNewCell(false);
                cell2 = GetNewCell(false);
                cell3 = GetNewCell(false);
                cell1.Border = PdfPCell.LEFT_BORDER;
                cell2.Border = PdfPCell.LEFT_BORDER;
                cell3.Border = PdfPCell.LEFT_BORDER | PdfPCell.RIGHT_BORDER;
                cell1.AddElement(GetNewParag("           __ เงินสด (Cash)                            __ เช็ค (Cheque)", font8, 0, 5));
                cell2.AddElement(GetNewParag(" ", font8, 0, 10));
                cell3.AddElement(GetNewParag("PRO WELDING & TOOLS CO., LTD.", font8, 1, 10));
                cellAry = new PdfPCell[] { cell1, cell2, cell3 };
                row = new PdfPRow(cellAry);
                tableSign.Rows.Add(row);

                cell1 = GetNewCell(false);
                cell2 = GetNewCell(false);
                cell3 = GetNewCell(false);
                cell1.Border = PdfPCell.LEFT_BORDER;
                cell2.Border = PdfPCell.LEFT_BORDER;
                cell3.Border = PdfPCell.LEFT_BORDER | PdfPCell.RIGHT_BORDER;
                cell1.AddElement(GetNewParag("ธนาคาร (Bank)................................................................................................", font8, 0, 10));
                cell2.AddElement(GetNewParag(" ", font8, 0, 10));
                cell3.AddElement(GetNewParag(" ", font8, 1, 10));
                cellAry = new PdfPCell[] { cell1, cell2, cell3 };
                row = new PdfPRow(cellAry);
                tableSign.Rows.Add(row);

                cell1 = GetNewCell(false);
                cell2 = GetNewCell(false);
                cell3 = GetNewCell(false);
                cell1.Border = PdfPCell.LEFT_BORDER;
                cell2.Border = PdfPCell.LEFT_BORDER;
                cell3.Border = PdfPCell.LEFT_BORDER | PdfPCell.RIGHT_BORDER;
                cell1.AddElement(GetNewParag("เลขที่ (No.)......................................................................................................", font8, 0, 10));
                cell2.AddElement(GetNewParag(".......................................            ......................", font8, 1, 10));
                cell3.AddElement(GetNewParag("...................................................               ...........................", font8, 1, 10));
                cellAry = new PdfPCell[] { cell1, cell2, cell3 };
                row = new PdfPRow(cellAry);
                tableSign.Rows.Add(row);

                cell1 = GetNewCell(false);
                cell2 = GetNewCell(false);
                cell3 = GetNewCell(false);
                cell1.Border = PdfPCell.LEFT_BORDER;
                cell2.Border = PdfPCell.LEFT_BORDER;
                cell3.Border = PdfPCell.LEFT_BORDER | PdfPCell.RIGHT_BORDER;
                cell1.AddElement(GetNewParag("วันที่ (Date)......................................................................................................", font8, 0, 10));
                cell2.AddElement(GetNewParag("    ผู้รับสินค้า                          วันที่", font8, 1, 10));
                cell3.AddElement(GetNewParag("     ผู้ส่งสินค้า ผู้รับเงิน                                 วันที่", font8, 1, 10));
                cellAry = new PdfPCell[] { cell1, cell2, cell3 };
                row = new PdfPRow(cellAry);
                tableSign.Rows.Add(row);

                cell1 = GetNewCell(false);
                cell2 = GetNewCell(false);
                cell3 = GetNewCell(false);
                cell1.Border = PdfPCell.LEFT_BORDER | PdfPCell.BOTTOM_BORDER;
                cell2.Border = PdfPCell.LEFT_BORDER | PdfPCell.BOTTOM_BORDER;
                cell3.Border = PdfPCell.LEFT_BORDER | PdfPCell.RIGHT_BORDER | PdfPCell.BOTTOM_BORDER;
                cell1.AddElement(GetNewParag(" ", font8, 0, 10));
                cell2.AddElement(GetNewParag(" Received by                        Date", font8, 1, 10));
                cell3.AddElement(GetNewParag(" Delivered & Received by                          Date", font8, 1, 10));
                cellAry = new PdfPCell[] { cell1, cell2, cell3 };
                row = new PdfPRow(cellAry);
                tableSign.Rows.Add(row);

                widths = new float[] { 37f, 28f, 31f };
                tableSign.SetWidths(widths);
                doc.Add(tableSign);
                #endregion

                //End Generate
            }
            catch (Exception ex)
            {
                error = ex.Message;
            }
            finally
            {
                doc.Close();
                writer.Close();
            }

            string FileName = "Report_" + Header.SaleNumber;
            Response.Clear();
            Response.Buffer = false;
            Response.ContentEncoding = Encoding.UTF8;
            Response.HeaderEncoding = Encoding.UTF8;
            Response.AddHeader("content-disposition", "attachment;filename=" + FileName + ".pdf");
            Response.ContentType = "application/pdf";
            Response.BinaryWrite(ms.ToArray());
            Response.Flush();
            Response.SuppressContent = true;

            return error;
        }

        //Version 1.0
        public string ExportPDFBillVatFromMonthly(List<ReportSaleMonthDTO> lst)//Model.SaleHeaderDTO Header, List<Model.SaleDetailMonthlyDTO> Detail)
        {
            //string filename = "/ExportPDF/MyFirstPDF.pdf";            
            string error = "";
            MemoryStream ms = new MemoryStream();
            //Rectangle A5 = new RectangleReadOnly(595, 450);
            Document doc = new Document(new RectangleReadOnly(595, 545.0f), 3, 3, 3, 0);
            //doc.SetPageSize(PageSize.A5.Rotate());
            //var output = new FileStream(Server.MapPath(filename), FileMode.Create);
            var writer = PdfWriter.GetInstance(doc, ms);
            doc.Open();
            try
            {
                BaseColor bc = new BaseColor(255, 255, 255);
                float[] widths;
                //string fontFamily = "C:/Windows/Fonts/Tahoma.ttf";
                string fontFamily = Server.MapPath("~") + "Fonts\\CSChatThai.ttf";
                BaseFont bf = BaseFont.CreateFont(fontFamily, BaseFont.IDENTITY_H, BaseFont.EMBEDDED);
                //Font fontHeaderGrid = new Font(bf, 12, iTextSharp.text.Font.BOLD, BaseColor.WHITE);
                Font fontGrid = new Font(bf, 9);
                Font fontDetail = new Font(bf, 11);
                Font fontDetailOld = new Font(bf, 10);
                Font fontAdd = new Font(bf, 9);
                Font fontAddB = new Font(bf, 9, 1);
                PdfPTable tableHead = new PdfPTable(3);
                PdfPTable tableDetail = new PdfPTable(8);
                PdfPTable tableRemark = new PdfPTable(2);
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
                PdfPCell cell20 = new PdfPCell();
                PdfPCell cell21 = new PdfPCell();
                tableHead.WidthPercentage = 99;
                tableDetail.WidthPercentage = 99;

                #region Header
                //Row Blank
                cell1 = GetNewCell(false);
                cell2 = GetNewCell(false);
                cell3 = GetNewCell(false);
                cell1.AddElement(new Paragraph(" ", fontDetailOld));
                cellAry = new PdfPCell[] { cell1, cell2, cell3 };
                row = new PdfPRow(cellAry);
                tableHead.Rows.Add(row);
                tableHead.Rows.Add(row);
                tableHead.Rows.Add(row);
                tableHead.Rows.Add(row);
                tableHead.Rows.Add(row);
                tableHead.Rows.Add(row);
                tableHead.Rows.Add(row);
                tableHead.Rows.Add(row);
                tableHead.Rows.Add(row);
                doc.Add(tableHead);

                #endregion

                string Add1 = "", Add2 = "";
                Add1 = lst[0].CustomerAddress;
                Add2 = lst[0].CustomerDistrict + " " + lst[0].CustomerCountry + " " + lst[0].CustomerProvince + " " + lst[0].CustomerPostalCode;

                PdfContentByte SaleNo = writer.DirectContent;
                PlaceText(SaleNo, lst[0].SaleNumber, fontAdd, 330, 373, 420, 454, 14, Element.ALIGN_LEFT);
                PdfContentByte Date = writer.DirectContent;
                PlaceText(Date, lst[0].ReceivedDateStr, fontAdd, 460, 373, 550, 454, 14, Element.ALIGN_LEFT);
                PdfContentByte CustName = writer.DirectContent;
                PlaceText(CustName, lst[0].CustomerName, fontAdd, 75, 369, 400, 434, 14, Element.ALIGN_LEFT);
                PdfContentByte CustAdd = writer.DirectContent;
                PlaceText(CustAdd, Add1, fontAdd, 75, 350, 400, 419, 14, Element.ALIGN_LEFT);
                PdfContentByte CustAdd2 = writer.DirectContent;
                PlaceText(CustAdd2, Add2, fontAdd, 75, 330, 400, 405, 14, Element.ALIGN_LEFT);

                #region Detail & Summary
                string[] spl, splAmt;
                string sn = "", RealSN = "", ItemDesc = "", ItemDescAmt = "";
                double pItem = 0, pVat = 0, pTotal = 0, Discount = 0;
                foreach (ReportSaleMonthDTO item in lst)
                {
                    ItemDesc = "";
                    ItemDescAmt = "";

                    cell1 = GetNewCell(false);
                    cell2 = GetNewCell(false);
                    cell3 = GetNewCell(false);
                    cell4 = GetNewCell(false);
                    cell5 = GetNewCell(false);
                    cell6 = GetNewCell(false);
                    cell7 = GetNewCell(false);
                    cell8 = GetNewCell(false);

                    cell2.AddElement(GetNewParag(item.ItemCode, fontAdd));
                    cell3.AddElement(GetNewParag(item.ItemName, fontAdd));
                    cell4.AddElement(GetNewParag(item.AmountStr, fontAdd, 1));
                    cell5.AddElement(GetNewParag(item.TotalExVatAmountStr, fontAdd, 2));
                    cell7.AddElement(GetNewParag(item.TotalExVatStr, fontAdd, 2));
                    cellAry = new PdfPCell[] { cell1, cell2, cell3, cell4, cell5, cell6, cell7, cell8 };
                    row = new PdfPRow(cellAry);
                    widths = new float[] { 5f, 34f, 115f, 26f, 30f, 20f, 30f, 12f };

                    tableDetail.SetWidths(widths);
                    tableDetail.Rows.Add(row);

                    if (item.ItemDescription != null)
                    {
                        spl = item.ItemDescription.Split(new string[] { "\r\n" }, StringSplitOptions.None);
                        if (spl != null && spl.Length > 0)
                        {
                            foreach (string s in spl)
                            {
                                cell1 = GetNewCell(false);
                                cell2 = GetNewCell(false);
                                cell3 = GetNewCell(false);
                                cell4 = GetNewCell(false);
                                cell5 = GetNewCell(false);
                                cell6 = GetNewCell(false);
                                cell7 = GetNewCell(false);
                                cell8 = GetNewCell(false);

                                splAmt = s.Split(new string[] { "จำนวน" }, StringSplitOptions.None);
                                if (splAmt != null && splAmt.Length > 0)
                                {
                                    ItemDesc = splAmt[0];
                                    if (splAmt.Length > 1)
                                        ItemDescAmt = splAmt[1].Trim();
                                    else
                                        ItemDescAmt = "";

                                    cell3.AddElement(GetNewParag(ItemDesc, fontAdd));
                                    cell4.AddElement(GetNewParag(ItemDescAmt, fontAdd, 1));
                                }

                                cellAry = new PdfPCell[] { cell1, cell2, cell3, cell4, cell5, cell6, cell7, cell8 };
                                row = new PdfPRow(cellAry);
                                widths = new float[] { 5f, 34f, 115f, 26f, 30f, 20f, 30f, 12f };
                                tableDetail.SetWidths(widths);
                                tableDetail.Rows.Add(row);
                            }
                        }
                    }

                    RealSN = RealSN + item.SerialNumber;
                    sn = sn + item.SerialNumber + ", ";
                    pTotal = pTotal + item.TotalExVat;
                    pVat = pVat + item.VATAmount;
                    Discount = Discount + item.Discount;
                }

                #region Discount
                //if(Discount > 0)
                //{
                //    cell1 = GetNewCell(false);
                //    cell2 = GetNewCell(false);
                //    cell3 = GetNewCell(false);
                //    cell4 = GetNewCell(false);
                //    cell5 = GetNewCell(false);
                //    cell6 = GetNewCell(false);
                //    cell7 = GetNewCell(false);
                //    cell8 = GetNewCell(false);

                //    cell3.AddElement(GetNewParag("ส่วนลด         ", fontAdd, 1));
                //    cell7.AddElement(GetNewParag(Discount.ToString("###,##0.00"), fontAdd, 2));
                //    cellAry = new PdfPCell[] { cell1, cell2, cell3, cell4, cell5, cell6, cell7, cell8 };
                //    row = new PdfPRow(cellAry);
                //    widths = new float[] { 7f, 34f, 115f, 26f, 30f, 20f, 30f, 12f };
                //    tableDetail.SetWidths(widths);
                //    tableDetail.Rows.Add(row);
                //}                
                #endregion

                #region Remark
                if (lst[0].Remark != "")
                {
                    cell1 = GetNewCell(false);
                    cell2 = GetNewCell(false);
                    cell3 = GetNewCell(false, 0);
                    cell4 = GetNewCell(false);
                    cell5 = GetNewCell(false);
                    cell6 = GetNewCell(false);
                    cell7 = GetNewCell(false);
                    cell8 = GetNewCell(false);

                    cell20 = GetNewCell(false, 0);
                    cell21 = GetNewCell(false, 0);
                    //cell3.HorizontalAlignment = PdfPCell.ALIGN_LEFT;
                    //cell20.HorizontalAlignment = PdfPCell.ALIGN_LEFT;

                    cell20.AddElement(new Chunk("หมายเหตุ : ", fontAddB));
                    //cell20.AddElement(GetNewParag("หมายเหตุ : ", fontAddB));
                    cell21.AddElement(GetNewParag(lst[0].Remark, fontAdd));
                    cellAry = new PdfPCell[] { cell20, cell21 };
                    row = new PdfPRow(cellAry);
                    widths = new float[] { 50f, 180f };
                    tableRemark.SetWidths(widths);
                    tableRemark.Rows.Add(row);
                    tableRemark.HorizontalAlignment = PdfPCell.ALIGN_LEFT;
                    cell3.AddElement(tableRemark);

                    //cell3.AddElement(GetNewParag("หมายเหตุ : ", fontAddB));
                    //cell3.AddElement(GetNewParag(Header.Remark, fontAdd));
                    cellAry = new PdfPCell[] { cell1, cell2, cell3, cell4, cell5, cell6, cell7, cell8 };
                    row = new PdfPRow(cellAry);
                    widths = new float[] { 5f, 34f, 115f, 26f, 30f, 20f, 30f, 12f };

                    tableDetail.SetWidths(widths);
                    tableDetail.Rows.Add(row);
                }
                #endregion

                #region S/N
                if (!string.IsNullOrEmpty(RealSN))
                {
                    cell1 = GetNewCell(false);
                    cell2 = GetNewCell(false);
                    cell3 = GetNewCell(false);
                    cell4 = GetNewCell(false);
                    cell5 = GetNewCell(false);
                    cell6 = GetNewCell(false);
                    cell7 = GetNewCell(false);
                    cell8 = GetNewCell(false);

                    sn = sn.Substring(0, sn.Length - 2);
                    cell3.AddElement(GetNewParag("S/N : " + sn, fontAdd));
                    cellAry = new PdfPCell[] { cell1, cell2, cell3, cell4, cell5, cell6, cell7, cell8 };
                    row = new PdfPRow(cellAry);
                    widths = new float[] { 5f, 34f, 115f, 26f, 30f, 20f, 30f, 12f };

                    tableDetail.SetWidths(widths);
                    tableDetail.Rows.Add(row);
                }
                #endregion

                //pTotal = pTotal - Discount;
                pItem = pTotal + pVat;
                doc.Add(tableDetail);

                PdfContentByte PriceItem = writer.DirectContent;
                PlaceText(PriceItem, pTotal.ToString("###,##0.00"), fontAdd, 480, 102, 563, 131, 0, Element.ALIGN_RIGHT);
                PdfContentByte PriceVat = writer.DirectContent;
                PlaceText(PriceVat, pVat.ToString("###,##0.00"), fontAdd, 480, 89, 563, 114, 0, Element.ALIGN_RIGHT);
                PdfContentByte PriceTotal = writer.DirectContent;
                PlaceText(PriceTotal, pItem.ToString("###,##0.00"), fontAdd, 480, 76, 563, 97, 0, Element.ALIGN_RIGHT);
                PdfContentByte Blank = writer.DirectContent;
                PlaceText(Blank, "  ", fontDetail, 480, 5, 563, 12, 0, Element.ALIGN_RIGHT);
                #endregion

                //End Generate
            }
            catch (Exception ex)
            {
                error = ex.Message;
            }
            finally
            {
                doc.Close();
                writer.Close();
            }

            string FileName = "Report_" + lst[0].SaleNumber;
            Response.Clear();
            Response.Buffer = false;
            Response.ContentEncoding = Encoding.UTF8;
            Response.HeaderEncoding = Encoding.UTF8;
            Response.AddHeader("content-disposition", "attachment;filename=" + FileName + ".pdf");
            Response.ContentType = "application/pdf";
            Response.BinaryWrite(ms.ToArray());
            Response.Flush();
            Response.SuppressContent = true;

            return error;
        }

        //Version 2.0
        public string ExportPDFManyBillVatFromMonthly(List<ReportSaleMonthDTO> lst)//Model.SaleHeaderDTO Header, List<Model.SaleDetailMonthlyDTO> Detail)
        {          
            string error = "";
            MemoryStream ms = new MemoryStream();
            Document doc = new Document();
            doc.SetPageSize(PageSize.A4);
            var writer = PdfWriter.GetInstance(doc, ms);
            doc.Open();
            doc.SetMargins(36f, 36f, 10f, 10f);
            try
            {
                #region Variable
                BaseColor bc = new BaseColor(255, 255, 255);
                float[] widths;                
                string fontFamily = Server.MapPath("~") + "Fonts\\CSChatThai.ttf";
                BaseFont bf = BaseFont.CreateFont(fontFamily, BaseFont.IDENTITY_H, BaseFont.EMBEDDED);
                Font fontComName18 = new Font(bf, 17, iTextSharp.text.Font.BOLD);
                Font fontComName = new Font(bf, 16, iTextSharp.text.Font.BOLD);
                Font font5 = new Font(bf, 5);
                Font font12 = new Font(bf, 12);
                Font font12B = new Font(bf, 12, iTextSharp.text.Font.BOLD);
                Font font12BU = new Font(bf, 12, iTextSharp.text.Font.BOLD | iTextSharp.text.Font.UNDERLINE);
                Font font11 = new Font(bf, 11);
                Font font11B = new Font(bf, 11, iTextSharp.text.Font.BOLD);
                Font font8 = new Font(bf, 8);
                PdfPTable tableHead = new PdfPTable(3);
                PdfPTable tableHead2 = new PdfPTable(6);
                PdfPTable tableDetail = new PdfPTable(2);
                PdfPTable tableDetail2 = new PdfPTable(6);
                PdfPTable tableRemark = new PdfPTable(2);
                PdfPTable tableSummary = new PdfPTable(3);
                PdfPTable tableSign = new PdfPTable(3);
                PdfPCell[] cellAry;
                PdfPRow row;
                PdfPCell cell1 = new PdfPCell();
                PdfPCell cell2 = new PdfPCell();
                PdfPCell cell3 = new PdfPCell();
                PdfPCell cell4 = new PdfPCell();
                PdfPCell cell5 = new PdfPCell();
                PdfPCell cell6 = new PdfPCell();
                PdfPCell cell20 = new PdfPCell();
                PdfPCell cell21 = new PdfPCell();
                tableHead.WidthPercentage = 99;
                tableHead2.WidthPercentage = 99;
                tableDetail.WidthPercentage = 99;
                tableDetail2.WidthPercentage = 99;
                tableSummary.WidthPercentage = 99;
                tableSign.WidthPercentage = 99;
                int count = 0, countRow = 0;
                string Add1 = "", Add2 = "", SaleNum = "";
                string[] spl, splAmt;
                List<Int32> lstHeaderID = new List<Int32>();
                string sn = "", RealSN = "", ItemDesc = "", ItemDescAmt = "";
                double pItem = 0, pVat = 0, pTotal = 0;//, Discount = 0;
                List<ReportSaleMonthDTO> lstDetail = new List<ReportSaleMonthDTO>();
                ReportSaleMonthDTO header = new ReportSaleMonthDTO();
                #endregion

                //Header
                lstHeaderID = lst.Select(s => s.HeaderID).Distinct().OrderBy(od => od).ToList();
                foreach (Int32 HeaderID in lstHeaderID)
                {
                    header = lst.FirstOrDefault(w => w.HeaderID.Equals(HeaderID));
                    if (header != null)
                    {
                        #region Reset
                        tableHead = new PdfPTable(3);
                        tableHead2 = new PdfPTable(6);
                        tableDetail = new PdfPTable(2);
                        tableDetail2 = new PdfPTable(6);
                        tableRemark = new PdfPTable(2);
                        tableSummary = new PdfPTable(3);
                        tableSign = new PdfPTable(3);

                        tableHead.WidthPercentage = 99;
                        tableHead2.WidthPercentage = 99;
                        tableDetail.WidthPercentage = 99;
                        tableDetail2.WidthPercentage = 99;
                        tableSummary.WidthPercentage = 99;
                        tableSign.WidthPercentage = 99;

                        pItem = 0;
                        pVat = 0;
                        pTotal = 0;
                        countRow = 0;

                        sn = "";
                        RealSN = "";
                        #endregion

                        #region Manage Page (New Page)
                        if (count == 0)
                            doc.Open();
                        else
                            doc.NewPage();
                        #endregion

                        #region Header
                        cell1 = GetNewCell(false);
                        cell3 = GetNewCell(false);
                        cell2 = GetNewCell(false);

                        cell2.AddElement(GetNewParag("บริษัท โปร เวลดิ้ง แอนด์ ทูลส์ จำกัด", fontComName18, 1, 18));
                        cellAry = new PdfPCell[] { cell1, cell2, cell3 };
                        row = new PdfPRow(cellAry);
                        tableHead.Rows.Add(row);

                        cell2 = GetNewCell(false);
                        cell2.AddElement(GetNewParag("PRO WELDING & TOOLS CO., LTD.", fontComName, 1, 18));
                        cellAry = new PdfPCell[] { cell1, cell2, cell3 };
                        row = new PdfPRow(cellAry);
                        tableHead.Rows.Add(row);

                        cell2 = GetNewCell(false);
                        cell2.AddElement(GetNewParag("9/1 ซอยบางบอน 4 (ซอย 14 แยก 1) ถนนบางบอน 4 แขวงบางบอนเหนือ เขตบางบอน กรุงเทพฯ", font12, 1, 10));
                        cellAry = new PdfPCell[] { cell1, cell2, cell3 };
                        row = new PdfPRow(cellAry);
                        tableHead.Rows.Add(row);

                        cell2 = GetNewCell(false);
                        cell2.AddElement(GetNewParag("9/1 Soi Bangbon 4 (Soi 14 Yeak 1) Bangbon 4 Rd., Kweang Bangbon Nua, Khet Bangbon, Bangkok", font12, 1, 10));
                        cellAry = new PdfPCell[] { cell1, cell2, cell3 };
                        row = new PdfPRow(cellAry);
                        tableHead.Rows.Add(row);

                        cell2 = GetNewCell(false);
                        cell2.AddElement(GetNewParag("TAX ID: 0105560093156 (สำนักงานใหญ่) Tel. 02-297-0744, 086-909-4777, 098-747-9932", font12, 1, 10));
                        cellAry = new PdfPCell[] { cell1, cell2, cell3 };
                        row = new PdfPRow(cellAry);
                        tableHead.Rows.Add(row);

                        cell1 = GetNewCell(false);
                        cell2 = GetNewCell(false, 1);
                        cell3 = GetNewCell(false);
                        cell2.AddElement(GetNewParag(" ", font5, 1));
                        cell1.Border = PdfPCell.BOTTOM_BORDER;
                        cell2.Border = PdfPCell.BOTTOM_BORDER;
                        cell3.Border = PdfPCell.BOTTOM_BORDER;
                        cellAry = new PdfPCell[] { cell1, cell2, cell3 };
                        row = new PdfPRow(cellAry);
                        tableHead.Rows.Add(row);
                        widths = new float[] { 20f, 160f, 20f };
                        tableHead.SetWidths(widths);
                        doc.Add(tableHead);
                        #endregion

                        #region Header2
                        SaleNum = header.SaleNumber;
                        cell1 = GetNewCell(false);
                        cell2 = GetNewCell(false);
                        cell3 = GetNewCell(false);
                        cell4 = GetNewCell(false);
                        cell5 = GetNewCell(false);
                        cell6 = GetNewCell(false);
                        cell1.Rowspan = 2;
                        cell2.Rowspan = 2;
                        cell3.Rowspan = 2;
                        cell4.Rowspan = 2;
                        cell5.Rowspan = 2;
                        cell1.VerticalAlignment = Element.ALIGN_MIDDLE;
                        cell2.VerticalAlignment = Element.ALIGN_MIDDLE;
                        cell3.VerticalAlignment = Element.ALIGN_MIDDLE;
                        cell4.VerticalAlignment = Element.ALIGN_MIDDLE;
                        cell5.VerticalAlignment = Element.ALIGN_MIDDLE;

                        cell1.AddElement(GetNewParag("วันที่", font12B, 1, 10));
                        cell2.AddElement(GetNewParag(header.ReceivedDateStr, font12, 0, 10));
                        cell3.AddElement(GetNewParag("เลขที่", font12B, 1, 10));
                        cell4.AddElement(GetNewParag(SaleNum, font12, 0, 10));
                        cell5.AddElement(GetNewParag("Original", font12B, 1, 10));
                        cell6.AddElement(GetNewParag("ใบกำกับภาษี / ใบส่งของ / ใบเสร็จรับเงิน", font12BU, 1, 10));
                        cellAry = new PdfPCell[] { cell1, cell2, cell3, cell4, cell5, cell6 };
                        row = new PdfPRow(cellAry);
                        tableHead2.Rows.Add(row);

                        cell1 = GetNewCell(false);
                        cell2 = GetNewCell(false);
                        cell3 = GetNewCell(false);
                        cell4 = GetNewCell(false);
                        cell5 = GetNewCell(false);
                        cell6 = GetNewCell(false);
                        cell6.AddElement(GetNewParag("Tax Invoice / Delivery Order / Receipt", font12B, 1, 10));
                        cellAry = new PdfPCell[] { cell1, cell2, cell3, cell4, cell5, cell6 };
                        row = new PdfPRow(cellAry);
                        tableHead2.Rows.Add(row);

                        cell1 = GetNewCell(false);
                        cell2 = GetNewCell(false);
                        cell3 = GetNewCell(false);
                        cell4 = GetNewCell(false);
                        cell5 = GetNewCell(false);
                        cell6 = GetNewCell(false);
                        cell6.AddElement(GetNewParag(" ", font5, 0, 1));
                        cellAry = new PdfPCell[] { cell1, cell2, cell3, cell4, cell5, cell6 };
                        row = new PdfPRow(cellAry);
                        tableHead2.Rows.Add(row);
                        widths = new float[] { 10f, 40f, 10f, 35f, 20f, 60f };
                        tableHead2.SetWidths(widths);
                        doc.Add(tableHead2);
                        #endregion

                        #region Name & Address
                        Add1 = header.CustomerAddress;// +" " + header.CustomerDistrict;
                        Add2 = header.CustomerDistrict + " " + header.CustomerCountry + " " + header.CustomerProvince + " " + header.CustomerPostalCode;

                        cell1 = GetNewCell(false);
                        cell2 = GetNewCell(false);
                        cell3 = GetNewCell(false);
                        cell4 = GetNewCell(false);
                        cell5 = GetNewCell(false);
                        cell6 = GetNewCell(false);

                        cell1.Border = PdfPCell.LEFT_BORDER | PdfPCell.TOP_BORDER;
                        cell2.Border = PdfPCell.RIGHT_BORDER | PdfPCell.TOP_BORDER;
                        cell1.AddElement(GetNewParag("Buyer / ผู้ซื้อ", font11, 0, 10));
                        cell2.AddElement(GetNewParag(header.CustomerName, font11, 0, 10));
                        cellAry = new PdfPCell[] { cell1, cell2 };
                        row = new PdfPRow(cellAry);
                        tableDetail.Rows.Add(row);

                        cell1 = GetNewCell(false);
                        cell2 = GetNewCell(false);
                        cell1.Border = PdfPCell.LEFT_BORDER;
                        cell2.Border = PdfPCell.RIGHT_BORDER;
                        cell1.AddElement(GetNewParag("Address / ที่อยู่", font11, 0, 10));
                        cell2.AddElement(GetNewParag(Add1, font11, 0, 10));
                        cellAry = new PdfPCell[] { cell1, cell2 };
                        row = new PdfPRow(cellAry);
                        tableDetail.Rows.Add(row);

                        cell1 = GetNewCell(false);
                        cell2 = GetNewCell(false);
                        cell1.Border = PdfPCell.LEFT_BORDER;
                        cell2.Border = PdfPCell.RIGHT_BORDER;
                        cell1.AddElement(GetNewParag("  ", font11));
                        cell2.AddElement(GetNewParag(Add2, font11, 0, 10));
                        cellAry = new PdfPCell[] { cell1, cell2 };
                        row = new PdfPRow(cellAry);
                        tableDetail.Rows.Add(row);
                        widths = new float[] { 10f, 60f };
                        tableDetail.SetWidths(widths);
                        doc.Add(tableDetail);
                        #endregion

                        #region Table Header
                        cell1 = GetNewCell(false);
                        cell2 = GetNewCell(false);
                        cell3 = GetNewCell(false);
                        cell4 = GetNewCell(false);
                        cell5 = GetNewCell(false);
                        cell6 = GetNewCell(false);
                        cell1.Border = PdfPCell.LEFT_BORDER | PdfPCell.TOP_BORDER | PdfPCell.BOTTOM_BORDER;
                        cell2.Border = PdfPCell.LEFT_BORDER | PdfPCell.TOP_BORDER | PdfPCell.BOTTOM_BORDER;
                        cell3.Border = PdfPCell.LEFT_BORDER | PdfPCell.TOP_BORDER | PdfPCell.BOTTOM_BORDER;
                        cell4.Border = PdfPCell.LEFT_BORDER | PdfPCell.TOP_BORDER | PdfPCell.BOTTOM_BORDER;
                        cell5.Border = PdfPCell.LEFT_BORDER | PdfPCell.TOP_BORDER | PdfPCell.BOTTOM_BORDER;
                        cell6.Border = PdfPCell.LEFT_BORDER | PdfPCell.TOP_BORDER | PdfPCell.BOTTOM_BORDER | PdfPCell.RIGHT_BORDER;
                        cell1.AddElement(GetNewParag("รหัสสินค้า", font12, 1));
                        cell2.AddElement(GetNewParag("รายการ", font12, 1));
                        cell3.AddElement(GetNewParag("จำนวน", font12, 1));
                        cell4.AddElement(GetNewParag("หน่วยละ", font12, 1));
                        cell5.AddElement(GetNewParag("ส่วนลด %", font12, 1));
                        cell6.AddElement(GetNewParag("จำนวนเงิน", font12, 1));
                        cellAry = new PdfPCell[] { cell1, cell2, cell3, cell4, cell5, cell6 };
                        row = new PdfPRow(cellAry);
                        tableDetail2.Rows.Add(row);
                        #endregion

                        #region Item Detail & Process Summary
                        lstDetail = lst.Where(w => w.HeaderID.Equals(HeaderID)).OrderBy(od => od.DetailID).ToList();
                        if (lstDetail != null && lstDetail.Count > 0)
                        {
                            foreach (ReportSaleMonthDTO item in lstDetail)
                            {
                                #region Reset
                                ItemDesc = "";
                                ItemDescAmt = "";

                                cell1 = GetNewCell(false);
                                cell2 = GetNewCell(false);
                                cell3 = GetNewCell(false);
                                cell4 = GetNewCell(false);
                                cell5 = GetNewCell(false);
                                cell6 = GetNewCell(false);
                                cell1.Border = PdfPCell.LEFT_BORDER;
                                cell2.Border = PdfPCell.LEFT_BORDER;
                                cell3.Border = PdfPCell.LEFT_BORDER;
                                cell4.Border = PdfPCell.LEFT_BORDER;
                                cell5.Border = PdfPCell.LEFT_BORDER;
                                cell6.Border = PdfPCell.LEFT_BORDER | PdfPCell.RIGHT_BORDER;
                                #endregion

                                //Start
                                cell1.AddElement(GetNewParag(item.ItemCode, font11));
                                cell2.AddElement(GetNewParag(item.ItemName, font11));
                                cell3.AddElement(GetNewParag(item.AmountStr, font11, 1));
                                cell4.AddElement(GetNewParag(item.TotalExVatAmountStr, font11, 2));
                                cell6.AddElement(GetNewParag(item.TotalExVatStr, font11, 2));
                                cellAry = new PdfPCell[] { cell1, cell2, cell3, cell4, cell5, cell6 };
                                row = new PdfPRow(cellAry);
                                tableDetail2.Rows.Add(row);
                                countRow++;

                                #region Item Description
                                if (!string.IsNullOrEmpty(item.ItemDescription))
                                {
                                    spl = item.ItemDescription.Split(new string[] { "\r\n" }, StringSplitOptions.None);
                                    if (spl != null && spl.Length > 0)
                                    {
                                        foreach (string s in spl)
                                        {
                                            #region Reset
                                            cell1 = GetNewCell(false);
                                            cell2 = GetNewCell(false);
                                            cell3 = GetNewCell(false);
                                            cell4 = GetNewCell(false);
                                            cell5 = GetNewCell(false);
                                            cell6 = GetNewCell(false);
                                            cell1.Border = PdfPCell.LEFT_BORDER;
                                            cell2.Border = PdfPCell.LEFT_BORDER;
                                            cell3.Border = PdfPCell.LEFT_BORDER;
                                            cell4.Border = PdfPCell.LEFT_BORDER;
                                            cell5.Border = PdfPCell.LEFT_BORDER;
                                            cell6.Border = PdfPCell.LEFT_BORDER | PdfPCell.RIGHT_BORDER;
                                            #endregion

                                            splAmt = s.Split(new string[] { "จำนวน" }, StringSplitOptions.None);
                                            if (splAmt != null && splAmt.Length > 0)
                                            {
                                                ItemDesc = splAmt[0];
                                                if (splAmt.Length > 1)
                                                    ItemDescAmt = splAmt[1].Trim();
                                                else
                                                    ItemDescAmt = "";

                                                cell2.AddElement(GetNewParag(ItemDesc, font11));
                                                cell3.AddElement(GetNewParag(ItemDescAmt, font11, 1));
                                            }

                                            cellAry = new PdfPCell[] { cell1, cell2, cell3, cell4, cell5, cell6 };
                                            row = new PdfPRow(cellAry);
                                            tableDetail2.Rows.Add(row);
                                            countRow++;
                                        }
                                    }
                                }
                                #endregion

                                RealSN = RealSN + item.SerialNumber;
                                sn = sn + item.SerialNumber + ", ";
                                pItem = pItem + item.TotalExVat;
                                pVat = pVat + item.VATAmount;
                                //Discount = Discount + item.Discount;

                            }

                            #region Remark
                            //if (header.Remark != "")
                            //{
                            //    #region Reset
                            //    cell1 = GetNewCell(false);
                            //    cell2 = GetNewCell(false);
                            //    cell3 = GetNewCell(false);
                            //    cell4 = GetNewCell(false);
                            //    cell5 = GetNewCell(false);
                            //    cell6 = GetNewCell(false);
                            //    cell1.Border = PdfPCell.LEFT_BORDER;
                            //    cell2.Border = PdfPCell.LEFT_BORDER;
                            //    cell3.Border = PdfPCell.LEFT_BORDER;
                            //    cell4.Border = PdfPCell.LEFT_BORDER;
                            //    cell5.Border = PdfPCell.LEFT_BORDER;
                            //    cell6.Border = PdfPCell.LEFT_BORDER | PdfPCell.RIGHT_BORDER;

                            //    cell20 = GetNewCell(false);
                            //    cell21 = GetNewCell(false);
                            //    #endregion

                            //    cell20.AddElement(new Chunk("หมายเหตุ : ", font11B));
                            //    cell21.AddElement(GetNewParag(header.Remark, font11));
                            //    cellAry = new PdfPCell[] { cell20, cell21 };
                            //    row = new PdfPRow(cellAry);
                            //    widths = new float[] { 50f, 180f };
                            //    tableRemark.SetWidths(widths);
                            //    tableRemark.Rows.Add(row);
                            //    tableRemark.HorizontalAlignment = PdfPCell.ALIGN_LEFT;
                            //    cell2.AddElement(tableRemark);

                            //    cellAry = new PdfPCell[] { cell1, cell2, cell3, cell4, cell5, cell6 };
                            //    row = new PdfPRow(cellAry);
                            //    tableDetail2.Rows.Add(row);
                            //    countRow++;
                            //}
                            #endregion

                            #region S/N
                            if (!string.IsNullOrEmpty(RealSN))
                            {
                                #region Reset
                                cell1 = GetNewCell(false);
                                cell2 = GetNewCell(false);
                                cell3 = GetNewCell(false);
                                cell4 = GetNewCell(false);
                                cell5 = GetNewCell(false);
                                cell6 = GetNewCell(false);
                                cell1.Border = PdfPCell.LEFT_BORDER;
                                cell2.Border = PdfPCell.LEFT_BORDER;
                                cell3.Border = PdfPCell.LEFT_BORDER;
                                cell4.Border = PdfPCell.LEFT_BORDER;
                                cell5.Border = PdfPCell.LEFT_BORDER;
                                cell6.Border = PdfPCell.LEFT_BORDER | PdfPCell.RIGHT_BORDER;
                                #endregion

                                sn = sn.Substring(0, sn.Length - 2);
                                cell2.AddElement(GetNewParag("S/N : " + sn, font11));
                                cellAry = new PdfPCell[] { cell1, cell2, cell3, cell4, cell5, cell6 };
                                row = new PdfPRow(cellAry);
                                tableDetail2.Rows.Add(row);
                                countRow++;
                            }
                            #endregion

                            pTotal = pItem + pVat;
                        }

                        #region Empty Row & Bottom
                        for (int i = countRow; i < 19; i++)
                        {
                            cell1 = GetNewCell(false);
                            cell2 = GetNewCell(false);
                            cell3 = GetNewCell(false);
                            cell4 = GetNewCell(false);
                            cell5 = GetNewCell(false);
                            cell6 = GetNewCell(false);
                            cell1.Border = PdfPCell.LEFT_BORDER;
                            cell2.Border = PdfPCell.LEFT_BORDER;
                            cell3.Border = PdfPCell.LEFT_BORDER;
                            cell4.Border = PdfPCell.LEFT_BORDER;
                            cell5.Border = PdfPCell.LEFT_BORDER;
                            cell6.Border = PdfPCell.LEFT_BORDER | PdfPCell.RIGHT_BORDER;
                            cell1.AddElement(GetNewParag(" ", font11));
                            cellAry = new PdfPCell[] { cell1, cell2, cell3, cell4, cell5, cell6 };
                            row = new PdfPRow(cellAry);
                            tableDetail2.Rows.Add(row);
                        }

                        cell1 = GetNewCell(false);
                        cell2 = GetNewCell(false);
                        cell3 = GetNewCell(false);
                        cell4 = GetNewCell(false);
                        cell5 = GetNewCell(false);
                        cell6 = GetNewCell(false);
                        cell1.Border = PdfPCell.LEFT_BORDER | PdfPCell.BOTTOM_BORDER;
                        cell2.Border = PdfPCell.LEFT_BORDER | PdfPCell.BOTTOM_BORDER;
                        cell3.Border = PdfPCell.LEFT_BORDER | PdfPCell.BOTTOM_BORDER;
                        cell4.Border = PdfPCell.LEFT_BORDER | PdfPCell.BOTTOM_BORDER;
                        cell5.Border = PdfPCell.LEFT_BORDER | PdfPCell.BOTTOM_BORDER;
                        cell6.Border = PdfPCell.LEFT_BORDER | PdfPCell.RIGHT_BORDER | PdfPCell.BOTTOM_BORDER;
                        cell1.AddElement(GetNewParag(" ", font11));
                        cellAry = new PdfPCell[] { cell1, cell2, cell3, cell4, cell5, cell6 };
                        row = new PdfPRow(cellAry);
                        tableDetail2.Rows.Add(row);

                        widths = new float[] { 15f, 40f, 10f, 10f, 9f, 12f };
                        tableDetail2.SetWidths(widths);
                        doc.Add(tableDetail2);
                        #endregion
                        #endregion

                        #region Summary
                        cell1 = GetNewCell(false);
                        cell2 = GetNewCell(false);
                        cell3 = GetNewCell(false);
                        cell1.Border = PdfPCell.LEFT_BORDER;
                        cell2.Border = PdfPCell.LEFT_BORDER;
                        cell3.Border = PdfPCell.LEFT_BORDER | PdfPCell.RIGHT_BORDER | PdfPCell.BOTTOM_BORDER;
                        cell1.AddElement(GetNewParag("สินค้าที่ออกใบกำกับภาษีเกิน 7 วัน แล้วไม่รับ", font8, 0, 10));
                        cell2.AddElement(GetNewParag("มูลค่าสินค้า / Goods Value", font8, 0, 10));
                        cell3.AddElement(GetNewParag(pItem.ToString("###,##0.00"), font11, 2, 10));
                        cellAry = new PdfPCell[] { cell1, cell2, cell3 };
                        row = new PdfPRow(cellAry);
                        tableSummary.Rows.Add(row);

                        cell1 = GetNewCell(false);
                        cell2 = GetNewCell(false);
                        cell3 = GetNewCell(false);
                        cell1.Border = PdfPCell.LEFT_BORDER;
                        cell2.Border = PdfPCell.LEFT_BORDER;
                        cell3.Border = PdfPCell.LEFT_BORDER | PdfPCell.RIGHT_BORDER | PdfPCell.BOTTOM_BORDER;
                        cell1.AddElement(GetNewParag("ใบเสร็จรับเงินจะสมบูรณ์เมื่อมีลายเซ็นผู้รับเงิน การชำระเงินจะสมบูรณ์เมื่อบริษัทฯ ได้เรียเก็บเงินตามเช็คเรียบร้อยแล้ว", font8, 0, 10));
                        cell2.AddElement(GetNewParag("ภาษีมูลค่าเพิ่ม 7% / VAT 7%", font8, 0, 10));
                        cell3.AddElement(GetNewParag(pVat.ToString("###,##0.00"), font11, 2, 10));
                        cellAry = new PdfPCell[] { cell1, cell2, cell3 };
                        row = new PdfPRow(cellAry);
                        tableSummary.Rows.Add(row);

                        cell1 = GetNewCell(false);
                        cell2 = GetNewCell(false);
                        cell3 = GetNewCell(false);
                        cell1.Border = PdfPCell.LEFT_BORDER | PdfPCell.BOTTOM_BORDER;
                        cell2.Border = PdfPCell.LEFT_BORDER | PdfPCell.BOTTOM_BORDER;
                        cell3.Border = PdfPCell.LEFT_BORDER | PdfPCell.RIGHT_BORDER | PdfPCell.BOTTOM_BORDER;
                        cell1.AddElement(GetNewParag("สินค้าตามใบกำกับภาษียังคงเป็นกรรมสิทธิ์ของทางบริษัทฯ จนกว่าจะได้รับการชำระเงินเป็นที่เรียบร้อยแล้ว                ผิดตกยกเว้น E.&O.E.", font8, 0, 10));
                        cell2.AddElement(GetNewParag("รวมมูลค่า / Total Amount", font8, 0, 10));
                        cell3.AddElement(GetNewParag(pTotal.ToString("###,##0.00"), font11B, 2, 10));
                        cellAry = new PdfPCell[] { cell1, cell2, cell3 };
                        row = new PdfPRow(cellAry);
                        tableSummary.Rows.Add(row);

                        widths = new float[] { 65f, 19f, 12f };
                        tableSummary.SetWidths(widths);
                        doc.Add(tableSummary);
                        #endregion

                        #region Sign
                        //PdfContentByte Box1 = writer.DirectContent;
                        //Box1.Rectangle(52f, 155f, 10f, 15f);
                        //Box1.Stroke();

                        //PdfContentByte Box2 = writer.DirectContent;
                        //Box2.Rectangle(140f, 155f, 10f, 15f);
                        //Box2.Stroke();

                        cell1 = GetNewCell(false);
                        cell2 = GetNewCell(false);
                        cell3 = GetNewCell(false);
                        cell1.Border = PdfPCell.LEFT_BORDER;
                        cell2.Border = PdfPCell.LEFT_BORDER;
                        cell3.Border = PdfPCell.LEFT_BORDER | PdfPCell.RIGHT_BORDER;
                        cell1.AddElement(GetNewParag(" ", font8, 0, 10));
                        cell2.AddElement(GetNewParag(" ", font8, 0, 10));
                        cell3.AddElement(GetNewParag("บริษัท โปร เวลดิ้ง แอนด์ ทูลส์ จำกัด", font8, 1, 10));
                        cellAry = new PdfPCell[] { cell1, cell2, cell3 };
                        row = new PdfPRow(cellAry);
                        tableSign.Rows.Add(row);

                        cell1 = GetNewCell(false);
                        cell2 = GetNewCell(false);
                        cell3 = GetNewCell(false);
                        cell1.Border = PdfPCell.LEFT_BORDER;
                        cell2.Border = PdfPCell.LEFT_BORDER;
                        cell3.Border = PdfPCell.LEFT_BORDER | PdfPCell.RIGHT_BORDER;

                        cell1.AddElement(GetNewParag("           __ เงินสด (Cash)                            __ เช็ค (Cheque)", font8, 0, 5));
                        cell2.AddElement(GetNewParag(" ", font8, 0, 10));
                        cell3.AddElement(GetNewParag("PRO WELDING & TOOLS CO., LTD.", font8, 1, 10));
                        cellAry = new PdfPCell[] { cell1, cell2, cell3 };
                        row = new PdfPRow(cellAry);
                        tableSign.Rows.Add(row);

                        cell1 = GetNewCell(false);
                        cell2 = GetNewCell(false);
                        cell3 = GetNewCell(false);
                        cell1.Border = PdfPCell.LEFT_BORDER;
                        cell2.Border = PdfPCell.LEFT_BORDER;
                        cell3.Border = PdfPCell.LEFT_BORDER | PdfPCell.RIGHT_BORDER;
                        cell1.AddElement(GetNewParag("ธนาคาร (Bank)................................................................................................", font8, 0, 10));
                        cell2.AddElement(GetNewParag(" ", font8, 0, 10));
                        cell3.AddElement(GetNewParag(" ", font8, 1, 10));
                        cellAry = new PdfPCell[] { cell1, cell2, cell3 };
                        row = new PdfPRow(cellAry);
                        tableSign.Rows.Add(row);

                        cell1 = GetNewCell(false);
                        cell2 = GetNewCell(false);
                        cell3 = GetNewCell(false);
                        cell1.Border = PdfPCell.LEFT_BORDER;
                        cell2.Border = PdfPCell.LEFT_BORDER;
                        cell3.Border = PdfPCell.LEFT_BORDER | PdfPCell.RIGHT_BORDER;
                        cell1.AddElement(GetNewParag("เลขที่ (No.)......................................................................................................", font8, 0, 10));
                        cell2.AddElement(GetNewParag(".......................................            ......................", font8, 1, 10));
                        cell3.AddElement(GetNewParag("...................................................               ...........................", font8, 1, 10));
                        cellAry = new PdfPCell[] { cell1, cell2, cell3 };
                        row = new PdfPRow(cellAry);
                        tableSign.Rows.Add(row);

                        cell1 = GetNewCell(false);
                        cell2 = GetNewCell(false);
                        cell3 = GetNewCell(false);
                        cell1.Border = PdfPCell.LEFT_BORDER;
                        cell2.Border = PdfPCell.LEFT_BORDER;
                        cell3.Border = PdfPCell.LEFT_BORDER | PdfPCell.RIGHT_BORDER;
                        cell1.AddElement(GetNewParag("วันที่ (Date)......................................................................................................", font8, 0, 10));
                        cell2.AddElement(GetNewParag("    ผู้รับสินค้า                          วันที่", font8, 1, 10));
                        cell3.AddElement(GetNewParag("     ผู้ส่งสินค้า ผู้รับเงิน                                 วันที่", font8, 1, 10));
                        cellAry = new PdfPCell[] { cell1, cell2, cell3 };
                        row = new PdfPRow(cellAry);
                        tableSign.Rows.Add(row);

                        cell1 = GetNewCell(false);
                        cell2 = GetNewCell(false);
                        cell3 = GetNewCell(false);
                        cell1.Border = PdfPCell.LEFT_BORDER | PdfPCell.BOTTOM_BORDER;
                        cell2.Border = PdfPCell.LEFT_BORDER | PdfPCell.BOTTOM_BORDER;
                        cell3.Border = PdfPCell.LEFT_BORDER | PdfPCell.RIGHT_BORDER | PdfPCell.BOTTOM_BORDER;
                        cell1.AddElement(GetNewParag(" ", font8, 0, 10));
                        cell2.AddElement(GetNewParag(" Received by                        Date", font8, 1, 10));
                        cell3.AddElement(GetNewParag(" Delivered & Received by                          Date", font8, 1, 10));
                        cellAry = new PdfPCell[] { cell1, cell2, cell3 };
                        row = new PdfPRow(cellAry);
                        tableSign.Rows.Add(row);

                        widths = new float[] { 37f, 28f, 31f };
                        tableSign.SetWidths(widths);
                        doc.Add(tableSign);
                        #endregion

                        count++;
                    }
                }                

                //End Generate
            }
            catch (Exception ex)
            {
                error = ex.Message;
            }
            finally
            {
                doc.Close();
                writer.Close();
            }

            string FileName = "Report_" + DateTime.Now.ToString("yyyyMMdd_HHmm");
            Response.Clear();
            Response.Buffer = false;
            Response.ContentEncoding = Encoding.UTF8;
            Response.HeaderEncoding = Encoding.UTF8;
            Response.AddHeader("content-disposition", "attachment;filename=" + FileName + ".pdf");
            Response.ContentType = "application/pdf";
            Response.BinaryWrite(ms.ToArray());
            Response.Flush();
            Response.SuppressContent = true;

            return error;
        }

        protected PdfPCell GetNewCell(bool isBorder, int hAlign = 0)
        {
            PdfPCell c = new PdfPCell();
            try
            {
                if (!isBorder)
                    c.Border = 0;

                switch (hAlign)
                {
                    case 0:
                        c.HorizontalAlignment = Element.ALIGN_LEFT;
                        break;
                    case 1:
                        c.HorizontalAlignment = Element.ALIGN_CENTER;
                        break;
                    case 2:
                        c.HorizontalAlignment = Element.ALIGN_RIGHT;
                        break;
                }
            }
            catch (Exception ex)
            {

            }
            return c;
        }

        protected Paragraph GetNewParag(string str, Font f, int hAlign = 0, float FixLead = 0)
        {
            Paragraph p = new Paragraph();
            try
            {
                p = new Paragraph(str, f);
                switch (hAlign)
                {
                    case 0:
                        p.Alignment = Element.ALIGN_LEFT;
                        break;
                    case 1:
                        p.Alignment = Element.ALIGN_CENTER;
                        break;
                    case 2:
                        p.Alignment = Element.ALIGN_RIGHT;
                        break;
                }

                if(FixLead > 0)
                    p.SetLeading(FixLead, 0f);
            }
            catch (Exception ex)
            {
                
            }
            return p;
        }

        protected void PlaceText(PdfContentByte pdfContentByte
                                , string text
                                , iTextSharp.text.Font font
                                , float lowerLeftx
                                , float lowerLefty
                                , float upperRightx
                                , float upperRighty
                                , float leading
                                , int alignment)
        {
            ColumnText ct = new ColumnText(pdfContentByte);
            ct.SetSimpleColumn(new Phrase(text, font), lowerLeftx, lowerLefty, upperRightx, upperRighty, leading, alignment);            
            ct.Go();            
        }
        #endregion

        #region PDF Bill Cash
        public string ExportPDFBillCash(Model.SaleHeaderDTO Header, List<Model.SaleDetailDTO> Detail)
        {
            //string filename = "/ExportPDF/MyFirstPDF.pdf";            
            string error = "";
            MemoryStream ms = new MemoryStream();
            //Rectangle A5 = new RectangleReadOnly(595, 450);
            //Document doc = new Document(PageSize.A4); //595,842  --> Comment 20181024
            Document doc = new Document(new RectangleReadOnly(595, 445.0f), 15, 30, 20, 10);
            //doc.SetPageSize(PageSize.A5.Rotate());
            //var output = new FileStream(Server.MapPath(filename), FileMode.Create);
            var writer = PdfWriter.GetInstance(doc, ms);
            doc.Open();
            try
            {
                BaseColor bc = new BaseColor(255, 255, 255);
                float[] widths;
                //string fontFamily = "C:/Windows/Fonts/Tahoma.ttf";
                string fontFamily = Server.MapPath("~") + "Fonts\\CSChatThai.ttf";
                BaseFont bf = BaseFont.CreateFont(fontFamily, BaseFont.IDENTITY_H, BaseFont.EMBEDDED);
                //Font fontHeaderGrid = new Font(bf, 12, iTextSharp.text.Font.BOLD, BaseColor.WHITE);
                Font fontGrid = new Font(bf, 9);
                Font fontDetail = new Font(bf, 11);
                Font fontDetailOld = new Font(bf, 10);
                Font fontSummary = new Font(bf, 10, 1);
                Font fontHead = new Font(bf, 18, 1);
                Font fontAdd = new Font(bf, 9);
                Font fontAddB = new Font(bf, 9, 1);
                PdfPTable tableHead = new PdfPTable(3);
                PdfPTable tableHead1 = new PdfPTable(3);
                PdfPTable tableDetail = new PdfPTable(4);
                PdfPTable tableRemark = new PdfPTable(2);
                //PdfPTable tableSum = new PdfPTable(2);
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
                PdfPCell cell20 = new PdfPCell();
                PdfPCell cell21 = new PdfPCell();
                tableHead.WidthPercentage = 99;
                tableHead1.WidthPercentage = 99;
                tableDetail.WidthPercentage = 99;
                PdfPTable tableSpace = new PdfPTable(1);
                PdfPTable tableSpace1 = new PdfPTable(1);

                #region Top
                #region Header Comment
                ////Company Info
                //string Head = "";
                ////Pic
                //string dir = Server.MapPath("~/Image/");
                //Image img = Image.GetInstance(dir + "PRO WELDING logo.jpg");
                //img.SetAbsolutePosition(40, 770);
                //img.ScaleAbsolute(66f, 45f);
                //doc.Add(img);

                ////Info
                //PdfContentByte ComNameTH = writer.DirectContent;
                //Head = "เลขที่ 9/1 ซอยบางบอน 4 (ซอย 14 แยก 1) ถนนบางบอน 4 แขวงบางบอน เขตบางบอน กรุงเทพฯ 10150";
                //PlaceText(ComNameTH, Head, fontDetailOld, 120, 790, 460, 805, 0, Element.ALIGN_LEFT);

                //PdfContentByte ComNameEn = writer.DirectContent;
                //Head = "No. 9/1 Soi Bangbon 4 (Soi 14 Yeak 1) Bangbon 4 Rd., Bangbon, Bangbon, Bangkok, 10150";
                //PlaceText(ComNameTH, Head, fontDetailOld, 120, 780, 460, 795, 0, Element.ALIGN_LEFT);

                //PdfContentByte Tel = writer.DirectContent;
                //Head = "Tel. 098-747-9932, 086-909-4777 Fax. 02-806-6807";
                //PlaceText(Tel, Head, fontDetailOld, 120, 770, 460, 785, 0, Element.ALIGN_LEFT);

                //PdfContentByte Tax = writer.DirectContent;
                //Head = "เลขประจำตัวผู้เสียภาษี 0105560093156 (สำนักงานใหญ่)";
                //PlaceText(Tax, Head, fontDetailOld, 120, 760, 460, 775, 0, Element.ALIGN_LEFT);
                #endregion

                //Add Space
                cell1 = GetNewCell(false);
                cell1.AddElement(new Paragraph(" ", fontDetailOld));
                cellAry = new PdfPCell[] { cell1 };
                row = new PdfPRow(cellAry);
                tableSpace1.Rows.Add(row);
                doc.Add(tableSpace1);

                //Row Blank
                cell1 = GetNewCell(false);
                cell2 = GetNewCell(true);
                cell3 = GetNewCell(false);
                cell2.AddElement(GetNewParag("ใบส่งของ / ใบเสร็จรับเงิน", fontHead, 1));
                cell2.FixedHeight = 40.0f;
                cell2.VerticalAlignment = Element.ALIGN_TOP;
                cellAry = new PdfPCell[] { cell1, cell2, cell3  };
                row = new PdfPRow(cellAry);
                tableHead.Rows.Add(row);
                widths = new float[] { 50f, 70f, 50f };
                tableHead.SetWidths(widths);
                doc.Add(tableHead);

                //Add Space
                cell1 = GetNewCell(false);
                cell1.AddElement(new Paragraph(" ", fontDetailOld));
                cellAry = new PdfPCell[] { cell1 };
                row = new PdfPRow(cellAry);
                tableSpace1.Rows.Add(row);
                doc.Add(tableSpace1);

                cell1 = GetNewCell(true);                               
                cell1.Rowspan = 2;
                cell1.AddElement(new Paragraph("Buyer / ผู้ซื้อ  " + Header.CustomerName, fontDetailOld));
                tableHead1.AddCell(cell1);

                cell2 = GetNewCell(true);
                cell2.AddElement(new Paragraph("เลขที่", fontDetailOld));
                tableHead1.AddCell(cell2);

                cell3 = GetNewCell(true);
                cell3.AddElement(new Paragraph(Header.SaleNumber, fontDetail));
                tableHead1.AddCell(cell3);

                cell2 = GetNewCell(true);
                cell2.AddElement(new Paragraph("วันที่", fontDetailOld));
                tableHead1.AddCell(cell2);

                cell3 = GetNewCell(true);
                cell3.AddElement(new Paragraph(Header.ReceivedDateStr, fontDetail));
                tableHead1.AddCell(cell3);
                
                #region Comment
                //cell1.AddElement(new Phrase("ที่อยู่  " + Header.Add1, fontAdd, fontDetailOld));
                //cell1.AddElement(new Paragraph("Address  \\t" + Header.Add2, fontAdd, fontDetailOld));
                //cellAry = new PdfPCell[] { cell1, cell2, cell3 };
                //row = new PdfPRow(cellAry);
                //tableHead1.Rows.Add(row);
                #endregion

                widths = new float[] { 100f, 20f, 20f };
                tableHead1.SetWidths(widths);
                doc.Add(tableHead1);

                //Add Space
                doc.Add(tableSpace1);
                

                string Add1 = "", Add2 = "";
                Add1 = Header.CustomerAddress;
                Add2 = Header.CustomerDistrict + " " + Header.CustomerCountry + " " + Header.CustomerProvince + " " + Header.CustomerPostalCode;

                #region Comment
                //PdfContentByte SaleNo = writer.DirectContent;
                //PlaceText(SaleNo, "เลขที่  " + Header.SaleNumber, fontAdd, 330, 745, 420, 775, 0, Element.ALIGN_LEFT);
                //PdfContentByte Date = writer.DirectContent;
                //PlaceText(Date, "วันที่  " + Header.ReceivedDateStr, fontAdd, 460, 745, 550, 775, 0, Element.ALIGN_LEFT);
                //PdfContentByte CustName = writer.DirectContent;
                //PlaceText(CustName, "Buyer / ผู้ซื้อ  " + Header.CustomerName, fontAdd, 30, 730, 400, 750, 0, Element.ALIGN_LEFT);
                //PdfContentByte CustAdd = writer.DirectContent;
                //PlaceText(CustAdd, "Address / ที่อยู่  " + Add1, fontAdd, 30, 715, 400, 730, 0, Element.ALIGN_LEFT);
                //PdfContentByte CustAdd2 = writer.DirectContent;
                //PlaceText(CustAdd2, Add2, fontAdd, 80, 700, 400, 715, 0, Element.ALIGN_LEFT);
                #endregion

                #region  --> Comment 20181024
                //string endtext = "                   ---------------------------------------------------------------------------------------------------------------------------------------------------------------------------  ";
                //PdfContentByte Half = writer.DirectContent;
                //PlaceText(Half, endtext, fontAdd, 1, 400, 550, 430, 0, Element.ALIGN_LEFT);
                #endregion

                #region Detail & Summary
                #region Header Grid
                cell2 = GetNewCell(true);
                cell3 = GetNewCell(true);
                cell4 = GetNewCell(true);
                cell7 = GetNewCell(true);

                cell2.AddElement(GetNewParag("สินค้า", fontDetail, 1));
                cell2.VerticalAlignment = Element.ALIGN_MIDDLE;
                cell3.AddElement(GetNewParag("รหัส", fontDetailOld, 1));
                cell3.VerticalAlignment = Element.ALIGN_MIDDLE;
                cell4.AddElement(GetNewParag("จำนวน (หน่วย)", fontAdd, 1));
                cell4.VerticalAlignment = Element.ALIGN_MIDDLE;
                cell7.AddElement(GetNewParag("รวม (บาท)", fontAdd, 1));
                cell7.VerticalAlignment = Element.ALIGN_MIDDLE;
                cellAry = new PdfPCell[] { cell2, cell3, cell4, cell7 };
                row = new PdfPRow(cellAry);
                widths = new float[] { 110f, 40f, 30f, 30f };

                tableDetail.SetWidths(widths);
                tableDetail.Rows.Add(row);
                #endregion

                string[] spl, splAmt;
                string sn = "", RealSN = "", ItemDesc = "", ItemDescAmt = "";
                double pItem = 0, pVat = 0, pTotal = 0, Discount = 0;
                int Count = 0;
                foreach (SaleDetailDTO item in Detail)
                {                    
                    ItemDesc = "";
                    ItemDescAmt = "";

                    cell2 = GetNewCell(false);
                    cell2.Border = PdfPCell.LEFT_BORDER | PdfPCell.RIGHT_BORDER;
                    cell3 = GetNewCell(false);
                    cell3.Border = PdfPCell.RIGHT_BORDER;
                    cell4 = GetNewCell(false);
                    cell4.Border = PdfPCell.RIGHT_BORDER;
                    cell7 = GetNewCell(false);
                    cell7.Border = PdfPCell.RIGHT_BORDER;

                    cell2.AddElement(GetNewParag(item.ItemName, fontAdd));
                    cell3.AddElement(GetNewParag(item.ItemCode, fontAdd));
                    cell4.AddElement(GetNewParag(item.AmountStr, fontAdd, 2));
                    cell7.AddElement(GetNewParag(item.Total.ToString("###,##0.00"), fontAdd, 2));
                    cellAry = new PdfPCell[] { cell2, cell3, cell4, cell7 };
                    row = new PdfPRow(cellAry);
                    //widths = new float[] { 115f, 50f, 30f, 15f };
                    //tableDetail.SetWidths(widths);
                    tableDetail.Rows.Add(row);
                    Count++;

                    #region Desc
                    if (item.ItemDescription != null)
                    {
                        spl = item.ItemDescription.Split(new string[] { "\r\n" }, StringSplitOptions.None);
                        if (spl != null && spl.Length > 0)
                        {
                            foreach (string s in spl)
                            {                               
                                cell2 = GetNewCell(false);
                                cell2.Border = PdfPCell.LEFT_BORDER | PdfPCell.RIGHT_BORDER;
                                cell3 = GetNewCell(false);
                                cell3.Border = PdfPCell.RIGHT_BORDER;
                                cell4 = GetNewCell(false);
                                cell4.Border = PdfPCell.RIGHT_BORDER;
                                cell7 = GetNewCell(false);
                                cell7.Border = PdfPCell.RIGHT_BORDER;

                                splAmt = s.Split(new string[] { "จำนวน" }, StringSplitOptions.None);
                                if (splAmt != null && splAmt.Length > 0)
                                {
                                    ItemDesc = splAmt[0];
                                    if (splAmt.Length > 1)
                                        ItemDescAmt = splAmt[1].Trim();
                                    else
                                        ItemDescAmt = "";

                                    cell2.AddElement(GetNewParag(ItemDesc, fontAdd));
                                    cell4.AddElement(GetNewParag(ItemDescAmt, fontAdd, 2));
                                }

                                cellAry = new PdfPCell[] { cell2, cell3, cell4, cell7 };
                                row = new PdfPRow(cellAry);
                                //widths = new float[] { 115f, 50f, 30f, 15f };
                                //tableDetail.SetWidths(widths);
                                tableDetail.Rows.Add(row);
                                Count++;
                            }
                        }
                    }
                    #endregion

                    RealSN = RealSN + item.SerialNumber;
                    sn = sn + item.SerialNumber + ", ";
                    pTotal = pTotal + item.TotalIndVat;
                    pVat = pVat + item.TotalVATAmount2Digit;
                    Discount = Discount + item.Discount.Value;
                }

                #region Remark
                //if (Header.Remark != "")
                //{
                //    cell2 = GetNewCell(false);
                //    cell2.Border = PdfPCell.LEFT_BORDER | PdfPCell.RIGHT_BORDER;
                //    cell3 = GetNewCell(false, 0);
                //    cell3.Border = PdfPCell.RIGHT_BORDER;
                //    cell4 = GetNewCell(false);
                //    cell4.Border = PdfPCell.RIGHT_BORDER;
                //    cell7 = GetNewCell(false);
                //    cell7.Border = PdfPCell.RIGHT_BORDER;

                //    //cell2 = GetNewCell(false);
                //    //cell3 = GetNewCell(false, 0);
                //    //cell4 = GetNewCell(false);
                //    //cell7 = GetNewCell(false);

                //    cell20 = GetNewCell(false, 0);
                //    cell21 = GetNewCell(false, 0);
                //    //cell3.HorizontalAlignment = PdfPCell.ALIGN_LEFT;
                //    //cell20.HorizontalAlignment = PdfPCell.ALIGN_LEFT;

                //    cell20.AddElement(new Chunk("หมายเหตุ : ", fontAddB));
                //    //cell20.AddElement(GetNewParag("หมายเหตุ : ", fontAddB));
                //    cell21.AddElement(GetNewParag(Header.Remark, fontAdd));
                //    cellAry = new PdfPCell[] { cell20, cell21 };
                //    row = new PdfPRow(cellAry);
                //    widths = new float[] { 50f, 180f };
                //    tableRemark.SetWidths(widths);
                //    tableRemark.Rows.Add(row);
                //    tableRemark.HorizontalAlignment = PdfPCell.ALIGN_LEFT;
                //    cell3.AddElement(tableRemark);

                //    //cell3.AddElement(GetNewParag("หมายเหตุ : ", fontAddB));
                //    //cell3.AddElement(GetNewParag(Header.Remark, fontAdd));
                //    cellAry = new PdfPCell[] { cell2, cell3, cell4, cell7 };
                //    row = new PdfPRow(cellAry);
                //    //widths = new float[] { 115f, 50f, 30f, 15f };
                //    //tableDetail.SetWidths(widths);
                //    tableDetail.Rows.Add(row);
                //    Count++;
                //}
                #endregion

                #region S/N
                if (!string.IsNullOrEmpty(RealSN))
                {
                    //cell2 = GetNewCell(false);
                    //cell3 = GetNewCell(false);
                    //cell4 = GetNewCell(false);
                    //cell7 = GetNewCell(false);

                    cell2 = GetNewCell(false);
                    cell2.Border = PdfPCell.LEFT_BORDER | PdfPCell.RIGHT_BORDER;
                    cell3 = GetNewCell(false, 0);
                    cell3.Border = PdfPCell.RIGHT_BORDER;
                    cell4 = GetNewCell(false);
                    cell4.Border = PdfPCell.RIGHT_BORDER;
                    cell7 = GetNewCell(false);
                    cell7.Border = PdfPCell.RIGHT_BORDER;

                    sn = sn.Substring(0, sn.Length - 2);
                    cell3.AddElement(GetNewParag("S/N : " + sn, fontAdd));
                    cellAry = new PdfPCell[] { cell2, cell3, cell4, cell7 };
                    row = new PdfPRow(cellAry);
                    //widths = new float[] { 115f, 50f, 30f, 15f };
                    //tableDetail.SetWidths(widths);
                    tableDetail.Rows.Add(row);
                    Count++;
                }
                #endregion

                #region Summary

                #region Empty Row to End Page
                for (int i = Count; i < 11; i++)
                {
                    cell2 = GetNewCell(false);
                    cell2.Border = PdfPCell.LEFT_BORDER | PdfPCell.RIGHT_BORDER;
                    cell3 = GetNewCell(false);
                    cell3.Border = PdfPCell.RIGHT_BORDER;
                    cell4 = GetNewCell(false);
                    cell4.Border = PdfPCell.RIGHT_BORDER;
                    cell7 = GetNewCell(false);
                    cell7.Border = PdfPCell.RIGHT_BORDER;

                    cell2.AddElement(GetNewParag(" ", fontAdd));
                    cellAry = new PdfPCell[] { cell2, cell3, cell4, cell7 };
                    row = new PdfPRow(cellAry);
                    tableDetail.Rows.Add(row);
                    //Count++;
                }
                
                #endregion

                cell2 = GetNewCell(false);
                cell7 = GetNewCell(false);

                cell2.Colspan = 3;
                cell2.Border = PdfPCell.LEFT_BORDER | PdfPCell.RIGHT_BORDER | PdfPCell.TOP_BORDER | PdfPCell.BOTTOM_BORDER;

                cell2.AddElement(GetNewParag("รวมทั้งสิ้น", fontSummary, 1));
                cell7.AddElement(GetNewParag(pTotal.ToString("###,##0.00"), fontSummary, 2));
                cell7.Border = PdfPCell.RIGHT_BORDER | PdfPCell.TOP_BORDER | PdfPCell.BOTTOM_BORDER;
                cell2.VerticalAlignment = Element.ALIGN_MIDDLE;
                cell7.VerticalAlignment = Element.ALIGN_MIDDLE;

                tableDetail.AddCell(cell2);
                tableDetail.AddCell(cell7);               

                #endregion
                
                pItem = pTotal - pVat;
                doc.Add(tableDetail);
                #endregion

                #endregion

                #region Copy2 Bottom --> Comment 20181024
                ////Add Space
                //cell1 = GetNewCell(false);
                //cell1.AddElement(new Paragraph(" ", fontDetailOld));
                //cellAry = new PdfPCell[] { cell1 };
                //row = new PdfPRow(cellAry);
                //for (int i = 0; i < 5; i++)
                //{
                //    tableSpace.Rows.Add(row);
                //}
                //doc.Add(tableSpace);                

                //doc.Add(tableHead);

                ////Add Space
                //doc.Add(tableSpace1);

                //doc.Add(tableHead1);

                ////Add Space
                //doc.Add(tableSpace1);

                //doc.Add(tableDetail);

                #region Copy3  --> Comment 20181024
                //doc.NewPage();
                //doc.Add(tableHead);

                ////Add Space
                //doc.Add(tableSpace1);

                //doc.Add(tableHead1);

                ////Add Space
                //doc.Add(tableSpace1);

                //doc.Add(tableDetail);
                #endregion

                #endregion
                //End Generate
            }
            catch (Exception ex)
            {
                error = ex.Message;
            }
            finally
            {
                doc.Close();
                writer.Close();
            }

            string FileName = "Report_" + Header.SaleNumber;
            Response.Clear();
            Response.Buffer = false;
            Response.ContentEncoding = Encoding.UTF8;
            Response.HeaderEncoding = Encoding.UTF8;
            Response.AddHeader("content-disposition", "attachment;filename=" + FileName + ".pdf");
            Response.ContentType = "application/pdf";
            Response.BinaryWrite(ms.ToArray());
            Response.Flush();
            Response.SuppressContent = true;

            return error;
        }
        #endregion

        protected List<SaleHeaderDTO> ModDataFromSale(List<SaleHeaderDTO> lst)
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

        public SaleHeaderDTO GetSaleHeaderFromDB(Int32 headerID = 0)
        {
            SaleHeaderDTO Header = new SaleHeaderDTO();
            try
            {
                using (BillingEntities cre = new BillingEntities())
                {
                    Header = (from h in cre.TransSaleHeaders
                              where h.SaleHeaderID.Equals(headerID)
                              select new SaleHeaderDTO()
                              {
                                  SaleHeaderID = h.SaleHeaderID,
                                  CustomerName = h.CustomerName,
                                  CustomerAddress = h.CustomerAddress,
                                  CustomerDistrict = h.CustomerDistrict,
                                  CustomerCountry = h.CustomerCountry,
                                  CustomerProvince = h.CustomerProvince,
                                  CustomerPostalCode = h.CustomerPostalCode,
                                  //CustomerAddress2 = h.CustomerAddress2,
                                  //CustomerAddress3 = h.CustomerAddress3,
                                  ReceivedDate = h.ReceivedDate,
                                  ReceivedBy = h.ReceivedBy,
                                  SaleNumber = h.SaleNumber,
                                  Remark = h.Remark,
                              }).FirstOrDefault();

                };
            }
            catch (Exception ex)
            {
                
            }
            return Header;
        }

        public List<SaleDetailDTO> GetSaleDetailFromDB(Int32 headerID = 0)
        {
            List<SaleDetailDTO> Detail = new List<SaleDetailDTO>();
            try
            {
                using (BillingEntities cre = new BillingEntities())
                {                    
                    Detail = (from d in cre.TransSaleDetails
                              join i in cre.MasItems on d.ItemID equals i.ItemID
                              where d.SaleHeaderID.Equals(headerID)
                              select new SaleDetailDTO()
                              {
                                  SaleHeaderID = d.SaleHeaderID,
                                  SaleDetailID = d.SaleDetailID,
                                  ItemID = d.ItemID,
                                  ItemCode = i.ItemCode,
                                  ItemName = i.ItemName,
                                  ItemDescription = d.ItemDetail,
                                  Amount = d.Amount,
                                  ItemPrice = d.ItemPrice.HasValue ? d.ItemPrice.Value : 0,
                                  Discount = d.Discount,
                                  SerialNumber = d.SerialNumber,
                              }).ToList();
                };
            }
            catch (Exception ex)
            {

            }
            return Detail;
        }

        #region Stock
        public void ManageStock(int ItemID, int Amount, string Action, string User, int Change = 0, int DetailID = 0)
        {
            try
            {
                //Inventory o = new Inventory();
                //InventoryLog Log = new InventoryLog();
                //int LastAmount = 0;
                //string Remark = "";
                //using (BillingEntities cre = new BillingEntities())
                //{
                //    #region Inventory
                //    o = cre.Inventories.FirstOrDefault(w => w.ItemID.Equals(ItemID));
                //    if(o == null)
                //    {
                //        o = new Inventory();
                //        o.ItemID = ItemID;
                //        o.Amount = 0;
                //        cre.Inventories.Add(o);
                //        cre.SaveChanges();
                //    }                   

                //    switch (Action)
                //    {
                //        case "Sale":
                //            LastAmount = o.Amount;
                //            o.Amount = LastAmount + Change;
                //            Remark = "Update From TransactionSale Amount = " + Amount.ToString();
                //            break;
                //        case "Outbound":
                //            LastAmount = o.Amount;
                //            o.Amount = LastAmount - Amount;
                //            break;
                //        case "Inbound":
                //            LastAmount = o.Amount;
                //            o.Amount = LastAmount + Amount;
                //            break;
                //        default:
                //            break;
                //    }

                //    o.UpdatedBy = User;
                //    o.UpdatedDate = DateTime.Now;
                //    cre.SaveChanges();
                //    #endregion

                //    #region Logs
                //    Log = new InventoryLog();
                //    Log.Action = Action;
                //    Log.Amount = Amount;
                //    Log.InventoryID = o.InventoryID;
                //    Log.LastAmount = LastAmount;
                //    Log.Remark = Remark;
                //    Log.DetailID = DetailID;
                //    Log.UpdatedBy = GetUsername();
                //    Log.UpdatedDate = DateTime.Now;
                //    cre.InventoryLogs.Add(Log);
                //    cre.SaveChanges();
                //    #endregion
                //};

            }
            catch (Exception ex)
            {
                
            }
        }
        #endregion
    }
}