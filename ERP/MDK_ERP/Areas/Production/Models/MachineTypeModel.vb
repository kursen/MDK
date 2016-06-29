Imports System.ComponentModel
Imports System.ComponentModel.DataAnnotations
Imports System.Globalization
<MetadataType(GetType(ModelMstMachineTypes))>
Public Class MstMachineTypes
    Protected ctx As ERPEntities

    Public Sub New()
        ctx = New ERPEntities
    End Sub

    Public Sub Save()
        ctx.MstMachineTypes.AddObject(Me)
        If Me.ID > 0 Then
            ctx.ObjectStateManager.ChangeObjectState(Me, EntityState.Modified)
        End If
        ctx.SaveChanges()
    End Sub
End Class

Public Class ModelMstMachineTypes
    <Required(ErrorMessage:="Wajib diisi")> _
    Public Property MachineType() As Global.System.String
    <Required(ErrorMessage:="Wajib diisi")> _
    Public Property Description() As Global.System.String
End Class
