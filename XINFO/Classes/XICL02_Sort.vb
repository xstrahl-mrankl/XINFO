Imports System.ComponentModel

Public Enum ErrorProperty
    ErrorCode
    ErrorMessage
    ErrorDescription
End Enum

Public Class XICL02_Sort
    Implements IComparer


    Private mSortedBy As ErrorProperty = ErrorProperty.ErrorCode
    Private mSortingDirection As ListSortDirection = ListSortDirection.Ascending

    Public Property SortedBy() As ErrorProperty
        Get
            Return mSortedBy
        End Get
        Set(ByVal value As ErrorProperty)
            mSortedBy = value
        End Set
    End Property

    Public Property SortingDirection() As ListSortDirection
        Get
            Return mSortingDirection
        End Get
        Set(ByVal value As ListSortDirection)
            mSortingDirection = value
        End Set
    End Property

    Public Function Compare(ByVal x As Object, ByVal y As Object) As Integer _
          Implements System.Collections.IComparer.Compare

        Dim itemX As XICL01_ErrorCodeTable = TryCast(x, XICL01_ErrorCodeTable)
        Dim itemY As XICL01_ErrorCodeTable = TryCast(y, XICL01_ErrorCodeTable)

        If itemX IsNot Nothing AndAlso itemY IsNot Nothing Then
            Dim sortMultiplier As Integer = 0
            If mSortingDirection = ListSortDirection.Ascending Then
                sortMultiplier = 1
            Else
                sortMultiplier = -1
            End If
            Select Case SelectedError ' mSortiertNach
                Case ErrorProperty.ErrorCode
                    Return String.Compare(itemX.ErrorCode.ToString, itemY.ErrorCode.ToString, System.StringComparison.Ordinal) * sortMultiplier
                Case ErrorProperty.ErrorDescription
                    Return String.Compare(itemX.ErrorDescription, itemY.ErrorDescription, System.StringComparison.CurrentCultureIgnoreCase) * sortMultiplier
                Case ErrorProperty.ErrorMessage
                    Return String.Compare(itemX.ErrorMessage, itemY.ErrorMessage, System.StringComparison.CurrentCultureIgnoreCase) * sortMultiplier
            End Select
        End If
        Return -1

    End Function

End Class
