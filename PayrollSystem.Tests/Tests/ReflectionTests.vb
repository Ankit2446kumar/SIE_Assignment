Imports System.Reflection
Imports NUnit.Framework

<TestFixture>
Public Class ReflectionTests

    <Test>
    Public Sub Test_Upload_HasExpectedMethods()
        Dim t = GetType(Upload)
        Assert.IsNotNull(t.GetMethod("Page_Load", BindingFlags.Instance Or BindingFlags.NonPublic))
        Assert.IsNotNull(t.GetMethod("btnAddEmployee_Click", BindingFlags.Instance Or BindingFlags.NonPublic))
        Assert.IsNotNull(t.GetMethod("btnUploadCSV_Click", BindingFlags.Instance Or BindingFlags.NonPublic))
    End Sub

    <Test>
    Public Sub Test_ProcessPayroll_HasBtnProcessClick()
        Dim t = GetType(ProcessPayroll)
        Assert.IsNotNull(t.GetMethod("btnProcess_Click", BindingFlags.Instance Or BindingFlags.NonPublic))
    End Sub

    <Test>
    Public Sub Test_ReportPage_HasExportAndBindMethods()
        Dim t = GetType(Report)
        Assert.IsNotNull(t.GetMethod("Page_Load", BindingFlags.Instance Or BindingFlags.NonPublic))
        Assert.IsNotNull(t.GetMethod("btnExportCSV_Click", BindingFlags.Instance Or BindingFlags.NonPublic))
        Assert.IsNotNull(t.GetMethod("BindReports", BindingFlags.Instance Or BindingFlags.Public Or BindingFlags.NonPublic))
    End Sub

End Class
