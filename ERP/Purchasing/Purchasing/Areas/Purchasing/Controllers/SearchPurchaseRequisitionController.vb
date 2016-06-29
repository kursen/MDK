Namespace Purchasing.Areas.Purchasing.Controllers
    Public Class SearchPurchaseRequisitionController
        Inherits System.Web.Mvc.Controller
        Private _purchaseEntities As PurchasingEntities
        Private CurrentUser As ERPBase.ErpUserProfile
        '
        ' GET: /Purchasing/SearchPurchaseRequisition

        Function Index(Optional SearchOpt As Integer = 1, Optional ByVal term As String = Nothing) As ActionResult
            Dim model As DocumentSearchResult() = Nothing
            'Dim prStaffOrManager = User.IsInRole("Purchasing.Staff") OrElse User.IsInRole("Purchasing.Manager")
            If term IsNot Nothing Then
                model = _purchaseEntities.ExecuteStoreQuery(Of DocumentSearchResult)("EXEC [prc].[SearchPR] @searchterm, @searchOpt, @currentUser",
                                                                                     New SqlClient.SqlParameter("@searchterm", term),
                                                                                     New SqlClient.SqlParameter("@searchOpt", SearchOpt),
                                                                                     New SqlClient.SqlParameter("@currentUser", CurrentUser.WorkUnitId)
                                                                                     ).ToArray()
            End If

            ViewData("SearchOpt") = SearchOpt
            Return View(model)
        End Function

        Public Sub New()
            _purchaseEntities = New PurchasingEntities
            CurrentUser = ERPBase.ErpUserProfile.GetUserProfile()
        End Sub
    End Class
End Namespace
