using Billing.AppData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Billing
{
    public partial class Index : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            //txtUser.Text = "admin";
            //txtPass.Text = "9499";
            //btnLogin_Click(sender, new EventArgs());
        }

        protected void btnLogin_Click(object sender, EventArgs e)
        {
            if (txtUser.Text == "")
            {
                lbMsg.Text = "กรุณาระบุ Username !!!";
                return;
            }

            if (txtPass.Text == "")
            {
                lbMsg.Text = "กรุณาระบุ Password !!!";
                return;
            }

            MasUserLogin o = new MasUserLogin();
            Session["Username"] = txtUser.Text;
            using (BillingEntities cre = new BillingEntities())
            {
                o = cre.MasUserLogins.FirstOrDefault(w => w.Username.Equals(txtUser.Text) && w.Password.Equals(txtPass.Text));
                if (o != null && o.Active == "Y")
                {
                    Session["Username"] = o.Username;
                    o.LastLogin = DateTime.Now;
                    cre.SaveChanges();
                }
                else
                {
                    lbMsg.Text = "Username, Password ไม่ถูกต้อง !!!";
                    return;
                }

            };

            Response.Redirect("/Default.aspx");
        }
    }
}