Imports System.IO
Imports System.Web

Public Class Logger
    Private Shared ReadOnly logFilePath As String = HttpContext.Current.Server.MapPath("~/App_Data/Log.txt")

    Public Shared Sub LogInfo(message As String)
        Log("INFO", message)
    End Sub

    Public Shared Sub LogWarning(message As String)
        Log("WARNING", message)
    End Sub

    Public Shared Sub LogError(message As String)
        Log("ERROR", message)
    End Sub

    Private Shared Sub Log(logType As String, message As String)
        Try
            Dim logMessage As String = $"{DateTime.Now:yyyy-MM-dd HH:mm:ss} [{logType}] {message}{Environment.NewLine}"
            SyncLock GetType(Logger)
                File.AppendAllText(logFilePath, logMessage)
            End SyncLock
        Catch
            ' Logging failures are silently ignored
        End Try
    End Sub
End Class
