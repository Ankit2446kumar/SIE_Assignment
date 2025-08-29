<%@ Page Language="VB" AutoEventWireup="true" CodeBehind="Report.aspx.vb" Inherits="PayrollSystem.Report" %>

<!DOCTYPE html>
<html>
<head>
    <title>Payroll Report</title>
    <link href="https://stackpath.bootstrapcdn.com/bootstrap/4.5.2/css/bootstrap.min.css" rel="stylesheet" />
</head>
<body>
    <form id="form1" runat="server">
        <div class="container mt-4">
            <div class="d-flex justify-content-between align-items-center mb-3">
                <h2>Payroll Report</h2>
                <asp:Button ID="btnHome" runat="server" Text="Home" CssClass="btn btn-secondary" PostBackUrl="~/Pages/Upload.aspx" />
            </div>

    <div class="row mb-3">
    <div class="col-md-4">
        <asp:TextBox ID="txtSearch" runat="server" CssClass="form-control" Placeholder="Search by name, ID " />
    </div>
    <div class="col-md-2">
        <asp:Button ID="btnSearch" runat="server" Text="Search" CssClass="btn btn-info" OnClick="btnSearch_Click" />
    </div>
    <div class="col-md-2">
        <asp:Button ID="btnClearSearch" runat="server" Text="Clear" CssClass="btn btn-secondary" OnClick="btnClearSearch_Click" />
    </div>
</div>

<asp:GridView ID="gvReport" runat="server" CssClass="table table-striped table-bordered"
    AutoGenerateColumns="False" AllowCustomPaging="True" AllowPaging="True" PageSize="10" OnPageIndexChanging="gvReport_PageIndexChanging">
    <Columns>
        <asp:BoundField DataField="Employee_Id" HeaderText="Employee ID" />
        <asp:BoundField DataField="Employee_Name" HeaderText="Name" />
        <asp:BoundField DataField="Employee_Designation" HeaderText="Designation" />
        <asp:BoundField DataField="Basic_Salary" HeaderText="Basic Salary" DataFormatString="{0:C}" />
        <asp:BoundField DataField="Allowance" HeaderText="Allowance" DataFormatString="{0:C}" />
        <asp:BoundField DataField="Gross_Salary" HeaderText="Gross Salary" DataFormatString="{0:C}" />
        <asp:BoundField DataField="Deduction" HeaderText="Deduction" DataFormatString="{0:C}" />
        <asp:BoundField DataField="NetPay" HeaderText="Net Pay" DataFormatString="{0:C}" />
    </Columns>
</asp:GridView>

            <div class="text-right mt-3">
                <asp:Button ID="btnExportCSV" runat="server" CssClass="btn btn-primary" Text="Export to CSV" OnClick="btnExportCSV_Click" />
            </div>
        </div>
    </form>
</body>
</html>
