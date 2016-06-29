Imports System.ComponentModel.DataAnnotations
<MetadataType(GetType(DeliveryOrderDataValidation))>
Public Class DeliveryOrder

End Class
Public Class DeliveryOrderDataValidation
    <Required(ErrorMessage:="Tanggal penerimaan harus diisi")>
    Public ReceiptDate As Date
    <Required(ErrorMessage:="Nomor PO Permintaan harus dipilih")>
    <Range(1, Integer.MaxValue, ErrorMessage:="Nomor PO Permintaan harus dipilih")>
    Public PurchaseOrderId As Integer
    <Required(ErrorMessage:="Nomor dokumen harus diisi")>
    Public DocNo As String
    <Required(ErrorMessage:="Nama penerima wajib diisi")>
    Public ReceiptBy_Name As String
End Class
