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

    <main>
        <div class="container">
            <asp:Panel runat="server" ID="PanelOne" CssClass="mt-5" Visibl="false">
                <div class="row justify-content-left">
                    <div class="col-12 col-md-10 col-lg-8">
                        <!-- Set a fixed width for the container -->
                        <div class="container">
                            <div class="row justify-content-center">
                                <div class="col-md-10">
                                    <asp:GridView runat="server" class="table table-hover mt-1 center-grid" ID="NotificationGrid" AutoGenerateColumns="false"
                                        OnRowDataBound="NotificationGrid_RowDataBound" OnSelectedIndexChanged="NotificationGrid_SelectedIndexChanged"
                                        Width="165%" PageSize="8" AllowPaging="True" OnPageIndexChanging="NotificationGrid_PageIndexChanging">
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
                                                                    <asp:Label runat="server" Text='<%# Eval("ClaimRef1") %>' ID="Label2" Visible="True" CssClass="smallFont"></asp:Label>
                                                                </div>
                                                                <asp:Label runat="server" Text='<%# Eval("PatientName") %>' ID="PatientName" CssClass="smallFont" Style="font-weight: bold;"></asp:Label>
                                                                <asp:Label runat="server" Text='<%# Eval("Hospital") %>' ID="Hospital" CssClass="smallFont"></asp:Label>
                                                                <asp:Label runat="server" Text='<%# Eval("ClaimRef1") %>' ID="Label1" Visible="false" CssClass="smallFont"></asp:Label>
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
                        </div>
                    </div>
                </div>
            </asp:Panel>
        </div>

        <asp:Panel runat="server" ID="PanelTwo" Visible="false" CssClass="mt-5 ">
            <div class="vh-75 d-flex justify-content-center mt-5">
                <div class="card p-2" style="width: 70%;">
                    <div class="card-header d-flex justify-content-center gap-2">
                        <div class="btn-container">
                            <asp:Button runat="server" ID="btnAccept" Text="Accept" CssClass="btn btn-accept" OnClick="Accepted_Click" />
                            <asp:Button runat="server" ID="btnReassign" Text="Reassign" CssClass="btn btn-reassign " OnClick="btnReassign_Click" />
                        </div>

                    </div>
                    <div class="card-body" style="background-color: #CFEFF2;">
                        <asp:Label runat="server" ID="PanelTwoContentLabel"></asp:Label>

                        <div class="row">
                            <div class="col-sm-4">
                                <asp:Label runat="server" class="fw-bold" Text="Claim Reference Number:"></asp:Label>
                            </div>
                            <div class="col-sm-4">
                                <asp:Label runat="server" ID="ClaimReferenceLabel"></asp:Label>
                            </div>
                            <div class="col-sm-4"></div>
                        </div>

                        <div class="row">
                            <div class="col-sm-4">
                                <asp:Label runat="server" class="fw-bold" Text="Patient Name:"></asp:Label>
                            </div>
                            <div class="col-sm-4">
                                <asp:Label runat="server" ID="PatientNameLabel"></asp:Label>
                            </div>
                            <div class="col-sm-4"></div>
                        </div>

                        <div class="row">
                            <div class="col-sm-4">
                                <asp:Label runat="server" class="fw-bold" Text="Customer Name:"></asp:Label>
                            </div>
                            <div class="col-sm-4">
                                <asp:Label runat="server" ID="CustomerNameLabel"></asp:Label>
                            </div>
                            <div class="col-sm-4"></div>
                        </div>

                        <div class="row">
                            <div class="col-sm-4">
                                <asp:Label runat="server" class="fw-bold" Text="Customer Phone:"></asp:Label>
                            </div>
                            <div class="col-sm-4">
                                <asp:Label runat="server" ID="CustomerPhoneLabel"></asp:Label>
                            </div>
                            <div class="col-sm-4"></div>
                        </div>

                        <div class="row">
                            <div class="col-sm-4">
                                <asp:Label runat="server" class="fw-bold" Text="Job Hospital:"></asp:Label>
                            </div>
                            <div class="col-sm-4">
                                <asp:Label runat="server" ID="JobHospitalLabel"></asp:Label>
                            </div>
                            <div class="col-sm-4"></div>
                        </div>

                        <div class="row">
                            <div class="col-sm-4">
                                <asp:Label runat="server" class="fw-bold" Text="Room Number:"></asp:Label>
                            </div>
                            <div class="col-sm-4">
                                <asp:Label runat="server" ID="RoomNumberLabel"></asp:Label>
                            </div>
                            <div class="col-sm-4"></div>
                        </div>

                        <div class="row">
                            <div class="col-sm-4">
                                <asp:Label runat="server" class="fw-bold" Text="Add Date:"></asp:Label>
                            </div>
                            <div class="col-sm-4">
                                <asp:Label runat="server" ID="AddDateLabel"></asp:Label>
                            </div>
                            <div class="col-sm-4"></div>
                        </div>

                        <div class="row">
                            <div class="col-sm-4">
                                <asp:Label runat="server" class="fw-bold" Text="Patient ID Number:"></asp:Label>
                            </div>
                            <div class="col-sm-4">
                                <asp:Label runat="server" ID="PatientIDNumberLabel"></asp:Label>
                            </div>
                            <div class="col-sm-4"></div>
                        </div>

                        <div class="row">
                            <div class="col-sm-4">
                                <asp:Label runat="server" class="fw-bold" Text="Discharge Date:"></asp:Label>
                            </div>
                            <div class="col-sm-4">
                                <asp:Label runat="server" ID="DischargeDateLabel"></asp:Label>
                            </div>
                            <div class="col-sm-4"></div>
                        </div>

                        <div class="row">
                            <div class="col-sm-4">
                                <asp:Label runat="server" class="fw-bold" Text="EPF:"></asp:Label>
                            </div>
                            <div class="col-sm-4">
                                <asp:Label runat="server" ID="EPFLabel"></asp:Label>
                            </div>
                            <div class="col-sm-4"></div>
                        </div>

                        <div class="row">
                            <div class="col-sm-4">
                                <asp:Label runat="server" class="fw-bold" Text="Policy Number:"></asp:Label>
                            </div>
                            <div class="col-sm-4">
                                <asp:Label runat="server" ID="PolicyLabel"></asp:Label>
                            </div>
                            <div class="col-sm-4"></div>
                        </div>

                        <div class="row">
                            <div class="col-sm-4">
                                <asp:Label runat="server" class="fw-bold" Text="Remark:"></asp:Label>
                            </div>
                            <div class="col-sm-4">
                                <asp:Label runat="server" ID="RemarkLabel"></asp:Label>
                            </div>
                            <div class="col-sm-4"></div>
                        </div>

                        <div class="row">
                            <div class="col-sm-4">
                                <asp:Label runat="server" class="fw-bold" Text="Job Status:"></asp:Label>
                            </div>
                            <div class="col-sm-4">
                                <asp:Label runat="server" ID="JobStatusLabel"></asp:Label>
                            </div>
                            <div class="col-sm-4"></div>
                        </div>

                        <div class="row jobTypeRow" runat="server" visible="false">
                            <div class="col-sm-4">
                                <asp:Label runat="server" class="fw-bold" Text="Job_Type:"></asp:Label>
                            </div>
                            <div class="col-sm-4">
                                <asp:Label runat="server" ID="Job_Typelabel"></asp:Label>
                            </div>
                            <div class="col-sm-4"></div>
                        </div>
                    <div class="card-footer d-flex justify-content-evenly">
                        <asp:Button runat="server" ID="btnBack" Text="Back" OnClick="BackButton_Click" CssClass="btn btn-back" />
                        <asp:Button runat="server" ID="btnClaimHistory" Text="Claim History" CssClass="btn btn-claimhistory " OnClick="ClaimHistory_Click" />
                        <asp:Button runat="server" ID="btnClaimPayment" Text="Claim Payment" CssClass="btn btn-claimpayment " OnClick="ClaimPayment_Click" />
                    </div>
                </div>
            </div>
        </asp:Panel>

       <asp:Panel runat="server" ID="PanelThree" CssClass="mt-1 " Visible="false">
            <div class="d-flex justify-content-center" style="font-size: 1.2rem; font-weight: bold;">Please select an agent to Reassign this task</div>
            <div>
                <asp:GridView runat="server" class="table table-hover mt-2 center-grid" ID="GridView2" AutoGenerateColumns="false"
                    OnRowDataBound="reassignGrid_RowDataBound" OnSelectedIndexChanged="reassignGrid_SelectedIndexChanged"
                    Width="60%" Style="background-color: #CFEFF2">
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
    </main>
</asp:Content>
