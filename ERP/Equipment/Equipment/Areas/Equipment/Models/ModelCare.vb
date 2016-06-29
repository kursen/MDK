Imports System.ComponentModel
Imports System.ComponentModel.DataAnnotations
Imports System.Globalization
<MetadataType(GetType(ModelCare))>
Public Class Care

End Class
Public Class ModelCare
    Public Property IDEqp As Integer
    <Required(ErrorMessage:="Biaya wajib diisi.")> _
    Public Property Cost As Integer
    <Required(ErrorMessage:="Kegiatan wajib diisi.")> _
     Public Property Activity As String
    <Required(ErrorMessage:="Tanggal Mulai Polisi wajib diisi.")> _
    Public Property [Date] As Date
    Public Property Remarks As String
End Class
