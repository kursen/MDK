Imports System.ComponentModel.DataAnnotations
<MetadataType(GetType(ModelPriceComparison))>
Public Class PriceComparison

End Class
Public Class ModelPriceComparison
    <Required(ErrorMessage:="Tanggal Pembuatan harus diisi")>
    Public Property CreateDate As Date

    <Range(1, 9999999, ErrorMessage:="No Permintaan harus diisi")>
    Public Property PurchaseRequisitionID As Integer
    Public Property VendorID1 As Integer
    Public Property VendorID2 As Integer
    Public Property VendorID3 As Integer
End Class

'<MetadataType(GetType(ModelViewPriceComparison))>
'Public Class PriceComparisonView

'End Class
'Public Class ModelViewPriceComparison
'    <Required(ErrorMessage:="Tanggal Pembuatan harus diisi")>
'    Public Property CreateDate As Date
'    <Required(ErrorMessage:="No Permintaan harus diisi")>
'    Public Property PurchaseRequisitionID As Integer
'    Public Property NoRecord As String
'    Public Property VendorID1 As Integer
'    Public Property VendorID2 As Integer
'    Public Property VendorID3 As Integer
'End Class
