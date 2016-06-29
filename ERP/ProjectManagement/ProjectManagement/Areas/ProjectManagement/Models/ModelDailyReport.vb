Imports System.ComponentModel
Imports System.ComponentModel.DataAnnotations
Imports System.Globalization

Public Class DailyReport

End Class

' A
<MetadataType(GetType(DailyReportTaskValidation))>
Public Class DailyReportTask
    Public Property DayWork As Integer
End Class

Public Class DailyReportTaskValidation
    <Required(ErrorMessage:="Jenis pekerjaan wajib diisi")> _
    Public Property ProjectTaskDivisionItemId As Integer

    '<RegularExpression("^[0-9]{1,2}([,.][0-9]{1,2})?$", ErrorMessage:="Masukan Volume tidak valid")> _
    <Range(0, Decimal.MaxValue, ErrorMessage:="Masukan Volume tidak valid")> _
    <Required(ErrorMessage:="Volume wajib diisi")> _
    Public Property Volume As Decimal
End Class



' B
<MetadataType(GetType(DailyReportBaseConstructionMaterialUsesValidation))>
Public Class DailyReportBaseConstructionMaterialUse
    Public Property DayWork As Integer
End Class

Public Class DailyReportBaseConstructionMaterialUsesValidation
    <Required(ErrorMessage:="Jenis Material wajib diisi")> _
    Public MaterialName As Integer

    <Required(ErrorMessage:="Satuan wajib diisi")> _
    Public QuantityUnit As String

    <Required(ErrorMessage:="Kuantitas Terpakai wajib diisi")> _
    <Range(0, Decimal.MaxValue, ErrorMessage:="Masukan Kuantitas Terpakai tidak valid")> _
    Public QuantityUse As Decimal

    <Required(ErrorMessage:="Kuantitas Didatangkan wajib diisi")> _
    <Range(0, Decimal.MaxValue, ErrorMessage:="Masukan Kuantitas Didatangkan tidak valid")> _
    Public QuantityImported As Decimal
End Class


' C
<MetadataType(GetType(DailyReportEquipmentUseValidation))>
Public Class DailyReportEquipmentUse
    Public Property DayWork As Integer
End Class

Public Class DailyReportEquipmentUseValidation
    <Required(ErrorMessage:="Nama Alat wajib diisi")> _
    Public Property EquipmentName As String

    <Required(ErrorMessage:="Jumlah Alat wajib diisi")> _
    <Range(0, Decimal.MaxValue, ErrorMessage:="Masukan jumlah tidak valid")> _
    Public Property Amount As Integer

    <Required(ErrorMessage:="Satuan wajib diisi")> _
    Public Property MeasurementUnit As String

    <Required(ErrorMessage:="Kondisi wajib diisi")> _
    Public Property Condition As String
End Class


' D
<MetadataType(GetType(DailyReportLaborUseValidation))>
Public Class DailyReportLaborUse
    Public Property DayWork As Integer
End Class

Public Class DailyReportLaborUseValidation
    <Required(ErrorMessage:="Nama Jabatan wajib diisi")> _
    Public Property Position As Integer

    <Range(0, Decimal.MaxValue, ErrorMessage:="Masukan jumlah tidak valid")> _
    <Required(ErrorMessage:="Jumlah wajib diisi")> _
    Public Property Amount As Decimal

    <Required(ErrorMessage:="Satuan wajib diisi")> _
    Public Property Unit As String
End Class


' E
Public Class DailyReportFieldPersonnelUse
    Public Property DayWork As Integer
End Class

Public Class DailyReportFieldPersonnelUseModel

    Public Property DayWork As Integer

    <Required(ErrorMessage:="Petugas wajib diisi")> _
    Public Property PositionTypeName As String

    Public Property ID As List(Of Long)

    <Required(ErrorMessage:="Posisi Jabatan wajib diisi")> _
    Public Property Position As List(Of String)

    <Required(ErrorMessage:="Jumlah wajib diisi")> _
    Public Property Amount As List(Of Integer)

End Class

' F
Public Class DailyReportIncidenceOfInhibitorActivity
    Public Property DayWork As Integer
End Class


' G
<MetadataType(GetType(GetDailyReportFor_G_ResultValidation))>
Public Class GetDailyReportFor_G_Result
    Public Property DayWork As Integer
End Class

Public Class GetDailyReportFor_G_ResultValidation
    <Required(ErrorMessage:="Saran/Instruksi/Tanggapan wajib diisi")> _
    Public Property Value As String

End Class