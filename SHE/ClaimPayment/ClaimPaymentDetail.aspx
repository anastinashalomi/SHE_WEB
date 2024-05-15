<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="ClaimPaymentDetail.aspx.cs" Inherits="SHE.ClaimPayment.ClaimPaymentDetail" Async="true" %>

<%@ Import Namespace="SHE.Code" %>

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
    <script>
        // VALIDATION SCRIPT FOR NUMBER.
        function isNumberKey(evt) {
            var iKeyCode = (evt.which) ? evt.which : evt.keyCode
            if (iKeyCode != 46 && iKeyCode > 31 && (iKeyCode < 48 || iKeyCode > 57))
                return false;

            return true;
        }

    </script>

    <script type="text/javascript">

        function Comma(Num) {
            Num += '';
            Num = Num.replace(',', ''); Num = Num.replace(',', ''); Num = Num.replace(',', '');
            Num = Num.replace(',', ''); Num = Num.replace(',', ''); Num = Num.replace(',', '');
            x = Num.split('.');
            x1 = x[0];
            x2 = x.length > 1 ? '.' + x[1] : '';
            var rgx = /(\d+)(\d{3})/;
            while (rgx.test(x1))
                x1 = x1.replace(rgx, '$1' + ',' + '$2');
            return x1 + x2;
        }

        function clientFunctionValidation() {


            //validate executve name
            var execName = $('#<%=execuName.ClientID %>').val();

            if (execName == "") {
                custom_alert2('Please enter Executive Name.', 'Alert');
                return false;
            }

            //validate total bill amount
            var totalBill = $('#<%=totBill.ClientID %>').val();
            totalBill = totalBill.replace(/,/g, '');

            if (totalBill == "") {
                custom_alert2('Please enter total bill amount.', 'Alert');
                return false;
            }

            //validate total paid amount
            var paidAmount = $('#<%=paidAmo.ClientID %>').val();
            paidAmount = paidAmount.replace(/,/g, '');

            if (paidAmount == "") {
                custom_alert2('Please enter total bill amount.', 'Alert');
                return false;
            }

            var totalBillValue = parseFloat(totalBill);
            var totalPaidValue = parseFloat(paidAmount);

            if (totalPaidValue > totalBillValue) {
                custom_alert2('Total bill amount cant less than paid amount.', 'Alert');
                return false;
            }

        }
    </script>

    <asp:Label ID="lblAlertMessage" runat="server" ClientIDMode="Static" Style="display: none;"></asp:Label>
    <asp:Panel ID="panel1" runat="server" Visible="true">
        <div class="container">
            <div class="row justify-content-center">
                <div class="col-md-12 panel-with-shadow" style="max-width: 700px; background-color: #CFEFF2; padding: 10px; border: 1px solid #c0c0c0; border-radius: 10px; padding: 20px;">
                    <h4 style="text-align: center;">SHE Claim Details</h4>
                    <br />

                    <%--DefaultButton="claimhist_submit"--%>

                    <div class="container" style="width: 80%;">
                        <div class="mx-auto">
                            <div class="row">
                                <div class="col-sm-4 ">Reference No</div>
                                <div class="col-sm-8 " runat="server" id="ClaimReferenceLabel"></div>
                            </div>
                            <div class="row">
                                <div class="col-sm-4 ">Claim No</div>
                                <div class="col-sm-8 " runat="server" id="ClaimNo"></div>
                            </div>
                            <div class="row">
                                <div class="col-sm-4 ">Hospital</div>
                                <div class="col-sm-8 " runat="server" id="hospital"></div>
                            </div>
                            <div class="row">
                                <div class="col-sm-4 ">Room Number</div>
                                <div class="col-sm-8 " runat="server" id="roomNo"></div>
                            </div>
                            <div class="row">
                                <div class="col-sm-4">Admitted Date</div>
                                <div class="col-sm-8 " runat="server" id="admitedDate"></div>
                            </div>
                            <div class="row">
                                <div class="col-sm-4 ">Discharge Date</div>
                                <div class="col-sm-8 " runat="server" id="dischargeDate"></div>
                            </div>
                            <div class="row">
                                <div class="col-4">Patient Name</div>
                                <div class="col-8 " runat="server" id="patientName"></div>
                            </div>
                            <div class="row">
                                <div class="col-sm-4 ">Bill Amount</div>
                                <div class="col-sm-8 " runat="server" id="billAmount"></div>
                            </div>
                            <div class="row">
                                <div class="col-sm-4 ">Paid Amount</div>
                                <div class="col-sm-8 " runat="server" id="paidAmount"></div>
                            </div>
                            <div class="row">
                                <div class="col-sm-4 ">Claim Status</div>
                                <div class="col-sm-8 " runat="server" id="claimStatus"></div>
                            </div>
                            <div class="row">
                                <div class="col-sm-4 ">Job Type</div>
                                <div class="col-sm-8 " runat="server" id="jobType"></div>
                            </div>
                            <br />
                            <div class="card-footer d-flex justify-content-evenly">
                                <asp:Button runat="server" ID="btnBack" Text="Back" CssClass="btn btn-claimPaymet" OnClick="back_click" />
                                <asp:Button runat="server" ID="btnClaimHistory" Text="Proceed" CssClass="btn btn-claimPaymet" OnClick="claimPayment_submit_Click" />
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </asp:Panel>

    <%-- for submit panel --%>
    <asp:Panel ID="panel2" runat="server" Visible="False">
        <br />
        <h4 style="text-align: center;">Add/Update SHE Claim Payment Details</h4>
        <br />
        <div class="container">
            <div class="row justify-content-center">
                <div class="col-md-12 panel-with-shadow" style="max-width: 600px; background-color: #CFEFF2; padding: 10px; border: 1px solid #c0c0c0; border-radius: 10px; padding: 20px;">

                    <asp:Label ID="Label1" runat="server" ClientIDMode="Static" Style="display: none;"></asp:Label>
                    <%--DefaultButton="claimhist_submit"--%>

                    <div class="container" style="width: 80%;">
                        <div class="mx-auto">
                            <div class="row">
                                <div class="col-sm-4 ">Reference No</div>
                                <div class="col-sm-8 " runat="server" id="refNoo"></div>
                            </div>
                            <div class="row">
                                <div class="col-sm-4 ">Employee No</div>
                                <div class="col-sm-8 " runat="server" id="emplNo"></div>
                            </div>
                            <div class="row">
                                <div class="col-sm-4 ">Policy No</div>
                                <div class="col-sm-8" runat="server" id="polNo"></div>
                            </div>
                            <div class="row">
                                <div class="col-sm-4 ">Hospital</div>
                                <div class="col-sm-8 " runat="server" id="hosp"></div>
                            </div>
                            <div class="row">
                                <div class="col-sm-4 ">Admitted Date</div>
                                <div class="col-sm-8 " runat="server" id="addDate"></div>
                            </div>
                            <div class="row">
                                <div class="col-sm-4 ">Discharge Date</div>
                                <div class="col-sm-8 " runat="server" id="disDate"></div>
                            </div>
                            <div class="row">
                                <div class="col-sm-4">Room No</div>
                                <div class="col-sm-8 " runat="server" id="rNo"></div>
                            </div>

                            <br />

                        </div>
                    </div>
                </div>
            </div>
        </div>

        <br />

        <div class="container">
            <div class="row justify-content-center">
                <div class="col-md-12 panel-with-shadow" style="max-width: 600px; background-color: #CFEFF2; padding: 10px; border: 1px solid #c0c0c0; border-radius: 10px; padding: 20px;">

                    <br />
                    <asp:Label ID="Label2" runat="server" ClientIDMode="Static" Style="display: none;"></asp:Label>
                    <%--DefaultButton="claimhist_submit"--%>

                    <div class="container" style="width: 80%;">
                        <div class="mx-auto">

                            <div class="form-group row">
                                <label for="lblreferenceNo" class="col-sm-6 col-form-label ">Claim No </label>
                                <div class="col-sm-6">
                                    <input type="text" class="form-control" readonly="readonly" runat="server" id="claNo" placeholder="claim no" style="padding-bottom: 10px; margin-bottom: 10px;">
                                </div>
                            </div>
                            <div class="form-group row">
                                <label for="lblreferenceNo" class="col-sm-6 col-form-label ">Ailment </label>
                                <div class="col-sm-6">
                                    <input type="text" class="form-control" runat="server" id="alment" placeholder="Ailment" style="padding-bottom: 10px; margin-bottom: 10px;">
                                </div>
                            </div>
                            <div class="form-group row">
                                <label for="lblreferenceNo" class="col-sm-6 col-form-label ">Executive Name</label>
                                <div class="col-sm-6">
                                    <input type="text" class="form-control" readonly="readonly" runat="server" id="execuName" placeholder="Executive Name" style="padding-bottom: 10px; margin-bottom: 10px;">
                                </div>
                            </div>
                            <div class="form-group row">
                                <label for="lblreferenceNo" class="col-sm-6 col-form-label">Total Bill</label>
                                <div class="col-sm-6">
                                    <input type="text" class="form-control" onkeypress="return isNumberKey(event)" onkeyup="javascript:this.value=Comma(this.value);" runat="server" id="totBill" placeholder="Total Bill" style="padding-bottom: 10px; margin-bottom: 10px;">
                                </div>
                            </div>
                            <div class="form-group row">
                                <label for="lblreferenceNo" class="col-sm-6 col-form-label ">Paid Amount</label>
                                <div class="col-sm-6">
                                    <input type="text" class="form-control" onkeypress="return isNumberKey(event)" onkeyup="javascript:this.value=Comma(this.value);" runat="server" id="paidAmo" placeholder="Paid Amount" style="padding-bottom: 10px; margin-bottom: 10px;">
                                </div>
                            </div>
                            <div class="form-group row">
                                <label for="lblreferenceNo" class="col-sm-6 col-form-label ">Remark 1</label>
                                <div class="col-sm-6">
                                    <input type="text" class="form-control" runat="server" id="R1" placeholder="Remark1" style="padding-bottom: 10px; margin-bottom: 10px;">
                                </div>
                            </div>
                            <div class="form-group row">
                                <label for="lblreferenceNo" class="col-sm-6 col-form-label ">Remark 2</label>
                                <div class="col-sm-6">
                                    <input type="text" class="form-control" runat="server" id="R2" placeholder="Remark2" style="padding-bottom: 10px; margin-bottom: 10px;">
                                </div>
                            </div>
                            <div class="form-group row">
                                <label for="lblreferenceNo" class="col-sm-6 col-form-label ">Claim Status</label>
                                <div class="col-sm-6">
                                    <input type="text" class="form-control" runat="server" readonly="readonly" id="claimSta" placeholder="Claim Status" style="padding-bottom: 10px; margin-bottom: 10px;">
                                </div>
                            </div>

                            <br />
                            <div class="card-footer d-flex justify-content-evenly">
                                <asp:Button runat="server" ID="Button1" Text="Back" CssClass="btn btn-claimPaymet" OnClick="back_click2" />
                                <asp:Button runat="server" ID="Button3" Text="Main Menu" CssClass="btn btn-claimPaymet" OnClick="back_main" />
                                <asp:Button runat="server" ID="Button2" Text="Submit" CssClass="btn btn-claimPaymet" OnClick="claim_payment_update" OnClientClick="return clientFunctionValidation()" />

                            </div>

                        </div>
                    </div>
                </div>
            </div>
        </div>

        <br />
    </asp:Panel>

    <%-- medicare statement panel --%>
    <asp:Panel ID="panel3" runat="server" Visible="false">
        <div class="container">
            <div class="row justify-content-center">
                <div class="col-md-12 panel-with-shadow" style="max-width: 700px; background-color: #CFEFF2; padding: 10px; border: 1px solid #c0c0c0; border-radius: 10px; padding: 20px;">
                    <div style="text-align: center;">
                        <!-- Centering the image -->
                        <asp:ImageButton runat="server" ImageUrl="~/images/slic.jpg" Style="width: 100px; height: 50px;" />
                    </div>
                    <h4 style="text-align: center;">Health Plus Medicare Hospital Statement</h4>
                    <br />

                    <%--DefaultButton="claimhist_submit"--%>

                    <div class="container" style="width: 80%;">
                        <div class="mx-auto">
                            <div class="row">
                                <div class="col-sm-4 ">Reference No</div>
                                <div class="col-sm-8 " runat="server" id="refNo1"></div>
                            </div>
                            <div class="row">
                                <div class="col-sm-4 ">Name of the Insured </div>
                                <div class="col-sm-8 " runat="server" id="nameInsu"></div>
                            </div>
                            <div class="row">
                                <div class="col-sm-4 ">Policy No :</div>
                                <div class="col-sm-8 " runat="server" id="polNo2"></div>
                            </div>
                            <div class="row">
                                <div class="col-sm-4 ">B.H.T. No </div>
                                <div class="col-sm-8 " runat="server" id="bhtNo2"></div>
                            </div>
                            <div class="row">
                                <div class="col-sm-4">Bill No </div>
                                <div class="col-sm-8 " runat="server" id="billNo2"></div>
                            </div>
                            <div class="row">
                                <div class="col-sm-4 ">Total Bill Amount(Rs.) </div>
                                <div class="col-sm-8 " runat="server" id="totStAm"></div>
                            </div>
                            <div class="row">
                                <div class="col-4">Amount Payable by Insurance(Rs.) :</div>
                                <div class="col-8 " runat="server" id="totStaePayAm"></div>
                            </div>

                            <div class="card-footer d-flex justify-content-evenly">
                                <asp:Button runat="server" ID="Button5" Text="Back" CssClass="btn btn-claimPaymet" OnClick="back_click" />
                                <asp:Button runat="server" ID="Button4" Text="Download Medi Receipt" CssClass="btn btn-claimPaymet" Width= "200px" OnClick="down_recei_pdf" Visible="false" />
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

        function custom_alert2(message, title) {
            if (!title)
                title = 'Alert';

            if (!message)
                message = 'No Message to Display.';
            

            if (title == 'Alert') {

                swal
                    ({
                        title: title,
                        text: message,
                        icon: "warning",
                        button: true,
                        closeOnClickOutside: false,
                    });
            }
            else if (title == 'Success') {
                swal
                    ({
                        title: title,
                        text: message,
                        icon: "success",
                        button: "OK",
                        closeOnClickOutside: false,
                    });
            }

        }

        function custom_alert(message, title) {

            var title = $("#lblAlertMessage").attr("data-alert-title");
            var message = $("#lblAlertMessage").attr("data-alert-message");

            if (!title) title = 'Alert';
            if (!message) message = 'No Message to Display.';

            //if (!title)
            //    title = 'Alert';

            //if (!message)
            //    message = 'No Message to Display.';
            

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
                    icon: "info",
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
