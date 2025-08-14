Imports PayrollSystem

Partial Class ProcessPayroll
        Inherits System.Web.UI.Page

        Protected Sub btnProcess_Click(sender As Object, e As EventArgs)
        Dim employees As List(Of PayrollSystem.Employee) = CType(Session("Employees"), List(Of PayrollSystem.Employee))
        Dim reports As New List(Of PayrollSystem.PayrollReport)()

        For Each Emp In employees
            Dim allowance = Emp.BasicSalary * 0.2D
            Dim grossSalary = Emp.BasicSalary + allowance
            Dim deduction = (Emp.LeavesTaken * Emp.BasicSalary) / Emp.WorkingDaysInMonth
            Dim netPay = grossSalary - deduction

            reports.Add(New PayrollSystem.PayrollReport() With {
                .EmployeeId = Emp.Id,
                .EmployeeName = Emp.Name,
                .Designation = Emp.Designation,
                .BasicSalary = Emp.BasicSalary,
                .Allowance = allowance,
                .GrossSalary = grossSalary,
                .Deduction = deduction,
                .NetPay = netPay
            })
        Next

        Session("Reports") = reports

        gvPayroll.DataSource = reports
        gvPayroll.DataBind()
    End Sub
    End Class




Public Class Employee
    Public Property Id As Integer
    Public Property Name As String
    Public Property Designation As String
    Public Property BasicSalary As Decimal
    Public Property LeavesTaken As Integer
    Public Property WorkingDaysInMonth As Integer
End Class
Public Class PayrollReport
        Public Property EmployeeId As Integer
        Public Property EmployeeName As String
        Public Property Designation As String
        Public Property BasicSalary As Decimal
        Public Property Allowance As Decimal
        Public Property GrossSalary As Decimal
        Public Property Deduction As Decimal
        Public Property NetPay As Decimal
    End Class

