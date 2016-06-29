Imports System.ComponentModel
Imports System.ComponentModel.DataAnnotations
Imports System.Globalization
Public Class ModelEditCrusher
    Private _idCrusher As Int64
    Property idCrusher() As Int64
        Set(value As Int64)
            _idCrusher = value
        End Set
        Get
            Return _idCrusher
        End Get
    End Property
    'Private _idMaterialUseJournal As Int64
    Property idMaterialUserJournal() As Int64()
    '    Set(value As Int64)
    '        _idMaterialUseJournal = value
    '    End Set
    '    Get
    '        Return _idMaterialUseJournal
    '    End Get
    'End Property
    Private _DateUse As DateTime
    <Required(ErrorMessage:="Isi Tanggal")>
    Property DateUse() As DateTime
        Set(value As DateTime)
            _DateUse = value
        End Set
        Get
            Return _DateUse
        End Get
    End Property
    Private _IdWorkSchedule As Integer
    <Required(ErrorMessage:="Pilih Shift")>
    Property IdWorkSchedule() As Integer
        Set(value As Integer)
            _IdWorkSchedule = value
        End Set
        Get
            Return _IdWorkSchedule
        End Get
    End Property
    'Private _Amount As Integer
    <Required(ErrorMessage:="Isi Jumlah")>
    Property Amount() As Integer()
    '    Set(value As Integer)
    '        _Amount = value
    '    End Set
    '    Get
    '        Return _Amount
    '    End Get
    'End Property
    Private _OperatorName As String
    <Required(ErrorMessage:="Isi Nama Operator")>
    Property OperatorName() As String
        Set(value As String)
            _OperatorName = value
        End Set
        Get
            Return _OperatorName
        End Get
    End Property
    Private _IdMachine As Byte
    <Required(ErrorMessage:="Pilih Mesin")>
    Property IdMachine() As Byte
        Set(value As Byte)
            _IdMachine = value
        End Set
        Get
            Return _IdMachine
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
    Property IDMaterial() As String()

    'Private _MaterialName As String()
    Property MaterialName() As String()
    '    Set(value As Integer)
    '        _MaterialName = value
    '    End Set
    '    Get
    '        Return _MaterialName
    '    End Get
    'End Property
End Class
