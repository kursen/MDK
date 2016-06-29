Imports System.ComponentModel.DataAnnotations
<MetadataType(GetType(ProjectPurchaseRequisitionValidation))>
Public Class ProjectPurchaseRequisition

End Class
Public Class ProjectPurchaseRequisitionValidation
    <Required(ErrorMessage:="No Permintaan harus diisi")>
    Public RecordNo As String

    <Required(ErrorMessage:="Kode Proyek harus diisi")>
    Public ProjectCode As String

    <Required(ErrorMessage:="Nama Proyek harus diisi")>
    Public ProjectTitle As String

    <Required(ErrorMessage:="Tanggal Permintaan harus diisi")>
    Public RequestDate As Date
    <Required(ErrorMessage:="Tanggal Kebutuhan harus diisi")>
    Public DeliveryDate As Date
    <Required(ErrorMessage:="Penerima harus diisi")>
    Public DeliveryTo As String
    <Required(ErrorMessage:="Alamat Penerima harus diisi")>
    Public DeliveryAddress As String
    <Required(ErrorMessage:="Nama Pejabat yang meminta harus diisi")>
    Public RequestedBy_Name As String
    <Required(ErrorMessage:="Jabatan yang meminta harus diisi")>
    Public RequestedBy_Occupation As String
End Class