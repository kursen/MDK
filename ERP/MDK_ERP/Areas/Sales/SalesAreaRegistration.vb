Namespace MDK_ERP.Areas.Sales
    Public Class SalesAreaRegistration
        Inherits AreaRegistration

        Public Overrides ReadOnly Property AreaName() As String
            Get
                Return "Sales"
            End Get
        End Property

        Public Overrides Sub RegisterArea(ByVal context As System.Web.Mvc.AreaRegistrationContext)
            context.MapRoute( _
                "Sales_default", _
               "Sales/{controller}/{action}/{id}", _
                New With {.controller = "SalesHome", .action = "Index", .id = UrlParameter.Optional} _
            )
        End Sub
    End Class
End Namespace

