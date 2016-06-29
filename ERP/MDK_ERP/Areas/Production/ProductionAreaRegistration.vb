Namespace MDK_ERP.Areas.Production
    Public Class ProductionAreaRegistration
        Inherits AreaRegistration

        Public Overrides ReadOnly Property AreaName() As String
            Get
                Return "Production"
            End Get
        End Property

        Public Overrides Sub RegisterArea(ByVal context As System.Web.Mvc.AreaRegistrationContext)
            context.MapRoute( _
                "Production_default", _
               "Production/{controller}/{action}/{id}", _
                New With {.action = "Index", .controller = "Dashboard", .id = UrlParameter.Optional} _
            )
        End Sub
    End Class
End Namespace

