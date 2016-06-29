Namespace Equipment.Areas.Equipment.Controllers
    Public Class VehicleMaintenanceController
        Inherits System.Web.Mvc.Controller
        Private currentUserProfile As ERPBase.ErpUserProfile
        Private _eqpEntities As EquipmentEntities

        Public Function Index() As ActionResult
            Return View()
        End Function

        <HttpPost()>
        Public Function GetMaintenanceList(ByVal selectedMonth As Integer, ByVal selectedYear As Integer) As JsonResult
            Dim dateStart = DateSerial(selectedYear, selectedMonth, 1)
            Dim dateEnd = dateStart.AddMonths(1).AddDays(-1)
            Dim models = From m In _eqpEntities.VehicleMaintenanceRecords
                         Where (m.MaintenanceDateStart >= dateStart AndAlso m.MaintenanceDateEnd <= dateEnd) AndAlso _
                         m.Vehicle.IDArea = currentUserProfile.WorkUnitId
                         Select New With {m.Id, m.Vehicle.Code, m.Vehicle.PoliceNumber, m.Vehicle.Species,
                                          .Merk = String.Concat(m.Vehicle.Merk, "/", m.Vehicle.Type),
                                          m.MaintenanceDateStart, m.MaintenanceState}

            Return Json(New With {.data = models})
        End Function
        
        Public Function Detail(ByVal Id As Integer) As ActionResult
            Dim model = _eqpEntities.VehicleMaintenanceRecords.Where(Function(m) m.Id = Id).SingleOrDefault()
            If model Is Nothing Then
                Throw New HttpException(404, "NOT FOUND")
            End If

            Return View(model)
        End Function


        ''' <summary>
        ''' 
        ''' </summary>
        ''' <param name="Id">vehicle Id</param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function CreateMaintenanceRecord(Optional ByVal Id As Integer = 0) As ActionResult
            Dim modVehicle As Vehicle
            Dim model As New VehicleMaintenanceRecord

            If Id = 0 Then
                modVehicle = New Vehicle
                modVehicle.Code = ""
            Else
                modVehicle = _eqpEntities.Vehicles.Where(Function(m) m.ID = Id).SingleOrDefault()
                If modVehicle Is Nothing Then
                    Throw New HttpException(404, "NOT FOUND")
                End If

            End If

            modVehicle.VehicleMaintenanceRecords.Add(model)
            model.MaintenanceDateStart = Date.Today.AddHours(8)
            model.MaintenanceDateEnd = Date.Today.AddHours(9).AddMinutes(30)
            Return View("FormMaintentance", model)
        End Function

        <HttpGet()>
        Public Function Edit(ByVal id As Integer) As ActionResult
            Dim model = _eqpEntities.VehicleMaintenanceRecords.Where(Function(m) m.Id = id).SingleOrDefault()
            If model Is Nothing Then
                Throw New HttpException(404, "NOT FOUND")
            End If
            Return View("FormMaintentance", model)
        End Function


        <HttpPost()>
        Public Function GetMaintenanceTemplateItem(ByVal term As String) As JsonResult
            Dim models = (From m In _eqpEntities.MaintenanceTemplateItems
                         Where m.MachineTypeID = 1 AndAlso m.Item.Contains(term)
                         Select New With {.Id = 0, .Item = m.Item, .VehicleMaintenanceCheckItemId = 0,
                                          .VehicleMaintenanceRecordItemId = 0}).ToList()

            Dim models2 = (From m In _eqpEntities.VehicleMaintenanceRecordItems
                          Where m.Item.Contains(term)
                          Select New With {.Id = 0, .Item = m.Item, .VehicleMaintenanceCheckItemId = 0,
                                          .VehicleMaintenanceRecordItemId = 0}).ToList()

          
            'get unique
            Dim rvalue = models.Union(models2).GroupBy(Function(m) m.Item).Select(Function(m) m.First).ToArray()

          
            Return Json(rvalue)
        End Function



        ''' <summary>
        ''' Used to load planning item to service.
        ''' </summary>
        ''' <param name="id">vehicle id</param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        <HttpPost()>
        Public Function GetItemToService(ByVal id As Integer) As JsonResult
            Dim startDate As Date = Date.Today
            While startDate.DayOfWeek <> DayOfWeek.Sunday
                startDate = startDate.AddDays(-1)
            End While
            Dim enddate As Date = startDate.AddDays(6)
            Dim itemToService = _eqpEntities.ExecuteStoreQuery(Of VehicleMaintenanceRecordItem)("EXEC [Eqp].[GetScheduledMaintenancePlanForVehicle] @vehicleId, @checkdate",
                                                                                              New SqlClient.SqlParameter("@vehicleId", id),
                                                                                              New SqlClient.SqlParameter("@Checkdate", enddate))



            Dim rvalue = From m In itemToService
                         Select New With {m.Id, m.Item, m.VehicleMaintenanceCheckItemId, m.VehicleMaintenanceRecordId}


            Return Json(New With {.data = rvalue})
        End Function

        ''' <summary>
        ''' Used to load items on maintenance
        ''' </summary>
        ''' <param name="id"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        <HttpPost()>
        Public Function GetServicedItems(ByVal id As Integer) As JsonResult
            Dim models = From m In _eqpEntities.VehicleMaintenanceRecordItems
                         Where m.VehicleMaintenanceRecordId = id
                         Select New With {m.Id, m.Item, m.VehicleMaintenanceCheckItemId, m.VehicleMaintenanceRecordId}

            Return Json(New With {.data = models})
        End Function


        <HttpPost()>
        Public Function GetMaterialUsed(ByVal Id As Integer) As JsonResult
            Dim models = From m In _eqpEntities.VehicleMaintenanceRecordMaterialUseds
                         Where m.VehicleMaintenanceRecordId = Id
                         Select New With {m.Id, m.MaterialUsed, m.Quantity, m.UnitQuantity}

            Return Json(New With {.data = models})
        End Function

        <HttpPost()>
        Public Function GetMaintenanceOther(ByVal Id As Integer) As JsonResult
            Dim models = From m In _eqpEntities.VehicleMaintenanceRecordOthers
                         Where m.VehicleMaintenanceRecordId = Id
                         Select New With {m.Id, m.Item, m.Cost, m.Remarks}

            Return Json(New With {.data = models})
        End Function
        <HttpPost()>
        Function ValidateItemToMaintain(ByVal model As VehicleMaintenanceRecordItem) As JsonResult
            If String.IsNullOrWhiteSpace(model.Item) Then
                ModelState.Clear()
                ModelState.AddModelError("ItemToMaintainName", "Item tidak boleh kosong")
                Return Json(New With {.stat = 0, .errors = HaloUIHelpers.Helpers.JsonErrorObject.Convert(ModelState)})
            End If
            Dim rvalue = New With {model.Id, model.Item, model.VehicleMaintenanceCheckItemId, model.VehicleMaintenanceRecordId}
            Return Json(New With {.stat = 1, .model = rvalue})
        End Function
        <HttpPost()>
        Function ValidateItemMaterialUsed(ByVal model As VehicleMaintenanceRecordMaterialUsed, ByVal rowIdx As Integer) As JsonResult
            If ModelState.IsValid Then
                Dim rvalue = New With {model.Id, model.MaterialUsed, model.Quantity, model.UnitQuantity, model.VehicleMaintenanceRecordId}
                Return Json(New With {.stat = 1, .model = rvalue, .rowIdx = rowIdx})
            End If
            Return Json(New With {.stat = 0, .errors = HaloUIHelpers.Helpers.JsonErrorObject.Convert(ModelState)})
        End Function
        <HttpPost()>
        Function ValidateItemOther(ByVal model As VehicleMaintenanceRecordOther, ByVal rowIdx As Integer) As JsonResult
            If ModelState.IsValid Then
                Dim rvalue = New With {model.Id, model.Item, model.Cost, model.Remarks, model.VehicleMaintenanceRecordId}
                Return Json(New With {.stat = 1, .model = rvalue, .rowIdx = rowIdx})
            End If
            Return Json(New With {.stat = 0, .errors = HaloUIHelpers.Helpers.JsonErrorObject.Convert(ModelState)})
        End Function

        <HttpPost()>
        Function Delete(ByVal Id As Integer) As JsonResult
            Dim model = _eqpEntities.VehicleMaintenanceRecords.Where(Function(m) m.Id = Id).SingleOrDefault
            If model IsNot Nothing Then
                Try
                    _eqpEntities.DeleteObject(model)
                    _eqpEntities.SaveChanges()
                Catch ex As Exception
                    Return Json(New With {.stat = 0, .errors = HaloUIHelpers.Helpers.JsonErrorObject.Convert(ex)})
                End Try

            End If
            Return Json(New With {.stat = 1})
        End Function

        <HttpPost()>
        Function DeleteItemToMaintain(ByVal Id As Integer) As JsonResult
            Dim model = _eqpEntities.VehicleMaintenanceRecordItems.Where(Function(m) m.Id = Id).SingleOrDefault
            If model IsNot Nothing Then
                Try
                    _eqpEntities.DeleteObject(model)
                    _eqpEntities.SaveChanges()
                Catch ex As Exception
                    Return Json(New With {.stat = 0, .errors = HaloUIHelpers.Helpers.JsonErrorObject.Convert(ex)})
                End Try

            End If
            Return Json(New With {.stat = 1})
        End Function
        <HttpPost()>
        Function DeleteMaterialUsed(ByVal Id As Integer) As JsonResult
            Dim model = _eqpEntities.VehicleMaintenanceRecordMaterialUseds.Where(Function(m) m.Id = Id).SingleOrDefault
            If model IsNot Nothing Then
                Try
                    _eqpEntities.DeleteObject(model)
                    _eqpEntities.SaveChanges()
                Catch ex As Exception
                    Return Json(New With {.stat = 0, .errors = HaloUIHelpers.Helpers.JsonErrorObject.Convert(ex)})
                End Try

            End If
            Return Json(New With {.stat = 1})
        End Function

        Function SaveMaintenanceRecord(ByVal model As VehicleMaintenanceRecord, ByVal sVehicleMaintenanceRecordItem As String,
                                       ByVal sVehicleMaintenanceRecordMaterialUsed As String,
                                       ByVal sVehicleMaintenanceRecordOther As String) As ActionResult

            If ModelState.IsValid Then
                Dim thisVehicleMaintenanceCheck = (From m In _eqpEntities.VehicleMaintenanceChecks
                                                    Where m.VehicleId = model.VehicleId).SingleOrDefault()


                Dim MaintenanceCheckItems = thisVehicleMaintenanceCheck.VehicleMaintenanceCheckItems
                Dim VehicleMaintenanceRecordItems = New List(Of VehicleMaintenanceRecordItem)

                Newtonsoft.Json.JsonConvert.PopulateObject(sVehicleMaintenanceRecordItem, VehicleMaintenanceRecordItems)
                If VehicleMaintenanceRecordItems.Count > 0 Then
                    For Each item In VehicleMaintenanceRecordItems
                        If item.VehicleMaintenanceCheckItemId = 0 Then
                            If thisVehicleMaintenanceCheck IsNot Nothing Then
                                ''find the name first
                                Dim theName = item.Item
                                Dim found = (From m In MaintenanceCheckItems
                                            Where m.ItemName.Equals(theName, StringComparison.CurrentCultureIgnoreCase)).SingleOrDefault()
                                If found Is Nothing Then

                                    Dim newCheckItem = New VehicleMaintenanceCheckItem
                                    newCheckItem.ItemName = item.Item
                                    newCheckItem.LastCheck_Date = Date.Today
                                    newCheckItem.LastCheck_Kilometer = model.OdometerValue
                                    newCheckItem.UnitValue = "Bulan"
                                    newCheckItem.Value = 6
                                    thisVehicleMaintenanceCheck.VehicleMaintenanceCheckItems.Add(newCheckItem)
                                    _eqpEntities.SaveChanges()
                                    item.VehicleMaintenanceCheckItemId = newCheckItem.ID
                                Else
                                    item.VehicleMaintenanceCheckItemId = found.ID
                                End If
                            End If
                        End If

                    Next
                Else
                    ModelState.AddModelError("General", "Item Pekerjaan harus diisi")
                    Return Json(New With {.stat = 0, .errors = HaloUIHelpers.Helpers.JsonErrorObject.Convert(ModelState)})
                End If





                _eqpEntities.VehicleMaintenanceRecords.AddObject(model)
                If model.Id <> 0 Then
                    _eqpEntities.ObjectStateManager.ChangeObjectState(model, EntityState.Modified)
                End If
                If VehicleMaintenanceRecordItems.Count > 0 Then
                    For Each item In VehicleMaintenanceRecordItems
                        model.VehicleMaintenanceRecordItems.Add(item)


                        Dim VehicleMaintenanceCheckItemId = item.VehicleMaintenanceCheckItemId
                        Dim checkitem = (From m In _eqpEntities.VehicleMaintenanceCheckItems
                                        Where m.ID = VehicleMaintenanceCheckItemId).SingleOrDefault
                        If checkitem IsNot Nothing Then
                            checkitem.LastCheck_Date = model.MaintenanceDateStart
                            checkitem.LastCheck_Kilometer = model.OdometerValue
                        End If
                        If item.Id > 0 Then
                            _eqpEntities.ObjectStateManager.ChangeObjectState(item, EntityState.Modified)
                        End If

                    Next
                End If

                Dim VehicleMaintenanceRecordMaterialUseds = New List(Of VehicleMaintenanceRecordMaterialUsed)
                Newtonsoft.Json.JsonConvert.PopulateObject(sVehicleMaintenanceRecordMaterialUsed, VehicleMaintenanceRecordMaterialUseds)
                If VehicleMaintenanceRecordMaterialUseds.Count > 0 Then
                    For Each item In VehicleMaintenanceRecordMaterialUseds
                        model.VehicleMaintenanceRecordMaterialUseds.Add(item)
                        If item.Id > 0 Then
                            _eqpEntities.ObjectStateManager.ChangeObjectState(item, EntityState.Modified)
                        End If
                    Next
                End If
                Dim VehicleMaintenanceRecordOthers = New List(Of VehicleMaintenanceRecordOther)
                Newtonsoft.Json.JsonConvert.PopulateObject(sVehicleMaintenanceRecordOther, VehicleMaintenanceRecordOthers)
                If VehicleMaintenanceRecordOthers.Count > 0 Then
                    For Each item In VehicleMaintenanceRecordOthers
                        model.VehicleMaintenanceRecordOthers.Add(item)
                        If item.Id > 0 Then
                            _eqpEntities.ObjectStateManager.ChangeObjectState(item, EntityState.Modified)
                        End If
                    Next
                End If
                _eqpEntities.SaveChanges()
                Return Json(New With {.stat = 1})
            End If
            Return Json(New With {.stat = 0, .errors = HaloUIHelpers.Helpers.JsonErrorObject.Convert(ModelState)})
        End Function

        <HttpGet()>
        Public Function ProposedMaintaenanceList() As ActionResult
            Return View()
        End Function
        <HttpPost()>
        Public Function GetProposedVehicleToMaintainList() As JsonResult
            Dim startDate As Date = Date.Today
            While startDate.DayOfWeek <> DayOfWeek.Sunday
                startDate = startDate.AddDays(-1)
            End While
            Dim enddate As Date = startDate.AddDays(6)

            Dim model = _eqpEntities.ExecuteStoreQuery(Of VehiclePeriodicMaintenanceListView)("EXEC eqp.GetScheduledVehicleMaintenancePlan  @startdate, @enddate, @areaid",
                                                                                              New SqlClient.SqlParameter("@startdate", startDate),
                                                                                              New SqlClient.SqlParameter("@enddate", enddate),
                                                                                              New SqlClient.SqlParameter("@areaid", currentUserProfile.WorkUnitId))

            Dim groupped = From m In model
                           Group m By m.VehicleId, m.Type, m.Code,
                           m.Species, m.PoliceNumber, m.Merk Into Vehicle = Group


            Return Json(New With {.data = groupped})
        End Function



        Public Sub New()
            _eqpEntities = New EquipmentEntities
            currentUserProfile = ERPBase.ErpUserProfile.GetUserProfile()
        End Sub
    End Class
End Namespace