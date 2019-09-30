<%@ Page Title="" Language="C#" MasterPageFile="~/Site1.Master" AutoEventWireup="true" CodeBehind="ReportSale.aspx.cs" Inherits="Billing.Report.ReportSale" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="row">
        <div class="col-xs-1"></div>
        <div class="col-xs-10">
            <div class="panel panel-info panel-info-dark" style="min-height: 370px;">
                <div class="panel-heading panel-heading-dark text-left">
                    <h3 class="panel-title">
                        <strong>
                            <asp:Label ID="lblHeader" runat="server" Text="">Report Sales</asp:Label>
                        </strong>
                    </h3>
                </div>
                <div class="panel-body text-center" style="margin-top: 5px;">
                    <div style="padding: 0px 15px 15px 15px;">
                        <div class="row">
                            <div class="col-xs-2 headerData"><b>ชื่อลูกค้า :</b></div>
                            <div class="col-xs-4 rowData">
                                <asp:TextBox ID="txtCustName" runat="server" class="form-control"></asp:TextBox>
                            </div>
                            <div class="col-xs-2 headerData"><b>เบอร์โทรศัพท์ :</b></div>
                            <div class="col-xs-4 rowData">
                                <asp:TextBox ID="txtTel" runat="server" class="form-control"></asp:TextBox>
                            </div>
                        </div>
                        <div class="row">
                            <div class="col-xs-2 headerData"><b>ตั้งแต่วันที่ :</b></div>
                            <div class="col-xs-4 rowData">
                                <asp:TextBox ID="txtDateFrom" runat="server" placeholder="กดเพื่อเปิดปฏิทิน" class="form-control"></asp:TextBox>
                                <asp:CalendarExtender ID="ceDate1" runat="server" Format="dd/MM/yyyy" TargetControlID="txtDateFrom" PopupPosition="TopLeft"></asp:CalendarExtender> 
                            </div>
                            <div class="col-xs-2 headerData"><b>ถึงวันที่ :</b></div>
                            <div class="col-xs-4 rowData">
                                <asp:TextBox ID="txtDateTo" runat="server" placeholder="กดเพื่อเปิดปฏิทิน" class="form-control"></asp:TextBox>
                                <asp:CalendarExtender ID="ceDate2" runat="server" Format="dd/MM/yyyy" TargetControlID="txtDateTo" PopupPosition="TopLeft"></asp:CalendarExtender> 
                            </div>
                        </div>
                        <div class="row" style="margin-top: 15px;">
                            <div class="col-xs-2"></div>
                            <div class="col-xs-8">
                                <asp:Button ID="btnSearch" runat="server" Text="Search" CssClass="btn btn-default" OnClick="btnSearch_Click"/>
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
                                <asp:GridView ID="gv" runat="server" Width="2000px" AutoGenerateColumns="False">
                                    <Columns>
                                        <asp:BoundField HeaderText="ชื่อลูกค้า" DataField="CustomerName">
                                            <HeaderStyle CssClass="text-center width10" />
                                            <ItemStyle CssClass="text-left"/>
                                        </asp:BoundField>
                                        <asp:BoundField HeaderText="โทร" DataField="Tel">
                                            <HeaderStyle CssClass="text-center width5" />
                                            <ItemStyle CssClass="text-left"/>
                                        </asp:BoundField>
                                        <%--<asp:BoundField HeaderText="ที่อยู่" DataField="Address">
                                            <HeaderStyle CssClass="text-center width9" />
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
                                        </asp:BoundField>--%>
                                        <asp:BoundField HeaderText="วันที่" DataField="ReceivedDateStr">
                                            <HeaderStyle CssClass="text-center width5" />
                                        </asp:BoundField>
                                        <asp:BoundField HeaderText="วันที่รับประกัน" DataField="WarrantyDateStr">
                                            <HeaderStyle CssClass="text-center width5" />
                                        </asp:BoundField>
                                        <asp:BoundField HeaderText="เลขที่" DataField="SaleNumber">
                                            <HeaderStyle CssClass="text-center width5" />
                                        </asp:BoundField>   
                                        <asp:BoundField HeaderText="สินค้า" DataField="ItemName">
                                            <HeaderStyle CssClass="text-center width15" />
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
                                        <asp:BoundField HeaderText="ราคา" DataField="ItemPriceStr">
                                            <HeaderStyle CssClass="text-center width4" />
                                            <ItemStyle CssClass="text-right"/>
                                        </asp:BoundField>  
                                        <asp:BoundField HeaderText="ส่วนลด" DataField="DiscountStr">
                                            <HeaderStyle CssClass="text-center width4" />
                                            <ItemStyle CssClass="text-right"/>
                                        </asp:BoundField> 
                                        <asp:BoundField HeaderText="รวม" DataField="TotalStr">
                                            <HeaderStyle CssClass="text-center width4" />
                                            <ItemStyle CssClass="text-right"/>
                                        </asp:BoundField>    
                                        <asp:BoundField HeaderText="ประเภทบิล" DataField="BillType">
                                            <HeaderStyle CssClass="text-center width4" />
                                            <ItemStyle CssClass="text-center"/>
                                        </asp:BoundField>  
                                        <asp:BoundField HeaderText="ช่องทางการชำระเงิน" DataField="PayType">
                                            <HeaderStyle CssClass="text-center width4" />
                                            <ItemStyle CssClass="text-center"/>
                                        </asp:BoundField> 
                                        <asp:BoundField HeaderText="บัญชีโอน" DataField="AccountTransfer">
                                            <HeaderStyle CssClass="text-center width4" />
                                            <ItemStyle CssClass="text-center"/>
                                        </asp:BoundField> 
                                        <asp:BoundField HeaderText="ผ่อน" DataField="Installment">
                                            <HeaderStyle CssClass="text-center width4" />
                                            <ItemStyle CssClass="text-center"/>
                                        </asp:BoundField>
                                        <asp:BoundField HeaderText="Consignment No." DataField="ConsignmentNo">
                                            <HeaderStyle CssClass="text-center width4" />
                                            <ItemStyle CssClass="text-left"/>
                                        </asp:BoundField> 
                                        <asp:BoundField HeaderText="ผู้ขาย" DataField="SaleName">
                                            <HeaderStyle CssClass="text-center width4" />
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
