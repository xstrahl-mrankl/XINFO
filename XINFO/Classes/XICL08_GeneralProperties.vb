Imports System.ComponentModel

Public Class XICL08_GeneralProperties
    Implements INotifyPropertyChanged
    Public Event PropertyChanged As PropertyChangedEventHandler Implements INotifyPropertyChanged.PropertyChanged

    Private _XINFOmenu As Boolean

    Public Property XINFOmenu As Boolean
        Get
            Return _XINFOmenu
        End Get
        Set(ByVal value As Boolean)
            _XINFOmenu = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("XINFOmenu"))
        End Set
    End Property


End Class
