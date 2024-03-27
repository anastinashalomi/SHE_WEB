﻿using SHE.Code;
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
    public partial class inquiry2 : System.Web.UI.Page
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
            string referenceno = (string)Request.QueryString["reference"];
            string fromdate = (string)Request.QueryString["fromdate"];
            string todate = (string)Request.QueryString["todate"];
            referenceno = dc.Decrypt(referenceno);
            fromdate = dc.Decrypt(fromdate);
            todate = dc.Decrypt(todate);

            if (fromdate != "" && todate != "" && referenceno == "")
            {


                DateTime date = DateTime.Parse(fromdate);
                string formatfromdate = date.ToString("yyyyMMdd");



                DateTime date2 = DateTime.Parse(todate);
                string formattodate = date2.ToString("yyyyMMdd");

                bool msg = this.LoadDataIntoGrid(formatfromdate, formattodate);

                gridview2update.Update();
                datediv.Visible = true;
                label1.InnerText = fromdate;
                label2.InnerText = todate;
            }
            else if (todate == "" && referenceno == "")
            {


                DateTime date = DateTime.Parse(fromdate);
                string formatfromdate = date.ToString("yyyyMMdd");
                bool msg = this.LoadDataIntoGrid2(formatfromdate);

                gridview2update.Update();
                datediv.Visible = true;
                label1.InnerText = fromdate;
                label2.InnerText = "Present";
            }
            else if (fromdate != "" && todate != "" && referenceno != "")
            {

                DateTime date = DateTime.Parse(fromdate);
                string formatfromdate = date.ToString("yyyyMMdd");


                DateTime date2 = DateTime.Parse(todate);
                string formattodate = date2.ToString("yyyyMMdd");


                int.TryParse(referenceno, out int formatclmno);
                bool msg = this.LoadDataIntoGrid3(formatfromdate, formattodate, formatclmno);

                gridview2update.Update();
                dateandref.Visible = true;
                label4.InnerText = fromdate;
                label5.InnerText = todate;
                label6.InnerText = referenceno;

            }
            else if (fromdate == "" && todate == "" && referenceno != "")
            {
                int.TryParse(referenceno, out int formatclmno);
                bool msg = this.LoadDataIntoGrid4(formatclmno);

                gridview2update.Update();
                reference.Visible = true;
                label3.InnerText = referenceno;

            }

            else if (todate == "")
            {
                DateTime date = DateTime.Parse(fromdate);
                string formatfromdate = date.ToString("yyyyMMdd");

                int.TryParse(referenceno, out int formatclmno);


                bool msg = this.LoadDataIntoGrid5(formatfromdate, formatclmno);

                gridview2update.Update();
                fromandref.Visible = true;
                label7.InnerText = fromdate;
                label8.InnerText = referenceno;

            }

            else
            {
                DateTime date = DateTime.Parse(fromdate);
                string formatfromdate = date.ToString("yyyyMMdd");

                int.TryParse(referenceno, out int formatclmno);


                bool msg = this.LoadDataIntoGrid5(formatfromdate, formatclmno);

                gridview2update.Update();
                fromandref.Visible = true;
                label7.InnerText = fromdate;
                label8.InnerText = referenceno;

            }

        }

        public bool LoadDataIntoGrid(string fromdate, string todate)
        {
            bool hasRows = false;

            if (oconn.State != ConnectionState.Open)
            {
                oconn.Open();
            }

            OracleCommand cmd = oconn.CreateCommand();

            try
            {
                using (cmd)
                {
                    string exe_Select_date = "SELECT SH.CLAIMREF, SH.PNAME, SH.ROOMNO, SH.CPHONE, SH.ADDDATE, SH.EPF, SH.POLICY, SH.CNAME, SH.SHADDUSR, SH.SHADDDAT, SH.SHADDTIM, SH.SHUPDDAT, SH.SHUPDTIM, SH.CIPERSON, SH.CIADDUSR, SH.CIADDDAT, SH.CIADDTIM, SH.DIPERSON, CHD.HOSPITAL_NAME " +
                                             "FROM SHEDATA.SHHOSINF00 SH, GENERAL_CLAIM.CLAIM_HOSPITAL_DETAILS CHD " +
                                             "WHERE SH.HOSPITAL = CHD.HOSPITAL_ID AND (ADDDATE BETWEEN :StartDate AND :EndDate " +
                                             "OR DISDATE BETWEEN :StartDate AND :EndDate)";



                    cmd.CommandText = exe_Select_date;
                    cmd.Parameters.AddWithValue("StartDate", fromdate);
                    cmd.Parameters.AddWithValue("EndDate", todate);


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
                        dtb1.Columns.Add("ROOM_NO", typeof(string));
                        dtb1.Columns.Add("CON_NO", typeof(string));
                        dtb1.Columns.Add("ADD_DATE", typeof(string));
                        dtb1.Columns.Add("EMP_NO", typeof(string));
                        dtb1.Columns.Add("POL_NO", typeof(string));
                        dtb1.Columns.Add("CALLER_NAME", typeof(string));
                        dtb1.Columns.Add("USER_ADD", typeof(string));
                        dtb1.Columns.Add("DIS_DATE", typeof(string));
                        dtb1.Columns.Add("REC_ADD_TIME", typeof(string));
                        dtb1.Columns.Add("UPDATE_DATE_DIS", typeof(string));
                        dtb1.Columns.Add("REC_UPDATE_TIME", typeof(string));
                        dtb1.Columns.Add("COORDINATOR_ADD", typeof(string));
                        dtb1.Columns.Add("USR_ADMIN_UPDATE", typeof(string));
                        dtb1.Columns.Add("DATE_ADMIN_UPDATE", typeof(string));
                        dtb1.Columns.Add("TIME_ADMIN_UPDATE", typeof(string));
                        dtb1.Columns.Add("COORDINATOR_DIS", typeof(string));

                        dsMemDetails.Tables.Add(dtb1);

                        while (reader.Read())
                        {
                            int clmref = int.Parse(reader["CLAIMREF"].ToString());
                            string pname = reader["PNAME"].ToString();
                            string hospital = reader["HOSPITAL_NAME"].ToString();
                            string roomno = reader["ROOMNO"].ToString();
                            string phone = reader["CPHONE"].ToString();

                            string adddateT = reader["ADDDATE"].ToString();
                            string year1 = adddateT.Substring(0, 4);
                            string month1 = adddateT.Substring(4, 2);
                            string day1 = adddateT.Substring(6, 2);
                            string adddate = $"{year1}/{month1}/{day1}";


                            string epf = reader["EPF"].ToString();
                            string policy = reader["POLICY"].ToString();
                            string cname = reader["CNAME"].ToString();
                            string user = reader["SHADDUSR"].ToString();

                            string disdateT = reader["SHADDDAT"].ToString();
                            string year2 = disdateT.Substring(0, 4);
                            string month2 = disdateT.Substring(4, 2);
                            string day2 = disdateT.Substring(6, 2);
                            string disdate = $"{year2}/{month2}/{day2}";

                            string distime = reader["SHADDTIM"].ToString();
                            string update = reader["SHUPDDAT"].ToString();
                            string uptime = reader["SHUPDTIM"].ToString();
                            string coordinator = reader["CIPERSON"].ToString();
                            string ciadd = reader["CIADDUSR"].ToString();
                            string cidate = reader["CIADDDAT"].ToString();
                            string citime = reader["CIADDTIM"].ToString();
                            string di_coordinator = reader["DIPERSON"].ToString();

                            dsMemDetails.Tables[0].Rows.Add(clmref, pname, hospital, roomno, phone, adddate, epf, policy, cname, user, disdate, distime, update, uptime, coordinator, ciadd, cidate, citime, di_coordinator);
                            this.GridView2.DataSource = dsMemDetails.Tables[0];
                            this.GridView2.DataBind();
                            hasRows = true;
                        }
                    }
                    else
                    {
                        //noClaims.Visible = true;
                        //newclaim.Visible = true;
                    }
                }
            }
            catch (Exception e)
            {
                string msgs = "ERROR:" + e.Message;
                Console.WriteLine(cmd.CommandText);
                foreach (OracleParameter p in cmd.Parameters)
                {
                    Console.WriteLine(p.ParameterName + " = " + p.Value);
                }

                Console.WriteLine(e.Message);
            }
            finally
            {
                if (oconn.State == ConnectionState.Open)
                {
                    oconn.Close();
                }
            }

            return hasRows;
        }

        public bool LoadDataIntoGrid2(string fromdate)
        {
            bool hasRows = false;

            if (oconn.State != ConnectionState.Open)
            {
                oconn.Open();
            }

            OracleCommand cmd = oconn.CreateCommand();

            try
            {
                using (cmd)
                {
                    string exe_Select_date = "SELECT SH.CLAIMREF, SH.PNAME, SH.ROOMNO, SH.CPHONE, SH.ADDDATE, SH.EPF, SH.POLICY, SH.CNAME, SH.SHADDUSR, SH.SHADDDAT, SH.SHADDTIM, SH.SHUPDDAT, SH.SHUPDTIM, SH.CIPERSON, SH.CIADDUSR, SH.CIADDDAT, SH.CIADDTIM, SH.DIPERSON, CHD.HOSPITAL_NAME " +
                                             "FROM SHEDATA.SHHOSINF00 SH, GENERAL_CLAIM.CLAIM_HOSPITAL_DETAILS CHD " +
                                             "WHERE SH.HOSPITAL = CHD.HOSPITAL_ID AND (ADDDATE BETWEEN :StartDate AND :EndDate " +
                                             "OR DISDATE BETWEEN :StartDate AND :EndDate)";


                    hiddenEndDate.Value = DateTime.Today.ToString("yyyyMMdd");
                    cmd.CommandText = exe_Select_date;
                    cmd.Parameters.AddWithValue("StartDate", fromdate);
                    cmd.Parameters.AddWithValue("EndDate", hiddenEndDate.Value);



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
                        dtb1.Columns.Add("ROOM_NO", typeof(string));
                        dtb1.Columns.Add("CON_NO", typeof(string));
                        dtb1.Columns.Add("ADD_DATE", typeof(string));
                        dtb1.Columns.Add("EMP_NO", typeof(string));
                        dtb1.Columns.Add("POL_NO", typeof(string));
                        dtb1.Columns.Add("CALLER_NAME", typeof(string));
                        dtb1.Columns.Add("USER_ADD", typeof(string));
                        dtb1.Columns.Add("DIS_DATE", typeof(string));
                        dtb1.Columns.Add("REC_ADD_TIME", typeof(string));
                        dtb1.Columns.Add("UPDATE_DATE_DIS", typeof(string));
                        dtb1.Columns.Add("REC_UPDATE_TIME", typeof(string));
                        dtb1.Columns.Add("COORDINATOR_ADD", typeof(string));
                        dtb1.Columns.Add("USR_ADMIN_UPDATE", typeof(string));
                        dtb1.Columns.Add("DATE_ADMIN_UPDATE", typeof(string));
                        dtb1.Columns.Add("TIME_ADMIN_UPDATE", typeof(string));
                        dtb1.Columns.Add("COORDINATOR_DIS", typeof(string));

                        dsMemDetails.Tables.Add(dtb1);

                        while (reader.Read())
                        {
                            int clmref = int.Parse(reader["CLAIMREF"].ToString());
                            string pname = reader["PNAME"].ToString();
                            string hospital = reader["HOSPITAL_NAME"].ToString();
                            string roomno = reader["ROOMNO"].ToString();
                            string phone = reader["CPHONE"].ToString();

                            //string adddate = reader["ADDDATE"].ToString();
                            string adddateT = reader["ADDDATE"].ToString();
                            string year1 = adddateT.Substring(0, 4);
                            string month1 = adddateT.Substring(4, 2);
                            string day1 = adddateT.Substring(6, 2);
                            string adddate = $"{year1}/{month1}/{day1}";

                            string epf = reader["EPF"].ToString();
                            string policy = reader["POLICY"].ToString();
                            string cname = reader["CNAME"].ToString();
                            string user = reader["SHADDUSR"].ToString();

                            //string disdate = reader["SHADDDAT"].ToString();
                            string disdateT = reader["SHADDDAT"].ToString();
                            string year2 = disdateT.Substring(0, 4);
                            string month2 = disdateT.Substring(4, 2);
                            string day2 = disdateT.Substring(6, 2);
                            string disdate = $"{year2}/{month2}/{day2}";

                            string distime = reader["SHADDTIM"].ToString();
                            string update = reader["SHUPDDAT"].ToString();
                            string uptime = reader["SHUPDTIM"].ToString();
                            string coordinator = reader["CIPERSON"].ToString();
                            string ciadd = reader["CIADDUSR"].ToString();
                            string cidate = reader["CIADDDAT"].ToString();
                            string citime = reader["CIADDTIM"].ToString();
                            string di_coordinator = reader["DIPERSON"].ToString();

                            dsMemDetails.Tables[0].Rows.Add(clmref, pname, hospital, roomno, phone, adddate, epf, policy, cname, user, disdate, distime, update, uptime, coordinator, ciadd, cidate, citime, di_coordinator);
                            this.GridView2.DataSource = dsMemDetails.Tables[0];
                            this.GridView2.DataBind();
                            hasRows = true;
                        }
                    }
                    else
                    {
                        //noClaims.Visible = true;
                        //newclaim.Visible = true;
                    }
                }
            }
            catch (Exception e)
            {
                string msgs = "ERROR:" + e.Message;
                Console.WriteLine(cmd.CommandText);
                foreach (OracleParameter p in cmd.Parameters)
                {
                    Console.WriteLine(p.ParameterName + " = " + p.Value);
                }

                Console.WriteLine(e.Message);
            }
            finally
            {
                if (oconn.State == ConnectionState.Open)
                {
                    oconn.Close();
                }
            }

            return hasRows;
        }

        public bool LoadDataIntoGrid3(string fromdate, string todate, int claim_ref)
        {
            bool hasRows = false;

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
                                             "WHERE SH.HOSPITAL = CHD.HOSPITAL_ID AND  (CLAIMREF = :clmno) AND " +
                                             "(ADDDATE BETWEEN :StartDate AND :EndDate OR DISDATE BETWEEN :StartDate AND :EndDate) ";



                    cmd.CommandText = exe_Select_date;
                    cmd.Parameters.AddWithValue("StartDate", fromdate);
                    cmd.Parameters.AddWithValue("EndDate", todate);
                    cmd.Parameters.AddWithValue("clmno", claim_ref);


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
                        dtb1.Columns.Add("ROOM_NO", typeof(string));
                        dtb1.Columns.Add("CON_NO", typeof(string));
                        dtb1.Columns.Add("ADD_DATE", typeof(string));
                        dtb1.Columns.Add("EMP_NO", typeof(string));
                        dtb1.Columns.Add("POL_NO", typeof(string));
                        dtb1.Columns.Add("CALLER_NAME", typeof(string));
                        dtb1.Columns.Add("USER_ADD", typeof(string));
                        dtb1.Columns.Add("DIS_DATE", typeof(string));
                        dtb1.Columns.Add("REC_ADD_TIME", typeof(string));
                        dtb1.Columns.Add("UPDATE_DATE_DIS", typeof(string));
                        dtb1.Columns.Add("REC_UPDATE_TIME", typeof(string));
                        dtb1.Columns.Add("COORDINATOR_ADD", typeof(string));
                        dtb1.Columns.Add("USR_ADMIN_UPDATE", typeof(string));
                        dtb1.Columns.Add("DATE_ADMIN_UPDATE", typeof(string));
                        dtb1.Columns.Add("TIME_ADMIN_UPDATE", typeof(string));
                        dtb1.Columns.Add("COORDINATOR_DIS", typeof(string));

                        dsMemDetails.Tables.Add(dtb1);

                        while (reader.Read())
                        {
                            int clmref = int.Parse(reader["CLAIMREF"].ToString());
                            string pname = reader["PNAME"].ToString();
                            string hospital = reader["HOSPITAL_NAME"].ToString();
                            string roomno = reader["ROOMNO"].ToString();
                            string phone = reader["CPHONE"].ToString();

                            //string adddate = reader["ADDDATE"].ToString();
                            string adddateT = reader["ADDDATE"].ToString();
                            string year1 = adddateT.Substring(0, 4);
                            string month1 = adddateT.Substring(4, 2);
                            string day1 = adddateT.Substring(6, 2);
                            string adddate = $"{year1}/{month1}/{day1}";


                            string epf = reader["EPF"].ToString();
                            string policy = reader["POLICY"].ToString();
                            string cname = reader["CNAME"].ToString();
                            string user = reader["SHADDUSR"].ToString();

                            //string disdate = reader["SHADDDAT"].ToString();
                            string disdateT = reader["SHADDDAT"].ToString();
                            string year2 = disdateT.Substring(0, 4);
                            string month2 = disdateT.Substring(4, 2);
                            string day2 = disdateT.Substring(6, 2);
                            string disdate = $"{year2}/{month2}/{day2}";


                            string distime = reader["SHADDTIM"].ToString();
                            string update = reader["SHUPDDAT"].ToString();
                            string uptime = reader["SHUPDTIM"].ToString();
                            string coordinator = reader["CIPERSON"].ToString();
                            string ciadd = reader["CIADDUSR"].ToString();
                            string cidate = reader["CIADDDAT"].ToString();
                            string citime = reader["CIADDTIM"].ToString();
                            string di_coordinator = reader["DIPERSON"].ToString();

                            dsMemDetails.Tables[0].Rows.Add(clmref, pname, hospital, roomno, phone, adddate, epf, policy, cname, user, disdate, distime, update, uptime, coordinator, ciadd, cidate, citime, di_coordinator);
                            this.GridView2.DataSource = dsMemDetails.Tables[0];
                            this.GridView2.DataBind();
                            hasRows = true;
                        }
                    }
                    else
                    {
                        //noClaims.Visible = true;
                        //newclaim.Visible = true;
                    }
                }
            }
            catch (Exception e)
            {
                string msgs = "ERROR:" + e.Message;
                Console.WriteLine(cmd.CommandText);
                foreach (OracleParameter p in cmd.Parameters)
                {
                    Console.WriteLine(p.ParameterName + " = " + p.Value);
                }

                Console.WriteLine(e.Message);
            }
            finally
            {
                if (oconn.State == ConnectionState.Open)
                {
                    oconn.Close();
                }
            }

            return hasRows;
        }

        public bool LoadDataIntoGrid4(int claim_ref)
        {
            bool hasRows = false;

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
                                             "WHERE SH.HOSPITAL = CHD.HOSPITAL_ID AND  (CLAIMREF = :clmno)";



                    cmd.CommandText = exe_Select_date;

                    cmd.Parameters.AddWithValue("clmno", claim_ref);


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
                        dtb1.Columns.Add("ROOM_NO", typeof(string));
                        dtb1.Columns.Add("CON_NO", typeof(string));
                        dtb1.Columns.Add("ADD_DATE", typeof(string));
                        dtb1.Columns.Add("EMP_NO", typeof(string));
                        dtb1.Columns.Add("POL_NO", typeof(string));
                        dtb1.Columns.Add("CALLER_NAME", typeof(string));
                        dtb1.Columns.Add("USER_ADD", typeof(string));
                        dtb1.Columns.Add("DIS_DATE", typeof(string));
                        dtb1.Columns.Add("REC_ADD_TIME", typeof(string));
                        dtb1.Columns.Add("UPDATE_DATE_DIS", typeof(string));
                        dtb1.Columns.Add("REC_UPDATE_TIME", typeof(string));
                        dtb1.Columns.Add("COORDINATOR_ADD", typeof(string));
                        dtb1.Columns.Add("USR_ADMIN_UPDATE", typeof(string));
                        dtb1.Columns.Add("DATE_ADMIN_UPDATE", typeof(string));
                        dtb1.Columns.Add("TIME_ADMIN_UPDATE", typeof(string));
                        dtb1.Columns.Add("COORDINATOR_DIS", typeof(string));

                        dsMemDetails.Tables.Add(dtb1);

                        while (reader.Read())
                        {
                            int clmref = int.Parse(reader["CLAIMREF"].ToString());
                            string pname = reader["PNAME"].ToString();
                            string hospital = reader["HOSPITAL_NAME"].ToString();
                            string roomno = reader["ROOMNO"].ToString();
                            string phone = reader["CPHONE"].ToString();

                            //string adddate = reader["ADDDATE"].ToString();
                            string adddateT = reader["ADDDATE"].ToString();
                            string year1 = adddateT.Substring(0, 4);
                            string month1 = adddateT.Substring(4, 2);
                            string day1 = adddateT.Substring(6, 2);
                            string adddate = $"{year1}/{month1}/{day1}";

                            string epf = reader["EPF"].ToString();
                            string policy = reader["POLICY"].ToString();
                            string cname = reader["CNAME"].ToString();
                            string user = reader["SHADDUSR"].ToString();

                            //string disdate = reader["SHADDDAT"].ToString();
                            string disdateT = reader["SHADDDAT"].ToString();
                            string year2 = disdateT.Substring(0, 4);
                            string month2 = disdateT.Substring(4, 2);
                            string day2 = disdateT.Substring(6, 2);
                            string disdate = $"{year2}/{month2}/{day2}";

                            string distime = reader["SHADDTIM"].ToString();
                            string update = reader["SHUPDDAT"].ToString();
                            string uptime = reader["SHUPDTIM"].ToString();
                            string coordinator = reader["CIPERSON"].ToString();
                            string ciadd = reader["CIADDUSR"].ToString();
                            string cidate = reader["CIADDDAT"].ToString();
                            string citime = reader["CIADDTIM"].ToString();
                            string di_coordinator = reader["DIPERSON"].ToString();

                            dsMemDetails.Tables[0].Rows.Add(clmref, pname, hospital, roomno, phone, adddate, epf, policy, cname, user, disdate, distime, update, uptime, coordinator, ciadd, cidate, citime, di_coordinator);
                            this.GridView2.DataSource = dsMemDetails.Tables[0];
                            this.GridView2.DataBind();
                            hasRows = true;
                        }
                    }
                    else
                    {
                        //noClaims.Visible = true;
                        //newclaim.Visible = true;
                    }
                }
            }
            catch (Exception e)
            {
                string msgs = "ERROR:" + e.Message;
                Console.WriteLine(cmd.CommandText);
                foreach (OracleParameter p in cmd.Parameters)
                {
                    Console.WriteLine(p.ParameterName + " = " + p.Value);
                }

                Console.WriteLine(e.Message);
            }
            finally
            {
                if (oconn.State == ConnectionState.Open)
                {
                    oconn.Close();
                }
            }

            return hasRows;
        }

        public bool LoadDataIntoGrid5(string fromdate, int claim_ref)
        {
            bool hasRows = false;

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
                                             "WHERE SH.HOSPITAL = CHD.HOSPITAL_ID AND  (CLAIMREF = :clmno) AND " +
                                             "(ADDDATE BETWEEN :StartDate AND :EndDate OR DISDATE BETWEEN :StartDate AND :EndDate) ";



                    cmd.CommandText = exe_Select_date;
                    hiddenEndDate.Value = DateTime.Today.ToString("yyyyMMdd");
                    cmd.Parameters.AddWithValue("StartDate", fromdate);
                    cmd.Parameters.AddWithValue("clmno", claim_ref);
                    cmd.Parameters.AddWithValue("EndDate", hiddenEndDate.Value);

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
                        dtb1.Columns.Add("ROOM_NO", typeof(string));
                        dtb1.Columns.Add("CON_NO", typeof(string));
                        dtb1.Columns.Add("ADD_DATE", typeof(string));
                        dtb1.Columns.Add("EMP_NO", typeof(string));
                        dtb1.Columns.Add("POL_NO", typeof(string));
                        dtb1.Columns.Add("CALLER_NAME", typeof(string));
                        dtb1.Columns.Add("USER_ADD", typeof(string));
                        dtb1.Columns.Add("DIS_DATE", typeof(string));
                        dtb1.Columns.Add("REC_ADD_TIME", typeof(string));
                        dtb1.Columns.Add("UPDATE_DATE_DIS", typeof(string));
                        dtb1.Columns.Add("REC_UPDATE_TIME", typeof(string));
                        dtb1.Columns.Add("COORDINATOR_ADD", typeof(string));
                        dtb1.Columns.Add("USR_ADMIN_UPDATE", typeof(string));
                        dtb1.Columns.Add("DATE_ADMIN_UPDATE", typeof(string));
                        dtb1.Columns.Add("TIME_ADMIN_UPDATE", typeof(string));
                        dtb1.Columns.Add("COORDINATOR_DIS", typeof(string));

                        dsMemDetails.Tables.Add(dtb1);

                        while (reader.Read())
                        {
                            int clmref = int.Parse(reader["CLAIMREF"].ToString());
                            string pname = reader["PNAME"].ToString();
                            string hospital = reader["HOSPITAL_NAME"].ToString();
                            string roomno = reader["ROOMNO"].ToString();
                            string phone = reader["CPHONE"].ToString();

                            //string adddate = reader["ADDDATE"].ToString();
                            string adddateT = reader["ADDDATE"].ToString();
                            string year1 = adddateT.Substring(0, 4);
                            string month1 = adddateT.Substring(4, 2);
                            string day1 = adddateT.Substring(6, 2);
                            string adddate = $"{year1}/{month1}/{day1}";

                            string epf = reader["EPF"].ToString();
                            string policy = reader["POLICY"].ToString();
                            string cname = reader["CNAME"].ToString();
                            string user = reader["SHADDUSR"].ToString();

                            //string disdate = reader["SHADDDAT"].ToString();
                            string disdateT = reader["SHADDDAT"].ToString();
                            string year2 = disdateT.Substring(0, 4);
                            string month2 = disdateT.Substring(4, 2);
                            string day2 = disdateT.Substring(6, 2);
                            string disdate = $"{year2}/{month2}/{day2}";

                            string distime = reader["SHADDTIM"].ToString();
                            string update = reader["SHUPDDAT"].ToString();
                            string uptime = reader["SHUPDTIM"].ToString();
                            string coordinator = reader["CIPERSON"].ToString();
                            string ciadd = reader["CIADDUSR"].ToString();
                            string cidate = reader["CIADDDAT"].ToString();
                            string citime = reader["CIADDTIM"].ToString();
                            string di_coordinator = reader["DIPERSON"].ToString();

                            dsMemDetails.Tables[0].Rows.Add(clmref, pname, hospital, roomno, phone, adddate, epf, policy, cname, user, disdate, distime, update, uptime, coordinator, ciadd, cidate, citime, di_coordinator);
                            this.GridView2.DataSource = dsMemDetails.Tables[0];
                            this.GridView2.DataBind();
                            hasRows = true;
                        }
                    }
                    else
                    {
                        //noClaims.Visible = true;
                        //newclaim.Visible = true;
                    }
                }
            }
            catch (Exception e)
            {
                string msgs = "ERROR:" + e.Message;
                Console.WriteLine(cmd.CommandText);
                foreach (OracleParameter p in cmd.Parameters)
                {
                    Console.WriteLine(p.ParameterName + " = " + p.Value);
                }

                Console.WriteLine(e.Message);
            }
            finally
            {
                if (oconn.State == ConnectionState.Open)
                {
                    oconn.Close();
                }
            }

            return hasRows;
        }

        protected void backbutton_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/Inquiry/inquiry1.aspx");
        }

    }
}