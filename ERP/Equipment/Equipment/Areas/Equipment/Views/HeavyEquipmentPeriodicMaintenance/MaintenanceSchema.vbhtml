@modelType  Equipment.HeavyEquipmentMaintenanceCheck
@Code
    ViewData("Title") = "Skema Perawatan Mesin"
    
    Dim dateFormat = New With {.format = "dd-mm-yyyy", .autoclose = "true",
                                              .orientation = "top left", .todayBtn = "linked", .language = "id", .daysOfWeekDisabled = {0}}
End Code
@Functions
    Function WriteSpanControl(ByVal value As String, ByVal id As String) As MvcHtmlString
        Return New MvcHtmlString("<p class='form-control-static'>" & value & "</p>")
    End Function
   
    
    Function WriteSpanControlKM(ByVal value As Decimal, ByVal id As String) As MvcHtmlString
        Return New MvcHtmlString("<p class='form-control-static'>" & value & "</p>")
    End Function
    Function WriteKmControlKM(ByVal value As Decimal, ByVal id As String) As MvcHtmlString
        Dim sb As New StringBuilder
        sb.AppendLine(" <div class='input-group'>")
        sb.AppendFormat("<input type='text' class='form-control text-right' id='{0}' name='{0}' value={1} />", id, value.ToString())
        sb.AppendLine("<div class='input-group-addon'>HM</div>")
        sb.AppendLine("</div>")
    
    
        Dim inputKm = New MvcHtmlString(sb.ToString())
        Return inputKm
    
    End Function
    
    Function CreateItemInput() As MvcHtmlString
        Dim openDiv = New MvcHtmlString("<div class='row'><div class='col-sm-4 col-lg-3'>")
        Dim input2 = Html.DropDownList("UnitValue", Nothing, New With {.class = "form-control"})
     
        Dim sp = New MvcHtmlString("</div><div class='col-sm-4 col-lg-3'>")
        Dim input1 = Html.TextBox("Value", Nothing, New With {.class = "form-control text-right", .Value = 0})
        Dim closeDiv = New MvcHtmlString("</div></div>")
        Return HaloUIHelpers.Helpers.Helpers.Concat(openDiv, input1, sp, input2, closeDiv)
    End Function
End Functions
@Using Html.BeginJUIBox("Skema Perawatan Mesin")
    
    Using Html.BeginForm("SaveHeavyEquipmentMaintenance", "HeavyEquipmentPeriodicMaintenance", Nothing, FormMethod.Post,
                         New With {.class = "form-horizontal", .id = "frmHeavyEquipmentpmaintenance", .autocomplete = "off"})
    
    @<div class="row">
        <div class="col-sm-12 col-lg-12 form-horizontal">
           <input type="hidden" name="ItemDetailCheck" id="ItemCheck" value="" />
            <input type="hidden" name="ID" id="model_ID" value="@Model.ID" />
            @Html.Hidden("HeavyEqpId", Model.HeavyEqp.ID)
            @Html.WriteFormInput(WriteSpanControl(Model.HeavyEqp.Code, "spCode"), "Kode Alat Berat")
            @Html.WriteFormInput(WriteSpanControl(Model.HeavyEqp.Merk & "/" & Model.HeavyEqp.Type, "spMerkType"), "Merk/Type")
            @Html.WriteFormInput(WriteKmControlKM(Model.AverageHourMeter, "AverageHourMeter"), "Pemakaian Harian", lgControlWidth:=3)
        </div>
    </div>
    End Using
    @<div class="row">
        <div class="col-sm-12 col-lg-12">
            <div class="panel panel-default">
                <div class="panel-heading">
                    Daftar Perawatan
                </div>
                <table class="table table-bordered" id="tblSchema">
                    <colgroup>
                        <col style="width: 60px" />
                        <col style="width: auto" />
                        <col style="width: 100px" />
                        <col style="width: 100px" />
                        <col style="width: 100px" />
                        <col style="width: 100px" />
                        <col style="width: 100px" />
                    </colgroup>
                    <thead class="bg-primary">
                        <tr>
                            <th rowspan="2">
                                #
                            </th>
                            <th rowspan="2">
                                Item
                            </th>
                            <th colspan="2" rowspan="2">
                                Setiap
                            </th>
                            <th colspan="2">
                                Pemeriksaan Terakhir
                            </th>
                            <th rowspan="2">
                                <button type="button" class="btn btn-success btn-label-left" id="btnadditem">
                                    <span><i class="fa fa-plus"></i></span>Tambah</button>
                            </th>
                        </tr>
                        <tr>
                            <th>
                                Tanggal
                            </th>
                            <th>
                                Jam Kerja
                            </th>
                        </tr>
                    </thead>
                </table>
            </div>
            <div id="form-item" style="display: none;">
                <!-- form frmitem tidak terdeteksi -->
                @Using Html.BeginForm("ValidateHeavyEquipmentCheckItem", "HeavyEquipmentPeriodicMaintenance", Nothing, FormMethod.Post, New With {.class = "form-horizontal", .id = "frmitem", .autocomplete = "off"})
            Using Html.BeginJUIBox("Kegiatan Perawatan Alat Berat", False, False, False, False, False, "fa fa-wrench")
                    @Html.HiddenFor(Function(m) m.ID, New With {.id = "IDHeavyEquipmentMaintenanceItem"})
                    @Html.Hidden("HeavyEquipmentMaintenanceCheckId", 0)
                    @Html.Hidden("rowIdx", -1, New With {.id = "rowIdx_item"})
                    @<div class="row">
                    </div>
                    @<div class="row">
                        @Html.WriteFormInput(Html.TextBox("ItemName", Nothing, New With {.class = "form-control"}),
                                                  "Item Kegiatan", smLabelWidth:=4, smControlWidth:=4, lgLabelWidth:=3, lgControlWidth:=4)
                        @Html.WriteFormInput(CreateItemInput, "Pemeriksaan Setiap", lgControlWidth:=6)
                        @Html.WriteFormInput(WriteKmControlKM(0.0, "LastCheck_HM"), "Pemakaian Harian", lgControlWidth:=3)
                        @Html.WriteFormInput(Html.DateInput("LastCheck_Date", Nothing, dateFormat), "Tanggal Check HM Terakhir",
                                     smLabelWidth:=4, smControlWidth:=4, lgLabelWidth:=3, lgControlWidth:=2)
                        <div class="col-sm-12 col-lg-12">
                            <div class="well">
                                <div style="width: 200px" class='center-block'>
                                    <button type="button" class="btn btn-primary" id="btn-saveItem">
                                        Simpan</button>
                                    <button type="button" class="btn btn-primary" onclick="$('#btnadditem').click();">
                                        Kembali</button>
                                </div>
                            </div>
                        </div>
                    </div>
            End Using
        End Using
                <!-- end form-->
            </div>
        </div>
    </div>
  
    @<div class="row">
        <div class="col-sm-12 col-lg-12">
            <div class="well">
                <div style="width: 200px" class='center-block'>
                    <button type="button" class="btn btn-primary" id="btn-save">
                        Simpan</button>
                    <a href='@Url.Action("Index", "HeavyEquipmentPeriodicMaintenance")' class="btn btn-primary">
                        Kembali</a>
                </div>
            </div>
        </div>
    </div>
    End Using
<script src="@Url.Content("~/plugins/datatables/jquery.dataTables.js")" type="text/javascript"></script>
<script src="@Url.Content("~/plugins/datatables/datatablehelper.js")" type="text/javascript"></script>
<script type="text/javascript">
    var tblSchema = null;

    submitFormCallback = function (data) {
        if (data.stat == 0) {
            showNotificationSaveError(data);
            return
        } else {
            window.location = "/Equipment/HeavyEquipmentPeriodicMaintenance/Index/";
        }
    }

    initTblSchema = function () {
        var _renderEditColumn = function (data, type, row) {
            if (type == "display") {
                return "<div class='btn-group' role='group'>" +
             "<button type='button' class='edit  btn btn-primary btn-xs' rel='tooltip' data-toggle='tooltip' data-placement='top' title='Edit'><i class='fa fa-edit'></i></button>" +
            "<button type='button' class='delete btn btn-danger btn-xs' rel='tooltip' data-toggle='tooltip' data-placement='right' title='Hapus'><i class='fa fa-trash-o'></i></button>" +
            "</div>"
            }
            return data;
        }

        var _localLoad = function (data, callback, setting) {

            $.ajax({
                url: '/HeavyEquipmentPeriodicMaintenance/GetListMaintenanceSchemaItem',
                data: { Id: $("#model_ID").val() },
                type: 'POST',
                success: callback,
                error: ajax_error_callback,
                datatype: 'json'
            });
        }

        var arrColumns = [
                { "data": "ID", "sClass": "text-right" },
              { "data": "ItemName" }, //
              {"data": "Value", "sClass": "text-right" },
              { "data": "UnitValue" },
              { "data": "LastCheck_Date", "sClass": "text-center", "mRender": _fnRenderNetDate }, //
              {"data": "LastCheck_HM", "sClass": "text-center" }, //
              {"data": "HeavyEquipmentMaintenanceCheckId", "sClass": "text-center", "mRender": _renderEditColumn }
        ];

        datatableDefaultOptions.searching = false;
        datatableDefaultOptions.aoColumns = arrColumns;
        datatableDefaultOptions.columnDefs = [{ "targets": [0, 5], "orderable": false}];
        datatableDefaultOptions.order = [[2, "asc"]];
        datatableDefaultOptions.autoWidth = false;
        datatableDefaultOptions.ordering = true;
        datatableDefaultOptions.ajax = _localLoad;
        tblSchema = $('#tblSchema').DataTable(datatableDefaultOptions)
        .on("click", ".delete", function (d) {

            if (confirm("Hapus item ini ?") == false) {
                return;
            }
            var tr = $(this).closest('tr');
            var row = tblSchema.row(tr);
            $.ajax({
                type: 'POST',
                url: '/Equipment/HeavyEquipmentPeriodicMaintenance/DeleteMaintenanceItem',
                data: { id: row.data().ID },
                dataType: 'json',
                success: function (get) {
                    if (get.stat == 1) {
                        showNotification("Data Telah terhapus!");
                        row.remove().draw();
                    } else {
                        //swal(get.msg, get.msgDesc, "error");
                        showFailedNotification(get.msg)
                    }
                    return false;
                },
                error: ajax_error_callback
            });

        })
        .on("click", ".edit", function () {
            var tr = $(this).closest('tr');
            var row = tblSchema.row(tr);
            var dataItem = row.data();

            $("#IDHeavyEquipmentMaintenanceItem").val(dataItem.ID);
            $("#rowIdx_item").val(row.index());

            $("#MachineEqpMaintenanceCheckId").val(dataItem.HeavyEquipmentMaintenanceCheckId);
            $('#ItemName').val(dataItem.ItemName);
            $('#UnitValue').val(dataItem.UnitValue);
            $('#Value').val(dataItem.Value);
            var checkdate = _fnRenderNetDate(dataItem.LastCheck_Date, 'display', dataItem)
            $('#dtpk_LastCheck_Date').datepicker("setDate", checkdate);
            $('#btn-saveItem').text('Simpan')
            $('#form-item').toggle('blind', null, null, function (e) {
                $('#ItemName').focus();
            });
        });

    };

    $(function () {

        $('#btnadditem').click(function () {
            $('#frmitem').trigger('reset');
            $('#frmitem,.form-group').removeClass('has-error');
            $('#IDHeavyEquipmentMaintenanceItem').val(0)
            $('#HeavyEquipmentMaintenanceCheckId').val(0)
            $('#rowIdx_item').val(-1);
            $('#btn-saveItem').text('Tambah')
            $('#form-item').toggle('blind', null, null, function (e) {
                $('#ItemName').focus();
            });
        });

        $('#btn-saveItem').click(function () {
            var _data = $('#frmitem').serialize();
            var _url = $('#frmitem').attr('action');

            $.ajax({
                type: 'POST',
                data: _data,
                url: _url,
                success: function (data) {
                    if (data.stat == 1) {
                        var NumberId = $('#IDHeavyEquipmentMaintenanceItem').val();
                        var ItemSchema = data.model;
                        if (data.idx == -1) {
                            tblSchema.row.add(ItemSchema);
                        } else {
                            //this is editing;
                            var arrData = tblSchema.data();
                            arrData.splice(data.idx, 1, ItemSchema);
                            tblSchema.clear();
                            tblSchema.rows.add(arrData);

                        }
                        tblSchema.draw();
                        $('#frmitem').trigger('reset');
                        $('#form-item').toggle('blind', null, null, function (e) {
                            $('#btnadditem').focus();
                        });
                    } else {
                        showNotificationSaveError(data, 'Penambahan gagal.');
                    }
                }
            });
        });

        $('#btn-save').click(function (e) {
            showSavingNotification('Menyimpan data');
            var _dataItemsCheck = tblSchema.data();
            var _dataItemsend = []
            $(_dataItemsCheck).each(function (d, e) {
                _dataItemsend.push(e);
            });
            $('#ItemCheck').val(JSON.stringify(_dataItemsend));
            var _data = $('#frmHeavyEquipmentpmaintenance').serializeArray();
            var _urlaction = $('#frmHeavyEquipmentpmaintenance').attr('action');

            $.ajax({
                type: 'POST',
                data: _data,
                url: _urlaction,
                dataType: 'json',
                success: submitFormCallback
            });
        });

        initTblSchema();


        $('#ItemName').autocomplete({
            source: function (request, response) {
                $.ajax({
                    url: '/Equipment/GlobalAjax/autoCompleteItemMaintenance',
                    data: {
                        term: $('#ItemName').val()
                    },
                    dataType: 'json',
                    success: function (data) {
                        response($.map(data, function (obj) {
                            return {
                                label: obj.Item,
                                value: obj.Item
                            }
                        }));
                    }
                });
            }
        });
    })//end init
</script>
