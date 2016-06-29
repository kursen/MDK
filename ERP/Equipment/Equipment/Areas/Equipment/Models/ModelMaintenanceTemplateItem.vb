Imports System.ComponentModel
Imports System.ComponentModel.DataAnnotations
Imports System.Globalization

<MetadataType(GetType(ModelMaintenanceTemplateItem))>
Public Class MaintenanceTemplateItem

End Class
Public Class ModelMaintenanceTemplateItem
    Public Property ID As Integer
    <Display(Name:="Point Pemeriksaan")> _
    <Required(ErrorMessage:="{0} tidak boleh kosong")> _
    Public Property Item As String
    <Required(ErrorMessage:="Nilai tidak boleh kosong")> _
    <Range(0, 9999, ErrorMessage:="{0} must be a decimal/number")> _
    Public Property Value As Double
    Public Property Unit As String
    Public Property MachineTypeID As Integer
End Class

Public Class MaintenanceTemplateItemView
    Inherits MaintenanceTemplateItem
    Public Property MachineTypeName As String
End Class

Enum MaintenanceTemplateUnit
    KM = 0
    HM = 0
    Minggu = 1
    Bulan = 2
    Tahun = 3


End Enum

