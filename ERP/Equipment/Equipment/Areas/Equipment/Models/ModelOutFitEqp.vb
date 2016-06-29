Imports System.ComponentModel
Imports System.ComponentModel.DataAnnotations
Imports System.Globalization
<MetadataType(GetType(ModelOutFitEqp))>
Public Class OutFitEqp
  
End Class
Public Class ModelOutFitEqp
    <Required(ErrorMessage:="Nama wajib diisi.")> _
    Public Property Name As String
    <Required(ErrorMessage:="Jumlah wajib diisi.")> _
    Public Property Total As Integer
    <Required(ErrorMessage:="Harga wajib diisi.")> _
    Public Property Cost As Integer
    <Required(ErrorMessage:="Keterangan wajib diisi.")> _
    Public Property Remark As String
End Class
