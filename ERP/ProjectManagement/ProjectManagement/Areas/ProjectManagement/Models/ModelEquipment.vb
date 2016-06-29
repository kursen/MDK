Imports System.ComponentModel
Imports System.ComponentModel.DataAnnotations
Imports System.Globalization
<MetadataType(GetType(ModelEquipment))>
Public Class Equipment
    Protected ctx As New ProjectManagement_ERPEntities
   
    Public Sub Save()
        ctx.Equipments.AddObject(Me)
        If Me.ID > 0 Then
            ctx.ObjectStateManager.ChangeObjectState(Me, EntityState.Modified)
        End If
        ctx.SaveChanges()
    End Sub
End Class
Public Class ModelEquipment
    'Public Property ID As Integer
    Public Property ProjectInfoID As Integer
    <Required(ErrorMessage:="Peralatan wajib diisi.")> _
    Public Property Equipments As String

    <Required(ErrorMessage:="Volume wajib diisi")>
    Public Property Volume As Integer

    <Required(ErrorMessage:="Satuan wajib diisi")> _
    Public Property Unit As String

    <Required(ErrorMessage:="Mulai wajib diisi")> _
    Public Property BeginDate As Date

    <Required(ErrorMessage:="Selesai wajib diisi")> _
    Public Property EndDate As Date

End Class