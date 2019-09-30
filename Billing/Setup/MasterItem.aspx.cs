using Billing.AppData;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Billing.Setup
{
    public partial class MasterItem : Billing.Common.BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
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
                List<MasItem> lst = new List<MasItem>();
                string code = txtCode.Text;
                string name = txtName.Text;
                using (BillingEntities cre = new BillingEntities())
                {
                    lst = cre.MasItems.Where(w => w.ItemCode.Contains(code) &&
                                w.ItemName.Contains(name) && w.Active.Equals("Y")).ToList();

                };

                if (lst != null && lst.Count > 0)
                    gv.DataSource = lst;
                else
                    gv.DataSource = null;

                gv.DataBind();
            }
            catch (Exception ex)
            {
                SendMailError(ex.Message, System.Reflection.MethodBase.GetCurrentMethod());
            }
        }

        protected void btnAdd_Click(object sender, EventArgs e)
        {
            try
            {
                txtMCode.Text = "";
                txtMName.Text = "";
                txtMDesc.Text = "";
                //txtMPrice.Text = "";
                txtMPrice.Text = "";

                ModalPopupExtender1.Show();
                hddMode.Value = "Add";
                hddID.Value = "";
            }
            catch (Exception ex)
            {
                SendMailError(ex.Message, System.Reflection.MethodBase.GetCurrentMethod());
            }

        }

        protected void btnModalSave_Click(object sender, EventArgs e)
        {
            try
            {
                //Validate
                //if (txtMCode.Text == "")
                //{
                //    ShowMessageBox("กรุณาระบุ รหัสสินค้า !!!");
                //    ModalPopupExtender1.Show();
                //    return;
                //}

                if (txtMName.Text == "")
                {
                    ShowMessageBox("กรุณาระบุ สินค้า !!!");
                    ModalPopupExtender1.Show();
                    return;
                }

                MasItem o = new MasItem();
                if (hddMode.Value == "Add") // Add
                {
                    o = new MasItem();
                    o.ItemCode = txtMCode.Text;
                    o.ItemName = txtMName.Text;
                    o.ItemDesc = txtMDesc.Text;
                    o.ItemPrice = ToDoudle(txtMPrice.Text);
                    o.Active = "Y";
                    o.CreatedBy = GetUsername();
                    o.CreatedDate = DateTime.Now;
                    using (BillingEntities cre = new BillingEntities())
                    {
                        cre.MasItems.Add(o);
                        cre.SaveChanges();
                    };
                }
                else //Edit
                {
                    int objID = ToInt32(hddID.Value);
                    using (BillingEntities cre = new BillingEntities())
                    {
                        o = cre.MasItems.FirstOrDefault(w => w.ItemID.Equals(objID));
                        if (o != null)
                        {
                            o.ItemCode = txtMCode.Text;
                            o.ItemName = txtMName.Text;
                            o.ItemDesc = txtMDesc.Text;
                            o.ItemPrice = ToDoudle(txtMPrice.Text);
                            o.UpdatedBy = GetUsername();
                            o.UpdatedDate = DateTime.Now;
                        }
                        cre.SaveChanges();
                    };
                }

                BindData();
                ShowMessageBox("บันทึกข้อมูลสำเร็จ.");
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
                ImageButton imb = (ImageButton)sender;
                MasItem obj = new MasItem();
                if (imb != null)
                {
                    int objID = ToInt32(imb.CommandArgument);
                    using (BillingEntities cre = new BillingEntities())
                    {
                        obj = cre.MasItems.FirstOrDefault(w => w.ItemID.Equals(objID));
                    };

                    if (obj != null)
                    {
                        hddID.Value = imb.CommandArgument;
                        txtMCode.Text = obj.ItemCode;
                        txtMName.Text = obj.ItemName;
                        txtMDesc.Text = obj.ItemDesc;
                        //txtMPrice.Text = obj.ItemPrice.HasValue ? obj.ItemPrice.Value.ToString("###,##0") : "0";
                        txtMPrice.Text = obj.ItemPrice.HasValue ? obj.ItemPrice.Value.ToString("###,##0") : "0";
                        ModalPopupExtender1.Show();
                        hddMode.Value = "Edit";
                    }
                    else
                    {
                        SendMailError("obj is null, objID = " + imb.CommandArgument, System.Reflection.MethodBase.GetCurrentMethod());
                    }
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

        protected void imgbtnDelete_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                ImageButton imb = (ImageButton)sender;
                if (imb != null)
                {
                    MasItem obj = new MasItem();
                    int objID = ToInt32(imb.CommandArgument);
                    using (BillingEntities cre = new BillingEntities())
                    {
                        obj = cre.MasItems.FirstOrDefault(w => w.ItemID.Equals(objID));
                        obj.Active = "N";
                        //cre.MasItems.Remove(obj);
                        cre.SaveChanges();
                    };
                    BindData();
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
    }
}