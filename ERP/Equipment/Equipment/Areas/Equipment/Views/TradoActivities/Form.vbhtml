@ModelType Equipment.TradoActivity
@Code
    ViewData("Title") = "Kegiatan Trado"
    
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
    Dim colWidth = {"60px", "auto", "120px", "80px", "80px", "120px", "80px", "200px", "120px"}
    
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
@Code
    ViewData("Title") = "Kegiatan Trado"
End Code
@code
    Using Html.BeginForm("SaveItemActivities", "TradoActivities", Nothing, FormMethod.Post, New With {.id = "frmactivity", .autocomplete = "off"})
        Using Html.BeginJUIBox("Kegiatan Trado", False, False, False, False, False, "fa fa-truck")
End Code
 <input type="hidden" name="ID" id="tradoActivityID" value="@Model.ID" />
<div class="row">
</div>
<div class="row">
    <div class="form-horizontal">
        <div class="col-md-4 col-sm-12">
            @Html.WriteFormDateInputFor(Function(m) m.Date, "Tanggal",
                                    New With {.format = "dd-mm-yyyy", .autoclose = "true",
                                              .orientation = "top left", .todayBtn = "linked", .language = "id", .daysOfWeekDisabled = {0}},
                                    lgLabelWidth:=5, lgControlWidth:=7)
        </div>
        <div class="col-md-4 col-sm-12">
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
                    NO. Trado
                </label>
                <div class="col-lg-8 col-sm-4">
                    @Html.HiddenFor(Function(m) m.Code)
                    @code
                                If Not IsNothing(Model.Code) Then
                                    @<span class="form-control" id="spCode">@Model.Code</span>
                                Else
                                    @<span class="form-control" id="spCode"></span>
                                End If
                    End Code
                    
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
                    <span class="form-control" id="spMerk">@Model.Merk / @Model.Type</span>
                </div>
            </div>
        </div>
        <div class="col-md-4 col-sm-12">
            <div class="form-group">
                <label class="col-lg-4 col-sm-4 control-label">
                    Supir
                </label>
                <div class="col-lg-7 col-sm-4">
                    @Html.TextBoxFor(Function(m) m.Driver, New With {.class = "form-control"})
                    @Html.HiddenFor(Function(m) m.IDDriver)
                </div>
            </div>
        </div>
    </div>
    <input type="hidden" name="ItemOperation" id="OpItems" value="" />
    <input type="hidden" name="ItemNonOperation" id="NonOpItems" value="" />
    <input type="hidden" name="ItemOil" id="OilItems" value="" />
    <input type="hidden" name="ItemFuel" id="fuelitem" value="" />
    <input type="hidden" name="ItemFuelOut" id="fuelitemout" value="" />
</div>
@code
        End Using
        Using Html.BeginJUIBox("Operation", True, False, False, False, False, "")
End code
<div class="table-responsive">
    <table class="table table-bordered" id="tblOperation">
        <colgroup>@WriteOperationTableColumnGroup()</colgroup>
        <thead>
            <tr>
                <th>
                    No.
                </th>
                <th>
                    Jenis Muatan / Alat Berat
                </th>
                <th>
                    Operator Alat Berat
                </th>
                <th>
                    Lokasi Asal
                </th>
                <th>
                    Waktu Berangkat
                </th>
                <th>
                    Tujuan
                </th>
                <th>
                    Waktu Tiba
                </th>
                <th>
                    KM Awal
                </th>
                <th>
                    KM Akhir
                </th>
                <th>
                    Jarak [KM]
                </th>
                <th>
                    <button type="button" class="btn btn-danger btn-label-left" id="btnformOp">
                        <span><i class="fa fa-plus"></i></span>Tambah</button>
                </th>
            </tr>
        </thead>
    </table>
</div>
<div id="formOperasi" style="display: none;">
    <form>
    </form>
    <form class="form-horizontal" autocomplete="Off" id="frmOperation" action="/TradoActivities/ValidateOperation"
    method="post">
    @Html.Hidden("IdOperation", 0)
    @Html.Hidden("ID", 0, New With {.id = "id_Operation"})
    @Html.Hidden("IDActivity", 0, New With {.id = "IDActivity_Operation"})
    @Html.Hidden("rowIdx", -1, New With {.id = "rowIdx_Operation"})
    @Html.Hidden("DepartureTime", "", New With {.id = "DepartureTime_Operation"})
    @Html.Hidden("ArrivalTime", "", New With {.id = "ArrivalTime_Operation"})
    <div class="form-group">
        <label class="col-lg-3 col-sm-4 control-label">
            Jenis Muatan / Alat Berat
        </label>
        <div class="col-lg-3 col-sm-4">
            @Html.TextBox("LoadType", Nothing, New With {.class = "form-control"})
        </div>
    </div>
    <div class="form-group">
        <label class="col-lg-3 col-sm-4 control-label">
            Operator
        </label>
        <div class="col-lg-3 col-sm-4">
            @Html.TextBox("Operator", Nothing, New With {.class = "form-control", .id = "OperatorHeavyEqp"})
        </div>
    </div>
    <div class="form-group">
        <label class="col-lg-3 col-sm-4 control-label">
            Lokasi Asal
        </label>
        <div class="col-lg-3 col-sm-4">
            @Html.TextBox("SourceLocation", Nothing, New With {.class = "form-control"})
        </div>
    </div>
    <div class="form-group">
        <label class="col-lg-3 col-sm-4 control-label">
            Jam Berangkat
        </label>
        <div class="col-lg-3 col-sm-4">
            <div class="input-group">
                <span id="sp_DepartureTime_Operation" class="form-control text-center">--:--</span>
                <span class="input-group-addon">Tiba</span> <span id="sp_ArrivalTime_Operation" class="form-control text-center">
                    --:--</span>
            </div>
        </div>
    </div>
    <div class="form-group">
        <label class="col-lg-3 col-sm-4 control-label">
            &nbsp;
        </label>
        <div class="col-lg-3 col-sm-4" style="padding-left: 25px; padding-right: 25px">
            <div id="ts_DepartureTime_Operation">
            </div>
        </div>
    </div>
    <div class="form-group">
        <label class="col-lg-3 col-sm-4 control-label">
            Lokasi Tujuan
        </label>
        <div class="col-lg-3 col-sm-4">
            @Html.TextBox("Destination", Nothing, New With {.class = "form-control"})
        </div>
    </div>
    <div class="form-group">
        <label class="col-lg-3 col-sm-4 control-label">
            KM Awal
        </label>
        <div class="col-lg-2 col-sm-4">
            @Html.DecimalInput("BeginKM", 0.0, ".", ",", "1")
        </div>
    </div>
    <div class="form-group">
        <label class="col-lg-3 col-sm-4 control-label">
            KM Akhir
        </label>
        <div class="col-lg-2 col-sm-4">
            @Html.DecimalInput("EndKM", 0.0, ".", ",", "1")
        </div>
    </div>
    <div class="form-group">
        <label class="col-lg-3 col-sm-4 control-label">
            Jarak
        </label>
        <div class="col-lg-2 col-sm-4">
            <div class="input-group">
                @Html.DecimalInput("Distance", 0.0, ".", ",", "1")
                <div class="input-group-addon">
                    KM</div>
            </div>
        </div>
    </div>
    <div class="form-group">
        <div class="col-sm-offset-3 col-sm-10">
            <button type="button" class="btn btn-primary" id="btnSaveOp">
                Simpan</button>
            <button type="button" class="btn btn-default" onclick="$('#btnformOp').click()">
                batal</button>
        </div>
    </div>
    </form>
</div>
@code
        End Using
End Code
<div class="row">
<div class="col-lg-12 col-sm-12">
        @code
                Using Html.BeginJUIBox("Non Operasi", True, False, False, False, False, "")
        End code
        <div class="table-responsive">
            <table class="table table-bordered" id="tblNonOperation">
                <thead>
                    <tr>
                        <th>
                            No
                        </th>
                        <th>
                            Jenis Pekerjaan
                        </th>
                        <th>
                            Alasan Non Operasional
                        </th>
                        <th>
                            <button type="button" class="btn btn-danger btn-label-left" id="btnNonOpr">
                                <span><i class="fa fa-plus"></i></span>Tambah</button>
                        </th>
                    </tr>
                </thead>
            </table>
        </div>
        <div id="formNonOpr" style="display: none;">
            @code
                        Using Html.BeginForm("ValidateNonOperation", "TradoActivities", Nothing, FormMethod.Post, New With {.class = "form-horizontal", .autocomplete = "Off", .id = "frmNonOperation"})
            End code
            @Html.Hidden("ID", 0, New With {.id = "id_NonOperation"})
            @Html.Hidden("IDActivity", 0, New With {.id = "IDActivity_NonOperation"})
            @Html.Hidden("rowIdx", -1, New With {.id = "rowIdx_NonOperation"})
            <div class="form-horizontal">
                <div class="form-group">
                    <label class="col-lg-4 col-sm-4 control-label ">
                        Jenis Pekerjaan
                    </label>
                    <div class="col-lg-5 col-sm-4">
                        @Html.TextArea("NonOperationType", Nothing, New With {.class = "form-control"})
                    </div>
                </div>
                <div class="form-group">
                    <label class="col-lg-4 col-sm-4 control-label">
                        Alasan
                    </label>
                    <div class="col-lg-5 col-sm-4">
                        @Html.TextArea("Reason", Nothing, New With {.class = "form-control"})
                    </div>
                </div>
                <div class="form-group">
                    <div class="col-sm-offset-3 col-sm-10">
                        <button type="button" class="btn btn-primary" id="btnSaveNon">
                            Simpan</button>
                        <button type="button" class="btn btn-default" onclick="$('#btnNonOpr').click()">
                            batal</button>
                    </div>
                </div>
                @code
                            End Using
                End Code
            </div>
            @code
                    End Using
            End Code
        </div>
    </div>
    <div class="col-lg-12 col-sm-12">
        @code
                Using Html.BeginJUIBox("Pemakaian BBM", True, False, False, False, False, "")
        End code
        <div class="table-responsive">
            <table id="tblbbm" class="table table-bordered">
                <thead>
                    <tr>
                        <th>
                            No
                        </th>
                        <th>
                            Waku Pengisian
                        </th>
                        <th>
                            Jumlah(Liter)
                        </th>
                        <th>
                            Nama / Lokasi SPBU
                        </th>
                        <th>
                            <button type="button" class="btn btn-danger btn-label-left" id="btnAddFuel">
                                <span><i class="fa fa-plus"></i></span>Tambah</button>
                        </th>
                    </tr>
                </thead>
            </table>
        </div>
        <div id="formbbm" style="display: none;">
            <!--form ini dibuat krn frmfuel tidak terdeteksi -->
            @code
                        Using Html.BeginForm("ValidateFuel", "TradoActivities", Nothing,
                                             FormMethod.Post,
                                             New With {.class = "form-horizontal",
                                                       .autocomplete = "Off", .id = "frmfuel"})
            End code
            @Html.Hidden("ID", 0, New With {.id = "id_FuelUsedTrado"})
            @Html.Hidden("IDActivity", 0, New With {.id = "IDTradoActivity_FuelUsed"})
            @Html.Hidden("rowIdx", -1, New With {.id = "rowIdx_FuelUsedTrado"})
            @Html.Hidden("TimeFill", "", New With {.id = "TimeFill_bbm"})
            <div class="form-group">
                <label class="col-lg-3 col-sm-4 control-label">
                    Waktu Pengisian
                </label>
                <div class="col-lg-3 col-sm-4">
                    <span id="sp_TimeFill_bbm" class="form-control text-center">--:--</span>
                </div>
            </div>
            <div class="form-group">
                <label class="col-lg-3 col-sm-4 control-label">
                    &nbsp;
                </label>
                <div class="col-lg-3 col-sm-4" style="padding-left: 25px; padding-right: 25px">
                    <div id="ts_TimeFill_bbm">
                    </div>
                </div>
            </div>
            <div class="form-group">
                <label class="col-lg-3 col-sm-4 control-label">
                    Jumlah
                </label>
                <div class="col-lg-2 col-sm-4">
                    <div class="input-group">
                        @Html.DecimalInput("AmountFuel", 0)
                        <div class="input-group-addon">
                            Liter</div>
                    </div>
                </div>
            </div>
            <div class="form-group">
                <label class="col-lg-3 col-sm-4 control-label">
                    Nama / Lokasi SPBU
                </label>
                <div class="col-lg-5 col-sm-4">
                    @Html.TextBox("Location", Nothing, New With {.class = "form-control"})
                </div>
            </div>
            <div class="form-group">
                <div class="col-sm-offset-3 col-sm-10">
                    <button type="button" class="btn btn-primary" id="btnSaveFuel">
                        Simpan</button>
                    <button type="button" class="btn btn-default" onclick="$('#btnAddFuel').click()">
                        batal</button>
                </div>
            </div>
            @code
                        End Using
            End Code
        </div>
        @code
                End Using
        End Code
    </div>
    <div class="col-lg-12 col-sm-12">
        @code
                Using Html.BeginJUIBox("Pemakaian BBM Diluar Operasi", True, False, False, False, False, "")
        End code
        <div class="table-responsive">
            <table id="tblbbmout" class="table table-bordered">
                <thead>
                    <tr>
                        <th>
                            No
                        </th>
                        <th>
                            BBM Dikeluarkan Untuk
                        </th>
                        <th>
                            Operator
                        </th>
                        <th>
                            Jumlah(Liter)
                        </th>
                        <th>
                            <button type="button" class="btn btn-danger btn-label-left" id="btnAddFuelOut">
                                <span><i class="fa fa-plus"></i></span>Tambah</button>
                        </th>
                    </tr>
                </thead>
            </table>
        </div>
        <div id="formbbmout" style="display: none;">
            @code
                        Using Html.BeginForm("ValidateFuelOut", "TradoActivities", Nothing, FormMethod.Post, New With {.class = "form-horizontal", .autocomplete = "Off", .id = "frmfuelout"})
            End code
            @Html.Hidden("ID", 0, New With {.id = "id_FuelUsedOutTrado"})
            @Html.Hidden("IDActivity", 0, New With {.id = "IDTradoActivity_FuelUsedOut"})
            @Html.Hidden("rowIdx", -1, New With {.id = "rowIdx_FuelUsedOutTrado"})
            <div class="form-group">
                <label class="col-lg-3 col-sm-4 control-label">
                    Alokasi BBM
                </label>
                <div class="col-lg-3 col-sm-4">
                    @Html.TextBox("Alocation", Nothing, New With {.class = "form-control"})
                </div>
            </div>
            <div class="form-group">
                <label class="col-lg-3 col-sm-4 control-label">
                    Jumlah
                </label>
                <div class="col-lg-3 col-sm-4">
                    <div class="input-group">
                        @Html.DecimalInput("AmountFuelOut", 0)
                        <div class="input-group-addon">
                            Liter</div>
                    </div>
                </div>
            </div>
            <div class="form-group">
                <label class="col-lg-3 col-sm-4 control-label">
                    Operator
                </label>
                <div class="col-lg-3 col-sm-4">
                    @Html.TextBox("Operator", Nothing, New With {.class = "form-control", .id = "OperatorBBM"})
                </div>
            </div>
            <div class="form-group">
                <div class="col-sm-offset-3 col-sm-10">
                    <button type="button" class="btn btn-primary" id="btnSaveBbmOut">
                        Simpan</button>
                    <button type="button" class="btn btn-default" onclick="$('#btnAddFuelOut').click()">
                        batal</button>
                </div>
            </div>
            @code
                        End Using
                   
            End Code
        </div>
        @code
                End Using
        End Code
    </div>
    
    <div class="col-lg-12 col-sm-12">
        @code
                Using Html.BeginJUIBox("Pemakaian Oli & Spare Part", True, False, False, False, False, "")
        End code
        <div class="table-responsive">
            <table id="tbloil_sparepart" class="table table-bordered">
                <thead>
                    <tr>
                        <th>
                            No
                        </th>
                        <th>
                            Jenis Oli
                        </th>
                        <th>
                            Qty
                        </th>
                        <th>
                            Satuan
                        </th>
                        <th>
                            <button type="button" class="btn btn-danger btn-label-left" id="btnAddoil_sparepart">
                                <span><i class="fa fa-plus"></i></span>Tambah</button>
                        </th>
                    </tr>
                </thead>
            </table>
        </div>
        <div id="formoil_sparepart" style="display: none;">
            @Using Html.BeginForm("ValidateOilAndSparePart", "TradoActivities", Nothing, FormMethod.Post, New With {.class = "form-horizontal", .autocomplete = "Off", .id = "frmoil_sparepart"})
                @<div>
                    @Html.Hidden("ID", 0, New With {.id = "id_OilSparePart"})
                    @Html.Hidden("IDActivity", 0, New With {.id = "IDActivity_OilSparePart"})
                    @Html.Hidden("rowIdx", -1, New With {.id = "rowIdx_OilSparePart"})
                    <div class="form-group">
                        <label class="col-lg-4 col-sm-4 control-label ">
                            Jenis Oli / Spare Part
                        </label>
                        <div class="col-lg-3 col-sm-3">
                            @Html.Hidden("IdPelumas", 0)
                            @Html.TextBox("OilOrSparePartType", Nothing, New With {.class = "form-control"})
                        </div>
                    </div>
                    <div class="form-group">
                        <label class="col-lg-4 col-sm-4 control-label">
                            Jumlah
                        </label>
                        <div class="col-lg-2 col-sm-2">
                            <div class="input-group">
                                @Html.DecimalInput("Amount", 0, ".", ",", 1)
                                <div class="input-group-addon">
                                    Liter</div>
                            </div>
                        </div>
                    </div>
                    <div class="form-group">
                        <label class="col-lg-4 col-sm-4 control-label">
                            Satuan
                        </label>
                        <div class="col-lg-3 col-sm-3">
                            @Html.TextBox("Unit", Nothing, New With {.class = "form-control"})
                        </div>
                    </div>
                    <div class="form-group">
                        <div class="col-sm-offset-3 col-sm-10">
                            <button type="button" class="btn btn-primary" id="btnSaveOil_SparePart">
                                Simpan</button>
                            <button type="button" class="btn btn-default" onclick="$('#btnAddoil_sparepart').click()">
                                batal</button>
                        </div>
                    </div>
                </div>
                    End Using
        </div>
        @code
                End Using
        End Code
    </div>
</div>
@code
        Using Html.BeginJUIBox("", True, False, False, False, False, "")
End Code

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
                                If Not IsNothing(Model.Driver) Then
                                    @<span class="form-control" id="sp_operatorName">@Model.Driver</span>
                                  
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
<div class="col-lg-offset-5 col-sm-offset-3">
    <button type="button" class="btn btn-primary" id="btnSaveActivity">
        Simpan</button>
    <a href="@Url.Action("Index", "TradoActivities")" class="btn btn-default" >Batal</a>
</div>
@code
        End Using
End Code
<!--end form-->
@code
    End Using
End Code
<!-- end form-->
<link rel="stylesheet" type="text/css" href="@Url.Content("~/plugins/datatables/dataTables.bootstrap.css")" />
<link href="../../../../plugins/bootstrap-datetimepicker/bootstrap-datetimepicker.min.css"
    rel="stylesheet" type="text/css" />
<script src="@Url.Content("~/plugins/datatables/jquery.dataTables.js")" type="text/javascript"></script>
<script src="@Url.Content("~/plugins/datatables/dataTables.bootstrap.js")" type="text/javascript"></script>
<script src="@Url.Content("~/plugins/datatables/sum.js")" type="text/javascript"></script>
<script src="@Url.Content("~/plugins/datatables/datatablehelper.js")" type="text/javascript"></script>
<style>
    .ui-state-focus, #formbbm .ui-state-focus, #formOperasi .ui-state-focus
    {
        background-color: Blue;
        background-image: none;
    }
    .ui-widget-header, #formbbm .ui-widget-header, #formOperasi .ui-widget-header
    {
        background-color: #00d11c;
        background-image: none;
    }
</style>
<script type="text/javascript">
    var tblOperation = null;
    var tblNonOp = null;
    var tblOilSparePart = null;
    var tblbbm = null;
    var tblbbmout = null;

    //set
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
                // _calculateHour();
            }
        });
    }

    //
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
                url: '/Equipment/TradoActivities/Detail/' + data.id
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
            window.location = '/Equipment/TradoActivities/';
        }
    }; //submitFormActivityCallback

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
    //init table operation
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
            {'data': 'Operator' },
            { 'data': 'SourceLocation', 'sClass': 'text-right' }, //
            {'data': 'DepartureTime', 'sClass': 'text-right' }, //
            {'data': 'ArrivalTime', 'sClass': 'text-right' },
            { 'data': 'Destination' },
            { 'data': 'BeginKM', 'sClass': 'text-right' },
            { 'data': 'EndKM', 'sClass': 'text-right' },
            { 'data': 'Distance' },
            { 'data': 'IDActivity', 'mRender': _renderEditColumnOp, 'sClass': 'text-center' }
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
                url: '/TradoActivities/DeleteOperation',
                success: function (data) {
                    if (data.stat == 1) {
                        row.remove().draw();
                    }
                }
            });
        }).on('click', '.btnEditItemOperation', function (d) {
            var tr = $(this).closest('tr');
            var row = tblOperation.row(tr);
            $('#LoadType').focus();
            var dataItem = row.data();
            $("#id_Operation").val(dataItem.ID);
            $("#rowIdx_Operation").val(row.index());

            $("#IDActivity_Operation").val(dataItem.IDActivity);
            $('#LoadType').val(dataItem.LoadType);
            $('#OperatorHeavyEqp').val(dataItem.Operator);
            $('#SourceLocation').val(dataItem.SourceLocation);
            $('#DepartureTime_Operation').val(dataItem.DepartureTime);
            $('#BeginKM').val(dataItem.BeginKM);
            $('#EndKM').val(dataItem.EndKM);
            $('#ArrivalTime_Operation').val(dataItem.ArrivalTime);
            $('#sp_DepartureTime_Operation').text(dataItem.DepartureTime);
            $('#sp_ArrivalTime_Operation').text(dataItem.ArrivalTime);
            $('#Destination').val(dataItem.Destination);
            $('#Distance').val(dataItem.Distance);
            setTimePicker('DepartureTime_Operation', 'ArrivalTime_Operation', '00:00', '23:59:59');
            $('#formOperasi').toggle('blind', null, null, function (e) {
                $('#LoadType').focus();
            });
        });
    }

    //init table non operation
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
            { data: "Reason" },
            { data: "IDActivity", sClass: "text-center", mRender: _renderEditColumnNon }
        ];


        _GeneralTable(arrColumnsNon);
        tblNonOp = $('#tblNonOperation').DataTable(datatableDefaultOptions)
        .on('click', '.btnRemoveItemNonOperation', function (d) {
            if (confirm('Hapus item ini ?') == false) {
                return;
            }
            var tr = $(this).closest('tr');
            var row = tblNonOp.row(tr);
            if (row.data().ID == 0) {
                row.remove().draw()
                return;
            }
            $.ajax({
                type: 'POST',
                data: {
                    id: row.data().ID
                },
                url: '/Equipment/TradoActivities/DeleteNonOperation',
                success: function (data) {
                    if (data.stat == 1) {
                        row.remove().draw();
                    }
                }
            });
        }).on('click', '.btnEditItemNonOperaton', function (d) {
            var tr = $(this).closest('tr');
            var row = tblNonOp.row(tr);

            var dataItem = row.data();
            $('#id_NonOperation').val(dataItem.ID);
            $('#rowIdx_NonOperation').val(row.index());
            $('#NonOperationType').val(dataItem.NonOperationType);
            $('#IDActivity_NonOperation').val(dataItem.IDActivity);
            $('#Reason').val(dataItem.Reason);
            $('#btnSaveNon').text('Simpan Perubahan');

            $('#formNonOpr').toggle('blind', null, null, function (e) {
                $('#NonOperationType').focus();
                $('#btnNonOpr').show('fast');
            });
        });
    }


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
        var arrColumnsOil_SparePart = [
            { 'data': 'ID', 'sClass': 'text-center' },
            { 'data': 'OilOrSparePartType', 'sClass': 'text-center' },
            { 'data': 'Amount', 'sClass': 'text-right' },
            { 'data': 'Unit' },
            { 'data': 'IDActivity', 'mRender': _renderEditColumnOil, 'sClass': 'text-center' }
        ];
        _GeneralTable(arrColumnsOil_SparePart);
        tblOilSparePart = $('#tbloil_sparepart').DataTable(datatableDefaultOptions).on('click', '.btnRemoveItemOil', function (d) {

            if (confirm('Hapus item ini ?') == false) {
                return;
            }
            var tr = $(this).closest('tr');
            var row = tblOilSparePart.row(tr);
            if (row.data().ID == 0) {
                row.remove().draw()
                return;
            }
            $.ajax({
                type: 'POST',
                data: {
                    id: row.data().ID
                },
                url: '/TradoActivities/DeleteOil_SparePart',
                success: function (data) {
                    if (data.stat == 1) {
                        row.remove().draw();
                    }
                }
            });
        }).on('click', '.btnEditItemOil', function (d) {
            var tr = $(this).closest('tr');
            var row = tblOilSparePart.row(tr);

            $('#OilOrSparePartType').focus();
            var dataItem = row.data();
            $('#id_OilSparePart').val(dataItem.ID);
            $('#IDActivity_OilSparePart').val(dataItem.IDActivity);
            $("#rowIdx_OilSparePart").val(row.index());
            $('#OilOrSparePartType').val(dataItem.OilOrSparePartType);
            $('#Amount').val(dataItem.Amount);
            $('#Unit').val(dataItem.Unit);
            $('#formoil_sparepart').toggle('blind', null, null, function (e) {
                $('#OilOrSparePartType').focus();
            });
        });
    }

    //init table bbm out
    var _initTblBBMOut = function () {
        var _renderEditColumnbbm = function (data, type, row) {
            if (type == 'display') {
                return (
          '<div class=\'btn-group\' role=\'group\'>' +
          '<button type=\'button\' class=\'btn btn-default btn-xs btnEditItemFuelOut\' ><i class=\'fa fa-edit\'></i></button>' +
          '<button type=\'button\' class=\'btn btn-default btn-xs btnRemoveItemFuelOut\'><i class=\'fa fa-remove\'></i></button>' +
          '</div>');
            }
            return data;
        }
        var arrColumnsbbmOut = [
            { data: "ID", sClass: "text-center" },
            { data: 'Alocation' },
            { data: 'Operator' },
            { data: "AmountFuelOut", sClass: "text-right" },
            { data: "IDActivity", mRender: _renderEditColumnbbm, "sClass": "text-center"}];
        _GeneralTable(arrColumnsbbmOut);
        tblbbmout = $('#tblbbmout').DataTable(datatableDefaultOptions)
        .on('click', '.btnRemoveItemFuelOut', function (d) {
            if ($('#formbbmout').is(':visible')) {
                $('#formbbmout').hide('fast');
                $('#btnAddFuelOut').show('fast');
            }
            if (confirm('Hapus item ini ?') == false) {
                return;
            }
            var tr = $(this).closest('tr');
            var row = tblbbmout.row(tr);
            if (row.data().ID == 0) {
                row.remove().draw()
                return;
            }
            $.ajax({
                type: 'POST',
                data: {
                    id: row.data().ID
                },
                url: '/TradoActivities/DeleteFuel',
                success: function (data) {
                    if (data.stat == 1) {
                        row.remove().draw();
                    }
                }
            });
        }).on('click', '.btnEditItemFuelOut', function (d) {
            var tr = $(this).closest('tr');
            var row = tblbbmout.row(tr);

            $('#AmountFuelOut').focus();
            var dataItem = row.data();

            $("#id_FuelUsedOutTrado").val(dataItem.ID);
            $("#IDTradoActivity_FuelUsedOut").val(dataItem.IDActivity);
            $("#rowIdx_FuelUsedOutTrado").val(row.index());
            $('#Alocation').val(dataItem.Alocation);
            $('#OperatorBBM').val(dataItem.Operator);
            $('#AmountFuelOut').val(dataItem.AmountFuelOut);
            $('#formbbmout').toggle('blind', null, null, function (e) {
                $('#AmountFuelOut').focus();
                $('#btnAddFuelOut').show('fast');
            });
        });
    }
    //
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
            { data: 'TimeFill', sClass: 'text-right' },
            { data: "AmountFuel", sClass: "text-right" },
            { data: 'Location' },
            { data: "IDActivity", mRender: _renderEditColumnbbm, "sClass": "text-center"}];
        _GeneralTable(arrColumnsbbm);
        tblbbm = $('#tblbbm').DataTable(datatableDefaultOptions)
        .on('click', '.btnRemoveItemFuel', function (d) {
            if ($('#formbbm').is(':visible')) {
                $('#formbbm').hide('fast');
                $('#btnAddFuel').show('fast');
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
                url: '/TradoActivities/DeleteFuel',
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

            $("#id_FuelUsedTrado").val(dataItem.ID);
            $("#IDTradoActivity_FuelUsed").val(dataItem.IDTradoActivity);
            $("#rowIdx_FuelUsedTrado").val(row.index());
            $('#AmountFuel').val(dataItem.AmountFuel);
            $('#TimeFill_bbm').val(dataItem.TimeFill);
            $('#sp_TimeFill_bbm').text(dataItem.TimeFill);
            setTimePicker('TimeFill_bbm', 'TimeFill_bbm', '00:00', '23:59:59');
            $('#Location').val(dataItem.Location);
            $('#formbbm').toggle('blind', null, null, function (e) {
                $('#AmountFuel').focus();
            });
        });
    };

    //load document
    var _loadDocument = function () {
        if (parseInt($('#tradoActivityID').val()) != 0) {
            $.ajax({
                method: 'post',
                url: '/TradoActivities/loadActivity',
                data: {
                    id: $('#tradoActivityID').val()
                },
                dataType: 'json',
                success: function (data) {
                    if (data.Operations.length > 0) {
                        tblOperation.rows.add(data.Operations).draw();
                    }
                    if (data.NonOperations.length > 0) {
                        tblNonOp.rows.add(data.NonOperations).draw();
                    }
                    if (data.OilUsages.length > 0) {
                        tblOilSparePart.rows.add(data.OilUsages).draw();
                    }
                    if (data.FuelConsumed.length > 0) {
                        tblbbm.rows.add(data.FuelConsumed).draw();
                    }
                    if (data.FuelConsumedOut.length > 0) {
                        tblbbmout.rows.add(data.FuelConsumedOut).draw();
                    }
                }
            });
        }
    }
    //


    var _initAutoComplete = function () {
        $('#LoadType').autocomplete({
            source: function (request, response) {
                $.ajax({
                    url: '/Equipment/HeavyEqpActivities/getEqpAutoComplete',
                    data: {
                        term: $('#Code').val()
                    },
                    dataType: 'json',
                    success: function (data) {
                        response($.map(data, function (obj) {
                            return {
                                label: obj.Value,
                                value: obj.Code, //+ ","+obj.Merk+"/"+obj.Type+"("+obj.Species+")",
                                merk: obj.Merk,
                                type: obj.Type,
                                kind: obj.Species,
                                areaId: obj.IDArea,
                                idOpr: obj.IDOpr,
                                OprName: obj.OprName
                            }
                        }));
                    }
                });
            },
            change: function (event, ui) {
                if (ui.item != null) {
                    $("#OperatorHeavyEqp").val(ui.item.OprName);
                }
            }
        }).data('ui-autocomplete')._renderItem = function (ul, item) {
            //location
            return ($('<li>').append('<a><strong>' + item.value + '</strong>, <i><strong>' +
            item.merk + '/' + item.type + '</strong> (' + item.kind + '</i>)</a>').appendTo(ul));
        };

        //        $('#OilType').autocomplete({
        //            source: function (request, response) {
        //                $.ajax({
        //                    url: '/DumpTruckActivities/AutocompleteOilType',
        //                    data: {
        //                        term: $('#OilType').val()
        //                    },
        //                    dataType: 'json',
        //                    success: function (data) {
        //                        response($.map(data, function (obj) {
        //                            return {
        //                                label: obj.Value,
        //                                value: obj.OilName
        //                            }
        //                        }));
        //                    }
        //                });
        //            }
        //        });
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
        $('#SourceLocation,#Destination').autocomplete({

            source: function (request, response) {
                $.ajax({
                    url: '/TradoActivities/AutocompleteLocation',
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


        $('#OperatorHeavyEqp').autocomplete({
            source: function (request, response) {
                $.ajax({
                    url: '/HeavyEqpActivities/OperatorType',
                    data: {
                        Query: $('#OperatorHeavyEqp').val()
                    },
                    dataType: 'json',
                    success: function (data) {
                        response($.map(data, function (obj) {
                            return {
                                label: obj.Value,
                                value: obj.FullName,
                                id: obj.ID
                            }
                        }));
                    }
                });
            }
        });

        $('#Driver').autocomplete({
            source: function (request, response) {
                $.ajax({
                    url: '/DumpTruckActivities/AutocompleteDrivers',
                    data: {
                        term: $('#Driver').val()
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
                    $('#IDDriver').val(0)
                    $('#IDDriver').parent().parent().addClass('has-error');
                } else {
                    $('#IDDriver').val(ui.item.id)
                    $('#sp_operatorName').text(ui.item.value);
                    $('#IDDriver').parent().parent().removeClass('has-error');
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

        $('#btnformOp').click(function () {
            $('#id_Operation').val(0);
            $('#frmOperation').trigger("reset");
            $('#frmOperation,.form-group').removeClass('has-error');
            if ($('#formOperasi').is(":hidden")) {
                $(this).hide();
            } else {
                $(this).show();
            }
            $('#formOperasi').slideToggle();
        });

        $('#btnNonOpr').click(function () {
            $('#id_NonOperation').val(0);

            $('#frmNonOperation').trigger("reset");
            $('#frmNonOperation,.form-group').removeClass('has-error');
            if ($('#formNonOpr').is(":hidden")) {
                $(this).hide();
            } else {
                $(this).show('slow');
            }
            $('#formNonOpr').toggle("drop");
        });

        $('#btnAddoil_sparepart').click(function () {
            // $('#frmoil_sparepart').trigger('reset');
            document.getElementById('frmoil_sparepart').reset();
            $('#id_OilSparePart').val(0);
            $('#frmoil_sparepart,.form-group').removeClass('has-error');
            //$('#rowIdx_OilSparePart').val(-1);
            if ($('#formoil_sparepart').is(":hidden")) {
                $(this).hide();
            } else {
                $(this).show();
            }
            $('#formoil_sparepart').toggle('blind');
        });


        $('#btnAddFuel').click(function () {
            $('#id_FuelUsedTrado').val(0);
            document.getElementById('frmfuel').reset();
            $('#frmfuel,.form-group').removeClass('has-error');
            // $('#rowIdx_FuelUsedTrado').val(-1);
            if ($('#formbbm').is(":hidden")) {
                $(this).hide();
            } else {
                $(this).show();
            }
            $('#formbbm').slideToggle();
        });

        $('#btnAddFuelOut').click(function () {
            document.getElementById('frmfuelout').reset();
            $('#id_FuelUsedOutTrado').val(0);
            $('#frmfuelout,.form-group').removeClass('has-error');
            if ($('#formbbmout').is(":hidden")) {
                $(this).hide();
            } else {
                $(this).show();
            }
            $('#formbbmout').slideToggle();
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
                            LoadType: $('#LoadType').val(),
                            Operator: $('#OperatorHeavyEqp').val(),
                            SourceLocation: $('#SourceLocation').val(),
                            DepartureTime: $('#DepartureTime_Operation').val(),
                            ArrivalTime: $('#ArrivalTime_Operation').val(),
                            Destination: $('#Destination').val(),
                            BeginKM: $('#BeginKM').val(),
                            EndKM: $('#EndKM').val(),
                            Distance: $('#Distance').val(),
                            IDActivity: $("#IDActivity_Operation").val()
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
                        $('#frmOperation').trigger("reset");
                        $('#frmOperation,.form-group').removeClass('has-error');
                        tblOperation.draw();
                        $('#formOperasi').toggle('blind', null, null, function (e) {
                            $('#btnformOp').show('fast');
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
                            IDActivity: $('#IDActivity_NonOperation').val(),
                            NonOperationType: $('#NonOperationType').val(),
                            Reason: $('#Reason').val()
                        };
                        if (data.idx == -1) {
                            tblNonOp.row.add(NonOperationItem);
                            console.log('data added!');
                        } else {
                            //this is editing;
                            var arrData = tblNonOp.data();
                            arrData.splice(data.idx, 1, NonOperationItem);
                            tblNonOp.clear();
                            tblNonOp.rows.add(arrData);
                        }
                        $('#frmNonOperation').trigger('reset');
                        tblNonOp.draw();
                        $('#formNonOpr').toggle('blind', null, null, function (e) {
                            $('#btnNonOpr').show();
                        });
                    } else {
                        showNotificationSaveError(data, 'Penambahan gagal.');
                    }
                }
            });
        });
        //Pelumas
        $('#btnSaveOil_SparePart').click(function () {
            var _data = $('#frmoil_sparepart').serialize();
            var _url = $('#frmoil_sparepart').attr('action');
            $.ajax({
                type: 'POST',
                data: _data,
                url: _url,
                success: function (data) {
                    if (data.stat == 1) {
                        var PelumasItem = {
                            ID: $('#id_OilSparePart').val(),
                            IDActivity: $('#IDActivity_OilSparePart').val(),
                            OilOrSparePartType: $('#OilOrSparePartType').val(),
                            Amount: $('#Amount').val(),
                            Unit: $('#Unit').val()
                        };
                        if (data.idx == -1) {
                            tblOilSparePart.row.add(PelumasItem);
                        } else {
                            //this is editing;
                            var arrData = tblOilSparePart.data();
                            arrData.splice(data.idx, 1, PelumasItem);
                            tblOilSparePart.clear();
                            tblOilSparePart.rows.add(arrData);

                        }
                        $('#frmoil_sparepart').trigger('reset');
                        tblOilSparePart.draw();
                        $('#formoil_sparepart').toggle('blind', null, null, function (e) {
                            $('#btnAddoil_sparepart').show('slow');
                        });
                    } else {
                        showNotificationSaveError(data, 'Penambahan gagal.');
                    }
                }
            });
        });

        //bbm
        $('#btnSaveFuel').click(function () {
            var _data = $('#frmfuel').serialize();
            var _url = $('#frmfuel').attr('action');
            $.ajax({
                type: 'POST',
                data: _data,
                url: _url,
                success: function (data) {
                    if (data.stat == 1) {
                        var bbmItem = {
                            ID: $('#id_FuelUsedTrado').val(),
                            IDActivity: $("IDTradoActivity_FuelUsed").val(),
                            AmountFuel: $('#AmountFuel').val(),
                            TimeFill: $('#TimeFill_bbm').val(),
                            Location: $('#Location').val()
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
                        $('#formbbm').toggle('blind', null, null, function (e) {
                            $('#btnAddFuel').show();
                        });
                    } else {
                        showNotificationSaveError(data, 'Penambahan gagal.');
                    }
                }
            });
            return false;
        });

        //bbm out
        $('#btnSaveBbmOut').click(function () {
            var _data = $('#frmfuelout').serialize();
            var _url = $('#frmfuelout').attr('action');
            $.ajax({
                type: 'POST',
                data: _data,
                url: _url,
                success: function (data) {
                    if (data.stat == 1) {
                        var bbmOutItem = {
                            ID: $('#id_FuelUsedOutTrado').val(),
                            IDActivity: $("IDTradoActivity_FuelUsedOut").val(),
                            AmountFuelOut: $('#AmountFuelOut').val(),
                            Operator: $('#OperatorBBM').val(),
                            Alocation: $('#Alocation').val()
                        };
                        if (data.idx == -1) {
                            tblbbmout.row.add(bbmOutItem);
                        } else {
                            //this is editing;
                            var arrData = tblbbmout.data();
                            arrData.splice(data.idx, 1, bbmOutItem);
                            tblbbmout.clear();
                            tblbbmout.rows.add(arrData);
                        }
                        $('#frmfuelout').trigger('reset');
                        tblbbmout.draw();
                        $('#formbbmout').toggle('blind', null, null, function (e) {
                            $('#btnAddFuelOut').show();
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
            var _dataOpsend = []
            $(_dataItemsOperation).each(function (d, e) {
                _dataOpsend.push(e);
            });
            $('#OpItems').val(JSON.stringify(_dataOpsend));
            //NonOperation
            var _dataItemsNonOperation = tblNonOp.data();
            var _dataNonOpsend = []
            $(_dataItemsNonOperation).each(function (d, e) {
                _dataNonOpsend.push(e);
            });
            $('#NonOpItems').val(JSON.stringify(_dataNonOpsend));
            //Oil Pelumas
            var _dataItemsPelumas = tblOilSparePart.data();
            var _dataPelumasSend = []
            $(_dataItemsPelumas).each(function (d, e) {
                _dataPelumasSend.push(e);
            });
            $('#OilItems').val(JSON.stringify(_dataPelumasSend));
            //BBM
            var _dataItemsbbm = tblbbm.data();
            var _databbmSend = []
            $(_dataItemsbbm).each(function (d, e) {
                _databbmSend.push(e);
            });
            $('#fuelitem').val(JSON.stringify(_databbmSend));
            //bbm out
            var _dataItemsbbmout = tblbbmout.data();
            var _databbmoutSend = [];
            $(_dataItemsbbmout).each(function (d, e) {
                _databbmoutSend.push(e);
            });
            $('#fuelitemout').val(JSON.stringify(_databbmoutSend));
            var _data = $('#frmactivity').serializeArray();


            $.ajax({
                type: 'POST',
                data: _data,
                url: '/Equipment/TradoActivities/SaveItemActivities',
                dataType: 'json',
                success: submitFormActivityCallback
            });
        });


        _initTblOperation();
        _initTblNonOperation();
        _initTblOilUsages();
        _initTblBBM();
        _initTblBBMOut();

        setTimePicker('DepartureTime_Operation', 'ArrivalTime_Operation', '00:00', '23:59:59');
        setTimePicker('TimeFill_bbm', 'TimeFill_bbm', '00:00', '23:59:59');
        _initAutoComplete();
        _loadDocument();

        if ($('#Code').val().length > 0) {
            $('#Code,#PoliceNumber').attr("readonly", "readonly");
        };
    });

</script>
