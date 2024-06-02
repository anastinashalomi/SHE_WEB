<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/Site.Master" CodeBehind="inquiry1.aspx.cs" Inherits="SHE.Inquiry.inquiry1" %>



<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">

    <link href="https://cdn.jsdelivr.net/npm/bootstrap@5.3.0/dist/css/bootstrap.min.css" rel="stylesheet" integrity="sha384-9ndCyUaIbzAi2FUVXJi0CjmCapSmO7SnpJef0486qhLnuZ2cdeRhO02iuK6FUUVM" crossorigin="anonymous" />

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


        .btn:hover {
            color: gainsboro; /* Maintain the same color on hover */
            background-color: #05ABB8; /* Maintain transparency on hover */
        }

        .form-group {
            width: 300px;
        }

        label {
            font-style: oblique;
        }

        .btn {
            /*//background-color: #05ABB8;*/
        }

        #exitbutton {
            width: 70px;
        }

        .panel-with-shadow {
            box-shadow: 0 0 10px rgba(0, 0, 0, 0.2);
            /* You can adjust the values to control the shadow appearance */
        }

        .radioButtonList, .radioButtonList input, .radioButtonList label {
            font-family: Arial, sans-serif; /* Replace with desired font */
            font-style:normal;
        }
    </style>
    <style>
        .radioButtonList {
            display: inline-block;
            margin-right: 10px; /* Adjust the spacing between radio buttons */
        }
    </style>
    <script type="text/javascript">
        function clientFunctionValidationFinished() {


            custom_alert('Successfully Processing Please Wait...', 'Processing');
            return true;

        }
    </script>

    <div class="container">
        <div class="row justify-content-center">
            <div class="col-md-12 panel-with-shadow" style="width: 600px; background-color: #CFEFF2; padding: 10px; border: 1px solid #c0c0c0; border-radius: 10px; padding: 20px;">
                <h4 style="text-align: center;">SHE Hospitalization Details - Inquiry</h4>
                <br />
                <br />
                <asp:Label ID="lblAlertMessage" runat="server" ClientIDMode="Static" Style="display: none;"></asp:Label>
                <asp:Panel ID="panel3" Class=" d-flex flex-column align-items-center ">
                <div style="text-align: left;" >
                    <asp:RadioButtonList ID="RadioButtonList1" runat="server" AutoPostBack="true"
                        OnSelectedIndexChanged="RadioButtonList1_SelectedIndexChanged" CssClass="radioButtonList">
                        <asp:ListItem Text="Use Date Range" Value="Date"></asp:ListItem>
                        <asp:ListItem Text="Use Reference No" Value="Reference"></asp:ListItem>
                    </asp:RadioButtonList>
                </div>
                </asp:Panel>
                <br />

                <asp:Panel ID="panel1" Visible="false" runat="server" DefaultButton="inquiry_submit" Class=" d-flex flex-column align-items-center ">
                    <div class="row mx-auto">
                        <div class="form-group col-sm-12 col-md-8 col-lg-6">
                            <label for="fromdate" style="font-style: normal"><b>From Date:</b></label>
                            <input runat="server" type="date" class="form-control" id="fromdate" onclick="hi" />
                        </div>

                    </div>
                    <br />
                    <div class="row mx-auto">
                        <div class="form-group col-sm-12 col-md-8 col-lg-6">
                            <label for="todate" style="font-style: normal"><b>To Date:</b></label>
                            <input runat="server" type="date" class="form-control" id="todate" />
                        </div>
                    </div>
                    <br />

                </asp:Panel>

                <asp:Panel runat="server" Visible="false" ID="panel2" Class=" d-flex flex-column align-items-center ">
                    <div class="row mx-auto">
                        <div class="form-group col-sm-12 col-md-8 col-lg-6">
                            <label for="reference" style="font-style: normal"><b>SHE Reference No:</b></label>
                            <input runat="server" type="text" class="form-control" id="reference" />
                        </div>
                    </div>
                </asp:Panel>

                <br />
                <asp:Label runat="server" ID="error1" Style="color: red; padding: 4px;" Visible="false">** Select From Date **</asp:Label>
                <asp:Label runat="server" ID="error2" Style="color: red; padding: 4px;" Visible="false">** Insert Date and Reference Number  **</asp:Label>
                <asp:Label runat="server" ID="Label3" Style="color: red; padding: 4px;" Visible="false">** Select one option **</asp:Label>
                <div class="d-flex justify-content-center">
                    <asp:Button ID="exitbutton" runat="server" Text="Exit" CssClass="btn btn-danger mx-4" OnClick="exitbutton_Click" />
                    <asp:Button ID="reset" runat="server" Text="Reset" CssClass="btn mx-4 btn-outline-secondary" OnClick="reset_Click" />
                    <asp:Button ID="inquiry_submit" runat="server" Text="Submit" CssClass="btn btn-primary mx-4" OnClick="inquiry_submit_Click" OnClientClick="return clientFunctionValidationFinished()" ClientIDMode="Static" />
                </div>



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
                    //icon: "success",
                    button: false,
                    closeOnClickOutside: false,
                });
            }
        }

    </script>


    <script src="https://ajax.googleapis.com/ajax/libs/jquery/3.5.1/jquery.min.js"></script>

    <script type="text/javascript">

        $(document).ready(function () {
            $('#inquiry_submit').click(function () {
                var fromD = $('#<%=fromdate.ClientID %>').val();
                var toD = $('#<%=todate.ClientID %>').val();
                var refeNo = $('#<%=reference.ClientID %>').val();

                var selectedValue = $('input[name="<%= RadioButtonList1.UniqueID %>"]:checked').val();

                if (selectedValue == "Date") {
                    if (fromD == "" && toD == "") {
                        custom_alert("Please enter from date and to date.", "Alert");
                        return false;
                    } else if (fromD == "") {
                        custom_alert("Please enter from date.", "Alert");
                        return false;
                    } else if (toD == "") {
                        custom_alert("Please enter to date.", "Alert");
                        return false;
                    }
                } else if (selectedValue == "Reference") {
                    if (refeNo == "") {
                        custom_alert("Please enter reference no.", "Alert");
                        return false;
                    }
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
