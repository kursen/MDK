Imports Microsoft.Reporting.WebForms

Namespace MDK_ERP.Areas.Production.Controllers

    <Authorize()> _
    Public Class ReportsController
        Inherits BaseController

        '
        ' GET: /Production/Report

        Function Index() As ActionResult
            Return RedirectToAction("weightScales", "Report")
        End Function

#Region "WeightScales"

        Function WeightScales() As ActionResult
            Return View()
        End Function

        Function GetListWeightScales(ByVal startDate As String, ByVal endDate As String) As JsonResult
            Dim date1 = StrtoDate(startDate)
            Dim date2 = StrtoDate(endDate).AddDays(1)
            Dim model = ctx.LaporanPenimbanganMaterial(date1, date2).ToList()

            Return Json(New With {.data = model}, "application/json", JsonRequestBehavior.AllowGet)
        End Function

        Function PrintWeightScales(ByVal startDate As String, ByVal endDate As String) As ActionResult
            Dim date1 = StrtoDate(startDate)
            Dim date2 = StrtoDate(endDate).AddDays(1)
            Dim model = ctx.LaporanPenimbanganMaterial(date1, date2).ToList()

            Try
                Dim ReportPath As String = Server.MapPath("~/Reports/PenimbanganMaterial.rdlc") 'set path of rdlc
                Dim DataSource = "DsPenimbanganMaterial" 'set datasource name
                Dim params As New Microsoft.Reporting.WebForms.LocalReport ' declare parameters

                'set parameters
                params.SetParameters(New ReportParameter("PeriodeAwal", date1))
                params.SetParameters(New ReportParameter("PeriodeAkhir", date2))

                Return ThrowPDFReport(model, ReportPath, DataSource, params)
            Catch ex As Exception
                Return RedirectToAction("WeightScales", "Reports")
            End Try
        End Function

#End Region

#Region "MaterialDelivery"

        Function MaterialDelivery() As ActionResult
            Return View()
        End Function

#End Region

#Region "MaterialReceived"

        Function MaterialReceived() As ActionResult
            Return View()
        End Function

#End Region





    End Class
End Namespace