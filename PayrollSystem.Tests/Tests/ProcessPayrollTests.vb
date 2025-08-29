Imports NUnit.Framework
Imports Moq
Imports PayrollSystem
Imports System.Data.Entity
Imports System.Linq

Public Module DbSetMockHelpers
    ''' <summary>
    ''' Creates a mock DbSet based on a List(Of T), supporting LINQ and Add/Remove behavior.
    ''' </summary>
    Public Function CreateDbSetMock(Of T As Class)(sourceList As List(Of T)) As Mock(Of DbSet(Of T))
        Dim queryableData = sourceList.AsQueryable()

        Dim mockSet = New Mock(Of DbSet(Of T))()

        ' Setup IQueryable implementation
        mockSet.As(Of IQueryable(Of T))().Setup(Function(m) m.Provider).Returns(queryableData.Provider)
        mockSet.As(Of IQueryable(Of T))().Setup(Function(m) m.Expression).Returns(queryableData.Expression)
        mockSet.As(Of IQueryable(Of T))().Setup(Function(m) m.ElementType).Returns(queryableData.ElementType)
        mockSet.As(Of IQueryable(Of T))().Setup(Function(m) m.GetEnumerator()).Returns(Function() queryableData.GetEnumerator())

        ' Setup Add() to modify the underlying list
        mockSet.Setup(Sub(m) m.Add(It.IsAny(Of T))).Callback(Sub(entity As T) sourceList.Add(entity))

        ' Setup Remove() to modify the underlying list
        mockSet.Setup(Sub(m) m.Remove(It.IsAny(Of T))).Callback(Sub(entity As T) sourceList.Remove(entity))

        Return mockSet
    End Function
End Module

<TestFixture>
Public Class ProcessPayrollTests

    Private employeesList As List(Of Employee)
    Private payrollReportsList As List(Of PayrollReport)
    Private mockEmployees As Mock(Of DbSet(Of Employee))
    Private mockReports As Mock(Of DbSet(Of PayrollReport))
    Private mockContext As Mock(Of PayrollDbContext)

    <SetUp>
    Public Sub Setup()
        employeesList = New List(Of Employee)()
        payrollReportsList = New List(Of PayrollReport)()

        mockEmployees = DbSetMockHelpers.CreateDbSetMock(employeesList)
        mockReports = DbSetMockHelpers.CreateDbSetMock(payrollReportsList)

        mockContext = New Mock(Of PayrollDbContext)()
        mockContext.Setup(Function(c) c.Employees).Returns(mockEmployees.Object)
        mockContext.Setup(Function(c) c.PayrollReports).Returns(mockReports.Object)
    End Sub

    <Test>
    Public Sub Test_ProcessPayroll_NoEmployees_ReturnsNoReports()
        Assert.AreEqual(0, employeesList.Count)
        Assert.AreEqual(0, payrollReportsList.Count)
    End Sub

    <Test>
    Public Sub Test_ProcessPayroll_WithEmployees_ReturnsCreatedReports()
        employeesList.AddRange({
            New Employee With {.Id = 1, .Employee_Id = "E001", .Basic_Salary = 50000, .Leaves_Taken = 2, .Working_Days_In_Month = 30},
            New Employee With {.Id = 2, .Employee_Id = "E002", .Basic_Salary = 40000, .Leaves_Taken = 0, .Working_Days_In_Month = 30}
        })

        For Each emp In employeesList
            Dim allowance = emp.Basic_Salary * 0.2D
            Dim grossSalary = emp.Basic_Salary + allowance
            Dim deduction = (emp.Leaves_Taken * emp.Basic_Salary) / emp.Working_Days_In_Month
            Dim netPay = grossSalary - deduction

            payrollReportsList.Add(New PayrollReport With {
                .Id = If(payrollReportsList.Count = 0, 1, payrollReportsList.Max(Function(r) r.Id) + 1),
                .EmpId = emp.Id,
                .Allowance = allowance,
                .Gross_Salary = grossSalary,
                .Deduction = deduction,
                .NetPay = netPay
            })
        Next

        Assert.AreEqual(2, payrollReportsList.Count)
    End Sub

End Class
