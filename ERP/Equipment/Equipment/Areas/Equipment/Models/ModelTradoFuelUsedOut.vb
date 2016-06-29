Imports System.ComponentModel
Imports System.ComponentModel.DataAnnotations
<MetadataType(GetType(ModelTradoFuelUsedOut))>
Public Class FuelUsedOutTrado

End Class
Public Class ModelTradoFuelUsedOut
    Public Property ID As Integer
    Public Property IDActivity As Integer
    <Required(ErrorMessage:="Alokasi wajib diisi.")> _
     Public Property Alocation As String
    <Required(ErrorMessage:="Jumlah wajib diisi.")> _
     Public Property AmountFuelOut As Decimal
    <Required(ErrorMessage:="Operator wajib diisi.")> _
     Public Property [Operator] As String
End Class
