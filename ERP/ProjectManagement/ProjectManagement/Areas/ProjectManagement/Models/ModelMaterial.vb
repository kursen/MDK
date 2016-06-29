Imports System.ComponentModel
Imports System.ComponentModel.DataAnnotations
Imports System.Globalization
<MetadataType(GetType(ModelMaterial))>
Public Class Material
    Protected ctx As ProjectManagement_ERPEntities
    Public Sub New()
        ctx = New ProjectManagement_ERPEntities
    End Sub

    Public Sub Save()
        ctx.Materials.AddObject(Me)
        If Me.ID > 0 Then
            ctx.ObjectStateManager.ChangeObjectState(Me, EntityState.Modified)
        End If
        ctx.SaveChanges()
    End Sub
End Class
Public Class ModelMaterial
    Public Property ProjectInfoID As Integer

    <Required(ErrorMessage:="Material wajib diisi")>
    Public Property Materials As String

    <Required(ErrorMessage:="Volume Lapangan wajib diisi")> _
    Public Property V_Lap As Decimal

    <Required(ErrorMessage:="Volume wajib diisi")> _
    Public Property Volume As Decimal

    <Required(ErrorMessage:="Satuan wajib diisi")> _
    Public Property Unit As String

    <Required(ErrorMessage:="Mulai wajib diisi")> _
    Public Property BeginDate As Date

    <Required(ErrorMessage:="Selesai wajib diisi")> _
    Public Property EndDate As Date
End Class
