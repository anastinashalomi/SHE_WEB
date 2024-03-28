<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Login.aspx.cs" Inherits="SHE.Login" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <asp:PlaceHolder runat="server">
        <%: Scripts.Render("~/bundles/modernizr") %>
    </asp:PlaceHolder>

    <webopt:BundleReference runat="server" Path="~/Content/css" />
    <link href="~/favicon.ico" rel="shortcut icon" type="image/x-icon" />

    <style>
        .bg-she {
            background-color: #ffffff;
        }

        .login-header {
            display: flex;
            flex-direction: column;
            align-items: center;
            justify-content: center;
            text-align: center; /* Optionally, to center the text horizontally */
          
        }

            .login-header h2,
            .login-header h6 {
                margin: 0;
                padding: 5px 0;
                line-height: 1;
            }

        .no-curve {
            border-radius: 0; /* Remove border-radius */
        }

        .with-icon {
            padding-left: 50px;  /* Adjust the padding to accommodate the icon */
            background-image: url('Images/charm_person.png'); /* Replace 'icon.png' with the path to your icon image */
            background-repeat: no-repeat;
            background-position: 10px center; /* Adjust the position of the icon */
            background-size: 25px 25px;
        }

        .password-with-icons{
            padding-left: 50px;  /* Adjust the padding to accommodate the icon */
            background-image: url('Images/carbon_password.png'); /* Replace 'icon.png' with the path to your icon image */
            background-repeat: no-repeat;
            background-position: 10px center; /* Adjust the position of the icon */
            background-size: 25px 25px;

        }


    </style>
</head>
<body>
    <form runat="server">
        <asp:ScriptManager runat="server">
            <Scripts>
                <%--To learn more about bundling scripts in ScriptManager see https://go.microsoft.com/fwlink/?LinkID=301884 --%>
                <%--Framework Scripts--%>
                <asp:ScriptReference Name="MsAjaxBundle" />
                <asp:ScriptReference Name="jquery" />
                <asp:ScriptReference Name="WebForms.js" Assembly="System.Web" Path="~/Scripts/WebForms/WebForms.js" />
                <asp:ScriptReference Name="WebUIValidation.js" Assembly="System.Web" Path="~/Scripts/WebForms/WebUIValidation.js" />
                <asp:ScriptReference Name="MenuStandards.js" Assembly="System.Web" Path="~/Scripts/WebForms/MenuStandards.js" />
                <asp:ScriptReference Name="GridView.js" Assembly="System.Web" Path="~/Scripts/WebForms/GridView.js" />
                <asp:ScriptReference Name="DetailsView.js" Assembly="System.Web" Path="~/Scripts/WebForms/DetailsView.js" />
                <asp:ScriptReference Name="TreeView.js" Assembly="System.Web" Path="~/Scripts/WebForms/TreeView.js" />
                <asp:ScriptReference Name="WebParts.js" Assembly="System.Web" Path="~/Scripts/WebForms/WebParts.js" />
                <asp:ScriptReference Name="Focus.js" Assembly="System.Web" Path="~/Scripts/WebForms/Focus.js" />
                <asp:ScriptReference Name="WebFormsBundle" />
                <%--Site Scripts--%>
            </Scripts>        
        </asp:ScriptManager>


        <div class="container-fluid d-flex justify-content-center align-items-center bg-she min-vh-100">
            <div class="card p-3 w-60">
                <div runat="server" class="alert alert-danger justify-content-center d-flex" id="LoginErrVisibility" visible="false">
                    <asp:Label runat="server" ID="LoginError" Text=""></asp:Label>
                </div>
                <div class="card-header d-flex justify-content-center align-items-center" style="background-color: #CFEFF2;">

                    <div class="login-header">
                        <h1>Login</h1>
                        <h6>Login to your account here</h6>
                    </div>
                </div>
                <div class="card-body p-3 g-2 row justify-content-center">                    
                    <asp:TextBox runat="server" Placeholder="Enter your user name" ID="Username" CssClass="form-control with-icon"></asp:TextBox>                 
                  
                    <asp:TextBox runat="server" Placeholder="Enter your password" TextMode="Password" ID="Password" CssClass="form-control password-with-icons"></asp:TextBox>
                </div>
                <div class="card-footer d-flex justify-content-center align-items-center" style="background-color: #CFEFF2;">
                   <asp:Button runat="server" class="btn btn-primary" Style="width: 20%; border-radius: 5px; background-color: #4dc6d0; color: white; font-weight: bold; font-size: 16px; border-color: #4dc6d0" Text="Login" ID="LoginButton" OnClick="LoginButton_Click" />
                </div>
            </div>
        </div>
    </form>

</body>
</html>
