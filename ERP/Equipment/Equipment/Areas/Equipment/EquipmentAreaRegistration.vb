Namespace Equipment.Areas.Equipment
    Public Class EquipmentAreaRegistration
        Inherits AreaRegistration

        Public Overrides ReadOnly Property AreaName() As String
            Get
                Return "Equipment"
            End Get
        End Property

        Public Overrides Sub RegisterArea(ByVal context As System.Web.Mvc.AreaRegistrationContext)
            context.MapRoute( _
                "Equipment_default", _
               "Equipment/{controller}/{action}/{id}", _
                New With {.action = "Index", .controller = "Home", .id = UrlParameter.Optional} _
            )
        End Sub
    End Class
End Namespace

