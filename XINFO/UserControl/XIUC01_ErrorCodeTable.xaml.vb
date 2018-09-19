Imports System.Globalization
Imports System.Collections
Imports System.Resources
Imports System.ComponentModel

Public Class XIUC01_ErrorCodeTable

    Public Sub New()

        ' This call is required by the designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.


        Me.CMBBX01_ErrorCodes.DataContext = Me
        Me.TXTBX01.DataContext = Me
        Me.TXTBX02.DataContext = Me

        Me.CMBBX01_ErrorCodes.ItemsSource = AllXstrahlErrors

        Call ReadResources("EN")

    End Sub

    Private Sub SetLanguage(sender As Object, e As RoutedEventArgs)

        Dim MyLanguage As String
        Dim CurrentIndex As Int32 = Me.CMBBX01_ErrorCodes.SelectedIndex

        If sender Is RDBTN01_EN Then
            MyLanguage = "EN"
        ElseIf sender Is RDBTN01_DE Then
            MyLanguage = "DE"
        ElseIf sender Is RDBTN01_DA Then
            MyLanguage = "DA"
        ElseIf sender Is RDBTN01_IT Then
            MyLanguage = "IT"
        ElseIf sender Is RDBTN01_SE Then
            MyLanguage = "SE"
        ElseIf sender Is RDBTN01_ES Then
            MyLanguage = "ES"
        Else
            MyLanguage = "EN"
        End If

        Call ReadResources(MyLanguage)

        If CurrentIndex >= 0 And CurrentIndex < Me.CMBBX01_ErrorCodes.Items.Count Then
            Me.CMBBX01_ErrorCodes.SelectedIndex = CurrentIndex
        End If

    End Sub

    Private Sub ReadResources(MyLanguage As String)

        If Me.CMBBX01_ErrorCodes.Items.Count > 0 Then
            XS100Errors.Clear()
        End If

        System.Threading.Thread.CurrentThread.CurrentCulture = New CultureInfo(MyLanguage)
        System.Threading.Thread.CurrentThread.CurrentUICulture = New CultureInfo(MyLanguage)

        Dim MyResourceSet As ResourceSet = XResErrCodes.GetResourceSet(CultureInfo.CurrentUICulture, True, True)

        For Each ResEntry As DictionaryEntry In MyResourceSet
            If ResEntry.Key.ToString.StartsWith("ErrorCode_") = True AndAlso ResEntry.Value.ToString.StartsWith("-") = False Then
                '                Me.CMBBX01_ErrorCodes.Items.Add(New ComboBoxItem With {.Content = Val(ResEntry.Key.ToString.Substring(10))})
                XS100Errors.Add(New XICL01_ErrorCodeTable With {
                                     .ErrorCode = Format(CInt(Val(ResEntry.Key.ToString.Substring(10))), "00000"),
                                     .ErrorDescription = XResErrCodes.GetString("ErrorDesc_" & Format(CInt(Val(ResEntry.Key.ToString.Substring(10))), "00000")),
                                     .ErrorMessage = ResEntry.Value.ToString})
            End If
        Next

        XS100ErrorsView.CustomSort = mSortierung42
        XS100ErrorsView.Refresh()


    End Sub

End Class
