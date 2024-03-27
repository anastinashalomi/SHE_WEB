<%@ Page Title="Home Page" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="SHE._Default" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    <style>
        .btn{
            color:white;
        }

        .btn:hover{
            background-color:darkblue;
            color:white;
        }
    </style>
      
    <script>
    </script>

    <main>
        <div class="container d-flex justify-content-center align-items-center vh-75">
            <div class="card" style="width: 75%">
                <asp:Image runat="server" Style="height: 300px;" ImageUrl="~/images/slic.jpg" />
                <div class="card-body row gap-3 p-5">
                    <asp:Button runat="server" CssClass="btn bg-she w-100" Text="NOTIFICATIONS" PostBackUrl="~/Notifications.aspx" ID="Notification" />
                    <asp:Button runat="server" CssClass="btn bg-she w-100" Text="INQUIRY" ID="Button1" onclick="Button1_Click"/>
                    <asp:Button runat="server" CssClass="btn bg-she w-100" Text="CLAIM HISTORY" PostBackUrl="~/Claim_History/claimhist1.aspx" ID="Button4" />
                    <asp:Button runat="server" CssClass="btn bg-she w-100" Text="SETTINGS" ID="Button3" />
                    <div class="d-flex justify-content-center">
                        <asp:Button runat="server" CssClass="btn bg-primary" Style="width: 50%" Text="EXIT" ID="Button2" />
                    </div>
                </div>
            </div>

        </div>
    </main>

</asp:Content>
