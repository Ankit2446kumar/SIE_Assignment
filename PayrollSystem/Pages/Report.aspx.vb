Imports System.Text

Partial Class Report
    Inherits System.Web.UI.Page

    Protected Sub btnSearch_Click(sender As Object, e As EventArgs)
        gvReport.PageIndex = 0 ' Reset to first page when searching
        Logger.LogInfo("Search triggered with input: " & txtSearch.Text.Trim()) ' 🟡 Log search input
        BindReports()
    End Sub

    Protected Sub btnClearSearch_Click(sender As Object, e As EventArgs)
        txtSearch.Text = ""
        gvReport.PageIndex = 0
        Logger.LogInfo("Search cleared by user.") ' 🟡 Log clear action
        BindReports()
    End Sub

    Protected Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load
        If Not IsPostBack Then
            Try
                Logger.LogInfo("Page_Load called on Report.aspx") ' 🟡 Log page load
                Helper.PayrollProcessor.ProcessPayroll()
                Logger.LogInfo("Payroll processing completed on page load.") ' 🟡 Log payroll success
            Catch ex As Exception
                Logger.LogError("Error during payroll processing: " & ex.Message) ' 🟡 Log payroll error
            End Try

            BindReports()
        End If
    End Sub

    Protected Sub gvReport_PageIndexChanging(sender As Object, e As GridViewPageEventArgs)
        Logger.LogInfo("Changing report page to index: " & e.NewPageIndex) ' 🟡 Log pagination
        gvReport.PageIndex = e.NewPageIndex
        BindReports(e.NewPageIndex)
    End Sub


    Public Sub BindReports(Optional ByVal nPageIndex As Integer = 0)
        Try
            Using oDb As New PayrollDbContext()
                Dim nPageSize As Integer = gvReport.PageSize
                Dim sSearchTerm As String = txtSearch.Text.Trim().ToLower()

                Dim oQuery = From oEmp In oDb.Employees
                             Join oPayroll In oDb.PayrollReports On oEmp.Id Equals oPayroll.EmpId
                             Select New PayrollReportView With {
                                 .Employee_Id = oEmp.Employee_Id,
                                 .Employee_Name = oEmp.Employee_Name,
                                 .Employee_Designation = oEmp.Employee_Designation,
                                 .Basic_Salary = oEmp.Basic_Salary,
                                 .Allowance = oPayroll.Allowance,
                                 .Gross_Salary = oPayroll.Gross_Salary,
                                 .Deduction = oPayroll.Deduction,
                                 .NetPay = oPayroll.NetPay
                             }
                If Not String.IsNullOrWhiteSpace(sSearchTerm) Then
                    oQuery = oQuery.Where(Function(r) r.Employee_Name.ToLower().Contains(sSearchTerm) _
                                            Or r.Employee_Id.ToLower().Contains(sSearchTerm))
                End If

                Dim nTotalRecords As Integer = oQuery.Count()

                Dim oPagedReports = oQuery.OrderBy(Function(r) r.Employee_Name) _
                                        .Skip(nPageIndex * nPageSize) _
                                        .Take(nPageSize) _
                                        .ToList()

                gvReport.PageIndex = nPageIndex
                gvReport.VirtualItemCount = nTotalRecords
                gvReport.DataSource = oPagedReports
                gvReport.DataBind()

                Logger.LogInfo($"Reports bound successfully. PageIndex: {nPageIndex}, SearchTerm: {sSearchTerm}, TotalRecords: {nTotalRecords}") ' 🟡 Log binding
            End Using
        Catch ex As Exception
            Logger.LogError("Error in BindReports: " & ex.Message) ' 🟡 Log bind error
        End Try
    End Sub
    Protected Sub btnExportCSV_Click(sender As Object, e As EventArgs)
        Try
            Using oDb As New PayrollDbContext()
                Dim oReports = (From oEmp In oDb.Employees
                                Join oPayroll In oDb.PayrollReports On oEmp.Id Equals oPayroll.EmpId
                                Select New PayrollReportView With {
                                    .Employee_Id = oEmp.Employee_Id,
                                    .Employee_Name = oEmp.Employee_Name,
                                    .Employee_Designation = oEmp.Employee_Designation,
                                    .Basic_Salary = oEmp.Basic_Salary,
                                    .Allowance = oPayroll.Allowance,
                                    .Gross_Salary = oPayroll.Gross_Salary,
                                    .Deduction = oPayroll.Deduction,
                                    .NetPay = oPayroll.NetPay
                                }).ToList()

                Dim sb As New StringBuilder()
                sb.AppendLine("Employee_Id,Name,Designation,BasicSalary,Allowance,GrossSalary,Deduction,NetPay")

                For Each oReport In oReports
                    sb.AppendLine($"{oReport.Employee_Id},{oReport.Employee_Name},{oReport.Employee_Designation},{oReport.Basic_Salary},{oReport.Allowance},{oReport.Gross_Salary},{oReport.Deduction},{oReport.NetPay}")
                Next

                Logger.LogInfo("CSV export triggered with " & oReports.Count & " records.") ' 🟡 Log export

                Response.Clear()
                Response.ContentType = "text/csv"
                Response.AddHeader("Content-Disposition", "attachment;filename=PayrollReport.csv")
                Response.Write(sb.ToString())
                Response.End()
            End Using
        Catch ex As Exception
            Logger.LogError("Error in ExportCSV: " & ex.Message) ' 🟡 Log export error
        End Try
    End Sub
End Class
