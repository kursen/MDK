Imports System.ComponentModel
Imports System.ComponentModel.DataAnnotations
Imports System.Globalization
Public Class ampModel
    '<Required(ErrorMessage:="Isi Tanggal")> _
    '<DisplayFormat(DataFormatString:="{0:dd/MM/yyyy}", ApplyFormatInEditMode:=True)> _
    Public Property DateUse As DateTime
    Private _IdMachine As Integer
    <Required(ErrorMessage:="Pilih Mesin AMP")> _
    Public Property IdMachine() As Integer
        Set(value As Integer)
            _IdMachine = value
        End Set
        Get
            Return _IdMachine
        End Get
    End Property
    Private _IDProject As Integer
    <Required(ErrorMessage:="Pilih No. Proyek tujuan")> _
    Public Property IDProject() As Integer
        Set(value As Integer)
            _IDProject = value
        End Set
        Get
            Return _IDProject
        End Get
    End Property

    Private _Operator As String
    <Required(ErrorMessage:="Isi Nama Operator")> _
    <DataType(DataType.Text)> _
    Public Property OperatorName() As String
        Set(value As String)
            _Operator = value
        End Set
        Get
            Return _Operator
        End Get
    End Property
    Private _IdShift As Integer
    Public Property IdShift() As Integer
        Set(value As Integer)
            _IdShift = value
        End Set
        Get
            Return _IdShift
        End Get
    End Property
    Private _IDProduk As Integer
    <Required(ErrorMessage:="Pilih Produk")> _
    Public Property IDProduk() As Integer
        Set(value As Integer)
            _IDProduk = value
        End Set
        Get
            Return _IDProduk
        End Get
    End Property
    Private _Description As String
    Public Property Description() As String
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
