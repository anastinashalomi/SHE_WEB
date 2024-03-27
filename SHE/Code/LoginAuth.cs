using System.Data;
using System.Data.Odbc;
using System.Configuration;
using System.Data.OracleClient;

namespace SHE.App_Code
{
    public class LoginAuth
    {

        OracleConnection oconn = new OracleConnection(ConfigurationManager.AppSettings["OracleDB"]);

        public bool as400_login(string user_id, string passwrd)
        {
            bool result = false;
            string passwd = fix_f_password(passwrd);
            try
            {
                if (oconn.State != ConnectionState.Open)
                {
                    oconn.Open();
                }
                // oconn.Open();

                string sql = "Select COUNT(*) from INTRANET.INTUSR where Userid = :userid AND MAINPASS = :password AND  USRTYP  NOT IN ('A','O')";
                using (OracleCommand com = new OracleCommand(sql, oconn))
                {
                    OracleParameter oUsrName = new OracleParameter();
                    oUsrName.Value = user_id;
                    oUsrName.ParameterName = "userid";

                    OracleParameter oPassword = new OracleParameter();
                    oPassword.Value = passwd;
                    oPassword.ParameterName = "password";

                    com.Parameters.Add(oUsrName);
                    com.Parameters.Add(oPassword);
                    OracleDataReader reader = com.ExecuteReader();
                    int kk = 0;
                    while (reader.Read())
                    {
                        kk = int.Parse(reader[0].ToString());
                    }
                    // int kk = Convert.ToInt32(com.ExecuteReader());

                    if (kk > 0)
                    {
                        result = true;
                    }
                }

            }
            catch (OdbcException ee)
            {
                string errorMsg = ee.ToString();
            }
            finally
            {
                oconn.Close();
            }
            return result;
        }


        private string fix_f_password(string passwd)
        {
            string result = passwd;

            char[] arr = new char[12];
            int len = passwd.Length;

            for (int i = 0; i < arr.Length; i++)
            {
                if (i < passwd.Length)
                {
                    arr[i] = passwd[i];

                }
                else
                {
                    arr[i] = ' ';
                }
            }

            result = arr[10].ToString().Trim() + arr[11].ToString().Trim() + arr[6].ToString().Trim() + arr[7].ToString().Trim() + arr[2].ToString().Trim() + arr[3].ToString().Trim() + arr[8].ToString().Trim() + arr[9].ToString().Trim() + arr[4].ToString().Trim() + arr[5].ToString().Trim() + arr[0].ToString().Trim() + arr[1].ToString().Trim();

            return result;
        }
    }
}