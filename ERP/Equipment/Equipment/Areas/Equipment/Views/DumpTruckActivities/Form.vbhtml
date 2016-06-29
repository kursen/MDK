@ModelType Equipment.DumpTruckActivity
@Code
    ViewData("Title") = "Kegiatan Dump Truk"
    
End Code
@helper WriteTimeInput(ByVal timeName As String, value As TimeSpan?)

    Dim spanname = "sp_" & timeName
    Dim timevalue As String = "--:--"
    If value.HasValue Then
        timevalue = value.Value.Hours.ToString("00") & ":" & value.Value.Minutes.ToString("00")
    End If
    @<span id="@spanname">@timevalue</span>
    @Html.Hidden(timeName, value)
    
    
End Helper
@helper WriteOperationTableColumnGroup()
    Dim colWidth = {"60px", "auto", "120px", "80px",  "80px", "120px", "80px", "200px", "120px"}
    
    For Each item In colWidth
    
    @Html.Raw("<col style='width :" & item & "'/>")
    
    Next
end helper
@helper WriteNonOperationTableColumnGroup()
    Dim colWidth = {"60px", "auto", "100px", "100px", "460px", "120px"}
    
    For Each item In colWidth
    
    @Html.Raw("<col style='width :" & item & "'/>")
    
    Next
end helper
@helper WriteFuelConsumedTableColumnGroup()
    Dim colWidth = {"60px", "auto", "120px"}
    
    For Each item In colWidth
    
    @Html.Raw("<col style='width :" & item & "'/>")
    
    Next
end helper
@helper WriteOilUsagesTableColumnGroup()
    Dim colWidth = {"60px", "auto", "100px", "120px"}
    
    For Each item In colWidth
    
    @Html.Raw("<col style='width :" & item & "'/>")
    
    Next
end helper
@Using Html.BeginForm("SaveItemActivities", "DumpTruckActivities", Nothing, FormMethod.Post, New With {.id = "frmactivity", .autocomplete = "off"})
    Using Html.BeginJUIBox("Kegiatan Dump Truck", False, False, False, False, False, "fa fa-truck")

    @<input type="hidden" name="ID" id="dumptruckActivityID" value="@Model.ID" />
    
    @<div class="row">
    </div>
    @<div class="row">
        <div class="form-horizontal">
            <div class="col-md-3 col-sm-12">
                @Html.WriteFormDateInputFor(Function(m) m.Date, "Tanggal",
                                    New With {.format = "dd-mm-yyyy", .autoclose = "true",
                                              .orientation = "top left", .todayBtn = "linked", .language = "id", .daysOfWeekDisabled = {0}},
                                    lgLabelWidth:=5, lgControlWidth:=7)
            </div>
            <div class="col-md-5 col-sm-12">
                <div class="form-group">
                    <label class="col-lg-4 col-sm-4 control-label">
                       No Polisi
                    </label>
                    <div class="col-lg-8 col-sm-12">
                       @Html.TextBoxFor(Function(m) m.PoliceNumber, New With {.class = "form-control"})
                        @Html.HiddenFor(Function(m) m.IDArea)
                    </div>
                </div>
                <div class="form-group">
                    <label class="col-lg-4 col-sm-4 control-label">
                        NO. DT
                    </label>
                    <div class="col-lg-8 col-sm-4">
                        @Html.HiddenFor(Function(m) m.Code)
                        <span class="form-control" id="spCode">@Model.Code</span>
                    </div>
                </div>
                <div class="form-group">
                    <label class="col-lg-4 col-sm-4 control-label">
                        Kategori
                    </label>
                    <div class="col-lg-8 col-sm-4">
                        @Html.HiddenFor(Function(m) m.Category)
                        <span class="form-control" id="spCategory">@Model.Category</span>
                    </div>
                </div>
                <div class="form-group">
                    <label class="col-lg-4 col-sm-4 control-label">
                        Merk/Type
                    </label>
                    <div class="col-lg-8 col-sm-4">
                        @Html.HiddenFor(Function(m) m.Merk)
                        @Html.HiddenFor(Function(m) m.Type)
                        <span class="form-control" id="spMerk">@Model.Merk /  @Model.Type</span>

                    </div>
                </div>
                
            </div>
            <div class="col-md-4 col-sm-12">
                <div class="form-group">
                    <label class="col-lg-4 col-sm-4 control-label">
                        Supir
                    </label>
                    <div class="col-lg-7 col-sm-4">
                        @Html.TextBoxFor(Function(m) m.Operator, New With {.class = "form-control"})
                        @Html.HiddenFor(Function(m) m.IDOp)
                    </div>
                </div>
            </div>
        </div>
        <input type="hidden" name="ItemOperation" id="OpItems" value="" />
        <input type="hidden" name="ItemNonOperation" id="NonOpItems" value="" />
        <input type="hidden" name="ItemOil" id="OilItems" value="" />
        <input type="hidden" name="ItemFuel" id="fuelitem" value="" />
    </div>
    End Using
    Using Html.BeginJUIBox("Jam Kerja", True, False, False, False, False, "")
  
    @<div class="row">
        <div class="col-lg-12 col-sm-12">
            <table class="table table-bordered table-striped table-hover  table-datatable dataTable responsive no-footer tblworkhour">
                <thead>
                    <tr>
                        <th colspan="2">
                            Lembur (2)<br />
                            00:00-05:00
                        </th>
                        <th colspan="2">
                            Lembur (1)
                            <br />
                            05:00-08:00
                        </th>
                        <th colspan="2">
                            Pagi s/d Siang
                        </th>
                        <th colspan="2">
                            Istirahat
                        </th>
                    </tr>
                </thead>
                <tbody>
                    <tr>
                        <td class="text-center">
                            Mulai
                        </td>
                        <td class="text-center">
                            Akhir
                        </td>
                        <td class="text-center">
                            Mulai
                        </td>
                        <td class="text-center">
                            Akhir
                        </td>
                        <td class="text-center">
                            Mulai
                        </td>
                        <td class="text-center">
                            Akhir
                        </td>
                        <td class="text-center">
                            Mulai
                        </td>
                        <td class="text-center">
                            Akhir
                        </td>
                    </tr>
                    <tr>
                        <td class="text-center">
                            @WriteTimeInput("BeginSecondOverTime", Model.BeginSecondOverTime)
                        </td>
                        <td class="text-center">
                            @WriteTimeInput("EndSecondOverTime", Model.EndSecondOverTime)
                        </td>
                        <td class="text-center">
                            @WriteTimeInput("BeginFirstOverTime1", Model.BeginFirstOverTime1)
                        </td>
                        <td class="text-center">
                            @WriteTimeInput("EndFirstOverTime1", Model.EndFirstOverTime1)
                        </td>
                        <td class="text-center">
                            @WriteTimeInput("BeginNormalI", Model.BeginNormalI)
                        </td>
                        <td class="text-center">
                            @WriteTimeInput("EndNormalI", Model.EndNormalI)
                        </td>
                        <td class="text-center">
                            @WriteTimeInput("BeginBreakI", Model.BeginBreakI)
                        </td>
                        <td class="text-center">
                            @WriteTimeInput("EndBreakI", Model.EndBreakI)
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2">
                            <div id="ts_BeginSecondOverTime">
                            </div>
                        </td>
                        <td colspan="2">
                            <div id="ts_BeginFirstOverTime1">
                            </div>
                        </td>
                        <td colspan="2">
                            <div id="ts_BeginNormalI">
                            </div>
                        </td>
                        <td colspan="2">
                            <div id="ts_BeginBreakI">
                            </div>
                        </td>
                    </tr>
                </tbody>
            </table>
            <table class="table table-bordered table-striped table-hover  table-datatable dataTable responsive no-footer tblworkhour">
                <thead>
                    <tr>
                        <th colspan="2">
                            Siang s/d Sore
                        </th>
                        <th colspan="2">
                            Istirahat
                        </th>
                        <th colspan="2">
                            Lembur (1)
                        </th>
                    </tr>
                </thead>
                <tbody>
                </tbody>
                <tr>
                    <td class="text-center">
                        Mulai
                    </td>
                    <td class="text-center">
                        Akhir
                    </td>
                    <td class="text-center">
                        Mulai
                    </td>
                    <td class="text-center">
                        Akhir
                    </td>
                    <td class="text-center">
                        Mulai
                    </td>
                    <td class="text-center">
                        Akhir
                    </td>
                </tr>
                <tr>
                    <td class="text-center">
                        @WriteTimeInput("BeginNormalII", Model.BeginNormalII)
                    </td>
                    <td class="text-center">
                        @WriteTimeInput("EndNormalII", Model.EndNormalII)
                    </td>
                    <td class="text-center">
                        @WriteTimeInput("BeginBreakII", Model.BeginBreakII)
                    </td>
                    <td class="text-center">
                        @WriteTimeInput("EndBreakII", Model.EndBreakII)
                    </td>
                    <td class="text-center">
                        @WriteTimeInput("BeginFirstOverTime2", Model.BeginFirstOverTime2)
                    </td>
                    <td class="text-center">
                        @WriteTimeInput("EndFirstOvertime2", Model.EndFirstOvertime2)
                    </td>
                </tr>
                <tr>
                    <td colspan="2">
                        <div id="ts_BeginNormalII">
                        </div>
                    </td>
                    <td colspan="2">
                    </td>
                    <td colspan="2">
                        <div id="ts_BeginFirstOverTime2">
                        </div>
                    </td>
                </tr>
            </table>
            <table class="table table-bordered">
                <thead>
                    <tr>
                        <th>
                        </th>
                        <th>
                            Jam Normal
                        </th>
                        <th>
                            Jam Lembur 1
                        </th>
                        <th>
                            Jam Lembur 2
                        </th>
                    </tr>
                </thead>
                <tbody>
                    <tr>
                        <td>
                            Total Jam
                        </td>
                        <td id='total_jam_normal' class="text-center text-bold">
                            0
                        </td>
                        <td id='total_jam_lembur_1' class="text-center text-bold">
                            0
                        </td>
                        <td id='total_jam_lembur_2' class="text-center text-bold">
                            0
                        </td>
                    </tr>
                </tbody>
            </table>
        </div>
    </div>
    
    End Using
End Using
@Using Html.BeginJUIBox("Operation", True, False, False, False, False, "")
  
    
    @<table class="table table-bordered" id="tblOperation">
        <colgroup>@WriteOperationTableColumnGroup()</colgroup>
        <thead>
            <tr>
                <th rowspan='2'>
                    No.
                </th>
                <th rowspan='2'>
                    Jenis Muatan
                </th>
                <th  rowspan='2'>
                    Lokasi Asal
                </th>
              
                <th  colspan='2'>
                    Waktu
                </th>
                <th rowspan='2'>
                    Tujuan Lokasi
                </th>
                <th rowspan='2'>
                    Jarak
                </th>
                <th rowspan='2'>Nama Penerima</th>
                <th  rowspan='2'>
                    <button type="button" class="btn btn-danger btn-label-left" id="btnAddOpr">
                        <span><i class="fa fa-plus"></i></span>Tambah</button>
                </th>
            </tr>
            <tr>
                <th>Berangkat</th>
                <th>Tiba</th>
            </tr>
        </thead>
    </table>
    @<div id="formOperasi" style="display: none;">
       
        @Using Html.BeginForm("ValidateOperation", "DumpTruckActivities", Nothing, FormMethod.Post, New With {.class = "form-horizontal", .autocomplete = "Off", .id = "frmOperation"})
               @Html.Hidden("ID", 0, New With {.id = "id_Operation"})
            @Html.Hidden("IDActivity", 0, New With {.id = "IDActivity_Operation"})
            @Html.Hidden("rowIdx", -1, New With {.id = "rowIdx_Operation"})
            @Html.Hidden("DepartureTime", "", New With {.id = "DepartureTime_Operation"})
            @Html.Hidden("ArrivalTime", "", New With {.id = "ArrivalTime_Operation"})
            @<div class="form-group">
                <label class="col-lg-3 col-sm-4 control-label">
                    Jenis Muatan
                </label>
                <div class="col-lg-3 col-sm-4">
                    @Html.TextBox("LoadType", Nothing, New With {.class = "form-control"})
                </div>
            </div>
            @<div class="form-group">
                <label class="col-lg-3 col-sm-4 control-label">
                    Asal
                </label>
                <div class="col-lg-3 col-sm-4">
                    @Html.TextBox("SourceLocation", Nothing, New With {.class = "form-control"})
                </div>
            </div>
          
            @<div class="form-group">
                            <label class="col-lg-3 col-sm-4 control-label">
                Jam Berangkat
                            </label>
                            <div class="col-lg-3 col-sm-4">
                                <div class="input-group">
                                    <span id="sp_DepartureTime_Operation" class="form-control text-center">--:--</span> <span
                                        class="input-group-addon">Tiba</span> <span id="sp_ArrivalTime_Operation" class="form-control text-center">
                                            --:--</span>
                                </div>
                            </div>
                        </div>
                        @<div class="form-group">
                            <label class="col-lg-3 col-sm-4 control-label">
                                &nbsp;
                            </label>
                            <div class="col-lg-3 col-sm-4" style="padding-left: 25px; padding-right: 25px">
                                <div id="ts_DepartureTime_Operation">
                                </div>
                            </div>
                        </div>
            
           
            @<div class="form-group">
                <label class="col-lg-3 col-sm-4 control-label">
                    Tujuan
                </label>
                <div class="col-lg-3 col-sm-4">
                    @Html.TextBox("Destination", Nothing, New With {.class = "form-control"})
                </div>
            </div>
            @<div class="form-group">
                <label class="col-lg-3 col-sm-4 control-label">
                    Jarak
                </label>
                <div class="col-lg-2 col-sm-4">
                 <div class="input-group">
                    @Html.DecimalInput("Distance", 0.0)
                     <div class="input-group-addon">
                           KM
                        </div>
                    </div>
                </div>
            </div>
            @<div class="form-group">
                <label class="col-lg-3 col-sm-4 control-label">
                    Penerima
                </label>
                <div class="col-lg-2 col-sm-4">
                 
                   @Html.TextBox("ReceiverName", "")
                    
                 
                </div>
            </div>
            @<div class="form-group">
                <div class="col-sm-offset-3 col-sm-10">
                    <button type="button" class="btn btn-primary" id="btnSaveOp">
                        Simpan</button>
                    <button type="button" class="btn btn-default" onclick="$('#btnformOp').click()">
                        batal</button>
                </div>
            </div>
End Using
    </div>
    
End Using
@Using Html.BeginJUIBox("Non Operasi", True, False, False, False, False, "")
    @<div class="row">
        <div class="col-lg-12 col-sm-12">
            <table class="table table-bordered " id="tblNonOperation">
                <colgroup>@WriteNonOperationTableColumnGroup()</colgroup>
                <thead>
                    <tr>
                        <th rowspan="2">
                            No
                        </th>
                        <th rowspan="2">
                            Jenis Pekerjaan
                        </th>
                        <th colspan="2">
                            Waktu
                        </th>
                        <th rowspan="2">
                            Alasan Non Operasional
                        </th>
                        <th class="text-center" rowspan="2">
                            <button type="button" class="btn btn-danger btn-label-left" id="btnAddNonOpr">
                                <span><i class="fa fa-plus"></i></span>Tambah</button>
                        </th>
                    </tr>
                    <tr>
                        <th>
                            Awal
                        </th>
                        <th>
                            Akhir
                        </th>
                    </tr>
                </thead>
            </table>
            <div id="formNonOpr" style="display: none;">
                @Using Html.BeginForm("ValidateNonOperation", "DumpTruckActivities", Nothing, FormMethod.Post, New With {.class = "form-horizontal", .autocomplete = "Off", .id = "frmNonOperation"})
                    @Html.Hidden("ID", 0, New With {.id = "id_NonOperation"})
                    @Html.Hidden("IDActivity", 0, New With {.id = "IDActivity_NonOperation"})
                    @Html.Hidden("rowIdx", -1, New With {.id = "rowIdx_NonOperation"})
                    @Html.Hidden("Begin", "", New With {.id = "Begin_NonOperation"})
                    @Html.Hidden("End", "", New With {.id = "End_NonOperation"})
                    @<div>
                        <div class="form-group">
                            <label class="col-lg-3 col-sm-4 control-label ">
                                Jenis Pekerjaan
                            </label>
                            <div class="col-lg-3 col-sm-4">
                                @Html.TextBox("NonOperationType", Nothing, New With {.class = "form-control"})
                            </div>
                        </div>
                        <div class="form-group">
                            <label class="col-lg-3 col-sm-4 control-label">
                                Jam
                            </label>
                            <div class="col-lg-3 col-sm-4">
                                <div class="input-group">
                                    <span id="sp_Begin_NonOperation" class="form-control text-center">--:--</span> <span
                                        class="input-group-addon">s/d</span> <span id="sp_End_NonOperation" class="form-control text-center">
                                            --:--</span>
                                </div>
                            </div>
                        </div>
                        <div class="form-group">
                            <label class="col-lg-3 col-sm-4 control-label">
                                &nbsp;
                            </label>
                            <div class="col-lg-3 col-sm-4" style="padding-left: 25px; padding-right: 25px">
                                <div id="ts_Begin_NonOperation">
                                </div>
                            </div>
                        </div>
                        <div class="form-group">
                            <label class="col-lg-3 col-sm-4 control-label">
                                Alasan
                            </label>
                            <div class="col-lg-3 col-sm-4">
                                @Html.TextBox("Reason", Nothing, New With {.class = "form-control"})
                            </div>
                        </div>
                        <div class="form-group">
                            <div class="col-sm-offset-3 col-sm-10">
                                <button type="button" class="btn btn-primary" id="btnSaveNon">
                                    Tambah</button>
                                <button type="button" class="btn btn-default" onclick="$('#btnAddNonOpr').click()">
                                    Tutup</button>
                            </div>
                        </div>
                    </div>
    End Using
            </div>
        </div>
    </div>

End Using
<div class="row">
    <div class="col-lg-6 col-sm-12">
        @Using Html.BeginJUIBox("Pemakaian Pelumas", True, False, False, False, False, "")
            @<div class="row">
                <div class="col-lg-12 col-sm-12">
                    <table id="tblpelumas" class="table table-bordered table-striped table-hover  table-datatable dataTable">
                        <colgroup>@WriteOilUsagesTableColumnGroup()</colgroup>
                        <thead>
                            <tr>
                                <th>
                                    No
                                </th>
                                <th>
                                    Uraian
                                </th>
                                <th>
                                    Liter
                                </th>
                                <th>
                                    <button type="button" class="btn btn-danger btn-label-left" id="btnAddOil">
                                        <span><i class="fa fa-plus"></i></span>Tambah</button>
                                </th>
                            </tr>
                        </thead>
                    </table>
                    <div id="formOil" style="display: none;">
                        @Using Html.BeginForm("ValidateOil", "DumpTruckActivities", Nothing, FormMethod.Post, New With {.class = "form-horizontal", .autocomplete = "Off", .id = "frmoil"})
                            @Html.Hidden("ID", 0, New With {.id = "id_OilUsedHeavyEqp"})
                            @Html.Hidden("IDActivity", 0, New With {.id = "IDActivity_OilUsedHeavyEqp"})
                            @Html.Hidden("rowIdx", 0, New With {.id = "rowIdx_OilUsedHeavyEqp"})

                            @<div>
                                <div class="form-group">
                                    <label class="col-lg-3 col-sm-4 control-label ">
                                        Jenis Pelumas
                                    </label>
                                    <div class="col-lg-6 col-sm-8">
                                        @Html.TextBox("OilType", Nothing, New With {.class = "form-control"})
                                    </div>
                                </div>
                                <div class="form-group">
                                    <label class="col-lg-3 col-sm-4 control-label">
                                        Jumlah
                                    </label>
                                    <div class="col-lg-4 col-sm-6">
                                        <div class="input-group">
                                            @Html.DecimalInput("Amount", 0)
                                            <div class="input-group-addon">
                                                Liter</div>
                                        </div>
                                    </div>
                                </div>
                                <div class="form-group">
                                    <div class="col-sm-offset-3 col-sm-10">
                                        <button type="button" class="btn btn-primary" id="btnSaveOil">
                                            Tambah</button>
                                        <button type="button" class="btn btn-default" onclick="$('#btnAddOil').click()">
                                            Tutup</button>
                                    </div>
                                </div>
                            </div>
            End Using
                    </div>
                </div>
            </div>


        End Using
    </div>
    <div class="col-lg-6 col-sm-12">
        @Using Html.BeginJUIBox("Pemakaian BBM", True, False, False, False, False, "")
            @<div class="row">
                <div class="col-lg-12 col-sm-12">
                    <table id="tblbbm" class="table table-bordered table-striped table-hover  table-datatable dataTable">
                        <colgroup>@WriteFuelConsumedTableColumnGroup()</colgroup>
                        <thead>
                            <tr>
                                <th>
                                    No
                                </th>
                                <th>
                                    Liter
                                </th>
                                <th>
                                    <button type="button" class="btn btn-danger btn-label-left" id="btnAddbbm">
                                        <span><i class="fa fa-plus"></i></span>Tambah</button>
                                </th>
                            </tr>
                        </thead>
                    </table>
                    <div id="formBBm" style="display: none;">
                        @Using Html.BeginForm("ValidateFuel", "DumpTruckActivities", Nothing, FormMethod.Post,
                             New With {.class = "form-horizontal", .autocomplete = "Off", .id = "frmfuel"})
                        @Html.Hidden("ID", 0, New With {.id = "id_FuelUsedHeavyEqp"})
                        @Html.Hidden("IDActivity", 0, New With {.id = "IDActivity_FuelUsedHeavyEqp"})
                        @Html.Hidden("rowIdx", 0, New With {.id = "rowIdx_FuelUsedHeavyEqp"})
                            @<div>
                                <div class="form-group">
                                    <label class="col-lg-3 col-sm-4 control-label">
                                        Jumlah
                                    </label>
                                    <div class="col-lg-4 col-sm-6">
                                        <div class="input-group">
                                            
                                            @Html.DecimalInput("AmountFuel", 0)
                                            <div class="input-group-addon">
                                                Liter</div>
                                        </div>
                                    </div>
                                </div>
                                <div class="form-group">
                                    <div class="col-sm-offset-3 col-sm-10">
                                        <button type="submit" class="btn btn-primary" id="btnSaveBbm">
                                            Tambah</button>
                                        <button type="button" class="btn btn-default" onclick="$('#btnAddbbm').click()">
                                            Tutup</button>
                                    </div>
                                </div>
                            </div>
            End Using
                    </div>
                </div>
            </div>
        End Using
    </div>
</div>
@Using Html.BeginJUIBox("Pemeriksaan")
    @<div class="row">
        <div class="col-lg-4 col-md-4">
            <div class="panel panel-primary">
                <div class="panel-heading">
                    Diperiksa Oleh
                </div>
                <div class="panel-body">
                    <div class="form-horizontal">
                        <div class="form-group">
                            <label class="col-lg-4 col-sm-4 control-label">
                                Nama
                            </label>
                            <div class="col-lg-8 col-sm-4">
                                @Html.TextBoxFor(Function(m) m.CorrectBy, New With {.class = "form-control"})
                            </div>
                        </div>
                        <div class="form-group">
                            <label class="col-lg-4 col-sm-4 control-label">
                                Jabatan
                            </label>
                            <div class="col-lg-8 col-sm-4">
                                @Html.TextBoxFor(Function(m) m.JabatanCorrectBy, New With {.class = "form-control"})
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
        <div class="col-lg-4 col-md-4">
            <div class="panel panel-primary">
                <div class="panel-heading">
                    DiKetahui Oleh
                </div>
                <div class="panel-body">
                    <div class="form-horizontal">
                        <div class="form-group">
                            <label class="col-lg-4 col-sm-4 control-label">
                                Nama
                            </label>
                            <div class="col-lg-8 col-sm-4">
                                @Html.TextBoxFor(Function(m) m.KnownBy, New With {.class = "form-control"})
                            </div>
                        </div>
                        <div class="form-group">
                            <label class="col-lg-4 col-sm-4 control-label">
                                Jabatan
                            </label>
                            <div class="col-lg-8 col-sm-4">
                                @Html.TextBoxFor(Function(m) m.JabatanKnownBy, New With {.class = "form-control"})
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
        <div class="col-lg-4 col-md-4">
         <div class="panel panel-primary">
                <div class="panel-heading">
                    Dibuat Oleh
                </div>
                <div class="panel-body">
                    <div class="form-horizontal">
                        <div class="form-group">
                            <label class="col-lg-4 col-sm-4 control-label">
                                Nama
                            </label>
                            <div class="col-lg-8 col-sm-4">
                            @code
                                If Not IsNothing(Model.Operator) Then
                                    @<span class="form-control" id="sp_operatorName">@Model.Operator</span>
                                  
                            Else
                                  @<span class="form-control" id="sp_operatorName"></span>
                            End If
                            End Code
                             
                            </div>
                        </div>
                        <div class="form-group">
                            <label class="col-lg-4 col-sm-4 control-label">
                                Jabatan
                            </label>
                            <div class="col-lg-8 col-sm-4">
                               <span class="form-control">SUPIR</span>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
    
End Using
<div class="well">
    <div class="col-lg-offset-5 col-sm-offset-3">
        <button type="button" class="btn btn-primary" id="btnSaveActivity">
            Simpan</button>
        <a href="@Url.Action("Index", "DumpTruckActivities")" class="btn btn-default" >Batal</a>
    </div>
</div>
<!--end form-->
<!-- end form-->
<script src="@Url.Content("~/plugins/datatables/jquery.dataTables.js")" type="text/javascript"></script>
<script src="@Url.Content("~/plugins/datatables/dataTables.bootstrap.js")" type="text/javascript"></script>
<script src="@Url.Content("~/plugins/datatables/sum.js")" type="text/javascript"></script>
<script src="@Url.Content("~/plugins/datatables/datatablehelper.js")" type="text/javascript"></script>
<style>
    .tblworkhour .ui-state-focus, #formNonOpr .ui-state-focus, #formOperasi .ui-state-focus
    {
        background-color: Blue;
        background-image: none;
    }
    .tblworkhour .ui-widget-header, #formNonOpr .ui-widget-header, #formOperasi .ui-widget-header
    {
        background-color: #00d11c;
        background-image: none;
    }
</style>
<script type="text/javascript">
    var tblOperation = null;
    var tblNonOperation = null;
    var tblOil = null;
    var tblbbm = null;

    var setTimePicker = function (beginTimeName, endTimeName, MinTime, MaxTime) {

        var tMin = moment('1970-12-31 ' + MinTime, 'YYYY-MM-DD HH:mm').valueOf();
        var tMax = moment('1970-12-31 ' + MaxTime, 'YYYY-MM-DD HH:mm').valueOf();

        var e = $('#sp_' + endTimeName).text();
        console.log(e);
        var bN2value = moment('1970-12-31 ' + $('#sp_' + beginTimeName).text(), 'YYYY-MM-DD HH:mm').valueOf();
        var eN2valuet = moment('1970-12-31 ' + $('#sp_' + endTimeName).text(), 'YYYY-MM-DD HH:mm')
        if (e == "00:00") { //only EndFirstOvertime2 could have this value
            eN2valuet.add(1, 'days');
            $('#sp_' + endTimeName).text('24:00');
        }
        var eN2value = eN2valuet.valueOf();
        $('#ts_' + beginTimeName).slider({
            max: tMax,
            min: tMin,
            values: [bN2value, eN2value],
            range: true,
            step: 600000, //step 10 minutes
            slide: function (e, ui) {
                var t1 = moment(ui.values[0] * 1);
                $('#sp_' + beginTimeName).text(t1.format('HH:mm'));
                $('#' + beginTimeName).val(t1.format('HH:mm'));
                var t2 = moment(ui.values[1] * 1);
                $('#sp_' + endTimeName).text(t2.format('HH:mm'));
                $('#' + endTimeName).val(t2.format('HH:mm'));
                if (t2.valueOf() >= 31510740000) {
                    $('#sp_' + endTimeName).text('24:00');
                    $('#' + endTimeName).val('00:00');
                };
                if (t2.format('HH:mm') == t1.format('HH:mm')) {
                    $('#sp_' + beginTimeName).text('--:--');
                    $('#sp_' + endTimeName).text('--:--');
                }
                _calculateHour();
            }
        });
    }
    //end setTimePicker

    var _initTimePicker = function () {
        setTimePicker('BeginSecondOverTime', 'EndSecondOverTime', '00:00', '05:00');
        setTimePicker('BeginFirstOverTime1', 'EndFirstOverTime1', '05:00', '08:00');
        setTimePicker('BeginNormalI', 'EndNormalI', '08:00', '12:00');
        setTimePicker('BeginNormalII', 'EndNormalII', '12:00', '17:00');
        setTimePicker('BeginFirstOverTime2', 'EndFirstOvertime2', '17:00', '23:59:59');
        setTimePicker('Begin_NonOperation', 'End_NonOperation', '08:00', '17:00');
        setTimePicker('DepartureTime_Operation', 'ArrivalTime_Operation', '00:00', '23:59:59');
    }; //_initTimePicker

    var _getDiff = function (time1, time2) {
        var start = moment('01/01/1970 ' + time2 + ':00', 'DD/MM/YYYY HH:mm:ss');
        var end = moment('01/01/1970 ' + time1 + ':00', 'DD/MM/YYYY HH:mm:ss');

        if (start.valueOf() < end.valueOf()) {
            start.add(1, 'days');
        }
        var ms = start.diff(end);
        var d = moment.duration(ms);
        return d;
    };
    var _calculateHour = function () {
        //total normal hour
        var d1 = _getDiff($('#BeginNormalI').val(), $('#EndNormalI').val());
        var d2 = _getDiff($('#BeginNormalII').val(), $('#EndNormalII').val());
        if ((d1 < 0) || (d2 < 0)) {
            $('#total_jam_normal').css('color', '#ff0000');
        } else {
            $('#total_jam_normal').removeAttr('style');
        }
        d = moment.duration(d1 + d2);
        $('#total_jam_normal').text($.number(d.asHours(), 1, ",", "."));
        //total lembur 1
       
        var d3 = _getDiff($('#BeginFirstOverTime1').val(), $('#EndFirstOverTime1').val());
        var d4 = _getDiff($('#BeginFirstOverTime2').val(), $('#EndFirstOvertime2').val());
       
        if ((d3 < 0) || (d4 < 0)) {
            $('#total_jam_lembur_1').css('color', '#ff0000');
        } else {
            $('#total_jam_lembur_1').removeAttr('style');
        }
        d = moment.duration(d3 + d4);
        $('#total_jam_lembur_1').text($.number(d.asHours(), 1, ",", "."));
        //total lembur 2
        var d5 = _getDiff($('#BeginSecondOverTime').val(), $('#EndSecondOverTime').val());
        if (d5 < 0) {
            $('#total_jam_lembur_2').css('color', '#ff0000');
        } else {
            $('#total_jam_lembur_2').removeAttr('style');
        }
        d = moment.duration(d5);
        $('#total_jam_lembur_2').text($.number(d.asHours(), 1, ",", "."));
    }
    //function submit
    var submitFormActivityCallback = function (data) {
        if (data.stat == 0) {
            showNotificationSaveError(data);
            return
        } else if (data.stat == 2) {
            //data already there
            $.notifyClose();
            $.notify({
                icon: 'fa  fa-warning',
                title: 'Data Tidak Tersimpan!.',
                message: 'Data kegiatan alat berat ini sudah ada di database.',
                url: '/Equipment/DumpTruckActivities/Detail/' + data.id
            }, {
                type: 'warning',
                newest_on_top: true,
                delay: 0,
                template: '<div data-notify="container" class="col-xs-11 col-sm-3 alert alert-{0}" role="alert">' +
                      '<button type="button" aria-hidden="true" class="close" data-notify="dismiss">×</button>' +
                      '<span data-notify="icon"></span> ' + '<span data-notify="title">{1}</span> <br/>' +
                      '<span data-notify="message">{2}</span>' + '<div class="row text-center" style="margin-top:20px"> ' +
                      '<a class="btn btn-primary" href="{3}">Lihat data</a> &nbsp;' +
                      '<button class="btn btn-primary" onclick=" $.notifyClose();">Tutup</button>' + '</div>' + '</div>'
            });
        } else {
            window.location = '/Equipment/DumpTruckActivities/';
        }
    }; //submitFormActivityCallback







    var _initTblOperation = function () {
        var _renderEditColumnOp = function (data, type, row) {
            if (type == 'display') {
                return ('<div class=\'btn-group\' role=\'group\'>' +
                      '<button type=\'button\' class=\'btn btn-default btn-xs btnEditItemOperation\' ><i class=\'fa fa-edit\'></i></button>' +
                      '<button type=\'button\' class=\'btn btn-default btn-xs btRemoveItemOperation\'><i class=\'fa fa-remove\'></i></button>' +
                      '</div>');
            }
            return data;
        };
       
        var arrColumnsOp = [
            { 'data': 'ID', 'sClass': 'text-right' }, //
            {'data': 'LoadType' }, //
            {'data': 'SourceLocation', 'sClass': 'text-right' }, //
            {'data': 'DepartureTime', 'sClass': 'text-right' }, //
            {'data': 'ArrivalTime', 'sClass': 'text-right' }, //
            {'data': 'Destination' },
            {'data': 'Distance' },
            { 'data': 'ReceiverName' },
            { 'data': 'ID', 'mRender': _renderEditColumnOp, 'sClass': 'text-center' }
        ];
        _GeneralTable(arrColumnsOp);
        tblOperation = $('#tblOperation').DataTable(datatableDefaultOptions)
        .on('click', '.btRemoveItemOperation', function (d) {
            if (confirm('Hapus item ini ?') == false) {
                return;
            }
            var tr = $(this).closest('tr');
            var row = tblOperation.row(tr);
            if (row.data().ID == 0) {
                row.remove().draw()
                return;
            }
            $.ajax({
                type: 'POST',
                data: {
                    id: row.data().ID
                },
                url: '/DumpTruckActivities/DeleteOperation',
                success: function (data) {
                    if (data.stat == 1) {
                        row.remove().draw();
                    }
                }
            });
        }).on('click', '.btnEditItemOperation', function (d) {
            var tr = $(this).closest('tr');
            var row = tblOperation.row(tr);
            $('#OperationType').focus();
            var dataItem = row.data();
            $("#id_Operation").val(dataItem.ID);
            $("#rowIdx_Operation").val(row.index());

            $("#IDActivity_Operation").val(dataItem.IDActivity);
            $('#LoadType').val(dataItem.LoadType);
            $('#SourceLocation').val(dataItem.SourceLocation);
            $('#DepartureTime_Operation').val(dataItem.DepartureTime);
            $('#ArrivalTime_Operation').val(dataItem.ArrivalTime);
            $('#sp_DepartureTime_Operation').text(dataItem.DepartureTime);
            $('#sp_ArrivalTime_Operation').text(dataItem.ArrivalTime);
            $('#Destination').val(dataItem.Destination);
            $('#Distance').val(dataItem.Distance);
            $('#ReceiverName').val(dataItem.ReceiverName);
            setTimePicker('DepartureTime_Operation', 'ArrivalTime_Operation', '00:00', '23:59:59');
            $('#formOperasi').toggle('blind', null, null, function (e) {
                $('#LoadType').focus();
            });
        });
    }
    //_initTblOperation
    //table Non Operation

    var _initTblNonOperation = function () {
        var _renderEditColumnNon = function (data, type, row) {
            if (type == 'display') {
                return ('<div class=\'btn-group\' role=\'group\'>' +
                        '<button type=\'button\' class=\'btn btn-default btn-xs btnEditItemNonOperaton\' ><i class=\'fa fa-edit\'></i></button>' +
                        '<button type=\'button\' class=\'btn btn-default btn-xs btnRemoveItemNonOperation\'><i class=\'fa fa-remove\'></i></button>' +
                        '</div>');
            }
            return data;
        }
        var arrColumnsNon = [
            { data: "ID", sClass: "text-right" },
            { data: "NonOperationType" },
            { data: "Begin", sClass: "text-center" },
            { data: "End", sClass: "text-center" },
            { data: "Reason" },
            { data: "ID", sClass: "text-center", mRender: _renderEditColumnNon }
        ];

        //data table form
        _GeneralTable(arrColumnsNon);
        tblNonOperation = $('#tblNonOperation').DataTable(datatableDefaultOptions)
        .on('click', '.btnRemoveItemNonOperation', function (d) {
            if (confirm('Hapus item ini ?') == false) {
                return;
            }
            var tr = $(this).closest('tr');
            var row = tblNonOperation.row(tr);
            if (row.data().Id == 0) {
                row.remove().draw()
                return;
            }
            $.ajax({
                type: 'POST',
                data: {
                    id: row.data().ID
                },
                url: '/DumpTruckActivities/DeleteNonOperation',
                success: function (data) {
                    if (data.stat == 1) {
                        row.remove().draw();
                    }
                }
            });
        }).on('click', '.btnEditItemNonOperaton', function (d) {
            var tr = $(this).closest('tr');
            var row = tblNonOperation.row(tr);

            var dataItem = row.data();
            $('#id_NonOperation').val(dataItem.ID);
            $('#rowIdx_NonOperation').val(row.index());
            $('#NonOperationType').val(dataItem.NonOperationType);
            $('#IDActivity_NonOperation').val(dataItem.IDActivity);
            $('#Begin_NonOperation').val(dataItem.Begin);
            $('#End_NonOperation').val(dataItem.End);
            $('#sp_Begin_NonOperation').text(dataItem.Begin)
            $('#sp_End_NonOperation').text(dataItem.End)
            setTimePicker('Begin_NonOperation', 'End_NonOperation', '08:00', '17:00');
            $('#Reason').val(dataItem.Reason);
            $('#btnSaveNon').text('Simpan Perubahan')

            $('#formNonOpr').toggle('blind', null, null, function (e) {
                $('#NonOperationType').focus();
            });
        });
    }
    //_initTblNonOperation
    //table pelumas

    var _initTblOilUsages = function () {
        //data table form
        var _renderEditColumnOil = function (data, type, row) {
            if (type == 'display') {
                return ('<div class=\'btn-group\' role=\'group\'>' +
                '<button type=\'button\' class=\'btn btn-default btn-xs btnEditItemOil\' ><i class=\'fa fa-edit\'></i></button>' +
                '<button type=\'button\' class=\'btn btn-default btn-xs btnRemoveItemOil\'><i class=\'fa fa-remove\'></i></button>' +
                '</div>');
            }
            return data;
        }
        var arrColumnsOil = [
            { 'data': 'ID', 'sClass': 'text-center' },
            { 'data': 'OilType', 'sClass': 'text-center' },
            { 'data': 'Amount', 'sClass': 'text-right' },
            { 'data': 'ID', 'mRender': _renderEditColumnOil, 'sClass': 'text-center' }
        ];
        _GeneralTable(arrColumnsOil);
        tblOil = $('#tblpelumas').DataTable(datatableDefaultOptions).on('click', '.btnRemoveItemOil', function (d) {

            if (confirm('Hapus item ini ?') == false) {
                return;
            }
            var tr = $(this).closest('tr');
            var row = tblOil.row(tr);
            if (row.data().ID == 0) {
                row.remove().draw()
                return;
            }
            $.ajax({
                type: 'POST',
                data: {
                    id: row.data().ID
                },
                url: '/DumpTruckActivities/DeleteOil',
                success: function (data) {
                    if (data.stat == 1) {
                        row.remove().draw();
                    }
                }
            });
        }).on('click', '.btnEditItemOil', function (d) {
            var tr = $(this).closest('tr');
            var row = tblOil.row(tr);
            $('#OilType').focus();
            var dataItem = row.data();
            $('#id_OilUsedHeavyEqp').val(dataItem.ID);
            $('#IDActivity_OilUsedHeavyEqp').val(dataItem.IDActivity);
            $("#rowIdx_OilUsedHeavyEqp").val(row.index());
            $('#OilType').val(dataItem.OilType);
            $('#Amount').val(dataItem.Amount);
            $('#formOil').toggle('blind', null, null, function (e) {
                $('#OilType').focus();
            });
        });
    }
    //_initTblOilUsages

    var _initTblBBM = function () {
        var _renderEditColumnbbm = function (data, type, row) {
            if (type == 'display') {
                return (
          '<div class=\'btn-group\' role=\'group\'>' +
          '<button type=\'button\' class=\'btn btn-default btn-xs btnEditItemFuel\' ><i class=\'fa fa-edit\'></i></button>' +
          '<button type=\'button\' class=\'btn btn-default btn-xs btnRemoveItemFuel\'><i class=\'fa fa-remove\'></i></button>' +
          '</div>');
            }
            return data;
        }
        var arrColumnsbbm = [
            { data: "ID", sClass: "text-center" },
            { data: "AmountFuel", sClass: "text-right" }, 
            { data: "IDActivity", mRender: _renderEditColumnbbm, "sClass":"text-center"}];
        _GeneralTable(arrColumnsbbm);
        tblbbm = $('#tblbbm').DataTable(datatableDefaultOptions)
        .on('click', '.btnRemoveItemFuel', function (d) {
            if ($('#formBBm').is(':visible')) {
                $('#formBBm').hide('fast');
                $('#btnAddbbm').show('fast');
            }
            if (confirm('Hapus item ini ?') == false) {
                return;
            }
            var tr = $(this).closest('tr');
            var row = tblbbm.row(tr);
            if (row.data().ID == 0) {
                row.remove().draw()
                return;
            }
            $.ajax({
                type: 'POST',
                data: {
                    id: row.data().ID
                },
                url: '/DumpTruckActivities/DeleteFuel',
                success: function (data) {
                    if (data.stat == 1) {
                        row.remove().draw();
                    }
                }
            });
        }).on('click', '.btnEditItemFuel', function (d) {
            var tr = $(this).closest('tr');
            var row = tblbbm.row(tr);

            $('#AmountFuel').focus();
            var dataItem = row.data();

            $("#id_FuelUsedHeavyEqp").val(dataItem.ID);
            $("#IDActivity_FuelUsedHeavyEqp").val(dataItem.IDActivity);
            $("#rowIdx_FuelUsedHeavyEqp").val(row.index());
            $('#AmountFuel').val(dataItem.AmountFuel);
            $('#formBBm').toggle('blind', null, null, function (e) {
                $('#AmountFuel').focus();
            });
        });
    };


    //_initTblBBM


   
   
    //gentable

    var _GeneralTable = function (arrColumns) {
        var _coldefs = [
  ];
        datatableDefaultOptions.searching = false;
        datatableDefaultOptions.aoColumns = arrColumns;
        datatableDefaultOptions.columnDefs = _coldefs;
        datatableDefaultOptions.autoWidth = false;
        datatableDefaultOptions.ordering = false;
        datatableDefaultOptions.fnDrawCallback = function (oSettings) {
            //show row number
            for (var i = 0, iLen = oSettings.aiDisplay.length; i < iLen; i++) {
                $('td:eq(0)', oSettings.aoData[oSettings.aiDisplay[i]].nTr).html((i + 1) + '.');
            }
        };
    }

    //load document
    var _loadDocument = function () {
        if (parseInt($('#dumptruckActivityID').val()) != 0) {
            $.ajax({
                method: 'post',
                url: '/DumpTruckActivities/loadActivity',
                data: {
                    id: $('#dumptruckActivityID').val()
                },
                dataType: 'json',
                success: function (data) {
                    if (data.Operations.length > 0) {
                        tblOperation.rows.add(data.Operations).draw();
                    }
                    if (data.NonOperations.length > 0) {
                        tblNonOperation.rows.add(data.NonOperations).draw();
                    }
                    if (data.OilUsages.length > 0) {
                        tblOil.rows.add(data.OilUsages).draw();
                    }
                    if (data.FuelConsumed.length > 0) {
                        tblbbm.rows.add(data.FuelConsumed).draw();
                    }
                }
            });
        }
    }
    //autocomplete
    var _initAutoComplete = function () {
        $('#OilType').autocomplete({
            source: function (request, response) {
                $.ajax({
                    url: '/DumpTruckActivities/AutocompleteOilType',
                    data: {
                        term: $('#OilType').val()
                    },
                    dataType: 'json',
                    success: function (data) {
                        response($.map(data, function (obj) {
                            return {
                                label: obj.Value,
                                value: obj.OilName
                            }
                        }));
                    }
                });
            }
        });
        $('#PoliceNumber').autocomplete({
            source: function (request, response) {
                $.ajax({
                    url: '/Equipment/DumpTruckActivities/AutocompleteVehicle',
                    data: {
                        term: $('#PoliceNumber').val()
                    },
                    dataType: 'json',
                    success: function (data) {
                        response($.map(data, function (obj) {
                            return {
                                label: obj.PoliceNumber,
                                value: obj.PoliceNumber,
                                merk: obj.Merk + '/' + obj.Type,
                                kind: obj.Species,
                                areaId: obj.IDArea,
                                idOpr: obj.IDDriver,
                                OprName: obj.DriverName,
                                code: obj.Code,
                                type: obj.Type
                            }
                        }));
                    }
                });
            },
            change: function (event, ui) {
                if (ui.item != null) {
                    $('#Merk').val(ui.item.merk);
                    $('#Type').val(ui.item.type);
                    $('#spMerk').text(ui.item.merk);
                    $('#Category').val(ui.item.kind);
                    $('#spCategory').text(ui.item.kind);
                    $('#spCode').text(ui.item.code);
                    $("#Code").val(ui.item.code);
                    $("#IDArea").val(ui.item.areaId);
                    if (ui.item.idOpr != null) {
                        $('#IDOp').val(ui.item.idOpr)
                        $("#Operator").val(ui.item.OprName);
                    }


                } else {
                    $('#Merk').val('');
                    $('#Type').val('');
                    $('#Category').val('');
                    $('#spMerk').text('');
                    $('#spType').text('');
                    $('#spCategory').text('');
                    $("#IDArea").val(0);
                    $('#IDOp').val(0)
                    $("#Code").val("");
                    $("#Operator").val("");

                }
            }
        }).data('ui-autocomplete')._renderItem = function (ul, item) {
            //location
            return ($('<li>').append('<a><strong>' + item.value + '</strong>, <i><strong>' +
            item.label + ',' + item.merk + '</strong> (' + item.kind + '</i>)</a>').appendTo(ul));
        };
        $('#SourceLocation, #Destination').autocomplete({
            source: function (request, response) {
                console.log($(this)[0].term);

              
                $.ajax({
                    url: '/DumpTruckActivities/AutocompleteLocation',
                    data: {
                        term: $(this)[0].term
                    },
                    dataType: 'json',
                    success: function (data) {
                        response($.map(data, function (obj) {
                            return {
                                label: obj.Value,
                                value: obj.Location
                            }
                        }));
                    }
                });
            }
        });
        $('#Operator').autocomplete({
            source: function (request, response) {
                $.ajax({
                    url: '/DumpTruckActivities/AutocompleteDrivers',
                    data: {
                        term: $('#Operator').val()
                    },
                    dataType: 'json',
                    success: function (data) {
                        response($.map(data, function (obj) {
                            return {
                                label: obj.Text,
                                value: obj.Text,
                                id: obj.Value
                            }
                        }));
                    }
                });
            },
            change: function (event, ui) {
                if (ui.item == null) {
                    $('#IDOp').val(0)
                    $('#IDOp').parent().parent().addClass('has-error');
                } else {
                    $('#IDOp').val(ui.item.id)
                    $('#sp_operatorName').text(ui.item.value);
                    $('#IDOp').parent().parent().removeClass('has-error');
                }
            }
        });
        $('#NonOperationType').autocomplete({
            source: function (request, response) {
                $.ajax({
                    url: '/DumpTruckActivities/AutocompleteNonOperationType',
                    data: {
                        term: $('#NonOperationType').val()
                    },
                    dataType: 'json',
                    success: function (data) {
                        response($.map(data, function (obj) {
                            return {
                                label: obj.Value,
                                value: obj.NonOpType
                            }
                        }));
                    }
                });
            }
        });
    };
    $(document).ready(function () {
        $('#btnAddOpr').click(function () {
            $('#frmOperation').trigger('reset');
            $('#frmOperation,.form-group').removeClass('has-error');
            $("#id_Operation").val(0);
            $("#IDActivity_Operation").val(0);
            $("#rowIdx_Operation").val(-1);


            $('#formOperasi').toggle('blind', null, null, function (e) {
                $('#LoadType').focus();
            });

        });
        $('#btnAddNonOpr').click(function () {
            $('#frmNonOperation').trigger('reset');
            $('#frmNonOperation,.form-group').removeClass('has-error');
            $('#id_NonOperation').val(0)
            $('#rowIdx_NonOperation').val(-1);
            $('#Begin_NonOperation').val('--:--');
            $('#End_NonOperation').val('--:--');
            $('#sp_Begin_NonOperation').text('--:--')
            $('#sp_End_NonOperation').text('--:--')
            setTimePicker('Begin_NonOperation', 'End_NonOperation', '08:00', '17:00');
            $('#btnSaveNon').text('Tambah')
            $('#formNonOpr').toggle('blind', null, null, function (e) {
                $('#NonOperationType').focus();
            });
        });
        $('#btnAddOil').click(function () {
            $('#frmoil').trigger('reset');
            $('#frmoil,.form-group').removeClass('has-error');
            $("#id_OilUsedHeavyEqp").val(0);
            $("#IDActivity_OilUsedHeavyEqp").val(0);
            $("#rowIdx_OilUsedHeavyEqp").val(-1);
            $('#formOil').toggle('blind', null, null, function (e) {
                $('#OilType').focus();
            });
        });
        $('#btnAddbbm').click(function () {
            $('#frmfuel').trigger('reset');
            $('#frmfuel,.form-group').removeClass('has-error');
            $("#id_FuelUsedHeavyEqp").val(0);
            $("#IDActivity_FuelUsedHeavyEqp").val(0);
            $("#rowIdx_FuelUsedHeavyEqp").val(-1);
            $('#formBBm').toggle('blind', null, null, function (e) {
                $('#AmountFuel').focus();
            });
        });
        //Operation
        $('#btnSaveOp').click(function () {
            var _data = $('#frmOperation').serialize();
            var _url = $('#frmOperation').attr('action');
            $.ajax({
                type: 'POST',
                data: _data,
                url: _url,
                success: function (data) {
                    if (data.stat == 1) {
                        var NumberId = $('#id_Operation').val();
                        var OperationItem = {
                            ID: NumberId,
                            IDActivity: $("#IDActivity_Operation").val(),
                            LoadType: $('#LoadType').val(),
                            SourceLocation: $('#SourceLocation').val(),
                            LocationOperation: $('#LocationOperation').val(),
                            DepartureTime: $('#DepartureTime_Operation').val(),
                            ArrivalTime: $('#ArrivalTime_Operation').val(),
                            Destination: $('#Destination').val(),
                            Distance: $('#Distance').val(),
                            ReceiverName: $('#ReceiverName').val()
                        };
                        if (data.idx == -1) {
                            tblOperation.row.add(OperationItem);
                        } else {
                            //this is editing;
                            var arrData = tblOperation.data();
                            arrData.splice(data.idx, 1, OperationItem);
                            tblOperation.clear();
                            tblOperation.rows.add(arrData);

                        }
                        tblOperation.draw();
                        $('#frmOperation').trigger('reset');
                        $('#formOperasi').toggle('blind', null, null, function (e) {
                            $('#btnAddOpr').focus();
                        });
                    } else {
                        showNotificationSaveError(data, 'Penambahan gagal.');
                    }
                }
            });
        });
        //NonOperation
        $('#btnSaveNon').click(function () {
            var _data = $('#frmNonOperation').serialize();
            var _url = $('#frmNonOperation').attr('action'); // "ValidateOperation";
            $.ajax({
                type: 'POST',
                data: _data,
                url: _url,
                success: function (data) {
                    if (data.stat == 1) {
                        var NonOperationItem = {
                            ID: $('#id_NonOperation').val(),
                            NonOperationType: $('#NonOperationType').val(),
                            Begin: $('#Begin_NonOperation').val(),
                            End: $('#End_NonOperation').val(),
                            Reason: $('#Reason').val()
                        };
                        if (data.idx == -1) {
                            tblNonOperation.row.add(NonOperationItem);
                           
                        } else {
                            //this is editing;
                            var arrData = tblNonOperation.data();
                            arrData.splice(data.idx, 1, NonOperationItem);
                            tblNonOperation.clear();
                            tblNonOperation.rows.add(arrData);
                        }
                        tblNonOperation.draw();
                        $('#frmNonOperation').trigger('reset');
                        $('#formNonOpr').toggle('blind', null, null, function (e) {
                            $('#btnAddNonOpr').focus();
                        });
                    } else {
                        showNotificationSaveError(data, 'Penambahan gagal.');
                    }
                }
            });
        });
        //Pelumas
        $('#btnSaveOil').click(function () {
            var _data = $('#frmoil').serialize();
            var _url = $('#frmoil').attr('action');
            $.ajax({
                type: 'POST',
                data: _data,
                url: _url,
                success: function (data) {
                    if (data.stat == 1) {
                        var PelumasItem = {
                            ID: $('#id_OilUsedHeavyEqp').val(),
                            IDActivity: $('#IDActivity_OilUsedHeavyEqp').val(),
                            OilType: $('#OilType').val(),
                            Amount: $('#Amount').val()
                        };
                        if (data.idx == -1) {
                            tblOil.row.add(PelumasItem);
                        } else {
                            //this is editing;
                            var arrData = tblOil.data();
                            arrData.splice(data.idx, 1, PelumasItem);
                            tblOil.clear();
                            tblOil.rows.add(arrData);

                        }
                        tblOil.draw();
                        $('#formOil').toggle('blind', null, null, function (e) {
                            $('#btnAddOil').focus();
                        });
                    } else {
                        showNotificationSaveError(data, 'Penambahan gagal.');
                    }
                }
            });
        });

        //bbm
        $('#btnSaveBbm').click(function () {
            var _data = $('#frmfuel').serialize();
            var _url = $('#frmfuel').attr('action');
            $.ajax({
                type: 'POST',
                data: _data,
                url: _url,
                success: function (data) {
                    if (data.stat == 1) {
                        var bbmItem = {
                            ID: $('#id_FuelUsedHeavyEqp').val(),
                            IDActivity: $("IDActivity_FuelUsedHeavyEqp").val(),
                            AmountFuel: $('#AmountFuel').val()
                        };
                        if (data.idx == -1) {
                            tblbbm.row.add(bbmItem);
                        } else {
                            //this is editing;
                            var arrData = tblbbm.data();
                            arrData.splice(data.idx, 1, bbmItem);
                            tblbbm.clear();
                            tblbbm.rows.add(arrData);
                        }
                        $('#frmfuel').trigger('reset');
                        tblbbm.draw();
                        $('#formBBm').toggle('blind', null, null, function (e) {
                            $('#btnAddbbm').focus();
                        });
                    } else {
                        showNotificationSaveError(data, 'Penambahan gagal.');
                    }
                }
            });
            return false;
        });


        //form activity submit
        $('#btnSaveActivity').click(function (e) {
            showSavingNotification('Menyimpan data');
            //Operation
            var _dataItemsOperation = tblOperation.data();
            var _dataOpsend = [
    ]
            $(_dataItemsOperation).each(function (d, e) {
                _dataOpsend.push(e);
            });
            $('#OpItems').val(JSON.stringify(_dataOpsend));
            //NonOperation
            var _dataItemsNonOperation = tblNonOperation.data();
            var _dataNonOpsend = [
    ]
            $(_dataItemsNonOperation).each(function (d, e) {
                _dataNonOpsend.push(e);
            });
            $('#NonOpItems').val(JSON.stringify(_dataNonOpsend));
            //Oil Pelumas
            var _dataItemsPelumas = tblOil.data();
            var _dataPelumasSend = [
    ]
            $(_dataItemsPelumas).each(function (d, e) {
                _dataPelumasSend.push(e);
            });
            $('#OilItems').val(JSON.stringify(_dataPelumasSend));
            //BBM
            var _dataItemsbbm = tblbbm.data();
            var _databbmSend = [
    ]
            $(_dataItemsbbm).each(function (d, e) {
                _databbmSend.push(e);
            });
            $('#fuelitem').val(JSON.stringify(_databbmSend));
            var _data = $('#frmactivity').serializeArray();
            $.ajax({
                type: 'POST',
                data: _data,
                url: '/Equipment/DumpTruckActivities/SaveItemActivities',
                dataType: 'json',
                success: submitFormActivityCallback
            });
        });


        _initTblOperation();
        _initTblNonOperation();
        _initTblBBM();
        _initTblOilUsages();
        _initTimePicker();
        _initAutoComplete();
        _loadDocument();

        if ($('#Code').val().length > 0) {
            $('#Code,#PoliceNumber').attr("readonly", "readonly");
        };

    });
  

</script>
