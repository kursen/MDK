@ModelType Equipment.HeavyEqp
@Code
    ViewData("Title") = "Alat Berat"
End Code
@Using Html.BeginJUIBox("Data Alat Berat", False, False, False, False, False, "fa fa-plus")
    Using Html.BeginForm("Create", "HeavyEquipment", Nothing, FormMethod.Post, New With {.class = "form form-horizontal", .id = "form-data", .autocomplete = "off"})

      
    @<div class="row">
        @Html.WriteFormInput(Html.TextBoxFor(Function(model) model.Code, New With {.class = "form-control"}), "Kode Alat Berat", lgControlWidth:=2)
        @Html.WriteFormInput(Html.TextBoxFor(Function(model) model.Species, New With {.class = "form-control"}), "Jenis")
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
        @Html.WriteFormInput(Html.TextBoxFor(Function(model) model.Merk, New With {.class = "form-control"}), "Merk")
        @Html.WriteFormInput(Html.TextBoxFor(Function(m) m.Type, New With {.class = "form-control"}), "Tipe")
        @Html.WriteFormInput(Html.TextBoxFor(Function(m) m.Capacity, New With {.class = "form-control"}), "Kapasitas")
        @Html.WriteFormYearInputFor(Function(m) m.Year, "Tahun Beli", smControlWidth:=3, smLabelWidth:=4, lgControlWidth:=1, lgLabelWidth:=3)
        @Html.WriteFormYearInputFor(Function(m) m.BuiltYear, "Tahun Pembuatan", smControlWidth:=3, smLabelWidth:=4, lgControlWidth:=1, lgLabelWidth:=3)
        @Html.WriteFormInput(Html.TextBoxFor(Function(m) m.TaxNumber, New With {.class = "form-control"}), "Nomor Register Pajak")
        @Html.WriteFormDateInputFor(Function(m) m.DueDate, "Jatuh Tempo Pajak",
                                    New With {.format = "dd-mm-yyyy", .autoclose = "true",
                                              .orientation = "top left", .todayBtn = "linked", .language = "id", .daysOfWeekDisabled = {0}},
                                    smControlWidth:=4, lgControlWidth:=2)
        @Html.WriteFormInput(Html.TextBoxFor(Function(m) m.SerialNumber, New With {.class = "form-control"}), "Nomor Seri")

       <div class="form-group">
            <label class="col-lg-3 col-sm-4 control-label">
                Keterangan
            </label>
            <div class="col-lg-3 col-sm-4">
                @Html.TextArea("Remarks", New With {.class = "form-control"})
            </div>
        </div>

    </div>
            
  @*  @<div class="form-actions form-actions-padding-sm">*@
        @<div class="row">
            <div class="col-lg-offset-5 col-sm-offset-5">
                <button class="btn btn-primary btn-label-left" type="submit" id="btnSave">
                    <span><i class="fa fa-save"></i></span>Simpan</button>
                @Html.ActionLink("Batal", "Index", "HeavyEquipment", Nothing, New With {.class = "btn btn-default"})
            </div>
        </div>
   @* </div>*@
    End Using
End Using
<!-- end form-->
<link href="../../../../plugins/bootstrap-typeahead/typeahead.css" rel="stylesheet"type="text/css" />
<script src="../../../../plugins/bootstrap-typeahead/bloodhound.min.js" type="text/javascript"></script>
<script src="../../../../plugins/bootstrap-typeahead/typeahead.jquery.min.js" type="text/javascript"></script>
<script type="text/javascript">
    //function show image
//       

    submitFormCallback = function (data) {
        if (data.stat == 0) {
            showNotificationSaveError(data);
            return
        } else {
            window.location = "/Equipment/HeavyEquipment/Detail/" + data.id;
        }
    }

    $(document).ready(function () {
        //       

        var heavyeqpTypes = new Bloodhound({
            datumTokenizer: Bloodhound.tokenizers.obj.whitespace('Value'),
            queryTokenizer: Bloodhound.tokenizers.whitespace,
            remote: {
                url: "/HeavyEquipment/HeavyEquipmentTypes/",
                replace: function (url, uriEncodedQuery) {
                    var q = $("#Species").val();
                    return url + "?query=" + q;

                }
            }

        });
        heavyeqpTypes.initialize();
        $("#Species").typeahead(null, {
            name: 'heavyequipment',
            display: "Value",
            source: heavyeqpTypes
        });

        ///
        var merkheavyeqpTypes = new Bloodhound({
            datumTokenizer: Bloodhound.tokenizers.obj.whitespace('Value'),
            queryTokenizer: Bloodhound.tokenizers.whitespace,
            remote: {
                url: "/HeavyEquipment/MerkHeavyEquipmentTypes/",
                replace: function (url, uriEncodedQuery) {
                    var q = $("#Merk").val();
                    return url + "?query=" + q;

                }
            }

        });
        merkheavyeqpTypes.initialize();
        $("#Merk").typeahead(null, {
            name: 'heavyequipment',
            display: "Value",
            source: merkheavyeqpTypes
        });


        //form submit
        $("#form-data").submit(function (e) {
            e.preventDefault();
            showSavingNotification();
            //var _data = new FormData($(this)[0]);
            var _data = $(this).serialize();
            var _url = $(this).attr("action");
            $.ajax({
                type: 'POST',
                url: _url,
                data: _data,
                success: submitFormCallback,
                error: ajax_error_callback
            });
        });
    });
   
</script>
