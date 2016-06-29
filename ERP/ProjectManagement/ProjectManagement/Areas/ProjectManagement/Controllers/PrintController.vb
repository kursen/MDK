Imports Microsoft.Reporting.WebForms
Namespace ProjectManagement.Areas.ProjectManagement.Controllers
    Public Class PrintController
        Inherits System.Web.Mvc.Controller

        '
        ' GET: /ProjectManagement/Print

        Function Index(ByVal id As Integer) As ActionResult
            Dim prmEntities As New ProjectManagement_ERPEntities
            Dim modeltitle = prmEntities.getTitle(id)
            Dim modelEquip = prmEntities.ScheduleEquipment(id)
            Dim modelMaterial = prmEntities.ScheduleMaterial(id)

            Try

                Dim mimeType As String = "application/pdf"
                Dim Encoding As String = Nothing
                Dim fileNameExtension As String = Nothing
                Dim streams As String() = Nothing
                Dim warnings() As Microsoft.Reporting.WebForms.Warning = Nothing

                Dim r As New Microsoft.Reporting.WebForms.LocalReport
                r.EnableExternalImages = True
                r.ReportPath = Server.MapPath("~/Areas/ProjectManagement/Reports/Schedule.rdlc")
                'r.ReportPath = System.AppDomain.CurrentDomain.BaseDirectory() & "Reports\ReportBeritaAcara.rdlc"

                If Not IsNothing(modeltitle) Then
                    'r.SetParameters(New ReportParameter("projectInfoId", id))
                    r.DataSources.Add(New ReportDataSource("TitleDataSet", modeltitle))
                    r.DataSources.Add(New ReportDataSource("EqDataSet", modelEquip))
                    r.DataSources.Add(New ReportDataSource("MaterialDataSet", modelMaterial))
                End If

                r.Refresh()
                Dim reportType As String = "PDF"

                Dim devinfo As String = "<DeviceInfo>" +
                "<ColorDepth>32</ColorDepth><DpiX>350</DpiX><DpiY>350</DpiY><OutputFormat>PDF</OutputFormat>" +
                "  <PageWidth>8.4in</PageWidth>" +
                "  <PageHeight>11in</PageHeight>" +
                "  <MarginTop>0.5in</MarginTop>" +
                "  <MarginLeft>0.5in</MarginLeft>" +
                "  <MarginRight>0in</MarginRight>" +
                "  <MarginBottom>0in</MarginBottom>" +
                "</DeviceInfo>"

                Dim output() As Byte = r.Render(reportType, devinfo, mimeType, Encoding, fileNameExtension, streams, warnings)
                Return File(output, mimeType)

            Catch ex As Exception
                ModelState.AddModelError("General", ex.Message)
            End Try
            Return View()
        End Function

    End Class
End Namespace