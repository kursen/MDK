Imports System.Data.Objects.SqlClient

Namespace MDK_ERP.Areas.Production.Controllers

    <Authorize()> _
    Public Class ProjectController
        Inherits BaseController

        '
        ' GET: /Production/Project

        Function Index() As ActionResult
            Return View()
        End Function

        Function GetList() As JsonResult
            Dim model = (From pl In ctx.ProjectList Select
                         New With {
                             .ID = pl.ID,
                             .NoProject = pl.NoProject,
                             .PackageName = pl.PackageName,
                             .StartDate = pl.StartDate,
                             .EndDate = pl.EndDate,
                             .AgentName = pl.AgentName,
                             .Description = pl.Description}).ToList

            Return Json(New With {.data = model}, JsonRequestBehavior.AllowGet)
        End Function

        Function Save(ByVal model As ProjectList, ByVal StartDate As String, ByVal EndDate As String, Optional ByVal IDRoad As List(Of Long) = Nothing, Optional ByVal RoadName As List(Of String) = Nothing, Optional ByVal Length As List(Of Integer) = Nothing) As JsonResult
            Dim stat As Integer = 0
            'If ModelState.IsValid Then
            Try
                If IsNothing(RoadName) Then
                    ModelState.AddModelError("", "Daftar Ruas Jalan Proyek Wajib Diisi")
                    Exit Try
                End If
                model.StartDate = StrtoDate(StartDate)
                model.EndDate = StrtoDate(StartDate)
                model.Save()
                'Save the details
                For i = 0 To IDRoad.Count - 1
                    Dim detailModel As New ProjectRoadList With {
                        .IDProject = model.ID,
                        .RoadName = RoadName(i),
                        .Length = Length(i)}
                    ctx.ProjectRoadList.AddObject(detailModel)
                Next
                ctx.SaveChanges()
                stat = 1
                ModelState.AddModelError("Success", "Data Berhasil Disimpan")
            Catch ex As Exception
                ModelState.AddModelError("Error ", ex.Message)
            End Try
            'End If
            Dim errorlist = ModelState.ToDictionary(Function(k) k.Key,
                                                      Function(k) k.Value.Errors.Select(Function(e) e.ErrorMessage).ToArray()).
                                                      Where(Function(k) k.Value.Count > 0)
            Return Json(New With {.msg = errorlist, .stat = stat})
        End Function

        Function Delete(ByVal id As Integer) As JsonResult
            Dim stat As Integer = 0
            Dim msg As String = ""
            Dim msgDesc As String = ""
            Try
                Dim dataProject = (From pl In ctx.ProjectList Where pl.ID = id).FirstOrDefault()
                If dataProject IsNot Nothing Then ctx.DeleteObject(dataProject)
                ctx.SaveChanges()
                stat = 1
                msg = "Success Deleted"
            Catch ex As Exception
                msg = "Get Exception"
                msgDesc = ex.InnerException.Message
            End Try
            Return Json(New With {.stat = stat, .msg = msg, .msgDesc = msgDesc}, JsonRequestBehavior.AllowGet)
        End Function

    End Class
End Namespace
