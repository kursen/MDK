Imports System.ComponentModel
Imports System.ComponentModel.DataAnnotations
<MetadataType(GetType(ModelNonOperation))>
Public Class HeavyEqpNonOperation
End Class
Public Class ModelNonOperation
    Public ID As Integer
    Public Property IDActivity As Integer
    <Required(ErrorMessage:="Jenis Pekerjaan wajib diisi.")> _
    Public Property NonOperationType As String
    <Required(ErrorMessage:="Awal wajib diisi.")> _
    Public Property [Begin] As TimeSpan
    <Required(ErrorMessage:="Akhir wajib diisi.")> _
    Public Property [End] As TimeSpan
    <Required(ErrorMessage:="Jenis Pekerjaan wajib diisi.")> _
    Public Property Reason As String
End Class

<MetadataType(GetType(ModelNonOperation))>
Public Class DumpTruckNonOperations
End Class