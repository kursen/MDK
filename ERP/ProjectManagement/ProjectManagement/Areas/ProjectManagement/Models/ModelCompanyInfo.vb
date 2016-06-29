Imports System.ComponentModel
Imports System.ComponentModel.DataAnnotations
Imports System.Globalization

<MetadataType(GetType(ModelCompanyInfo))>
Public Class CompanyInfo
    'Protected ctx As ProjectManagement_ERPEntities
    'Public Sub New()
    '    ctx = New ProjectManagement_ERPEntities
    'End Sub

    'Public Sub Save()
    '    ctx.CompanyInfoes.AddObject(Me)
    '    Dim ExistData = (From c In ctx.CompanyInfoes Where c.ID = Me.ID Select c.Name).FirstOrDefault()
    '    If Not IsNothing(ExistData) Then
    '        ctx.ObjectStateManager.ChangeObjectState(Me, EntityState.Modified)
    '    End If
    '    ctx.SaveChanges()
    'End Sub
End Class

Public Class ModelCompanyInfo
    <Required(ErrorMessage:="Nama Perusahaan wajib diisi.")> _
    Public Property Name As String

    <Required(ErrorMessage:="Alias harus diisi")>
    <StringLength(10, ErrorMessage:="Alias tidak dapat melebihi 10 karakter")>
    Public Property [Alias] As String

    <Required(ErrorMessage:="Alamat 1 wajib diisi")> _
    Public Property Address1 As String

    <Required(ErrorMessage:="Kota wajib diisi")> _
    Public Property City As String

    <Required(ErrorMessage:="Nomor telepon wajib diisi")> _
    <RegularExpression("[0-9+]*\.?[0-9]+", ErrorMessage:="Nomor telefon tidak valid")> _
    <StringLength(18, ErrorMessage:="Nomor telefon tidak dapat melebihi 18 karakter")>
    Public Property Phone1 As String



End Class