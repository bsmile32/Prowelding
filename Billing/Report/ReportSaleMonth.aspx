<%@ Page Title="" Language="C#" MasterPageFile="~/Site1.Master" AutoEventWireup="true" CodeBehind="ReportSaleMonth.aspx.cs" Inherits="Billing.Report.ReportSaleMonth" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script type="text/javascript">
        function keyintdot() {
            var key = event.keyCode ? event.keyCode : event.which ? event.which : event.charCode;            
            if ((key < 48 || key > 57) && key != 46) { //46 = "."
                event.returnValue = false;
            }
        }
        function keyintNodot() {
            var key = event.keyCode ? event.keyCode : event.which ? event.which : event.charCode;
            if ((key < 48 || key > 57)) {
                event.returnValue = false;
            }
        }
        function checkHeader() {
            var checkHeader = document.getElementById("chkHeader");
            if (checkHeader.checked)
                CheckAll();
            else
                UnCheckAll();
        }
        function CheckAll() {
            var gv = document.getElementById("<%= gv.ClientID %>");
            var inputList = gv.getElementsByTagName("input");

            for (var i = 0; i < inputList.length; i++) {
                if (inputList[i].type == "checkbox") {
                    inputList[i].checked = true;
                }
            }
        }

        function UnCheckAll() {
            var gv = document.getElementById("<%= gv.ClientID %>");
            var inputList = gv.getElementsByTagName("input");

            for (var i = 0; i < inputList.length; i++) {
                if (inputList[i].type == "checkbox") {
                    inputList[i].checked = false;
                }
            }
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="row">
        <div class="col-xs-1"></div>
        <div class="col-xs-10">
            <div class="panel panel-info panel-info-dark" style="min-height: 370px;">
                <div class="panel-heading panel-heading-dark text-left">
                    <h3 class="panel-title">
                        <strong>
                            <asp:Label ID="lblHeader" runat="server" Text="">Report Sales Monthly</asp:Label>
                        </strong>
                    </h3>
                </div>
                <div class="panel-body text-center" style="margin-top: 5px;">
                    <div style="padding: 0px 15px 15px 15px;">
                        <div class="row">
                            <div class="col-xs-2 headerData"><b>เดือน :</b></div>
                            <div class="col-xs-4 rowData">
                                <asp:DropDownList ID="ddlMonth" runat="server" AppendDataBoundItems="true" class="form-control dis-inline" Width="120px"></asp:DropDownList>
                                &nbsp;&nbsp;&nbsp;
                                <asp:DropDownList ID="ddlYear" runat="server" AppendDataBoundItems="true" class="form-control dis-inline" Width="80px"></asp:DropDownList>
                            </div>
                            <div class="col-xs-2 headerData"><b>xx (%) :</b></div>
                            <div class="col-xs-4 rowData">
                                <asp:TextBox ID="txtXX" runat="server" onKeyPress="keyintNodot()" TextMode="Number" class="form-control" Width="100px"></asp:TextBox>
                            </div>
                        </div>
                        <div class="row">
                            <div class="col-xs-2 headerData"><b></b></div>
                            <div class="col-xs-4 rowData">                                
                            </div>
                            <div class="col-xs-2 headerData"><b>MAGMIX200 :</b></div>
                            <div class="col-xs-4 rowData">
                                <asp:TextBox ID="txtMM200" runat="server" onKeyPress="keyintNodot()" TextMode="Number" class="form-control" Width="100px"></asp:TextBox>
                            </div>
                        </div>
                        <div class="row">
                            <div class="col-xs-2 headerData"><b></b></div>
                            <div class="col-xs-4 rowData">                                
                            </div>
                            <div class="col-xs-2 headerData"><b>MAGMIX225 :</b></div>
                            <div class="col-xs-4 rowData">
                                <asp:TextBox ID="txtMM225" runat="server" onKeyPress="keyintNodot()" TextMode="Number" class="form-control" Width="100px"></asp:TextBox>
                            </div>
                        </div>
                        <div class="row" style="margin-top: 15px;">
                            <div class="col-xs-2"></div>
                            <div class="col-xs-8">
                                <asp:Button ID="btnSearch" runat="server" Text="Search" CssClass="btn btn-default" OnClick="btnSearch_Click"/>
                                &nbsp;&nbsp;&nbsp;
                                <asp:Button ID="btnPrint" runat="server" Text="Print Bill" CssClass="btn btn-default" OnClick="btnPrint_Click"/>
                                &nbsp;&nbsp;&nbsp;
                                <asp:Button ID="btnExport" runat="server" Text="Export PDF" CssClass="btn btn-default" OnClick="btnExport_Click"/>
                                &nbsp;&nbsp;&nbsp;
                                <asp:Button ID="btnExportExcel" runat="server" Text="Export Excel" CssClass="btn btn-default" OnClick="btnExportExcel_Click"/>
                            </div>
                            <div class="col-xs-2">
                            </div>
                        </div>
                        <div class="row" style="margin-top: 15px; max-height:350px; overflow:auto;">
                            <div class="col-xs-12">
                                <%--<div class="alert alert-info dark text-left">
                                    <strong>List Sales</strong>
                                </div>--%>
                                <asp:GridView ID="gv" runat="server" Width="2200px" AutoGenerateColumns="False">
                                    <Columns>
                                        <asp:TemplateField>
                                            <HeaderTemplate>
                                                <asp:CheckBox ID="chkHeader" runat="server" onclick="checkHeader()" />
                                            </HeaderTemplate>
                                            <ItemTemplate>                                                
                                                <asp:CheckBox ID="chk" runat="server" 
                                                    CommandArgument='<%# DataBinder.Eval(Container.DataItem, "HeaderID").ToString()%>' 
                                                    Visible='<%# Convert.ToBoolean(DataBinder.Eval(Container.DataItem, "VisImgBtn").ToString())%>'/>                                       
                                            </ItemTemplate>
                                            <HeaderStyle CssClass="text-center width3" />
                                            <ItemStyle CssClass="text-center" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Print" >
                                            <ItemTemplate>
                                                <asp:ImageButton ID="imgbtnPrint" runat="server" Height="20px" Width="20px" ImageUrl="~/img/icon/Print.jpg"
                                                    CommandArgument='<%# DataBinder.Eval(Container.DataItem, "HeaderID").ToString()%>'
                                                    CommandName='<%# DataBinder.Eval(Container.DataItem, "BillType").ToString()%>'
                                                    Visible='<%# Convert.ToBoolean(DataBinder.Eval(Container.DataItem, "VisImgBtn").ToString())%>'
                                                    OnClick="imgbtnPrint_Click" />                                                
                                            </ItemTemplate>
                                            <HeaderStyle CssClass="text-center width3" />
                                            <ItemStyle CssClass="text-center" />
                                        </asp:TemplateField>
                                        <asp:BoundField HeaderText="ชื่อลูกค้า" DataField="CustomerName">
                                            <HeaderStyle CssClass="text-center width8" />
                                            <ItemStyle CssClass="text-left"/>
                                        </asp:BoundField>
                                        <asp:BoundField HeaderText="โทร" DataField="Tel">
                                            <HeaderStyle CssClass="text-center width4" />
                                            <ItemStyle CssClass="text-left"/>
                                        </asp:BoundField>
                                        <asp:BoundField HeaderText="ที่อยู่" DataField="Address">
                                            <HeaderStyle CssClass="text-center width8" />
                                            <ItemStyle CssClass="text-left"/>
                                        </asp:BoundField>
                                        <asp:BoundField HeaderText="ตำบล" DataField="District">
                                            <HeaderStyle CssClass="text-center width5" />
                                            <ItemStyle CssClass="text-left"/>
                                        </asp:BoundField>
                                        <asp:BoundField HeaderText="อำเภอ" DataField="Country">
                                            <HeaderStyle CssClass="text-center width5" />
                                            <ItemStyle CssClass="text-left"/>
                                        </asp:BoundField>
                                        <asp:BoundField HeaderText="จังหวัด" DataField="Province">
                                            <HeaderStyle CssClass="text-center width5" />
                                            <ItemStyle CssClass="text-left"/>
                                        </asp:BoundField>
                                        <asp:BoundField HeaderText="รหัสไปรษณีย์" DataField="PostalCode">
                                            <HeaderStyle CssClass="text-center width3" />
                                        </asp:BoundField>
                                        <asp:BoundField HeaderText="วันที่" DataField="ReceivedDateStr">
                                            <HeaderStyle CssClass="text-center width5" />
                                        </asp:BoundField>
                                        <asp:BoundField HeaderText="วันที่รับประกัน" DataField="WarrantyDateStr">
                                            <HeaderStyle CssClass="text-center width5" />
                                        </asp:BoundField>
                                        <asp:BoundField HeaderText="เลขที่" DataField="SaleNumber">
                                            <HeaderStyle CssClass="text-center width4" />
                                        </asp:BoundField>   
                                        <asp:BoundField HeaderText="สินค้า" DataField="ItemName">
                                            <HeaderStyle CssClass="text-center width10" />
                                            <ItemStyle CssClass="text-left"/>
                                        </asp:BoundField>  
                                        <asp:BoundField HeaderText="รหัส" DataField="ItemCode">
                                            <HeaderStyle CssClass="text-center width5" />
                                            <ItemStyle CssClass="text-left"/>
                                        </asp:BoundField>  
                                        <asp:BoundField HeaderText="จำนวน" DataField="AmountStr">
                                            <HeaderStyle CssClass="text-center width2" />
                                            <ItemStyle CssClass="text-center"/>
                                        </asp:BoundField>  
                                        <asp:BoundField HeaderText="รวม" DataField="TotalStr">
                                            <HeaderStyle CssClass="text-center width5" />
                                            <ItemStyle CssClass="text-right"/>
                                        </asp:BoundField>    
                                        <asp:BoundField HeaderText="ราคา" DataField="TotalPriceStr">
                                            <HeaderStyle CssClass="text-center width3" />
                                            <ItemStyle CssClass="text-right"/>
                                        </asp:BoundField>  
                                        <asp:BoundField HeaderText="ราคาก่อน VAT" DataField="TotalExVatStr">
                                            <HeaderStyle CssClass="text-center width3" />
                                            <ItemStyle CssClass="text-right"/>
                                        </asp:BoundField>  
                                        <asp:BoundField HeaderText="ภาษีมูลค่าเพิ่ม" DataField="VATAmountStr">
                                            <HeaderStyle CssClass="text-center width3" />
                                            <ItemStyle CssClass="text-right"/>
                                        </asp:BoundField> 
                                        
                                        <asp:BoundField HeaderText="ประเภทบิล" DataField="BillType">
                                            <HeaderStyle CssClass="text-center width3" />
                                            <ItemStyle CssClass="text-center"/>
                                        </asp:BoundField>  
                                        <asp:BoundField HeaderText="ช่องทางการชำระเงิน" DataField="PayType">
                                            <HeaderStyle CssClass="text-center width3" />
                                            <ItemStyle CssClass="text-center"/>
                                        </asp:BoundField>
                                        <asp:BoundField HeaderText="บัญชีโอน" DataField="AccountTransfer">
                                            <HeaderStyle CssClass="text-center width3" />
                                            <ItemStyle CssClass="text-center"/>
                                        </asp:BoundField>
                                        <asp:BoundField HeaderText="ผ่อน" DataField="Installment">
                                            <HeaderStyle CssClass="text-center width3" />
                                            <ItemStyle CssClass="text-center"/>
                                        </asp:BoundField>
                                        <asp:BoundField HeaderText="Consignment No." DataField="ConsignmentNo">
                                            <HeaderStyle CssClass="text-center width3" />
                                            <ItemStyle CssClass="text-left"/>
                                        </asp:BoundField>                                     
                                    </Columns>
                                    <EmptyDataTemplate>
                                        <table>
                                            <tr>
                                                <td></td>
                                            </tr>
                                            <tr>
                                                <td colspan="5">
                                                    No data.
                                                </td>
                                            </tr>
                                        </table>
                                    </EmptyDataTemplate>
                                </asp:GridView>
                            </div>                            
                        </div>
                        <div class="row" style="margin-top: 15px;">
                            <div class="col-xs-8 headerData"><b>Summary :</b></div>
                            <div class="col-xs-4 rowData">
                                <asp:Label ID="lbSummary" runat="server" Font-Bold="true" Font-Size="Larger" Text="0"></asp:Label>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
        <div class="col-xs-1"></div>  
    </div>   
</asp:Content>
