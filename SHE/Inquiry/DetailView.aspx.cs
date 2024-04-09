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

namespace SHE.Inquiry
{
    public partial class DetailView : System.Web.UI.Page
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

        protected void Page_Load(object sender, EventArgs e)
        {
            string referenceno = Request.QueryString["referenceNo"];
            string fromdate = (string)Request.QueryString["fromdate"];
            string todate = (string)Request.QueryString["todate"];

            fromdate = dc.Decrypt(fromdate);
            todate = dc.Decrypt(todate);
            referenceno = dc.Decrypt(referenceno);

            if (oconn.State != ConnectionState.Open)
            {
                oconn.Open();
            }

            OracleCommand cmd = oconn.CreateCommand();

            try
            {
                using (cmd)
                {
                    string exe_Select_date = "SELECT SH.CLAIMREF, SH.PNAME, SH.ROOMNO, SH.CPHONE, SH.ADDDATE, SH.EPF, SH.POLICY, SH.CNAME, SH.SHADDUSR, " +
                                             "SH.SHADDDAT, SH.SHADDTIM, SH.SHUPDDAT, SH.SHUPDTIM, SH.CIPERSON, SH.CIADDUSR, SH.CIADDDAT, SH.CIADDTIM, SH.DIPERSON, CHD.HOSPITAL_NAME " +
                                             "FROM SHEDATA.SHHOSINF00 SH, GENERAL_CLAIM.CLAIM_HOSPITAL_DETAILS CHD " +
                                             "WHERE SH.HOSPITAL = CHD.HOSPITAL_ID AND  (CLAIMREF = :clmno) ";


                    cmd.CommandText = exe_Select_date;
                    cmd.Parameters.AddWithValue("clmno", referenceno);

                    OracleDataReader reader = cmd.ExecuteReader();

                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            int clmref = int.Parse(reader["CLAIMREF"].ToString());
                            referenceNo.InnerText = (reader["CLAIMREF"].ToString());
                            name.InnerText = reader["PNAME"].ToString();
                            hospitalName.InnerText = reader["HOSPITAL_NAME"].ToString();
                            roomNo.InnerText = reader["ROOMNO"].ToString();
                            admittedDate.InnerText = reader["ADDDATE"].ToString();
                            contactNo.InnerText = reader["CPHONE"].ToString();
                            dischargeDate.InnerText = reader["SHADDDAT"].ToString();
                            employeeNo.InnerText = reader["EPF"].ToString();
                            policyNo.InnerText = reader["POLICY"].ToString();

                        }
                    }
                    else
                    {
                        //panel1.Visible = true;
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


        protected void back_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/Inquiry/inquiry1.aspx");
        }

        protected void menu_Click(object sender, EventArgs e)
        {
            string fromDate = (string)Request.QueryString["fromdate"];
            string toDate = (string)Request.QueryString["todate"];
            string referenceno1 = (string)Request.QueryString["referenceNo"];
            string dRefno = dc.Decrypt(referenceno1);

            Response.Redirect("~/Inquiry/HospitalizeList.aspx?fromdate=" + fromDate + "&todate=" + toDate + "&reference=" + referenceno1);

        }

        protected void history_Click(object sender, EventArgs e)
        {
            string policy = dc.Encrypt(policyNo.InnerText.ToString());
            string epfno = dc.Encrypt(employeeNo.InnerText.ToString());
            Response.Redirect("~/Claim_History/claimhistory1_Redirect.aspx?policy=" + policy + "&epf=" + epfno);
            
        }


    }
}