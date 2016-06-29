Imports System.ComponentModel
Imports System.ComponentModel.DataAnnotations
<MetadataType(GetType(ModelOil))>
Public Class OilUsedHeavyEqp

End Class
Public Class ModelOil
    Public Property IDActivity As Integer
    <Required(ErrorMessage:="Oli wajib diisi.")> _
    Public Property OilType As String
    <Required(ErrorMessage:="Jumlah wajib diisi.")> _
    Public Property Amount As Integer
End Class

<MetadataType(GetType(ModelOil))>
Public Class OilUsedDumpTruck

End Class