Namespace MDK_ERP.Areas.Production.Controllers
    Public Class ScheduleController
        Inherits System.Web.Mvc.Controller

        '
        ' GET: /Production/Schedule

        'default schedule shows production schedule
        Function Index() As ActionResult
            Return View()
        End Function


        Function RawMaterialRequest() As ActionResult
            Return View()
        End Function

        Function Delivery() As ActionResult
            Return View()
        End Function
    End Class
End Namespace
