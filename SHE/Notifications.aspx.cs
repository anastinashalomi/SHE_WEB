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

                        switch (JobStatusLabel.Text.Trim())
                        {
                            case "ACCEPTED":
                                btnAccept.Enabled = false;
                                btnReassign.Enabled = true;
                                btnClaimHistory.Enabled = true;
                                btnBack.Enabled = true;

                                // Enable btnClaimPayment if Job_Typelabel is "D"
                                btnClaimPayment.Enabled = (Job_Typelabel.Text.Trim() == "D");
                                break;
                            case "NOT_UPDATED":
                                btnAccept.Enabled = true;
                                btnReassign.Enabled = true;
                                btnClaimHistory.Enabled = true;
                                btnClaimPayment.Enabled = false;
                                btnBack.Enabled = true;
                                break;
                            case "COMPLETED":
                                btnAccept.Enabled = false;
                                btnReassign.Enabled = false;
                                btnClaimHistory.Enabled = true;
                                btnClaimPayment.Enabled = false;
                                btnBack.Enabled = true;
                                break;
                            case "REASSIGNED":
                                btnAccept.Enabled = false;
                                btnReassign.Enabled = false;
                                btnClaimHistory.Enabled = false;
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
                    btnClaimHistory.Enabled = true;
                    btnBack.Enabled = true;

                    // Enable btnClaimPayment if Job_Typelabel is "D"
                    btnClaimPayment.Enabled = (Job_Typelabel.Text.Trim() == "D");
                    break;
                case "NOT_UPDATED":
                    btnAccept.Enabled = true;
                    btnReassign.Enabled = true;
                    btnClaimHistory.Enabled = false;
                    btnClaimPayment.Enabled = false;
                    btnBack.Enabled = true;
                    break;
                case "COMPLETED":
                    btnAccept.Enabled = false;
                    btnReassign.Enabled = false;
                    btnClaimHistory.Enabled = true;
                    btnClaimPayment.Enabled = true;
                    btnBack.Enabled = true;
                    break;
                case "REASSIGNED":
                    btnAccept.Enabled = false;
                    btnReassign.Enabled = false;
                    btnClaimHistory.Enabled = false;
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





    }
}



