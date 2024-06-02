<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="ClaimPayList.aspx.cs" Inherits="SHE.ClaimPayment.ClaimPayList" %>
<%@ Import Namespace="SHE.Code" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    <asp:HiddenField runat="server" ID="hiddenEndDate" />
    <style>
        .sticky-header {
            position: sticky;
            top: 0; /* Stick to the top of the container */
            background-color: #D8D8D8;
            z-index: 1;
            border: none; /* Add the desired border style */
        }

        label {
            font-weight: 400;
        }

        .btn-primary {
            background-color: #05ABB8;
            color: #fff;
            border-color: #05ABB8;
        }

        .btn:hover {
            color: gainsboro; /* Maintain the same color on hover */
            background-color: #05ABB8; /* Maintain transparency on hover */
        }

        .btn {
            background-color: #05ABB8;
        }

        .gridViewHeaderColor {
            background-color: #00ADBB; /* Light gray background color for headers */
        }

        .gridViewItemColor {
            background-color: #70cbd1; /* White background color for items */
        }

        /* Additional styling for specific cells if needed */
        .highlightedCell {
            background-color: #70cbd1; /* Yellow background color for highlighted cells */
        }

        .row-color-odd {
            background-color: #fff; /* Light gray background color for odd rows */
        }

        .row-color-even {
            background-color: #61d4de; /* White background color for even rows */
        }
    </style>

    <script type="text/javascript">
        function clientFunctionValidationFinished() {
            custom_alert('Successfully Processing Please Wait...', 'Processing');
            return true;
        }
    </script>

    <asp:Label ID="lblAlertMessage" runat="server" ClientIDMode="Static" Style="display: none;"></asp:Label>
    <asp:Panel ID="panel1" Visible="false" runat="server" CssClass="d-flex flex-column align-items-center">
        <div class="container" style="padding: 10px;">
            <div class="row">
                <div runat="server" class="col-12" id="datediv" visible="false">
                    <h4 class="text-center">
                        Claim Payment List
                        <label runat="server" id="label1"></label>
                        To
                        <label runat="server" id="label2"></label>
                    </h4>
                </div>
            </div>
        </div>
        <br />
        <div class="container">
            <div class="row">
                <div class="col-12">
                    <asp:UpdatePanel runat="server" ID="gridview2update" UpdateMode="Conditional">
                        <ContentTemplate>
                            <div class="table-responsive" style="max-height: 500px; overflow: auto;">
                                <asp:GridView ID="GridView2" runat="server" DataKeyNames="CLAIM_REF_NO" CellPadding="3" CellSpacing="1" EnableEventValidation="false"
                                    CssClass="table table-striped table-hover border-0 fw-normal table-bordered w-100" OnRowDataBound="GridView2_RowDataBound" OnRowCommand="GridView2_RowCommand"
                                    GridLines="None" AutoGenerateColumns="false" Style="overflow: scroll;">
                                    <HeaderStyle CssClass="sticky-header gridViewHeaderColor" />
                                    <Columns>
                                        <asp:BoundField DataField="CLAIM_REF_NO" HeaderText="Claim Reference Number"
                                            HeaderStyle-CssClass="testClassHeader" ItemStyle-CssClass="testClass">
                                            <HeaderStyle CssClass="testClassHeader"></HeaderStyle>
                                            <ItemStyle CssClass="testClass" Width="300px"></ItemStyle>
                                        </asp:BoundField>
                                        <asp:BoundField DataField="PNAME" HeaderText="Patient's Name"
                                            HeaderStyle-CssClass="testClassHeader" ItemStyle-CssClass="testClass">
                                            <HeaderStyle CssClass="testClassHeader"></HeaderStyle>
                                            <ItemStyle CssClass="testClass" Width="400px"></ItemStyle>
                                        </asp:BoundField>
                                        <asp:BoundField DataField="HOSPITAL" HeaderText="Hospital"
                                            HeaderStyle-CssClass="testClassHeader" ItemStyle-CssClass="testClass">
                                            <HeaderStyle CssClass="testClassHeader"></HeaderStyle>
                                            <ItemStyle CssClass="testClass" Width="400px"></ItemStyle>
                                        </asp:BoundField>
                                        <asp:TemplateField HeaderText="Claim Payment" HeaderStyle-CssClass="testClassHeader"
                                            ItemStyle-CssClass="testClass" ItemStyle-Width="200px" ItemStyle-VerticalAlign="Middle">
                                            <ItemTemplate>
                                                <asp:Button runat="server" Text="View" CommandName="ViewRow" CssClass="btn" AutoPostBack="true" OnClientClick="return clientFunctionValidationFinished()" CommandArgument='<%# Container.DataItemIndex %>' />
                                            </ItemTemplate>
                                            <HeaderStyle CssClass="testClassHeader"></HeaderStyle>
                                            <ItemStyle CssClass="testClass" HorizontalAlign="Center"></ItemStyle>
                                        </asp:TemplateField>
                                    </Columns>
                                    <PagerStyle BackColor="Transparent" CssClass="pagerClz" Height="10px" VerticalAlign="Bottom" HorizontalAlign="Left" />
                                </asp:GridView>
                                <div class="d-flex justify-content-center align-items-center mt-3">
                                    <asp:Button ID="backbutton" runat="server" Text="Go Back" CssClass="btn btn-primary mx-4" OnClick="backbutton_Click" />
                                </div>
                            </div>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </div>
            </div>
        </div>
    </asp:Panel>

    <script src="https://unpkg.com/sweetalert/dist/sweetalert.min.js"></script>
    <script type="text/javascript">
        function custom_alert(message, title) {
            if (!title) title = 'Alert';
            if (!message) message = 'No Message to Display.';

            var imageUrl = '<%= ResolveUrl("~/images/process4.gif") %>';

            if (title === 'Alert') {
                swal({
                    title: title,
                    text: message,
                    icon: "warning",
                    button: true,
                    closeOnClickOutside: false,
                });
            } else if (title === 'Success') {
                swal({
                    title: title,
                    text: message,
                    icon: "success",
                    button: false,
                    closeOnClickOutside: false,
                });
            } else if (title === 'Processing') {
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