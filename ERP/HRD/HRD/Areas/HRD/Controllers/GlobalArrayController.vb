Namespace HRD.Areas.HRD.Controllers
    Public Class GlobalArrayController
        Inherits System.Web.Mvc.Controller
        Private _hrdEntities As HrdEntities
        '
        ' GET: /HRD/GlobalArray

        Function Index() As ActionResult
            Return View()
        End Function


        Public Function AutoCompleteEmployeeForOffice(ByVal term As String) As JsonResult
            Dim p = ERPBase.ErpUserProfile.GetUserProfile()
            Dim model = _hrdEntities.ExecuteStoreQuery(Of EmployeeSearchResult)("Exec  HRD.SearchEmployee @name, @officeid",
                                                                                New SqlClient.SqlParameter("@name", term),
                                                                                New SqlClient.SqlParameter("@officeId", p.WorkUnitId))

            Return Json(model, JsonRequestBehavior.AllowGet)
        End Function
        Public Sub New()
            _hrdEntities = New HrdEntities
        End Sub
    End Class
End Namespace
