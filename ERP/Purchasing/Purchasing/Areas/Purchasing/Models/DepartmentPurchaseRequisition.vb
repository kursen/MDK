Imports System.ComponentModel.DataAnnotations
<MetadataType(GetType(DepartmentPurchaseRequisitionValidation))>
Public Class DepartmentPurchaseRequisition
 

End Class

Public Class DepartmentPurchaseRequisitionValidation
    <Required()>
    Public ID As Integer
    <Required(ErrorMessage:="Nomor Dokumen wajib diisi")>
    Public RecordNo As String
    <Required(ErrorMessage:="Departemen belum dipilih")>
    Public OfficeID As Integer
    <Required(ErrorMessage:="Tgl permintaan harus diisi")>
    Public RequestDate As Date
    <Required(ErrorMessage:="Tgl kebutuhan barang harus diisi")>
    Public DeliveryDate As Date
    <Required(ErrorMessage:="Tujuan pengiriman wajib diisi")>
    Public DeliveryTo As String
    <Required(ErrorMessage:="Alamat pengiriman wajib diisi")>
    Public DeliveryAddress As String
    <Required(ErrorMessage:="Diminta oleh wajib diisi")>
    Public RequestedBy_Name As String

    <Required(ErrorMessage:="Jabatan harus diisi")>
    Public RequestedBy_Occupation As String

End Class