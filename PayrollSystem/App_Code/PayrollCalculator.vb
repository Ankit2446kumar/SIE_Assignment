Namespace PayrollSystem
    Public Class PayrollCalculator
        Public Shared Function CalculateNetPay(emp As Employee) As Decimal
            Dim allowance = emp.BasicSalary * 0.2D
            Dim grossSalary = emp.BasicSalary + allowance
            Dim deduction = (emp.LeavesTaken * emp.BasicSalary) / emp.WorkingDaysInMonth
            Dim netPay = grossSalary - deduction
            Return Math.Round(netPay, 2)
        End Function
    End Class
End Namespace

