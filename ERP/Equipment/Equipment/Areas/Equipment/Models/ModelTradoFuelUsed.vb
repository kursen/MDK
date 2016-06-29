Imports System.ComponentModel
Imports System.ComponentModel.DataAnnotations
<MetadataType(GetType(ModelTradoFuelUsed))> _
Public Class FuelUsedTrado

End Class

Public Class ModelTradoFuelUsed
    Public Property ID As Integer
    Public Property IDActivity As Integer
    <Required(ErrorMessage:="Waktu wajib diisi.")> _
    Public Property TimeFill As TimeSpan

    <Required(ErrorMessage:="Jumlah wajib diisi.")> _
    Public Property AmountFuel As Decimal
    <Required(ErrorMessage:="Lokasi SPBU wajib diisi.")> _
    Public Property Location As String
End Class
