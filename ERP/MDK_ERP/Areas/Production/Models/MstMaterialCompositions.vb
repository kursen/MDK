Imports System.ComponentModel
Imports System.ComponentModel.DataAnnotations
Imports System.Globalization
<MetadataType(GetType(MstMaterialCompositionsValidation))>
Public Class MstMaterialCompositions
    Protected ctx As ERPEntities
    Public Sub New()
        ctx = New ERPEntities
    End Sub

    Public Sub Save()
        ctx.MstMaterialCompositions.AddObject(Me)
            If Me.ID > 0 Then
                ctx.ObjectStateManager.ChangeObjectState(Me, EntityState.Modified)
            End If
        ctx.SaveChanges()
    End Sub
End Class

Public Class MstMaterialCompositionsValidation
    <Required(ErrorMessage:="Material Tidak Boleh Kosong")>
    Public Property IDMaterial() As Integer

    <Required(ErrorMessage:="Komposisi Tidak Boleh Kosong")>
    Public Property IDMaterialComposition() As Integer

    <Required(ErrorMessage:="Jumlah Tidak Boleh Kosong")>
    Public Property Amount() As Double
End Class