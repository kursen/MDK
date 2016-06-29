Namespace Equipment.Areas.Equipment.Controllers
    Public Class WorkEquipmentController
        Inherits System.Web.Mvc.Controller
        Dim ctx As New EquipmentEntities
        Function Index() As ActionResult
            Return View()
        End Function

        '
        ' GET: /Equipment/EquipmentList/Details/5
        Function getData() As JsonResult
            Dim model = ctx.OutFitEqps.ToList()
            Return Json(New With {.data = model}, JsonRequestBehavior.AllowGet)
        End Function


        '
        ' POST: /Equipment/EquipmentList/Create

        <HttpPost()> _
        Function Create(ByVal model As OutFitEqp) As JsonResult
            If ModelState.IsValid Then
                Try
                    ctx.OutFitEqps.AddObject(model)
                    If model.ID > 0 Then
                        ctx.ObjectStateManager.ChangeObjectState(Me, EntityState.Modified)
                    End If
                    ctx.SaveChanges()
                    Return Json(New With {.stat = 1})
                Catch ex As Exception
                    ModelState.AddModelError("General", ex.Message)
                End Try
            End If

            'if we got this far so return back the view
            Return Json(New With {.stat = 0, .errors = HaloUIHelpers.Helpers.JsonErrorObject.Convert(ModelState)})
        End Function


        '
        ' POST: /Equipment/EquipmentList/Delete/5

        Function Delete(ByVal id As Integer) As JsonResult
            Dim stat As Integer = 0
            Dim msg As String = ""
            Dim msgDesc As String = ""
            Try
                Dim data = (From pl In ctx.OutFitEqps Where pl.ID = id).FirstOrDefault()
                If data IsNot Nothing Then ctx.DeleteObject(data)
                ctx.SaveChanges()
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