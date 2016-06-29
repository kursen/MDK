Imports System.ComponentModel
Imports System.ComponentModel.DataAnnotations
Imports System.Globalization

<MetadataType(GetType(DistributionJournalsValidation))>
Public Class DistributionJournals
    Protected ctx As ERPEntities
    Public Sub New()
        ctx = New ERPEntities
    End Sub

    Public Sub Save()
        If Weight1 IsNot Nothing AndAlso Weight2 IsNot Nothing Then
            If Weight1 > Weight2 Then
                IdDeliveryStatus = 1
            Else
                IdDeliveryStatus = 2
            End If
            Dim modelInventory As New MaterialInventories With {
                .IdInventoryStatus = IdDeliveryStatus + 1,
                .IsPlus = (IdDeliveryStatus = 1)
            }
            ctx.MaterialInventories.AddObject(modelInventory)
            ctx.SaveChanges()
            IDInventory = modelInventory.ID
        End If

        ctx.DistributionJournals.AddObject(Me)
        If Me.ID > 0 Then
            ctx.ObjectStateManager.ChangeObjectState(Me, EntityState.Modified)
        End If
        ctx.SaveChanges()
    End Sub
End Class
Public Class DistributionJournalsValidation
    Private _IdWeigher As Byte
    Public Property IdWeigher() As Byte
        Get
            Return _IdWeigher
        End Get
        Set(value As Byte)
            _IdWeigher = value
        End Set
    End Property
    Private _NoRec As String
    Public Property NoRec() As String
        Get
            Return _NoRec
        End Get
        Set(value As String)
            _NoRec = value
        End Set
    End Property

    Private _IDInventory As Int64
    Public Property IDInventory() As Int64
        Get
            Return _IDInventory
        End Get
        Set(value As Int64)
            _IDInventory = value
        End Set
    End Property

    Private _InTime As DateTime
    Public Property InTime() As DateTime
        Get
            Return _InTime
        End Get
        Set(value As DateTime)
            _InTime = value
        End Set
    End Property
    Private _OutTime As DateTime
    <Required(ErrorMessage:="Isi Tanggal Keluar")>
    Public Property OutTime() As DateTime
        Get
            Return _OutTime
        End Get
        Set(value As DateTime)
            _OutTime = value
        End Set
    End Property
    Private _PoliceLicense As String
    Public Property PoliceLicense() As String
        Get
            Return _PoliceLicense
        End Get
        Set(value As String)
            _PoliceLicense = value
        End Set
    End Property

    Private _DriverName As String
    Public Property DriverName() As String
        Get
            Return _DriverName
        End Get
        Set(value As String)
            _DriverName = value
        End Set
    End Property
    Private _IdDeliveryStatus As Byte
    Public Property IdDeliveryStatus() As Byte
        Get
            Return _IdDeliveryStatus
        End Get
        Set(value As Byte)
            _IdDeliveryStatus = value
        End Set
    End Property
    Private _IdCompany As Integer
    Public Property IdCompany() As Integer
        Get
            Return _IdCompany
        End Get
        Set(value As Integer)
            _IdCompany = value
        End Set
    End Property

    Private _IdMaterial As Integer
    Public Property IdMaterial() As Integer
        Get
            Return _IdMaterial
        End Get
        Set(value As Integer)
            _IdMaterial = value
        End Set
    End Property

    Private _Weight1 As Double
    Public Property Weight1() As Double
        Get
            Return _Weight1
        End Get
        Set(value As Double)
            _Weight1 = value
        End Set
    End Property

    Private _Weight2 As Double
    <Required(ErrorMessage:="Isi Berat Kedua")>
    Public Property Weight2() As Double
        Get
            Return _Weight2
        End Get
        Set(value As Double)
            _Weight2 = value
        End Set
    End Property
    Private _IdMeasurementUnit As Byte
    Public Property IdMeasurementUnit() As Byte
        Get
            Return _IdMeasurementUnit
        End Get
        Set(value As Byte)
            _IdMeasurementUnit = value
        End Set
    End Property
    Private _Place As String
    Public Property Place() As String
        Get
            Return _Place
        End Get
        Set(value As String)
            _Place = value
        End Set
    End Property
    Private _Description As String
    '<Required(ErrorMessage:="Isi Keterangan")>
    Public Property Description() As String
        Get
            Return _Description
        End Get
        Set(value As String)
            _Description = value
        End Set
    End Property

    Private _Clerk1 As String
    <Required(ErrorMessage:="Isi Clerk1")>
    Public Property Clerk1() As String
        Get
            Return _Clerk1
        End Get
        Set(value As String)
            _Clerk1 = value
        End Set
    End Property

    Private _Clerk2 As String
    <Required(ErrorMessage:="Isi Clerk2")>
    Public Property Clerk2() As String
        Get
            Return _Clerk2
        End Get
        Set(value As String)
            _Clerk2 = value
        End Set
    End Property

    Private _Copy As Integer
    <Required(ErrorMessage:="Isi Copy")>
    Public Property Copy() As Integer
        Get
            Return _Copy
        End Get
        Set(value As Integer)
            _Copy = value
        End Set
    End Property
End Class