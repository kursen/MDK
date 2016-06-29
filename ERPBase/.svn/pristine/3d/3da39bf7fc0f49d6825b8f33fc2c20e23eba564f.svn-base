Namespace Controllers


    Public Class HomeController
        Inherits System.Web.Mvc.Controller

        <Authorize()>
        Function Index() As ActionResult

            Return View()
        End Function

        <Authorize()>
        Function setDisplayState(displaystate As Boolean) As ActionResult
            'Dim p = ERPBase.Profile
            Dim p = ProfileBase.Create(User.Identity.Name)
            p.SetPropertyValue("MenuDisplayState", displaystate)
            p.Save()


            Return (Json(New With {.set = True}))

        End Function
    End Class
End Namespace