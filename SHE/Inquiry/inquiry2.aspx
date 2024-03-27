<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/Site.Master"  CodeBehind="inquiry2.aspx.cs" Inherits="SHE.Inquiry.inquiry2" %>
<%@ Import Namespace="SHE.Code"%>




<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    <asp:HiddenField runat="server" ID="hiddenEndDate"/>
    <style>
        .sticky-header {
            position: sticky;
            top: -1%;
            background-color: #D8D8D8;
            z-index: 1;
            border: none; /* Add the desired border style */
        }

        label {
            font-weight: 400;
        }

        .btn-primary {
            background-color: #4dc6d0;
            color: #fff;
            border-color: #4dc6d0;
            margin-left: 8%;
        }

        .btn {
            background-color: #4dc6d0;
        }
    </style>


  <%--  <%


        EncryptDecrypt dc = new EncryptDecrypt();
        //Class1 class1 = new Class1(); 

        string fromdate = (string)Request.QueryString["fromdate"];
        fromdate = dc.Decrypt(fromdate);
        string todate = (string)Request.QueryString["todate"];
        todate = dc.Decrypt(todate);

    %>--%>

    <div class="container" style="padding: 10px;">
        <div class="row">
            <div runat="server" class="col-12" id="datediv" visible="false">

                <h4 style="text-align: center;">SHE Hospitalization Details From 
              <label runat="server" id="label1"></label> To <label runat="server" id="label2"></label>
                
                </h4>
            </div>

            <div runat="server" class="col-12" id="reference" visible="false">

                <h4 style="text-align: center;">SHE Hospitalization Details Of Reference Number <label runat="server" id="label3"></label></h4>
                
            </div>
            <div runat="server" class="col-12" id="dateandref" Visible="false">

                <h4 style="text-align: center;">SHE Hospitalization Details From 
              <label runat="server" id="label4">  </label> To <label runat="server" id="label5"></label> Of Reference Number <label runat="server" id="label6"></label></h4>
            </div>
            
            <div runat="server" class="col-12" id="fromandref" Visible="false">

                <h4 style="text-align: center;">SHE Hospitalization Details From <label runat="server" id="label7"></label>
             Of Reference Number <label runat="server" id="label8"></label></h4>
            </div>
        </div>
    </div>
    <br />
    <div class="container">
        <div class="row">
            <div class="col-12">

                <asp:UpdatePanel runat="server" ID="gridview2update" UpdateMode="Conditional">
                    <ContentTemplate>
                        <div class="table-responsive" style="height: 500px;">
                            <asp:GridView ID="GridView2" runat="server" DataKeyNames="CLAIM_REF_NO" class="w-100 " CellPadding="3" CellSpacing="1" EnableEventValidation="false"
                                CssClass="table table-striped table-hover  border-0 fw-normal table-bordered "
                                GridLines="None" AutoGenerateColumns="false" Style="overflow: scroll;">
                                <HeaderStyle CssClass="sticky-header" />
                                <Columns>
                                    <asp:BoundField DataField="CLAIM_REF_NO" HeaderText="Claim Reference Number"
                                        HeaderStyle-CssClass="testClassHeader" ItemStyle-CssClass="testClass">
                                        <HeaderStyle CssClass="testClassHeader"></HeaderStyle>
                                        <ItemStyle CssClass="testClass" Width="300px"></ItemStyle>
                                    </asp:BoundField>

                                    <asp:BoundField DataField="PNAME" HeaderText="Patient's Name"
                                        HeaderStyle-CssClass="testClassHeader" ItemStyle-CssClass="testClass">
                                        <HeaderStyle CssClass="testClassHeader"></HeaderStyle>
                                        <ItemStyle CssClass="testClass" Width="300px"></ItemStyle>
                                    </asp:BoundField>

                                    <asp:BoundField DataField="HOSPITAL" HeaderText="Hospital"
                                        HeaderStyle-CssClass="testClassHeader" ItemStyle-CssClass="testClass">
                                        <HeaderStyle CssClass="testClassHeader"></HeaderStyle>
                                        <ItemStyle CssClass="testClass" Width="300px"></ItemStyle>
                                    </asp:BoundField>

                                    <asp:BoundField DataField="ROOM_NO" HeaderText="Room Number"
                                        HeaderStyle-CssClass="testClassHeader" ItemStyle-CssClass="testClass">
                                        <HeaderStyle CssClass="testClassHeader"></HeaderStyle>
                                        <ItemStyle CssClass="testClass" Width="300px"></ItemStyle>
                                    </asp:BoundField>

                                    <asp:BoundField DataField="CON_NO" HeaderText="Contact Number"
                                        HeaderStyle-CssClass="testClassHeader" ItemStyle-CssClass="testClass">
                                        <HeaderStyle CssClass="testClassHeader"></HeaderStyle>
                                        <ItemStyle CssClass="testClass" Width="300px"></ItemStyle>
                                    </asp:BoundField>


                                    <asp:BoundField DataField="ADD_DATE" HeaderText="Admitted Date"
                                        HeaderStyle-CssClass="testClassHeader" ItemStyle-CssClass="testClass">
                                        <HeaderStyle CssClass="testClassHeader"></HeaderStyle>
                                        <ItemStyle CssClass="testClass" Width="300px"></ItemStyle>
                                    </asp:BoundField>

                                    <asp:BoundField DataField="EMP_NO" HeaderText="Employee Number"
                                        HeaderStyle-CssClass="testClassHeader" ItemStyle-CssClass="testClass">
                                        <HeaderStyle CssClass="testClassHeader"></HeaderStyle>
                                        <ItemStyle CssClass="testClass" Width="300px"></ItemStyle>
                                    </asp:BoundField>

                                    <asp:BoundField DataField="POL_NO" HeaderText="Policy Number"
                                        HeaderStyle-CssClass="testClassHeader" ItemStyle-CssClass="testClass">
                                        <HeaderStyle CssClass="testClassHeader"></HeaderStyle>
                                        <ItemStyle CssClass="testClass" Width="300px"></ItemStyle>
                                    </asp:BoundField>

                                    <asp:BoundField DataField="CALLER_NAME" HeaderText="Caller Name"
                                        HeaderStyle-CssClass="testClassHeader" ItemStyle-CssClass="testClass">
                                        <HeaderStyle CssClass="testClassHeader"></HeaderStyle>
                                        <ItemStyle CssClass="testClass" Width="300px"></ItemStyle>
                                    </asp:BoundField>

                                    <asp:BoundField DataField="USER_ADD" HeaderText="User(Adm. Entered)"
                                        HeaderStyle-CssClass="testClassHeader" ItemStyle-CssClass="testClass">
                                        <HeaderStyle CssClass="testClassHeader"></HeaderStyle>
                                        <ItemStyle CssClass="testClass" Width="300px"></ItemStyle>
                                    </asp:BoundField>

                                    <asp:BoundField DataField="DIS_DATE" HeaderText="Discharge Date"
                                        HeaderStyle-CssClass="testClassHeader" ItemStyle-CssClass="testClass">
                                        <HeaderStyle CssClass="testClassHeader"></HeaderStyle>
                                        <ItemStyle CssClass="testClass" Width="300px"></ItemStyle>
                                    </asp:BoundField>

                                    <asp:BoundField DataField="REC_ADD_TIME" HeaderText="Record Added Time"
                                        HeaderStyle-CssClass="testClassHeader" ItemStyle-CssClass="testClass">
                                        <HeaderStyle CssClass="testClassHeader"></HeaderStyle>
                                        <ItemStyle CssClass="testClass" Width="300px"></ItemStyle>
                                    </asp:BoundField>

                                    <asp:BoundField DataField="UPDATE_DATE_DIS" HeaderText="Update Date(Discharge)"
                                        HeaderStyle-CssClass="testClassHeader" ItemStyle-CssClass="testClass">
                                        <HeaderStyle CssClass="testClassHeader"></HeaderStyle>
                                        <ItemStyle CssClass="testClass" Width="300px"></ItemStyle>
                                    </asp:BoundField>

                                    <asp:BoundField DataField="REC_UPDATE_TIME" HeaderText="Record Update Time(Discharge)"
                                        HeaderStyle-CssClass="testClassHeader" ItemStyle-CssClass="testClass">
                                        <HeaderStyle CssClass="testClassHeader"></HeaderStyle>
                                        <ItemStyle CssClass="testClass" Width="300px"></ItemStyle>
                                    </asp:BoundField>

                                    <asp:BoundField DataField="COORDINATOR_ADD" HeaderText="Coordinator (Admission Inform)"
                                        HeaderStyle-CssClass="testClassHeader" ItemStyle-CssClass="testClass">
                                        <HeaderStyle CssClass="testClassHeader"></HeaderStyle>
                                        <ItemStyle CssClass="testClass" Width="300px"></ItemStyle>
                                    </asp:BoundField>

                                    <asp:BoundField DataField="USR_ADMIN_UPDATE" HeaderText="User(Adm. Co-od. Update)"
                                        HeaderStyle-CssClass="testClassHeader" ItemStyle-CssClass="testClass">
                                        <HeaderStyle CssClass="testClassHeader"></HeaderStyle>
                                        <ItemStyle CssClass="testClass" Width="300px"></ItemStyle>
                                    </asp:BoundField>

                                    <asp:BoundField DataField="DATE_ADMIN_UPDATE" HeaderText="Date(Adm. Co-od. Update)"
                                        HeaderStyle-CssClass="testClassHeader" ItemStyle-CssClass="testClass">
                                        <HeaderStyle CssClass="testClassHeader"></HeaderStyle>
                                        <ItemStyle CssClass="testClass" Width="300px"></ItemStyle>
                                    </asp:BoundField>

                                    <asp:BoundField DataField="TIME_ADMIN_UPDATE" HeaderText="Time(Adm. Co-od. Update)"
                                        HeaderStyle-CssClass="testClassHeader" ItemStyle-CssClass="testClass">
                                        <HeaderStyle CssClass="testClassHeader"></HeaderStyle>
                                        <ItemStyle CssClass="testClass" Width="300px"></ItemStyle>
                                    </asp:BoundField>

                                    <asp:BoundField DataField="COORDINATOR_DIS" HeaderText="Co-ordinator(Discharge Inform)"
                                        HeaderStyle-CssClass="testClassHeader" ItemStyle-CssClass="testClass">
                                        <HeaderStyle CssClass="testClassHeader"></HeaderStyle>
                                        <ItemStyle CssClass="testClass" Width="300px"></ItemStyle>
                                    </asp:BoundField>

                                    <asp:TemplateField HeaderText="Claim History"
                                        HeaderStyle-CssClass="testClassHeader"
                                        ItemStyle-CssClass="testClass" ItemStyle-Width="200px" ItemStyle-VerticalAlign="Middle">
                                        <ItemTemplate>
                                            <asp:Button runat="server" Text=" View " CommandName="View" />
                                        </ItemTemplate>
                                        <HeaderStyle CssClass="testClassHeader"></HeaderStyle>
                                        <ItemStyle CssClass="testClass" HorizontalAlign="Center"></ItemStyle>
                                    </asp:TemplateField>

                                </Columns>
                                <PagerStyle BackColor="Transparent" CssClass="pagerClz" Height="10px" VerticalAlign="Bottom" HorizontalAlign="Left" />

                            </asp:GridView>
                        </div>
                    </ContentTemplate>
                    <Triggers>
                        <asp:AsyncPostBackTrigger ControlID="GridView2" EventName="RowCommand" />
                    </Triggers>
                </asp:UpdatePanel>
                
            </div>
        </div>
        <div class="d-flex justify-content-center align-items-center" style="min-height: 10vh; background-color:">
            <div class="row">
                <div class="row">
                    <div class="col-12">
                        <asp:Button ID="backbutton" runat="server" Text=" Go Back" CssClass="btn btn-primary mx-4" OnClick="backbutton_Click"/>
                    </div>
                </div>
            </div>
        </div>
        </div>
</asp:Content>