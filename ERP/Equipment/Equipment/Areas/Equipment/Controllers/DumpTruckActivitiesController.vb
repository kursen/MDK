Namespace Equipment.Areas.Equipment.Controllers
    Public Class DumpTruckActivitiesController
        Inherits System.Web.Mvc.Controller

        '
        ' GET: /Equipment/DumpTruckActivities
        Dim ctx As New EquipmentEntities
        Function Index() As ActionResult
            Return View()
        End Function
        'Code Dump Truck Type
        Function AutocompleteVehicle(ByVal term As String) As JsonResult
            Dim p = ERPBase.ErpUserProfile.GetUserProfile()

            Dim model = (From vhc In ctx.Vehicles Where (vhc.Code.Contains(term) OrElse
                         vhc.Merk.Contains(term) OrElse
                         vhc.Type.Contains(term) OrElse
                         vhc.PoliceNumber.Contains(term) OrElse
                         vhc.Species.Contains(term)) And vhc.IDArea = p.WorkUnitId
                                              Select New With {vhc.Merk, vhc.Type, vhc.Code, vhc.Species, vhc.IDArea, vhc.PoliceNumber,
                                                               vhc.IDDriver, vhc.DriverName}).ToList




            Return Json(model, JsonRequestBehavior.AllowGet)
        End Function
        Function AutocompleteLocation(ByVal term As String) As JsonResult
            Dim model = ctx.AutocompleteLocation(term).ToList
            Return Json(model, JsonRequestBehavior.AllowGet)
        End Function
        Function AutocompleteLoadType(ByVal term As String) As JsonResult
            Dim model = ctx.AutocompleteLoadType(term).ToList
            Return Json(model, JsonRequestBehavior.AllowGet)
        End Function
        Function AutocompleteDrivers(ByVal term As String) As JsonResult
            Dim p = ERPBase.ErpUserProfile.GetUserProfile()
            Dim model = ctx.ExecuteStoreQuery(Of ERPBase.OptionItem)("SELECT cast( ID as varchar(10)) AS Value, Fullname AS Text FROM Eqp.GetDrivers(@officeId, @term);",
                                                                 New SqlClient.SqlParameter("officeId", p.WorkUnitId),
                                                                 New SqlClient.SqlParameter("@term", term))

            Return Json(model, JsonRequestBehavior.AllowGet)
        End Function

        Function AutocompleteOilType(ByVal term As String) As JsonResult
            Dim model = (From eqp In ctx.AllOils Where eqp.OilName.Contains(term) Select New With {eqp.OilName}).ToList
            Return Json(model, JsonRequestBehavior.AllowGet)
        End Function
        'General non OpType
        Function AutocompleteNonOperationType(ByVal term As String) As JsonResult
            Dim model = ctx.NonOpType(term).ToList
            Return Json(model, JsonRequestBehavior.AllowGet)
        End Function

        Function Detail(ByVal id As Integer) As ActionResult
            Dim model = ctx.DumpTruckActivities.Where(Function(m) m.ID = id).FirstOrDefault
        
            Return View(model)
        End Function

        <HttpPost()>
        Function getListActivity(ByVal reportDate As Date) As JsonResult
          
            Dim p = ERPBase.ErpUserProfile.GetUserProfile()

            Dim model = (From act In ctx.DumpTruckActivities
                        Where act.Date.Equals(reportDate)
                         Select New With {act.ID, act.Type, act.Operator, _
                                          act.Merk, act.Code, act.Date, act.Category, act.PoliceNumber, act.IDArea, .DTID = 0}).ToList

            Dim DumpTruck = (From e In ctx.Vehicles
                             Where e.IDArea = p.WorkUnitId AndAlso Not e.Species.Contains("TRADO")).ToList()

            Dim filtereddt = (From e In DumpTruck
                             Where Not (From m In model Select m.Code).Contains(e.Code)
                             Select New With {.ID = 0, e.Type, .Operator = "", e.Merk,
                                              e.Code, .Date = reportDate, .Category = e.Species,
                                              e.PoliceNumber, e.IDArea, .DTID = e.ID}).ToList()


            model.AddRange(filtereddt)
            Dim rvalue = model.Where(Function(m) m.IDArea = p.WorkUnitId)
            Return Json(New With {.data = rvalue})
        End Function
		

     
        'End
		
		
		' save all activities of dump truck
        Function SaveItemActivities(ByVal header As DumpTruckActivity, ByVal ItemOperation As String, ByVal ItemNonOperation As String, _
                                    ByVal ItemOil As String, ByVal ItemFuel As String) As ActionResult


            'check if the record already exist
            If header.ID = 0 Then
                Dim existing = (From m In ctx.DumpTruckActivities
                                        Where m.Code.Equals(header.Code) AndAlso m.Date.Equals(header.Date)
                                        Select m).FirstOrDefault
                If existing IsNot Nothing Then
                    ModelState.AddModelError("General", "Data untuk operasional dumptruck ini telah ada di database")
                    Return Json(New With {.stat = 2, .id = existing.ID, .errors = HaloUIHelpers.Helpers.JsonErrorObject.Convert(ModelState)})
                End If
            End If


            If ModelState.IsValid Then
                ctx.DumpTruckActivities.AddObject(header)
                If header.ID > 0 Then
                    ctx.ObjectStateManager.ChangeObjectState(header, EntityState.Modified)
                End If
                'ctx.SaveChanges()
                If header.EndFirstOvertime2.HasValue AndAlso header.EndFirstOvertime2.Value = New TimeSpan(24, 0, 0) Then
                    header.EndFirstOvertime2 = New TimeSpan(0, 0, 0)
                End If

                '
                Dim OpItems As New List(Of DumpTruckOperation)
                Newtonsoft.Json.JsonConvert.PopulateObject(ItemOperation, OpItems)


                Dim NonOpItems As New List(Of DumpTruckNonOperation)

                Newtonsoft.Json.JsonConvert.
                    PopulateObject(ItemNonOperation, _
                                    NonOpItems)

                Dim OilItems As New List(Of OilUsedDumpTruck)
                Newtonsoft.Json.JsonConvert.PopulateObject(ItemOil, OilItems)

                Dim fuelItems As New List(Of FuelUsedDumpTruck)
                Newtonsoft.Json.JsonConvert.PopulateObject(ItemFuel, fuelItems)
                If OpItems.Count <> 0 Then
                    For Each listOp In OpItems

                        header.DumpTruckOperations.Add(listOp)
                        If listOp.ID > 0 Then
                            ctx.ObjectStateManager.ChangeObjectState(listOp, EntityState.Modified)
                        End If
                    Next
                End If


                If NonOpItems.Count <> 0 Then
                    For Each listNonOp In NonOpItems

                        header.DumpTruckNonOperations.Add(listNonOp)
                        If listNonOp.ID > 0 Then
                            ctx.ObjectStateManager.ChangeObjectState(listNonOp, EntityState.Modified)
                        End If

                    Next
                End If

                If OpItems.Count = 0 AndAlso NonOpItems.Count = 0 Then
                    ModelState.AddModelError("General", "Data Non Operasional harus dibuat bila Data Operasional tidak dilaporkan")
                    Return Json(New With {.stat = 0, .errors = HaloUIHelpers.Helpers.JsonErrorObject.Convert(ModelState)})
                End If

                If OilItems.Count <> 0 Then
                    For Each listOil In OilItems

                        header.OilUsedDumpTrucks.Add(listOil)
                        If listOil.ID > 0 Then
                            ctx.ObjectStateManager.ChangeObjectState(listOil, EntityState.Modified)
                        End If

                    Next
                End If

                If fuelItems.Count <> 0 Then
                    For Each listfuel In fuelItems
                        header.FuelUsedDumpTrucks.Add(listfuel)
                        If listfuel.ID > 0 Then
                            ctx.ObjectStateManager.ChangeObjectState(listfuel, EntityState.Modified)
                        End If
                    Next
                End If
                If header.EndFirstOvertime2.Equals(New TimeSpan(24, 0, 0)) Then
                    header.EndFirstOvertime2 = New TimeSpan(0, 0, 0)
                End If
                ctx.SaveChanges()
                Return Json(New With {.stat = 1})
            End If

            Return Json(New With {.stat = -0, .errors = HaloUIHelpers.Helpers.JsonErrorObject.Convert(ModelState)})
       
        End Function
		
        Function ValidateOperation(ByVal item As DumpTruckOperation, ByVal rowIdx As Integer) As ActionResult
            If item.DepartureTime >= item.ArrivalTime Then
                ModelState.AddModelError("Error", "Jam Waktu Berangkat Salah")
            End If
            If ModelState.IsValid Then
                Return Json(New With {.stat = 1, .idx = rowIdx})
            End If
            Return Json(New With {.stat = 0, .errors = HaloUIHelpers.Helpers.JsonErrorObject.Convert(ModelState)}, JsonRequestBehavior.AllowGet)
        End Function
        Function ValidateNonOperation(ByVal item As DumpTruckNonOperation, ByVal rowIdx As Integer) As ActionResult

            If ModelState.IsValid Then
                If item.Begin >= item.End Then
                    ModelState.AddModelError("Error", "Jam Awal tidak boleh melewati Jam Akhir")
                    Return Json(New With {.stat = 0, .errors = HaloUIHelpers.Helpers.JsonErrorObject.Convert(ModelState)}, JsonRequestBehavior.AllowGet)
                End If

                Return Json(New With {.stat = 1, .idx = rowIdx})
            End If
            Return Json(New With {.stat = 0, .errors = HaloUIHelpers.Helpers.JsonErrorObject.Convert(ModelState)}, JsonRequestBehavior.AllowGet)
        End Function



        Function ValidateOil(ByVal item As OilUsedDumpTruck, ByVal rowIdx As Integer) As ActionResult
            If ModelState.IsValid Then
                Return Json(New With {.stat = 1, .idx = rowIdx})
            End If
            Return Json(New With {.stat = 0, .errors = HaloUIHelpers.Helpers.JsonErrorObject.Convert(ModelState)}, JsonRequestBehavior.AllowGet)
        End Function
        Function ValidateFuel(ByVal item As FuelUsedDumpTruck, ByVal rowIdx As Integer) As ActionResult
            If ModelState.IsValid Then
                Return Json(New With {.stat = 1, .idx = rowIdx})
            End If
            Return Json(New With {.stat = 0, .errors = HaloUIHelpers.Helpers.JsonErrorObject.Convert(ModelState)}, JsonRequestBehavior.AllowGet)
        End Function
     

   
        Function getOperations(ByVal id As Integer) As JsonResult
            Dim model = (From op In ctx.DumpTruckOperations Where op.IDActivity = id
                         Select New With {.arrivaltime = op.ArrivalTime.Hours & _
                             ":" & op.ArrivalTime.Minutes, .departuretime = op.DepartureTime.Hours _
                                         & ":" & op.DepartureTime.Minutes, op.Destination, op.Distance,
                                         op.ID, op.SourceLocation, op.LoadType
                                         }).ToList
            Return Json(model, JsonRequestBehavior.AllowGet)
        End Function

        Function getNonOperations(ByVal id As Integer) As JsonResult
            Dim model = (From nonop In ctx.DumpTruckNonOperations Where nonop.IDActivity = id
                         Select New With {.Begin = nonop.Begin.Hours & ":" & _
                             nonop.Begin.Minutes, .End = nonop.End.Hours & ":" _
                                         & nonop.End.Minutes, nonop.ID, _
                                          nonop.NonOperationType, nonop.Reason}).ToList
            Return (Json(model, JsonRequestBehavior.AllowGet))
        End Function

        Function getFuels(ByVal id As Integer) As JsonResult
            Dim model = (From f In ctx.FuelUsedDumpTrucks Where f.IDActivity = id
                         Select New With {f.ID, f.AmountFuel}).ToList
            Return Json(model, JsonRequestBehavior.AllowGet)
        End Function

        Function getOils(ByVal id As Integer) As JsonResult
            Dim model = (From oil In ctx.OilUsedDumpTrucks Where oil.IDActivity = id
                         Select New With {oil.ID, oil.OilType, _
                                         oil.Amount}).ToList
            Return Json(model, JsonRequestBehavior.AllowGet)
        End Function


        Function Edit(ByVal id As Integer) As ActionResult

            Dim model = ctx.DumpTruckActivities.Where(Function(m) m.ID = id).FirstOrDefault
            Return View("Form", model)
        End Function

        Function Create(Optional ByVal Id As Integer = 0, Optional ByVal ReportDate As Date = Nothing) As ActionResult
            Dim getDT As Vehicle = Nothing
            If Id <> 0 Then
                getDT = ctx.Vehicles.Where(Function(m) m.ID = Id).SingleOrDefault
            End If
            If ReportDate = Nothing Then ReportDate = Date.Today

            Dim p = ERPBase.ErpUserProfile.GetUserProfile()
            Dim model As New DumpTruckActivity
            model.ID = 0
            model.IDOp = 0
            model.IDArea = p.WorkUnitId
            model.Date = ReportDate
            model.BeginBreakI = Nothing : model.EndBreakI = Nothing
            model.BeginBreakII = Nothing : model.EndBreakII = Nothing
            model.BeginFirstOverTime1 = Nothing : model.EndFirstOverTime1 = Nothing
            model.BeginFirstOverTime2 = Nothing : model.EndFirstOvertime2 = Nothing
            model.BeginNormalI = Nothing : model.EndNormalI = Nothing
            model.BeginNormalII = Nothing : model.EndNormalII = Nothing
            model.BeginSecondOverTime = Nothing : model.EndSecondOverTime = Nothing
            If Not IsNothing(getDT) Then
                model.Merk = getDT.Merk
                model.PoliceNumber = getDT.PoliceNumber
                model.Type = getDT.Type
                model.Category = getDT.Species
                model.Code = getDT.Code
            End If
            Return View("Form", model)
        End Function

        <HttpPost()>
        Function loadActivity(ByVal id As Integer) As ActionResult

            Dim model = ctx.DumpTruckActivities.Where(Function(m) m.ID = id).SingleOrDefault()

            Dim Operations = From m In model.DumpTruckOperations
                             Select New With {m.ID, m.IDActivity, m.LoadType, m.SourceLocation,
                                              .DepartureTime = New Date(m.DepartureTime.Ticks).ToString("HH:mm"),
                            .ArrivalTime = New Date(m.ArrivalTime.Ticks).ToString("HH:mm"), m.Destination, m.Distance, m.ReceiverName}

            Dim NonOperations = From m In model.DumpTruckNonOperations
                                Select New With {m.ID, m.IDActivity, .Begin = New Date(m.Begin.Ticks).ToString("HH:mm"),
                                                 .End = New Date(m.End.Ticks).ToString("HH:mm"), m.NonOperationType, m.Reason}

            Dim OilUsages = From m In model.OilUsedDumpTrucks
                            Select New With {m.ID, m.IDActivity, m.OilType, m.Amount}

            Dim FuelConsumed = From m In model.FuelUsedDumpTrucks
                               Select New With {m.ID, m.IDActivity, m.AmountFuel}


            Return Json(New With {NonOperations, Operations, OilUsages, FuelConsumed})
        End Function
        Function DeleteFuel(ByVal id As Integer) As JsonResult
            Dim stat As Integer = 0
            Dim msg As String = ""
            Dim msgDesc As String = ""
            Try
                Dim data = (From pl In ctx.FuelUsedDumpTrucks Where pl.ID = id).FirstOrDefault()
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

        Function DeleteOil(ByVal id As Integer) As JsonResult
            Dim stat As Integer = 0
            Dim msg As String = ""
            Dim msgDesc As String = ""
            Try
                Dim data = (From pl In ctx.OilUsedDumpTrucks Where pl.ID = id).FirstOrDefault()
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

        Function DeleteNonOperation(ByVal id As Integer) As JsonResult
            Dim stat As Integer = 0
            Dim msg As String = ""
            Dim msgDesc As String = ""
            Try
                Dim data = (From pl In ctx.DumpTruckNonOperations Where pl.ID = id).FirstOrDefault()
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

        Function DeleteOperation(ByVal id As Integer) As JsonResult
            Dim stat As Integer = 0
            Dim msg As String = ""
            Dim msgDesc As String = ""
            Try
                Dim data = (From pl In ctx.DumpTruckOperations Where pl.ID = id).FirstOrDefault()
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

        Function Delete(ByVal id As Integer) As JsonResult
            Dim stat As Integer = 0
            Dim msg As String = ""
            Dim msgDesc As String = ""
            Try
                Dim data = (From pl In ctx.DumpTruckActivities Where pl.ID = id).FirstOrDefault()
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