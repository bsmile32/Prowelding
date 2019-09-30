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
    public partial class MasterSender : Billing.Common.BasePage
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
                List<MasSender> lst = new List<MasSender>();
                string Name = txtSenderName.Text;
                string Tel = txtTel.Text;
                using (BillingEntities cre = new BillingEntities()){
                    lst = cre.MasSenders.Where(w => w.SenderName.Contains(Name) &&
                                w.Tel.Contains(Tel)).ToList();

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
                txtMSenderName.Text = "";
                txtMTel.Text = "";

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
                if (txtMSenderName.Text == "")
                {
                    ShowMessageBox("กรุณาระบุ ชื่อลูกค้า !!!");
                    ModalPopupExtender1.Show();
                    return;
                }

                MasSender o = new MasSender();
                if (hddMode.Value == "Add") // Add
                {
                    o = new MasSender();
                    o.SenderName = txtMSenderName.Text;
                    o.Tel = txtMTel.Text;
                    o.CreatedBy = GetUsername();
                    o.CreatedDate = DateTime.Now;
                    using (BillingEntities cre = new BillingEntities())
                    {
                        cre.MasSenders.Add(o);
                        cre.SaveChanges();
                    };
                }
                else //Edit
                {
                    int objID = ToInt32(hddID.Value);
                    using (BillingEntities cre = new BillingEntities())
                    {
                        o = cre.MasSenders.FirstOrDefault(w => w.SenderID.Equals(objID));
                        if (o != null)
                        {
                            o.SenderName = txtMSenderName.Text;
                            o.Tel = txtMTel.Text;
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
                MasSender obj = new MasSender();
                if (imb != null)
                {
                    int objID = ToInt32(imb.CommandArgument);
                    using (BillingEntities cre = new BillingEntities())
                    {
                        obj = cre.MasSenders.FirstOrDefault(w => w.SenderID.Equals(objID));
                    };

                    if (obj != null)
                    {
                        hddID.Value = imb.CommandArgument;
                        txtMSenderName.Text = obj.SenderName;
                        txtMTel.Text = obj.Tel;

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
                    MasSender obj = new MasSender();
                    int objID = ToInt32(imb.CommandArgument);
                    using (BillingEntities cre = new BillingEntities())
                    {
                        obj = cre.MasSenders.FirstOrDefault(w => w.SenderID.Equals(objID));
                        cre.MasSenders.Remove(obj);
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