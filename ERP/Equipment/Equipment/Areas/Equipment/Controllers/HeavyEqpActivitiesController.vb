
Namespace Equipment.Areas.Equipment.Controllers
    Public Class HeavyEqpActivitiesController
        Inherits System.Web.Mvc.Controller

        Dim ctx As New EquipmentEntities

        '
        ' GET: /Equipment/HeavyEqpActivities

        Function Index() As ActionResult
            Return View()
        End Function

        Function Detail(ByVal ID As Integer) As ActionResult
            Dim model = ctx.HeavyEqpActivities.Where(Function(m) m.ID = ID).FirstOrDefault
            If model Is Nothing Then
                Throw New HttpException(404, "NOT FOUND")
            End If
            Return View(model)
        End Function

        Function getListActivity(ByVal reportDate As Date) As JsonResult
            Dim p = ERPBase.ErpUserProfile.GetUserProfile()

            Dim model = (From act In ctx.HeavyEqpActivities
                        Where act.Date.Equals(reportDate)
                         Select New With {act.ID, act.Type, act.Operator, _
                                                                           act.Merk, .Code = act.Code.Trim(), act.Date, act.Category, .HqID = 0, act.IDArea,
                                         .hw = act.HeavyEqpOperations.Sum(Function(m) CType(m.EndHM, Decimal?) - CType(m.BeginHM, Decimal?))}).ToList()


            Dim Equipments = (From e In ctx.HeavyEqps
                             Where e.IDArea = p.WorkUnitId).ToList()

            Dim nullableDecimal As Decimal? = Nothing

            Dim filteredEqp = (From e In Equipments
                             Where Not (From m In model Select m.Code).Contains(e.Code)
                             Select New With {.ID = 0, e.Type, .Operator = "", e.Merk, e.Code, .Date = reportDate,
                                              .Category = e.Species, .HqID = e.ID, .IDArea = p.WorkUnitId, .hw = nullableDecimal}).ToList()



            model.AddRange(filteredEqp)
            Dim rvalue = model.Where(Function(m) m.IDArea = p.WorkUnitId)

            Return Json(New With {.data = rvalue})
        End Function

        Function DeleteFuel(ByVal id As Integer) As JsonResult

            Try
                Dim data = (From pl In ctx.FuelUsedHeavyEqps Where pl.ID = id).FirstOrDefault()
                If data IsNot Nothing Then ctx.DeleteObject(data)
                ctx.SaveChanges()
                Return Json(New With {.stat = 1})
            Catch ex As Exception
                Return Json(New With {.stat = 0, .errors = HaloUIHelpers.Helpers.JsonErrorObject.Convert(ex)})
            End Try

        End Function

        Function DeleteOil(ByVal id As Integer) As JsonResult

            Try
                Dim data = (From pl In ctx.OilUsedHeavyEqps Where pl.ID = id).FirstOrDefault()
                If data IsNot Nothing Then ctx.DeleteObject(data)
                ctx.SaveChanges()
                Return Json(New With {.stat = 1})
            Catch ex As Exception
                Return Json(New With {.stat = 0, .errors = HaloUIHelpers.Helpers.JsonErrorObject.Convert(ex)})
            End Try

        End Function

        Function DeleteNonOperation(ByVal id As Integer) As JsonResult

            Try
                Dim data = (From pl In ctx.HeavyEqpNonOperations Where pl.ID = id).FirstOrDefault()
                If data IsNot Nothing Then ctx.DeleteObject(data)
                ctx.SaveChanges()
                Return Json(New With {.stat = 1})
            Catch ex As Exception
                Return Json(New With {.stat = 0, .errors = HaloUIHelpers.Helpers.JsonErrorObject.Convert(ex)})
            End Try

        End Function

        Function Create(Optional ByVal Id As Integer = 0, Optional ByVal reportDate As Date = Nothing) As ActionResult
            Dim Hq As HeavyEqp = Nothing

            If reportDate = Nothing Then reportDate = Date.Today

            If Id <> 0 Then
                Hq = ctx.HeavyEqps.Where(Function(m) m.ID = Id).SingleOrDefault

            End If

            Dim model As New HeavyEqpActivity
            model.IDOp = 0
            model.ID = 0
            model.Date = reportDate
            model.BeginBreakI = Nothing : model.EndBreakI = Nothing
            model.BeginBreakII = Nothing : model.EndBreakII = Nothing
            model.BeginFirstOverTime1 = Nothing : model.EndFirstOverTime1 = Nothing
            model.BeginFirstOverTime2 = Nothing : model.EndFirstOvertime2 = Nothing
            model.BeginNormalI = Nothing : model.EndNormalI = Nothing
            model.BeginNormalII = Nothing : model.EndNormalII = Nothing
            model.BeginSecondOverTime = Nothing : model.EndSecondOverTime = Nothing
            If Not Hq Is Nothing Then
                model.Code = Hq.Code
                model.IDArea = Hq.IDArea
                model.IDOp = If(Hq.IDOpr.HasValue, Hq.IDOpr.Value, 0)
                model.Operator = IIf(String.IsNullOrEmpty(Hq.OprName), "", Hq.OprName)
                model.Type = Hq.Type
                model.Merk = Hq.Merk
                model.Category = Hq.Species

            End If



            Return View("Form", model)
        End Function

        <HttpGet()> _
        Function Edit(ByVal id As Integer) As ActionResult

            Dim model = ctx.HeavyEqpActivities.Where(Function(m) m.ID = id).SingleOrDefault()

            If model Is Nothing Then
                Throw New HttpException(404, "NOT FOUND")
            End If
            Return View("Form", model)
        End Function

        <HttpPost()>
        Function loadActivity(ByVal id As Integer) As ActionResult
            Dim model = ctx.HeavyEqpActivities.Where(Function(m) m.ID = id).SingleOrDefault()

            Dim Operations = From m In model.HeavyEqpOperations
                             Select New With {m.ID, m.IDActivity, m.BeginHM, m.EndHM, m.LocationOperation,
                             m.OperationType, m.PacketOperation, m.VolumeOperation}

            Dim NonOperations = From m In model.HeavyEqpNonOperations
                                Select New With {m.ID, m.IDActivity, .Begin = New Date(m.Begin.Ticks).ToString("HH:mm"),
                                                 .End = New Date(m.End.Ticks).ToString("HH:mm"), m.NonOperationType, m.Reason}

            Dim OilUsages = From m In model.OilUsedHeavyEqps
                            Select New With {m.ID, m.IDActivity, m.OilType, m.Amount}

            Dim FuelConsumed = From m In model.FuelUsedHeavyEqps
                               Select New With {m.ID, m.IDActivity, m.AmountFuel}


            Return Json(New With {NonOperations, Operations, OilUsages, FuelConsumed})
        End Function

        Function getEqpAutoComplete(ByVal term As String) As JsonResult
            Dim p = ERPBase.ErpUserProfile.GetUserProfile()

            Dim model = (From eqp In ctx.HeavyEqps Where (eqp.Code.Contains(term) Or
                         eqp.Merk.Contains(term) Or
                         eqp.Type.Contains(term) Or
                         eqp.Species.Contains(term)) And eqp.IDArea = p.WorkUnitId
                                              Select New With {eqp.Merk, eqp.Type, eqp.Code, eqp.Species, eqp.IDArea,
                                                               eqp.IDOpr, eqp.OprName}).ToList

            Dim arrHEqId = model.Select(Function(m) m.Code)


            Return Json(model, JsonRequestBehavior.AllowGet)
        End Function

        Function getOilType(ByVal term As String) As JsonResult
            Dim model = (From eqp In ctx.AllOils Where eqp.OilName.Contains(term) Select New With {eqp.OilName}).ToList
            Return Json(model, JsonRequestBehavior.AllowGet)
        End Function

        Function DeleteOperation(ByVal id As Integer) As JsonResult

            Try
                Dim data = (From pl In ctx.HeavyEqpOperations Where pl.ID = id).FirstOrDefault()
                If data IsNot Nothing Then ctx.DeleteObject(data)
                ctx.SaveChanges()
                Return Json(New With {.stat = 1}, JsonRequestBehavior.AllowGet)
            Catch ex As Exception
                Return Json(New With {.stat = 0, .errors = HaloUIHelpers.Helpers.JsonErrorObject.Convert(ex)}, JsonRequestBehavior.AllowGet)
            End Try

        End Function

        Function OperatorType(ByVal Query As String) As JsonResult
            Dim p = ERPBase.ErpUserProfile.GetUserProfile()


            Dim a = ctx.OperatorType(p.WorkUnitId, Query).ToList

            Return Json(a, JsonRequestBehavior.AllowGet)
        End Function

        Function SaveItemActivities(ByVal header As HeavyEqpActivity, ByVal ItemOperation As String, ByVal ItemNonOperation As String, _
                                    ByVal ItemOil As String, ByVal ItemFuel As String) As ActionResult

            'check if the record already exist
            If header.ID = 0 Then
                Dim existing = (From m In ctx.HeavyEqpActivities
                                        Where m.Code.Equals(header.Code) AndAlso m.Date.Equals(header.Date)
                                        Select m).FirstOrDefault
                If existing IsNot Nothing Then
                    ModelState.AddModelError("General", "Data untuk alat berat ini telah ada di database")
                    Return Json(New With {.stat = 2, .id = existing.ID, .errors = HaloUIHelpers.Helpers.JsonErrorObject.Convert(ModelState)})
                End If
            End If


            If ModelState.IsValid Then
                ctx.HeavyEqpActivities.AddObject(header)
                If header.ID > 0 Then
                    ctx.ObjectStateManager.ChangeObjectState(header, EntityState.Modified)
                End If

                If header.EndFirstOvertime2.HasValue AndAlso header.EndFirstOvertime2.Value = New TimeSpan(24, 0, 0) Then
                    header.EndFirstOvertime2 = New TimeSpan(0, 0, 0)
                End If

                '
                Dim OpItems As New List(Of HeavyEqpOperation)
                Newtonsoft.Json.JsonConvert.PopulateObject(ItemOperation, OpItems)


                Dim NonOpItems As New List(Of HeavyEqpNonOperation)

                Newtonsoft.Json.JsonConvert.
                    PopulateObject(ItemNonOperation, _
                                    NonOpItems)

                Dim OilItems As New List(Of OilUsedHeavyEqp)
                Newtonsoft.Json.JsonConvert.PopulateObject(ItemOil, OilItems)

                Dim fuelItems As New List(Of FuelUsedHeavyEqp)
                Newtonsoft.Json.JsonConvert.PopulateObject(ItemFuel, fuelItems)
                If OpItems.Count <> 0 Then
                    For Each listOp In OpItems

                        header.HeavyEqpOperations.Add(listOp)
                        If listOp.ID > 0 Then
                            ctx.ObjectStateManager.ChangeObjectState(listOp, EntityState.Modified)
                        End If
                    Next
                End If


                If NonOpItems.Count <> 0 Then
                    For Each listNonOp In NonOpItems

                        header.HeavyEqpNonOperations.Add(listNonOp)
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

                        header.OilUsedHeavyEqps.Add(listOil)
                        If listOil.ID > 0 Then
                            ctx.ObjectStateManager.ChangeObjectState(listOil, EntityState.Modified)
                        End If

                    Next
                End If

                If fuelItems.Count <> 0 Then
                    For Each listfuel In fuelItems

                        header.FuelUsedHeavyEqps.Add(listfuel)
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
        Function ValidateOperation(ByVal item As HeavyEqpOperation, ByVal rowIdx As Integer) As ActionResult
            If ModelState.IsValid Then
                If item.BeginHM > item.EndHM Then
                    ModelState.AddModelError("Error", "Hourmeter mulai tidak boleh lebih besar dari Hourmeter Akhir")
                    Return Json(New With {.stat = 0, .errors = HaloUIHelpers.Helpers.JsonErrorObject.Convert(ModelState)}, JsonRequestBehavior.AllowGet)
                End If
                Return Json(New With {.stat = 1, .idx = rowIdx})
            End If
            Return Json(New With {.stat = 0, .errors = HaloUIHelpers.Helpers.JsonErrorObject.Convert(ModelState)}, JsonRequestBehavior.AllowGet)
        End Function

        Function ValidateNonOperation(ByVal item As HeavyEqpNonOperation, ByVal rowIdx As Integer) As ActionResult


            If ModelState.IsValid Then
                If item.Begin >= item.End Then
                    ModelState.AddModelError("Error", "Jam Awal tidak boleh melewati Jam Akhir")
                    Return Json(New With {.stat = 0, .errors = HaloUIHelpers.Helpers.JsonErrorObject.Convert(ModelState)}, JsonRequestBehavior.AllowGet)
                End If

                Return Json(New With {.stat = 1, .idx = rowIdx})
            End If
            Return Json(New With {.stat = 0, .errors = HaloUIHelpers.Helpers.JsonErrorObject.Convert(ModelState)}, JsonRequestBehavior.AllowGet)
        End Function



        Function ValidateOil(ByVal item As OilUsedHeavyEqp, ByVal rowIdx As Integer) As ActionResult
            If ModelState.IsValid Then
                Return Json(New With {.stat = 1, .idx = rowIdx})
            End If
            Return Json(New With {.stat = 0, .errors = HaloUIHelpers.Helpers.JsonErrorObject.Convert(ModelState)}, JsonRequestBehavior.AllowGet)
        End Function

        Function ValidateFuel(ByVal item As FuelUsedHeavyEqp, ByVal rowIdx As Integer) As ActionResult
            If ModelState.IsValid Then
                Return Json(New With {.stat = 1, .idx = rowIdx})
            End If
            Return Json(New With {.stat = 0, .errors = HaloUIHelpers.Helpers.JsonErrorObject.Convert(ModelState)}, JsonRequestBehavior.AllowGet)
        End Function




        Function Delete(ByVal id As Integer) As JsonResult
            Dim stat As Integer = 0
            Dim msg As String = ""
            Dim msgDesc As String = ""
            Try
                Dim data = (From pl In ctx.HeavyEqpActivities Where pl.ID = id).FirstOrDefault()
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


        Function AutocompleteLocation(ByVal term As String) As JsonResult
            Dim model = ctx.AutocompleteLocation(term).ToList
            Return Json(model, JsonRequestBehavior.AllowGet)
        End Function
        Function AutocompleteNonOperationType(ByVal term As String) As JsonResult
            Dim model = ctx.NonOpType(term).ToList
            Return Json(model, JsonRequestBehavior.AllowGet)
        End Function
    End Class

End Namespace