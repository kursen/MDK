Imports System.ComponentModel
Imports System.ComponentModel.DataAnnotations
Imports System.Globalization
<MetadataType(GetType(ModelTradoActivity))>
Public Class TradoActivity

End Class

Public Class ModelTradoActivity
    <Required(ErrorMessage:="Kode wajib diisi.")> _
    Public Property Code As String
    <Required(ErrorMessage:="Merk wajib diisi.")> _
    Public Property Merk As String
    <Required(ErrorMessage:="Tipe wajib diisi.")> _
    Public Property Type As String
    <Required(ErrorMessage:="Supir wajib diisi.")> _
    Public Property Driver As String
    Public Property IDDriver As Integer
    <Required(ErrorMessage:="Tanggal wajib diisi.")> _
    Public Property [Date] As Date
    <Required(ErrorMessage:="Nomor Polisi wajib diisi.")> _
    Public Property PoliceNumber As String
End Class
