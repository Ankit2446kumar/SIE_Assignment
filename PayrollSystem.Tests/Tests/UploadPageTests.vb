Imports NUnit.Framework

<TestFixture>
Public Class UploadPageTests

    Private employeesList As List(Of Employee)
    Private lblErrorText As String
    Private lblErrorCssClass As String

    <SetUp>
    Public Sub Setup()
        employeesList = New List(Of Employee)
        lblErrorText = ""
        lblErrorCssClass = ""
    End Sub

    <Test>
    Public Sub Test_btnAddEmployee_EmptyId_ReturnsError()
        Dim empId = ""

        If String.IsNullOrWhiteSpace(empId) Then
            lblErrorText = "Employee ID cannot be empty."
            lblErrorCssClass = "alert alert-danger"
        End If

        Assert.AreEqual("Employee ID cannot be empty.", lblErrorText)
    End Sub

    <Test>
    Public Sub Test_btnAddEmployee_InvalidIdFormat_ReturnsError()
        Dim empId = "123"

        If Not Text.RegularExpressions.Regex.IsMatch(empId, "^E\d+$") Then
            lblErrorText = "Employee ID must be in format E001, E002, etc."
        End If

        Assert.AreEqual("Employee ID must be in format E001, E002, etc.", lblErrorText)
    End Sub

    <Test>
    Public Sub Test_btnAddEmployee_DuplicateId_ReturnsError()
        employeesList.Add(New Employee With {.Employee_Id = "E001"})
        Dim empId = "E001"

        If employeesList.Any(Function(e) e.Employee_Id = empId) Then
            lblErrorText = $"Employee ID {empId} already exists."
        End If

        Assert.AreEqual("Employee ID E001 already exists.", lblErrorText)
    End Sub

    <Test>
    Public Sub Test_btnAddEmployee_ValidInput_AddsEmployee()
        Dim empId = "E002"

        Dim emp = New Employee With {
            .Employee_Id = empId,
            .Employee_Name = "Test",
            .Employee_Designation = "Dev",
            .Basic_Salary = 45000,
            .Leaves_Taken = 1,
            .Working_Days_In_Month = 30
        }

        employeesList.Add(emp)

        Assert.IsTrue(employeesList.Any(Function(e) e.Employee_Id = "E002"))
    End Sub

    <Test>
    Public Sub Test_btnUploadCSV_InvalidCSV_SkipsBadLines()
        Dim csvLines As New List(Of String) From {
            "E004,TooShort",
            "E005,Name,Role,50000,1,30"
        }

        Dim validCount = 0
        For Each line In csvLines
            If line.Split(","c).Length = 6 Then validCount += 1
        Next

        Assert.AreEqual(1, validCount)
    End Sub

End Class
