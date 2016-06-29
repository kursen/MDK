Imports System.ComponentModel
Imports System.ComponentModel.DataAnnotations
Imports System.Globalization

<MetadataType(GetType(ProjectTaskDivisionItemDataValidation))>
Public Class ProjectTaskDivisionItem
    'Sub x()
    '    Me.
    'End Sub
End Class
Public Class ProjectTaskDivisionItemDataValidation
    <Required(ErrorMessage:="No Mata Pembayaran harus diisi")>
    <MaxLength(10, ErrorMessage:="Jumlah Karakter tidak boleh lebih dari 10.")>
    Public PaymentNumber As String

    <MaxLength(150, ErrorMessage:="Jumlah Karakter tidak boleh lebih dari 5.")>
    <Required(ErrorMessage:="Uraian harus diisi.")>
    Public TaskTitle As String

    <Required(ErrorMessage:="Satuan harus diisi.")>
    <MaxLength(10, ErrorMessage:="Satuan tidak boleh lebih dari 10 karakter")>
    Public UnitQuantity As String

    <Required(ErrorMessage:="Harga Satuan harus diisi.")>
    <Range(0, 999999999999, ErrorMessage:="Nilai minimum tidak boleh sama atau kurang dari 0")>
    Public Value As Decimal

    <Required(ErrorMessage:="Kuantitas harus diisi.")>
    <Range(0, 999999999999, ErrorMessage:="Nilai minimum tidak boleh sama atau kurang dari 0")>
    Public Quantity As Decimal
End Class