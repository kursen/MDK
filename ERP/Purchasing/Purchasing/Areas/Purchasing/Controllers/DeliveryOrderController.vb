Imports Microsoft.Reporting.WebForms
Namespace Purchasing.Areas.Purchasing.Controllers
    Public Class DeliveryOrderController
        Inherits System.Web.Mvc.Controller
        Private _purchaseEntities As New PurchasingEntities
        Private CurrentUser As ERPBase.ErpUserProfile

        Dim mimeType As String = "application/pdf"
        Dim Encoding As String = Nothing
        Dim fileNameExtension As String = Nothing
        Dim streams As String() = Nothing
        Dim warnings() As Microsoft.Reporting.WebForms.Warning = Nothing
        '
        ' GET: /Purchasing/DeliveryOrder

        Function Index() As ActionResult
            Return View()
        End Function

        Function Detail(ByVal Id As Integer) As ActionResult
            Dim model = _purchaseEntities.DeliveryOrders.Where(Function(d) d.ID = Id).FirstOrDefault
            ViewData("OrderNumber") = (From po In _purchaseEntities.PurchaseOrders Where po.ID = model.PurchaseOrderId Select po.OrderNumber).FirstOrDefault
            Return View(model)
        End Function

        Function DetailPO(ByVal Id As Integer) As ActionResult
            'Dim model = _purchaseEntities.DeliveryOrders.Where(Function(d) d.ID = Id).FirstOrDefault
            'If IsNothing(model) Then
            '    Return View(model)
            'End If
            Dim modelPO = _purchaseEntities.PurchaseOrders.Where(Function(d) d.ID = Id).FirstOrDefault

            Return View(modelPO)
        End Function

        Function Edit(ByVal Id As Integer) As ActionResult
            Dim model = _purchaseEntities.DeliveryOrders.Where(Function(d) d.ID = Id).FirstOrDefault
            ViewData("OrderNumber") = (From po In _purchaseEntities.PurchaseOrders Where po.ID = model.PurchaseOrderId Select po.OrderNumber).FirstOrDefault
            Return View("Create", model)
        End Function

        Public Function Create() As ActionResult
            Dim model As New DeliveryOrder
            model.ReceiptDate = Date.Today
            ViewData("OrderNumber") = ""
            Return View(model)
        End Function

        Public Function Delete(ByVal Id As Integer) As ActionResult
            Dim model = _purchaseEntities.DeliveryOrders.Where(Function(d) d.ID = Id).FirstOrDefault
            If IsNothing(model) Then
                ModelState.AddModelError("General", "Data yang mau dihapus tidak ditemukan")
                Return Json(New With {.stat = 0, .errors = HaloUIHelpers.Helpers.JsonErrorObject.Convert(ModelState)})
            End If
            Try
                _purchaseEntities.DeliveryOrders.DeleteObject(model)
                _purchaseEntities.SaveChanges()

                Return Json(New With {.stat = 1, .id = model.ID})
            Catch ex As Exception
                Return Json(New With {.stat = 0, .errors = HaloUIHelpers.Helpers.JsonErrorObject.Convert(ModelState)})
            End Try
        End Function

        <HttpPost()>
        <ValidateInput(False)>
        Public Function Save(ByVal model As DeliveryOrder, ByVal PurchaseOrderDetailId As List(Of Integer), ByVal Quantity As List(Of Integer)) As ActionResult
            If ModelState.IsValid Then
                'check Document Number is exist
                Dim docNoExist = _purchaseEntities.DeliveryOrders.Where(Function(d) d.DocNo = model.DocNo AndAlso d.ID <> model.ID).FirstOrDefault
                If docNoExist IsNot Nothing Then
                    ModelState.AddModelError("General", "Nomor dokumen ini sudah pernah tersimpan")
                    Return Json(New With {.stat = 0, .errors = HaloUIHelpers.Helpers.JsonErrorObject.Convert(ModelState)})
                End If
                'compare receipt date with purchase order date
                Dim purchaseOrderModel = _purchaseEntities.PurchaseOrders.Where(Function(po) po.ID = model.PurchaseOrderId).FirstOrDefault
                If IsNothing(purchaseOrderModel) Then
                    ModelState.AddModelError("General", "Data ini tidak memiliki hubungan dengan data permintaan")
                    Return Json(New With {.stat = 0, .errors = HaloUIHelpers.Helpers.JsonErrorObject.Convert(ModelState)})
                End If
                If model.ReceiptDate < purchaseOrderModel.OrderDate Then
                    ModelState.AddModelError("General", "Tanggal penerimaan lebih lama dari tanggal permintaan")
                    Return Json(New With {.stat = 0, .errors = HaloUIHelpers.Helpers.JsonErrorObject.Convert(ModelState)})
                End If

                For i = 0 To Quantity.Count - 1
                    If model.ID = 0 Then
                        model.DeliveryOrderDetails.Add(New DeliveryOrderDetail With {.DeliveryOrderId = model.ID,
                                                                                     .PurchaseOrderDetailId = PurchaseOrderDetailId(i),
                                                                                     .Quantity = Quantity(i)})
                    Else
                        Dim POId = PurchaseOrderDetailId(i)
                        Dim deliveryOrderDetail = (From dod In _purchaseEntities.DeliveryOrderDetails Where
                                                   dod.DeliveryOrderId = model.ID AndAlso
                                                   dod.PurchaseOrderDetailId = POId
                                                   Select dod).FirstOrDefault
                        If deliveryOrderDetail IsNot Nothing Then
                            deliveryOrderDetail.Quantity = Quantity(i)
                        End If
                    End If
                Next


                _purchaseEntities.DeliveryOrders.AddObject(model)
                If model.ID > 0 Then
                    _purchaseEntities.ObjectStateManager.ChangeObjectState(model, EntityState.Modified)
                End If

                _purchaseEntities.SaveChanges()
                Return Json(New With {.stat = 1, .id = model.ID})
            Else
                Return Json(New With {.stat = 0, .errors = HaloUIHelpers.Helpers.JsonErrorObject.Convert(ModelState)})
            End If
        End Function

        <HttpPost()>
        Public Function GetDOList() As ActionResult
            Dim model = (From d In _purchaseEntities.DeliveryOrders
                        Join po In _purchaseEntities.PurchaseOrders On po.ID Equals d.PurchaseOrderId
                        Where po.Archive = False
                        Select New With {d.ID, d.PurchaseOrderId, po.OrderNumber, d.ReceiptDate, d.DocNo, po.Vendor_CompanyName, d.ReceiptBy_Name,
                                         .Details = From dod In d.DeliveryOrderDetails
                                                    Select New With {.PurchaseOrderDetailId = dod.PurchaseOrderDetailId,
                                                                    .ItemName = dod.PurchaseOrderDetail.ItemName,
                                                                     .UnitQuantity = dod.PurchaseOrderDetail.UnitQuantity,
                                                                     .QuantityOrder = dod.PurchaseOrderDetail.Quantity,
                                                                     .QuantityReady = (From dd In _purchaseEntities.DeliveryOrderDetails
                                                                          Join dr In _purchaseEntities.DeliveryOrders On dd.DeliveryOrderId Equals dr.ID
                                                                          Where dr.PurchaseOrderId = d.PurchaseOrderId AndAlso
                                                                          dd.PurchaseOrderDetailId = dod.PurchaseOrderDetailId AndAlso
                                                                          dd.ID < dod.ID
                                                                          Select dd.Quantity).DefaultIfEmpty(0).Sum(),
                                                                     .QuantityReceipt = dod.Quantity}}).OrderBy(Function(m) m.PurchaseOrderId).ToList()





            Return Json(New With {.data = model})
        End Function

        Public Function GetPurchaseOrderNumber(ByVal term As String) As JsonResult
            Dim model = _purchaseEntities.PurchaseOrders.Where(Function(m) m.OrderNumber.Contains(term)).Select(Function(m) New With {.key = m.ID, .value = m.OrderNumber}).ToArray

            Return Json(model, JsonRequestBehavior.AllowGet)
        End Function

        Public Function GetPurchaseOrderItems(ByVal POId As Integer, ByVal DOId As Integer) As JsonResult
         

            Dim PoDetails = From p In _purchaseEntities.PurchaseOrderDetails
                            Where p.PurchaseOrderId = POId
                            Select p.ID, p.ItemName, p.Quantity, p.UnitQuantity

            Dim DOReceived = From d In _purchaseEntities.DeliveryOrderDetails
                             Where d.DeliveryOrder.PurchaseOrderId = POId
                             Group d By d.PurchaseOrderDetailId Into g = Group
            Dim rvalue As IQueryable
            If DOId = 0 Then

                If DOReceived.Count = 0 Then
                    rvalue = From p In PoDetails
                             Select New With {.ID = p.ID, .NamaItem = p.ItemName, .UnitQuantity = p.UnitQuantity, .QuantityOrder = p.Quantity,
                                                  .QuantityReady = 0,
                                                  .Quantity = 0}
                Else
                    rvalue = From p In PoDetails
                         Join r In DOReceived.DefaultIfEmpty()
                         On p.ID Equals r.PurchaseOrderDetailId
                                 Select New With {.ID = p.ID, .NamaItem = p.ItemName, .UnitQuantity = p.UnitQuantity, .QuantityOrder = p.Quantity,
                                                  .QuantityReady = r.g.Sum(Function(m) m.Quantity),
                                                  .Quantity = 0}
                End If

                

            Else
                Dim DoDetails = From d In _purchaseEntities.DeliveryOrderDetails
                                          Where d.DeliveryOrderId = DOId
                                          Select New With {.ID = If(d.ID, d.ID, 0), .Quantity = If(d.Quantity, d.Quantity, 0),
                                                           .PurchaseOrderDetailId = If(d.PurchaseOrderDetailId, d.PurchaseOrderDetailId, 0)}




                rvalue = From p In PoDetails
                                    Join d In DoDetails
                                    On p.ID Equals d.PurchaseOrderDetailId
                                    Join r In DOReceived
                                    On p.ID Equals r.PurchaseOrderDetailId
                                    Select New With {.ID = p.ID, .NamaItem = p.ItemName, .UnitQuantity = p.UnitQuantity, .QuantityOrder = p.Quantity,
                                                     .QuantityReady = r.g.Sum(Function(m) m.Quantity),
                                                     .Quantity = If(d.Quantity, d.Quantity, 0)}
            End If







            Return Json(rvalue, JsonRequestBehavior.AllowGet)
        End Function

        <HttpPost()>
        Function GetPOItems(ByVal Id As Integer) As JsonResult
            Dim models = From m In _purchaseEntities.PurchaseOrderDetails
                         Where m.PurchaseOrderId = Id
                         Select New With {.Id = m.ID, m.ItemName, m.PurchaseOrderId,
                                          m.Quantity, m.UnitQuantity,
                                          .QuantityReady = (From d In _purchaseEntities.DeliveryOrderDetails
                                           Where d.DeliveryOrder.PurchaseOrderId = m.PurchaseOrderId AndAlso
                                           d.PurchaseOrderDetailId = m.ID Select d.Quantity).DefaultIfEmpty(0).Sum()}
            Return Json(New With {.data = models})
        End Function

        <HttpPost()>
        Public Function SavePartial(ByVal pk As Integer, name As String, value As String) As ActionResult
            Dim model = _purchaseEntities.DeliveryOrders.Where(Function(m) m.ID = pk).SingleOrDefault()
            If model Is Nothing Then
                ModelState.AddModelError("General", "Data penerimaan barang tidak ditemukan. Muat ulang halaman ini")
                Return Json(New With {.stat = 0, .errors = HaloUIHelpers.Helpers.JsonErrorObject.Convert(ModelState)})
            End If
            Select Case name
                Case "ReceiptDate"
                    Dim d As Date
                    Date.TryParse(value, d)
                    model.GetType.GetProperty(name).SetValue(model, d, Nothing)
                Case Else
                    model.GetType.GetProperty(name).SetValue(model, value, Nothing)
            End Select

            _purchaseEntities.SaveChanges()
            Return Json(New With {.stat = 1})
        End Function

        <HttpPost()>
        Public Function SavePartialDetail(ByVal pk As Integer, name As String, value As String) As ActionResult
            Dim model = _purchaseEntities.DeliveryOrderDetails.Where(Function(m) m.ID = pk).SingleOrDefault()
            If model Is Nothing Then
                ModelState.AddModelError("General", "Data detail penerimaan barang tidak ditemukan. Muat ulang halaman ini")
                Return Json(New With {.stat = 0, .errors = HaloUIHelpers.Helpers.JsonErrorObject.Convert(ModelState)})
            End If
            Select Case name
                Case "Quantity" & pk
                    Dim i As Integer
                    Integer.TryParse(value, i)
                    model.GetType.GetProperty("Quantity").SetValue(model, i, Nothing)
                Case Else
                    model.GetType.GetProperty(name).SetValue(model, value, Nothing)
            End Select

            _purchaseEntities.SaveChanges()
            Return Json(New With {.stat = 1})
        End Function

        <HttpGet()>
        Public Function PrintDO(ByVal Id As Integer) As ActionResult
            Dim model = _purchaseEntities.DeliveryOrders.Where(Function(m) m.ID = Id).SingleOrDefault()
            If model Is Nothing Then
                Throw New HttpException(404, "NOT FOUND")
            End If

            Dim doDetails = From dod In _purchaseEntities.DeliveryOrderDetails
                            Where dod.DeliveryOrderId = Id
                                                    Select New With {
                                                                    .ItemName = dod.PurchaseOrderDetail.ItemName,
                                                                     .UnitQuantity = dod.PurchaseOrderDetail.UnitQuantity,
                                                                     .QuantityOrder = dod.PurchaseOrderDetail.Quantity,
                                                                     .QuantityReady = (From dd In _purchaseEntities.DeliveryOrderDetails
                                                                          Join dr In _purchaseEntities.DeliveryOrders On dd.DeliveryOrderId Equals dr.ID
                                                                          Where dr.PurchaseOrderId = dod.DeliveryOrder.PurchaseOrderId AndAlso
                                                                          dd.PurchaseOrderDetailId = dod.PurchaseOrderDetailId
                                                                          Select dd.Quantity).DefaultIfEmpty(0).Sum(),
                                                                     .Quantity = dod.Quantity}

            Dim r As New Microsoft.Reporting.WebForms.LocalReport
            r.ReportPath = Server.MapPath("~/Areas/Purchasing/Views/DeliveryOrder/DeliveryOrder.rdlc")
            r.SetParameters(New ReportParameter("NoDO", model.DocNo))
            r.SetParameters(New ReportParameter("NoPO", (From po In _purchaseEntities.PurchaseOrders Where po.ID = model.PurchaseOrderId Select po.OrderNumber).SingleOrDefault))
            r.SetParameters(New ReportParameter("ReceiptDate", model.ReceiptDate))
            r.SetParameters(New ReportParameter("ReceiptBy_Name", model.ReceiptBy_Name))

            r.DataSources.Add(New ReportDataSource("DsDeliveryOrder", doDetails))

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
            If term IsNot Nothing Then
                model = _purchaseEntities.ExecuteStoreQuery(Of DocumentSearchResult)("EXEC [prc].[SearchDO] @searchterm",
                                                                                     New SqlClient.SqlParameter("@searchterm", term)).ToArray()
            End If
            Return View(model)
        End Function
    End Class
End Namespace
