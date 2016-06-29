Imports System.ComponentModel.DataAnnotations
<MetadataType(GetType(DeliveryOrderDetailDataValidation))>
Public Class DeliveryOrderDetail

End Class
Public Class DeliveryOrderDetailDataValidation
    Property DeliveryOrderId As Integer
    Property PurchaseOrderDetailId As Integer
    <Range(0, 99999999, ErrorMessage:="Angka Kuantitas tidak benar")>
    Property Quantity As Decimal
End Class