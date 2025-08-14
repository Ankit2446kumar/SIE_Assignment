Imports System.IO
Imports System.Collections.Generic
Imports PayrollSystem

Partial Class Upload
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load
        If Not IsPostBack Then
            Dim employees As List(Of PayrollSystem.Employee) = TryCast(Session("Employees"), List(Of PayrollSystem.Employee))
            If employees IsNot Nothing Then
                gvEmployees.DataSource = employees
                gvEmployees.DataBind()
            End If
        End If
    End Sub


    Protected Sub btnUploadCSV_Click(sender As Object, e As EventArgs)
        If fileUpload IsNot Nothing AndAlso fileUpload.HasFile Then
            Dim employees As List(Of PayrollSystem.Employee) = TryCast(Session("Employees"), List(Of PayrollSystem.Employee))
            If employees Is Nothing Then
                employees = New List(Of PayrollSystem.Employee)()
            End If
            Using reader As New StreamReader(fileUpload.PostedFile.InputStream)
                While Not reader.EndOfStream
                    Dim line = reader.ReadLine()
                    If String.IsNullOrWhiteSpace(line) Then
                        Continue While ' Skip completely blank lines
                    End If
                    Dim values = line.Split(","c)
                    ' Skip header rows or lines with wrong number of fields
                    If values.Length <> 6 OrElse Not Integer.TryParse(values(0), Nothing) Then
                        Continue While
                    End If

                    Dim emp As New PayrollSystem.Employee()
                    Try
                        emp.Id = Integer.Parse(values(0))
                        emp.Name = values(1)
                        emp.Designation = values(2)
                        emp.BasicSalary = Decimal.Parse(values(3))
                        emp.LeavesTaken = Integer.Parse(values(4))
                        emp.WorkingDaysInMonth = Integer.Parse(values(5))
                        employees.Add(emp)
                    Catch ex As Exception
                        ' Optionally log or collect bad rows for admin review
                        ' Continue While will skip this row
                    End Try
                End While
            End Using

            Session("Employees") = employees
            gvEmployees.DataSource = employees
            gvEmployees.DataBind()
        End If
    End Sub

    Protected Sub btnAddEmployee_Click(sender As Object, e As EventArgs)
        Dim emp As New PayrollSystem.Employee() With {
            .Id = Integer.Parse(txtId.Text),
            .Name = txtName.Text,
            .Designation = txtDesignation.Text,
            .BasicSalary = Decimal.Parse(txtBasicSalary.Text),
            .LeavesTaken = Integer.Parse(txtLeaves.Text),
            .WorkingDaysInMonth = Integer.Parse(txtWorkingDays.Text)
        }

        Dim employees As List(Of PayrollSystem.Employee) = TryCast(Session("Employees"), List(Of PayrollSystem.Employee))
        If employees Is Nothing Then
            employees = New List(Of PayrollSystem.Employee)()
        End If
        employees.Add(emp)
        Session("Employees") = employees
        gvEmployees.DataSource = employees
        gvEmployees.DataBind()
    End Sub
End Class

