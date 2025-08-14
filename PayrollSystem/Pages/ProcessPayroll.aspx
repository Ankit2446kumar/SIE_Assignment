<%@ Page Language="VB" AutoEventWireup="true" CodeBehind="ProcessPayroll.aspx.vb" Inherits="PayrollSystem.ProcessPayroll" %>

<!DOCTYPE html>
<html>
<head>
    <title>Process Payroll</title>
</head>
<body>
    <h2>Payroll Processing</h2>
    <form id="form1" runat="server">
        <asp:Button ID="btnProcess" runat="server" Text="Generate Payroll" OnClick="btnProcess_Click" /><br /><br />
        <asp:GridView ID="gvPayroll" runat="server" AutoGenerateColumns="True" /><br />
        <asp:Button ID="btnGoToReport" runat="server" Text="Go to Reports" PostBackUrl="~/Pages/Report.aspx" />
    </form>
</body>
</html>
