<%@ Page Title="" Language="C#" MasterPageFile="~/Site1.Master" AutoEventWireup="true" CodeBehind="UserLogin.aspx.cs" Inherits="Billing.Setup.UserLogin" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="row">
        <div class="col-xs-1"></div>
        <div class="col-xs-10">
            <div class="panel panel-info panel-info-dark" style="min-height: 500px;">
                <div class="panel-heading panel-heading-dark text-left">
                    <h3 class="panel-title">
                        <strong>
                            <asp:Label ID="lblHeader" runat="server" Text="">Search User</asp:Label>
                        </strong>
                    </h3>
                </div>
                <div class="panel-body text-center" style="margin-top: 5px;">
                    <div style="padding: 0px 15px 15px 15px;">
                        <div class="row">
                            <div class="col-xs-2 headerData"><b>Username :</b></div>
                            <div class="col-xs-4 rowData">
                                <asp:TextBox ID="txtName" runat="server"></asp:TextBox>
                            </div>
                            <div class="col-xs-2 headerData"><b></b></div>
                            <div class="col-xs-4 rowData">
                            </div>
                        </div>
                        <div class="row" style="margin-top: 15px;">
                            <div class="col-xs-2"></div>
                            <div class="col-xs-8">
                                <asp:Button ID="btnSearch" runat="server" Text="Search" CssClass="btn btn-default" OnClick="btnSearch_Click"/>
                                &nbsp;&nbsp;&nbsp;
                               <asp:Button ID="btnAdd" runat="server" Text="Add" CssClass="btn btn-default" OnClick="btnAdd_Click"/>
                            </div>
                            <div class="col-xs-2">
                            </div>
                        </div>
                        <div class="row" style="margin-top: 15px; max-height:350px; overflow:auto;">
                            <div class="col-xs-12">
                                <div class="alert alert-info dark text-left">
                                    <strong>List User</strong>
                                </div>
                                <asp:GridView ID="gv" runat="server" Width="100%" AutoGenerateColumns="False">
                                    <Columns>
                                        <asp:BoundField HeaderText="Username" DataField="Username">
                                            <HeaderStyle CssClass="text-center width40 headerData" />
                                            <ItemStyle CssClass="text-left rowData" />
                                        </asp:BoundField>
                                        <%--<asp:BoundField HeaderText="Tel" DataField="Tel">
                                            <HeaderStyle CssClass="text-center width20 headerData" />
                                            <ItemStyle CssClass="text-left rowData" />
                                        </asp:BoundField>--%>
                                        <asp:BoundField HeaderText="Active" DataField="Active">
                                            <HeaderStyle CssClass="text-center width10 headerData" />
                                            <ItemStyle CssClass="text-center rowData" />
                                        </asp:BoundField>
                                        <asp:BoundField HeaderText="Last Login" DataField="LastLogin">
                                            <HeaderStyle CssClass="text-center width20 headerData" />
                                            <ItemStyle CssClass="text-left rowData" />
                                        </asp:BoundField>
                                        <asp:TemplateField HeaderText="Tools">
                                            <ItemTemplate>
                                                <%--<asp:HiddenField ID="hddGID" runat="server" Value='<%# DataBinder.Eval(Container.DataItem, "AccountID").ToString()%>' />--%>
                                                <asp:ImageButton ID="imgbtnEdit" runat="server" Height="20px" Width="20px" ImageUrl="~/img/icon/b_edit.png" 
                                                    CommandArgument='<%# DataBinder.Eval(Container.DataItem, "Username").ToString()%>'
                                                    OnClick="imgbtnEdit_Click"/>
                                                &nbsp;
                                                <asp:ImageButton ID="imgbtnDelete" runat="server" Height="20px" Width="20px" ImageUrl="~/img/icon/icon_delete.gif" 
                                                    CommandArgument='<%# DataBinder.Eval(Container.DataItem, "Username").ToString()%>'
                                                    OnClick="imgbtnDelete_Click" OnClientClick="return confirm('ยืนยันการปิดการใช้งาน UserLogin นี้?');"/>
                                                    <%-- OnClientClick="return confirm('คุณต้องการเปลี่ยนสถานะข้อมูลนี้หรือไม่ ?');" OnClick="imgbtnDelete_Click" ToolTip="เปลี่ยนสถานะ"
                                                    Visible='<%# string.IsNullOrEmpty(DataBinder.Eval(Container.DataItem, "Name").ToString()) ? false : true %>'--%>
                                            </ItemTemplate>
                                            <HeaderStyle CssClass="text-center width10 headerData" />
                                            <ItemStyle CssClass="text-center rowData" />
                                        </asp:TemplateField>
                                    </Columns>
                                    <EmptyDataTemplate>
                                        <table>
                                            <tr>
                                                <td></td>
                                            </tr>
                                            <tr>
                                                <td colspan="4">
                                                    No data.
                                                </td>
                                            </tr>
                                        </table>
                                    </EmptyDataTemplate>
                                </asp:GridView>

                                <%--<uc1:ucpaging ID="ucPaging1" runat="server" />--%>
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
    <asp:Panel ID="Panel1" Height="370px" Width="800px" runat="server" Style="display: none;">
        <%--Style="display: none;"--%>
        <div class="panel panel-info-dark width98" style="min-height: 320px;">
            <div class="panel-heading text-left">
                <h3 class="panel-title">
                    <asp:Label ID="lbl_modal_view" runat="server" CssClass="modalHeader" Text="Manage User Login"></asp:Label>
                </h3>
            </div>
            <div class="row" style="margin-top: 20px;">
                <div class="col-md-1"></div>
                <div class="col-md-2 headerData"><b>Username :</b></div>
                <div class="col-md-8 rowData">
                    <asp:TextBox ID="txtMUsername" runat="server"></asp:TextBox>
                    <asp:HiddenField ID="hddMode" runat="server" />
                </div>
                <div class="col-md-1"></div>
            </div>
            <div class="row" id="resultOldPass" runat="server">
                <div class="col-md-1"></div>
                <div class="col-md-2 headerData"><b>Old Password :</b></div>
                <div class="col-md-8 rowData">
                    <asp:TextBox ID="txtMOldPass" runat="server" TextMode="Password"></asp:TextBox>
                </div>
                <div class="col-md-1"></div>
            </div>
            <div class="row">
                <div class="col-md-1"></div>
                <div class="col-md-2 headerData"><b>New Password :</b></div>
                <div class="col-md-8 rowData">
                    <asp:TextBox ID="txtMNewPass" runat="server" TextMode="Password"></asp:TextBox>
                </div>
                <div class="col-md-1"></div>
            </div>
            <div class="row">
                <div class="col-md-1"></div>
                <div class="col-md-2 headerData"><b>Confirm :</b></div>
                <div class="col-md-8 rowData">
                    <asp:TextBox ID="txtMCheckPass" runat="server" TextMode="Password"></asp:TextBox>
                </div>
                <div class="col-md-1"></div>
            </div>
            <div class="row">
                <div class="col-md-1"></div>
                <div class="col-md-2 headerData"><b>Status :</b></div>
                <div class="col-md-8 rowData">
                    <asp:CheckBox ID="chkActive" runat="server" Checked="true"/>
                </div>
                <div class="col-md-1"></div>
            </div>
            <div class="row" style="margin-top: 15px;">
                <div class="col-md-12 text-center">
                    <asp:Button ID="btnModalSave" runat="server" CssClass="btn btn-default" Text="Save" OnClick="btnModalSave_Click"/>
                    &nbsp;&nbsp;
                    <asp:Button ID="btnModalClose" runat="server" CssClass="btn btn-default" Text="Close"></asp:Button>
                </div>
            </div>
        </div>
    </asp:Panel>
</asp:Content>
