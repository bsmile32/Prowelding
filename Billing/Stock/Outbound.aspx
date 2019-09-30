<%@ Page Title="" Language="C#" MasterPageFile="~/Site1.Master" AutoEventWireup="true" CodeBehind="Outbound.aspx.cs" Inherits="Billing.Stock.Outbound" %>
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
                            <asp:Label ID="lblHeader" runat="server" Text="">นำสินค้าออก</asp:Label>
                        </strong>
                    </h3>
                </div>
                <div class="panel-body text-center">
                    <div style="padding: 0px 25px 5px 25px;">
                        
                        <%--Header--%>  
                        <div class="row tab tab-border">
                            <div class="row">
                                <div class="text-left" style="height:25px; padding-left:35px;">
                                    <strong>Header</strong>
                                    <asp:HiddenField ID="hddID" runat="server" />
                                </div>
                            </div>
                            <div class="row width99" style="padding-left:35px;">
                                <div class="col-xs-2 headerData"><b>วันที่ทำรายการ :</b></div>
                                <div class="col-xs-4 rowData">
                                    <asp:TextBox ID="txtDate" runat="server" placeholder="กดเพื่อเปิดปฏิทิน" Width="90%" class="form-control"></asp:TextBox>
                                    <asp:CalendarExtender ID="ceDate" runat="server" Format="dd/MM/yyyy" TargetControlID="txtDate" PopupPosition="TopLeft"></asp:CalendarExtender> 
                                </div> 
                                <%--<div class="col-xs-2 headerData"><b>สถานะ :</b></div>
                                <div class="col-xs-4 rowData">
                                    <asp:Label ID="lbStatus" runat="server" Width="90%" Text="Create"></asp:Label>
                                </div> --%>                         
                            </div>
                        </div>  
                        
                        <%--Detail--%>   
                        <div class="row tab tab-border">
                            <div class="row">
                                <div class="text-left col-xs-6" style="height:25px; padding-left:35px;">
                                    <strong>Detail</strong>                                    
                                </div>
                                <div class="text-right col-xs-6" style="height:25px; padding-right:40px;">
                                    <asp:Button ID="btnAddModal" runat="server" Text="Add Detail" CssClass="btn btn-default" OnClick="btnAddModal_Click"/>
                                </div>
                            </div>
                            <div class="row width99" style="padding-left:35px; padding-top:20px;">                              
                                <div class="col-xs-12">
                                    <asp:GridView ID="gvItem" runat="server" Width="100%" AutoGenerateColumns="False">
                                        <Columns>
                                            <asp:BoundField HeaderText="รหัสสินค้า" DataField="ItemCode">
                                                <HeaderStyle CssClass="text-center width30 headerData" />
                                                <ItemStyle CssClass="text-left rowData"/>
                                            </asp:BoundField>
                                            <asp:BoundField HeaderText="สินค้า" DataField="ItemName">
                                                <HeaderStyle CssClass="text-center width50 headerData" />
                                                <ItemStyle CssClass="text-left rowData"/>
                                            </asp:BoundField>                                            
                                            <asp:BoundField HeaderText="จำนวน" DataField="AmountStr">
                                                <HeaderStyle CssClass="text-center width10 headerData" />
                                                <ItemStyle CssClass="text-center rowData"/>
                                            </asp:BoundField>      
                                            <%--<asp:BoundField HeaderText="ราคา/หน่วย" DataField="ItemPriceStr">
                                                <HeaderStyle CssClass="text-center width10" />
                                                <ItemStyle CssClass="text-right"/>
                                            </asp:BoundField>  
                                            <asp:BoundField HeaderText="ส่วนลด %" DataField="DiscountStr">
                                                <HeaderStyle CssClass="text-center width10" />
                                            </asp:BoundField>
                                            <asp:BoundField HeaderText="ราคารวม" DataField="TotalStr">
                                                <HeaderStyle CssClass="text-center width12" />
                                                <ItemStyle CssClass="text-right"/>
                                            </asp:BoundField>    --%>                                      
                                            <asp:TemplateField HeaderText="Tools">
                                                <ItemTemplate>
                                                    <asp:ImageButton ID="imgbtnEdit" runat="server" Height="20px" Width="20px" ImageUrl="~/img/icon/b_edit.png"
                                                        CommandArgument='<%# DataBinder.Eval(Container.DataItem, "TempID").ToString()%>'
                                                        OnClick="imgbtnEdit_Click" />
                                                    &nbsp;
                                                    <asp:ImageButton ID="imgbtnDelete" runat="server" Height="20px" Width="20px" ImageUrl="~/img/icon/icon_delete.gif" 
                                                        CommandArgument='<%# DataBinder.Eval(Container.DataItem, "TempID").ToString()%>'
                                                        OnClick="imgbtnDelete_Click" OnClientClick="return confirm('ยืนยันการลบข้อมูล?');"/>
                                                </ItemTemplate>
                                                <HeaderStyle CssClass="text-center width10 headerData" Height="30px"/>
                                                <ItemStyle CssClass="text-center rowData" />
                                            </asp:TemplateField>
                                        </Columns>
                                        <EmptyDataTemplate>
                                            <table border="1" style="width:100%; padding:5px; height:50px;">
                                                <%--<tr>
                                                    <td>สินค้า</td>
                                                    <td>รหัสสินค้า</td>
                                                    <td>จำนวน</td>
                                                </tr>--%>
                                                <tr>
                                                    <td colspan="3" style="text-align:left;" class="rowData">
                                                        No data.
                                                    </td>
                                                </tr>
                                            </table>
                                        </EmptyDataTemplate>
                                    </asp:GridView>
                                </div>
                            </div>
                            <%--<div class="row width99" style="padding-left:35px;">
                                
                                <div class="col-xs-8 headerData"><b>รวมเงิน :</b></div>
                                <div class="col-xs-4 rowData" style="text-align:right; padding-right:90px;">
                                    <asp:Label ID="lbTotal" runat="server" Font-Bold="true" Font-Size="X-Large"></asp:Label>
                                </div>
                            </div>--%>
                        </div>   

                        <%--Button--%>                                         
                        <div class="row" style="margin-top: 15px;">
                            <div class="col-xs-2"></div>
                            <div class="col-xs-8">
                                <asp:Button ID="btnSave" runat="server" Text="Save" CssClass="btn btn-default" OnClick="btnSave_Click" />
                                &nbsp;&nbsp;&nbsp;
                               <asp:Button ID="btnCancel" runat="server" Text="Cancel" CssClass="btn btn-default" />
                            </div>
                            <div class="col-xs-2">
                            </div>
                        </div>
                        <div class="row">

                        </div>
                    </div>
                </div>

            </div>
        </div>
        <div class="col-xs-1"></div>  
    </div>    

    <asp:ModalPopupExtender ID="ModalPopupExtender1" runat="server" BackgroundCssClass="modalBackground"
        PopupControlID="Panel1" TargetControlID="lbl_modal_view">
    </asp:ModalPopupExtender>
    <asp:Panel ID="Panel1" Height="400px" Width="900px" runat="server" Style="display: none;">
        <div class="panel panel-info-dark width100" style="min-height: 300px;">
            <div class="panel-heading text-left">
                <h3 class="panel-title">
                    <asp:Label ID="lbl_modal_view" runat="server" CssClass="modalHeader" Text="Description"></asp:Label>
                </h3>
            </div>
            <div class="row" style="margin-top: 20px;">    
                <div class="col-md-1"></div>            
                <div class="col-md-2 headerData"><b>สินค้า :</b></div>
                <div class="col-md-8 rowData">
                    <asp:HiddenField ID="hddDetailID" runat="server" />
                    <asp:HiddenField ID="hddItemID" runat="server" />
                    <asp:TextBox ID="txtMItem" runat="server" Width="92%" ReadOnly="true"></asp:TextBox>
                    <asp:ImageButton ID="imgbtnSearchItem_Click" runat="server" Height="20px" Width="20px" ImageUrl="~/img/icon/b_edit.png"                                        
                        OnClick="imgbtnSearchItem_Click_Click" />
                </div>                
                <div class="col-md-1"></div>
            </div>      
            <div class="row">    
                <div class="col-md-1"></div>            
                <div class="col-md-2 headerData"><b>รหัสสินค้า :</b></div>
                <div class="col-md-3 rowData">
                    <asp:TextBox ID="txtMItemCode" runat="server" class="form-control"></asp:TextBox>
                </div>
                <div class="col-md-2 headerData"><b>จำนวน (&nbsp;<asp:Label ID="lbAmount" runat="server"></asp:Label>&nbsp;) : </b></div>
                <div class="col-md-3 rowData">
                    <asp:TextBox ID="txtMAmount" runat="server" onKeyPress="keyintNodot()" class="form-control"></asp:TextBox>                    
                </div>
                <%--<div class="col-md-2 headerData"><b>ราคา/หน่วย :</b></div>
                <div class="col-md-3 rowData">
                    <asp:TextBox ID="txtMPrice" runat="server" onKeyPress="keyintdot()"></asp:TextBox>
                </div>--%>                
                <div class="col-md-1"></div>
            </div> 
            <%--<div class="row" style="height:155px;">    
                <div class="col-md-1"></div>            
                <div class="col-md-2 headerData" style="height:155px;"><b>รายละเอียด :</b></div>
                <div class="col-md-8 rowData" style="height:155px;">
                    <asp:TextBox ID="txtMDescription" runat="server" TextMode="MultiLine" Rows="7" Width="99%"></asp:TextBox>
                </div>                
                <div class="col-md-1"></div>
            </div>--%>
            <div class="row">&nbsp;</div> 
            <div class="row" style="margin-top: 15px;">
                <div class="col-md-12 text-center">
                    <asp:Button ID="btnMSave" runat="server" CssClass="btn btn-save" Text="Save" OnClick="btnMSave_Click"/>
                    <asp:Button ID="btnModalClose" runat="server" CssClass="btn btn-save" Text="Close"/>
                </div>
            </div>
        </div>
    </asp:Panel>

    <asp:ModalPopupExtender ID="ModalPopupExtender3" runat="server" BackgroundCssClass="modalBackground"
        PopupControlID="Panel3" TargetControlID="lbl_modal_view">
    </asp:ModalPopupExtender>
    <asp:Panel ID="Panel3" Height="500px" Width="1100px" runat="server" Style="display: none;">
        <%--Style="display: none;"--%>
        <div class="panel panel-info-dark width100" style="min-height: 500px;">
            <div class="panel-heading text-left">
                <h3 class="panel-title">
                    <asp:Label ID="Label2" runat="server" CssClass="modalHeader" Text="List Item"></asp:Label>
                </h3>
            </div>
            <div class="row" style="margin-top: 20px;">    
                <div class="col-md-1"></div>            
                <div class="col-md-2 headerData"><b>รหัสสินค้า :</b></div>
                <div class="col-md-3 rowData">
                    <asp:TextBox ID="txtSearchItemCode" runat="server" CssClass="form-control"></asp:TextBox>
                </div>
                <div class="col-md-2 headerData"><b>ชื่อสินค้า :</b></div>
                <div class="col-md-3 rowData">
                    <asp:TextBox ID="txtSearchItemName" runat="server" CssClass="form-control"></asp:TextBox>                    
                </div>                
                <div class="col-md-1"></div>
            </div>                  
            <div class="row" style="height:300px !important;">    
                <div class="col-md-1"></div>            
                <div class="col-md-10 rowData" style="overflow:auto; height:300px !important;">
                    <asp:GridView ID="gvItemSearch" runat="server" Width="100%" AutoGenerateColumns="False">
                        <Columns>
                            <asp:BoundField HeaderText="รหัสสินค้า" DataField="ItemCode">
                                <HeaderStyle CssClass="text-center width25 headerData" />
                                <ItemStyle CssClass="text-left rowData"/>
                            </asp:BoundField>
                            <asp:BoundField HeaderText="ชื่อสินค้า" DataField="itemName">
                                <HeaderStyle CssClass="text-center width55 headerData" />
                                <ItemStyle CssClass="text-left rowData"/>
                            </asp:BoundField>
                            <asp:BoundField HeaderText="คงเหลือ" DataField="Amount">
                                <HeaderStyle CssClass="text-center width10 headerData" />
                                <ItemStyle CssClass="text-center rowData"/>
                            </asp:BoundField>      
                            <%--<asp:BoundField HeaderText="รายละเอียด" DataField="ItemDesc" Visible="false">
                                <HeaderStyle CssClass="text-center width15" />
                                <ItemStyle CssClass="text-right"/>
                            </asp:BoundField>   --%>                           
                            <asp:TemplateField HeaderText="Tools">
                                <ItemTemplate>
                                    <asp:HiddenField ID="hddItemID" runat="server" Value='<%# DataBinder.Eval(Container.DataItem, "ItemID").ToString()%>' />
                                    <asp:Label ID="lbItemDesc" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container.DataItem, "ItemDesc").ToString()%>'></asp:Label>
                                    <asp:ImageButton ID="imgbtnChooseItem" runat="server" Height="20px" Width="20px" ImageUrl="~/img/icon/b_edit.png"                                        
                                        CommandArgument='<%# Container.DataItemIndex.ToString()%>'
                                        OnClick="imgbtnChooseItem_Click" />
                                </ItemTemplate>
                                <HeaderStyle CssClass="text-center width10 headerData" Height="30px"/>
                                <ItemStyle CssClass="text-center rowData" />
                            </asp:TemplateField>                            
                        </Columns>
                        <HeaderStyle BackColor="#ff7777" />
                        <EmptyDataTemplate>
                            <table border="1" style="width:100%; padding:5px;">
                                <tr>
                                    <td>รหัสสินค้า</td>
                                    <td>ชื่อสินค้า</td>
                                    <td>คงเหลือ</td>                                    
                                    <%-- <td>รายละเอียด</td>--%>
                                </tr>
                                <tr>
                                    <td colspan="2" style="text-align:left;">
                                        No data.
                                    </td>
                                </tr>
                            </table>
                        </EmptyDataTemplate>
                    </asp:GridView>
                </div>                
                <div class="col-md-1"></div>
            </div>
            <div class="row">&nbsp;</div> 
            <div class="row" style="margin-top: 15px;">
                <div class="col-md-12 text-center">
                    <asp:Button ID="btnMItemSearch" runat="server" CssClass="btn btn-save" Text="Search" OnClick="btnMItemSearch_Click"/>
                    <asp:Button ID="btnModalClose3" runat="server" CssClass="btn btn-save" Text="Close" OnClick="btnModalClose3_Click"/>
                </div>
            </div>
        </div>
    </asp:Panel>
</asp:Content>
