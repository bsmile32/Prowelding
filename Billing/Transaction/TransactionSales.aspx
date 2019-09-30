<%@ Page Title="" Language="C#" MasterPageFile="~/Site1.Master" AutoEventWireup="true" CodeBehind="TransactionSales.aspx.cs" Inherits="Billing.Transaction.TransactionSales" %>
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
    <script>
        $(document).ready(function () {
            SetPayType();

            $("#ddlPay").change(function () {
                SetPayType();
            });
            
        });
        function SetPayType() {
            if ($("#ddlPay").val() == "5") {
                $("#dvTransfer").show();
                $("#dvInst").hide();
                $("#dvNull").hide();
                $("#txtTimeTransfer").prop("disabled", false);
                $("#ddlAccountInst").val('');
            } else if ($("#ddlPay").val() == "8") {
                $("#dvNull").hide();
                $("#dvTransfer").hide();
                $("#dvInst").show();
                $("#txtTimeTransfer").prop("disabled", true);
                $("#txtTimeTransfer").val('');
                $("#ddlAccount").val('');
            } else {
                $("#dvNull").show();
                $("#dvTransfer").hide();
                $("#dvInst").hide();
                $("#txtTimeTransfer").prop("disabled", true);
                $("#txtTimeTransfer").val('');
                $("#ddlAccount").val('');
                $("#ddlAccountInst").val('');
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
                            <asp:Label ID="lblHeader" runat="server" Text="">Sale</asp:Label>
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
                            <%--<div class="row width99" style="padding-left:35px;">
                                <div class="col-xs-2 headerData"><b></b></div>
                                <div class="col-xs-4 rowData">
                                    
                                </div>
                                <div class="col-xs-2 headerData"><b></b></div>
                                <div class="col-xs-4 rowData">                                    
                                </div> 
                            </div>--%>
                            <div class="row width99" style="padding-left:35px;">                                
                                <div class="col-xs-2 headerData"><b>เลขที่ :</b></div>
                                <div class="col-xs-4 rowData">
                                    <asp:TextBox ID="txtSaleNumber" runat="server" Width="90%"></asp:TextBox>
                                </div> 
                                <div class="col-xs-2 headerData"><b>เบอร์โทรศัพท์ :</b></div>
                                <div class="col-xs-4 rowData">
                                    <asp:TextBox ID="txtTel" runat="server" Width="90%"></asp:TextBox>
                                </div>                    
                            </div> 
                            <div class="row width99" style="padding-left:35px;">
                                <div class="col-xs-2 headerData"><b>วันที่ :</b></div>
                                <div class="col-xs-4 rowData">
                                    <asp:TextBox ID="txtDate" runat="server" placeholder="กดเพื่อเปิดปฏิทิน" Width="90%" 
                                        AutoPostBack="true" OnTextChanged="txtDate_TextChanged"></asp:TextBox>
                                    <asp:CalendarExtender ID="ceDate" runat="server" Format="dd/MM/yyyy" TargetControlID="txtDate" PopupPosition="TopLeft"></asp:CalendarExtender> 
                                </div> 
                                <div class="col-xs-2 headerData"><b>วันที่รับประกัน :</b></div>
                                <div class="col-xs-4 rowData">
                                    <asp:TextBox ID="txtWarranty" runat="server" placeholder="กดเพื่อเปิดปฏิทิน" Width="90%" ></asp:TextBox>
                                    <asp:CalendarExtender ID="CalendarExtender1" runat="server" Format="dd/MM/yyyy" TargetControlID="txtWarranty" PopupPosition="TopLeft"></asp:CalendarExtender> 
                                </div>                             
                            </div>  
                            <div class="row width99" style="padding-left:35px;">
                                <div class="col-xs-2 headerData"><b>ประเภทบิล :</b></div>
                                <div class="col-xs-4 rowData">
                                    <asp:RadioButton ID="rdbCash" runat="server" Text=" เงินสด" GroupName="pay" />
                                    &nbsp;&nbsp;&nbsp;
                                    <asp:RadioButton ID="rdbVat" runat="server" Text=" Vat" GroupName="pay" />
                                </div> 
                                <div class="col-xs-2 headerData"><b>ช่องทางการชำระเงิน :</b></div>
                                <div class="col-xs-4 rowData">
                                    <asp:DropDownList ID="ddlPay" runat="server" AppendDataBoundItems="true" Width="90%" class="form-control" >
                                    </asp:DropDownList>
                                </div>                             
                            </div>  
                            <div class="row width99" style="padding-left:35px;">
                                <div class="col-xs-2 headerData"><b>Consignment No. :</b></div>
                                <div class="col-xs-4 rowData">
                                    <asp:TextBox ID="txtConsignmentNo" runat="server" Width="90%" ></asp:TextBox>
                                </div> 
                                <div id="dvTransfer" class="col-xs-6" style="padding:0 !important;">
                                    <div class="col-xs-4 headerData"><b>บัญชีที่โอน :</b></div>
                                    <div class="col-xs-8 rowData">
                                        <asp:DropDownList ID="ddlAccount" runat="server" AppendDataBoundItems="true" Width="90%" class="form-control" >
                                        </asp:DropDownList>
                                    </div>
                                </div>
                                <div id="dvInst" class="col-xs-6" style="padding:0 !important;">
                                    <div class="col-xs-4 headerData"><b>ธนาคาร :</b></div>
                                    <div class="col-xs-8 rowData">
                                        <asp:DropDownList ID="ddlAccountInst" runat="server" AppendDataBoundItems="true" Width="90%" class="form-control" >
                                        </asp:DropDownList>
                                        
                                    </div>
                                </div>
                                <div id="dvNull" class="col-xs-6" style="padding:0 !important;">
                                    <div class="col-xs-4 headerData"><b></b></div>
                                    <div class="col-xs-8 rowData">
                                    </div>
                                </div>
                                                            
                            </div> 
                            <div class="row width99" style="padding-left:35px;">
                                <div class="col-xs-2 headerData"><b>ผู้ขาย :</b></div>
                                <div class="col-xs-4 rowData">
                                    <asp:DropDownList ID="ddlSaleName" runat="server" AppendDataBoundItems="true" Width="90%" class="form-control" >
                                    </asp:DropDownList>
                                </div>  
                                <div class="col-xs-2 headerData"><b>เวลาโอน :</b></div>
                                <div class="col-xs-4 rowData">
                                    <asp:TextBox ID="txtTimeTransfer" runat="server" Width="90%" Enabled="false"></asp:TextBox>
                                </div>                           
                            </div>
                            <div class="row width99" style="padding-left:35px;">
                                <div class="col-xs-5 text-center" style="padding: 9px 3px 3px 3px;"><b>ที่อยู่สำหรับออกใบเสร็จ</b></div>
                                <div class="col-xs-2 center-block">
                                    <asp:Button ID="btnCopyAdd" runat="server" Text="Copy Address" CssClass="btn btn-default" OnClick="btnCopyAdd_Click"/>
                                </div>
                                <div class="col-xs-5 text-center" style="padding: 8px 3px 3px 3px;"><b>ที่อยู่สำหรับส่งสินค้า</b></div>
                            </div>
                            <div class="row width99" style="padding-left:35px;">
                                <div class="col-xs-2 headerData"><b>ชื่อลูกค้า :</b></div>
                                <div class="col-xs-4 rowData">
                                    <asp:TextBox ID="txtCustName" runat="server" Width="90%"></asp:TextBox>
                                    <asp:ImageButton ID="imgbtnSearch" runat="server" Height="20px" Width="20px" ImageUrl="~/img/icon/b_edit.png"                                        
                                        OnClick="imgbtnSearch_Click" />
                                </div>
                                <div class="col-xs-2 headerData"><b>ชื่อผู้รับ :</b></div>
                                <div class="col-xs-4 rowData">
                                    <asp:TextBox ID="txtDeliveryName" runat="server" Width="90%"></asp:TextBox>
                                </div>                                                            
                            </div>
                            <div class="row width99" style="padding-left:35px;">
                                <div class="col-xs-2 headerData"><b>ที่อยู่ :</b></div>
                                <div class="col-xs-4 rowData">
                                    <asp:TextBox ID="txtCustAddress" runat="server" Width="90%"></asp:TextBox>
                                </div>
                                <div class="col-xs-2 headerData"><b>ที่อยู่ :</b></div>
                                <div class="col-xs-4 rowData">
                                    <asp:TextBox ID="txtDeliverAddress" runat="server" Width="90%"></asp:TextBox>
                                </div>                                                            
                            </div>    
                            <div class="row width99" style="padding-left:35px;">
                                <div class="col-xs-2 headerData"><b>แขวง/ตำบล :</b></div>
                                <div class="col-xs-4 rowData">
                                    <asp:TextBox ID="txtCustDistrict" runat="server" Width="90%"></asp:TextBox>
                                </div>
                                <div class="col-xs-2 headerData"><b>แขวง/ตำบล :</b></div>
                                <div class="col-xs-4 rowData">
                                    <asp:TextBox ID="txtDeliverDistrict" runat="server" Width="90%"></asp:TextBox>
                                </div>                                                          
                            </div>   
                            <div class="row width99" style="padding-left:35px;">
                                <div class="col-xs-2 headerData"><b>เขต/อำเภอ :</b></div>
                                <div class="col-xs-4 rowData">
                                    <asp:TextBox ID="txtCustCountry" runat="server" Width="90%"></asp:TextBox>
                                </div>
                                <div class="col-xs-2 headerData"><b>เขต/อำเภอ :</b></div>
                                <div class="col-xs-4 rowData">
                                    <asp:TextBox ID="txtDeliverCountry" runat="server" Width="90%"></asp:TextBox>
                                </div>                                                           
                            </div>  
                            <div class="row width99" style="padding-left:35px;">
                                <div class="col-xs-2 headerData"><b>จังหวัด :</b></div>
                                <div class="col-xs-4 rowData">
                                    <asp:TextBox ID="txtCustProvince" runat="server" Width="90%"></asp:TextBox>
                                </div>
                                <div class="col-xs-2 headerData"><b>จังหวัด :</b></div>
                                <div class="col-xs-4 rowData">
                                    <asp:TextBox ID="txtDeliverProvince" runat="server" Width="90%"></asp:TextBox>
                                </div>                                                          
                            </div>   
                            <div class="row width99" style="padding-left:35px;">
                                <div class="col-xs-2 headerData"><b>รหัสไปรษณีย์ :</b></div>
                                <div class="col-xs-4 rowData">
                                    <asp:TextBox ID="txtCustPostalCode" runat="server" Width="90%"></asp:TextBox>
                                </div>
                                <div class="col-xs-2 headerData"><b>รหัสไปรษณีย์ :</b></div>
                                <div class="col-xs-4 rowData">
                                    <asp:TextBox ID="txtDeliverPostalCode" runat="server" Width="90%"></asp:TextBox>
                                </div>                                                           
                            </div>
                            <%-- Comment
                            <div class="row width99" style="padding-left:35px;">
                                <div class="col-xs-2 headerData"><b>ที่อยู่1 :</b></div>
                                <div class="col-xs-4 rowData">
                                    <asp:TextBox ID="txtCustAddress" runat="server" Width="90%"></asp:TextBox>
                                </div>
                                <div class="col-xs-2 headerData"><b>ที่อยู่1 :</b></div>
                                <div class="col-xs-4 rowData">
                                    <asp:TextBox ID="txtDeliverAddress" runat="server" Width="90%"></asp:TextBox>
                                </div>                                                            
                            </div>    
                            <div class="row width99" style="padding-left:35px;">
                                <div class="col-xs-2 headerData"><b>ที่อยู่2 :</b></div>
                                <div class="col-xs-4 rowData">
                                    <asp:TextBox ID="txtCustAddress2" runat="server" Width="90%"></asp:TextBox>
                                </div>
                                <div class="col-xs-2 headerData"><b>ที่อยู่2 :</b></div>
                                <div class="col-xs-4 rowData">
                                    <asp:TextBox ID="txtDeliverAddress2" runat="server" Width="90%"></asp:TextBox>
                                </div>                                                          
                            </div>   
                            <div class="row width99" style="padding-left:35px;">
                                <div class="col-xs-2 headerData"><b>ที่อยู่3 :</b></div>
                                <div class="col-xs-4 rowData">
                                    <asp:TextBox ID="txtCustAddress3" runat="server" Width="90%"></asp:TextBox>
                                </div>
                                  <div class="col-xs-2 headerData"><b>ที่อยู่3 :</b></div>
                                <div class="col-xs-4 rowData">
                                    <asp:TextBox ID="txtDeliverAddress3" runat="server" Width="90%"></asp:TextBox>
                                </div>                                                           
                            </div>--%>                                                   
                            <div class="row width99" style="padding-left:35px; height:80px;">
                                <div class="col-xs-2 headerData" style="height:80px;"><b>หมายเหตุ :</b></div>
                                <div class="col-xs-10 rowData" style="height:80px;">
                                    <asp:TextBox ID="txtRemark" runat="server" Width="98%" TextMode="MultiLine" Rows="3"></asp:TextBox>
                                </div>                              
                            </div>
                        </div>

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
                                            <asp:BoundField HeaderText="สินค้า" DataField="ItemName">
                                                <HeaderStyle CssClass="text-center width25 headerData" />
                                                <ItemStyle CssClass="text-left rowData"/>
                                            </asp:BoundField>
                                            <asp:BoundField HeaderText="S/N" DataField="SerialNumber">
                                                <HeaderStyle CssClass="text-center width12 headerData" />
                                                <ItemStyle CssClass="text-left rowData"/>
                                            </asp:BoundField>
                                            <asp:BoundField HeaderText="จำนวน" DataField="AmountStr">
                                                <HeaderStyle CssClass="text-center width7 headerData" />
                                                <ItemStyle CssClass="text-center rowData"/>
                                            </asp:BoundField>      
                                            <asp:BoundField HeaderText="ราคา/หน่วย" DataField="ItemPriceStr">
                                                <HeaderStyle CssClass="text-center width10 headerData" />
                                                <ItemStyle CssClass="text-right rowData"/>
                                            </asp:BoundField>  
                                            <%--<asp:BoundField HeaderText="ส่วนลด %" DataField="DiscountPerStr">
                                                <HeaderStyle CssClass="text-center width10 headerData" />
                                                <ItemStyle CssClass="text-right rowData"/>
                                            </asp:BoundField> --%>  
                                            <asp:BoundField HeaderText="ส่วนลด (บาท)" DataField="DiscountStr">
                                                <HeaderStyle CssClass="text-center width10 headerData" />
                                                <ItemStyle CssClass="text-right rowData"/>
                                            </asp:BoundField> 
                                            <asp:BoundField HeaderText="ราคารวม" DataField="TotalStr">
                                                <HeaderStyle CssClass="text-center width12 headerData" />
                                                <ItemStyle CssClass="text-right rowData"/>
                                            </asp:BoundField>                                        
                                            <asp:TemplateField HeaderText="Tools">
                                                <ItemTemplate>
                                                    <asp:ImageButton ID="imgbtnEdit" runat="server" Height="20px" Width="20px" ImageUrl="~/img/icon/b_edit.png"
                                                        CommandArgument='<%# DataBinder.Eval(Container.DataItem, "SaleDetailID").ToString()%>'
                                                        OnClick="imgbtnEdit_Click" />
                                                    &nbsp;
                                                    <asp:ImageButton ID="imgbtnDelete" runat="server" Height="20px" Width="20px" ImageUrl="~/img/icon/icon_delete.gif" 
                                                        CommandArgument='<%# DataBinder.Eval(Container.DataItem, "SaleDetailID").ToString()%>'
                                                        OnClick="imgbtnDelete_Click" OnClientClick="return confirm('ยืนยันการลบข้อมูล?');"/>
                                                </ItemTemplate>
                                                <HeaderStyle CssClass="text-center width9 headerData" Height="30px"/>
                                                <ItemStyle CssClass="text-center rowData" />
                                            </asp:TemplateField>
                                        </Columns>
                                        <EmptyDataTemplate>
                                            <table border="1" style="width:100%; padding:5px;">
                                                <tr>
                                                    <td>สินค้า</td>
                                                    <td>จำนวน</td>
                                                    <td>ราคา/หน่วย</td>
                                                    <td>ส่วนลด (บาท)</td>
                                                    <td>ราคารวม</td>
                                                </tr>
                                                <tr>
                                                    <td colspan="5" style="text-align:left;">
                                                        No data.
                                                    </td>
                                                </tr>
                                            </table>
                                        </EmptyDataTemplate>
                                    </asp:GridView>
                                </div>
                            </div>
                            <div class="row width99" style="padding-left:35px;">
                                
                                <div class="col-xs-8 headerData"><b>รวมเงิน :</b></div>
                                <div class="col-xs-4 rowData" style="text-align:right; padding-right:90px;">
                                    <asp:Label ID="lbTotal" runat="server" Font-Bold="true" Font-Size="X-Large"></asp:Label>
                                </div>
                            </div>
                        </div>
                        
                        <div class="row" style="margin-top: 15px;">
                            <div class="col-xs-2"></div>
                            <div class="col-xs-8">
                                <asp:Button ID="btnSave" runat="server" Text="Save" CssClass="btn btn-default" OnClick="btnSave_Click" />
                                &nbsp;&nbsp;&nbsp;
                               <asp:Button ID="btnBack" runat="server" Text="Back" CssClass="btn btn-default" PostBackUrl="/Transaction/TransactionSaleList.aspx"/>
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
    <asp:Panel ID="Panel1" Height="90%" Width="90%" runat="server" Style="display: none;">
        <%--Style="display: none;"--%>
        <div class="panel panel-info-dark width100" style="min-height: 90%;">
            <div class="panel-heading text-left">
                <h3 class="panel-title">
                    <asp:Label ID="lbl_modal_view" runat="server" CssClass="modalHeader" Text="Description"></asp:Label>
                </h3>
            </div>
            <div class="row" style="margin-top: 20px;">    
                <div class="col-md-1"></div>            
                <div class="col-md-1 headerData"><b>สินค้า :</b></div>
                <div class="col-md-4 rowData">
                    <asp:HiddenField ID="hddDetailID" runat="server" />
                    <%--<asp:DropDownList ID="ddlMItem" runat="server" AutoPostBack="true" class="form-control" 
                        OnSelectedIndexChanged="ddlMItem_SelectedIndexChanged">
                    </asp:DropDownList>--%>
                    <asp:HiddenField ID="hddItemID" runat="server" />
                    <asp:HiddenField ID="hddStockID" runat="server" />
                    <asp:TextBox ID="txtMItem" runat="server" Width="92%" ReadOnly="true"></asp:TextBox>
                    <asp:ImageButton ID="imgbtnSearchItem_Click" runat="server" Height="20px" Width="20px" ImageUrl="~/img/icon/b_edit.png"                                        
                        OnClick="imgbtnSearchItem_Click_Click" />
                </div>
                <div class="col-md-1 headerData"><b>ราคา/หน่วย :</b></div>
                <div class="col-md-4 rowData">
                    <asp:TextBox ID="txtMPrice" runat="server" onKeyPress="keyintdot()"></asp:TextBox>
                </div>                
                <div class="col-md-1"></div>
            </div>      
            <div class="row">    
                <div class="col-md-1"></div>            
                <div class="col-md-1 headerData"><b>จำนวน :</b></div>
                <div class="col-md-4 rowData">
                    <asp:TextBox ID="txtMAmount" runat="server" onKeyPress="kteeyintNodot()"></asp:TextBox>
                </div>
                <div class="col-md-1 headerData">                    
                    <%--<b>ส่วนลด (%) :</b>--%>
                </div>
                <div class="col-md-4 rowData">
                    <%--<asp:TextBox ID="txtMDiscountPer" runat="server" onKeyPress="keyintNodot()"></asp:TextBox>--%>
                </div>
                <div class="col-md-1"></div>
            </div> 
            <div class="row">    
                <div class="col-md-1"></div>            
                <div class="col-md-1 headerData"><b>S / N :</b></div>
                <div class="col-md-4 rowData">
                    <asp:TextBox ID="txtMSN" runat="server"></asp:TextBox>
                </div>
                <div class="col-md-1 headerData">                    
                    <b>ส่วนลด (บาท) :</b>
                </div>
                <div class="col-md-4 rowData">
                    <asp:TextBox ID="txtMDiscount" runat="server" onKeyPress="keyintdot()"></asp:TextBox>
                </div>
                <div class="col-md-1"></div>
            </div> 
            <%--<div class="row">    
                <div class="col-md-1"></div>            
                <div class="col-md-2 headerData"><b></b></div>
                <div class="col-md-3 rowData">
                    
                </div>
                
            </div>--%>
            <div class="row" style="height:155px;">    
                <div class="col-md-1"></div>            
                <div class="col-md-1 headerData" style="height:200px;"><b>รายละเอียด :</b></div>
                <div class="col-md-9 rowData" style="height:200px;">
                    <asp:TextBox ID="txtMDescription" runat="server" TextMode="MultiLine" Rows="7" Width="99%"></asp:TextBox>
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
    </asp:Panel>

    <asp:ModalPopupExtender ID="ModalPopupExtender2" runat="server" BackgroundCssClass="modalBackground"
        PopupControlID="Panel2" TargetControlID="lbl_modal_view">
    </asp:ModalPopupExtender>
    <asp:Panel ID="Panel2" Height="600px" Width="1100px" runat="server" Style="display: none;">
        <%--Style="display: none;"--%>
        <div class="panel panel-info-dark width100" style="min-height: 500px;">
            <div class="panel-heading text-left">
                <h3 class="panel-title">
                    <asp:Label ID="Label1" runat="server" CssClass="modalHeader" Text="List Customer"></asp:Label>
                </h3>
            </div>
            <div class="row" style="margin-top: 20px;">    
                <div class="col-md-1"></div>            
                <div class="col-md-2 headerData"><b>ชื่อลูกค้า :</b></div>
                <div class="col-md-3 rowData">
                    <asp:TextBox ID="txtSearchCust" runat="server"></asp:TextBox>                    
                </div>
                <div class="col-md-2 headerData"><b>ที่อยู่ :</b></div>
                <div class="col-md-3 rowData">
                    <asp:TextBox ID="txtSearchAdd" runat="server"></asp:TextBox>
                </div>
                <div class="col-md-1"></div>
            </div>                  
            <div class="row">    
                <div class="col-md-1"></div>            
                <div class="col-md-10 rowData" style="overflow:auto; height:270px;">                    
                    <asp:GridView ID="gvCust" runat="server" Width="2000px" AutoGenerateColumns="False">                        
                        <Columns>
                            <asp:TemplateField HeaderText="Tools">
                                <ItemTemplate>
                                    <asp:ImageButton ID="imgbtnChoose" runat="server" Height="20px" Width="20px" ImageUrl="~/img/icon/b_edit.png"                                        
                                        CommandArgument='<%# Container.DataItemIndex.ToString()%>'
                                        OnClick="imgbtnChoose_Click" />
                                </ItemTemplate>
                                <HeaderStyle CssClass="text-center width3" Height="30px"/>
                                <ItemStyle CssClass="text-center" />
                            </asp:TemplateField> 
                            <asp:BoundField HeaderText="ชื่อลูกค้า" DataField="CustomerName">
                                <HeaderStyle CssClass="text-center width12" />
                                <ItemStyle CssClass="text-left"/>
                            </asp:BoundField>
                            <asp:BoundField HeaderText="ที่อยู่ใบเสร็จ" DataField="CustomerAddress">
                                <HeaderStyle CssClass="text-center width13" />
                                <ItemStyle CssClass="text-left"/>
                            </asp:BoundField>
                            <asp:BoundField HeaderText="แขวง/ตำบล" DataField="CustomerDistrict">
                                <HeaderStyle CssClass="text-center width5" />
                                <ItemStyle CssClass="text-left"/>
                            </asp:BoundField>      
                            <asp:BoundField HeaderText="เขต/อำเภอ" DataField="CustomerCountry">
                                <HeaderStyle CssClass="text-center width5" />
                                <ItemStyle CssClass="text-left"/>
                            </asp:BoundField>  
                            <asp:BoundField HeaderText="จังหวัด" DataField="CustomerProvince">
                                <HeaderStyle CssClass="text-center width5" />
                                <ItemStyle CssClass="text-left"/>
                            </asp:BoundField> 
                            <asp:BoundField HeaderText="รหัสไปรษณีย์" DataField="CustomerPostalCode">
                                <HeaderStyle CssClass="text-center width5" />
                                <ItemStyle CssClass="text-center"/>
                            </asp:BoundField>  
                            <asp:BoundField HeaderText="ชื่อผู้รับ" DataField="DeliveryName">
                                <HeaderStyle CssClass="text-center width12" />
                                <ItemStyle CssClass="text-left"/>
                            </asp:BoundField>
                            <asp:BoundField HeaderText="ที่อยู่ส่งสินค้า" DataField="DeliverAdd">
                                <HeaderStyle CssClass="text-center width13" />
                                <ItemStyle CssClass="text-left"/>
                            </asp:BoundField>
                            <asp:BoundField HeaderText="แขวง/ตำบล" DataField="DeliverDistrict">
                                <HeaderStyle CssClass="text-center width5" />
                                <ItemStyle CssClass="text-left"/>
                            </asp:BoundField>      
                            <asp:BoundField HeaderText="เขต/อำเภอ" DataField="DeliverCountry">
                                <HeaderStyle CssClass="text-center width5" />
                                <ItemStyle CssClass="text-left"/>
                            </asp:BoundField>  
                            <asp:BoundField HeaderText="จังหวัด" DataField="DeliverProvince">
                                <HeaderStyle CssClass="text-center width5" />
                                <ItemStyle CssClass="text-left"/>
                            </asp:BoundField> 
                            <asp:BoundField HeaderText="รหัสไปรษณีย์" DataField="DeliverPostalCode">
                                <HeaderStyle CssClass="text-center width5" />
                                <ItemStyle CssClass="text-center"/>
                            </asp:BoundField>  
                            <asp:BoundField HeaderText="เบอร์โทรศัพท์" DataField="Tel">
                                <HeaderStyle CssClass="text-center width7" />
                                <ItemStyle CssClass="text-center"/>
                            </asp:BoundField>                                                                                              
                        </Columns>
                        <HeaderStyle BackColor="#ff7777" />
                        <EmptyDataTemplate>
                            <table border="1" style="width:100%; padding:5px;">
                                <tr>
                                    <td>ชื่อลูกค้า</td>
                                    <td>ที่อยู่ใบเสร็จ</td>
                                    <td>แขวง/ตำบล</td>
                                    <td>เขต/อำเภอ</td>
                                    <td>จังหวัด</td>
                                    <td>รหัสไปรษณีย์</td>
                                    <td>ชื่อผู้รับ</td>
                                    <td>ที่อยู่ส่งสินค้า</td>
                                    <td>แขวง/ตำบล</td>
                                    <td>เขต/อำเภอ</td>
                                    <td>จังหวัด</td>
                                    <td>รหัสไปรษณีย์</td>
                                    <td>เบอร์โทรศัพท์</td>
                                    <td></td>
                                </tr>
                                <tr>
                                    <td colspan="14" style="text-align:left;">
                                        No data.
                                    </td>
                                </tr>
                            </table>
                        </EmptyDataTemplate>
                        <%--<PagerSettings Mode="Numeric" PageButtonCount="8"/>
                        <PagerStyle Font-Underline="True" CssClass="font-pager" />--%>
                    </asp:GridView>
                </div>                
                <div class="col-md-1"></div>
            </div>    
            <div class="row">
                    <div class="col-md-1"></div>
                    <div class="col-md-1">
                        <asp:Button ID="btnMPrevious" runat="server" CssClass="btn btn-default" Height="30" Text="<<<" OnClick="btnMPrevious_Click"/>
                    </div>
                    <div class="col-md-8"></div>
                    <div class="col-md-1">
                        <asp:Button ID="btnMNext" runat="server" CssClass="btn btn-default" Height="30" Text=">>>" OnClick="btnMNext_Click"/>
                    </div>
                    <div class="col-md-1"></div>
                </div>         
            <div class="row" style="margin-top: 15px;">
                <div class="col-md-12 text-center">
                    <asp:Button ID="btnMSearch" runat="server" CssClass="btn btn-save" Text="Search" OnClick="btnMSearch_Click"/>
                    <asp:Button ID="btnModalClose2" runat="server" CssClass="btn btn-save" Text="Close"/>
                </div>
            </div>
        </div>
    </asp:Panel>

    <asp:ModalPopupExtender ID="ModalPopupExtender3" runat="server" BackgroundCssClass="modalBackground"
        PopupControlID="Panel3" TargetControlID="lbl_modal_view">
    </asp:ModalPopupExtender>
    <asp:Panel ID="Panel3" Height="600px" Width="1100px" runat="server" Style="display: none;">
        <%--Style="display: none;"--%>
        <div class="panel panel-info-dark width100" style="min-height: 600px;">
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
            <div class="row" style="height:400px !important;">    
                <div class="col-md-1"></div>            
                <div class="col-md-10 rowData" style="overflow:auto; height:400px !important;">
                    <asp:GridView ID="gvItemSearch" runat="server" Width="100%" AutoGenerateColumns="False">
                        <Columns>
                            <asp:BoundField HeaderText="รหัสสินค้า" DataField="ItemCode">
                                <HeaderStyle CssClass="text-center width15 headerData" />
                                <ItemStyle CssClass="text-left rowData"/>
                            </asp:BoundField>
                            <asp:BoundField HeaderText="ชื่อสินค้า" DataField="itemName">
                                <HeaderStyle CssClass="text-center width25 headerData" />
                                <ItemStyle CssClass="text-left rowData"/>
                            </asp:BoundField>
                            <%--<asp:BoundField HeaderText="Serial" DataField="Serial">
                                <HeaderStyle CssClass="text-center width10 headerData" />
                                <ItemStyle CssClass="text-left rowData"/>
                            </asp:BoundField>--%>
                            <asp:BoundField HeaderText="ราคา" DataField="ItemPrice">
                                <HeaderStyle CssClass="text-center width15 headerData" />
                                <ItemStyle CssClass="text-center rowData"/>
                            </asp:BoundField>      
                            <%--<asp:BoundField HeaderText="รายละเอียด" DataField="ItemDesc" Visible="false">
                                <HeaderStyle CssClass="text-center width15 headerData" />
                                <ItemStyle CssClass="text-right rowData"/>
                            </asp:BoundField>   --%>                           
                            <asp:TemplateField HeaderText="Tools">
                                <ItemTemplate>
                                    <asp:HiddenField ID="hddItemID" runat="server" Value='<%# DataBinder.Eval(Container.DataItem, "ItemID").ToString()%>' />
                                    <asp:HiddenField ID="hddStockID" runat="server" Value='<%# DataBinder.Eval(Container.DataItem, "StockID").ToString()%>' />
                                    <asp:Label ID="lbItemDesc" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container.DataItem, "ItemDesc").ToString()%>'></asp:Label>
                                    <asp:ImageButton ID="imgbtnChooseItem" runat="server" Height="20px" Width="20px" ImageUrl="~/img/icon/b_edit.png"                                        
                                        CommandArgument='<%# Container.DataItemIndex.ToString()%>'
                                        OnClick="imgbtnChooseItem_Click" />
                                </ItemTemplate>
                                <HeaderStyle CssClass="text-center width5 headerData" Height="30px"/>
                                <ItemStyle CssClass="text-center rowData" />
                            </asp:TemplateField>                            
                        </Columns>
                        <HeaderStyle BackColor="#ff7777" />
                        <EmptyDataTemplate>
                            <table border="1" style="width:100%; padding:5px;">
                                <tr>
                                    <td>รหัสสินค้า</td>
                                    <td>ชื่อสินค้า</td>
                                    <td>Serial</td>
                                    <td>ราคา</td>                                    
                                    <td></td>
                                </tr>
                                <tr>
                                    <td colspan="4" style="text-align:left;">
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
