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
using System.Web.UI;
using System.Net;
using System.Web.Script.Serialization;

namespace SHE
{
    public partial class Notifications : System.Web.UI.Page
    {
        private const string host_ip = "http://172.24.90.100:8084";

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
            string sql = @"SELECT m.PNAME, m.claimref, c.job_status, c.job_type, g.hospital_name FROM shedata.cor_job_status c INNER JOIN SHEDATA.SHHOSINF00 m ON c.claimref = m.claimref INNER JOIN GENERAL_CLAIM.CLAIM_HOSPITAL_DETAILS g ON m.hospital = g.hospital_id WHERE c.cordinator_userid = :userid ORDER BY c.adddate DESC, c.disdate DESC ";

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
                            Jobtype = reader["job_type"].ToString(),
                            AdmittedType = reader["job_status"].ToString(),
                            Hospital = reader["hospital_name"].ToString(),
                            ClaimRef1 = reader["claimref"].ToString(),

                        };
                        // Use switch statement to set AdmittedType based on job_type
                        string jobType = reader["job_type"].ToString();
                        switch (jobType)
                        {
                            case "A":
                                notification.Jobtype = "ADMITTED";
                                break;
                            case "D":
                                notification.Jobtype = "DISCHARGED";
                                break;
                            default:
                                notification.Jobtype = "UNKNOWN"; // Or handle other cases as needed
                                break;

                        }
                        string jobType1 = notification.Jobtype; // Assuming Jobtype1 is your variable
                        cmd.Parameters.Add("job_type", OracleType.VarChar).Value = jobType1;
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
            string type = admittedType.ToString().ToUpper();
            switch (type)
            {
                case "ACCEPTED":
                    return "accepted-color-class";
                case "COMPLETED":
                    return "completed-color-class";
                case "NOT_UPDATED":
                    return "not-updated-color-class";
                case "REASSIGNED":
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

                NotificationDTO notification = e.Row.DataItem as NotificationDTO;

                // Check if the notification is not null
                if (notification != null)
                {
                    // Set CSS class based on AdmittedType
                    e.Row.CssClass = GetAdmittedTypeCssClass(notification.AdmittedType);

                    // Find the Image control within the row
                    Image image = e.Row.FindControl("NotificationImage") as Image;
                    if (image != null)
                    {
                        // Set the image URL based on AdmittedType
                        switch (notification.AdmittedType.Trim().ToUpper())
                        {
                            case "ACCEPTED":
                                image.ImageUrl = "~/images/Yellowwhite.png";
                                break;
                            case "COMPLETED":
                                image.ImageUrl = "~/images/green.png";
                                break;
                            case "NOT_UPDATED":
                                image.ImageUrl = "~/images/blue.png";
                                break;
                            case "REASSIGNED":
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

                Label AdmitType = (Label)NotificationGrid.Rows[selectedIndex].FindControl("AdmitType");
                Label Jobtype = (Label)NotificationGrid.Rows[selectedIndex].FindControl("Jobtype");


                if (label1 != null && AdmitType != null)
                {
                    // Get the value of ClaimRef1 from the Label control
                    string claimRef1 = label1.Text;

                    string AdmitType1 = AdmitType.Text;

                    string Jobtype1 = Jobtype.Text;

                    // Set the visibility of panels
                    PanelOne.Visible = false;
                    PanelTwo.Visible = true;
                    PanelThree.Visible = false;

                    // Fetch data for PanelTwo
                    FetchPanelTwoData(claimRef1, AdmitType1, Jobtype1);
                }
            }
        }


        protected void FetchPanelTwoData(string claimRef1, string AdmitType1, string Jobtype1)
        {
            string userid = (string)Session["LoggedUser"];
            string sqlPanelTwo = @"SELECT m.claimref, m.pname, m.cname, m.cphone, g.hospital_name, m.roomno, m.adddate, m.pidno, m.disdate, m.epf, m.policy, m.remark1, c.job_status, c.JOB_TYPE from shedata.cor_job_status c INNER JOIN SHEDATA.SHHOSINF00 m  ON m.claimref = c.claimref INNER JOIN GENERAL_CLAIM.CLAIM_HOSPITAL_DETAILS g ON m.hospital = g.hospital_id WHERE m.claimref = :claimRef and c.CORDINATOR_USERID = :cordiId and c.job_status=:job_status and c.job_type = :job_type";

            using (OracleConnection oconn = new OracleConnection(ConfigurationManager.AppSettings["OracleDB"]))
            using (OracleCommand cmd = new OracleCommand(sqlPanelTwo, oconn))
            {
                String joType;
                if (Jobtype1 == "ADMITTED")
                {
                    joType = "A";
                }
                else if (Jobtype1 == "DISCHARGED")
                {
                    joType = "D";
                }
                else
                {
                    joType = "";
                }


                cmd.Parameters.Add("claimRef", OracleType.VarChar).Value = claimRef1;
                cmd.Parameters.Add("cordiId", OracleType.VarChar).Value = userid;
                cmd.Parameters.Add("job_status", OracleType.VarChar).Value = AdmitType1;
                cmd.Parameters.Add("job_type", OracleType.VarChar).Value = joType;

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
                        JobHospitalLabel.Text = reader["hospital_name"].ToString();
                        RoomNumberLabel.Text = reader["roomno"].ToString();
                        AddDateLabel.Text = reader["adddate"].ToString();
                        PatientIDNumberLabel.Text = reader["pidno"].ToString();
                        DischargeDateLabel.Text = reader["disdate"].ToString();
                        EPFLabel.Text = reader["epf"].ToString();
                        PolicyLabel.Text = reader["policy"].ToString();
                        RemarkLabel.Text = reader["remark1"].ToString();
                        JobStatusLabel.Text = reader["job_status"].ToString();
                        Job_Typelabel.Text = reader["JOB_TYPE"].ToString();

                        // You can continue setting other labels here

                        FetchClaimAction(claimRef1, joType, userid);

                        switch (JobStatusLabel.Text.Trim())
                        {
                            case "ACCEPTED":
                                btnAccept.Enabled = false;
                                btnReassign.Enabled = true;
                                btnreject.Enabled = true;
                                btnremovereject.Enabled = true;
                                btnClaimHistory.Enabled = true;
                                btnBack.Enabled = true;

                                // Enable btnClaimPayment if Job_Typelabel is "D"
                                btnClaimPayment.Enabled = (Job_Typelabel.Text.Trim() == "D");
                                break;
                            case "NOT_UPDATED":
                                btnAccept.Enabled = true;
                                btnReassign.Enabled = true;
                                btnreject.Enabled = true;
                                btnremovereject.Enabled = true;
                                btnClaimHistory.Enabled = true;
                                btnClaimPayment.Enabled = false;
                                btnBack.Enabled = true;
                                break;
                            case "COMPLETED":
                                btnAccept.Enabled = false;
                                btnReassign.Enabled = false;
                                btnClaimHistory.Enabled = true;
                                btnreject.Enabled = false;
                                btnremovereject.Enabled = false;
                                btnClaimPayment.Enabled = false;
                                btnBack.Enabled = true;
                                rePrint.Enabled = true;
                                payEdit.Enabled = true;
                                break;
                            case "REASSIGNED":
                                btnAccept.Enabled = false;
                                btnReassign.Enabled = false;
                                btnClaimHistory.Enabled = false;
                                btnreject.Enabled = false;
                                btnremovereject.Enabled = false;
                                btnClaimPayment.Enabled = false;
                                btnBack.Enabled = true;
                                break;
                            default:
                                // Handle other cases as needed
                                break;
                        }
                    }
                }
            }
        }

        private void FetchClaimAction(string claimRef1, string jobType, string userId)
        {
            string sqlClaimAction = "SELECT ACTION FROM shedata.reject_reason WHERE CLAIM_REF = :claimRef AND JOB_TYPE = :jobType AND UPDATE_USER = :updateUser AND (CLAIM_STATUS = 'RJ' OR CLAIM_STATUS IS NULL)";

            using (OracleConnection oconn = new OracleConnection(ConfigurationManager.AppSettings["OracleDB"]))
            using (OracleCommand cmd = new OracleCommand(sqlClaimAction, oconn))
            {
                cmd.Parameters.Add("claimRef", OracleType.VarChar).Value = claimRef1;
                cmd.Parameters.Add("jobType", OracleType.VarChar).Value = jobType;
                cmd.Parameters.Add("updateUser", OracleType.VarChar).Value = userId;

                oconn.Open();

                using (OracleDataReader reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        Claim_Action.Text = reader["ACTION"].ToString();
                    }
                    else
                    {
                        Claim_Action.Text = "No action found";
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

            EncryptDecrypt dc = new EncryptDecrypt();
            var policy = dc.Encrypt(PolicyLabel.Text);
            var epfno = dc.Encrypt(EPFLabel.Text);
            var clamRef = dc.Encrypt(ClaimReferenceLabel.Text);
            Response.Redirect("~/ClaimPayment/ClaimSearch.aspx?policy=" + policy + "&epf=" + epfno + "&claimRef=" + clamRef + "&fromNotifi=true");

        }



        protected void BackButton_Click(object sender, EventArgs e)
        {
            PanelOne.Visible = true;
            PanelTwo.Visible = false;
            PanelThree.Visible = false;

            Response.Redirect("~/Notifications.aspx");
        }


        //protected void BackButton_Click(object sender, EventArgs e)
        //{
        //    PanelOne.Visible = true;
        //    PanelTwo.Visible = false;
        //    PanelThree.Visible = false;


        //}
        protected void btnReassign_Click(object sender, EventArgs e)
        {
            try
            {
                // Hide other panels if necessary
                PanelOne.Visible = false;
                PanelTwo.Visible = false;

                // Show PanelThree
                PanelThree.Visible = true;

                List<MtoDetails> mtolist = new List<MtoDetails>();

                // Assuming you have properly configured your Oracle connection string in the web.config file
                string connectionString = ConfigurationManager.AppSettings["OracleDB"];

                using (OracleConnection oconn = new OracleConnection(connectionString))
                {
                    // Open the connection if it's closed
                    if (oconn.State == ConnectionState.Closed)
                    {
                        oconn.Open();
                    }

                    // SQL query to fetch CSRNAME and CSRBRNC from the database
                    string sql = @" SELECT s.CSRNAME, s.CSRBRNC,s.CSRUSRN,s.csrtpno, s.CSRGXNO FROM shedata.sheglxag s WHERE s.CSRSTAT = 'Y' AND s.CSRGTAB = 'Y' ORDER BY s.CSRNAME ASC";

                    using (OracleCommand cmd = new OracleCommand(sql, oconn))
                    {
                        using (OracleDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                MtoDetails details = new MtoDetails
                                {
                                    CLAIMINF1 = reader["CSRNAME"].ToString(),
                                    BRANCH1 = reader["CSRBRNC"].ToString(),
                                    CSRUSRN1 = reader["CSRUSRN"].ToString(),
                                    csrtpno1 = reader["csrtpno"].ToString(),
                                    CSRGXNO1 = reader["CSRGXNO"].ToString(),
                                };

                                mtolist.Add(details);
                            }
                        }
                    }
                }

                // Bind data to GridView2
                GridView2.DataSource = mtolist;
                GridView2.DataBind();
            }
            catch (Exception ex)
            {
                // Handle the exception appropriately.
                // For example, log the error.
            }
        }

        public class MtoDetails
        {
            public string CLAIMINF1 { get; set; }
            public string BRANCH1 { get; set; }
            public string CSRUSRN1 { get; set; }
            public string csrtpno1 { get; set; }
            public string CSRGXNO1 { get; set; }

        }

        //protected void RadioButton_CheckedChanged(object sender, EventArgs e)
        //{
        //    foreach (GridViewRow row in GridView2.Rows)
        //    {
        //        RadioButton rb = (RadioButton)row.FindControl("RadioButton1");
        //        if (rb.Checked)
        //        {
        //            rb.Checked = false;
        //        }
        //    }
        //    RadioButton selectedRadioButton = (RadioButton)sender;
        //    selectedRadioButton.Checked = true;
        //}

        protected void RadioButton_CheckedChanged(object sender, EventArgs e)
        {
            RadioButton selectedRadioButton = (RadioButton)sender;
            GridViewRow row = (GridViewRow)selectedRadioButton.NamingContainer;

            // Get data from the selected row
            string CSRUSRN = ((Label)row.FindControl("CSRUSRN")).Text;
            string CLAIMINF1 = ((Label)row.FindControl("agentname")).Text;
            string BRANCH1 = ((Label)row.FindControl("BRANCH_NAME")).Text;
            string csrtpno1 = ((Label)row.FindControl("CSRTNO")).Text;
            string CSRGXNO1 = ((Label)row.FindControl("CSRGXNO")).Text;

            // Store the data in session variables
            Session["CSRUSRN"] = CSRUSRN;
            Session["CLAIMINF1"] = CLAIMINF1;
            Session["BRANCH1"] = BRANCH1;
            Session["csrtpno1"] = csrtpno1;
            Session["CSRGXNO1"] = CSRGXNO1;

            // Uncheck all other radio buttons in the GridView
            foreach (GridViewRow gridViewRow in GridView2.Rows)
            {
                RadioButton rb = (RadioButton)gridViewRow.FindControl("RadioButton1");
                if (rb != selectedRadioButton && rb.Checked)
                {
                    rb.Checked = false;
                }
            }

            // Scroll to the selected radio button
            ScriptManager.RegisterStartupScript(this, GetType(), "scrollToSelectedRadioButton", "scrollToElement('" + selectedRadioButton.ClientID + "');", true);

        }



        protected void Accepted_Click(object sender, EventArgs e)
        {
            string userid = (string)Session["LoggedUser"];

            int selectedIndex = NotificationGrid.SelectedIndex;

            if (selectedIndex >= 0 && selectedIndex < NotificationGrid.Rows.Count)
            {
                Label label1 = (Label)NotificationGrid.Rows[selectedIndex].FindControl("Label1");

                Label AdmitType = (Label)NotificationGrid.Rows[selectedIndex].FindControl("AdmitType");

                if (label1 != null)
                {
                    string claimRef1 = label1.Text;

                    string AdmitType1 = AdmitType.Text;

                    String joType = Job_Typelabel.Text;

                    // Update the job status in the database from "NOT_UPDATED" to "ACCEPTED"
                    string updateSql = "UPDATE shedata.cor_job_status SET job_status = 'ACCEPTED', STATUS_CHANGE_DATE = SYSDATE WHERE claimref = :claimRef1 and CORDINATOR_USERID = :cordiId and job_status=:job_status and job_type = :job_type";

                    using (OracleConnection oconn = new OracleConnection(ConfigurationManager.AppSettings["OracleDB"]))
                    using (OracleCommand cmd = new OracleCommand(updateSql, oconn))
                    {


                        cmd.Parameters.Add("claimRef1", OracleType.VarChar).Value = claimRef1;
                        cmd.Parameters.Add("cordiId", OracleType.VarChar).Value = userid;
                        cmd.Parameters.Add("job_status", OracleType.VarChar).Value = AdmitType1;
                        cmd.Parameters.Add("job_type", OracleType.VarChar).Value = joType;




                        oconn.Open();
                        int rowsAffected = cmd.ExecuteNonQuery();
                        oconn.Close();

                        if (rowsAffected > 0)
                        {

                            // Update the job status label on the page
                            JobStatusLabel.Text = "ACCEPTED";
                            ClientScript.RegisterStartupScript(this.GetType(), "displaySuccessMessage", "displayPopup('ACCEPTED');", true);
                            UpdateButtonStates();

                        }
                        else
                        {
                            // Handle error if the update operation fails
                            // Optionally, display a message or log the error
                        }
                    }
                }
            }
        }



        protected void btnSubmitButton_Click(object sender, EventArgs e)
        {
            try
            {
                string refn = ClaimReferenceLabel.Text;
                string adddate = AddDateLabel.Text;
                string disdate = DischargeDateLabel.Text;
                string JobType = Job_Typelabel.Text;

                // Get the CSRUSRN value from session or another source
                string CSRUSRN = Session["CSRUSRN"] as string; // Example: Retrieving from session
                string csrtpno1 = Session["csrtpno1"] as string;
                string CSRGXNO1 = Session["CSRGXNO1"] as string;

                string userid = (string)Session["LoggedUser"];

                int selectedIndex = NotificationGrid.SelectedIndex;

                if (selectedIndex >= 0 && selectedIndex < NotificationGrid.Rows.Count)
                {
                    Label label1 = (Label)NotificationGrid.Rows[selectedIndex].FindControl("Label1");

                    Label AdmitType = (Label)NotificationGrid.Rows[selectedIndex].FindControl("AdmitType");

                    if (label1 != null)
                    {
                        string claimRef1 = label1.Text;
                        string AdmitType1 = AdmitType.Text;
                        String joType = Job_Typelabel.Text;

                        // Update the job status in the database to "REASSIGNED"
                        string updateSql = "UPDATE shedata.cor_job_status SET job_status = 'REASSIGNED', STATUS_CHANGE_DATE = SYSDATE WHERE claimref = :claimRef1 and CORDINATOR_USERID = :cordId and job_status=:job_status and job_type = :job_type ";

                        using (OracleConnection oconn = new OracleConnection(ConfigurationManager.AppSettings["OracleDB"]))
                        using (OracleCommand cmd = new OracleCommand(updateSql, oconn))
                        {


                            cmd.Parameters.Add("claimRef1", OracleType.VarChar).Value = claimRef1;
                            cmd.Parameters.Add("cordId", OracleType.VarChar).Value = userid;
                            cmd.Parameters.Add("job_status", OracleType.VarChar).Value = AdmitType1;
                            cmd.Parameters.Add("job_type", OracleType.VarChar).Value = joType;


                            oconn.Open();
                            int rowsAffected = cmd.ExecuteNonQuery();

                            if (rowsAffected > 0)
                            {
                                string insertSql = "INSERT INTO shedata.cor_job_status(CLAIMREF, ADDDATE ,CORDINATOR_USERID, JOB_TYPE, JOB_STATUS, DISDATE,STATUS_CHANGE_DATE) " +
                                                    "VALUES (:claimref, :adddate, :CSRUSRN, :JOB_TYPE, 'NOT_UPDATED', :disdate, SYSDATE)";

                                using (OracleCommand insertCmd = new OracleCommand(insertSql, oconn))
                                {
                                    insertCmd.Parameters.Add("claimref", OracleType.VarChar).Value = refn;
                                    insertCmd.Parameters.Add("adddate", OracleType.VarChar).Value = adddate;
                                    insertCmd.Parameters.Add("CSRUSRN", OracleType.VarChar).Value = CSRUSRN;
                                    insertCmd.Parameters.Add("JOB_TYPE", OracleType.VarChar).Value = JobType;
                                    insertCmd.Parameters.Add("disdate", OracleType.VarChar).Value = disdate;

                                    // Execute the insert command
                                    insertCmd.ExecuteNonQuery();
                                }
                                // Send message to MTO

                                // Update the job status label on the page
                                JobStatusLabel.Text = "REASSIGNED";
                                ClientScript.RegisterStartupScript(this.GetType(), "displaySuccessMessage", "displayPopup('REASSIGNED');", true);
                                UpdateButtonStates();

                                SendMessageToMTO(csrtpno1, CSRGXNO1);


                                SubmitButton.Enabled = false;
                            }
                            else
                            {
                                // Handle case where no rows were updated
                                // Display a message or take appropriate action
                            }

                        }
                    }
                }
            }
            catch (Exception ex)
            {
                // Handle the exception appropriately.
                // For example, log the error.
                // Response.Write("An error occurred while updating job status: " + ex.Message);
            }
        }

        protected void UpdateButtonStates()
        {
            switch (JobStatusLabel.Text.Trim())
            {
                case "ACCEPTED":
                    btnAccept.Enabled = false;
                    btnReassign.Enabled = true;
                    btnreject.Enabled = true;
                    btnremovereject.Enabled = true;
                    btnClaimHistory.Enabled = true;
                    btnBack.Enabled = true;

                    // Enable btnClaimPayment if Job_Typelabel is "D"
                    btnClaimPayment.Enabled = (Job_Typelabel.Text.Trim() == "D");
                    break;
                case "NOT_UPDATED":
                    btnAccept.Enabled = true;
                    btnReassign.Enabled = true;
                    btnreject.Enabled = true;
                    btnremovereject.Enabled = true;
                    btnClaimHistory.Enabled = true;
                    btnClaimPayment.Enabled = false;
                    btnBack.Enabled = true;
                    break;
                case "COMPLETED":
                    btnAccept.Enabled = false;
                    btnReassign.Enabled = false;
                    btnClaimHistory.Enabled = true;
                    btnreject.Enabled = false;
                    btnremovereject.Enabled = false;
                    btnClaimPayment.Enabled = false;
                    btnBack.Enabled = true;
                    rePrint.Enabled = true;
                    payEdit.Enabled = true;
                    break;

                case "REASSIGNED":
                    btnAccept.Enabled = false;
                    btnReassign.Enabled = false;
                    btnClaimHistory.Enabled = false;
                    btnreject.Enabled = false;
                    btnremovereject.Enabled = false;
                    btnClaimPayment.Enabled = false;
                    btnBack.Enabled = true;
                    break;
                default:
                    // Handle other cases as needed
                    break;
            }
        }

        // Method to send message to MTO
        private void SendMessageToMTO(string csrtpno1, string CSRGXNO1)
        {
            try
            {
                string refn1 = ClaimReferenceLabel.Text;
                string hospital = JobHospitalLabel.Text;
                string roomNo = RoomNumberLabel.Text;

                // Get the next RECORD_SEQUENCE
                int nextRecordSequence = GetNextRecordSequence();

                // Insert data into the specified table
                string insertSql = "INSERT INTO SMS.SMS_GATEWAY(RECORD_SEQUENCE, APPLICATION_ID, JOB_CATEGORY, SMS_TYPE, MOBILE_NUMBER, TEXT_MESSAGE, SHORT_CODE, RECORD_STATUS, JOB_OTHER_INFO) " +
                                    "VALUES (:recordSeq, :appId, :jobCategory, :smsType, :mobileNum, :textMsg, :shortCode, :recordStatus, :jobOtherInfo)";

                using (OracleConnection oconn = new OracleConnection(ConfigurationManager.AppSettings["OracleDB"]))
                {
                    oconn.Open();

                    // Insert data with the first mobile number
                    using (OracleCommand insertCmd = new OracleCommand(insertSql, oconn))
                    {
                        // Set parameters for insertion                    
                        insertCmd.Parameters.Add("recordSeq", OracleType.Number).Value = nextRecordSequence;
                        insertCmd.Parameters.Add("appId", OracleType.VarChar).Value = "SHE_INT_HEALTH_CSR"; // Hardcoded value
                        insertCmd.Parameters.Add("jobCategory", OracleType.VarChar).Value = "CAT231"; // Hardcoded value
                        insertCmd.Parameters.Add("smsType", OracleType.VarChar).Value = "I"; // Hardcoded value
                        insertCmd.Parameters.Add("mobileNum", OracleType.VarChar).Value = csrtpno1; // Using the first mobile number passed as parameter
                        insertCmd.Parameters.Add("textMsg", OracleType.VarChar).Value = "A new task has been assigned to you, please check your SHE system. Reference No: " + refn1 + " Hospital: " + hospital + " Room No: " + roomNo;
                        insertCmd.Parameters.Add("shortCode", OracleType.VarChar).Value = "SLIC"; // Provide appropriate values
                        insertCmd.Parameters.Add("recordStatus", OracleType.VarChar).Value = "N"; // Provide appropriate values
                        insertCmd.Parameters.Add("jobOtherInfo", OracleType.VarChar).Value = refn1; // Provide appropriate values

                        // Execute the insert command
                        insertCmd.ExecuteNonQuery();
                    }

                    // Insert data with the second mobile number
                    using (OracleCommand insertCmd = new OracleCommand(insertSql, oconn))
                    {
                        // Set parameters for insertion                    
                        insertCmd.Parameters.Add("recordSeq", OracleType.Number).Value = nextRecordSequence; // Using the same record sequence
                        insertCmd.Parameters.Add("appId", OracleType.VarChar).Value = "SHE_INT_HEALTH_CSR"; // Hardcoded value
                        insertCmd.Parameters.Add("jobCategory", OracleType.VarChar).Value = "CAT231"; // Hardcoded value
                        insertCmd.Parameters.Add("smsType", OracleType.VarChar).Value = "I"; // Hardcoded value
                        insertCmd.Parameters.Add("mobileNum", OracleType.VarChar).Value = CSRGXNO1; // Using the second mobile number passed as parameter
                        insertCmd.Parameters.Add("textMsg", OracleType.VarChar).Value = "A new task has been assigned to you, please check your SHE system. Reference No: " + refn1 + " Hospital: " + hospital + " Room No: " + roomNo;
                        insertCmd.Parameters.Add("shortCode", OracleType.VarChar).Value = "SLIC"; // Provide appropriate values
                        insertCmd.Parameters.Add("recordStatus", OracleType.VarChar).Value = "N"; // Provide appropriate values
                        insertCmd.Parameters.Add("jobOtherInfo", OracleType.VarChar).Value = refn1; // Provide appropriate values

                        // Execute the insert command
                        insertCmd.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                // Handle the exception appropriately.
                // For example, log the error.
            }
        }
        // Method to get the next RECORD_SEQUENCE
        private int GetNextRecordSequence()
        {
            int nextRecordSequence = 1; // Default value if no records exist

            try
            {
                string selectMaxSql = "SELECT MAX(RECORD_SEQUENCE) FROM SMS.SMS_GATEWAY";

                using (OracleConnection oconn = new OracleConnection(ConfigurationManager.AppSettings["OracleDB"]))
                using (OracleCommand cmd = new OracleCommand(selectMaxSql, oconn))
                {
                    oconn.Open();
                    object result = cmd.ExecuteScalar();
                    if (result != DBNull.Value && result != null)
                    {
                        nextRecordSequence = Convert.ToInt32(result) + 1;
                    }
                }
            }
            catch (Exception ex)
            {
                // Handle the exception appropriately.
                // For example, log the error.
                Console.WriteLine("An error occurred while getting the next record sequence: " + ex.Message);
            }

            return nextRecordSequence;
        }


        protected void exitbutton_Click(object sender, EventArgs e)
        {
            {
                Session.Clear(); // Clear all session variables
                Session.Abandon(); // End the current session
                Response.Redirect("~/login.aspx");
            }


        }
        protected void displayPopup(object sender, EventArgs e)
        {

        }
        protected void reassignGrid_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                // Check if the current row corresponds to the logged-in user
                string loggedInUserID = (string)Session["LoggedUser"];
                Label CSRUSRN = (Label)e.Row.FindControl("CSRUSRN");
                if (CSRUSRN != null && CSRUSRN.Text == loggedInUserID)
                {
                    e.Row.Visible = false; // Hide the row if it corresponds to the logged-in user
                }
            }
        }

        protected void reassignGrid_SelectedIndexChanged(object sender, EventArgs e)
        {

        }


        protected void btnReject_Click(object sender, EventArgs e)
        {

            PanelOne.Visible = false;
            PanelTwo.Visible = false;

            // Show PanelThree
            PanelThree.Visible = false;
            Panelfour.Visible = true;
        }
        //       protected void reject_submit_Click(object sender, EventArgs e)
        //        {
        //        try
        //        {
        //        // Get the values from the TextBox and session
        //        string rejectReason = rejectReasonTextBox.Text;
        //        string refn1 = ClaimReferenceLabel.Text;
        //        string jobType = Job_Typelabel.Text;
        //        string userId = (string)Session["LoggedUser"];

        //        // Check if the reject reason is empty
        //        if (string.IsNullOrWhiteSpace(rejectReason))
        //        {
        //            // Display error message and return
        //            ClientScript.RegisterStartupScript(this.GetType(), "PopupScript", "displayPopup1('REJECTION NOT INSERTED: Reject Reason is empty', false);", true);
        //            return;
        //        }


        //        // Check if data already exists for the given claim reference, job type, and user
        //        string connectionString = ConfigurationManager.AppSettings["OracleDB"];
        //        using (OracleConnection connection = new OracleConnection(connectionString))
        //        {
        //            connection.Open();
        //            string selectQuery = "SELECT COUNT(*) FROM shedata.reject_reason WHERE CLAIM_REF = :claimRef AND JOB_TYPE = :jobType AND UPDATE_USER = :updateUser AND CLAIM_STATUS = 'RJ' AND ACTION = 'Reject claim'";
        //            using (OracleCommand selectCommand = new OracleCommand(selectQuery, connection))
        //            {
        //                selectCommand.Parameters.Add(new OracleParameter("claimRef", OracleType.VarChar)).Value = refn1;
        //                selectCommand.Parameters.Add(new OracleParameter("jobType", OracleType.VarChar)).Value = jobType;
        //                selectCommand.Parameters.Add(new OracleParameter("updateUser", OracleType.VarChar)).Value = userId;
        //                int count = Convert.ToInt32(selectCommand.ExecuteScalar());

        //                if (count > 0)
        //                {
        //                    // Data exists, perform an update
        //                    string updateQuery = "UPDATE shedata.reject_reason SET REJECT_REASON = :rejectReason, UPDATE_DATE = SYSDATE WHERE CLAIM_REF = :claimRef AND JOB_TYPE = :jobType AND UPDATE_USER = :updateUser AND CLAIM_STATUS = 'RJ' AND ACTION = 'Reject claim'";
        //                    using (OracleCommand updateCommand = new OracleCommand(updateQuery, connection))
        //                    {
        //                        updateCommand.Parameters.Add(new OracleParameter("rejectReason", OracleType.VarChar)).Value = rejectReason;
        //                        updateCommand.Parameters.Add(new OracleParameter("claimRef", OracleType.VarChar)).Value = refn1;
        //                        updateCommand.Parameters.Add(new OracleParameter("jobType", OracleType.VarChar)).Value = jobType;
        //                        updateCommand.Parameters.Add(new OracleParameter("updateUser", OracleType.VarChar)).Value = userId;
        //                        int rowsAffected = updateCommand.ExecuteNonQuery();

        //                        if (rowsAffected > 0)
        //                        {
        //                            // Data updated successfully, display success message
        //                            ClientScript.RegisterStartupScript(this.GetType(), "PopupScript", "displayPopup1('REJECTION UPDATED', true);", true);
        //                            rejectReasonTextBox.Text = "";
        //                        }
        //                        else
        //                        {
        //                            // No data updated, display failure message
        //                            ClientScript.RegisterStartupScript(this.GetType(), "PopupScript", "displayPopup1('REJECTION NOT UPDATED', false);", true);
        //                        }
        //                    }
        //                }
        //                else
        //                {
        //                    // Data doesn't exist, perform an insert
        //                    string insertQuery = "INSERT INTO shedata.reject_reason (CLAIM_REF, JOB_TYPE, CLAIM_STATUS, REJECT_REASON, UPDATE_USER, UPDATE_DATE, ACTION, PAY_UP_REA, ACTIVE_STATUS) " +
        //                                         "VALUES (:claimRef, :jobType, 'RJ', :rejectReason, :updateUser, SYSDATE, 'Reject claim', 'no', 0)";
        //                    using (OracleCommand insertCommand = new OracleCommand(insertQuery, connection))
        //                    {
        //                        insertCommand.Parameters.Add(new OracleParameter("claimRef", OracleType.VarChar)).Value = refn1;
        //                        insertCommand.Parameters.Add(new OracleParameter("jobType", OracleType.VarChar)).Value = jobType;
        //                        insertCommand.Parameters.Add(new OracleParameter("rejectReason", OracleType.VarChar)).Value = rejectReason;
        //                        insertCommand.Parameters.Add(new OracleParameter("updateUser", OracleType.VarChar)).Value = userId;
        //                        int rowsAffected = insertCommand.ExecuteNonQuery();

        //                        if (rowsAffected > 0)
        //                        {
        //                            // Data inserted successfully, display success message
        //                            ClientScript.RegisterStartupScript(this.GetType(), "PopupScript", "displayPopup1('REJECTION INSERTED', true);", true);
        //                            rejectReasonTextBox.Text = "";
        //                        }
        //                        else
        //                        {
        //                            // No data inserted, display failure message
        //                            ClientScript.RegisterStartupScript(this.GetType(), "PopupScript", "displayPopup1('REJECTION NOT INSERTED', false);", true);
        //                        }
        //                    }
        //                }
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        // Handle exception (e.g., log the error, show a message to the user, etc.)
        //        // For demonstration, we can just rethrow it
        //        throw new Exception("An error occurred while processing the reject reason: " + ex.Message);
        //    }
        //}

        protected void reject_submit_Click(object sender, EventArgs e)
        {
            try
            {
                // Get the values from the TextBox and session
                string rejectReason = rejectReasonTextBox.Text;
                string refn1 = ClaimReferenceLabel.Text;
                string jobType = Job_Typelabel.Text;
                string userId = (string)Session["LoggedUser"];


                // Check if the reject reason is empty
                if (string.IsNullOrWhiteSpace(rejectReason))
                {
                    // Display error message and return
                    ClientScript.RegisterStartupScript(this.GetType(), "PopupScript", "displayPopup1('REJECTION NOT INSERTED: Reject Reason is empty', false);", true);
                    return;
                }

                bool apiCallSuccessful = false;

                // Call the API to change claim status
                using (System.Net.WebClient webClient = new WebClient())
                {
                    try
                    {
                        string url = $"{host_ip}/SHE_Tab_API/Service.svc/ClaimStatusChange?refNo=" + refn1 + "&claimStatus=REJECT";
                        string json = webClient.DownloadString(url);

                        JavaScriptSerializer serializer = new JavaScriptSerializer();
                        var result = serializer.Deserialize<dynamic>(json);

                        if (result["ID"] == 200)
                        {
                            apiCallSuccessful = true;
                        }
                        else
                        {
                            ClientScript.RegisterStartupScript(this.GetType(), "PopupScript", "displayPopup1('API call failed.', false);", true);
                            return;
                        }
                    }
                    catch (Exception ex)
                    {
                        ClientScript.RegisterStartupScript(this.GetType(), "PopupScript", $"displayPopup1('API call error: {ex.Message}', false);", true);
                        return;
                    }
                }

                if (apiCallSuccessful)
                {
                    // Check if data already exists for the given claim reference, job type, and user
                    string connectionString = ConfigurationManager.AppSettings["OracleDB"];
                    using (OracleConnection connection = new OracleConnection(connectionString))
                    {
                        connection.Open();
                        string selectQuery = "SELECT COUNT(*) FROM shedata.reject_reason WHERE CLAIM_REF = :claimRef AND JOB_TYPE = :jobType AND UPDATE_USER = :updateUser AND CLAIM_STATUS = 'RJ' AND ACTION = 'Reject claim'";
                        using (OracleCommand selectCommand = new OracleCommand(selectQuery, connection))
                        {
                            selectCommand.Parameters.Add(new OracleParameter("claimRef", OracleType.VarChar)).Value = refn1;
                            selectCommand.Parameters.Add(new OracleParameter("jobType", OracleType.VarChar)).Value = jobType;
                            selectCommand.Parameters.Add(new OracleParameter("updateUser", OracleType.VarChar)).Value = userId;
                            int count = Convert.ToInt32(selectCommand.ExecuteScalar());

                            if (count > 0)
                            {
                                // Data exists, perform an update
                                string updateQuery = "UPDATE shedata.reject_reason SET REJECT_REASON = :rejectReason, UPDATE_DATE = SYSDATE WHERE CLAIM_REF = :claimRef AND JOB_TYPE = :jobType AND UPDATE_USER = :updateUser AND (CLAIM_STATUS = 'RJ' OR CLAIM_STATUS IS NULL) AND (ACTION = 'Reject claim' OR ACTION = 'Remove Claim Rejection'";
                                using (OracleCommand updateCommand = new OracleCommand(updateQuery, connection))
                                {
                                    updateCommand.Parameters.Add(new OracleParameter("rejectReason", OracleType.VarChar)).Value = rejectReason;
                                    updateCommand.Parameters.Add(new OracleParameter("claimRef", OracleType.VarChar)).Value = refn1;
                                    updateCommand.Parameters.Add(new OracleParameter("jobType", OracleType.VarChar)).Value = jobType;
                                    updateCommand.Parameters.Add(new OracleParameter("updateUser", OracleType.VarChar)).Value = userId;
                                    int rowsAffected = updateCommand.ExecuteNonQuery();

                                    if (rowsAffected > 0)
                                    {
                                        // Data updated successfully, display success message
                                        ClientScript.RegisterStartupScript(this.GetType(), "PopupScript", "displayPopup1('REJECTION UPDATED', true);", true);
                                        rejectReasonTextBox.Text = "";
                                    }
                                    else
                                    {
                                        // No data updated, display failure message
                                        ClientScript.RegisterStartupScript(this.GetType(), "PopupScript", "displayPopup1('REJECTION NOT UPDATED', false);", true);
                                    }
                                }
                            }
                            else
                            {
                                // Data doesn't exist, perform an insert
                                string insertQuery = "INSERT INTO shedata.reject_reason (CLAIM_REF, JOB_TYPE, CLAIM_STATUS, REJECT_REASON, UPDATE_USER, UPDATE_DATE, ACTION, PAY_UP_REA, ACTIVE_STATUS) " +
                                                     "VALUES (:claimRef, :jobType, 'RJ', :rejectReason, :updateUser, SYSDATE, 'Reject claim', 'no', 0)";
                                using (OracleCommand insertCommand = new OracleCommand(insertQuery, connection))
                                {
                                    insertCommand.Parameters.Add(new OracleParameter("claimRef", OracleType.VarChar)).Value = refn1;
                                    insertCommand.Parameters.Add(new OracleParameter("jobType", OracleType.VarChar)).Value = jobType;
                                    insertCommand.Parameters.Add(new OracleParameter("rejectReason", OracleType.VarChar)).Value = rejectReason;
                                    insertCommand.Parameters.Add(new OracleParameter("updateUser", OracleType.VarChar)).Value = userId;
                                    int rowsAffected = insertCommand.ExecuteNonQuery();

                                    if (rowsAffected > 0)
                                    {
                                        // Data inserted successfully, display success message
                                        ClientScript.RegisterStartupScript(this.GetType(), "PopupScript", "displayPopup1('REJECTION INSERTED', true);", true);
                                        rejectReasonTextBox.Text = "";
                                    }
                                    else
                                    {
                                        // No data inserted, display failure message
                                        ClientScript.RegisterStartupScript(this.GetType(), "PopupScript", "displayPopup1('REJECTION NOT INSERTED', false);", true);
                                    }
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                // Handle exception (e.g., log the error, show a message to the user, etc.)
                // For demonstration, we can just rethrow it
                throw new Exception("An error occurred while processing the reject reason: " + ex.Message);
            }
        }



        protected void rejectBack_Click(object sender, EventArgs e)
        {
            PanelTwo.Visible = true;
            Panelfour.Visible = false;
            PanelFive.Visible = false;

            Response.Redirect("~/Notifications.aspx");
        }


        protected void Remove_Rejection_click(object sender, EventArgs e)
        {

            PanelOne.Visible = false;
            PanelTwo.Visible = false;

            // Show PanelThree
            PanelThree.Visible = false;
            Panelfour.Visible = false;
            PanelFive.Visible = true;

        }
        protected void reject_removesubmit_Click(object sender, EventArgs e)
        {
            string refn1 = ClaimReferenceLabel.Text;
            string jobType = Job_Typelabel.Text;
            string removerejectReason = rejectionRemove.Text;


            // Check if removal reason is provided
            if (string.IsNullOrWhiteSpace(removerejectReason))
            {
                // Display error message for empty reason
                ClientScript.RegisterStartupScript(this.GetType(), "PopupScript", "displayPopup1('Please provide a removal reason.', false);", true);
                return;
            }

            bool apiCallSuccessful3 = false;

            // Call the API to change claim status
            using (WebClient webClient = new WebClient())
            {
                try
                {
                    string url = $"{host_ip}/SHE_Tab_API/Service.svc/ClaimStatusChange?refNo={refn1}&claimStatus=PENDING";
                    string json = webClient.DownloadString(url);

                    JavaScriptSerializer serializer = new JavaScriptSerializer();
                    var result = serializer.Deserialize<dynamic>(json);

                    if (result["ID"] == 200)
                    {
                        apiCallSuccessful3 = true;
                    }
                    else
                    {
                        ClientScript.RegisterStartupScript(this.GetType(), "PopupScript", "displayPopup1('API call failed.', false);", true);
                        return;
                    }
                }
                catch (Exception ex)
                {
                    ClientScript.RegisterStartupScript(this.GetType(), "PopupScript", $"displayPopup1('API call error: {ex.Message}', false);", true);
                    return;
                }
            }

            if (apiCallSuccessful3)
            {
                // The record exists, proceed with the update
                string updateSql = "UPDATE shedata.reject_reason SET ACTIVE_STATUS = 1, REMOVE_REJECTION = :removerejectReason, REJECTION_REMOVE_DATE = SYSDATE, CLAIM_STATUS = NULL, ACTION = 'Remove Claim Rejection' WHERE ACTIVE_STATUS = 0 AND CLAIM_REF = :refn1 AND job_type = :jobType";

                using (OracleConnection oconn = new OracleConnection(ConfigurationManager.AppSettings["OracleDB"]))
                using (OracleCommand cmd = new OracleCommand(updateSql, oconn))
                {
                    cmd.Parameters.Add(new OracleParameter(":removerejectReason", OracleType.VarChar)).Value = removerejectReason;
                    cmd.Parameters.Add(new OracleParameter(":refn1", OracleType.VarChar)).Value = refn1;
                    cmd.Parameters.Add(new OracleParameter(":jobType", OracleType.VarChar)).Value = jobType;

                    try
                    {
                        oconn.Open();
                        int rowsAffected = cmd.ExecuteNonQuery();
                        if (rowsAffected > 0)
                        {
                            // Update successful, display success message
                            ClientScript.RegisterStartupScript(this.GetType(), "PopupScript", "displayPopup1('REJECTION REMOVED', true);", true);
                            rejectionRemove.Text = "";
                            remove_reject.Enabled = true;
                        }
                        else
                        {
                            // No rows were affected, display error message
                            ClientScript.RegisterStartupScript(this.GetType(), "PopupScript", "displayPopup1('No rejection found to remove.', false);", true);
                            rejectionRemove.Text = "";
                        }
                    }
                    catch (Exception ex)
                    {
                        // Handle the exception
                        ClientScript.RegisterStartupScript(this.GetType(), "PopupScript", $"displayPopup1('Database error: {ex.Message}', false);", true);
                    }
                }
            }
        }

        protected void Reprint_Click(object sender, EventArgs e)
        {

            EncryptDecrypt dc = new EncryptDecrypt();
            var policy = dc.Encrypt(PolicyLabel.Text);
            var epfno = dc.Encrypt(EPFLabel.Text);
            var clamRef = dc.Encrypt(ClaimReferenceLabel.Text);
            Response.Redirect("~/ClaimPayment/ClaimStatementReprint.aspx?policy=" + policy + "&epf=" + epfno + "&claimRef=" + clamRef + "&fromNotifi=true");

        }
        protected void Edit_Pay_Click(object sender, EventArgs e)
        {

            EncryptDecrypt dc = new EncryptDecrypt();
            var policy = dc.Encrypt(PolicyLabel.Text);
            var epfno = dc.Encrypt(EPFLabel.Text);
            var clamRef = dc.Encrypt(ClaimReferenceLabel.Text);
            Response.Redirect("~/ClaimPayment/ClaimPaymentDetail.aspx?POLICYNO=" + policy + "&EPF=" + epfno + "&CLAIMREF=" + clamRef + "&fromNotifiEdit=true");
        }

    }
}



