Imports System.ComponentModel.DataAnnotations

Public Class Employee
    <Key>
    Public Property Id As Integer

    Public Property Employee_Id As String       ' Store IDs like E001, E002, etc.
    Public Property Employee_Name As String
    Public Property Employee_Designation As String
    Public Property Basic_Salary As Decimal
    Public Property Leaves_Taken As Integer
    Public Property Working_Days_In_Month As Integer
End Class

