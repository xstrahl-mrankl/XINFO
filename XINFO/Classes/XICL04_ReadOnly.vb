Imports System.IO
Imports System.Text.RegularExpressions

Public Class XICL04_ReadOnly
    'ALL DIRECTORIES MUST END WITH A BACKSLASH!!!
    'Otherwise they are not recognized as directory when using DirectoryInfo method.
    '-------------------------------------------------------------------------------

    ''' <summary>
    ''' Environment.SpecialFolder.CommonDocuments
    ''' </summary>
    Public Shared ReadOnly CUsersPublicPublicDocuments As DirectoryInfo = New DirectoryInfo(Environment.GetFolderPath(Environment.SpecialFolder.CommonDocuments))

    ''' <summary>
    ''' CUsersPublicPublicDocuments.Parent.FullName
    ''' </summary>
    Public Shared ReadOnly CUsersPublic As String = CUsersPublicPublicDocuments.Parent.FullName

    ''' <summary>
    ''' Xstrahl\XPhys
    ''' </summary>
    Public Shared ReadOnly Folder_CUPXstrahlXInfo As String = Path.Combine(CUsersPublic, "Xstrahl\XInfo_Data\")

    Public Shared ReadOnly XInfoDirSettings As String = Path.Combine(CUsersPublic, "Xstrahl\XInfo_Data\Settings\")

    Public Shared ReadOnly XInfoDirTemp As String = Path.Combine(CUsersPublic, "Xstrahl\XInfo_Data\Temp\")

    Public Shared ReadOnly XInfoDirLicence As String = Path.Combine(CUsersPublic, "Xstrahl\XInfo_Data\Licence\")

    Public Shared ReadOnly XInfoDirExportCSV As String = Path.Combine(CUsersPublic, "Xstrahl\XInfo_Data\Export_CSV\")

    Public Shared ReadOnly XInfoDirExportXML As String = Path.Combine(CUsersPublic, "Xstrahl\XInfo_Data\Export_XML\")

    Public Shared ReadOnly XInfoDirExportXPS As String = Path.Combine(CUsersPublic, "Xstrahl\XInfo_Data\Export_XPS\")

    Public Shared ReadOnly XInfoDirClassicDB As String = Path.Combine(CUsersPublic, "Xstrahl\XInfo_Data\ClassicDB\")


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
