Imports System.Data.Entity

Public Class PayrollDbContext
    Inherits DbContext

    Public Sub New()
        MyBase.New("name=PayrollDbConnection")
    End Sub

    Public Overridable Property Employees As DbSet(Of Employee)
    Public Overridable Property PayrollReports As DbSet(Of PayrollReport)
End Class
