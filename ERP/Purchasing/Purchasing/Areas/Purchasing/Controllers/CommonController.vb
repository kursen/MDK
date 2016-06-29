Namespace Purchasing.Areas.Purchasing.Controllers
    Public Class CommonController
        Inherits System.Web.Mvc.Controller
        Private _prcEntities As PurchasingEntities
        '

        <HttpPost()>
        Function UnitQuantityName(ByVal query As String) As ActionResult
            Dim measureList = (From m In _prcEntities.Measure
                               Where m.MeasureName.StartsWith(query)
                               Select m.MeasureName).ToArray()
            If measureList.Count = 0 Then
                Return Json(String.Empty)
            End If
            Return Json(measureList)
        End Function

        <HttpPost()>
        Function GlobalEmployeeNames(ByVal term As String) As ActionResult
            Dim EmployeeNames = _prcEntities.ExecuteStoreQuery(Of ERPBase.OptionItem)("SELECT Fullname as Text, Occupation AS Value FROM [prc].[GetEmployees](@term)",
                                                                                      New SqlClient.SqlParameter("@term", term))

            Return Json(EmployeeNames)
        End Function


        Public Sub New()
            _prcEntities = New PurchasingEntities
        End Sub
    End Class
End Namespace
