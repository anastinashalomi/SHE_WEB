<%@ Page Title="Home Page" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="SHE._Default" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    <style>
        .btn {
            color: white;
          
        }

        .btn:hover {
                background-color: darkblue;
                color: white;
        }

        .image-container {
            display: flex;
            justify-content: center;
            align-items: center;
            height: 25vh; /* Adjust the height of the container as needed */
        }

        .custom-rounded {
            border-radius: 10px; /* Adjust the border-radius value as needed */
        }

        .custom-rounded {
            border-radius: 0px; /* Adjust the border-radius value as needed */
        }

        .mx-auto {
            margin-left: auto;
            margin-right: auto;
        }

        .custom-button {
            font-size: 18px; /* Change the font size as needed */
            background-color: gray; /* Change the background color as needed */
            color: white; /* Change the text color as needed */
            /* Add any other custom styles here */
        }




    </style>

    <script>
</script>

    <main>
        <div class="container d-flex justify-content-center align-items-center vh-75">
            <div class="card" style="width: 75%">
                <div class="image-container">
                    <asp:Image runat="server" Style="height: 150px; width: 200px;" ImageUrl="~/images/GENERAL EST LOGO 1.png" />
                </div>

                <div class="card-body row gap-3 p-5">

                    <asp:Button runat="server" CssClass="btn custom-button mx-auto" Style="width: 50%; border-radius: 5px; background-color: #4dc6d0; color: white; font-weight: bold; font-size: 16px; border-color: #4dc6d0;" Text="NOTIFICATIONS" PostBackUrl="~/Notifications.aspx" ID="Button5" />
                    <asp:Button runat="server" CssClass="btn custom-button mx-auto" Style="width: 50%; border-radius: 5px; background-color: #4dc6d0; color: white; font-weight: bold; font-size: 16px; border-color: #4dc6d0;" Text="INQUIRY" ID="Button1" OnClick="Button1_Click" />
                    <asp:Button runat="server" CssClass="btn custom-button mx-auto" Style="width: 50%; border-radius: 5px; background-color: #4dc6d0; color: white; font-weight: bold; font-size: 16px; border-color: #4dc6d0;" Text="CLAIM HISTORY" PostBackUrl="~/Claim_History/claimhist1.aspx" ID="Button4" />
                    <%--<asp:Button runat="server" CssClass="btn custom-button mx-auto" Style="width: 50%; border-radius: 0; background-color: transparent; color: black; font-weight: bold; font-size: 16px; border-color: #4dc6d0;" Text="SETTINGS" ID="Button3" />--%>
                    <asp:Button runat="server" CssClass="btn custom-button mx-auto" Style="width: 50%; border-radius: 5px; background-color: red; color: white; font-weight: bold; font-size: 16px; border-color: red;" Text="EXIT" ID="Button6" />



                    <%--     <div class="d-flex justify-content-center">
                        <asp:Button runat="server"CssClass="btn bg-primary custom-rounded mx-auto" Style="width: 50%" Text="EXIT" ID="Button2" />
                    </div>--%>
                </div>
            </div>

        </div>
    </main>

</asp:Content>
