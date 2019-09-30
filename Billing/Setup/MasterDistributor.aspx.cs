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
    public partial class MasterDistributor : Billing.Common.BasePage
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
                List<MasDistributor> lst = new List<MasDistributor>();
                string Name = txtDistributorName.Text;
                string Code = txtDistributorCode.Text;
                using (BillingEntities cre = new BillingEntities()){
                    lst = cre.MasDistributors.Where(w => w.DistributorName.Contains(Name) &&
                                w.DistributorCode.Contains(Code)).ToList();

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
                txtMDistributorCode.Text = "";
                txtMDistributorName.Text = "";
                txtMDistributorAddress.Text = "";

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
                if (txtMDistributorCode.Text == "")
                {
                    ShowMessageBox("กรุณาระบุ รหัส !!!");
                    ModalPopupExtender1.Show();
                    return;
                }

                if (txtMDistributorName.Text == "")
                {
                    ShowMessageBox("กรุณาระบุ ผู้จัดจำหน่าย !!!");
                    ModalPopupExtender1.Show();
                    return;
                }

                MasDistributor o = new MasDistributor();
                if (hddMode.Value == "Add") // Add
                {
                    o = new MasDistributor();
                    o.DistributorName = txtMDistributorName.Text;
                    o.DistributorCode = txtMDistributorCode.Text;
                    o.DistributorAddress = txtMDistributorAddress.Text;
                    o.CreatedBy = GetUsername();
                    o.CreatedDate = DateTime.Now;
                    using (BillingEntities cre = new BillingEntities())
                    {
                        cre.MasDistributors.Add(o);
                        cre.SaveChanges();
                    };
                }
                else //Edit
                {
                    int objID = ToInt32(hddID.Value);
                    using (BillingEntities cre = new BillingEntities())
                    {
                        o = cre.MasDistributors.FirstOrDefault(w => w.DistributorID.Equals(objID));
                        if (o != null)
                        {
                            o.DistributorName = txtMDistributorName.Text;
                            o.DistributorCode = txtMDistributorCode.Text;
                            o.DistributorAddress = txtMDistributorAddress.Text;
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
                MasDistributor obj = new MasDistributor();
                if (imb != null)
                {
                    int objID = ToInt32(imb.CommandArgument);
                    using (BillingEntities cre = new BillingEntities())
                    {
                        obj = cre.MasDistributors.FirstOrDefault(w => w.DistributorID.Equals(objID));
                    };

                    if (obj != null)
                    {
                        hddID.Value = imb.CommandArgument;
                        txtMDistributorCode.Text = obj.DistributorCode;
                        txtMDistributorName.Text = obj.DistributorName;
                        txtMDistributorAddress.Text = obj.DistributorAddress;

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
                    MasDistributor obj = new MasDistributor();
                    int objID = ToInt32(imb.CommandArgument);
                    using (BillingEntities cre = new BillingEntities())
                    {
                        obj = cre.MasDistributors.FirstOrDefault(w => w.DistributorID.Equals(objID));
                        if (obj != null)
                        {
                            obj.Active = "N";
                            obj.UpdatedBy = GetUsername();
                            obj.UpdatedDate = DateTime.Now;
                        }
                        //cre.MasAccounts.Remove(obj);
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