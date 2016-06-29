Imports Microsoft.Reporting.WebForms
Namespace Purchasing.Areas.Purchasing.Controllers
    Public Class PurchaseOrderController
        Inherits System.Web.Mvc.Controller
        Private _purchaseEntities As PurchasingEntities
        Private CurrentUser As ERPBase.ErpUserProfile
        Dim mimeType As String = "application/pdf"
        Dim Encoding As String = Nothing
        Dim fileNameExtension As String = Nothing
        Dim streams As String() = Nothing
        Dim warnings() As Microsoft.Reporting.WebForms.Warning = Nothing
        Function Index() As ActionResult

            Return View()
        End Function

        <HttpPost()>
        Public Function GetPOList() As ActionResult
            Dim model = From m In _purchaseEntities.PurchaseOrders
                        Where m.Archive = False
                        Select New With {m.ID, m.OrderNumber, m.OrderDate, m.Vendor_CompanyName, m.DocState,
                                         .Details = From d In m.PurchaseOrderDetails
                                                    Select New With {d.ItemName, d.UnitPrice, d.UnitQuantity, d.Quantity, d.TotalPrice}}

            Return Json(New With {.data = model})
        End Function

       
        <HttpPost()>
        Public Function UnArchive(ByVal Id As Integer) As ActionResult
            Dim model = _purchaseEntities.PurchaseOrders.Where(Function(m) m.ID = Id).SingleOrDefault()
            If model IsNot Nothing Then
                model.Archive = False

                _purchaseEntities.SaveChanges()
            End If

            Return Json(New With {.stat = 1})
        End Function
        <HttpPost()>
        Public Function Delete(ByVal id As Integer) As ActionResult


            Dim model = _purchaseEntities.PurchaseOrders.Where(Function(m) m.ID = id).SingleOrDefault()
            If model IsNot Nothing Then
                _purchaseEntities.DeleteObject(model)
                _purchaseEntities.SaveChanges()
            End If

            Return Json(New With {.stat = 1})
        End Function
        <HttpPost()>
        Public Function ClosePO(ByVal Id As Integer) As ActionResult

            ''check if all item is complete

            Dim poItems = From m In _purchaseEntities.PurchaseOrderDetails
                        Where m.PurchaseOrderId = Id
                        Select New With {.Id = m.ID, m.ItemName, m.PurchaseOrderId,
                                         m.Quantity, m.UnitQuantity,
                                         .QuantityReady = (From d In _purchaseEntities.DeliveryOrderDetails
                                          Where d.DeliveryOrder.PurchaseOrderId = m.PurchaseOrderId AndAlso
                                          d.PurchaseOrderDetailId = m.ID Select d.Quantity).DefaultIfEmpty(0).Sum()}

            Dim complete As Boolean = True
            For Each item In poItems
                complete = item.QuantityReady = item.Quantity And complete
            Next
            If Not complete Then
                ModelState.AddModelError("General", "Jumlah barang yang diterima belum lengkap!")
                Return Json(New With {.stat = 0, .errors = HaloUIHelpers.Helpers.JsonErrorObject.Convert(ModelState)})

            End If
            Dim model = _purchaseEntities.PurchaseOrders.Where(Function(m) m.ID = Id).SingleOrDefault()
            If model IsNot Nothing Then
                model.DocState = 2
                model.Archive = True
                _purchaseEntities.SaveChanges()
            End If

            Return Json(New With {.stat = 1})
        End Function

        Public Function Create() As ActionResult
            Dim model As New PurchaseOrder
            model.PPnAdded = True
            model.OrderDate = Date.Today
            Dim remarksTemplate = IO.File.ReadAllText(Server.MapPath("~/Areas/Purchasing/Views/PurchaseOrder/PORemarks.vbhtml"))
            model.Remarks = remarksTemplate
            PrepareForm(model)
            Return View("Form", model)
        End Function

        Private Sub PrepareForm(ByVal model As PurchaseOrder)
            Dim currencyList = (From c In _purchaseEntities.CurrencyMaster
                        Select New With {.id = c.CurrencyName, .name = c.CurrencyName}).ToList()
            ViewData("Currency") = New SelectList(currencyList, "Name", "Name", model.Currency)

            Dim lsRequestType As New List(Of ERPBase.OptionItem)
            lsRequestType.Add(New ERPBase.OptionItem With {.Text = "Pembelian Untuk Proyek", .Value = 0})
            lsRequestType.Add(New ERPBase.OptionItem With {.Text = "Pembelian Untuk Unit Kerja", .Value = 1})
            ViewData("POType") = New SelectList(lsRequestType, "Value", "Text", model.POType)

        End Sub


        Public Function Edit(ByVal Id As Integer) As ActionResult
            Dim model = _purchaseEntities.PurchaseOrders.Where(Function(m) m.ID = Id).Single()
            If model Is Nothing Then
                Throw New HttpException(404, "NOT FOUND")
            End If
            PrepareForm(model)
            Return View("Form", model)
        End Function

        <HttpPost()>
        <ValidateInput(False)>
        Public Function Save(ByVal model As PurchaseOrder, ByVal ItemPODetail As String) As ActionResult
            Try
                If ModelState.IsValid Then
                    Dim dataExist = _purchaseEntities.PurchaseOrders.Where(Function(po) po.OrderNumber = model.OrderNumber).FirstOrDefault
                    If dataExist IsNot Nothing AndAlso dataExist.ID <> model.ID Then
                        ModelState.AddModelError("General", "No. PO sudah pernah terdaftar")
                        Return Json(New With {.stat = -0, .errors = HaloUIHelpers.Helpers.JsonErrorObject.Convert(ModelState)})
                    End If
                    If Not (model.DocState = 0 OrElse model.DocState = 1) Then
                        ModelState.AddModelError("General", "Persetujuan telah dilakukan. Data tidak dapat diubah.")
                        Return Json(New With {.stat = 0, .errors = HaloUIHelpers.Helpers.JsonErrorObject.Convert(ModelState)})
                    End If

                    ModelState.AddModelError("DeliveryDate", "Tanggal Kebutuhan lebih lama dari Tanggal PO")

                    _purchaseEntities.PurchaseOrders.AddObject(model)
                    If model.ID > 0 Then
                        _purchaseEntities.ObjectStateManager.ChangeObjectState(model, EntityState.Modified)
                    End If

                    Dim PRDetailList As New List(Of PurchaseOrderDetail)
                    Newtonsoft.Json.JsonConvert.PopulateObject(ItemPODetail, PRDetailList)

                    If PRDetailList.Count <> 0 Then
                        For Each listPRDetail In PRDetailList
                            model.PurchaseOrderDetails.Add(listPRDetail)
                            If listPRDetail.ID > 0 Then
                                _purchaseEntities.ObjectStateManager.ChangeObjectState(listPRDetail, EntityState.Modified)
                            End If
                        Next
                    Else
                        ModelState.AddModelError("General", "Daftar Item yang akan dibeli belum diisi.")
                        Return Json(New With {.stat = 0, .errors = HaloUIHelpers.Helpers.JsonErrorObject.Convert(ModelState)})
                    End If

                    _purchaseEntities.SaveChanges()
                    Return Json(New With {.stat = 1, .id = model.ID})
                Else
                    Return Json(New With {.stat = 0, .errors = HaloUIHelpers.Helpers.JsonErrorObject.Convert(ModelState)})
                End If
            Catch ex As Exception
                ModelState.AddModelError("General", ex.Message)
                Return Json(New With {.stat = -0, .errors = HaloUIHelpers.Helpers.JsonErrorObject.Convert(ModelState)})
            End Try
        End Function

        <HttpPost()>
        Public Function SearchRequisition(ByVal term As String, ByVal ReqType As Integer) As ActionResult

            Select Case ReqType
                Case 0 'Pembelian Proyek
                    Dim models = (From m In _purchaseEntities.ProjectPurchaseRequisitions
                                            Where m.RecordNo.Contains(term) AndAlso m.DocState = 2 AndAlso m.Archive = False
                                            Select New With {m.ID, m.RecordNo, m.DeliveryTo, m.DeliveryDate, m.RequestedBy_Name,
                                                             m.DeliveryAddress, .Description = "Permintaan untuk proyek"}).ToArray()

                    Return Json(models)

                Case 1 ' Pembelian departement


                    Dim models = (From m In _purchaseEntities.DepartmentPurchaseRequisitions
                                          Where m.RecordNo.Contains(term) AndAlso m.DocState = 2 AndAlso m.Archive = False
                                           Select New With {m.ID, m.RecordNo, m.DeliveryTo, m.DeliveryDate, m.RequestedBy_Name,
                                                             m.DeliveryAddress, .Description = "Permintaan " & m.RequestType.Name}).ToArray()

                    Return Json(models)

            End Select
            Return Nothing
        End Function

        <HttpPost()>
        Public Function GetRequisitionItem(ByVal ReqNo As String, ByVal ReqType As Integer) As ActionResult
           
            Dim models = (From m In _purchaseEntities.DepartmentPRDetails
                                  Where m.DepartmentPurchaseRequisition.RecordNo = ReqNo
                                  Select New With {.Id = 0, .ItemName = m.ItemName, .PurchaseOrderId = 0, .DT_RowId = "row_" & m.ID, .PRDetailId = m.ID,
                                                     m.Quantity, m.UnitQuantity, .UnitPrice = m.EstUnitPrice,
                                                     .TotalPrice = m.TotalEstPrice})


                    Return Json(models)

        End Function

        <HttpPost()>
        Public Function PODetailValidation(ByVal model As PurchaseOrderDetail, ByVal rowIdx As Integer, ByVal Currency As String) As ActionResult
            If ModelState.IsValid Then
                Return Json(New With {.stat = 1, .model = New With {.Id = model.ID, model.ItemName, .Currency = Currency, .DT_RowId = "row_" & model.PRDetailId,
                                                                    model.UnitPrice, model.PurchaseOrderId, model.PRDetailId,
                                                                    model.Quantity, .TotalPrice = model.UnitPrice * model.Quantity, model.UnitQuantity}, rowIdx})
            End If
            Return Json(New With {.stat = 0, .errors = HaloUIHelpers.Helpers.JsonErrorObject.Convert(ModelState)}, JsonRequestBehavior.AllowGet)
        End Function

        Function Detail(ByVal id As Integer) As ActionResult
            Dim model = _purchaseEntities.PurchaseOrders.Where(Function(m) m.ID = id).SingleOrDefault()
            If model Is Nothing Then
                Throw New HttpException(404, "NOT FOUND")
            End If

            Return View(model)
        End Function

        <HttpPost()>
        Function GetPOItems(ByVal Id As Integer) As JsonResult

            Dim models = From m In _purchaseEntities.PurchaseOrderDetails
                         Where m.PurchaseOrderId = Id
                         Select New With {.Id = m.ID, m.ItemName, m.PurchaseOrderId, .Currency = m.PurchaseOrder.Currency,
                                          .DT_RowId = "row_" & m.PRDetailId, m.PRDetailId,
                                          m.Quantity, m.TotalPrice, m.UnitPrice, m.UnitQuantity}
            Return Json(New With {.data = models})
        End Function

        <HttpPost()>
        Public Function GetVendorInfo(ByVal term As String) As ActionResult
            Dim model = _purchaseEntities.Vendor _
                        .Where(Function(m) m.Name.Contains(term)) _
                        .Select(Function(s) New With {s.Name, s.Number, s.Id, s.Phone, s.ContactName, s.Address, s.City, s.Province}) _
            .ToArray()

            Return Json(model)
        End Function
        ''' <summary>
        ''' Get vendor price based on the purchase requisition
        ''' </summary>
        ''' <param name="rqnumber">purchase requistionid</param>
        ''' <param name="VendorId">vendorid</param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        <HttpPost()>
        Public Function GetVendorPriceComparison(ByVal rqnumber As String, ByVal VendorId As Integer) As JsonResult
            rqnumber = rqnumber.Trim()
            Dim modeldetail() As Object = Nothing
            Dim model = (From m In _purchaseEntities.PriceComparison
                        Where m.DepartmentPurchaseRequisition.RecordNo = rqnumber).SingleOrDefault
            If model IsNot Nothing Then
                'check vendor 
                If model.VendorID1 = VendorId Then
                    modeldetail = (From m In model.PriceComparisonDetail
                                      Select New With {m.PRDetailID, .Price = m.Price1}).ToArray()
                ElseIf model.VendorID2 = VendorId Then
                    modeldetail = (From m In model.PriceComparisonDetail
                                    Select New With {m.PRDetailID, .Price = m.Price2}).ToArray()
                ElseIf model.VendorID3 = VendorId Then
                    modeldetail = (From m In model.PriceComparisonDetail
                                  Select New With {m.PRDetailID, .Price = m.Price3}).ToArray()
                End If
            End If
            Return Json(modeldetail)

        End Function

        <HttpGet()>
        Public Function PrintPO(ByVal Id As Integer) As ActionResult
            Dim model = _purchaseEntities.PurchaseOrders.Where(Function(m) m.ID = Id).SingleOrDefault()
            If model Is Nothing Then
                Throw New HttpException(404, "NOT FOUND")
            End If

            Dim poDetails = From d In model.PurchaseOrderDetails
                            Select New With {d.ItemName, d.Quantity, d.UnitQuantity, d.UnitPrice, d.TotalPrice, .Currency = d.PurchaseOrder.Currency}

            Dim r As New Microsoft.Reporting.WebForms.LocalReport
            r.ReportPath = Server.MapPath("~/Areas/Purchasing/Views/PurchaseOrder/PurchaseOrder.rdlc")
            r.SetParameters(New ReportParameter("OrderNumber", model.OrderNumber))
            r.SetParameters(New ReportParameter("OrderDate", model.OrderDate))
            r.SetParameters(New ReportParameter("QuotationRef", model.QuotationRef))
            r.SetParameters(New ReportParameter("ContactPerson_Name", model.ContactPerson_Name))
            r.SetParameters(New ReportParameter("ContactPerson_Phone", model.ContactPerson_Phone))
            r.SetParameters(New ReportParameter("TermOfPayment", model.TermOfPayment))
            r.SetParameters(New ReportParameter("DeliveryDate", model.DeliveryDate))
            r.SetParameters(New ReportParameter("DeliveryTo", model.DeliveryTo))
            r.SetParameters(New ReportParameter("DeliveryAddress", model.DeliveryAddress))
            r.SetParameters(New ReportParameter("PreparedBy_Name", model.PreparedBy_Name))
            r.SetParameters(New ReportParameter("ApprovedBy_Name", model.ApprovedBy_Name))
            r.SetParameters(New ReportParameter("Remarks", model.Remarks))
            r.SetParameters(New ReportParameter("Currency", model.Currency))
            r.SetParameters(New ReportParameter("Vendor_CompanyName", model.Vendor_CompanyName))
            r.SetParameters(New ReportParameter("Vendor_ContactName", model.Vendor_ContactName))
            r.SetParameters(New ReportParameter("Vendor_Phone", model.Vendor_Phone))
            r.SetParameters(New ReportParameter("Vendor_Address", model.Vendor_Address))
            r.SetParameters(New ReportParameter("Vendor_City", model.Vendor_City))
            r.SetParameters(New ReportParameter("Vendor_Province", model.Vendor_Province))
            r.SetParameters(New ReportParameter("PPnAdded", model.PPnAdded))

            r.DataSources.Add(New ReportDataSource("PoDetails", poDetails))

            r.Refresh()
            Dim reportType As String = "PDF"

            Dim devinfo As String = "<DeviceInfo>" +
            "<ColorDepth>32</ColorDepth><DpiX>350</DpiX><DpiY>350</DpiY><OutputFormat>PDF</OutputFormat>" +
            "  <PageWidth>8.27in</PageWidth>" +
            "  <PageHeight>11.69in</PageHeight>" +
            "  <MarginTop>0.5in</MarginTop>" +
            "  <MarginLeft>0.5in</MarginLeft>" +
            "  <MarginRight>0in</MarginRight>" +
            "  <MarginBottom>0in</MarginBottom>" +
            "</DeviceInfo>"

            Dim output() As Byte = r.Render(reportType, devinfo, mimeType, Encoding, fileNameExtension, streams, warnings)

            Return File(output, mimeType)
        End Function

        Public Function Search(Optional ByVal term As String = Nothing) As ActionResult
            Dim model As DocumentSearchResult() = Nothing

            If term IsNot Nothing AndAlso String.IsNullOrWhiteSpace(term) = False Then
                model = _purchaseEntities.ExecuteStoreQuery(Of DocumentSearchResult)("EXEC [prc].[SearchPO] @searchterm",
                                                                                     New SqlClient.SqlParameter("@searchterm", term)).ToArray()

            Else
                model = New DocumentSearchResult() {}

            End If



            Return View(model)
        End Function


        Public Sub New()
            _purchaseEntities = New PurchasingEntities
            CurrentUser = ERPBase.ErpUserProfile.GetUserProfile()
        End Sub
    End Class
End Namespace
