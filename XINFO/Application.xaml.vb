Class Application

    ' Application-level events, such as Startup, Exit, and DispatcherUnhandledException
    ' can be handled in this file.

    Protected Overrides Sub OnStartup(e As StartupEventArgs)
        MyBase.OnStartup(e)

        If Process.GetProcessesByName(Process.GetCurrentProcess.ProcessName).Length > 1 Then
            MessageBox.Show(XIRes.GetString("XINFO_Main_005"), XIRes.GetString("XINFO_Main_006"), MessageBoxButton.OK, MessageBoxImage.Information)

            End
        End If

    End Sub

End Class
