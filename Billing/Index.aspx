<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Index.aspx.cs" Inherits="Billing.Index" %>

<!DOCTYPE html> 

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <link rel="stylesheet" href="/bower_components/Font-Awesome/css/font-awesome.css"/>
    <script src="/Script/jquery.min.js"></script>
    <link href="/bootstrap-3.3.7-dist/css/bootstrap.css" rel="stylesheet" />
    <script src="/bootstrap-3.3.7-dist/js/bootstrap.min.js"></script>
    <link href="/CSS/StyleSheet1.css" rel="stylesheet" />
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
        <div class="row" style="height:150px;">
            &nbsp;
        </div>
        <div class="row" style="height:400px;">
            <div class="col-xs-3"></div>
            <div class="col-xs-6" style="border: 4px solid #ff202d; height:350px; border-radius: 20px 20px;">
                <div class="row">
                    <div class="col-xs-12 text-center" style="padding-top: 10px;">
                        <img src="Image/PRO WELDING logo.jpg" style="width:165px; height:100px;"/>
                    </div>
                </div>
                <div class="row">
                    <div class="col-xs-12 text-center" style="padding-top: 5px;">
                        <h2>PRO WELDING & TOOLS <i class="fa fa-archive"></i></h2>
                    </div>
                </div>
                <div class="row">
                    &nbsp;
                </div>
                <div class="row">
                    <div class="col-xs-3"></div>
                    <div class="col-xs-2">
                        <h4>Username</h4>
                    </div>
                    <div class="col-xs-4">
                        <asp:TextBox ID="txtUser" runat="server"></asp:TextBox>
                    </div>
                    <div class="col-xs-3"></div>
                </div>
                <div class="row">
                    <div class="col-xs-3"></div>
                    <div class="col-xs-2">
                        <h4>Password</h4>
                    </div>
                    <div class="col-xs-4">
                        <asp:TextBox ID="txtPass" runat="server" TextMode="Password"></asp:TextBox>
                        <asp:Label ID="lbMsg" runat="server" ForeColor="Red"></asp:Label>
                    </div>
                    <div class="col-xs-3"></div>
                </div>
                <div class="row" style="margin-top:10px;">
                    <div class="col-xs-5"></div>
                    <div class="col-xs-7">
                        <asp:Button ID="btnLogin" runat="server" Text="Login" CssClass="btn btn-default" OnClick="btnLogin_Click"/>
                    </div>
                </div>
            </div>
            <div class="col-xs-3"></div>
        </div>
        <div class="row">
    
        </div>
    </form>
</body>
</html>
