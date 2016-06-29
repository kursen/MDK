Imports System.ComponentModel
Imports System.ComponentModel.DataAnnotations
Imports System.Globalization
Public Class ModelTimbangan
    Private _TglMasuk As DateTime
    <Required(ErrorMessage:="Wajib diisi")>
    Public Property TglMasuk() As DateTime
        Get
            Return _TglMasuk
        End Get
        Set(value As DateTime)
            _TglMasuk = value
        End Set
    End Property

    Private _NoPol1 As String
    <Required(ErrorMessage:="Wajib diisi")>
    <RegularExpression("^[A-Za-z]+$", ErrorMessage:="Harus Huruf")>
    Public Property NoPol1() As String
        Get
            Return _NoPol1
        End Get
        Set(value As String)
            _NoPol1 = value
        End Set
    End Property
    Private _NoPol2 As Integer
    <Required(ErrorMessage:="Wajib diisi")>
    Public Property NoPol2() As Integer
        Get
            Return _NoPol2
        End Get
        Set(value As Integer)
            _NoPol2 = value
        End Set
    End Property
    Private _NoPol3 As String
    <Required(ErrorMessage:="Wajib diisi")>
    <RegularExpression("^[A-Za-z]+$", ErrorMessage:="Harus dengan Huruf")>
    Public Property NoPol3() As String
        Get
            Return _NoPol3
        End Get
        Set(value As String)
            _NoPol3 = value
        End Set
    End Property
    Private _Sopir As String
    <Required(ErrorMessage:="Wajib diisi")>
    Public Property Sopir() As String
        Get
            Return _Sopir
        End Get
        Set(value As String)
            _Sopir = value
        End Set
    End Property

    Private _KodePeru As Integer
    <Required(ErrorMessage:="Wajib diisi")>
    Public Property KodePeru() As Integer
        Get
            Return _KodePeru
        End Get
        Set(value As Integer)
            _KodePeru = value
        End Set
    End Property

    Private _KodeBrg As Integer
    '<Required(ErrorMessage:="No Polisi Tidak Boleh Kosong")>
    Public Property KodeBrg() As Integer
        Get
            Return _KodeBrg
        End Get
        Set(value As Integer)
            _KodeBrg = value
        End Set
    End Property

    Private _Berat1 As Integer
    <Required(ErrorMessage:="Berat Pertama Tidak Boleh Kosong")> _
    <RegularExpression("[0-9]*\.?[0-9]+", ErrorMessage:="Wajib diisi angka")> _
    Public Property Berat1() As Integer
        Get
            Return _Berat1
        End Get
        Set(value As Integer)
            _Berat1 = value
        End Set
    End Property

    Private _Deliverynote As String
    '<Required(ErrorMessage:="Isi Catatan Pengiriman")>
    Public Property Deliverynote() As String
        Get
            Return _Deliverynote
        End Get
        Set(value As String)
            _Deliverynote = value
        End Set
    End Property

    Private _Clerk1 As String
    <Required(ErrorMessage:="Clerk 1 Tidak Boleh Kosong")>
    Public Property Clerk1() As String
        Get
            Return _Clerk1
        End Get
        Set(value As String)
            _Clerk1 = value
        End Set
    End Property

End Class
