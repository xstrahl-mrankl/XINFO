Imports System.Globalization
Imports System.IO
Imports System.Windows.Threading

Class XIWW001_MainWindow

    Dim GlobalTimer As New DispatcherTimer


    Public Sub New()

        ' Dieser Aufruf ist für den Designer erforderlich.
        InitializeComponent()


        'Dynamically load a DLL, e.g. when it is unsigned.
        'Dim a As System.Reflection.Assembly = System.Reflection.Assembly.LoadFrom("C:\XstrahlCrypto.dll")
        'Dim oType As System.Type
        'Dim oAssembly As System.Reflection.Assembly
        'Dim oObject As System.Object
        'oAssembly = System.Reflection.Assembly.LoadFrom("C:\XstrahlCrypto.dll")
        'oType = oAssembly.GetType("XstrahlCrypto.XstrahlFunctions") '.GetDateTimeAsYYYYMMDD_HHMMSS")
        'oObject = Activator.CreateInstance(oType)
        'LBLmain_DateTime.Content = oObject.GetDateTimeAsYYYYMMDD_HHMMSS(Now).ToString




        ' Fügen Sie Initialisierungen nach dem InitializeComponent()-Aufruf hinzu.

        AddHandler GlobalTimer.Tick, AddressOf GlobalTickerEvents
        GlobalTimer.Interval = New TimeSpan(0, 0, 1)
        GlobalTimer.Start()


        Dim XInfoOwnFilename As String = System.Reflection.Assembly.GetExecutingAssembly.Location
        Dim DateOfBuild As DateTime = DateAdd(DateInterval.Day, Application.ResourceAssembly.GetName.Version.Build, #1/1/2000#) 'the automatic set build is the number of days since Jan 1, 2000.
        Dim TimeOfBuild As String = New TimeSpan(0, 0, Application.ResourceAssembly.GetName.Version.Revision * 2).ToString 'the automatic set revision is the number of seconds since midnight divided by two - not taking daylight saving into account!!!

        LBLmain_XInfoVersion.Content = Application.ResourceAssembly.GetName.Name.ToString & " v" & FileVersionInfo.GetVersionInfo(XInfoOwnFilename).ProductVersion
        'LongVersion
        'LBLmain_XLoopVersion.Content = Application.ResourceAssembly.GetName.Name.ToString & " v" & FileVersionInfo.GetVersionInfo(XInfoOwnFilename).ProductVersion & " (" & DateOfBuild & " - " & TimeOfBuild & ")"

        CreateXLicenceFolderStructure()
        XInfoGeneral.XINFOmenu = True
        XINFOGridMenu.DataContext = XInfoGeneral

    End Sub

    Public Shared Sub CreateXLicenceFolderStructure()

        'Create_Directory_Structure
        Dim DirInfo As DirectoryInfo

        DirInfo = New DirectoryInfo(XICL04_ReadOnly.Folder_CUPXstrahlXInfo)
        If DirInfo.Exists = False Then
            DirInfo.Create()
        End If

        For Each SubDir In {XICL04_ReadOnly.XInfoDirTemp, XICL04_ReadOnly.XInfoDirExportCSV,
                            XICL04_ReadOnly.XInfoDirExportXML, XICL04_ReadOnly.XInfoDirExportXPS,
                            XICL04_ReadOnly.XInfoDirSettings, XICL04_ReadOnly.XInfoDirClassicDB,
                            XICL04_ReadOnly.XInfoDirLicence}
            DirInfo = New DirectoryInfo(SubDir)
            If DirInfo.Exists = False Then
                Try
                    DirInfo.Create()
                Catch ex As Exception
                End Try
            End If
        Next SubDir

    End Sub


    Private Sub OpenMyUserControl(sender As Object, e As RoutedEventArgs)

        If RemoveChildWindow() = False Then Exit Sub

        If sender Is BTN_ErrorCodes Then
            NumberOfActiveMenu = 1
        ElseIf sender Is BTN_Manuals Then
            NumberOfActiveMenu = 2
        ElseIf sender Is BTN_Imperium Then
            NumberOfActiveMenu = 3
        End If
        Call AddChildWindow()

    End Sub


    Private Sub AddChildWindow()

        Dim XHWindow As UserControl
        Dim XHWindowName As String
        Dim XHGridRow As Int32 = 1

        Select Case NumberOfActiveMenu
            Case 1
                XHWindow = New XIUC01_ErrorCodeTable
                XHWindowName = "XICUC01"
                XHGridRow = 1
            Case 2
                XHWindow = New XIUC02_Manuals
                XHWindowName = "XICUC02"
                XHGridRow = 1
            Case 3
                XHWindow = New XIUC03_Imperium
                XHWindowName = "XICUC03"
                XHGridRow = 1
            Case Else
                XHWindow = New XIUC01_ErrorCodeTable
                XHWindowName = "XICUC01"
                XHGridRow = 1
        End Select

        XHWindow.Name = XHWindowName
        XINFOMainGrid.RegisterName(XHWindow.Name, XHWindow)
        NameOfActiveChildUC = XHWindow.Name

        XINFOMainGrid.Children.Add(XHWindow)
        Grid.SetRow(XHWindow, 1)

    End Sub

    Private Function RemoveChildWindow() As Boolean

        If NameOfActiveChildUC = "" Then Return True 'Nothing to do

        Try
            XINFOMainGrid.Children.Remove(CType(XINFOMainGrid.FindName(NameOfActiveChildUC), UIElement))
            XINFOMainGrid.UnregisterName(NameOfActiveChildUC)
            NameOfActiveChildUC = ""
        Catch ex As Exception
            Throw
        Finally

        End Try

        Return True

    End Function

    Private Sub XLoopExit()

        Me.Close() 'this will call MainWindow_Closing. If it is cancelled there it is cancelled here as well

    End Sub

    Private Sub MainWindow_Closing(sender As Object, e As ComponentModel.CancelEventArgs) Handles Me.Closing

        'Answer = XLoopCL14functions.ShowXLoopMsg(MsgTypeID:=5, MsgID:=1, Wildcard:={"", "", ""})

        'If Answer = MessageBoxResult.No Then
        '    e.Cancel = True
        '    Exit Sub
        'Else
        '    If RemoveChildWindow() = False Then
        '        e.Cancel = True
        '    Else
        '        e.Cancel = False
        '        GlobalTimer.Stop()
        '        Call XLoopMD05hl7.StartStopHL7Streamer("OFF") ', Me.ELPS06_01, Me.ELPS06_02)
        '        Call XLoopCL15subs.WriteToXLoopLog(True, "XLoop stopped" & vbCrLf & StrDup(60, "-"), True, "", "", 0)
        '    End If
        'End If

    End Sub

    Private Sub GlobalTickerEvents(sender As Object, e As EventArgs)
        'NO LOGGING AT THE GLOBAL TICKER !!!

        LBLmain_DateTime.Content = Strings.FormatDateTime(Now, DateFormat.ShortDate) & " - " & Strings.FormatDateTime(Now, DateFormat.LongTime)

    End Sub

    Private Sub XInfoExit()

        Me.Close() 'this will call MainWindow_Closing. If it is cancelled there it is cancelled here as well

    End Sub

End Class
