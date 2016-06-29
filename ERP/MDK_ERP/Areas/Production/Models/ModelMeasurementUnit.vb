Imports System.ComponentModel
Imports System.ComponentModel.DataAnnotations
Imports System.Globalization
<MetadataType(GetType(ModelMeasurementUnitsValidation))>
Public Class MstMeasurementUnits
    Protected ctx As ERPEntities

    Public Sub New()
        ctx = New ERPEntities
    End Sub

    Public Sub Save()
        ctx.MstMeasurementUnits.AddObject(Me)
        If Me.ID > 0 Then
            ctx.ObjectStateManager.ChangeObjectState(Me, EntityState.Modified)
        End If
        ctx.SaveChanges()
    End Sub
End Class

Public Class ModelMeasurementUnitsValidation
    <Required(ErrorMessage:="Simbol Tidak Boleh Kosong")>
    Public Property Symbol As String

    <Required(ErrorMessage:="Satuan Tidak Boleh Kosong")>
    Public Property Unit As String

    <Required(ErrorMessage:="Satuan Tidak Boleh Kosong")>
    Public Property Ratio As Double
End Class
