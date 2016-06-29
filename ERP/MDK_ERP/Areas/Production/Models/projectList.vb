Imports System.ComponentModel
Imports System.ComponentModel.DataAnnotations
Imports System.Globalization
<MetadataType(GetType(ModelProjectList))>
Public Class ProjectList
    Protected ctx As ERPEntities

    Public Sub New()
        ctx = New ERPEntities
    End Sub

    Public Sub Save()
        ctx.ProjectList.AddObject(Me)
        If Me.ID > 0 Then
            ctx.ObjectStateManager.ChangeObjectState(Me, EntityState.Modified)
        End If
        ctx.SaveChanges()
    End Sub
End Class

Public Class ModelProjectList
    <Required(ErrorMessage:="Wajib diisi")> _
    Public Property NoProject() As Global.System.String
    <Required(ErrorMessage:="Wajib diisi")> _
    Public Property StartDate() As Global.System.DateTime
    <Required(ErrorMessage:="Wajib diisi")> _
    Public Property EndDate() As Global.System.DateTime
    <Required(ErrorMessage:="Wajib diisi")> _
    Public Property PackageName() As Global.System.String
    <Required(ErrorMessage:="Wajib diisi")> _
    Public Property AgentName() As String
    Public Property Description() As String
End Class
