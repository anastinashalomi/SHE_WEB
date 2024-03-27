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
            background-color: #4dc6d0;
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
                <div class="card-header d-flex justify-content-center align-items-center">
                    <asp:Image runat="server" ImageUrl="~/images/login.png" />
                    <h3>Login</h3>
                </div>
                <div class="card-body p-3 g-2 row justify-content-center">
                    <label for="username" class="form-label">Username:</label>
                    <asp:TextBox runat="server" Placeholder="Username" ID="Username" CssClass="form-control"></asp:TextBox>
                    <label for="password" class="form-label">Password:</label>
                    <asp:TextBox runat="server" Placeholder="Password" TextMode="Password" ID="Password" CssClass="form-control"></asp:TextBox>
                </div>
                <div class="card-footer row justify-content-center">
                    <asp:Button runat="server" class="btn btn-primary" Style="width: 100%" Text="Login" ID="LoginButton" OnClick="LoginButton_Click" />
                </div>
            </div>
        </div>
    </form>
</body>
</html>
