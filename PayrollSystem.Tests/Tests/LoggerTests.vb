Imports NUnit.Framework
Imports System.IO
Imports System.Web
Imports System.Reflection

<TestFixture>
Public Class LoggerTests

    Private tempDir As String
    Private originalContext As HttpContext

    <SetUp>
    Public Sub Setup()
        ' Backup the original context
        originalContext = HttpContext.Current

        ' Create a fake HttpContext
        Dim request = New HttpRequest("", "http://localhost/", "")
        Dim response = New HttpResponse(New StringWriter())
        Dim context = New HttpContext(request, response)

        ' Assign HttpContext.Current BEFORE using it
        HttpContext.Current = context

        ' Create a temporary directory for testing logs
        tempDir = Path.Combine(Path.GetTempPath(), Path.GetRandomFileName())
        Directory.CreateDirectory(tempDir)

        ' Override the private logFilePath field in Logger using reflection
        Dim field = GetType(Logger).GetField("logFilePath", BindingFlags.NonPublic Or BindingFlags.Static)
        field.SetValue(Nothing, Path.Combine(tempDir, "Log.txt"))
    End Sub

    <TearDown>
    Public Sub TearDown()
        ' Restore original HttpContext
        HttpContext.Current = originalContext

        ' Clean up
        If Directory.Exists(tempDir) Then
            Directory.Delete(tempDir, recursive:=True)
        End If
    End Sub

    <Test>
    Public Sub LogInfo_WritesExpectedContent()
        Logger.LogInfo("Test Info Message")
        Dim content = File.ReadAllText(Path.Combine(tempDir, "Log.txt"))
        StringAssert.Contains("[INFO] Test Info Message", content)
    End Sub

    <Test>
    Public Sub LogWarning_WritesExpectedContent()
        Logger.LogWarning("Warning occurred")
        Dim content = File.ReadAllText(Path.Combine(tempDir, "Log.txt"))
        StringAssert.Contains("[WARNING] Warning occurred", content)
    End Sub

    <Test>
    Public Sub LogError_WritesExpectedContent()
        Logger.LogError("An error happened")
        Dim content = File.ReadAllText(Path.Combine(tempDir, "Log.txt"))
        StringAssert.Contains("[ERROR] An error happened", content)
    End Sub

    <Test>
    Public Sub Log_FileLocked_DoesNotThrow()
        Dim logPath = Path.Combine(tempDir, "Log.txt")
        Using fs As New FileStream(logPath, FileMode.Create, FileAccess.ReadWrite, FileShare.None)
            Assert.DoesNotThrow(Sub() Logger.LogInfo("While file is locked"))
        End Using
    End Sub

End Class
