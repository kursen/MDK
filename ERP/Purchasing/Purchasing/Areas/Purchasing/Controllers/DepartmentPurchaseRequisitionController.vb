Imports Microsoft.Reporting.WebForms

Namespace Purchasing.Areas.Purchasing.Controllers
    Public Class DepartmentPurchaseRequisitionController
        Inherits System.Web.Mvc.Controller
        Dim _purchaseEntities As New PurchasingEntities
        Dim currentUser As ERPBase.ErpUserProfile

        Dim mimeType As String = "application/pdf"
        Dim Encoding As String = Nothing
        Dim fileNameExtension As String = Nothing
        Dim streams As String() = Nothing
        Dim warnings() As Microsoft.Reporting.WebForms.Warning = Nothing

        Function Index() As ActionResult
            Return View()
        End Function

        <HttpPost()>
        Function GetRequisitionList() As ActionResult
            Dim model As IList
            Dim prStaffOrManager = User.IsInRole("Purchasing.Staff") OrElse User.IsInRole("Purchasing.Manager")
            If prStaffOrManager Then
                model = (From a In _purchaseEntities.DepartmentPurchaseRequisitions
                         Where a.Archive = False
                         Select New With {a.ID, a.RecordNo, a.RequestDate,
                                          a.RequestedBy_Name, .RequestTypeName = a.RequestType.Name, a.RequestedBy_Occupation, a.DocState,
                                          .Details = From d In a.DepartmentPRDetails
                                                     Select New With {d.ID, d.ItemName, d.Allocation, d.Quantity, d.UnitQuantity,
                                                                      .UnitPrice = d.EstUnitPrice, .TotalPrice = d.TotalEstPrice, d.Currency}}).ToList()

            Else
                model = (From a In _purchaseEntities.DepartmentPurchaseRequisitions
                         Where a.Archive = False _
                            AndAlso a.OfficeID = currentUser.WorkUnitId
                         Select New With {a.ID, a.RecordNo, a.RequestDate,
                                          a.RequestedBy_Name, .RequestTypeName = a.RequestType.Name, a.RequestedBy_Occupation, a.DocState,
                                          .Details = From d In a.DepartmentPRDetails
                                                     Select New With {d.ID, d.ItemName, d.Allocation, d.Quantity, d.UnitQuantity,
                                                                      .UnitPrice = d.EstUnitPrice, .TotalPrice = d.TotalEstPrice, d.Currency}}).ToList()
            End If
            
            Dim list = model

            Return Json(New With {.data = list})
        End Function
        Function Create() As ActionResult

            ViewData("officeName") = StrConv((From o In _purchaseEntities.Office Where
                                              o.Id = currentUser.WorkUnitId Select o.Name).FirstOrDefault, vbProperCase)

            Dim model As New DepartmentPurchaseRequisition With {.OfficeID = currentUser.WorkUnitId, .DocState = 0}
            model.RequestDate = Today
            model.DeliveryDate = Today.AddDays(7)


            Dim lsDocState = New List(Of ERPBase.OptionItem)
            For i As Integer = 0 To 1
                lsDocState.Add(New ERPBase.OptionItem With {.Value = i, .Text = GlobalArray.PurchaseRequisitionDocState(i)})
            Next

            PrepareForm(model)
            Return View(model)
        End Function
        Public Function Edit(ByVal id As Integer) As ActionResult
            Dim model = _purchaseEntities.DepartmentPurchaseRequisitions.Where(Function(m) m.ID = id).SingleOrDefault()
            If model Is Nothing Then
                Throw New HttpException(404, "NOT Found")
            End If

            If model.OfficeID <> currentUser.WorkUnitId Then
                Return View("AccessDenied")
            End If
            PrepareForm(model)
            Return View("Create", model)
        End Function

        Private Sub PrepareForm(ByVal model As DepartmentPurchaseRequisition)
            'For detail 
            Dim currencyList = (From c In _purchaseEntities.CurrencyMaster
                           Select New With {.id = c.CurrencyName, .name = c.CurrencyName}).ToList()
            ViewData("Currency") = New SelectList(currencyList, "Name", "Name", 1)
            Dim ReqTypes = From r In _purchaseEntities.RequestTypes
                           Select New With {r.ID, r.Name}

            ViewData("RequestTypeId") = New SelectList(ReqTypes, "ID", "Name", model.RequestTypeId)

        End Sub

        Function Detail(ByVal id As Integer) As ActionResult
            'For detail 
            Dim currencyList = (From c In _purchaseEntities.CurrencyMaster
                            Select New With {.id = c.Id, .name = c.CurrencyName}).ToList()
            ViewData("currencyList") = New SelectList(currencyList, "id", "Name", 0)
            Dim measureList = (From m In _purchaseEntities.Measure
                            Select New With {.id = m.Id, .name = m.MeasureName}).ToList()
            ViewData("measureList") = New SelectList(measureList, "id", "Name", 0)

            Dim model = (From a As DepartmentPurchaseRequisition In _purchaseEntities.DepartmentPurchaseRequisitions
                         Where a.ID = id
                         Select a).SingleOrDefault()

            Return View(model)
        End Function

        <HttpPost()>
        Function GetRequestItems(ByVal Id As Integer) As ActionResult

            Dim model = _purchaseEntities.DepartmentPurchaseRequisitions.Where(Function(m) m.ID = Id).SingleOrDefault()
            If model Is Nothing Then
                Throw New HttpException(404, "NOT FOUND")
            End If
            Dim items = From m In model.DepartmentPRDetails
                        Select New With {m.ID, m.ItemName, m.Allocation, m.Currency, m.EstUnitPrice, m.UnitQuantity, m.TotalEstPrice,
                                         m.Quantity, m.Remarks, m.DepartmentPurchaseRequisitionId}

            Return Json(New With {.data = items.ToArray()})
        End Function

        <HttpPost()>
        Function DeleteDocument(ByVal id As Integer) As ActionResult
            Dim model = _purchaseEntities.DepartmentPurchaseRequisitions.Where(Function(m) m.ID = id).SingleOrDefault
            _purchaseEntities.DeleteObject(model)
            _purchaseEntities.SaveChanges()
            Return Json(New With {.stat = 1})
        End Function


        Function Save(ByVal prData As DepartmentPurchaseRequisition,
                                        ByVal ItemPRDetail As String) As JsonResult
            Try
                If ModelState.IsValid Then
                    'validate data
                    Dim dataExist = _purchaseEntities.DepartmentPurchaseRequisitions.Where(Function(dpr) dpr.RecordNo = prData.RecordNo).FirstOrDefault
                    If dataExist IsNot Nothing AndAlso dataExist.ID <> prData.ID Then
                        ModelState.AddModelError("General", "No. Permintaan sudah pernah terdaftar")
                        Return Json(New With {.stat = -0, .errors = HaloUIHelpers.Helpers.JsonErrorObject.Convert(ModelState)})
                    End If
                    If prData.OfficeID <= 0 Then
                        ModelState.AddModelError("General", "User belum terdaftar ke dalam departemen")
                        Return Json(New With {.stat = -0, .errors = HaloUIHelpers.Helpers.JsonErrorObject.Convert(ModelState)})
                    End If
                    If prData.DeliveryDate < prData.RequestDate Then
                        ModelState.AddModelError("General", "Tanggal Kebutuhan harus lebih besar dari tanggal permintaan")
                        Return Json(New With {.stat = 0, .errors = HaloUIHelpers.Helpers.JsonErrorObject.Convert(ModelState)})
                    End If
                    Dim PRDetailList As New List(Of DepartmentPRDetail)
                    Newtonsoft.Json.JsonConvert.PopulateObject(ItemPRDetail, PRDetailList)
                    If PRDetailList.Count <= 0 Then
                        ModelState.AddModelError("General", "Data ini tidak memiliki detail item yang diminta, harus diisi")
                        Return Json(New With {.stat = 0, .errors = HaloUIHelpers.Helpers.JsonErrorObject.Convert(ModelState)})
                    End If

                    If prData.ID = 0 Then
                        _purchaseEntities.DepartmentPurchaseRequisitions.AddObject(prData)
                        '_purchaseEntities.ObjectStateManager.ChangeObjectState(prData, EntityState.Modified)
                        For Each listPRDetail In PRDetailList
                            prData.DepartmentPRDetails.Add(listPRDetail)
                            '_purchaseEntities.ObjectStateManager.ChangeObjectState(listPRDetail, EntityState.Modified)
                        Next
                        _purchaseEntities.SaveChanges()
                    Else
                        Dim prDataUpdate = _purchaseEntities.DepartmentPurchaseRequisitions.Where(Function(dpr) dpr.ID = prData.ID).FirstOrDefault
                        If prDataUpdate IsNot Nothing Then
                            With prDataUpdate
                                .ApprovedBy_Name = prData.ApprovedBy_Name
                                .ApprovedBy_Occupation = prData.ApprovedBy_Occupation
                                .Archive = prData.Archive
                                .DeliveryAddress = prData.DeliveryAddress
                                .DeliveryDate = prData.DeliveryDate
                                .DeliveryTo = prData.DeliveryTo
                                .DocApproveRejectDate = prData.DocApproveRejectDate
                                .DocState = prData.DocState
                                .OfficeID = prData.OfficeID
                                .RequestedBy_Occupation = prData.RequestedBy_Occupation
                                .RequestTypeId = prData.RequestTypeId
                            End With
                            _purchaseEntities.SaveChanges()
                            For Each listPRDetail In PRDetailList
                                If listPRDetail.ID = 0 Then
                                    prDataUpdate.DepartmentPRDetails.Add(listPRDetail)
                                    '_purchaseEntities.ObjectStateManager.ChangeObjectState(listPRDetail, EntityState.Modified)
                                Else
                                    Dim lstPRDetailUpdate = _purchaseEntities.DepartmentPRDetails.Where(Function(dprd) dprd.ID = listPRDetail.ID).FirstOrDefault
                                    With lstPRDetailUpdate
                                        .Allocation = listPRDetail.Allocation
                                        .Currency = listPRDetail.Currency
                                        .DepartmentPurchaseRequisitionId = listPRDetail.DepartmentPurchaseRequisitionId
                                        .EstUnitPrice = listPRDetail.EstUnitPrice
                                        .ItemName = listPRDetail.ItemName
                                        .Quantity = listPRDetail.Quantity
                                        .Remarks = listPRDetail.Remarks
                                        .TotalEstPrice = listPRDetail.TotalEstPrice
                                        .UnitQuantity = listPRDetail.UnitQuantity
                                    End With
                                End If
                            Next
                            _purchaseEntities.SaveChanges()
                        End If
                    End If


                    Return Json(New With {.stat = 1, .id = prData.ID})
                End If

                Return Json(New With {.stat = -0, .errors = HaloUIHelpers.Helpers.JsonErrorObject.Convert(ModelState)})
            Catch ex As Exception
                ModelState.AddModelError("General", ex.Message)
                Return Json(New With {.stat = -0, .errors = HaloUIHelpers.Helpers.JsonErrorObject.Convert(ModelState)})
            End Try
        End Function

        <HttpPost()>
        Function DeleteRequestItem(ByVal Id As Integer) As JsonResult
            Dim model = _purchaseEntities.DepartmentPRDetails.Where(Function(m) m.ID = Id).SingleOrDefault
            If model IsNot Nothing Then
                Try
                    _purchaseEntities.DeleteObject(model)
                    _purchaseEntities.SaveChanges()
                Catch ex As Exception
                    Return Json(New With {.stat = 0, .errors = HaloUIHelpers.Helpers.JsonErrorObject.Convert(ex)})
                End Try

            End If
            Return Json(New With {.stat = 1})
        End Function

        <HttpPost()>
        Public Function Archive(ByVal Id As Integer) As ActionResult
            Dim model = _purchaseEntities.DepartmentPurchaseRequisitions.Where(Function(m) m.ID = Id).SingleOrDefault()
            If model IsNot Nothing Then
                model.Archive = True
                _purchaseEntities.SaveChanges()
            End If

            Return Json(New With {.stat = 1})
        End Function


        Public Function GetDetails(ByVal DepartmentPRId As Integer) As JsonResult
            Dim detailList = (From p In _purchaseEntities.DepartmentPRDetails
                              Where p.DepartmentPurchaseRequisitionId = DepartmentPRId
                            Select p.ID, p.ItemName, p.Allocation, p.Quantity, p.UnitQuantity, p.Currency,
                            p.EstUnitPrice, p.TotalEstPrice, p.Remarks).ToList()
            Return Json(New With {.data = detailList}, JsonRequestBehavior.AllowGet)
        End Function

        Function DeleteDetail(ByVal id As Integer) As JsonResult
            Dim stat As Integer = 0
            Dim msg As String = ""
            Dim msgDesc As String = ""
            Try
                Dim data = (From pl In _purchaseEntities.DepartmentPRDetails Where pl.ID = id).FirstOrDefault()
                If data IsNot Nothing Then _purchaseEntities.DepartmentPRDetails.DeleteObject(data)
                _purchaseEntities.SaveChanges()
                stat = 1
                msg = "Success Deleted"
            Catch ex As Exception
                msg = "Got Exception"
                msgDesc = ex.Message
            End Try
            Return Json(New With {.stat = stat, .msg = msg, .msgDesc = msgDesc}, JsonRequestBehavior.AllowGet)
        End Function

        Function DepartmentPRDetailValidation(model As DepartmentPRDetail, ByVal rowIdx As Integer) As JsonResult
            If ModelState.IsValid Then
                Return Json(New With {.stat = 1, .model = New With {model.ID, model.ItemName, model.Allocation, model.Currency,
                                                                    model.EstUnitPrice, model.DepartmentPurchaseRequisitionId,
                                                                    model.Quantity, model.Remarks, model.TotalEstPrice, model.UnitQuantity}, rowIdx})
            End If
            Return Json(New With {.stat = 0, .errors = HaloUIHelpers.Helpers.JsonErrorObject.Convert(ModelState)}, JsonRequestBehavior.AllowGet)
        End Function

        Function GetDescription(ByVal term As String, ByVal isProject As String) As JsonResult
            Dim context As New PurchasingEntities

            Dim model = (From a In context.GetDescriptionPurchase(Val(isProject)) Where a.Name.Contains(term)
                         Select a.Name).ToList()
            Return Json(model, JsonRequestBehavior.AllowGet)
        End Function

        Public Sub New()
            currentUser = ERPBase.ErpUserProfile.GetUserProfile()
            _purchaseEntities = New PurchasingEntities
        End Sub

        <HttpGet()>
        Public Function PrintPR(ByVal Id As Integer) As ActionResult
            'Data Master
            Dim model = (From a As DepartmentPurchaseRequisition In _purchaseEntities.DepartmentPurchaseRequisitions
                         Where a.ID = Id
                         Select a).SingleOrDefault()

            If model Is Nothing Then
                Throw New HttpException(404, "NOT FOUND")
            End If
            'Data Detail
            Dim items = (From m In model.DepartmentPRDetails
                        Where m.DepartmentPurchaseRequisitionId = model.ID
                        Select m).ToList


            If items.Count < 20 Then
                For i As Integer = 1 To 20 - items.Count
                    items.Add(New DepartmentPRDetail With {.ID = 0})
                Next
            End If

            Dim r As New Microsoft.Reporting.WebForms.LocalReport
            r.ReportPath = Server.MapPath("~/Areas/Purchasing/Views/DepartmentPurchaseRequisition/DepartmentPurchasingRequest.rdlc")
            r.SetParameters(New ReportParameter("Id", Id))
            r.SetParameters(New ReportParameter("Tanggal", model.RequestDate))
            r.SetParameters(New ReportParameter("NoDokumen", model.RecordNo))
            r.SetParameters(New ReportParameter("TipePermintaan", model.RequestType.Name))
            r.SetParameters(New ReportParameter("TanggalKebutuhan", model.DeliveryDate))
            r.SetParameters(New ReportParameter("DikirimkanKe", model.DeliveryTo))
            r.SetParameters(New ReportParameter("Alamat", model.DeliveryAddress))
            r.SetParameters(New ReportParameter("DimintaOleh", model.RequestedBy_Name))
            r.SetParameters(New ReportParameter("Jabatan", model.RequestedBy_Occupation))

            r.SetParameters(New ReportParameter("ApprovedBy_Name", model.ApprovedBy_Name))
            r.SetParameters(New ReportParameter("ApprovedBy_Occupation", model.ApprovedBy_Occupation))
            r.SetParameters(New ReportParameter("KnownBy_Name", model.KnownBy_Name))
            r.SetParameters(New ReportParameter("KnownBy_Occupation", model.KnownBy_Occupation))

            r.DataSources.Add(New ReportDataSource("ItemList", items))

            r.Refresh()
            Dim reportType As String = "PDF"

            Dim devinfo As String = "<DeviceInfo>" +
            "<ColorDepth>32</ColorDepth><DpiX>350</DpiX><DpiY>350</DpiY><OutputFormat>PDF</OutputFormat>" +
            "  <PageWidth>8.5in</PageWidth>" +
            "  <PageHeight>11in</PageHeight>" +
            "  <MarginTop>0.2in</MarginTop>" +
            "  <MarginLeft>0.2in</MarginLeft>" +
            "  <MarginRight>0in</MarginRight>" +
            "  <MarginBottom>0in</MarginBottom>" +
            "</DeviceInfo>"

            Dim output() As Byte = r.Render(reportType, devinfo, mimeType, Encoding, fileNameExtension, streams, warnings)
            Return File(output, mimeType)
        End Function
    End Class
End Namespace
