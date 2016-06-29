Imports System.ComponentModel.DataAnnotations
<MetadataType(GetType(VendorViewModel))>
Public Class Vendor
End Class

Public Class VendorViewModel
    <DisplayFormat(ConvertEmptyStringToNull:=False)> _
    <Required(ErrorMessage:="Nama Vendor wajib diisi")> _
    Public Property Name As String

    '<DisplayFormat(ConvertEmptyStringToNull:=False)> _
    <Required(ErrorMessage:="Nomor Vendor wajib diisi")> _
    Public Property Number As String

    '<DisplayFormat(ConvertEmptyStringToNull:=False)> _
    <Required(ErrorMessage:="Nomor Telepon kontak wajib diisi")> _
    Public Property Phone As String

    '<DisplayFormat(ConvertEmptyStringToNull:=False)> _
    <Required(ErrorMessage:="serial NPWP wajib diisi")> _
    Public Property NPWPNumber As String

    <Required(ErrorMessage:="Nama kontak wajib diisi")> _
    Public Property ContactName As String

    '<DisplayFormat(ConvertEmptyStringToNull:=False)> _
    <Required(ErrorMessage:="Kategori wajib diisi")> _
    <Range(1, Integer.MaxValue, ErrorMessage:="Kategori wajib diisi")> _
    Public Property CategoryId As Integer

End Class

