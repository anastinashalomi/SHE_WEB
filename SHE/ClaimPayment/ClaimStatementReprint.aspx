<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="ClaimStatementReprint.aspx.cs" Inherits="SHE.ClaimPayment.ClaimStatementReprint" Async="true" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">

    <style>
        .btn {
            color: gainsboro;
            border: 2px solid #05ABB8;
            padding: 2px 5px;
            border-radius: 5px;
            margin-right: 10px;
        }

            .btn:hover {
                color: gainsboro; /* Maintain the same color on hover */
                background-color: #05ABB8; /* Maintain transparency on hover */
            }

        .mt-5 {
            margin-top: 5px;
        }

        .btn-container {
            display: flex;
            justify-content: flex-start;
            margin-top: 10px;
        }

        .btn-claimPaymet {
            background-color: #05ABB8;
            color: white;
            border: 2px solid #05ABB8;
            width: 115px;
        }
    </style>

     <asp:Label ID="lblAlertMessage" runat="server" ClientIDMode="Static" Style="display: none;"></asp:Label>
    <%-- medicare statement panel --%>
    <asp:Panel ID="panel3" runat="server" Visible="true">
        <div class="container">
            <div class="row justify-content-center">
                <div class="col-md-12 panel-with-shadow" style="max-width: 700px; background-color: #CFEFF2; padding: 10px; border: 1px solid #c0c0c0; border-radius: 10px; padding: 20px;">
                    <div style="text-align: center;">
                        <!-- Centering the image -->
                        <asp:ImageButton runat="server" ImageUrl="~/images/New_Slic_Logo.png" Style="width: 80px; height: 50px;" />
                    </div>
                    <h4 style="text-align: center;">Health Plus Medicare Hospital Statement</h4>
                    <br />

                    <%--DefaultButton="claimhist_submit"--%>

                    <div class="container" style="width: 80%;">
                        <div class="mx-auto">
                            <div class="row mb-2">
                                <div class="col-sm-4 ">Reference No :</div>
                                <div class="col-sm-8 " runat="server" id="refNo1"></div>
                            </div>
                            <div class="row mb-2">
                                <div class="col-sm-4 ">Name of the Insured:</div>
                                <div class="col-sm-8 " runat="server" id="nameInsu"></div>
                            </div>
                            <div class="row mb-2">
                                <div class="col-sm-4 ">Hospital Name :</div>
                                <div class="col-sm-8 " runat="server" id="hosName"></div>
                            </div>
                            <div class="row mb-2">
                                <div class="col-sm-4 ">Policy No :</div>
                                <div class="col-sm-8 " runat="server" id="polNo2"></div>
                            </div>
                            <div class="row mb-2">
                                <div class="col-sm-4 ">B.H.T. No :</div>
                                <div class="col-sm-8 " runat="server" id="bhtNo2"></div>
                            </div>
                            <div class="row mb-2">
                                <div class="col-sm-4">Bill No :</div>
                                <div class="col-sm-8 " runat="server" id="billNo2"></div>
                            </div>
                            <div class="row mb-2">
                                <div class="col-sm-4 ">Total Bill Amount(Rs.): </div>
                                <div class="col-sm-8 " runat="server" id="totStAm"></div>
                            </div>
                            <div class="row mb-2">
                                <div class="col-4">Amount Payable by Insurance(Rs.) :</div>
                                <div class="col-8 " runat="server" id="totStaePayAm"></div>
                            </div>
                            <div class="row mb-2">
                                <div class="col-4">Remark 1 :</div>
                                <div class="col-8 " runat="server" id="rema1"></div>
                            </div>
                            <div class="row mb-2">
                                <div class="col-4">Remark 2 :</div>
                                <div class="col-8 " runat="server" id="rema2"></div>
                            </div>
                            <br />
                            <div class="card-footer d-flex justify-content-evenly">
                                <asp:Button runat="server" ID="Button5" Text="Back" CssClass="btn btn-claimPaymet" style="width: 100%; max-width: 180px;" OnClick="back_click" />
                                <asp:Button runat="server" ID="Button4" Text="Reprint Receipt" CssClass="btn btn-claimPaymet" style="width: 100%; max-width: 180px;" OnClick="down_recei_pdf" Visible="true" />
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </asp:Panel>
    <%-- end of statement --%>



    <script src="https://unpkg.com/sweetalert/dist/sweetalert.min.js"></script>
    <script type="text/javascript">

        

        function custom_alert(message, title) {

            var title = $("#lblAlertMessage").attr("data-alert-title");
            var message = $("#lblAlertMessage").attr("data-alert-message");

            if (!title) title = 'Alert';
            if (!message) message = 'No Message to Display.';

            //if (!title)
            //    title = 'Alert';

            //if (!message)
            //    message = 'No Message to Display.';

            var imageUrl = '<%= ResolveUrl("~/images/process3.gif") %>';

            if (title === 'Alert') {
                swal({
                    title: title,
                    text: message,
                    icon: "warning",
                    button: true,
                    closeOnClickOutside: false,
                }).then(function () {
                    // Redirect to index page
                    window.location.href = "/Default.aspx";
                });
            } else if (title === 'Success') {
                 swal({
                    title: title,
                    text: message,
                    icon: "success",
                    button: "OK",
                    closeOnClickOutside: false,
                }); then(function () {
                    // Redirect to index page
                    window.location.href = "/Default.aspx";
                });
            } else if (title === 'Info') {
                 swal({
                    title: title,
                    text: message,
                    //icon: "Images/process3.gif",
                     icon: imageUrl,
                    button: false,
                    closeOnClickOutside: false,
                });
            }
        }

        function clearAlert() {
            document.getElementById('lblAlertMessage').innerText = ''; // Replace 'alertMessage' with the ID of your alert message element
            if (typeof swal !== 'undefined' && swal.isVisible()) {
                swal.close(); // Close any active sweet alert popups
            }
        }

    </script>


    <script src="https://ajax.googleapis.com/ajax/libs/jquery/3.5.1/jquery.min.js"></script>

    <script type="text/javascript">

        //$(document).ready(function () {
        //    $('#claimPay_submit').click(function () {

        //    });
        //});

        $(document).ready(function () {
            // Show custom alerts
            var alertType = $("#lblAlertMessage").attr("data-alert-type");
            var message = $("#lblAlertMessage").text();

            if (alertType !== "" && message !== "") {
                custom_alert(message, alertType);
            }
        });
    </script>


</asp:Content>
