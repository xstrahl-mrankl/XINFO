Public Class XIUC02_Manuals

    Public Sub New()

        ' This call is required by the designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.

    End Sub

    Private Sub ShowTheManual(sender As Object, e As RoutedEventArgs)

        Dim XInfoManualFilename As String = ""

        If sender Is BTN02_FisicaX100EN Then
            XInfoManualFilename = ".\Resources\Manuals_Fisica\" & "FMTP2TECHMAN100F_100TechManual"
        ElseIf sender Is BTN02_FisicaX150EN Then
            XInfoManualFilename = ".\Resources\Manuals_Fisica\" & "FMTP2TECHMAN150C_150TechManual"
        ElseIf sender Is BTN02_FisicaX200EN Then
            XInfoManualFilename = ".\Resources\Manuals_Fisica\" & "FMTP2TECHMAN200E (200 Technical Manual)"
        ElseIf sender Is BTN02_FisicaX300EN Then
            XInfoManualFilename = ".\Resources\Manuals_Fisica\" & "FMTP2TECHMAN300D (300 Tech Manual)"

        ElseIf sender Is BTN02_ConcertoX100EN Then
            XInfoManualFilename = ".\Resources\Manuals_Concerto\X100\" & "100_OpMan_GB_G"
        ElseIf sender Is BTN02_ConcertoX150EN Then
            XInfoManualFilename = ".\Resources\Manuals_Concerto\X150\" & "150_OpMan_GB_G"
        ElseIf sender Is BTN02_ConcertoX200EN Then
            XInfoManualFilename = ".\Resources\Manuals_Concerto\X200\" & "200_OpMan_GB_G"
        ElseIf sender Is BTN02_ConcertoX300EN Then
            XInfoManualFilename = ".\Resources\Manuals_Concerto\X300\" & "300_OpMan_GB_G"

        ElseIf sender Is BTN02_ConcertoX100DE Then
            XInfoManualFilename = ".\Resources\Manuals_Concerto\X100\" & "100_OpMan_DE_G"
        ElseIf sender Is BTN02_ConcertoX150DE Then
            XInfoManualFilename = ".\Resources\Manuals_Concerto\X150\" & "150_OpMan_DE_G"
        ElseIf sender Is BTN02_ConcertoX200DE Then
            XInfoManualFilename = ".\Resources\Manuals_Concerto\X200\" & "200_OpMan_DE_G"

        ElseIf sender Is BTN02_ConcertoX100IT Then
            XInfoManualFilename = ".\Resources\Manuals_Concerto\X100\" & "100_OpMan_IT_G"
        ElseIf sender Is BTN02_ConcertoX150IT Then
            XInfoManualFilename = ".\Resources\Manuals_Concerto\X150\" & "150_OpMan_IT_G"
        ElseIf sender Is BTN02_ConcertoX200IT Then
            XInfoManualFilename = ".\Resources\Manuals_Concerto\X200\" & "200_OpMan_IT_F"

        ElseIf sender Is BTN02_ConcertoX150SE Then
            XInfoManualFilename = ".\Resources\Manuals_Concerto\X150\" & "150_OpMan_SE_F"
        ElseIf sender Is BTN02_ConcertoX200SE Then
            XInfoManualFilename = ".\Resources\Manuals_Concerto\X200\" & "200_OpMan_SE_F"

        ElseIf sender Is BTN02_ConcertoX100DA Then
            XInfoManualFilename = ".\Resources\Manuals_Concerto\X100\" & "100_OpMan_DA_G"

        ElseIf sender Is BTN02_ConcertoX200ES Then
            XInfoManualFilename = ".\Resources\Manuals_Concerto\X200\" & "200_OpMan_ES_G"

        ElseIf sender Is BTN02_ExtraDE Then
            XInfoManualFilename = ".\Resources\Manuals_Fisica\" & "TP2FISICA_DEA"


        End If

        XInfoManualFilename &= ".pdf"

        Dim FileInfoManual As New System.IO.FileInfo(XInfoManualFilename)
        If FileInfoManual.Exists = False Then
            'ShowMessage
        End If

        Try
            System.Diagnostics.Process.Start(XInfoManualFilename)
        Catch ex As Exception
        End Try

    End Sub


End Class
