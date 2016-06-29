Imports System.ComponentModel
Imports System.ComponentModel.DataAnnotations
Imports System.Globalization

<MetadataType(GetType(ProjectMutualCheck0DataValidation))>
Public Class ProjectMutualCheck0

End Class
Public Class ProjectMutualCheck0DataValidation
    <MaxLength(100, ErrorMessage:="Nama Penyetuju terlalu panjang. Maksimum jumlah karakter adalah 100")>
    Public ApprovedByName As String

    <MaxLength(100, ErrorMessage:="Jabatan Penyetuju terlalu panjang. Maksimum jumlah karakter adalah 100")>
    Public ApprovedByOccupation As String

    <MaxLength(100, ErrorMessage:="Nama Instansi Penyetuju terlalu panjang. Maksimum jumlah karakter adalah 100")>
    Public ApprovedByCompany As String

    <MaxLength(50, ErrorMessage:="NIP Penyetuju terlalu panjang. Maksimum jumlah karakter adalah 50")>
    Public ApprovedByNIPP As String

    <MaxLength(100, ErrorMessage:="Nama Pemeriksa terlalu panjang. Maksimum jumlah karakter adalah 100")>
    Public CheckedByName As String
    <MaxLength(100, ErrorMessage:="Jabatan Pemeriksa terlalu panjang. Maksimum jumlah karakter adalah 100")>
    Public CheckedByOccupation As String

    <MaxLength(100, ErrorMessage:="Nama Perusahaan Pemeriksa terlalu panjang. Maksimum jumlah karakter adalah 100")>
    Public CheckedByCompany As String

    <MaxLength(100, ErrorMessage:="Nama Pengaju terlalu panjang. Maksimum jumlah karakter adalah 100")>
    Public [ProposedByName] As String

    <MaxLength(100, ErrorMessage:="Jabatan Pengaju terlalu panjang. Maksimum jumlah karakter adalah 100")>
    Public [ProposedByOccupation] As String
    <MaxLength(100, ErrorMessage:="Nama Perusahaan Pengaju terlalu panjang. Maksimum jumlah karakter adalah 100")>
    Public [ProposedByCompany] As String

End Class