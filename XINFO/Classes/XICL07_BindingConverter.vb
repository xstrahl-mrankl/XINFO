Imports System.Globalization

Namespace XInfoClasses

    Public Class XICL07_BindingConverter


    End Class



    Public Class DateConverter01
        Implements IValueConverter

        Public Function Convert(value As Object, targetType As Type, parameter As Object, culture As CultureInfo) As Object Implements IValueConverter.Convert

            Dim TempDate As Date = CType(value, Date)
            Return XstrahlCrypto.XstrahlFunctions.GetTimeAsHHMMSSwithColon(TempDate) & " - " & XstrahlCrypto.XstrahlFunctions.GetDateAsDDMMYYYYwithDot(TempDate)

        End Function

        Public Function ConvertBack(value As Object, targetType As Type, parameter As Object, culture As CultureInfo) As Object Implements IValueConverter.ConvertBack
            'Throw New NotImplementedException()
            Return Binding.DoNothing

        End Function
    End Class

    Public Class DateConverter02
        Implements IValueConverter

        Public Function Convert(value As Object, targetType As Type, parameter As Object, culture As CultureInfo) As Object Implements IValueConverter.Convert

            'Dim TempDate As Date = CType(value, Date)

            Dim TempDate As DateTime = DateTime.ParseExact(CType(value, String), "yyyyMMdd", CultureInfo.InvariantCulture)

            Return XstrahlCrypto.XstrahlFunctions.GetDateAsDDMMYYYYwithDot(TempDate)

        End Function

        Public Function ConvertBack(value As Object, targetType As Type, parameter As Object, culture As CultureInfo) As Object Implements IValueConverter.ConvertBack
            'Throw New NotImplementedException()
            Return Binding.DoNothing

        End Function
    End Class


End Namespace