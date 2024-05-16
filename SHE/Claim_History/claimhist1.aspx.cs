using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SHE.Code;

namespace SHE.Claim_History
{
    public partial class claimhist1 : System.Web.UI.Page
    {
        EncryptDecrypt dc = new EncryptDecrypt();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (!string.IsNullOrEmpty(Request.QueryString["alert"]))
                {
                    lblAlertMessage.Text = Request.QueryString["alert"];
                    lblAlertMessage.CssClass = "alert alert-warning"; // Add CSS class for styling
                    lblAlertMessage.Attributes.Add("data-alert-type", "custom"); // Add custom attribute to identify the alert type
                    lblAlertMessage.Visible = true;
                }
            }

        }

        protected void claimhist_submit_Click(object sender, EventArgs e)
        {
            string policy = policyno.Value;
            string epfno = epf.Value;

            if((policy == null || policy == "") & (epfno == null || epfno == ""))
            {
                //error2.Visible = true;
            }
            else
            {
                Response.Redirect("~/Claim_History/claimhist2.aspx?POLICYNO=" + dc.Encrypt(policy) + "&EPF=" + dc.Encrypt(epfno)+ "&backBtnToDefault=true");
                //error2.Visible = false;
            }
           
        }

        protected void exitbutton_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/default.aspx");
        }

        protected void reset_Click(object sender, EventArgs e)
        {
            policyno.Value = null;
            epf.Value = null;

        }
    }
}