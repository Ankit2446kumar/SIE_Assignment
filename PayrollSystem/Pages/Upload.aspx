<%@ Page Language="VB" AutoEventWireup="true" CodeBehind="Upload.aspx.vb" Inherits="PayrollSystem.Upload" %>

<!DOCTYPE html>
<html>
<head>
    <title>Upload Employees</title>
</head>
<body>
    <h2>Upload Employees</h2>
    <form id="form1" runat="server">
        <asp:FileUpload ID="fileUpload" runat="server" />
        <asp:Button ID="btnUploadCSV" runat="server" Text="Upload CSV" OnClick="btnUploadCSV_Click" />
        <br /><br />

        <h3>Manual Entry</h3>
        <asp:TextBox ID="txtId" runat="server" Placeholder="Employee ID"></asp:TextBox><br />
        <asp:TextBox ID="txtName" runat="server" Placeholder="Name"></asp:TextBox><br />
        <asp:TextBox ID="txtDesignation" runat="server" Placeholder="Designation"></asp:TextBox><br />
        <asp:TextBox ID="txtBasicSalary" runat="server" Placeholder="Basic Salary"></asp:TextBox><br />
        <asp:TextBox ID="txtLeaves" runat="server" Placeholder="Leaves Taken"></asp:TextBox><br />
        <asp:TextBox ID="txtWorkingDays" runat="server" Placeholder="Working Days in Month"></asp:TextBox><br />
        <asp:Button ID="btnAddEmployee" runat="server" Text="Add Employee" OnClick="btnAddEmployee_Click" /> <br /><br />
        <asp:Button ID="btnGoToProcess" runat="server" Text="Go to Payroll Processing" PostBackUrl="~/Pages/ProcessPayroll.aspx" />
        <asp:GridView ID="gvEmployees" runat="server" AutoGenerateColumns="True" />

    </form>
</body>
</html>
