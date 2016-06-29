Namespace MDK_ERP.Areas.Production.Controllers

    <Authorize()> _
    Public Class DataMasterController
        Inherits BaseController

        Function Index() As ActionResult
            Return View()
        End Function

        Function getMeasurementUnit(ByVal IdMaterial As Integer) As JsonResult
            Dim model = (From s In ctx.MstMeasurementUnits
                            Join m In ctx.MstMaterials On s.ID Equals m.IdMeasurementUnit
                         Where m.ID = IdMaterial
                         Select s.ID, s.Symbol).FirstOrDefault()
            Return Json(New With {.data = model}, JsonRequestBehavior.AllowGet)
        End Function

#Region "Inventory Statuses"
        Function InventoryStatus() As ActionResult
            Return View()
        End Function

        Function getInventoryData() As JsonResult
            Dim model = (From s In ctx.MstInventoryStatuses Select s.ID, _
                                                            s.StatusName).ToList()
            Return Json(New With {.data = model}, JsonRequestBehavior.AllowGet)
        End Function

        Function IS_Save(ByVal model As MstInventoryStatuses) As JsonResult
            Dim stat As Integer = 0
            Dim message As String = ""
            Try
                Dim data = (From m In ctx.MstInventoryStatuses Where m.ID = model.ID).FirstOrDefault()
                If Not IsNothing(data) Then
                    data.StatusName = model.StatusName
                Else
                    ctx.MstInventoryStatuses.AddObject(model)
                End If
                ctx.SaveChanges()
                stat = 1
                message = "Success"
            Catch ex As Exception
                message = ex.Message
            End Try
            Dim errorlist = ModelState.ToDictionary(Function(k) k.Key,
                                                      Function(k) k.Value.Errors.Select(Function(e) e.ErrorMessage).ToArray()).
                                                      Where(Function(k) k.Value.Count > 0)
            Return Json(New With {.msg = errorlist, .stat = stat})
        End Function

        Function IS_Delete(ByVal ID As Integer) As JsonResult
            Dim stat As Integer = 0
            Dim msg As String = ""
            Dim msgDesc As String = ""
            Try
                Dim data = (From s In ctx.MstInventoryStatuses Where s.ID = ID Select s).FirstOrDefault()
                ctx.DeleteObject(data)
                ctx.SaveChanges()
                stat = 1
                msg = "Success Deleted"
            Catch ex As Exception
                msg = "Error :" + ex.Message
            End Try
            Return Json(New With {.stat = stat, .msg = msg, .msgDesc = msgDesc})
        End Function
#End Region

#Region "Material"

        Function Material() As ActionResult
            ViewData("listRatio") = ctx.MstMeasurementUnits.ToList()
            Return View()
        End Function

        Function GetListMaterial() As JsonResult
            Dim model = ctx.ListMaterial.ToList()
            Return (Json(New With {.data = model}, JsonRequestBehavior.AllowGet))
        End Function

        Function Material_(Optional ByVal ID As Integer = 0) As ActionResult
            Dim model = (From m In ctx.MstMaterials Where m.ID = ID Select m).FirstOrDefault()

            ViewData("materials") = ctx.MstMaterials.ToList()
            ViewData("MaterialTypes") = New SelectList(ctx.MstMaterialTypes, "ID", "Type")
            ViewData("IDMachineTypes") = ctx.MstMachineTypes.ToList()
            ViewData("unit") = ctx.MstMeasurementUnits.ToList()
            Return View(model)
        End Function

        <HttpPost()> _
        Function Material_(ByVal model As MstMaterials) As ActionResult
            If ModelState.IsValid Then
                Try
                    model.Save()
                    Return RedirectToAction("Material", "Datamaster")
                Catch ex As Exception
                    ModelState.AddModelError("", ex.Message)
                End Try
            End If

            ViewData("materials") = ctx.MstMaterials.ToList()
            ViewData("MaterialTypes") = New SelectList(ctx.MstMaterialTypes, "ID", "Type")
            ViewData("IDMachineTypes") = ctx.MstMachineTypes.ToList()
            ViewData("unit") = ctx.MstMeasurementUnits.ToList()
            Return View(model)
        End Function

       

        Function DeleteMaterial(ByVal id As Integer) As JsonResult
            Dim stat As Integer = 0
            Dim msg As String = ""
            Try
                Dim dataMaterial = (From mm In ctx.MstMaterials Where mm.ID = id).FirstOrDefault()
                If dataMaterial IsNot Nothing Then ctx.DeleteObject(dataMaterial)
                ctx.SaveChanges()
                stat = 1
                msg = "Success Deleted"
            Catch ex As Exception
                msg = "Error :" + ex.Message
            End Try
            Return Json(New With {.stat = stat, .msg = msg}, JsonRequestBehavior.AllowGet)
        End Function

#End Region

#Region "Material Composition"
        Function MaterialComposition() As ActionResult
            ViewData("AllProduk") = GetMaterialProductList("ID", "Name")
            '(From a In ctx.MstMaterials
            '                 Where a.IdMaterialType = MaterialType.inProcess _
            '                    OrElse a.IdMaterialType = MaterialType.Product
            '                 Select a.ID, a.Name).ToList()
            ViewData("ListAsphal") = ctx.DropdownAsphal.ToList()
            Dim sbQuery As New System.Text.StringBuilder
            sbQuery.Append("SELECT * " & vbCrLf)
            sbQuery.Append("FROM   prod.mstmaterials " & vbCrLf)
            sbQuery.Append("WHERE  (idmaterialtype = " & MaterialType.inProcess & " OR idmaterialtype = " & MaterialType.Product & ")" & vbCrLf)
            sbQuery.Append("       AND id NOT IN (SELECT [idmaterial] " & vbCrLf)
            sbQuery.Append("                      FROM   [Prod].[mstmaterialcompositions])")
            ViewData("produk") = ctx.ExecuteStoreQuery(Of MstMaterials)(sbQuery.ToString()).ToList()
            ViewData("listComp") = (From m In ctx.MstMaterials
                                    Where m.IdMaterialType <= MaterialType.inProcess _
                                        OrElse m.IdMaterialType = MaterialType.Subsidiary
                                    Order By m.Code, m.Name
                                    Select ID = m.ID, Name = m.Name).ToList()


            ViewBag.MaterialList = GetMaterialList("ID", "Name")

            Return View()
        End Function

        Public Function GetMaterialList(ByVal valueField As String, ByVal textField As String, Optional ByVal GroupData As String = Nothing) As Dictionary(Of String, SelectList)
            Dim List As New Dictionary(Of String, SelectList)
            Dim subList As SelectList

            Dim data = From m In ctx.MstMaterials _
                         Join ts In ctx.MstMaterialTypes On ts.ID Equals m.IdMaterialType
                                 Where Not m.IdMaterialType = MaterialType.Product
                         Group m By itemID = ts.ID, item = ts.Type Into subItem = Group
                         Order By itemID

            For Each Value In data
                subList = New SelectList(Value.subItem, valueField, textField)
                List.Add(Value.item, subList)
            Next
            Return List
        End Function

        Public Function GetMaterialProductList(ByVal valueField As String, ByVal textField As String, Optional ByVal GroupData As String = Nothing) As Dictionary(Of String, SelectList)
            Dim List As New Dictionary(Of String, SelectList)
            Dim subList As SelectList

            Dim data = From m In ctx.MstMaterials _
                         Join ts In ctx.MstMaterialTypes On ts.ID Equals m.IdMaterialType
                                 Where m.IdMaterialType = MaterialType.inProcess _
                                    OrElse m.IdMaterialType = MaterialType.Product
                         Group m By itemID = ts.ID, item = ts.Type Into subItem = Group
                         Order By itemID

            For Each Value In data
                subList = New SelectList(Value.subItem, valueField, textField)
                List.Add(Value.item, subList)
            Next
            Return List
        End Function

        Function MC_GetList() As JsonResult
            Dim model = ctx.ListKomposisi.ToList()
            Return Json(New With {.data = model}, JsonRequestBehavior.AllowGet)
        End Function

        Function MC_Save(ByVal form As FormCollection, ByVal IDMaterialComposition As List(Of Integer), ByVal Amount As List(Of Double)) As JsonResult
            Dim stat As Integer = 0
            '   If ModelState.IsValid Then
            If form(0).Equals("") Then
                ModelState.AddModelError("Error", "Pilih Material")
            Else
                If Not IsNothing(IDMaterialComposition) Then
                    If Not IsNothing(Amount) Then
                        Try
                            For counter As Integer = 0 To IDMaterialComposition.Count - 1
                                Dim model As New MstMaterialCompositions
                                model.IDMaterial = form(0)
                                model.IDMaterialComposition = IDMaterialComposition(counter)
                                model.Amount = Amount(counter)
                                ctx.MstMaterialCompositions.AddObject(model)
                            Next
                            ctx.SaveChanges()
                            stat = 1
                            ModelState.AddModelError("Success", "Data Berhasil Disimpan")
                        Catch ex As Exception
                            ModelState.AddModelError("Error", " Gagal Menyimpan Data")
                        End Try
                    Else
                        ModelState.AddModelError("Error", "Isi Jumlah")
                    End If
                Else
                    ModelState.AddModelError("Error", "Pilih Komposisi")
                End If
            End If
            Dim errorlist = ModelState.ToDictionary(Function(k) k.Key,
                                                     Function(k) k.Value.Errors.Select(Function(e) e.ErrorMessage).ToArray()).
                                                     Where(Function(k) k.Value.Count > 0)
            Return Json(New With {.msg = errorlist, .stat = stat})
        End Function

        Function Laporan(Optional ByVal HotMix As Integer = 0) As JsonResult
            Try
                If HotMix = 0 Then
                    Return Json(New With {.msg = "Mohon pilih hotmix yang hendak dicetak pada dropdown disamping tombol cetak"}, JsonRequestBehavior.AllowGet)
                End If
                Dim dataReport = ctx.produkCompositions(HotMix).ToList()
                Return Json(New With {.data = dataReport}, JsonRequestBehavior.AllowGet)
            Catch ex As Exception
                Return Json(New With {.msg = "Terjadi kesalan mohon laporkan ke administrator. Error Message :" + ex.Message}, JsonRequestBehavior.AllowGet)
            End Try
        End Function

        Function MC_Delete(ByVal id As Integer) As JsonResult
            Dim stat As Integer = 0
            Dim msg As String = ""
            Try
                Dim data = (From comp In ctx.MstMaterialCompositions Where comp.ID = id Select comp).FirstOrDefault ' Selecting First or default data here
                ctx.MstMaterialCompositions.DeleteObject(data)
                ctx.SaveChanges()
                stat = 1
                msg = "Success Deleted"
            Catch ex As Exception
                msg = "Error :" + ex.Message
            End Try
            Return Json(New With {.stat = stat, .msg = msg}, JsonRequestBehavior.AllowGet)
        End Function

        Function MC_DeleteAll(ByVal id As Integer) As JsonResult
            Dim stat As Integer = 0
            Dim msg As String = ""
            Try
                Dim data = (From comp In ctx.MstMaterialCompositions Where comp.IDMaterial = id Select comp).ToList()
                For Each d In data
                    ctx.MstMaterialCompositions.DeleteObject(d)
                Next
                ctx.SaveChanges()
                stat = 1
                msg = "Success Deleted"
            Catch ex As Exception
                msg = "Error :" + ex.Message
            End Try
            Return Json(New With {.stat = stat, .msg = msg}, JsonRequestBehavior.AllowGet)
        End Function

        Function MC_Edit(ByVal model As MstMaterialCompositions) As JsonResult
            Dim stat As Integer = 0
            If ModelState.IsValid Then
                Try
                    model.Save()
                    stat = 1
                    ModelState.AddModelError("Success", "Data Berhasil DiUpdate")
                Catch ex As Exception
                    ModelState.AddModelError("Error", ex.Message)
                End Try

            End If
            Dim errorlist = ModelState.ToDictionary(Function(k) k.Key,
                                                      Function(k) k.Value.Errors.Select(Function(e) e.ErrorMessage).ToArray()).
                                                      Where(Function(k) k.Value.Count > 0)
            Return Json(New With {.msg = errorlist, .stat = stat})
        End Function
#End Region

#Region "Material Unit"
        Function MaterialUnit() As ActionResult
            Return View()
        End Function

        Function Unit_GetList() As JsonResult
            Dim model = (From s In ctx.MstMeasurementUnits Select s.ID, s.Unit, s.Symbol, s.Ratio).ToList()
            Return Json(New With {.data = model}, JsonRequestBehavior.AllowGet)
        End Function

        Function Unit_Save(ByVal model As MstMeasurementUnits) As JsonResult
            Dim stat As Integer = 0
            If ModelState.IsValid Then
                Try
                    model.Save()
                    stat = 1
                    ModelState.AddModelError("Success", "Data Berhasil Disimpan")
                Catch ex As Exception
                    ModelState.AddModelError("Error", ex.Message)
                End Try
            End If
            Dim errorlist = ModelState.ToDictionary(Function(k) k.Key,
                                                    Function(k) k.Value.Errors.Select(Function(e) e.ErrorMessage).ToArray()).
                                                    Where(Function(k) k.Value.Count > 0)
            Return Json(New With {.msg = errorlist, .stat = stat})
        End Function

        Function Unit_Delete(ByVal id As Integer) As JsonResult
            Dim stat As Integer = 0
            Dim msg As String = ""
            Try
                Dim del_unit = (From s In ctx.MstMeasurementUnits Where s.ID = id Select s).FirstOrDefault()
                ctx.DeleteObject(del_unit)
                ctx.SaveChanges()
                stat = 1
                msg = "Success Deleted"
            Catch ex As Exception
                msg = "Error :" + ex.Message
            End Try
            Return Json(New With {.stat = stat, .msg = msg}, JsonRequestBehavior.AllowGet)
        End Function
#End Region

#Region "Material Vehicle"

        Function MaterialVehicle() As ActionResult
            Return View()
        End Function

        Function Vehicle_GetList() As JsonResult
            Dim model = Nothing ' Selecting First or default data here
            Return Json(New With {.data = model}, JsonRequestBehavior.AllowGet)
        End Function

        Function Vehicle_Save(ByVal model As Object) As JsonResult
            Dim stat As Integer = 0
            Dim message As String = ""
            Try
                Dim data = Nothing ' Selecting First or default data here
                If Not IsNothing(data) Then
                    ' Update code here
                Else
                    ' Insert code here
                End If
                ctx.SaveChanges()
                stat = 1
                message = "Success"
            Catch ex As Exception
                message = ex.Message
            End Try
            Return Json(New With {.msg = message, .stat = stat}, JsonRequestBehavior.AllowGet)
        End Function

        Function Vehicle_Delete(ByVal id As Integer) As JsonResult
            Dim stat As Integer = 0
            Dim msg As String = ""
            Try
                Dim data = Nothing ' Selecting First or default data here
                ctx.DeleteObject(data)
                ctx.SaveChanges()
                stat = 1
                msg = "Success Deleted"
            Catch ex As Exception
                msg = "Error :" + ex.Message
            End Try
            Return Json(New With {.stat = stat, .msg = msg}, JsonRequestBehavior.AllowGet)
        End Function
#End Region

#Region "Material Weigher"

        Function MaterialWeigher() As ActionResult
            Return View()
        End Function

        Function Weigher_GetList() As JsonResult
            Dim model = Nothing ' Selecting First or default data here
            Return Json(New With {.data = model}, JsonRequestBehavior.AllowGet)
        End Function

        Function Weigher_Save(ByVal model As Object) As JsonResult
            Dim stat As Integer = 0
            Dim message As String = ""
            Try
                Dim data = Nothing ' Selecting First or default data here
                If Not IsNothing(data) Then
                    ' Update code here
                Else
                    ' Insert code here
                End If
                ctx.SaveChanges()
                stat = 1
                message = "Success"
            Catch ex As Exception
                message = ex.Message
            End Try
            Return Json(New With {.msg = message, .stat = stat}, JsonRequestBehavior.AllowGet)
        End Function

        Function Weigher_Delete(ByVal id As Integer) As JsonResult
            Dim stat As Integer = 0
            Dim msg As String = ""
            Try
                Dim data = Nothing ' Selecting First or default data here
                ctx.DeleteObject(data)
                ctx.SaveChanges()
                stat = 1
                msg = "Success Deleted"
            Catch ex As Exception
                msg = "Error :" + ex.Message
            End Try
            Return Json(New With {.stat = stat, .msg = msg}, JsonRequestBehavior.AllowGet)
        End Function

#End Region

#Region "MaterialWeightRatio"

        Function MaterialWeightRatio() As ActionResult
            Dim materials = ctx.MstMaterials.ToList
            Dim listRatio = ctx.MstMeasurementUnits.ToList
            ViewData("materials") = materials
            ViewData("listRatio") = listRatio
            Return View()
        End Function

        Function getListsRatio() As JsonResult
            Dim model = ctx.ListRatio.ToList()
            Return Json(New With {.data = model}, JsonRequestBehavior.AllowGet)
        End Function

        Function checkRasio(ByVal idMaterial As Integer)
            Dim model = (From r In ctx.MstMaterialRatioUnits Where r.IDMaterial = idMaterial Select r).FirstOrDefault()
            Return Json(model)
        End Function

        Function MWR_Save(ByVal form As FormCollection, ByVal ratioVal As List(Of Int32), ByVal Weight As List(Of Int32)) As JsonResult
            Dim stat As Integer = 0
            Dim message As String = ""
            Dim idMaterial As Integer = form(0)
            Try
                Dim data = (From b In ctx.MstMaterialRatioUnits Where b.IDMaterial = idMaterial).FirstOrDefault()  ' Selecting First or default data here
                'If Not IsNothing(data) Then
                '    ' Update code here
                'Else
                '    ' Insert code here
                'End If
                For count As Integer = 0 To ratioVal.Count - 1
                    If Weight(count).Equals(0) Then
                        Continue For
                    End If
                    Dim model As New MstMaterialRatioUnits
                    model.IDMaterial = idMaterial
                    model.Weight = Weight(count)
                    model.IDMeasurementUnit = ratioVal(count)
                    ctx.AddToMstMaterialRatioUnits(model)
                Next
                ctx.SaveChanges()
                stat = 1
                message = "Success"
            Catch ex As Exception
                message = ex.Message
            End Try
            Return Json(New With {.msg = message, .stat = stat}, JsonRequestBehavior.AllowGet)
        End Function

        Function MWR_Delete(ByVal id As Integer) As JsonResult
            Dim stat As Integer = 0
            Dim msg As String = ""
            Try
                Dim data = (From m In ctx.MstMaterialRatioUnits Where m.ID = id Select m).FirstOrDefault() ' Selecting First or default data here
                ctx.DeleteObject(data)
                ctx.SaveChanges()
                stat = 1
                msg = "Success Deleted"
            Catch ex As Exception
                msg = "Error :" + ex.Message
            End Try
            Return Json(New With {.stat = stat, .msg = msg}, JsonRequestBehavior.AllowGet)
        End Function
#End Region

#Region "Machine Types"
        Function MachineType() As ActionResult
            Return View()
        End Function

        Function GetMachineTypeList() As JsonResult
            Dim model = (From mt In ctx.MstMachineTypes Select New With {.ID = mt.ID, .MachineType = mt.MachineType, .Description = mt.Description}).ToList
            Return Json(New With {.data = model}, JsonRequestBehavior.AllowGet)
        End Function

        Function SaveMachineType(ByVal model As MstMachineTypes) As JsonResult
            Dim stat As Integer = 0
            If ModelState.IsValid Then
                Try
                    model.Save()
                    stat = 1
                    ModelState.AddModelError("Success", "Data Berhasil DiSimpan")
                Catch ex As Exception
                    ModelState.AddModelError("Error ", ex.Message)
                End Try
            End If
            Dim errorlist = ModelState.ToDictionary(Function(k) k.Key,
                                                      Function(k) k.Value.Errors.Select(Function(e) e.ErrorMessage).ToArray()).
                                                      Where(Function(k) k.Value.Count > 0)
            Return Json(New With {.msg = errorlist, .stat = stat})
        End Function

        Function DeleteMachineType(ByVal id As Integer) As JsonResult
            Dim stat As Integer = 0
            Dim msg As String = ""
            Try
                Dim data = (From m In ctx.MstMachineTypes Where m.Id = id Select m).FirstOrDefault ' Selecting First or default data here
                ctx.DeleteObject(data)
                ctx.SaveChanges()
                stat = 1
                msg = "Success Deleted"
            Catch ex As Exception
                msg = "Error :" + ex.Message
            End Try
            Return Json(New With {.stat = stat, .msg = msg}, JsonRequestBehavior.AllowGet)
        End Function
#End Region

#Region "Material Machines"
        Function MaterialMachine() As ActionResult
            ViewData("MachineTypes") = ctx.MstMachineTypes.ToList()
            Return View()
        End Function

        Function GetMachineList() As JsonResult
            Dim model = (From mm In ctx.MstMachines Join
                         mt In ctx.MstMachineTypes On mt.ID Equals mm.IdMachineType
                         Select New With {
                             .ID = mm.ID,
                         .MachineName = mm.MachineName,
                         .MachineType = mt.MachineType,
                         .SeriesNumber = mm.SeriesNumber}).ToList
            Return Json(New With {.data = model}, JsonRequestBehavior.AllowGet)
        End Function

        Function SaveMachine(ByVal model As MstMachines) As JsonResult
            Dim stat As Integer = 0
            If ModelState.IsValid Then
                Try
                    model.Save()
                    stat = 1
                    ModelState.AddModelError("Success", "Data Berhasil Disimpan")
                Catch ex As Exception
                    ModelState.AddModelError("Error ", ex.Message)
                End Try
            End If
            Dim errorlist = ModelState.ToDictionary(Function(k) k.Key,
                                                      Function(k) k.Value.Errors.Select(Function(e) e.ErrorMessage).ToArray()).
                                                      Where(Function(k) k.Value.Count > 0)
            Return Json(New With {.msg = errorlist, .stat = stat})
        End Function

        Function DeleteMachine(ByVal id As Integer) As JsonResult
            Dim stat As Integer = 0
            Dim msg As String = ""
            Try
                Dim data = (From m In ctx.MstMachines Where m.ID = id Select m).FirstOrDefault ' Selecting First or default data here
                ctx.DeleteObject(data)
                ctx.SaveChanges()
                stat = 1
                msg = "Success Deleted"
            Catch ex As Exception
                msg = "Error :" + ex.Message
            End Try
            Return Json(New With {.stat = stat, .msg = msg}, JsonRequestBehavior.AllowGet)
        End Function
#End Region
    End Class

End Namespace