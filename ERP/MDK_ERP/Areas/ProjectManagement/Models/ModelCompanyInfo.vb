Imports System.ComponentModel
Imports System.ComponentModel.DataAnnotations
Imports System.Globalization

<MetadataType(GetType(ModelCompanyInfo))>
Public Class CompanyInfo
    Protected ctx As ERPEntities
    Public Sub New()
        ctx = New ERPEntities
    End Sub

    Public Sub Save()
        ctx.CompanyInfo.AddObject(Me)
        Dim ExistData = (From c In ctx.CompanyInfo Where c.ID = Me.ID Select c.Name).FirstOrDefault()
        If Not IsNothing(ExistData) Then
            ctx.ObjectStateManager.ChangeObjectState(Me, EntityState.Modified)
        End If
        ctx.SaveChanges()
    End Sub
End Class

Public Class ModelCompanyInfo
    <Required(ErrorMessage:="Wajib diisi")> _
    Public Property Name As String

    <StringLength(10, ErrorMessage:="tidak dapat melebihi 10 karakter")>
    Public Property [Alias] As String

    <Required(ErrorMessage:="Wajib diisi")> _
    Public Property Address1 As String

    <Required(ErrorMessage:="Wajib diisi")> _
    Public Property City As String

    <Required(ErrorMessage:="Wajib diisi")> _
    <RegularExpression("[0-9+]*\.?[0-9]+", ErrorMessage:="masukan tidak valid")> _
    <StringLength(18, ErrorMessage:="tidak dapat melebihi 18 karakter")>
    Public Property Phone1 As String
End Class