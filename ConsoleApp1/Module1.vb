Module Module1
    Sub Main()
        ' Welcome message
        Console.WriteLine("=== Age Checker ===")

        ' Ask for user's name
        Console.Write("Enter your name: ")
        Dim name As String = Console.ReadLine()

        ' Ask for user's age
        Console.Write("Enter your age: ")
        Dim ageInput As String = Console.ReadLine()
        Dim age As Integer

        ' Validate and parse age
        If Integer.TryParse(ageInput, age) Then
            Console.WriteLine()
            If age < 18 Then
                Console.WriteLine("Hello " & name & "! You are a minor.")
            Else
                Console.WriteLine("Hello " & name & "! You are an adult.")
            End If
        Else
            Console.WriteLine("Invalid age entered. Please enter a valid number.")
        End If

        ' Pause before exit
        Console.WriteLine(vbCrLf & "Press any key to exit...")
        Console.ReadKey()
    End Sub
End Module