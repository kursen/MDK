Imports System.ComponentModel
Imports System.ComponentModel.DataAnnotations
Imports System.Globalization
<MetadataType(GetType(ModelHeavyEqp))>
Public Class HeavyEqp
   
End Class

Public Class ModelHeavyEqp
    <Required(ErrorMessage:="Kode Alat berat wajib diisi.")> _
    Public Property Code As String
    <Required(ErrorMessage:="Merk wajib diisi.")> _
    Public Property Merk As String
    <Required(ErrorMessage:="Jenis wajib diisi.")> _
    Public Property Species As String
    <Required(ErrorMessage:="Tipe wajib diisi.")> _
    Public Property Type As String

    <Required(ErrorMessage:="Harga Perolehan wajib diisi.")> _
    Public Cost As System.Decimal

    <Required(ErrorMessage:="Tahun Beli wajib diisi.", AllowEmptyStrings:=True)> _
    <Range(1900, 9999, ErrorMessage:="Tahun beli harus berupa angka antara 1900 dan tahun ini ")>
    Public Year As Integer
    Public Property SerialNumber As String
    Public Property TaxNumber As String
    Public Property Capacity As Integer
    Public Property Img As Byte
    Public Property Remarks As String
End Class
Public Class HeavyEqpView
    Inherits HeavyEqp
    Public Property OfficeId As Integer?
    Public Property OfficeName As String
End Class