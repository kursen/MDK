Imports System.ComponentModel
Imports System.ComponentModel.DataAnnotations
Imports System.Globalization
<MetadataType(GetType(ModelHeavyEqpAvtivity))>
Public Class HeavyEqpActivity

End Class
Public Class ModelHeavyEqpAvtivity
    <Required(ErrorMessage:="Kode alat berat wajib diisi.")> _
    Public Property Code As String
    <Required(ErrorMessage:="Merk wajib diisi.")> _
    Public Property Merk As String
    <Required(ErrorMessage:="Tipe wajib diisi.")> _
    Public Property Type As String
    <Required(ErrorMessage:="Operator wajib diisi.")> _
    Public Property [Operator] As String

    <Range(1, Integer.MaxValue, ErrorMessage:="Operator tidak terdapat dalam daftar")>
    Public Property IDOp As Integer
    <Required(ErrorMessage:="Tanggal wajib diisi.")> _
    Public Property [Date] As Date
End Class
