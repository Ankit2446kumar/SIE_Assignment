Imports NUnit.Framework
Imports PayrollSystem
Imports System.ComponentModel.DataAnnotations
Imports System.Reflection

<TestFixture>
Public Class ModelTests

    <Test>
    Public Sub Test_EmployeePropertiesSetCorrectly_ReturnsExpectedValues()
        Dim emp As New Employee With {
            .Employee_Id = "E001",
            .Employee_Name = "Alice",
            .Employee_Designation = "Analyst",
            .Basic_Salary = 50000D,
            .Leaves_Taken = 2,
            .Working_Days_In_Month = 30
        }

        Assert.AreEqual("E001", emp.Employee_Id)
        Assert.AreEqual("Alice", emp.Employee_Name)
        Assert.AreEqual("Analyst", emp.Employee_Designation)
        Assert.AreEqual(50000D, emp.Basic_Salary)
        Assert.AreEqual(2, emp.Leaves_Taken)
        Assert.AreEqual(30, emp.Working_Days_In_Month)
    End Sub

    <Test>
    Public Sub Test_Employee_IdProperty_HasKeyAttribute()
        Dim attr = GetType(Employee).GetProperty("Id").GetCustomAttribute(Of KeyAttribute)()
        Assert.IsNotNull(attr)
    End Sub

    <Test>
    Public Sub Test_PayrollReportPropertiesSetCorrectly_ReturnsExpectedValues()
        Dim rpt As New PayrollReport With {
            .EmpId = "1",
            .Allowance = 10000,
            .Gross_Salary = 60000,
            .Deduction = 2000,
            .NetPay = 58000
        }

        Assert.AreEqual("1", rpt.EmpId)
        Assert.AreEqual(10000, rpt.Allowance)
        Assert.AreEqual(60000, rpt.Gross_Salary)
        Assert.AreEqual(2000, rpt.Deduction)
        Assert.AreEqual(58000, rpt.NetPay)
    End Sub

    <Test>
    Public Sub Test_PayrollReport_IdProperty_HasKeyAttribute()
        Dim attr = GetType(PayrollReport).GetProperty("Id").GetCustomAttribute(Of KeyAttribute)()
        Assert.IsNotNull(attr)
    End Sub

End Class
