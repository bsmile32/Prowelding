using Billing.AppData;
using Billing.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Billing.Setup
{
    public partial class UserLogin : BasePage
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
                List<MasUserLogin> lst = new List<MasUserLogin>();
                string name = txtName.Text;
                using (BillingEntities cre = new BillingEntities())
                {
                    lst = cre.MasUserLogins.Where(w => w.Username.Contains(name)).ToList();
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
                txtMUsername.Text = "";
                txtMNewPass.Text = "";
                txtMCheckPass.Text = "";
                chkActive.Checked = true;

                txtMOldPass.Text = "";
                resultOldPass.Visible = false;

                ModalPopupExtender1.Show();
                hddMode.Value = "Add";
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
                if (txtMNewPass.Text == "" || txtMCheckPass.Text == "")
                {
                    ShowMessageBox("กรุณาระบุ Password !!!");
                    ModalPopupExtender1.Show();
                    return;
                }

                if (txtMNewPass.Text != txtMCheckPass.Text)
                {
                    ShowMessageBox("ระบุ Password ไม่ตรงกัน !!!");
                    ModalPopupExtender1.Show();
                    return;
                }

                MasUserLogin o = new MasUserLogin();
                if (hddMode.Value == "Add") // Add
                {


                    o = new MasUserLogin();
                    o.Username = txtMUsername.Text;
                    o.Password = txtMNewPass.Text;
                    o.Active = chkActive.Checked ? "Y" : "N";
                    using (BillingEntities cre = new BillingEntities())
                    {
                        cre.MasUserLogins.Add(o);
                        cre.SaveChanges();
                    };

                    BindData();
                    ShowMessageBox("บันทึกข้อมูลสำเร็จ.");
                }
                else //Edit
                {
                    using (BillingEntities cre = new BillingEntities())
                    {
                        o = cre.MasUserLogins.FirstOrDefault(w => w.Username.Equals(txtMUsername.Text));
                        if (o != null)
                        {
                            if (o.Password != txtMOldPass.Text)
                            {
                                ShowMessageBox("Password ไม่ถูกต้อง !!!");
                                ModalPopupExtender1.Show();
                            }
                            else
                            {
                                o.Password = txtMNewPass.Text;
                                o.Active = chkActive.Checked ? "Y" : "N";
                                cre.SaveChanges();

                                BindData();
                                ShowMessageBox("บันทึกข้อมูลสำเร็จ.");
                            }

                        }

                    };
                }
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
                MasUserLogin obj = new MasUserLogin();
                if (imb != null)
                {
                    using (BillingEntities cre = new BillingEntities())
                    {
                        obj = cre.MasUserLogins.FirstOrDefault(w => w.Username.Equals(imb.CommandArgument));
                    };

                    if (obj != null)
                    {
                        txtMUsername.Text = obj.Username;
                        chkActive.Checked = obj.Active == "Y" ? true : false;

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
                    MasUserLogin obj = new MasUserLogin();
                    int objID = ToInt32(imb.CommandArgument);
                    using (BillingEntities cre = new BillingEntities())
                    {
                        obj = cre.MasUserLogins.FirstOrDefault(w => w.Username.Equals(imb.CommandArgument));
                        obj.Active = "N";
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