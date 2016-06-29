Imports System.ComponentModel
Imports System.ComponentModel.DataAnnotations
Imports System.Globalization
<MetadataType(GetType(MachineMaintenanceRecordOtherDataValidation))>
Public Class MachineMaintenanceRecordOther

End Class
Public Class MachineMaintenanceRecordOtherDataValidation
    <Required(ErrorMessage:="Poin pekerjaan harus diisi.")> _
    Public [Item] As String
    <Required(ErrorMessage:="Biaya pekerjaan harus diisi.")> _
    Public [Cost] As Double

End Class