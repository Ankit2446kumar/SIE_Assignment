Imports System.Data
Imports PayrollSystem
Imports System
Imports System.Linq

Namespace Helper
    Public Class PayrollProcessor
        Public Shared Sub ProcessPayroll()
            Using db As New PayrollDbContext()
                Dim employees = db.Employees.ToList()

                For Each emp In employees
                    Dim allowance = emp.Basic_Salary * 0.2D
                    Dim grossSalary = emp.Basic_Salary + allowance
                    Dim deduction = (emp.Leaves_Taken * emp.Basic_Salary) / emp.Working_Days_In_Month
                    Dim netPay = grossSalary - deduction

                    Dim existingReport = db.PayrollReports.SingleOrDefault(Function(r) r.EmpId = emp.Id)

                    If existingReport Is Nothing Then
                        db.PayrollReports.Add(New PayrollSystem.PayrollReport With {
                            .EmpId = emp.Id,
                            .Allowance = allowance,
                            .Gross_Salary = grossSalary,
                            .Deduction = deduction,
                            .NetPay = netPay
                        })
                    Else
                        existingReport.Allowance = allowance
                        existingReport.Gross_Salary = grossSalary
                        existingReport.Deduction = deduction
                        existingReport.NetPay = netPay
                        db.Entry(existingReport).State = Entity.EntityState.Modified
                    End If
                Next

                db.SaveChanges()
            End Using
        End Sub
    End Class
End Namespace

