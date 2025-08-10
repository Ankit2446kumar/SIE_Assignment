Module Module1
    Sub Main()
        Try
            Console.WriteLine("=== Create a New Employee ===")

            ' Input Name
            Console.Write("Enter Name: ")
            Dim name As String = Console.ReadLine()

            ' Input Age
            Console.Write("Enter Age: ")
            Dim ageInput As String = Console.ReadLine()
            Dim age As Integer
            If Not Integer.TryParse(ageInput, age) OrElse age < 0 Then
                Throw New ArgumentException("Invalid age. Age must be a non-negative number.")
            End If

            ' Input Employee ID
            Console.Write("Enter Employee ID: ")
            Dim empId As String = Console.ReadLine()

            ' Input Salary
            Console.Write("Enter Salary: ")
            Dim salaryInput As String = Console.ReadLine()
            Dim salary As Decimal
            If Not Decimal.TryParse(salaryInput, salary) OrElse salary < 0 Then
                Throw New ArgumentException("Invalid salary. Salary must be a non-negative number.")
            End If

            ' Create Employee object
            Dim emp As New Employee(name, age, empId, salary)

            Console.WriteLine()
            Console.WriteLine("=== Employee Information ===")
            emp.DisplayInfo()

        Catch ex As Exception
            Console.WriteLine("Error: " & ex.Message)
        End Try

        Console.WriteLine()
        Console.WriteLine("Press any key to exit...")
        Console.ReadKey()
    End Sub
End Module

' Base Class: Person
Public Class Person
    Public Property Name As String
    Public Property Age As Integer

    Public Sub New(name As String, age As Integer)
        Me.Name = name
        Me.Age = age
    End Sub

    Public Overridable Sub DisplayInfo()
        Console.WriteLine("Name: " & Name)
        Console.WriteLine("Age: " & Age)
    End Sub
End Class

' Derived Class: Employee inherits Person
Public Class Employee
    Inherits Person

    Public Property EmployeeID As String
    Public Property Salary As Decimal

    Public Sub New(name As String, age As Integer, employeeId As String, salary As Decimal)
        MyBase.New(name, age)
        Me.EmployeeID = employeeId
        Me.Salary = salary
    End Sub

    Public Overrides Sub DisplayInfo()
        MyBase.DisplayInfo()
        Console.WriteLine("Employee ID: " & EmployeeID)
        Console.WriteLine("Salary: " & Salary.ToString("C"))
    End Sub
End Class
