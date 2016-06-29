Imports System.Web.Script.Serialization

Namespace MDK_ERP.Areas.Production.Controllers

    <Authorize()> _
    Public Class StockController
        Inherits BaseController

#Region "Initial Stock"

        Function InitialStock() As ActionResult
            Try
                ViewData("IDMaterial") = New SelectList(ctx.MstMaterials, "ID", "Name")
                ViewData("IDMeasurementUnit") = New SelectList(ctx.MstMeasurementUnits, "ID", "Unit")
                Return View()
            Catch ex As Exception
                TempData("exeptionMsg") = ex.Message
                Return RedirectToRoute("Default", New With {.Action = "Index", .Controller = "Error"})
            End Try
        End Function

        Function GetInitialStockList() As JsonResult
            Dim model = (From mis In ctx.MaterialInitialStock
                         Join mm In ctx.MstMaterials On mm.ID Equals mis.IDMaterial
                         Join mu In ctx.MstMeasurementUnits On mu.ID Equals mis.IDMeasurementUnit
                         Select
                         New With {
                             .Tanggal = mis.DateInput,
                             .NamaMaterial = mm.Name,
                             .JumlahInput = mis.Amount,
                             .Satuan = mu.Unit,
                             .Description = mis.Description}).ToList
            Return Json(New With {.data = model}, JsonRequestBehavior.AllowGet)
        End Function

        Function SaveInitialStock(ByVal model As MaterialInitialStock) As JsonResult
            Dim stat As Integer = 0
            Dim message As String = ""

            Try

                Dim dataInitialStock = (From m In ctx.MaterialInitialStock Where m.ID = model.ID Select m).FirstOrDefault
                If Not IsNothing(dataInitialStock) Then
                    With dataInitialStock
                        .DateInput = model.DateInput
                        .IDMaterial = model.IDMaterial
                        .Amount = model.Amount
                        .IDMeasurementUnit = model.IDMeasurementUnit
                        .Description = model.Description
                    End With
                Else
                    'Save data inventory
                    Dim dataInventory As New MaterialInventories With {
                            .IdInventoryStatus = 1,
                            .IsPlus = True
                        }

                    ctx.MaterialInventories.AddObject(dataInventory)
                    ctx.SaveChanges()
                    model.IDInventory = dataInventory.ID

                    ctx.MaterialInitialStock.AddObject(model)
                End If
                ctx.SaveChanges()
                stat = 1
                message = "Success"
            Catch ex As Exception
                message = ex.Message
            End Try
            Return Json(New With {.msg = message, .stat = stat}, JsonRequestBehavior.AllowGet)
        End Function

        Function DeleteInitialStock(ByVal id As Integer) As JsonResult
            Dim stat As Integer = 0
            Dim msg As String = ""
            Try
                Dim data = (From m In ctx.MaterialInitialStock Where m.ID = id Select m).FirstOrDefault ' Selecting First or default data here
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

#Region "Stock"

        Function Index() As ActionResult
            Try
                ViewBag.MaterialList = GetMaterialList("ID", "Name")
                Return View()
            Catch ex As Exception
                TempData("exeptionMsg") = ex.InnerException.Message
                Return RedirectToRoute("Default", New With {.Action = "Index", .Controller = "Error"})
            End Try
        End Function

        Public Function GetMaterialList(ByVal valueField As String, ByVal textField As String, Optional ByVal GroupData As String = Nothing) As Dictionary(Of String, SelectList)
            Dim List As New Dictionary(Of String, SelectList)
            Dim subList As SelectList

            Dim data = From m In ctx.MstMaterials _
                         Join ts In ctx.MstMaterialTypes On ts.ID Equals m.IdMaterialType
                         Where m.isInventory = True
                         Group m By itemID = ts.ID, item = ts.Type Into subItem = Group
                         Order By itemID

            For Each Value In data
                subList = New SelectList(Value.subItem, valueField, textField)
                List.Add(Value.item, subList)
            Next
            Return List
        End Function

        Function GetInventoryData(IDMaterial As Integer, monthData As Byte, yearData As Integer) As JsonResult
            Dim model = (From gid In ctx.GetInventoryData(IDMaterial, monthData, yearData) Select gid Order By gid.UrutBaris, gid.Tanggal Ascending).ToList()
            Return Json(model)
        End Function
#End Region

#Region "Distribution"

        Function Distribution() As ActionResult
            Try
                Return View()
            Catch ex As Exception
                TempData("exeptionMsg") = ex.InnerException.Message
                Return RedirectToRoute("Default", New With {.Action = "Index", .Controller = "Error"})
            End Try
        End Function

        Function GetReceivedData(ByVal startDate As String, ByVal endDate As String) As JsonResult 'get Received Distribution List
            Dim date1 = StrtoDate(startDate)
            Dim date2 = StrtoDate(endDate).AddDays(1)
            Dim model = (From d In ctx.DistributionJournals _
                    Join m In ctx.MstMaterials On d.IdMaterial Equals m.ID _
                    Join p In ctx.CompanyLists On d.IdCompany Equals p.ID _
                    Where d.IdDeliveryStatus = 1 _
                        AndAlso (d.InTime.Value >= date1 AndAlso d.InTime.Value <= date2)
                    Select New DistributionList With {
                            .NumRecord = d.NoRec,
                            .MaterialCode = m.Code,
                            .Material = m.Name,
                            .DateDist = d.InTime,
                            .Company = p.Name,
                            .Netto = Math.Abs(CDbl(d.Weight1 - d.Weight2)),
                            .PoliceLicense = d.PoliceLicense,
                            .Driver = d.DriverName,
                            .Place = d.Place,
                            .Clerk1 = d.Clerk1,
                            .Clerk2 = d.Clerk2
                        }).ToList()
            Return Json(New With {.data = model}, "application/json", JsonRequestBehavior.AllowGet)
        End Function

        Function GetDeliveredData(ByVal startDate As String, ByVal endDate As String) As JsonResult 'get Delivered Distribution List
            Dim date1 = StrtoDate(startDate)
            Dim date2 = StrtoDate(endDate).AddDays(1)
            Dim model = (From d In ctx.DistributionJournals _
                    Join m In ctx.MstMaterials On d.IdMaterial Equals m.ID _
                    Join p In ctx.CompanyLists On d.IdCompany Equals p.ID _
                    Where d.IdDeliveryStatus = 2 _
                        AndAlso (d.OutTime.Value >= date1 AndAlso d.OutTime.Value <= date2)
                    Select New DistributionList With {
                            .NumRecord = d.NoRec,
                            .MaterialCode = m.Code,
                            .Material = m.Name,
                            .DateDist = d.OutTime,
                            .Company = p.Name,
                            .Netto = Math.Abs(CDbl(d.Weight1 - d.Weight2)),
                            .PoliceLicense = d.PoliceLicense,
                            .Driver = d.DriverName,
                            .Place = d.Place,
                            .Clerk1 = d.Clerk1,
                            .Clerk2 = d.Clerk2
                        }).ToList()

            'Dim serializer = New JavaScriptSerializer With {.MaxJsonLength = Int32.MaxValue, .RecursionLimit = 100}
            'Dim content = New ContentResult() With {.Content = serializer.Serialize(model), .ContentType = "application/json"}
            'Return content
            'Dim test = New JsonResult() With {.Data = New With {.Data = serializer.Serialize(model)}, .ContentType = "application/json", .JsonRequestBehavior = JsonRequestBehavior.AllowGet}
            'Return test

            Return Json(New With {.data = model}, "application/json", JsonRequestBehavior.AllowGet)
        End Function

        Class DistributionList
            Public Property NumRecord As String
            Public Property MaterialCode As String
            Public Property Material As String
            Public Property DateDist As String
            Public Property Company As String
            Public Property Netto As Double
            Public Property PoliceLicense As String
            Public Property Driver As String
            Public Property Place As String
            Public Property Clerk1 As String
            Public Property Clerk2 As String
        End Class

#End Region

#Region "Data Scales"

        Function DataScales() As ActionResult
            Return View()
        End Function

        Function DataScalesIn_() As ActionResult
            ViewBag.MaterialList = GetMaterialList("ID", "Name")
            ViewData("KodePeru") = GetCompanyList("ID", "Name")
            Return View()
        End Function

        <HttpPost()> _
        Function DataScalesIn_(ByVal model As ModelTimbangan, ByVal TglMasuk As String) As ActionResult
            'If ModelState.IsValid Then
            Try
                If model.KodePeru = 0 Then
                    ModelState.AddModelError("", "Kode Perusahaan wajib dipilih")
                    Exit Try
                End If
                model.TglMasuk = StrtoDatetime(TglMasuk)
                Dim distribusi As New DistributionJournals With {
                    .InTime = model.TglMasuk,
                    .Clerk1 = model.Clerk1,
                    .Weight1 = model.Berat1,
                    .DriverName = model.Sopir,
                    .Place = model.Deliverynote,
                    .IdCompany = model.KodePeru,
                    .IdMaterial = model.KodeBrg,
                    .PoliceLicense = model.NoPol1.ToUpper() + " " + model.NoPol2.ToString.ToUpper() + " " + model.NoPol3.ToUpper(),
                    .IdDeliveryStatus = 2
                }

                ctx.DistributionJournals.AddObject(distribusi)
                ctx.SaveChanges()
                Return RedirectToAction("DataScales", "Stock")
            Catch ex As Exception
                ModelState.AddModelError("", ex.Message)
            End Try
            'End If

            'if we got this far so return back the view
            ViewBag.MaterialList = GetMaterialList("ID", "Name")
            ViewData("KodePeru") = GetCompanyList("ID", "Name", model.KodePeru)
            Return View(model)
        End Function

        Function DataScalesOut_(ByVal ID As Integer) As ActionResult
            Dim model As DistributionJournals
            Try
                ViewData("Place") = New SelectList(ctx.ProjectList, "NoProject", "NoProject")
                model = ctx.DistributionJournals.Where(Function(m) m.ID = ID).FirstOrDefault()
                ViewData("tglMasuk") = model.InTime.Value.ToString("dd-MM-yyyy HH:mm")
                ViewData("tglKeluar") = Date.Now.ToString("dd-MM-yyyy HH:mm")

                If Not IsNothing(model) Then
                    Return (View(model))
                Else
                    TempData("errTitle") = "Data Not Found !"
                    TempData("exeptionMsg") = "Data Tidak Ditemukan. Mohon kembali dan periksa kesalahan. Apabila terus berlanjut mohon hubungi Administrator"
                    Return RedirectToRoute("Default", New With {.Action = "Index", .Controller = "Error"})
                End If
            Catch ex As Exception
                TempData("exeptionMsg") = ex.Message
                Return RedirectToRoute("Default", New With {.Action = "Index", .Controller = "Error"})
            End Try
        End Function

        <HttpPost()>
        Function DataScalesOut_(ByVal model As DistributionJournals, ByVal InTime As String, ByVal OutTime As String) As ActionResult
            'MsgBox(ModelState.IsValid)
            'If ModelState.IsValid Then
            Try
                model.InTime = StrtoDatetime(InTime)
                model.OutTime = StrtoDatetime(OutTime)
                model.Save()
                Return RedirectToAction("DataScales", "Stock")
            Catch ex As Exception
                ModelState.AddModelError("", ex.Message)
            End Try
            'End If
            ViewData("Place") = New SelectList(ctx.ProjectList, "NoProject", "NoProject")
            Return View(model)
        End Function

        Function Scales_GetList() As JsonResult
            Dim model = ctx.ListTimbangan1.ToList()
            Return Json(New With {.data = model}, JsonRequestBehavior.AllowGet)
        End Function

        Private Function GetCompanyList(ByVal valueField As String, ByVal textField As String, Optional selected As Object = 0) As SelectList
            Dim subList As SelectList

            Dim data = From c In ctx.CompanyLists ' Select selectList With {.text = c.Name, .value = c.ID}
            subList = New SelectList(data, valueField, textField, selected)
            Return subList
        End Function

        'Function Scales_Save(ByVal model As TblTrans_Penimbangan2) As JsonResult
        '    Dim stat As Integer = 0
        '    If ModelState.IsValid Then
        '        Try
        '            model.Save()
        '            stat = 1
        '            ModelState.AddModelError("Success", "Data Berhasil Disimpan")
        '        Catch ex As Exception
        '            ModelState.AddModelError("Error", ex.Message)
        '        End Try
        '    End If
        '    Dim errorlist = ModelState.ToDictionary(Function(k) k.Key,
        '                                             Function(k) k.Value.Errors.Select(Function(e) e.ErrorMessage).ToArray()).
        '                                             Where(Function(k) k.Value.Count > 0)
        '    Return Json(New With {.msg = errorlist, .stat = stat})
        'End Function

        Function Scales_Delete(ByVal id As Integer) As JsonResult
            Dim stat As Integer = 0
            Dim msg As String = ""
            Try
                Dim del_unit = (From s In ctx.DistributionJournals Where s.ID = id Select s).FirstOrDefault()
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

    End Class
End Namespace