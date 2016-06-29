Imports System.ComponentModel
Imports System.ComponentModel.DataAnnotations
Imports System.Globalization

<MetadataType(GetType(TblTrans_Penimbangan2Validation))>
Public Class TblTrans_Penimbangan2
    Protected ctx As ERPEntities
    Public Sub New()
        ctx = New ERPEntities
    End Sub

    Public Sub Save()
        ctx.TblTrans_Penimbangan2.AddObject(Me)
        If IsNothing(Me.NoRecord) Then
            ctx.ObjectStateManager.ChangeObjectState(Me, EntityState.Modified)
        End If
        ctx.SaveChanges()
    End Sub
End Class
Public Class TblTrans_Penimbangan2Validation
    Private _Norecord As String
    <Required(ErrorMessage:="Norecord Tidak Boleh Kosong")>
    Public Property NoRecord() As String
        Get
            Return _Norecord
        End Get
        Set(value As String)
            _Norecord = value
        End Set
    End Property

    Private _TglMasuk As DateTime
    <Required(ErrorMessage:="Tanggal Mulai Tidak Boleh Kosong")>
    Public Property TglMasuk() As DateTime
        Get
            Return _TglMasuk
        End Get
        Set(value As DateTime)
            _TglMasuk = value
        End Set
    End Property

    Private _TglKeluar As DateTime
    <Required(ErrorMessage:="Tanggal Akhir Tidak Boleh Kosong")>
    Public Property TglKeluar() As DateTime
        Get
            Return _TglKeluar
        End Get
        Set(value As DateTime)
            _TglKeluar = value
        End Set
    End Property

    Private _NoPolisi As String
    <Required(ErrorMessage:="No Polisi Tidak Boleh Kosong")>
    Public Property NoPolisi() As String
        Get
            Return _NoPolisi
        End Get
        Set(value As String)
            _NoPolisi = value
        End Set
    End Property

    Private _Sopir As String
    <Required(ErrorMessage:="Nama Sopir Tidak Boleh Kosong")>
    Public Property Sopir() As String
        Get
            Return _Sopir
        End Get
        Set(value As String)
            _Sopir = value
        End Set
    End Property

    Private _KodePeru As Integer
    <Required(ErrorMessage:="Perushaan Tidak Boleh Kosong")>
    Public Property KodePeru() As Integer
        Get
            Return _KodePeru
        End Get
        Set(value As Integer)
            _KodePeru = value
        End Set
    End Property

    Private _KodeBrg As Integer
    <Required(ErrorMessage:="Material Tidak Boleh Kosong")>
    Public Property KodeBrg() As Integer
        Get
            Return _KodeBrg
        End Get
        Set(value As Integer)
            _KodeBrg = value
        End Set
    End Property

    Private _Berat1 As Integer
    <Required(ErrorMessage:="Berat Pertama Tidak Boleh Kosong")>
    Public Property Berat1() As Integer
        Get
            Return _Berat1
        End Get
        Set(value As Integer)
            _Berat1 = value
        End Set
    End Property

    Private _Berat2 As Integer
    <Required(ErrorMessage:="Berat Kedua Tidak Boleh Kosong")>
    Public Property Berat2() As Integer
        Get
            Return _Berat2
        End Get
        Set(value As Integer)
            _Berat2 = value
        End Set
    End Property

    Private _DeliveryNote As String
    <Required(ErrorMessage:="Silahkan Isi Catatan Pengiriman")>
    Public Property DeliveryNote() As String
        Get
            Return _DeliveryNote
        End Get
        Set(value As String)
            _DeliveryNote = value
        End Set
    End Property

    Private _Clerk1 As String
    <Required(ErrorMessage:="Clerk1 Tidak Boleh Kosong")>
    Public Property Clerk1() As String
        Get
            Return _Clerk1
        End Get
        Set(value As String)
            _Clerk1 = value
        End Set
    End Property

    Private _Clerk2 As String
    <Required(ErrorMessage:="Clerk2 Tidak Boleh Kosong")>
    Public Property Clerk2() As String
        Get
            Return _Clerk2
        End Get
        Set(value As String)
            _Clerk2 = value
        End Set
    End Property

    Private _Copy As Integer
    <Required(ErrorMessage:="Copy Tidak Boleh Kosong")>
    Public Property Copy() As Integer
        Get
            Return _Copy
        End Get
        Set(value As Integer)
            _Copy = value
        End Set
    End Property
End Class