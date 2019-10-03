<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="WebForm1.aspx.cs" Inherits="Billing.WebForm1" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <link rel="stylesheet" href="/bower_components/Font-Awesome/css/font-awesome.css"/>
    <link href="/bootstrap-3.3.7-dist/css/bootstrap.css" rel="stylesheet" />
    <script src="/Script/jquery.min.js"></script>
    <%--<script src="https://ajax.googleapis.com/ajax/libs/jquery/3.2.1/jquery.min.js"></script>--%>
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
        <div class="row">
            <div class="col-lg-12" id="result1">
                <button type="button" id="btnHide" class="btn btn-circle blue">Hide</button>
                <input type="text" id="txt1" />
                <button type="button" id="btnShow" class="btn btn-circle blue">Show</button>
                <a id="btnExport" href="/ExportExcel/20170725.xlsx" 
                    <i class="fa fa-user-plus"></i> Export
                </a>
                <button type="button" id="btnTestServer" runat="server" class="btn btn-circle blue"
                    onserverclick="btnTestServer_Click" value="Submit">Test Server</button>
                
            </div>
        </div>
        <div class="row">
            <div class="col-lg-12">
                <asp:Button ID="btnTest1" runat="server" Text="Testttt" 
                    class="btn btn-circle blue" OnClick="btnTest1_Click" Visible="true"/>
            </div>
        </div>
        <div class="row">
            <div class="col-lg-2">
                Test
            </div>
            <div class="col-lg-4">
                <div class="form-group col-md-10" style="padding-left:0px;padding-top: 0;padding-bottom: 0; margin:0px;">
                    <select id="sel1" class="form-control">
                        <option value="1" selected="selected">Option1</option>
                        <option value="2">Option2</option>
                    </select>
                </div>
            </div>
            <div class="col-lg-2">
                <span class="fa fa-facebook-official"> Facebook</span>
                
            </div>
            <div class="col-lg-4">
                <i class="fa fa-save"></i> Export
                <input type="text" />
            </div>
        </div>
    </form>
</body>
</html>
<script>
    $(function () {
        $('button[id=btnHide]').click(function () {
            $('input[id=txt1]').fadeOut();
        });

        $('button[id=btnShow]').click(function () {
            $('input[id=txt1]').fadeIn();
        });

        $('button[id=btnTestServer]').click(function () {
            $('input[id=txt1]').val('test');
        });

        
    });

    
</script>
