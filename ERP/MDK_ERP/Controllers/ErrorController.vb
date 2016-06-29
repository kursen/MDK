Namespace MDK_ERP
    Public Class ErrorController
        Inherits System.Web.Mvc.Controller

        '
        ' GET: /Error

        Function Index(ByVal ErrorMsg As String) As ActionResult
            ViewData("errTitle") = IIf(IsNothing(TempData("errTitle")), "Application Get Error!", TempData("errTitle"))
            If Not IsNothing(TempData("exeptionMsg")) Then
                ViewData("Exception") = TempData("exeptionMsg")
                Return View()
            Else
                Return RedirectToRoute("Default", New With {.Action = "Index", .Controller = "Home"})
            End If
        End Function

    End Class
End Namespace