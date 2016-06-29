Imports Microsoft.Reporting.WebForms

Namespace Purchasing.Areas.Purchasing.Controllers
    Public Class ProjectPurchasingRequestController
        Inherits System.Web.Mvc.Controller
        Private _purchaseEntities As PurchasingEntities

        Dim mimeType As String = "application/pdf"
        Dim Encoding As String = Nothing
        Dim fileNameExtension As String = Nothing
        Dim streams As String() = Nothing
        Dim warnings() As Microsoft.Reporting.WebForms.Warning = Nothing
        '
        ' GET: /Purchasing/ProjectPurchasingRequest

        Function Index() As ActionResult
            Return View()
        End Function

        <HttpPost()>
        Function GetRequisitionList() As ActionResult
            Dim model = (From a In _purchaseEntities.ProjectPurchaseRequisitions
                           Where a.Archive = False
                         Select New With {a.ID, a.ProjectCode, a.ProjectTitle, a.RecordNo, a.RequestDate,
                                          a.RequestedBy_Name, a.RequestedBy_Occupation, a.DocState,
                                          .Details = From d In a.ProjectPurchaseRequisitionDetails
                                                     Select New With {d.ID, .ItemName = String.Concat(d.ItemName, " ", d.Brand), d.Quantity, d.UnitQuantity,
                                                                      .UnitPrice = d.EstUnitPrice, .TotalPrice = d.TotalEstPrice, d.Currency}})
            Dim list = model.ToList()
            Return Json(New With {.data = list})
        End Function

        Function Create()
            Dim model As New ProjectPurchaseRequisition
            model.RequestDate = Date.Today
            model.DeliveryDate = Date.Today.AddDays(7)


            Dim _mainEntities = New ERPBase.MainEntities
            Dim projectInfo = (From c In _purchaseEntities.ProjectInfo
                               Where c.Archive = False
                           Select New ERPBase.OptionItem With {.Text = c.ProjectTitle, .Value = c.ProjectCode}).ToList()
            ViewData("ProjectCode") = projectInfo



            'For detail 
            Dim currencyList = (From c In _purchaseEntities.CurrencyMaster
                            Select New With {.id = c.CurrencyName, .name = c.CurrencyName}).ToList()
            ViewData("Currency") = New SelectList(currencyList, "Name", "Name", 1)




            Return View(model)
        End Function


        Public Function Edit(ByVal Id As Integer) As ActionResult
            Dim model = _purchaseEntities.ProjectPurchaseRequisitions.Where(Function(m) m.ID = Id).SingleOrDefault()
            If model Is Nothing Then
                Throw New HttpException(404, "NOT FOUND")
            End If



            Dim _mainEntities = New ERPBase.MainEntities
            Dim projectInfo = (From c In _purchaseEntities.ProjectInfo
                               Where c.Archive = False
                           Select New ERPBase.OptionItem With {.Text = c.ProjectTitle, .Value = c.ProjectCode}).ToList()
            ViewData("ProjectCode") = projectInfo




            'For detail 
            Dim currencyList = (From c In _purchaseEntities.CurrencyMaster
                            Select New With {.id = c.CurrencyName, .name = c.CurrencyName}).ToList()
            ViewData("Currency") = New SelectList(currencyList, "Name", "Name", 1)
            Dim measureList = (From m In _purchaseEntities.Measure
                            Select New With {.id = m.Id, .name = m.MeasureName}).ToList()

            Dim opList As New List(Of ERPBase.OptionItem)
            For i As Integer = 0 To 1
                opList.Add(New ERPBase.OptionItem With {.Text = GlobalArray.PurchaseRequisitionDocState(i), .Value = i.ToString()})
            Next
            ViewData("DocState") = New SelectList(opList, "Value", "Text", model.DocState)

            Return View("Create", model)

        End Function

        Function SaveProjectPurchasingRequest(ByVal model As ProjectPurchaseRequisition, ByVal ItemPRDetail As String)



            If ModelState.IsValid Then
                If Not (model.DocState = 0 OrElse model.DocState = 1) Then
                    ModelState.AddModelError("General", "Persetujuan telah dilakukan. Data tidak dapat diubah.")
                    Return Json(New With {.stat = 0, .errors = HaloUIHelpers.Helpers.JsonErrorObject.Convert(ModelState)})
                End If

                If model.RequestDate.CompareTo(model.DeliveryDate) > 0 Then
                    ModelState.AddModelError("DeliveryDate", "Tanggal permintaan lebih lama dari tangggal kebutuhan")
                    Return Json(New With {.stat = 0, .errors = HaloUIHelpers.Helpers.JsonErrorObject.Convert(ModelState)})
                End If


                _purchaseEntities.ProjectPurchaseRequisitions.AddObject(model)
                If model.ID > 0 Then
                    _purchaseEntities.ObjectStateManager.ChangeObjectState(model, EntityState.Modified)
                End If

                Dim PRDetailList As New List(Of ProjectPurchaseRequisitionDetail)
                Newtonsoft.Json.JsonConvert.PopulateObject(ItemPRDetail, PRDetailList)

                If PRDetailList.Count <> 0 Then
                    For Each listPRDetail In PRDetailList
                        model.ProjectPurchaseRequisitionDetails.Add(listPRDetail)
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

        End Function
        Function ProjectPRDetailValidation(ByVal model As ProjectPurchaseRequisitionDetail, ByVal rowIdx As Integer) As ActionResult
            If ModelState.IsValid Then
                Return Json(New With {.stat = 1, .model = New With {model.ID, model.ItemName, model.Brand, model.Currency,
                                                                    model.EstUnitPrice, model.ProjectPurchaseRequisitionId,
                                                                    model.Quantity, model.Remarks, model.TotalEstPrice, model.UnitQuantity}, rowIdx})
            End If
            Return Json(New With {.stat = 0, .errors = HaloUIHelpers.Helpers.JsonErrorObject.Convert(ModelState)}, JsonRequestBehavior.AllowGet)
        End Function

        <HttpPost()>
        Function GetProjectRequestItems(ByVal Id As Integer) As ActionResult

            Dim model = _purchaseEntities.ProjectPurchaseRequisitions.Where(Function(m) m.ID = Id).SingleOrDefault()
            If model Is Nothing Then
                Throw New HttpException(404, "NOT FOUND")
            End If
            Dim items = From m In model.ProjectPurchaseRequisitionDetails
                        Select New With {m.ID, m.ItemName, m.Brand, m.Currency, m.EstUnitPrice, m.UnitQuantity, m.TotalEstPrice,
                                         m.Quantity, m.Remarks, m.ProjectPurchaseRequisitionId}

            Return Json(New With {.data = items.ToArray()})
        End Function

        <HttpPost()>
        Function DeleteRequestItem(ByVal Id As Integer) As ActionResult
            Dim model = _purchaseEntities.ProjectPurchaseRequisitionDetails.Where(Function(m) m.ID = Id).SingleOrDefault
            _purchaseEntities.DeleteObject(model)
            _purchaseEntities.SaveChanges()

            Return Json(New With {.stat = 1})
        End Function
        <HttpPost()>
        Function DeleteDocument(ByVal id As Integer) As ActionResult
            Dim model = _purchaseEntities.ProjectPurchaseRequisitions.Where(Function(m) m.ID = id).SingleOrDefault
            _purchaseEntities.DeleteObject(model)
            _purchaseEntities.SaveChanges()
            Return Json(New With {.stat = 1})
        End Function
        Public Function Detail(ByVal id As Integer) As ActionResult
            Dim model = _purchaseEntities.ProjectPurchaseRequisitions.Where(Function(m) m.ID = id).SingleOrDefault()
            If model Is Nothing Then
                Throw New HttpException(404, "NOT FOUND")
            End If

            Return View(model)
        End Function

        <HttpPost()>
        Public Function Archive(ByVal Id As Integer) As ActionResult
            Dim model = _purchaseEntities.ProjectPurchaseRequisitions.Where(Function(m) m.ID = Id).SingleOrDefault()
            If model IsNot Nothing Then
                model.Archive = True
                _purchaseEntities.SaveChanges()
            End If

            Return Json(New With {.stat = 1})
        End Function
        Public Function GetProposedGoodList(ByVal term As String) As ActionResult
            Return Json(New With {.data = ""})
        End Function

        Public Sub New()
            _purchaseEntities = New PurchasingEntities
        End Sub

        <HttpGet()>
        Public Function PrintPR(ByVal Id As Integer) As ActionResult
            'Data Master
            Dim model = _purchaseEntities.ProjectPurchaseRequisitions.Where(Function(m) m.ID = Id).SingleOrDefault()
            If model Is Nothing Then
                Throw New HttpException(404, "NOT FOUND")
            End If
            'Data Detail
            Dim items = (From m In model.ProjectPurchaseRequisitionDetails
                        Where m.ProjectPurchaseRequisitionId = model.ID
                        Select m).ToList

            If items.Count < 20 Then
                For i As Integer = 1 To 20 - items.Count
                    items.Add(New ProjectPurchaseRequisitionDetail With {.ID = 0})
                Next
            End If

            Dim r As New Microsoft.Reporting.WebForms.LocalReport
            r.ReportPath = Server.MapPath("~/Areas/Purchasing/Views/ProjectPurchasingRequest/ProjectPurchasingRequest.rdlc")
            r.SetParameters(New ReportParameter("Id", Id))
            r.SetParameters(New ReportParameter("Tanggal", model.RequestDate))
            r.SetParameters(New ReportParameter("NoDokumen", model.RecordNo))
            r.SetParameters(New ReportParameter("KodeProyek", model.ProjectCode))
            r.SetParameters(New ReportParameter("NoProyek", model.ProjectTitle))
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
