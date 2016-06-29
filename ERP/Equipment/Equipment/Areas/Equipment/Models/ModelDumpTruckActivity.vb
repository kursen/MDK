Imports System.ComponentModel
Imports System.ComponentModel.DataAnnotations
Imports System.Globalization
<MetadataType(GetType(ModelDumpTruckActivity))>
Public Class DumpTruckActivity

End Class
Public Class ModelDumpTruckActivity
    <Required(ErrorMessage:="Kode wajib diisi.")> _
    Public Property Code As String
    <Required(ErrorMessage:="Merk wajib diisi.")> _
    Public Property Merk As String
    <Required(ErrorMessage:="Tipe wajib diisi.")> _
    Public Property Type As String
    <Required(ErrorMessage:="Operator wajib diisi.")> _
    Public Property [Operator] As String
    Public Property IDOp As Integer
    <Required(ErrorMessage:="Tanggal wajib diisi.")> _
    Public Property [Date] As Date
    <Required(ErrorMessage:="Nomor Polisi wajib diisi.")> _
    Public Property PoliceNumber As String
End Class
