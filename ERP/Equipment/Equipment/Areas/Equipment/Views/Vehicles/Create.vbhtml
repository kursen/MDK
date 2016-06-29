@ModelType Equipment.Vehicle
@Code
    ViewData("Title") = "Kendaraan"
End Code
@Using Html.BeginJUIBox("Data Kendaraan", False, False, False, False, False, "fa fa-plus")
    Using Html.BeginForm("Create", "Vehicles", Nothing, FormMethod.Post, New With {.enctype = "multipart/form-data", .class = "form form-horizontal", .id = "form-data", .autocomplete = "off"})
    @Html.ValidationSummary(True, "Penyimpanan data tidak berhasil. Harap perbaiki kesalahan dan coba lagi.")
      
    @<div class="row">
        @Html.WriteFormInput(Html.TextBoxFor(Function(model) model.Species, New With {.class = "form-control"}), "Jenis")
        @Html.WriteFormInput(Html.TextBoxFor(Function(model) model.Code, New With {.class = "form-control"}), "Kode", smControlWidth:=4, lgControlWidth:=2)
        <div class="form-group">
            <label class="col-lg-3 col-sm-4 control-label">
                Harga Perolehan
            </label>
            <div class="col-lg-3 col-sm-4">
                <div class="input-group">
                    <div class="input-group-addon">
                        Rp</div>
                    @Html.IntegerInputFor(Function(m) m.Cost)
                </div>
            </div>
        </div>
        @Html.WriteFormInput(Html.TextBoxFor(Function(m) m.Merk, New With {.class = "form-control"}), "Merk")
        @Html.WriteFormInput(Html.TextBoxFor(Function(m) m.Type, New With {.class = "form-control"}), "Tipe")
        @Html.WriteFormInput(Html.TextBoxFor(Function(m) m.Capacity, New With {.class = "form-control"}), "Kapasitas")
        @Html.WriteFormYearInputFor(Function(m) m.Year, "Tahun Beli", smControlWidth:=3, smLabelWidth:=4, lgControlWidth:=1, lgLabelWidth:=3)
        @Html.WriteFormYearInputFor(Function(m) m.BuiltYear, "Tahun Pembuatan", smControlWidth:=3, smLabelWidth:=4, lgControlWidth:=1, lgLabelWidth:=3)
        <div class="form-group">
            <label class="col-lg-3 col-sm-4 control-label">
                Nomor Mesin
            </label>
            <div class="col-lg-3 col-sm-4">
                @Html.TextBoxFor(Function(model) model.MachineNumber, New With {.class = "form-control"})
            </div>
        </div>
        <div class="form-group">
            <label class="col-lg-3 col-sm-4 control-label">
                Nomor Rangka
            </label>
            <div class="col-lg-3 col-sm-4">
                @Html.TextBoxFor(Function(model) model.BonesNumber, New With {.class = "form-control"})
            </div>
        </div>
        <div class="form-group">
            <label class="col-lg-3 col-sm-4 control-label">
                Nomor Polisi
            </label>
            <div class="col-lg-3 col-sm-4">
                <div class="input-group">
                    @Html.TextBox("BK1", Nothing, New With {.class = "form-control", .maxlength = "2"})
                    <div class="input-group-addon">
                        -</div>
                    @Html.TextBox("BK2", Nothing, New With {.class = "form-control", .maxlength = "4"})
                    <div class="input-group-addon">
                        -</div>
                    @Html.TextBox("BK3", Nothing, New With {.class = "form-control", .maxlength = "3"})
                </div>
            </div>
            @Html.HiddenFor(Function(m) m.PoliceNumber)
        </div>
        @Html.WriteFormDateInputFor(Function(m) m.DueDate, "Jatuh Tempo",
                                    New With {.format = "dd-mm-yyyy", .autoclose = "true",
                                              .orientation = "top left", .todayBtn = "linked", .language = "id", .daysOfWeekDisabled = {0}},
                                    smControlWidth:=4, lgControlWidth:=2)
        <div class="form-group">
            <label class="col-lg-3 col-sm-4 control-label">
                Nomor Kir
            </label>
            <div class="col-lg-3 col-sm-4">
                @Html.TextBoxFor(Function(model) model.KeurNumber, New With {.class = "form-control"})
            </div>
        </div>
        @Html.WriteFormDateInputFor(Function(m) m.KeurDueDate, "Jatuh Tempo Kir",
                                    New With {.format = "dd-mm-yyyy", .autoclose = "true",
                                              .orientation = "top left", .todayBtn = "linked", .language = "id", .daysOfWeekDisabled = {0}},
                                    smControlWidth:=4, lgControlWidth:=2)
      
        <div class="form-group">
            <label class="col-lg-3 col-sm-4 control-label">
                Keterangan
            </label>
            <div class="col-lg-3 col-sm-4">
                @Html.TextArea("Remarks", New With {.class = "form-control"})
            </div>
        </div>
    </div>
            
   @* @<div class="form-actions form-actions-padding-sm">*@
        @<div class="row">
            <div class="col-lg-offset-5 col-sm-offset-5">
                <button class="btn btn-primary btn-label-left" type="submit" id="btnSave">
                    <span><i class="fa fa-save"></i></span>Simpan</button>
                @Html.ActionLink("Batal", "Index", "Vehicles", Nothing, New With {.class = "btn btn-default"})
            </div>
        </div>
    '</div>
    End Using
End Using
<!-- end form-->
<link href="../../../../plugins/bootstrap-typeahead/typeahead.css" rel="stylesheet"
    type="text/css" />
<script src="../../../../plugins/bootstrap-typeahead/bloodhound.min.js" type="text/javascript"></script>
<script src="../../../../plugins/bootstrap-typeahead/typeahead.jquery.min.js" type="text/javascript"></script>

<script type="text/javascript">
   

    submitFormCallback = function (data) {
        if (data.stat == 0) {
            showNotificationSaveError(data);
            return
        } else {
            window.location = "/Equipment/Vehicles/Detail/" + data.id;
        }
    }

    $(document).ready(function () {


        //
        var VehiclesTypes = new Bloodhound({
            datumTokenizer: Bloodhound.tokenizers.obj.whitespace('Value'),
            queryTokenizer: Bloodhound.tokenizers.whitespace,
            remote: {
                url: "/Vehicles/VehiclesTypes/",
                replace: function (url, uriEncodedQuery) {
                    var q = $("#Species").val();
                    return url + "?query=" + q;

                }
            }

        });
        VehiclesTypes.initialize();
        $("#Species").typeahead(null, {
            name: 'vehicles',
            display: "Value",
            source: VehiclesTypes
        });

        //
        var VehiclesMerkTypes = new Bloodhound({
            datumTokenizer: Bloodhound.tokenizers.obj.whitespace('Value'),
            queryTokenizer: Bloodhound.tokenizers.whitespace,
            remote: {
                url: "/Vehicles/MerkVehiclesTypes/",
                replace: function (url, uriEncodedQuery) {
                    var q = $("#Merk").val();
                    return url + "?query=" + q;

                }
            }

        });
        VehiclesMerkTypes.initialize();
        $("#Merk").typeahead(null, {
            name: 'vehicles',
            display: "Value",
            source: VehiclesMerkTypes
        });
        //

        $('#BK1,#BK3').keypress(function (event) {
            var inputValue = event.which;
            // allow letters and whitespaces only.
            if ((inputValue > 47 && inputValue < 58) && (inputValue != 32)) {
                event.preventDefault();
            }
            var a = $(this).val().toUpperCase();
            $(this).val(a);
            var nopol = $.trim($('#BK1').val()) + " " + $.trim($('#BK2').val()) + " " + $.trim($('#BK3').val());
            $('#PoliceNumber').val(nopol);
        }).keyup(function () {
            var a = $(this).val().toUpperCase();
            $(this).val(a);
        });
        $('#BK2').keydown(function (e) {
            if ($.inArray(e.keyCode, [46, 8, 9, 27, 13, 110, 190]) !== -1 ||
            // Allow: Ctrl+A
            (e.keyCode == 65 && e.ctrlKey === true) ||
            // Allow: Ctrl+C
            (e.keyCode == 67 && e.ctrlKey === true) ||
            // Allow: Ctrl+X
            (e.keyCode == 88 && e.ctrlKey === true) ||
            // Allow: home, end, left, right
            (e.keyCode >= 35 && e.keyCode <= 39)) {
                // let it happen, don't do anything
                return;
            }
            // Ensure that it is a number and stop the keypress
            if ((e.shiftKey || (e.keyCode < 48 || e.keyCode > 57)) && (e.keyCode < 96 || e.keyCode > 105)) {
                e.preventDefault();
            }
        });
        //form submit
        $("#form-data").submit(function (e) {
            e.preventDefault();
            showSavingNotification();
            var nopol = $.trim($('#BK1').val()) + " " + $.trim( $('#BK2').val()) + " " + $.trim($('#BK3').val());
            $('#PoliceNumber').val(nopol);
            var _data = $(this).serialize();
            var _url = $(this).attr('action');
            $.ajax({
                type: 'POST',
                url: _url,
                data: _data,
                success: submitFormCallback
            });
        });
    });
   
</script>
