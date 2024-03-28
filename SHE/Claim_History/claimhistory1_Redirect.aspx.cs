using SHE.Code;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SHE.Claim_History
{
    public partial class claimhistory1_Redirect : System.Web.UI.Page
    {
        //EncryptDecrypt dc = new EncryptDecrypt();
        EncryptDecrypt dc = new EncryptDecrypt();
        protected void Page_Load(object sender, EventArgs e)
        {
            string policy = Request.QueryString["policy"];
            string epfno = Request.QueryString["epf"];

            policy = dc.Decrypt(policy);
            epfno = dc.Decrypt(epfno);

            policyno.Value = policy;

            epf.Value = epfno;
        }

        protected void claimhist_submit_Click(object sender, EventArgs e)
        {
            string policy = policyno.Value;
            string epfno = epf.Value;
            Response.Redirect("~/Claim_History/claimhist2.aspx?POLICYNO=" + dc.Encrypt(policy) + "&EPF=" + dc.Encrypt(epfno));
        }

        protected void exitbutton_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/default.aspx");
        }
    }
}




