Namespace Equipment.Areas.Equipment.Controllers
    Public Class HeavyEquipmentMaintenanceController
        Inherits System.Web.Mvc.Controller

        '
        ' GET: /Equipment/HeavyEquipmentMaintenance
        Private currentUserProfile As ERPBase.ErpUserProfile
        Private _eqpEntities As EquipmentEntities
        Function Index() As ActionResult
            Return View()
        End Function

        <HttpPost()>
        Public Function GetMaintenanceList(ByVal selectedMonth As Integer, ByVal selectedYear As Integer) As JsonResult
            Dim dateStart = DateSerial(selectedYear, selectedMonth, 1)
            Dim dateEnd = dateStart.AddMonths(1).AddDays(-1)
            Dim models = From m In _eqpEntities.HeavyEqpMaintenanceRecords
                         Where (m.MaintenanceDateStart >= dateStart AndAlso m.MaintenanceDateEnd <= dateEnd) AndAlso _
                         m.HeavyEqp.IDArea = currentUserProfile.WorkUnitId
                         Select New With {m.Id, m.HeavyEqp.Code, m.HeavyEqp.Species,
                                          .Merk = String.Concat(m.HeavyEqp.Merk, "/", m.HeavyEqp.Type),
                                          m.MaintenanceDateStart, m.MaintenanceState}



            Return Json(New With {.data = models})
        End Function

        Public Function CreateMaintenanceRecord(Optional ByVal Id As Integer = 0) As ActionResult
            Dim modHeavyEqp As HeavyEqp
            Dim model As New HeavyEqpMaintenanceRecord

            If Id = 0 Then
                modHeavyEqp = New HeavyEqp
                modHeavyEqp.Code = ""
            Else
                modHeavyEqp = _eqpEntities.HeavyEqps.Where(Function(m) m.ID = Id).SingleOrDefault()
                If modHeavyEqp Is Nothing Then
                    Throw New HttpException(404, "NOT FOUND")
                End If

            End If

            modHeavyEqp.HeavyEqpMaintenanceRecords.Add(model)
            model.MaintenanceDateStart = Date.Today.AddHours(8)
            model.MaintenanceDateEnd = Date.Today.AddHours(9).AddMinutes(30)
            Return View("FormMaintentance", model)
        End Function

        Function SaveMaintenanceRecord(ByVal model As HeavyEqpMaintenanceRecord, ByVal sHeavyEqpMaintenanceRecordItem As String,
                                      ByVal sHeavyEqpMaintenanceRecordMaterialUsed As String,
                                      ByVal sHeavyEqpMaintenanceRecordOther As String) As ActionResult

            If ModelState.IsValid Then
                Dim thisHeavyEqpMaintenanceCheck = (From m In _eqpEntities.HeavyEquipmentMaintenanceChecks
                                                    Where m.HeavyEqpId = model.HeavyEqpId).SingleOrDefault()


                Dim MaintenanceCheckItems = thisHeavyEqpMaintenanceCheck.HeavyEquipmentMaintenanceCheckItems
                Dim HeavyEqpMaintenanceRecordItems = New List(Of HeavyEqpMaintenanceRecordItem)

                Newtonsoft.Json.JsonConvert.PopulateObject(sHeavyEqpMaintenanceRecordItem, HeavyEqpMaintenanceRecordItems)
                If HeavyEqpMaintenanceRecordItems.Count > 0 Then
                    For Each item In HeavyEqpMaintenanceRecordItems
                        If item.HeavyEquipmentMaintenanceCheckItemId = 0 Then
                            If thisHeavyEqpMaintenanceCheck IsNot Nothing Then
                                ''find the name first
                                Dim theName = item.Item
                                Dim found = (From m In MaintenanceCheckItems
                                            Where m.ItemName.Equals(theName, StringComparison.CurrentCultureIgnoreCase)).SingleOrDefault()
                                If found Is Nothing Then

                                    Dim newCheckItem = New HeavyEquipmentMaintenanceCheckItem
                                    newCheckItem.ItemName = item.Item
                                    newCheckItem.LastCheck_Date = Date.Today
                                    newCheckItem.LastCheck_HM = model.HourmeterValue
                                    newCheckItem.UnitValue = "Bulan"
                                    newCheckItem.Value = 6
                                    thisHeavyEqpMaintenanceCheck.HeavyEquipmentMaintenanceCheckItems.Add(newCheckItem)
                                    _eqpEntities.SaveChanges()
                                    item.HeavyEquipmentMaintenanceCheckItemId = newCheckItem.ID
                                Else
                                    item.HeavyEquipmentMaintenanceCheckItemId = found.ID
                                End If
                            End If
                        End If

                    Next
                Else
                    ModelState.AddModelError("General", "Item Pekerjaan harus diisi")
                    Return Json(New With {.stat = 0, .errors = HaloUIHelpers.Helpers.JsonErrorObject.Convert(ModelState)})
                End If





                _eqpEntities.HeavyEqpMaintenanceRecords.AddObject(model)
                If model.Id <> 0 Then
                    _eqpEntities.ObjectStateManager.ChangeObjectState(model, EntityState.Modified)
                End If
                If HeavyEqpMaintenanceRecordItems.Count > 0 Then
                    For Each item In HeavyEqpMaintenanceRecordItems
                        model.HeavyEqpMaintenanceRecordItems.Add(item)


                        Dim HeavyEqpMaintenanceCheckItemId = item.HeavyEquipmentMaintenanceCheckItemId
                        Dim checkitem = (From m In _eqpEntities.HeavyEquipmentMaintenanceCheckItems
                                        Where m.ID = HeavyEqpMaintenanceCheckItemId).SingleOrDefault
                        If checkitem IsNot Nothing Then
                            checkitem.LastCheck_Date = model.MaintenanceDateStart
                            checkitem.LastCheck_HM = model.HourmeterValue
                        End If
                        If item.Id > 0 Then
                            _eqpEntities.ObjectStateManager.ChangeObjectState(item, EntityState.Modified)
                        End If

                    Next
                End If

                Dim HeavyEqpMaintenanceRecordMaterialUseds = New List(Of HeavyEqpMaintenanceRecordMaterialUsed)
                Newtonsoft.Json.JsonConvert.PopulateObject(sHeavyEqpMaintenanceRecordMaterialUsed, HeavyEqpMaintenanceRecordMaterialUseds)
                If HeavyEqpMaintenanceRecordMaterialUseds.Count > 0 Then
                    For Each item In HeavyEqpMaintenanceRecordMaterialUseds
                        model.HeavyEqpMaintenanceRecordMaterialUseds.Add(item)
                        If item.Id > 0 Then
                            _eqpEntities.ObjectStateManager.ChangeObjectState(item, EntityState.Modified)
                        End If
                    Next
                End If
                Dim HeavyEqpMaintenanceRecordOthers = New List(Of HeavyEqpMaintenanceRecordOther)
                Newtonsoft.Json.JsonConvert.PopulateObject(sHeavyEqpMaintenanceRecordOther, HeavyEqpMaintenanceRecordOthers)
                If HeavyEqpMaintenanceRecordOthers.Count > 0 Then
                    For Each item In HeavyEqpMaintenanceRecordOthers
                        model.HeavyEqpMaintenanceRecordOthers.Add(item)
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

        <HttpGet()> _
        Function ProposedMaintenanceList() As ActionResult
            Return View()
        End Function

        Public Function Detail(ByVal Id As Integer) As ActionResult
            Dim model = _eqpEntities.HeavyEqpMaintenanceRecords.Where(Function(m) m.Id = Id).SingleOrDefault()
            If model Is Nothing Then
                Throw New HttpException(404, "NOT FOUND")
            End If

            Return View(model)
        End Function


        <HttpGet()>
        Public Function Edit(ByVal id As Integer) As ActionResult
            Dim model = _eqpEntities.HeavyEqpMaintenanceRecords.Where(Function(m) m.Id = id).SingleOrDefault()
            If model Is Nothing Then
                Throw New HttpException(404, "NOT FOUND")
            End If
            Return View("FormMaintentance", model)
        End Function

        <HttpPost()>
        Public Function GetMaintenanceTemplateItem(ByVal term As String) As JsonResult
            Dim models = (From m In _eqpEntities.MaintenanceTemplateItems
                         Where m.MachineTypeID = 2 AndAlso m.Item.Contains(term)
                         Select New With {.Id = 0, .Item = m.Item, .HeavyEqpMaintenanceCheckItemId = 0,
                                          .HeavyEqpMaintenanceRecordItemId = 0}).ToList()

            Dim models2 = (From m In _eqpEntities.HeavyEqpMaintenanceRecordItems
                          Where m.Item.Contains(term)
                          Select New With {.Id = 0, .Item = m.Item, .HeavyEqpMaintenanceCheckItemId = 0,
                                          .HeavyEqpMaintenanceRecordItemId = 0}).ToList()


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
            Dim itemToService = _eqpEntities.ExecuteStoreQuery(Of HeavyEqpMaintenanceRecordItem)("EXEC [Eqp].[GetScheduledMaintenancePlanForHeavyEqp] @heavyeqpId, @checkdate",
                                                                                              New SqlClient.SqlParameter("@heavyeqpId", id),
                                                                                              New SqlClient.SqlParameter("@Checkdate", enddate))



            Dim rvalue = From m In itemToService
                         Select New With {m.Id, m.Item, m.HeavyEquipmentMaintenanceCheckItemId, m.HeavyEqpMaintenanceRecordId}


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
            Dim models = From m In _eqpEntities.HeavyEqpMaintenanceRecordItems
                         Where m.HeavyEqpMaintenanceRecordId = id
                         Select New With {m.Id, m.Item, m.HeavyEquipmentMaintenanceCheckItemId, m.HeavyEqpMaintenanceRecordId}

            Return Json(New With {.data = models})
        End Function


        <HttpPost()>
        Public Function GetMaterialUsed(ByVal Id As Integer) As JsonResult
            Dim models = From m In _eqpEntities.HeavyEqpMaintenanceRecordMaterialUseds
                         Where m.HeavyEqpMaintenanceRecordId = Id
                         Select New With {m.Id, m.MaterialUsed, m.Quantity, m.UnitQuantity}

            Return Json(New With {.data = models})
        End Function

        <HttpPost()>
        Public Function GetMaintenanceOther(ByVal Id As Integer) As JsonResult
            Dim models = From m In _eqpEntities.HeavyEqpMaintenanceRecordOthers
                         Where m.HeavyEqpMaintenanceRecordId = Id
                         Select New With {m.Id, m.Item, m.Cost, m.Remarks}

            Return Json(New With {.data = models})
        End Function
        <HttpPost()>
        Function ValidateItemToMaintain(ByVal model As HeavyEqpMaintenanceRecordItem) As JsonResult
            If String.IsNullOrWhiteSpace(model.Item) Then
                ModelState.Clear()
                ModelState.AddModelError("ItemToMaintainName", "Item tidak boleh kosong")
                Return Json(New With {.stat = 0, .errors = HaloUIHelpers.Helpers.JsonErrorObject.Convert(ModelState)})
            End If
            Dim rvalue = New With {model.Id, model.Item, model.HeavyEquipmentMaintenanceCheckItemId, model.HeavyEqpMaintenanceRecordId}
            Return Json(New With {.stat = 1, .model = rvalue})
        End Function
        <HttpPost()>
        Function ValidateItemMaterialUsed(ByVal model As HeavyEqpMaintenanceRecordMaterialUsed, ByVal rowIdx As Integer) As JsonResult
            If ModelState.IsValid Then
                Dim rvalue = New With {model.Id, model.MaterialUsed, model.Quantity, model.UnitQuantity, model.HeavyEqpMaintenanceRecordId}
                Return Json(New With {.stat = 1, .model = rvalue, .rowIdx = rowIdx})
            End If
            Return Json(New With {.stat = 0, .errors = HaloUIHelpers.Helpers.JsonErrorObject.Convert(ModelState)})
        End Function
        <HttpPost()>
        Function ValidateItemOther(ByVal model As HeavyEqpMaintenanceRecordOther, ByVal rowIdx As Integer) As JsonResult
            If ModelState.IsValid Then
                Dim rvalue = New With {model.Id, model.Item, model.Cost, model.Remarks, model.HeavyEqpMaintenanceRecordId}
                Return Json(New With {.stat = 1, .model = rvalue, .rowIdx = rowIdx})
            End If
            Return Json(New With {.stat = 0, .errors = HaloUIHelpers.Helpers.JsonErrorObject.Convert(ModelState)})
        End Function


        <HttpPost()>
        Function Delete(ByVal Id As Integer) As JsonResult
            Dim model = _eqpEntities.HeavyEqpMaintenanceRecords.Where(Function(m) m.Id = Id).SingleOrDefault
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
            Dim model = _eqpEntities.HeavyEqpMaintenanceRecordItems.Where(Function(m) m.Id = Id).SingleOrDefault
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
            Dim model = _eqpEntities.HeavyEqpMaintenanceRecordMaterialUseds.Where(Function(m) m.Id = Id).SingleOrDefault
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
        Public Function GetProposedHeavyEqpToMaintainList() As JsonResult
            Dim startDate As Date = Date.Today
            While startDate.DayOfWeek <> DayOfWeek.Sunday
                startDate = startDate.AddDays(-1)
            End While
            Dim enddate As Date = startDate.AddDays(6)

            Dim model = _eqpEntities.ExecuteStoreQuery(Of HeavyEquipmentMaintenanceListView)("EXEC eqp.GetScheduledHeavyEqpMaintenancePlan  @startdate, @enddate, @areaid",
                                                                                              New SqlClient.SqlParameter("@startdate", startDate),
                                                                                              New SqlClient.SqlParameter("@enddate", enddate),
                                                                                              New SqlClient.SqlParameter("@areaid", currentUserProfile.WorkUnitId))

            Dim groupped = From m In model
                           Group m By m.HeavyEqpId, m.Type, m.Code,
                           m.Species, m.Merk Into HeavyEqp = Group


            Return Json(New With {.data = groupped})
        End Function


        Public Sub New()
            _eqpEntities = New EquipmentEntities
            currentUserProfile = ERPBase.ErpUserProfile.GetUserProfile()
        End Sub
    End Class
End Namespace