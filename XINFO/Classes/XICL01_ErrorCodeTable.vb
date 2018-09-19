Public Class XICL01_ErrorCodeTable

    Private _ErrorCode As String
    Private _ErrorMessage As String
    Private _ErrorDescription As String

    Public Property ErrorCode As String
        Get
            Return _ErrorCode
        End Get
        Set(value As String)
            _ErrorCode = value
        End Set
    End Property

    Public Property ErrorMessage As String
        Get
            Return _ErrorMessage
        End Get
        Set(value As String)
            _ErrorMessage = value
        End Set
    End Property

    Public Property ErrorDescription As String
        Get
            Return _ErrorDescription
        End Get
        Set(value As String)
            _ErrorDescription = value
        End Set
    End Property

    Public Sub New()

    End Sub

End Class
