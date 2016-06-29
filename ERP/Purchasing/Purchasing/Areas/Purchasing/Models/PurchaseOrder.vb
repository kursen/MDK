Imports System.ComponentModel.DataAnnotations
<MetadataType(GetType(PurchaseOrderDataValidation))>
Public Class PurchaseOrder

End Class
Public Class PurchaseOrderDataValidation
    <Required(ErrorMessage:="Nomor PO harus diisi")>
    Public OrderNumber As String
    <Required(ErrorMessage:="Tanggal PO harus diisi")>
    Public OrderDate As Date

    Public POType As Integer
    <Required(ErrorMessage:="Nomor Dokumen Permintaan harus diisi.")>
    Public RQNumber As String
    Public QuotationRef As String
    <Required(ErrorMessage:="Contact Person harus diisi")>
    Public ContactPerson_Name As String
    Public ContactPerson_Phone As String

    Public TermOfPayment As String
    <Required(ErrorMessage:="Tanggal Kebutuhan harus diisi")>
    Public DeliveryDate As String
    Public DeliveryTo As String
    Public DeliveryAddress As String
    <Required(ErrorMessage:="Nama Pembuat PO harus diisi")>
    Public PreparedBy_Name As String
    <Required(ErrorMessage:="Nama Penyetuju PO harus diisi")>
    Public ApprovedBy_Name As String
    Public Remarks As String
    Public Currency As String
    Public DocState As String
    Public Archive As String
    Public Vendor_CompanyName As String
    Public Vendor_ContactName As String
    Public Vendor_Phone As String
    Public Vendor_Address As String
    Public Vendor_City As String
    Public Vendor_Province As String
End Class
