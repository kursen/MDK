Namespace HRD.Areas.HRD
    Public Class HRDAreaRegistration
        Inherits AreaRegistration

        Public Overrides ReadOnly Property AreaName() As String
            Get
                Return "HRD"
            End Get
        End Property

        Public Overrides Sub RegisterArea(ByVal context As System.Web.Mvc.AreaRegistrationContext)
            context.MapRoute( _
                "HRD_default", _
               "HRD/{controller}/{action}/{id}", _
                New With {.action = "Index", .controller = "Home", .id = UrlParameter.Optional},
                namespaces:=New String() {"HRD.HRD.Areas.HRD.Controllers"}
            )
        End Sub
    End Class
End Namespace

