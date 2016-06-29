@ModelType  MDK_ERP.DistributionJournals
@Code
    ViewData("Title") = "Input Data Timbangan"
    ViewBag.headIcon = "icon-filter"
    'ViewBag.showHeader = False
    ViewData("BreadCrumb") = Html.BreadCrumb({ _
                                            {"Home", "/"}, _
                                            {"Production", "/Production"}, _
                                            {"Daftar Data Timbangan Masuk", "/Production/Stock/DataScales"}, _
                                            {"Form Input Data Timbangan - Keluar", Nothing} _
                                         }).ToString()
End Code
@imports MDK_ERP.HtmlHelpers
<h4 class='inline'>
    <i class='icon-pencil'></i> Form Input Data Timbangan - Keluar</h4>
@Using Html.RowBox("", True)
    Using Html.BeginForm("DataScalesOut_", "Stock", Nothing, FormMethod.Post, New With {.class = "form form-horizontal"})
    @Html.ValidationSummary(True, "Data tidak dapat tersimpan, mohon perbaiki kesalahan lalu silahkan coba kembali")
    @Html.Hidden("ID")
    @<div>
        <div class="form-group">
            <label class="col-md-2 control-label">
                Jam Masuk</label>
            <div class="col-md-3">
                <div class='input-group date' id='dtpStartDate'>
                    @Html.TextBox("InTime", ViewData("tglMasuk"), New With {.class = "form-control", .readonly = True})
                    <span class="input-group-addon"><span class="icon-calendar"></span></span>
                </div>
            </div>
        </div>
        <div class="form-group">
            <label class="col-md-2 control-label">
                Jam Keluar</label>
            <div class="col-md-3">
                <div class='input-group' id='dtpEndDate'>
                    @Html.TextBox("OutTime", ViewData("tglKeluar"), New With {.class = "form-control"})
                    <span class="input-group-addon"><span class="icon-calendar"></span></span>
                </div>
                @Html.ValidationMessageFor(Function(m) m.OutTime)
            </div>
        </div>
        <div class="form-group">
            <label class="col-md-2 control-label">
                No Polisi</label>
            <div class="col-md-3">
                @Html.TextBox("PoliceLicense", Nothing, New With {.class = "form-control", .readonly = True})
            </div>
        </div>
        <div class="form-group">
            <label class="col-md-2 control-label">
                Nama Sopir</label>
            <div class="col-md-3">
                @Html.TextBox("DriverName", Nothing, New With {.class = "form-control", .readonly = True})
            </div>
        </div>
        <hr class="hr-normal">
        @Html.Hidden("IdCompany")
        @Html.Hidden("IdMaterial")
        @Html.Hidden("IdDeliveryStatus")
        @Html.Hidden("IdInventory")
        @Html.Hidden("IdMeasurementUnit")
        @* <div class="form-group">
            <label class="col-md-2 control-label">Perusahaan sumber/tujuan</label>
            <div class="col-md-5">
                @Html.DropDownList("KodePeru", Nothing, "Pilih Perusahaan", New With {.class = "form-control select2"})
                @Html.ValidationMessageFor(Function(m) m.KodePeru)
            </div>
        </div>*@ @* <div class="form-group">
            <label class="col-md-2 control-label">Material</label>
            <div class="col-md-5">
                @Html.DropDownGroupListFor("MaterialList", ViewBag.MaterialList, New With {.class = "form-control select2", .name = "KodeBrg"})
                @Html.ValidationMessageFor(Function(m) m.KodeBrg)
            </div>
        </div>*@
        <div class="form-group">
            <label class="col-md-2 control-label">
                Berat 1</label>
            <div class="col-md-2">
                @Html.TextBox("Weight1", Nothing, New With {.class = "form-control text-right", .readonly = True})
            </div>
        </div>
        <div class="form-group">
            <label class="col-md-2 control-label">
                Berat 2</label>
            <div class="col-md-2">
                @Html.TextBox("Weight2", Nothing, New With {.class = "form-control text-right"})
                @Html.ValidationMessageFor(Function(m) m.Weight2)
            </div>
        </div>
        <div class="form-group proj">
            <label class="col-md-2 control-label">
                No. Proyek</label>
            <div class="col-md-4">
                @Html.DropDownList("Place", Nothing, "Pilih Proyek", New With {.class = "form-control"})
                @Html.ValidationMessageFor(Function(m) m.Place)
            </div>
        </div>
        <div class="form-group">
            <label class="col-md-2 control-label">
                Keterangan</label>
            <div class="col-md-4">
                @Html.TextArea("Description", New With {.class = "form-control"})
                @Html.ValidationMessageFor(Function(m) m.Description)
            </div>
        </div>
        <div class="form-group">
            <label class="col-md-2 control-label">
                Clerk 1</label>
            <div class="col-md-3">
                @Html.TextBox("Clerk1", Nothing, New With {.class = "form-control", .readonly = True})
                @Html.ValidationMessageFor(Function(m) m.Clerk1)
            </div>
        </div>
        <div class="form-group">
            <label class="col-md-2 control-label">
                Clerk 2</label>
            <div class="col-md-3">
                @Html.TextBox("Clerk2", User.Identity.Name, New With {.class = "form-control"})
                @Html.ValidationMessageFor(Function(m) m.Clerk2)
            </div>
        </div>
        <div class="form-group">
            <label class="col-md-2 control-label">
                Copy</label>
            <div class="col-md-1">
                @Html.TextBox("Copy", Nothing, New With {.class = "form-control"})
                @Html.ValidationMessageFor(Function(m) m.Copy)
            </div>
        </div>
    </div>
    
    @<div class="form-actions form-actions-padding-sm">
        <div class="row">
            <div class="col-md-5 col-md-offset-2">
                <button class="btn btn-primary" type="submit">
                    <i class="icon-save"></i> Simpan
                </button>
                <button class="btn" type="reset">
                    Reset</button>
                &nbsp;
                <button class="btn" type="button" onclick="goBack();">
                    Kembali</button>
            </div>
        </div>
    </div>
    End Using
End Using
@Section StyleSheet
    <link type="text/css" rel="Stylesheet" href="@Url.Content("~/Content/Plugin/DatetimePicker/bootstrap-datetimepicker.css")" />
End Section
@Section jsScript
    <script type="text/javascript" src="@Url.Content("~/Content/Plugin/DatetimePicker/bootstrap-datetimepicker.js")"></script>
    <script src="@Url.Content("~/Scripts/jquery.validate.min.js")" type="text/javascript"></script>
    <script src="@Url.Content("~/Scripts/jquery.validate.unobtrusive.min.js")" type="text/javascript"></script>
    <script type="text/javascript">
        var weight1, weight2, weight3;
        $(document).ready(function () {

            datetimePickerLinked_ByDate($("#dtpStartDate"), $("#dtpEndDate"));

            $('.proj').hide();

            $('#Weight2').keypress(function (e) {
                if (e.which != 8 && e.which != 0 && (e.which < 48 || e.which > 57)) {
                    //display error message
                    swal("Error", "Hanya Angka", "error");
                    return false;
                } else {
                     weight1 = $('#Weight1').val();
                     weight2 = $(this).val();
                     weight3 = weight2 - weight1;
                    if (weight3 > 0) {
                        $('.proj').show();
                    } else {
                        $('.proj').hide();
                    }
                }
            });
            
            $('#Weight2').keyup(function () {
                 weight1 = $('#Weight1').val();
                 weight2 = $(this).val();
                 weight3 = weight2 - weight1;
                if (weight3 > 0) {
                    $('.proj').show();
                } else {
                    $('.proj').hide();
                }
            });
        });


    </script>
End Section
