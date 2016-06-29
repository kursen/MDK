' Note: For instructions on enabling IIS6 or IIS7 classic mode, 
' visit http://go.microsoft.com/?LinkId=9394802

Public Class MvcApplication
    Inherits System.Web.HttpApplication

    Shared Sub RegisterGlobalFilters(ByVal filters As GlobalFilterCollection)
        filters.Add(New HandleErrorAttribute())
    End Sub

    Shared Sub RegisterRoutes(ByVal routes As RouteCollection)
        routes.IgnoreRoute("{resource}.axd/{*pathInfo}")

        ' MapRoute takes the following parameters, in order:
        ' (1) Route name
        ' (2) URL with parameters
        ' (3) Parameter defaults
        routes.MapRoute( _
            "Default", _
            "{controller}/{action}/{id}", _
            New With {.controller = "Home", .action = "Index", .id = UrlParameter.Optional},
            namespaces:=New String() {"ERPBase.Controllers"} _
        )

    End Sub

    Sub Application_Start()
        AreaRegistration.RegisterAllAreas()

        RegisterGlobalFilters(GlobalFilters.Filters)
        RegisterRoutes(RouteTable.Routes)
        System.Threading.Thread.CurrentThread.CurrentUICulture = New System.Globalization.CultureInfo("id-ID")
        System.Threading.Thread.CurrentThread.CurrentCulture = New System.Globalization.CultureInfo("id-ID")
        ModelBinders.Binders.Add(GetType(Decimal), New HaloUIHelpers.Helpers.DecimalModelBinder())
        ModelBinders.Binders.Add(GetType(Double), New HaloUIHelpers.Helpers.DoubleModelBinder())
        ModelBinders.Binders.Add(GetType(Single), New HaloUIHelpers.Helpers.SingleModelBinder())
        ModelBinders.Binders.Add(GetType(Integer), New HaloUIHelpers.Helpers.IntegerModelBinder())
        ModelBinders.Binders.Add(GetType(Long), New HaloUIHelpers.Helpers.LongModelBinder())

    End Sub

    Protected Sub Application_Error(sender As Object, e As EventArgs)
        Dim app = DirectCast(sender, MvcApplication)
        Dim context = app.Context
        Dim ex = app.Server.GetLastError()
        context.Response.Clear()
        context.ClearError()
        Dim httpException = TryCast(ex, HttpException)

        Dim routeData = New RouteData()
        routeData.Values("controller") = "Error"
        routeData.Values("exception") = ex
        routeData.Values("action") = "ServerError"
        If httpException IsNot Nothing Then
            Select Case httpException.GetHttpCode()
                Case 401
                    routeData.Values("action") = "unauthorized"
                    Exit Select
                Case 404
                    routeData.Values("action") = "NotFound"
                    Exit Select
                Case 403
                    routeData.Values("action") = "AccessDenied"
                    Exit Select
                Case 500
                    routeData.Values("action") = "ServerError"
                    Exit Select
            End Select
        End If
        Dim controller As IController = New Controllers.ErrorController
        controller.Execute(New RequestContext(New HttpContextWrapper(context), routeData))
    End Sub

End Class
