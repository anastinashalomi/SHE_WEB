<%@ Page Title="" EnableEventValidation="false"  Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Notifications.aspx.cs" Inherits="SHE.Notifications" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
      <style>
        .btn{
            color:white;
        }

        .btn:hover{
            background-color:darkblue;
            color:white;
        }
    </style>

    <main>
        <div class="container">
            <asp:Panel runat="server" ID="PanelOne" CssClass="mt-5">
                <ul class="nav nav-pills nav-justified">
                    <li class="nav-item row p-3">
                        <%--<a class="nav-link active" href="#">Notifications</a>--%>
                        <asp:Button runat="server" CssClass="btn btn-primary w-100" Enabled="false" Text="Notifications" />
                    </li>
                    <li class="nav-item row p-3">
                        <asp:Button runat="server" CssClass="btn btn-dark w-100 " Enabled="false" Text="Details" />
                    </li>
                </ul>
                <div>
                    <asp:GridView runat="server" class="table table-hover mt-5" ID="NotificationGrid" AutoGenerateColumns="false" OnRowDataBound="NotificationGrid_RowDataBound" OnSelectedIndexChanged="NotificationGrid_SelectedIndexChanged">
                        <Columns>
                            <asp:TemplateField HeaderStyle-CssClass="d-flex justify-content-center align-items-center" HeaderText="Notifications">
                                <ItemTemplate>
                                    <div class="d-flex gap-3 align-items-center">
                                        <div>
                                            <asp:Image runat="server" ID="NotificationImage" Width="40px" Height="40px" />
                                        </div>
                                        <div class="d-flex flex-column">
                                            <asp:Label runat="server" Text='<%# Eval("AdmittedType") %>' ID="AdmitType"></asp:Label>
                                            <asp:Label runat="server" Text='<%# Eval("PatientName") %>' ID="PatientName"></asp:Label>
                                            <asp:Label runat="server" Text='<%# Eval("Hospital") %>' ID="Hospital"></asp:Label>
                                        </div>
                                    </div>
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                    </asp:GridView>
                </div>
            </asp:Panel>
            <asp:Panel runat="server" ID="PanelTwo" CssClass="mt-5">
                <ul class="nav nav-pills nav-justified">
                    <li class="nav-item row p-3">
                        <%--<a class="nav-link active" href="#">Notifications</a>--%>
                        <asp:Button runat="server" CssClass="btn btn-dark w-100 " ID="BackButton" OnClick="BackButton_Click" Text="Notifications" />
                    </li>
                    <li class="nav-item row p-3">
                        <asp:Button runat="server" CssClass="btn btn-primary w-100" Enabled="false" Text="Details" />
                    </li>
                </ul>
                <div class="vh-75 d-flex justify-content-center mt-5">
                    <div class="card w-75 p-2" style="height: 400px">
                        <div class="card-header d-flex justify-content-center gap-2">
                            <asp:Button runat="server" Text="ACCEPT" CssClass="btn bg-she" />
                            <asp:Button runat="server" Text="REASSIGN" CssClass="btn bg-she" />
                        </div>
                        <div class="card-body">Content</div>
                        <div class="card-footer d-flex justify-content-evenly">
                            <asp:Button runat="server" Text="BACK" OnClick="BackButton_Click" CssClass="btn bg-she button-style" />
                            <asp:Button runat="server" Text="CLAIM HISTORY" CssClass="btn bg-she button-style" />
                            <asp:Button runat="server" Text="CLAIM PAYMENT" CssClass="btn bg-she button-style" />
                        </div>
                    </div>

                </div>
            </asp:Panel>
        </div>
    </main>
</asp:Content>
