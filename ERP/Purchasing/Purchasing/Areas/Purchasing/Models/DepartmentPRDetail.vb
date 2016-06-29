Imports System.ComponentModel.DataAnnotations
<MetadataType(GetType(DepartmentPRDetailValidation))>
Public Class DepartmentPRDetail
    
End Class

Public Class DepartmentPRDetailValidation
    <Required(ErrorMessage:="Nama item harus diisi")>
    Public ItemName As String
    <Required(ErrorMessage:="Alokasi harus diisi")>
    Public Allocation As String
    <Required()>
    <Range(1, Integer.MaxValue, ErrorMessage:="Jumlah permintaan harus ada")>
    Public Quantity As Integer
    <Required(ErrorMessage:="Mata uang wajib dipilih")>
    Public Currency As String
    <Required()>
    <Range(1, Decimal.MaxValue, ErrorMessage:="Harga per item harus lebih besar dari nol")>
    Public EstUnitPrice As Decimal
    <Required()>
    <Range(1, Decimal.MaxValue, ErrorMessage:="Total harga harus lebih besar dari nol")>
    Public TotalEstPrice As Decimal
    Public Remarks As String
    <Required(ErrorMessage:="Unit satuan wajib dipilih")>
    Public UnitQuantity As String
End Class