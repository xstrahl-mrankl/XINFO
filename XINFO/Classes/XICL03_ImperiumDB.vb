Imports System.Text
Imports System.Collections.ObjectModel
Imports System.ComponentModel
Imports System.IO
Imports System.Runtime.CompilerServices
Imports System.Windows.Markup
Imports System.Xml.Serialization
Imports System.Globalization

Public Class XInfoImperiumDB
    Implements INotifyPropertyChanged

    Public Event PropertyChanged As PropertyChangedEventHandler Implements INotifyPropertyChanged.PropertyChanged
    Public Sub NotifyPropertyChanged(<CallerMemberName()> Optional ByVal propertyName As String = Nothing)
        RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs(propertyName))
    End Sub

    Private Shared PaperWidth As Single = XICL04_ReadOnly.PSA4Width * 96
    Private Shared PaperHeight As Single = XICL04_ReadOnly.PSA4Height * 96

#Region "Properties"

    Private _DataBaseIndex As String
    Private _PatientFullName As String
    Private _PatientID As String
    Private _PatientDoB As String
    Private _PatientPicture As String
    Private _Clinician As String
    Private _TreatmentDateAndTime As DateTime
    Private _FilterCode As Int32
    Private _FilterkV As Int32
    Private _FiltermA As Single
    Private _FilterHVL As String
    Private _ApplicatorCode As String
    Private _ApplicatorData As String
    Private _DoseSet As String
    Private _DoseAdmin As String
    Private _TimeSet As String
    Private _TimeAdmin As String
    Private _TreatmentField As String
    Private _TreatmentPosition As String
    Private _TreatmentOperator As String
    Private _GeneratorError As String

    Private _HospitalName As String
    Private _DepartmentName As String
    Private _SystemName As String

    Public Property DataBaseIndex As String
        Get
            Return _DataBaseIndex
        End Get
        Set(value As String)
            _DataBaseIndex = value
            NotifyPropertyChanged()
        End Set
    End Property

    Public Property PatientFullName As String
        Get
            Return _PatientFullName
        End Get
        Set(value As String)
            _PatientFullName = value.Trim
            NotifyPropertyChanged()
        End Set
    End Property

    Public Property PatientID As String
        Get
            Return _PatientID
        End Get
        Set(value As String)
            _PatientID = value.Trim
            NotifyPropertyChanged()
        End Set
    End Property

    <XmlIgnore>
    Public Property DoNotSerialize As DateTime


    Public Property PatientDoB As String
        Get
            Return DoNotSerialize.ToString("yyyyMMdd")
        End Get
        Set(value As String)
            If value.Substring(2, 1) = "." And value.Substring(5, 1) = "." Then
                value = value.Substring(6) & value.Substring(3, 2) & value.Substring(0, 2)
            ElseIf value.Contains("/") = True Then
                Dim DatumSplit() As String = value.Split("/"c)
                value = Format(Val(DatumSplit(2)), "0000") & Format(Val(DatumSplit(0)), "00") & Format(Val(DatumSplit(1)), "00")
            ElseIf value.Contains("-") = True Then
                Dim DatumSplit() As String = value.Split("-"c)
                value = Format(Val(DatumSplit(2)), "0000") & Format(Val(DatumSplit(0)), "00") & Format(Val(DatumSplit(1)), "00")
            End If
            DoNotSerialize = DateTime.ParseExact(value, "yyyyMMdd", CultureInfo.InvariantCulture)

            NotifyPropertyChanged()

        End Set
    End Property

    Public Property PatientPicture As String
        Get
            Return _PatientPicture
        End Get
        Set(value As String)
            _PatientPicture = value.Trim
            NotifyPropertyChanged()
        End Set
    End Property

    Public Property Clinician As String
        Get
            Return _Clinician
        End Get
        Set(value As String)
            _Clinician = value.Trim
            NotifyPropertyChanged()
        End Set
    End Property

    Public Property TreatmentDateAndTime As DateTime
        Get
            Return _TreatmentDateAndTime
        End Get
        Set(value As DateTime)
            _TreatmentDateAndTime = value
            NotifyPropertyChanged()
        End Set
    End Property

    Public Property FilterCode As Int32
        Get
            Return _FilterCode
        End Get
        Set(value As Int32)
            _FilterCode = value
            NotifyPropertyChanged()
        End Set
    End Property

    Public Property FilterkV As Int32
        Get
            Return _FilterkV
        End Get
        Set(value As Int32)
            _FilterkV = value
            NotifyPropertyChanged()
        End Set
    End Property

    Public Property FiltermA As Single
        Get
            Return _FiltermA
        End Get
        Set(value As Single)
            _FiltermA = value
            NotifyPropertyChanged()
        End Set
    End Property

    Public Property FilterHVL As String
        Get
            Return _FilterHVL
        End Get
        Set(value As String)
            _FilterHVL = value.Trim
            NotifyPropertyChanged()
        End Set
    End Property

    Public Property ApplicatorCode As String
        Get
            Return _ApplicatorCode
        End Get
        Set(value As String)
            _ApplicatorCode = value.Trim
            NotifyPropertyChanged()
        End Set
    End Property

    Public Property ApplicatorData As String
        Get
            Return _ApplicatorData
        End Get
        Set(value As String)
            _ApplicatorData = value.Trim
            NotifyPropertyChanged()
        End Set
    End Property

    Public Property DoseSet As String
        Get
            Return _DoseSet
        End Get
        Set(value As String)
            _DoseSet = value.Trim
            NotifyPropertyChanged()
        End Set
    End Property

    Public Property DoseAdmin As String
        Get
            Return _DoseAdmin
        End Get
        Set(value As String)
            _DoseAdmin = value.Trim
            NotifyPropertyChanged()
        End Set
    End Property

    Public Property TimeSet As String
        Get
            Return _TimeSet
        End Get
        Set(value As String)
            _TimeSet = value.Trim
            NotifyPropertyChanged()
        End Set
    End Property

    Public Property TimeAdmin As String
        Get
            Return _TimeAdmin
        End Get
        Set(value As String)
            _TimeAdmin = value.Trim
            NotifyPropertyChanged()
        End Set
    End Property

    Public Property TreatmentOperator As String
        Get
            Return _TreatmentOperator
        End Get
        Set(value As String)
            _TreatmentOperator = value.Trim
            NotifyPropertyChanged()
        End Set
    End Property

    Public Property TreatmentField As String
        Get
            Return _TreatmentField
        End Get
        Set(value As String)
            _TreatmentField = value.Trim
            NotifyPropertyChanged()
        End Set
    End Property

    Public Property TreatmentPosition As String
        Get
            Return _TreatmentPosition
        End Get
        Set(value As String)
            _TreatmentPosition = value.Trim
            NotifyPropertyChanged()
        End Set
    End Property

    Public Property GeneratorError As String
        Get
            Return _GeneratorError
        End Get
        Set(value As String)
            _GeneratorError = value.Trim
            NotifyPropertyChanged()
        End Set
    End Property

    Public Property HospitalName As String
        Get
            Return _HospitalName
        End Get
        Set(value As String)
            _HospitalName = value
            NotifyPropertyChanged()
        End Set
    End Property

    Public Property DepartmentName As String
        Get
            Return _DepartmentName
        End Get
        Set(value As String)
            _DepartmentName = value
            NotifyPropertyChanged()
        End Set
    End Property

    Public Property SystemName As String
        Get
            Return _SystemName
        End Get
        Set(value As String)
            _SystemName = value
            NotifyPropertyChanged()
        End Set
    End Property

#End Region

    Public Sub CopyItem(ByRef ItemToCopy As XInfoImperiumDB)

        DataBaseIndex = ItemToCopy.DataBaseIndex
        PatientFullName = ItemToCopy.PatientFullName
        PatientID = ItemToCopy.PatientID
        PatientDoB = ItemToCopy.PatientDoB
        PatientPicture = ItemToCopy.PatientPicture
        Clinician = ItemToCopy.Clinician
        TreatmentField = ItemToCopy.TreatmentField
        TreatmentPosition = ItemToCopy.TreatmentPosition
        FilterCode = ItemToCopy.FilterCode
        ApplicatorCode = ItemToCopy.ApplicatorCode
        TreatmentDateAndTime = ItemToCopy.TreatmentDateAndTime
        FilterkV = ItemToCopy.FilterkV
        FiltermA = ItemToCopy.FiltermA
        FilterHVL = ItemToCopy.FilterHVL
        ApplicatorData = ItemToCopy.ApplicatorData
        DoseAdmin = ItemToCopy.DoseAdmin
        TimeAdmin = ItemToCopy.TimeAdmin
        TreatmentOperator = ItemToCopy.TreatmentOperator
        DoseSet = ItemToCopy.DoseSet
        TimeSet = ItemToCopy.TimeSet
        GeneratorError = ItemToCopy.GeneratorError
        HospitalName = ItemToCopy.HospitalName
        DepartmentName = ItemToCopy.DepartmentName
        SystemName = ItemToCopy.SystemName

    End Sub

    Public Sub ResetItem()

        DataBaseIndex = ""
        PatientFullName = ""
        PatientID = ""
        PatientDoB = CStr(New DateTime(101, 1, 1))
        PatientPicture = ""
        Clinician = ""
        TreatmentField = ""
        TreatmentPosition = ""
        FilterCode = -1
        ApplicatorCode = "@"
        TreatmentDateAndTime = New DateTime(101, 1, 1, 0, 0, 0)
        FilterkV = 0
        FiltermA = 0
        FilterHVL = ""
        ApplicatorData = ""
        DoseAdmin = ""
        TimeAdmin = ""
        TreatmentOperator = ""
        DoseSet = ""
        TimeSet = ""
        GeneratorError = ""
        HospitalName = ""
        DepartmentName = ""
        SystemName = ""

    End Sub

    Public Shared Sub SaveCollectionAsXML(Templist As ObservableCollection(Of XInfoImperiumDB), FilenamePrefix As String)

        Dim xs As New XmlSerializer(Templist.GetType) ', "XICL03_ImperiumDB")

        System.Threading.Thread.Sleep(2000)

        'Dim tw As TextWriter = New StreamWriter(XICL04_ReadOnly.XInfoDirExportXML & FilenamePrefix & "_Export_" & XstrahlCrypto.XstrahlFunctions.GetDateTimeAsYYYYMMDD_HHMMSS(Now) & ".xml")
        'xs.Serialize(tw, Templist)

        Dim tw As New StreamWriter(XICL04_ReadOnly.XInfoDirExportXML & FilenamePrefix & "_Export_" & XstrahlCrypto.XstrahlFunctions.GetDateTimeAsYYYYMMDD_HHMMSS(Now) & ".xml", False, Encoding.Unicode)
        xs.Serialize(tw, Templist)
        tw.Close()

    End Sub

    Public Function CreateFilename(ReplaceBadChar As Boolean, ReplacementChar As Char, ItemSeparator As String) As String

        If ItemSeparator.Trim = "" Then ItemSeparator = "---"
        CreateFilename = PatientID & ItemSeparator
        CreateFilename &= XstrahlCrypto.XstrahlFunctions.GetDateAsYYYYMMDD(TreatmentDateAndTime)
        CreateFilename &= ItemSeparator
        CreateFilename &= XstrahlCrypto.XstrahlFunctions.GetTimeAsHHMMSS(TreatmentDateAndTime)
        CreateFilename &= ItemSeparator

        If ReplaceBadChar = True Then
            CreateFilename = String.Join("_", CreateFilename.Split(Path.GetInvalidFileNameChars()))
        End If

        Return CreateFilename

    End Function

    Public Function CreateFolderNamePatID(ReplaceBadChar As Boolean, ReplacementChar As Char, ItemSeparator As String) As String

        If ItemSeparator.Trim = "" Then ItemSeparator = "---"
        CreateFolderNamePatID = PatientID

        If ReplaceBadChar = True Then
            CreateFolderNamePatID = String.Join("_", CreateFolderNamePatID.Split(Path.GetInvalidFileNameChars()))
        End If

        Return CreateFolderNamePatID

    End Function

    Public Function CreateFolderNameTreatDate(ReplaceBadChar As Boolean, ReplacementChar As Char, ItemSeparator As String) As String

        If ItemSeparator.Trim = "" Then ItemSeparator = "---"
        CreateFolderNameTreatDate = XstrahlCrypto.XstrahlFunctions.GetDateAsYYYYMMDD(TreatmentDateAndTime)

        If ReplaceBadChar = True Then
            CreateFolderNameTreatDate = String.Join("_", CreateFolderNameTreatDate.Split(Path.GetInvalidFileNameChars()))
        End If

        Return CreateFolderNameTreatDate

    End Function




    Public Shared Function CreateFixedDocumentWithPages(ByRef TreatmentObject As ObservableCollection(Of XInfoImperiumDB), DBItemIndex As Int32, PageNrTotal As Int32, XPSFileNameInFooter As String, SelectedPaperSize As XICL05_Enums.Papersize) As FixedDocument
        Dim PageNrCurrent As Int32 = 1

        If SelectedPaperSize = XICL05_Enums.Papersize.A4 Then
            PaperWidth = XICL04_ReadOnly.PSA4Width * 96
            PaperHeight = XICL04_ReadOnly.PSA4Height * 96

        ElseIf SelectedPaperSize = XICL05_Enums.Papersize.Letter = True Then
            PaperWidth = XICL04_ReadOnly.PSLetterWidth * 96
            PaperHeight = XICL04_ReadOnly.PSLetterHeight * 96
        End If

        Dim MyFixedDocument As New FixedDocument
        MyFixedDocument = CreateFixedDocument(SelectedPaperSize)

        Dim MyFixedPage As FixedPage
        Dim MyFixedPageContent As PageContent

        'If DBItemIndex is set to -1 then ALL items shall be printed in the summary report
        If DBItemIndex >= 0 Then 'this is a single page report

            For PageNrCurrent = 1 To PageNrTotal
                MyFixedPage = New FixedPage
                MyFixedPage = CreatePageContent(TreatmentObject, DBItemIndex, XPSFileNameInFooter, PageNrCurrent, PageNrTotal, SelectedPaperSize) 'General1

                MyFixedPageContent = New PageContent
                CType(MyFixedPageContent, IAddChild).AddChild(MyFixedPage)

                MyFixedDocument.Pages.Add(MyFixedPageContent)
            Next
        ElseIf DBItemIndex = -1 Then 'this is a summary report
            For PageNrCurrent = 1 To PageNrTotal
                MyFixedPage = New FixedPage
                'for a summary report it doesn't matter from where to take the header information.
                'the header information is identical for all Database Records. Therefore I always use the first record with index 0 (zero)
                MyFixedPage = CreatePageContent(TreatmentObject, DBItemIndex, XPSFileNameInFooter, PageNrCurrent, PageNrTotal, SelectedPaperSize)

                MyFixedPageContent = New PageContent
                CType(MyFixedPageContent, IAddChild).AddChild(MyFixedPage)

                MyFixedDocument.Pages.Add(MyFixedPageContent)
            Next
        ElseIf DBItemIndex = -2 Then 'BLANK PAGE
            MyFixedPage = New FixedPage
            MyFixedPageContent = New PageContent
            CType(MyFixedPageContent, IAddChild).AddChild(MyFixedPage)

            MyFixedDocument.Pages.Add(MyFixedPageContent)
        End If

        Return MyFixedDocument

    End Function

    Private Shared Function CreateFixedDocument(ByRef SelectedPaperSize As XICL05_Enums.Papersize) As FixedDocument

        Dim MyFixedDocument As New FixedDocument
        MyFixedDocument.DocumentPaginator.PageSize = New Size(PaperWidth, PaperHeight)
        Return MyFixedDocument

    End Function

    Private Shared Function CreatePageContent(ByRef TreatmentObject As ObservableCollection(Of XInfoImperiumDB), ByRef DBItemIndex As Int32, ByRef XPSFileNameInFooter As String, ByRef PageNr As Int32, ByRef PageTotalNr As Int32, ByRef SelectedPaperSize As XICL05_Enums.Papersize) As FixedPage

        Dim MyPage As FixedPage = New FixedPage
        MyPage.Width = PaperWidth
        MyPage.Height = PaperHeight
        MyPage.Background = Brushes.White

        Dim PaperMarginLeft As Double = XICL06_Functions.ConvmmToPixel(XICL04_ReadOnly.MarginLeftMax, 96)
        Dim PaperMarginTop As Double = XICL06_Functions.ConvmmToPixel(XICL04_ReadOnly.MarginTopMax, 96)
        Dim PaperMarginRight As Double = XICL06_Functions.ConvmmToPixel(XICL04_ReadOnly.MarginRightMax, 96)
        Dim PaperMarginBottom As Double = XICL06_Functions.ConvmmToPixel(XICL04_ReadOnly.MarginBottomMax, 96)

        Dim PaperMarginWidth As Double = MyPage.Width - (PaperMarginLeft + PaperMarginRight)
        Dim PaperMarginHeight As Double = MyPage.Height - (PaperMarginTop + PaperMarginBottom)

        'The header will always be printed!
        Dim XInfoHeader As UIElement

        'Since the header information (hospital name etc) is the same for all objects, I always can take the information from the first record
        XInfoHeader = CreateHeaderAndFooter(TreatmentObject, 0, XPSFileNameInFooter, PageNr, PageTotalNr, PaperMarginWidth, PaperMarginHeight) 'here set the page number
        FixedPage.SetLeft(XInfoHeader, PaperMarginLeft)
        FixedPage.SetTop(XInfoHeader, PaperMarginTop)
        FixedPage.SetRight(XInfoHeader, PaperMarginRight)
        FixedPage.SetBottom(XInfoHeader, PaperMarginBottom)

        MyPage.Children.Add(XInfoHeader)

        Dim XInfoContent As UIElement

        If DBItemIndex >= 0 Then
            'Here is the content for a single page report
            XInfoContent = CreateContentSinglePage(TreatmentObject,
                                                  DBItemIndex,
                                                  PaperMarginWidth, PaperMarginHeight)
            FixedPage.SetLeft(XInfoContent, PaperMarginLeft)
            FixedPage.SetTop(XInfoContent, PaperMarginTop + XICL06_Functions.ConvmmToPixel(50 + 20, 96)) ' papermargin_top, 50=Height of header, 20 = height of title
            FixedPage.SetRight(XInfoContent, PaperMarginRight)
            FixedPage.SetBottom(XInfoContent, PaperMarginBottom + XICL06_Functions.ConvmmToPixel(10, 96))
            MyPage.Children.Add(XInfoContent)

        ElseIf DBItemIndex = -1 Then
            'Here is the content for a summary report
            Dim TopPos As Single = 0
            Dim HeightOfCanvas = (PaperMarginHeight - XICL06_Functions.ConvmmToPixel(50 + 20 + 10, 96)) / 10

            For DBitem = (PageNr - 1) * 10 To PageNr * 10 - 1
                If DBitem < TreatmentObject.Count Then

                    XInfoContent = CreateContentSummaryReport(TreatmentObject, DBitem, PaperMarginWidth, PaperMarginHeight)
                    FixedPage.SetLeft(XInfoContent, PaperMarginLeft)
                    FixedPage.SetTop(XInfoContent, PaperMarginTop + XICL06_Functions.ConvmmToPixel(50 + 20, 96) + (DBitem - (PageNr - 1) * 10) * HeightOfCanvas) ' papermargin_top, 50=Height of header, 20 = height of title
                    FixedPage.SetRight(XInfoContent, PaperMarginRight)
                    FixedPage.SetBottom(XInfoContent, PaperMarginBottom + XICL06_Functions.ConvmmToPixel(10, 96))
                    MyPage.Children.Add(XInfoContent)

                End If
            Next
        End If


        Return MyPage

    End Function

    Private Shared Function CreateHeaderAndFooter(ByRef TreatmentObject As ObservableCollection(Of XInfoImperiumDB), ByRef DBItemIndex As Int32, ByRef XPSFileNameInFooter As String, ByRef PageNr As Int32, ByRef PageTotalNr As Int32, ByRef PaperMarginWidth As Double, ByRef PaperMarginHeigth As Double) As Canvas

        Dim MyPageCanvas As Canvas = New Canvas With {.Width = PaperMarginWidth, .Height = PaperMarginHeigth}

        Dim CNVSHeaderLeft As Canvas = New Canvas With {.ClipToBounds = True, .Width = PaperMarginWidth / 2, .Height = XICL06_Functions.ConvmmToPixel(50, 96)}
        Dim CNVSHeaderRight As Canvas = New Canvas With {.ClipToBounds = True, .Width = PaperMarginWidth / 2, .Height = XICL06_Functions.ConvmmToPixel(50, 96)}
        Dim CNVSHeaderTitle As Canvas = New Canvas With {.ClipToBounds = True, .Width = PaperMarginWidth, .Height = XICL06_Functions.ConvmmToPixel(20, 96)}
        Dim CNVSFooter As Canvas = New Canvas With {.ClipToBounds = True, .Width = PaperMarginWidth, .Height = XICL06_Functions.ConvmmToPixel(10, 96)}
        CNVSHeaderLeft.Background = Brushes.Transparent
        CNVSHeaderRight.Background = Brushes.Transparent
        CNVSHeaderTitle.Background = Brushes.Transparent
        CNVSFooter.Background = Brushes.Transparent


        Dim TitleHeader As TextBlock = New MyTextBlock
        With TitleHeader
            .FontWeight = System.Windows.FontWeights.Bold
            .FontSize = 36
            .Text = XIRes.GetString("XINFO_UC03_300")
            .Margin = New Thickness(2)
            .Width = CNVSHeaderTitle.Width
            .TextAlignment = TextAlignment.Center
        End With
        CNVSHeaderTitle.Children.Add(TitleHeader)

        Dim XstrahlLogo As BitmapImage = New BitmapImage(MyXstrahlLogoURI)

        Dim MyImage As Image = New Image With {
            .Source = XstrahlLogo,
            .Height = XICL06_Functions.ConvmmToPixel(40, 96)
        }
        MyImage.Width = MyImage.Height
        MyImage.Margin = New Thickness(1, XICL06_Functions.ConvmmToPixel(9, 96), 0, 0)
        CNVSHeaderLeft.Children.Add(MyImage)

        Dim TitleHospitalName As TextBlock = New MyTextBlock
        With TitleHospitalName
            .FontWeight = System.Windows.FontWeights.Bold
            .FontSize = 16
            .Text = TreatmentObject(DBItemIndex).HospitalName '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
            .Margin = New Thickness(1)
            .Width = CNVSHeaderLeft.Width
            .TextAlignment = TextAlignment.Center
        End With
        CNVSHeaderLeft.Children.Add(TitleHospitalName)

        Dim HospitalDepartment As TextBlock = New MyTextBlock
        With HospitalDepartment
            .TextWrapping = TextWrapping.Wrap
            .FontWeight = System.Windows.FontWeights.Bold
            .FontSize = 11
            .Text = TreatmentObject(DBItemIndex).DepartmentName & vbCrLf & TreatmentObject(DBItemIndex).SystemName '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
            .Margin = New Thickness(XICL06_Functions.ConvmmToPixel(40, 96), XICL06_Functions.ConvmmToPixel(10, 96), 0, 0)
            .Width = CNVSHeaderLeft.Width - XICL06_Functions.ConvmmToPixel(40, 96)
        End With
        CNVSHeaderLeft.Children.Add(HospitalDepartment)

#Region "FOOTER LEFT"
        Dim FooterLeft As New MyTextBlock()
        With FooterLeft
            .Text = Application.ResourceAssembly.GetName.Name.ToString & " v" & FileVersionInfo.GetVersionInfo(System.Reflection.Assembly.GetExecutingAssembly.Location).ProductVersion
            FooterLeft.Margin = New Thickness(5, 0, 0, 0)
        End With

        Dim FooterLeftBorder As New Border With {
            .Height = XICL06_Functions.ConvmmToPixel(10, 96),
            .Width = PaperMarginWidth / 5,
            .Child = FooterLeft
        } 'I need a border so that the textblock centers vertically
        CNVSFooter.Children.Add(FooterLeftBorder)
#End Region
#Region "FOOTER CENTER"
        Dim FooterCenter As New MyTextBlock()
        With FooterCenter
            .Text = XPSFileNameInFooter
            .TextAlignment = TextAlignment.Center
            .TextWrapping = TextWrapping.Wrap
        End With

        Dim FooterCenterBorder As New Border With {
            .Height = XICL06_Functions.ConvmmToPixel(10, 96),
            .Child = FooterCenter,
            .Width = PaperMarginWidth * 3 / 5
        } 'I need a border so that the textblock centers vertically
        Canvas.SetLeft(FooterCenterBorder, PaperMarginWidth / 5)
        CNVSFooter.Children.Add(FooterCenterBorder)
#End Region
#Region "FOOTER RIGHT"
        Dim FooterRight As New MyTextBlock()
        With FooterRight
            .Text = XIRes.GetString("XINFO_UC03_301") & " " & PageNr & " / " & PageTotalNr
            .TextAlignment = TextAlignment.Right
            .Margin = New Thickness(0, 0, 5, 0)
        End With

        Dim FooterRightBorder As New Border With {
            .Height = XICL06_Functions.ConvmmToPixel(10, 96),
            .Child = FooterRight,
            .Width = PaperMarginWidth / 5
        } 'I need a border so that the textblock centers vertically
        Canvas.SetRight(FooterRightBorder, 0)
        CNVSFooter.Children.Add(FooterRightBorder)
#End Region


        Canvas.SetTop(CNVSHeaderLeft, 0)
        Canvas.SetLeft(CNVSHeaderLeft, 0)

        Canvas.SetTop(CNVSHeaderRight, 0)
        Canvas.SetLeft(CNVSHeaderRight, CNVSHeaderLeft.Width)

        Canvas.SetTop(CNVSHeaderTitle, Canvas.GetTop(CNVSHeaderLeft) + CNVSHeaderLeft.Height)
        Canvas.SetLeft(CNVSHeaderTitle, 0)

        Canvas.SetTop(CNVSFooter, MyPageCanvas.Height - XICL06_Functions.ConvmmToPixel(10, 96))
        Canvas.SetLeft(CNVSFooter, 0)



        Dim RectHeaderPartLeft As New Rectangle With {
            .Height = CNVSHeaderLeft.Height,
            .Width = CNVSHeaderLeft.Width,
            .Fill = Brushes.Transparent,
            .StrokeThickness = 1,
            .Stroke = Brushes.Black
        }
        CNVSHeaderLeft.Children.Add(RectHeaderPartLeft)

        Dim RectHeaderPartRight As New Rectangle With {
            .Height = CNVSHeaderRight.Height,
            .Width = CNVSHeaderRight.Width,
            .Fill = Brushes.Transparent,
            .StrokeThickness = 1,
            .Stroke = Brushes.Black
        }
        CNVSHeaderRight.Children.Add(RectHeaderPartRight)

        Dim RectFooter As New Rectangle With {
            .Height = CNVSFooter.Height,
            .Width = CNVSFooter.Width,
            .Fill = Brushes.Transparent,
            .StrokeThickness = 1,
            .Stroke = Brushes.Black
        }
        CNVSFooter.Children.Add(RectFooter)

        MyPageCanvas.Children.Add(CNVSHeaderLeft)
        MyPageCanvas.Children.Add(CNVSHeaderRight)
        MyPageCanvas.Children.Add(CNVSHeaderTitle)
        MyPageCanvas.Children.Add(CNVSFooter)

        Dim RectPage As New Rectangle With {
            .Height = PaperMarginHeigth,
            .Width = PaperMarginWidth,
            .Fill = Brushes.Transparent,
            .StrokeThickness = 1,
            .Stroke = Brushes.Black
        }

        MyPageCanvas.Children.Add(RectPage)

        Return MyPageCanvas

    End Function

    Private Shared Function CreateContentSinglePage(ByRef TreatmentObject As ObservableCollection(Of XInfoImperiumDB), ByRef DBItemIndex As Int32, ByRef PaperMarginWidth As Double, ByRef PaperMarginHeigth As Double) As Canvas

        Dim MyPageCanvas As Canvas = New Canvas With {.Width = PaperMarginWidth, .Height = PaperMarginHeigth - XICL06_Functions.ConvmmToPixel(50 + 20 + 10, 96)}

        Dim CNVSContentLeft As Canvas = New Canvas With {.ClipToBounds = True, .Width = PaperMarginWidth * 2 / 5, .Height = MyPageCanvas.Height}
        Dim CNVSContentRight As Canvas = New Canvas With {.ClipToBounds = True, .Width = PaperMarginWidth * 3 / 5, .Height = MyPageCanvas.Height}
        CNVSContentLeft.Background = Brushes.Transparent
        CNVSContentRight.Background = Brushes.Transparent

        Dim MyStackpanelLeft As New StackPanel With {
            .Margin = New Thickness(5, 0, 5, 0),
            .Width = CNVSContentLeft.Width
        }

        Dim MyStackpanelRight As New StackPanel With {
            .Margin = New Thickness(25, 0, 5, 0),
            .Width = CNVSContentRight.Width
        }


        Dim MyCollectionOfLabels As New Collection
        Dim MyCollectionOfValues As New Collection
        Dim MyTxtblckImpLabel As MyTextBlock
        Dim MyTxtblckImpValue As MyTextBlock

        For I = 0 To 21
            MyTxtblckImpLabel = New MyTextBlock With {
                .Name = "DBLabel" & Format(I, "000"),
                .FontSize = 12,
                .TextAlignment = TextAlignment.Right
            }

            MyTxtblckImpValue = New MyTextBlock With {
                .Name = "DBValue" & Format(I, "000"),
                .FontSize = 12,
                .TextAlignment = TextAlignment.Left
            }

            Select Case I
                Case 0 'ImperiumIndex
                    MyTxtblckImpLabel.Text = XIRes.GetString("XINFO_UC03_100")
                    MyTxtblckImpValue.Text = TreatmentObject(DBItemIndex).DataBaseIndex

                Case 1 'ID
                    MyTxtblckImpLabel.Text = XIRes.GetString("XINFO_UC03_102")
                    MyTxtblckImpValue.Text = TreatmentObject(DBItemIndex).PatientID

                Case 2 'Full Name
                    MyTxtblckImpLabel.Text = XIRes.GetString("XINFO_UC03_101")
                    MyTxtblckImpValue.Text = TreatmentObject(DBItemIndex).PatientFullName

                Case 3 'DoB
                    MyTxtblckImpLabel.Text = XIRes.GetString("XINFO_UC03_103")
                    MyTxtblckImpValue.Text = XstrahlCrypto.XstrahlFunctions.GetDateAsDDMMYYYYwithDot(DateTime.ParseExact(TreatmentObject(DBItemIndex).PatientDoB, "yyyyMMdd",CultureInfo.InvariantCulture))

                Case 4 'Picture
                    If TreatmentObject(DBItemIndex).DataBaseIndex.StartsWith(XICL05_Enums.ClassicDatabaseType.Imperium.ToString) = True Then
                        MyTxtblckImpLabel.Text = XIRes.GetString("XINFO_UC03_104")
                        MyTxtblckImpValue.Text = TreatmentObject(DBItemIndex).PatientPicture
                    ElseIf TreatmentObject(DBItemIndex).DataBaseIndex.StartsWith(XICL05_Enums.ClassicDatabaseType.PDSD3K.ToString) = True Then
                        MyTxtblckImpLabel.Text = XIRes.GetString("XINFO_UC03_104_TRT")
                        MyTxtblckImpValue.Text = TreatmentObject(DBItemIndex).PatientPicture
                    End If

                Case 5 'Clinician
                    MyTxtblckImpLabel.Text = XIRes.GetString("XINFO_UC03_105")
                    MyTxtblckImpValue.Text = TreatmentObject(DBItemIndex).Clinician

                Case 6 'Treatment Date
                    MyTxtblckImpLabel.Text = XIRes.GetString("XINFO_UC03_110")
                    MyTxtblckImpValue.Text = XstrahlCrypto.XstrahlFunctions.GetDateAsDDMMYYYYwithDot(TreatmentObject(DBItemIndex).TreatmentDateAndTime)

                Case 7 'Treatment Time
                    MyTxtblckImpLabel.Text = XIRes.GetString("XINFO_UC03_111")
                    MyTxtblckImpValue.Text = XstrahlCrypto.XstrahlFunctions.GetTimeAsHHMMSSwithColon(TreatmentObject(DBItemIndex).TreatmentDateAndTime)

                Case 8 'Treatment Field
                    If TreatmentObject(DBItemIndex).DataBaseIndex.StartsWith(XICL05_Enums.ClassicDatabaseType.Imperium.ToString) = True Then
                        MyTxtblckImpLabel.Text = XIRes.GetString("XINFO_UC03_106")
                        MyTxtblckImpValue.Text = TreatmentObject(DBItemIndex).TreatmentField
                    ElseIf TreatmentObject(DBItemIndex).DataBaseIndex.StartsWith(XICL05_Enums.ClassicDatabaseType.PDSD3K.ToString) = True Then
                        MyTxtblckImpLabel.Text = XIRes.GetString("XINFO_UC03_106_TRT")
                        MyTxtblckImpValue.Text = TreatmentObject(DBItemIndex).TreatmentField
                    End If

                Case 9 'Treatment Position
                    MyTxtblckImpLabel.Text = XIRes.GetString("XINFO_UC03_107")
                    MyTxtblckImpValue.Text = TreatmentObject(DBItemIndex).TreatmentPosition

                Case 10 'Filter Code
                    MyTxtblckImpLabel.Text = XIRes.GetString("XINFO_UC03_108")
                    MyTxtblckImpValue.Text = TreatmentObject(DBItemIndex).FilterCode.ToString

                Case 11 ' FIlter kV
                    MyTxtblckImpLabel.Text = XIRes.GetString("XINFO_UC03_112")
                    MyTxtblckImpValue.Text = TreatmentObject(DBItemIndex).FilterkV.ToString

                Case 12 'Filter mA
                    MyTxtblckImpLabel.Text = XIRes.GetString("XINFO_UC03_113")
                    MyTxtblckImpValue.Text = TreatmentObject(DBItemIndex).FiltermA.ToString

                Case 13 'Filter HVL
                    MyTxtblckImpLabel.Text = XIRes.GetString("XINFO_UC03_114")
                    MyTxtblckImpValue.Text = TreatmentObject(DBItemIndex).FilterHVL

                Case 14 'Applicator Code
                    MyTxtblckImpLabel.Text = XIRes.GetString("XINFO_UC03_109")
                    MyTxtblckImpValue.Text = TreatmentObject(DBItemIndex).ApplicatorCode

                Case 15 'Applicator Data
                    MyTxtblckImpLabel.Text = XIRes.GetString("XINFO_UC03_115")
                    MyTxtblckImpValue.Text = TreatmentObject(DBItemIndex).ApplicatorData

                Case 16 'Dose Set
                    MyTxtblckImpLabel.Text = XIRes.GetString("XINFO_UC03_116")
                    MyTxtblckImpValue.Text = TreatmentObject(DBItemIndex).DoseSet

                Case 17 'Dose Admin
                    MyTxtblckImpLabel.Text = XIRes.GetString("XINFO_UC03_119")
                    MyTxtblckImpValue.Text = TreatmentObject(DBItemIndex).DoseAdmin

                Case 18 'Time Set
                    MyTxtblckImpLabel.Text = XIRes.GetString("XINFO_UC03_117")
                    MyTxtblckImpValue.Text = TreatmentObject(DBItemIndex).TimeSet

                Case 19 'Time Admin
                    MyTxtblckImpLabel.Text = XIRes.GetString("XINFO_UC03_120")
                    MyTxtblckImpValue.Text = TreatmentObject(DBItemIndex).TimeAdmin

                Case 20 'Operator
                    MyTxtblckImpLabel.Text = XIRes.GetString("XINFO_UC03_118")
                    MyTxtblckImpValue.Text = TreatmentObject(DBItemIndex).TreatmentOperator

                Case 21 'Error Codes
                    MyTxtblckImpLabel.Text = XIRes.GetString("XINFO_UC03_121")
                    MyTxtblckImpValue.Text = TreatmentObject(DBItemIndex).GeneratorError


            End Select

            MyCollectionOfLabels.Add(MyTxtblckImpLabel, MyTxtblckImpLabel.Name)
            MyCollectionOfValues.Add(MyTxtblckImpValue, MyTxtblckImpValue.Name)

            MyTxtblckImpLabel.Margin = New Thickness(0, 10, 5, 0)
            MyTxtblckImpValue.Margin = New Thickness(0, 10, 5, 0)
        Next

        'Here I can determine the order of elements. Must be the same for LABEL and VALUE
        MyStackpanelLeft.Children.Add(CType(MyCollectionOfLabels("DBLabel000"), UIElement))
        MyStackpanelLeft.Children.Add(CType(MyCollectionOfLabels("DBLabel001"), UIElement))
        MyStackpanelLeft.Children.Add(CType(MyCollectionOfLabels("DBLabel002"), UIElement))
        MyStackpanelLeft.Children.Add(CType(MyCollectionOfLabels("DBLabel003"), UIElement))
        MyStackpanelLeft.Children.Add(CType(MyCollectionOfLabels("DBLabel005"), UIElement))
        MyStackpanelLeft.Children.Add(CType(MyCollectionOfLabels("DBLabel006"), UIElement))
        MyStackpanelLeft.Children.Add(CType(MyCollectionOfLabels("DBLabel007"), UIElement))
        MyStackpanelLeft.Children.Add(CType(MyCollectionOfLabels("DBLabel004"), UIElement))
        MyStackpanelLeft.Children.Add(CType(MyCollectionOfLabels("DBLabel008"), UIElement))
        MyStackpanelLeft.Children.Add(CType(MyCollectionOfLabels("DBLabel009"), UIElement))
        MyStackpanelLeft.Children.Add(CType(MyCollectionOfLabels("DBLabel010"), UIElement))
        MyStackpanelLeft.Children.Add(CType(MyCollectionOfLabels("DBLabel011"), UIElement))
        MyStackpanelLeft.Children.Add(CType(MyCollectionOfLabels("DBLabel012"), UIElement))
        MyStackpanelLeft.Children.Add(CType(MyCollectionOfLabels("DBLabel013"), UIElement))
        MyStackpanelLeft.Children.Add(CType(MyCollectionOfLabels("DBLabel014"), UIElement))
        MyStackpanelLeft.Children.Add(CType(MyCollectionOfLabels("DBLabel015"), UIElement))
        MyStackpanelLeft.Children.Add(CType(MyCollectionOfLabels("DBLabel016"), UIElement))
        MyStackpanelLeft.Children.Add(CType(MyCollectionOfLabels("DBLabel017"), UIElement))
        MyStackpanelLeft.Children.Add(CType(MyCollectionOfLabels("DBLabel018"), UIElement))
        MyStackpanelLeft.Children.Add(CType(MyCollectionOfLabels("DBLabel019"), UIElement))
        MyStackpanelLeft.Children.Add(CType(MyCollectionOfLabels("DBLabel020"), UIElement))
        MyStackpanelLeft.Children.Add(CType(MyCollectionOfLabels("DBLabel021"), UIElement))

        MyStackpanelRight.Children.Add(CType(MyCollectionOfValues("DBValue000"), UIElement))
        MyStackpanelRight.Children.Add(CType(MyCollectionOfValues("DBValue001"), UIElement))
        MyStackpanelRight.Children.Add(CType(MyCollectionOfValues("DBValue002"), UIElement))
        MyStackpanelRight.Children.Add(CType(MyCollectionOfValues("DBValue003"), UIElement))
        MyStackpanelRight.Children.Add(CType(MyCollectionOfValues("DBValue005"), UIElement))
        MyStackpanelRight.Children.Add(CType(MyCollectionOfValues("DBValue006"), UIElement))
        MyStackpanelRight.Children.Add(CType(MyCollectionOfValues("DBValue007"), UIElement))
        MyStackpanelRight.Children.Add(CType(MyCollectionOfValues("DBValue004"), UIElement))
        MyStackpanelRight.Children.Add(CType(MyCollectionOfValues("DBValue008"), UIElement))
        MyStackpanelRight.Children.Add(CType(MyCollectionOfValues("DBValue009"), UIElement))
        MyStackpanelRight.Children.Add(CType(MyCollectionOfValues("DBValue010"), UIElement))
        MyStackpanelRight.Children.Add(CType(MyCollectionOfValues("DBValue011"), UIElement))
        MyStackpanelRight.Children.Add(CType(MyCollectionOfValues("DBValue012"), UIElement))
        MyStackpanelRight.Children.Add(CType(MyCollectionOfValues("DBValue013"), UIElement))
        MyStackpanelRight.Children.Add(CType(MyCollectionOfValues("DBValue014"), UIElement))
        MyStackpanelRight.Children.Add(CType(MyCollectionOfValues("DBValue015"), UIElement))
        MyStackpanelRight.Children.Add(CType(MyCollectionOfValues("DBValue016"), UIElement))
        MyStackpanelRight.Children.Add(CType(MyCollectionOfValues("DBValue017"), UIElement))
        MyStackpanelRight.Children.Add(CType(MyCollectionOfValues("DBValue018"), UIElement))
        MyStackpanelRight.Children.Add(CType(MyCollectionOfValues("DBValue019"), UIElement))
        MyStackpanelRight.Children.Add(CType(MyCollectionOfValues("DBValue020"), UIElement))
        MyStackpanelRight.Children.Add(CType(MyCollectionOfValues("DBValue021"), UIElement))



        CNVSContentLeft.Children.Add(MyStackpanelLeft)
        CNVSContentRight.Children.Add(MyStackpanelRight)


        Canvas.SetTop(CNVSContentLeft, 0)
        Canvas.SetLeft(CNVSContentLeft, 0)

        Canvas.SetTop(CNVSContentRight, 0)
        Canvas.SetLeft(CNVSContentRight, CNVSContentLeft.Width)

        Canvas.SetTop(MyPageCanvas, XICL06_Functions.ConvmmToPixel(70, 96))
        Canvas.SetLeft(MyPageCanvas, 0)

        MyPageCanvas.Children.Add(CNVSContentLeft)
        MyPageCanvas.Children.Add(CNVSContentRight)

        Return MyPageCanvas

    End Function

    Private Shared Function CreateContentSummaryReport(ByRef TreatmentObject As ObservableCollection(Of XInfoImperiumDB), ByRef DBitem As Int32, ByRef PaperMarginWidth As Double, ByRef PaperMarginHeight As Double) As Canvas

#Region "Canvas Definition"
        Dim MyPageCanvas As Canvas = New Canvas With {.Width = PaperMarginWidth, .Height = (PaperMarginHeight - XICL06_Functions.ConvmmToPixel(50 + 20 + 10, 96))}
        'using 10 items per page -> height = height \ 10

        'Are also used below to position each Canvas!
        Dim WidthCanvasLeft As Decimal = CDec(0.35)
        Dim WidthCanvasMiddle As Decimal = CDec(0.2)
        Dim WidthCanvasRight As Decimal = CDec(0.45)

        Dim CNVSContentLeft As Canvas = New Canvas With {.ClipToBounds = True, .Width = PaperMarginWidth * WidthCanvasLeft, .Height = MyPageCanvas.Height / 10}
        CNVSContentLeft.Background = Brushes.Transparent
        Dim CNVSContentMiddle As Canvas = New Canvas With {.ClipToBounds = True, .Width = PaperMarginWidth * WidthCanvasMiddle, .Height = MyPageCanvas.Height / 10}
        CNVSContentMiddle.Background = Brushes.Transparent
        Dim CNVSContentRight As Canvas = New Canvas With {.ClipToBounds = True, .Width = PaperMarginWidth * WidthCanvasRight, .Height = MyPageCanvas.Height / 10}
        CNVSContentRight.Background = Brushes.Transparent
#End Region

#Region "Stackpanel Definition"
        Dim MyStackpanelLeft As New StackPanel With {
            .Margin = New Thickness(5, 0, 5, 0),
            .Width = CNVSContentLeft.Width,
            .Orientation = Controls.Orientation.Horizontal
        }
        Dim MyStackpanelLeftL As New StackPanel With {
            .Margin = New Thickness(0, 2, 5, 2),
            .Orientation = Controls.Orientation.Vertical
        }
        Dim MyStackpanelLeftR As New StackPanel With {
            .Margin = New Thickness(0, 2, 0, 2),
            .Orientation = Controls.Orientation.Vertical
        }
        MyStackpanelLeft.Children.Add(MyStackpanelLeftL)
        MyStackpanelLeft.Children.Add(MyStackpanelLeftR)

        Dim MyStackpanelMiddle As New StackPanel With {
            .Margin = New Thickness(5, 0, 5, 0),
            .Width = CNVSContentMiddle.Width,
            .Orientation = Orientation.Horizontal
        }
        Dim MyStackpanelMiddleL As New StackPanel With {
            .Margin = New Thickness(0, 2, 5, 2),
            .Orientation = Controls.Orientation.Vertical
        }
        Dim MyStackpanelMiddleR As New StackPanel With {
            .Margin = New Thickness(0, 2, 0, 2),
            .Orientation = Controls.Orientation.Vertical
        }
        MyStackpanelMiddle.Children.Add(MyStackpanelMiddleL)
        MyStackpanelMiddle.Children.Add(MyStackpanelMiddleR)

        Dim MyStackpanelRight As New StackPanel With {
            .Margin = New Thickness(5, 0, 5, 0),
            .Width = CNVSContentRight.Width,
            .Orientation = Controls.Orientation.Horizontal
        }
        Dim MyStackpanelRightL As New StackPanel With {
            .Margin = New Thickness(0, 2, 5, 2),
            .Orientation = Controls.Orientation.Vertical
        }
        Dim MyStackpanelRightR As New StackPanel With {
            .Margin = New Thickness(0, 2, 0, 2),
            .Orientation = Controls.Orientation.Vertical
        }
        MyStackpanelRight.Children.Add(MyStackpanelRightL)
        MyStackpanelRight.Children.Add(MyStackpanelRightR)
#End Region

        For I = 1 To 21
            Dim MyTxtblckImpLabel As New MyTextBlock With {
                .Name = "DBLabel" & Format(I, "000"),
                .FontSize = 8,
                .TextAlignment = TextAlignment.Right
            }

            Dim MyTxtblckImpValue As New MyTextBlock With {
                .Name = "DBValue" & Format(I, "000"),
                .FontSize = 8,
                .TextAlignment = TextAlignment.Left
            }

            Select Case I
                Case 1 'ID
                    MyTxtblckImpLabel.Text = XIRes.GetString("XINFO_UC03_102")
                    MyTxtblckImpValue.Text = TreatmentObject(DBitem).PatientID

                Case 2 'Full Name
                    MyTxtblckImpLabel.Text = XIRes.GetString("XINFO_UC03_101")
                    MyTxtblckImpValue.Text = TreatmentObject(DBitem).PatientFullName

                Case 3 'DoB
                    MyTxtblckImpLabel.Text = XIRes.GetString("XINFO_UC03_103")
                    Dim DOB As New DateTime(CInt(TreatmentObject(DBitem).PatientDoB.Substring(0, 4)), CInt(TreatmentObject(DBitem).PatientDoB.Substring(4, 2)), CInt(TreatmentObject(DBitem).PatientDoB.Substring(6, 2)))
                    MyTxtblckImpValue.Text = XstrahlCrypto.XstrahlFunctions.GetDateAsDDMMYYYYwithDot(DOB)

                Case 4 'Picture
                    If TreatmentObject(DBitem).DataBaseIndex.StartsWith(XICL05_Enums.ClassicDatabaseType.Imperium.ToString) = True Then
                        MyTxtblckImpLabel.Text = XIRes.GetString("XINFO_UC03_104")
                        MyTxtblckImpValue.Text = TreatmentObject(DBitem).PatientPicture
                    ElseIf TreatmentObject(DBitem).DataBaseIndex.StartsWith(XICL05_Enums.ClassicDatabaseType.PDSD3K.ToString) = True Then
                        MyTxtblckImpLabel.Text = XIRes.GetString("XINFO_UC03_104_TRT")
                        MyTxtblckImpValue.Text = TreatmentObject(DBitem).PatientPicture
                    End If
                Case 5 'Clinician
                    MyTxtblckImpLabel.Text = XIRes.GetString("XINFO_UC03_105")
                    MyTxtblckImpValue.Text = TreatmentObject(DBitem).Clinician

                Case 6 'Treatment Date
                    MyTxtblckImpLabel.Text = XIRes.GetString("XINFO_UC03_110")
                    MyTxtblckImpValue.Text = XstrahlCrypto.XstrahlFunctions.GetDateAsDDMMYYYYwithDot(TreatmentObject(DBitem).TreatmentDateAndTime)

                Case 7 'Treatment Time
                    MyTxtblckImpLabel.Text = XIRes.GetString("XINFO_UC03_111")
                    MyTxtblckImpValue.Text = XstrahlCrypto.XstrahlFunctions.GetTimeAsHHMMSSwithColon(TreatmentObject(DBitem).TreatmentDateAndTime)

                Case 8 'Treatment Field
                    If TreatmentObject(DBitem).DataBaseIndex.StartsWith(XICL05_Enums.ClassicDatabaseType.Imperium.ToString) = True Then
                        MyTxtblckImpLabel.Text = XIRes.GetString("XINFO_UC03_106")
                        MyTxtblckImpValue.Text = TreatmentObject(DBitem).TreatmentField
                    ElseIf TreatmentObject(DBitem).DataBaseIndex.StartsWith(XICL05_Enums.ClassicDatabaseType.PDSD3K.ToString) = True Then
                        MyTxtblckImpLabel.Text = XIRes.GetString("XINFO_UC03_106_TRT")
                        MyTxtblckImpValue.Text = TreatmentObject(DBitem).TreatmentField
                    End If

                Case 9 'Treatment Position
                    MyTxtblckImpLabel.Text = XIRes.GetString("XINFO_UC03_107")
                    MyTxtblckImpValue.Text = TreatmentObject(DBitem).TreatmentPosition

                Case 10 'Filter Code
                    MyTxtblckImpLabel.Text = XIRes.GetString("XINFO_UC03_108")
                    MyTxtblckImpValue.Text = TreatmentObject(DBitem).FilterCode.ToString

                Case 11 ' FIlter kV
                    MyTxtblckImpLabel.Text = XIRes.GetString("XINFO_UC03_112")
                    MyTxtblckImpValue.Text = TreatmentObject(DBitem).FilterkV.ToString

                Case 12 'Filter mA
                    MyTxtblckImpLabel.Text = XIRes.GetString("XINFO_UC03_113")
                    MyTxtblckImpValue.Text = TreatmentObject(DBitem).FiltermA.ToString

                Case 13 'Filter HVL
                    MyTxtblckImpLabel.Text = XIRes.GetString("XINFO_UC03_114")
                    MyTxtblckImpValue.Text = TreatmentObject(DBitem).FilterHVL

                Case 14 'Applicator Code
                    MyTxtblckImpLabel.Text = XIRes.GetString("XINFO_UC03_109")
                    MyTxtblckImpValue.Text = TreatmentObject(DBitem).ApplicatorCode

                Case 15 'Applicator Data
                    MyTxtblckImpLabel.Text = XIRes.GetString("XINFO_UC03_115")
                    MyTxtblckImpValue.Text = TreatmentObject(DBitem).ApplicatorData

                Case 16 'Dose Set
                    MyTxtblckImpLabel.Text = XIRes.GetString("XINFO_UC03_116")
                    MyTxtblckImpValue.Text = TreatmentObject(DBitem).DoseSet

                Case 17 'Dose Admin
                    MyTxtblckImpLabel.Text = XIRes.GetString("XINFO_UC03_119")
                    MyTxtblckImpValue.Text = TreatmentObject(DBitem).DoseAdmin

                Case 18 'Time Set
                    MyTxtblckImpLabel.Text = XIRes.GetString("XINFO_UC03_117")
                    MyTxtblckImpValue.Text = TreatmentObject(DBitem).TimeSet

                Case 19 'Time Admin
                    MyTxtblckImpLabel.Text = XIRes.GetString("XINFO_UC03_120")
                    MyTxtblckImpValue.Text = TreatmentObject(DBitem).TimeAdmin

                Case 20 'Operator
                    MyTxtblckImpLabel.Text = XIRes.GetString("XINFO_UC03_118")
                    MyTxtblckImpValue.Text = TreatmentObject(DBitem).TreatmentOperator

                Case 21 'Error Codes
                    MyTxtblckImpLabel.Text = XIRes.GetString("XINFO_UC03_121")
                    MyTxtblckImpValue.Text = TreatmentObject(DBitem).GeneratorError


            End Select

            Select Case I
                Case 1, 2, 3, 4, 5, 6, 7
                    MyStackpanelLeftL.Children.Add(MyTxtblckImpLabel)
                    MyStackpanelLeftR.Children.Add(MyTxtblckImpValue)
                Case 10, 14, 16, 17, 18, 19, 20
                    MyStackpanelMiddleL.Children.Add(MyTxtblckImpLabel)
                    MyStackpanelMiddleR.Children.Add(MyTxtblckImpValue)
                Case 8, 9, 11, 12, 13, 14, 15, 21
                    MyStackpanelRightL.Children.Add(MyTxtblckImpLabel)
                    MyStackpanelRightR.Children.Add(MyTxtblckImpValue)
            End Select

        Next


        Dim BorderAboveL As New Border With {
            .Height = CNVSContentLeft.Height,
            .Width = CNVSContentLeft.Width,
            .Background = Brushes.Transparent,
            .BorderThickness = New Thickness(0, 0.5, 0, 0.5),
            .BorderBrush = Brushes.Black}

        Dim BorderAboveM As New Border With {
            .Height = CNVSContentMiddle.Height,
            .Width = CNVSContentMiddle.Width,
            .Background = Brushes.Transparent,
            .BorderThickness = New Thickness(0, 0.5, 0, 0.5),
            .BorderBrush = Brushes.Black}

        Dim BorderAboveR As New Border With {
            .Height = CNVSContentRight.Height,
            .Width = CNVSContentRight.Width,
            .Background = Brushes.Transparent,
            .BorderThickness = New Thickness(0, 0.5, 0, 0.5),
            .BorderBrush = Brushes.Black}

        BorderAboveL.Child = MyStackpanelLeft
        BorderAboveM.Child = MyStackpanelMiddle
        BorderAboveR.Child = MyStackpanelRight

        CNVSContentLeft.Children.Add(BorderAboveL)
        CNVSContentMiddle.Children.Add(BorderAboveM)
        CNVSContentRight.Children.Add(BorderAboveR)

        Canvas.SetTop(CNVSContentLeft, 0)
        Canvas.SetLeft(CNVSContentLeft, 0)

        Canvas.SetTop(CNVSContentMiddle, 0)
        Canvas.SetLeft(CNVSContentMiddle, PaperMarginWidth * WidthCanvasLeft)

        Canvas.SetTop(CNVSContentRight, 0)
        Canvas.SetLeft(CNVSContentRight, PaperMarginWidth * (WidthCanvasLeft + WidthCanvasMiddle))

        Canvas.SetTop(MyPageCanvas, XICL06_Functions.ConvmmToPixel(70, 96))
        Canvas.SetLeft(MyPageCanvas, 0)

        MyPageCanvas.Children.Add(CNVSContentLeft)
        MyPageCanvas.Children.Add(CNVSContentMiddle)
        MyPageCanvas.Children.Add(CNVSContentRight)

        Return MyPageCanvas

    End Function



End Class
