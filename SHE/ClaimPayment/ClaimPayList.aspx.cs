using SHE.Code;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.Odbc;
using System.Data.OracleClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using static SHE.ClaimPayment.ClaimPaymentDetail;

namespace SHE.ClaimPayment
{
    public partial class ClaimPayList : System.Web.UI.Page
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
        string policy, epfno, claimRefNo, userName, eventLimit;

        private List<ClaimData> claimDataList = new List<ClaimData>();

        private ClaimData claimDataset;

        protected void Page_Load(object sender, EventArgs e)
        {
            userName = Session["LoggedUser"].ToString();
            policy = Request.QueryString["POLICYNO"];
            epfno = Request.QueryString["EPF"];
            //claimRefNo = Request.QueryString["CLAIMREF"];

            bool hasRows = false;


            policy = dc.Decrypt(policy);
            epfno = dc.Decrypt(epfno);
            //claimRefNo = dc.Decrypt(claimRefNo);
            if (!IsPostBack)
            {
                if (oconn.State != ConnectionState.Open)
                {
                    oconn.Open();
                }

                OracleCommand cmd = oconn.CreateCommand();

                try
                {
                    //use SHEDATA.SHHOSINF00BKUP or SHEDATA.SHHOSINF00 as your need
                    panel1.Visible = false;
                    datediv.Visible = true;
                    using (cmd)
                    {
                        string exe_Select_date = "SELECT m.claimref, m.pname, m.cname, m.cphone, g.hospital_name, m.roomno, m.adddate, m.pidno, m.disdate, m.epf, m.policy, m.remark1,m.remark2, c.job_status, m.totbil, m.pdamt, c.job_type, m.bhtno, m.billNo, c.CORDINATOR_USERID,  m.aliment,  m.fileclo " +
                                                  " from shedata.cor_job_status c" +
                                                  " INNER JOIN SHEDATA.SHHOSINF00 m  ON m.claimref = c.claimref" +
                                                  " INNER JOIN GENERAL_CLAIM.CLAIM_HOSPITAL_DETAILS g ON m.hospital = g.hospital_id" +
                                                  " WHERE m.epf = :pin_epf and m.policy = :pin_policy and c.cordinator_userid=:pin_log_user and c.job_type='D' and c.job_status='ACCEPTED' and (m.fileclo is null or m.fileclo='PN')";


                        cmd.CommandText = exe_Select_date;

                        cmd.Parameters.Add("pin_epf", OracleType.VarChar).Value = epfno;
                        cmd.Parameters.Add("pin_policy", OracleType.VarChar).Value = policy;
                        cmd.Parameters.Add("pin_log_user", OracleType.VarChar).Value = userName;

                        OracleDataReader reader = cmd.ExecuteReader();

                        if (reader.HasRows)
                        {

                            //newclaim.Visible = true;
                            //noClaims.Visible = false;
                            DataSet dsMemDetails = new DataSet();
                            DataTable dtb1 = new DataTable();

                            dtb1.Columns.Add("CLAIM_REF_NO", typeof(int));
                            dtb1.Columns.Add("PNAME", typeof(string));
                            dtb1.Columns.Add("HOSPITAL", typeof(string));


                            dsMemDetails.Tables.Add(dtb1);

                            while (reader.Read())
                            {
                                panel1.Visible = true;

                                int clmref = int.Parse(reader["claimref"].ToString());
                                string pname = reader["pname"].ToString();
                                string hospital = reader["hospital_name"].ToString();



                                dsMemDetails.Tables[0].Rows.Add(clmref, pname, hospital);
                                this.GridView2.DataSource = dsMemDetails.Tables[0];
                                this.GridView2.DataBind();

                                //this.GridView3.DataSource = dsMemDetails.Tables[0];
                                // this.GridView3.DataBind();

                                //List<YourDataModel> dataList = GetDataFromDatabase(); // Assuming you have a method to retrieve data from the database
                                //GridView1.DataSource = dataList;
                                //GridView1.DataBind();
                                hasRows = true;
                            }
                        }
                        else
                        {
                            hasRows = false;
                            Response.Redirect("~/ClaimPayment/ClaimSearch.aspx?alert=PolicyNo+and+employeeNo+dose+not+match");

                            //panel2.Visible = false;
                            //noClaims.Visible = true;
                            //newclaim.Visible = true;
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
            }
        }


        protected void GridView2_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            // Check if the current row is a data row
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                // Apply alternate row color based on row index
                if (e.Row.RowIndex % 2 == 0)
                {
                    e.Row.CssClass = "row-color-even"; // Apply even row color
                }
                else
                {
                    e.Row.CssClass = "row-color-odd"; // Apply odd row color
                }
            }

        }

        protected void GridView2_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            EncryptDecrypt dcn = new EncryptDecrypt();

            if (e.CommandName == "ViewRow")
            {



                int index = Convert.ToInt32(e.CommandArgument);

                string claimRefNo = GridView2.DataKeys[index].Values["CLAIM_REF_NO"].ToString();
                string drefe = dcn.Encrypt(claimRefNo);


                EncryptDecrypt dc = new EncryptDecrypt();
                Response.Redirect("~/ClaimPayment/ClaimPaymentDetail.aspx?POLICYNO=" + dc.Encrypt(policy) + "&EPF=" + dc.Encrypt(epfno) + "&CLAIMREF=" + dc.Encrypt(claimRefNo) );

            }
        }

        protected void backbutton_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/ClaimPayment/ClaimSearch.aspx");
        }

    }
}