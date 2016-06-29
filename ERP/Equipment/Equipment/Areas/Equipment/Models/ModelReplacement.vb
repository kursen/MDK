Imports System.ComponentModel
Imports System.ComponentModel.DataAnnotations
Imports System.Globalization
<MetadataType(GetType(ModelReplacement))>
Public Class Replacement
End Class
Public Class ModelReplacement
    Public Property IDEqp As Integer
    <Required(ErrorMessage:="Spare Part wajib diisi.")> _
    Public Property IDSparePart As Integer
    <Required(ErrorMessage:="Biaya wajib diisi.")> _
    Public Property Cost As Integer
    <Required(ErrorMessage:="Jumlah wajib diisi.")> _
    Public Property Amount As Integer
    <Required(ErrorMessage:="Kegiatan wajib diisi.")> _
    Public Property Activity As String
    <Required(ErrorMessage:="Tanggal Mulai Polisi wajib diisi.")> _
    Public Property [Date] As Date
    Public Property Remarks As String
End Class

