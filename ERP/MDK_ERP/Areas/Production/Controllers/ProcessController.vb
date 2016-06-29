Namespace MDK_ERP.Areas.Production.Controllers

    <Authorize()> _
    Public Class ProcessController
        Inherits BaseController

        '
        ' GET: /Production/Pengolahan

        Function Index() As ActionResult
            Return View()
        End Function
        Function saveCrusher(ByVal form As FormCollection, ByVal model As CrusherJournals) As JsonResult
            Dim stat As Integer = 0
            Dim message As String = ""
            Try
                'ctx.AddToProductionJournals(model)
                ctx.SaveChanges()
                stat = 1
                message = "Success"
            Catch ex As Exception
                message = ex.Message
            End Try
            Return Json(New With {.msg = message, .stat = stat})
        End Function

        Function AMP() As ActionResult
            Return View()
        End Function

        Function AmpDelete(ID As Integer) As JsonResult
            Dim stat As Integer = 0
            Dim msg As String = ""
            Try
                Dim data = (From amp In ctx.AMPJournals Where amp.ID = ID Select amp).FirstOrDefault ' Selecting First or default data here
                ctx.DeleteObject(data)
                ctx.SaveChanges()
                stat = 1
                msg = "Success Deleted"
            Catch ex As Exception
                msg = "Error :" + ex.Message
            End Try
            Return Json(New With {.stat = stat, .msg = msg}, JsonRequestBehavior.AllowGet)
        End Function

        Function getListAmp() As JsonResult
            Dim data = ctx.ListDataAMP.ToList()
            Return Json(New With {.data = data}, JsonRequestBehavior.AllowGet)
        End Function
        <HttpGet()> _
        Function IndexAmp() As ActionResult
            ViewData("Shift") = ctx.MstWorkSchedule.ToList()
            ViewData("ListAsphal") = ctx.DropdownAsphal.ToList()
            ViewData("ListMachine") = (From m In ctx.MstMachines Where m.MachineName.Contains("AMP") Select m).ToList()
            ViewData("projectList") = (From p In ctx.ProjectList Select p).ToList()

            Return View()
        End Function

        <HttpPost()> _
        Function IndexAmp(ByVal model As ampModel, ByVal AmountComp As List(Of Double), ByVal IDMaterial As List(Of Integer)) As ActionResult
            Dim arrUnit = Request.Form("IDMeasurementUnit")
            Dim unit = arrUnit.Split(",")

            If model.BeginProd > model.EndProd Then
                ModelState.AddModelError("Error ", "Jam Produksi Salah!")
            End If
            If ModelState.IsValid Then
                Try

                    'save to data inventory first
                    Dim dataInventory As New MaterialInventories With {
                        .IdInventoryStatus = InventoryStatus.Pemakaian, _
                        .IsPlus = False
                    }
                    ctx.MaterialInventories.AddObject(dataInventory)
                    ctx.SaveChanges()

                    'save to AMPJournals
                    Dim ampJournal As New AMPJournals
                    ampJournal.DateUse = model.DateUse
                    ampJournal.IdMachine = model.IdMachine
                    'ampJournal.IDProject = model.IDProject
                    ampJournal.Operator = model.OperatorName
                    ampJournal.IDMaterial = model.IDProduk
                    ampJournal.IdShift = model.IdShift
                    ampJournal.Description = model.Description
                    ampJournal.BeginProd = model.BeginProd
                    ampJournal.EndProd = model.EndProd

                    ctx.AMPJournals.AddObject(ampJournal)
                    ctx.SaveChanges()

                    'save to MaterialUseJournals
                    For counter As Integer = 0 To AmountComp.Count - 1
                        ' untuk validasi material produk wajib diisi
                        'If Amount(counter) = 0 Then
                        '    ModelState.AddModelError("", "Nilai komposisi untuk produksi produk Wajib diisi")
                        '    Exit Try
                        'End If

                        Dim modelUseJournal As New MaterialUseJournal With {
                            .DateUse = model.DateUse.Add(model.BeginProd),
                            .IdMachine = model.IdMachine,
                            .OperatorName = model.OperatorName,
                            .IDMaterial = IDMaterial(counter),
                            .Amount = AmountComp(counter),
                            .IDInventory = dataInventory.ID,
                            .Description = model.Description,
                            .IDMeasurementUnit = unit(counter),
                            .IDAmp = ampJournal.ID
                        }
                        ctx.AddToMaterialUseJournal(modelUseJournal)
                    Next
                    ctx.SaveChanges()

                    Return RedirectToAction("AMP", "Process")
                Catch ex As Exception
                    'Throw New HttpException(404, "Not Found")
                    ModelState.AddModelError("ExError", "Data belum dapat tersimpan. Mohon hubungi Administrator. Error Message: " + ex.Message)
                End Try
            End If

            'if we got this far, so turn back to the view
            ViewData("Shift") = ctx.MstWorkSchedule.ToList()
            ViewData("ListAsphal") = ctx.DropdownAsphal.ToList()
            ViewData("ListMachine") = (From m In ctx.MstMachines Where m.MachineName.Contains("AMP") Select m).ToList()
            ViewData("projectList") = (From p In ctx.ProjectList Select p).ToList()
            Return View(model)
        End Function

        Function EditAmp(ByVal ID As Integer) As ActionResult
            Dim model As AMPJournals
            If IsNothing(TempData("dataEditAmp")) Then
                model = (From amp In ctx.AMPJournals Where amp.ID = ID Select amp).FirstOrDefault()
            Else
                model = TempData("dataEditAmp")
                For Each t In TempData("modelStateEditAmp")
                    ModelState.AddModelError(t.key, t.value)
                Next
            End If
            If model Is Nothing Then
                TempData("errTitle") = "Data Not Found !"
                TempData("exeptionMsg") = "Data Tidak Ditemukan. Mohon kembali dan periksa kesalahan. Apabila terus berlanjut mohon hubungi Administrator"
                Return RedirectToRoute("Default", New With {.Action = "Index", .Controller = "Error"})
            End If

            'ViewData("ListAsphal") = ctx.DropdownAsphal.ToList()
            ViewData("IdMachine") = New SelectList((From m In ctx.MstMachines Where m.MachineName.Contains("AMP") Select m).ToList(), "ID", "MachineName", model.IdMachine)
            ViewData("IDShift") = New SelectList(ctx.MstWorkSchedule.ToList(), "ID", "Shift", model.IdShift)
            ViewData("IDProject") = New SelectList((From p In ctx.ProjectList Select p).ToList(), "ID", "NoProject", model.IDProject)
            ViewData("MaterialName") = ctx.MstMaterials.Where(Function(w) w.ID = model.IDMaterial).Select(Function(s) s.Name).FirstOrDefault()
            'ViewData("ListComposition") = ctx.CompInUseJournal(model.ID).ToList()

            Return View(model)
        End Function

        <HttpPost()> _
        Function EditAmp(ByVal model As AMPJournals, ByVal Amount As List(Of Integer), ByVal IdComp As List(Of Integer)) As ActionResult
            Dim ErrMessage As New Dictionary(Of String, String)
            Try
                Dim arrUnit = Request.Form("IDMeasurementUnit")
                Dim unit As String() = arrUnit.Split(",")
                'If ModelState.IsValid Then
                model.Save()
                Dim IdCompItem As Integer
                For counter As Integer = 0 To Amount.Count - 1
                    IdCompItem = IdComp(counter)
                    Dim data = (From m In ctx.MaterialUseJournal Where m.ID = IdCompItem
                               Select m).FirstOrDefault()
                    With data
                        .Amount = Amount(counter)
                        .Description = model.Description
                        .DateUse = model.DateUse
                        .IdMachine = model.IdMachine
                        .IDMeasurementUnit = unit(counter)
                        .IDProject = model.IDProject
                        .OperatorName = model.Operator
                    End With
                Next
                ctx.SaveChanges()
                Return RedirectToAction("Amp", "Process")
                'End If
            Catch ex As Exception
                ErrMessage.Add("", "Data belum dapat tersimpan. Mohon hubungi Administrator. Error Message: " + ex.Message)
            End Try

            ' if we got this far, return this view
            TempData("dataEditAmp") = model
            TempData("modelStateEditAmp") = ErrMessage
            Return RedirectToAction("EditAmp", "Process", New With {.ID = model.ID})
        End Function

        Function getListComposition(ByVal idProduk As Integer, ByVal MeasurementUnit As String) As JsonResult
            'Dim data = ctx.produkCompositions(idProduk).OrderBy(Function(o) o.IDMaterialComposition).ToList()
            Dim idMeasurementUnit = (From x In ctx.MstMeasurementUnits Where x.Unit.Contains(MeasurementUnit) Select x.ID).FirstOrDefault()

            Dim data = ctx.CompositionProductList(idProduk, idMeasurementUnit).OrderBy(Function(o) o.IDMaterialComposition).ToList()
            Return Json(New With {.data = data}, JsonRequestBehavior.AllowGet)
        End Function

        Function Solar() As ActionResult
            'Dim shift = ctx.MstWorkSchedule.ToList()
            Dim mesin = ctx.MstMachines.ToList()
            'ViewData("shift") = shift
            ViewData("mesin") = mesin
            ViewData("solar") = (From m In ctx.MstMaterials Where m.Code = 6 Select m.ID).FirstOrDefault()
            'ViewData("Unit") = (From s In ctx.MstMeasurementUnits Where s.Unit.Contains("L") Select s).FirstOrDefault
            'ViewData("inventory") = (From i In MaterialInventories Where i.
            Return View()
        End Function
        Function getListSolar(ByVal MonthVal As Byte, ByVal YearVal As Integer) As JsonResult
            Dim model = ctx.ListSolar(MonthVal, YearVal).ToList()
            Return Json(New With {.data = model}, JsonRequestBehavior.AllowGet)
        End Function

        Function solarSave(ByVal model As MaterialUseJournal, ByVal DateUse As String) As JsonResult
            Dim stat As Integer = 0
            'If ModelState.IsValid Then
            Try
                model.DateUse = StrtoDate(DateUse)
                model.Save()
                stat = 1
                ModelState.AddModelError("Success", "Data Berhasil di Simpan")
            Catch ex As Exception
                ModelState.AddModelError("Error", ex.Message)
            End Try
            'End If
            Dim errorlist = ModelState.ToDictionary(Function(k) k.Key,
                                                      Function(k) k.Value.Errors.Select(Function(e) e.ErrorMessage).ToArray()).
                                                      Where(Function(k) k.Value.Count > 0)
            Return Json(New With {.msg = errorlist, .stat = stat})
        End Function

        Function deleteSolar(id As Integer) As JsonResult
            Dim stat As Integer = 0
            Dim msg As String = ""
            Try
                Dim data = (From mu In ctx.MaterialUseJournal Where mu.ID = id Select mu).FirstOrDefault ' Selecting First or default data here
                ctx.DeleteObject(data)
                ctx.SaveChanges()
                stat = 1
                msg = "Success Deleted"
            Catch ex As Exception
                msg = "Error :" + ex.Message
            End Try
            Return Json(New With {.stat = stat, .msg = msg}, JsonRequestBehavior.AllowGet)
        End Function
        'End Function

        Function Asphal() As ActionResult
            ViewData("aspal") = ctx.ListAspal.ToList()
            ViewData("sifht") = ctx.MstWorkSchedule.ToList()
            ViewData("IDaspal") = (From m In ctx.MstMaterials Where m.Code = 5 Select m.ID).FirstOrDefault()
            ViewData("IDMesin") = (From m In ctx.MstMachines Where m.MachineName.Contains("AMP") Select m.ID).FirstOrDefault()
            Return View()
        End Function
        Function asphalSave(ByVal form As FormCollection, ByVal model As MaterialUseJournal, ByVal DateUse As String) As JsonResult
            Dim stat As Integer = 0
            'If ModelState.IsValid Then
            Try
                model.DateUse = StrtoDate(DateUse)
                model.Save()
                stat = 1
                ModelState.AddModelError("Success", "Data Berhasil Disimpan")
            Catch ex As Exception
                ModelState.AddModelError("Error", ex.Message)
            End Try
            'End If
            Dim errorlist = ModelState.ToDictionary(Function(k) k.Key,
                                                    Function(k) k.Value.Errors.Select(Function(e) e.ErrorMessage).ToArray()).
                                                    Where(Function(k) k.Value.Count > 0)
            Return Json(New With {.msg = errorlist, .stat = stat})
        End Function
        Function getListAsphal(ByVal MonthVal As Byte, ByVal YearVal As Integer) As JsonResult
            Dim model = ctx.getListAspal(MonthVal, YearVal).ToList()
            Return Json(New With {.data = model}, JsonRequestBehavior.AllowGet)
        End Function
    End Class
End Namespace