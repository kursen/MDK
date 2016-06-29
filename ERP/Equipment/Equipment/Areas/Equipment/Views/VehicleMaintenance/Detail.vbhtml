@MOdelType Equipment.VehicleMaintenanceRecord
@Code
    ViewData("Title") = "Detail"
    Dim status As String = ""
    If Model.MaintenanceState = 2 Then
        status = "Selesai"
    ElseIf Model.MaintenanceState = 1 Then
        status = "Sedang Dikerjakan"
    Else
        status = "Menunggu"
    End If
End Code
<div class="row">
    <div class="col-lg-12 col-sm-12">
        <div class="pull-right">
            <div class="button-group">
                <a   href="@Url.Action("Edit", "VehicleMaintenance", New With {.id = Model.Id})"class="btn btn-danger btn-label-left" >
                    <span><i class="fa fa-edit"></i></span>Edit</a> <a href="@Url.Action("Index", "VehicleMaintenance")" class="btn btn-danger btn-label-left">
                        <span><i class="fa fa-arrow-left"></i></span>Kembali</a> <a href="@Url.Action("ReportVehicleMaintenance", "Report", New With {.id = Model.Id})" class="btn btn-danger btn-label-left">
                            <span><i class="fa fa-print"></i></span>Print</a>
            </div>
        </div>
    </div>
</div>
@Using Html.BeginJUIBox("Detail Kegiatan Kendaraan")
    @<div class="row">
        <div class="col-sm-12 col-lg-12">
            <div class="alert alert-info">
                <h3>
                    <u>Tanggal Perawatan</u></h3>
                <br />
                <label>
                    Tanggal</label>
                : @Model.MaintenanceDateStart.ToString("dd MMMM yyyy HH:mm") <strong>-</strong> @Model.MaintenanceDateEnd.ToString("dd MMMM yyyyy HH:mm")
            </div>
            <table class="table table-bordered">
                <thead>
                    <tr>
                        <th>
                            Kode Kendaraan
                        </th>
                        <th>
                            Jenis
                        </th>
                        <th>
                            No. Polisi
                        </th>
                        <th>
                            MEREK / TIPE
                        </th>
                        <th>
                            NAMA SUPIR
                        </th>
                        <th>
                            STATUS
                        </th>
                    </tr>
                </thead>
                <tbody>
                    <tr class="text-center">
                        <td>@Model.Vehicle.Code
                        </td>
                        <td>@Model.Vehicle.Species
                        </td>
                        <td>@Model.Vehicle.PoliceNumber
                        </td>
                        <td>
                            @Model.Vehicle.Merk / @Model.Vehicle.Type
                        </td>
                        <td>@Model.Vehicle.DriverName
                        </td>
                        <td>
                            <label class="label label-info">@status</label>
                        </td>
                    </tr>
                </tbody>
            </table>
        </div>
    </div>
    @<div class="row">
        <div class="col-lg-12 col-sm-12">
            <div class="panel panel-primary">
                <div class="panel-heading">
                    Item Pekerjaan</div>
                <div class="panel-body" style="min-height: 100px;">
                    <table class="table table-bordered">
                        <colgroup>
                            @code
    Dim colWidth = {"60px", "auto"}
    For Each item In colWidth
                                @<col style="width:@item" />
    Next
                            End Code
                        </colgroup>
                        <thead>
                            <tr>
                                <th>
                                    No.
                                </th>
                                <th>
                                    Item Pekerjaan
                                </th>
                            </tr>
                        </thead>
                        <tbody>
                            @code
    Dim counter As Integer = 1
                            End Code
                            @For Each item In Model.VehicleMaintenanceRecordItems
                                @<tr>
                                    <td>@counter
                                    </td>
                                    <td>@item.Item
                                    </td>
                                </tr>
        counter += 1
    Next
                            @code 
    If Model.VehicleMaintenanceRecordItems.Count = 0 Then
                                @<tr>
                                    <td colspan="2">
                                        [Tidak ada data]
                                    </td>
                                </tr>
    End If
                            End Code
                        </tbody>
                    </table>
                </div>
            </div>
        </div>
        
    </div>
    @<div class="row">
    <div class="col-lg-12 col-sm-12">
            <div class="panel panel-primary">
                <div class="panel-heading">
                    Material Suku Cadang</div>
                <div class="panel-body" style="min-height: 100px;">
                    <table class="table table-bordered">
                        <thead>
                            <tr>
                                <th>
                                    No.
                                </th>
                                <th>
                                    Item
                                </th>
                                <th>
                                    Jumlah
                                </th>
                                <th>
                                    Satuan
                                </th>
                            </tr>
                        </thead>
                        <tbody>
                            @code
    Dim number As Integer = 1
                            End Code
                            @For Each item In Model.VehicleMaintenanceRecordMaterialUseds
                                @<tr>
                                    <td>
                                        @number
                                    </td>
                                    <td>
                                        @item.MaterialUsed
                                    </td>
                                    <td class="text-right">
                                        @item.Quantity
                                    </td>
                                    <td>
                                        @item.UnitQuantity
                                    </td>
                                </tr>
        number += 1
    Next
                            @code
    number = 1
                            End Code
                            @If Model.VehicleMaintenanceRecordMaterialUseds.Count = 0 Then
                                @<tr>
                                    <td colspan="4">
                                        -
                                    </td>
                                </tr>
                                
    End If
                        </tbody>
                    </table>
                </div>
            </div>
        </div>
    </div>
    @<div class="row">
        <div class="col-lg-12 col-sm-12">
            <div class="panel panel-primary">
                <div class="panel-heading">
                    Lainnya</div>
                <div class="panel-body" style="min-height: 100px;">
                    <table class="table table-bordered">
                        <thead>
                            <tr>
                                <th>
                                    No.
                                </th>
                                <th>
                                    Item
                                </th>
                                <th>
                                    Biaya
                                </th>
                                <th>
                                    Catatan
                                </th>
                            </tr>
                        </thead>
                        <tbody>
                            @For Each item In Model.VehicleMaintenanceRecordOthers
                                @<tr>
                                    <td>
                                        @number
                                    </td>
                                    <td>
                                        @item.Item
                                    </td>
                                    <td class="text-right">
                                        @item.Cost
                                    </td>
                                    <td>
                                        @item.Remarks
                                    </td>
                                </tr>
        number += 1
    Next
                            @If Model.VehicleMaintenanceRecordOthers.Count = 0 Then
                                @<tr>
                                    <td colspan="4">
                                        -
                                    </td>
                                </tr>
    End If
                        </tbody>
                    </table>
                </div>
            </div>
        </div>
    </div>
  
   
End Using

