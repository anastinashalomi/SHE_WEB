using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SHE
{
    public partial class _Default : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (Session.Contents["LoggedUser"] != null)
                {
                    // Session variable exists
                    // Perform necessary actions
                }
                else
                {
                    Response.Redirect("~/login.aspx");
                }
            }

        }

        protected void Test_Click(object sender, EventArgs e)
        {
            ScriptManager.RegisterStartupScript(this, GetType(), "modalShow", "showModal();", true);
        }

        protected void Button1_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/Inquiry/inquiry1.aspx");
        }

        protected void Button2_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/ClaimPayment/ClaimSearch.aspx");
        }

        protected void Exit_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/login.aspx");
        }
    }
}