Namespace Purchasing.Areas.Purchasing.Controllers
    Public Class HomeController
        Inherits System.Web.Mvc.Controller
        Private _prcEntities As PurchasingEntities
        '
        ' GET: /Purchasing/Home

        Function Index() As ActionResult
            Return View()
        End Function


        Function RequestStatistic() As JsonResult

            Dim NumberOfUnApprovedDepartementRequesition = (From m In _prcEntities.DepartmentPurchaseRequisitions
                                            Where m.DocState = 1).Count

            Dim NumberOfUnApprovedProjectRequesition = (From m In _prcEntities.ProjectPurchaseRequisitions
                                                        Where m.DocState = 1).Count


            Dim NumbeberOfPOInProcess = (From m In _prcEntities.PurchaseOrders
                                         Where m.DocState = 1).Count
            Return Json(New With {NumberOfUnApprovedDepartementRequesition, NumberOfUnApprovedProjectRequesition})
        End Function

        Public Sub New()
            _prcEntities = New PurchasingEntities
        End Sub
    End Class
End Namespace
