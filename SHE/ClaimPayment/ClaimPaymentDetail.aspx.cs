using iTextSharp.text;
using iTextSharp.text.pdf;
using Newtonsoft.Json;
using SHE.Code;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.Odbc;
using System.Data.OracleClient;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Script.Serialization;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SHE.ClaimPayment
{
    public partial class ClaimPaymentDetail : System.Web.UI.Page
    {
        
        private const string host_ip = "http://172.24.90.100:8084";

        OdbcConnection db2conn = new OdbcConnection(ConfigurationManager.AppSettings["DB2"]);
        OracleConnection oconn = new OracleConnection(ConfigurationManager.AppSettings["OracleDB"]);
        OracleConnection oconnLife = new OracleConnection(ConfigurationManager.AppSettings["OraLifeDB"]);

        //When go live
        //string domainAndPort = "www.srilankainsurance.lk/SHE2_Intimation";

        //Test Environment 172.24.90.100
        string domainAndPort = "172.24.90.100:8084/Slicgeneral/SHE2_Intimation";

        EncryptDecrypt dc = new EncryptDecrypt();
        string policy, epfno, claimRefNo, userName, eventLimit, fromNotifiEdit;

        private List<ClaimData> claimDataList = new List<ClaimData>();

        private ClaimData claimDataset;

        protected void Page_Load(object sender, EventArgs e)
        {
            userName = Session["LoggedUser"].ToString();
            policy = Request.QueryString["POLICYNO"];
            epfno = Request.QueryString["EPF"];
            claimRefNo = Request.QueryString["CLAIMREF"];
            fromNotifiEdit = Request.QueryString["fromNotifiEdit"];


            policy = dc.Decrypt(policy);
            epfno = dc.Decrypt(epfno);
            claimRefNo = dc.Decrypt(claimRefNo);

            bool hasRows = false;

            if (!IsPostBack)
            {
                Reg_Policy_Mast polObj = new Reg_Policy_Mast();
                string SchemaName = polObj.getSchema(policy, epfno);

                eventLimit = polObj.GetEventLimit(policy, SchemaName);
                hiddenEventLimit.Value = decimal.Parse(eventLimit).ToString("F2");
                Session["eventLimit"] = eventLimit;

                if(fromNotifiEdit == "true")
                {
                   
                    default_hed.Visible = false;
                    edit_hed.Visible = true;
                    editReason.Visible = true;
                    Button2.Visible = false;
                    Button6.Visible = true;
                    ediReatxt.Visible = true;

                }
                else
                {
                    
                }

                if (oconn.State != ConnectionState.Open)
                {
                    oconn.Open();
                }

                OracleCommand cmd = oconn.CreateCommand();

                try
                {
                   
                        panel1.Visible = true; 

                        using (cmd)
                        {

                        string exe_Select_date;

                        if (fromNotifiEdit == "true")
                        {
                            exe_Select_date = "SELECT m.claimref, m.pname, m.cname, m.cphone, g.hospital_name, m.roomno, m.adddate, m.pidno, m.disdate, m.epf, m.policy, m.claimno, m.remark1,m.remark2, c.job_status, m.totbil, m.pdamt, c.job_type, m.bhtno, m.billNo, m.aliment, c.CORDINATOR_USERID, m.FILECLO" +
                                                     " from shedata.cor_job_status c" +
                                                     " INNER JOIN SHEDATA.SHHOSINF00 m  ON m.claimref = c.claimref" +
                                                     " INNER JOIN GENERAL_CLAIM.CLAIM_HOSPITAL_DETAILS g ON m.hospital = g.hospital_id" +
                                                     " WHERE m.claimref = :pin_claimref and m.policy = :pin_policy and m.epf = :pin_epf and c.cordinator_userid=:pin_log_user and c.job_type='D' and c.job_status='COMPLETED' and (m.FILECLO is null or m.FILECLO='PN' or m.FILECLO = 'PD')";
                        }
                        else
                        {
                            exe_Select_date = "SELECT m.claimref, m.pname, m.cname, m.cphone, g.hospital_name, m.roomno, m.adddate, m.pidno, m.disdate, m.epf, m.policy, m.claimno, m.remark1,m.remark2, c.job_status, m.totbil, m.pdamt, c.job_type, m.bhtno, m.billNo, m.aliment, c.CORDINATOR_USERID, m.FILECLO" +
                                                     " from shedata.cor_job_status c" +
                                                     " INNER JOIN SHEDATA.SHHOSINF00 m  ON m.claimref = c.claimref" +
                                                     " INNER JOIN GENERAL_CLAIM.CLAIM_HOSPITAL_DETAILS g ON m.hospital = g.hospital_id" +
                                                     " WHERE m.claimref = :pin_claimref and m.policy = :pin_policy and m.epf = :pin_epf and c.cordinator_userid=:pin_log_user and c.job_type='D' and c.job_status='ACCEPTED' and (m.FILECLO is null or m.FILECLO='PN' or  m.FILECLO = 'PD') ";
                        }
                           


                            cmd.CommandText = exe_Select_date;

                            cmd.Parameters.Add("pin_claimref", OracleType.VarChar).Value = claimRefNo;
                            cmd.Parameters.Add("pin_policy", OracleType.VarChar).Value = policy;
                            cmd.Parameters.Add("pin_epf", OracleType.VarChar).Value = epfno;
                            cmd.Parameters.Add("pin_log_user", OracleType.VarChar).Value = userName;

                            OracleDataReader reader = cmd.ExecuteReader();

                            if (reader.Read()) // Assuming only one row is expected
                            {
                                claimDataset = new ClaimData
                                {
                                    ClaimRef = reader["CLAIMREF"].ToString(),
                                    PName = reader["pname"].ToString(),
                                    CName = reader["cname"].ToString(),
                                    CPhone = reader["cphone"].ToString(),
                                    HospitalName = reader["hospital_name"].ToString(),
                                    RoomNo = reader["roomno"].ToString(),
                                    AddDate = reader["adddate"].ToString(),
                                    PidNo = reader["pidno"].ToString(),
                                    DisDate = reader["disdate"].ToString(),
                                    Epf = reader["epf"].ToString(),
                                    Policy = reader["policy"].ToString(),
                                    clamNo = reader["claimno"].ToString(),
                                    Remark1 = reader["remark1"].ToString(),
                                    JobStatus = reader["job_status"].ToString(),
                                    TotBil = reader["totbil"].ToString(),
                                    PDamt = reader["pdamt"].ToString(),
                                    JobType = reader["job_type"].ToString(),
                                    bilNo = reader["billNo"].ToString(),
                                    bthNo = reader["bhtno"].ToString(),
                                    aliment = reader["aliment"].ToString(),
                                    Remark2 = reader["remark2"].ToString(),
                                    coordiUserId = reader["CORDINATOR_USERID"].ToString()
                                };

                                Session["ClaimData"] = claimDataset;
                            }
                            else
                            {
                                //lblAlertMessage.Visible = true;
                                //lblAlertMessage.Text = "Cant proceed payment for this claim.";


                                lblAlertMessage.Text = "Claim payment cant proceed. Because claim status not accepted or claim was rejected.";
                                lblAlertMessage.Attributes.Add("data-alert-title", "Alert");
                                lblAlertMessage.Attributes.Add("data-alert-message", lblAlertMessage.Text);
                                lblAlertMessage.Visible = true;
                            }
                        }

                }
                catch (Exception ex)
                {
                    string msgs = "ERROR:" + ex.Message;
                    Console.WriteLine(cmd.CommandText);
                    foreach (OracleParameter p in cmd.Parameters)
                    {
                        Console.WriteLine(p.ParameterName + " = " + p.Value);
                    }

                    Console.WriteLine(ex.Message);
                }
                finally
                {
                    if (oconn.State == ConnectionState.Open)
                    {
                        oconn.Close();
                    }
                }

                if (claimDataset != null)
                {
                    ClaimReferenceLabel.InnerText = claimDataset.ClaimRef;
                    ClaimNo.InnerText = claimDataset.clamNo;
                    hospital.InnerText = claimDataset.HospitalName;
                    roomNo.InnerText = claimDataset.RoomNo;
                    admitedDate.InnerText = claimDataset.AddDate;
                    dischargeDate.InnerText = claimDataset.DisDate;
                    patientName.InnerText = claimDataset.PName;
                    billAmount.InnerText = decimal.Parse(claimDataset.TotBil).ToString("F2");
                    paidAmount.InnerText = decimal.Parse(claimDataset.PDamt).ToString("F2");
                    claimStatus.InnerText = claimDataset.JobStatus;
                    totBill.Value = decimal.Parse(claimDataset.TotBil).ToString("F2");
                    paidAmo.Value = decimal.Parse(claimDataset.PDamt).ToString("F2");
                    bhtNo3.Value = claimDataset.bthNo;
                    alment.Value = claimDataset.aliment;
                    R1.Value = claimDataset.Remark1;
                    R2.Value = claimDataset.Remark2;

                    if (claimDataset.JobType == "A")
                    {
                        jobType.InnerHtml = "ADMITED";
                    }
                    else if (claimDataset.JobType == "D")
                    {
                        jobType.InnerHtml = "DISCHARGE";
                    }
                    else
                    {

                    }
                }


            }
        }

        protected void claimPayment_submit_Click(object sender, EventArgs e)
        {

            panel1.Visible = false;
            panel2.Visible = true;

            ClaimData claimData = Session["ClaimData"] as ClaimData;

            if (claimData != null)
            {
                refNoo.InnerText = claimData.ClaimRef;
                emplNo.InnerText = claimData.Epf;
                polNo.InnerText = claimData.Policy;
                hosp.InnerText = claimData.HospitalName;
                addDate.InnerText = claimData.AddDate;
                disDate.InnerText = claimData.DisDate;
                rNo.InnerText = claimData.RoomNo;

                claNo.Value = claimData.ClaimRef;
                claimSta.Value = claimData.JobStatus;
                execuName.Value = Session["LoggedUser"].ToString();
                totBill.Value = billAmount.InnerText;
                paidAmo.Value = paidAmount.InnerText;
                
            }

            //clear alert messages if exist
            ClientScript.RegisterStartupScript(this.GetType(), "clearAlert", "clearAlert();", true);

        }


       


        protected void back_click(object sender, EventArgs e)
        {
            string fronNotificationPanel = Request.QueryString["fromNotifi"];

            if (fronNotificationPanel == "true" || fromNotifiEdit == "true")
            {
                Response.Redirect("~/Notifications.aspx");
            }
            else
            {
                Response.Redirect("~/ClaimPayment/ClaimPayList.aspx?POLICYNO=" + dc.Encrypt(policy) + "&EPF=" + dc.Encrypt(epfno));
                
            }

            //Response.Redirect("~/Default.aspx");

        }

        protected void back_click2(object sender, EventArgs e)
        {
            panel1.Visible = true;
            panel2.Visible = false;
            panel3.Visible = false;
            ClientScript.RegisterStartupScript(this.GetType(), "clearAlert", "clearAlert();", true);
        }

        protected void back_main(object sender, EventArgs e)
        {
            //if (epfno == "" || epfno == null)
            //{
            //    Response.Redirect("~/Default.aspx");
            //}
            //else
            //{
            //    Response.Redirect("~/Notifications.aspx");
            //}

            Response.Redirect("~/Default.aspx");
        }

        protected void claim_payment_update(object sender, EventArgs e)
        {


            int claimRef = Int32.Parse(refNoo.InnerHtml);
            string aliment = alment.Value;
            string execName = execuName.Value;
            double totalBillAm = Convert.ToDouble(totBill.Value);
            double paidAmount = Convert.ToDouble(paidAmo.Value);
            string remark1 = R1.Value;
            string remark2 = R2.Value;
            string jobStatus = "COMPLETED";
            string jobStatus2 = "PD";
            string bhtNo = bhtNo3.Value;
            int refNo = Int32.Parse(refNoo.InnerHtml);
            string billNo = "";

            string jobTypeVal = jobType.InnerHtml;
            string jobSatusVal = claimStatus.InnerHtml;

            double evenLimi = Convert.ToDouble(eventLimit);
            

            //check claim job type and status
            if (jobTypeVal == "DISCHARGE" && (jobSatusVal == "ACCEPTED" || jobSatusVal=="COMPLETED") )
            {

                //use API for update claim payment details
                using (WebClient webClient = new System.Net.WebClient())
                {
                    try
                    {
                        WebClient n = new WebClient();

                        var json = n.DownloadString(host_ip + "/SHE_Tab_API/GIService.svc/ClaimPayment?claimNo=" + claimRef + "&aliment=" + aliment + "&exName=" + execName + "&totalBill=" + totalBillAm + "&paidAmount=" + paidAmount + "&remark1=" + remark1 + "&remark2=" + remark2 +
                        "&claimStatus=" + jobStatus2 + "&userName=" + userName + "&bhtNo=" + bhtNo + "&billNo=" + billNo + "&refNo=" + refNo);
                        ;

                        JavaScriptSerializer serializer = new JavaScriptSerializer();
                        var result = serializer.Deserialize<dynamic>(json);

                        string dataT = result["Data"];
                        int idT = result["ID"];
                        int execution = 0;



                        if (idT == 200)
                        {
                            string jbType = "";
                            if (jobType.InnerHtml == "ADMITED")
                            {
                                jbType = "A";
                            }
                            else if (jobType.InnerHtml == "DISCHARGE")
                            {
                                jbType = "D";
                            }
                            else
                            {
                                 jbType = "";

                            }

                            //update job status table
                            string exe_Update = "update shedata.cor_job_status set JOB_STATUS =:pin_jobSta WHERE CLAIMREF = :pin_cliRef and CORDINATOR_USERID = :cordId and JOB_STATUS = :job_status and JOB_TYPE = :job_type";

                            OracleTransaction transaction = null;
                            oconn.Open();
                            try
                            {
                                transaction = oconn.BeginTransaction();

                                OracleCommand cmd_backup = oconn.CreateCommand();
                                cmd_backup.CommandType = CommandType.Text;
                                cmd_backup.CommandText = exe_Update;

                                cmd_backup.Parameters.Add(":pin_jobSta", OracleType.VarChar).Value = jobStatus;
                                cmd_backup.Parameters.Add(":pin_cliRef", OracleType.VarChar).Value = claimRef;
                                cmd_backup.Parameters.Add(":cordId", OracleType.VarChar).Value = userName;
                                cmd_backup.Parameters.Add(":job_status", OracleType.VarChar).Value = claimStatus.InnerHtml;
                                cmd_backup.Parameters.Add(":job_type", OracleType.VarChar).Value = jbType;

                                cmd_backup.Transaction = transaction;
                                execution = cmd_backup.ExecuteNonQuery();

                                transaction.Commit();
                            }

                            catch (OracleException ex)
                            {
                                result = false;
                                transaction.Rollback();
                            }

                            finally
                            {
                                oconn.Close();
                                oconn.Dispose();
                            }

                            if (execution > 0)
                            {
                                lblAlertMessage.Text = "Claim payment detail was updated";
                                lblAlertMessage.Attributes.Add("data-alert-title", "Success");
                                lblAlertMessage.Attributes.Add("data-alert-message", lblAlertMessage.Text);
                                lblAlertMessage.Visible = true;

                                Button4.Visible = true;
                                panel3.Visible = true;
                                panel2.Visible = false;

                                ClaimData claimData = Session["ClaimData"] as ClaimData;
                                polNo2.InnerHtml = claimData.Policy;
                                nameInsu.InnerHtml = claimData.CName;
                                refNo1.InnerHtml = claimData.ClaimRef;
                                bhtNo2.InnerHtml = bhtNo3.Value;
                                billNo2.InnerHtml = claimData.bilNo;
                                hosName.InnerHtml = claimData.HospitalName;
                                rema1.InnerHtml = claimData.Remark1;
                                rema2.InnerHtml = claimData.Remark2;

                                string totalbillamo = totBill.Value;
                                double totalBillAmount = Convert.ToDouble(totalbillamo);
                                totStAm.InnerHtml = totalBillAmount.ToString("0.00");

                                string totPayAmount = paidAmo.Value;
                                double totPayAmount2 = Convert.ToDouble(totPayAmount);
                                totStaePayAm.InnerHtml = totPayAmount2.ToString("0.00");

                            }
                            else
                            {
                                lblAlertMessage.Text = "Claim payment detail not updated";
                                lblAlertMessage.Attributes.Add("data-alert-title", "Alert");
                                lblAlertMessage.Attributes.Add("data-alert-message", lblAlertMessage.Text);
                                lblAlertMessage.Visible = true;

                                
                            }



                        }
                        else
                        {
                            lblAlertMessage.Text = "Claim payment detail not updated";
                            lblAlertMessage.Attributes.Add("data-alert-title", "Alert");
                            lblAlertMessage.Attributes.Add("data-alert-message", lblAlertMessage.Text);
                            lblAlertMessage.Visible = true;
                        }
                    }
                    catch (Exception ex)
                    {
                        throw ex;
                        //this.lblErrorMsg.Text = "No data found for this policy no";
                    }

                }
            }
            else
            {
                lblAlertMessage.Text = "Claim payment cant proceed. Because claim status not accepted.";
                lblAlertMessage.Attributes.Add("data-alert-title", "Alert");
                lblAlertMessage.Attributes.Add("data-alert-message", lblAlertMessage.Text);
                lblAlertMessage.Visible = true;
            }
            //end of update claim payment using API

            //update claimpayment by sql query
            //int exe = 0, execution =0;
            //string exe_Update2 = "update shedata.shhosinf00 set ALIMENT=:pin_aliment , EXNAME=:pin_execNam, TOTBIL=:pin_totBil, PDAMT=:pin_paiAmou, REMARK1=:pin_r1, REMARK2=:pin_r2, CLAIMNO=:pin_refNo, " +
            //                    "BHTNO =:pin_bhtno, BILLNO = :pin_bilno where claimref =:pin_climrefno";

            //OracleTransaction transaction2 = null;
            //oconn.Open();
            //try
            //{
            //    transaction2 = oconn.BeginTransaction();

            //    OracleCommand cmd_backup = oconn.CreateCommand();
            //    cmd_backup.CommandType = CommandType.Text;
            //    cmd_backup.CommandText = exe_Update2;

            //    //cmd_backup.Parameters.Add(":pin_jobSta", OracleType.VarChar).Value = jobStatus;
            //    cmd_backup.Parameters.Add(":pin_climrefno", OracleType.Int32).Value = claimRef;
            //    cmd_backup.Parameters.Add(":pin_aliment", OracleType.VarChar).Value = aliment;
            //    cmd_backup.Parameters.Add(":pin_execNam", OracleType.VarChar).Value = execName;
            //    cmd_backup.Parameters.Add(":pin_totBil", OracleType.Double).Value = totalBillAm;
            //    cmd_backup.Parameters.Add(":pin_paiAmou", OracleType.Double).Value = paidAmount;
            //    cmd_backup.Parameters.Add(":pin_r1", OracleType.VarChar).Value = remark1;
            //    cmd_backup.Parameters.Add(":pin_r2", OracleType.VarChar).Value = remark2;
            //    cmd_backup.Parameters.Add(":pin_refNo", OracleType.VarChar).Value = refNo;
            //    cmd_backup.Parameters.Add(":pin_bhtno", OracleType.VarChar).Value = bhtNo;
            //    cmd_backup.Parameters.Add(":pin_bilno", OracleType.VarChar).Value = billNo;

            //    cmd_backup.Transaction = transaction2;
            //    exe = cmd_backup.ExecuteNonQuery();

            //    transaction2.Commit();
            //}

            //catch (OracleException ex)
            //{
            //    //result = false;
            //    transaction2.Rollback();
            //    lblAlertMessage.Text = "Some thing went to wrong";
            //    lblAlertMessage.Attributes.Add("data-alert-title", "Success");
            //    lblAlertMessage.Attributes.Add("data-alert-message", lblAlertMessage.Text);
            //    lblAlertMessage.Visible = true;
            //}

            //finally
            //{
            //    oconn.Close();
            //    oconn.Dispose();
            //}

            //if (exe > 0)
            //{
            //    //update job status table
            //    string exe_Update = "update shedata.cor_job_status set JOB_STATUS =:pin_jobSta WHERE CLAIMREF = :pin_cliRef and CORDINATOR_USERID = :cordId";

            //    OracleTransaction transaction = null;
            //    OracleConnection oconn2 = new OracleConnection(ConfigurationManager.AppSettings["OracleDB"]);

            //    if (oconn2.State != ConnectionState.Open)
            //    {

            //        oconn2.Open();
            //    }

            //    //OracleCommand cmd = oconn.CreateCommand();
            //    //oconn.Open();
            //    try
            //    {
            //        transaction = oconn2.BeginTransaction();

            //        OracleCommand cmd_backup = oconn2.CreateCommand();
            //        cmd_backup.CommandType = CommandType.Text;
            //        cmd_backup.CommandText = exe_Update;

            //        cmd_backup.Parameters.Add(":pin_jobSta", OracleType.VarChar).Value = jobStatus;
            //        cmd_backup.Parameters.Add(":pin_cliRef", OracleType.VarChar).Value = claimRef;
            //        cmd_backup.Parameters.Add(":cordId", OracleType.VarChar).Value = userName;

            //        cmd_backup.Transaction = transaction;
            //        execution = cmd_backup.ExecuteNonQuery();

            //        transaction.Commit();
            //    }

            //    catch (OracleException ex)
            //    {
            //        //result = false;
            //        transaction.Rollback();
            //        lblAlertMessage.Text = "Some thing went to wrong";
            //        lblAlertMessage.Attributes.Add("data-alert-title", "Success");
            //        lblAlertMessage.Attributes.Add("data-alert-message", lblAlertMessage.Text);
            //        lblAlertMessage.Visible = true;
            //    }

            //    finally
            //    {
            //        oconn2.Close();
            //        oconn2.Dispose();
            //    }

            //    if (execution > 0)
            //    {
            //        lblAlertMessage.Text = "Claim payment detail was updated";
            //        lblAlertMessage.Attributes.Add("data-alert-title", "Success");
            //        lblAlertMessage.Attributes.Add("data-alert-message", lblAlertMessage.Text);
            //        lblAlertMessage.Visible = true;
            //    }


            //}
            //else
            //{
            //    lblAlertMessage.Text = "Claim payment detail not updated";
            //    lblAlertMessage.Attributes.Add("data-alert-title", "Alert");
            //    lblAlertMessage.Attributes.Add("data-alert-message", lblAlertMessage.Text);
            //    lblAlertMessage.Visible = true;
            //}

            //end of sql update

        }


        //edit payment click
        protected void claim_payment_edit(object sender, EventArgs e)
        {


            int claimRef = Int32.Parse(refNoo.InnerHtml);
            string aliment = alment.Value;
            string execName = execuName.Value;
            double totalBillAm = Convert.ToDouble(totBill.Value);
            double paidAmount = Convert.ToDouble(paidAmo.Value);
            string remark1 = R1.Value;
            string remark2 = R2.Value;
            string jobStatus = "COMPLETED";
            string jobStatus2 = "PD";
            string bhtNo = bhtNo3.Value;
            int refNo = Int32.Parse(refNoo.InnerHtml);
            string billNo = "";

            string jobTypeVal = jobType.InnerHtml;
            string jobSatusVal = claimStatus.InnerHtml;

            double evenLimi = Convert.ToDouble(eventLimit);


            //check claim job type and status
            if (jobTypeVal == "DISCHARGE" && (jobSatusVal == "ACCEPTED" || jobSatusVal == "COMPLETED"))
            {

                //use API for update claim payment details
                using (WebClient webClient = new System.Net.WebClient())
                {
                    try
                    {
                        WebClient n = new WebClient();

                        var json = n.DownloadString(host_ip + "/SHE_Tab_API/GIService.svc/ClaimPayment?claimNo=" + claimRef + "&aliment=" + aliment + "&exName=" + execName + "&totalBill=" + totalBillAm + "&paidAmount=" + paidAmount + "&remark1=" + remark1 + "&remark2=" + remark2 +
                        "&claimStatus=" + jobStatus2 + "&userName=" + userName + "&bhtNo=" + bhtNo + "&billNo=" + billNo + "&refNo=" + refNo);
                        ;

                        JavaScriptSerializer serializer = new JavaScriptSerializer();
                        var result = serializer.Deserialize<dynamic>(json);

                        string dataT = result["Data"];
                        int idT = result["ID"];
                        int execution = 0;



                        if (idT == 200)
                        {
                            string jbType = "";
                            if (jobType.InnerHtml == "ADMITED")
                            {
                                jbType = "A";
                            }
                            else if (jobType.InnerHtml == "DISCHARGE")
                            {
                                jbType = "D";
                            }
                            else
                            {
                                jbType = "";

                            }

                            //update job status table
                            string exe_Update = "update shedata.cor_job_status set JOB_STATUS =:pin_jobSta WHERE CLAIMREF = :pin_cliRef and CORDINATOR_USERID = :cordId and JOB_STATUS = :job_status and JOB_TYPE = :job_type";

                            OracleTransaction transaction = null;
                            oconn.Open();
                            try
                            {
                                transaction = oconn.BeginTransaction();

                                OracleCommand cmd_backup = oconn.CreateCommand();
                                cmd_backup.CommandType = CommandType.Text;
                                cmd_backup.CommandText = exe_Update;

                                cmd_backup.Parameters.Add(":pin_jobSta", OracleType.VarChar).Value = jobStatus;
                                cmd_backup.Parameters.Add(":pin_cliRef", OracleType.VarChar).Value = claimRef;
                                cmd_backup.Parameters.Add(":cordId", OracleType.VarChar).Value = userName;
                                cmd_backup.Parameters.Add(":job_status", OracleType.VarChar).Value = claimStatus.InnerHtml;
                                cmd_backup.Parameters.Add(":job_type", OracleType.VarChar).Value = jbType;

                                cmd_backup.Transaction = transaction;
                                execution = cmd_backup.ExecuteNonQuery();

                                transaction.Commit();
                            }

                            catch (OracleException ex)
                            {
                                result = false;
                                transaction.Rollback();
                            }

                            finally
                            {
                                oconn.Close();
                                oconn.Dispose();
                            }
                            //end of update

                            if (execution > 0)
                            {
                                int execution2 = 0;
                                //insert edit record to reason table 
                                string exe_insert_reason = "INSERT INTO shedata.reject_reason(CLAIM_REF, JOB_TYPE ,CLAIM_STATUS, REJECT_REASON, UPDATE_USER, UPDATE_DATE,ACTION, PAY_UP_REA) " +
                                                    "VALUES (:claimref, 'D', 'PD', 'No', :updUser, :editDate, 'Edit_Payment', :payEditRea)";
                                OracleTransaction transaction2 = null;

                                OracleConnection oconn2 = new OracleConnection(ConfigurationManager.AppSettings["OracleDB"]);

                                if (oconn2.State != ConnectionState.Open)
                                {

                                    oconn2.Open();
                                }
                                try
                                {
                                    transaction2 = oconn2.BeginTransaction();

                                    OracleCommand cmd_backup2 = oconn2.CreateCommand();
                                    cmd_backup2.CommandType = CommandType.Text;
                                    cmd_backup2.CommandText = exe_insert_reason;

                                    cmd_backup2.Parameters.Add(":claimref", OracleType.VarChar).Value = claimRef;
                                    cmd_backup2.Parameters.Add(":updUser", OracleType.VarChar).Value = userName;
                                    cmd_backup2.Parameters.Add(":editDate", OracleType.VarChar).Value = DateTime.Now.ToString("dd-MMM-yy").ToUpper();
                                    cmd_backup2.Parameters.Add(":payEditRea", OracleType.VarChar).Value = editReason.InnerHtml;

                                    cmd_backup2.Transaction = transaction2;
                                    execution2 = cmd_backup2.ExecuteNonQuery();

                                    transaction2.Commit();
                                }

                                catch (OracleException ex)
                                {
                                    result = false;
                                    transaction2.Rollback();
                                }

                                finally
                                {
                                    oconn2.Close();
                                    oconn2.Dispose();
                                }
                                //end of insert

                                if(execution2 >0)
                                {
                                    lblAlertMessage.Text = "Payment details was edited";
                                    lblAlertMessage.Attributes.Add("data-alert-title", "Success");
                                    lblAlertMessage.Attributes.Add("data-alert-message", lblAlertMessage.Text);
                                    lblAlertMessage.Visible = true;

                                    Button4.Visible = true;
                                    panel3.Visible = true;
                                    panel2.Visible = false;

                                    ClaimData claimData = Session["ClaimData"] as ClaimData;
                                    polNo2.InnerHtml = claimData.Policy;
                                    nameInsu.InnerHtml = claimData.CName;
                                    refNo1.InnerHtml = claimData.ClaimRef;
                                    bhtNo2.InnerHtml = bhtNo3.Value;
                                    billNo2.InnerHtml = claimData.bilNo;
                                    hosName.InnerHtml = claimData.HospitalName;
                                    rema1.InnerHtml = claimData.Remark1;
                                    rema2.InnerHtml = claimData.Remark2;

                                    string totalbillamo = totBill.Value;
                                    double totalBillAmount = Convert.ToDouble(totalbillamo);
                                    totStAm.InnerHtml = totalBillAmount.ToString("0.00");

                                    string totPayAmount = paidAmo.Value;
                                    double totPayAmount2 = Convert.ToDouble(totPayAmount);
                                    totStaePayAm.InnerHtml = totPayAmount2.ToString("0.00");

                                }
                                else
                                {
                                    lblAlertMessage.Text = "Claim payment detail not edited";
                                    lblAlertMessage.Attributes.Add("data-alert-title", "Alert");
                                    lblAlertMessage.Attributes.Add("data-alert-message", lblAlertMessage.Text);
                                    lblAlertMessage.Visible = true;
                                }

                                

                            }
                            else
                            {
                                lblAlertMessage.Text = "Claim payment detail not edited";
                                lblAlertMessage.Attributes.Add("data-alert-title", "Alert");
                                lblAlertMessage.Attributes.Add("data-alert-message", lblAlertMessage.Text);
                                lblAlertMessage.Visible = true;


                            }



                        }
                        else
                        {
                            lblAlertMessage.Text = "Claim payment detail not edited";
                            lblAlertMessage.Attributes.Add("data-alert-title", "Alert");
                            lblAlertMessage.Attributes.Add("data-alert-message", lblAlertMessage.Text);
                            lblAlertMessage.Visible = true;
                        }
                    }
                    catch (Exception ex)
                    {
                        throw ex;
                        //this.lblErrorMsg.Text = "No data found for this policy no";
                    }

                }
            }
            else
            {
                lblAlertMessage.Text = "Claim payment cant proceed. Because claim status not accepted.";
                lblAlertMessage.Attributes.Add("data-alert-title", "Alert");
                lblAlertMessage.Attributes.Add("data-alert-message", lblAlertMessage.Text);
                lblAlertMessage.Visible = true;
            }
            //end of update claim payment using API

        }

        //end of edit payment

        //medical statement button click
        protected async void down_recei_pdf(object sender, EventArgs e)
        {
            byte[] pdfBytes = await GenerateCovernote();
            Response.Clear();
            Response.ContentType = "application/pdf";
            Response.AddHeader("content-disposition", "attachment;filename=Covernote.pdf");
            Response.Cache.SetCacheability(HttpCacheability.NoCache);
            Response.BinaryWrite(pdfBytes);
            Response.Flush(); // Send the response to the client
            HttpContext.Current.ApplicationInstance.CompleteRequest(); // Terminate the request without throwing ThreadAbortException

        }

        private async Task<byte[]> GenerateCovernote()
        {


            try
            {
                ClaimData claimData = Session["ClaimData"] as ClaimData;

                

                string policyNo = claimData.Policy;
                string nameOfInsure = claimData.CName;
                string referenceNo = claimData.ClaimRef;
                string bhtNo = claimData.bthNo;
                string billNo = claimData.bilNo;
                string hospitalName = claimData.HospitalName;
                string remark1 = claimData.Remark1;
                string remark2 = claimData.Remark2;

                string totalbillamo = totBill.Value;
                double totalBillAmount = Convert.ToDouble(totalbillamo);
                string formattedTotalBillAmount = totalBillAmount.ToString("0.00");

                string totPayAmount = paidAmo.Value;
                double totPayAmount2 = Convert.ToDouble(totPayAmount);
                string ftotPayAmount2 = totPayAmount2.ToString("0.00");

                string coorId = claimData.coordiUserId;
                string coorName = "";


                // for get coordinatoer name for the job


                if (oconn.State != ConnectionState.Open)
                {
                    oconn.Open();
                }

                OracleCommand cmd = oconn.CreateCommand();

                try
                {

                    using (cmd)
                    {
                        string exe_Select_date = "SELECT s.CSRNAME, s.CSRBRNC,s.CSRUSRN,s.csrtpno, s.CSRGXNO FROM shedata.sheglxag s WHERE s.CSRSTAT = 'Y' AND s.CSRGTAB = 'Y' and  s.csrusrn= :pin_userId ";


                        cmd.CommandText = exe_Select_date;

                        cmd.Parameters.Add("pin_userId", OracleType.VarChar).Value = coorId;


                        OracleDataReader reader = cmd.ExecuteReader();

                        if (reader.Read()) // Assuming only one row is expected
                        {

                            coorName = reader["CSRNAME"].ToString();
                        }
                        else
                        {
                            lblAlertMessage.Text = "Claim statement reprint cant proceed. Because claim status not completed.";
                            lblAlertMessage.Attributes.Add("data-alert-title", "Alert");
                            lblAlertMessage.Attributes.Add("data-alert-message", lblAlertMessage.Text);
                            lblAlertMessage.Visible = true;
                        }
                    }


                }
                catch (Exception ex)
                {
                    string msgs = "ERROR:" + ex.Message;
                    Console.WriteLine(cmd.CommandText);
                    foreach (OracleParameter p in cmd.Parameters)
                    {
                        Console.WriteLine(p.ParameterName + " = " + p.Value);
                    }

                    Console.WriteLine(ex.Message);
                }
                finally
                {
                    if (oconn.State == ConnectionState.Open)
                    {
                        oconn.Close();
                    }
                }


                int RECEIPTNUMBER = 111;
                using (MemoryStream memoryStream = new MemoryStream())
                {
                    Document document = new Document();
                    PdfWriter writer = PdfWriter.GetInstance(document, memoryStream);

                    document.Open();


                    var content = writer.DirectContent;
                    var pageBorderRect = new Rectangle(document.PageSize);

                    pageBorderRect.Left += document.LeftMargin;
                    pageBorderRect.Right -= document.RightMargin;
                    pageBorderRect.Top -= document.TopMargin;
                    pageBorderRect.Bottom += document.BottomMargin;


                    content.Rectangle(pageBorderRect.Left, pageBorderRect.Bottom, pageBorderRect.Width, pageBorderRect.Height);
                    content.Stroke();

                    Font fontBold = FontFactory.GetFont(FontFactory.HELVETICA, 10);
                    fontBold.SetStyle(Font.BOLD);

                    Font normalFont = FontFactory.GetFont(FontFactory.HELVETICA, 10);
                    Font normalFont2 = FontFactory.GetFont(FontFactory.HELVETICA, 11);



                    string imagePath = Server.MapPath("~/images/New_Slic_Logo.png");
                    iTextSharp.text.Image image1 = iTextSharp.text.Image.GetInstance(imagePath);
                    image1.ScaleAbsolute(80f, 50f);
                    // Calculate the X coordinate to center the image horizontally
                    float x = (PageSize.A4.Width - image1.ScaledWidth) / 2;
                    // Set the position of the image
                    image1.SetAbsolutePosition(x, document.PageSize.Height - 100f); // Adjust the Y coordinate as needed
                    image1.SpacingBefore = 20f;
                    // Add the image to the document
                    document.Add(image1);

                    Font font = new Font();
                    font.SetStyle(Font.BOLD);
                    font.SetStyle(Font.UNDERLINE);
                    Paragraph paragraph1 = new Paragraph("Health Plus Medicare Hospital Statement", font);

                    paragraph1.Alignment = Element.ALIGN_CENTER;
                    paragraph1.IndentationLeft = 20f; // Set left margin
                    paragraph1.SpacingBefore = 70f;
                    document.Add(paragraph1);

                    Paragraph paragraph2 = new Paragraph(hospitalName, fontBold);
                    paragraph2.IndentationLeft = 10f; // Set left margin
                    paragraph2.SpacingBefore = 10f;
                    document.Add(paragraph2);

                    Paragraph paragraph3 = new Paragraph("_____________________", fontBold);
                    paragraph3.IndentationLeft = 10f; // Set left margin
                    paragraph3.SpacingBefore = 10f;
                    document.Add(paragraph3);


                    //table for statement details start

                    PdfPTable table4 = new PdfPTable(2);
                    table4.SpacingBefore = 10f;
                    table4.WidthPercentage = 80; // Set the table width to 80% of the page width
                    table4.DefaultCell.Border = PdfPCell.NO_BORDER; // Remove borders for all cells
                    table4.SpacingBefore = 20f;
                    // Set the border color for all cells
                    table4.DefaultCell.BorderColor = new BaseColor(0, 0, 255);
                    table4.DefaultCell.BorderWidth = 1;

                    PdfPCell cell1 = new PdfPCell(new Phrase("Reference No :", normalFont));
                    PdfPCell cell2 = new PdfPCell(new Phrase(referenceNo, normalFont));
                    PdfPCell cell3 = new PdfPCell(new Phrase("Name of the Insured :", normalFont));
                    PdfPCell cell4 = new PdfPCell(new Phrase(nameOfInsure.ToUpper(), normalFont));
                    PdfPCell cell5 = new PdfPCell(new Phrase("Policy No :", normalFont));
                    PdfPCell cell6 = new PdfPCell(new Phrase(policyNo, normalFont));
                    PdfPCell cell7 = new PdfPCell(new Phrase("B.H.T. No :", normalFont));
                    PdfPCell cell8 = new PdfPCell(new Phrase(bhtNo.ToUpper(), normalFont));
                    PdfPCell cell9 = new PdfPCell(new Phrase("Bill No :", normalFont));
                    PdfPCell cell10 = new PdfPCell(new Phrase(billNo.ToUpper(), normalFont));
                    PdfPCell cell11 = new PdfPCell(new Phrase("Total Bill Amount(Rs.) :", normalFont));
                    PdfPCell cell12 = new PdfPCell(new Phrase(formattedTotalBillAmount, normalFont));
                    PdfPCell cell13 = new PdfPCell(new Phrase("Amount Payable by Insurance(Rs.) :", normalFont));
                    PdfPCell cell14 = new PdfPCell(new Phrase(ftotPayAmount2, normalFont));
                    PdfPCell cell15 = new PdfPCell(new Phrase("Remark 1 :", normalFont));
                    PdfPCell cell16 = new PdfPCell(new Phrase(remark1, normalFont));
                    PdfPCell cell17 = new PdfPCell(new Phrase("Remark 2 :", normalFont));
                    PdfPCell cell18 = new PdfPCell(new Phrase(remark2, normalFont));



                    cell1.Padding = 3f;
                    cell2.Padding = 3f;
                    cell3.Padding = 3f;
                    cell4.Padding = 3f;
                    cell5.Padding = 3f;
                    cell6.Padding = 3f;
                    cell7.Padding = 3f;
                    cell8.Padding = 3f;
                    cell9.Padding = 3f;
                    cell10.Padding = 3f;
                    cell11.Padding = 3f;
                    cell12.Padding = 3f;
                    cell13.Padding = 3f;
                    cell14.Padding = 3f;
                    cell15.Padding = 3f;
                    cell16.Padding = 3f;
                    cell17.Padding = 3f;
                    cell18.Padding = 3f;



                    //cell1.Border = PdfPCell.NO_BORDER;
                    //cell2.Border = PdfPCell.NO_BORDER;
                    //cell3.Border = PdfPCell.NO_BORDER;
                    //cell4.Border = PdfPCell.NO_BORDER;
                    //cell5.Border = PdfPCell.NO_BORDER;
                    //cell6.Border = PdfPCell.NO_BORDER;
                    //cell7.Border = PdfPCell.NO_BORDER;
                    //cell8.Border = PdfPCell.NO_BORDER;
                    //cell9.Border = PdfPCell.NO_BORDER;
                    //cell10.Border = PdfPCell.NO_BORDER;
                    //cell11.Border = PdfPCell.NO_BORDER;
                    //cell12.Border = PdfPCell.NO_BORDER;
                    //cell13.Border = PdfPCell.NO_BORDER;
                    //cell14.Border = PdfPCell.NO_BORDER;


                    table4.AddCell(cell1);
                    table4.AddCell(cell2);
                    table4.AddCell(cell3);
                    table4.AddCell(cell4);
                    table4.AddCell(cell5);
                    table4.AddCell(cell6);
                    table4.AddCell(cell7);
                    table4.AddCell(cell8);
                    table4.AddCell(cell9);
                    table4.AddCell(cell10);
                    table4.AddCell(cell11);
                    table4.AddCell(cell12);
                    table4.AddCell(cell13);
                    table4.AddCell(cell14);
                    table4.AddCell(cell15);
                    table4.AddCell(cell16);
                    table4.AddCell(cell17);
                    table4.AddCell(cell18);


                    document.Add(table4);

                    //end of statement details

                    Paragraph paragraph4 = new Paragraph("I hereby agree to the above 'Amount payable by insurance' as the full and final settlement, with regard to the above claim/bill.", normalFont);
                    paragraph4.IndentationLeft = 10f; // Set left margin
                    paragraph4.SpacingBefore = 10f;
                    document.Add(paragraph4);


                    //signature table
                    PdfPTable table5 = new PdfPTable(3);
                    table5.SpacingBefore = 10f;
                    table5.WidthPercentage = 80; // Set the table width to 80% of the page width
                    table5.DefaultCell.Border = PdfPCell.BOX; // Remove borders for all cells
                    table5.SpacingBefore = 20f;

                    Font font1 = new Font();
                    font1.SetStyle(Font.UNDERLINE);

                    Phrase phrase = new Phrase();
                    phrase.Add(new Chunk("              ", normalFont)); // Adding spaces before the name
                    phrase.Add(new Chunk(coorName, font1));                // Adding the underlined name
                    phrase.Add(new Chunk("         ", normalFont)); // Adding spaces after the name

                    PdfPCell cell51 = new PdfPCell(phrase);
                    //PdfPCell cell51 = new PdfPCell(new Phrase("            "+ coorName + "     ", font1));
                    PdfPCell cell52 = new PdfPCell(new Phrase("______________________", normalFont));
                    PdfPCell cell53 = new PdfPCell(new Phrase("______________________", normalFont));
                    PdfPCell cell54 = new PdfPCell(new Phrase("Authorized Signature ", normalFont));
                    cell54.HorizontalAlignment = Element.ALIGN_CENTER;
                    PdfPCell cell55 = new PdfPCell(new Phrase("I Agree For Above Settlement(Hospital)", normalFont));
                    cell55.HorizontalAlignment = Element.ALIGN_CENTER;
                    PdfPCell cell56 = new PdfPCell(new Phrase("Date", normalFont));
                    cell56.HorizontalAlignment = Element.ALIGN_CENTER;

                    cell51.Padding = 3f;
                    cell52.Padding = 3f;
                    cell53.Padding = 3f;
                    cell54.Padding = 3f;
                    cell55.Padding = 3f;
                    cell56.Padding = 3f;
                    
                    cell51.Border = PdfPCell.NO_BORDER;
                    cell52.Border = PdfPCell.NO_BORDER;
                    cell53.Border = PdfPCell.NO_BORDER;
                    cell54.Border = PdfPCell.NO_BORDER;
                    cell55.Border = PdfPCell.NO_BORDER;
                    cell56.Border = PdfPCell.NO_BORDER;

                    table5.AddCell(cell51);
                    table5.AddCell(cell52);
                    table5.AddCell(cell53);
                    table5.AddCell(cell54);
                    table5.AddCell(cell55);
                    table5.AddCell(cell56);                    

                    document.Add(table5);

                    //end of signature table

                    // Define a PdfPTable for the company name
                    PdfPTable companyTable = new PdfPTable(1);
                    companyTable.TotalWidth = document.PageSize.Width;
                    companyTable.DefaultCell.Border = PdfPCell.NO_BORDER;

                    PdfPCell cell61 = new PdfPCell(new Phrase("21, Vauxhall Street, Colombo - 2, Sri Lanka.", normalFont));
                    cell61.Border = PdfPCell.NO_BORDER;
                    cell61.HorizontalAlignment = Element.ALIGN_CENTER;
                    cell61.Phrase.Font.Size = 8;
                    companyTable.AddCell(cell61);

                    PdfPCell cell62 = new PdfPCell(new Phrase("Tel : General (94-11)2357457, Fax (94-11)2447742", normalFont));
                    cell62.Border = PdfPCell.NO_BORDER;
                    cell62.HorizontalAlignment = Element.ALIGN_CENTER;
                    cell62.Phrase.Font.Size = 8;
                    companyTable.AddCell(cell62);

                    // Calculate the position to add the company name (at the bottom of the page)
                    float bottomMargin = document.BottomMargin;
                    float footerHeight = companyTable.TotalHeight;
                    float yPosition = bottomMargin + footerHeight;

                    // Add the company name table to the page
                    companyTable.WriteSelectedRows(0, -1, 0, yPosition, writer.DirectContent);

                    // end of PdfPTable for the company name



                    document.Close();

                    return memoryStream.ToArray();
                }



            }
            catch (Exception ex)
            {
                //Logger logger = new Logger();
                //logger.write_log("GenerateCovernote function exception" + ex.Message);
            }


            return null;


        }

        public class ClaimData
        {
            public string ClaimRef { get; set; }
            public string PName { get; set; }
            public string CName { get; set; }
            public string CPhone { get; set; }
            public string HospitalName { get; set; }
            public string RoomNo { get; set; }
            public string AddDate { get; set; }
            public string PidNo { get; set; }
            public string DisDate { get; set; }
            public string Epf { get; set; }
            public string Policy { get; set; }
            public string Remark1 { get; set; }
            public string JobStatus { get; set; }
            public string JobType { get; set; }
            public string TotBil { get; set; }
            public string PDamt { get; set; }
            public string clamNo { get; set; }
            public string bthNo { get; set; }
            public string bilNo { get; set; }
            public string Remark2 { get; set; }
            public string coordiUserId { get; set; }
            public string aliment { get; set; }
        }

    }


}