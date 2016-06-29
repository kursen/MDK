Imports System.ComponentModel
Imports System.ComponentModel.DataAnnotations
<MetadataType(GetType(ModelMachineEqpMaintenanceCheck))>
Public Class MachineEqpMaintenanceCheck

End Class
Public Class ModelMachineEqpMaintenanceCheck

    <Required(ErrorMessage:="Nilai Jadwal wajib diisi.")> _
    Public Property AverageHourMeter As Decimal

End Class
