Imports System.ComponentModel
Imports System.ComponentModel.DataAnnotations
Imports System.Globalization

<MetadataType(GetType(Master_PersonalDataValidation))>
Public Class Master_Personal
  
End Class
Public Class Master_PersonalDataValidation
    Public ID As Int32

    <Required(ErrorMessage:="Kode Karyawan harus diisi")>
    Public EmployeeID As String

    <Required(ErrorMessage:="Nama harus diisi")>
    Public FullName As String
    <Required(ErrorMessage:="Nomor Induk Kependudukan harus diisi")>
    Public NIK As String

    <Required(ErrorMessage:="Jenis kelamin tidak boleh Null")>
    Public Gender As String

    <Required(ErrorMessage:="Tanggal Lahir tidak boleh kosong")>
    Public [DateOfBirth] As Date?


    



    <Required(ErrorMessage:="Nomor telfon darurat harus diisi.")>
    Public EmergencyCallNumber As String


    

End Class
