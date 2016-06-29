Namespace Equipment.Areas.Equipment.Controllers
    Public Class MaintenanceTemplateController
        Inherits System.Web.Mvc.Controller

        Dim ctx As New EquipmentEntities

        '
        ' GET: /Equipment/MaintenanceTemplate

        Function Index() As ActionResult
            ListItem()
            Return View()
        End Function

        <HttpPost()>
        Function getListMaintenance() As JsonResult
            'Dim p = ERPBase.ErpUserProfile.GetUserProfile()

            Dim rModel = From m In ctx.MaintenanceTemplateItems
                         Select New With {m.ID, m.Item, m.Value, m.Unit, m.MachineType.MachineTypeName, m.MachineTypeID}

            Dim lModel = From m In rModel
                         Group m By m.MachineTypeID, m.MachineTypeName Into Group

            Return Json(New With {.data = lModel})
        End Function

        Sub ListItem(Optional ByVal model As MaintenanceTemplateItem = Nothing)
            If model Is Nothing Then model = New MaintenanceTemplateItem

            'list of unit
            Dim objUnit As New Dictionary(Of String, String)
            objUnit.Add(MaintenanceTemplateUnit.KM.ToString(), MaintenanceTemplateUnit.KM.ToString())
            objUnit.Add(MaintenanceTemplateUnit.Minggu.ToString(), MaintenanceTemplateUnit.Minggu.ToString())
            objUnit.Add(MaintenanceTemplateUnit.Bulan.ToString(), MaintenanceTemplateUnit.Bulan.ToString())
            objUnit.Add(MaintenanceTemplateUnit.Tahun.ToString(), MaintenanceTemplateUnit.Tahun.ToString())
            ViewData("Unit") = New SelectList(objUnit, "key", "value", IIf(model IsNot Nothing, model.Unit, ""))

            'list of MachineType
            Dim objMachineType = (From m In ctx.MachineTypes Select m).ToList()
            ViewData("MachineTypeID") = New SelectList(objMachineType, "ID", "MachineTypeName", IIf(Not IsNothing(model), model.MachineTypeID, 0))
            ViewData("MachineType") = objMachineType.Select(Function(s) s.ID).ToArray()
        End Sub

        <HttpPost()> _
        Function Save(ByVal model As MaintenanceTemplateItem) As ActionResult
            If ModelState.IsValid Then
                ctx.MaintenanceTemplateItems.AddObject(model)
                If model.ID <> Nothing Then
                    ctx.ObjectStateManager.ChangeObjectState(model, EntityState.Modified)
                End If
                ctx.SaveChanges()
                Return Json(New With {.id = model.ID, .stat = 1, .machineTypeID = model.MachineTypeID})
            End If
            Return Json(New With {.stat = 0, .errors = HaloUIHelpers.Helpers.JsonErrorObject.Convert(ModelState)})
        End Function

        Function Detail(ByVal id As Integer) As ActionResult
            Dim model = ctx.MaintenanceTemplateItems.Where(Function(m) m.ID = id).FirstOrDefault()
            Return View(model)
        End Function

        <HttpPost()>
        Public Function SavePartial(ByVal pk As Integer, name As String, value As String) As ActionResult
            Dim model = ctx.MaintenanceTemplateItems.Where(Function(m) m.ID = pk).SingleOrDefault()
            If model Is Nothing Then
                ModelState.AddModelError("General", "Data tidak ditemukan. Muat ulang halaman ini")
                Return Json(New With {.stat = 0, .errors = HaloUIHelpers.Helpers.JsonErrorObject.Convert(ModelState)})
            End If
            Select Case name
                Case "Value"
                    Dim d As Decimal
                    Decimal.TryParse(value, d)
                    model.GetType.GetProperty(name).SetValue(model, d, Nothing)
                Case Else
                    model.GetType.GetProperty(name).SetValue(model, value, Nothing)
            End Select
            ctx.SaveChanges()
            Return Json(New With {.stat = 1})
        End Function

        Function Delete(ByVal id As Integer) As JsonResult
            Dim stat As Integer = 0
            Dim msg As String = ""
            Dim msgDesc As String = ""
            Try
                Dim data = (From pl In ctx.MaintenanceTemplateItems Where pl.ID = id).FirstOrDefault()
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
