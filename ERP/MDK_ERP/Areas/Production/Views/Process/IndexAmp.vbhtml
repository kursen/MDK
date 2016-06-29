@ModelType MDK_ERP.ampModel
@Code
    ViewData("Title") = "Jurnal Hotmix"
    ViewBag.headIcon = "icon-cogs"
    ViewData("BreadCrumb") = Html.BreadCrumb({ _
                                            {"Home", "/"}, _
                                            {"Production", "/Production"}, _
                                            {"Daftar Jurnal Hotmix", "/Production/Process/AMP"},
                                            {"Form Jurnal Hotmix", Nothing} _
                                         }).ToString()
End Code
@imports MDK_ERP.HtmlHelpers
<h4 class='inline'>
    <i class='icon-pencil'></i> Form Jurnal Hotmix</h4>
@Using Html.RowBox("", True)
    Using Html.BeginForm("IndexAmp", "Process", Nothing, FormMethod.Post, New With {.name = "form-amp", .id = "form-amp", .class = "form-horizontal", .autocomplete = False})
    @Html.ValidationSummary(True, "Proses simpan data tidak berhasil. Harap perbaiki kesalahan dan coba lagi.")
    @<div class="form-group">
        <label class="col-sm-3 control-label">
            Tanggal Produksi</label>
        <div class="col-md-3 col-sm-4">
            <div class='input-group date' id='dtpDateUse'>
            @Html.TextBoxFor(Function(m) m.DateUse, New With {.class = "form-control", .Value = Now.Date.ToShortDateString})
                <span class="input-group-addon"><span class="icon-calendar"></span></span>
            </div>
            @Html.ValidationMessageFor(Function(m) m.DateUse)
        </div>  
    </div>
    
    @<div class="form-group">
    <div class="col-sm-8">
        <div class="form-group">
            <label class="col-sm-offset-1 col-md-offset-3 col-sm-3 col-lg-2 control-label">
                Jam Mulai</label>
            <div class="col-md-3 col-sm-3">
                <div class='input-group time' id="BeginTime">
                    @Html.TextBox("BeginProd", Now.ToShortTimeString, New With {.class = "form-control text-right"})
                    <span class="input-group-addon"><span class="icon-time"></span></span>
                </div>
            </div>
        </div>
         <div class="form-group">
            <label class="col-sm-offset-1 col-md-offset-3 col-sm-3 col-lg-2 control-label">
                Jam Selesai</label>
            <div class="col-md-3 col-sm-3">
                <div class='input-group time' id="EndTime">
                    @Html.TextBox("EndProd",  Now.ToShortTimeString, New With {.class = "form-control text-right"})
                    <span class="input-group-addon"><span class="icon-time"></span></span>
                </div>
            </div>
        </div>
    </div>
    </div>
    
    @<div class="form-group">
        <label for="" class="col-sm-3 control-label">
            Operator</label>
        <div class="col-md-3 col-sm-4">
            @Html.TextBox("OperatorName", Nothing, New With {.class = "form-control"})
            @Html.ValidationMessageFor(Function(m) m.OperatorName)
        </div>
    </div>
   @* @<div class="form-group">
        <label for="" class="col-sm-3 control-label">
            Shift</label>
        <div class="col-sm-2">
             @Html.DropDownList("IdShift", New SelectList(DirectCast(ViewData("Shift"), IEnumerable),
                                                          "ID", "Shift"), "-", New With {.class = "form-control select2"})
            @Html.ValidationMessageFor(Function(m) m.IdShift)
        </div>
    </div>*@
    @<div class="form-group">
        <label for="" class="col-sm-3 control-label">
            Mesin AMP</label>
        <div class="col-sm-2">
            @Html.DropDownList("IdMachine", New SelectList(DirectCast(ViewData("ListMachine"), IEnumerable),
                                                           "ID", "MachineName"), "-", New With {.class = "form-control select2"})
            @Html.ValidationMessageFor(Function(m) m.IdMachine)
        </div>
    </div>
    @*@<div class="form-group">
        <label for="" class="col-sm-3 control-label">
            Project</label>
        <div class="col-lg-4 col-sm-4">
            @Html.DropDownList("IDProject", New SelectList(DirectCast(ViewData("projectList"), IEnumerable), "ID", "NoProject"), "Pilih No. Proyek", New With {.class = "form-control select2"})
            @Html.ValidationMessageFor(Function(m) m.IDProject)
        </div>
    </div>*@
    @<div class="form-group">
        <label for="" class="col-sm-3 control-label">
            Produk</label>
        <div class="col-lg-2 col-sm-3">
            @Html.DropDownList("IdProduk", New SelectList(DirectCast(ViewData("ListAsphal"), IEnumerable), "IDMaterial", "Name"), "Pilih Hot Mix", New With {.class = "form-control select2"})
            @Html.Hidden("MeasurementDefault", "Ton")
            @Html.ValidationMessageFor(Function(m) m.IDProduk)
        </div>
    </div>
     @<div class="form-group">
        <label for="" class="col-sm-3 control-label">
            Jumlah</label>
        <div class="col-md-2 col-sm-3">
        <div class="input-group">
            @Html.TextBox("AmountHotmix", 0, New With {.class = "form-control text-right", .disabled = True})
            <div class="input-group-addon"><span>Ton</span></div>
         </div>
        </div>
    </div>
    @<div class="form-group">
        <div class="col-sm-8">
            <div id="detailAMP"></div>
        </div>
    </div>
    @<div class="form-group">
        <label for="" class="col-sm-3 control-label">
            Keterangan</label>
        <div class="col-sm-3">
            @Html.TextArea("Description",New With{.class="form-control"})
        </div>
    </div>
    
    
    @<div class="form-actions form-actions-padding-sm">
    <div class="row">
        <div class="col-md-5 col-md-offset-2">
        <button type="submit" class="btn btn-primary" id="btnSubmit"><i class="icon-save"></i> Simpan</button>
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
<script type="text/javascript" src="@Url.Content("~/Content/js/moment.min.js")"></script>
<script type="text/javascript" src="@Url.Content("~/Content/js/moment-with-locales.js")"></script>
<script type="text/javascript" src="@Url.Content("~/Content/Plugin/DatetimePicker/bootstrap-datetimepicker.js")"></script>
<script type="text/javascript" src="@Url.Content("~/Content/Plugin/Select2/select2.js")"></script>
<script type="text/javascript" src="@Url.Content("~/Scripts/jquery.validate.min.js")"></script>
<script type="text/javascript" src="@Url.Content("~/Scripts/jquery.validate.unobtrusive.min.js")"></script>
@*<script type="text/javascript">
    $(function () {
        $.validator.methods.date = function (value, element) {
            if ($.browser.webkit) {
                var d = new Date();
                return this.optional(element) || !/Invalid|NaN/.test(new Date(d.toLocaleDateString(value)));
            }
            else {
                return this.optional(element) || !/Invalid|NaN/.test(new Date(value));
            }
        };
    });
</script>*@
<script type="text/javascript" src="@Url.Content("~/Scripts/shared-function.js")"></script>
<script type="text/javascript">
    function inputnumber(obj){
        $(obj).keydown(function (e) {
            // Allow: backspace, delete, tab, escape, enter and .
            if ($.inArray(e.keyCode, [46, 8, 9, 27, 13, 110, 190]) !== -1 ||
            // Allow: Ctrl+A, Command+A
                (e.keyCode == 65 && (e.ctrlKey === true || e.metaKey === true)) ||
            // Allow: home, end, left, right, down, up
                (e.keyCode >= 35 && e.keyCode <= 40)) {
                // let it happen, don't do anything
                return;
            }
            // Ensure that it is a number and stop the keypress
            if ((e.shiftKey || (e.keyCode < 48 || e.keyCode > 57)) && (e.keyCode < 96 || e.keyCode > 105)) {
                e.preventDefault();

            }
        });
    }

    var arrtemp = [];

    $(document).ready(function () {
        datePicker_ByDate('#dtpDateUse', true);
        datetimePickerLinked_ByTime("#BeginTime", "#EndTime");

        $('#btnSubmit').attr('disabled', 'disabled');

        inputnumber('#AmountHotmix');

        $('#AmountHotmix').on('keyup', function () {
            var beginval = parseFloat($(this).val());
            var tempval;
            if (!isNaN(this.value) && this.value.length != 0) {
                $('.amountcomposition').each(function (k, v) {
                    var total = beginval * arrtemp[k];
                    $(this).val(total.toString().replace('.',','));
                });
            }
            else {
                var count = 0;
                $('.amountcomposition').each(function () {
                    $(this).val(0);
                    //$(this).val(arrtemp[count]);
                    count += 1;
                });
            }
        });

        $('#IdProduk').change(function () {
            $('#btnSubmit,#AmountHotmix').attr('disabled', 'disabled');
            $('#AmountHotmix').val(0);
            $('.new').remove();
            var data = $(this).val();
            var dataUnit = $(this).siblings("input").val();
            if (data != "") {
                var strHtml = '';
                $.ajax({
                    type: 'POST',
                    dataType: 'json',
                    url: 'getListComposition',
                    data: { idProduk: data, MeasurementUnit: dataUnit }
                })
                .done(function (get) {
                    var i;
                    if (get.data.length > 0) {
                        for (i = 0; i < get.data.length; i++) {
                            strHtml += '<div class="form-group new">' +
                                '<input type="hidden" name="IDMaterial" value="' + get.data[i].IDMaterialComposition + '" />' +
                                '<input type="hidden" name="IDMeasurementUnit" value="' + get.data[i].IdMeasurementUnit + '" />' +
                                '<label class="col-xs-offset-1 col-sm-offset-4 col-sm-3 col-lg-2 control-label">' + get.data[i].MaterialComp + '</label>' +
                                ' <div class="col-xs-offset-1 col-sm-offset-0 col-sm-3 col-lg-3 input-group">' +
                                '    <input type="text" class="form-control text-right amountcomposition" name="AmountComp" value="' + get.data[i].AmountComp + '" readonly/>' +
                                '    <span class="input-group-addon">Ton</span>' +
                                '</div>' +
                                '</div>';
                            arrtemp.push(get.data[i].AmountComp);
                        }
                    }
                    $('#detailAMP').append(strHtml);
                    $('#btnSubmit,#AmountHotmix').removeAttr('disabled');
                    $('#AmountHotmix').keyup();
                })
                .fail(function (xhr) {
                    alert("Error, " + xhr.status + ' : ' + xhr.statusText);
                });
                return false;
            }
        }).change();

        //$('#OperatorName').autocomplete({
        //    source: function (request, response) {
        //        $.ajax({
        //            url: '/HeavyEqpActivities/OperatorType',
        //            data: { Query: $('#Operator').val() },
        //            dataType: 'json',
        //            success: function (data) {
        //                response($.map(data, function (obj) {
        //                    return { label: obj.Value, value: obj.FullName, id: obj.ID }
        //                }));
        //            }
        //        });
        //    },
        //    select: function (event, ui) {
        //        $('#IDOp').val(ui.item.id);
        //    }
        //});
    });
</script>
End Section
