Imports System.ComponentModel
Imports System.ComponentModel.DataAnnotations
<MetadataType(GetType(ModelFuelHeavyEqp))>
Public Class FuelUsedHeavyEqp

End Class
Public Class ModelFuelHeavyEqp
    Public Property IDActivity As Integer
    <Required(ErrorMessage:="Jumlah wajib diisi.")> _
    Public Property AmountFuel As Integer
End Class

<MetadataType(GetType(ModelFuelDumpTruck))>
Public Class FuelUsedDumpTruck

End Class
Public Class ModelFuelDumpTruck
    Public Property IDActivity As Integer
    <Required(ErrorMessage:="Jumlah wajib diisi.")> _
    Public Property AmountFuel As Integer
End Class