Imports System.Text.RegularExpressions

Public Class XICL04_ReadOnly
    'ALL DIRECTORIES MUST END WITH A BACKSLASH!!!
    'Otherwise they are not recognized as directory when using DirectoryInfo method.
    '-------------------------------------------------------------------------------


    Public Shared ReadOnly Property XInfoDirMain As String
        Get
            Return "C:\XInfo_Data"
        End Get
    End Property

    Public Shared ReadOnly Property XInfoDirSettings As String
        Get
            Return "C:\XInfo_Data\Settings\"
        End Get
    End Property

    Public Shared ReadOnly Property XInfoDirTemp As String
        Get
            Return "C:\XInfo_Data\Temp\"
        End Get
    End Property

    Public Shared ReadOnly Property XInfoDirExportCSV As String
        Get
            Return "C:\XInfo_Data\Export_CSV\"
        End Get
    End Property
    Public Shared ReadOnly Property XInfoDirExportXML As String
        Get
            Return "C:\XInfo_Data\Export_XML\"
        End Get
    End Property

    Public Shared ReadOnly Property XInfoDirExportXPS As String
        Get
            Return "C:\XInfo_Data\Export_XPS\"
        End Get
    End Property

    Public Shared ReadOnly Property XInfoDirClassicDB As String
        Get
            Return "C:\XInfo_Data\ClassicDB\"
        End Get
    End Property

#Region "Read Only values for printing"

    Public Shared ReadOnly Property PSLetterWidth As Single
        Get
            Return 8.5
        End Get
    End Property

    Public Shared ReadOnly Property PSLetterHeight As Single
        Get
            Return 11
        End Get
    End Property

    Public Shared ReadOnly Property PSA4Width As Single
        Get
            Return 8.27
        End Get
    End Property

    Public Shared ReadOnly Property PSA4Height As Single
        Get
            Return 11.96
        End Get
    End Property

    Public Shared ReadOnly Property MarginLeftMax As Integer
        Get
            Return 30
        End Get
    End Property
    Public Shared ReadOnly Property MarginTopMax As Integer
        Get
            Return 20
        End Get
    End Property

    Public Shared ReadOnly Property MarginRightMax As Integer
        Get
            Return 20
        End Get
    End Property

    Public Shared ReadOnly Property MarginBottomMax As Integer
        Get
            Return 20
        End Get
    End Property

#End Region

End Class
