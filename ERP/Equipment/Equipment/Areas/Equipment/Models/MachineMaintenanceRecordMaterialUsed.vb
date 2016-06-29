Imports System.ComponentModel
Imports System.ComponentModel.DataAnnotations
Imports System.Globalization
<MetadataType(GetType(MachineMaintenanceRecordMaterialUsedDataValidation))>
Public Class MachineMaintenanceRecordMaterialUsed

End Class
Public Class MachineMaintenanceRecordMaterialUsedDataValidation
    <Required(ErrorMessage:="Item pekerjaan harus diisi.")> _
    Public [MaterialUsed] As String
    <Required(ErrorMessage:="Jumlah harus diisi.")> _
    <Range(0.01, Decimal.MaxValue, ErrorMessage:="Jumlah tidak boleh 0")> _
    Public [Quantity] As Decimal
    <Required(ErrorMessage:="Satuan harus diisi.")> _
    Public [UnitQuantity] As String

End Class