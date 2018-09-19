Imports System.ComponentModel

Public Class XIUC99_Progress
    Implements INotifyPropertyChanged

    Public Event PropertyChanged As PropertyChangedEventHandler Implements INotifyPropertyChanged.PropertyChanged

    Private _ProgressStart As Double

    Public Property ProgressStart As Double
        Get
            Return _ProgressStart
        End Get
        Set(value As Double)
            _ProgressStart = value
        End Set
    End Property


    Public Function GetDoneRate(NrItemsDone As Int32, ProgressMoment As Double) As Double

        Return NrItemsDone / (ProgressMoment - ProgressStart)

    End Function


End Class
