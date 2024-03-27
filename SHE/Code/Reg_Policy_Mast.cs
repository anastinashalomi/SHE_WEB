using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.OracleClient;
using System.Configuration;
using System.Collections;
using System.Data;
using System.Data.Odbc;
using System.Web.UI.HtmlControls;

/// <summary>
/// Summary description for Reg_Policy_Mast
/// </summary>
public class Reg_Policy_Mast
{
    public string policyNo { get; set; }
    public string comp_name { get; set; }
    public string bus_reg_no { get; set; }
    public string policy_start_date { get; set; }
    public string policy_end_date { get; set; }
    public string active { get; set; }
    public string added_user { get; set; }
    public string added_date { get; set; }
    public string app_allowed { get; set; }
    public string intimation_allowed { get; set; }
    public string resultMsg { get; set; }

    //OracleConnection oraconn = new OracleConnection(ConfigurationManager.AppSettings["DBConString"]);
    private HtmlGenericControl control;
    OdbcConnection db2conn = new OdbcConnection(ConfigurationManager.AppSettings["DB2"]);
    OracleConnection oraconn = new OracleConnection(ConfigurationManager.AppSettings["OracleDB"]);
    OracleConnection oconnLife = new OracleConnection(ConfigurationManager.AppSettings["OraLifeDB"]);
    public Reg_Policy_Mast()
	{
		//
		// TODO: Add constructor logic here
		//
	}

    public Reg_Policy_Mast(string policNo)
    {
        try
        {
            if (oraconn.State == System.Data.ConnectionState.Closed)
            {
                oraconn.Open();
            }

            resultMsg = "No data found for this Policy No.";
            string sql = "select to_char(POLICY_START_DATE, 'YYYY-MM-DD'), to_char(POLICY_END_DATE, 'YYYY-MM-DD'), ACTIVE, ADDED_BY, " +
                          //" to_char(ADDED_DATE, 'YYYY-MM-DD') , COMPANY_NAME, BUS_REG_NO, APP_ALLOWED, INTIMATON_ALLOWED " +
                          " to_char(ADDED_DATE, 'YYYY-MM-DD') , COMPANY_NAME, BUS_REG_NO, ISSUE_DIGITAL_CARD, INTIMATON_ALLOWED " +
                          " from SHEDATA.SHE_POLICYMAST " +
                          " where POLICY_NO  =: poln_v";

            using (OracleCommand cmd = new OracleCommand(sql, oraconn))
            {
                OracleParameter orclParaPolicyNo = new OracleParameter();
                orclParaPolicyNo.Value = policNo;
                orclParaPolicyNo.ParameterName = "poln_v";                

                cmd.Parameters.Add(orclParaPolicyNo);                

                OracleDataReader PolicyDetailsReader = cmd.ExecuteReader();

                while (PolicyDetailsReader.Read())
                {
                    policyNo = policNo;
                    if (!PolicyDetailsReader.IsDBNull(0)) { policy_start_date = PolicyDetailsReader.GetString(0); } else { policy_start_date = ""; }
                    if (!PolicyDetailsReader.IsDBNull(1)) { policy_end_date = PolicyDetailsReader.GetString(1); } else { policy_end_date = ""; }
                    if (!PolicyDetailsReader.IsDBNull(2)) { active = PolicyDetailsReader.GetString(2); } else { active = ""; }
                    if (!PolicyDetailsReader.IsDBNull(3)) { added_user = PolicyDetailsReader.GetString(3); } else { added_user = ""; }
                    if (!PolicyDetailsReader.IsDBNull(4)) { added_date = PolicyDetailsReader.GetString(4); } else { added_date = ""; }                    
                    if (!PolicyDetailsReader.IsDBNull(5)) { comp_name = PolicyDetailsReader.GetString(5); } else { comp_name = ""; }
                    if (!PolicyDetailsReader.IsDBNull(6)) { bus_reg_no = PolicyDetailsReader.GetString(6); } else { bus_reg_no = ""; }
                    if (!PolicyDetailsReader.IsDBNull(7)) { app_allowed = PolicyDetailsReader.GetString(7); } else { app_allowed = ""; }
                    if (!PolicyDetailsReader.IsDBNull(8)) { intimation_allowed = PolicyDetailsReader.GetString(8); } else { intimation_allowed = ""; }
                    resultMsg = "Success";
                }
                PolicyDetailsReader.Close();
            }
        }
        catch (Exception ex)
        {
            throw ex;
        }
        finally
        {
            if (oraconn.State == System.Data.ConnectionState.Open)
            {
                oraconn.Close();
            }
        }
    }

    public ArrayList getRegisteredPolicies(string bus_reg_no)
    {
        ArrayList registeredPols = new ArrayList();

        try
        {
            if (oraconn.State == System.Data.ConnectionState.Closed)
            {
                oraconn.Open();
            }

            string sql = "select POLICY_NO " + 
                         " from SHEDATA.SHE_POLICYMAST " +
                         " where BUS_REG_NO = :bus_reg_num " ;
                    
            using (OracleCommand cmd = new OracleCommand(sql, oraconn))
            {
                OracleParameter orclParaBusRegNo = new OracleParameter();
                orclParaBusRegNo.Value = bus_reg_no;
                orclParaBusRegNo.ParameterName = "bus_reg_num";

                cmd.Parameters.Add(orclParaBusRegNo);   

                OracleDataReader PolicyDetailsReader = cmd.ExecuteReader();

                while (PolicyDetailsReader.Read())
                {
                    if (!PolicyDetailsReader.IsDBNull(0)) { registeredPols.Add(PolicyDetailsReader.GetString(0)); } 
                }
                PolicyDetailsReader.Close();
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
            if (oraconn.State == System.Data.ConnectionState.Open)
            {
                oraconn.Close();
            }
        }

        return registeredPols;
    }

    public string getCompanyName(string bus_reg_no)
    {
        string companyName = "";

        try
        {
            if (oraconn.State == System.Data.ConnectionState.Closed)
            {
                oraconn.Open();
            }

            string sql = "select COMPANY_NAME " +
                         " from SHEDATA.SHE_POLICYMAST " +
                         " where BUS_REG_NO = :bus_reg_num ";

            using (OracleCommand cmd = new OracleCommand(sql, oraconn))
            {
                OracleParameter orclParaBusRegNo = new OracleParameter();
                orclParaBusRegNo.Value = bus_reg_no;
                orclParaBusRegNo.ParameterName = "bus_reg_num";

                cmd.Parameters.Add(orclParaBusRegNo);  
                OracleDataReader companyNameReader = cmd.ExecuteReader();

                while (companyNameReader.Read())
                {
                    if (!companyNameReader.IsDBNull(0)) { companyName = companyNameReader.GetString(0); }
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
            if (oraconn.State == System.Data.ConnectionState.Open)
            {
                oraconn.Close();
            }
        }

        return companyName;
    }

    public DataSet getPolicyDetails(string policyNo)
    {        

        DataSet ds = new DataSet();
        string sql = "select to_char(A.POLICY_START_DATE, 'YYYY-MM-DD') EXPR1, to_char(A.POLICY_END_DATE, 'YYYY-MM-DD') EXPR2, c.stf_name, c.stf_contact_no, c.stf_email " + 
            " from shedata.she_policymast A, shedata.she_policy_assigned_details B, SHEDATA.SHESTAFF C " + 
            " where A.POLICY_NO = b.POLICY_NO " + 
            " and B.ASSIGNED_TO_USER = c.stf_epf " + 
            " and A.POLICY_NO = :polNum " + 
            " and A.ACTIVE = 'Y' " + 
            " and c.active = 'Y' " + 
            " UNION" +
            " select  to_char(POLICY_START_DATE, 'YYYY-MM-DD'), to_char(POLICY_END_DATE, 'YYYY-MM-DD'), null, null, null " + 
            " from shedata.she_policymast where POLICY_NO not in ( " +
            " select POLICY_NO from shedata.she_policy_assigned_details ) and POLICY_NO = :polNum and ACTIVE = 'Y'";
        try
        {
            if (oraconn.State == System.Data.ConnectionState.Closed)
            {
                oraconn.Open();
            }

            OracleCommand cmd = new OracleCommand(sql, oraconn);
            cmd.Parameters.Add(new OracleParameter("polNum", policyNo));

            OracleDataAdapter adapter = new OracleDataAdapter(cmd);

            adapter.Fill(ds);
        }
        catch (Exception ex)
        {
            //log log1 = new log();
            //log1.write_log("Failed at getRegisteredPolicies" + ex.ToString());
            throw ex;
        }
        finally
        {
            if (oraconn.State == System.Data.ConnectionState.Open)
            {
                oraconn.Close();
            }
        }  
 
        //ds.Tables[0].Columns[0].ColumnName = "Policy StartDate";
        //ds.Tables[0].Columns[1].ColumnName = "Policy End Date";
        //ds.Tables[0].Columns[2].ColumnName = "SLIC Officer/s";
        //ds.Tables[0].Columns[3].ColumnName = "Contact No";
        //ds.Tables[0].Columns[4].ColumnName = "Email Address";

        return ds;
        
    }

    public int getEmployeesCount(string policyNo)
    {
        int count = 0;

        try
        {
            if (oraconn.State == System.Data.ConnectionState.Closed)
            {
                oraconn.Open();
            }

            string sql = "select count(*) from SLIC_NET.SHE_POLICY_DETAILS where POLICY_NUMBER = :pol_num";

            using (OracleCommand cmd = new OracleCommand(sql, oraconn))
            {
                OracleParameter orclParaPolNo = new OracleParameter();
                orclParaPolNo.Value = policyNo;
                orclParaPolNo.ParameterName = "pol_num";

                cmd.Parameters.Add(orclParaPolNo);  
                OracleDataReader companyNameReader = cmd.ExecuteReader();

                while (companyNameReader.Read())
                {
                    if (!companyNameReader.IsDBNull(0)) { count = companyNameReader.GetInt32(0); }
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
            if (oraconn.State == System.Data.ConnectionState.Open)
            {
                oraconn.Close();
            }
        }

        return count;
    }

    public DataSet getMembers(string policyNo)
    {

        DataSet ds = new DataSet();
        string sql = "select MEM_NUMBER, NIC_NUMBER, MOBILE_NUMBER " + 
                    " from SLIC_NET.SHE_POLICY_DETAILS " + 
                    " where policy_number = :polNum " + 
                    " order by MEM_NUMBER";
        try
        {
            if (oraconn.State == System.Data.ConnectionState.Closed)
            {
                oraconn.Open();
            }

            OracleCommand cmd = new OracleCommand(sql, oraconn);
            cmd.Parameters.Add(new OracleParameter("polNum", policyNo));

            OracleDataAdapter adapter = new OracleDataAdapter(cmd);

            adapter.Fill(ds);
        }
        catch (Exception ex)
        {
            //log log1 = new log();
            //log1.write_log("Failed at getRegisteredPolicies" + ex.ToString());
            throw ex;
        }
        finally
        {
            if (oraconn.State == System.Data.ConnectionState.Open)
            {
                oraconn.Close();
            }
        }

        //ds.Tables[0].Columns[0].ColumnName = "Member Number";
        //ds.Tables[0].Columns[1].ColumnName = "NIC Number";
        //ds.Tables[0].Columns[2].ColumnName = "Mobile Number";
        

        return ds;

    }

    public string getMemberNo(string polNo, string memNIC)
    {
        string memNo = "";

        try
        {
            if (oraconn.State == System.Data.ConnectionState.Closed)
            {
                oraconn.Open();
            }

            string sql = "select MEM_NUMBER from SLIC_NET.SHE_POLICY_DETAILS where POLICY_NUMBER = :polNum_v and NIC_NUMBER = :nicNum_v";

            using (OracleCommand cmd = new OracleCommand(sql, oraconn))
            {
                OracleParameter orclParaPolNo = new OracleParameter();
                orclParaPolNo.Value = polNo;
                orclParaPolNo.ParameterName = "polNum_v";

                OracleParameter orclParaNic = new OracleParameter();
                orclParaNic.Value = memNIC;
                orclParaNic.ParameterName = "nicNum_v";

                cmd.Parameters.Add(orclParaPolNo);
                cmd.Parameters.Add(orclParaNic);

                OracleDataReader MemNoReader = cmd.ExecuteReader();

                while (MemNoReader.Read())
                {
                    if (!MemNoReader.IsDBNull(0)) { memNo = MemNoReader.GetString(0); }
                }
                MemNoReader.Close();
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
            if (oraconn.State == System.Data.ConnectionState.Open)
            {
                oraconn.Close();
            }
        }

        return memNo;
    }

    public string getNicNo(string polNo, string memNo)
    {
        string memNic = "";

        try
        {
            if (oraconn.State == System.Data.ConnectionState.Closed)
            {
                oraconn.Open();
            }

            string sql = "select NIC_NUMBER from SLIC_NET.SHE_POLICY_DETAILS where POLICY_NUMBER = :polNum_v and MEM_NUMBER = :memNum_v";

            using (OracleCommand cmd = new OracleCommand(sql, oraconn))
            {
                OracleParameter orclParaPolNo = new OracleParameter();
                orclParaPolNo.Value = polNo;
                orclParaPolNo.ParameterName = "polNum_v";

                OracleParameter orclParaMemNo = new OracleParameter();
                orclParaMemNo.Value = memNo;
                orclParaMemNo.ParameterName = "memNum_v";

                cmd.Parameters.Add(orclParaPolNo);
                cmd.Parameters.Add(orclParaMemNo);

                OracleDataReader MemNoReader = cmd.ExecuteReader();

                while (MemNoReader.Read())
                {
                    if (!MemNoReader.IsDBNull(0)) { memNic = MemNoReader.GetString(0); }
                }
                MemNoReader.Close();
            }
        }
        catch (Exception ex)
        {
            string msgs = "ERROR:" + ex.Message;
            //log log1 = new log();
            //log1.write_log("Failed at getRegisteredPolicies" + ex.ToString());
            throw ex;
        }
        finally
        {
            if (oraconn.State == System.Data.ConnectionState.Open)
            {
                oraconn.Close();
            }
        }

        return memNic;
    }

    public string getUserType(string policyNo)
    {
        string usrTyp = "";

        try
        {
            if (oraconn.State == System.Data.ConnectionState.Closed)
            {
                oraconn.Open();
            }

            string sql = "select distinct decode(usr_type, 'St', 'SLIC Officer/s', 'Sp', 'Service Person') from " +
                        " shedata.she_policy_assigned_details A, SHEDATA.SHESTAFF B " +
                        " where A.assigned_to_user = B.stf_epf " +
                        "and a.policy_no = :policyNo_v";

            using (OracleCommand cmd = new OracleCommand(sql, oraconn))
            {
                OracleParameter orclParaPolicyNo = new OracleParameter();
                orclParaPolicyNo.Value = policyNo;
                orclParaPolicyNo.ParameterName = "policyNo_v";

                cmd.Parameters.Add(orclParaPolicyNo);
                OracleDataReader usrTypReader = cmd.ExecuteReader();

                while (usrTypReader.Read())
                {
                    if (!usrTypReader.IsDBNull(0)) { usrTyp = usrTypReader.GetString(0); }
                }
                usrTypReader.Close();
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
            if (oraconn.State == System.Data.ConnectionState.Open)
            {
                oraconn.Close();
            }
        }

        return usrTyp;
    }

}