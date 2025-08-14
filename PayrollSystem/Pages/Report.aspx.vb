Imports System.IO
Imports System.Text
Imports PayrollSystem

Partial Class Report
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load
        Dim reports As List(Of PayrollSystem.PayrollReport) = TryCast(Session("Reports"), List(Of PayrollSystem.PayrollReport))
        gvReport.DataSource = reports
        gvReport.DataBind()
    End Sub

    Protected Sub btnExportCSV_Click(sender As Object, e As EventArgs)
        Dim reports As List(Of PayrollSystem.PayrollReport) = CType(Session("Reports"), List(Of PayrollSystem.PayrollReport))
        Dim sb As New StringBuilder()
        sb.AppendLine("ID,Name,Designation,BasicSalary,Allowance,GrossSalary,Deduction,NetPay")

        For Each r In reports
            sb.AppendLine($"{r.EmployeeId},{r.EmployeeName},{r.Designation},{r.BasicSalary},{r.Allowance},{r.GrossSalary},{r.Deduction},{r.NetPay}")
        Next

        Response.Clear()
        Response.ContentType = "text/csv"
        Response.AddHeader("Content-Disposition", "attachment;filename=PayrollReport.csv")
        Response.Write(sb.ToString())
        Response.End()
    End Sub
End Class

