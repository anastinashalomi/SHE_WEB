<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/Site.Master" CodeBehind="claimhist2.aspx.cs" Inherits="SHE.Claim_History.claimhist2" %>

<%@ Import Namespace="SHE.Code" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">

    <script  type="text/javascript">
        function clientFunctionValidationFinished() {


            custom_alert('Successfully Processing Please Wait...', 'Processing');
            return true;

        }
    </script>

    <style>
        .sticky-header {
            position: sticky;
            top: -1%;
            background-color: #D8D8D8;
            z-index: 1;
            /*border: none;*/ /* Add the desired border style */
            text-align:center;
        }
        .gridViewHeaderColor {
            background-color: #61d4de; /* Light gray background color for headers */
        }

        .gridViewItemColor {
            background-color: #70cbd1; /* White background color for items */
        }

        /* Additional styling for specific cells if needed */
        .highlightedCell {
            background-color: #70cbd1; /* Yellow background color for highlighted cells */
        }

        /* .table-hover tbody tr:hover {
            background-color: #a2f9f1;
        }
*/
        .row-color-odd {
            background-color: #FFFFFF; /* Light gray background color for odd rows */
        }

        .row-color-even {
            background-color: #CFEFF2; /* White background color for even rows */
        }
    </style>


    <%--<label runat="server" id="label1"></label> <br />--%>
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

        <div class="container">
            <div class="row justify-content-center">
                <div class="col-md-12 panel-with-shadow" style="width: 700px; background-color: #CFEFF2; padding: 10px; border: 1px solid #c0c0c0; border-radius: 10px; padding: 20px;">
                    <table runat="server" class="table table-responsive border-color: #ffffff;">
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
         <div class="container" >
            <div class="row justify-content-center">
                <div class="col-md-12" style="width: 700px;">
                    <div class="table-striped table-responsive">

                        <asp:GridView ID="GridView2" runat="server" DataKeyNames="DEPENDENTNAME" class="w-100" CellPadding="3" CellSpacing="1"
                            CssClass="table table-striped table-hover  border-0 fw-normal table-bordered "
                            GridLines="None" AutoGenerateColumns="False" Style="overflow: scroll;" OnRowDataBound="GridView2_RowDataBound">
                            <HeaderStyle CssClass="sticky-header gridViewHeaderColor" />
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
       <div class="container" >
            <div class="row justify-content-center">
                <div class="col-md-12" style="width: 700px;">
                    <div class="table-striped table-responsive">

                        <asp:GridView ID="GridView3" runat="server" DataKeyNames="MEMBERAGE" class="w-100" CellPadding="3" CellSpacing="1"
                            CssClass="table table-striped table-hover  border-0 fw-normal table-bordered "
                            GridLines="None" AutoGenerateColumns="False" style="overflow: scroll;" OnRowDataBound="GridView2_RowDataBound">
                             <HeaderStyle CssClass="sticky-header gridViewHeaderColor" />
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
                <div class="col-md-12 panel-with-shadow" style="width: 900px; background-color: #CFEFF2; padding: 10px; border: 1px solid #c0c0c0; border-radius: 10px; padding: 20px;">
                    <table runat="server" class="table table-responsive border-color: #ffffff;">
                        <tr>
                            <td style="width: 55%;"><b>Policy No: </b>
                                <label runat="server" id="lblPoNo"></label>
                            </td>
                            <td style="width: 55%;">
                                <b>Company:</b>
                                <label runat="server" id="lblComNam" class="indented"></label>
                            </td>
                        </tr>
                        <tr>
                            <td class="auto-style1">
                                <b>Active State:</b>
                                <label runat="server" id="activeState" class="indented"></label>                               
                            </td>
                            <td class="auto-style1">
                                <b>Renewal Date:</b>
                                <label runat="server" id="lblReDate" class="indented"></label>
                            </td>
                        </tr>

                        <tr>
                            <td>
                                <b>Effective Date:</b>
                                <label runat="server" id="lblEffeDa" class="indented"></label>                                
                            </td>
                            <td>
                                <b>Debit Note No: </b>
                                <label runat="server" id="lblDebNoteNo" class="indented"></label>                               
                            </td>

                        </tr>

                        <tr>
                            <td>
                                <b>Critical Illness Employee Limit: </b>
                                <label runat="server" id="lblCriEmp2" class="indented"></label>                                                                
                            </td>
                            <td>
                                <b>Critical Illness Limit: </b>
                                <label runat="server" id="lblCriIlLim" class="indented"></label>                                
                            </td>

                        </tr>

                        <tr>
                            <td>
                                <b>Period:</b>
                                <label runat="server" id="lblPeri" class="indented"></label>                                
                            </td>
                            <td>
                                <b>Critical Illness Description1 :</b>
                                <label runat="server" id="lblCriDis1" class="indented"></label>                                
                            </td>

                        </tr>

                        <tr>
                            <td>
                                <b>Critical Illness Description2: </b>
                                <label runat="server" id="lblCriDis2" class="indented"></label>                               
                            </td>
                            <td>
                                <b>Critical Illness Description3:</b>
                                <label runat="server" id="lblCriDis3" class="indented"></label>                               
                            </td>

                        </tr>

                        <tr>
                            <td>
                                <b>Critical Illness Description4: </b>
                                <label runat="server" id="lblCriDis4" class="indented"></label>                                
                            </td>
                            <td>
                                <b>Other Benefits:</b>
                                <label runat="server" id="lblOtheBen" class="indented"></label>                               
                            </td>

                        </tr>

                        <tr>
                            <td>
                                <b>Indoor Medicine (I/F): </b>
                                <label runat="server" id="InMedi" class="indented"></label>                                
                            </td>
                            <td>
                                <b>Indoor Category Description:</b>
                                <label runat="server" id="inCaDe" class="indented"></label>                                
                            </td>

                        </tr>

                        <tr>
                            <td>
                                <b>Indoor Category: </b>
                                <label runat="server" id="InCa" class="indented"></label>                                
                            </td>
                            <td>
                                <b>Outdoor Medicine (I/F):</b>
                                <label runat="server" id="OutMed" class="indented"></label>                                
                            </td>

                        </tr>

                        <tr>
                            <td>
                                <b>Outdoor Category Description: </b>
                                <label runat="server" id="OutCaDe" class="indented"></label>
                                
                            </td>
                            <td>
                               <b>Outdoor Category:</b>
                                <label runat="server" id="OutCa" class="indented"></label>
                            </td>

                        </tr>


                    </table>


                </div>
            </div>
        </div>

        <%--<div class="container">
            <div class="row justify-content-center">
                <div class="col-md-12 panel-with-shadow" style="width: 700px; background-color: #CFEFF2; padding: 10px; border: 1px solid #c0c0c0; border-radius: 10px; padding: 20px;">
                    <br />
                    <div class="container">
                        <div class="row justify-content-center mt-4">
                            <div class="col-md-8">
                                <div class="form-group row">
                                    <label for="lblreferenceNo" class="col-sm-4 col-form-label ">Policy No </label>
                                    <div class="col-sm-8">--%>
                                        <%--<label class="col-sm-8" runat="server" id="lblPoNo"></label>--%>
                                        <%--<input type="text" class="form-control" runat="server" readonly="readonly" id="referenceNo" placeholder="referenceNo" style="width: 70%;">--%>
                                  <%--  </div>
                                </div>
                            </div>
                        </div>
                    </div>
                    <div class="container">
                        <div class="row justify-content-center mt-4">
                            <div class="col-md-8">
                                <div class="form-group row">
                                    <label for="lblName" class="col-sm-4 col-form-label ">Company </label>
                                    <div class="col-sm-8">--%>
                                        <%--<label class="col-sm-8" runat="server" id="lblComNam"></label>--%>
                                        <%--<input type="Text" class="form-control" runat="server" readonly="readonly" id="name" placeholder="Patient Name" style="width: 70%;">--%>
                                    <%--</div>
                                </div>
                            </div>
                        </div>
                    </div>
                    <div class="container">
                        <div class="row justify-content-center mt-4">
                            <div class="col-md-8">
                                <div class="form-group row">
                                    <label for="lblHospital" class="col-sm-4 col-form-label ">Renewal Date</label>
                                    <div class="col-sm-8">--%>
                                        <%--<label class="col-sm-8" runat="server" id="lblReDate"></label>--%>
                                   <%-- </div>
                                </div>
                            </div>
                        </div>
                    </div>
                    <div class="container">
                        <div class="row justify-content-center mt-4">
                            <div class="col-md-8">
                                <div class="form-group row">
                                    <label for="lblRoomNo" class="col-sm-4 col-form-label ">Effective Date</label>
                                    <div class="col-sm-8">--%>
                                       <%-- <label class="col-sm-8" runat="server" id="lblEffeDa"></label>--%>
                                    <%--</div>
                                </div>
                            </div>
                        </div>
                    </div>
                    <div class="container">
                        <div class="row justify-content-center mt-4">
                            <div class="col-md-8">
                                <div class="form-group row">
                                    <label for="lblAdmittedDate" class="col-sm-4 col-form-label ">Debit Note No</label>
                                    <div class="col-sm-8">--%>
                                        <%--<label class="col-sm-8" runat="server" id="lblDebNoteNo"></label>--%>
                                    <%--</div>
                                </div>
                            </div>
                        </div>
                    </div>

                    <div class="container">
                        <div class="row justify-content-center mt-4">
                            <div class="col-md-8">
                                <div class="form-group row">
                                    <label for="lblContactNo" class="col-md-6 col-form-label ">Critical Illness Limit</label>
                                    <div class="col-sm-8">--%>
                                        <%--<label class="col-md-6" runat="server" id="lblCriIlLim"></label>--%>
                                    <%--</div>
                                </div>
                            </div>
                        </div>
                    </div>

                    <div class="container">
                        <div class="row justify-content-center mt-4">
                            <div class="col-md-8">
                                <div class="form-group row">
                                    <label for="lblDischargeDate" class="col-sm-4 col-form-label ">Critical Illness Employee Limit </label>
                                    <div class="col-sm-8">--%>
                                        <%--<label class="col-sm-8" runat="server" id="lblCriEmp2"></label>--%>
                                    <%--</div>
                                </div>
                            </div>
                        </div>
                    </div>

                    <div class="container">
                        <div class="row justify-content-center mt-4">
                            <div class="col-md-8">
                                <div class="form-group row">
                                    <label for="lblEmployeeNo" class="col-sm-4 col-form-label ">Period </label>
                                    <div class="col-sm-8">--%>
                                        <%--<label class="col-sm-8" runat="server" id="lblPeri"></label>--%>
                                    <%--</div>
                                </div>
                            </div>
                        </div>
                    </div>

                    <div class="container">
                        <div class="row justify-content-center mt-4">
                            <div class="col-md-8">
                                <div class="form-group row">
                                    <label for="lblPolicyNo" class="col-sm-4 col-form-label ">Critical Illness Description1  </label>
                                    <div class="col-sm-8">--%>
                                       <%-- <label class="col-sm-8" runat="server" id="lblCriDis1"></label>--%>
                                    <%--</div>
                                </div>
                            </div>
                        </div>
                    </div>

                    <div class="container">
                        <div class="row justify-content-center mt-4">
                            <div class="col-md-8">
                                <div class="form-group row">
                                    <label for="lblPolicyNo" class="col-sm-4 col-form-label ">Critical Illness Description2 </label>
                                    <div class="col-sm-8">--%>
                                        <%--<label class="col-sm-8" runat="server" id="lblCriDis2"></label>--%>
                                    <%--</div>
                                </div>
                            </div>
                        </div>
                    </div>

                    <div class="container">
                        <div class="row justify-content-center mt-4">
                            <div class="col-md-8">
                                <div class="form-group row">
                                    <label for="lblPolicyNo" class="col-sm-4 col-form-label ">Critical Illness Description3</label>
                                    <div class="col-sm-8">--%>
                                        <%--<label class="col-sm-8" runat="server" id="lblCriDis3"></label>--%>
                                    <%--</div>
                                </div>
                            </div>
                        </div>
                    </div>

                    <div class="container">
                        <div class="row justify-content-center mt-4">
                            <div class="col-md-8">
                                <div class="form-group row">
                                    <label for="lblPolicyNo" class="col-sm-4 col-form-label ">Critical Illness Description4</label>
                                    <div class="col-sm-8">--%>
                                        <%--<label class="col-sm-8" runat="server" id="lblCriDis4"></label>--%>
                                    <%--</div>
                                </div>
                            </div>
                        </div>
                    </div>

                    <div class="container">
                        <div class="row justify-content-center mt-4">
                            <div class="col-md-8">
                                <div class="form-group row">
                                    <label for="lblPolicyNo" class="col-sm-4 col-form-label ">Other Benefits</label>
                                    <div class="col-sm-8">--%>
                                       <%-- <label class="col-sm-8" runat="server" id="lblOtheBen"></label>--%>
                                   <%-- </div>
                                </div>
                            </div>
                        </div>
                    </div>

                    <div class="container">
                        <div class="row justify-content-center mt-4">
                            <div class="col-md-8">
                                <div class="form-group row">
                                    <label for="lblPolicyNo" class="col-sm-4 col-form-label ">Indoor Medicine (I/F)</label>
                                    <div class="col-sm-8">--%>
                                        <%--<label class="col-sm-8" runat="server" id="InMedi"></label>--%>
                                   <%-- </div>
                                </div>
                            </div>
                        </div>
                    </div>

                    <div class="container">
                        <div class="row justify-content-center mt-4">
                            <div class="col-md-8">
                                <div class="form-group row">
                                    <label for="lblPolicyNo" class="col-sm-4 col-form-label ">Indoor Category Description</label>
                                    <div class="col-sm-8">--%>
                                        <%--<label class="col-sm-8" runat="server" id="inCaDe"></label>--%>
                                    <%--</div>
                                </div>
                            </div>
                        </div>
                    </div>

                    <div class="container">
                        <div class="row justify-content-center mt-4">
                            <div class="col-md-8">
                                <div class="form-group row">
                                    <label for="lblPolicyNo" class="col-sm-4 col-form-label ">Indoor Category </label>
                                    <div class="col-sm-8">--%>
                                       <%-- <label class="col-sm-8" runat="server" id="InCa"></label>--%>
                                    <%--</div>
                                </div>
                            </div>
                        </div>
                    </div>

                    <div class="container">
                        <div class="row justify-content-center mt-4">
                            <div class="col-md-8">
                                <div class="form-group row">
                                    <label for="lblPolicyNo" class="col-sm-4 col-form-label ">Outdoor Medicine (I/F)</label>
                                    <div class="col-sm-8">--%>
                                        <%--<label class="col-sm-8" runat="server" id="OutMed"></label>--%>
                       <%--             </div>
                                </div>
                            </div>
                        </div>
                    </div>

                    <div class="container">
                        <div class="row justify-content-center mt-4">
                            <div class="col-md-8">
                                <div class="form-group row">
                                    <label for="lblPolicyNo" class="col-md-6 col-form-label ">Outdoor Category Description</label>
                                    <div class="col-sm-8">--%>
                                        <%--<label class="col-md-6" runat="server" id="OutCaDe"></label>--%>
                                    <%--</div>
                                </div>
                            </div>
                        </div>
                    </div>

                    <div class="container">
                        <div class="row justify-content-center mt-4">
                            <div class="col-md-8">
                                <div class="form-group row">
                                    <label for="lblPolicyNo" class="col-sm-4 col-form-label ">Outdoor Category </label>
                                    <div class="col-sm-8">--%>
                                        <%--<label class="col-sm-8" runat="server" id="OutCa"></label>--%>
                                   <%-- </div>
                                </div>
                            </div>
                        </div>
                    </div>

                    <br />
                </div>
            </div>
        </div>--%>



    </asp:Panel>

    <%--panel 2--%>
    <asp:Panel runat="server" Visible="false" ID="mainpanel2">

        <br />
        <div class="container">
            <div class="row">
                <div class="col text-center">
                    <h4>Employee Benefit Details</h4>
                </div>
            </div>
        </div>

        <br />
        <div class="container" >
            <div class="row justify-content-center">
                <div class="col-md-12 panel-with-shadow" style="width: 700px; background-color: #CFEFF2; padding: 10px; border: 1px solid #c0c0c0; border-radius: 10px; padding: 20px;">
                    <table runat="server" class="table" style="border-color: #c0c0c0;">
                        <tr>
                            <td class="px-5"><b>Year Limit: </b>
                                <label runat="server" id="label3"></label>
                            </td>
                            <td>
                                <b>Event Limit: </b>
                                <label runat="server" id="label8"></label>
                            </td>
                        </tr>
                        <tr>
                            <td class="px-5">
                                <b>OPD Limit: </b>
                                <label runat="server" id="label9"></label>

                            </td>
                            <td>
                                <b>OPD Balance Limit:</b>
                                <label runat="server" id="label10"></label>
                            </td>
                        </tr>

                        <tr>
                            <td class="px-5">
                                <b>Room Limit: </b>
                                <label runat="server" id="label11"></label>
                            </td>
                            <td>
                                <b>ICU Room Limit: </b>
                                <label runat="server" id="label12"></label>
                            </td>

                        </tr>


                    </table>


                </div>
            </div>
        </div>
        <br />


        <%--<div class="container">
            <div class="row">
                <div class="col text-center">
                    <h5>Beneficiary Details</h5>
                </div>
            </div>
        </div>--%>
        <%-- <script type="text/javascript">
            function displaySelectedName() {
                var selectedValue = document.getElementById("<%= myDropdown.ClientID %>").value;
            var selectedName = document.getElementById("<%= myDropdown.ClientID %>").options[document.getElementById("<%= myDropdown.ClientID %>").selectedIndex].text;
                document.getElementById("selectedName").innerHTML = " " + selectedValue;
                sessionStorage.setItem("selectedValue", selectedValue);
                sessionStorage.setItem("selectedName", selectedName);
                document.getElementById("selectedValueHiddenField").value = selectedValue; // set the value of the first hidden field
                document.getElementById("selectedNameHiddenField").value = selectedName; // set the value of the second hidden field

            }
        </script>--%>

        <%--  <div class="container">
            <div class="row">
                <div class="col-12">
                    <b>Member:</b>
                    <asp:DropDownList ID="myDropdown" runat="server" onchange="displaySelectedName()" Visible="true" CssClass="alert-light" Style="border-color: ButtonShadow; outline: none; margin-top: 15px; margin-left: 20px; width: 250px; height: 25px; border-collapse: collapse;">
                        <%--<asp:ListItem Text="Select a value" Value=""></asp:ListItem>--%>
        <%--</asp:DropDownList>
                    <asp:RequiredFieldValidator ID="myDropdownValidator" runat="server" ControlToValidate="myDropdown" CssClass="validation"
                        ErrorMessage="*Please select a Beneficiary from the list." Display="Dynamic"></asp:RequiredFieldValidator>
                </div>
            </div>
        </div>--%>
        <br />


        <style>
            .row-divider {
                border-bottom: 1px solid black; /* Set the border color and thickness as needed */
                padding-bottom: 5px; /* Adjust the spacing between rows as needed */
            }

            .indented {
                margin-left: 5px; /* Adjust the value as needed for indentation */
                font-weight: 600
            }
        </style>

        <%--grid3--%>

        <div class="container" >
            <div class="row justify-content-center">
                <div class="col-md-12" style="width: 700px;">
                    <div class="table-striped table-responsive">


                        <%--<asp:GridView ID="GridView1" runat="server" AutoGenerateColumns="False">--%>
                        <asp:GridView ID="GridView1" runat="server" DataKeyNames="AnnualLimit" class="w-80" CellPadding="3" CellSpacing="1"
                            CssClass="table table-striped  border-0 fw-normal table-bordered "
                            GridLines="Both" AutoGenerateColumns="False" style="overflow: scroll;" OnRowDataBound="GridView2_RowDataBound">
                            <HeaderStyle CssClass="sticky-header gridViewHeaderColor" />
                            <Columns>

                                <asp:TemplateField HeaderText="Sublimit Name" ControlStyle-Height="35px">
                                    <ItemTemplate>
                                        <b>
                                            <asp:Label ID="IndLimit1" runat="server" Text="Indoor 1 Limit"></asp:Label></b><br />

                                        <b>
                                            <asp:Label ID="IndLimit2" runat="server" Text="Indoor 2 Limit"></asp:Label></b><br />

                                        <b>
                                            <asp:Label ID="IndLimit3" runat="server" Text="Indoor 3 Limit"></asp:Label></b><br />

                                        <b>
                                            <asp:Label ID="IndLimit4" runat="server" Text="Indoor 4 Limit"></asp:Label></b><br />

                                        <b>
                                            <asp:Label ID="IndLimit5" runat="server" Text="Indoor 5 Limit"></asp:Label></b><br />

                                        <b>
                                            <asp:Label ID="IndLimit6" runat="server" Text="Indoor 6 Limit"></asp:Label></b><br />

                                        <b>
                                            <asp:Label ID="IndLimit7" runat="server" Text="Indoor EXT1 Limit"></asp:Label></b><br />

                                        <b>
                                            <asp:Label ID="IndLimit8" runat="server" Text="Indoor EXT2 Limit"></asp:Label></b><br />
                                        <br />

                                        <b>
                                            <asp:Label ID="IndLimit9" runat="server" Text="Available Balance"></asp:Label></b>


                                    </ItemTemplate>
                                </asp:TemplateField>

                                <asp:TemplateField HeaderText="Sublimit Value" ControlStyle-Height="35px">
                                    <ItemTemplate>
                                        <asp:Label ID="Subval1" runat="server" Text="" Style="text-align:right;"></asp:Label><br />
                                        <asp:Label ID="Subval2" runat="server" Text=""></asp:Label><br />
                                        <asp:Label ID="Subval3" runat="server" Text=""></asp:Label><br />
                                        <asp:Label ID="Subval4" runat="server" Text=""></asp:Label><br />
                                        <asp:Label ID="Subval5" runat="server" Text=""></asp:Label><br />
                                        <asp:Label ID="Subval6" runat="server" Text=""></asp:Label><br />
                                        <asp:Label ID="Subval7" runat="server" Text=""></asp:Label><br />
                                        <asp:Label ID="Subval8" runat="server" Text=""></asp:Label><br />
                                        <%--<asp:Label ID="Subval9" runat="server" Text=""></asp:Label>--%>
                                    </ItemTemplate>
                                </asp:TemplateField>


                                <asp:TemplateField HeaderText="Paid Amount" ControlStyle-Height="35px">
                                    <ItemTemplate>
                                        <asp:Label ID="paidam1" runat="server" Text=""></asp:Label><br />
                                        <asp:Label ID="paidam2" runat="server" Text=""></asp:Label><br />
                                        <asp:Label ID="paidam3" runat="server" Text=""></asp:Label><br />
                                        <asp:Label ID="paidam4" runat="server" Text=""></asp:Label><br />
                                        <asp:Label ID="paidam5" runat="server" Text=""></asp:Label><br />
                                        <asp:Label ID="paidam6" runat="server" Text=""></asp:Label><br />
                                        <asp:Label ID="paidam7" runat="server" Text=""></asp:Label><br />
                                        <asp:Label ID="paidam8" runat="server" Text=""></asp:Label><br />
                                        <%--<asp:Label ID="paidam9" runat="server" Text=""></asp:Label>--%>
                                    </ItemTemplate>
                                </asp:TemplateField>


                                <asp:TemplateField HeaderText="Balance" ControlStyle-Height="35px">
                                    <ItemTemplate>
                                        <asp:Label ID="bal1" runat="server" Text=""></asp:Label><br />
                                        <asp:Label ID="bal2" runat="server" Text=""></asp:Label><br />
                                        <asp:Label ID="bal3" runat="server" Text=""></asp:Label><br />
                                        <asp:Label ID="bal4" runat="server" Text=""></asp:Label><br />
                                        <asp:Label ID="bal5" runat="server" Text=""></asp:Label><br />
                                        <asp:Label ID="bal6" runat="server" Text=""></asp:Label><br />
                                        <asp:Label ID="bal7" runat="server" Text=""></asp:Label><br />
                                        <asp:Label ID="bal8" runat="server" Text=""></asp:Label><br />
                                        <br />
                                        <asp:Label ID="bal9" runat="server" Text="" Style="font-weight: 700;"></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                        </asp:GridView>
                    </div>
                </div>
            </div>
        </div>
        <br />

        <div class="container">
            <div class="row justify-content-center">
                <div class="col-md-12 panel-with-shadow" style="width: 700px; background-color: #CFEFF2; padding: 10px; border: 1px solid #c0c0c0; border-radius: 10px; padding: 20px;">
                    <table runat="server" class="table table-responsive border-color: #ffffff;">
                        <tr>
                            <td style="width: 55%;"><b>CIC Status: </b>
                                <label runat="server" id="label13"></label>
                            </td>
                            <td style="width: 55%;">
                                <b>CIC Limit:</b>
                                <label runat="server" id="label14" class="indented"></label>
                            </td>


                        </tr>
                        <tr>
                            <td>
                                <b>Day Care Surgery Limit:</b>
                                <label runat="server" id="label15" class="indented"></label>

                            </td>
                            <td>
                                <b>Vat Payable:</b>
                                <label runat="server" id="label16" class="indented"></label>
                            </td>
                        </tr>

                        <tr>
                            <td>
                                <b>Endoscopy/Colonoscopy: </b>
                                <label runat="server" id="label17" class="indented"></label>
                            </td>
                            <td>
                                <b>Catract Lence limit: </b>
                                <label runat="server" id="label18" class="indented"></label>
                            </td>

                        </tr>

                        <tr>
                            <td>
                                <b>Fertility Cover Limit: </b>
                                <label runat="server" id="label19" class="indented"></label>
                            </td>
                            <td>
                                <b>Congenital Cover:</b>
                                <label runat="server" id="label20" class="indented"></label>
                            </td>

                        </tr>

                        <tr>
                            <td>
                                <b>Covid Cover Limit:</b>
                                <label runat="server" id="label21" class="indented"></label>
                            </td>
                            <td>
                                <b>Dental Doctor's Fee Limit: </b>
                                <label runat="server" id="label22" class="indented"></label>
                            </td>

                        </tr>

                        <tr>
                            <td>
                                <b>Policy Excess:</b>
                                <label runat="server" id="label23" class="indented"></label>
                            </td>
                            <td>
                                <b>Ten Month Applicable: </b>
                                <label runat="server" id="label24" class="indented"></label>
                            </td>

                        </tr>

                        <tr>
                            <td>
                                <b>Ceserian Cover Limit:</b>
                                <label runat="server" id="label25" class="indented"></label>
                            </td>
                            <td>
                                <b>NDC Cover Limit: </b>
                                <label runat="server" id="label26" class="indented"></label>
                            </td>

                        </tr>

                        <tr>
                            <td>
                                <b>Forcep/Vaccum Limit:</b>
                                <label runat="server" id="label27" class="indented"></label>
                            </td>
                            <td>
                                <b>Pregnancy Related Cover Limit: </b>
                                <label runat="server" id="label28" class="indented"></label>
                            </td>

                        </tr>

                        <tr>
                            <td>
                                <b>Terms and Conditions:</b>
                                <label runat="server" id="label29" class="indented"></label>
                            </td>
                            <td>
                                <b>Exclusions(Special Remark): </b>
                                <label runat="server" id="label30" class="indented"></label>
                            </td>

                        </tr>


                    </table>


                </div>
            </div>
        </div>
        <br />
    </asp:Panel>
    <br />

    <%--panel 3--%>
    <asp:Panel runat="server" Visible="false" ID="mainpanel3">

        <br />
        <div class="container ">
            <div class="row">
                <div class="col text-center">
                    <h4>SHE Pending Claims Details</h4>
                </div>
            </div>
        </div>

        <br />
        <div class="container" >
            <div class="row justify-content-center">
                <div class="col-md-12 panel-with-shadow" style="width: 700px; background-color: #CFEFF2; padding: 10px; border: 1px solid #c0c0c0; border-radius: 10px; padding: 20px;">
                    <table runat="server" class="table table-responsive" style="border-color: #c0c0c0;">
                        <tr>
                            <td class="px-5"><b>Available Balance: </b>
                                <label runat="server" id="label33"></label>
                            </td>
                            <td>
                                <b>Policy Period: </b>
                                <label runat="server" id="label34"></label>
                            </td>
                        </tr>

                    </table>


                </div>
            </div>
        </div>

        <br />

        <div class="container" >
             <div class="row justify-content-center">
                <div class="col-md-12" style="width: 800px;">
                    <div class="table-striped table-responsive">

                        <asp:GridView ID="GridView4" runat="server" DataKeyNames="CLAIMNO" class="w-100" CellPadding="3" CellSpacing="1"
                            CssClass="table table-striped table-hover  border-0 fw-normal table-bordered "
                            GridLines="None" AutoGenerateColumns="False" style="overflow: scroll;" OnRowDataBound="GridView2_RowDataBound">
                             <HeaderStyle CssClass="sticky-header gridViewHeaderColor" />
                            <Columns>

                                <asp:BoundField DataField="CLAIMNO" HeaderText="Reference"
                                    HeaderStyle-CssClass="testClassHeader" ItemStyle-CssClass="testClass">
                                    <HeaderStyle CssClass="testClassHeader"></HeaderStyle>
                                    <ItemStyle CssClass="testClass" Width="350px"></ItemStyle>
                                </asp:BoundField>


                                <asp:BoundField DataField="HOSPITAL" HeaderText="Hospital"
                                    HeaderStyle-CssClass="testClassHeader" ItemStyle-CssClass="testClass">
                                    <HeaderStyle CssClass="testClassHeader"></HeaderStyle>
                                    <ItemStyle CssClass="testClass" Width="300px"></ItemStyle>
                                </asp:BoundField>

                                <asp:BoundField DataField="ROOMNUMBER" HeaderText="Room Number"
                                    HeaderStyle-CssClass="testClassHeader" ItemStyle-CssClass="testClass">
                                    <HeaderStyle CssClass="testClassHeader"></HeaderStyle>
                                    <ItemStyle CssClass="testClass" Width="350px"></ItemStyle>
                                </asp:BoundField>

                                <asp:BoundField DataField="ADMISSION_DATE" HeaderText="Admission Date"
                                    HeaderStyle-CssClass="testClassHeader" ItemStyle-CssClass="testClass">
                                    <HeaderStyle CssClass="testClassHeader"></HeaderStyle>
                                    <ItemStyle CssClass="testClass" Width="300px"></ItemStyle>
                                </asp:BoundField>

                                <asp:BoundField DataField="DISCHARGE_DATE" HeaderText="Discharge Date"
                                    HeaderStyle-CssClass="testClassHeader" ItemStyle-CssClass="testClass">
                                    <HeaderStyle CssClass="testClassHeader"></HeaderStyle>
                                    <ItemStyle CssClass="testClass" Width="300px"></ItemStyle>
                                </asp:BoundField>

                                <asp:BoundField DataField="PATIENT_NAME" HeaderText="Patient Name"
                                    HeaderStyle-CssClass="testClassHeader" ItemStyle-CssClass="testClass">
                                    <HeaderStyle CssClass="testClassHeader"></HeaderStyle>
                                    <ItemStyle CssClass="testClass" Width="300px"></ItemStyle>
                                </asp:BoundField>

                                <asp:BoundField DataField="PAYMENT_AMOUNT" HeaderText="Payment Amount"
                                    HeaderStyle-CssClass="testClassHeader" ItemStyle-CssClass="testClass">
                                    <HeaderStyle CssClass="testClassHeader"></HeaderStyle>
                                    <ItemStyle CssClass="testClass" Width="400px" HorizontalAlign="Right"></ItemStyle>
                                </asp:BoundField>

                                <asp:BoundField DataField="STATUS_TYPE_NAME" HeaderText="Claim Status"
                                    HeaderStyle-CssClass="testClassHeader" ItemStyle-CssClass="testClass">
                                    <HeaderStyle CssClass="testClassHeader"></HeaderStyle>
                                    <ItemStyle CssClass="testClass" Width="350px"></ItemStyle>
                                </asp:BoundField>


                            </Columns>
                            <PagerStyle BackColor="Transparent" CssClass="pagerClz" Height="10px" VerticalAlign="Bottom" HorizontalAlign="Left" />
                        </asp:GridView>

                    </div>
                </div>
            </div>
        </div>
        <br />
    </asp:Panel>
     
    <%--panel 4--%>
    <asp:Panel runat="server" Visible="false" ID="mainpanel4">

        <br />
        <div class="container ">
            <div class="row">
                <div class="col text-center">
                    <h4>SHE Previous Claims Details</h4>
                </div>
            </div>
        </div>

        <br />
        <%--  <div class="container" style="background-color: #c0c0c0; padding-top:8px;">
            <div class="row">
                <div class="col-12 ">
                    <table runat="server" class="table" style="border-color: #c0c0c0;">
                        <tr>
                            <td class="px-5"><b>Available Balance: </b>
                                <label runat="server" id="label35"></label>
                            </td>
                            <td>
                                <b>Policy Period: </b>
                                <label runat="server" id="label36"></label>
                            </td>
                        </tr>
                  
                    </table>


                </div>
            </div>
        </div>--%>

        <div class="container" >
            <div class="row justify-content-center">
                <div class="col-md-12" style="width: 800px;">
                    <div class="table-striped table-responsive">

                        <asp:GridView ID="GridView5" runat="server" DataKeyNames="PATIENTNAME" class="w-100" CellPadding="3" CellSpacing="1"
                            CssClass="table table-striped table-hover  border-0 fw-normal table-bordered "
                            GridLines="None" AutoGenerateColumns="False" style="overflow: scroll;" OnRowDataBound="GridView2_RowDataBound">
                            <HeaderStyle CssClass="sticky-header gridViewHeaderColor" />
                            <Columns>

                                <asp:BoundField DataField="PATIENTNAME" HeaderText="Patient Name"
                                    HeaderStyle-CssClass="testClassHeader" ItemStyle-CssClass="testClass">
                                    <HeaderStyle CssClass="testClassHeader"></HeaderStyle>
                                    <ItemStyle CssClass="testClass" Width="350px"></ItemStyle>
                                </asp:BoundField>

                                <asp:BoundField DataField="CLAIMDATE" HeaderText="Claim Date"
                                    HeaderStyle-CssClass="testClassHeader" ItemStyle-CssClass="testClass">
                                    <HeaderStyle CssClass="testClassHeader"></HeaderStyle>
                                    <ItemStyle CssClass="testClass" Width="300px"></ItemStyle>
                                </asp:BoundField>

                                <asp:BoundField DataField="CLAIMAMOUNT" HeaderText="Claim Amount"
                                    HeaderStyle-CssClass="testClassHeader" ItemStyle-CssClass="testClass" >
                                    <HeaderStyle CssClass="testClassHeader"></HeaderStyle>
                                    <ItemStyle CssClass="testClass" Width="350px" HorizontalAlign="Right"></ItemStyle>
                                </asp:BoundField>

                                <asp:BoundField DataField="AMOUNTPAID" HeaderText="Amount Paid"
                                    HeaderStyle-CssClass="testClassHeader" ItemStyle-CssClass="testClass" >
                                    <HeaderStyle CssClass="testClassHeader"></HeaderStyle>
                                    <ItemStyle CssClass="testClass" Width="300px" HorizontalAlign="Right"></ItemStyle>
                                </asp:BoundField>

                                <asp:BoundField DataField="CLAIMNO" HeaderText="Reference Number"
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
        <br />
    </asp:Panel>
     
    <div class="container">
        <div class="row justify-content-center">
            <div class="col-md-12 panel" style="width: 900px; ">
                <div class="row justify-content-center ">
                <div class="col-sm-6 col-md-3 mb-2 d-flex justify-content-center">
                <asp:Button runat="server" ID="btnIconClick1" class="btn btn-primary mb-2 mb-sm-0" Style="width: 100%; max-width: 200px; border-radius: 5px; background-color: #4dc6d0; color: white; font-weight: bold; font-size: 15px; border-color: #4dc6d0" Text="Policy Details" OnClick="IconClick_ServerClick" />
                </div>
                <%--Page--%>
                <%--<svg xmlns="http://www.w3.org/2000/svg" width="20" height="20" fill="currentColor" CssClass="btn btn-danger mx-4" viewBox="0 0 16 16" onclick="document.getElementById('<%= btnIconClick1.ClientID %>').click()">
                    <path d="M2 0a2 2 0 0 0-2 2v12a2 2 0 0 0 2 2h12a2 2 0 0 0 2-2V2a2 2 0 0 0-2-2H2Zm7.283 4.002V12H7.971V5.338h-.065L6.072 6.656V5.385l1.899-1.383h1.312Z" />
                </svg>--%>

                <div class="col-sm-6 col-md-3 mb-2 d-flex justify-content-center">
                <asp:Button runat="server" ID="btnIconClick2" OnClick="IconClick_ServerClick2" class="btn btn-primary mb-2 mb-sm-0" Style="width: 100%; max-width: 200px; border-radius: 5px; background-color: #4dc6d0; color: white; font-weight: bold; font-size: 15px; border-color: #4dc6d0" Text="Benefits" OnClientClick="return clientFunctionValidationFinished()"/>
                </div>
                    <%--<svg xmlns="http://www.w3.org/2000/svg" width="20" height="20" fill="currentColor" class="bi bi-2-square-fill" viewBox="0 0 16 16" onclick="document.getElementById('<%= btnIconClick2.ClientID %>').click()">
                    <path d="M2 0a2 2 0 0 0-2 2v12a2 2 0 0 0 2 2h12a2 2 0 0 0 2-2V2a2 2 0 0 0-2-2H2Zm4.646 6.24v.07H5.375v-.064c0-1.213.879-2.402 2.637-2.402 1.582 0 2.613.949 2.613 2.215 0 1.002-.6 1.667-1.287 2.43l-.096.107-1.974 2.22v.077h3.498V12H5.422v-.832l2.97-3.293c.434-.475.903-1.008.903-1.705 0-.744-.557-1.236-1.313-1.236-.843 0-1.336.615-1.336 1.306Z" />
                </svg>--%>

                <div class="col-sm-6 col-md-3 mb-2 d-flex justify-content-center">
                <asp:Button runat="server" ID="btnIconClick3" OnClick="IconClick_ServerClick3" class="btn btn-primary mb-2 mb-sm-0" Style="width: 100%; max-width: 200px; border-radius: 5px; background-color: #4dc6d0; color: white; font-weight: bold; font-size: 15px; border-color: #4dc6d0" Text="Pending Claims" OnClientClick="return clientFunctionValidationFinished()" />
                </div>
                    <%--<svg xmlns="http://www.w3.org/2000/svg" width="20" height="20" fill="currentColor" class="bi bi-3-square-fill" viewBox="0 0 16 16" onclick="document.getElementById('<%= btnIconClick3.ClientID %>').click()">
                    <path d="M2 0a2 2 0 0 0-2 2v12a2 2 0 0 0 2 2h12a2 2 0 0 0 2-2V2a2 2 0 0 0-2-2H2Zm5.918 8.414h-.879V7.342h.838c.78 0 1.348-.522 1.342-1.237 0-.709-.563-1.195-1.348-1.195-.79 0-1.312.498-1.348 1.055H5.275c.036-1.137.95-2.115 2.625-2.121 1.594-.012 2.608.885 2.637 2.062.023 1.137-.885 1.776-1.482 1.875v.07c.703.07 1.71.64 1.734 1.917.024 1.459-1.277 2.396-2.93 2.396-1.705 0-2.707-.967-2.754-2.144H6.33c.059.597.68 1.06 1.541 1.066.973.006 1.6-.563 1.588-1.354-.006-.779-.621-1.318-1.541-1.318Z" />
                </svg>--%>

                <div class="col-sm-6 col-md-3 mb-2 d-flex justify-content-center">
                <asp:Button runat="server" ID="btnIconClick4" OnClick="IconClick_ServerClick4" class="btn btn-primary mb-2 mb-sm-0" Style="width: 100%; max-width: 200px; border-radius: 5px; background-color: #4dc6d0; color: white; font-weight: bold; font-size: 15px; border-color: #4dc6d0" Text="Previous Claims"  OnClientClick="return clientFunctionValidationFinished()" />
                </div>
                    <%--<svg xmlns="http://www.w3.org/2000/svg" width="20" height="20" fill="currentColor" class="bi bi-4-square-fill" viewBox="0 0 16 16" onclick="document.getElementById('<%= btnIconClick4.ClientID %>').click()">
                    <path d="M6.225 9.281v.053H8.85V5.063h-.065c-.867 1.33-1.787 2.806-2.56 4.218Z" />
                    <path d="M2 0a2 2 0 0 0-2 2v12a2 2 0 0 0 2 2h12a2 2 0 0 0 2-2V2a2 2 0 0 0-2-2H2Zm5.519 5.057c.22-.352.439-.703.657-1.055h1.933v5.332h1.008v1.107H10.11V12H8.85v-1.559H4.978V9.322c.77-1.427 1.656-2.847 2.542-4.265Z" />
                </svg>--%>
                </div>
            </div>
        </div>
        <br />
        <div class="row">
            <div class="col-12 text-center">
                <asp:Button runat="server" ID="backToDataView" OnClick="backClick" class="btn btn-primary mb-2 mb-sm-0" Style="width: 100%; max-width: 200px; border-radius: 5px; background-color: #4dc6d0; color: white; font-weight: bold; font-size: 15px; border-color: #4dc6d0" Text="Back" />
            <%--<asp:Button ID="backToDataView" runat="server" Text="Back"  OnClick="backClick"/>--%>
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
                icon: 'Processing',
                button: false,
                closeOnClickOutside: false,
            });

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
                    //icon: "Images/process3.gif",
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