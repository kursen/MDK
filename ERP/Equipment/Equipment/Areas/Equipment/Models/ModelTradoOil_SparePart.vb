Imports System.ComponentModel
Imports System.ComponentModel.DataAnnotations
<MetadataType(GetType(ModelTradoOil_SparePart))>
Public Class OilAndSparePartUsedTrado

End Class
Public Class ModelTradoOil_SparePart
    Public Property ID As Integer
    Public Property IDActivity As Integer
    <Required(ErrorMessage:="Jenis Oli Atau Spare Part wajib diisi.")> _
    Public Property OilOrSparePartType As String
    <Required(ErrorMessage:="Jumlah wajib diisi.")> _
    Public Property Amount As Decimal
    <Required(ErrorMessage:="Satuan wajib diisi.")> _
    Public Property Unit As String
End Class
