using Newtonsoft.Json;
using SHE.Code;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.Odbc;
using System.Data.OracleClient;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Script.Serialization;
using System.Web.UI;
using System.Web.UI.WebControls;
using static SHE.Claim_History.claimhist2;

namespace SHE.Claim_History
{
    public partial class HistoryDetail1 : System.Web.UI.Page
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

                    /////AgeLimit

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


                }
                else
                {

                    Response.Redirect("~/Claim_History/claimhist1.aspx?alert=PolicyNo+and+employeeNo+dose+not+match");

                }
            }
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


    }
}