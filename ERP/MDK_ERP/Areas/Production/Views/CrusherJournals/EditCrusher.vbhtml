@ModelType MDK_ERP.ModelEditCrusher
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
    Using Html.BeginForm("EditCrusher", "CrusherJournals", Nothing, FormMethod.Post, New With {.class = "form form-horizontal"})
    @Html.ValidationSummary(True, "Penyimpanan data Jurnal Crusher tidak berhasil. Harap perbaiki kesalahan dan coba lagi.")
    
    @<div class="form-group">
        <label class="col-sm-3 control-label">
            Tanggal</label>
        <div class="col-md-3 col-sm-4">
            @Html.Hidden("idCrusher")
            <div class='input-group date' id='dtpDateUse'>
                @Html.TextBox("DateUse", Nothing, New With {.class = "form-control"})
                <span class="input-group-addon"><span class="icon-calendar"></span></span>
                @Html.ValidationMessageFor(Function(m) m.DateUse)
            </div>
        </div>
    </div>
    @<div class="form-group">
        <label class="col-md-3 control-label">
            Shift</label>
        <div class="col-md-2">
            @Html.DropDownList("IdWorkSchedule", New SelectList(DirectCast(ViewData("lstWorkSchedule"), IEnumerable), "ID", "Shift"), "Pilih Shift Kerja", New With {.class = "form-control select2"})
            @Html.ValidationMessageFor(Function(m) m.IdWorkSchedule)
        </div>
    </div>
    @<div class="form-group">
        <label class="col-md-3 control-label">
            Operator</label>
        <div class="col-md-3">
            @Html.TextBox("OperatorName", Nothing, New With {.class = "form-control"})
            @Html.ValidationMessageFor(Function(m) m.OperatorName)
        </div>
    </div>
    @<div class="form-group">
        <label class="col-md-3 control-label">
            Mesin Crusher</label>
        <div class="col-md-2">
            @Html.DropDownList("IdMachine", New SelectList(DirectCast(ViewData("lstMachine"), IEnumerable), "ID", "MachineName"), "", New With {.class = "form-control select2"})
            @Html.ValidationMessageFor(Function(m) m.IdMachine)
        </div>
    </div>
    
    If Model.Amount.Count > 0 Then
        Dim i = 0
        For Each a In Model.Amount
   @<div class="form-group">
        <label class="col-md-3 control-label">
            Input @Model.MaterialName(i) @Html.HiddenFor(Function(m) m.IDMaterial(i))</label>
        <div class="col-md-2">
            @Html.Hidden("idMaterialUserJournal", Model.idMaterialUserJournal(i))
            <div class="input-group">
                @Html.TextBox("Amount", a, New With {.class = "form-control text-right"})
                <div class="input-group-addon">Bucket</div>
            </div>
            @Html.ValidationMessageFor(Function(m) m.Amount(i))
        </div>
    </div>
    i+=1
Next
End If
    
    
    @<div class="form-group">
        <label class="col-md-3 control-label">
            Keterangan</label>
        <div class="col-md-4">
            @Html.TextArea("Description", Nothing, New With {.class = "form-control"})
        </div>
    </div>
    @<div class="row">
        <div class="col-md-8">
            <h5 class='inline'>
                <i class='icon-list'></i> Hasil Produksi Crusher</h5>
            @Using Html.RowBox("", True)
            Dim listsAmount = ViewBag.listAmount
            For Each item In listsAmount
                @<div class="form-group">
                    @Html.Hidden("IDCs", item.IDCs)
                    <label class="col-md-3 control-label">
                        @item.NamaMaterial</label>
                    <div class="col-md-2">
                        <div class="input-group">
                            @Html.TextBox("AMounts", item.Amount, New With {.class = "form-control text-right"})
                            <div class="input-group-addon">Bucket</div>
                        </div>
                    </div>
                </div>
            Next
            
        End Using
        </div>
    </div>
    @<div class="form-actions form-actions-padding-sm">
        <div class="row">
            <div class="col-md-5 col-md-offset-2">
                <button class="btn btn-primary" type="submit">
                    <i class="icon-save"></i> Simpan</button>
                <button class="btn" type="button" onclick="goBack();">
                    Kembali</button>
            </div>
        </div>
    </div>
    End Using
End Using
@Section StyleSheet
    <link type="text/css" rel="Stylesheet" href="@Url.Content("~/Content/Plugin/Select2/select2.css")" />
    <link type="text/css" rel="Stylesheet" href="@Url.Content("~/Content/Plugin/DatetimePicker/bootstrap-datetimepicker.css")" />
End Section
@Section jsScript
    <script src="@Url.Content("~/Scripts/jquery.validate.min.js")" type="text/javascript"></script>
    <script src="@Url.Content("~/Scripts/jquery.validate.unobtrusive.min.js")" type="text/javascript"></script>
    <script src="@Url.Content("~/Content/Plugin/DatetimePicker/bootstrap-datetimepicker.js")" type="text/javascript"></script>
    <script src="@Url.Content("~/Content/Plugin/Select2/select2.js")" type="text/javascript"></script>
    <script type="text/javascript">
        $(document).ready(function () {
            datetimePicker_ByDate($("#dtpDateUse"), false);
        });
    </script>
End Section
