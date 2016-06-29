Namespace MDK_ERP.Areas.ProjectManagement.Controllers

    <Authorize()> _
    Public Class CompanyInfoController
        Inherits BaseController

        '
        ' GET: /ProjectManagement/CompanyInfo

        Function Index() As ActionResult
            Return View()
        End Function

        Function getData() As JsonResult
            Dim model = (From m In ctx.CompanyInfo Select m.ID, _
                                                         m.Name, _
                                                         m.Alias, _
                                                         m.City).ToList()
            Return Json(New With {.data = model}, JsonRequestBehavior.AllowGet)
        End Function

        Function Create(Optional ByVal id As Integer = 0) As ActionResult
            Dim model As New CompanyInfo
            If id <> 0 Then
                model = (From m In ctx.CompanyInfo Where m.ID = id Select m).FirstOrDefault()
            End If
            Return View(model)
        End Function

        <HttpPost()> _
        Function Create(ByVal model As CompanyInfo) As ActionResult
            If ModelState.IsValid Then
                Try
                    model.Save()
                    ctx.SaveChanges()
                    Return RedirectToAction("Index", "CompanyInfo")
                Catch ex As Exception
                    ModelState.AddModelError("", ex.Message)
                End Try
            End If

            'if we got this far so return back the view
            Return View(model)
        End Function

        Function Delete(ByVal id As Integer) As JsonResult
            Dim stat As Integer = 0
            Dim msg As String = ""
            Dim msgDesc As String = ""
            Try
                Dim data = (From pl In ctx.CompanyInfo Where pl.ID = id).FirstOrDefault()
                If data IsNot Nothing Then ctx.DeleteObject(data)
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