<%@ Page Language="VB" AutoEventWireup="true" CodeBehind="Upload.aspx.vb" Inherits="PayrollSystem.Upload" %>

<!DOCTYPE html>
<html>
<head>
    <title>Upload Employees</title>
    <link href="https://stackpath.bootstrapcdn.com/bootstrap/4.5.2/css/bootstrap.min.css" rel="stylesheet" />
</head>
<body>
    <div class="container mt-4">
        <h2>Upload Employees</h2>
        <form id="form1" runat="server">
            
            <!-- CSV Upload Section -->
            <div class="row mb-4">
                <div class="col-md-12">
                    <asp:FileUpload ID="fileUpload" runat="server" CssClass="form-control-file" />
                    <asp:Button ID="btnUploadCSV" runat="server" CssClass="btn btn-primary mt-2" Text="Upload CSV" OnClick="btnUploadCSV_Click" />
                </div>
            </div>

            <!-- Manual Entry Section -->
            <h3>Manual Entry</h3>
            <div class="row">
                <div class="col-md-6">
                    <div class="form-group">
                        <asp:Label ID="lblEId" runat="server">Employee ID:</asp:Label>
                        <asp:TextBox ID="txtId" runat="server" CssClass="form-control" Placeholder="E001, E002, etc."></asp:TextBox>
                    </div>
                    
                    <div class="form-group">
                        <asp:Label ID="lblName" runat="server">Employee Name:</asp:Label>
                        <asp:TextBox ID="txtName" runat="server" CssClass="form-control" Placeholder="John Doe"></asp:TextBox>
                    </div>
                    
                    <div class="form-group">
                        <asp:Label ID="lblDesignation" runat="server">Employee Designation:</asp:Label>
                        <asp:TextBox ID="txtDesignation" runat="server" CssClass="form-control" Placeholder="Software Engineer"></asp:TextBox>
                    </div>
                </div>
                
                <div class="col-md-6">
                    <div class="form-group">
                        <asp:Label ID="lblBasicSalary" runat="server">Basic Salary:</asp:Label>
                        <asp:TextBox ID="txtBasicSalary" runat="server" CssClass="form-control" Placeholder="60000"></asp:TextBox>
                    </div>
                    
                    <div class="form-group">
                        <asp:Label ID="lblLeaves" runat="server">Leaves Taken:</asp:Label>
                        <asp:TextBox ID="txtLeaves" runat="server" CssClass="form-control" Placeholder="2"></asp:TextBox>
                    </div>
                    
                    <div class="form-group">
                        <asp:Label ID="lblWorkingDays" runat="server">Working Days in Month:</asp:Label>
                        <asp:TextBox ID="txtWorkingDays" runat="server" CssClass="form-control" Placeholder="30"></asp:TextBox>
                    </div>
                </div>
            </div>

            <asp:Label ID="lblError" runat="server" ForeColor="Red"></asp:Label> <br /><br />
            <asp:HiddenField ID="hdnSelectedEmployeeId" runat="server" />
            
            <div class="mb-3">
                <asp:Button ID="btnAddEmployee" runat="server" Text="Add Employee" CssClass="btn btn-primary mr-2" OnClick="btnAddEmployee_Click" />
                <asp:Button ID="btnUpdateEmployee" runat="server" Text="Update Employee" CssClass="btn btn-warning mr-2" OnClick="btnUpdateEmployee_Click" />
                <asp:Button ID="btnClearForm" runat="server" Text="Clear Form" CssClass="btn btn-secondary mr-2" OnClick="btnClearForm_Click" />
                <asp:Button ID="btnGoToReport" runat="server" CssClass="btn btn-primary"  Text="Go to Reports"  OnClick="btnGoToReport_Click" />  
            </div>

            <!-- Filter/Search Section -->
<div class="row mb-3">
    <div class="col-md-4">
        <asp:TextBox ID="txtSearch" runat="server" CssClass="form-control" Placeholder="Search by name, ID" />
    </div>
    <div class="col-md-2">
        <asp:Button ID="btnSearch" runat="server" Text="Search" CssClass="btn btn-info" OnClick="btnSearch_Click" />
    </div>
    <div class="col-md-2">
        <asp:Button ID="btnClearSearch" runat="server" Text="Clear" CssClass="btn btn-secondary" OnClick="btnClearSearch_Click" />
    </div>
</div>


            <!-- Employees Grid -->
           <asp:GridView ID="gvEmployees" runat="server"
              AutoGenerateColumns="False"
              AllowPaging="True"
              PageSize="10"
              CssClass="table table-striped table-bordered"
              OnRowCommand="gvEmployees_RowCommand"
              OnPageIndexChanging="gvEmployees_PageIndexChanging">
    <Columns>
        <asp:TemplateField HeaderText="Employee ID">
            <ItemTemplate>
                <asp:LinkButton ID="lnkEmployeeId" 
                                runat="server" 
                                Text='<%# Eval("Employee_Id") %>' 
                                CommandName="SelectEmployee" 
                                CommandArgument='<%# Eval("Id") %>' 
                                CssClass="text-primary" />
            </ItemTemplate>
        </asp:TemplateField>

        <asp:BoundField DataField="Employee_Name" HeaderText="Name" />
        <asp:BoundField DataField="Employee_Designation" HeaderText="Designation" />
        <asp:BoundField DataField="Basic_Salary" HeaderText="Basic Salary" DataFormatString="{0:C}" />
        <asp:BoundField DataField="Leaves_Taken" HeaderText="Leaves Taken" />
        <asp:BoundField DataField="Working_Days_In_Month" HeaderText="Working Days" />
    </Columns>
</asp:GridView>


        </form>
    </div>
</body>
</html>
