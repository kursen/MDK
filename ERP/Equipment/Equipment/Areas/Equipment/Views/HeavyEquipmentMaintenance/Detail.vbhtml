@MOdelType Equipment.HeavyEqpMaintenanceRecord
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
                <a   href="@Url.Action("Edit", "HeavyEquipmentMaintenance", New With {.id = Model.Id})"class="btn btn-danger btn-label-left" >
                    <span><i class="fa fa-edit"></i></span>Edit</a> <a href="@Url.Action("Index", "HeavyEquipmentMaintenance")" class="btn btn-danger btn-label-left">
                        <span><i class="fa fa-arrow-left"></i></span>Kembali</a> <a href="@Url.Action("ReportHeavyEquipmentMaintenance", "Report", New With {.id = Model.Id})" class="btn btn-danger btn-label-left">
                            <span><i class="fa fa-print"></i></span>Print</a>
            </div>
        </div>
    </div>
</div>
@Using Html.BeginJUIBox("Detail Kegiatan Alat Berat")
    @<div class="row">
        <div class="col-sm-12 col-lg-12">
            <div class="alert alert-info">
                <h3>
                    <u>Jadwal</u></h3>
                <br />
                <label>
                    Tanggal</label>
                : @Model.MaintenanceDateStart.ToShortDateString <strong>-</strong> @Model.MaintenanceDateEnd.ToShortDateString
                <br />
                <label>
                    Jam</label>
                &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;:
                @Model.MaintenanceDateStart.ToShortTimeString <strong>-</strong> @Model.MaintenanceDateEnd.ToShortTimeString
            </div>
            <table class="table table-bordered">
                <thead>
                    <tr>
                        <th>
                            Kode Alat Berat
                        </th>
                        <th>
                            Jenis
                        </th>
                       
                        <th>
                            MEREK / TIPE
                        </th>
                        <th>
                            NAMA OOPERATOR
                        </th>
                        <th>
                            STATUS
                        </th>
                    </tr>
                </thead>
                <tbody>
                    <tr class="text-center">
                        <td>@Model.HeavyEqp.Code
                        </td>
                        <td>@Model.HeavyEqp.Species
                        </td>
                       
                        <td>
                            @Model.HeavyEqp.Merk / @Model.HeavyEqp.Type
                        </td>
                        <td>@Model.HeavyEqp.OprName
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
                            @For Each item In Model.HeavyEqpMaintenanceRecordItems
                                @<tr>
                                    <td>@counter
                                    </td>
                                    <td>@item.Item
                                    </td>
                                </tr>
        counter += 1
    Next
                            @code 
                                If Model.HeavyEqpMaintenanceRecordItems.Count = 0 Then
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
                            @For Each item In Model.HeavyEqpMaintenanceRecordMaterialUseds
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
                            @If Model.HeavyEqpMaintenanceRecordMaterialUseds.Count = 0 Then
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
                            @For Each item In Model.HeavyEqpMaintenanceRecordOthers
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
                            @If Model.HeavyEqpMaintenanceRecordOthers.Count = 0 Then
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

