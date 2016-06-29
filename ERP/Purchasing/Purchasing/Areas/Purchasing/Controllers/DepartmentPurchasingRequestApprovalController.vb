Namespace Purchasing.Areas.Purchasing.Controllers
    Public Class DepartmentPurchasingRequestApprovalController
        Inherits System.Web.Mvc.Controller
        Private _prcEntities As PurchasingEntities
        Function Index() As ActionResult
            Return View()
        End Function

        <HttpPost()>
        Function GetRequisitionList() As ActionResult
            Dim model = (From a In _prcEntities.DepartmentPurchaseRequisitions
                         Where a.DocState > 0 And a.Archive = False
                         Select New With {a.ID, a.RecordNo, a.RequestDate, .RequestTypeName = a.RequestType.Name,
                                          a.RequestedBy_Name, a.RequestedBy_Occupation, a.DocState})
            Dim list = model.ToList()
            Return Json(New With {.data = list})
        End Function
        Public Function Detail(ByVal id As Integer) As ActionResult
            Dim model = _prcEntities.DepartmentPurchaseRequisitions.Where(Function(m) m.ID = id).SingleOrDefault()
            If model Is Nothing Then
                Throw New HttpException(404, "NOT FOUND")
            End If
            Dim opList As New List(Of ERPBase.OptionItem)
            For i As Integer = 1 To 3
                opList.Add(New ERPBase.OptionItem With {.Text = GlobalArray.PurchaseRequisitionDocState(i), .Value = i.ToString()})
            Next



            ViewData("DocState") = New SelectList(opList, "Value", "Text", model.DocState)
            Return View(model)
        End Function

        <HttpPost()>
        Public Function UpdateDocState(ByVal docState As Integer, ByVal id As Integer, ByVal DocApproveRejectDate As Date?) As JsonResult
            If Not DocApproveRejectDate.HasValue Then
                ModelState.AddModelError("DocApproveRejectDate", "Tanggal Harus diisi.")
            End If
            If Not ModelState.IsValid Then
                Return Json(New With {.stat = 0, .errors = HaloUIHelpers.Helpers.JsonErrorObject.Convert(ModelState)})
            End If
            Dim model = _prcEntities.DepartmentPurchaseRequisitions.Where(Function(m) m.ID = id).SingleOrDefault()
            If model Is Nothing Then
                Throw New HttpException(404, "NOT FOUND")
            End If
            If model.RequestDate.CompareTo(DocApproveRejectDate) > 0 Then
                ModelState.AddModelError("DocApproveRejectDate", "Tanggal Konfirmasi persetujuan lebih lama dari Tanggal Dokumen Permintaan")
                Return Json(New With {.stat = 0, .errors = HaloUIHelpers.Helpers.JsonErrorObject.Convert(ModelState)})
            End If
            model.DocApproveRejectDate = DocApproveRejectDate
            model.DocState = docState

            _prcEntities.SaveChanges()
            Return Json(New With {.stat = 1})
        End Function
        Public Sub New()
            _prcEntities = New PurchasingEntities
        End Sub
    End Class
End Namespace
