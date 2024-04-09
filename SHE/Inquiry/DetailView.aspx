<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="DetailView.aspx.cs" Inherits="SHE.Inquiry.DetailView" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">

    <style>
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

        label {
            font-style: oblique;
        }

        .btn {
            background-color: #4dc6d0;
        }

        #exitbutton {
            width: 70px;
        }

        .panel-with-shadow {
            box-shadow: 0 0 10px rgba(0, 0, 0, 0.2);
            /* You can adjust the values to control the shadow appearance */
        }
    </style>

    <%-- Panel with details --%>

    <asp:Panel ID="panel2" runat="server" Visible="true">
        <div class="container">
            <div class="row justify-content-center">
                <div class="col-md-12 panel-with-shadow" style="max-width: 700px; background-color: #CFEFF2; padding: 10px; border: 1px solid #c0c0c0; border-radius: 10px; padding: 20px;">
                    <h4 style="text-align: center;">Policy Details View</h4>
                    <br />

                    <%--DefaultButton="claimhist_submit"--%>

                    <div class="container" style="width: 80%;">
                        <div class="mx-auto">
                            <div class="row mt-4">
                                <div class="col-sm-4 ">Reference No</div>
                                <div class="col-sm-8 " runat="server" id="referenceNo"></div>
                            </div>
                            <div class="row mt-4">
                                <div class="col-sm-4 ">Patient Name  </div>
                                <div class="col-sm-8 " runat="server" id="name"></div>
                            </div>
                            <div class="row mt-4">
                                <div class="col-sm-4 ">Hosptal Name</div>
                                <div class="col-sm-8 " runat="server" id="hospitalName"></div>
                            </div>
                            <div class="row mt-4">
                                <div class="col-sm-4 ">Room No</div>
                                <div class="col-sm-8 " runat="server" id="roomNo"></div>
                            </div>
                            <div class="row mt-4">
                                <div class="col-sm-4">Admitted Date</div>
                                <div class="col-sm-8 " runat="server" id="admittedDate"></div>
                            </div>
                            <div class="row mt-4">
                                <div class="col-sm-4 ">Contact No</div>
                                <div class="col-sm-8 " runat="server" id="contactNo"></div>
                            </div>
                            <div class="row mt-4">
                                <div class="col-4">Discharge Date </div>
                                <div class="col-8 " runat="server" id="dischargeDate"></div>
                            </div>
                            <div class="row mt-4">
                                <div class="col-sm-4 ">Employee No</div>
                                <div class="col-sm-8 " runat="server" id="employeeNo"></div>
                            </div>
                            <div class="row mt-4">
                                <div class="col-sm-4 ">Policy No</div>
                                <div class="col-sm-8 " runat="server" id="policyNo"></div>
                            </div>

                            <br />
                            <div class="card-footer d-flex justify-content-evenly">

                                <asp:Button ID="back" runat="server" Text="Back" CssClass="btn btn-primary mx-4" OnClick="back_Click" />
                                <asp:Button ID="menu" runat="server" Text="Menu" CssClass="btn btn-primary mx-4" OnClick="menu_Click" />
                                <asp:Button ID="history" runat="server" Text="History" CssClass="btn btn-primary mx-4" OnClick="history_Click" ClientIDMode="Static" />

                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </asp:Panel>

    <%-- end of panel --%>

    <%--        <asp:Panel ID="panel2" Visible="true" runat="server" Class=" d-flex flex-column align-items-center">
        <div class="container">
            <div class="row justify-content-center">
                <div class="col-md-12 panel-with-shadow" style="width: 900px; background-color: white; padding: 10px; border: 1px solid #c0c0c0; border-radius: 10px; padding: 20px;">
                    <h4 style="text-align: center;">Policy Details View</h4>
                    <br />


                    <div class="container">
                        <div class="row justify-content-center mt-4">
                            <div class="col-md-8">
                                <div class="form-group row">
                                    <label for="lblreferenceNo" class="col-sm-3 col-form-label text-center" >Reference No </label>
                                    <div class="col-sm-9">
                                        <input type="text" class="form-control" runat="server" readonly="readonly" id="referenceNo" placeholder="referenceNo" style="width: 70%;">
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                    <div class="container">
                        <div class="row justify-content-center mt-4">
                            <div class="col-md-8">
                                <div class="form-group row">
                                    <label for="lblName" class="col-sm-3 col-form-label text-center">Patient Name  </label>
                                    <div class="col-sm-9">
                                        <input type="Text" class="form-control" runat="server" readonly="readonly" id="name" placeholder="Patient Name" style="width: 70%;">
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                    <div class="container">
                        <div class="row justify-content-center mt-4">
                            <div class="col-md-8">
                                <div class="form-group row">
                                    <label for="lblHospital" class="col-sm-3 col-form-label text-center">Hosptal Name</label>
                                    <div class="col-sm-9">
                                        <input type="Text" class="form-control" runat="server" readonly="readonly" id="hospitalName" placeholder="Hospital Name" style="width: 70%;">
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                    <div class="container">
                        <div class="row justify-content-center mt-4">
                            <div class="col-md-8">
                                <div class="form-group row">
                                    <label for="lblRoomNo" class="col-sm-3 col-form-label text-center">Room No</label>
                                    <div class="col-sm-9">
                                        <input type="Text" class="form-control" runat="server" readonly="readonly" id="roomNo" placeholder="Room No" style="width: 70%;">
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                    <div class="container">
                        <div class="row justify-content-center mt-4">
                            <div class="col-md-8">
                                <div class="form-group row">
                                    <label for="lblAdmittedDate" class="col-sm-3 col-form-label text-center">Admitted Date</label>
                                    <div class="col-sm-9">
                                        <input type="Text" class="form-control" runat="server" readonly="readonly" id="admittedDate" placeholder="Admitted Date" style="width: 70%;">
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>

                    <div class="container">
                        <div class="row justify-content-center mt-4">
                            <div class="col-md-8">
                                <div class="form-group row">
                                    <label for="lblContactNo" class="col-sm-3 col-form-label text-center">Contact No</label>
                                    <div class="col-sm-9">
                                        <input type="Text" class="form-control" runat="server" readonly="readonly" id="contactNo" placeholder="Contact No" style="width: 70%;">
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>

                    <div class="container">
                        <div class="row justify-content-center mt-4">
                            <div class="col-md-8">
                                <div class="form-group row">
                                    <label for="lblDischargeDate" class="col-sm-3 col-form-label text-center">Discharge Date </label>
                                    <div class="col-sm-9">
                                        <input type="Text" class="form-control" runat="server" readonly="readonly" id="dischargeDate" placeholder="Discharge Date " style="width: 70%;">
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>

                    <div class="container">
                        <div class="row justify-content-center mt-4">
                            <div class="col-md-8">
                                <div class="form-group row">
                                    <label for="lblEmployeeNo" class="col-sm-3 col-form-label text-center">Employee No </label>
                                    <div class="col-sm-9">
                                        <input type="Text" class="form-control" runat="server" readonly="readonly" id="employeeNo" placeholder="Employee No " style="width: 70%;">
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>

                    <div class="container">
                        <div class="row justify-content-center mt-4">
                            <div class="col-md-8">
                                <div class="form-group row">
                                    <label for="lblPolicyNo" class="col-sm-3 col-form-label text-center">Policy No  </label>
                                    <div class="col-sm-9">
                                        <input type="Text" class="form-control" runat="server" readonly="readonly" id="policyNo" placeholder="Policy No " style="width: 70%;">
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                    <br />
                    <div class="d-flex justify-content-center">
                        <asp:Button ID="back" runat="server" Text="Back" CssClass="btn btn-primary mx-4" OnClick="back_Click" />
                        <asp:Button ID="menu" runat="server" Text="Menu" CssClass="btn btn-primary mx-4" OnClick="menu_Click" />
                        <asp:Button ID="history" runat="server" Text="History" CssClass="btn btn-primary mx-4" OnClick="history_Click" ClientIDMode="Static" />
                    </div>

                </div>
            </div>
        </div>

    </asp:Panel>--%>
</asp:Content>
