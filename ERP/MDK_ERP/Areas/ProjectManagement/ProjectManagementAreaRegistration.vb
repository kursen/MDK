Namespace MDK_ERP.Areas.ProjectManagement
    Public Class ProjectManagementAreaRegistration
        Inherits AreaRegistration

        Public Overrides ReadOnly Property AreaName() As String
            Get
                Return "ProjectManagement"
            End Get
        End Property

        Public Overrides Sub RegisterArea(ByVal context As System.Web.Mvc.AreaRegistrationContext)
            context.MapRoute( _
                "ProjectManagement_default", _
               "ProjectManagement/{controller}/{action}/{id}", _
                New With {.action = "Index", .id = UrlParameter.Optional} _
            )
        End Sub
    End Class
End Namespace

