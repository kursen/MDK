Namespace Purchasing.Areas.Purchasing.Controllers
    Public Class PriceComparisonController
        Inherits System.Web.Mvc.Controller

        '
        ' GET: /Purchasing/PriceComparisson
        Private _prcEntities As PurchasingEntities
        Function Index() As ActionResult
            Return View()

        End Function

        Public Function GetComparisonList(ByVal m As Integer, ByVal y As Integer) As JsonResult
            Dim model = (From p In _prcEntities.PriceComparison
                        Where p.CreateDate.Month = m AndAlso p.CreateDate.Year.Equals(y)
                        Select New With {p.ID, p.CreateDate, .RecordNo = p.DepartmentPurchaseRequisition.RecordNo}).ToArray()
            Return Json(New With {.data = model})
        End Function

        Function Create() As ActionResult
            Dim modelPR As New PriceComparison
            modelPR.CreateDate = Date.Today
            Dim companyList = (From m In _prcEntities.Vendor
                            Select m).ToList
            ViewData("companyList") = companyList
            Return View("Form", modelPR)
        End Function

        Function Edit(ByVal id As Integer) As ActionResult
            Dim model = _prcEntities.PriceComparison.Where(Function(m) m.ID = id).SingleOrDefault()
            If model Is Nothing Then
                Throw New HttpException(404, "NOT FOUND")
            End If
            Dim companyList = (From m In _prcEntities.Vendor
                             Select m).ToList

            ViewData("companyList") = companyList
            Return View("Form", model)
        End Function

        Public Function Detail(ByVal Id As Integer) As ActionResult
            Dim model = (From pc In _prcEntities.PriceComparison Where pc.ID = Id).SingleOrDefault

            If model Is Nothing Then
                Throw New HttpException(404, "NOT FOUND")
            End If
            Dim companyIds = {model.VendorID1, model.VendorID2, model.VendorID3}
            Dim companyList = (From m In _prcEntities.Vendor
                              Where companyIds.Contains(m.Id)
                              Select m).ToList

            ViewData("Companylist") = companyList
            Return View(model)
        End Function

        <HttpPost()>
        Function Delete(ByVal id As Integer) As ActionResult
            Try
                Dim model = _prcEntities.PriceComparison.Where(Function(pc) pc.ID = id).SingleOrDefault
                If model IsNot Nothing Then
                    _prcEntities.DeleteObject(model)
                End If

                _prcEntities.SaveChanges()
                Return Json(New With {.stat = 1})
            Catch ex As Exception
                Return Json(New With {.stat = 0, .errors = HaloUIHelpers.Helpers.JsonErrorObject.Convert(ex)})
            End Try
        End Function

        <HttpPost()>
        Public Function GetVendorList(query As String) As ActionResult
            Dim model = (From v In _prcEntities.Vendor
                         Where v.Name.Contains(query)
                        Select New With {.value = v.Id, .text = v.Name})

            Return Json(model)
        End Function

        <HttpPost()> _
        Function autocompleteRequestNumber(ByVal term As String) As JsonResult
            Dim modelautocomplete As List(Of GetNoRecord_Result) = _prcEntities.GetNoRecord(term).ToList
            If modelautocomplete.Count = 0 Then
                Dim model = New GetNoRecord_Result With {.ID = 0, .Norecord = "Nomor Permintaan Tidak Ada", .officeId = 0}
                modelautocomplete.Add(model)
            End If
            Return Json(modelautocomplete)
        End Function

        <HttpPost()> _
        Function GetRequestNumber(ByVal NoRecord As String) As JsonResult
            Dim PRIDList = (From prc In _prcEntities.PriceComparison Select prc.PurchaseRequisitionID).ToList
            Dim model = (From dpr In _prcEntities.DepartmentPurchaseRequisitions Where
                         dpr.RecordNo = NoRecord Select New With {.ID = dpr.ID, .DocState = dpr.DocState}).DefaultIfEmpty.FirstOrDefault
            Dim IDReturn = 0 'status not found
            If model IsNot Nothing Then
                Select Case model.DocState
                    Case 0
                        IDReturn = -1 'status still draft
                    Case 1
                        IDReturn = -2 'status not approval
                    Case 2
                        IDReturn = model.ID
                        If PRIDList.Contains(IDReturn) Then IDReturn = -3 'already have price comparison
                End Select
            End If
            Return Json(IDReturn)
        End Function

        <HttpPost()>
        Function ValidatePrice(ByVal value As String, colindex As Integer, rowindex As Integer) As JsonResult
            Dim price As Decimal
            If Decimal.TryParse(value, price) Then
                Return Json(New With {.value = price, .stat = 1, rowindex, colindex})
            End If
            Return Json(New With {.value = price})
        End Function
        <HttpPost()>
        Function GetItemToPriceComparison(ByVal id As Integer) As JsonResult

            Dim priceComparison = (From m In _prcEntities.PriceComparison
                                  Where m.PurchaseRequisitionID = id).SingleOrDefault()


            If priceComparison IsNot Nothing Then
                Dim priceComparisonDetail = (From d In priceComparison.PriceComparisonDetail
                                                      Select New With {d.ID, .ItemName = d.DepartmentPRDetail.ItemName, .Quantity = d.DepartmentPRDetail.Quantity,
                                                                       .UnitQuantity = d.DepartmentPRDetail.UnitQuantity, d.PRDetailID,
                                                                       .PriceComparisonid = d.PRDetailID, d.Price1, d.Price2, d.Price3,
                                                                       .TotalEstPrice1 = d.Price1 * d.DepartmentPRDetail.Quantity,
                                                                       .TotalEstPrice2 = d.Price2 * d.DepartmentPRDetail.Quantity,
                                                                       .TotalEstPrice3 = d.Price3 * d.DepartmentPRDetail.Quantity}).ToList()
                Return Json(New With {.data = priceComparisonDetail, .model = New With {priceComparison.ID, priceComparison.PurchaseRequisitionID,
                                                                              priceComparison.VendorID1, priceComparison.VendorID2, priceComparison.VendorID3,
                                                                                        .existing = 1}})
            End If
            Dim PurchaseRequest = (From p In _prcEntities.DepartmentPurchaseRequisitions
                                  Where p.ID = id).SingleOrDefault

            If PurchaseRequest Is Nothing Then
                Return Json(New With {.data = Nothing, .model = Nothing})
            End If
            Dim model = New PriceComparison With {.PurchaseRequisitionID = id}
            Dim modelDetail = From pd In PurchaseRequest.DepartmentPRDetails
                             Select New With {.ID = 0, .ItemName = pd.ItemName, .Quantity = pd.Quantity, .UnitQuantity = pd.UnitQuantity,
                                              .PRDetailID = pd.ID,
                                              .PriceComparisonID = 0, .Price1 = 0, .Price2 = 0, .Price3 = 0,
                                              .TotalEstPrice1 = 0, .TotalEstPrice2 = 0, .TotalEstPrice3 = 0}


            Return Json(New With {.data = modelDetail,
                                  .model = New With {model.ID, model.PurchaseRequisitionID, model.VendorID1, model.VendorID2, model.VendorID3},
                                  .existing = 0})
        End Function

        <HttpPost()> _
        Function autocompleteVendor(ByVal term As String) As JsonResult
            Dim model = (From v In _prcEntities.Vendor Where v.Name.Contains(term)
                         Select New With {v.Id, v.Name}).ToList
            Return Json(model)
        End Function

        <HttpPost()> _
        Function SaveItemPriceComparison(ByVal header As PriceComparison, ByVal listItemComparison As String) As JsonResult

            Dim lsVendorId As New List(Of Integer)
            If header.VendorID1 > 0 Then lsVendorId.Add(header.VendorID1)
            If header.VendorID2 > 0 Then lsVendorId.Add(header.VendorID2)
            If header.VendorID3 > 0 Then lsVendorId.Add(header.VendorID3)

            Select Case header.VendorID1
                Case header.VendorID1 = header.VendorID2

            End Select

            If lsVendorId.Count = 0 Then
                ModelState.AddModelError("General", "Vendor belum dipilih.")
            End If
            Dim uniques = lsVendorId.Distinct().ToList()
            If uniques.Count <> lsVendorId.Count Then
                ModelState.AddModelError("General", "Pastikan bahwa semua vendor berbeda")
            End If

            If ModelState.IsValid Then

                _prcEntities.PriceComparison.AddObject(header)
                If header.ID > 0 Then
                    _prcEntities.ObjectStateManager.ChangeObjectState(header, EntityState.Modified)
                End If
                Dim lsItems = New List(Of PriceComparisonDetail)


                Newtonsoft.Json.JsonConvert.PopulateObject(listItemComparison, lsItems)
                For Each item In lsItems
                    header.PriceComparisonDetail.Add(item)
                    If item.ID > 0 Then
                        _prcEntities.ObjectStateManager.ChangeObjectState(item, EntityState.Modified)
                    End If
                Next
                _prcEntities.SaveChanges()


                Return Json(New With {.stat = 1, .id = header.ID})
            End If
            Return Json(New With {.stat = -0, .errors = HaloUIHelpers.Helpers.JsonErrorObject.Convert(ModelState)})
        End Function


        Public Sub New()
            _prcEntities = New PurchasingEntities
        End Sub
    End Class
End Namespace