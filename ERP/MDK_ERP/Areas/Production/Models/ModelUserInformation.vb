Imports System.ComponentModel
Imports System.ComponentModel.DataAnnotations
Imports System.Globalization

Public Class ModelUserInformation
    <Required(ErrorMessage:="Wajib diisi")> _
    Property UserName As String
    'Property UserKey As Guid
    Property ID As Guid
    Property IsLock As Boolean
End Class

Public Class NewUser
    Inherits ModelUserInformation

    <Required(ErrorMessage:="Wajib diisi")> _
    <DataType(DataType.Password)> _
    <StringLength(25, ErrorMessage:="Masukkan 6 hingga 25 karakter", MinimumLength:=6)> _
    Property Password As String

    <DataType(DataType.Password)> _
    <Compare("Password", ErrorMessage:="konfirmasi password tidak sesuai dengan password")> _
    <StringLength(25, ErrorMessage:="Masukkan 6 hingga 25 karakter", MinimumLength:=6)> _
    <Required(ErrorMessage:="Wajib diisi")> _
    Property PasswordConfirm As String

    <Required(ErrorMessage:="Wajib dipilih")> _
    Property UserRole As String()
End Class
