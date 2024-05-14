using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SHE
{
    public partial class SiteMaster : MasterPage
    {
        protected string logUser;
        protected void Page_Load(object sender, EventArgs e)
        {
            logUser = Session["LoggedUser"].ToString().ToUpper();
        }

        protected void Logout_Click(object sender, EventArgs e)
        {
            Session.Clear(); // Clear all session variables
            Session.Abandon(); // End the current session
            Response.Redirect("~/login.aspx");
        }
    }
}