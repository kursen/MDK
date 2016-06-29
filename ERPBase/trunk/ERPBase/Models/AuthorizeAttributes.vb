<AttributeUsage(AttributeTargets.[Class] Or AttributeTargets.Method, Inherited:=True, AllowMultiple:=True)> _
Public Class AuthorizeAttribute
    Inherits System.Web.Mvc.AuthorizeAttribute
    Protected Overrides Sub HandleUnauthorizedRequest(filterContext As System.Web.Mvc.AuthorizationContext)
        If filterContext.HttpContext.Request.IsAuthenticated Then

            'filterContext.Result = new System.Web.Mvc.HttpStatusCodeResult((int)System.Net.HttpStatusCode.Forbidden);
            filterContext.Result = New System.Web.Mvc.ViewResult() With {.ViewName = "AccessDenied"}
        Else
            If filterContext.HttpContext.Request.IsAjaxRequest Then
                filterContext.Result = New System.Web.Mvc.ViewResult() With {.ViewName = "PleaseLogin"}
            Else
                MyBase.HandleUnauthorizedRequest(filterContext)
            End If

        End If
    End Sub
    Public Overrides Sub OnAuthorization(filterContext As System.Web.Mvc.AuthorizationContext)
        MyBase.OnAuthorization(filterContext)
        If (TypeOf filterContext.Result Is HttpUnauthorizedResult) Then

        End If
    End Sub
End Class

