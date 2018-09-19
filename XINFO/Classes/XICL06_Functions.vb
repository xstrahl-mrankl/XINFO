Public Class XICL06_Functions


    Public Shared Function ConvmmToPixel(Millimeter As Double, dpi As Double) As Double

        '1mm = 96px/25.4
        Return CDbl(Millimeter / 25.4 * dpi)

    End Function

End Class
