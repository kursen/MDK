Namespace ProjectManagement.Areas.ProjectManagement.Controllers
    Public Class MaterialController
        Inherits System.Web.Mvc.Controller
        Dim prmEntities As New ProjectManagement_ERPEntities
        '
        ' GET: /ProjectManagement/Material
     
        Function Index(ByVal id As Integer) As ActionResult
            Dim model = prmEntities.ProjectInfoes.Where(Function(m) m.Id = id).SingleOrDefault()
            If model Is Nothing Then
                Return View("~/Views/Error/NotFound.vbhtml")
            End If
            ViewData("ProjectInfo") = model
            Return View()
        End Function

            '
            ' GET: /ProjectManagement/Material/Details/5

        Function getData(ByVal id As Integer, Optional ByVal MaterialName As String = "") As JsonResult
            Dim model = (From m In prmEntities.Materials Where m.ProjectInfoID = id _
                         AndAlso m.Materials.ToLower().Contains(MaterialName) _ 
                         Select New With {m.ID,
                                                         m.Materials, _
                                                         m.Unit, _
                                                         m.Volume, _
                                                         m.V_Lap, _
                                                         m.BeginDate, _
                                                         m.ProjectInfo.ProjectTitle, _
                                                         m.EndDate}).ToList()
            Return Json(New With {.data = model}, JsonRequestBehavior.AllowGet)
        End Function

        '
        ' POST: /ProjectManagement/Material/Create

        <HttpPost()> _
        Function Create(ByVal model As Material) As JsonResult
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
        ' POST: /ProjectManagement/Material/Edit/5

       Function Delete(ByVal id As Integer) As JsonResult
            Dim stat As Integer = 0
            Dim msg As String = ""
            Dim msgDesc As String = ""
            Try
                Dim data = (From pl In prmEntities.Materials Where pl.ID = id).FirstOrDefault()
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