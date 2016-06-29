Namespace Equipment.Areas.Equipment.Controllers
    Public Class MachineMaintenanceController
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
            Dim models = From m In _eqpEntities.MachineMaintenanceRecords
                         Where (m.MaintenanceDateStart >= dateStart AndAlso m.MaintenanceDateEnd <= dateEnd) AndAlso _
                         m.MachineEqp.IDArea = currentUserProfile.WorkUnitId
                         Select New With {m.Id, m.MachineEqp.Name, m.MachineEqp.SerialNumber, m.MachineEqp.Remark,
                                          .Merk = String.Concat(m.MachineEqp.Merk, "/", m.MachineEqp.Type),
                                          m.MaintenanceDateStart, m.MaintenanceState}

            Return Json(New With {.data = models})
        End Function

        Public Function Detail(ByVal Id As Integer) As ActionResult
            Dim model = _eqpEntities.MachineMaintenanceRecords.Where(Function(m) m.Id = Id).SingleOrDefault()
            If model Is Nothing Then
                Throw New HttpException(404, "NOT FOUND")
            End If

            Return View(model)
        End Function


        ''' <summary>
        ''' 
        ''' </summary>
        ''' <param name="Id">Machine Id</param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function CreateMaintenanceRecord(Optional ByVal Id As Integer = 0) As ActionResult
            Dim modMachine As MachineEqp
            Dim model As New MachineMaintenanceRecord

            If Id = 0 Then
                modMachine = New MachineEqp
            Else
                modMachine = _eqpEntities.MachineEqps.Where(Function(m) m.ID = Id).SingleOrDefault()
                If modMachine Is Nothing Then
                    Throw New HttpException(404, "NOT FOUND")
                End If
            End If

            modMachine.MachineMaintenanceRecords.Add(model)
            model.MaintenanceDateStart = Date.Today.AddHours(8)
            model.MaintenanceDateEnd = Date.Today.AddHours(9).AddMinutes(30)
            Return View("FormMaintenance", model)
        End Function

        <HttpGet()>
        Public Function Edit(ByVal id As Integer) As ActionResult
            Dim model = _eqpEntities.MachineMaintenanceRecords.Where(Function(m) m.Id = id).SingleOrDefault()
            If model Is Nothing Then
                Throw New HttpException(404, "NOT FOUND")
            End If
            Return View("FormMaintenance", model)
        End Function


        <HttpPost()>
        Public Function GetMaintenanceTemplateItem(ByVal term As String) As JsonResult
            Dim models = (From m In _eqpEntities.MaintenanceTemplateItems
                         Where m.MachineTypeID = 3 AndAlso m.Item.Contains(term)
                         Select New With {.Id = 0, .Item = m.Item, .MachineMaintenanceCheckItemId = 0,
                                          .MachineMaintenanceRecordItemId = 0}).ToList()

            Dim models2 = (From m In _eqpEntities.MachineMaintenanceRecordItems
                          Where m.Item.Contains(term)
                          Select New With {.Id = 0, .Item = m.Item, .MachineMaintenanceCheckItemId = 0,
                                          .MachineMaintenanceRecordItemId = 0}).ToList()


            'get unique
            Dim rvalue = models.Union(models2).GroupBy(Function(m) m.Item).Select(Function(m) m.First).ToArray()


            Return Json(rvalue)
        End Function



        ''' <summary>
        ''' Used to load planning item to service.
        ''' </summary>
        ''' <param name="id">Machine id</param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        <HttpPost()>
        Public Function GetItemToService(ByVal id As Integer) As JsonResult
            Dim startDate As Date = Date.Today
            While startDate.DayOfWeek <> DayOfWeek.Sunday
                startDate = startDate.AddDays(-1)
            End While
            Dim enddate As Date = startDate.AddDays(6)
            Dim itemToService = _eqpEntities.ExecuteStoreQuery(Of MachineMaintenanceRecordItem)("EXEC [Eqp].[GetScheduledMaintenancePlanForMachine] @machineId, @checkdate",
                                                                                              New SqlClient.SqlParameter("@machineId", id),
                                                                                              New SqlClient.SqlParameter("@Checkdate", enddate))

            Dim rvalue = From m In itemToService
                         Select New With {m.Id, m.Item, m.MachineMaintenanceCheckItemId, m.MachineMaintenanceRecordId}

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
            Dim models = From m In _eqpEntities.MachineMaintenanceRecordItems
                         Where m.MachineMaintenanceRecordId = id
                         Select New With {m.Id, m.Item, m.MachineMaintenanceCheckItemId, m.MachineMaintenanceRecordId}

            Return Json(New With {.data = models})
        End Function


        <HttpPost()>
        Public Function GetMaterialUsed(ByVal Id As Integer) As JsonResult
            Dim models = From m In _eqpEntities.MachineMaintenanceRecordMaterialUseds
                         Where m.MachineMaintenanceRecordId = Id
                         Select New With {m.Id, m.MaterialUsed, m.Quantity, m.UnitQuantity}

            Return Json(New With {.data = models})
        End Function

        <HttpPost()>
        Public Function GetMaintenanceOther(ByVal Id As Integer) As JsonResult
            Dim models = From m In _eqpEntities.MachineMaintenanceRecordOthers
                         Where m.MachineMaintenanceRecordId = Id
                         Select New With {m.Id, m.Item, m.Cost, m.Remarks}

            Return Json(New With {.data = models})
        End Function
        <HttpPost()>
        Function ValidateItemToMaintain(ByVal model As MachineMaintenanceRecordItem) As JsonResult
            If String.IsNullOrWhiteSpace(model.Item) Then
                ModelState.Clear()
                ModelState.AddModelError("ItemToMaintainName", "Item tidak boleh kosong")
                Return Json(New With {.stat = 0, .errors = HaloUIHelpers.Helpers.JsonErrorObject.Convert(ModelState)})
            End If
            Dim rvalue = New With {model.Id, model.Item, model.MachineMaintenanceCheckItemId, model.MachineMaintenanceRecordId}
            Return Json(New With {.stat = 1, .model = rvalue})
        End Function
        <HttpPost()>
        Function ValidateItemMaterialUsed(ByVal model As MachineMaintenanceRecordMaterialUsed, ByVal rowIdx As Integer) As JsonResult
            If ModelState.IsValid Then
                Dim rvalue = New With {model.Id, model.MaterialUsed, model.Quantity, model.UnitQuantity, model.MachineMaintenanceRecordId}
                Return Json(New With {.stat = 1, .model = rvalue, .rowIdx = rowIdx})
            End If
            Return Json(New With {.stat = 0, .errors = HaloUIHelpers.Helpers.JsonErrorObject.Convert(ModelState)})
        End Function
        <HttpPost()>
        Function ValidateItemOther(ByVal model As MachineMaintenanceRecordOther, ByVal rowIdx As Integer) As JsonResult
            If ModelState.IsValid Then
                Dim rvalue = New With {model.Id, model.Item, model.Cost, model.Remarks, model.MachineMaintenanceRecordId}
                Return Json(New With {.stat = 1, .model = rvalue, .rowIdx = rowIdx})
            End If
            Return Json(New With {.stat = 0, .errors = HaloUIHelpers.Helpers.JsonErrorObject.Convert(ModelState)})
        End Function


        <HttpPost()>
        Function Delete(ByVal Id As Integer) As JsonResult
            Dim model = _eqpEntities.MachineMaintenanceRecords.Where(Function(m) m.Id = Id).SingleOrDefault
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
            Dim model = _eqpEntities.MachineMaintenanceRecordItems.Where(Function(m) m.Id = Id).SingleOrDefault
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
            Dim model = _eqpEntities.MachineMaintenanceRecordMaterialUseds.Where(Function(m) m.Id = Id).SingleOrDefault
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

        Function SaveMaintenanceRecord(ByVal model As MachineMaintenanceRecord, ByVal sMachineMaintenanceRecordItem As String,
                                       ByVal sMachineMaintenanceRecordMaterialUsed As String,
                                       ByVal sMachineMaintenanceRecordOther As String) As ActionResult

            If ModelState.IsValid Then
                Dim thisMachineMaintenanceCheck = (From m In _eqpEntities.MachineEqpMaintenanceChecks
                                                    Where m.MachineEqpId = model.MachineId).SingleOrDefault()

                Dim MaintenanceCheckItems = thisMachineMaintenanceCheck.MachineEqpMaintenanceCheckItems
                Dim MachineMaintenanceRecordItems = New List(Of MachineMaintenanceRecordItem)

                Newtonsoft.Json.JsonConvert.PopulateObject(sMachineMaintenanceRecordItem, MachineMaintenanceRecordItems)
                If MachineMaintenanceRecordItems.Count > 0 Then
                    For Each item In MachineMaintenanceRecordItems
                        If item.MachineMaintenanceCheckItemId = 0 Then
                            If thisMachineMaintenanceCheck IsNot Nothing Then
                                ''find the name first
                                Dim theName = item.Item
                                Dim found = (From m In MaintenanceCheckItems
                                            Where m.ItemName.Equals(theName, StringComparison.CurrentCultureIgnoreCase)).SingleOrDefault()
                                If found Is Nothing Then
                                    Dim newCheckItem = New MachineEqpMaintenanceCheckItem
                                    newCheckItem.ItemName = item.Item
                                    newCheckItem.LastCheck_Date = Date.Today
                                    newCheckItem.UnitValue = "Bulan"
                                    newCheckItem.Value = 6
                                    thisMachineMaintenanceCheck.MachineEqpMaintenanceCheckItems.Add(newCheckItem)
                                    _eqpEntities.SaveChanges()
                                    item.MachineMaintenanceCheckItemId = newCheckItem.ID
                                Else
                                    item.MachineMaintenanceCheckItemId = found.ID
                                End If
                            End If
                        End If

                    Next
                Else
                    ModelState.AddModelError("General", "Item Pekerjaan harus diisi")
                    Return Json(New With {.stat = 0, .errors = HaloUIHelpers.Helpers.JsonErrorObject.Convert(ModelState)})
                End If

                _eqpEntities.MachineMaintenanceRecords.AddObject(model)
                If model.Id <> 0 Then
                    _eqpEntities.ObjectStateManager.ChangeObjectState(model, EntityState.Modified)
                End If
                If MachineMaintenanceRecordItems.Count > 0 Then
                    For Each item In MachineMaintenanceRecordItems
                        model.MachineMaintenanceRecordItems.Add(item)


                        Dim MachineMaintenanceCheckItemId = item.MachineMaintenanceCheckItemId
                        Dim checkitem = (From m In _eqpEntities.MachineEqpMaintenanceCheckItems
                                        Where m.ID = MachineMaintenanceCheckItemId).SingleOrDefault
                        If checkitem IsNot Nothing Then
                            checkitem.LastCheck_Date = model.MaintenanceDateStart
                        End If
                        If item.Id > 0 Then
                            _eqpEntities.ObjectStateManager.ChangeObjectState(item, EntityState.Modified)
                        End If

                    Next
                End If

                Dim MachineMaintenanceRecordMaterialUseds = New List(Of MachineMaintenanceRecordMaterialUsed)
                Newtonsoft.Json.JsonConvert.PopulateObject(sMachineMaintenanceRecordMaterialUsed, MachineMaintenanceRecordMaterialUseds)
                If MachineMaintenanceRecordMaterialUseds.Count > 0 Then
                    For Each item In MachineMaintenanceRecordMaterialUseds
                        model.MachineMaintenanceRecordMaterialUseds.Add(item)
                        If item.Id > 0 Then
                            _eqpEntities.ObjectStateManager.ChangeObjectState(item, EntityState.Modified)
                        End If
                    Next
                End If
                Dim MachineMaintenanceRecordOthers = New List(Of MachineMaintenanceRecordOther)
                Newtonsoft.Json.JsonConvert.PopulateObject(sMachineMaintenanceRecordOther, MachineMaintenanceRecordOthers)
                If MachineMaintenanceRecordOthers.Count > 0 Then
                    For Each item In MachineMaintenanceRecordOthers
                        model.MachineMaintenanceRecordOthers.Add(item)
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
        Public Function ProposedMaintenanceList() As ActionResult
            Return View()
        End Function
        <HttpPost()>
        Public Function GetProposedMachineToMaintainList() As JsonResult
            Dim startDate As Date = Date.Today
            While startDate.DayOfWeek <> DayOfWeek.Sunday
                startDate = startDate.AddDays(-1)
            End While
            Dim enddate As Date = startDate.AddDays(6)
            Dim model = _eqpEntities.ExecuteStoreQuery(Of MachinePeriodicMaintenanceListView)("EXEC eqp.GetScheduledMachineMaintenancePlan  @startdate, @enddate, @areaid",
                                                                                              New SqlClient.SqlParameter("@startdate", startDate),
                                                                                              New SqlClient.SqlParameter("@enddate", enddate),
                                                                                              New SqlClient.SqlParameter("@areaid", currentUserProfile.WorkUnitId))

            Dim groupped = From m In model
                           Group m By m.MachineEqpId, m.Type, m.SerialNumber, m.Merk, m.Name Into Machine = Group

            Return Json(New With {.data = groupped})
        End Function


        Public Sub New()
            _eqpEntities = New EquipmentEntities
            currentUserProfile = ERPBase.ErpUserProfile.GetUserProfile()
        End Sub
    End Class
End Namespace