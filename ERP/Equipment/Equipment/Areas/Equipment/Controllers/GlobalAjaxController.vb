Namespace Equipment.Areas.Equipment.Controllers
    Public Class GlobalAjaxController
        Inherits System.Web.Mvc.Controller
        Private _eqpEntities As EquipmentEntities
        Dim currentUser As ERPBase.ErpUserProfile
        <HttpPost()>
        Function AutocompleteHeavyEqp(ByVal term As String) As JsonResult

            Dim model = (From vhc In _eqpEntities.HeavyEqps Where (vhc.Code.Contains(term) OrElse
                         vhc.Merk.Contains(term) OrElse
                         vhc.Type.Contains(term) OrElse
                         vhc.Species.Contains(term)) And vhc.IDArea = currentUser.WorkUnitId
                                              Select New With {vhc.ID, vhc.Merk, vhc.Type, vhc.Code, vhc.Species, vhc.IDArea,
                                                               vhc.IDOpr, vhc.OprName}).ToList




            Return Json(model)
        End Function
        <HttpPost()> _
        Function AutocompleteVehicle(ByVal term As String) As JsonResult

            Dim model = (From vhc In _eqpEntities.Vehicles Where (vhc.Code.Contains(term) OrElse
                         vhc.Merk.Contains(term) OrElse
                         vhc.Type.Contains(term) OrElse
                         vhc.PoliceNumber.Contains(term) OrElse
                         vhc.Species.Contains(term)) AndAlso vhc.IDArea = currentUser.WorkUnitId
                                              Select New With {vhc.ID, vhc.Merk, vhc.Type, vhc.Code, vhc.Species, vhc.IDArea, vhc.PoliceNumber,
                                                               vhc.IDDriver, vhc.DriverName}).ToList

            Return Json(model)
        End Function

        <HttpPost()>
        Function AutocompleteMachine(ByVal term As String) As JsonResult

            Dim model = (From mcn In _eqpEntities.MachineEqps Where mcn.Name.Contains(term) OrElse
                         mcn.SerialNumber.Contains(term) OrElse
                         mcn.Merk.Contains(term) OrElse
                         mcn.Type.Contains(term) AndAlso
                         mcn.IDArea = currentUser.WorkUnitId
                                              Select New With {mcn.ID, mcn.Name, mcn.SerialNumber, mcn.Merk, mcn.Type,
                                                               mcn.Capacity, mcn.Remark, mcn.IDArea}).ToList

            Return Json(model)
        End Function

        <HttpPost()>
        Function AutocompleteUnitQuantity(ByVal term As String) As JsonResult
            Dim model = _eqpEntities.ExecuteStoreQuery(Of ERPBase.OptionItem)("EXEC Eqp.QuantityUnit @term", New SqlClient.SqlParameter("@term", term)).ToArray()
            If model.Count = 0 Then
                Return Json(String.Empty)
            End If
            Return Json(model)
        End Function

        <HttpGet()>
        Function AllUnitQuantity() As JsonResult
            Dim model = _eqpEntities.ExecuteStoreQuery(Of ERPBase.OptionItem)("EXEC Eqp.[AllQuantityUnit]").ToArray()
            If model.Count = 0 Then
                Return Json(String.Empty)
            End If
            Return Json(model, behavior:=JsonRequestBehavior.AllowGet)
        End Function

        <HttpPost()>
        Function AutocompleteGoods(ByVal term As String) As JsonResult
            Dim model = _eqpEntities.ExecuteStoreQuery(Of ERPBase.OptionItem)("EXEC Eqp.GoodList @term", New SqlClient.SqlParameter("@term", term)).ToArray()
            If model.Count = 0 Then
                Return Json(String.Empty)
            End If
            Return Json(model.Select(Function(m) m.Text))
        End Function
        Function autoCompleteItemMaintenance(ByVal term As String) As JsonResult
            Dim model = _eqpEntities.AutocompleteItemMaintenance(term).ToList
            Return Json(model, JsonRequestBehavior.AllowGet)
        End Function
        Public Sub New()
            _eqpEntities = New EquipmentEntities
            currentUser = ERPBase.ErpUserProfile.GetUserProfile
        End Sub
    End Class
End Namespace
