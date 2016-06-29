Namespace HRD.Areas.HRD.Controllers
    Public Class OvertimeSummaryController
        Inherits System.Web.Mvc.Controller
        Private _hrdEntities As HrdEntities
        '
        ' GET: /HRD/OvertimeSummary

        Function Index() As ActionResult
            Return View()
        End Function


        <HttpPost()>
        Function GetOvertimeListForEmploye() As ActionResult
            Dim dateStart, dateEnd As Date, EmployeeId As Int32
            Dim dtstart = Request.Form("datestart")
            Dim dtend = Request.Form("dateend")
            Dim sEmployeeId = Request.Form("EmployeeId")

            ModelState.Clear()

            If Date.TryParse(dtstart, dateStart) = False Then
                ModelState.AddModelError("dtStartDate", "Tanggal Mulai Salah")
            End If

            If Date.TryParse(dtend, dateEnd) = False Then
                ModelState.AddModelError("dtEndDate", "Tanggal Akhir Salah")
            End If

            If Integer.TryParse(sEmployeeId, EmployeeId) = False Then
                ModelState.AddModelError("EmployeeId", "Pegawai Tidak terdaftar")
            End If
            Dim minDateValue = New Date(1753, 1, 1, 0, 0, 0)
            If dateStart < minDateValue OrElse dateStart > Date.MaxValue Then
                ModelState.AddModelError("dtStartDate", "Tanggal mulai salah")
            End If

            If dateEnd < minDateValue OrElse dateEnd > Date.MaxValue Then
                ModelState.AddModelError("dtEndDate", "Tanggal akhir salah")
            End If
            If ModelState.Count > 0 Then
                Return Json(New With {.stat = 0, .errors = HaloUIHelpers.Helpers.JsonErrorObject.Convert(ModelState)})
            End If

            Dim model = GetEmployeeOverTimeList(EmployeeId, dateStart, dateEnd)

            Dim rvalue = Newtonsoft.Json.JsonConvert.SerializeObject(model)
            Return Content("{""stat"":1, ""data"":" & rvalue & "}", "text/json")
        End Function
        Private Function GetEmployeeOverTimeList(ByVal EmployeeId As Integer, dtStart As Date, dtEnd As Date) As DataTable
            Dim sql = "Exec HRD.NormalTimesheet @employeeId, @startdate, @enddate;"
            Dim params As New List(Of SqlClient.SqlParameter)
            params.Add(New SqlClient.SqlParameter("@employeeId", EmployeeId))
            params.Add(New SqlClient.SqlParameter("@startdate", dtStart))
            params.Add(New SqlClient.SqlParameter("@enddate", dtEnd))

            Dim rvalue = EFHelper.ExecuteDataTable(_hrdEntities, sql, params.ToArray())


            Return rvalue
        End Function

        Public Sub New()
            _hrdEntities = New HrdEntities
        End Sub
    End Class
End Namespace
