Imports System.ComponentModel.DataAnnotations
<MetadataType(GetType(PurchaseOrderDetailDataValidation))>
Public Class PurchaseOrderDetail

End Class
Public Class PurchaseOrderDetailDataValidation
    <Required(ErrorMessage:="Nama Item harus diisi")>
    Property ItemName As String
    <Range(0, 99999999, ErrorMessage:="Angka Kuantitas tidak benar")>
    Property Quantity As Decimal
    <Required(ErrorMessage:="Satuan barang harus diisi")>
    Property UnitQuantity As String
    
    <Range(0.001, Decimal.MaxValue, ErrorMessage:="Perkiraan Harga tidak benar")>
    Property UnitPrice As Decimal
    <Required(ErrorMessage:="Total Perkiraan harga salah.")>
    Property TotalPrice As Double

End Class