Namespace Equipment.Areas.Equipment.Controllers
    Public Class TradoActivitiesController
        Inherits System.Web.Mvc.Controller
        Dim ctx As New EquipmentEntities
        '
        ' GET: /Equipment/Trado

        Function AutocompleteLocation(ByVal term As String) As JsonResult
            Dim model = ctx.AutocompleteLocation(term).ToList
            Return Json(model, JsonRequestBehavior.AllowGet)
        End Function

        Function Index() As ActionResult
            Return View()
        End Function

        Function Detail(ByVal id As Integer) As ActionResult
            Dim model = ctx.TradoActivities.Where(Function(m) m.ID = id).FirstOrDefault

            Return View(model)
        End Function

        Function getListActivity(ByVal reportDate As Date) As JsonResult
         

            Dim p = ERPBase.ErpUserProfile.GetUserProfile()

            Dim model = (From act In ctx.TradoActivities
                        Where act.Date.Equals(reportDate)
                         Select New With {act.ID, act.Type, act.Driver, _
                                          act.Merk, act.Code, act.Date, act.Category, act.PoliceNumber, act.IDArea, .TRID = 0}).ToList


            Dim Trado = (From e In ctx.Vehicles
                             Where e.IDArea = p.WorkUnitId AndAlso e.Species.Contains("TRADO")).ToList()

            Dim filteredtd = (From e In Trado
                             Where Not (From m In model Select m.Code).Contains(e.Code)
                             Select New With {.ID = 0, e.Type, .Driver = "", e.Merk, e.Code,
                                              .Date = reportDate, .Category = e.Species, e.PoliceNumber, e.IDArea, .TRID = e.ID}).ToList()
            model.AddRange(filteredtd)
            Dim rvalue = model.Where(Function(m) m.IDArea = p.WorkUnitId)
            Return Json(New With {.data = rvalue})
        End Function

        Function Create(Optional ByVal Id As Integer = 0, Optional ByVal ReportDate As Date = Nothing) As ActionResult
            Dim getDT = (From dt In ctx.Vehicles Where dt.ID = Id Select New With _
                                                                      {dt.Species,
                                                                       dt.Merk, dt.PoliceNumber,
                                                                       dt.Type, dt.Code}).FirstOrDefault()

            If ReportDate = Nothing Then ReportDate = Date.Today

            Dim model As New TradoActivity
            model.ID = 0
            model.IDDriver = 0
            model.Date = ReportDate
            If Not IsNothing(getDT) Then
                model.Merk = getDT.Merk
                model.PoliceNumber = getDT.PoliceNumber
                model.Type = getDT.Type
                model.Category = getDT.Species
                model.Code = getDT.Code
            End If
            Dim p = ERPBase.ErpUserProfile.GetUserProfile()
            model.IDArea = p.WorkUnitId

            Return View("Form", model)
        End Function

        Function Edit(ByVal id As Integer) As ActionResult

            Dim model = ctx.TradoActivities.Where(Function(m) m.ID = id).FirstOrDefault
            Return View("Form", model)
        End Function

        <HttpPost()>
        Function loadActivity(ByVal id As Integer) As ActionResult
            Dim model = ctx.TradoActivities.Where(Function(m) m.ID = id).SingleOrDefault()

            Dim Operations = From m In model.TradoOperations
                             Select New With {m.ID, m.IDActivity, m.LoadType, m.SourceLocation,
                                              .DepartureTime = New Date(m.DepartureTime.Ticks).ToString("HH:mm"),
                            .ArrivalTime = New Date(m.ArrivalTime.Ticks).ToString("HH:mm"), m.Destination, m.Distance, m.BeginKM, _
                                              m.EndKM, m.Operator}

            Dim NonOperations = From m In model.TradoNonOperations
                                Select New With {m.ID, m.IDActivity, m.NonOperationType, m.Reason}

            Dim OilUsages = From m In model.OilAndSparePartUsedTradoes
                            Select New With {m.ID, m.IDActivity, m.OilOrSparePartType, m.Amount, m.Unit}

            Dim FuelConsumed = From m In model.FuelUsedTradoes
                               Select New With {m.ID, m.IDActivity, m.AmountFuel, m.Location, _
                                                .TimeFill = New Date(m.TimeFill.Ticks).ToString("HH:mm")}
            Dim FuelConsumedOut = From m In model.FuelUsedOutTradoes
                               Select New With {m.ID, m.IDActivity, m.Alocation, m.AmountFuelOut, m.Operator}

            Return Json(New With {NonOperations, Operations, OilUsages, FuelConsumed, FuelConsumedOut})
        End Function

        Function SaveItemActivities(ByVal header As TradoActivity, ByVal ItemOperation As String, ByVal ItemNonOperation As String, _
                                    ByVal ItemOil As String, ByVal ItemFuel As String, ByVal ItemFuelOut As String) As ActionResult
            'check if the record already exist
            If header.ID = 0 Then
                Dim existing = (From m In ctx.TradoActivities
                                        Where m.Code.Equals(header.Code) AndAlso m.Date.Equals(header.Date)
                                        Select m).FirstOrDefault
                If existing IsNot Nothing Then
                    ModelState.AddModelError("General", "Data untuk operasional dumptruck ini telah ada di database")
                    Return Json(New With {.stat = 2, .id = existing.ID, .errors = HaloUIHelpers.Helpers.JsonErrorObject.Convert(ModelState)})
                End If
            End If


            If ModelState.IsValid Then
                ctx.TradoActivities.AddObject(header)
                If header.ID > 0 Then
                    ctx.ObjectStateManager.ChangeObjectState(header, EntityState.Modified)
                End If
                'ctx.SaveChanges()


                '
                Dim OpItems As New List(Of TradoOperation)
                Newtonsoft.Json.JsonConvert.PopulateObject(ItemOperation, OpItems)


                Dim NonOpItems As New List(Of TradoNonOperation)

                Newtonsoft.Json.JsonConvert.
                    PopulateObject(ItemNonOperation, _
                                    NonOpItems)

                Dim OilItems As New List(Of OilAndSparePartUsedTrado)
                Newtonsoft.Json.JsonConvert.PopulateObject(ItemOil, OilItems)

                Dim fuelItems As New List(Of FuelUsedTrado)
                Newtonsoft.Json.JsonConvert.PopulateObject(ItemFuel, fuelItems)

                Dim fuelItemsOut As New List(Of FuelUsedOutTrado)
                Newtonsoft.Json.JsonConvert.PopulateObject(ItemFuelOut, fuelItemsOut)
                If OpItems.Count <> 0 Then
                    For Each listOp In OpItems
                        header.TradoOperations.Add(listOp)
                        If listOp.ID > 0 Then
                            ctx.ObjectStateManager.ChangeObjectState(listOp, EntityState.Modified)
                        End If
                    Next
                End If


                If NonOpItems.Count <> 0 Then
                    For Each listNonOp In NonOpItems

                        header.TradoNonOperations.Add(listNonOp)
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

                        header.OilAndSparePartUsedTradoes.Add(listOil)
                        If listOil.ID > 0 Then
                            ctx.ObjectStateManager.ChangeObjectState(listOil, EntityState.Modified)
                        End If

                    Next
                End If

                If fuelItems.Count <> 0 Then
                    For Each listfuel In fuelItems
                        header.FuelUsedTradoes.Add(listfuel)
                        If listfuel.ID > 0 Then
                            ctx.ObjectStateManager.ChangeObjectState(listfuel, EntityState.Modified)
                        End If
                    Next
                End If

                If fuelItemsOut.Count <> 0 Then
                    For Each listfuelout In fuelItemsOut
                        header.FuelUsedOutTradoes.Add(listfuelout)
                        If listfuelout.ID > 0 Then
                            ctx.ObjectStateManager.ChangeObjectState(listfuelout, EntityState.Modified)
                        End If
                    Next
                End If
                ctx.SaveChanges()
                Return Json(New With {.stat = 1})
            End If

            Return Json(New With {.stat = -0, .errors = HaloUIHelpers.Helpers.JsonErrorObject.Convert(ModelState)})

        End Function
        'validate function
        Function ValidateOperation(ByVal item As TradoOperation, ByVal rowIdx As Integer) As ActionResult
            If item.DepartureTime >= item.ArrivalTime Then
                ModelState.AddModelError("Error", "Jam Waktu Berangkat Salah")
            End If
            If ModelState.IsValid Then
                Return Json(New With {.stat = 1, .idx = rowIdx})
            End If
            Return Json(New With {.stat = 0, .errors = HaloUIHelpers.Helpers.JsonErrorObject.Convert(ModelState)}, JsonRequestBehavior.AllowGet)
        End Function

        Function ValidateNonOperation(ByVal item As TradoNonOperation, ByVal rowIdx As Integer) As ActionResult

            If ModelState.IsValid Then
                Return Json(New With {.stat = 1, .idx = rowIdx})
            End If
            Return Json(New With {.stat = 0, .errors = HaloUIHelpers.Helpers.JsonErrorObject.Convert(ModelState)}, JsonRequestBehavior.AllowGet)
        End Function

        Function ValidateFuel(ByVal item As FuelUsedTrado, ByVal rowIdx As Integer) As ActionResult
            If ModelState.IsValid Then
                Return Json(New With {.stat = 1, .idx = rowIdx})
            End If
            Return Json(New With {.stat = 0, .errors = HaloUIHelpers.Helpers.JsonErrorObject.Convert(ModelState)}, JsonRequestBehavior.AllowGet)
        End Function

        Function ValidateFuelOut(ByVal item As FuelUsedOutTrado, ByVal rowIdx As Integer) As ActionResult
            If ModelState.IsValid Then
                Return Json(New With {.stat = 1, .idx = rowIdx})
            End If
            Return Json(New With {.stat = 0, .errors = HaloUIHelpers.Helpers.JsonErrorObject.Convert(ModelState)}, JsonRequestBehavior.AllowGet)
        End Function

        Function ValidateOilAndSparePart(ByVal item As OilAndSparePartUsedTrado, ByVal rowIdx As Integer) As ActionResult
            If ModelState.IsValid Then
                Return Json(New With {.stat = 1, .idx = rowIdx})
            End If

            Return Json(New With {.stat = 0, .errors = HaloUIHelpers.Helpers.JsonErrorObject.Convert(ModelState)}, JsonRequestBehavior.AllowGet)
        End Function
    End Class
End Namespace