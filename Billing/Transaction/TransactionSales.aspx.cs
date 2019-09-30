using Billing.AppData;
using Entities.DTO;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DAL;

namespace Billing.Transaction
{
    public partial class TransactionSales : Billing.Common.BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                txtDate.Attributes.Add("readonly", "readonly");
                string ID = Request.QueryString.Get("ID");
                hddID.Value = ID;                
                Session["saleDetail"] = null;
                BindDDL();
                BindData();
            }
        }

        #region BindData
        protected void BindDDL()
        {
            try
            {
                List<MasValueList> lst = new List<MasValueList>();

                #region Pay Type
                using (BillingEntities cre = new BillingEntities())
                {
                    lst = cre.MasValueLists.Where(w => w.FUNC.Equals("PAYMENT") && w.ISACTIVE.Equals("Y")).OrderBy(od => od.TYPES).ToList();
                }
                if(lst != null && lst.Count > 0)
                {
                    ddlPay.DataSource = lst;
                    ddlPay.DataTextField = "Description";
                    ddlPay.DataValueField = "Code";
                }
                else
                {
                    ddlPay.DataSource = null;
                }
                ddlPay.DataBind();
                #endregion

                #region Account
                List<MasAccountTransfer> lstAcc = new List<MasAccountTransfer>();
                using (BillingEntities cre = new BillingEntities())
                {
                    lstAcc = cre.MasAccountTransfers.Where(w => w.ISACTIVE.Equals("Y")).OrderBy(od => od.AccountName).ToList();
                }
                if (lst != null && lst.Count > 0)
                {
                    ddlAccount.DataSource = lstAcc;
                    ddlAccount.DataTextField = "AccountName";
                    ddlAccount.DataValueField = "AccountTransferID";
                }
                else
                {
                    ddlAccount.DataSource = null;
                }
                ddlAccount.DataBind();
                #endregion

                #region Sale
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
                ddlSaleName.Items.Insert(0, new ListItem("", "0"));
                ddlSaleName.DataBind();
                #endregion

                #region Installment
                using (BillingEntities cre = new BillingEntities())
                {
                    lst = cre.MasValueLists.Where(w => w.FUNC.Equals("INSTALLMENT") && w.ISACTIVE.Equals("Y")).OrderBy(od => od.CODE).ToList();
                }
                if (lst != null && lst.Count > 0)
                {
                    ddlAccountInst.DataSource = lst;
                    ddlAccountInst.DataTextField = "Description";
                    ddlAccountInst.DataValueField = "Code";
                }
                else
                {
                    ddlAccountInst.DataSource = null;
                }
                ddlAccountInst.DataBind();
                #endregion
            }
            catch (Exception ex)
            {
                
            }
        }
        protected void BindData()
        {
            try
            {
                if (!string.IsNullOrEmpty(hddID.Value))
                {
                    int tid = ToInt32(hddID.Value);
                    TransSaleHeader obj = new TransSaleHeader();
                    List<SaleDetailDTO> lst = new List<SaleDetailDTO>();
                    using (BillingEntities cre = new BillingEntities())
                    {
                        obj = cre.TransSaleHeaders.FirstOrDefault(w => w.SaleHeaderID.Equals(tid));
                        if (obj != null)
                        {
                            hddID.Value = obj.SaleHeaderID.ToString();
                            txtDate.Text = obj.ReceivedDate.HasValue ? obj.ReceivedDate.Value.ToString("dd/MM/yyyy") : "";
                            txtCustAddress.Text = obj.CustomerAddress;
                            txtCustDistrict.Text = obj.CustomerDistrict;
                            txtCustCountry.Text = obj.CustomerCountry;
                            txtCustProvince.Text = obj.CustomerProvince;
                            txtCustPostalCode.Text = obj.CustomerPostalCode;
                            //txtCustAddress2.Text = obj.CustomerAddress2;
                            //txtCustAddress3.Text = obj.CustomerAddress3;
                            txtDeliverAddress.Text = obj.DeliverAdd;
                            txtDeliverDistrict.Text = obj.DeliverDistrict;
                            txtDeliverCountry.Text = obj.DeliverCountry;
                            txtDeliverProvince.Text = obj.DeliverProvince;
                            txtDeliverPostalCode.Text = obj.DeliverPostalCode;
                            //txtDeliverAddress2.Text = obj.DeliverAdd2;
                            //txtDeliverAddress3.Text = obj.DeliverAdd3;
                            txtCustName.Text = obj.CustomerName;
                            txtDeliveryName.Text = obj.DeliveryName;
                            txtTel.Text = obj.Tel;
                            txtSaleNumber.Text = obj.SaleNumber;
                            txtRemark.Text = obj.Remark;
                            //chkCOD.Checked = string.IsNullOrEmpty(obj.COD) ? false : obj.COD.Equals("0") ? false : true;
                            txtWarranty.Text = obj.WarrantyDate.HasValue ? obj.WarrantyDate.Value.ToString("dd/MM/yyyy") : "";
                            txtConsignmentNo.Text = obj.ConsignmentNo;

                            #region 201808
                            if(!string.IsNullOrEmpty(obj.BillType))
                            {
                                if (obj.BillType.Equals("Vat"))
                                {
                                    rdbVat.Checked = true;
                                }
                                else //if (obj.BillType.Equals("Cash"))
                                {
                                    rdbCash.Checked = true;
                                }
                            }
                            if (!string.IsNullOrEmpty(obj.PayType))
                                ddlPay.SelectedIndex = ddlPay.Items.IndexOf(ddlPay.Items.FindByValue(obj.PayType)); //ToInt32(obj.PayType);
                            #endregion

                            #region 201901
                            ddlAccount.SelectedIndex = ddlAccount.Items.IndexOf(ddlAccount.Items.FindByText(obj.AccountTransfer));
                            ddlSaleName.SelectedIndex = ddlSaleName.Items.IndexOf(ddlSaleName.Items.FindByText(obj.SaleName));
                            txtTimeTransfer.Text = obj.TimeTransfer;
                            #endregion

                            #region 201903
                            ddlAccountInst.SelectedIndex = ddlAccountInst.Items.IndexOf(ddlAccountInst.Items.FindByText(obj.Installment));
                            
                            #endregion

                            lst = (from d in cre.TransSaleDetails
                                   join i in cre.MasItems on d.ItemID equals i.ItemID
                                   where d.SaleHeaderID.Equals(tid)
                                   select new SaleDetailDTO()
                                   {
                                       SaleDetailID = d.SaleDetailID,
                                       SaleHeaderID = d.SaleHeaderID,
                                       ItemID = d.ItemID.HasValue ? d.ItemID.Value : 0,
                                       ItemName = i.ItemName,
                                       ItemPrice = d.ItemPrice.HasValue ? d.ItemPrice.Value : 0,
                                       ItemDescription = d.ItemDetail,
                                       Amount = d.Amount,
                                       Discount = d.Discount,
                                       SerialNumber = d.SerialNumber,
                                       Status = "Old",
                                       
                                   }).ToList();

                            Session["saleDetail"] = lst;
                        }
                    }
                }
                else
                {
                    txtDate.Text = DateTime.Now.ToString("dd/MM/yyyy");
                    txtWarranty.Text = DateTime.Now.ToString("dd/MM/yyyy");
                    rdbCash.Checked = true;

                    //Get Salenumber
                    string salenumber = DateTime.Now.ToString("yyMM");
                    string[] spl;
                    TransSaleHeader s = new TransSaleHeader();
                    List<TransSaleHeader> lstS = new List<TransSaleHeader>();
                    using (BillingEntities cre = new BillingEntities())
                    {
                        lstS = cre.TransSaleHeaders.Where(w => w.SaleNumber.Contains(salenumber)).ToList();
                        if(lstS != null && lstS.Count > 0)
                        {
                            s = lstS.OrderByDescending(o => o.SaleNumber).FirstOrDefault();
                            spl = s.SaleNumber.Split('-');
                            if (spl != null)
                                salenumber = salenumber + "-" + (ToInt32(spl[1]) + 1).ToString("000");

                            txtSaleNumber.Text = salenumber;
                        }
                        else
                        {
                            txtSaleNumber.Text = salenumber + "-001";
                        }
                    };
                }
                BindDataGrid();
            }
            catch (Exception ex)
            {

            }
        }

        protected void BindDataGrid()
        {
            try
            {
                List<SaleDetailDTO> lst = new List<SaleDetailDTO>();
                if (Session["saleDetail"] != null)
                {
                    lst = (List<SaleDetailDTO>)Session["saleDetail"];
                    lst = lst.Where(w => w.Status != "Delete").ToList();
                    double Total = 0;
                    foreach (SaleDetailDTO item in lst)
                    {                        
                        Total = Total + item.Total;
                    }
                    lbTotal.Text = (Total).ToString("###,##0.00");
                }
                else
                {
                    lst = null;
                }

                gvItem.DataSource = lst;
                gvItem.DataBind();
            }
            catch (Exception ex)
            {

            }
        }
        #endregion

        protected void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                //Validate
                if (txtCustName.Text == "")
                {
                    ShowMessageBox("กรุณาระบุ ชื่อลูกค้า");
                    return;
                }
                if (txtDate.Text == "")
                {
                    ShowMessageBox("กรุณาระบุ วันที่");
                    return;
                }

                string Message = "";
                TransSaleHeader o = new TransSaleHeader();
                SaleHeaderDTO obj = new SaleHeaderDTO();
                List<SaleDetailDTO> lst = new List<SaleDetailDTO>();
                List<SaleDetailDTO> tmp = new List<SaleDetailDTO>();
                int hid = 0;
                int Change = 0;
                if (Session["saleDetail"] != null)
                {
                    lst = (List<SaleDetailDTO>)Session["saleDetail"];
                }

                if (string.IsNullOrEmpty(hddID.Value)) //New --> Insert
                {
                    using (BillingEntities cre = new BillingEntities())
                    {
                        #region Header
                        o.Tel = txtTel.Text;
                        o.CustomerName = txtCustName.Text;
                        o.CustomerAddress = txtCustAddress.Text;
                        o.CustomerDistrict = txtCustDistrict.Text;
                        o.CustomerCountry = txtCustCountry.Text;
                        o.CustomerProvince = txtCustProvince.Text;
                        o.CustomerPostalCode = txtCustPostalCode.Text;
                        //o.CustomerAddress2 = txtCustAddress2.Text;
                        //o.CustomerAddress3 = txtCustAddress3.Text;
                        o.DeliveryName = txtDeliveryName.Text;
                        o.DeliverAdd = txtDeliverAddress.Text;
                        o.DeliverDistrict = txtDeliverDistrict.Text;
                        o.DeliverCountry = txtDeliverCountry.Text;
                        o.DeliverProvince = txtDeliverProvince.Text;
                        o.DeliverPostalCode = txtDeliverPostalCode.Text;
                        //o.DeliverAdd2 = txtDeliverAddress2.Text;
                        //o.DeliverAdd3 = txtDeliverAddress3.Text;
                        o.ReceivedDate = DateTime.ParseExact(txtDate.Text, "dd/MM/yyyy", new System.Globalization.CultureInfo("en-US"));
                        o.ReceivedBy = "เงินสด";
                        o.SaleNumber = txtSaleNumber.Text;
                        o.Remark = txtRemark.Text;
                        o.CreatedBy = GetUsername();
                        o.CreatedDate = DateTime.Now;
                        o.Active = "1";
                        o.ConsignmentNo = txtConsignmentNo.Text;
                        //o.COD = chkCOD.Checked ? "1" : "0";
                        o.COD = "0";
                        if (!string.IsNullOrEmpty(txtWarranty.Text))
                            o.WarrantyDate = DateTime.ParseExact(txtWarranty.Text, "dd/MM/yyyy", new System.Globalization.CultureInfo("en-US"));

                        #region 201808
                        o.BillType = rdbVat.Checked ? "Vat" : "Cash";
                        o.PayType = ddlPay.SelectedValue;
                        #endregion

                        #region 201901
                        o.SaleName = ddlSaleName.SelectedItem.Text;
                        if (ddlPay.SelectedItem.Value == "5")
                        {
                            o.AccountTransfer = ddlAccount.SelectedItem.Text;
                            o.TimeTransfer = txtTimeTransfer.Text;
                        }
                        else
                        {
                            o.AccountTransfer = "";
                            o.TimeTransfer = "";
                        }
                        #endregion

                        #region 201903
                        if (ddlPay.SelectedItem.Value == "8")
                            o.Installment = ddlAccountInst.SelectedItem.Text;
                        else
                            o.Installment = "";
                        #endregion

                        cre.TransSaleHeaders.Add(o);
                        cre.SaveChanges();
                        hid = o.SaleHeaderID;
                        #endregion

                        #region Description
                        lst = lst.Where(w => w.Status == "New").ToList();
                        if (lst != null && lst.Count > 0)
                        {
                            TransSaleDetail oDetail = new TransSaleDetail();
                            //TransStock ts = new TransStock();
                            int StockID = 0;
                            foreach (SaleDetailDTO item in lst)
                            {
                                oDetail = new TransSaleDetail();
                                oDetail.SaleHeaderID = hid;
                                oDetail.ItemID = item.ItemID;
                                oDetail.ItemPrice = item.ItemPrice;
                                oDetail.Amount = item.Amount;
                                oDetail.Discount = item.Discount;
                                //oDetail.DiscountPer = item.DiscountPer;
                                oDetail.SerialNumber = item.SerialNumber;
                                oDetail.ItemDetail = item.ItemDescription;
                                cre.TransSaleDetails.Add(oDetail);
                                cre.SaveChanges();

                                #region Stock
                                //StockID = item.StockID;
                                //ts = cre.TransStocks.FirstOrDefault(w => w.StockID.Equals(StockID) && w.Active.ToLower().Equals("y"));
                                //if(ts != null)
                                //{
                                //    ts.Active = "N";
                                //    ts.StockType = "Sold";
                                //    ts.SaleHeaderID = hid;
                                //    ts.SaleDetailID = oDetail.SaleDetailID;
                                //    ts.UpdatedBy = GetUsername();
                                //    ts.UpdatedDate = DateTime.Now;
                                //    cre.SaveChanges();
                                //}
                                //else // Item Is Sold Already
                                //{
                                //    Message = Message + " " + item.ItemName + ",";
                                //    cre.TransSaleDetails.Remove(oDetail);
                                //    cre.SaveChanges();
                                //}
                                #endregion

                                //Change = ToInt32(item.Amount.Value.ToString()) * -1;
                                //ManageStock(ToInt32(item.ItemID.Value.ToString()), ToInt32(item.Amount.Value.ToString()), "Sale", GetUsername(), Change, oDetail.SaleDetailID);
                            }
                        }
                        cre.SaveChanges();
                        #endregion
                    };
                }
                else // --> Update
                {
                    hid = ToInt32(hddID.Value);
                    using (BillingEntities cre = new BillingEntities())
                    {
                        o = cre.TransSaleHeaders.FirstOrDefault(w => w.SaleHeaderID.Equals(hid));
                        if (o != null)
                        {
                            #region Header 
                            o.Tel = txtTel.Text;
                            o.CustomerName = txtCustName.Text;
                            o.CustomerAddress = txtCustAddress.Text;
                            o.CustomerDistrict = txtCustDistrict.Text;
                            o.CustomerCountry = txtCustCountry.Text;
                            o.CustomerProvince = txtCustProvince.Text;
                            o.CustomerPostalCode = txtCustPostalCode.Text;
                            //o.CustomerAddress2 = txtCustAddress2.Text;
                            //o.CustomerAddress3 = txtCustAddress3.Text;
                            o.DeliveryName = txtDeliveryName.Text;
                            o.DeliverAdd = txtDeliverAddress.Text;
                            o.DeliverDistrict = txtDeliverDistrict.Text;
                            o.DeliverCountry = txtDeliverCountry.Text;
                            o.DeliverProvince = txtDeliverProvince.Text;
                            o.DeliverPostalCode = txtDeliverPostalCode.Text;
                            //o.DeliverAdd2 = txtDeliverAddress2.Text;
                            //o.DeliverAdd3 = txtDeliverAddress3.Text;
                            o.ReceivedDate = DateTime.ParseExact(txtDate.Text, "dd/MM/yyyy", new System.Globalization.CultureInfo("en-US"));
                            o.SaleNumber = txtSaleNumber.Text;
                            o.Remark = txtRemark.Text;
                            o.UpdatedBy = GetUsername();
                            o.UpdatedDate = DateTime.Now;
                            o.ConsignmentNo = txtConsignmentNo.Text;
                            //o.COD = chkCOD.Checked ? "1" : "0";
                            o.COD = "0";
                            if (!string.IsNullOrEmpty(txtWarranty.Text)) 
                                o.WarrantyDate = DateTime.ParseExact(txtWarranty.Text, "dd/MM/yyyy", new System.Globalization.CultureInfo("en-US"));

                            #region 201808
                            o.BillType = rdbVat.Checked ? "Vat" : "Cash";
                            o.PayType = ddlPay.SelectedValue;
                            #endregion

                            #region 201901
                            o.SaleName = ddlSaleName.SelectedItem.Text;
                            if(ddlPay.SelectedItem.Value == "5")
                            {
                                o.AccountTransfer = ddlAccount.SelectedItem.Text;
                                o.TimeTransfer = txtTimeTransfer.Text;
                            }
                            else
                            {
                                o.AccountTransfer = "";
                                o.TimeTransfer = "";
                            }
                            #endregion

                            #region 201903
                            if (ddlPay.SelectedItem.Value == "8")
                                o.Installment = ddlAccountInst.SelectedItem.Text;
                            else
                                o.Installment = "";
                            #endregion

                            cre.SaveChanges();
                            #endregion

                            #region Detail
                            if (lst != null && lst.Count > 0)
                            {                                
                                TransSaleDetail objDetail = new TransSaleDetail();
                                tmp = lst.Where(w => w.Status == "Old").ToList();
                                foreach (SaleDetailDTO item in tmp)
                                {
                                    objDetail = cre.TransSaleDetails.FirstOrDefault(w => w.SaleDetailID.Equals(item.SaleDetailID));
                                    if (objDetail != null)
                                    {
                                        if (objDetail.Amount.Value - item.Amount.Value != 0)
                                            Change = ToInt32(objDetail.Amount.Value - item.Amount.Value);

                                        objDetail.ItemID = item.ItemID;
                                        objDetail.ItemPrice = item.ItemPrice;
                                        objDetail.Amount = item.Amount;
                                        objDetail.Discount = item.Discount;
                                        //objDetail.DiscountPer = item.DiscountPer;
                                        objDetail.SerialNumber = item.SerialNumber;
                                        objDetail.ItemDetail = item.ItemDescription;
                                    }
                                    cre.SaveChanges();

                                    //ManageStock(ToInt32(item.ItemID.Value.ToString()), ToInt32(item.Amount.Value.ToString()), "Sale", GetUsername(), Change, item.SaleDetailID);
                                }

                                tmp = lst.Where(w => w.Status == "New").ToList();
                                foreach (SaleDetailDTO item in tmp)
                                {
                                    Change = 0;
                                    objDetail = new TransSaleDetail();
                                    objDetail.SaleHeaderID = hid;
                                    objDetail.ItemID = item.ItemID;
                                    objDetail.ItemPrice = item.ItemPrice;
                                    objDetail.Amount = item.Amount;
                                    objDetail.Discount = item.Discount;
                                    //objDetail.DiscountPer = item.DiscountPer;
                                    objDetail.SerialNumber = item.SerialNumber;
                                    objDetail.ItemDetail = item.ItemDescription;
                                    cre.TransSaleDetails.Add(objDetail);
                                    cre.SaveChanges();

                                    //Change = ToInt32(item.Amount.Value.ToString()) * -1;
                                    //ManageStock(ToInt32(item.ItemID.Value.ToString()), ToInt32(item.Amount.Value.ToString()), "Sale", GetUsername(), Change, item.SaleDetailID);
                                }

                                tmp = lst.Where(w => w.Status == "Delete").ToList();
                                foreach (SaleDetailDTO item in tmp)
                                {
                                    Change = 0;
                                    objDetail = new TransSaleDetail();
                                    objDetail = cre.TransSaleDetails.FirstOrDefault(w => w.SaleDetailID.Equals(item.SaleDetailID));
                                    if (objDetail != null)
                                    {
                                        Change = ToInt32(objDetail.Amount.Value);
                                        cre.TransSaleDetails.Remove(objDetail);
                                    }
                                    
                                    cre.SaveChanges();

                                    //ManageStock(ToInt32(item.ItemID.Value.ToString()), ToInt32(item.Amount.Value.ToString()), "Sale", GetUsername(), Change, item.SaleDetailID);
                                }
                            }
                            #endregion
                        }
                    };
                }

                ShowMessageBox("บันทึกสำเร็จ.", this.Page, "TransactionSaleList.aspx");
            }
            catch (Exception ex)
            {
                ShowMessageBox("เกิดข้อผิดพลาด กรุณาติดต่อผู้ดูแลระบบ.");
                SendMailError(ex.Message, System.Reflection.MethodBase.GetCurrentMethod());
            }
        }

        #region Event Detail
        protected void btnAddModal_Click(object sender, EventArgs e)
        {
            try
            {
                //BindMDDL();
                ModalPopupExtender1.Show();
                ClearTextDetail();
            }
            catch (Exception ex)
            {
                ShowMessageBox("เกิดข้อผิดพลาด กรุณาติดต่อผู้ดูแลระบบ.");
                SendMailError(ex.Message, System.Reflection.MethodBase.GetCurrentMethod());
            }
        }

        protected void btnMSave_Click(object sender, EventArgs e)
        {
            try
            {
                int ItemID = 0, Count = 0;
                #region Validate
                if (txtMItem.Text == "")
                {
                    ShowMessageBox("กรุณาระบุ Item");
                    ModalPopupExtender1.Show();
                    return;
                }

                if (txtMAmount.Text == "" || txtMAmount.Text == "0")
                {
                    ShowMessageBox("กรุณาระบุ Amount");
                    ModalPopupExtender1.Show();
                    return;
                }

                //if (txtMDiscountPer.Text != "" && ToDoudle(txtMDiscountPer.Text) > 100)
                //{
                //    ShowMessageBox("ระบุส่วนลด % ไม่ถูกต้อง");
                //    ModalPopupExtender1.Show();
                //    return;
                //}

                ItemID = ToInt32(hddItemID.Value);
                //using (BillingEntities cre = new BillingEntities())
                //{
                //    Count = cre.TransStocks.Where(w => w.ItemID.Equals(ItemID) && w.Active.ToLower().Equals("y")).Count();
                //}
                //if(Count == 0)
                //{
                //    ShowMessageBox("สินค้า " + txtMItem.Text + " ไม่มีในสต๊อก กรุณาเพิ่มสินค้าก่อนทำรายการขาย !!");
                //    ModalPopupExtender1.Show();
                //    return;
                //}
                #endregion

                #region Save to Session
                List<SaleDetailDTO> lst = new List<SaleDetailDTO>();
                SaleDetailDTO o = new SaleDetailDTO();
                if (Session["saleDetail"] != null)
                {
                    lst = (List<SaleDetailDTO>)Session["saleDetail"];
                }

                if (hddDetailID.Value != "" && hddDetailID.Value != "0")
                {
                    Int32 did = ToInt32(hddDetailID.Value);
                    o = lst.FirstOrDefault(w => w.SaleDetailID.Equals(did));
                    if(o != null)
                    {
                        o.ItemID = ItemID;
                        o.StockID = ToInt32(hddStockID.Value);
                        o.ItemName = txtMItem.Text;
                        o.ItemPrice = ToDoudle(txtMPrice.Text);
                        o.Discount = ToDoudle(txtMDiscount.Text);
                        //o.DiscountPer = ToDoudle(txtMDiscountPer.Text);
                        o.Amount = ToDoudle(txtMAmount.Text);
                        o.SerialNumber = txtMSN.Text;
                        o.ItemDescription = GetItemDesc();// txtMDescription.Text;
                    }
                }
                else
                {
                    o = new SaleDetailDTO();
                    o.SaleDetailID = lst.Count == 0 ? 1 : ToInt32(lst.Max(m => m.SaleDetailID).ToString()) + 1;
                    o.ItemID = ItemID; // ToInt32(hddItemID.Value);
                    o.StockID = ToInt32(hddStockID.Value);
                    o.ItemName = txtMItem.Text;
                    o.ItemPrice = ToDoudle(txtMPrice.Text);
                    o.Discount = ToDoudle(txtMDiscount.Text);
                    //o.DiscountPer = ToDoudle(txtMDiscountPer.Text);
                    o.Amount = ToDoudle(txtMAmount.Text);
                    o.SerialNumber = txtMSN.Text;
                    o.ItemDescription = GetItemDesc();// txtMDescription.Text;
                    o.Status = "New";
                    lst.Add(o);
                }
                                
                Session["saleDetail"] = lst;
                #endregion

                //ShowMessageBox("Save Succ");
                BindDataGrid();
                gvItem.Focus();
            }
            catch (Exception ex)
            {
                ShowMessageBox("เกิดข้อผิดพลาด กรุณาติดต่อผู้ดูแลระบบ.");
                SendMailError(ex.Message, System.Reflection.MethodBase.GetCurrentMethod());
            }
        }     
        
        protected string GetItemDesc()
        {
            string result = "";
            try
            {
                result = txtMDescription.Text;
                //TextBox txt = new TextBox();
                //for (int i = 1; i <= 10; i++)
                //{
                //    txt = (TextBox)Panel1.FindControl("txtMDesc" + i.ToString());
                //    if (txt != null && txt.Visible)
                //    {
                //        result = result + txt.Text + "\r\n";
                //    }
                //}
            }
            catch (Exception ex)
            {
                ShowMessageBox("เกิดข้อผิดพลาด กรุณาติดต่อผู้ดูแลระบบ.");
                SendMailError(ex.Message, System.Reflection.MethodBase.GetCurrentMethod());
            }
            return result;
        }

        protected void BindItemDesc(string ItemDesc)
        {
            try
            {
                if(!string.IsNullOrEmpty(ItemDesc))
                {
                    string[] spl = ItemDesc.Split(new string[] { "\r\n" }, StringSplitOptions.None);
                    if(spl != null)
                    {
                        TextBox txt = new TextBox();
                        for (int i = 1; i <= 10; i++)
                        {
                            txt = (TextBox)Panel1.FindControl("txtMDesc" + i.ToString());
                            if (txt != null && !string.IsNullOrEmpty(spl[i-0]))
                            {
                                txt.Text = spl[i - 0];
                                txt.Visible = true;
                            }
                            else
                            {
                                txt.Visible = false;
                            }
                        }
                    }
                    
                }
            }
            catch (Exception ex)
            {
                ShowMessageBox("เกิดข้อผิดพลาด กรุณาติดต่อผู้ดูแลระบบ.");
                SendMailError(ex.Message, System.Reflection.MethodBase.GetCurrentMethod());
            }
        }

        protected void ClearTextDetail()
        {
            hddDetailID.Value = "0";
            hddItemID.Value = "0";
            txtMItem.Text = "";
            txtMPrice.Text = "";
            txtMAmount.Text = "1";
            txtMDiscount.Text = "0";
            //txtMDiscountPer.Text = "0";
            txtMSN.Text = "";
            txtMDescription.Text = "";
        }

        protected void imgbtnEdit_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                ModalPopupExtender1.Show();                
                ImageButton imb = (ImageButton)sender;
                if (imb != null)
                {
                    int objID = ToInt32(imb.CommandArgument);
                    List<SaleDetailDTO> lst = new List<SaleDetailDTO>();
                    SaleDetailDTO obj = new SaleDetailDTO();
                    if (Session["saleDetail"] != null)
                    {
                        lst = (List<SaleDetailDTO>)Session["saleDetail"];
                        if (lst != null && lst.Count > 0)
                        {
                            obj = lst.FirstOrDefault(w => w.SaleDetailID.Equals(objID));
                            if (obj != null)
                            {
                                hddDetailID.Value = obj.SaleDetailID.ToString();
                                hddItemID.Value = obj.ItemID.ToString();                                
                                txtMItem.Text = obj.ItemName;
                                txtMPrice.Text = obj.ItemPriceStr;
                                txtMAmount.Text = obj.AmountStr;
                                txtMDiscount.Text = obj.DiscountStr;
                                //txtMDiscountPer.Text = obj.DiscountPerStr;
                                txtMSN.Text = obj.SerialNumber;
                                //BindItemDesc(obj.ItemDescription);
                                txtMDescription.Text = obj.ItemDescription;
                            }
                        }
                    }
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
                ImageButton imb = (ImageButton)sender;
                if (imb != null)
                {
                    int objID = ToInt32(imb.CommandArgument);
                    List<SaleDetailDTO> lst = new List<SaleDetailDTO>();
                    SaleDetailDTO obj = new SaleDetailDTO();
                    if (Session["saleDetail"] != null)
                    {
                        lst = (List<SaleDetailDTO>)Session["saleDetail"];
                        if (lst != null && lst.Count > 0)
                        {
                            obj = lst.FirstOrDefault(w => w.SaleDetailID.Equals(objID));
                            obj.Status = "Delete";
                            Session["saleDetail"] = lst;
                        }
                    }

                    BindDataGrid();
                }
                else
                {
                    SendMailError("imb is null", System.Reflection.MethodBase.GetCurrentMethod());
                }
            }
            catch (Exception ex)
            {
                ShowMessageBox("เกิดข้อผิดพลาด กรุณาติดต่อผู้ดูแลระบบ.");
                SendMailError(ex.Message, System.Reflection.MethodBase.GetCurrentMethod());
            }
        }

        #endregion                

        protected void txtDate_TextChanged(object sender, EventArgs e)
        {
            try
            {
                DateTime dt = DateTime.ParseExact(txtDate.Text, "dd/MM/yyyy", new System.Globalization.CultureInfo("en-US"));
                TransSaleHeader o = new TransSaleHeader();                
                using (BillingEntities cre = new BillingEntities())
                {
                    o = cre.TransSaleHeaders.Where(w => w.ReceivedDate.Value.Year == dt.Year && w.ReceivedDate.Value.Month == dt.Month)
                        .OrderByDescending(od => od.SaleNumber).FirstOrDefault();

                    if(o != null)
                    {
                        string[] spl = o.SaleNumber.Split('-');
                        if(spl != null)
                        {
                            int no = ToInt32(spl[1]);
                            no++;
                            txtSaleNumber.Text = dt.ToString("yyMM") + "-" + no.ToString("000"); 
                        }
                    }
                    else
                    {
                        txtSaleNumber.Text = dt.ToString("yyMM") + "-001";
                    }
                };
            }
            catch (Exception)
            {
                
            }
        }

        #region Modal2 Customer
        protected void btnMSearch_Click(object sender, EventArgs e)
        {
            try
            {
                ModalPopupExtender2.Show();
                Session["pageindexSale"] = "1";
                BindSearch();
            }
            catch (Exception ex)
            {
                
            }
        }

        protected void imgbtnSearch_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                ClearTextSearch();
                Session["pageindexSale"] = "1";
                BindSearch();
                ModalPopupExtender2.Show();
                
            }
            catch (Exception ex)
            {
                
            }
        }

        protected void BindSearch()
        {
            try
            {
                string Name = txtSearchCust.Text;
                string Add = txtSearchAdd.Text;
                int PageSize = 10;
                int RowIndex = 0;
                int PageIndex = Session["pageindexSale"] == null ? 1 : Convert.ToInt32(Session["pageindexSale"]);
                List<CustomerDTO> lst = new List<CustomerDTO>();
                List<CustomerDTO> Temp = new List<CustomerDTO>();                
                using (BillingEntities cre = new BillingEntities())
                {
                    lst = (from h in cre.GetCustomerDist()                         
                           select new CustomerDTO()
                           {
                               CustomerName = h.CustomerName,
                               CustomerAddress = h.CustomerAddress,
                               CustomerDistrict = h.CustomerDistrict,
                               CustomerCountry = h.CustomerCountry,
                               CustomerProvince = h.CustomerProvince,
                               CustomerPostalCode = h.CustomerPostalCode,
                               //CustomerAddress2 = h.CustomerAddress2,
                               //CustomerAddress3 = h.CustomerAddress3,
                               DeliveryName = h.DeliveryName,
                               DeliverAdd = h.DeliverAdd,
                               DeliverDistrict = h.DeliverDistrict,
                               DeliverCountry = h.DeliverCountry,
                               DeliverProvince = h.DeliverProvince,
                               DeliverPostalCode = h.DeliverPostalCode,
                               Tel = h.Tel,
                           }).Distinct().ToList();

                    if (lst != null)
                    {
                        if (!string.IsNullOrEmpty(Name))
                        {
                            lst = lst.Where(w => w.CustomerName.ToLower().Contains(Name.ToLower()) || w.DeliveryName.ToLower().Contains(Name.ToLower())).ToList();
                        }
                        
                        if (!string.IsNullOrEmpty(Add))
                        {
                            lst = lst.Where(w => w.CustomerAddressAll.ToLower().Contains(Add.ToLower()) || w.DeliverAddressAll.ToLower().Contains(Add.ToLower())).ToList();
                        }

                        RowIndex = PageIndex == 1 ? 0 : (PageIndex - 1) * PageSize;
                        if (lst.Count < PageSize)
                        {
                            Temp = lst;
                        }
                        else
                        {
                            Temp = lst.GetRange(RowIndex, PageSize);
                        }
                        
                        gvCust.DataSource = Temp;
                        gvCust.DataBind();

                        if(PageIndex == 1)
                        {
                            btnMPrevious.Visible = false;
                        }
                        else
                        {
                            btnMPrevious.Visible = true;
                        }


                        if ((RowIndex + PageSize) > lst.Count)
                        {
                            btnMNext.Visible = false;
                        }
                        else
                        {
                            btnMNext.Visible = true;
                        }
                    }
                    else
                    {
                        gvCust.DataSource = null;
                        gvCust.DataBind();
                    }
                };
            }
            catch (Exception ex)
            {
                
            }
        }

        protected void ClearTextSearch()
        {
            try
            {
                txtSearchAdd.Text = "";
                txtSearchCust.Text = "";
            }
            catch (Exception ex)
            {

            }
        }

        protected void imgbtnChoose_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                ImageButton imb = (ImageButton)sender;
                if(imb != null)
                {
                    Int32 index = ToInt32(imb.CommandArgument);
                    //ใบเสร็จ
                    txtCustName.Text = gvCust.Rows[index].Cells[1].Text;
                    txtCustAddress.Text = gvCust.Rows[index].Cells[2].Text;
                    txtCustDistrict.Text = gvCust.Rows[index].Cells[3].Text;
                    txtCustCountry.Text = gvCust.Rows[index].Cells[4].Text;
                    txtCustProvince.Text = gvCust.Rows[index].Cells[5].Text;
                    txtCustPostalCode.Text = gvCust.Rows[index].Cells[6].Text;
                    
                    //ส่งสินค้า
                    txtDeliveryName.Text = gvCust.Rows[index].Cells[7].Text;
                    txtDeliverAddress.Text = gvCust.Rows[index].Cells[8].Text;
                    txtDeliverDistrict.Text = gvCust.Rows[index].Cells[9].Text;
                    txtDeliverCountry.Text = gvCust.Rows[index].Cells[10].Text;
                    txtDeliverProvince.Text = gvCust.Rows[index].Cells[11].Text;
                    txtDeliverPostalCode.Text = gvCust.Rows[index].Cells[12].Text;
                    //txtCustAddress2.Text = gvCust.Rows[index].Cells[2].Text;
                    //txtCustAddress3.Text = gvCust.Rows[index].Cells[3].Text;
                    txtTel.Text = gvCust.Rows[index].Cells[13].Text == "&nbsp;" ? "" : gvCust.Rows[index].Cells[13].Text;
                }
            }
            catch (Exception ex)
            {
                
            }
        }

        protected void btnCopyAdd_Click(object sender, EventArgs e)
        {
            try
            {
                txtDeliveryName.Text = txtCustName.Text;
                txtDeliverAddress.Text = txtCustAddress.Text;
                txtDeliverDistrict.Text = txtCustDistrict.Text;
                txtDeliverCountry.Text = txtCustCountry.Text;
                txtDeliverProvince.Text = txtCustProvince.Text;
                txtDeliverPostalCode.Text = txtCustPostalCode.Text;
                //txtDeliverAddress2.Text = txtCustAddress2.Text;
                //txtDeliverAddress3.Text = txtCustAddress3.Text;
            }
            catch (Exception ex)
            {
                
            }
        }

        protected void btnMPrevious_Click(object sender, EventArgs e)
        {
            try
            {
                ModalPopupExtender2.Show();
                int PageIndex = Session["pageindexSale"] == null ? 1 : Convert.ToInt32(Session["pageindexSale"]);
                Session["pageindexSale"] = PageIndex - 1;
                BindSearch();
            }
            catch (Exception ex)
            {

            }
        }

        protected void btnMNext_Click(object sender, EventArgs e)
        {
            try
            {
                ModalPopupExtender2.Show();
                int PageIndex = Session["pageindexSale"] == null ? 1 : Convert.ToInt32(Session["pageindexSale"]);
                Session["pageindexSale"] = PageIndex + 1;
                BindSearch();
            }
            catch (Exception ex)
            {

            }
        }
        #endregion

        #region Modal Item
        protected void btnMItemSearch_Click(object sender, EventArgs e)
        {
            try
            {
                ModalPopupExtender1.Show();
                ModalPopupExtender3.Show();
                SearchItem();
                //ClearTextSearchItem();
            }
            catch (Exception ex)
            {
                
            }
        }

        protected void imgbtnChooseItem_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                ModalPopupExtender1.Show();
                ImageButton imb = (ImageButton)sender;
                HiddenField hdd = new HiddenField();
                HiddenField hddStock = new HiddenField();
                TextBox txt = new TextBox();
                Int32 i = 1;
                if (imb != null)
                {
                    Int32 index = ToInt32(imb.CommandArgument);
                    hdd = (HiddenField)gvItemSearch.Rows[index].FindControl("hddItemID");
                    hddStock = (HiddenField)gvItemSearch.Rows[index].FindControl("hddStockID");
                    if (hdd != null && hddStock != null)
                    {
                        hddItemID.Value = hdd.Value;
                        hddStockID.Value = hddStock.Value;
                        txtMItem.Text = gvItemSearch.Rows[index].Cells[1].Text;
                        txtMPrice.Text = gvItemSearch.Rows[index].Cells[2].Text;
                        //txtMSN.Text = gvItemSearch.Rows[index].Cells[2].Text;
                        //txtMDescription.Text = gvItemSearch.Rows[index].Cells[3].Text;
                        txtMDescription.Text = ((Label)gvItemSearch.Rows[index].FindControl("lbItemDesc")).Text;
                        #region ItemDetail
                        //var dal = ItemDal.Instance;
                        //List<MasItemDTO> lst = dal.GetSearchItemDetailByID(ToInt32(hddItemID.Value));
                        //if (lst != null)
                        //{
                        //    foreach (MasItemDTO item in lst)
                        //    {
                        //        txt = (TextBox)Panel1.FindControl("txtMDesc" + i.ToString());
                        //        if(txt != null)
                        //        {
                        //            txt.Text = item.ItemDetail;
                        //            txt.ReadOnly = item.CanChange == "N" ? true : false;
                        //            txt.Visible = !string.IsNullOrEmpty(item.ItemDetail);
                        //        }

                        //        i++;
                        //    }
                        //}
                        #endregion
                    }                    
                }
            }
            catch (Exception ex)
            {

            }
        }

        protected void imgbtnSearchItem_Click_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                ModalPopupExtender1.Show();
                ModalPopupExtender3.Show();
                SearchItem();                
            }
            catch (Exception ex)
            {
                
            }
        }

        protected void SearchItem()
        {
            try
            {
                string ItemCode = txtSearchItemCode.Text.ToLower();
                string ItemName = txtSearchItemName.Text.ToLower();
                List<Entities.DTO.InventoryDTO> lst = new List<Entities.DTO.InventoryDTO>();
                var dal = StockDal.Instance;
                lst = dal.GetItemInStock();
                if (lst != null)
                {
                    if (!string.IsNullOrEmpty(ItemCode))
                        lst = lst.Where(w => w.ItemCode.ToLower().Contains(ItemCode)).ToList();

                    if (!string.IsNullOrEmpty(ItemName))
                    {
                        lst = lst.Where(w => w.ItemName.ToLower().Contains(ItemName)).ToList();
                    }

                    lst = lst.OrderBy(od => od.ItemID).OrderBy(od => od.StockID).ToList();
                    gvItemSearch.DataSource = lst;
                    gvItemSearch.DataBind();
                }
                else
                {
                    gvItemSearch.DataSource = null;
                    gvItemSearch.DataBind();
                }
            }
            catch (Exception ex)
            {
                gvItemSearch.DataSource = null;
                gvItemSearch.DataBind();
            }
        }

        protected void btnModalClose3_Click(object sender, EventArgs e)
        {
            ModalPopupExtender1.Show();
        }

        protected void ClearTextSearchItem()
        {
            try
            {
                txtSearchItemCode.Text = "";
                txtSearchItemName.Text = "";
            }
            catch (Exception ex)
            {

            }
        }
        #endregion
      
    }
}