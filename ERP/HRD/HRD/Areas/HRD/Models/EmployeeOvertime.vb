Imports System.ComponentModel
Imports System.ComponentModel.DataAnnotations
Imports System.Globalization

<MetadataType(GetType(EmployeeOvertime_DataValidation))>
Public Class EmployeeOvertime

End Class

Public Class EmployeeOvertime_DataValidation
    <Required(ErrorMessage:="Nama Karyawan harus diisi")>
    <Range(1, 9999, ErrorMessage:="Karyawan tidak terdaftar")>
    Public EmployeeId As Integer


    <Required(ErrorMessage:="Kantor tidak terdaftar")>
    <Range(1, 9999, ErrorMessage:="Kantor tidak terdaftar")>
    Public OfficeId As Integer


    <Required(ErrorMessage:="Kegiatan harus diisi")>
    Public Activity As String

    <Required(ErrorMessage:="Tanggal harus diisi")>
    Public ActivityDate As Date


End Class