using System;
using SHE.Code;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.Odbc;
using System.Data.OracleClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace SHE.Inquiry
{
    public partial class inquiry1 : System.Web.UI.Page
    {
        private const string host_ip = "http://172.24.90.100:8084";
        //private const string host_ip = "http://receipt-app/Beegeneral";

        private HtmlGenericControl control;
        OdbcConnection db2conn = new OdbcConnection(ConfigurationManager.AppSettings["DB2"]);
        OracleConnection oconn = new OracleConnection(ConfigurationManager.AppSettings["OracleDB"]);
        OracleConnection oconnLife = new OracleConnection(ConfigurationManager.AppSettings["OraLifeDB"]);


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

                    // JavaScript to redirect to a new page after the alert is dismissed
                    string redirectScript = @"
                    <script>
                        function redirectAfterAlert() {
                            window.location.href = 'inquiry1.aspx';
                        }
                    </script>";
                    ClientScript.RegisterStartupScript(this.GetType(), "RedirectAfterAlertScript", redirectScript);

                    // Register a client-side script to call the redirect function after the alert is dismissed
                    string alertScript = @"
                    <script>
                        function custom_alert(message, title) {
                            if (!title)
                                title = 'Alert';

                            if (!message)
                                message = 'No Message to Display.';

                            if (title == 'Alert') {
                    swal({
                                    title: title,
                                    text: message,
                        icon: 'warning',
                                    button: true,
                                    closeOnClickOutside: false,
                                }).then((value) => {
                                               if (value) {
                                        redirectAfterAlert();
                                    }
                                });
                            } else if (title == 'Success') {
                                swal({
                                    title: title,
                                    text: message,
                                    icon: 'success',
                                    button: false,
                                    closeOnClickOutside: false,
                                });
                            }
                        }
                        custom_alert('Not found test', 'Alert');
                    </script>";
                    ClientScript.RegisterStartupScript(this.GetType(), "CustomAlertScript", alertScript);



                }
                string RadiValue = RadioButtonList1.SelectedValue.ToString();

                if (RadiValue == "Date")
                {
                    panel1.Visible = true;
                    panel2.Visible = false;
                    inquiry_submit.Enabled = true;
                }
                else if (RadiValue == "Reference")
                {
                    panel2.Visible = true;
                    panel1.Visible = false;
                    inquiry_submit.Enabled = true;
                }
                else
                {
                    inquiry_submit.Enabled = false;
                    //Label3.Visible = true;
                }
            }




        }

        protected void inquiry_submit_Click(object sender, EventArgs e)
        {

            string fromDateValue = fromdate.Value;
            string toDateValue = todate.Value;
            string clmno = reference.Value;
            string selectedValue = RadioButtonList1.SelectedValue.ToString();

            EncryptDecrypt dc = new EncryptDecrypt();
            Response.Redirect("~/Inquiry/HospitalizeList.aspx?fromdate=" + dc.Encrypt(fromDateValue) + "&todate=" + dc.Encrypt(toDateValue) + "&reference=" + dc.Encrypt(clmno));

        }

        protected void exitbutton_Click(object sender, EventArgs e)
        {

            Response.Redirect("~/Default.aspx?fromdate=");

        }

        protected void reset_Click(object sender, EventArgs e)
        {
            fromdate.Value = null;
            todate.Value = null;
            reference.Value = null;

        }



        protected void RadioButtonList1_SelectedIndexChanged(object sender, EventArgs e)
        {
            string RadiValue = RadioButtonList1.SelectedValue.ToString();

            if (RadiValue == "Date")
            {
                panel1.Visible = true;
                panel2.Visible = false;
                inquiry_submit.Enabled = true;
            }
            else if (RadiValue == "Reference")
            {
                panel2.Visible = true;
                panel1.Visible = false;
                inquiry_submit.Enabled = true;
            }
            else
            {
                inquiry_submit.Enabled = false;
                //Label3.Visible = true;
            }
        }
    }
}