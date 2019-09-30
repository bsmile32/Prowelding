<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ucPaging.ascx.cs" Inherits="Billing.Usercontrols.ucPaging" %>
<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="asp" %>
<%--<div id="dvPager" runat="server" class="alert alert-info width100" style="height: 50px;" role="heading">
    <table class="width100" style="margin-top: -10px;">
        <tr>
            <td class="width30 text-left">Total&nbsp;<asp:Label ID="lblRecord" runat="server" Text="0"></asp:Label>&nbsp;Records
            </td>
            <td class="width70 text-right">Display/Page&nbsp;
                    <asp:TextBox ID="txtPageSize" runat="server" MaxLength="10" Width="60px"></asp:TextBox>
                <asp:FilteredTextBoxExtender ID="txtPageSize_FilteredTextBoxExtender" runat="server" Enabled="True" TargetControlID="txtPageSize" ValidChars="0123456789">
                </asp:FilteredTextBoxExtender>
                &nbsp;Records
                    <asp:Button runat="server" ID="imgPageSize" Text="OK" CssClass="btnSave" Height="30px" Width="30px" />
                &nbsp;
                    <asp:ImageButton ID="imgFirstPage" OnCommand="NavigationLink_Click" CommandName="First" ToolTip="First" ImageUrl="~/images/icon-first.png" ImageAlign="AbsMiddle" runat="server"></asp:ImageButton>&nbsp;&nbsp;
                    <asp:ImageButton ID="imgPreviousPage" OnCommand="NavigationLink_Click" CommandName="Prev" ToolTip="Previous" ImageUrl="~/images/icon-pre.png" ImageAlign="AbsMiddle" runat="server"></asp:ImageButton>
                &nbsp;Page&nbsp;<asp:Label ID="lblCurrentPage" runat="server"></asp:Label>&nbsp;of&nbsp;<asp:Label ID="lblTotalPages" runat="server">1</asp:Label>
                &nbsp;
                    <asp:ImageButton ID="imgNextPage" OnCommand="NavigationLink_Click" CommandName="Next" ToolTip="Next" ImageUrl="~/images/icon-next.png" ImageAlign="AbsMiddle" runat="server"></asp:ImageButton>&nbsp;&nbsp;
                    <asp:ImageButton ID="imgLastPage" OnCommand="NavigationLink_Click" CommandName="Last" ToolTip="Last" ImageUrl="~/images/icon-last.png" ImageAlign="AbsMiddle" runat="server"></asp:ImageButton>
                Go To&nbsp;<asp:TextBox ID="txtGoToPage" MaxLength="10" runat="server" Width="60px"></asp:TextBox>
                <asp:FilteredTextBoxExtender ID="txtGoToPage_FilteredTextBoxExtender" runat="server" Enabled="True" TargetControlID="txtGoToPage" ValidChars="0123456789">
                </asp:FilteredTextBoxExtender>
                &nbsp;
                    <asp:Button runat="server" ID="imgGo" Text="GO" CssClass="btnSave" Height="30px" Width="30px" />
            </td>
        </tr>
    </table>
</div>--%>

<div id="dvPager" runat="server" class="alert alert-info width100" style="height: 50px;" role="heading">
    <table class="width100" style="margin-top: -5px;">
        <tr>
            <td class="width30 text-left">Total&nbsp;<asp:Label ID="lblRecord" runat="server" Text="0"></asp:Label>&nbsp;Records
            </td>
            <td class="width70 text-right">Display/Page&nbsp;
                    <asp:TextBox ID="txtPageSize" runat="server" MaxLength="10" Width="60px"></asp:TextBox>
                <asp:FilteredTextBoxExtender ID="txtPageSize_FilteredTextBoxExtender" runat="server" Enabled="True" TargetControlID="txtPageSize" ValidChars="0123456789">
                </asp:FilteredTextBoxExtender>
                &nbsp;Records
                    <asp:Button runat="server" ID="imgPageSize" Text="OK" CssClass="btnSave" Height="30px" Width="30px" />
                &nbsp;
                    <asp:ImageButton ID="imgFirstPage" CommandName="First" ToolTip="First" ImageUrl="~/img/icon/icon-first.png" ImageAlign="AbsMiddle" runat="server"></asp:ImageButton>&nbsp;&nbsp;
                    <asp:ImageButton ID="imgPreviousPage" CommandName="Prev" ToolTip="Previous" ImageUrl="~/img/icon/icon-pre.png" ImageAlign="AbsMiddle" runat="server"></asp:ImageButton>
                &nbsp;Page&nbsp;<asp:Label ID="lblCurrentPage" runat="server"></asp:Label>&nbsp;of&nbsp;<asp:Label ID="lblTotalPages" runat="server">1</asp:Label>
                &nbsp;
                    <asp:ImageButton ID="imgNextPage" CommandName="Next" ToolTip="Next" ImageUrl="~/img/icon/icon-next.png" ImageAlign="AbsMiddle" runat="server"></asp:ImageButton>&nbsp;&nbsp;
                    <asp:ImageButton ID="imgLastPage" CommandName="Last" ToolTip="Last" ImageUrl="~/img/icon/icon-last.png" ImageAlign="AbsMiddle" runat="server"></asp:ImageButton>
                Go To&nbsp;<asp:TextBox ID="txtGoToPage" MaxLength="10" runat="server" Width="60px"></asp:TextBox>
                <asp:FilteredTextBoxExtender ID="txtGoToPage_FilteredTextBoxExtender" runat="server" Enabled="True" TargetControlID="txtGoToPage" ValidChars="0123456789">
                </asp:FilteredTextBoxExtender>
                &nbsp;
                    <asp:Button runat="server" ID="imgGo" Text="GO" CssClass="btnSave" Height="30px" Width="30px" />
            </td>
        </tr>
    </table>
</div>
