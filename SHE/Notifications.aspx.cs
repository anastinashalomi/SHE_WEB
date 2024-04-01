using Newtonsoft.Json;
using SHE.App_Code;
using SHE.App_Code.DTO;
using SHE.Code;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.OracleClient;
using System.Data.SqlClient;
using System.Web.UI.WebControls;

namespace SHE
{
    public partial class Notifications : System.Web.UI.Page
    {
        private object dc;
        private object fromDateValue;
        private object toDateValue;
        private string connectionString;

        public object CLAIMINF { get; private set; }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                BindNotificationGrid();

            }
        }

        private void BindNotificationGrid()
        {
            string userid = (string)Session["LoggedUser"];
            string connectionString = ConfigurationManager.AppSettings["OracleDB"];

            // Check if user is logged in
            if (Session["LoggedUser"] == null)
            {
                Response.Redirect("~/login.aspx");
                return;
            }

            // SQL query to fetch data
            string sql = @"SELECT m.PNAME, B.jobhos, B.jobsts, M.claimref FROM shedata.sheglxag S INNER JOIN SHEDATA.SHHOSINF00 m ON S.CSRNAME = M.ciperson INNER JOIN shedata.SHEBKLOG B ON m.claimref = B.jobref WHERE s.csrusrn = :userid ORDER BY M.ADDDATE DESC, M.DISDATE DESC ";

            // Create a list to hold the notification data
            List<NotificationDTO> patientList = new List<NotificationDTO>();

            // Create connection and command objects
            using (OracleConnection oconn = new OracleConnection(connectionString))
            using (OracleCommand cmd = new OracleCommand(sql, oconn))
            {
                // Set parameter value for userid
                cmd.Parameters.Add("userid", OracleType.VarChar).Value = userid;

                // Open connection
                oconn.Open();

                // Execute query and read data
                using (OracleDataReader reader = cmd.ExecuteReader())
                {
                    // Read each row and create NotificationDTO object
                    while (reader.Read())
                    {
                        NotificationDTO notification = new NotificationDTO
                        {
                            PatientName = reader["PNAME"].ToString(),
                            AdmittedType = reader["jobsts"].ToString(),
                            Hospital = reader["jobhos"].ToString(),
                            //ClaimRef1 = reader["claimref"].ToString(),

                        };

                        // Add NotificationDTO object to the list
                        patientList.Add(notification);
                    }
                }
            }

            // Add an empty data row if no data is available
            if (patientList.Count == 0)
            {
                //patientList.Add(new NotificationDTO());
            }

            // Bind data to GridView
            NotificationGrid.DataSource = patientList;
            NotificationGrid.DataBind();
        }

        protected void NotificationGrid_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            NotificationGrid.PageIndex = e.NewPageIndex;
            BindNotificationGrid();
        }



        protected string GetAdmittedTypeCssClass(object admittedType)
        {
            string type = admittedType.ToString().ToLower();
            switch (type)
            {
                case "accepted":
                    return "accepted-color-class";
                case "completed":
                    return "completed-color-class";
                case "not_updated":
                    return "not-updated-color-class";
                case "reassigned":
                    return "reassigned-color-class";
                default:
                    return string.Empty;
            }
        }
        
        protected void NotificationGrid_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                // Make the row clickable
                e.Row.Attributes["onclick"] = Page.ClientScript.GetPostBackClientHyperlink(NotificationGrid, "Select$" + e.Row.RowIndex);
                e.Row.Style["cursor"] = "pointer";

                // Get the NotificationDTO object from the DataItem property
                NotificationDTO patient = e.Row.DataItem as NotificationDTO;

                // Check if the patient is not null
                if (patient != null)
                {
                    // Set CSS class based on AdmittedType
                    e.Row.CssClass = GetAdmittedTypeCssClass(patient.AdmittedType);

                    // Find the Image control within the row
                    Image image = e.Row.FindControl("NotificationImage") as Image;
                    if (image != null)
                    {
                        // Set the image URL based on AdmittedType
                        switch (patient.AdmittedType.Trim().ToLower())
                        {
                            case "accepted":
                                image.ImageUrl = "~/images/Yellowwhite.png";
                                break;
                            case "completed":
                                image.ImageUrl = "~/images/green.png";
                                break;
                            case "not_updated":
                                image.ImageUrl = "~/images/blue.png";
                                break;
                            case "reassigned":
                                image.ImageUrl = "~/images/red.png";
                                break;
                            default:
                                image.ImageUrl = "~/images/default.jpg";
                                break;
                        }
                    }
                }
            }
        }


        protected void NotificationGrid_SelectedIndexChanged(object sender, EventArgs e)
        {

            int selectedIndex = NotificationGrid.SelectedIndex;

            if (selectedIndex >= 0 && selectedIndex < NotificationGrid.Rows.Count)
            {
                // Find the Label control within the selected row
                Label label1 = (Label)NotificationGrid.Rows[selectedIndex].FindControl("Label1");

                if (label1 != null)
                {
                    // Get the value of ClaimRef1 from the Label control
                    string claimRef1 = label1.Text;



                    // Set the text of the label control in PanelTwo to the value of label1
                    // PanelTwoContentLabel.Text = claimRef1;

                    // Show PanelTwo
                    //PanelTwo.Visible = true;

                    PanelOne.Visible = false;
                    PanelTwo.Visible = true;
                    PanelThree.Visible = false;

                    FetchPanelTwoData(claimRef1);
                }
            }
        }


        protected void FetchPanelTwoData(string claimRef1)
        {
            string sqlPanelTwo = @"SELECT M.claimref, M.pname, M.cname, M.cphone, b.jobhos, M.roomno, M.adddate, M.pidno, M.disdate, M.epf, M.policy, M.remark1, b.jobsts FROM shedata.sheglxag S INNER JOIN SHEDATA.SHHOSINF00 M ON S.CSRNAME = M.ciperson INNER JOIN shedata.SHEBKLOG b  ON m.claimref = B.jobref WHERE m.claimref = :claimRef ORDER BY M.ADDDATE DESC, M.DISDATE DESC ";

            using (OracleConnection oconn = new OracleConnection(ConfigurationManager.AppSettings["OracleDB"]))
            using (OracleCommand cmd = new OracleCommand(sqlPanelTwo, oconn))
            {
                cmd.Parameters.Add("claimRef", OracleType.VarChar).Value = claimRef1;

                oconn.Open();

                using (OracleDataReader reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        // Set the text of the labels
                        ClaimReferenceLabel.Text = reader["claimref"].ToString();
                        PatientNameLabel.Text = reader["pname"].ToString();
                        CustomerNameLabel.Text = reader["cname"].ToString();
                        CustomerPhoneLabel.Text = reader["cphone"].ToString();
                        JobHospitalLabel.Text = reader["jobhos"].ToString();
                        RoomNumberLabel.Text = reader["roomno"].ToString();
                        AddDateLabel.Text = reader["adddate"].ToString();
                        PatientIDNumberLabel.Text = reader["pidno"].ToString();
                        DischargeDateLabel.Text = reader["disdate"].ToString();
                        EPFLabel.Text = reader["epf"].ToString();
                        PolicyLabel.Text = reader["policy"].ToString();
                        RemarkLabel.Text = reader["remark1"].ToString();
                        JobStatusLabel.Text = reader["jobsts"].ToString();
                        // You can continue setting other labels here

                        if (JobStatusLabel.Text == "ACCEPTED")
                        {
                            btnAccept.Enabled = false;
                            btnReassign.Enabled = true;
                            btnClaimHistory.Enabled = true;
                            btnClaimPayment.Enabled = true;
                            btnBack.Enabled = true;

                        }
                        else if (JobStatusLabel.Text == "NOT_UPDATED")
                        {
                            btnAccept.Enabled = true;
                            btnReassign.Enabled = true;
                            btnClaimHistory.Enabled = false;
                            btnClaimPayment.Enabled = false;
                            btnBack.Enabled = true;
                        }
                        else if (JobStatusLabel.Text == "COMPLETED")
                        {
                            btnAccept.Enabled = false;
                            btnReassign.Enabled = false;
                            btnClaimHistory.Enabled = true;
                            btnClaimPayment.Enabled = true;
                            btnBack.Enabled = true;
                        }
                        else if (JobStatusLabel.Text == "REASSIGNED")
                        {
                            btnAccept.Enabled = false;
                            btnReassign.Enabled = false;
                            btnClaimHistory.Enabled = false;
                            btnClaimPayment.Enabled = false;
                            btnBack.Enabled = true;
                        }

                    }
                }
            }
        }
        protected void ClaimHistory_Click(object sender, EventArgs e)
        {
            // Assuming 'dc' is an instance of the DataEncryptor class

            EncryptDecrypt dc = new EncryptDecrypt();
            var policy = "";
            var epfno = "";
            if (!string.IsNullOrEmpty(PolicyLabel.Text) || !string.IsNullOrEmpty(EPFLabel.Text))
            {
                policy = dc.Encrypt(PolicyLabel.Text);
                epfno = dc.Encrypt(EPFLabel.Text);
            }


            Response.Redirect("~/Claim_History/claimhistory1_Redirect.aspx?policy=" + policy + "&epf=" + epfno);
        }

        protected void ClaimPayment_Click(object sender, EventArgs e)
        {

            Response.Redirect("~/Claim_History/claimhistory1_Redirect.aspx");
        }


        protected void BackButton_Click(object sender, EventArgs e)
        {
            PanelOne.Visible = true;
            PanelTwo.Visible = false;
            PanelThree.Visible = false;


        }


        protected void SubmitButton_Click(object sender, EventArgs e)
        {


        }
        protected void exitbutton_Click(object sender, EventArgs e)
        {


        }

        protected void btnReassign_Click(object sender, EventArgs e)
        {
            try
            {
                // Hide other panels if necessary
                PanelOne.Visible = false;
                PanelTwo.Visible = false;

                // Show PanelThree
                PanelThree.Visible = true;

                List<MTODEtails> mtolist = new List<MTODEtails>();

                // Assuming you have properly configured your Oracle connection string in the web.config file
                string connectionString = ConfigurationManager.AppSettings["OracleDB"];

                using (OracleConnection oconn = new OracleConnection(connectionString))
                {
                    // Open the connection if it's closed
                    if (oconn.State == ConnectionState.Closed)
                    {
                        oconn.Open();
                    }

                    // SQL query to fetch CSRCDE and CLAIMINF from the database
                    string sql = "SELECT CSRCDE, CLAIMINF, BRANCH FROM SHEDATA.shclaimhan m LEFT JOIN shedata.sheglxag s ON m.CSRCDE = s.csrcode ORDER BY CSRCDE ASC";

                    using (OracleCommand cmd = new OracleCommand(sql, oconn))
                    {
                        using (OracleDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                // Check if CLAIMINF is null, if so, skip adding this entry
                                if (!reader.IsDBNull(reader.GetOrdinal("CLAIMINF")))
                                {
                                    MTODEtails AgeInfo = new MTODEtails
                                    {
                                        CSRCDE1 = reader["CSRCDE"].ToString(),
                                        CLAIMINF1 = reader["CLAIMINF"].ToString(),
                                        BRANCH1 = reader["BRANCH"].ToString(),
                                    };

                                    mtolist.Add(AgeInfo);
                                }
                            }
                        }
                    }
                    // Bind data to GridView2
                    GridView2.DataSource = mtolist;
                    GridView2.DataBind();
                }
            }
            catch (Exception ex)
            {
                // Handle the exception appropriately.
                // For example, display an error message or log the error.
                // You can also rethrow the exception if needed.
                // throw ex;
            }
        }

        public class MTODEtails
        {
            public string CSRCDE1 { get; set; }
            public string CLAIMINF1 { get; set; }
            public string BRANCH1 { get; set; }
        }


        protected void RadioButton_CheckedChanged(object sender, EventArgs e)
        {
            foreach (GridViewRow row in GridView2.Rows)
            {
                RadioButton rb = (RadioButton)row.FindControl("RadioButton1");
                if (rb.Checked)
                {
                    rb.Checked = false;
                }
            }
            RadioButton selectedRadioButton = (RadioButton)sender;
            selectedRadioButton.Checked = true;
        }



        protected void reassignGrid_RowDataBound(object sender, EventArgs e)
        {

        }

        protected void reassignGrid_SelectedIndexChanged(object sender, EventArgs e)
        {

        }



        //protected void ReasignClick(object sender, EventArgs e)
        //{

        //    string refno = ClaimReferenceLabel.Text;




        //    //var json = n.DownloadString(host_ip + "/SHE_Tab_API/GIService.svc/GetPolicy?policyNo=" + policyNo);
        //    //polDetails = JsonConvert.DeserializeObject<BV_resp_SHEPolicyData>(json);
        //    //if (polDetails != null)
        //    //{
        //    //    //string data = polDetails.Data.Replace("\\", "");
        //    //    polDetailsList = JsonConvert.DeserializeObject<List<SHEPolicyData>>(polDetails.Data);


        //    //}
        //    //else
        //    //{
        //    //    //lblErrorMsg.InnerText = "No data found for this policy no";
        //    //}

        //}

    }
}



