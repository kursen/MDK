Imports System.ComponentModel
Imports System.ComponentModel.DataAnnotations
Imports System.Globalization
<MetadataType(GetType(ModelMaintenanceHeavyEquipment))> _
Public Class MaintenanceHeavyEquipment

End Class
Public Class ModelMaintenanceHeavyEquipment
    Public Property IDHeavyEqp As Integer
    Public Property HM As Decimal
    Public Property [Operator] As String
    Public Property DateMaintenance As Date
    Public Property Trouble As String
    Public Property Cost As Integer
End Class
