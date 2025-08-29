Imports System.ComponentModel.DataAnnotations

Public Class PayrollReport
    <Key>
    Public Property Id As Integer
    Public Property EmpId As String
    Public Property Allowance As Decimal
    Public Property Gross_Salary As Decimal
    Public Property Deduction As Decimal
    Public Property NetPay As Decimal
End Class

