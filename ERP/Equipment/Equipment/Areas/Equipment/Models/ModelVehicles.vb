Imports System.ComponentModel
Imports System.ComponentModel.DataAnnotations
Imports System.Globalization
<MetadataType(GetType(ModelVehicles))>
Public Class Vehicle
End Class
Public Class ModelVehicles
    <Required(ErrorMessage:="Kode wajib diisi.")> _
    Public Property Code As String
    <Required(ErrorMessage:="Merk wajib diisi.")> _
    Public Property Merk As String
    <Required(ErrorMessage:="Jenis wajib diisi.")> _
    Public Property Species As String
    <Required(ErrorMessage:="Tipe wajib diisi.")> _
    Public Property Type As String
    <Required(ErrorMessage:="Harga Beli wajib diisi.")> _
    Public Property Cost As Integer
    Public Property Capacity As String
    <Required(ErrorMessage:="Tahun Beli wajib diisi.")> _
    Public Property Year As String
    <Required(ErrorMessage:="Tahun Pembuatan wajib diisi.")> _
    Public Property BuiltYear As String
    <Required(ErrorMessage:="Nomor Polisi wajib diisi.")> _
    Public Property PoliceNumber As String
    Public Property MachineNumber As String
    Public Property BonesNumber As String
    Public Property DueDate As Date
    Public Property KeurNumber As String
    Public Property KeurDueDate As Date
    Public Property Remarks As String
    Public Property Img As System.Text.Encoding
End Class
