Imports System.ComponentModel
Imports System.ComponentModel.DataAnnotations
<MetadataType(GetType(ModelVehicleMaintenanceCheckItem))>
Public Class VehicleMaintenanceCheckItem

End Class
Public Class ModelVehicleMaintenanceCheckItem
    <Required(ErrorMessage:="Item Pekerjaan wajib diisi.")> _
    Public Property ItemName As String

    <Required(ErrorMessage:="Nilai Jadwal wajib diisi.")> _
    Public Property Value As Decimal

    <Required(ErrorMessage:="Waktu Perencanaan wajib diisi.")> _
    Public Property UnitValue As String

    <Required(ErrorMessage:="KM terakhir wajib diisi.")> _
    Public Property LastCheck_Kilometer As Decimal

    <Required(ErrorMessage:="Tanggal Cek KM Terakhir wajib diisi.")> _
    Public Property LastCheck_Date As DateTime
End Class
