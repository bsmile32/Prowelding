using Billing.AppData;
//using Billing.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Entities.DTO;

namespace Billing.Stock
{
    public partial class Outbound : Billing.Common.BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                txtDate.Text = DateTime.Now.ToString("dd/MM/yyyy", new System.Globalization.CultureInfo("en-US"));
                Session["OutboundDetail"] = null;
                BindDataGrid();
            }
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                //Validate
                if (txtDate.Text == "")
                {
                    ShowMessageBox("กรุณาระบุ วันที่");
                    return;
                }

                List<InventoryDTO> lst = new List<InventoryDTO>();
                if (Session["OutboundDetail"] != null)
                {
                    lst = (List<InventoryDTO>)Session["OutboundDetail"];
                    if(lst != null && lst.Count > 0)
                    {
                        foreach (InventoryDTO item in lst)
                        {
                            ManageStock(item.ItemID, item.Amount, "Outbound", GetUsername());
                        }
                    }
                }
                
                ShowMessageBox("บันทึกสำเร็จ.", this.Page, "InventoryList.aspx");
            }
            catch (Exception ex)
            {
                ShowMessageBox("เกิดข้อผิดพลาด กรุณาติดต่อผู้ดูแลระบบ.");
                SendMailError(ex.Message, System.Reflection.MethodBase.GetCurrentMethod());
            }
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
                    List<InventoryDTO> lst = new List<InventoryDTO>();
                    InventoryDTO obj = new InventoryDTO();
                    if (Session["OutboundDetail"] != null)
                    {
                        lst = (List<InventoryDTO>)Session["OutboundDetail"];
                        if (lst != null && lst.Count > 0)
                        {
                            obj = lst.FirstOrDefault(w => w.TempID.Equals(objID));
                            if (obj != null)
                            {
                                hddDetailID.Value = obj.TempID.ToString();
                                hddItemID.Value = obj.ItemID.ToString();
                                txtMItem.Text = obj.ItemName;
                                txtMItemCode.Text = obj.ItemCode;
                                txtMAmount.Text = obj.AmountStr;
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
                    List<InventoryDTO> lst = new List<InventoryDTO>();
                    InventoryDTO obj = new InventoryDTO();
                    if (Session["OutboundDetail"] != null)
                    {
                        lst = (List<InventoryDTO>)Session["OutboundDetail"];
                        if (lst != null && lst.Count > 0)
                        {
                            obj = lst.FirstOrDefault(w => w.TempID.Equals(objID));
                            if (obj != null)
                                lst.Remove(obj);
                            Session["OutboundDetail"] = lst;
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

            }
        }

        protected void btnMSave_Click(object sender, EventArgs e)
        {
            try
            {
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
                #endregion

                #region Save to Session
                List<InventoryDTO> lst = new List<InventoryDTO>();
                InventoryDTO o = new InventoryDTO();
                if (Session["OutboundDetail"] != null)
                {
                    lst = (List<InventoryDTO>)Session["OutboundDetail"];
                }

                if (hddDetailID.Value != "" && hddDetailID.Value != "0")
                {
                    Int32 did = ToInt32(hddDetailID.Value);
                    o = lst.FirstOrDefault(w => w.TempID.Equals(did));
                    if (o != null)
                    {
                        o.ItemID = ToInt32(hddItemID.Value);
                        o.ItemName = txtMItem.Text;
                        o.ItemCode = txtMItemCode.Text;
                        o.Amount = ToInt32(txtMAmount.Text);
                        //o.ItemDescription = txtMDescription.Text;
                    }
                }
                else
                {
                    o = new InventoryDTO();
                    o.TempID = lst.Count == 0 ? 1 : ToInt32(lst.Max(m => m.TempID).ToString()) + 1;
                    o.ItemID = ToInt32(hddItemID.Value);
                    o.ItemName = txtMItem.Text;
                    o.ItemCode = txtMItemCode.Text;
                    o.Amount = ToInt32(txtMAmount.Text);
                    lst.Add(o);
                }

                Session["OutboundDetail"] = lst;
                #endregion

                //ShowMessageBox("Save Succ");
                BindDataGrid();
                gvItem.Focus();
            }
            catch (Exception ex)
            {

            }
        }

        protected void BindDataGrid()
        {
            try
            {
                List<InventoryDTO> lst = new List<InventoryDTO>();
                if (Session["OutboundDetail"] != null)
                {
                    lst = (List<InventoryDTO>)Session["OutboundDetail"];
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

        protected void ClearTextDetail()
        {
            hddDetailID.Value = "0";
            hddItemID.Value = "0";
            txtMItem.Text = "";
            txtMItemCode.Text = "";
            txtMAmount.Text = "1";
            lbAmount.Text = "";
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
                ClearTextSearchItem();
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
                HiddenField hddDesc = new HiddenField();
                if (imb != null)
                {
                    Int32 index = ToInt32(imb.CommandArgument);
                    hdd = (HiddenField)gvItemSearch.Rows[index].FindControl("hddItemID");
                    if (hdd != null)
                    {
                        hddDesc = (HiddenField)gvItemSearch.Rows[index].FindControl("hddItemID");
                        hddItemID.Value = hdd.Value;                        
                        txtMItem.Text = gvItemSearch.Rows[index].Cells[1].Text;
                        txtMItemCode.Text = gvItemSearch.Rows[index].Cells[0].Text;
                        lbAmount.Text = gvItemSearch.Rows[index].Cells[2].Text;
                        if (ToInt32(gvItemSearch.Rows[index].Cells[2].Text) < 5)
                            lbAmount.ForeColor = System.Drawing.Color.Red;
                        else
                            lbAmount.ForeColor = System.Drawing.Color.Black;
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
                List<MasItemDTO> lst = new List<MasItemDTO>();
                using (BillingEntities cre = new BillingEntities())
                {
                    //lst = (from i in cre.MasItems
                    //       join d in cre.Inventories on i.ItemID equals d.ItemID
                    //       where i.Active.Equals("Y") && d.Amount > 0
                    //       select new MasItemDTO()
                    //       {emName,
                    //           ItemCode = i.I
                    //           ItemID = i.ItemID,
                    //           ItemName = i.IttemCode,
                    //           ItemDesc = i.ItemDesc,
                    //           ItemPrice = i.ItemPrice.HasValue ? i.ItemPrice.Value : 0,
                    //           Amount = d.Amount,
                    //       }).ToList();

                    if (lst != null)
                    {
                        if (!string.IsNullOrEmpty(ItemCode))
                            lst = lst.Where(w => w.ItemCode.ToLower().Contains(ItemCode)).ToList();

                        if (!string.IsNullOrEmpty(ItemName))
                        {
                            lst = lst.Where(w => w.ItemName.ToLower().Contains(ItemName)).ToList();
                        }

                        lst = lst.OrderBy(od => od.ItemID).ToList();
                        gvItemSearch.DataSource = lst;
                        gvItemSearch.DataBind();
                    }
                    else
                    {
                        gvItemSearch.DataSource = null;
                        gvItemSearch.DataBind();
                    }
                };
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