Imports System.ComponentModel
Imports System.ComponentModel.DataAnnotations
Imports System.Globalization
<MetadataType(GetType(ModelMachine))>
Public Class MstMachines
    Protected ctx As ERPEntities

    Public Sub New()
        ctx = New ERPEntities
    End Sub

    Public Sub Save()
        ctx.MstMachines.AddObject(Me)
        If Me.ID > 0 Then
            ctx.ObjectStateManager.ChangeObjectState(Me, EntityState.Modified)
        End If
        ctx.SaveChanges()
    End Sub
End Class

Public Class ModelMachine
    <Required(ErrorMessage:="Wajib diisi")> _
    Public Property MachineName() As Global.System.String
    <Required(ErrorMessage:="Wajib diisi")> _
    Public Property IdMachineType() As Global.System.Int32
    <Required(ErrorMessage:="Wajib diisi")> _
    Public Property SeriesNumber() As Global.System.String
End Class
