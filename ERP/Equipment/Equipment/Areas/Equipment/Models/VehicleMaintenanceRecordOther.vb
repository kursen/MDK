Imports System.ComponentModel
Imports System.ComponentModel.DataAnnotations
Imports System.Globalization
<MetadataType(GetType(VehicleMaintenanceRecordOtherDataValidation))>
Public Class VehicleMaintenanceRecordOther

End Class
Public Class VehicleMaintenanceRecordOtherDataValidation
    <Required(ErrorMessage:="Poin pekerjaan harus diisi.")> _
    Public [Item] As String
    <Required(ErrorMessage:="Biaya pekerjaan harus diisi.")> _
    Public [Cost] As Double

End Class