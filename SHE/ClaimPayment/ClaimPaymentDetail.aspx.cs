using Newtonsoft.Json;
using SHE.Code;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.Odbc;
using System.Data.OracleClient;
using System.Linq;
using System.Net;
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
        string policy, epfno, claimRefNo, userName;
        private List<ClaimData> claimDataList = new List<ClaimData>();

        private ClaimData claimDataset;

        protected void Page_Load(object sender, EventArgs e)
        {
            userName = Session["LoggedUser"].ToString();
            policy = Request.QueryString["POLICYNO"];
            epfno = Request.QueryString["EPF"];
            claimRefNo = Request.QueryString["CLAIMREF"];

            policy = dc.Decrypt(policy);
            epfno = dc.Decrypt(epfno);
            claimRefNo = dc.Decrypt(claimRefNo);

            if (!IsPostBack)
            {
               

                if (oconn.State != ConnectionState.Open)
                {
                    oconn.Open();
                }

                OracleCommand cmd = oconn.CreateCommand();

                try
                {
                    if (epfno == "" )
                    {
                        using (cmd)
                        {
                            string exe_Select_date = "SELECT m.claimref, m.pname, m.cname, m.cphone, g.hospital_name, m.roomno, m.adddate, m.pidno, m.disdate, m.epf, m.policy, m.remark1, c.job_status, m.totbil, m.pdamt" +
                                                      " from shedata.cor_job_status c" +
                                                      " INNER JOIN SHEDATA.SHHOSINF00BKUP m  ON m.claimref = c.claimref" +
                                                      " INNER JOIN GENERAL_CLAIM.CLAIM_HOSPITAL_DETAILS g ON m.hospital = g.hospital_id" +
                                                      " WHERE m.claimref = :pin_claimref and m.policy = :pin_policy ";


                            cmd.CommandText = exe_Select_date;

                            cmd.Parameters.Add("pin_claimref", OracleType.VarChar).Value = claimRefNo;
                            cmd.Parameters.Add("pin_policy", OracleType.VarChar).Value = policy;


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
                                    Remark1 = reader["remark1"].ToString(),
                                    JobStatus = reader["job_status"].ToString(),
                                    TotBil = reader["totbil"].ToString(),
                                    PDamt = reader["pdamt"].ToString()
                                };

                                Session["ClaimData"] = claimDataset;
                            }
                            else
                            {
                                lblAlertMessage.Visible = true;
                                lblAlertMessage.Text = "No data available";
                            }
                        }
                    }
                    else
                    {
                        using (cmd)
                        {
                            string exe_Select_date = "SELECT m.claimref, m.pname, m.cname, m.cphone, g.hospital_name, m.roomno, m.adddate, m.pidno, m.disdate, m.epf, m.policy, m.remark1, c.job_status, m.totbil, m.pdamt" +
                                                      " from shedata.cor_job_status c" +
                                                      " INNER JOIN SHEDATA.SHHOSINF00BKUP m  ON m.claimref = c.claimref" +
                                                      " INNER JOIN GENERAL_CLAIM.CLAIM_HOSPITAL_DETAILS g ON m.hospital = g.hospital_id" +
                                                      " WHERE m.claimref = :pin_claimref and m.policy = :pin_policy and m.epf = :pin_epf";


                            cmd.CommandText = exe_Select_date;

                            cmd.Parameters.Add("pin_claimref", OracleType.VarChar).Value = claimRefNo;
                            cmd.Parameters.Add("pin_policy", OracleType.VarChar).Value = policy;
                            cmd.Parameters.Add("pin_epf", OracleType.VarChar).Value = epfno;

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
                                    Remark1 = reader["remark1"].ToString(),
                                    JobStatus = reader["job_status"].ToString(),
                                    TotBil = reader["totbil"].ToString(),
                                    PDamt = reader["pdamt"].ToString()
                                };

                                Session["ClaimData"] = claimDataset;
                            }
                            else
                            {
                                lblAlertMessage.Visible = true;
                                lblAlertMessage.Text = "No data available";
                            }
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
               
            
                    ClaimReferenceLabel.InnerText = claimDataset.ClaimRef;
                    ClaimNo.InnerText = claimDataset.clamNo;
                    hospital.InnerText = claimDataset.HospitalName;
                    roomNo.InnerText = claimDataset.RoomNo;
                    admitedDate.InnerText = claimDataset.AddDate;
                    dischargeDate.InnerText = claimDataset.DisDate;
                    patientName.InnerText = claimDataset.PName;
                    billAmount.InnerText = claimDataset.TotBil;
                    paidAmount.InnerText = claimDataset.PDamt;
                    claimStatus.InnerText = claimDataset.JobStatus;               
               
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
            }


        }

        protected void back_click(object sender, EventArgs e)
        {
            if(epfno == "" || epfno==null)
            {
                Response.Redirect("~/Default.aspx");
            }
            else
            {
                Response.Redirect("~/Notifications.aspx");
            }
            
        }

        protected void back_click2(object sender, EventArgs e)
        {
            panel1.Visible = true;
            panel2.Visible = false;
        }

        protected void back_main(object sender, EventArgs e)
        {
            if (epfno == "" || epfno == null)
            {
                Response.Redirect("~/Default.aspx");
            }
            else
            {
                Response.Redirect("~/Notifications.aspx");
            }
        }

        protected void claim_payment_update(object sender, EventArgs e)
        {
           

            int claimRef = Int32.Parse(claNo.Value);
            string aliment = alment.Value;
            string execName = execuName.Value;
            double totalBillAm =Convert.ToDouble(totBill.Value);
            double paidAmount = Convert.ToDouble(paidAmo.Value);
            string remark1 = R1.Value;
            string remark2 = R2.Value;
            string jobStatus = "COMPLETED";          
            string bhtNo = "";
            string refNo ="";
            string billNo ="";

            //use API for update claim payment details
            //            using (WebClient webClient = new System.Net.WebClient())
            //            {
            //                try
            //                {
            //                    WebClient n = new WebClient();

            //                    var json = n.DownloadString(host_ip + "/SHE_Tab_API/GIService.svc/ClaimPayment?claimNo="+ claimRef + "&aliment=" + aliment + "&exName=" + execName + "&totalBill=" + totalBillAm + "&paidAmount=" + paidAmount +"&remark1=" + remark1 +"&remark2="+ remark2 +
            //                    "&claimStatus="+ jobStatus + "&userName="+ userName+"&bhtNo="+ bhtNo+"&billNo="+ billNo+"&refNo="+ refNo);
            //;

            //                    JavaScriptSerializer serializer = new JavaScriptSerializer();
            //                    var result = serializer.Deserialize<dynamic>(json);

            //                    string dataT = result["Data"];
            //                    int idT = result["ID"];
            //                    int execution = 0;



            //                    if (idT == 200)
            //                    {
            //                        //update job status table
            //                        string exe_Update = "update shedata.cor_job_status set JOB_STATUS =:pin_jobSta WHERE CLAIMREF = :pin_cliRef";

            //                        OracleTransaction transaction = null;
            //                        oconn.Open();
            //                        try
            //                        {
            //                            transaction = oconn.BeginTransaction();

            //                            OracleCommand cmd_backup = oconn.CreateCommand();
            //                            cmd_backup.CommandType = CommandType.Text;
            //                            cmd_backup.CommandText = exe_Update;

            //                            cmd_backup.Parameters.Add(":pin_jobSta", OracleType.VarChar).Value = jobStatus;
            //                            cmd_backup.Parameters.Add(":pin_cliRef", OracleType.VarChar).Value = claimRef;

            //                            cmd_backup.Transaction = transaction;
            //                            execution = cmd_backup.ExecuteNonQuery();

            //                            transaction.Commit();
            //                        }

            //                        catch (OracleException ex)
            //                        {
            //                            result = false;
            //                            transaction.Rollback();
            //                        }

            //                        finally
            //                        {
            //                            oconn.Close();
            //                            oconn.Dispose();
            //                        }

            //                        if(execution > 0)
            //                        {
            //                            lblAlertMessage.Text = "Claim payment detail was updated";
            //                            lblAlertMessage.Attributes.Add("data-alert-title", "Success");
            //                            lblAlertMessage.Attributes.Add("data-alert-message", lblAlertMessage.Text);
            //                            lblAlertMessage.Visible = true;
            //                        }



            //                    }
            //                    else
            //                    {
            //                        lblAlertMessage.Text = "Claim payment detail not updated";
            //                        lblAlertMessage.Attributes.Add("data-alert-title", "Alert");
            //                        lblAlertMessage.Attributes.Add("data-alert-message", lblAlertMessage.Text);
            //                        lblAlertMessage.Visible = true;
            //                    }
            //                }
            //                catch (Exception ex)
            //                {
            //                    throw ex;
            //                    //this.lblErrorMsg.Text = "No data found for this policy no";
            //                }

            //            }
            //end of update claim payment using API

            //update claimpayment by sql query
            int exe = 0, execution =0;
            string exe_Update2 = "update shedata.shhosinf00bkup set ALIMENT=:pin_aliment , EXNAME=:pin_execNam, TOTBIL=:pin_totBil, PDAMT=:pin_paiAmou, REMARK1=:pin_r1, REMARK2=:pin_r2, CLAIMNO=:pin_refNo, " +
                                "BHTNO =:pin_bhtno, BILLNO = :pin_bilno where claimref =:pin_climrefno";

            OracleTransaction transaction2 = null;
            oconn.Open();
            try
            {
                transaction2 = oconn.BeginTransaction();

                OracleCommand cmd_backup = oconn.CreateCommand();
                cmd_backup.CommandType = CommandType.Text;
                cmd_backup.CommandText = exe_Update2;

                //cmd_backup.Parameters.Add(":pin_jobSta", OracleType.VarChar).Value = jobStatus;
                cmd_backup.Parameters.Add(":pin_climrefno", OracleType.Int32).Value = claimRef;
                cmd_backup.Parameters.Add(":pin_aliment", OracleType.VarChar).Value = aliment;
                cmd_backup.Parameters.Add(":pin_execNam", OracleType.VarChar).Value = execName;
                cmd_backup.Parameters.Add(":pin_totBil", OracleType.Double).Value = totalBillAm;
                cmd_backup.Parameters.Add(":pin_paiAmou", OracleType.Double).Value = paidAmount;
                cmd_backup.Parameters.Add(":pin_r1", OracleType.VarChar).Value = remark1;
                cmd_backup.Parameters.Add(":pin_r2", OracleType.VarChar).Value = remark2;
                cmd_backup.Parameters.Add(":pin_refNo", OracleType.VarChar).Value = refNo;
                cmd_backup.Parameters.Add(":pin_bhtno", OracleType.VarChar).Value = bhtNo;
                cmd_backup.Parameters.Add(":pin_bilno", OracleType.VarChar).Value = billNo;

                cmd_backup.Transaction = transaction2;
                exe = cmd_backup.ExecuteNonQuery();

                transaction2.Commit();
            }

            catch (OracleException ex)
            {
                //result = false;
                transaction2.Rollback();
                lblAlertMessage.Text = "Some thing went to wrong";
                lblAlertMessage.Attributes.Add("data-alert-title", "Success");
                lblAlertMessage.Attributes.Add("data-alert-message", lblAlertMessage.Text);
                lblAlertMessage.Visible = true;
            }

            finally
            {
                oconn.Close();
                oconn.Dispose();
            }

            if (exe > 0)
            {
                //update job status table
                string exe_Update = "update shedata.cor_job_status set JOB_STATUS =:pin_jobSta WHERE CLAIMREF = :pin_cliRef";

                OracleTransaction transaction = null;
                OracleConnection oconn2 = new OracleConnection(ConfigurationManager.AppSettings["OracleDB"]);

                if (oconn2.State != ConnectionState.Open)
                {
                   
                    oconn2.Open();
                }

                //OracleCommand cmd = oconn.CreateCommand();
                //oconn.Open();
                try
                {
                    transaction = oconn2.BeginTransaction();

                    OracleCommand cmd_backup = oconn2.CreateCommand();
                    cmd_backup.CommandType = CommandType.Text;
                    cmd_backup.CommandText = exe_Update;

                    cmd_backup.Parameters.Add(":pin_jobSta", OracleType.VarChar).Value = jobStatus;
                    cmd_backup.Parameters.Add(":pin_cliRef", OracleType.VarChar).Value = claimRef;

                    cmd_backup.Transaction = transaction;
                    execution = cmd_backup.ExecuteNonQuery();

                    transaction.Commit();
                }

                catch (OracleException ex)
                {
                    //result = false;
                    transaction.Rollback();
                    lblAlertMessage.Text = "Some thing went to wrong";
                    lblAlertMessage.Attributes.Add("data-alert-title", "Success");
                    lblAlertMessage.Attributes.Add("data-alert-message", lblAlertMessage.Text);
                    lblAlertMessage.Visible = true;
                }

                finally
                {
                    oconn2.Close();
                    oconn2.Dispose();
                }

                if (execution > 0)
                {
                    lblAlertMessage.Text = "Claim payment detail was updated";
                    lblAlertMessage.Attributes.Add("data-alert-title", "Success");
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

            //end of sql update

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
            public string TotBil { get; set; }
            public string PDamt { get; set; }
            public string clamNo { get; set; }
        }

    }


}