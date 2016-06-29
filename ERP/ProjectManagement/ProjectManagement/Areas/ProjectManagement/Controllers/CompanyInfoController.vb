Imports System.Web.Helpers

Namespace ProjectManagement.Areas.ProjectManagement.Controllers
    <Authorize()> _
    Public Class CompanyInfoController
        Inherits System.Web.Mvc.Controller
        Dim prmEntities As ProjectManagement_ERPEntities
        '
        ' GET: /ProjectManagement/CompanyInfo

        Function Index() As ActionResult
            Return View()
        End Function

        Function getData(Optional ByVal name As String = "") As JsonResult
            Dim model = (From m In prmEntities.CompanyInfoes
                         Where m.Name.ToLower().Contains(name)
                         Select New With {m.ID, _
                                        m.Name, _
                                        m.Alias, _
                                        m.City}).ToList()
            Return Json(New With {.data = model}, JsonRequestBehavior.AllowGet)
        End Function

        Function Details(Optional ByVal id As Integer = 0) As ActionResult
            Dim model As New CompanyInfo
            If id <> 0 Then
                model = (From m In prmEntities.CompanyInfoes Where m.ID = id Select m).FirstOrDefault()
            Else
                Return View("~/Views/Error/NotFound.vbhtml")
            End If
            Return View(model)
        End Function

        Function Create(Optional ByVal id As Integer = 0) As ActionResult
            Dim model As New CompanyInfo
            If id <> 0 Then
                model = (From m In prmEntities.CompanyInfoes Where m.ID = id Select m).FirstOrDefault()
            End If
            Return View(model)
        End Function

        <HttpPost()> _
        Function Save(ByVal model As CompanyInfo) As ActionResult

            If ModelState.IsValid Then
                Try
                    prmEntities.CompanyInfoes.AddObject(model)
                    If model.ID <> 0 Then
                        prmEntities.ObjectStateManager.ChangeObjectState(model, EntityState.Modified)
                    End If
                    prmEntities.SaveChanges()
                    Return Json(New With {.stat = 1})
                Catch ex As Exception
                    ModelState.AddModelError("General", ex.Message)
                End Try
            End If

            'if we got this far so return back the view
            Return Json(New With {.stat = 0, .errors = HaloUIHelpers.Helpers.JsonErrorObject.Convert(ModelState)})
        End Function

        Function Delete(ByVal id As Integer) As JsonResult
            Dim stat As Integer = 0
            Dim msg As String = ""
            Dim msgDesc As String = ""
            Try
                Dim data = (From pl In prmEntities.CompanyInfoes Where pl.ID = id).FirstOrDefault()
                If data IsNot Nothing Then prmEntities.CompanyInfoes.DeleteObject(data)
                prmEntities.SaveChanges()
                stat = 1
                msg = "Success Deleted"
            Catch ex As Exception
                msg = "Got Exception"
                msgDesc = ex.InnerException.Message
            End Try
            Return Json(New With {.stat = stat, .msg = msg, .msgDesc = msgDesc}, JsonRequestBehavior.AllowGet)
        End Function

        Public Sub New()
            prmEntities = New ProjectManagement_ERPEntities
        End Sub
    End Class
End Namespace