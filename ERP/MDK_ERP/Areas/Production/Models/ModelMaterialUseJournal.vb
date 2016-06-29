Imports System.ComponentModel
Imports System.ComponentModel.DataAnnotations
Imports System.Globalization
<MetadataType(GetType(ModelMaterialUseJurnalValidation))>
Public Class MaterialUseJournal
    Protected ctx As ERPEntities

    Public Sub New()
        ctx = New ERPEntities
    End Sub

    Public Sub Save()
        If Me.ID <= 0 Then
            Dim modelInventories As New MaterialInventories With {
                .IdInventoryStatus = 4,
                .IsPlus = False
            }
            ctx.MaterialInventories.AddObject(modelInventories)
            ctx.SaveChanges()
            Me.IDInventory = modelInventories.ID
        End If
        ctx.MaterialUseJournal.AddObject(Me)
        If Me.ID > 0 Then
            ctx.ObjectStateManager.ChangeObjectState(Me, EntityState.Modified)
        End If
        ctx.SaveChanges()
    End Sub
End Class
Public Class ModelMaterialUseJurnalValidation
    'Public Property ID() As Integer
    'Public Property IDMaterial() As Integer
    Private _DateUse As DateTime
    <Required(ErrorMessage:="Tanggal Tidak Boleh Kosong")>
    Public Property DateUse() As DateTime
        Set(value As DateTime)
            _DateUse = value
        End Set
        Get
            Return _DateUse
        End Get
    End Property

    Private _IdMachine As Integer
    <Required(ErrorMessage:="Silahkan Pilih Mesin")>
    Public Property IdMachine() As Integer
        Get
            Return _IdMachine
        End Get
        Set(value As Integer)
            _IdMachine = value
        End Set
    End Property

    Private _OperatorName As String
    <Required(ErrorMessage:="Tanggal Tidak Boleh Kosong")>
    Public Property OperatorName() As String
        Set(value As String)
            _OperatorName = value
        End Set
        Get
            Return _OperatorName
        End Get
    End Property
    Private _Amount As Integer
    <Required(ErrorMessage:="Jumlah Tidak Boleh Kosong")>
    Public Property Amount() As Integer
        Set(value As Integer)
            _Amount = value
        End Set
        Get
            Return _Amount
        End Get
    End Property

    'Private _IDMeasurementUnit As Integer

    'Public Property IDMeasurementUnit() As Integer

    'Public Property IDProject() As Integer

    'Public Property Description() As String

    'Public Property IDInventory() As Integer
End Class