using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SHE.App_Code;

namespace SHE
{
    public partial class Login : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {

            }
            
        }

        protected void LoginButton_Click(object sender, EventArgs e)
        {
            var username = Username.Text.Trim();
            var password = Password.Text.Trim();

            LoginAuth loginAuth = new LoginAuth();
            bool auth =  loginAuth.as400_login(username,password);

            if (auth)
            {
                Session["LoggedUser"] = username;
                Session.Timeout = 60;
                Response.Redirect("~/default.aspx");

            }
            else
            {
                //Response.Redirect("~/login.aspx");
                LoginError.Text = "<strong>Your Username or password is incorrect</strong>";
                LoginErrVisibility.Visible = true;
                Session.Clear(); // Clear all session variables
                Session.Abandon(); // End the current session
            }
         

        }
    }
}

//SEC6665
//Newhope@2022