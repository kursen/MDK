@ModelType  MDK_ERP.ModelTimbangan

@Code
    ViewData("Title") = "Input Data Timbangan"
    ViewBag.headIcon = "icon-filter"
    'ViewBag.showHeader = False
    ViewData("BreadCrumb") = Html.BreadCrumb({ _
                                            {"Home", "/"}, _
                                            {"Production", "/Production"}, _
                                            {"Daftar Data Timbangan Masuk", "/Production/Stock/DataScales"}, _
                                            {"Form Input Data Timbangan - Masuk", Nothing} _
                                         }).ToString()
End Code
@imports MDK_ERP.HtmlHelpers

<h4 class='inline'>
    <i class='icon-pencil'></i> Form Input Data Timbangan - Masuk</h4>

@Using Html.RowBox("", True)
    Using Html.BeginForm("DataScalesIn_", "Stock", Nothing, FormMethod.Post, New With {.class = "form form-horizontal"})
    @Html.ValidationSummary(True, "Data tidak dapat tersimpan, mohon perbaiki kesalahan lalu silahkan coba kembali")
        @*@Html.HiddenFor(Function(m) m.KodeTimb1)*@
        @<div>
        <div class="form-group">
            <label class="col-md-2 control-label">Jam Masuk</label>
            <div class="col-md-3 col-sm-4">
                <div class='input-group date' id='dtpStartDate'>
                    @Html.TextBox("TglMasuk", Nothing, New With {.class = "form-control"})
                    <span class="input-group-addon"><span class="icon-calendar"></span></span>
                </div>
                @Html.ValidationMessageFor(Function(m) m.TglMasuk)
            </div>
        </div>
        <div class="form-group">
            <label class="col-md-2 control-label">No Polisi</label>
            <div class="col-md-4">
                <div class="col-xs-1" style="width: 80px; padding-left: 0px;">
                    @Html.TextBox("NoPol1", "BA", New With {.class = "form-control", .maxLength = "2", .Style = "text-transform: uppercase;"})
                    @Html.ValidationMessageFor(Function(m) m.NoPol1)
                </div>
                  <div class="col-xs-2" style="padding: 0px;width: 100px;">
                    @Html.TextBox("NoPol2", Nothing, New With {.class = "form-control",.maxlength="4"})
                    @Html.ValidationMessageFor(Function(m) m.NoPol2)
                </div>
                  <div class="col-xs-1" style="width: 90px;">
                    @Html.TextBox("NoPol3", Nothing, New With {.class = "form-control", .maxlength = "3", .Style = "text-transform: uppercase;"})
                  @Html.ValidationMessageFor(Function(m) m.NoPol3)
                </div>
            </div>
        </div>
        <div class="form-group">
            <label class="col-md-2 control-label">Nama Sopir</label>
            <div class="col-md-3">
                @Html.TextBox("Sopir", Nothing, New With {.class = "form-control"})
                @Html.ValidationMessageFor(Function(m) m.Sopir)
            </div>
        </div>
        <hr class="hr-normal" />
        <div class="form-group">
            <label class="col-md-2 control-label">Sumber/Tujuan</label>
            <div class="col-md-3">
                @Html.DropDownList("KodePeru", Nothing, "Pilih Perusahaan/Instansi", New With {.class = "form-control select2"})
                @Html.ValidationMessageFor(Function(m) m.KodePeru)
            </div>
        </div>
        <div class="form-group">
            <label class="col-md-2 control-label">Material</label>
            <div class="col-md-3">
                @Html.DropDownGroupListFor("MaterialList", ViewBag.MaterialList, New With {.class = "form-control select2", .name = "KodeBrg"})
             @*   @Html.ValidationMessageFor(Function(m) m.KodeBrg)*@
            </div>
        </div>
        <div class="form-group">
            <label class="col-md-2 control-label">Berat 1</label>
            <div class="col-md-2">
                @Html.TextBox("Berat1", Nothing, New With {.class = "form-control text-right"})
                @Html.ValidationMessageFor(Function(m) m.Berat1)
            </div>
        </div>
        <div class="form-group">
            <label class="col-md-2 control-label">Keterangan</label>
            <div class="col-md-4">
                @Html.TextAreaFor(Function(m) m.Deliverynote, 3, 4, New With {.class = "form-control"})
                @Html.ValidationMessageFor(Function(m) m.Deliverynote)
            </div>
        </div>
        <hr class="hr-normal">
        <div class="form-group">
            <label class="col-md-2 control-label">Clerk 1</label>
            <div class="col-md-3">
                @Html.TextBox("Clerk1", User.Identity.Name, New With {.class = "form-control"})
                @Html.ValidationMessageFor(Function(m) m.Clerk1)
            </div>
        </div>
        </div>
    
        @<div class="form-actions form-actions-padding-sm">
        <div class="row">
            <div class="col-md-5 col-md-offset-2">
            <button class="btn btn-primary" type="submit">
                <i class="icon-save"></i> Simpan
            </button>
            <button class="btn" type="reset"> Reset</button>
            &nbsp;
            <button class="btn" type="button" onclick="goBack();"> Kembali</button>
            </div>
        </div>
        </div>
End Using
End Using


@Section StyleSheet
    <link type="text/css" rel="Stylesheet" href="@Url.Content("~/Content/Plugin/DatetimePicker/bootstrap-datetimepicker.css")" />
    <link type="text/css" rel="Stylesheet" href="@Url.Content("~/Content/Plugin/Select2/select2.css")" />
End Section

@Section jsScript
    <script type="text/javascript" src="@Url.Content("~/Content/Plugin/DatetimePicker/bootstrap-datetimepicker.js")"></script>
    <script type="text/javascript" src="@Url.Content("~/Content/Plugin/Select2/select2.js")"></script>

    <script src="@Url.Content("~/Scripts/jquery.validate.min.js")" type="text/javascript"></script>
    <script src="@Url.Content("~/Scripts/jquery.validate.unobtrusive.min.js")" type="text/javascript"></script>

    <script type="text/javascript">
        $(document).ready(function () {
            datetimePicker_ByDate($("#dtpStartDate"),true);
        });

    </script>
End Section
