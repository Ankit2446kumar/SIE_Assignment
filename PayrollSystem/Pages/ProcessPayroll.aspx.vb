'Imports PayrollSystem
'Imports System.ComponentModel.DataAnnotations


Partial Class ProcessPayroll
    Inherits System.Web.UI.Page

    Protected Sub btnProcess_Click(sender As Object, e As EventArgs)
        Dim oReportsList As New List(Of PayrollReport)()

        Using db As New PayrollDbContext()
            Dim oEmployeesList = db.Employees.ToList()

            For Each emp In oEmployeesList
                Dim allowance = emp.Basic_Salary * 0.2D
                Dim grossSalary = emp.Basic_Salary + allowance
                Dim deduction = (emp.Leaves_Taken * emp.Basic_Salary) / emp.Working_Days_In_Month
                Dim netPay = grossSalary - deduction

                Dim existingReport = db.PayrollReports.SingleOrDefault(Function(r) r.EmpId = emp.Id)
                If existingReport Is Nothing Then
                    Dim report = New PayrollReport() With {
                        .EmpId = emp.Id,
                        .Allowance = allowance,
                        .Gross_Salary = grossSalary,
                        .Deduction = deduction,
                        .NetPay = netPay
                    }
                    db.PayrollReports.Add(report)
                Else
                    existingReport.Allowance = allowance
                    existingReport.Gross_Salary = grossSalary
                    existingReport.Deduction = deduction
                    existingReport.NetPay = netPay
                    db.Entry(existingReport).State = System.Data.Entity.EntityState.Modified
                End If
            Next
            db.SaveChanges()

            oReportsList = db.PayrollReports.ToList()
        End Using

        gvPayroll.DataSource = oReportsList
        gvPayroll.DataBind()
    End Sub
End Class





'Public Class Employee
'    <Key>
'    Public Property Id As Integer

'    Public Property Employee_Id As String       ' Store IDs like E001, E002, etc.
'    Public Property Employee_Name As String
'    Public Property Employee_Designation As String
'    Public Property Basic_Salary As Decimal
'    Public Property Leaves_Taken As Integer
'    Public Property Working_Days_In_Month As Integer
'End Class

'Public Class PayrollReport
'    <Key>
'    Public Property Id As Integer
'    Public Property EmpId As String
'    Public Property Allowance As Decimal
'    Public Property Gross_Salary As Decimal
'    Public Property Deduction As Decimal
'    Public Property NetPay As Decimal

'End Class

' DTO for joined report data


'Public Class PayrollReportView
'    'Public Property Id As String
'    Public Property Employee_Id As String
'    Public Property Employee_Name As String
'    Public Property Employee_Designation As String
'    Public Property Basic_Salary As Decimal
'    Public Property Allowance As Decimal
'    Public Property Gross_Salary As Decimal
'    Public Property Deduction As Decimal
'    Public Property NetPay As Decimal
'End Class


