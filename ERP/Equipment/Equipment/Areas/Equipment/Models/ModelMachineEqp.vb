Imports System.ComponentModel
Imports System.ComponentModel.DataAnnotations
Imports System.Globalization
<MetadataType(GetType(ModelMachineEqp))>
Public Class MachineEqp
End Class
Public Class ModelMachineEqp
    <Required(ErrorMessage:="Nama wajib diisi.")> _
    Public Property Name As String
    <Required(ErrorMessage:="Harga wajib diisi.")> _
    Public Property Cost As Integer
    Public Property Remark As String
    Public Property Capacity As String
    Public SerialNumber As String
    <Required(ErrorMessage:="Merk wajib diisi.")> _
    Public Merk As String
    <Required(ErrorMessage:="Tipe wajib diisi.")> _
    Public Type As String
    Public MadeYear As String
    Public BuyYear As String
End Class
