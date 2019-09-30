<%@ Page Title="" Language="C#" MasterPageFile="~/Site1.Master" AutoEventWireup="true" CodeBehind="Inbound.aspx.cs" Inherits="Billing.Stock.Inbound" %>
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

        //$(document).ready(function () {
        //    $("ddl1").searchable({
        //        maxListSize: 200, // if list size are less than maxListSize, show them all
        //        maxMultiMatch: 300, // how many matching entries should be displayed
        //        exactMatch: false, // Exact matching on search
        //        wildcards: true, // Support for wildcard characters (*, ?)
        //        ignoreCase: true, // Ignore case sensitivity
        //        latency: 200, // how many millis to wait until starting search
        //        warnMultiMatch: 'top {0} matches ...',
        //        warnNoMatch: 'no matches ...',
        //        zIndex: 'auto'
        //    });
        //});
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
                            <asp:Label ID="lblHeader" runat="server" Text="">รับเข้า</asp:Label>
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
                                    <%--<asp:HiddenField ID="hddID" runat="server" />--%>
                                </div>
                            </div>
                            <div class="row width99" style="padding-left:35px;">
                                <div class="col-xs-2 headerData"><b>ค้นหาสินค้า :</b></div>
                                <div class="col-xs-4 rowData">
                                    <asp:HiddenField ID="hddItemID" runat="server" />
                                    <asp:HiddenField ID="hddItemCode" runat="server" />
                                    <asp:TextBox ID="txtItem" runat="server" Width="90%" ReadOnly="true" class="form-control dis-inline"></asp:TextBox>
                                    &nbsp;
                                    <asp:ImageButton ID="imgbtnSearchItem_Click" runat="server" Height="20px" Width="20px" ImageUrl="~/img/icon/b_edit.png"                                        
                                        OnClick="imgbtnSearchItem_Click_Click" />
                                </div> 
                                <div class="col-xs-2 headerData"><b>จำนวนรับเข้า :</b></div>
                                <div class="col-xs-4 rowData">
                                    <asp:TextBox ID="txtAmount" runat="server" onKeyPress="keyintNodot()" class="form-control"></asp:TextBox>
                                </div>                          
                            </div>
                            <div class="row width99" style="padding-left:35px;">
                                <div class="col-xs-2 headerData" style="height:100px;"><b>Serial :</b></div>
                                <div class="col-xs-4 rowData" style="height:100px;">
                                    <asp:TextBox ID="txtSerial" runat="server" class="form-control"></asp:TextBox>
                                </div> 
                                <div class="col-xs-2 headerData" style="height:100px;"><b>หมายเหตุ :</b></div>
                                <div class="col-xs-4 rowData" style="height:100px;">
                                    <asp:TextBox ID="txtRemark" runat="server" Rows="3" TextMode="MultiLine" Width="100%" class="form-control"></asp:TextBox>
                                </div>                          
                            </div>
                            <br />
                            <asp:Button ID="btnAdd" runat="server" Text="รับเข้า" CssClass="btn btn-default" OnClick="btnAdd_Click"/>
                        </div>  
                        
                        <%--Detail--%>   
                        <div class="row tab tab-border">
                            <div class="row">
                                <div class="text-left col-xs-6" style="height:25px; padding-left:35px;">
                                    <strong>Detail</strong>                                    
                                </div>
                                <%--<div class="text-right col-xs-6" style="height:25px; padding-right:40px;">
                                    
                                </div>--%>
                            </div>
                            <div class="row width99" style="padding-left:35px; padding-top:20px;">                              
                                <div class="col-xs-12">
                                    <asp:GridView ID="gvItem" runat="server" Width="100%" AutoGenerateColumns="False">
                                        <Columns>
                                            <asp:BoundField HeaderText="รหัสสินค้า" DataField="ItemCode">
                                                <HeaderStyle CssClass="text-center width15 headerData" />
                                                <ItemStyle CssClass="text-left rowData"/>
                                            </asp:BoundField>
                                            <asp:BoundField HeaderText="สินค้า" DataField="ItemName">
                                                <HeaderStyle CssClass="text-center width45 headerData" />
                                                <ItemStyle CssClass="text-left rowData"/>
                                            </asp:BoundField> 
                                            <asp:BoundField HeaderText="หน่วย" DataField="UnitName">
                                                <HeaderStyle CssClass="text-center width10 headerData" />
                                                <ItemStyle CssClass="text-center rowData"/>
                                            </asp:BoundField>  
                                            <asp:BoundField HeaderText="คงเหลือ" DataField="RemainingStr">
                                                <HeaderStyle CssClass="text-center width10 headerData" />
                                                <ItemStyle CssClass="text-center rowData"/>
                                            </asp:BoundField>
                                            <asp:BoundField HeaderText="รับเข้า" DataField="AmountStr">
                                                <HeaderStyle CssClass="text-center width10 headerData" />
                                                <ItemStyle CssClass="text-center rowData"/>
                                            </asp:BoundField>              
                                            <asp:TemplateField HeaderText="Tools">
                                                <ItemTemplate>
                                                    <%--<asp:ImageButton ID="imgbtnEdit" runat="server" Height="20px" Width="20px" ImageUrl="~/img/icon/b_edit.png"
                                                        CommandArgument='<%# DataBinder.Eval(Container.DataItem, "TempID").ToString()%>'
                                                        OnClick="imgbtnEdit_Click" />
                                                    &nbsp;--%>
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
                        </div>   

                        <%--Button--%>                                         
                        <div class="row" style="margin-top: 15px;">
                            <div class="col-xs-2"></div>
                            <div class="col-xs-8">
                                <asp:Button ID="btnSave" runat="server" Text="บันทึก" CssClass="btn btn-default" OnClick="btnSave_Click" />
                                &nbsp;&nbsp;&nbsp;
                               <asp:Button ID="btnCancel" runat="server" Text="ยกเลิก" CssClass="btn btn-default" />
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

    <%--<asp:ModalPopupExtender ID="ModalPopupExtender1" runat="server" BackgroundCssClass="modalBackground"
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
                <div class="col-md-2 headerData"><b>จำนวน :</b></div>
                <div class="col-md-3 rowData">
                    <asp:TextBox ID="txtMAmount" runat="server" onKeyPress="kteeyintNodot()" class="form-control"></asp:TextBox>
                </div>              
                <div class="col-md-1"></div>
            </div> 
            <div class="row">&nbsp;</div> 
            <div class="row" style="margin-top: 15px;">
                <div class="col-md-12 text-center">
                    <asp:Button ID="btnMSave" runat="server" CssClass="btn btn-save" Text="Save" OnClick="btnMSave_Click"/>
                    <asp:Button ID="btnModalClose" runat="server" CssClass="btn btn-save" Text="Close"/>
                </div>
            </div>
        </div>
    </asp:Panel>--%>

    <asp:ModalPopupExtender ID="ModalPopupExtender3" runat="server" BackgroundCssClass="modalBackground"
        PopupControlID="Panel3" TargetControlID="Label2">
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
                    <asp:TextBox ID="txtSearchItemCode" runat="server"></asp:TextBox>
                </div>
                <div class="col-md-2 headerData"><b>ชื่อสินค้า :</b></div>
                <div class="col-md-3 rowData">
                    <asp:TextBox ID="txtSearchItemName" runat="server"></asp:TextBox>                    
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
                                <HeaderStyle CssClass="text-center width65 headerData" />
                                <ItemStyle CssClass="text-left rowData"/>
                            </asp:BoundField>                        
                            <asp:TemplateField HeaderText="Choose">
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
