Imports System.Collections.Generic
Imports System.Data.Entity
Imports Moq
Imports NUnit.Framework
Imports PayrollSystem.Helper
Imports System.Linq

Namespace Helper
    Public Class PayrollProcessor

        ' Refactored to accept DbContext as a parameter for easier unit testing
        Public Shared Sub ProcessPayroll(db As PayrollDbContext)
            Dim employees = db.Employees.ToList()

            For Each emp In employees
                Dim allowance = emp.Basic_Salary * 0.2D
                Dim grossSalary = emp.Basic_Salary + allowance

                Dim deduction As Decimal
                If emp.Working_Days_In_Month > 0 Then
                    deduction = (emp.Leaves_Taken * emp.Basic_Salary) / emp.Working_Days_In_Month
                Else
                    deduction = 0D ' Avoid division by zero
                End If

                Dim netPay = grossSalary - deduction

                Dim existingReport = db.PayrollReports.SingleOrDefault(Function(r) r.EmpId = emp.Id)

                If existingReport Is Nothing Then
                    db.PayrollReports.Add(New PayrollReport With {
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
        End Sub
    End Class
End Namespace
