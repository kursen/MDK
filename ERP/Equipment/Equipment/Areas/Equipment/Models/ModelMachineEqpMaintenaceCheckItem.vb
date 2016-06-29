Imports System.ComponentModel
Imports System.ComponentModel.DataAnnotations
<MetadataType(GetType(ModelMachineEqpMaintenaceCheckItem))>
Public Class MachineEqpMaintenanceCheckItem

End Class
Public Class ModelMachineEqpMaintenaceCheckItem
    <Required(ErrorMessage:="Item Pekerjaan wajib diisi.")> _
    Public Property ItemName As String

    <Required(ErrorMessage:="Nilai Jadwal wajib diisi.")> _
    Public Property Value As Decimal

    <Required(ErrorMessage:="Waktu Perencanaan wajib diisi.")> _
    Public Property UnitValue As String

    <Required(ErrorMessage:="Tanggal Terakhir wajib diisi.")> _
    Public Property LastCheck_Date As DateTime
End Class
