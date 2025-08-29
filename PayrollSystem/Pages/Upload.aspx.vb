Imports System.IO
Imports NUnit.Framework.Internal

Partial Class Upload
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load
        If Not IsPostBack Then
            Logger.LogInfo("Page loaded for the first time.")
            BindEmployees()
        End If
    End Sub

    Protected Sub btnSearch_Click(sender As Object, e As EventArgs)
        Logger.LogInfo($"Search initiated. Filter: '{txtSearch.Text.Trim()}'")
        gvEmployees.PageIndex = 0 ' Reset to first page when searching
        BindEmployees()
    End Sub

    Protected Sub btnClearSearch_Click(sender As Object, e As EventArgs)
        Logger.LogInfo("Search cleared.")
        txtSearch.Text = ""
        gvEmployees.PageIndex = 0
        BindEmployees()
    End Sub

    Protected Sub gvEmployees_PageIndexChanging(sender As Object, e As GridViewPageEventArgs)
        Logger.LogInfo($"GridView page changing to index: {e.NewPageIndex}")
        gvEmployees.PageIndex = e.NewPageIndex
        BindEmployees()
    End Sub

    Private Sub BindEmployees()
        Using oDb As New PayrollDbContext()
            Dim oEmployees = oDb.Employees.AsQueryable()

            Dim sFilter As String = txtSearch.Text.Trim().ToLower()
            If Not String.IsNullOrEmpty(sFilter) Then
                oEmployees = oEmployees.Where(Function(oEmp) oEmp.Employee_Id.ToLower().Contains(sFilter) _
                                              OrElse oEmp.Employee_Name.ToLower().Contains(sFilter) _
                                              OrElse oEmp.Employee_Designation.ToLower().Contains(sFilter))
                Logger.LogInfo($"Employees filtered by '{sFilter}'. Count: {oEmployees.Count()}")
            Else
                Logger.LogInfo("Employees bound without filter.")
            End If
            gvEmployees.DataSource = oEmployees.ToList()
            gvEmployees.DataBind()
        End Using
    End Sub

    Protected Sub btnUploadCSV_Click(sender As Object, e As EventArgs)
        If fileUpload.HasFile Then
            Logger.LogInfo($"CSV Upload started. Filename: {fileUpload.FileName}")
            Dim oEmployeesToAdd As New List(Of Employee)()
            Dim sDuplicateIds As New List(Of String)()

            Using oReader As New StreamReader(fileUpload.PostedFile.InputStream)
                While Not oReader.EndOfStream
                    Dim sLine As String = oReader.ReadLine()
                    If String.IsNullOrWhiteSpace(sLine) Then Continue While

                    Dim sValues = sLine.Split(","c)
                    If sValues.Length <> 6 Then
                        Logger.LogError($"CSV row skipped due to invalid column count: '{sLine}'")
                        Continue While
                    End If

                    Dim sEmpId As String = sValues(0).Trim()

                    Try
                        Dim oEmp As New Employee() With {
                            .Employee_Id = sEmpId,
                            .Employee_Name = sValues(1).Trim(),
                            .Employee_Designation = sValues(2).Trim(),
                            .Basic_Salary = Decimal.Parse(sValues(3)),
                            .Leaves_Taken = Integer.Parse(sValues(4)),
                            .Working_Days_In_Month = Integer.Parse(sValues(5))
                        }

                        Using oDb As New PayrollDbContext()
                            Dim bExists As Boolean = oDb.Employees.Any(Function(x) x.Employee_Id = oEmp.Employee_Id)
                            If bExists Then
                                sDuplicateIds.Add(oEmp.Employee_Id)
                                Logger.LogInfo($"Duplicate Employee ID found in CSV: {oEmp.Employee_Id}")
                            Else
                                oEmployeesToAdd.Add(oEmp)
                            End If
                        End Using
                    Catch ex As Exception
                        Logger.LogError($"Error parsing CSV row: '{sLine}'. Exception: {ex.Message}")
                    End Try
                End While
            End Using

            Using oDb As New PayrollDbContext()
                oDb.Employees.AddRange(oEmployeesToAdd)
                oDb.SaveChanges()
            End Using

            Logger.LogInfo($"{oEmployeesToAdd.Count} employees added from CSV upload.")
            BindEmployees()

            If sDuplicateIds.Count > 0 Then
                lblError.Text = "These Employee IDs already exist and were not added: " & String.Join(", ", sDuplicateIds)
                lblError.CssClass = "alert alert-warning"
            Else
                lblError.Text = "CSV uploaded successfully."
                lblError.CssClass = "alert alert-success"
            End If
        Else
            Logger.LogInfo("CSV Upload attempted but no file was selected.")
        End If
    End Sub

    Protected Sub btnAddEmployee_Click(sender As Object, e As EventArgs)
        Dim sEmpId As String = txtId.Text.Trim()
        Logger.LogInfo($"AddEmployee clicked for Employee ID: {sEmpId}")

        If String.IsNullOrWhiteSpace(sEmpId) Then
            Logger.LogError("Attempted to add employee with empty Employee ID.")
            lblError.Text = "Employee ID cannot be empty."
            lblError.CssClass = "alert alert-danger"
            Return
        End If

        If Not System.Text.RegularExpressions.Regex.IsMatch(sEmpId, "^E\d+$") Then
            Logger.LogError($"Invalid Employee ID format: {sEmpId}")
            lblError.Text = "Employee ID must be in format E001, E002, etc."
            lblError.CssClass = "alert alert-danger"
            Return
        End If

        Try
            Dim sEmpName As String = txtName.Text.Trim()
            Dim sDesignation As String = txtDesignation.Text.Trim()
            Dim nBasicSalary As Decimal = Decimal.Parse(txtBasicSalary.Text.Trim())
            Dim nLeavesTaken As Integer = Integer.Parse(txtLeaves.Text.Trim())
            Dim nWorkingDays As Integer = Integer.Parse(txtWorkingDays.Text.Trim())

            Using oDb As New PayrollDbContext()
                Dim oExistingEmp = oDb.Employees.FirstOrDefault(Function(x) x.Employee_Id = sEmpId)

                If oExistingEmp IsNot Nothing Then
                    If oExistingEmp.Employee_Name.Trim().ToLower() = sEmpName.ToLower() Then
                        oExistingEmp.Employee_Designation = sDesignation
                        oExistingEmp.Basic_Salary = nBasicSalary
                        oExistingEmp.Leaves_Taken = nLeavesTaken
                        oExistingEmp.Working_Days_In_Month = nWorkingDays

                        oDb.SaveChanges()

                        Logger.LogInfo($"Existing employee updated: {sEmpId}")
                        lblError.Text = "Existing employee updated successfully."
                        lblError.CssClass = "alert alert-success"
                    Else
                        Logger.LogWarning($"Employee ID '{sEmpId}' exists with a different name: '{oExistingEmp.Employee_Name}' (input name: {sEmpName})")
                        lblError.Text = $"Employee ID '{sEmpId}' already exists with a different name: '{oExistingEmp.Employee_Name}'."
                        lblError.CssClass = "alert alert-warning"
                        Return
                    End If
                Else
                    Dim oNewEmp As New Employee() With {
                        .Employee_Id = sEmpId,
                        .Employee_Name = sEmpName,
                        .Employee_Designation = sDesignation,
                        .Basic_Salary = nBasicSalary,
                        .Leaves_Taken = nLeavesTaken,
                        .Working_Days_In_Month = nWorkingDays
                    }

                    oDb.Employees.Add(oNewEmp)
                    oDb.SaveChanges()

                    Logger.LogInfo($"New employee added: {sEmpId}")
                    lblError.Text = "New employee added successfully."
                    lblError.CssClass = "alert alert-success"
                End If
            End Using

            ClearForm()
            BindEmployees()
        Catch ex As Exception
            Logger.LogError($"Error adding/updating employee {sEmpId}: {ex.Message}")
            lblError.Text = "Error adding/updating employee. Please check your input."
            lblError.CssClass = "alert alert-danger"
        End Try
    End Sub

    Protected Sub btnUpdateEmployee_Click(sender As Object, e As EventArgs)
        If String.IsNullOrEmpty(hdnSelectedEmployeeId.Value) Then
            Logger.LogWarning("UpdateEmployee clicked but no employee selected.")
            lblError.Text = "Please select an employee to update."
            lblError.CssClass = "alert alert-warning"
            Return
        End If

        Dim nEmpId As Integer
        If Not Integer.TryParse(hdnSelectedEmployeeId.Value, nEmpId) Then
            Logger.LogError("Invalid selected Employee ID for update.")
            lblError.Text = "Invalid selected Employee ID."
            lblError.CssClass = "alert alert-danger"
            Return
        End If

        Try
            Dim sInputEmployeeId As String = txtId.Text.Trim()
            Dim sInputName As String = txtName.Text.Trim()
            Dim sInputDesignation As String = txtDesignation.Text.Trim()
            Dim nInputSalary As Decimal = Decimal.Parse(txtBasicSalary.Text.Trim())
            Dim nInputLeaves As Integer = Integer.Parse(txtLeaves.Text.Trim())
            Dim nInputWorkingDays As Integer = Integer.Parse(txtWorkingDays.Text.Trim())

            Using oDb As New PayrollDbContext()
                Dim oEmp = oDb.Employees.Find(nEmpId)

                If oEmp IsNot Nothing Then
                    Dim oExistingEmpWithSameEmpId = oDb.Employees.FirstOrDefault(Function(x) x.Employee_Id = sInputEmployeeId AndAlso x.Id <> nEmpId)

                    If oExistingEmpWithSameEmpId IsNot Nothing Then
                        Logger.LogError($"Duplicate Employee ID on update: {sInputEmployeeId}")
                        lblError.Text = $"Employee ID '{sInputEmployeeId}' is already assigned to another employee."
                        lblError.CssClass = "alert alert-danger"
                        Return
                    End If

                    If oEmp.Employee_Id = sInputEmployeeId Then
                        If oEmp.Employee_Name.Trim().ToLower() <> sInputName.Trim().ToLower() Then
                            Logger.LogWarning($"Employee ID '{sInputEmployeeId}' update attempted with different name. Original: '{oEmp.Employee_Name}', Input: '{sInputName}'")
                            lblError.Text = $"Employee ID '{sInputEmployeeId}' exists with a different name ('{oEmp.Employee_Name}')."
                            lblError.CssClass = "alert alert-warning"
                            Return
                        End If
                    End If

                    oEmp.Employee_Id = sInputEmployeeId
                    oEmp.Employee_Name = sInputName
                    oEmp.Employee_Designation = sInputDesignation
                    oEmp.Basic_Salary = nInputSalary
                    oEmp.Leaves_Taken = nInputLeaves
                    oEmp.Working_Days_In_Month = nInputWorkingDays

                    oDb.SaveChanges()

                    Logger.LogInfo($"Employee updated: ID {sInputEmployeeId}")
                    lblError.Text = "Employee updated successfully."
                    lblError.CssClass = "alert alert-success"

                    ClearForm()
                    BindEmployees()
                Else
                    Logger.LogError($"Employee to update not found. Internal ID: {nEmpId}")
                    lblError.Text = "Employee not found."
                    lblError.CssClass = "alert alert-danger"
                End If
            End Using
        Catch ex As Exception
            Logger.LogError($"Error updating employee: {ex.Message}")
            lblError.Text = "Error updating employee. Please check your input."
            lblError.CssClass = "alert alert-danger"
        End Try
    End Sub

    Protected Sub gvEmployees_RowCommand(sender As Object, e As GridViewCommandEventArgs)
        If e.CommandName = "SelectEmployee" Then
            Dim sEmployeeIdString As String = e.CommandArgument.ToString()
            Dim nEmployeeId As Integer

            If Integer.TryParse(sEmployeeIdString, nEmployeeId) Then
                Using oDb As New PayrollDbContext()
                    Dim oEmp = oDb.Employees.Find(nEmployeeId)
                    If oEmp IsNot Nothing Then
                        hdnSelectedEmployeeId.Value = oEmp.Id.ToString()
                        txtId.Text = oEmp.Employee_Id
                        txtName.Text = oEmp.Employee_Name
                        txtDesignation.Text = oEmp.Employee_Designation
                        txtBasicSalary.Text = oEmp.Basic_Salary.ToString()
                        txtLeaves.Text = oEmp.Leaves_Taken.ToString()
                        txtWorkingDays.Text = oEmp.Working_Days_In_Month.ToString()

                        Logger.LogInfo($"Employee selected for editing: {oEmp.Employee_Id}")
                        lblError.Text = "Employee selected for editing."
                        lblError.CssClass = "alert alert-info"
                    Else
                        Logger.LogWarning($"Selected employee not found: Internal ID {nEmployeeId}")
                        lblError.Text = "Employee not found."
                        lblError.CssClass = "alert alert-warning"
                    End If
                End Using
            Else
                Logger.LogError("Invalid Employee ID in GridView command.")
                lblError.Text = "Invalid Employee ID."
                lblError.CssClass = "alert alert-danger"
            End If
        End If
    End Sub

    Protected Sub btnClearForm_Click(sender As Object, e As EventArgs)
        Logger.LogInfo("Form cleared.")
        ClearForm()
    End Sub

    Private Sub ClearForm()
        txtId.Text = ""
        txtName.Text = ""
        txtDesignation.Text = ""
        txtBasicSalary.Text = ""
        txtLeaves.Text = ""
        txtWorkingDays.Text = ""
        hdnSelectedEmployeeId.Value = ""
        lblError.Text = ""
    End Sub

    Protected Sub btnGoToReport_Click(sender As Object, e As EventArgs)
        Try
            Logger.LogInfo("Payroll processing started.")
            Helper.PayrollProcessor.ProcessPayroll()
            Logger.LogInfo("Payroll processing completed. Redirecting to Report.aspx")
            Response.Redirect("~/Pages/Report.aspx", False)
        Catch ex As Exception
            Logger.LogError("Error generating payroll: " & ex.Message)
            lblError.Text = "Error generating payroll: " & ex.Message
            lblError.CssClass = "alert alert-danger"
        End Try
    End Sub

End Class
