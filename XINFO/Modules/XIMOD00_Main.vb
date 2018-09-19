Imports System.Collections.ObjectModel

Imports System.Resources

Module XIMOD00_Main

    Public NumberOfActiveMenu As Integer
    Public NameOfActiveChildUC As String = ""

    Public XResErrCodes As New ResourceManager("XINFO.Resources_ErrCodes", GetType(XIMOD00_Main).Assembly)

    Public XS100Errors As New ObservableCollection(Of XICL01_ErrorCodeTable)
    Public XS100ErrorsView As ListCollectionView = CType(CollectionViewSource.GetDefaultView(XS100Errors), ListCollectionView)
    Public mSortierung42 As New XICL02_Sort

    Public Property AllXstrahlErrors As ObservableCollection(Of XICL01_ErrorCodeTable)
        Get
            Return XS100Errors
        End Get
        Set(ByVal value As ObservableCollection(Of XICL01_ErrorCodeTable))
            XS100Errors = value
        End Set
    End Property


    Public ReadOnly Property Sortierung42 As XICL02_Sort
        Get
            Return mSortierung42
        End Get
    End Property

    Public SelectedError As New ErrorProperty
    Public XIRes As New ResourceManager("XInfo.Resources", GetType(XIWW001_MainWindow).Assembly)

    Public XInfoGeneral As New XICL08_GeneralProperties

End Module
