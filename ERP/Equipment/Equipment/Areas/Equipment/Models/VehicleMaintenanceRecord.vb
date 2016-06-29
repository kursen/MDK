Imports System.ComponentModel
Imports System.ComponentModel.DataAnnotations
<MetadataType(GetType(VehicleMaintenanceRecordDataValidation))>
Public Class VehicleMaintenanceRecord

End Class
Public Class VehicleMaintenanceRecordDataValidation
    <Required(ErrorMessage:="Tanggal mulai perawatan harus diisi")> _
    Public [MaintenanceDateStart] As Date
    <Required(ErrorMessage:="Tanggal akhir perawatan harus diisi")> _
    Public [MaintenanceDateEnd] As Date

    <Required(ErrorMessage:="Kendaraan harus diisi")> _
    <Range(1, Integer.MaxValue, ErrorMessage:="Kode Kendaraan Harus diisi")> _
    Public [VehicleId] As Integer

    <Required(ErrorMessage:="Angka kilometer harus diisi")> _
     <Range(1D, Double.MaxValue, ErrorMessage:="Angka kilometer harus diisi")> _
    Public [OdometerValue] As Double

End Class
