<%@ Page Title="" Language="C#" MasterPageFile="~/Site1.Master" AutoEventWireup="true" CodeBehind="ReportSaleDelivery.aspx.cs" Inherits="Billing.Report.ReportSaleDelivery" %>
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
                            <asp:Label ID="lblHeader" runat="server" Text="">ใบส่งสินค้า</asp:Label>
                        </strong>
                    </h3>
                </div>
                <div class="panel-body text-center" style="margin-top: 5px;">
                    <div style="padding: 0px 15px 15px 15px;">
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
                        <div class="row">
                            <div class="col-xs-2 headerData"><b>ชื่อผู้ส่ง :</b></div>
                            <div class="col-xs-4 rowData">
                                <asp:DropDownList ID="ddlSender" runat="server" class="form-control" >
                                </asp:DropDownList>
                            </div>
                            <div class="col-xs-2 headerData"></div>
                            <div class="col-xs-4 rowData">                                
                            </div>
                        </div>
                        <div class="row" style="margin-top: 15px;">
                            <div class="col-xs-2"></div>
                            <div class="col-xs-8">
                                <asp:Button ID="btnSearch" runat="server" Text="Search" CssClass="btn btn-default" OnClick="btnSearch_Click"/>
                                &nbsp;&nbsp;&nbsp;
                               <asp:Button ID="btnExport" runat="server" Text="Export" CssClass="btn btn-default" OnClick="btnExport_Click"/>
                            </div>
                            <div class="col-xs-2">
                            </div>
                        </div>
                        <div class="row" style="margin-top: 15px; max-height:350px; overflow:auto;">
                            <div class="col-xs-12">
                                <asp:GridView ID="gv" runat="server" Width="100%" AutoGenerateColumns="False">
                                    <Columns>       
                                        <asp:TemplateField HeaderText="Print">
                                            <ItemTemplate>
                                                <asp:CheckBox ID="chk" runat="server"/>
                                                <asp:HiddenField ID="hddID" runat="server" 
                                                    Value='<%# DataBinder.Eval(Container.DataItem, "SaleHeaderID").ToString()%>' />
                                            </ItemTemplate>
                                            <HeaderStyle CssClass="text-center width3" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="COD">
                                            <ItemTemplate>
                                                <asp:CheckBox ID="chkCOD" runat="server"/>
                                            </ItemTemplate>
                                            <HeaderStyle CssClass="text-center width3" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="P2">
                                            <ItemTemplate>
                                                <asp:CheckBox ID="chkP2" runat="server"/>
                                            </ItemTemplate>
                                            <HeaderStyle CssClass="text-center width3" />
                                        </asp:TemplateField>
                                        <asp:BoundField HeaderText="เลขที่" DataField="SaleNumber">
                                            <HeaderStyle CssClass="text-center width6" />
                                            <ItemStyle CssClass="text-center"/>
                                        </asp:BoundField> 
                                        <asp:BoundField HeaderText="ชื่อลูกค้า" DataField="CustomerName">
                                            <HeaderStyle CssClass="text-center width15" />
                                            <ItemStyle CssClass="text-left"/>
                                        </asp:BoundField>
                                        <asp:BoundField HeaderText="เบอร์โทรศัพท์" DataField="Tel">
                                            <HeaderStyle CssClass="text-center width8" />
                                            <ItemStyle CssClass="text-left"/>
                                        </asp:BoundField>
                                        <asp:BoundField HeaderText="ที่อยู่" DataField="AddressAll">
                                            <HeaderStyle CssClass="text-center width25" />
                                            <ItemStyle CssClass="text-left"/>
                                        </asp:BoundField>
                                        <asp:BoundField HeaderText="รหัสสินค้า" DataField="ItemCode">
                                            <HeaderStyle CssClass="text-center width10" />
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
                    </div>
                </div>
            </div>
        </div>
        <div class="col-xs-1"></div>  
    </div>    
</asp:Content>
