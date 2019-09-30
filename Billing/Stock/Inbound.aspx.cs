using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Entities.DTO;
using DAL;
using Entities;

namespace Billing.Stock
{
    public partial class Inbound : Billing.Common.BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                Session["InBoundDetail"] = null;
                BindDataGrid();
            }
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                string err = "";
                List<InventoryDTO> lst = new List<InventoryDTO>();
                if (Session["InBoundDetail"] != null)
                {
                    lst = (List<InventoryDTO>)Session["InBoundDetail"];
                    if (lst != null && lst.Count > 0)
                    {
                        var dal = StockDal.Instance;
                        err = dal.InsertStock(lst, GetUsername());
                    }
                    else
                    {
                        ShowMessageBox("กรุณาเพิ่ม สินค้า ที่ต้องการบันทึก!!");
                        return;
                    }
                }
                //else
                //{
                //    ShowMessageBox("Session Timeout. !!", this, "InBound.aspx");
                //    return;
                //}

                if (!string.IsNullOrEmpty(err))
                {
                    ShowMessageBox("เกิดข้อผิดพลาด กรุณาติดต่อผู้ดูแลระบบ.!!" + err);
                    return;
                }
                ShowMessageBox("บันทึกสำเร็จ.", this.Page, "InventoryList.aspx");
            }
            catch (Exception ex)
            {
                ShowMessageBox("เกิดข้อผิดพลาด กรุณาติดต่อผู้ดูแลระบบ.");
                SendMailError(ex.Message, System.Reflection.MethodBase.GetCurrentMethod());
            }
        }

        #region Event Detail
        protected void btnAdd_Click(object sender, EventArgs e)
        {
            try
            {
                #region Validate
                if (txtItem.Text == "")
                {
                    ShowMessageBox("กรุณาระบุ สินค้า");
                    return;
                }

                if (txtAmount.Text == "" || txtAmount.Text == "0")
                {
                    ShowMessageBox("กรุณาระบุ จำนวนรับเข้า");
                    return;
                }
                #endregion

                int tid = 1;
                List<InventoryDTO> lst = new List<InventoryDTO>();
                List<TransStock> lstS = new List<TransStock>();
                InventoryDTO o = new InventoryDTO();
                if (Session["InBoundDetail"] != null)
                {
                    lst = (List<InventoryDTO>)Session["InBoundDetail"];
                    tid = lst.Count() == 0 ? 1 : lst.Select(s => s.TempID).Max() + 1;
                }
                
                o.TempID = tid;
                o.ItemID = ToInt32(hddItemID.Value);
                o.ItemCode = hddItemCode.Value;
                o.ItemName = txtItem.Text;
                o.Amount = ToInt32(txtAmount.Text);
                o.Serial = txtSerial.Text;
                o.Remark = txtRemark.Text;

                var dal = StockDal.Instance;
                lstS = dal.CheckRemainingItem(o.ItemID);
                if(lstS != null && lstS.Count > 0)
                {
                    o.Remaining = lstS.Count;
                }

                lst.Add(o);
                Session["InBoundDetail"] = lst;
                ClearTextAddItem();
                BindDataGrid();
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
                    if (Session["InBoundDetail"] != null)
                    {
                        lst = (List<InventoryDTO>)Session["InBoundDetail"];
                        if (lst != null && lst.Count > 0)
                        {
                            obj = lst.FirstOrDefault(w => w.TempID.Equals(objID));
                            if (obj != null)
                                lst.Remove(obj);
                            Session["InBoundDetail"] = lst;
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

        protected void ClearTextAddItem()
        {
            try
            {
                hddItemID.Value = "";
                hddItemCode.Value = "";
                txtItem.Text = "";
                txtAmount.Text = "";
                txtSerial.Text = "";
                txtRemark.Text = "";
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
                if (Session["InBoundDetail"] != null)
                {
                    lst = (List<InventoryDTO>)Session["InBoundDetail"];
                    lst = lst.OrderByDescending(od => od.TempID).ToList();
                }
                else
                {
                    lst = null;
                }

                if(lst == null || lst.Count() == 0)
                {
                    btnSave.Visible = false;
                    btnCancel.Visible = false;
                }
                else
                {
                    btnSave.Visible = true;
                    btnCancel.Visible = true;
                }

                gvItem.DataSource = lst;
                gvItem.DataBind();
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
                ModalPopupExtender3.Show();
                SearchItem();
            }
            catch (Exception ex)
            {

            }
        }

        protected void imgbtnChooseItem_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
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
                        hddItemCode.Value = gvItemSearch.Rows[index].Cells[0].Text;
                        txtItem.Text = gvItemSearch.Rows[index].Cells[1].Text;
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
                var dal = ItemDal.Instance;
                lst = dal.GetSearchItem();
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

                #region Comment
                //using (BillingEntities cre = new BillingEntities())
                //{
                //    lst = (from i in cre.MasItems
                //           join u in cre.MasUnits on i.UnitID equals u.UnitID into u1
                //           from u in u1.DefaultIfEmpty()
                //           //join u in cre.tra on i.UnitID equals u.UnitID
                //           where i.Active.Equals("Y")
                //           select new MasItemDTO()
                //           {
                //               ItemID = i.ItemID,
                //               ItemName = i.ItemName,
                //               ItemCode = i.ItemCode,
                //               ItemDesc = i.ItemDesc,
                //               UnitName = u.UnitName,
                //               ItemPrice = i.ItemPrice.HasValue ? i.ItemPrice.Value : 0,
                //           }).ToList();

                //    if (lst != null)
                //    {
                //        if (!string.IsNullOrEmpty(ItemCode))
                //            lst = lst.Where(w => w.ItemCode.ToLower().Contains(ItemCode)).ToList();

                //        if (!string.IsNullOrEmpty(ItemName))
                //        {
                //            lst = lst.Where(w => w.ItemName.ToLower().Contains(ItemName)).ToList();
                //        }

                //        lst = lst.OrderBy(od => od.ItemID).ToList();
                //        gvItemSearch.DataSource = lst;
                //        gvItemSearch.DataBind();
                //    }
                //    else
                //    {
                //        gvItemSearch.DataSource = null;
                //        gvItemSearch.DataBind();
                //    }
                //};
                #endregion
            }
            catch (Exception ex)
            {
                gvItemSearch.DataSource = null;
                gvItemSearch.DataBind();
            }
        }

        protected void btnModalClose3_Click(object sender, EventArgs e)
        {
            //ModalPopupExtender1.Show();
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