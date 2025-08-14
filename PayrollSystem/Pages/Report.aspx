<%@ Page Language="VB" AutoEventWireup="true" CodeBehind="Report.aspx.vb" Inherits="PayrollSystem.Report" %>

<!DOCTYPE html>
<html>
<head>
    <title>Payroll Report</title>
</head>
<body>
    <h2>Payroll Report</h2>
    <form id="form1" runat="server">
        <asp:GridView ID="gvReport" runat="server" AutoGenerateColumns="True"></asp:GridView><br />
        <asp:Button ID="btnExportCSV" runat="server" Text="Export to CSV" OnClick="btnExportCSV_Click" />
    </form>
</body>
</html>
