Namespace MDK_ERP.Controllers

    <Authorize()> _
    Public Class HomeController
        Inherits System.Web.Mvc.Controller

        Function Index() As ActionResult
            Return RedirectToRoute("Production_default", New With {.controller = "Dashboard", .action = "Index"})
            Return View()
        End Function
    End Class

End Namespace

