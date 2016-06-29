Imports System.ComponentModel
Imports System.ComponentModel.DataAnnotations
Imports System.Globalization

<MetadataType(GetType(ProjectCostRecordDataValidation))>
Public Class ProjectCostRecord

End Class

Public Class ProjectCostRecordDataValidation
    <Required(ErrorMessage:="Uraian harus diisi")>
    Public [TaskGroupTitle] As String
    <Required(ErrorMessage:="Kode Paket harus diisi")>
    Public [PacketCode] As String
    <Required(ErrorMessage:="Tanggal Pembayaran harus diisi")>
    Public [TransactionDate] As Date
    Public [RecordedBy] As String
    Public [Cashier] As String
End Class

Public Class ProjectCostRecordList
    Property ProjectCostRecordId As Integer
    Property ProjectInfoId As Integer
    Property TaskgroupTitle As Integer
    Property PacketCode As String
    Property TransactionDate As Date
    Property RecordedBy As String
    Property Cashier As String
    Property Id As Integer
    Property PostCategory As String
    Property ItemDescription As String
    Property Value As Decimal
    Property Notes As String
End Class