Imports System.ComponentModel
Imports System.ComponentModel.DataAnnotations
<MetadataType(GetType(ModelHeavyEqpOperation))>
Public Class HeavyEqpOperation
End Class
Public Class ModelHeavyEqpOperation
    Public Property IDActivity As Integer
    <Required(ErrorMessage:="Jenis Pekerjaan wajib diisi.")> _
    Public Property OperationType As String
    <Required(ErrorMessage:="Volume Pekerjaan wajib diisi.")> _
     <Range(0, Decimal.MaxValue, ErrorMessage:="Data Tidak Valid.")> _
     <RegularExpression("^[1-9]\d*(\.\d+)?$", ErrorMessage:="volume tidak valid")> _
    Public Property VolumeOperation As Decimal
    <Required(ErrorMessage:="Lokasi Pekerjaan wajib diisi.")> _
    Public Property LocationOperation As String
    <Required(ErrorMessage:="HM awal wajib diisi.")> _
    <Range(0, Decimal.MaxValue, ErrorMessage:="Data Tidak Valid.")> _
    <RegularExpression("^[1-9]\d*(\.\d+)?$", ErrorMessage:="HM Awal tidak valid")> _
    Public Property BeginHM As Decimal
    <Required(ErrorMessage:="HM akhir  wajib diisi.")> _
     <Range(0, Decimal.MaxValue, ErrorMessage:="Data Tidak Valid.")> _
    Public Property EndHM As Decimal
    <Required(ErrorMessage:="Paket  wajib diisi.")> _
    Public Property PacketOperation As String
End Class
