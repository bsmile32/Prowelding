using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Billing
{
    public partial class Site1 : System.Web.UI.MasterPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            Common.BasePage bp = new Common.BasePage();
            bp.CheckUser();
            string user = bp.GetUsername(); // DateTime.Now.ToString("dd/MM/yyyy HH:mm"); 
            Username.InnerHtml = user;
            CheckAdmin(user);
        }

        protected void CheckAdmin(string user)
        {
            try
            {
                if(user.ToLower() == "admin" || user.ToLower() == "superadmin")
                {
                    liAdmin.Visible = true;
                    liReport.Visible = true;
                }

                if (user.ToLower() == "nut")
                {
                    liAdmin.Visible = true;
                }

                if (user.ToLower() == "fern")
                {
                    liReport.Visible = true;
                }
            }
            catch (Exception)
            {
                
            }
        }
    }
}