Namespace ProjectManagement.Areas.ProjectManagement.Controllers
    Public Class EquipmentScheduleController
        Inherits System.Web.Mvc.Controller
        Dim prmEntities As New ProjectManagement_ERPEntities
        '
        ' GET: /ProjectManagement/EquipmentSchedule

        Function Index(ByVal id As Integer) As ActionResult
            Dim model = prmEntities.ProjectInfoes.Where(Function(m) m.Id = id).SingleOrDefault()
            If model Is Nothing Then
                Return View("~/Views/Error/NotFound.vbhtml")
            End If
            ViewData("ProjectInfo") = model
            Return View()
        End Function

        '
        ' GET: /ProjectManagement/EquipmentSchedule/Details/5

        Function getData(ByVal id As Integer, Optional ByVal EqName As String = "") As JsonResult
            Dim model = (From m In prmEntities.Equipments
                         Where m.ProjectInfoID = id _
                            AndAlso m.Equipments.ToLower().Contains(EqName) _
                         Select New With { _
                             m.ID,
                             m.Equipments, _
                             m.Unit, _
                             m.Volume, _
                             m.BeginDate, _
                             m.ProjectInfo.ProjectTitle, _
                             m.EndDate}).ToList()
            Return Json(New With {.data = model}, JsonRequestBehavior.AllowGet)
        End Function
       

        <HttpPost()> _
        Function Create(ByVal model As Equipment) As JsonResult
          If ModelState.IsValid Then
                Try
                    model.Save()
                    Return Json(New With {.stat = 1})
                Catch ex As Exception
                    ModelState.AddModelError("General", ex.Message)
                End Try
            End If

            'if we got this far so return back the view
            Return Json(New With {.stat = 0, .errors = HaloUIHelpers.Helpers.JsonErrorObject.Convert(ModelState)})
        End Function
        
        '
        ' GET: /ProjectManagement/EquipmentSchedule/Edit/5

        Function Delete(ByVal id As Integer) As JsonResult
            Dim stat As Integer = 0
            Dim msg As String = ""
            Dim msgDesc As String = ""
            Try
                Dim data = (From pl In prmEntities.Equipments Where pl.ID = id).FirstOrDefault()
                If data IsNot Nothing Then prmEntities.DeleteObject(data)
                prmEntities.SaveChanges()
                stat = 1
                msg = "Success Deleted"
            Catch ex As Exception
                msg = "Got Exception"
                msgDesc = ex.InnerException.Message
            End Try
            Return Json(New With {.stat = stat, .msg = msg, .msgDesc = msgDesc}, JsonRequestBehavior.AllowGet)
        End Function

    End Class
End Namespace