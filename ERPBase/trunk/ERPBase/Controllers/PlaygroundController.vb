Namespace Controllers

    Public Class PlaygroundController
        Inherits System.Web.Mvc.Controller

        '
        ' GET: /Playground

        Function Index() As ActionResult
            Dim t As New TestModel

            Return View(t)
        End Function

        <HttpPost()>
        Public Function SaveData() As ActionResult
        
            If ModelState.IsValid Then

            End If
            Return View()
        End Function
        Public Function TestSaveData(ByVal model As TestModel) As ActionResult
            If ModelState.IsValid Then

            End If
            Return View("Index", model)
        End Function
    End Class
End Namespace