Imports System.ComponentModel
Imports System.ComponentModel.DataAnnotations
Imports System.Globalization

<MetadataType(GetType(ModelMstmaterials))>
Public Class MstMaterials
    Protected ctx As ERPEntities

    Public Sub New()
        ctx = New ERPEntities
    End Sub

    Public Sub Save()
        ctx.MstMaterials.AddObject(Me)
        If Me.ID > 0 Then
            ctx.ObjectStateManager.ChangeObjectState(Me, EntityState.Modified)
        End If
        ctx.SaveChanges()
    End Sub
End Class

Public Class ModelMstmaterials
    Private _Code As String
    <Required(ErrorMessage:="Wajib diisi")> _
    Public Property Code() As String
        Set(value As String)
            _Code = value
        End Set
        Get
            Return _Code
        End Get
    End Property

    Private _Symbol As String
    <Required(ErrorMessage:="Wajib diisi")> _
    Public Property Symbol() As String
        Set(value As String)
            _Symbol = value
        End Set
        Get
            Return _Symbol
        End Get
    End Property

    Private _Name As String
    <Required(ErrorMessage:="Wajib diisi")> _
    Public Property Name() As String
        Set(value As String)
            _Name = value
        End Set
        Get
            Return _Name
        End Get
    End Property

    Private _IdMaterialType As Byte
    Public Property IdMaterialType() As Byte
        Set(value As Byte)
            _IdMaterialType = value
        End Set
        Get
            Return _IdMaterialType
        End Get
    End Property
    Private _IdMeasurementUnit As Byte
    <Required(ErrorMessage:="Pilih Satuan")> _
    Public Property IdMeasurementUnit() As Byte
        Set(value As Byte)
            _IdMeasurementUnit = value
        End Set
        Get
            Return _IdMeasurementUnit
        End Get
    End Property
    Private _InitialStock As Double
    <RegularExpression("[0-9]*\.?[0-9]+", ErrorMessage:="Wajib diisi angka")> _
    <Range(0, 100000, ErrorMessage:="Bilangan antara 0 hingga 100.000")> _
    Public Property InitialStock() As Double
        Set(value As Double)
            _InitialStock = value
        End Set
        Get
            Return _InitialStock
        End Get
    End Property
    Private _DateInitialStock As DateTime
    Public Property DateInitialStock() As DateTime
        Set(value As DateTime)
            _DateInitialStock = value
        End Set
        Get
            Return _DateInitialStock
        End Get
    End Property
    Private _isInventory As Boolean
    Public Property isInventory() As Boolean
        Set(value As Boolean)
            _isInventory = value
        End Set
        Get
            Return _isInventory
        End Get
    End Property
    Private _IDMachineType As Byte
    Public Property IDMachineType() As Byte
        Set(value As Byte)
            _IDMachineType = value
        End Set
        Get
            Return _IDMachineType
        End Get
    End Property

End Class
