@ModelType MDK_ERP.ModelCrusherJournal
@*@ModelType MDK_ERP.LogOnModel*@
@Code
    ViewData("Title") = "Jurnal Crusher"
    ViewBag.headIcon = "icon-tasks"
    ViewData("BreadCrumb") = Html.BreadCrumb({ _
                                            {"Home", "/"}, _
                                            {"Production", "/Production"}, _
                                            {"Jurnal Crusher", "/Production/CrusherJournals/"}, _
                                            {"Form Jurnal Crusher", Nothing} _
                                         }).ToString()
End Code
@imports MDK_ERP.HtmlHelpers
<h4 class='inline'>
    <i class='icon-pencil'></i> Form Data Jurnal Crusher</h4>
@Using Html.RowBox("", True)
    Using Html.BeginForm("CreateCrusherJournal", "CrusherJournals", Nothing, FormMethod.Post, New With {.class = "form form-horizontal"})
    @Html.ValidationSummary(True, "Penyimpanan data Jurnal Crusher tidak berhasil. Harap perbaiki kesalahan dan coba lagi.")
    @<div class="form-group">
        <label class="col-sm-3 control-label">
            Tanggal</label>
        <div class="col-md-3 col-sm-4">
        @Html.Hidden("ID",0)
            <div class='input-group date' id='dtpWorkDate'>
                @Html.TextBox("WorkDate", Nothing, New With {.class = "form-control"})
                <span class="input-group-addon"><span class="icon-calendar"></span></span>
                @Html.ValidationMessageFor(Function(m) m.WorkDate)
            </div>
        </div>
    </div>
    @<div class="form-group">
        <label class="col-sm-3 control-label">
            Shift</label>
        <div class="col-sm-2">
            @Html.DropDownList("IDWorkSchedule", New SelectList(DirectCast(ViewData("lstWorkSchedule"), IEnumerable), "ID", "Shift"), " ", New With {.class = "form-control select2"})
            @Html.ValidationMessageFor(Function(m) m.IDWorkSchedule)
        </div>
    </div>
    @<div class="form-group">
        <label class="col-sm-3 control-label">
            Operator</label>
        <div class="col-sm-3">
            @Html.TextBox("OperatorName", "", New With {.class = "form-control"})
            @Html.ValidationMessageFor(Function(m) m.OperatorName)
        </div>
    </div>
    @<div class="form-group">
        <label class="col-sm-3 control-label">
            Mesin Crusher</label>
        <div class="col-sm-2">
            @Html.DropDownList("IDMachine", New SelectList(DirectCast(ViewData("lstMachine"), IEnumerable), "ID", "MachineName"), " ", New With {.class = "form-control select2"})
            @Html.ValidationMessageFor(Function(m) m.IDMachine)
        </div>
    </div>
    @<div class="form-group">
        <label class="col-sm-3 control-label">
            Jumlah Grogol</label>
        <div class="col-sm-2">
            <div class="input-group">
                @Html.TextBox("InputGrogol", 0, New With {.class = "form-control text-right"})
                <div class="input-group-addon">Bucket</div>
            </div>
            @Html.ValidationMessageFor(Function(m) m.InputGrogol)
        </div>
    </div>
    @<div class="form-group">
        <label class="col-sm-3 control-label">
            Jumlah Gresley</label>
        <div class="col-sm-2">
            <div class="input-group">
                @Html.TextBox("InputGresley", 0, New With {.class = "form-control text-right"})
                <div class="input-group-addon">Bucket</div>
            </div>
            @Html.ValidationMessageFor(Function(m) m.InputGresley)
        </div>
    </div>
    @<div class="form-group">
        <label class="col-sm-3 control-label">
            Keterangan</label>
        <div class="col-sm-4">
            @Html.TextArea("Description", Nothing, New With {.class = "form-control"})
        </div>
    </div>
    @<div class="row">
        <div class="col-sm-8">
            <h5 class='inline'>
                <i class='icon-list'></i> Hasil Produksi Crusher</h5>
            @Using Html.RowBox("", True)
                @<div class="form-group">
                    <label class="col-sm-3 control-label">
                        Medium</label>
                    <div class="col-xs-4 col-sm-3">
                        <div class="input-group">
                            @Html.TextBox("MediumAmount", 0, New With {.class = "form-control text-right"})
                            <div class="input-group-addon">Bucket</div>
                        </div>
                        @Html.ValidationMessageFor(Function(m) m.MediumAmount)
                    </div>
                </div>
                @<div class="form-group">
                    <label class="col-sm-3 control-label">
                        Abu Batu</label>
                    <div class="col-xs-4 col-sm-3">
                        <div class="input-group">
                            @Html.TextBox("AbuBatuAmount", 0, New With {.class = "form-control text-right"})
                            <div class="input-group-addon">Bucket</div>
                        </div>
                        @Html.ValidationMessageFor(Function(m) m.AbuBatuAmount)
                    </div>
                </div>
                @<div class="form-group">
                    <label class="col-sm-3 control-label">
                        Base A</label>
                    <div class="col-xs-4 col-sm-3">
                        <div class="input-group">
                            @Html.TextBox("BaseAAmount", 0, New With {.class = "form-control text-right"})
                            <div class="input-group-addon">Bucket</div>
                        </div>
                        @Html.ValidationMessageFor(Function(m) m.BaseAAmount)
                    </div>
                </div>
                @<div class="form-group">
                    <label class="col-sm-3 control-label">
                        Base B</label>
                    <div class="col-xs-4 col-sm-3">
                        <div class="input-group">
                            @Html.TextBox("BaseBAmount", 0, New With {.class = "form-control text-right"})
                            <div class="input-group-addon">Bucket</div>
                        </div>
                        @Html.ValidationMessageFor(Function(m) m.BaseBAmount)
                    </div>
                </div>
                @<div class="form-group">
                    <label class="col-sm-3 control-label">
                        Split 1-2</label>
                    <div class="col-xs-4 col-sm-3">
                        <div class="input-group">
                            @Html.TextBox("Split12Amount", 0, New With {.class = "form-control text-right"})
                            <div class="input-group-addon">Bucket</div>
                        </div>
                        @Html.ValidationMessageFor(Function(m) m.Split12Amount)
                    </div>
                </div>
                @<div class="form-group">
                    <label class="col-sm-3 control-label">
                        Split 2-3</label>
                    <div class="col-xs-4 col-sm-3">
                        <div class="input-group">
                            @Html.TextBox("Split23Amount", 0, New With {.class = "form-control text-right"})
                            <div class="input-group-addon">Bucket</div>
                        </div>
                        @Html.ValidationMessageFor(Function(m) m.Split23Amount)
                    </div>
                </div>
                @<div class="form-group">
                    <label class="col-sm-3 control-label">
                        Gresley</label>
                    <div class="col-xs-4 col-sm-3">
                        <div class="input-group">
                            @Html.TextBox("GresleyOutAmount", 0, New With {.class = "form-control text-right"})
                            <div class="input-group-addon">Bucket</div>
                        </div>
                        @Html.ValidationMessageFor(Function(m) m.GresleyOutAmount)
                    </div>
                </div>
        End Using
        </div>
    </div>
    @<div class="form-actions form-actions-padding-sm">
        <div class="row">
            <div class="col-sm-5 col-sm-offset-2">
                <button class="btn btn-primary" type="submit">
                    <i class="icon-save"></i> Simpan</button>
                <button class="btn" type="button" onclick="goBack();">
                    Kembali</button>
            </div>
        </div>
    </div>
    End Using
    @<div style="display: none;" id="divMaterial">
        @Html.DropDownList("ddrMaterial", New SelectList(DirectCast(ViewData("lstMaterial"), IEnumerable), "ID", "Name"), "Pilih Material", New With {.class = "form-control"})
    </div>
    @<div style="display: none;" id="divMeasurementUnit">
        @Html.DropDownList("ddrMeasurementUnit", New SelectList(DirectCast(ViewData("lstMeasurement"), IEnumerable), "ID", "Symbol"), "Pilih Simbol Satuan", New With {.class = "form-control"})
    </div>
    
End Using
@Section StyleSheet
    <link type="text/css" rel="Stylesheet" href="@Url.Content("~/Content/Plugin/Select2/select2.css")" />
    <link type="text/css" rel="Stylesheet" href="@Url.Content("~/Content/Plugin/DatetimePicker/bootstrap-datetimepicker.css")" />
End Section
@Section jsScript
    <script src="@Url.Content("~/Content/Plugin/DatetimePicker/bootstrap-datetimepicker.js")" type="text/javascript"></script>
    <script src="@Url.Content("~/Content/Plugin/Select2/select2.js")" type="text/javascript"></script>
    <script src="@Url.Content("~/Scripts/jquery.validate.min.js")" type="text/javascript"></script>
<script src="@Url.Content("~/Scripts/jquery.validate.unobtrusive.min.js")" type="text/javascript"></script>
    <script type="text/javascript">
    </script>
    <script type="text/javascript">
        $(document).ready(function () {
            datetimePicker_ByDate($("#dtpWorkDate"), true);
        });
    </script>
End Section
