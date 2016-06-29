Namespace Equipment.Areas.Equipment.Controllers
    Public Class EquipmentListController
        Inherits System.Web.Mvc.Controller

        '
        ' GET: /Equipment/EquipmentList
        Dim ctx As New EquipmentEntities
        Function Index() As ActionResult
            Return View()
        End Function

        '
        ' GET: /Equipment/EquipmentList/Details/5
        Function getData() As JsonResult
            Dim model = From m In ctx.MachineEqps
                        Select New With {m.ID, m.Name, m.IDArea, m.Merk, m.Type, m.Cost, m.Remark}
            Return Json(New With {.data = model}, JsonRequestBehavior.AllowGet)
        End Function

        Function getOfficeId() As JsonResult
            Dim ent2 = New ERPBase.MainEntities
            Dim officelist = (From m In ent2.Offices
                             Where m.Parent_ID = 0
                             Select New With {.value = m.Id, .text = m.Name, .key = m.Code}).ToList()
            Return Json(New With {.officelist = officelist})
        End Function
        '
        ' POST: /Equipment/EquipmentList/Create

        <HttpPost()> _
        Function Create(ByVal model As MachineEqp) As JsonResult
            If ModelState.IsValid Then
                Try

                    ctx.MachineEqps.AddObject(model)
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

        <HttpPost()>
        Public Function SavePartial(ByVal pk As Integer, name As String, value As String) As ActionResult
            Dim model = ctx.MachineEqps.Where(Function(m) m.ID = pk).SingleOrDefault()
            If model Is Nothing Then
                ModelState.AddModelError("General", "Data peralatan tidak ditemukan. Muat ulang halaman ini")
                Return Json(New With {.stat = 0, .errors = HaloUIHelpers.Helpers.JsonErrorObject.Convert(ModelState)})
            End If
            Select Case name
                Case "IDArea"
                    Dim d As Integer
                    Integer.TryParse(value, d)
                    model.GetType.GetProperty(name).SetValue(model, d, Nothing)
                Case "Cost"
                    Dim d As Decimal
                    Decimal.TryParse(value, d)
                    model.GetType.GetProperty(name).SetValue(model, d, Nothing)
                Case Else
                    model.GetType.GetProperty(name).SetValue(model, value, Nothing)
            End Select
            ctx.SaveChanges()
            Return Json(New With {.stat = 1})
        End Function

        Function Detail(ByVal id As Integer) As ActionResult
            Dim model = ctx.MachineEqps.Where(Function(m) m.ID = id).FirstOrDefault()
            Dim ent2 = New ERPBase.MainEntities
            ViewData("area") = (From m In ent2.Offices
                             Where m.Id = model.IDArea
                             Select m.Name).FirstOrDefault
            Return View(model)
        End Function
        '
        ' POST: /Equipment/EquipmentList/Delete/5

        Function Delete(ByVal id As Integer) As JsonResult
            Dim stat As Integer = 0
            Dim msg As String = ""
            Dim msgDesc As String = ""
            Try
                Dim data = (From pl In ctx.MachineEqps Where pl.ID = id).FirstOrDefault()
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