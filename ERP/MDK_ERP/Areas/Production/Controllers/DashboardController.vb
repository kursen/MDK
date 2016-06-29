Namespace MDK_ERP.Areas.Production.Controllers

    <Authorize()> _
    Public Class DashboardController
        Inherits BaseController

        '
        ' GET: /Production/Dashboard

        Function Index() As ActionResult
            Return View()
        End Function

        Function GetAMPData() As JsonResult
            Dim model = ctx.GetAMPDataForDashboard(Now.Date).ToList()
            Return Json(model, JsonRequestBehavior.AllowGet)
        End Function

        Function GetInventoryData() As JsonResult
            Dim model = ctx.GetInventoryDataForDashboard(Now.Date).ToList()
            Return Json(model, JsonRequestBehavior.AllowGet)
        End Function
    End Class
End Namespace