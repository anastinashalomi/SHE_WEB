<%@ Page Title="" EnableEventValidation="false" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Notifications.aspx.cs" Inherits="SHE.Notifications" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <style>
        .btn {
            color: gainsboro;
        }

            .btn:hover {
                background-color: darkblue;
                color: white;
            }

        .accepted-color-class {
            color: black;
        }

        .completed-color-class {
            color: green;
        }

        .not-updated-color-class {
            color: #5C61FF;
        }

        .reassigned-color-class {
            color: red;
        }

        .smallFont {
            font-size: 13px; /* Adjust the font size as needed */
        }

        .smallFont1 {
            font-size: 13px; /* Adjust the font size as needed */
        }

        .table {
            width: 100%; /* Make sure the table takes the full width of its container */
        }

        .mt-5 {
            margin-top: 5px; /* Adjust top margin */
        }

        /* Add more CSS styles as needed */
        .center-grid {
            margin: 0 auto; /* This centers the grid horizontally */
        }

        .btn-container {
            display: flex;
            justify-content: flex-start; /* Align buttons to the start of the container */
            margin-top: 10px; /* Add margin between buttons and other elements */
        }

        .btn {
            background-color: transparent;
            border: 2px solid #05ABB8; /* Change 'your_accept_border_color' to the desired border color */
            color: black;
            padding: 2px 5px; /* Adjust padding as needed */
            border-radius: 5px; /* Ensure buttons have straight corners */
            margin-right: 10px; /* Add margin between buttons */
        }

            .btn:last-child {
                margin-right: 0; /* Remove margin from the last button to prevent extra gap */
            }


        .btn-accept {
            background-color: #05ABB8; /* Change 'your_accept_color' to the desired background color for ACCEPT button */
            color: white;
            border: 2px solid #05ABB8;
            width: 115px;
        }

        .btn-reassign {
            background-color: #FF2020; /* Change 'your_reassign_color' to the desired background color for REASSIGN button */
            color: white;
            border: 2px solid #FF2020;
            width: 115px;
        }

        .btn-back {
            background-color: #05ABB8; /* Change 'your_reassign_color' to the desired background color for REASSIGN button */
            color: white;
            border: 2px solid #05ABB8;
            width: 115px;
        }

        .btn-claimhistory {
            background-color: #05ABB8; /* Change 'your_reassign_color' to the desired background color for REASSIGN button */
            color: white;
            border: 2px solid #05ABB8;
            width: 115px;
        }

        .btn-claimpayment {
            background-color: #05ABB8; /* Change 'your_reassign_color' to the desired background color for REASSIGN button */
            color: white;
            border: 2px solid #05ABB8;
            width: 115px;
        }

        .btn-SUBMIT {
            background-color: #05ABB8; /* Change 'your_accept_color' to the desired background color for ACCEPT button */
            color: white;
            border: 2px solid #05ABB8;
            width: 115px;
        }

        .btn-danger {
            background-color: #FF2020; /* Change 'your_accept_color' to the desired background color for ACCEPT button */
            color: white;
            border: 2px solid #FF2020;
            width: 115px;
        }

        .hideLabel {
            display: none;
        }

        .centered {
            text-align: center;
        }

        .form-group {
            max-width: 400px; /* Adjust the maximum width as needed */
            margin: 0 auto; /* This centers the form group horizontally */
        }

        .form-control {
            width: 100%; /* This makes the text box fill the available width */
            max-width: 100%; /* Set maximum width to 100% to respect parent's max-width */
        }
    </style>
    <script src="https://unpkg.com/sweetalert/dist/sweetalert.min.js"></script>
    <script type="text/javascript">
        function displayPopup(job_status) {
            // Display sweetalert confirmation with a message
            swal({
                title: job_status + " ALERT",
                text: "SUCCESSFULLY UPDATED.",
                icon: "success",
                buttons: {
                    confirm: {
                        text: "Ok",
                        value: true,
                        visible: true,
                        className: "",
                        closeModal: true
                    }
                }
            }).then((value) => {
                // If user confirms, return true to continue with button click event
                if (value) {
                    return true;
                } else {
                    // If user cancels or closes the dialog, return false to cancel the button click event
                    return false;
                }
            });

            // Ensure the button click event is not executed by default
            return false;
        }
    </script>

    <script src="https://unpkg.com/sweetalert/dist/sweetalert.min.js"></script>
    <script type="text/javascript">
        function displayPopup1(ACTION, isSuccess) {
            // Define the default message for success and failure
            let title = isSuccess ? ACTION + " ALERT" : "ERROR";
            let text = isSuccess ? "SUCCESS." : " FAILED.";

            // Define the default icon and button
            let icon = isSuccess ? "success" : "error";
            let button = isSuccess ? {
                text: "Ok",
                value: true,
                visible: true,
                className: "",
                closeModal: true
            } : "Ok";

            // Display sweetalert confirmation with a message
            swal({
                title: title,
                text: text,
                icon: icon,
                buttons: {
                    confirm: button
                }
            }).then((value) => {
                // If user confirms, return true to continue with button click event
                if (value) {
                    return true;
                } else {
                    // If user cancels or closes the dialog, return false to cancel the button click event
                    return false;
                }
            });

            // Ensure the button click event is not executed by default
            return false;
        }
    </script>

    <script type="text/javascript">
        function maintainScrollPosition() {
            var scrollTop = document.documentElement.scrollTop || document.body.scrollTop;
            document.documentElement.scrollTop = document.body.scrollTop = scrollTop;
        }

        function scrollToElement(elementID) {
            var element = document.getElementById(elementID);
            if (element) {
                var yOffset = element.getBoundingClientRect().top + window.pageYOffset;
                window.scrollTo(0, yOffset);
            }
        }
    </script>


    <script type="text/javascript">
        function displayPopup2(message) {
            alert(message);
        }
    </script>

    <script type="text/javascript">
        function clientFunctionValidationFinished() {

            //alert("test");
            custom_alert('Successfully Processing Please Wait...', 'Processing');
            return true;

        }
    </script>



    <main>
        <div class="container">
            <asp:Panel runat="server" ID="PanelOne" CssClass="mt-5" Visibl="false">
                <div class="row justify-content-center">
                    <div class="col-12 ">
                        <!-- Set a fixed width for the container -->
                        <asp:GridView runat="server" class="table table-hover mt-1 center-grid" ID="NotificationGrid" AutoGenerateColumns="false"
                            OnRowDataBound="NotificationGrid_RowDataBound" OnSelectedIndexChanged="NotificationGrid_SelectedIndexChanged"
                            Width="100%" PageSize="8" AllowPaging="True" OnPageIndexChanging="NotificationGrid_PageIndexChanging">
                            <RowStyle BackColor="White" />
                            <Columns>
                                <asp:TemplateField HeaderText="Notifications">
                                    <ItemTemplate>
                                        <div class='<%# GetAdmittedTypeCssClass(Eval("AdmittedType")) %>'>
                                            <div class="d-flex gap-3 align-items-center">
                                                <div>
                                                    <asp:Image runat="server" ID="NotificationImage" Width="26px" Height="32px" Style="margin-left: 15px; margin-top: 3px;" />
                                                </div>
                                                <div class="d-flex flex-column" style="margin-left: 20px;">
                                                    <asp:Label runat="server" Text='<%# Eval("AdmittedType") %>' ID="AdmitType" Visible="false" CssClass="smallFont"></asp:Label>
                                                    <div style="display: inline;">
                                                        <asp:Label runat="server" Text='<%# Eval("Jobtype") %>' ID="Jobtype" CssClass="smallFont"></asp:Label>
                                                        -
                                                    <asp:Label runat="server" Text='<%# Eval("ClaimRef1") %>' ID="Label1" Visible="True" CssClass="smallFont"></asp:Label>
                                                    </div>
                                                    <asp:Label runat="server" Text='<%# Eval("PatientName") %>' ID="PatientName" CssClass="smallFont" Style="font-weight: bold;"></asp:Label>
                                                    <asp:Label runat="server" Text='<%# Eval("Hospital") %>' ID="Hospital" CssClass="smallFont"></asp:Label>
                                                    <div class="d-flex justify-content-center">
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                            <PagerSettings Mode="Numeric" Position="Bottom" />
                            <EmptyDataTemplate>
                                <div class="text-center">
                                    <h6 style="color: red;">No data is currently available. Please check again.</h6>
                                </div>
                            </EmptyDataTemplate>
                        </asp:GridView>
                    </div>
                </div>
            </asp:Panel>
        </div>


        <asp:Panel runat="server" ID="PanelTwo" Visible="false" CssClass="mt-5">
            <div class="vh-75 d-flex justify-content-center mt-5">
                <div class="card p-2 w-100" style="max-width: 800px;">
                    <div class="card-header d-flex justify-content-center gap-2">
                        <div class="container">
                            <div class="row justify-content-center">
                                <div class="col-md-12 panel" style="width: 900px;">
                                    <div class="row justify-content-center ">
                                        <div class="col-sm-6 col-md-3 mb-2 d-flex justify-content-center">
                                        <asp:Button runat="server" ID="btnAccept" Text="Accept" CssClass="btn btn-accept" OnClick="Accepted_Click" />
                                        </div>
                                        <div class="col-sm-6 col-md-3 mb-2 d-flex justify-content-center">
                                        <asp:Button runat="server" ID="btnReassign" Text="Reassign" CssClass="btn btn-reassign" OnClick="btnReassign_Click" />
                                        </div>
                                        <div class="col-sm-6 col-md-3 mb-2 d-flex justify-content-center">
                                        <asp:Button runat="server" ID="btnreject" Text="Reject" CssClass="btn btn-reassign" OnClick="btnReject_Click" />
                                        </div>
                                        <div class="col-sm-6 col-md-3 mb-2 d-flex justify-content-center">
                                        <asp:Button runat="server" ID="btnremovereject" Text="Remove Reject" CssClass="btn btn-accept" OnClick="Remove_Rejection_click" />
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                    <div class="card-body" style="background-color: #CFEFF2;">
                        <asp:Label runat="server" ID="PanelTwoContentLabel"></asp:Label>

                        <!-- Claim Reference Number -->
                        <div class="row mb-2">
                            <div class="col-sm-4 fw-bold">Claim Reference Number:</div>
                            <div class="col-sm-8">
                                <asp:Label runat="server" ID="ClaimReferenceLabel"></asp:Label>
                            </div>
                        </div>

                        <!-- Patient Name -->
                        <div class="row mb-2">
                            <div class="col-sm-4 fw-bold">Patient Name:</div>
                            <div class="col-sm-8">
                                <asp:Label runat="server" ID="PatientNameLabel"></asp:Label>
                            </div>
                        </div>

                        <!-- Customer Name -->
                        <div class="row mb-2">
                            <div class="col-sm-4 fw-bold">Customer Name:</div>
                            <div class="col-sm-8">
                                <asp:Label runat="server" ID="CustomerNameLabel"></asp:Label>
                            </div>
                        </div>

                        <!-- Customer Phone -->
                        <div class="row mb-2">
                            <div class="col-sm-4 fw-bold">Customer Phone:</div>
                            <div class="col-sm-8">
                                <asp:Label runat="server" ID="CustomerPhoneLabel"></asp:Label>
                            </div>
                        </div>

                        <!-- Job Hospital -->
                        <div class="row mb-2">
                            <div class="col-sm-4 fw-bold">Job Hospital:</div>
                            <div class="col-sm-8">
                                <asp:Label runat="server" ID="JobHospitalLabel"></asp:Label>
                            </div>
                        </div>

                        <!-- Room Number -->
                        <div class="row mb-2">
                            <div class="col-sm-4 fw-bold">Room Number:</div>
                            <div class="col-sm-8">
                                <asp:Label runat="server" ID="RoomNumberLabel"></asp:Label>
                            </div>
                        </div>

                        <!-- Add Date -->
                        <div class="row mb-2">
                            <div class="col-sm-4 fw-bold">Add Date:</div>
                            <div class="col-sm-8">
                                <asp:Label runat="server" ID="AddDateLabel"></asp:Label>
                            </div>
                        </div>

                        <!-- Patient ID Number -->
                        <div class="row mb-2">
                            <div class="col-sm-4 fw-bold">Patient ID Number:</div>
                            <div class="col-sm-8">
                                <asp:Label runat="server" ID="PatientIDNumberLabel"></asp:Label>
                            </div>
                        </div>

                        <!-- Discharge Date -->
                        <div class="row mb-2">
                            <div class="col-sm-4 fw-bold">Discharge Date:</div>
                            <div class="col-sm-8">
                                <asp:Label runat="server" ID="DischargeDateLabel"></asp:Label>
                            </div>
                        </div>

                        <!-- EPF -->
                        <div class="row mb-2">
                            <div class="col-sm-4 fw-bold">EPF:</div>
                            <div class="col-sm-8">
                                <asp:Label runat="server" ID="EPFLabel"></asp:Label>
                            </div>
                        </div>

                        <!-- Policy Number -->
                        <div class="row mb-2">
                            <div class="col-sm-4 fw-bold">Policy Number:</div>
                            <div class="col-sm-8">
                                <asp:Label runat="server" ID="PolicyLabel"></asp:Label>
                            </div>
                        </div>

                        <!-- Remark -->
                        <div class="row mb-2">
                            <div class="col-sm-4 fw-bold">Remark:</div>
                            <div class="col-sm-8">
                                <asp:Label runat="server" ID="RemarkLabel"></asp:Label>
                            </div>
                        </div>

                        <!-- Job Status -->
                        <div class="row mb-2">
                            <div class="col-sm-4 fw-bold">Job Status:</div>
                            <div class="col-sm-8">
                                <asp:Label runat="server" ID="JobStatusLabel"></asp:Label>
                            </div>
                        </div>

                        <!-- Job Type -->
                        <div class="row mb-2" runat="server" visible="false">
                            <div class="col-sm-4 fw-bold">Job Type:</div>
                            <div class="col-sm-8">
                                <asp:Label runat="server" ID="Job_Typelabel"></asp:Label>
                            </div>
                        </div>

                        <!-- Claim Action -->
                        <div class="row mb-2">
                            <div class="col-sm-4 fw-bold">Claim Action:</div>
                            <div class="col-sm-8">
                                <asp:Label runat="server" ID="Claim_Action"></asp:Label>
                            </div>
                        </div>
                    </div>

                    <div class="card-footer d-flex justify-content-evenly">
                        <div class="container">
                            <div class="row justify-content-center">
                                <div class="col-md-12 panel" style="width: 900px;">
                                    <div class="row justify-content-center ">
                                        <div class="col-sm-6 col-md-3 mb-2 d-flex justify-content-center">
                                            <asp:Button runat="server" ID="btnBack" Text="Back" OnClick="BackButton_Click" CssClass="btn btn-back" Style="width: 140px;" />
                                        </div>
                                        <div class="col-sm-6 col-md-3 mb-2 d-flex justify-content-center">
                                            <asp:Button runat="server" ID="btnClaimHistory" Text="Claim History" CssClass="btn btn-claimhistory" OnClick="ClaimHistory_Click" Style="width: 140px;" />
                                        </div>
                                        <div class="col-sm-6 col-md-3 mb-2 d-flex justify-content-center">
                                            <asp:Button runat="server" ID="btnClaimPayment" Text="Claim Payment" CssClass="btn btn-claimpayment" OnClick="ClaimPayment_Click" Style="width: 140px;" />
                                        </div>
                                        <div class="col-sm-6 col-md-3 mb-2 d-flex justify-content-center">
                                            <asp:Button runat="server" ID="rePrint" Enabled="false" Text="Reprint Statement" CssClass="btn btn-claimpayment" OnClick="Reprint_Click" Style="width: 140px;" />
                                        </div>
                                        <div class="col-sm-6 col-md-3 mb-2 d-flex justify-content-center">
                                            <asp:Button runat="server" ID="payEdit" Enabled="false" Text="Edit Payment" CssClass="btn btn-claimpayment" OnClick="Edit_Pay_Click" OnClientClick="return clientFunctionValidationFinished()" Style="width: 140px;" />
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </asp:Panel>

        <asp:Panel runat="server" ID="PanelThree" CssClass="mt-1 " Visible="false">
            <div class="d-flex flex-column justify-content-center align-items-center" style="font-size: 1.2rem; font-weight: bold; margin-bottom: 20px; margin-top: 20px;">
                Please select an agent to Reassign this task
            </div>
            <div style="overflow-y: scroll; max-height: 650px;">
                <!-- Adjust max-height as needed -->
                <asp:GridView runat="server" class="table table-hover mt-2 center-grid" ID="GridView2" AutoGenerateColumns="false"
                    OnRowDataBound="reassignGrid_RowDataBound" OnSelectedIndexChanged="reassignGrid_SelectedIndexChanged"
                    Width="100%" Style="background-color: #CFEFF2">
                    <Columns>
                        <asp:TemplateField HeaderText="Select an Agent">
                            <ItemTemplate>
                                <div class="row myRow">
                                    <div class="row">
                                        <div class="col-md-1">
                                            <asp:RadioButton runat="server" ID="RadioButton1" GroupName="AgentSelection" AutoPostBack="true" Class="radioButtonList" OnCheckedChanged="RadioButton_CheckedChanged" />
                                        </div>
                                        <div class="col-md-3">
                                            <asp:Label runat="server" Text='<%# Eval("CSRUSRN1") %>' ID="CSRUSRN" CssClass="smallFont"></asp:Label>
                                        </div>
                                        <div class="col-md-3">
                                            <asp:Label runat="server" Text='<%# Eval("CLAIMINF1") %>' ID="agentname" CssClass="smallFont"></asp:Label>
                                        </div>
                                        <div class="col-md-4">
                                            <asp:Label runat="server" Text='<%# Eval("BRANCH1") %>' ID="BRANCH_NAME" CssClass="smallFont"></asp:Label>
                                        </div>
                                        <div class="col-md-4">
                                            <asp:Label runat="server" Text='<%# Eval("csrtpno1") %>' ID="CSRTNO" CssClass="smallFont hideLabel"></asp:Label>
                                        </div>
                                        <div class="col-md-4">
                                            <asp:Label runat="server" Text='<%# Eval("CSRGXNO1") %>' ID="CSRGXNO" CssClass="smallFont hideLabel"></asp:Label>
                                        </div>
                                    </div>
                                </div>
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                </asp:GridView>
            </div>

            <div style="margin-top: 20px; display: flex; justify-content: center;">
                <asp:Button runat="server" ID="exitbutton" Text="Exit" CssClass="btn btn-danger " OnClick="exitbutton_Click" />
                <asp:Button runat="server" ID="BackButton" Text="Back" CssClass="btn btn-back  " OnClick="BackButton_Click" />
                <asp:Button runat="server" ID="SubmitButton" Text="Submit" CssClass="btn btn-accept" OnClick="btnSubmitButton_Click" />
            </div>
        </asp:Panel>

        <asp:Panel runat="server" ID="Panelfour" Visible="false">
            <div class="d-flex justify-content-center align-items-center" style="min-height: 60vh;">
                <div class="container">
                    <div class="row justify-content-center">
                        <div class="col-md-12 panel-with-shadow" style="max-width: 450px; background-color: #CFEFF2; padding: 10px; border: 1px solid #c0c0c0; border-radius: 10px; padding: 20px;">
                            <h4 style="text-align: center;"><strong>Claim Reject</strong></h4>
                            <br />
                            <br />
                            <!-- Text Field for Reject Reason -->
                            <div class="centered">
                                <div class="form-group">
                                    <label for="rejectReason"><b>Reason for Rejection:</b></label>
                                    <asp:TextBox runat="server" ID="rejectReasonTextBox" CssClass="form-control"></asp:TextBox>
                                </div>
                                <br />
                                <br />
                                <div class="d-flex justify-content-center">
                                    <asp:Button ID="Button1" runat="server" Text="Exit" CssClass="btn btn-danger " OnClick="exitbutton_Click" />
                                    <asp:Button ID="rejectback" runat="server" Text="Back" CssClass="btn btn-back" OnClick="rejectBack_Click" />
                                    <asp:Button ID="Reject_submit" runat="server" Text="Submit" CssClass="btn btn-back" OnClick="reject_submit_Click" />
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </asp:Panel>

        <asp:Panel runat="server" ID="PanelFive" Visible="false">
            <div class="d-flex justify-content-center align-items-center" style="min-height: 60vh;">
                <div class="container">
                    <div class="row justify-content-center">
                        <div class="col-md-12 panel-with-shadow" style="max-width: 450px; background-color: #CFEFF2; padding: 10px; border: 1px solid #c0c0c0; border-radius: 10px; padding: 20px;">
                            <h4 style="text-align: center;"><strong>Remove the claim rejection</strong></h4>
                            <br />
                            <br />
                            <!-- Text Field for Reject Reason -->
                            <div class="centered">
                                <div class="form-group">

                                    <label for="RemoverejectReason"><b>Reason for rejection removed</b></label>
                                    <asp:TextBox runat="server" ID="rejectionRemove" CssClass="form-control"></asp:TextBox>
                                </div>
                                <br />
                                <br />
                                <div class="d-flex justify-content-center">
                                    <asp:Button ID="Button2" runat="server" Text="Exit" CssClass="btn btn-danger" OnClick="exitbutton_Click" />
                                    <asp:Button runat="server" ID="Button3" Text="Back" CssClass="btn btn-back  " OnClick="rejectBack_Click" />
                                    <asp:Button ID="remove_reject" runat="server" Text="Submit" CssClass="btn btn-back " OnClick="reject_removesubmit_Click" />
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </asp:Panel>

    </main>


    <script src="https://unpkg.com/sweetalert/dist/sweetalert.min.js"></script>
    <script type="text/javascript">
        function custom_alert(message, title) {
            if (!title)
                title = 'Alert';

            if (!message)
                message = 'No Message to Display.';

            var imageUrl = '<%= ResolveUrl("~/images/process4.gif") %>';

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
            } else if (title == 'Processing') {
                swal({
                    title: title,
                    text: message,
                    icon: imageUrl,
                    button: false,
                    closeOnClickOutside: false,
                });
            }
        }

    </script>

    <script src="https://ajax.googleapis.com/ajax/libs/jquery/3.5.1/jquery.min.js"></script>

    <script type="text/javascript">

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
