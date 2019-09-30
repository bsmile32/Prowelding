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
    public partial class MasterItemType : Billing.Common.BasePage
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
                List<MasItemType> lst = new List<MasItemType>();
                string Code = txtItemTypeCode.Text;
                string Name = txtItemTypeName.Text;
                using (BillingEntities cre = new BillingEntities()){
                    lst = cre.MasItemTypes.Where(w => w.ItemTypeCode.Contains(Code) &&
                        w.ItemTypeName.Contains(Name) && w.Active.Equals("Y")).ToList();

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
                txtMItemTypeCode.Text = "";
                txtMItemTypeName.Text = "";

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
                if (txtMItemTypeCode.Text == "")
                {
                    ShowMessageBox("กรุณาระบุ รหัส !!!");
                    ModalPopupExtender1.Show();
                    return;
                }

                if (txtMItemTypeName.Text == "")
                {
                    ShowMessageBox("กรุณาระบุ ชื่อกลุ่ม !!!");
                    ModalPopupExtender1.Show();
                    return;
                }

                MasItemType o = new MasItemType();
                if (hddMode.Value == "Add") // Add
                {
                    o = new MasItemType();
                    o.ItemTypeName = txtMItemTypeName.Text;
                    o.ItemTypeCode = txtMItemTypeCode.Text;
                    o.Active = "Y";
                    o.CreatedBy = GetUsername();
                    o.CreatedDate = DateTime.Now;
                    using (BillingEntities cre = new BillingEntities())
                    {
                        cre.MasItemTypes.Add(o);
                        cre.SaveChanges();
                    };
                }
                else //Edit
                {
                    int objID = ToInt32(hddID.Value);
                    using (BillingEntities cre = new BillingEntities())
                    {
                        o = cre.MasItemTypes.FirstOrDefault(w => w.ItemTypeID.Equals(objID));
                        if (o != null)
                        {
                            o.ItemTypeName = txtMItemTypeName.Text;
                            o.ItemTypeCode = txtMItemTypeCode.Text;
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
                MasItemType obj = new MasItemType();
                if (imb != null)
                {
                    int objID = ToInt32(imb.CommandArgument);
                    using (BillingEntities cre = new BillingEntities())
                    {
                        obj = cre.MasItemTypes.FirstOrDefault(w => w.ItemTypeID.Equals(objID));
                    };

                    if (obj != null)
                    {
                        hddID.Value = imb.CommandArgument;
                        txtMItemTypeCode.Text = obj.ItemTypeCode;
                        txtMItemTypeName.Text = obj.ItemTypeName;

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
                    MasItemType obj = new MasItemType();
                    int objID = ToInt32(imb.CommandArgument);
                    using (BillingEntities cre = new BillingEntities())
                    {
                        obj = cre.MasItemTypes.FirstOrDefault(w => w.ItemTypeID.Equals(objID));
                        if(obj != null)
                        {
                            obj.Active = "N";
                            obj.UpdatedBy = GetUsername();
                            obj.UpdatedDate = DateTime.Now;
                        }
                        
                        //cre.MasItemTypes.Remove(obj);
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