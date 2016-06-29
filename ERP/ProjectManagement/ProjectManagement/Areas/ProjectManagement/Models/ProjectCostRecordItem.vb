Imports System.ComponentModel
Imports System.ComponentModel.DataAnnotations
Imports System.Globalization

<MetadataType(GetType(ProjectCostRecordItemDataValidation))>
Public Class ProjectCostRecordItem

End Class

Public Class ProjectCostRecordItemDataValidation

    <Required(ErrorMessage:="Posting harus diisi")>
    Public [PostCategory] As String

    <Required(ErrorMessage:="Uraian harus diisi")>
    Public [ItemDescription] As String

    <Required(ErrorMessage:="Nilai transaksi Harus diisi")>
    <Range(1, Decimal.MaxValue, ErrorMessage:="Nilai Transaksi Tidak boleh 0")>
    Public [Value] As Decimal
    <MaxLength(255, ErrorMessage:="Keterangan tidak boleh lebih dari 255 Karakter")>
    Public [Notes] As String
End Class