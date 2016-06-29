Imports System.ComponentModel
Imports System.ComponentModel.DataAnnotations
Imports System.Globalization
Public Class ModelCrusherJournal
    Property ID As Long

    <Required(ErrorMessage:="Wajib diisi")> _
    Public Property WorkDate As DateTime

    Private _IDWorkSchedule As Integer
    <Required(ErrorMessage:="Wajib dipilih")> _
    Public Property IDWorkSchedule As Integer
        Set(value As Integer)
            _IDWorkSchedule = value
        End Set
        Get
            Return _IDWorkSchedule
        End Get
    End Property

    Private _OperatorName As String
    <Required(ErrorMessage:="Isi nama operator")> _
    Public Property OperatorName As String
        Set(value As String)
            _OperatorName = value
        End Set
        Get
            Return _OperatorName
        End Get
    End Property

    Private _IDMachine As Byte
    <Required(ErrorMessage:="Pilih mesin crusher")> _
    Public Property IDMachine As Byte
        Set(value As Byte)
            _IDMachine = value
        End Set
        Get
            Return _IDMachine
        End Get
    End Property

    Private _InputGrogol As Integer
    <Required(ErrorMessage:="Isi jumlah input grogol")> _
    Public Property InputGrogol As Integer
        Set(value As Integer)
            _InputGrogol = value
        End Set
        Get
            Return _InputGrogol
        End Get
    End Property
    Private _Description As String
    Public Property Description As String
        Set(value As String)
            _Description = value
        End Set
        Get
            Return _Description
        End Get
    End Property

    Private _MediumAmount As Integer
    <Required(ErrorMessage:="Wajib Diisi")> _
    Public Property MediumAmount() As Integer
        Set(value As Integer)
            _MediumAmount = value
        End Set
        Get
            Return _MediumAmount
        End Get
    End Property

    Private _AbuBatuAmount As Integer
    <Required(ErrorMessage:="Wajib Diisi")> _
    Public Property AbuBatuAmount() As Integer
        Set(value As Integer)
            _AbuBatuAmount = value
        End Set
        Get
            Return _AbuBatuAmount
        End Get
    End Property

    Private _BaseAAmount As Integer
    <Required(ErrorMessage:="Wajib Diisi")> _
    Public Property BaseAAmount() As Integer
        Set(value As Integer)
            _BaseAAmount = value
        End Set
        Get
            Return _BaseAAmount
        End Get
    End Property

    Private _BaseBAmount As Integer
    <Required(ErrorMessage:="Wajib Diisi")> _
    Public Property BaseBAmount() As Integer
        Set(value As Integer)
            _BaseBAmount = value
        End Set
        Get
            Return _BaseBAmount
        End Get
    End Property

    Private _Split12Amount As Integer
    <Required(ErrorMessage:="Wajib Diisi")> _
    Public Property Split12Amount() As Integer
        Set(value As Integer)
            _Split12Amount = value
        End Set
        Get
            Return _Split12Amount
        End Get
    End Property

    Private _Split23Amount As Integer
    <Required(ErrorMessage:="Wajib Diisi")> _
    Public Property Split23Amount() As Integer
        Set(value As Integer)
            _Split23Amount = value
        End Set
        Get
            Return _Split23Amount
        End Get
    End Property

    Private _InputGresley As Nullable(Of Integer)
    Public Property InputGresley() As Nullable(Of Integer)
        Set(value As Nullable(Of Integer))
            _InputGresley = value
        End Set
        Get
            Return _InputGresley
        End Get
    End Property

    Private _GresleyOutAmount As Integer
    <Required(ErrorMessage:="Wajib Diisi")> _
    Public Property GresleyOutAmount() As Integer
        Set(value As Integer)
            _GresleyOutAmount = value
        End Set
        Get
            Return _GresleyOutAmount
        End Get
    End Property
End Class
