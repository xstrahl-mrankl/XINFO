Imports System.Windows.Xps
Imports System.Windows.Xps.Packaging
Imports Microsoft.Win32
Imports System.Collections.ObjectModel
Imports System.ComponentModel
Imports System.IO
Imports System.Text
'Imports System.Windows.Forms
Imports System.Globalization
Imports System.Windows.Markup
Imports System.Threading
Imports System.Xml
Imports System.Security.Cryptography
Imports XstrahlCrypto
Imports System.Xml.Serialization
Imports System.Text.RegularExpressions

Public Class XIUC03_Imperium
    Implements INotifyPropertyChanged

    Dim XCRYPT As New XstrahlCrypto.XstrahlCrypto
    Dim ClassicDatabase As XICL05_Enums.ClassicDatabaseType

    Public Event PropertyChanged As PropertyChangedEventHandler Implements INotifyPropertyChanged.PropertyChanged


    Dim MyBackgrndWrkr As New BackgroundWorker
    Dim MyUC As New XIUC99_Progress

    'ALL Treatments in the database
    Dim MyImpTreatments As New ObservableCollection(Of XInfoImperiumDB)
    Dim MyImpTreatmentsView As ICollectionView = CollectionViewSource.GetDefaultView(MyImpTreatments)

    'Treatments after FIND
    Dim MyImpSearchTreatments As New ObservableCollection(Of XInfoImperiumDB)
    Dim MyImpSearchTreatmentsView As ICollectionView = CollectionViewSource.GetDefaultView(MyImpSearchTreatments)

    'Current selected Treatment
    Public MyImpSelectedTreatment As New XInfoImperiumDB
    Private _ImperiumSearchCounter As Int32
    Private _CurrentItemCounter As Int32 = 0
    Private _CurrentPrintItemCounter As Int32 = 0

    Private _ImperiumFolder As String
    Private _ImperiumTotalCounter As Int32 = 0

    Public Property MyImpTreatmentsProperty As ObservableCollection(Of XInfoImperiumDB)
        Get
            Return MyImpTreatments
        End Get
        Set(value As ObservableCollection(Of XInfoImperiumDB))
            MyImpTreatments = value
        End Set
    End Property

    Public Property ImperiumTotalCounter As Int32
        Get
            If MyImpTreatments Is Nothing = True Then
                Return 0
            Else
                Return MyImpTreatments.Count
            End If
        End Get
        Set(value As Int32)
            _ImperiumTotalCounter = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("ImperiumTotalCounter"))
        End Set
    End Property

    Public ReadOnly Property PaperSize As XICL05_Enums.Papersize
        Get
            If RDBTN03_PapersizeA4.IsChecked = True Then
                Return XICL05_Enums.Papersize.A4
            ElseIf RDBTN03_PapersizeLetter.IsChecked = True Then
                Return XICL05_Enums.Papersize.Letter
            Else
                Return XICL05_Enums.Papersize.A4
            End If
            'RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("PaperSize"))
        End Get
    End Property

    Public Property ImperiumSearchCounter As Int64
        Get
            Return _ImperiumSearchCounter
        End Get
        Set(value As Int64)
            If MyImpSearchTreatments Is Nothing = True Then
                _ImperiumSearchCounter = 0
            Else
                _ImperiumSearchCounter = MyImpSearchTreatments.Count
                RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("ImperiumSearchCounter"))
            End If
        End Set
    End Property

    Public Property CurrentItemCounter As Int32
        Get
            Return _CurrentItemCounter
        End Get
        Set(value As Int32)
            _CurrentItemCounter = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("CurrentItemCounter"))
        End Set
    End Property

    Public Property CurrentPrintItemCounter As Int32
        Get
            Return _CurrentPrintItemCounter
        End Get
        Set(value As Int32)
            _CurrentPrintItemCounter = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("CurrentPrintItemCounter"))
        End Set
    End Property

    Public Property ImperiumFolder As String
        Get
            Return _ImperiumFolder
        End Get
        Set(value As String)
            _ImperiumFolder = value
        End Set
    End Property

    Public Sub New()

        'System.Threading.Thread.CurrentThread.CurrentCulture = New CultureInfo("EN")
        'System.Threading.Thread.CurrentThread.CurrentUICulture = New CultureInfo("EN")

        ' This call is required by the designer.
        InitializeComponent()

        System.Threading.Thread.CurrentThread.CurrentCulture = New CultureInfo("EN")
        System.Threading.Thread.CurrentThread.CurrentUICulture = New CultureInfo("EN")


        ' Add any initialization after the InitializeComponent() call.

        Me.LBL03_NrOfAllItems.DataContext = Me
        Me.TXTBX03_CurrentItem.DataContext = Me
        Me.TXTBX03_AllItems.DataContext = Me
        'Me.TXTBX03_CurrentPrintItem.DataContext = Me

        Me.TXTBLCK03_Imperium01.DataContext = MyImpSelectedTreatment
        Me.TXTBLCK03_Imperium02.DataContext = MyImpSelectedTreatment
        Me.TXTBLCK03_Imperium03.DataContext = MyImpSelectedTreatment
        Me.TXTBLCK03_Imperium04.DataContext = MyImpSelectedTreatment
        Me.TXTBLCK03_Imperium05.DataContext = MyImpSelectedTreatment
        Me.TXTBLCK03_Imperium06.DataContext = MyImpSelectedTreatment
        Me.TXTBLCK03_Imperium07.DataContext = MyImpSelectedTreatment
        Me.TXTBLCK03_Imperium08.DataContext = MyImpSelectedTreatment
        Me.TXTBLCK03_Imperium09.DataContext = MyImpSelectedTreatment
        Me.TXTBLCK03_Imperium10.DataContext = MyImpSelectedTreatment '11 (time) is included in the total date!
        Me.TXTBLCK03_Imperium12.DataContext = MyImpSelectedTreatment
        Me.TXTBLCK03_Imperium13.DataContext = MyImpSelectedTreatment
        Me.TXTBLCK03_Imperium14.DataContext = MyImpSelectedTreatment
        Me.TXTBLCK03_Imperium15.DataContext = MyImpSelectedTreatment
        Me.TXTBLCK03_Imperium16.DataContext = MyImpSelectedTreatment
        Me.TXTBLCK03_Imperium17.DataContext = MyImpSelectedTreatment
        Me.TXTBLCK03_Imperium18.DataContext = MyImpSelectedTreatment
        Me.TXTBLCK03_Imperium19.DataContext = MyImpSelectedTreatment
        Me.TXTBLCK03_Imperium20.DataContext = MyImpSelectedTreatment
        Me.TXTBLCK03_Imperium21.DataContext = MyImpSelectedTreatment

        MyBackgrndWrkr.WorkerSupportsCancellation = True
        MyBackgrndWrkr.WorkerReportsProgress = True

        AddHandler MyBackgrndWrkr.DoWork, AddressOf ShowProgressDoWork ' ExportAsXPSDoWork
        AddHandler MyBackgrndWrkr.ProgressChanged, AddressOf ExportAsXPSProgressChanged
        AddHandler MyBackgrndWrkr.RunWorkerCompleted, AddressOf ExportAsXPSComplete

    End Sub

    Private Sub SearchForImperium(sender As Object, es As RoutedEventArgs)

        Dim INIline As String = ""
        Dim INIcurrentsection As String = ""
        Dim ImpHospitalNameSystem As String = ""
        Dim ImpHospitalNameD3KSystem As String = ""
        Dim ImpDepartmentName As String = ""
        Dim ImpSystemName As String = ""
        Dim ImpHospitalCode As String = ""
        Dim DBFileName As String = ""
        Dim INIFileName As String = ""

        ImperiumFolder = GetNewFolder() 'will contain a backslash at the end

        If String.IsNullOrWhiteSpace(ImperiumFolder) = True Then Exit Sub

        Dim MyDBFFileInfo As FileInfo
        Dim MyINIFileInfo As FileInfo

        If RDBTN03_DbaseD3KTMNT.IsChecked = True Then
            DBFileName = "D3KTMNT.DBF"
            INIFileName = "IMPERIUM.INI"
        ElseIf RDBTN03_DbaseTREATMNT.IsChecked = True Then
            DBFileName = "TREATMNT.DBF"
            INIFileName = "D3K.INI"
        ElseIf RDBTN03_DbaseVERDATA.IsChecked = True Then
            DBFileName = "VERDATA.DBF"
            INIFileName = "VERD3K.INI"
        Else
            Exit Sub
        End If

        MyDBFFileInfo = New FileInfo(ImperiumFolder & DBFileName)
        If MyDBFFileInfo.Exists = False Then
            MessageBox.Show(String.Format(XIRes.GetString("XINFO_UC03_052"), MyDBFFileInfo, XICL04_ReadOnly.XInfoDirClassicDB).Replace("~", vbCrLf), XIRes.GetString("XINFO_UC03_053"), MessageBoxButton.OK, MessageBoxImage.Information)
            Exit Sub
        End If

        MyINIFileInfo = New FileInfo(ImperiumFolder & INIFileName)
        If MyINIFileInfo.Exists = False Then
            MessageBox.Show(String.Format(XIRes.GetString("XINFO_UC03_052"), MyINIFileInfo, XICL04_ReadOnly.XInfoDirClassicDB).Replace("~", vbCrLf), XIRes.GetString("XINFO_UC03_053"), MessageBoxButton.OK, MessageBoxImage.Information)
            Exit Sub
        End If

        MyImpSearchTreatments.Clear()
        MyImpSelectedTreatment.ResetItem()
        CurrentItemCounter = 0
        ImperiumSearchCounter = MyImpSearchTreatments.Count
        DCMNTVWR03_Imperium.Document = Nothing

        'OK, the database exists and the ini file as well.
        'Read in first the ini file to get the information that the licence is based on.
        'The information will be added to each record when reading in the database.
        Dim MyImpIniReader As New StreamReader(MyINIFileInfo.FullName, Encoding.Default)
        Do
            INIline = MyImpIniReader.ReadLine()
            If INIline Is Nothing = False Then '[D3K-SYSTEM]
                If INIline.StartsWith("[") = True Then INIcurrentsection = INIline.ToUpper
                'It will use (by default) first the Hospital Name stored in the [SYSTEM] section.
                'If it finds another Hospital Name and the section is [D3K-SYSTEM], then this is used.
                'The Hospital Name in [D3K-SYSTEM] is the one typed in by the user.
                If INIline.StartsWith("HospitalName") = True And INIcurrentsection.Contains("SYSTEM") = True Then ImpHospitalNameSystem = INIline.Substring(InStr(INIline, "=")).Trim
                If INIline.StartsWith("HospitalName") = True And INIcurrentsection.Contains("D3K-SYSTEM") = True Then ImpHospitalNameD3KSystem = INIline.Substring(InStr(INIline, "=")).Trim
                If INIline.StartsWith("HospitalName") = True And INIcurrentsection.Contains("D3K-VERIFICATION") = True Then ImpHospitalNameD3KSystem = INIline.Substring(InStr(INIline, "=")).Trim
                If INIline.StartsWith("DepartmentName") = True Then ImpDepartmentName = INIline.Substring(InStr(INIline, "=")).Trim
                If INIline.StartsWith("SystemName") = True Then ImpSystemName = INIline.Substring(InStr(INIline, "=")).Trim
            End If
        Loop Until INIline Is Nothing = True

        If ImpHospitalNameD3KSystem = "" Then ImpHospitalNameD3KSystem = ImpHospitalNameSystem
        Dim cCrypt As String = "Melissa/Surtees" 'I am using the Hospital Code Mechanism of ELEVAR.
        ImpHospitalCode = XstrahlCrypto.XstrahlFunctions.GetVOAsciiSum(XstrahlCrypto.XstrahlSimpleVOCRYPT.EncodeOrDecodeInformation(ImpHospitalNameD3KSystem & ImpDepartmentName & ImpSystemName, cCrypt))
        If ImpHospitalCode.Length > 4 Then ImpHospitalCode = ImpHospitalCode.Substring(0, 4)

#Region "NOW CHECK THE LICENCE!!!"

        Dim XInfoLicenceFileNamePattern As String = "Xstrahl_" & ImpHospitalCode & "_XII_" & "?" & "????????_????????_??????.exml"

        Dim XInfoLicenceFiles() As FileInfo
        Dim SettingsFolder As New FileInfo(XICL04_ReadOnly.XInfoDirSettings)

        XInfoLicenceFiles = SettingsFolder.Directory.GetFiles(XInfoLicenceFileNamePattern, SearchOption.TopDirectoryOnly)

        If XInfoLicenceFiles.Count = 0 Then
            MessageBox.Show(String.Format(XIRes.GetString("XINFO_UC03_056").Replace("~", vbCrLf), ImpHospitalCode, XICL04_ReadOnly.XInfoDirSettings),
                            XIRes.GetString("XINFO_UC03_057"), MessageBoxButton.OK, MessageBoxImage.Information)
            Exit Sub
        ElseIf XInfoLicenceFiles.Count > 1 Then
            MessageBox.Show(String.Format(XIRes.GetString("XINFO_UC03_058").Replace("~", vbCrLf), ImpHospitalCode, XICL04_ReadOnly.XInfoDirSettings),
                            XIRes.GetString("XINFO_UC03_057"), MessageBoxButton.OK, MessageBoxImage.Exclamation)
            Exit Sub
        End If

        'Now since there is only one licence file where the file name matches the wanted pattern, read the licence.

        Dim TheXInfoLicenceFromXML As New XstrahlCrypto.XstrahlLicenceObject
        Dim TheXInfoLicenceTemp As New XstrahlReturnCodes.ReturnCodes '     New XstrahlCrypto.XstrahlLicenceObject

        If TheXInfoLicenceTemp <> XstrahlReturnCodes.ReturnCodes.ERR_No_Error Then
            MessageBox.Show(String.Format(XIRes.GetString("XINFO_UC03_064").Replace("~", vbCrLf), TheXInfoLicenceTemp),
                            XIRes.GetString("XINFO_UC03_057"), MessageBoxButton.OK, MessageBoxImage.Exclamation)
            Exit Sub
        End If

        'Initialize the licence file en/decryption parameters
        Dim MyDeCryptParamsAES As New XstrahlCrypto.XstrahlEnDeCrypt With {
            .EnDeCryptionModeToUse = XstrahlCrypto.XstrahlCryptoModes.EnDecryptionMode.AES,
            .InitVector = "*",
            .Password = "My94$$W0r6"
        }

        Dim TempErrCode As XstrahlCrypto.XstrahlReturnCodes.ReturnCodes
        Dim TempLicenceContent As String = File.ReadAllText(XInfoLicenceFiles(0).FullName)
        MyDeCryptParamsAES.InitVector = TempLicenceContent.Substring(0, 16)
        Dim EnDecryptAES As New XstrahlCrypto.XstrahlSimpleAES(MyDeCryptParamsAES.Password, MyDeCryptParamsAES.InitVector, TempErrCode, Nothing)
        Dim TempLicenceXML As String = EnDecryptAES.DecryptData(TempLicenceContent.Substring(16), TempErrCode, Nothing)
        Dim TempXML As New XmlDocument
        TempXML.LoadXml(TempLicenceXML)

        TheXInfoLicenceFromXML.LicenceXMLDoc = TempXML


        If TheXInfoLicenceTemp <> XstrahlReturnCodes.ReturnCodes.ERR_No_Error _
            And
            TheXInfoLicenceTemp <> XstrahlReturnCodes.ReturnCodes.SIG_Signature_In_Enveloped_XML_Valid _
            Then
            MessageBox.Show(String.Format(XIRes.GetString("XINFO_UC03_059").Replace("~", vbCrLf),
                                              TheXInfoLicenceTemp.ToString & vbCrLf & TheXInfoLicenceFromXML.ReturnCode.ToString,
                                              TheXInfoLicenceFromXML.LicenceDetails.LicenceActive,
                                              TheXInfoLicenceFromXML.LicenceDetails.LicenceCanExpire,
                                              TheXInfoLicenceFromXML.LicenceDetails.LicenceCreationDate,
                                              TheXInfoLicenceFromXML.LicenceDetails.LicenceDeviceSN,
                                              TheXInfoLicenceFromXML.LicenceDetails.LicenceExpireDate,
                                              TheXInfoLicenceFromXML.LicenceDetails.LicenceFilename,
                                              TheXInfoLicenceFromXML.LicenceDetails.LicenceKey,
                                              TheXInfoLicenceFromXML.LicenceDetails.LicenceType),
                                XIRes.GetString("XINFO_UC03_057"), MessageBoxButton.OK, MessageBoxImage.Exclamation)
            Exit Sub
        End If

        If TheXInfoLicenceFromXML.IsLicenceValid = False Then
            'General message that Licence is not valid. Reason is given by the error message code
            MessageBox.Show(String.Format(XIRes.GetString("XINFO_UC03_059").Replace("~", vbCrLf),
                                          TheXInfoLicenceTemp.ToString & vbCrLf & TheXInfoLicenceFromXML.ReturnCode.ToString,
                                          TheXInfoLicenceFromXML.LicenceDetails.LicenceActive,
                                          TheXInfoLicenceFromXML.LicenceDetails.LicenceCanExpire,
                                          TheXInfoLicenceFromXML.LicenceDetails.LicenceCreationDate,
                                          TheXInfoLicenceFromXML.LicenceDetails.LicenceDeviceSN,
                                          TheXInfoLicenceFromXML.LicenceDetails.LicenceExpireDate,
                                          TheXInfoLicenceFromXML.LicenceDetails.LicenceFilename,
                                          TheXInfoLicenceFromXML.LicenceDetails.LicenceKey,
                                          TheXInfoLicenceFromXML.LicenceDetails.LicenceType),
                            XIRes.GetString("XINFO_UC03_057"), MessageBoxButton.OK, MessageBoxImage.Exclamation)
            Exit Sub
        ElseIf TheXInfoLicenceFromXML.IsLicenceValid = True Then
            If Format(TheXInfoLicenceFromXML.LicenceDetails.LicenceDeviceSN, "0000").Substring(0, 4) <> ImpHospitalCode Then
                MessageBox.Show(String.Format(XIRes.GetString("XINFO_UC03_060").Replace("~", vbCrLf), ImpHospitalCode),
                            XIRes.GetString("XINFO_UC03_057"), MessageBoxButton.OK, MessageBoxImage.Exclamation)
                Exit Sub
            ElseIf TheXInfoLicenceFromXML.LicenceDetails.LicenceType <> "[XII]" Then
                MessageBox.Show(XIRes.GetString("XINFO_UC03_061"),
                            XIRes.GetString("XINFO_UC03_057"), MessageBoxButton.OK, MessageBoxImage.Exclamation)
                Exit Sub
            ElseIf TheXInfoLicenceFromXML.LicenceDetails.LicenceActive = False Then
                MessageBox.Show(XIRes.GetString("XINFO_UC03_062"),
                            XIRes.GetString("XINFO_UC03_057"), MessageBoxButton.OK, MessageBoxImage.Exclamation)
                Exit Sub
            End If

        End If

#End Region

        Me.Cursor = System.Windows.Input.Cursors.Wait
        GetImperiumData(MyDBFFileInfo.FullName, ImpHospitalNameD3KSystem, ImpDepartmentName, ImpSystemName)
        Me.Cursor = System.Windows.Input.Cursors.Arrow

        '[System]
        'HospitalName = Gulmay Medical Ltd.
        'DepartmentName = Orthovolttherapie
        'SystemName = XStrahl200
        'DataDirectory = C :  \Imperium
        'PhysicsDirectory = C :  \Fisica
        'BackupDirectory =
        'SerialPort = 4
        'LastTreatmentTime = 20 : 08:09
        'LastTreatmentDate = 7.1.2015
        '[D3K-SYSTEM]
        'HospitalName = Radiologie Neustadt

    End Sub

    Public Shared Function GetNewFolder() As String

        Dim NewFolder As String = ""
        Dim Result As MessageBoxResult
        Dim SelectLogFolder As New System.Windows.Forms.FolderBrowserDialog With {
            .Description = XIRes.GetString("XINFO_UC03_050"),
            .SelectedPath = XICL04_ReadOnly.XInfoDirClassicDB,
            .RootFolder = Environment.SpecialFolder.MyComputer,
            .ShowNewFolderButton = False
        }

        Result = CType(SelectLogFolder.ShowDialog(), MessageBoxResult)

        If Result = Forms.DialogResult.OK Then
            NewFolder = ReturnPathWithBackslash(SelectLogFolder.SelectedPath)
        Else
            NewFolder = ""
        End If

        Return NewFolder

    End Function

    Public Shared Function ReturnPathWithBackslash(PathInformation As String) As String

        If IsNothing(PathInformation) = True Then Return XICL04_ReadOnly.XInfoDirClassicDB

        If PathInformation.EndsWith("\", StringComparison.OrdinalIgnoreCase) = False Then PathInformation &= "\"
        Return PathInformation

    End Function

    Private Sub GoToItem(sender As Object, es As RoutedEventArgs)

        If MyImpSearchTreatments.Count <= 0 Then Exit Sub

        If sender Is BTN03_ItemsFirst Then
            CurrentItemCounter = 1
        ElseIf sender Is BTN03_ItemsPrev Then
            CurrentItemCounter -= 1
            If CurrentItemCounter < 1 Then CurrentItemCounter = 1
        ElseIf sender Is BTN03_ItemsNext Then
            CurrentItemCounter += 1
            If CurrentItemCounter > MyImpSearchTreatments.Count Then CurrentItemCounter = MyImpSearchTreatments.Count
        ElseIf sender Is BTN03_ItemsLast Then
            CurrentItemCounter = MyImpSearchTreatments.Count
        End If

        Dim LocalIndex As Int32 = CurrentItemCounter - 1

        Try
            MyImpSelectedTreatment.CopyItem(MyImpSearchTreatments(LocalIndex))
            'Dim MyDocument As FixedDocument = XInfoImperiumDB.CreateFixedDocumentWithPages(MyImpSelectedTreatment, 1, "", PaperSize)
            Dim MyDocument As FixedDocument = XInfoImperiumDB.CreateFixedDocumentWithPages(MyImpSearchTreatments, LocalIndex, 1, "", PaperSize)
            DCMNTVWR03_Imperium.Document = TryCast(MyDocument, IDocumentPaginatorSource)
        Catch ex As Exception
        End Try

    End Sub

    Private Sub SearchForSelectedProperty(sender As Object, e As RoutedEventArgs)

        MyImpSearchTreatments.Clear()

        Dim SelectedPaperSize As XICL05_Enums.Papersize = PaperSize

        If Me.TXTBX03_SearchValue.Text = XIRes.GetString("XINFO_UC03_010") Then Me.TXTBX03_SearchValue.Text = String.Empty

        Dim SearchValue As String = Me.TXTBX03_SearchValue.Text.Trim
        Dim ImpDataValue As String = ""
        Dim MatchPosition As Int32 = -1

        For Each ImpTreatment In MyImpTreatments
            If RDBTN03_Imperium01.IsChecked = True Then ImpDataValue = ImpTreatment.PatientFullName
            If RDBTN03_Imperium02.IsChecked = True Then ImpDataValue = ImpTreatment.PatientID
            If RDBTN03_Imperium03.IsChecked = True Then ImpDataValue = Format(ImpTreatment.PatientDoB, "yyyyMMdd")
            If RDBTN03_Imperium04.IsChecked = True Then ImpDataValue = ImpTreatment.PatientPicture
            If RDBTN03_Imperium05.IsChecked = True Then ImpDataValue = ImpTreatment.Clinician
            If RDBTN03_Imperium06.IsChecked = True Then ImpDataValue = ImpTreatment.TreatmentField
            If RDBTN03_Imperium07.IsChecked = True Then ImpDataValue = ImpTreatment.TreatmentPosition
            If RDBTN03_Imperium08.IsChecked = True Then ImpDataValue = ImpTreatment.FilterCode.ToString
            If RDBTN03_Imperium09.IsChecked = True Then ImpDataValue = ImpTreatment.ApplicatorCode
            If RDBTN03_Imperium10.IsChecked = True Then ImpDataValue = Format(ImpTreatment.TreatmentDateAndTime, "yyyyMMdd")
            'If RDBTN03_Imperium11.IsChecked = True Then ImpDataValue = Format(ImpTreatment.TreatmentDateAndTime, "HH:mm:ss")
            If RDBTN03_Imperium12.IsChecked = True Then ImpDataValue = ImpTreatment.FilterkV.ToString
            If RDBTN03_Imperium13.IsChecked = True Then ImpDataValue = ImpTreatment.FiltermA.ToString
            If RDBTN03_Imperium14.IsChecked = True Then ImpDataValue = ImpTreatment.FilterHVL
            If RDBTN03_Imperium15.IsChecked = True Then ImpDataValue = ImpTreatment.ApplicatorData
            If RDBTN03_Imperium16.IsChecked = True Then ImpDataValue = ImpTreatment.DoseAdmin
            If RDBTN03_Imperium17.IsChecked = True Then ImpDataValue = ImpTreatment.TimeAdmin
            If RDBTN03_Imperium18.IsChecked = True Then ImpDataValue = ImpTreatment.TreatmentOperator
            If RDBTN03_Imperium19.IsChecked = True Then ImpDataValue = ImpTreatment.DoseSet
            If RDBTN03_Imperium20.IsChecked = True Then ImpDataValue = ImpTreatment.TimeSet
            If RDBTN03_Imperium21.IsChecked = True Then ImpDataValue = ImpTreatment.GeneratorError


            If (RDBTN03_MatchPositionStart.IsChecked = True And ImpDataValue.IndexOf(SearchValue, 0, StringComparison.InvariantCultureIgnoreCase) = 0) _
                Or
               (RDBTN03_MatchPositionAny.IsChecked = True And ImpDataValue.IndexOf(SearchValue, 0, StringComparison.InvariantCultureIgnoreCase) > -1) _
               Or
               (RDBTN03_MatchPositionExact.IsChecked = True And ImpDataValue.Equals(SearchValue, StringComparison.InvariantCultureIgnoreCase) = True) Then

                MyImpSearchTreatments.Add(New XInfoImperiumDB With {
                                        .DataBaseIndex = ImpTreatment.DataBaseIndex,
                                        .PatientFullName = ImpTreatment.PatientFullName,
                                        .PatientID = ImpTreatment.PatientID,
                                        .PatientDoB = ImpTreatment.PatientDoB,
                                        .PatientPicture = ImpTreatment.PatientPicture,
                                        .Clinician = ImpTreatment.Clinician,
                                        .TreatmentField = ImpTreatment.TreatmentField,
                                        .TreatmentPosition = ImpTreatment.TreatmentPosition,
                                        .FilterCode = ImpTreatment.FilterCode,
                                        .ApplicatorCode = ImpTreatment.ApplicatorCode,
                                        .TreatmentDateAndTime = ImpTreatment.TreatmentDateAndTime,
                                        .FilterkV = ImpTreatment.FilterkV,
                                        .FiltermA = ImpTreatment.FiltermA,
                                        .FilterHVL = ImpTreatment.FilterHVL,
                                        .ApplicatorData = ImpTreatment.ApplicatorData,
                                        .DoseAdmin = ImpTreatment.DoseAdmin,
                                        .TimeAdmin = ImpTreatment.TimeAdmin,
                                        .TreatmentOperator = ImpTreatment.TreatmentOperator,
                                        .DoseSet = ImpTreatment.DoseSet,
                                        .TimeSet = ImpTreatment.TimeSet,
                                        .GeneratorError = ImpTreatment.GeneratorError,
                                        .HospitalName = ImpTreatment.HospitalName,
                                        .DepartmentName = ImpTreatment.DepartmentName,
                                        .SystemName = ImpTreatment.SystemName})

            End If

        Next

        If RDBTN03_MatchPositionExact.IsChecked = True _
           And
           (RDBTN03_Imperium02.IsChecked = True Or RDBTN03_Imperium10.IsChecked = True) Then
            CHKBX03_ReportSummary.Visibility = Visibility.Visible
            If RDBTN03_Imperium02.IsChecked = True Then CHKBX03_ReportSummary.Tag = "PATID"
            If RDBTN03_Imperium10.IsChecked = True Then CHKBX03_ReportSummary.Tag = "TREATDATE"
        Else
            CHKBX03_ReportSummary.Visibility = Visibility.Hidden
            CHKBX03_ReportSummary.Tag = ""
        End If


        If MyImpSearchTreatments.Count > 0 Then
            MyImpSelectedTreatment.CopyItem(MyImpSearchTreatments(0))
            CurrentItemCounter = 1
            Dim MyDocument As FixedDocument = XInfoImperiumDB.CreateFixedDocumentWithPages(MyImpSearchTreatments, 0, 1, "", SelectedPaperSize)
            DCMNTVWR03_Imperium.Document = TryCast(MyDocument, IDocumentPaginatorSource)
        Else
            MyImpSelectedTreatment.ResetItem()
            CurrentItemCounter = 0
        End If

        ImperiumSearchCounter = MyImpSearchTreatments.Count

    End Sub

    Private Function GetImperiumData(ImperiumDBFile As String, HospitalName As String, DepartmentName As String, SystemName As String) As String

        Dim ShowDetails As Boolean = False
        Dim ImperiumPathAndFile As String = ImperiumDBFile
        Dim PadValue As Int32 = 40
        Dim Counter As Int32 = 0
        Dim RecCounter As Int32 = 0
        Dim I As Int32 = 0
        Dim II As Int32 = 0
        Dim NumberOfColumns As Int32 = 0
        Dim MaxNumberRecords As Int32 = 250000

        If ImperiumDBFile.ToUpper.Contains("D3KTMNT.DBF") Then 'Imperium
            ClassicDatabase = XICL05_Enums.ClassicDatabaseType.Imperium
            NumberOfColumns = 20
        ElseIf ImperiumDBFile.ToUpper.Contains("TREATMNT.DBF") Then
            ClassicDatabase = XICL05_Enums.ClassicDatabaseType.PDSD3K
            NumberOfColumns = 20
        ElseIf ImperiumDBFile.ToUpper.Contains("VERDATA.DBF") Then
            ClassicDatabase = XICL05_Enums.ClassicDatabaseType.VERD3K
            NumberOfColumns = 154
        ElseIf ImperiumDBFile.ToUpper.Contains("VERDATA.DBF") Then
            ClassicDatabase = XICL05_Enums.ClassicDatabaseType.Elevar
            NumberOfColumns = 166
        End If

        Dim fi As IO.FileInfo
        fi = New IO.FileInfo(ImperiumPathAndFile)


        If fi.Exists = True Then

            MyImpTreatments.Clear()

            Dim ByteArray() As Byte
            Dim StrDupCount As Int32 = 50

            Dim BinReader As BinaryReader = New BinaryReader(File.Open(ImperiumPathAndFile, FileMode.Open, FileAccess.Read), New UTF7Encoding)

            Dim DBFHeader0000 As Byte = BinReader.ReadByte 'DBF File Type
            Dim DBFHeader0103 As Byte() = BinReader.ReadBytes(3) 'Last Update
            Dim DBFHeader0407 As Int32 = BinReader.ReadInt32 'Number of records in file
            Dim DBFHeader0809 As Int16 = BinReader.ReadInt16 'Position of first data record
            Dim DBFHeader1011 As Int16 = BinReader.ReadInt16 'Length of one data record 
            Dim DBFHeader1227 As Byte() = BinReader.ReadBytes(16) 'Reserved
            Dim DBFHeader2828 As Byte = BinReader.ReadByte 'MDX flag for dBase IV
            Dim DBFHeader2929 As Byte = BinReader.ReadByte 'Code Page Mark (&h01=437, &h02=850, &h03=1252, &h64=852, &h65=865, &h66=866, &hc8=1250)
            Dim DBFHeader3031 As Byte() = BinReader.ReadBytes(2) 'Reserved


            Dim DBFSubRec0009(NumberOfColumns) As String ' = BinReader.ReadChars(10) 'Field Name has a length of 10, filled with &H00 if less
            Dim DBFSubRec1010(NumberOfColumns) As Byte ' = BinReader.ReadByte 'End sign &h00 of field name
            Dim DBFSubRec1111(NumberOfColumns) As String ' = BinReader.ReadByte 'Field type: C=char, Y=currency, N=Numeric, F=Float, D=Date, T=DateTime, B=Double, I=Integer, L=Logical, M=Memo, G=General, C=Character (binary), P=Picture
            Dim DBFSubRec1215(NumberOfColumns) As Int32 ' = BinReader.ReadInt32 'Field data address
            Dim DBFSubRec1616(NumberOfColumns) As Byte ' = BinReader.ReadByte 'Length of field
            Dim DBFSubRec1717(NumberOfColumns) As Byte ' = BinReader.ReadByte 'Number of decimal places
            Dim DBFSubRec1831(NumberOfColumns) As String ' = BinReader.ReadBytes(14) 'probably not usable for me

            Do 'Determine the columns
                ByteArray = BinReader.ReadBytes(10)

                'DBFSubRec0009(Counter) = BinReader.ReadChars(10) 'Field Name has a length of 10, filled with &H00 if less
                For I = 0 To UBound(ByteArray)
                    If Char.IsWhiteSpace(Convert.ToChar(ByteArray(I))) = True Or Char.IsControl(Convert.ToChar(ByteArray(I))) = True Then
                        ByteArray(I) = CByte(32)
                    End If
                Next
                DBFSubRec0009(Counter) = System.Text.Encoding.Default.GetString(ByteArray)

                'DBFSubRec0009(Counter) = System.Text.Encoding.UTF7.GetString(BinReader.ReadBytes(10)) 'Field Name has a length of 10, filled with &H00 if less

                DBFSubRec1010(Counter) = BinReader.ReadByte 'End sign &h00 of field name
                DBFSubRec1111(Counter) = BinReader.ReadChar 'Field type: C=char, Y=currency, N=Numeric, F=Float, D=Date, T=DateTime, B=Double, I=Integer, L=Logical
                DBFSubRec1215(Counter) = BinReader.ReadInt32 'Field data address
                DBFSubRec1616(Counter) = BinReader.ReadByte 'Length of field
                DBFSubRec1717(Counter) = BinReader.ReadByte 'Number of decimal places
                DBFSubRec1831(Counter) = System.Text.Encoding.Unicode.GetString(BinReader.ReadBytes(14)) 'probably not usable for me
                Counter += 1
            Loop Until BinReader.PeekChar = &HD
            Dim DBFHeader3232 As Byte = BinReader.ReadByte 'End of Header = &h0d

            Dim DBFRecord0000 As Byte '= BinReader.ReadByte 'Delete Mark for each record. &h2a=deleted, &h20=valid record
            Dim DBFRecord(MaxNumberRecords, Counter) As String

            Dim RawData As New StringBuilder("")
            Dim Temp00PatientFullName As String = ""
            Dim Temp01PatientID As String = ""
            Dim Temp02PatientDoB As String = ""
            Dim Temp03IMPPicture_TRTClinician As String = ""
            Dim Temp04IMPClinician_TRTField1 As String = ""
            Dim Temp05IMPField1_TRTField2 As String = ""
            Dim Temp06Position As String = ""

            Dim Temp07FilterCode As String = ""
            Dim Temp08AppCode As String = ""
            Dim Temp09TreatmentDate As String = ""
            Dim Temp10TreatmentTime As String = ""
            Dim Temp11FilterkV As String = ""
            Dim Temp12FiltermA As String = ""
            Dim Temp13FilterHVL As String = ""
            Dim Temp14ApplicatorData As String = ""
            Dim Temp15DoseAdmin As String = ""
            Dim Temp16TimeAdmin As String = ""
            Dim Temp17Operator As String = ""
            Dim Temp18DoseSet As String = ""
            Dim Temp19TimeSet As String = ""
            Dim Temp20Errors As String = ""

            BinReader.BaseStream.Position = DBFHeader0809


            For I = 0 To DBFHeader0407 - 1
                Temp00PatientFullName = ""
                Temp01PatientID = ""
                Temp02PatientDoB = ""
                Temp03IMPPicture_TRTClinician = ""
                Temp04IMPClinician_TRTField1 = ""
                Temp05IMPField1_TRTField2 = ""
                Temp06Position = ""
                Temp07FilterCode = ""
                Temp08AppCode = ""
                Temp09TreatmentDate = ""
                Temp10TreatmentTime = ""
                Temp11FilterkV = ""
                Temp12FiltermA = ""
                Temp13FilterHVL = ""
                Temp14ApplicatorData = ""
                Temp15DoseAdmin = ""
                Temp16TimeAdmin = ""
                Temp17Operator = ""
                Temp18DoseSet = ""
                Temp19TimeSet = ""
                Temp20Errors = ""

#If DEBUG Then
                If ShowDetails = True Then RawData.AppendLine(Format(I, "000000")).AppendLine(StrDup(60, "-"))
#End If


                DBFRecord0000 = BinReader.ReadByte 'Delete Mark for each record. &h2a=deleted, &h20=valid record
                'If DBFRecord0000 = &H2A Then
                '    'This Record was deleted
                'End If


                For II = 0 To Counter - 1 'Evaluate each records

                    DBFRecord(I, II) = System.Text.Encoding.UTF7.GetString(BinReader.ReadBytes(DBFSubRec1616(II)))
                    'remove unicodes
                    DBFRecord(I, II) = Regex.Replace(DBFRecord(I, II), "[^\u0000-\u00FF]", String.Empty).Trim

#If DEBUG Then
                    If ShowDetails = True Then
                        RawData.Append(DBFSubRec0009(II)).Append(" - ").AppendFormat("{0,4:N0}", DBFSubRec1111(II)).Append(" - ").AppendFormat("{0,2:N0}", DBFSubRec1616(II)).Append(" - ").AppendFormat("{0,2:N0}", DBFSubRec1717(II)).Append(" - ")
                    End If
#End If

                    Select Case True
                        Case (II = 0)
                            Temp00PatientFullName = DBFRecord(I, II)
#If DEBUG Then
                            If ShowDetails = True Then RawData.AppendLine("Name".PadRight(PadValue) & ": " & Temp00PatientFullName)
#End If
                        Case (II = 1)
                            Temp01PatientID = DBFRecord(I, II)
#If DEBUG Then
                            If ShowDetails = True Then RawData.AppendLine("ID".PadRight(PadValue) & ": " & Temp01PatientID)
#End If

                        Case (II = 2)
                            Temp02PatientDoB = DBFRecord(I, II)
                            If String.IsNullOrWhiteSpace(Temp02PatientDoB) = True Then Temp02PatientDoB = "01010101"
#If DEBUG Then
                            If ShowDetails = True Then RawData.AppendLine("Date of Birth".PadRight(PadValue) & ": " & Temp02PatientDoB)
#End If

#Region "Imperium01"
                        Case (II = 3) AndAlso (ClassicDatabase = XICL05_Enums.ClassicDatabaseType.Imperium)
                            Temp03IMPPicture_TRTClinician = DBFRecord(I, II)
#If DEBUG Then
                            If ShowDetails = True Then RawData.AppendLine("Picture".PadRight(PadValue) & ": " & Temp03IMPPicture_TRTClinician)
#End If

                        Case (II = 3) AndAlso (ClassicDatabase = XICL05_Enums.ClassicDatabaseType.PDSD3K)
                            Temp04IMPClinician_TRTField1 = DBFRecord(I, II)
#If DEBUG Then
                            If ShowDetails = True Then RawData.AppendLine("Clinician".PadRight(PadValue) & ": " & Temp04IMPClinician_TRTField1)
#End If

#End Region
#Region "Imperium02"
                        Case (II = 4) AndAlso (ClassicDatabase = XICL05_Enums.ClassicDatabaseType.Imperium)
                            Temp04IMPClinician_TRTField1 = DBFRecord(I, II)
#If DEBUG Then
                            If ShowDetails = True Then RawData.AppendLine("Clinician".PadRight(PadValue) & ": " & Temp04IMPClinician_TRTField1)
#End If

                        Case (II = 4) AndAlso (ClassicDatabase = XICL05_Enums.ClassicDatabaseType.PDSD3K)
                            Temp03IMPPicture_TRTClinician = DBFRecord(I, II)
#If DEBUG Then
                            If ShowDetails = True Then RawData.AppendLine("Field 1".PadRight(PadValue) & ": " & Temp03IMPPicture_TRTClinician)
#End If
#End Region
#Region "Imperium03"
                        Case (II = 5) AndAlso (ClassicDatabase = XICL05_Enums.ClassicDatabaseType.Imperium)
                            Temp05IMPField1_TRTField2 = DBFRecord(I, II)
#If DEBUG Then
                            If ShowDetails = True Then RawData.AppendLine("Field".PadRight(PadValue) & ": " & Temp05IMPField1_TRTField2)
#End If

                        Case (II = 5) AndAlso (ClassicDatabase = XICL05_Enums.ClassicDatabaseType.PDSD3K)
                            Temp05IMPField1_TRTField2 = DBFRecord(I, II)
#If DEBUG Then
                            If ShowDetails = True Then RawData.AppendLine("Field 2".PadRight(PadValue) & ": " & Temp05IMPField1_TRTField2)
#End If
#End Region

                        Case (II = 6)
                            Temp06Position = DBFRecord(I, II)
#If DEBUG Then
                            If ShowDetails = True Then RawData.AppendLine("Position".PadRight(PadValue) & ": " & Temp06Position)
#End If

                        Case (II = 7)
                            Temp07FilterCode = DBFRecord(I, II)
#If DEBUG Then
                            If ShowDetails = True Then RawData.AppendLine("Filter Code".PadRight(PadValue) & ": " & Temp07FilterCode)
#End If

                        Case (II = 8)
                            Temp08AppCode = DBFRecord(I, II)
#If DEBUG Then
                            If ShowDetails = True Then RawData.AppendLine("App Code".PadRight(PadValue) & ": " & Temp08AppCode)
#End If

                        Case (II = 9)
                            Temp09TreatmentDate = DBFRecord(I, II)
                            If String.IsNullOrWhiteSpace(Temp09TreatmentDate) = True Then Temp09TreatmentDate = "01010101"
#If DEBUG Then
                            If ShowDetails = True Then RawData.AppendLine("Treatment Date".PadRight(PadValue) & ": " & Temp09TreatmentDate)
#End If

                        Case (II = 10)
                            Temp10TreatmentTime = DBFRecord(I, II)
                            If String.IsNullOrWhiteSpace(Temp10TreatmentTime) = True Then Temp10TreatmentTime = "00:00:00"
#If DEBUG Then
                            If ShowDetails = True Then RawData.AppendLine("Treatment Time".PadRight(PadValue) & ": " & Temp10TreatmentTime)
#End If

                        Case (II = 11)
                            Temp11FilterkV = DBFRecord(I, II)
#If DEBUG Then
                            If ShowDetails = True Then RawData.AppendLine("kV".PadRight(PadValue) & ": " & Temp11FilterkV)
#End If

                        Case (II = 12)
                            Temp12FiltermA = DBFRecord(I, II)
#If DEBUG Then
                            If ShowDetails = True Then RawData.AppendLine("mA".PadRight(PadValue) & ": " & Temp12FiltermA)
#End If

                        Case (II = 13)
                            Temp13FilterHVL = DBFRecord(I, II)
#If DEBUG Then
                            If ShowDetails = True Then RawData.AppendLine("HVL".PadRight(PadValue) & ": " & Temp13FilterHVL)
#End If

                        Case (II = 14)
                            Temp14ApplicatorData = DBFRecord(I, II)
#If DEBUG Then
                            If ShowDetails = True Then RawData.AppendLine("App Data".PadRight(PadValue) & ": " & Temp14ApplicatorData)
#End If

                        Case (II = 15)
                            Temp15DoseAdmin = DBFRecord(I, II)
#If DEBUG Then
                            If ShowDetails = True Then RawData.AppendLine("Dose Admin".PadRight(PadValue) & ": " & Temp15DoseAdmin)
#End If

                        Case (II = 16)
                            Temp16TimeAdmin = DBFRecord(I, II)
#If DEBUG Then
                            If ShowDetails = True Then RawData.AppendLine("Time Admin".PadRight(PadValue) & ": " & Temp16TimeAdmin)
#End If

                        Case (II = 17)
                            Temp17Operator = DBFRecord(I, II)
#If DEBUG Then
                            If ShowDetails = True Then RawData.AppendLine("Operator".PadRight(PadValue) & ": " & Temp17Operator)
#End If

                        Case (II = 18)
                            Temp18DoseSet = DBFRecord(I, II)
#If DEBUG Then
                            If ShowDetails = True Then RawData.AppendLine("Dose Set".PadRight(PadValue) & ": " & Temp18DoseSet)
#End If

                        Case (II = 19)
                            Temp19TimeSet = DBFRecord(I, II)
#If DEBUG Then
                            If ShowDetails = True Then RawData.AppendLine("Time Set".PadRight(PadValue) & ": " & Temp19TimeSet)
#End If

                        Case (II = 20)
                            Temp20Errors = DBFRecord(I, II)
#If DEBUG Then
                            If ShowDetails = True Then RawData.AppendLine("Error(s)".PadRight(PadValue) & ": " & Temp20Errors)
#End If

                    End Select

                Next II

                'Dim RawDataString As String = RawData.ToString
                'Dim LocalCounter As Int32

                If (ClassicDatabase = XICL05_Enums.ClassicDatabaseType.Imperium And I > 0) Or
                    (ClassicDatabase = XICL05_Enums.ClassicDatabaseType.PDSD3K And I >= 0) Then 'because first record is column definition at Imperium
                    'LocalCounter += 1

                    MyImpTreatments.Add(New XInfoImperiumDB With {
                                                .DataBaseIndex = String.Concat(ClassicDatabase.ToString & "-" & Format(I, "0000000")),
                                                .PatientFullName = Temp00PatientFullName,
                                                .PatientID = Temp01PatientID,
                                                .PatientDoB = CStr(New DateTime(CInt(Val(Temp02PatientDoB.Substring(0, 4))), CInt(Val(Temp02PatientDoB.Substring(4, 2))), CInt(Val(Temp02PatientDoB.Substring(6, 2))), 0, 0, 0)),
                                                .PatientPicture = Temp03IMPPicture_TRTClinician,
                                                .Clinician = Temp04IMPClinician_TRTField1,
                                                .TreatmentField = Temp05IMPField1_TRTField2,
                                                .TreatmentPosition = Temp06Position,
                                                .FilterCode = CInt(Val(Temp07FilterCode)),
                                                .ApplicatorCode = Temp08AppCode,
                                                .TreatmentDateAndTime = New DateTime(CInt(Val(Temp09TreatmentDate.Substring(0, 4))), CInt(Val(Temp09TreatmentDate.Substring(4, 2))), CInt(Val(Temp09TreatmentDate.Substring(6, 2))), CInt(Val(Temp10TreatmentTime.Substring(0, 2))), CInt(Val(Temp10TreatmentTime.Substring(3, 2))), CInt(Val(Temp10TreatmentTime.Substring(6, 2)))),
                                                .FilterkV = CInt(Val(Temp11FilterkV)),
                                                .FiltermA = CSng(Val(Temp12FiltermA)),
                                                .FilterHVL = Temp13FilterHVL,
                                                .ApplicatorData = Temp14ApplicatorData,
                                                .DoseAdmin = Temp15DoseAdmin,
                                                .TimeAdmin = Temp16TimeAdmin,
                                                .TreatmentOperator = Temp17Operator,
                                                .DoseSet = Temp18DoseSet,
                                                .TimeSet = Temp19TimeSet,
                                                .GeneratorError = Temp20Errors,
                                                .HospitalName = HospitalName,
                                                .DepartmentName = DepartmentName,
                                                .SystemName = SystemName})
                End If

                If I / 2500 = I \ 2500 Then
                    ImperiumTotalCounter = 0 'This is just needed to trigger "PropertyChanged"
                    System.Windows.Forms.Application.DoEvents()
                End If

            Next I

            BinReader.Close()

            ImperiumTotalCounter = 0 'This is just needed to trigger "PropertyChanged"

#If DEBUG Then
            If ShowDetails = True Then Return RawData.ToString
#End If

            Return ""
        End If

        Return "File Not Found"

    End Function

    Private Sub CopyContentToFilter(sender As Object, e As MouseButtonEventArgs)

        Dim ChosenImperium As TextBlock = CType(sender, TextBlock)
        Dim ChosenImperiumValue As String = ""
        Dim Provider As CultureInfo = CultureInfo.InvariantCulture

        If sender Is TXTBLCK03_Imperium03 Then
            Dim DOB As New DateTime(CInt(MyImpSelectedTreatment.PatientDoB.Substring(0, 4)), CInt(MyImpSelectedTreatment.PatientDoB.Substring(4, 2)), CInt(MyImpSelectedTreatment.PatientDoB.Substring(6, 2)))
            ChosenImperiumValue = XstrahlCrypto.XstrahlFunctions.GetDateAsYYYYMMDD(DOB)
        ElseIf sender Is TXTBLCK03_Imperium10 Then
            ChosenImperiumValue = XstrahlCrypto.XstrahlFunctions.GetDateAsYYYYMMDD(MyImpSelectedTreatment.TreatmentDateAndTime)   'Format(Year(MyImpSelectedTreatment.TreatmentDateAndTime), "0000") & Format((MyImpSelectedTreatment.TreatmentDateAndTime), "00") & Format(DateAndTime.Day(MyImpSelectedTreatment.TreatmentDateAndTime), "00")
        Else
            ChosenImperiumValue = ChosenImperium.Text
        End If
        Me.TXTBX03_SearchValue.Text = ChosenImperiumValue

    End Sub

    Private Sub ExportAsXML(sender As Object, e As RoutedEventArgs)

        XInfoImperiumDB.SaveCollectionAsXML(MyImpSearchTreatments, ClassicDatabase.ToString)
        MessageBox.Show(XIRes.GetString("XINFO_UC03_024"), XIRes.GetString("XINFO_UC03_025"), MessageBoxButton.OK, MessageBoxImage.Information)

    End Sub

    Private Sub ExportAsCSV(sender As Object, e As RoutedEventArgs)

        Dim MyCSVLine As New StringBuilder
        Dim LeftRightChar As String = ""
        Dim CSVseparator As String = ""
        If Me.RDBTN03_CSVcomma.IsChecked = True Then
            CSVseparator = ","
        ElseIf Me.RDBTN03_CSVsemicolon.IsChecked = True Then
            CSVseparator = ";"
        End If

        If Me.CHKBX03_InvertedCommas.IsChecked = True Then
            LeftRightChar = Chr(34)
        End If

        MyCSVLine.Append(AddQuoteSign("TreatmentIndex", LeftRightChar) & CSVseparator)
        MyCSVLine.Append(AddQuoteSign("PatientFullName", LeftRightChar) & CSVseparator)
        MyCSVLine.Append(AddQuoteSign("PatientID", LeftRightChar) & CSVseparator)
        MyCSVLine.Append(AddQuoteSign("PatientDateOfBirth_ddMMyyyy", LeftRightChar) & CSVseparator)
        If ClassicDatabase = XICL05_Enums.ClassicDatabaseType.Imperium Then
            MyCSVLine.Append(AddQuoteSign("PatientPicture", LeftRightChar) & CSVseparator)
        ElseIf ClassicDatabase = XICL05_Enums.ClassicDatabaseType.PDSD3K Then
            MyCSVLine.Append(AddQuoteSign("TreatmentField1", LeftRightChar) & CSVseparator)
        End If
        MyCSVLine.Append(AddQuoteSign("Clinician", LeftRightChar) & CSVseparator)
        MyCSVLine.Append(AddQuoteSign("TreatmentDate_ddMMyyyy", LeftRightChar) & CSVseparator)
        MyCSVLine.Append(AddQuoteSign("TreatmentTime_HHmmss", LeftRightChar) & CSVseparator)
        MyCSVLine.Append(AddQuoteSign("FilterCode", LeftRightChar) & CSVseparator)
        MyCSVLine.Append(AddQuoteSign("FilterkV", LeftRightChar) & CSVseparator)
        MyCSVLine.Append(AddQuoteSign("FiltermA", LeftRightChar) & CSVseparator)
        MyCSVLine.Append(AddQuoteSign("FilterHVL", LeftRightChar) & CSVseparator)
        MyCSVLine.Append(AddQuoteSign("ApplicatorCode", LeftRightChar) & CSVseparator)
        MyCSVLine.Append(AddQuoteSign("ApplicatorData", LeftRightChar) & CSVseparator)
        MyCSVLine.Append(AddQuoteSign("DoseSet", LeftRightChar) & CSVseparator)
        MyCSVLine.Append(AddQuoteSign("DoseAdmin", LeftRightChar) & CSVseparator)
        MyCSVLine.Append(AddQuoteSign("TimeSet", LeftRightChar) & CSVseparator)
        MyCSVLine.Append(AddQuoteSign("TimeAdmin", LeftRightChar) & CSVseparator)
        If ClassicDatabase = XICL05_Enums.ClassicDatabaseType.Imperium Then
            MyCSVLine.Append(AddQuoteSign("TreatmentField", LeftRightChar) & CSVseparator)
        ElseIf ClassicDatabase = XICL05_Enums.ClassicDatabaseType.PDSD3K Then
            MyCSVLine.Append(AddQuoteSign("TreatmentField2", LeftRightChar) & CSVseparator)
        End If
        MyCSVLine.Append(AddQuoteSign("TreatmentPosition", LeftRightChar) & CSVseparator)
        MyCSVLine.Append(AddQuoteSign("TreatmentOperator", LeftRightChar) & CSVseparator)
        MyCSVLine.Append(AddQuoteSign("GeneratorError", LeftRightChar) & CSVseparator)
        MyCSVLine.Append(AddQuoteSign("HospitalName", LeftRightChar) & CSVseparator)
        MyCSVLine.Append(AddQuoteSign("DepartmentName", LeftRightChar) & CSVseparator)
        MyCSVLine.AppendLine(AddQuoteSign("SystemName", LeftRightChar))

        Dim ItemCounter As Int32 = 0
        For Each ImpTreatment In MyImpSearchTreatments
            ItemCounter += 1
            MyCSVLine.Append(AddQuoteSign(ItemCounter.ToString, LeftRightChar) & CSVseparator)
            MyCSVLine.Append(AddQuoteSign(ImpTreatment.PatientFullName, LeftRightChar) & CSVseparator)
            MyCSVLine.Append(AddQuoteSign(ImpTreatment.PatientID, LeftRightChar) & CSVseparator)
            'MyCSVLine.Append(AddQuoteSign(Format(DateAndTime.Day(ImpTreatment.PatientDoB), "00") & "." & Format(Month(ImpTreatment.PatientDoB), "00") & "." & Format(Year(ImpTreatment.PatientDoB), "0000"), LeftRightChar) & CSVseparator)
            Dim DOB As New DateTime(CInt(ImpTreatment.PatientDoB.Substring(0, 4)), CInt(ImpTreatment.PatientDoB.Substring(4, 2)), CInt(ImpTreatment.PatientDoB.Substring(6, 2)))
            'MyCSVLine.Append(AddQuoteSign(XstrahlCrypto.XstrahlFunctions.GetDateAsDDMMYYYYwithDot(ImpTreatment.PatientDoB), LeftRightChar) & CSVseparator)
            MyCSVLine.Append(AddQuoteSign(XstrahlCrypto.XstrahlFunctions.GetDateAsDDMMYYYYwithDot(DOB), LeftRightChar) & CSVseparator)

            MyCSVLine.Append(AddQuoteSign(ImpTreatment.PatientPicture, LeftRightChar) & CSVseparator)
            MyCSVLine.Append(AddQuoteSign(ImpTreatment.Clinician, LeftRightChar) & CSVseparator)
            'MyCSVLine.Append(AddQuoteSign(Format(DateAndTime.Day(ImpTreatment.TreatmentDateAndTime), "00") & "." & Format(Month(ImpTreatment.TreatmentDateAndTime), "00") & "." & Format(Year(ImpTreatment.TreatmentDateAndTime), "0000"), LeftRightChar) & CSVseparator)
            MyCSVLine.Append(AddQuoteSign(XstrahlCrypto.XstrahlFunctions.GetDateAsDDMMYYYYwithDot(ImpTreatment.TreatmentDateAndTime), LeftRightChar) & CSVseparator)
            'MyCSVLine.Append(AddQuoteSign(Format(Hour(ImpTreatment.TreatmentDateAndTime), "00") & ":" & Format(Minute(ImpTreatment.TreatmentDateAndTime), "00") & ":" & Format(Second(ImpTreatment.TreatmentDateAndTime), "00"), LeftRightChar) & CSVseparator)
            MyCSVLine.Append(AddQuoteSign(XstrahlCrypto.XstrahlFunctions.GetTimeAsHHMMSSwithColon(ImpTreatment.TreatmentDateAndTime), LeftRightChar) & CSVseparator)
            MyCSVLine.Append(AddQuoteSign(ImpTreatment.FilterCode.ToString, LeftRightChar) & CSVseparator)
            MyCSVLine.Append(AddQuoteSign(ImpTreatment.FilterkV.ToString, LeftRightChar) & CSVseparator)
            MyCSVLine.Append(AddQuoteSign(ImpTreatment.FiltermA.ToString, LeftRightChar) & CSVseparator)
            MyCSVLine.Append(AddQuoteSign(ImpTreatment.FilterHVL, LeftRightChar) & CSVseparator)
            MyCSVLine.Append(AddQuoteSign(ImpTreatment.ApplicatorCode, LeftRightChar) & CSVseparator)
            MyCSVLine.Append(AddQuoteSign(ImpTreatment.ApplicatorData, LeftRightChar) & CSVseparator)
            MyCSVLine.Append(AddQuoteSign(ImpTreatment.DoseSet, LeftRightChar) & CSVseparator)
            MyCSVLine.Append(AddQuoteSign(ImpTreatment.DoseAdmin, LeftRightChar) & CSVseparator)
            MyCSVLine.Append(AddQuoteSign(ImpTreatment.TimeSet, LeftRightChar) & CSVseparator)
            MyCSVLine.Append(AddQuoteSign(ImpTreatment.TimeAdmin, LeftRightChar) & CSVseparator)
            MyCSVLine.Append(AddQuoteSign(ImpTreatment.TreatmentField, LeftRightChar) & CSVseparator)
            MyCSVLine.Append(AddQuoteSign(ImpTreatment.TreatmentPosition, LeftRightChar) & CSVseparator)
            MyCSVLine.Append(AddQuoteSign(ImpTreatment.TreatmentOperator, LeftRightChar) & CSVseparator)
            MyCSVLine.Append(AddQuoteSign(ImpTreatment.GeneratorError, LeftRightChar) & CSVseparator)
            MyCSVLine.Append(AddQuoteSign(ImpTreatment.HospitalName, LeftRightChar) & CSVseparator)
            MyCSVLine.Append(AddQuoteSign(ImpTreatment.DepartmentName, LeftRightChar) & CSVseparator)
            MyCSVLine.AppendLine(AddQuoteSign(ImpTreatment.SystemName, LeftRightChar))
        Next

        Dim MyCSVFile As New StreamWriter(XICL04_ReadOnly.XInfoDirExportCSV & ClassicDatabase.ToString & "_Export_" & XstrahlCrypto.XstrahlFunctions.GetDateTimeAsYYYYMMDD_HHMMSS(Now) & ".csv", False, Encoding.Unicode)
        MyCSVFile.Write(MyCSVLine.ToString)
        MyCSVFile.Close()

        MessageBox.Show(XIRes.GetString("XINFO_UC03_024"), XIRes.GetString("XINFO_UC03_025"), MessageBoxButton.OK, MessageBoxImage.Information)

    End Sub

    Private Function AddQuoteSign(StringWithoutQuotes As String, LeftRightChar As String) As String

        If LeftRightChar = "" Then Return StringWithoutQuotes

        If StringWithoutQuotes Is Nothing = True Then
            StringWithoutQuotes = Chr(34) & Chr(34)
        End If
        If StringWithoutQuotes.StartsWith(Chr(34)) = False Then
            StringWithoutQuotes = Chr(34) & StringWithoutQuotes
        End If
        If StringWithoutQuotes.EndsWith(Chr(34)) = False Then
            StringWithoutQuotes &= Chr(34)
        End If
        If StringWithoutQuotes = Chr(34) Then 'In case the string is empty the above code would result in a single quote
            StringWithoutQuotes &= Chr(34)
        End If
        Return StringWithoutQuotes

    End Function

    Private Sub ExportAsXPS(sender As Object, e As RoutedEventArgs)

        If MyImpSearchTreatments.Count <= 0 Then Exit Sub

        If CHKBX03_ReportSummary.IsChecked = False Then

            Dim MyAnswer As MessageBoxResult
            MyAnswer = MessageBox.Show(String.Format(XIRes.GetString("XINFO_UC03_054"), MyImpSearchTreatments.Count, (MyImpSearchTreatments.Count * 100) \ 600).Replace("~", vbCrLf),
                                       XIRes.GetString("XINFO_UC03_063"),
                                       MessageBoxButton.YesNo,
                                       MessageBoxImage.Exclamation)
            If MyAnswer = MessageBoxResult.No Then Exit Sub


            MyUC = New XIUC99_Progress With {
                .ProgressStart = DateAndTime.Timer
            }

            Grid.SetRow(MyUC, 0)
            Grid.SetColumn(MyUC, 0)
            Grid.SetColumnSpan(MyUC, 99)
            Grid.SetRowSpan(MyUC, 99)
            MyUC.Margin = New Thickness(0)
            MyUC.HorizontalAlignment = Windows.HorizontalAlignment.Center
            MyUC.VerticalAlignment = VerticalAlignment.Center
            MyUC.Visibility = Visibility.Visible
            MyUC.Name = "UC99_XPSprogress"
            GRD03_Main.RegisterName(MyUC.Name, MyUC)
            GRD03_Main.Children.Add(MyUC)

            GRD03_DBBrowser.IsEnabled = False

            XInfoGeneral.XINFOmenu = False

            CurrentPrintItemCounter = MyImpSearchTreatments.Count
            If Not MyBackgrndWrkr.IsBusy = True Then
                MyBackgrndWrkr.RunWorkerAsync()
            End If
            Call ExportAsXPSDoWork()
            GRD03_DBBrowser.IsEnabled = True
            XInfoGeneral.XINFOmenu = True
        Else
            Call ExportAsXPSSummaryDoWork()
        End If

    End Sub

    Private Sub ExportAsXPSCancel(sender As Object, e As RoutedEventArgs)

        If MyBackgrndWrkr.WorkerSupportsCancellation = True Then
            MyBackgrndWrkr.CancelAsync()
        End If

    End Sub

    Private Sub ExportAsXPSProgressChanged(ByVal sender As Object, ByVal e As ProgressChangedEventArgs)

        Dim CurrentExpRate As Double = MyUC.GetDoneRate(MyImpSearchTreatments.Count - e.ProgressPercentage + 1, DateAndTime.Timer) '+1 needed to avoid division by zero error at the beginning!
        Dim FinishDate As DateTime = DateAndTime.DateAdd(DateInterval.Second, (e.ProgressPercentage / CurrentExpRate), Now)
        'Dim FinishDateAsSec As Double = (FinishDate - Today).TotalSeconds

        MyUC.LBL99_Info.Text = String.Format(XIRes.GetString("XINFO_UC99_002").Replace("~", vbCrLf),
                                                MyImpSearchTreatments.Count - e.ProgressPercentage - 1,
                                                MyImpSearchTreatments.Count,
                                                String.Format("{0:F2}", CurrentExpRate),
                                                XstrahlCrypto.XstrahlFunctions.GetTimeAsHHMMSSwithColon(FinishDate))

    End Sub

    Private Sub ExportAsXPSComplete(ByVal sender As Object, ByVal e As RunWorkerCompletedEventArgs)

        If e.Cancelled = True Then
            MessageBox.Show(XIRes.GetString("XINFO_UC03_201"), XIRes.GetString("XINFO_UC03_200"), MessageBoxButton.OK, MessageBoxImage.Information)
        ElseIf e.Error IsNot Nothing Then
            MessageBox.Show(XIRes.GetString("XINFO_UC03_202"), XIRes.GetString("XINFO_UC03_200"), MessageBoxButton.OK, MessageBoxImage.Exclamation)
        Else
            If Not MyBackgrndWrkr.IsBusy = True Then
                If CurrentPrintItemCounter > 0 Then
                    MyBackgrndWrkr.RunWorkerAsync()
                    Exit Sub
                ElseIf CurrentPrintItemCounter = -1 Then
                    'User has cancelled. This is not the cancel of the background worker, because the backgroundworker is always finished after each XPS export!
                    MessageBox.Show(XIRes.GetString("XINFO_UC03_203"), XIRes.GetString("XINFO_UC03_200"), MessageBoxButton.OK, MessageBoxImage.Exclamation)
                Else
                    MessageBox.Show(XIRes.GetString("XINFO_UC03_204"), XIRes.GetString("XINFO_UC03_200"), MessageBoxButton.OK, MessageBoxImage.Information)
                End If
            Else
                MessageBox.Show(XIRes.GetString("XINFO_UC03_205"), XIRes.GetString("XINFO_UC03_200"), MessageBoxButton.OK, MessageBoxImage.Exclamation)
            End If
        End If

        Try
            GRD03_Main.Children.Remove(CType(GRD03_Main.FindName("UC99_XPSprogress"), UIElement))
            GRD03_Main.UnregisterName("UC99_XPSprogress")
        Catch ex As Exception

        End Try

    End Sub

    Private Sub ShowProgressDoWork(sender As Object, e As DoWorkEventArgs)

        MyBackgrndWrkr.ReportProgress(CurrentPrintItemCounter)

    End Sub

    Private Sub ExportAsXPSDoWork()

        Dim XPSPathAndFileName As String = ""
        Dim XPSFileName As String = ""
        Dim MyDocument As FixedDocument
        Dim TreatmentDateTime As String = ""
        Dim MyFileInfo As FileInfo
        Dim DirInfoGroupFolder As DirectoryInfo

        Dim StartSecond As Double = DateAndTime.Timer

        Dim SelectedPaperSize As XICL05_Enums.Papersize = XICL05_Enums.Papersize.A4
        If RDBTN03_PapersizeA4.IsChecked = True Then
            SelectedPaperSize = XICL05_Enums.Papersize.A4
        Else
            SelectedPaperSize = XICL05_Enums.Papersize.Letter
        End If

        For DBItemIndex As Int32 = 0 To MyImpSearchTreatments.Count - 1
            CurrentPrintItemCounter -= 1

            If MyUC.CHKBX99_Cancel.IsChecked = True Then
                Exit For
            End If

            'Build the file name
            XPSFileName = MyImpSearchTreatments(DBItemIndex).CreateFilename(True, CChar("_"), "---")

            If RDBTN03_GroupID.IsChecked = True Then
                DirInfoGroupFolder = New DirectoryInfo(XICL04_ReadOnly.XInfoDirExportXPS & "PatID_" & MyImpSearchTreatments(DBItemIndex).CreateFolderNamePatID(True, CChar("_"), "---"))
            Else
                DirInfoGroupFolder = New DirectoryInfo(XICL04_ReadOnly.XInfoDirExportXPS & "TreatDate_" & MyImpSearchTreatments(DBItemIndex).CreateFolderNameTreatDate(True, CChar("_"), "---"))
            End If

            If DirInfoGroupFolder.Exists = False Then
                Try
                    DirInfoGroupFolder.Create()
                Catch ex As Exception
                End Try
            End If

            'Now concatenate everything 
            XPSPathAndFileName = DirInfoGroupFolder.FullName & "\" & XPSFileName & Format(DBItemIndex, "0000000") & ".xps"
            MyFileInfo = New FileInfo(XPSPathAndFileName)

            MyDocument = New FixedDocument
            MyDocument = XInfoImperiumDB.CreateFixedDocumentWithPages(MyImpSearchTreatments, DBItemIndex, 1, MyFileInfo.Name, SelectedPaperSize)

            Try
                Dim XLoopSettingsDoc = New XpsDocument(XPSPathAndFileName, FileAccess.Write, IO.Packaging.CompressionOption.SuperFast)
                Dim xpsdw As XpsDocumentWriter = XpsDocument.CreateXpsDocumentWriter(XLoopSettingsDoc)
                xpsdw.Write(MyDocument)
                XLoopSettingsDoc.Close()
            Catch ex As Exception
                MessageBox.Show(String.Format(XIRes.GetString("XINFO_UC03_206"), ex.Message), XIRes.GetString("XINFO_UC03_207"), MessageBoxButton.OK, MessageBoxImage.Exclamation)
                Exit For
            End Try

            If DBItemIndex / 50 = DBItemIndex \ 50 Then
                System.Windows.Forms.Application.DoEvents()
            End If
        Next

        'The following is necessary so that the backgroundworker reacts correctly
        If CurrentPrintItemCounter < MyImpSearchTreatments.Count Then
            CurrentPrintItemCounter = -1
        End If

    End Sub

    Private Sub ExportAsXPSSummaryDoWork()

        Dim SelectedPaperSize As XICL05_Enums.Papersize = XICL05_Enums.Papersize.A4
        If RDBTN03_PapersizeA4.IsChecked = True Then
            SelectedPaperSize = XICL05_Enums.Papersize.A4
        Else
            SelectedPaperSize = XICL05_Enums.Papersize.Letter
        End If

        Dim PageNrTotal As Int32 = CInt(Math.Ceiling(MyImpSearchTreatments.Count / 10))
        Dim FileNameSummaryReport As String

        If CHKBX03_ReportSummary.Tag.ToString = "PATID" Then
            'For all DBitems the ID is the same. Therefore it is sufficient to take the ID from the first record
            FileNameSummaryReport = XICL04_ReadOnly.XInfoDirExportXPS & "SummaryReport_" & CHKBX03_ReportSummary.Tag.ToString & "_" & MyImpSearchTreatments(0).CreateFolderNamePatID(True, CChar("_"), "---")
        ElseIf CHKBX03_ReportSummary.Tag.ToString = "TREATDATE" Then
            'For all DBitems the Treatment Date is the same. Therefore it is sufficient to take the Treatment Date from the first record
            FileNameSummaryReport = XICL04_ReadOnly.XInfoDirExportXPS & "SummaryReport_" & CHKBX03_ReportSummary.Tag.ToString & "_" & MyImpSearchTreatments(0).CreateFolderNameTreatDate(True, CChar("_"), "---")
        Else
            FileNameSummaryReport = XICL04_ReadOnly.XInfoDirExportXPS & "SummaryReport_" & MyImpSearchTreatments(0).CreateFolderNameTreatDate(True, CChar("_"), "---")
        End If

        FileNameSummaryReport &= "_" & XstrahlCrypto.XstrahlFunctions.GetDateTimeAsYYYYMMDD_HHMMSS(Now) & ".xps"
        Dim MyDocument As New FixedDocument
        MyDocument = XInfoImperiumDB.CreateFixedDocumentWithPages(MyImpSearchTreatments, -1, PageNrTotal, FileNameSummaryReport, SelectedPaperSize)

        Dim XLoopSettingsDoc = New XpsDocument(FileNameSummaryReport, FileAccess.Write, IO.Packaging.CompressionOption.SuperFast)
        Dim xpsdw As XpsDocumentWriter = XpsDocument.CreateXpsDocumentWriter(XLoopSettingsDoc)
        xpsdw.Write(MyDocument)
        XLoopSettingsDoc.Close()

    End Sub

    Private Sub ChangeDescriptionLabel(Sender As Object, e As RoutedEventArgs)

        If Sender Is RDBTN03_DbaseD3KTMNT Then
            RDBTN03_Imperium06.Content = XIRes.GetString("XINFO_UC03_106")
            RDBTN03_Imperium04.Content = XIRes.GetString("XINFO_UC03_104")
        ElseIf Sender Is RDBTN03_DbaseTREATMNT Then
            RDBTN03_Imperium06.Content = XIRes.GetString("XINFO_UC03_106_TRT")
            RDBTN03_Imperium04.Content = XIRes.GetString("XINFO_UC03_104_TRT")
        End If
    End Sub

    Private Sub ReadMeFirst(Sender As Object, e As RoutedEventArgs)

        Dim ShowHelpWindow As New XIWW002_ShowDBInfo
        ShowHelpWindow.ShowDialog()

    End Sub
End Class

Class MyTextBlock
    Inherits TextBlock

    ''' <summary>
    ''' Left as example in the code!
    ''' </summary>
    ''' <returns></returns>
    Public Shadows Property Foreground As Brush
        Get
            Return MyBase.Foreground
        End Get
        Set(value As Brush)
            MyBase.Foreground = value
        End Set
    End Property

    Public Sub New()
        MyBase.New

        Me.Background = Brushes.Transparent
        Me.Foreground = Brushes.Black
        Me.TextWrapping = TextWrapping.NoWrap
        Me.TextAlignment = TextAlignment.Left
        Me.VerticalAlignment = VerticalAlignment.Center
        Me.FontFamily = New System.Windows.Media.FontFamily("Arial")
        Me.FontSize = 10
        Me.FontWeight = FontWeights.Normal
        Me.FontStyle = FontStyles.Normal


    End Sub

End Class