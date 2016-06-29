
Imports System.Collections.Generic
Imports System.Linq
Imports System.Web
Imports System.Web.Mvc

Namespace Controllers
    Public Class ErrorController
        Inherits Controller
        '
        ' GET: /Error/

        Public Function NotFound() As ActionResult
            ViewBag.referer = Request.QueryString("aspxerrorpath")
            Return View()
        End Function



        Public Function AccessDenied() As ActionResult

            Return View()
        End Function
        Public Function ServerError() As ActionResult
            Dim exception As Exception = Nothing
            If RouteData.Values.ContainsKey("exception") Then
                exception = DirectCast(RouteData.Values("exception"), Exception)
                ViewBag.StackTrace = exception.StackTrace
                ViewBag.ErrorMessage = exception.Message
                If exception.InnerException IsNot Nothing Then
                    ViewBag.InnerMessage = exception.InnerException.Message
                End If
            End If


            Return View()
        End Function


    End Class
End Namespace