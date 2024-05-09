using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.Odbc;
using System.Data.OracleClient;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Newtonsoft.Json;
using SHE.Code;
using System.Web.Script.Serialization;

namespace SHE.Claim_History
{
    public partial class claimhist2 : System.Web.UI.Page
    {

        EncryptDecrypt dc = new EncryptDecrypt();
        Reg_Policy_Mast polObj = new Reg_Policy_Mast();
        private const string host_ip = "http://172.24.90.100:8084";
        OdbcConnection db2conn = new OdbcConnection(ConfigurationManager.AppSettings["DB2"]);
        OracleConnection oconn = new OracleConnection(ConfigurationManager.AppSettings["OracleDB"]);
        OracleConnection oconnLife = new OracleConnection(ConfigurationManager.AppSettings["OraLifeDB"]);
        int TotalPaid = 0;
        int FinalTotalPaid = 0;
        ///Panel1
        protected void IconClick_ServerClick(object sender, EventArgs e)
        {
            mainpanel1.Visible = true;
            mainpanel2.Visible = false;
            mainpanel3.Visible = false;
            mainpanel4.Visible = false;
        }
        protected void Page_Load(object sender, EventArgs e)
        {

            if (!IsPostBack)
            {

                string epfno = Request.QueryString["EPF"];
                string policy = Request.QueryString["POLICYNO"];



                ///employeeDetails

                SHEEmployeeDataStr empDetails = this.get_she_employee_details(dc.Decrypt(policy), dc.Decrypt(epfno));

                if (empDetails != null)
                {

                    label5.InnerText = dc.Decrypt(epfno);
                    label4.InnerText = dc.Decrypt(policy);
                    dob.InnerText = empDetails.DOB;
                    empCate.InnerText = empDetails.EMPLOYEECATEGORY;
                    //if (empDetails.Count > 0)
                    if (empDetails != null && empDetails.MEMBERNAME != null)
                    {
                        label6.InnerText = empDetails.MEMBERNAME;
                    }

                    //string ActiveStatus = getCompanyActiveStatus(dc.Decrypt(policy));
                    //label7.InnerText = ActiveStatus;


                    ///ActiveStatus
                    ///Policy Period and Type

                    IList<PolicyDetails> PolInfo = this.GetPolicyDetails(dc.Decrypt(policy));
                    DataSet PolDetails = new DataSet();
                    if (PolInfo.Count > 0)
                    {
                        for (int i = 0; i < PolInfo.Count; i++)
                        {

                            label31.InnerText = PolInfo[i].PolicyPeriod.ToString();
                            label32.InnerText = PolInfo[i].PolicyType.ToString();
                            label7.InnerText = PolInfo[i].ClaimRemark.ToString();
                            lblPoNo.InnerText = dc.Decrypt(policy);
                            lblPeri.InnerText = PolInfo[i].PolicyPeriod.ToString();
                        }

                    }

                    ///Renewal Date and Company Name

                    List<SHEPolicyData> polDetailsList = this.get_she_policy_details(dc.Decrypt(policy));
                    foreach (SHEPolicyData polDetails in polDetailsList)
                    {
                        // Assuming you have retrieved these values from the polDetails object

                        DateTime renewalDate = polDetails.RENEWALDATE;
                        DateTime endDate = polDetails.ENDDATE;
                        string companyName = polDetails.Name;

                        string jsonBeforeDeserialization = polDetails.Data;

                        // Assign data to labels

                        label2.InnerText = renewalDate.ToString("yyyy-MM-dd"); // Convert to string as per your format
                        label1.InnerText = companyName;
                        lblComNam.InnerText = companyName;
                        lblReDate.InnerText = renewalDate.ToString();
                        lblEffeDa.InnerText = endDate.ToString();

                    }


                    //added by shalomi 2024/05/08 for find unique policies ex: kirula

                    List<UniquePolicyData> uniPolDetails = this.get_unique_pol_det(dc.Decrypt(policy));

                    if (uniPolDetails.Count > 0)
                    {
                        foreach (UniquePolicyData polDetails in uniPolDetails)
                        {
                            if (polDetails.POLICYTAG == "KIRULA")
                            {
                                List<kirulaPolicyData> kirulaPolDetails = this.get_kirula_pol_det(dc.Decrypt(policy), dc.Decrypt(epfno));

                                foreach (kirulaPolicyData kirulaDetail in kirulaPolDetails)
                                {
                                    if (kirulaDetail != null)
                                    {
                                        totPre.InnerHtml = kirulaDetail.MON_PREMIUM;
                                    }
                                }


                            }
                            else
                            {

                            }

                        }
                    }

                    //end of get unique policies  using API by shalomi 2024/05/08



                    ///dependentDetails


                    IList<SHEDependentDataStr> depDetails = this.get_she_dependant_details(dc.Decrypt(policy), dc.Decrypt(epfno));
                    DataSet dsDepDetails = new DataSet();
                    DataTable dtb1 = new DataTable();
                    dtb1.Columns.Add("DEPENDENTNAME", typeof(string));
                    dtb1.Columns.Add("DOB", typeof(string));
                    dtb1.Columns.Add("EFFDATE", typeof(string));
                    dtb1.Columns.Add("RELATIONSHIP", typeof(string));
                    dtb1.Columns.Add("AGE", typeof(string));
                    dsDepDetails.Tables.Add(dtb1);
                    if (depDetails.Count > 0)
                    {
                        for (int i = 0; i < depDetails.Count; i++)
                        {


                            string inputDate2 = depDetails[i].DOB.ToString();
                            DateTime today = DateTime.Now;
                            DateTime birthdate = DateTime.ParseExact(inputDate2, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                            int age = today.Year - birthdate.Year;

                            ///*string dateOfBirth = parsedDate2.ToString("yyyy/MM/dd");*/ // Replace with the actual date of birth

                            if (today.Month < birthdate.Month || (today.Month == birthdate.Month && today.Day < birthdate.Day))
                            {
                                age--;
                            }

                            dsDepDetails.Tables[0].Rows.Add(depDetails[i].DEPENDENTNAME, depDetails[i].DOB, depDetails[i].EFFDATE, depDetails[i].RELATIONSHIP, age + " Years");


                        }

                        this.GridView2.DataSource = dsDepDetails.Tables[0];
                        this.GridView2.DataBind();


                    }

                    ///AgeLimit

                    string SchemaName = this.GetSchemaName(dc.Decrypt(policy), dc.Decrypt(epfno));
                    //string SchemaName = "S1";
                    string trimmedSchemaName = SchemaName.Trim();
                    Console.WriteLine(trimmedSchemaName);
                    IList<AgeLimitDetails> AgeDetails = this.GetAgeLimitInfo(dc.Decrypt(policy), trimmedSchemaName);

                    DataSet dsAgeDetails = new DataSet();
                    DataTable dtb2 = new DataTable();
                    dtb2.Columns.Add("MEMBERAGE", typeof(string));
                    dtb2.Columns.Add("SPOUSEAGE", typeof(string));
                    dtb2.Columns.Add("CHILDAGE", typeof(string));
                    dtb2.Columns.Add("PARENTAGE", typeof(string));
                    dsAgeDetails.Tables.Add(dtb2);
                    if (AgeDetails.Count > 0)
                    {
                        for (int i = 0; i < AgeDetails.Count; i++)
                        {
                            dsAgeDetails.Tables[0].Rows.Add(AgeDetails[i].MEMBERAGE + " Years", AgeDetails[i].SPOUSEAGE + " Years", AgeDetails[i].CHILDAGE + " Years", AgeDetails[i].PARENTAGE + " Years");

                        }

                        this.GridView3.DataSource = dsAgeDetails.Tables[0];
                        this.GridView3.DataBind();


                    }

                    IList<LimitInfo> limitInfo = this.GetCompanyLimit(dc.Decrypt(policy), SchemaName);

                    if (limitInfo.Count > 0)
                    {
                        for (int i = 0; i < limitInfo.Count; i++)
                        {
                            {
                                Label35.InnerText = limitInfo[i].AnnualLimit.ToString();
                                Label36.InnerText = limitInfo[i].EventLimit.ToString();

                            }
                        }
                    }
                }
                else
                {

                    Response.Redirect("~/Claim_History/claimhist1.aspx?alert=PolicyNo+and+employeeNo+dose+not+match");

                }
            }
        }

        ///Get_The_Policy_Details///
        ///Renewal Date & Company Name

        public class SHEPolicyData
        {
            public string Name { get; set; }
            public DateTime RENEWALDATE { get; set; }
            public DateTime ENDDATE { get; set; }
            public string Data { get; set; }

        }


        public class BV_resp_SHEPolicyData
        {
            public int ID { get; set; }
            public string Data { get; set; }
        }

        //added by shalomi for kirula policies 2024/05/09
        public class UniquePolicyData
        {
            public string POLICYNO { get; set; }

            public string POLICYTYPE { get; set; }
            public string POLICYTAG { get; set; }

        }

        public class BV_resp_UniquePolicyData
        {
            public int ID { get; set; }
            public string Data { get; set; }
        }

        public class kirulaPolicyData
        {
            public string POLICY_ID { get; set; }

            public string YEARMON { get; set; }
            public string EPF { get; set; }
            public string MON_PREMIUM { get; set; }

        }

        public class BV_resp_kirulaPolicyData
        {
            public int ID { get; set; }
            public string Data { get; set; }
        }

        //added by shalomi for kirula policies 2024/05/09


        public List<SHEPolicyData> get_she_policy_details(string policyNo)
        {
            List<SHEPolicyData> polDetailsList = new List<SHEPolicyData>();
            //BV_SHE_svc she = new BV_SHE_svc();
            //var pol = she.get_she_pol_details(policyInfo);

            //SHEEmployeeData employee = null;
            BV_resp_SHEPolicyData polDetails = null;
            using (WebClient webClient = new System.Net.WebClient())
            {
                try
                {
                    WebClient n = new WebClient();
                    var json = n.DownloadString(host_ip + "/SHE_Tab_API/GIService.svc/GetPolicy?policyNo=" + policyNo);
                    polDetails = JsonConvert.DeserializeObject<BV_resp_SHEPolicyData>(json);
                    if (polDetails != null)
                    {
                        //string data = polDetails.Data.Replace("\\", "");
                        polDetailsList = JsonConvert.DeserializeObject<List<SHEPolicyData>>(polDetails.Data);


                    }
                    else
                    {
                        //lblErrorMsg.InnerText = "No data found for this policy no";
                    }
                }
                catch (Exception ex)
                {
                    string msgs = "ERROR:" + ex.Message;
                    //throw ex;
                    //this.lblErrorMsg.Text = "No data found for this policy no";
                }

            }


            return polDetailsList;
        }


        //added by shalomi for kirula policies 2024/05/09
        public List<UniquePolicyData> get_unique_pol_det(string policyNo)
        {
            List<UniquePolicyData> uniPolDetList = new List<UniquePolicyData>();

            BV_resp_UniquePolicyData polDetails = null;
            using (WebClient webClient = new System.Net.WebClient())
            {
                try
                {
                    WebClient n = new WebClient();
                    var json = n.DownloadString(host_ip + "/SHE_Tab_API/Service.svc/GetUniquePolicyDetails?policyNo=" + policyNo);
                    polDetails = JsonConvert.DeserializeObject<BV_resp_UniquePolicyData>(json);

                    if (polDetails.ID == 200)
                    {
                        string data = polDetails.Data.Replace("\\", "");
                        uniPolDetList = JsonConvert.DeserializeObject<List<UniquePolicyData>>(polDetails.Data);
                    }
                    else
                    {
                        //lblErrorMsg.InnerText = "No data found for this policy no";
                    }
                }
                catch (Exception ex)
                {
                    string msgs = "ERROR:" + ex.Message;

                }

            }


            return uniPolDetList;
        }

        //end of added by shalomi for kirula policies 2024/05/09

        //added by shalomi for get kirula policies 2024/05/09
        public List<kirulaPolicyData> get_kirula_pol_det(string policyNo, string empNo)
        {
            List<kirulaPolicyData> kirulaPolDetList = new List<kirulaPolicyData>();

            BV_resp_kirulaPolicyData kirulaDetails = null;
            using (WebClient webClient = new System.Net.WebClient())
            {
                try
                {
                    WebClient n = new WebClient();
                    var json = n.DownloadString(host_ip + "/SHE_Tab_API/Service.svc/GetKirulaClubMemPremium?policyNo=" + policyNo + "&memberNo=" + empNo);
                    kirulaDetails = JsonConvert.DeserializeObject<BV_resp_kirulaPolicyData>(json);

                    if (kirulaDetails.ID == 200)
                    {
                        string data = kirulaDetails.Data.Replace("\\", "");
                        kirulaPolDetList = JsonConvert.DeserializeObject<List<kirulaPolicyData>>(kirulaDetails.Data);


                    }
                    else
                    {
                        //lblErrorMsg.InnerText = "No data found for this policy no";
                    }
                }
                catch (Exception ex)
                {
                    string msgs = "ERROR:" + ex.Message;

                }

            }


            return kirulaPolDetList;
        }

        //end of added by shalomi for kirula policies 2024/05/09



        ///Get_The_Employee_Details///

        public class SHEEmployeeData
        {
            public string EMPLOYEENUMBER { get; set; }
            public string MEMBERNAME { get; set; }
            public string EMPLOYEECATEGORY { get; set; }
            public string DATEOFBIRTH { get; set; }
            public string EFFECTIVEDATE { get; set; }
            public string NIC { get; set; }
            public string ADDRESS1 { get; set; }
            public string ADDRESS2 { get; set; }
        }

        public class SHEEmployeeDataStr : SHEEmployeeData
        {
            public string DOB { get; set; }
            public string EFFDATE { get; set; }
        }

        public class BV_resp_SHEEmployeeData
        {
            public int ID { get; set; }
            public string Data { get; set; }
        }


        public SHEEmployeeDataStr get_she_employee_details(string policyNo, string membNo)
        {
            SHEEmployeeDataStr empDetailsList = new SHEEmployeeDataStr();
            //BV_SHE_svc she = new BV_SHE_svc();
            //var pol = she.get_she_pol_details(policyInfo);

            SHEEmployeeData employee = null;
            BV_resp_SHEEmployeeData empDetails = null;
            using (WebClient webClient = new System.Net.WebClient())
            {
                try
                {
                    WebClient n = new WebClient();
                    var json = n.DownloadString(host_ip + "/SHE_Tab_API/GIService.svc/GetEmployeeInformation?policyNo=" + policyNo + "&memberNo=" + membNo);
                    empDetails = JsonConvert.DeserializeObject<BV_resp_SHEEmployeeData>(json);

                    JavaScriptSerializer serializer = new JavaScriptSerializer();
                    var result = serializer.Deserialize<dynamic>(json);

                    // Accessing the values
                    string dataT = result["Data"];
                    int idT = result["ID"];

                    if (idT == 400)
                    {
                        empDetails = null;
                    }

                    if (empDetails != null)
                    {
                        string data = empDetails.Data.Replace("\\", "");
                        employee = JsonConvert.DeserializeObject<SHEEmployeeData>(data);
                    }
                    else
                    {
                        //lblErrorMsg.InnerText = "No data found for this policy no";
                        //lblAlertMessage.Text = "This is a custom alert message.";
                        //lblAlertMessage.CssClass = "alert alert-warning"; // Add CSS class for styling
                        //lblAlertMessage.Attributes.Add("data-alert-type", "custom"); // Add custom attribute to identify the alert type
                        //lblAlertMessage.Visible = true;
                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                    //this.lblErrorMsg.Text = "No data found for this policy no";
                }

            }

            if (employee != null)
            {

                SHEEmployeeDataStr i = new SHEEmployeeDataStr();

                if (employee.DATEOFBIRTH != null)
                {
                    System.DateTime a = new DateTime(1970, 1, 1, 0, 0, 0, 0, System.DateTimeKind.Utc);
                    a = a.AddMilliseconds(long.Parse(this.GetNumbers(employee.DATEOFBIRTH))).ToLocalTime();
                    i.DOB = a.Day.ToString().PadLeft(2, '0') + "/" + a.Month.ToString().PadLeft(2, '0') + "/" + a.Year.ToString();
                }

                if (employee.EFFECTIVEDATE != null)
                {
                    System.DateTime a = new DateTime(1970, 1, 1, 0, 0, 0, 0, System.DateTimeKind.Utc);
                    a = a.AddMilliseconds(long.Parse(this.GetNumbers(employee.EFFECTIVEDATE))).ToLocalTime();
                    i.EFFDATE = a.Day.ToString().PadLeft(2, '0') + "/" + a.Month.ToString().PadLeft(2, '0') + "/" + a.Year.ToString();
                }
                i.EMPLOYEENUMBER = employee.EMPLOYEENUMBER;
                i.MEMBERNAME = employee.MEMBERNAME;
                //i.NIC = employee.NIC;
                Reg_Policy_Mast polObj = new Reg_Policy_Mast();
                i.NIC = polObj.getNicNo(policyNo, membNo);
                i.EMPLOYEECATEGORY = employee.EMPLOYEECATEGORY;
                i.ADDRESS1 = employee.ADDRESS1;
                i.ADDRESS2 = employee.ADDRESS2;
                //empDetailsList.Add(i);
                empDetailsList = i;
                //}
            }
            else
            {
                empDetailsList = null;
            }

            return empDetailsList;
        }

        ///Get_The_Dependent_Details///

        public class SHEDependantData
        {
            public string DEPENDENTNAME { get; set; }
            public string DEPNDENTBIRTHDAY { get; set; }
            public string EFFECTIVEDATE { get; set; }
            public string MEMBERCODE { get; set; }
            public string RELATIONSHIP { get; set; }
        }

        public class SHEDependentDataStr : SHEDependantData
        {
            public string DOB { get; set; }
            public string EFFDATE { get; set; }
        }

        public class BV_resp_SHEDependentData
        {
            public int ID { get; set; }
            public string Data { get; set; }
        }

        public IList<SHEDependentDataStr> get_she_dependant_details(string policyNo, string membNo)
        {
            List<SHEDependentDataStr> depDetailsList = new List<SHEDependentDataStr>();
            //BV_SHE_svc she = new BV_SHE_svc();
            //var pol = she.get_she_pol_details(policyInfo);

            string data = "";
            IList<SHEDependantData> dependent = null;
            BV_resp_SHEDependentData depDetails = null;
            using (WebClient webClient = new System.Net.WebClient())
            {
                try
                {
                    WebClient n = new WebClient();
                    var json = n.DownloadString(host_ip + "/SHE_Tab_API/GIService.svc/GetDependent?policyNo=" + policyNo + "&memberNo=" + membNo);
                    depDetails = JsonConvert.DeserializeObject<BV_resp_SHEDependentData>(json);
                    if (depDetails != null)
                    {
                        data = depDetails.Data.Replace("\\", "");
                        dependent = JsonConvert.DeserializeObject<IList<SHEDependantData>>(data);
                    }
                    else
                    {
                        //this.lblErrorMsg.Text = "No data found for this policy no";
                    }
                }
                catch (Exception ex)
                {
                    //throw ex;
                    //this.lblErrorMsg.Text = "No data found for this policy no";
                    //lblAlertMessage.Text = "This is a custom alert message.";
                    //lblAlertMessage.CssClass = "alert alert-warning"; // Add CSS class for styling
                    //lblAlertMessage.Attributes.Add("data-alert-type", "custom"); // Add custom attribute to identify the alert type
                    //lblAlertMessage.Visible = true;
                }

            }

            if (dependent != null)
            {
                foreach (var item in dependent)
                {
                    SHEDependentDataStr i = new SHEDependentDataStr();
                    i.DEPENDENTNAME = item.DEPENDENTNAME;


                    if (item.DEPNDENTBIRTHDAY != null)
                    {
                        System.DateTime a = new DateTime(1970, 1, 1, 0, 0, 0, 0, System.DateTimeKind.Utc);
                        //a = a.AddMilliseconds(long.Parse(this.GetNumbers(item.DEPNDENTBIRTHDAY))).ToLocalTime();
                        if (item.DEPNDENTBIRTHDAY.Contains("-"))
                        {
                            a = a.AddMilliseconds(long.Parse(this.GetNumbers(item.DEPNDENTBIRTHDAY)) * -1).ToLocalTime();
                        }
                        else
                        {
                            a = a.AddMilliseconds(long.Parse(this.GetNumbers(item.DEPNDENTBIRTHDAY))).ToLocalTime();
                        }
                        i.DOB = a.Day.ToString().PadLeft(2, '0') + "/" + a.Month.ToString().PadLeft(2, '0') + "/" + a.Year.ToString();
                    }

                    if (item.EFFECTIVEDATE != null)
                    {
                        System.DateTime a = new DateTime(1970, 1, 1, 0, 0, 0, 0, System.DateTimeKind.Utc);
                        a = a.AddMilliseconds(long.Parse(this.GetNumbers(item.EFFECTIVEDATE))).ToLocalTime();
                        i.EFFDATE = a.Day.ToString().PadLeft(2, '0') + "/" + a.Month.ToString().PadLeft(2, '0') + "/" + a.Year.ToString();
                    }

                    i.MEMBERCODE = item.MEMBERCODE;
                    i.RELATIONSHIP = item.RELATIONSHIP;
                    depDetailsList.Add(i);
                }
            }

            return depDetailsList;
        }




        private string GetNumbers(String InputString)
        {
            String Result = "";
            string Numbers = "0123456789";
            int i = 0;

            for (i = 0; i < InputString.Length; i++)
            {
                if (Numbers.Contains(InputString.ElementAt(i)))
                {
                    Result += InputString.ElementAt(i);
                }
            }
            return Result;
        }




        ///Get_the_compnany_active_status

        //public class SHERemarkData
        //{
        //    public string S15REMR { get; set; }

        //}


        //public class BV_resp_SHERemarkData
        //{
        //    public int ID { get; set; }
        //    public string Data { get; set; }
        //}


        //public List<SHERemarkData> get_the_remark(string policyNo)
        //{
        //    List<SHERemarkData> ActiveStatus = new List<SHERemarkData>();
        //    //BV_SHE_svc she = new BV_SHE_svc();
        //    //var pol = she.get_she_pol_details(policyInfo);

        //    //SHEEmployeeData employee = null;
        //    BV_resp_SHERemarkData ActStat = null;
        //    using (WebClient webClient = new System.Net.WebClient())
        //    {
        //        try
        //        {
        //            WebClient n = new WebClient();
        //            var json = n.DownloadString(host_ip + "/SHE_Tab_API/GIService.svc/GetRemark?policyNo=" + policyNo);
        //            ActStat = JsonConvert.DeserializeObject<BV_resp_SHERemarkData>(json);
        //            if (ActStat != null)
        //            {



        //                string data = ActStat.Data.Replace("\\", "");
        //                ActiveStatus = JsonConvert.DeserializeObject<List<SHERemarkData>>(ActStat.Data);


        //            }
        //            else
        //            {
        //                //lblErrorMsg.InnerText = "No data found for this policy no";
        //            }
        //        }
        //        catch (Exception ex)
        //        {
        //            string msgs = "ERROR:" + ex.Message;
        //            //throw ex;
        //            //this.lblErrorMsg.Text = "No data found for this policy no";
        //        }

        //    }


        //    return ActiveStatus;
        //}

        public string getCompanyActiveStatus(string policyNo)
        {
            string ActiveStatus = "";

            try
            {
                if (oconn.State == System.Data.ConnectionState.Closed)
                {
                    oconn.Open();
                }

                string sql = "select DECODE(ACTIVE, 'Y', 'Active', 'N', 'Not Active') AS ACTIVE " +
                             " from SHEDATA.SHE_POLICYMAST " +
                             " where POLICY_NO = :policy_no ";

                using (OracleCommand cmd = new OracleCommand(sql, oconn))
                {
                    OracleParameter orclParaBusRegNo = new OracleParameter();
                    orclParaBusRegNo.Value = policyNo;
                    orclParaBusRegNo.ParameterName = "policy_no";

                    cmd.Parameters.Add(orclParaBusRegNo);
                    OracleDataReader companyNameReader = cmd.ExecuteReader();

                    while (companyNameReader.Read())
                    {
                        ActiveStatus = companyNameReader.GetString(0);
                    }
                    companyNameReader.Close();
                }
            }
            catch (Exception ex)
            {
                //log log1 = new log();
                //log1.write_log("Failed at getRegisteredPolicies" + ex.ToString());
                throw ex;
            }
            finally
            {
                if (oconn.State == System.Data.ConnectionState.Open)
                {
                    oconn.Close();
                }
            }

            return ActiveStatus;
        }

        ///Get_The_Policy_Details///
        ///Policy Period & Policy Type


        public class PolicyDetails
        {
            //public int RenewalDate { get; set; }
            public int PolicyPeriod { get; set; }
            public string PolicyType { get; set; }
            public string ClaimRemark { get; set; }

        }

        public PolicyDetails[] GetPolicyDetails(string policyNo)
        {
            List<PolicyDetails> PolDetList = new List<PolicyDetails>();

            try
            {
                if (oconn.State == System.Data.ConnectionState.Closed)
                {
                    oconn.Open();
                }

                string sql = "SELECT S1INPD,S1PLTY,DECODE(S1CMST, 'A', 'Active', ' ', 'Not Active') AS S1CMST FROM SHEDATA.SH01PF WHERE S1PLNM=:POLICYNO"; //change this query to get the claim remark

                using (OracleCommand cmd = new OracleCommand(sql, oconn))
                {
                    OracleParameter orclParaBusRegNo = new OracleParameter();
                    orclParaBusRegNo.Value = policyNo;
                    orclParaBusRegNo.ParameterName = "POLICYNO";

                    cmd.Parameters.Add(orclParaBusRegNo);

                    OracleDataReader reader = cmd.ExecuteReader();
                    if (reader.HasRows)
                    {

                        while (reader.Read())
                        {
                            PolicyDetails PolInfo = new PolicyDetails
                            {

                                PolicyPeriod = reader.GetInt32(0),
                                PolicyType = reader.GetString(1),
                                ClaimRemark = reader.GetString(2)

                            };

                            PolDetList.Add(PolInfo);
                            //GridView1.DataSource = PolDetList;
                            //GridView1.DataBind();
                        }
                    }

                    reader.Close();
                }
            }
            catch (Exception ex)
            {
                // Handle the exception appropriately.
                throw ex;
            }
            finally
            {
                if (oconn.State == System.Data.ConnectionState.Open)
                {
                    oconn.Close();
                }
            }
            return PolDetList.ToArray();
        }



        public class AgeLimitDetails
        {
            public int MEMBERAGE { get; set; }
            public int SPOUSEAGE { get; set; }
            public int CHILDAGE { get; set; }
            public int PARENTAGE { get; set; }

        }

        public AgeLimitDetails[] GetAgeLimitInfo(string policyNo, string schemaName)
        {
            List<AgeLimitDetails> AgeLimitInfo = new List<AgeLimitDetails>();

            try
            {
                if (oconn.State == System.Data.ConnectionState.Closed)
                {
                    oconn.Open();
                }

                string sql = "SELECT OVERAGE_LIMIT_MAX,OVERAGE_LIMITMAX_SPO,CHILD_AGE_LIMIT_MAX,OVERAGE_LIMITMAX_PA FROM NM_UNDERWRITING.HEALTH_SCHEMA WHERE POLICY_ID=:POLICYNO AND SCHEMA_NAME=:SCHEMA";

                using (OracleCommand cmd = new OracleCommand(sql, oconn))
                {
                    OracleParameter orclParaBusRegNo = new OracleParameter();
                    orclParaBusRegNo.Value = policyNo;
                    orclParaBusRegNo.ParameterName = "POLICYNO";

                    OracleParameter orclParaSchemaName = new OracleParameter();
                    orclParaSchemaName.Value = schemaName;
                    orclParaSchemaName.ParameterName = "SCHEMA";

                    cmd.Parameters.Add(orclParaBusRegNo);
                    cmd.Parameters.Add(orclParaSchemaName);
                    OracleDataReader reader = cmd.ExecuteReader();
                    if (reader.HasRows)
                    {

                        while (reader.Read())
                        {
                            AgeLimitDetails AgeInfo = new AgeLimitDetails
                            {
                                MEMBERAGE = reader.GetInt32(0),
                                SPOUSEAGE = reader.GetInt32(1),
                                CHILDAGE = reader.GetInt32(2),
                                PARENTAGE = reader.GetInt32(3),

                            };

                            AgeLimitInfo.Add(AgeInfo);
                            GridView3.DataSource = AgeLimitInfo;
                            GridView3.DataBind();
                        }
                    }

                    reader.Close();
                }
            }
            catch (Exception ex)
            {
                // Handle the exception appropriately.
                throw ex;
            }
            finally
            {
                if (oconn.State == System.Data.ConnectionState.Open)
                {
                    oconn.Close();
                }
            }
            return AgeLimitInfo.ToArray();
        }

        ///panel2

        public class SublimitInfo
        {
            public string Subval1 { get; set; }
            public string Subval2 { get; set; }
            public string Subval3 { get; set; }
            public string Subval4 { get; set; }
            public string Subval5 { get; set; }
            public string Subval6 { get; set; }
            public string Subval7 { get; set; }
            public string Subval8 { get; set; }
            public string Subval9 { get; set; }
            // ... Add more properties for Subval3, Subval4, and so on ...
        }
        public class PaidAmountInfo
        {
            public string paidam1 { get; set; }
            public string paidam2 { get; set; }
            public string paidam3 { get; set; }
            public string paidam4 { get; set; }
            public string paidam5 { get; set; }
            public string paidam6 { get; set; }
            public string paidam7 { get; set; }
            public string paidam8 { get; set; }
            public string paidam9 { get; set; }
            // ... Add more properties for Subval3, Subval4, and so on ...
        }

        //panel2
        protected void IconClick_ServerClick2(object sender, EventArgs e)
        {
            //myDropdown.Items.Clear();
            string epfno = Request.QueryString["EPF"];
            string policy = Request.QueryString["POLICYNO"];
            mainpanel1.Visible = false;
            mainpanel2.Visible = true;
            mainpanel3.Visible = false;
            mainpanel4.Visible = false;
            SHEEmployeeDataStr empDetails = this.get_she_employee_details(dc.Decrypt(policy), dc.Decrypt(epfno));

            DataSet dsMemDetails = new DataSet();
            DataTable dtb1 = new DataTable();
            dtb1.Columns.Add("MEMBERCODE", typeof(string));
            dtb1.Columns.Add("MEMBERNAME", typeof(string));

            //if (empDetails.Count > 0)

            IList<SHEDependentDataStr> depDetails = this.get_she_dependant_details(dc.Decrypt(policy), dc.Decrypt(epfno));

            dtb1.Columns.Add("DEPENDENTNAME", typeof(string));
            dtb1.Columns.Add("RELATIONSHIP", typeof(string));
            dsMemDetails.Tables.Add(dtb1);

            //if (empDetails != null)
            //{
            //    myDropdown.Items.Clear();
            //    //dsMemDetails.Tables[0].Rows.Add(empDetails.MEMBERNAME, "Main Life");

            //    Dictionary<string, string> data = new Dictionary<string, string>();
            //    //data.Add("", "select a name");
            //    data.Add("Main Life", empDetails.MEMBERNAME);
            //    //myDropdown.Items.Add(new ListItem("Select a name", ""));
            //    //myDropdown.Items[0].Enabled = false;
            //    foreach (KeyValuePair<string, string> item in data)
            //    {
            //        ListItem listItem = new ListItem(item.Value, item.Key);
            //        if (item.Key == "")
            //        {
            //            listItem.Attributes["disabled"] = "disabled";
            //            listItem.Selected = true;
            //        }
            //        myDropdown.Items.Add(listItem);
            //    }
            //}

            //if (depDetails.Count > 0)
            //{
            //    for (int i = 0; i < depDetails.Count; i++)
            //    {
            //        // dsMemDetails.Tables[0].Rows.Add(depDetails[i].DEPENDENTNAME, depDetails[i].RELATIONSHIP);

            //        Dictionary<string, string> data2 = new Dictionary<string, string>();
            //        data2.Add(depDetails[i].RELATIONSHIP, depDetails[i].DEPENDENTNAME);
            //        foreach (KeyValuePair<string, string> item in data2)
            //        {
            //            ListItem listItem = new ListItem(item.Value, item.Key);
            //            myDropdown.Items.Add(listItem);
            //            myDropdown.DataBind();
            //        }
            //    }
            //}




            //string SchemaName = this.GetSchemaName(dc.Decrypt(policy), dc.Decrypt(epfno));
            string SchemaName = (string)Session["schemaname"];
            IList<LimitInfo> limitInfo = this.GetCompanyLimit(dc.Decrypt(policy), SchemaName);

            if (limitInfo.Count > 0)
            {
                for (int i = 0; i < limitInfo.Count; i++)
                {
                    {
                        label3.InnerText = limitInfo[i].AnnualLimit.ToString();
                        label8.InnerText = limitInfo[i].EventLimit.ToString();
                        label9.InnerText = limitInfo[i].OPDLimit.ToString();

                        int genRoomLimit = limitInfo[i].GenRoomLimit;
                        int prvRoomLimit = limitInfo[i].PrvRoomLimit;
                        int icuRoomLimit = limitInfo[i].ICURoomLimit;

                        int RoomLimit = genRoomLimit + prvRoomLimit;
                        label11.InnerText = RoomLimit.ToString();

                        label12.InnerText = icuRoomLimit.ToString();

                    }
                }
            }

            List<SublimitInfo> sublimitInfoList = GetSublimitInfoList();
            List<PaidAmountInfo> PaidAmountInfoList = GetPaidAmountInfoList();

            for (int i = 0; i < GridView1.Rows.Count && i < sublimitInfoList.Count && i < PaidAmountInfoList.Count; i++)
            {
                Label subval1 = GridView1.Rows[i].FindControl("Subval1") as Label;
                Label subval2 = GridView1.Rows[i].FindControl("Subval2") as Label;
                Label subval3 = GridView1.Rows[i].FindControl("Subval3") as Label;
                Label subval4 = GridView1.Rows[i].FindControl("Subval4") as Label;
                Label subval5 = GridView1.Rows[i].FindControl("Subval5") as Label;
                Label subval6 = GridView1.Rows[i].FindControl("Subval6") as Label;
                Label subval7 = GridView1.Rows[i].FindControl("Subval7") as Label;
                Label subval8 = GridView1.Rows[i].FindControl("Subval8") as Label;
                //Label subval9 = GridView1.Rows[i].FindControl("Subval9") as Label;

                Label paidam1 = GridView1.Rows[i].FindControl("paidam1") as Label;
                Label paidam2 = GridView1.Rows[i].FindControl("paidam2") as Label;
                Label paidam3 = GridView1.Rows[i].FindControl("paidam3") as Label;
                Label paidam4 = GridView1.Rows[i].FindControl("paidam4") as Label;
                Label paidam5 = GridView1.Rows[i].FindControl("paidam5") as Label;
                Label paidam6 = GridView1.Rows[i].FindControl("paidam6") as Label;
                Label paidam7 = GridView1.Rows[i].FindControl("paidam7") as Label;
                Label paidam8 = GridView1.Rows[i].FindControl("paidam8") as Label;
                //Label paidam9 = GridView1.Rows[i].FindControl("paidam9") as Label;


                Label bal1 = GridView1.Rows[i].FindControl("bal1") as Label;
                Label bal2 = GridView1.Rows[i].FindControl("bal2") as Label;
                Label bal3 = GridView1.Rows[i].FindControl("bal3") as Label;
                Label bal4 = GridView1.Rows[i].FindControl("bal4") as Label;
                Label bal5 = GridView1.Rows[i].FindControl("bal5") as Label;
                Label bal6 = GridView1.Rows[i].FindControl("bal6") as Label;
                Label bal7 = GridView1.Rows[i].FindControl("bal7") as Label;
                Label bal8 = GridView1.Rows[i].FindControl("bal8") as Label;
                Label bal9 = GridView1.Rows[i].FindControl("bal9") as Label;

                subval1.Text = sublimitInfoList[i].Subval1;
                subval2.Text = sublimitInfoList[i].Subval2;
                subval3.Text = sublimitInfoList[i].Subval3;
                subval4.Text = sublimitInfoList[i].Subval4;
                subval5.Text = sublimitInfoList[i].Subval5;
                subval6.Text = sublimitInfoList[i].Subval6;
                subval7.Text = sublimitInfoList[i].Subval7;
                subval8.Text = sublimitInfoList[i].Subval8;
                //subval9.Text = sublimitInfoList[i].Subval9;

                paidam1.Text = PaidAmountInfoList[i].paidam1;
                paidam2.Text = PaidAmountInfoList[i].paidam2;
                paidam3.Text = PaidAmountInfoList[i].paidam3;
                paidam4.Text = PaidAmountInfoList[i].paidam4;
                paidam5.Text = PaidAmountInfoList[i].paidam5;
                paidam6.Text = PaidAmountInfoList[i].paidam6;
                paidam7.Text = PaidAmountInfoList[i].paidam7;
                paidam8.Text = PaidAmountInfoList[i].paidam8;
                //paidam9.Text = PaidAmountInfoList[i].paidam9;

                int.TryParse(sublimitInfoList[i].Subval1, out int subval1Value);
                int.TryParse(sublimitInfoList[i].Subval2, out int subval2Value);
                int.TryParse(sublimitInfoList[i].Subval3, out int subval3Value);
                int.TryParse(sublimitInfoList[i].Subval4, out int subval4Value);
                int.TryParse(sublimitInfoList[i].Subval5, out int subval5Value);
                int.TryParse(sublimitInfoList[i].Subval6, out int subval6Value);
                int.TryParse(sublimitInfoList[i].Subval7, out int subval7Value);
                int.TryParse(sublimitInfoList[i].Subval8, out int subval8Value);
                //int.TryParse(sublimitInfoList[i].Subval9, out int subval9Value);

                int.TryParse(PaidAmountInfoList[i].paidam1, out int paidam1Value);
                int.TryParse(PaidAmountInfoList[i].paidam2, out int paidam2Value);
                int.TryParse(PaidAmountInfoList[i].paidam3, out int paidam3Value);
                int.TryParse(PaidAmountInfoList[i].paidam4, out int paidam4Value);
                int.TryParse(PaidAmountInfoList[i].paidam5, out int paidam5Value);
                int.TryParse(PaidAmountInfoList[i].paidam6, out int paidam6Value);
                int.TryParse(PaidAmountInfoList[i].paidam7, out int paidam7Value);
                int.TryParse(PaidAmountInfoList[i].paidam8, out int paidam8Value);
                //int.TryParse(PaidAmountInfoList[i].paidam9, out int paidam9Value);

                int val1 = (subval1Value - paidam1Value);
                int val2 = (subval2Value - paidam2Value);
                int val3 = (subval3Value - paidam3Value);
                int val4 = (subval4Value - paidam4Value);
                int val5 = (subval5Value - paidam5Value);
                int val6 = (subval6Value - paidam6Value);
                int val7 = (subval7Value - paidam7Value);
                int val8 = (subval8Value - paidam8Value);
                int val9 = (val1 + val2 + val3 + val4 + val5 + val6 + val7 + val8);

                //TotalPaid = paidam1Value + paidam2Value + paidam3Value + paidam4Value + paidam5Value + paidam6Value + paidam7Value + paidam8Value;



                bal1.Text = val1.ToString();
                bal2.Text = val2.ToString();
                bal3.Text = val3.ToString();
                bal4.Text = val4.ToString();
                bal5.Text = val5.ToString();
                bal6.Text = val6.ToString();
                bal7.Text = val7.ToString();
                bal8.Text = val8.ToString();
                bal9.Text = val9.ToString();


            }



            //for (int i = 0; i < GridView1.Rows.Count && i < PaidAmountInfoList.Count; i++)
            //{





            //}


            IList<SHEBenefitsData> BenefitInfo = this.get_she_benefit_details(dc.Decrypt(policy), SchemaName);

            if (BenefitInfo.Count > 0)
            {
                for (int i = 0; i < BenefitInfo.Count; i++)
                {
                    label14.InnerText = BenefitInfo[i].CicLimit.ToString();
                    label15.InnerText = BenefitInfo[i].DayCareSurgeryCover.ToString();
                    label17.InnerText = BenefitInfo[i].EndoscopyColonoscopyLimit.ToString();
                    label18.InnerText = BenefitInfo[i].CatractLenceLimit.ToString();
                    label19.InnerText = BenefitInfo[i].FertilityCover.ToString();
                    label20.InnerText = BenefitInfo[i].CongenitalCover.ToString();
                    label21.InnerText = BenefitInfo[i].DentalDoctorsFeeLimit.ToString();
                    label22.InnerText = BenefitInfo[i].PolicyExcess.ToString();
                    label23.InnerText = BenefitInfo[i].TenMonthWaitPeriod.ToString();
                    label24.InnerText = BenefitInfo[i].CeserianCoverLimit.ToString();
                    label25.InnerText = BenefitInfo[i].NdcCoverLimit.ToString();
                    label26.InnerText = BenefitInfo[i].ForcepVaccumLimit.ToString();
                    label27.InnerText = BenefitInfo[i].PregnancyRelatedCover.ToString();

                }
            }
        }


        ///Get_The_Benefit_Details///

        public class SHEBenefitsData
        {
            public string CicLimit { get; set; }
            public string DayCareSurgeryCover { get; set; }
            public string EndoscopyColonoscopyLimit { get; set; }
            public string CatractLenceLimit { get; set; }
            public string FertilityCover { get; set; }
            public string CongenitalCover { get; set; }
            public string CovidCover { get; set; }
            public string DentalDoctorsFeeLimit { get; set; }
            public string PolicyExcess { get; set; }
            public string TenMonthWaitPeriod { get; set; }
            public string CeserianCoverLimit { get; set; }
            public string NdcCoverLimit { get; set; }
            public string ForcepVaccumLimit { get; set; }
            public string PregnancyRelatedCover { get; set; }


        }

        public class BV_resp_SHEBenefitData
        {
            public int ID { get; set; }
            public string Data { get; set; }
        }


        public IList<SHEBenefitsData> get_she_benefit_details(string policyNo, string schemaName)
        {
            List<SHEBenefitsData> BenDetailsList = new List<SHEBenefitsData>();
            //BV_SHE_svc she = new BV_SHE_svc();
            //var pol = she.get_she_pol_details(policyInfo);

            string data = "";
            IList<SHEBenefitsData> benefit = null;
            BV_resp_SHEBenefitData BenDetails = null;
            using (WebClient webClient = new System.Net.WebClient())
            {
                try
                {
                    WebClient n = new WebClient();
                    var json = n.DownloadString(host_ip + "/SHE_Tab_API/GIService.svc/GetBenefit?policyNo=" + policyNo + "&schemaName=" + schemaName);
                    BenDetails = JsonConvert.DeserializeObject<BV_resp_SHEBenefitData>(json);
                    if (BenDetails != null)
                    {
                        data = BenDetails.Data.Replace("\\", "");
                        benefit = JsonConvert.DeserializeObject<IList<SHEBenefitsData>>(data);
                    }

                }
                catch (Exception ex)
                {
                    throw ex;
                    //this.lblErrorMsg.Text = "No data found for this policy no";
                }

            }

            if (benefit != null)
            {
                foreach (var item in benefit)
                {

                    SHEBenefitsData i = new SHEBenefitsData();
                    i.CicLimit = item.CicLimit;
                    i.DayCareSurgeryCover = item.DayCareSurgeryCover;
                    i.EndoscopyColonoscopyLimit = item.EndoscopyColonoscopyLimit;
                    i.CatractLenceLimit = item.CatractLenceLimit;
                    i.CeserianCoverLimit = item.CeserianCoverLimit;
                    i.FertilityCover = item.FertilityCover;
                    i.CongenitalCover = item.CongenitalCover;
                    i.CovidCover = item.CovidCover;
                    i.DentalDoctorsFeeLimit = item.CovidCover;
                    i.PolicyExcess = item.PolicyExcess;
                    i.TenMonthWaitPeriod = item.TenMonthWaitPeriod;
                    i.NdcCoverLimit = item.NdcCoverLimit;
                    i.ForcepVaccumLimit = item.ForcepVaccumLimit;
                    i.PregnancyRelatedCover = item.PregnancyRelatedCover;

                    BenDetailsList.Add(i);

                }
            }

            return BenDetailsList;
        }

        ///sublimit

        private List<SublimitInfo> GetSublimitInfoList()
        {
            string epfno = Request.QueryString["EPF"];
            string policy = Request.QueryString["POLICYNO"];
            string SchemaName = (string)Session["schemaname"];
            IList<SublimitValue> limitvalue = this.GetSublimitValue(dc.Decrypt(policy), SchemaName);
            DataSet SubLimitValueDetails = new DataSet();
            List<SublimitInfo> sublimitInfoList = new List<SublimitInfo>();

            if (limitvalue.Count > 0)
            {
                SublimitInfo sublimitInfo = new SublimitInfo();
                for (int i = 0; i < limitvalue.Count; i++)
                {
                    sublimitInfo.Subval1 = limitvalue[i].Indoor1.ToString();
                    sublimitInfo.Subval2 = limitvalue[i].Indoor2.ToString();
                    sublimitInfo.Subval3 = limitvalue[i].Indoor3.ToString();
                    sublimitInfo.Subval4 = limitvalue[i].Indoor4.ToString();
                    sublimitInfo.Subval5 = limitvalue[i].Indoor5.ToString();
                    sublimitInfo.Subval6 = limitvalue[i].Indoor6.ToString();
                    sublimitInfo.Subval7 = limitvalue[i].Indoor7.ToString();
                    sublimitInfo.Subval8 = limitvalue[i].Indoor8.ToString();
                    //sublimitInfo.Subval9 = limitvalue[i].Indoor9.ToString();


                    sublimitInfoList.Add(sublimitInfo);
                }

            }

            //commented by shalomi
            //Session["schemaname"] = null;
            return sublimitInfoList;
        }

        ///PaidAmount

        private List<PaidAmountInfo> GetPaidAmountInfoList()
        {
            string epfno = Request.QueryString["EPF"];
            string policy = Request.QueryString["POLICYNO"];
            //string SchemaName = (string)Session["schemaname"];
            IList<PaidAmount> PaidAmountValue = this.GetPaidamount(dc.Decrypt(policy), dc.Decrypt(epfno));
            DataSet SubLimitValueDetails = new DataSet();
            List<PaidAmountInfo> PaidInfoList = new List<PaidAmountInfo>();

            if (PaidAmountValue.Count > 0)
            {
                PaidAmountInfo PdAmtInfo = new PaidAmountInfo();
                for (int i = 0; i < PaidAmountValue.Count; i++)
                {
                    PdAmtInfo.paidam1 = PaidAmountValue[i].PdIndoor1.ToString();
                    PdAmtInfo.paidam2 = PaidAmountValue[i].PdIndoor2.ToString();
                    PdAmtInfo.paidam3 = PaidAmountValue[i].PdIndoor3.ToString();
                    PdAmtInfo.paidam4 = PaidAmountValue[i].PdIndoor4.ToString();
                    PdAmtInfo.paidam5 = PaidAmountValue[i].PdIndoor5.ToString();
                    PdAmtInfo.paidam6 = PaidAmountValue[i].PdIndoor6.ToString();
                    PdAmtInfo.paidam7 = PaidAmountValue[i].PdIndoor7.ToString();
                    PdAmtInfo.paidam8 = PaidAmountValue[i].PdIndoor8.ToString();

                    //sublimitInfo.Subval9 = limitvalue[i].Indoor9.ToString();


                    PaidInfoList.Add(PdAmtInfo);
                }

            }

            //commented by shalomi
            //Session["schemaname"] = null;
            return PaidInfoList;
        }
        ///Get Schema Name///

        public string GetSchemaName(string policyNo, string epfno)  /*NO NEED TO USE THIS METHOD RETRIVE SCHEMA ID FROM SH02PF*/
        {
            string SchemaName = "";
            int SchemaID = 0; // Initialize SchemaID as an integer
            try
            {
                if (oconn.State == System.Data.ConnectionState.Closed)
                {
                    oconn.Open();
                }

                string sql = "SELECT S2EMGD FROM SHEDATA.SH02PF WHERE S2CPLN=:POLICYNO AND S2EPFN=:EPFNO AND ROWNUM = 1";


                using (OracleCommand cmd = new OracleCommand(sql, oconn))
                {
                    OracleParameter orclParaBusRegNo = new OracleParameter();
                    orclParaBusRegNo.Value = policyNo;
                    orclParaBusRegNo.ParameterName = "POLICYNO";
                    OracleParameter orclParaEPFNO = new OracleParameter();
                    orclParaEPFNO.Value = epfno;
                    orclParaEPFNO.ParameterName = "EPFNO";

                    cmd.Parameters.Add(orclParaBusRegNo);
                    cmd.Parameters.Add(orclParaEPFNO);
                    OracleDataReader reader = cmd.ExecuteReader();

                    while (reader.Read())
                    {
                        // Use GetInt32 to correctly retrieve an integer value
                        //SchemaID = reader.IsDBNull(0) ? 0 : reader.GetInt32(0);
                        SchemaName = reader.IsDBNull(0) ? null : reader.GetString(0);
                    }
                    reader.Close();
                }

                //string sql2 = "select SCHEMA_NAME FROM NM_UNDERWRITING.HEALTH_SCHEMA WHERE POLICY_ID=:POLICYNO AND SCHEMA_ID=:SCHEMAID ";

                //using (OracleCommand cmd = new OracleCommand(sql2, oconn))
                //{
                //    OracleParameter orclParaSchemaID = new OracleParameter();
                //    orclParaSchemaID.Value = SchemaID;
                //    orclParaSchemaID.ParameterName = "SCHEMAID";

                //    OracleParameter orclParaBusRegNo = new OracleParameter();
                //    orclParaBusRegNo.Value = policyNo;
                //    orclParaBusRegNo.ParameterName = "POLICYNO";

                //    cmd.Parameters.Add(orclParaSchemaID);
                //    cmd.Parameters.Add(orclParaBusRegNo);
                //    OracleDataReader reader = cmd.ExecuteReader();

                //    while (reader.Read())
                //    {
                //        SchemaName = reader.IsDBNull(0) ? null : reader.GetString(0);
                //    }
                //    reader.Close();
                //}

                //SchemaName = SchemaName.Trim().PadRight(10);
            }
            catch (Exception ex)
            {
                // Handle the exception appropriately.
                throw ex;
            }
            finally
            {
                if (oconn.State == System.Data.ConnectionState.Open)
                {
                    oconn.Close();
                }
            }
            Session["schemaname"] = SchemaName;
            return SchemaName;
        }

        ///GetCompanyLimit

        public class LimitInfo
        {
            public int AnnualLimit { get; set; }
            public int EventLimit { get; set; }
            public int OPDLimit { get; set; }
            public int GenRoomLimit { get; set; }
            public int PrvRoomLimit { get; set; }
            public int ICURoomLimit { get; set; }
            public int RoomLimit { get; set; }
        }

        public LimitInfo[] GetCompanyLimit(string policyNo, string schema)
        {
            List<LimitInfo> limitInfoList = new List<LimitInfo>();

            try
            {
                if (oconn.State == System.Data.ConnectionState.Closed)
                {
                    oconn.Open();
                }

                string sql = "SELECT S4YRLM,S4EVLM,S4OPDL,S4RMC1,S4RMC2,S4RMC3 FROM SHEDATA.SH04PF WHERE S4CPLC=:POLICYNO AND S4EMGD=:SCHEMA AND ROWNUM=1 ";

                using (OracleCommand cmd = new OracleCommand(sql, oconn))
                {
                    OracleParameter orclParaBusRegNo = new OracleParameter();
                    orclParaBusRegNo.Value = policyNo;
                    orclParaBusRegNo.ParameterName = "POLICYNO";

                    OracleParameter orclParaSchemaName = new OracleParameter();
                    //orclParaSchemaName.Value = schema.Trim().PadRight(10); 
                    orclParaSchemaName.Value = schema;
                    orclParaSchemaName.ParameterName = "SCHEMA";

                    cmd.Parameters.Add(orclParaBusRegNo);
                    cmd.Parameters.Add(orclParaSchemaName);
                    OracleDataReader reader = cmd.ExecuteReader();
                    if (reader.HasRows)
                    {

                        while (reader.Read())
                        {
                            LimitInfo limitinfo = new LimitInfo
                            {
                                AnnualLimit = reader.GetInt32(0),
                                EventLimit = reader.GetInt32(1),
                                OPDLimit = reader.GetInt32(2),
                                GenRoomLimit = reader.GetInt32(3),
                                PrvRoomLimit = reader.GetInt32(4),
                                ICURoomLimit = reader.GetInt32(5),

                            };

                            limitInfoList.Add(limitinfo);
                            GridView1.DataSource = limitInfoList;
                            GridView1.DataBind();
                        }
                    }

                    reader.Close();
                }
            }
            catch (Exception ex)
            {
                // Handle the exception appropriately.
                throw ex;
            }
            finally
            {
                if (oconn.State == System.Data.ConnectionState.Open)
                {
                    oconn.Close();
                }
            }
            return limitInfoList.ToArray();
        }


        ///GetSublimitValue

        public class SublimitValue
        {
            public int Indoor1 { get; set; }
            public int Indoor2 { get; set; }
            public int Indoor3 { get; set; }
            public int Indoor4 { get; set; }
            public int Indoor5 { get; set; }
            public int Indoor6 { get; set; }
            public int Indoor7 { get; set; }
            public int Indoor8 { get; set; }
            public int Indoor9 { get; set; }
        }

        public SublimitValue[] GetSublimitValue(string policyNo, string schema)
        {
            List<SublimitValue> SubmitValueList = new List<SublimitValue>();

            try
            {
                if (oconn.State == System.Data.ConnectionState.Closed)
                {
                    oconn.Open();
                }

                string sql = "SELECT S4IND1,S4IND2,S4IND3,S4IND4,S4IND5,S4IND6,S4IN11,S4IN12 FROM SHEDATA.SH04PF WHERE S4CPLC=:POLICYNO AND S4EMGD=:SCHEMA AND ROWNUM=1 ";

                using (OracleCommand cmd = new OracleCommand(sql, oconn))
                {
                    OracleParameter orclParaBusRegNo = new OracleParameter();
                    orclParaBusRegNo.Value = policyNo;
                    orclParaBusRegNo.ParameterName = "POLICYNO";

                    OracleParameter orclParaSchemaName = new OracleParameter();
                    //orclParaSchemaName.Value = schema.Trim().PadRight(10); 
                    orclParaSchemaName.Value = schema;
                    orclParaSchemaName.ParameterName = "SCHEMA";

                    cmd.Parameters.Add(orclParaBusRegNo);
                    cmd.Parameters.Add(orclParaSchemaName);
                    OracleDataReader reader = cmd.ExecuteReader();
                    if (reader.HasRows)
                    {

                        while (reader.Read())
                        {
                            SublimitValue limitvalue = new SublimitValue
                            {

                                Indoor1 = reader.GetInt32(0),
                                Indoor2 = reader.GetInt32(1),
                                Indoor3 = reader.GetInt32(2),
                                Indoor4 = reader.GetInt32(3),
                                Indoor5 = reader.GetInt32(4),
                                Indoor6 = reader.GetInt32(5),
                                Indoor7 = reader.GetInt32(6),
                                Indoor8 = reader.GetInt32(7),


                            };

                            SubmitValueList.Add(limitvalue);
                            //GridView1.DataSource = limitInfoList;
                            //GridView1.DataBind();
                        }
                    }

                    reader.Close();
                }
            }
            catch (Exception ex)
            {
                // Handle the exception appropriately.
                throw ex;
            }
            finally
            {
                if (oconn.State == System.Data.ConnectionState.Open)
                {
                    oconn.Close();
                }
            }
            return SubmitValueList.ToArray();
        }

        ///GetPaidamount

        public class PaidAmount
        {
            public int PdIndoor1 { get; set; }
            public int PdIndoor2 { get; set; }
            public int PdIndoor3 { get; set; }
            public int PdIndoor4 { get; set; }
            public int PdIndoor5 { get; set; }
            public int PdIndoor6 { get; set; }
            public int PdIndoor7 { get; set; }
            public int PdIndoor8 { get; set; }
            public int PdIndoor9 { get; set; }
        }

        public PaidAmount[] GetPaidamount(string policyNo, string MemNo)
        {
            List<PaidAmount> PaidAmountList = new List<PaidAmount>();

            try
            {
                if (oconn.State == System.Data.ConnectionState.Closed)
                {
                    oconn.Open();
                }

                string sql = "SELECT S5IND1,S5IND2,S5IND3,S5IND4,S5IND5,S5IND6,S5IN11,S5IN12 FROM SHEDATA.SH05PF WHERE S5CPLC=:POLICYNO AND S5EPFN=:MEMNO";

                using (OracleCommand cmd = new OracleCommand(sql, oconn))
                {
                    OracleParameter orclParaBusRegNo = new OracleParameter();
                    orclParaBusRegNo.Value = policyNo;
                    orclParaBusRegNo.ParameterName = "POLICYNO";

                    OracleParameter orclParaMemNo = new OracleParameter();
                    orclParaMemNo.Value = MemNo;
                    orclParaMemNo.ParameterName = "MEMNO";

                    cmd.Parameters.Add(orclParaBusRegNo);
                    cmd.Parameters.Add(orclParaMemNo);
                    OracleDataReader reader = cmd.ExecuteReader();
                    if (reader.HasRows)
                    {

                        while (reader.Read())
                        {
                            PaidAmount PaidAmountvalue = new PaidAmount
                            {

                                PdIndoor1 = reader.GetInt32(0),
                                PdIndoor2 = reader.GetInt32(1),
                                PdIndoor3 = reader.GetInt32(2),
                                PdIndoor4 = reader.GetInt32(3),
                                PdIndoor5 = reader.GetInt32(4),
                                PdIndoor6 = reader.GetInt32(5),
                                PdIndoor7 = reader.GetInt32(6),
                                PdIndoor8 = reader.GetInt32(7),


                            };

                            PaidAmountList.Add(PaidAmountvalue);
                            //GridView1.DataSource = limitInfoList;
                            //GridView1.DataBind();
                        }
                    }

                    reader.Close();
                }
            }
            catch (Exception ex)
            {
                // Handle the exception appropriately.
                throw ex;
            }
            finally
            {
                if (oconn.State == System.Data.ConnectionState.Open)
                {
                    oconn.Close();
                }
            }
            return PaidAmountList.ToArray();
        }


        ///panel3
        protected void IconClick_ServerClick3(object sender, EventArgs e)
        {
            mainpanel1.Visible = false;
            mainpanel2.Visible = false;
            mainpanel4.Visible = false;
            mainpanel3.Visible = true;

            string epfno = Request.QueryString["EPF"];
            string policy = Request.QueryString["POLICYNO"];
            // string schema = GetSchemaName(dc.Decrypt(policy), dc.Decrypt(epfno));
            string schema = (string)Session["schemaname"];
            int yearLimit = this.GetYearLimit(dc.Decrypt(policy), schema.PadRight(10));
            //int.TryParse(yearLimitString, out int yearLimit);
            List<PaidAmountInfo> PaidAmountInfoList = GetPaidAmountInfoList();

            for (int i = 0; i < PaidAmountInfoList.Count; i++)
            {
                int.TryParse(PaidAmountInfoList[i].paidam1, out int paidam1Value);
                int.TryParse(PaidAmountInfoList[i].paidam2, out int paidam2Value);
                int.TryParse(PaidAmountInfoList[i].paidam3, out int paidam3Value);
                int.TryParse(PaidAmountInfoList[i].paidam4, out int paidam4Value);
                int.TryParse(PaidAmountInfoList[i].paidam5, out int paidam5Value);
                int.TryParse(PaidAmountInfoList[i].paidam6, out int paidam6Value);
                int.TryParse(PaidAmountInfoList[i].paidam7, out int paidam7Value);
                int.TryParse(PaidAmountInfoList[i].paidam8, out int paidam8Value);
                TotalPaid = paidam1Value + paidam2Value + paidam3Value + paidam4Value + paidam5Value + paidam6Value + paidam7Value + paidam8Value;
            }

            IList<TotalpaidDetails> TotPaidDetails = this.GetTotPaidInfo(dc.Decrypt(policy), dc.Decrypt(epfno));

            if (TotPaidDetails.Count > 0)
            {
                for (int i = 0; i < TotPaidDetails.Count; i++)
                {
                    int GH = TotPaidDetails[i].GOVERNMENT;
                    int PH = TotPaidDetails[i].PRIVATE;
                    int ICU = TotPaidDetails[i].ICU;
                    FinalTotalPaid = TotalPaid + GH + PH + ICU;
                }

            }
            int AvailableBal = yearLimit - FinalTotalPaid;
            label33.InnerText = AvailableBal.ToString();
            string fromdate = label2.InnerText;
            DateTime.TryParseExact(fromdate, "yyyy-MM-dd", null, System.Globalization.DateTimeStyles.None, out DateTime renewalDate);

            // Calculate the date one year back
            DateTime oneYearBack = renewalDate.AddYears(+1);

            // Format the result as "yyyy-MM-dd" string
            string resultDateStr = oneYearBack.ToString("yyyy-MM-dd");



            IList<SHEPendingClaims> PenClmDetails = this.get_she_Pending_Claims(dc.Decrypt(policy), dc.Decrypt(epfno), fromdate, resultDateStr);
            DataSet ClmDetails = new DataSet();
            DataTable dtb1 = new DataTable();
            dtb1.Columns.Add("CLAIMNO", typeof(string));
            dtb1.Columns.Add("ADMISSION_DATE", typeof(string));
            dtb1.Columns.Add("DISCHARGE_DATE", typeof(string));
            dtb1.Columns.Add("HOSPITAL", typeof(string));
            dtb1.Columns.Add("PATIENT_NAME", typeof(string));
            dtb1.Columns.Add("PAYMENT_AMOUNT", typeof(string));
            dtb1.Columns.Add("ROOMNUMBER", typeof(string));
            dtb1.Columns.Add("STATUS_TYPE_NAME", typeof(string));
            ClmDetails.Tables.Add(dtb1);
            if (PenClmDetails.Count > 0)
            {
                for (int i = 0; i < PenClmDetails.Count; i++)
                {

                    ClmDetails.Tables[0].Rows.Add(PenClmDetails[i].CLAIMNO, PenClmDetails[i].ADMISSION_DATE, PenClmDetails[i].DISCHARGE_DATE, PenClmDetails[i].HOSPITAL, PenClmDetails[i].PATIENT_NAME, PenClmDetails[i].PAYMENT_AMOUNT, PenClmDetails[i].ROOMNUMBER, PenClmDetails[i].STATUS_TYPE_NAME);


                }

                this.GridView4.DataSource = ClmDetails.Tables[0];
                this.GridView4.DataBind();

                IList<PolicyDetails> polperiod = this.GetPolicyDetails(dc.Decrypt(policy));
                DataSet PolDetails = new DataSet();
                if (polperiod.Count > 0)
                {
                    for (int i = 0; i < polperiod.Count; i++)
                    {

                        label34.InnerText = polperiod[i].PolicyPeriod.ToString();
                        //label32.InnerText = PolInfo[i].PolicyType.ToString();

                    }

                }
            }

            //IList<SHERemainingBalance> shebalance = this.get_she_Balance(dc.Decrypt(policy), dc.Decrypt(epfno), fromdate, resultDateStr);
        }

        //get she remaining balance of policy
        public class SHERemainingBalance
        {
            public double GOVHOSPITALAMOUNT { get; set; }
            public double PRIVATEHOSPITALAMOUNT { get; set; }
            public double ICUAMOUNT { get; set; }
            public double CICTOTALAMOUNT { get; set; }
            public double DAYCARESURGERYAMOUNT { get; set; }
            public double ENDOCOLONOAMOUNT { get; set; }
            public double CATRACTLENSAMOUNT { get; set; }
            public double DENTALDOCTORSAMOUNT { get; set; }
            public double CESERIANCOVERAMOUNT { get; set; }
            public double NDCCOVERAMOUNT { get; set; }
            public double FORCEPVACCUMCOVERAMOUNT { get; set; }
            public double INDOOR1 { get; set; }
            public double INDOOR2 { get; set; }
            public double INDOOR3 { get; set; }
            public double INDOOR4 { get; set; }
            public double INDOOR5 { get; set; }
            public double INDOOR6 { get; set; }
            public double INDOOR2346 { get; set; }
            public double INDOOR23456 { get; set; }
            public double INDOOR123456 { get; set; }
            public double INDOOREXT1 { get; set; }
            public double INDOOROTHER { get; set; }
            public double TOTALPAID { get; set; }

        }

        public IList<SHERemainingBalance> get_she_Balance(string policyNo, string membNo, string fromdate, string enddate)
        {
            List<SHERemainingBalance> shebalancelist = new List<SHERemainingBalance>();
            //BV_SHE_svc she = new BV_SHE_svc();
            //var pol = she.get_she_pol_details(policyInfo);

            //string data = "";

            DateTime.TryParseExact(fromdate, "yyyy-MM-dd", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime editedfromdate);
            fromdate = editedfromdate.ToString("dd-MMM-yy").ToUpper();

            DateTime.TryParseExact(enddate, "yyyy-MM-dd", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime editedenddate);
            enddate = editedenddate.ToString("dd-MMM-yy").ToUpper();

            IList<SHERemainingBalance> clmdet = null;
            BV_resp_SHEBalance Balance = null;
            using (WebClient webClient = new System.Net.WebClient())
            {
                try
                {
                    //Hard coded
                    //policyNo = "GCHI210101000051";
                    //membNo = "75";
                    //enddate = "03-MAR-21";
                    //fromdate = "09-FEB-21";

                    WebClient n = new WebClient();
                    var json = n.DownloadString(host_ip + "/SHE_Tab_API/GIService.svc/GetSHEBalance?policyNo=" + policyNo + "&memberNo=" + membNo + "&fromDate=" + fromdate + "&toDate=" + enddate);
                    Balance = JsonConvert.DeserializeObject<BV_resp_SHEBalance>(json);
                    if (Balance != null)
                    {
                        string data = Balance.Data.Replace("\\", "");
                        clmdet = JsonConvert.DeserializeObject<List<SHERemainingBalance>>(data);

                    }
                    else
                    {
                        //this.lblErrorMsg.Text = "No data found for this policy no";
                    }
                }
                catch (Newtonsoft.Json.JsonReaderException jsonEx)
                {

                    SHERemainingBalance i = new SHERemainingBalance();
                    //i.CLAIMNO = "";
                    //i.ADMISSION_DATE = "";
                    //i.DISCHARGE_DATE = "";
                    //i.ROOMNUMBER = "";
                    //i.STATUS_TYPE_NAME = "";
                    //i.HOSPITAL = "";
                    //i.PATIENT_NAME = "";
                    //i.PAYMENT_AMOUNT = "";

                    //ClmDetailslist.Add(i);
                    return shebalancelist;
                }
                catch (Exception ex)
                {
                    throw ex;
                    //this.lblErrorMsg.Text = "No data found for this policy no";
                }

            }

            if (clmdet != null)
            {
                foreach (var item in clmdet)
                {
                    SHERemainingBalance i = new SHERemainingBalance();
                    i.CATRACTLENSAMOUNT = item.CATRACTLENSAMOUNT;

                    // i.ADMISSION_DATE = item.ADMISSION_DATE;
                    System.DateTime a = new DateTime(1970, 1, 1, 0, 0, 0, 0, System.DateTimeKind.Utc);
                    //a = a.AddMilliseconds(long.Parse(this.GetNumbers(item.ADMISSION_DATE))).ToLocalTime();
                    //i.ADMISSION_DATE = a.Day.ToString().PadLeft(2, '0') + "/" + a.Month.ToString().PadLeft(2, '0') + "/" + a.Year.ToString();

                    //i.DISCHARGE_DATE = item.DISCHARGE_DATE;
                    System.DateTime b = new DateTime(1970, 1, 1, 0, 0, 0, 0, System.DateTimeKind.Utc);
                    //b = b.AddMilliseconds(long.Parse(this.GetNumbers(item.DISCHARGE_DATE))).ToLocalTime();
                    //i.DISCHARGE_DATE = b.Day.ToString().PadLeft(2, '0') + "/" + b.Month.ToString().PadLeft(2, '0') + "/" + b.Year.ToString();

                    //i.ROOMNUMBER = item.ROOMNUMBER;
                    //i.STATUS_TYPE_NAME = item.STATUS_TYPE_NAME;
                    //i.HOSPITAL = item.HOSPITAL;
                    //i.PATIENT_NAME = item.PATIENT_NAME;
                    //i.PAYMENT_AMOUNT = item.PAYMENT_AMOUNT;

                    shebalancelist.Add(i);
                }
            }

            return shebalancelist;
        }

        ///Get_The_Pending_Claims_Details///

        public class SHEPendingClaims
        {
            public string CLAIMNO { get; set; }
            public string HOSPITAL { get; set; }
            public string ROOMNUMBER { get; set; }
            public string ADMISSION_DATE { get; set; }
            public string DISCHARGE_DATE { get; set; }
            public string PATIENT_NAME { get; set; }
            public string PAYMENT_AMOUNT { get; set; }
            public string STATUS_TYPE_NAME { get; set; }
        }

        public class BV_resp_SHEBalance
        {
            public int ID { get; set; }
            public string Data { get; set; }
        }

        public class BV_resp_SHEPendingClaims
        {
            public int ID { get; set; }
            public string Data { get; set; }
        }

        public IList<SHEPendingClaims> get_she_Pending_Claims(string policyNo, string membNo, string fromdate, string enddate)
        {
            List<SHEPendingClaims> ClmDetailslist = new List<SHEPendingClaims>();
            //BV_SHE_svc she = new BV_SHE_svc();
            //var pol = she.get_she_pol_details(policyInfo);

            //string data = "";

            DateTime.TryParseExact(fromdate, "yyyy-MM-dd", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime editedfromdate);
            fromdate = editedfromdate.ToString("dd-MMM-yy").ToUpper();

            DateTime.TryParseExact(enddate, "yyyy-MM-dd", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime editedenddate);
            enddate = editedenddate.ToString("dd-MMM-yy").ToUpper();

            IList<SHEPendingClaims> clmdet = null;
            BV_resp_SHEPendingClaims PenDetails = null;
            using (WebClient webClient = new System.Net.WebClient())
            {
                try
                {
                    //Hard coded
                    //policyNo = "GCHI210101000051";
                    //membNo = "75";
                    //enddate = "03-MAR-21";
                    //fromdate = "09-FEB-21";

                    WebClient n = new WebClient();
                    var json = n.DownloadString(host_ip + "/SHE_Tab_API/GIService.svc/GetPendingClaims?policyNo=" + policyNo + "&memberNo=" + membNo + "&toDate=" + enddate + "&fromDate=" + fromdate);
                    PenDetails = JsonConvert.DeserializeObject<BV_resp_SHEPendingClaims>(json);
                    if (PenDetails != null)
                    {
                        string data = PenDetails.Data.Replace("\\", "");
                        clmdet = JsonConvert.DeserializeObject<List<SHEPendingClaims>>(data);

                    }
                    else
                    {
                        //this.lblErrorMsg.Text = "No data found for this policy no";
                    }
                }
                catch (Newtonsoft.Json.JsonReaderException jsonEx)
                {

                    SHEPendingClaims i = new SHEPendingClaims();
                    i.CLAIMNO = "";
                    i.ADMISSION_DATE = "";
                    i.DISCHARGE_DATE = "";
                    i.ROOMNUMBER = "";
                    i.STATUS_TYPE_NAME = "";
                    i.HOSPITAL = "";
                    i.PATIENT_NAME = "";
                    i.PAYMENT_AMOUNT = "";

                    ClmDetailslist.Add(i);
                    return ClmDetailslist;
                }
                catch (Exception ex)
                {
                    throw ex;
                    //this.lblErrorMsg.Text = "No data found for this policy no";
                }

            }

            if (clmdet != null)
            {
                foreach (var item in clmdet)
                {
                    SHEPendingClaims i = new SHEPendingClaims();
                    i.CLAIMNO = item.CLAIMNO;

                    // i.ADMISSION_DATE = item.ADMISSION_DATE;
                    System.DateTime a = new DateTime(1970, 1, 1, 0, 0, 0, 0, System.DateTimeKind.Utc);
                    a = a.AddMilliseconds(long.Parse(this.GetNumbers(item.ADMISSION_DATE))).ToLocalTime();
                    i.ADMISSION_DATE = a.Day.ToString().PadLeft(2, '0') + "/" + a.Month.ToString().PadLeft(2, '0') + "/" + a.Year.ToString();

                    //added by shalomi 2024/05/08 for fix null exception error
                    if (item.DISCHARGE_DATE != null)
                    {
                        //i.DISCHARGE_DATE = item.DISCHARGE_DATE;
                        System.DateTime b = new DateTime(1970, 1, 1, 0, 0, 0, 0, System.DateTimeKind.Utc);
                        b = b.AddMilliseconds(long.Parse(this.GetNumbers(item.DISCHARGE_DATE))).ToLocalTime();
                        i.DISCHARGE_DATE = b.Day.ToString().PadLeft(2, '0') + "/" + b.Month.ToString().PadLeft(2, '0') + "/" + b.Year.ToString();
                    }
                    else
                    {

                    }

                    //comented by shalomi for above fixinh
                    //i.DISCHARGE_DATE = item.DISCHARGE_DATE;
                    //System.DateTime b = new DateTime(1970, 1, 1, 0, 0, 0, 0, System.DateTimeKind.Utc);
                    //b = b.AddMilliseconds(long.Parse(this.GetNumbers(item.DISCHARGE_DATE))).ToLocalTime();
                    //i.DISCHARGE_DATE = b.Day.ToString().PadLeft(2, '0') + "/" + b.Month.ToString().PadLeft(2, '0') + "/" + b.Year.ToString();

                    //end of change by shalomi 2024/05/08

                    i.ROOMNUMBER = item.ROOMNUMBER;
                    i.STATUS_TYPE_NAME = item.STATUS_TYPE_NAME;
                    i.HOSPITAL = item.HOSPITAL;
                    i.PATIENT_NAME = item.PATIENT_NAME;
                    i.PAYMENT_AMOUNT = item.PAYMENT_AMOUNT;

                    ClmDetailslist.Add(i);
                }
            }

            return ClmDetailslist;
        }


        ///panel4
        protected void IconClick_ServerClick4(object sender, EventArgs e)
        {
            mainpanel1.Visible = false;
            mainpanel2.Visible = false;
            mainpanel3.Visible = false;
            mainpanel4.Visible = true;

            string epfno = Request.QueryString["EPF"];
            string policy = Request.QueryString["POLICYNO"];




            IList<SHEPrevClaims> PrevClmDetails = this.get_she_Previous_Claims(dc.Decrypt(policy), dc.Decrypt(epfno));



            DataSet prevclmdet = new DataSet();
            DataTable dtb1 = new DataTable();
            dtb1.Columns.Add("CLAIMNO", typeof(string));
            dtb1.Columns.Add("PATIENTNAME", typeof(string));
            dtb1.Columns.Add("CLAIMDATE", typeof(string));
            dtb1.Columns.Add("CLAIMAMOUNT", typeof(string));
            dtb1.Columns.Add("AMOUNTPAID", typeof(string));
            string formattedDate = "";
            prevclmdet.Tables.Add(dtb1);
            if (PrevClmDetails.Count > 0)
            {
                for (int i = 0; i < PrevClmDetails.Count; i++)
                {
                    string input = PrevClmDetails[i].CLAIMDATE;

                    // Use a regular expression to extract the timestamp
                    Match match = Regex.Match(input, @"/Date\((\d+)\)/");

                    if (match.Success)
                    {
                        long timestamp = long.Parse(match.Groups[1].Value);

                        // Convert the Unix timestamp to a DateTime
                        DateTime dateTime = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc)
                            .AddMilliseconds(timestamp);

                        // Format the DateTime as "dd-MMM-yy"
                        formattedDate = dateTime.ToString("dd-MMM-yy");


                    }
                    prevclmdet.Tables[0].Rows.Add(PrevClmDetails[i].CLAIMNO, PrevClmDetails[i].PATIENTNAME, formattedDate, PrevClmDetails[i].CLAIMAMOUNT, PrevClmDetails[i].AMOUNTPAID);


                }

                this.GridView5.DataSource = prevclmdet.Tables[0];
                this.GridView5.DataBind();


            }
        }

        ///Get_The_Pending_Claims_Details///

        public class SHEPrevClaims
        {
            public string CLAIMNO { get; set; }
            public string PATIENTNAME { get; set; }
            public string CLAIMDATE { get; set; }
            public string CLAIMAMOUNT { get; set; }
            public string AMOUNTPAID { get; set; }

        }

        public class BV_resp_SHEPrevClaims
        {
            public int ID { get; set; }
            public string Data { get; set; }
        }

        public IList<SHEPrevClaims> get_she_Previous_Claims(string policyNo, string membNo)
        {
            List<SHEPrevClaims> PrevClmDetailslist = new List<SHEPrevClaims>();
            //BV_SHE_svc she = new BV_SHE_svc();
            //var pol = she.get_she_pol_details(policyInfo);

            //string data = "";

            //DateTime.TryParseExact(fromdate, "yyyy-MM-dd", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime editedfromdate);
            //fromdate = editedfromdate.ToString("dd-MMM-yy").ToUpper();

            //DateTime.TryParseExact(enddate, "yyyy-MM-dd", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime editedenddate);
            //enddate = editedenddate.ToString("dd-MMM-yy").ToUpper();

            IList<SHEPrevClaims> prevclmdet = null;
            BV_resp_SHEPrevClaims PrevClmDetails = null;
            using (WebClient webClient = new System.Net.WebClient())
            {
                try
                {


                    WebClient n = new WebClient();
                    var json = n.DownloadString(host_ip + "/SHE_Tab_API/GIService.svc/GetPreviousClaims?policyNo=" + policyNo + "&memberNo=" + membNo);
                    PrevClmDetails = JsonConvert.DeserializeObject<BV_resp_SHEPrevClaims>(json);
                    if (PrevClmDetails != null)
                    {
                        string data = PrevClmDetails.Data.Replace("\\", "");
                        prevclmdet = JsonConvert.DeserializeObject<List<SHEPrevClaims>>(data);

                    }
                    else
                    {
                        //this.lblErrorMsg.Text = "No data found for this policy no";
                    }
                }
                catch (Newtonsoft.Json.JsonReaderException jsonEx)
                {

                    SHEPrevClaims i = new SHEPrevClaims();
                    i.CLAIMNO = "";
                    i.PATIENTNAME = "";
                    i.CLAIMDATE = "";
                    i.CLAIMAMOUNT = "";
                    i.AMOUNTPAID = "";


                    PrevClmDetailslist.Add(i);
                    return PrevClmDetailslist;
                }
                catch (Exception ex)
                {
                    throw ex;
                    //this.lblErrorMsg.Text = "No data found for this policy no";
                }

            }

            if (prevclmdet != null)
            {
                foreach (var item in prevclmdet)
                {
                    SHEPrevClaims i = new SHEPrevClaims();

                    i.CLAIMNO = item.CLAIMNO;
                    i.PATIENTNAME = item.PATIENTNAME;
                    i.CLAIMDATE = item.CLAIMDATE;
                    i.CLAIMAMOUNT = item.CLAIMAMOUNT;
                    i.AMOUNTPAID = item.AMOUNTPAID;


                    PrevClmDetailslist.Add(i);
                }
            }

            return PrevClmDetailslist;
        }

        public class TotalpaidDetails
        {
            public int GOVERNMENT { get; set; }
            public int PRIVATE { get; set; }
            public int ICU { get; set; }


        }

        public TotalpaidDetails[] GetTotPaidInfo(string policyNo, string epf)
        {
            List<TotalpaidDetails> TotPaidInfo = new List<TotalpaidDetails>();

            try
            {
                if (oconn.State == System.Data.ConnectionState.Closed)
                {
                    oconn.Open();
                }

                string sql = "SELECT S5GHD1,S5PHD1,S5ICD1 FROM SHEDATA.SH05PF WHERE S5CPLC=:POLICYNO AND S5EPFN=:EPF";

                using (OracleCommand cmd = new OracleCommand(sql, oconn))
                {
                    OracleParameter orclParaBusRegNo = new OracleParameter();
                    orclParaBusRegNo.Value = policyNo;
                    orclParaBusRegNo.ParameterName = "POLICYNO";

                    OracleParameter orclParaSchemaName = new OracleParameter();
                    orclParaSchemaName.Value = epf;
                    orclParaSchemaName.ParameterName = "EPF";

                    cmd.Parameters.Add(orclParaBusRegNo);
                    cmd.Parameters.Add(orclParaSchemaName);
                    OracleDataReader reader = cmd.ExecuteReader();
                    if (reader.HasRows)
                    {

                        while (reader.Read())
                        {
                            TotalpaidDetails PaidInfo = new TotalpaidDetails
                            {
                                GOVERNMENT = reader.GetInt32(0),
                                PRIVATE = reader.GetInt32(1),
                                ICU = reader.GetInt32(2),


                            };

                            TotPaidInfo.Add(PaidInfo);
                            //GridView5.DataSource = TotPaidInfo;
                            //GridView5.DataBind();
                        }
                    }

                    reader.Close();
                }
            }
            catch (Exception ex)
            {
                // Handle the exception appropriately.
                throw ex;
            }
            finally
            {
                if (oconn.State == System.Data.ConnectionState.Open)
                {
                    oconn.Close();
                }
            }
            return TotPaidInfo.ToArray();
        }

        public int GetYearLimit(string policyNo, string schema)
        {
            int YearLim = 0;

            try
            {
                if (oconn.State == System.Data.ConnectionState.Closed)
                {
                    oconn.Open();
                }

                string sql = "SELECT S4YRLM FROM SHEDATA.SH04PF WHERE S4CPLC=:POLICYNO AND S4EMGD=:SCHEMA AND ROWNUM=1";

                using (OracleCommand cmd = new OracleCommand(sql, oconn))
                {
                    OracleParameter orclParaBusRegNo = new OracleParameter();
                    orclParaBusRegNo.Value = policyNo;
                    orclParaBusRegNo.ParameterName = "POLICYNO";


                    OracleParameter orclParaSchemaName = new OracleParameter();
                    orclParaSchemaName.Value = schema;
                    orclParaSchemaName.ParameterName = "SCHEMA";

                    cmd.Parameters.Add(orclParaBusRegNo);
                    cmd.Parameters.Add(orclParaSchemaName);
                    OracleDataReader reader = cmd.ExecuteReader();
                    if (reader.HasRows)
                    {

                        while (reader.Read())
                        {
                            YearLim = reader.GetInt32(0);


                        }



                    }

                    reader.Close();
                }
            }
            catch (Exception ex)
            {
                // Handle the exception appropriately.
                throw ex;
            }
            finally
            {
                if (oconn.State == System.Data.ConnectionState.Open)
                {
                    oconn.Close();
                }
            }
            return YearLim;
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

    }



}