Imports System.ComponentModel
Imports System.ComponentModel.DataAnnotations
Imports System.Globalization
<MetadataType(GetType(ModelTradoNonOperation))>
Public Class TradoNonOperation

End Class
Public Class ModelTradoNonOperation
    Public ID As Integer
    Public Property IDActivity As Integer
    <Required(ErrorMessage:="Jenis Pekerjaan wajib diisi.")> _
    Public Property NonOperationType As String
   
    <Required(ErrorMessage:="Jenis Pekerjaan wajib diisi.")> _
    Public Property Reason As String
End Class
