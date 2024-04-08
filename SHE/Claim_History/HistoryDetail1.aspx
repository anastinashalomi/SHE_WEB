<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="HistoryDetail1.aspx.cs" Inherits="SHE.Claim_History.HistoryDetail1" %>

<%@ Import Namespace="SHE.Code" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">

    <script type="text/javascript">
        function clientFunctionValidationFinished() {


            custom_alert('Successfully Processing wait...', 'Info');
            return true;

        }
    </script>


    <asp:Panel runat="server" Visible="true" ID="mainpanel1">
        <br />
        <div class="container">
            <div class="row">
                <div class="col text-center">
                    <h4>SHE Claim History Details</h4>
                </div>
            </div>
        </div>

        <br />
        <div class="container">
            <div class="row">
                <div class="col text-center">
                    <h5>Employee Details</h5>
                </div>
            </div>
        </div>
        <br />
        <asp:Label ID="lblAlertMessage" runat="server" ClientIDMode="Static" Style="display: none;"></asp:Label>
        <br />
        <div class="container" style="background-color: #D3D3D3; border-radius: 10px; padding: 20px;">
            <div class="row">
                <div class="col-12 ">
                    <table runat="server" class="table" style="border-color: #c0c0c0;">
                        <tr hidden="hidden">
                            <td class="px-3"><b>Company Name: </b>
                                <label runat="server" id="label1"></label>
                            </td>
                            <td class="px-3">
                                <b>Active Status: </b>
                                <label runat="server" id="label7"></label>
                            </td>
                        </tr>
                        <tr>
                            <td class="px-3">
                                <b>Employee Number: </b>
                                <label runat="server" id="label5"></label>
                            </td>
                            <td class="px-3">
                                <b>Employee Name: </b>
                                <label runat="server" id="label6"></label>
                            </td>
                        </tr>
                        <tr>
                            <td class="px-3">
                                <b>Policy Number: </b>
                                <label runat="server" id="label4"></label>

                            </td>
                            <td class="px-3">
                                <b>Employee Category: </b>
                                <label runat="server" id="empCate"></label>
                            </td>

                        </tr>

                        <tr>
                            <td class="px-3">
                                <b>Date of Birth: </b>
                                <label runat="server" id="dob"></label>
                            </td>
                            <td class="px-3">
                                <b>Year Limit: </b>
                                <label runat="server" id="Label35"></label>
                            </td>

                        </tr>
                        <tr>
                            <td class="px-3">
                                <b>Event Limit: </b>
                                <label runat="server" id="Label36"></label>
                            </td>
                            <td class="px-3">
                                <b>Total Premium: </b><%--<label runat="server" id="label3"></label>--%>
                                <label runat="server" id="totPre"></label>
                            </td>
                        </tr>
                        <tr>
                            <td class="px-3"><b>Receivable Premium: </b>
                                <label runat="server" id="Label37"></label>
                            </td>
                            <td class="px-3">
                                <b>Balance Premium: </b><%--<label runat="server" id="label3"></label>--%>
                                <label runat="server" id="Label38"></label>
                            </td>
                        </tr>
                        <tr hidden="hidden">
                            <td class="px-3">
                                <b>Policy Type: </b><%--<label runat="server" id="label3"></label>--%>
                                <label runat="server" id="label32"></label>
                            </td>

                        </tr>
                        <tr hidden="hidden" id="hiddenRow" runat="server">
                            <td class="px-3">
                                <b>Renewal Date:</b>
                                <label runat="server" id="label2"></label>
                            </td>
                            <td class="px-3">
                                <b>Policy Period:</b>
                                <label runat="server" id="label31"></label>
                            </td>
                        </tr>

                    </table>


                </div>
            </div>
        </div>
        <br />

        <div class="container">
            <div class="row">
                <div class="col text-center">
                    <h5>Beneficiary Details</h5>
                </div>
            </div>
        </div>
        <%--grid2--%>
        <div class="container">
            <div class="row">
                <div class="col-12">
                    <div class="table-striped">

                        <asp:GridView ID="GridView2" runat="server" DataKeyNames="DEPENDENTNAME" class="w-100" CellPadding="3" CellSpacing="1"
                            CssClass="table table-striped table-hover  border-0 fw-normal table-bordered "
                            GridLines="None" AutoGenerateColumns="False">
                            <Columns>

                                <asp:BoundField DataField="DEPENDENTNAME" HeaderText="Member Name"
                                    HeaderStyle-CssClass="testClassHeader" ItemStyle-CssClass="testClass">
                                    <HeaderStyle CssClass="testClassHeader"></HeaderStyle>
                                    <ItemStyle CssClass="testClass" Width="350px"></ItemStyle>
                                </asp:BoundField>

                                <asp:BoundField DataField="RELATIONSHIP" HeaderText="Relationship"
                                    HeaderStyle-CssClass="testClassHeader" ItemStyle-CssClass="testClass">
                                    <HeaderStyle CssClass="testClassHeader"></HeaderStyle>
                                    <ItemStyle CssClass="testClass" Width="300px"></ItemStyle>
                                </asp:BoundField>

                                <asp:BoundField DataField="EFFDATE" HeaderText="Effective Date"
                                    HeaderStyle-CssClass="testClassHeader" ItemStyle-CssClass="testClass">
                                    <HeaderStyle CssClass="testClassHeader"></HeaderStyle>
                                    <ItemStyle CssClass="testClass" Width="300px"></ItemStyle>
                                </asp:BoundField>

                                <asp:BoundField DataField="DOB" HeaderText="Date of Birth"
                                    HeaderStyle-CssClass="testClassHeader" ItemStyle-CssClass="testClass">
                                    <HeaderStyle CssClass="testClassHeader"></HeaderStyle>
                                    <ItemStyle CssClass="testClass" Width="300px"></ItemStyle>
                                </asp:BoundField>

                                <asp:BoundField DataField="AGE" HeaderText="Age"
                                    HeaderStyle-CssClass="testClassHeader" ItemStyle-CssClass="testClass">
                                    <HeaderStyle CssClass="testClassHeader"></HeaderStyle>
                                    <ItemStyle CssClass="testClass" Width="300px"></ItemStyle>
                                </asp:BoundField>
                                <%--<asp:BoundField DataField="AGE" HeaderText="Age"
                                    HeaderStyle-CssClass="testClassHeader" ItemStyle-CssClass="testClass">
                                    <HeaderStyle CssClass="testClassHeader"></HeaderStyle>
                                    <ItemStyle CssClass="testClass" Width="350px"></ItemStyle>
                                </asp:BoundField>--%>

                                <%--Select Button--%>

                                <%--<asp:CommandField ButtonType="Button" ShowSelectButton="True" HeaderText="Select Member" HeaderStyle-CssClass="testClassHeader"
                                    ItemStyle-CssClass="testClass" ControlStyle-Height="28px" ControlStyle-Width="60px" ItemStyle-HorizontalAlign="center" />--%>
                            </Columns>
                            <PagerStyle BackColor="Transparent" CssClass="pagerClz" Height="10px" VerticalAlign="Bottom" HorizontalAlign="Left" />
                        </asp:GridView>

                    </div>
                </div>
            </div>
        </div>


        <div class="container" style="padding-top: 6px;">
            <div class="row">
                <div class="col text-center">
                    <h5>Age Limit</h5>
                </div>
            </div>
        </div>

        <%--grid3--%>
        <div class="container">
            <div class="row">
                <div class="col-12">
                    <div class="table-striped">

                        <asp:GridView ID="GridView3" runat="server" DataKeyNames="MEMBERAGE" class="w-100" CellPadding="3" CellSpacing="1"
                            CssClass="table table-striped table-hover  border-0 fw-normal table-bordered "
                            GridLines="None" AutoGenerateColumns="False">
                            <Columns>

                                <asp:BoundField DataField="MEMBERAGE" HeaderText="Member"
                                    HeaderStyle-CssClass="testClassHeader" ItemStyle-CssClass="testClass">
                                    <HeaderStyle CssClass="testClassHeader"></HeaderStyle>
                                    <ItemStyle CssClass="testClass" Width="350px"></ItemStyle>
                                </asp:BoundField>
                                <asp:BoundField DataField="SPOUSEAGE" HeaderText="Spouse"
                                    HeaderStyle-CssClass="testClassHeader" ItemStyle-CssClass="testClass">
                                    <HeaderStyle CssClass="testClassHeader"></HeaderStyle>
                                    <ItemStyle CssClass="testClass" Width="300px"></ItemStyle>
                                </asp:BoundField>
                                <asp:BoundField DataField="CHILDAGE" HeaderText="Child"
                                    HeaderStyle-CssClass="testClassHeader" ItemStyle-CssClass="testClass">
                                    <HeaderStyle CssClass="testClassHeader"></HeaderStyle>
                                    <ItemStyle CssClass="testClass" Width="300px"></ItemStyle>
                                </asp:BoundField>
                                <asp:BoundField DataField="PARENTAGE" HeaderText="Parent"
                                    HeaderStyle-CssClass="testClassHeader" ItemStyle-CssClass="testClass">
                                    <HeaderStyle CssClass="testClassHeader"></HeaderStyle>
                                    <ItemStyle CssClass="testClass" Width="300px"></ItemStyle>
                                </asp:BoundField>

                            </Columns>
                            <PagerStyle BackColor="Transparent" CssClass="pagerClz" Height="10px" VerticalAlign="Bottom" HorizontalAlign="Left" />
                        </asp:GridView>

                    </div>
                </div>
            </div>
        </div>

        <%-- grid 4 policy details --%>
        <div class="container" style="padding-top: 6px;">
            <div class="row">
                <div class="col text-center">
                    <h5>Policy Detalis</h5>
                </div>
            </div>
        </div>

        <div class="container">
            <div class="row justify-content-center">
                <div class="col-md-12 panel-with-shadow" style="width: 700px; background-color: rgb(211 211 211 / 0.65); padding: 10px; border: 1px solid #c0c0c0; border-radius: 10px; padding: 20px;">
                    <br />
                    <div class="container">
                        <div class="row justify-content-center mt-4">
                            <div class="col-md-8">
                                <div class="form-group row">
                                    <label for="lblreferenceNo" class="col-sm-4 col-form-label ">Policy No </label>
                                    <div class="col-sm-8">
                                        <label class="col-sm-8" runat="server" id="lblPoNo"></label>
                                        <%--<input type="text" class="form-control" runat="server" readonly="readonly" id="referenceNo" placeholder="referenceNo" style="width: 70%;">--%>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                    <div class="container">
                        <div class="row justify-content-center mt-4">
                            <div class="col-md-8">
                                <div class="form-group row">
                                    <label for="lblName" class="col-sm-4 col-form-label ">Company </label>
                                    <div class="col-sm-8">
                                        <label class="col-sm-8" runat="server" id="lblComNam"></label>
                                        <%--<input type="Text" class="form-control" runat="server" readonly="readonly" id="name" placeholder="Patient Name" style="width: 70%;">--%>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                    <div class="container">
                        <div class="row justify-content-center mt-4">
                            <div class="col-md-8">
                                <div class="form-group row">
                                    <label for="lblHospital" class="col-sm-4 col-form-label ">Renewal Date</label>
                                    <div class="col-sm-8">
                                        <label class="col-sm-8" runat="server" id="lblReDate"></label>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                    <div class="container">
                        <div class="row justify-content-center mt-4">
                            <div class="col-md-8">
                                <div class="form-group row">
                                    <label for="lblRoomNo" class="col-sm-4 col-form-label ">Effective Date</label>
                                    <div class="col-sm-8">
                                        <label class="col-sm-8" runat="server" id="lblEffeDa"></label>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                    <div class="container">
                        <div class="row justify-content-center mt-4">
                            <div class="col-md-8">
                                <div class="form-group row">
                                    <label for="lblAdmittedDate" class="col-sm-4 col-form-label ">Debit Note No</label>
                                    <div class="col-sm-8">
                                        <label class="col-sm-8" runat="server" id="lblDebNoteNo"></label>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>

                    <div class="container">
                        <div class="row justify-content-center mt-4">
                            <div class="col-md-8">
                                <div class="form-group row">
                                    <label for="lblContactNo" class="col-md-6 col-form-label ">Critical Illness Limit</label>
                                    <div class="col-sm-8">
                                        <label class="col-md-6" runat="server" id="lblCriIlLim"></label>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>

                    <div class="container">
                        <div class="row justify-content-center mt-4">
                            <div class="col-md-8">
                                <div class="form-group row">
                                    <label for="lblDischargeDate" class="col-sm-4 col-form-label ">Critical Illness Employee Limit </label>
                                    <div class="col-sm-8">
                                        <label class="col-sm-8" runat="server" id="lblCriEmp2"></label>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>

                    <div class="container">
                        <div class="row justify-content-center mt-4">
                            <div class="col-md-8">
                                <div class="form-group row">
                                    <label for="lblEmployeeNo" class="col-sm-4 col-form-label ">Period </label>
                                    <div class="col-sm-8">
                                        <label class="col-sm-8" runat="server" id="lblPeri"></label>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>

                    <div class="container">
                        <div class="row justify-content-center mt-4">
                            <div class="col-md-8">
                                <div class="form-group row">
                                    <label for="lblPolicyNo" class="col-sm-4 col-form-label ">Critical Illness Description1  </label>
                                    <div class="col-sm-8">
                                        <label class="col-sm-8" runat="server" id="lblCriDis1"></label>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>

                    <div class="container">
                        <div class="row justify-content-center mt-4">
                            <div class="col-md-8">
                                <div class="form-group row">
                                    <label for="lblPolicyNo" class="col-sm-4 col-form-label ">Critical Illness Description2 </label>
                                    <div class="col-sm-8">
                                        <label class="col-sm-8" runat="server" id="lblCriDis2"></label>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>

                    <div class="container">
                        <div class="row justify-content-center mt-4">
                            <div class="col-md-8">
                                <div class="form-group row">
                                    <label for="lblPolicyNo" class="col-sm-4 col-form-label ">Critical Illness Description3</label>
                                    <div class="col-sm-8">
                                        <label class="col-sm-8" runat="server" id="lblCriDis3"></label>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>

                    <div class="container">
                        <div class="row justify-content-center mt-4">
                            <div class="col-md-8">
                                <div class="form-group row">
                                    <label for="lblPolicyNo" class="col-sm-4 col-form-label ">Critical Illness Description4</label>
                                    <div class="col-sm-8">
                                        <label class="col-sm-8" runat="server" id="lblCriDis4"></label>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>

                    <div class="container">
                        <div class="row justify-content-center mt-4">
                            <div class="col-md-8">
                                <div class="form-group row">
                                    <label for="lblPolicyNo" class="col-sm-4 col-form-label ">Other Benefits</label>
                                    <div class="col-sm-8">
                                        <label class="col-sm-8" runat="server" id="lblOtheBen"></label>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>

                    <div class="container">
                        <div class="row justify-content-center mt-4">
                            <div class="col-md-8">
                                <div class="form-group row">
                                    <label for="lblPolicyNo" class="col-sm-4 col-form-label ">Indoor Medicine (I/F)</label>
                                    <div class="col-sm-8">
                                        <label class="col-sm-8" runat="server" id="InMedi"></label>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>

                    <div class="container">
                        <div class="row justify-content-center mt-4">
                            <div class="col-md-8">
                                <div class="form-group row">
                                    <label for="lblPolicyNo" class="col-sm-4 col-form-label ">Indoor Category Description</label>
                                    <div class="col-sm-8">
                                        <label class="col-sm-8" runat="server" id="inCaDe"></label>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>

                    <div class="container">
                        <div class="row justify-content-center mt-4">
                            <div class="col-md-8">
                                <div class="form-group row">
                                    <label for="lblPolicyNo" class="col-sm-4 col-form-label ">Indoor Category </label>
                                    <div class="col-sm-8">
                                        <label class="col-sm-8" runat="server" id="InCa"></label>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>

                    <div class="container">
                        <div class="row justify-content-center mt-4">
                            <div class="col-md-8">
                                <div class="form-group row">
                                    <label for="lblPolicyNo" class="col-sm-4 col-form-label ">Outdoor Medicine (I/F)</label>
                                    <div class="col-sm-8">
                                        <label class="col-sm-8" runat="server" id="OutMed"></label>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>

                    <div class="container">
                        <div class="row justify-content-center mt-4">
                            <div class="col-md-8">
                                <div class="form-group row">
                                    <label for="lblPolicyNo" class="col-md-6 col-form-label ">Outdoor Category Description</label>
                                    <div class="col-sm-8">
                                        <label class="col-md-6" runat="server" id="OutCaDe"></label>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>

                    <div class="container">
                        <div class="row justify-content-center mt-4">
                            <div class="col-md-8">
                                <div class="form-group row">
                                    <label for="lblPolicyNo" class="col-sm-4 col-form-label ">Outdoor Category </label>
                                    <div class="col-sm-8">
                                        <label class="col-sm-8" runat="server" id="OutCa"></label>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>

                    <br />
                </div>
            </div>
        </div>



    </asp:Panel>

    <div class="container">
        <div class="row">
            <div class="col-12 text-center">
                <asp:Button runat="server" ID="btnIconClick1" class="btn btn-primary" Style="width: 10%; border-radius: 5px; background-color: #4dc6d0; color: white; font-weight: bold; font-size: 15px; border-color: #4dc6d0" Text="Policy Details" OnClick="IconClick_ServerClick" />
                <%--Page--%>
                <%--<svg xmlns="http://www.w3.org/2000/svg" width="20" height="20" fill="currentColor" CssClass="btn btn-danger mx-4" viewBox="0 0 16 16" onclick="document.getElementById('<%= btnIconClick1.ClientID %>').click()">
                    <path d="M2 0a2 2 0 0 0-2 2v12a2 2 0 0 0 2 2h12a2 2 0 0 0 2-2V2a2 2 0 0 0-2-2H2Zm7.283 4.002V12H7.971V5.338h-.065L6.072 6.656V5.385l1.899-1.383h1.312Z" />
                </svg>--%>

                <%--<asp:Button runat="server" ID="btnIconClick2" OnClick="IconClick_ServerClick2" class="btn btn-primary" Style="width: 10%; border-radius: 5px; background-color: #4dc6d0; color: white; font-weight: bold; font-size: 15px; border-color: #4dc6d0" Text="Benefits" OnClientClick="return clientFunctionValidationFinished()" />--%>
                <%--<svg xmlns="http://www.w3.org/2000/svg" width="20" height="20" fill="currentColor" class="bi bi-2-square-fill" viewBox="0 0 16 16" onclick="document.getElementById('<%= btnIconClick2.ClientID %>').click()">
                    <path d="M2 0a2 2 0 0 0-2 2v12a2 2 0 0 0 2 2h12a2 2 0 0 0 2-2V2a2 2 0 0 0-2-2H2Zm4.646 6.24v.07H5.375v-.064c0-1.213.879-2.402 2.637-2.402 1.582 0 2.613.949 2.613 2.215 0 1.002-.6 1.667-1.287 2.43l-.096.107-1.974 2.22v.077h3.498V12H5.422v-.832l2.97-3.293c.434-.475.903-1.008.903-1.705 0-.744-.557-1.236-1.313-1.236-.843 0-1.336.615-1.336 1.306Z" />
                </svg>--%>

                <%--<asp:Button runat="server" ID="btnIconClick3" OnClick="IconClick_ServerClick3" class="btn btn-primary" Style="width: 10%; border-radius: 5px; background-color: #4dc6d0; color: white; font-weight: bold; font-size: 15px; border-color: #4dc6d0" Text="Pending Claims" OnClientClick="return clientFunctionValidationFinished()" />--%>
                <%--<svg xmlns="http://www.w3.org/2000/svg" width="20" height="20" fill="currentColor" class="bi bi-3-square-fill" viewBox="0 0 16 16" onclick="document.getElementById('<%= btnIconClick3.ClientID %>').click()">
                    <path d="M2 0a2 2 0 0 0-2 2v12a2 2 0 0 0 2 2h12a2 2 0 0 0 2-2V2a2 2 0 0 0-2-2H2Zm5.918 8.414h-.879V7.342h.838c.78 0 1.348-.522 1.342-1.237 0-.709-.563-1.195-1.348-1.195-.79 0-1.312.498-1.348 1.055H5.275c.036-1.137.95-2.115 2.625-2.121 1.594-.012 2.608.885 2.637 2.062.023 1.137-.885 1.776-1.482 1.875v.07c.703.07 1.71.64 1.734 1.917.024 1.459-1.277 2.396-2.93 2.396-1.705 0-2.707-.967-2.754-2.144H6.33c.059.597.68 1.06 1.541 1.066.973.006 1.6-.563 1.588-1.354-.006-.779-.621-1.318-1.541-1.318Z" />
                </svg>--%>

                <%--<asp:Button runat="server" ID="btnIconClick4" OnClick="IconClick_ServerClick4" class="btn btn-primary" Style="width: 10%; border-radius: 5px; background-color: #4dc6d0; color: white; font-weight: bold; font-size: 15px; border-color: #4dc6d0" Text="Previous Claims" OnClientClick="return clientFunctionValidationFinished()" />--%>
                <%--<svg xmlns="http://www.w3.org/2000/svg" width="20" height="20" fill="currentColor" class="bi bi-4-square-fill" viewBox="0 0 16 16" onclick="document.getElementById('<%= btnIconClick4.ClientID %>').click()">
                    <path d="M6.225 9.281v.053H8.85V5.063h-.065c-.867 1.33-1.787 2.806-2.56 4.218Z" />
                    <path d="M2 0a2 2 0 0 0-2 2v12a2 2 0 0 0 2 2h12a2 2 0 0 0 2-2V2a2 2 0 0 0-2-2H2Zm5.519 5.057c.22-.352.439-.703.657-1.055h1.933v5.332h1.008v1.107H10.11V12H8.85v-1.559H4.978V9.322c.77-1.427 1.656-2.847 2.542-4.265Z" />
                </svg>--%>
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

            // Create a div element for the processing icon
            var processingIcon = document.createElement('div');
            processingIcon.innerHTML = '<i class="fa fa-spinner fa-spin"></i>';

            // Append the processing icon to the title of the alert
            var alertTitle = document.createElement('div');
            alertTitle.appendChild(processingIcon);
            alertTitle.appendChild(document.createTextNode(' ' + title));

            // Show the alert with the processing icon
            swal({
                title: alertTitle,
                text: message,
                icon: 'info',
                button: false,
                closeOnClickOutside: false,
            });

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
            // Show custom alerts
            var alertType = $("#lblAlertMessage").attr("data-alert-type");
            var message = $("#lblAlertMessage").text();

            if (alertType === "custom" && message !== "") {
                custom_alert(message, "Alert");
            }
        });
    </script>

</asp:Content>
