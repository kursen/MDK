Imports System.ComponentModel
Imports System.ComponentModel.DataAnnotations
Imports System.Globalization
<MetadataType(GetType(ModelEditAmp))>
Public Class AMPJournals
    Protected ctx As ERPEntities
    Public Sub New()
        ctx = New ERPEntities
    End Sub

    Public Sub Save()
        ctx.AMPJournals.AddObject(Me)
        If Me.ID > 0 Then
            ctx.ObjectStateManager.ChangeObjectState(Me, EntityState.Modified)
        End If
        ctx.SaveChanges()
    End Sub
End Class
Public Class ModelEditAmp
    Private _IdMachine As Byte
    Property IdMachine() As Byte
        Set(value As Byte)
            _IdMachine = value
        End Set
        Get
            Return _IdMachine
        End Get
    End Property

    Private _DateUse As Date
    Property DateUse() As Date
        Set(value As Date)
            _DateUse = value
        End Set
        Get
            Return _DateUse
        End Get
    End Property

    Private _IdShift As Integer
    Property IdShift() As Integer
        Set(value As Integer)
            _IdShift = value
        End Set
        Get
            Return _IdShift
        End Get
    End Property
    Private _Operator As String
    <Required(ErrorMessage:="Isi Name Operator")>
    Property [Operator]() As String
        Set(value As String)
            _Operator = value
        End Set
        Get
            Return _Operator
        End Get
    End Property
    Private _IDMaterial As Integer
    <Required(ErrorMessage:="Pilih Material")>
    Property IDMaterial() As Integer
        Set(value As Integer)
            _IDMaterial = value
        End Set
        Get
            Return _IDMaterial
        End Get
    End Property
    Private _IDProject As Int64
    Property IDProject() As Int64
        Set(value As Int64)
            _IDProject = value
        End Set
        Get
            Return _IDProject
        End Get
    End Property
    Private _Description As String
    Property Description() As String
        Set(value As String)
            _Description = value
        End Set
        Get
            Return _Description
        End Get
    End Property
    Public Property BeginProd As TimeSpan
    Public Property EndProd As TimeSpan
End Class
