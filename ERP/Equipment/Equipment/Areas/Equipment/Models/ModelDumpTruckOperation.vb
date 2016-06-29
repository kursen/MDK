Imports System.ComponentModel
Imports System.ComponentModel.DataAnnotations
Imports System.Globalization
<MetadataType(GetType(ModelDumpTruckOperation))> _
Public Class DumpTruckOperation

End Class
Public Class ModelDumpTruckOperation
    Public Property ID As Integer
    Public Property IDActivity As Integer
    <Required(ErrorMessage:="Jenis Muatan wajib diisi.")> _
    Public Property LoadType As String

    <Required(ErrorMessage:="Lokasi Asal wajib diisi.")> _
    Public Property SourceLocation As String

    <Required(ErrorMessage:="Waktu Berangkat wajib diisi.")> _
    Public Property DepartureTime As TimeSpan

    <Required(ErrorMessage:="Waktu Tiba wajib diisi.")> _
    Public Property ArrivalTime As TimeSpan

    <Required(ErrorMessage:="Lokasi Tujuan wajib diisi.")> _
    Public Property Destination As String

    <Required(ErrorMessage:="Jarak wajib diisi.")> _
    Public Property Distance As Decimal
End Class
