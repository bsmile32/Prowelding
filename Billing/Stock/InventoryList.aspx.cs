
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DAL;
using Entities.DTO;

namespace Billing.Stock
{
    public partial class InventoryList : Billing.Common.BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {                
                BindData();
            }
        }

        #region BindData
        protected void BindData()
        {
            try
            {
                List<InventoryDTO> lst = new List<InventoryDTO>();
                string Code = txtItemCode.Text;
                string Name = txtItemName.Text;
                var dal = StockDal.Instance;
                lst = dal.GetItemStock(Code, Name);
                
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