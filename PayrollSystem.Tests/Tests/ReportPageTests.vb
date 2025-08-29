Imports NUnit.Framework

<TestFixture>
Public Class ReportPageTests

    <Test>
    Public Sub Test_BindReports_JoinEmployeeAndReport_ReturnsExpectedView()
        Dim employees = New List(Of Employee) From {
            New Employee With {.Id = 1, .Employee_Id = "E001", .Employee_Name = "Alice", .Employee_Designation = "Manager", .Basic_Salary = 60000}
        }

        Dim reports = New List(Of PayrollReport) From {
            New PayrollReport With {.Id = 1, .EmpId = 1, .Allowance = 12000, .Gross_Salary = 72000, .Deduction = 2000, .NetPay = 70000}
        }

        Dim result = (From emp In employees
                      Join rep In reports On emp.Id Equals rep.EmpId
                      Select New PayrollReportView With {
                          .Employee_Id = emp.Employee_Id,
                          .Employee_Name = emp.Employee_Name,
                          .Employee_Designation = emp.Employee_Designation,
                          .Basic_Salary = emp.Basic_Salary,
                          .Allowance = rep.Allowance,
                          .Gross_Salary = rep.Gross_Salary,
                          .Deduction = rep.Deduction,
                          .NetPay = rep.NetPay
                      }).ToList()

        Assert.AreEqual(1, result.Count)
        Assert.AreEqual("E001", result(0).Employee_Id)
        Assert.AreEqual(70000, result(0).NetPay)
    End Sub

End Class
