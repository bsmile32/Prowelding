<%@ Page Title="" Language="C#" MasterPageFile="~/Site1.Master" AutoEventWireup="true" CodeBehind="ReportSaleItem.aspx.cs" Inherits="Billing.Report.ReportSaleItem" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <%--<script src="https://code.jquery.com/jquery-1.12.4.js"></script>
    <script src="https://code.jquery.com/ui/1.12.1/jquery-ui.js"></script>
    <script>
      $( function() {
        var availableTags = [
          "ActionScript",
          "AppleScript",
          "Asp",
          "BASIC",
          "C",
          "C++",
          "Clojure",
          "COBOL",
          "ColdFusion",
          "Erlang",
          "Fortran",
          "Groovy",
          "Haskell",
          "Java",
          "JavaScript",
          "Lisp",
          "Perl",
          "PHP",
          "Python",
          "Ruby",
          "Scala",
          "Scheme"
        ];
        $( "#tags" ).autocomplete({
          source: availableTags
        });
      } );
      </script>--%>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="row">
        <div class="col-xs-1"></div>
        <div class="col-xs-10">
            <div class="panel panel-info panel-info-dark" style="min-height: 370px;">
                <div class="panel-heading panel-heading-dark text-left">
                    <h3 class="panel-title">
                        <strong>
                            <asp:Label ID="lblHeader" runat="server" Text="">Report Sale Items</asp:Label>
                        </strong>
                    </h3>
                </div>
                <div class="panel-body text-center" style="margin-top: 5px;">
                    <div style="padding: 0px 15px 15px 15px;">
                        <div class="row">
                            <div class="col-xs-2 headerData"><b>สินค้า :</b></div>
                            <div class="col-xs-4 rowData">
                                <asp:TextBox ID="txtItemCode" runat="server" class="form-control"></asp:TextBox>
                                <%--<asp:DropDownList ID="ddlItem" runat="server" AutoPostBack="true" class="form-control" >
                                </asp:DropDownList>--%>
                                <%--<input id="tags" />--%>
                            </div>
                            <div class="col-xs-2 headerData"><b>แสดงราคา :</b></div>
                            <div class="col-xs-4 rowData">
                                <asp:CheckBox ID="chkShow" runat="server"/>                                
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
                               <asp:Button ID="btnExport" runat="server" Text="Export" CssClass="btn btn-default" OnClick="btnExport_Click"/>
                            </div>
                            <div class="col-xs-2">
                            </div>
                        </div>
                        <div class="row" style="margin-top: 15px; max-height:350px; overflow:auto;">
                            <div class="col-xs-12">
                                <asp:GridView ID="gv" runat="server" Width="100%" AutoGenerateColumns="False">
                                    <Columns>       
                                        <asp:BoundField HeaderText="รหัสสินค้า" DataField="ItemCode">
                                            <HeaderStyle CssClass="text-center width10" />
                                            <ItemStyle CssClass="text-left"/>
                                        </asp:BoundField>                                  
                                        <asp:BoundField HeaderText="สินค้า" DataField="ItemName">
                                            <HeaderStyle CssClass="text-center width20" />
                                            <ItemStyle CssClass="text-left"/>
                                        </asp:BoundField>  
                                        <asp:BoundField HeaderText="จำนวน" DataField="AmountStr">
                                            <HeaderStyle CssClass="text-center width5" />
                                            <ItemStyle CssClass="text-center"/>
                                        </asp:BoundField>  
                                        <asp:BoundField HeaderText="ราคา" DataField="ItemPriceStr">
                                            <HeaderStyle CssClass="text-center width8" />
                                            <ItemStyle CssClass="text-right"/>
                                        </asp:BoundField> 
                                        <asp:BoundField HeaderText="รวม" DataField="TotalStr">
                                            <HeaderStyle CssClass="text-center width8" />
                                            <ItemStyle CssClass="text-right"/>
                                        </asp:BoundField>
                                        <asp:TemplateField HeaderText="Tools">
                                            <ItemTemplate>
                                                <asp:ImageButton ID="imgbtnView" runat="server" Height="20px" Width="20px" ImageUrl="~/img/icon/b_edit.png"
                                                    CommandArgument='<%# DataBinder.Eval(Container.DataItem, "ItemCode").ToString()%>'
                                                    OnClick="imgbtnView_Click" />
                                            </ItemTemplate>
                                            <HeaderStyle CssClass="text-center width3" />
                                            <ItemStyle CssClass="text-center" />
                                        </asp:TemplateField>   
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
    
    <asp:ModalPopupExtender ID="ModalPopupExtender1" runat="server" BackgroundCssClass="modalBackground"
        PopupControlID="Panel1" TargetControlID="lbl_modal_view">
    </asp:ModalPopupExtender>
    <asp:Panel ID="Panel1" Height="400px" Width="500px" runat="server" Style="display: none;">
        <%--Style="display: none;"--%>
        <div class="panel panel-info-dark width100" style="min-height: 400px;">
            <div class="panel-heading text-left">
                <h3 class="panel-title">
                    <asp:Label ID="lbl_modal_view" runat="server" CssClass="modalHeader" Text="List Sale"></asp:Label>
                </h3>
            </div>            
            <div class="row" style="height:280px !important;">    
                <div class="col-md-1"></div>            
                <div class="col-md-10 rowData" style="overflow:auto; height:290px !important;">
                    <asp:GridView ID="gvSale" runat="server" Width="99%" AutoGenerateColumns="False">
                        <Columns>
                            <asp:BoundField HeaderText="เลขที่" DataField="SaleNumber">
                                <HeaderStyle CssClass="text-center width25" />
                                <ItemStyle CssClass="text-left"/>
                            </asp:BoundField>                                                                                               
                            <asp:TemplateField HeaderText="Tools">
                                <ItemTemplate>
                                    <asp:ImageButton ID="imgbtnChoose" runat="server" Height="20px" Width="20px" ImageUrl="~/img/icon/b_edit.png"                                        
                                        CommandArgument='<%# DataBinder.Eval(Container.DataItem, "SaleHeaderID").ToString()%>'
                                        OnClick="imgbtnChoose_Click" />
                                </ItemTemplate>
                                <HeaderStyle CssClass="text-center width5" Height="30px"/>
                                <ItemStyle CssClass="text-center" />
                            </asp:TemplateField>                            
                        </Columns>
                        <HeaderStyle BackColor="#ff7777" />
                        <EmptyDataTemplate>
                            <table border="1" style="width:30%; padding:5px;">
                                <tr>
                                    <td>เลขที่</td>
                                    <td></td>
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
                <div class="col-md-1"></div>
            </div>
            <div class="row">&nbsp;</div> 
            <div class="row" style="margin-top: 15px;">
                <div class="col-md-12 text-center">
                    <asp:Button ID="btnModalClose" runat="server" CssClass="btn btn-save" Text="Close"/>
                </div>
            </div>
        </div>
    </asp:Panel>  
</asp:Content>
