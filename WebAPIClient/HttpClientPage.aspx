<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="HttpClientPage.aspx.cs" Inherits="WebAPIClient.HttpClientPage" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <script type="text/javascript">
        $(document).ready(function () {
            $('#btnClear').click(function () {
                ulEmployees.empty();
            });
        });
    </script>
</head>
<body>
    <form id="form1" runat="server">
    Username : <input type="text" id="txtUsername" />
    Password : <input type="password" id="txtPassword" />
    <br /><br />
        <asp:Button ID="Button1" runat="server" Text="Get Employees Basic Authentication" OnClick="btn_Click" />
    <input id="btnClear" type="button" value="Clear" />
    <ul id="ulEmployees" runat="server"></ul>
    </form>
</body>
</html>
