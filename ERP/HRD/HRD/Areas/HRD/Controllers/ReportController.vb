Imports Microsoft.Reporting.WebForms
Namespace HRD.Areas.HRD.Controllers
    Public Class ReportController
        Inherits System.Web.Mvc.Controller

        '
        ' GET: /HRD/Report
        Dim ctx As New HrdEntities
        Function Index() As ActionResult
            Return View()
        End Function

        Function Employee(ByVal Id As Integer) As ActionResult
            Dim detailEmployee = ctx.ReportEmployee(Id)
          
            Dim mimeType As String = "application/pdf"
            Dim Encoding As String = Nothing
            Dim fileNameExtension As String = Nothing
            Dim streams As String() = Nothing
            Dim warnings() As Microsoft.Reporting.WebForms.Warning = Nothing

            Dim r As New Microsoft.Reporting.WebForms.LocalReport
            r.EnableExternalImages = True
            r.ReportPath = Server.MapPath("~/Areas/HRD/Report/RptEmployee.rdlc")
          
            
            'r.ReportPath = System.AppDomain.CurrentDomain.BaseDirectory() & "Reports\ReportBeritaAcara.rdlc"

            If Not IsNothing(detailEmployee) Then
                r.DataSources.Add(New ReportDataSource("DataSetEmployee", detailEmployee))
                'If IsNothing(detailEmployee.Photo) Then
                'r.EnableExternalImages = True
                'Dim imagePath As String = New Uri(Server.MapPath("~/img/_default_male.png")).AbsoluteUri
                'Dim parameter As New ReportParameter("ReportParameter1", imagePath)
                'r.SetParameters(parameter)
                'End If
            End If

            r.Refresh()
            Dim reportType As String = "PDF"

            Dim devinfo As String = "<DeviceInfo>" +
            "<ColorDepth>32</ColorDepth><DpiX>350</DpiX><DpiY>350</DpiY><OutputFormat>PDF</OutputFormat>" +
            "  <PageWidth>8.5in</PageWidth>" +
            "  <PageHeight>11in</PageHeight>" +
            "  <MarginTop>0.5in</MarginTop>" +
            "  <MarginLeft>0.5in</MarginLeft>" +
            "  <MarginRight>0in</MarginRight>" +
            "  <MarginBottom>0in</MarginBottom>" +
            "</DeviceInfo>"

            Dim output() As Byte = r.Render(reportType, devinfo, mimeType, Encoding, fileNameExtension, streams, warnings)
            Return File(output, mimeType)

        End Function

    End Class
End Namespace