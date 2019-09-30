<%@ Page Title="" Language="C#" MasterPageFile="~/Site1.Master" AutoEventWireup="true" CodeBehind="StockLogs.aspx.cs" Inherits="Billing.Stock.StockLogs" %>
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
                            <asp:Label ID="lblHeader" runat="server" Text="">ประวัติสินค้า เข้า-ออก</asp:Label>
                        </strong>
                    </h3>
                </div>
                <div class="panel-body text-center">
                    <div style="padding: 0px 25px 5px 25px;">
                        <div class="row tab tab-border">
                            <div class="row">
                                <div class="text-left" style="height:25px; padding-left:35px;">
                                    <strong>Header</strong>
                                    <asp:HiddenField ID="hddID" runat="server" />
                                </div>
                            </div>                            
                            <div class="row width99" style="padding-left:35px;">
                                <div class="col-md-2 headerData"><b>รหัสสินค้า :</b></div>
                                <div class="col-md-3 rowData">
                                    <asp:TextBox ID="txtItemCode" runat="server" class="form-control"></asp:TextBox>
                                </div>    
                                <div class="col-md-2 headerData"><b>สินค้า :</b></div>
                                <div class="col-md-3 rowData">
                                    <asp:TextBox ID="txtItemName" runat="server" class="form-control"></asp:TextBox>
                                </div>                       
                            </div> 
                            <div class="row width99" style="padding-left:35px;">
                                <div class="col-md-2 headerData"><b>ตั้งแต่วันที่ :</b></div>
                                <div class="col-md-3 rowData">
                                    <asp:TextBox ID="txtDateFrom" runat="server" class="form-control"></asp:TextBox>
                                    <asp:CalendarExtender ID="ceDateFrom" runat="server" Format="dd/MM/yyyy" TargetControlID="txtDateFrom" PopupPosition="TopLeft"></asp:CalendarExtender> 
                                </div>    
                                <div class="col-md-2 headerData"><b>ถึงวันที่ :</b></div>
                                <div class="col-md-3 rowData">
                                    <asp:TextBox ID="txtDateTo" runat="server" class="form-control"></asp:TextBox>
                                    <asp:CalendarExtender ID="ceDateTo" runat="server" Format="dd/MM/yyyy" TargetControlID="txtDateTo" PopupPosition="TopLeft"></asp:CalendarExtender> 
                                </div>                       
                            </div> 
                        </div>                                               
                        <div class="row" style="margin-top: 15px;">
                            <div class="col-xs-2"></div>
                            <div class="col-xs-8">
                                <asp:Button ID="btnSearch" runat="server" Text="Search" CssClass="btn btn-default" OnClick="btnSearch_Click"/>   
                            </div>
                            <div class="col-xs-2">
                            </div>
                        </div>
                        <div class="row" style="margin-top: 15px; max-height:350px; overflow:auto;">
                            <div class="col-xs-12">
                                <div class="alert alert-info dark text-left">
                                    <strong>ประวัติรายการ</strong>
                                </div>
                                <asp:GridView ID="gv" runat="server" Width="100%" AutoGenerateColumns="False" >
                                    <Columns>   
                                        <asp:BoundField HeaderText="วันที่" DataField="UpdatedDateStr">
                                            <HeaderStyle CssClass="headerData text-center width15 " />
                                            <ItemStyle CssClass="text-center rowData"/>
                                        </asp:BoundField>                                      
                                        <asp:BoundField HeaderText="รหัสสินค้า" DataField="ItemCode">
                                            <HeaderStyle CssClass="headerData text-center width20 " />
                                            <ItemStyle CssClass="text-left rowData"/>
                                        </asp:BoundField>
                                        <asp:BoundField HeaderText="สินค้า" DataField="ItemName">
                                            <HeaderStyle CssClass="headerData text-center width55 " />
                                            <ItemStyle CssClass="text-left rowData"/>
                                        </asp:BoundField>
                                        <asp:BoundField HeaderText="จำนวน" DataField="Amount">
                                            <HeaderStyle CssClass="headerData text-center width10 " />
                                            <ItemStyle CssClass="text-center rowData"/>
                                        </asp:BoundField>                                                                                
                                        <%--<asp:TemplateField HeaderText="Tools">
                                            <ItemTemplate>
                                                <asp:ImageButton ID="imgbtnPrint" runat="server" Height="20px" Width="20px" ImageUrl="~/img/icon/Print.jpg"
                                                    CommandArgument='<%# DataBinder.Eval(Container.DataItem, "SaleHeaderID").ToString()%>'
                                                    CommandName='<%# DataBinder.Eval(Container.DataItem, "BillType").ToString()%>'
                                                    OnClick="imgbtnPrint_Click" />
                                                &nbsp;
                                                <asp:ImageButton ID="imgbtnEdit" runat="server" Height="20px" Width="20px" ImageUrl="~/img/icon/b_edit.png"
                                                    CommandArgument='<%# DataBinder.Eval(Container.DataItem, "SaleHeaderID").ToString()%>'
                                                    OnClick="imgbtnEdit_Click" />
                                                &nbsp;
                                                <asp:ImageButton ID="imgbtnDelete" runat="server" Height="20px" Width="20px" ImageUrl="~/img/icon/icon_delete.gif" 
                                                    CommandArgument='<%# DataBinder.Eval(Container.DataItem, "SaleHeaderID").ToString()%>'                                                    
                                                    OnClick="imgbtnDelete_Click" OnClientClick="return confirm('ยืนยันการลบข้อมูล?');"/>
                                            </ItemTemplate>
                                            <HeaderStyle CssClass="text-center width10" />
                                            <ItemStyle CssClass="text-center" />
                                        </asp:TemplateField>--%>
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
