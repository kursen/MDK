Namespace Equipment.Areas.Equipment.Controllers
    Public Class VehiclePeriodicMaintenanceController
        Inherits System.Web.Mvc.Controller
        Private _eqpEntities As EquipmentEntities
        Public Function Index() As ActionResult
            Return View()
        End Function

        Public Function MaintenanceSchema(ByVal Id As Integer) As ActionResult

            Dim model = (From m In _eqpEntities.VehicleMaintenanceChecks
                         Where m.VehicleId = Id).SingleOrDefault()

            If model Is Nothing Then
                model = New VehicleMaintenanceCheck
                model.VehicleId = Id
                model.AverageUsagePerKilometer = 100

                Dim maintenanceTemplate = (From m In _eqpEntities.MaintenanceTemplateItems
                                          Where m.MachineTypeID = 1).ToList()

                For Each item In maintenanceTemplate
                    Dim itemCheck = New VehicleMaintenanceCheckItem
                    itemCheck.ItemName = item.Item
                    itemCheck.Value = item.Value
                    itemCheck.UnitValue = item.Unit
                    itemCheck.LastCheck_Date = New Date(1900, 1, 1)
                    itemCheck.LastCheck_Kilometer = 0
                    model.VehicleMaintenanceCheckItems.Add(itemCheck)
                Next
                _eqpEntities.VehicleMaintenanceChecks.AddObject(model)
                _eqpEntities.SaveChanges()
            End If
            ' If modelma Is Nothing Then model = New MaintenanceTemplateItem
            Dim objUnit As New Dictionary(Of String, String)
            objUnit.Add(MaintenanceTemplateUnit.KM.ToString(), MaintenanceTemplateUnit.KM.ToString())
            objUnit.Add(MaintenanceTemplateUnit.Minggu.ToString(), MaintenanceTemplateUnit.Minggu.ToString())
            objUnit.Add(MaintenanceTemplateUnit.Bulan.ToString(), MaintenanceTemplateUnit.Bulan.ToString())
            objUnit.Add(MaintenanceTemplateUnit.Tahun.ToString(), MaintenanceTemplateUnit.Tahun.ToString())
            ViewData("UnitValue") = New SelectList(objUnit, "key", "value")

            Return View(model)
        End Function

        Public Function GetListMaintenanceSchemaItem(ByVal Id As Integer) As ActionResult
            Dim model = From m In _eqpEntities.VehicleMaintenanceCheckItems
                        Where m.VehicleMaintenanceCheckId = Id
                            Select New With {m.ID, m.ItemName, m.Value, m.UnitValue, m.LastCheck_Date, m.LastCheck_Kilometer, m.VehicleMaintenanceCheckId}


            Return Json(New With {.data = model})
        End Function

        Public Function Create() As ActionResult
            Return View("formPlan")
        End Function

        Function ValidateVehicleCheckItem(ByVal item As VehicleMaintenanceCheckItem, ByVal rowIdx As Integer) As ActionResult
            If ModelState.IsValid Then
                Dim model = New With {item.ID, item.ItemName,
                                      item.VehicleMaintenanceCheckId, item.LastCheck_Date, item.Value,
                                      item.UnitValue, item.LastCheck_Kilometer}
                Return Json(New With {.stat = 1, .idx = rowIdx, .model = model})
            End If
            Return Json(New With {.stat = 0, .errors = HaloUIHelpers.Helpers.JsonErrorObject.Convert(ModelState)}, JsonRequestBehavior.AllowGet)
        End Function

        Function SaveVehicleMaintenance(ByVal header As VehicleMaintenanceCheck, ByVal ItemDetailCheck As String) As ActionResult
            If ModelState.IsValid Then
                _eqpEntities.VehicleMaintenanceChecks.AddObject(header)
                If header.ID > 0 Then
                    _eqpEntities.ObjectStateManager.ChangeObjectState(header, EntityState.Modified)
                End If
                Dim ItemChecks As New List(Of VehicleMaintenanceCheckItem)
                Newtonsoft.Json.JsonConvert.PopulateObject(ItemDetailCheck, ItemChecks)

                If ItemChecks.Count <> 0 Then
                    For Each listitem In ItemChecks
                        header.VehicleMaintenanceCheckItems.Add(listitem)
                        If listitem.ID > 0 Then
                            _eqpEntities.ObjectStateManager.ChangeObjectState(listitem, EntityState.Modified)
                        End If
                    Next
                End If

                _eqpEntities.SaveChanges()
                Return Json(New With {.stat = 1})

            End If
            Return Json(New With {.stat = -0, .errors = HaloUIHelpers.Helpers.JsonErrorObject.Convert(ModelState)})
        End Function

        Public Sub New()
            _eqpEntities = New EquipmentEntities
        End Sub

        Function DeleteMaintenanceItem(ByVal id As Integer) As JsonResult

            Try
                Dim data = (From pl In _eqpEntities.VehicleMaintenanceCheckItems Where pl.ID = id).FirstOrDefault()
                If data IsNot Nothing Then _eqpEntities.DeleteObject(data)
                _eqpEntities.SaveChanges()
                Return Json(New With {.stat = 1})
            Catch ex As Exception
                Return Json(New With {.stat = 0, .errors = HaloUIHelpers.Helpers.JsonErrorObject.Convert(ex)})
            End Try

        End Function

    End Class
End Namespace
