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
    public partial class MasterUnit : Billing.Common.BasePage
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
                List<MasUnit> lst = new List<MasUnit>();
                string Name = txtUnitName.Text;
                string Code = txtUnitCode.Text;
                using (BillingEntities cre = new BillingEntities()){
                    lst = cre.MasUnits.Where(w => w.UnitCode.Contains(Code) &&
                                w.UnitName.Contains(Name)).ToList();

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
                txtMUnitCode.Text = "";
                txtMUnitName.Text = "";

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
                if (txtMUnitCode.Text == "")
                {
                    ShowMessageBox("กรุณาระบุ ชื่อลูกค้า !!!");
                    ModalPopupExtender1.Show();
                    return;
                }

                if (txtMUnitName.Text == "")
                {
                    ShowMessageBox("กรุณาระบุ รหัส !!!");
                    ModalPopupExtender1.Show();
                    return;
                }

                MasUnit o = new MasUnit();
                if (hddMode.Value == "Add") // Add
                {
                    o = new MasUnit();
                    o.UnitCode = txtMUnitCode.Text;
                    o.UnitName = txtMUnitName.Text;
                    o.Active = "Y";
                    o.CreatedBy = GetUsername();
                    o.CreatedDate = DateTime.Now;
                    using (BillingEntities cre = new BillingEntities())
                    {
                        cre.MasUnits.Add(o);
                        cre.SaveChanges();
                    };
                }
                else //Edit
                {
                    int objID = ToInt32(hddID.Value);
                    using (BillingEntities cre = new BillingEntities())
                    {
                        o = cre.MasUnits.FirstOrDefault(w => w.UnitID.Equals(objID));
                        if (o != null)
                        {
                            o.UnitCode = txtMUnitCode.Text;
                            o.UnitName = txtMUnitName.Text;
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
                MasUnit obj = new MasUnit();
                if (imb != null)
                {
                    int objID = ToInt32(imb.CommandArgument);
                    using (BillingEntities cre = new BillingEntities())
                    {
                        obj = cre.MasUnits.FirstOrDefault(w => w.UnitID.Equals(objID));
                    };

                    if (obj != null)
                    {
                        hddID.Value = imb.CommandArgument;
                        txtMUnitCode.Text = obj.UnitCode;
                        txtMUnitName.Text = obj.UnitName;

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
                    MasUnit obj = new MasUnit();
                    int objID = ToInt32(imb.CommandArgument);
                    using (BillingEntities cre = new BillingEntities())
                    {
                        obj = cre.MasUnits.FirstOrDefault(w => w.UnitID.Equals(objID));
                        if (obj != null)
                        {
                            obj.Active = "N";
                            obj.UpdatedBy = GetUsername();
                            obj.UpdatedDate = DateTime.Now;
                        }
                        //cre.MasUnits.Remove(obj);
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