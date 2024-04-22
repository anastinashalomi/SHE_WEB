<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="ClaimSearch.aspx.cs" Inherits="SHE.ClaimPayment.ClaimSearch" %>

<%@ Import Namespace="SHE.Code" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">

    <style>
        .container {
            padding: 20px;
            background-color: white;
        }


        .btn-primary {
            background-color: #05ABB8;
            color: #fff;
            border-color: #05ABB8;
            margin-left: 8%;
        }

        .form-group {
            width: 300px;
        }

        .btn:hover {
                color: gainsboro; /* Maintain the same color on hover */
                background-color: #05ABB8; /* Maintain transparency on hover */
            }

        label {
            font-style: oblique;
        }

        .btn {
            //background-color: #05ABB8;
        }

        #exitbutton {
            width: 70px;
        }

        .panel-with-shadow {
            box-shadow: 0 0 10px rgba(0, 0, 0, 0.2);
            /* You can adjust the values to control the shadow appearance */
        }
    </style>

    <script type="text/javascript">
        function clientFunctionValidationFinished() {

            //alert("test");
            custom_alert('Successfully Processing wait...', 'Info');
            return true;

        }
    </script>

    <div class="container ">
        <div class="row justify-content-center">
            <div class="col-md-12 panel-with-shadow" style="max-width: 425px; background-color: #CFEFF2; padding: 10px; border: 1px solid #c0c0c0; border-radius: 10px; padding: 20px;">
                <h4 style="text-align: center;">SHE Claim Payment Details</h4>
                <br />
                <br />
                <asp:Label ID="lblAlertMessage" runat="server" ClientIDMode="Static" Style="display: none;"></asp:Label>
                <%--DefaultButton="claimhist_submit"--%>
                <asp:Panel runat="server" Class=" d-flex flex-column align-items-center ">

                    <div class="row mx-auto">
                        <div class="form-group col-sm-12 col-md-8 col-lg-6">
                            <label for="reference"><b>Policy Number:</b></label>
                            <input runat="server" type="text" class="form-control" id="policyno" />
                        </div>
                    </div>

                    <br />

                    <div class="row mx-auto">
                        <div class="form-group col-sm-12 col-md-8 col-lg-6">
                            <label for="epf" class="mr-2" id="epflbl" visible="true" runat="server"><b>Employee Number:</b></label>
                            <input runat="server" type="text" class="form-control" visible="true" id="epf" />
                        </div>
                    </div>

                    <br />

                    <div class="row mx-auto">
                        <div class="form-group col-sm-12 col-md-8 col-lg-6">
                            <label for="claimRef" class="mr-2"><b>Reference Number:</b></label>
                            <input runat="server" type="text" class="form-control" id="claimRef" />
                        </div>
                    </div>

                    <br />
                    <br />
                    <label class="col-sm-1 text-danger" runat="server" id="error" style="display: none">** Select From Date **</label>

                    <asp:Label runat="server" ID="error2" Style="color: red; padding: 4px;" Visible="false">** Insert CUs.Id and Reference Number  **</asp:Label>

                    <div class="d-flex justify-content-center">
                        <asp:Button ID="Back" runat="server" Text="Back" CssClass="btn mx-4 btn-outline-secondary" OnClick="back_Click" />
                        <asp:Button ID="claimPay_submit" runat="server" Text="Submit" CssClass="btn btn-primary mx-4" ClientIDMode="Static" OnClick="claimPayment_submit_Click" OnClientClick="return clientFunctionValidationFinished()" />
                    </div>

                </asp:Panel>

            </div>

        </div>

    </div>


    <script src="https://unpkg.com/sweetalert/dist/sweetalert.min.js"></script>
    <script type="text/javascript">
        function custom_alert(message, title) {
            if (!title)
                title = 'Alert';

            if (!message)
                message = 'No Message to Display.';

            if (title == 'Alert') {
                swal({
                    title: title,
                    text: message,
                    icon: "warning",
                    button: true,
                    closeOnClickOutside: false,
                });
            } else if (title == 'Success') {
                swal({
                    title: title,
                    text: message,
                    icon: "success",
                    button: false,
                    closeOnClickOutside: false,
                });
            } else if (title == 'Info') {
                swal({
                    title: title,
                    text: message,
                    icon: "info",
                    button: false,
                    closeOnClickOutside: false,
                });
            }
        }

    </script>


    <script src="https://ajax.googleapis.com/ajax/libs/jquery/3.5.1/jquery.min.js"></script>

    <script type="text/javascript">

        $(document).ready(function () {
            $('#claimPay_submit').click(function () {
                var ddlBranch = $('#<%=policyno.ClientID %>').val();
                var employeeNo = $('#<%=epf.ClientID %>').val();
                var referenceNo = $('#<%=claimRef.ClientID %>').val();
                if (ddlBranch == "" && employeeNo == "" && referenceNo == "") {
                    custom_alert("Please Enter Policy Number, Employee Number and Refernce No.", "Alert");
                    return false;
                } else if (ddlBranch == "") {
                    custom_alert("Please Enter Policy Number.", "Alert");
                    return false;
                } else if (employeeNo == "") {
                    custom_alert("Please Enter Employee Number.", "Alert");
                    return false;
                } else if (referenceNo == "") {
                    custom_alert("Please Enter Claim Reference Number.", "Alert");
                    return false;
                }
            });
        });

        $(document).ready(function () {
            // Show custom alerts
            var alertType = $("#lblAlertMessage").attr("data-alert-type");
            var message = $("#lblAlertMessage").text();

            if (alertType === "custom" && message !== "") {
                custom_alert(message, "Alert");
            }
        });
    </script>

</asp:Content>
