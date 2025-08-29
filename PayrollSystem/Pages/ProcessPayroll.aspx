<%@ Page Language="VB" AutoEventWireup="true" CodeBehind="ProcessPayroll.aspx.vb" Inherits="PayrollSystem.ProcessPayroll" %>

<!DOCTYPE html>
<html>
<head>
    <title>Process Payroll</title>
    <link href="https://stackpath.bootstrapcdn.com/bootstrap/4.5.2/css/bootstrap.min.css" rel="stylesheet" />

</head>
<body>
    <h2>Payroll Processing</h2>
    <form id="form1" runat="server">
        <asp:Button ID="btnProcess" runat="server" CssClass="btn btn-primary"  Text="Generate Payroll" OnClick="btnProcess_Click" /><br /><br />
        <asp:GridView ID="gvPayroll" runat="server" CssClass="table table-striped table-bordered" AutoGenerateColumns="True" /><br />
        <asp:Button ID="btnGoToReport" runat="server" CssClass="btn btn-primary"  Text="Go to Reports" PostBackUrl="~/Pages/Report.aspx" />
    </form>
</body>
</html>
