
<%@ Page Title="" EnableEventValidation="false" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="claimhistory1_Redirect.aspx.cs" Inherits="SHE.Claim_History.claimhistory1_Redirect" %>
<%@ Import Namespace="SHE.Code"%>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">

    <style>
        .container {
            padding:20px;
            background-color: white;
        }

        .btn-primary {
            background-color: #4dc6d0;
            color: #fff;
            border-color: #4dc6d0;
            margin-left: 8%;
        }

        .form-group {
            width: 300px;
        }

        label {
            font-style: oblique;
        }

        .btn {
            background-color: #4dc6d0;
        }

        #exitbutton {
            width: 70px;
        }
    
    .panel-with-shadow {
        box-shadow: 0 0 10px rgba(0, 0, 0, 0.2);
        /* You can adjust the values to control the shadow appearance */
    }
</style>

    <div class="container ">
        <div class="row justify-content-center">
            <div class="col-md-12 panel-with-shadow" style="width: 425px; background-color: white; padding: 10px; border: 1px solid #c0c0c0;">
                <h4 style="text-align: center;">SHE Claim Payment History Details</h4>
                <br />
                <br />


                <asp:Panel runat="server" DefaultButton="claimhist_submit" Class=" d-flex flex-column align-items-center ">

                    <div class="row mx-auto">
                        <div class="form-group col-sm-12 col-md-8 col-lg-6">
                            <label for="reference"><b>Policy Number:</b></label>
                            <input runat="server" type="text" class="form-control" id="policyno" />
                        </div>
                    </div>

                    <br />

                    <div class="row mx-auto">
                        <div class="form-group col-sm-12 col-md-8 col-lg-6">
                            <label for="epf" class="mr-2"><b>Employee Number:</b></label>
                            <input runat="server" type="text" class="form-control" id="epf" />
                        </div>
                    </div>

                    <br />
                    <br />

                    <asp:Label runat="server" ID="error1" Style="color: red; padding: 4px;" Visible="false">** Select From Date **</asp:Label>
                    <asp:Label runat="server" ID="error2" Style="color: red; padding: 4px;" Visible="false">** Insert Date and Reference Number  **</asp:Label>

                    <div class="d-flex justify-content-center">
                        <asp:Button ID="claimhist_submit" runat="server" Text="Submit" CssClass="btn btn-primary mx-4" onclick="claimhist_submit_Click"/>
                        <asp:Button ID="exitbutton" runat="server" Text="Exit" CssClass="btn btn-primary mx-4" OnClick="exitbutton_Click"/>
                    </div>

                </asp:Panel>

            </div>

        </div>

    </div>

</asp:Content>
