Namespace Purchasing.Areas.Purchasing
    Public Class PurchasingAreaRegistration
        Inherits AreaRegistration

        Public Overrides ReadOnly Property AreaName() As String
            Get
                Return "Purchasing"
            End Get
        End Property

        Public Overrides Sub RegisterArea(ByVal context As System.Web.Mvc.AreaRegistrationContext)
            context.MapRoute( _
                "Purchasing_default", _
               "Purchasing/{controller}/{action}/{id}", _
                New With {.action = "Index", .controller = "Home", .id = UrlParameter.Optional}, _
                namespaces:=New String() {"Purchasing.Purchasing.Areas.Purchasing.Controllers"}
            )
        End Sub
    End Class
End Namespace

