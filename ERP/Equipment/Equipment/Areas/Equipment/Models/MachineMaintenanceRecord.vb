Imports System.ComponentModel
Imports System.ComponentModel.DataAnnotations
<MetadataType(GetType(MachineMaintenanceRecordDataValidation))>
Public Class MachineMaintenanceRecord

End Class
Public Class MachineMaintenanceRecordDataValidation
    <Required(ErrorMessage:="Tanggal mulai perawatan harus diisi")> _
    Public [MaintenanceDateStart] As Date
    <Required(ErrorMessage:="Tanggal akhir perawatan harus diisi")> _
    Public [MaintenanceDateEnd] As Date

    <Required(ErrorMessage:="Kendaraan harus diisi")> _
    <Range(1, Integer.MaxValue, ErrorMessage:="Kode Kendaraan Harus diisi")> _
    Public [MachineId] As Integer

End Class
