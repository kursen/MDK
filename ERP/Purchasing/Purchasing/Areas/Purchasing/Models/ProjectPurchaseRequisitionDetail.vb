Imports System.ComponentModel.DataAnnotations
<MetadataType(GetType(ProjectPurchaseRequisitionDetailDataValidation))>
Public Class ProjectPurchaseRequisitionDetail

End Class
Public Class ProjectPurchaseRequisitionDetailDataValidation
    <Required(ErrorMessage:="Nama Item harus diisi")>
    Property ItemName As String
    <Range(0, 99999999, ErrorMessage:="Angka Kuantitas tidak benar")>
    Property Quantity As Decimal

    <Required(ErrorMessage:="Merk/Jenis atau keterangan lainnya harus diisi")>
    Property Brand As String

    <Required(ErrorMessage:="Satuan barang harus diisi")>
    Property UnitQuantity As String
    <Required(ErrorMessage:="Mata uang harus diisi")>
    Property Currency As String
    <Range(0.001, Decimal.MaxValue, ErrorMessage:="Perkiraan Harga tidak benar")>
    Property EstUnitPrice As Decimal
    <Required(ErrorMessage:="Total Perkiraan harga salah.")>
    Property TotalEstPrice As Double
    
End Class