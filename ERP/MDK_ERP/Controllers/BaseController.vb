Imports Microsoft.Reporting.WebForms

Namespace MDK_ERP
    Public Class BaseController
        Inherits System.Web.Mvc.Controller
        Public ctx As New ERPEntities


        Public Function StrtoDate(ByVal strDate As String) As DateTime
            Dim arr = strDate.Split("-")
            Return New DateTime(arr(2), arr(1), arr(0), 0, 0, 0)
        End Function

        Public Function StrtoDatetime(ByVal strDate As String) As DateTime
            Dim arr = strDate.Split(" ")
            Dim arrDt = arr(0).Split("-")
            Dim arrTm = arr(1).Split(":")
            Return New DateTime(arrDt(2), arrDt(1), arrDt(0), arrTm(0), arrTm(1), 0)
        End Function

        Public Function ThrowPDFReport(ByVal model As Object, _
                                 ByVal ReportPath As String, _
                                 ByVal DataSourceName As String, _
                                 ByVal parameters As Microsoft.Reporting.WebForms.LocalReport) As FileContentResult

            Dim mimeType As String = "application/pdf"
            Dim Encoding As String = Nothing
            Dim fileNameExtension As String = Nothing
            Dim streams As String() = Nothing
            Dim warnings() As Microsoft.Reporting.WebForms.Warning = Nothing

            parameters.EnableExternalImages = True
            parameters.ReportPath = Server.MapPath(ReportPath)

            If Not IsNothing(model) Then


                parameters.DataSources.Add(New ReportDataSource(DataSourceName, model))
            End If

            parameters.Refresh()
            Dim reportType As String = "PDF"
            Dim deviceInfo As String = "<DeviceInfo>" &
            "  <OutputFormat>PDF</OutputFormat>" &
            "  <PageWidth>13in</PageWidth>" &
            "  <PageHeight>8.5in</PageHeight>" &
            "  <MarginTop>0.25in</MarginTop>" &
            "  <MarginLeft>0.25in</MarginLeft>" &
            "  <MarginRight>0.25in</MarginRight>" &
            "  <MarginBottom>0.25in</MarginBottom>" &
            "</DeviceInfo>"

            Dim output() As Byte = parameters.Render(reportType, deviceInfo, mimeType, Encoding, fileNameExtension, streams, warnings)
            Return File(output, mimeType)
        End Function
    End Class

    Enum InventoryStatus
        StokAwal = 1
        Pembelian = 2
        Penjualan = 3
        Pemakaian = 4
        HasilProduksi = 5
    End Enum

    Enum MaterialType
        Raw = 1
        inProcess = 2
        Product = 3
        Subsidiary = 4
    End Enum
End Namespace