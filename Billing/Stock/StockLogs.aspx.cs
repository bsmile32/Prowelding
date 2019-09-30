using Billing.AppData;
using Billing.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Billing.Stock
{
    public partial class StockLogs : Billing.Common.BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                txtDateFrom.Text = DateTime.Now.AddDays(-7).ToString("dd/MM/yyyy", new System.Globalization.CultureInfo("en-US"));
                txtDateTo.Text = DateTime.Now.ToString("dd/MM/yyyy", new System.Globalization.CultureInfo("en-US"));
                BindData();
            }
        }

        #region BindData
        protected void BindData()
        {
            try
            {
                if(string.IsNullOrEmpty(txtDateFrom.Text) || string.IsNullOrEmpty(txtDateTo.Text))
                {
                    ShowMessageBox("กรุณาระบุวันที่ที่ต้องการค้นหา. !!!");
                    return;
                }
                List<InventoryDTO> lst = new List<InventoryDTO>();
                string Code = txtItemCode.Text;
                string Name = txtItemName.Text;
                DateTime dateFrom = string.IsNullOrEmpty(txtDateFrom.Text) ? DateTime.MinValue : DateTime.ParseExact(txtDateFrom.Text, "dd/MM/yyyy", new System.Globalization.CultureInfo("en-US"));
                DateTime dateTo = string.IsNullOrEmpty(txtDateTo.Text) ? DateTime.MaxValue : DateTime.ParseExact(txtDateTo.Text, "dd/MM/yyyy", new System.Globalization.CultureInfo("en-US")).AddDays(1);
                using (BillingEntities cre = new BillingEntities())
                {
                    //lst = cre.Inventories.Where(w => w.Amount > 0).ToList();
                    //lst = (from l in cre.InventoryLogs
                    //       join i in cre.Inventories on l.InventoryID equals i.InventoryID
                    //       join m in cre.MasItems on i.ItemID equals m.ItemID
                    //       where l.Amount > 0 
                    //       && (l.Action.Equals("Inbound") || l.Action.Equals("Outbound"))
                    //       && l.UpdatedDate >= dateFrom && l.UpdatedDate < dateTo
                    //       select new InventoryDTO()
                    //       {
                    //           InventoryID = l.InventoryID,
                    //           ItemID = m.ItemID,
                    //           ItemCode = m.ItemCode,
                    //           ItemName = m.ItemName,
                    //           Amount = l.Amount.HasValue ? l.Amount.Value : 0,
                    //           UpdatedDate = l.UpdatedDate.HasValue ? l.UpdatedDate.Value : DateTime.Now,
                    //           UpdatedBy = l.UpdatedBy,
                    //       }).OrderByDescending(od => od.UpdatedDate).ThenBy(od => od.ItemCode).ToList();
                };                

                if (lst != null && lst.Count > 0)
                {
                    gv.DataSource = lst;
                }
                else
                {
                    gv.DataSource = null;
                }

                gv.DataBind();
            }
            catch (Exception ex)
            {

            }
        }
        #endregion


        protected void btnSearch_Click(object sender, EventArgs e)
        {
            try
            {
                BindData();
            }
            catch (Exception ex)
            {
                
            }
        }        
      
    }
}