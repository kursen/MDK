﻿@ModelType Equipment.VehicleMaintenanceRecord
@Code
    ViewData("Title") = "Perawatan Kendaraan"
    
    Dim defaultDateFormat = New With {.format = "dd-mm-yyyy", .autoclose = "true",
                                              .orientation = "top left", .todayBtn = "linked", .language = "id", .daysOfWeekDisabled = {0}}
    
    
    
    Dim lsStatus = New List(Of ERPBase.OptionItem)
    lsStatus.Add(New ERPBase.OptionItem With {.Value = 0, .Text = "Menunggu"})
    lsStatus.Add(New ERPBase.OptionItem With {.Value = 1, .Text = "Sedang Dikerjakan"})
    lsStatus.Add(New ERPBase.OptionItem With {.Value = 2, .Text = "Selesai"})
    Dim selectStatus As New SelectList(lsStatus, "Value", "Text", Model.MaintenanceState)
    
End Code
@Functions
    Function WriteSpanControl(ByVal value As String, ByVal id As String) As MvcHtmlString
        Return New MvcHtmlString("<span class='form-control' id='" & id & "' >" & value & "</span>")
    End Function
    Function WriteCostControl() As MvcHtmlString
        Dim divGroupOpen = New MvcHtmlString("<div class='input-group'>")
        Dim rp = New MvcHtmlString("<span class='input-group-addon'>Rp.</span>")
        Dim divGroupClose = New MvcHtmlString("</div>")
        
        Return HaloUIHelpers.Helpers.Helpers.Concat(divGroupOpen, rp,
                                                    Html.TextBox("Cost", 0.0F, New With {.class = "form-control text-right", .id = "ItemOtherCost"}),
                                                    divGroupClose)
        
    End Function
   
End Functions
@Using Html.BeginJUIBox("Form Perawatan Kendaraan")
    
    @<div class="row">
        @Using Html.BeginForm("SaveMaintenanceRecord", "VehicleMaintenance", Nothing, FormMethod.Post, New With {.class = "form-horizontal", .id = "frmMaintenance",
                                                                                                               .autocomplete = "off"})
    
            @<input type='hidden' name='Id' value='@Model.Id' id='recordId' />
            @Html.HiddenFor(Function(m) m.VehicleId)
            @Html.Hidden("sVehicleMaintenanceRecordItem")
            @Html.Hidden("sVehicleMaintenanceRecordMaterialUsed")
            @Html.Hidden("sVehicleMaintenanceRecordOther")
            @Html.HiddenFor(Function(m) m.MaintenanceDateStart)
            @Html.HiddenFor(Function(m) m.MaintenanceDateEnd)
            @Html.HiddenFor(Function(m) m.OdometerValue)
            @<div class="row">
                <div class="col-sm-5 col-lg-6">
                    @*  @Html.WriteFormInput(Html.DateInput("DateStart", Model.MaintenanceDateStart.Date, defaultDateFormat), "Waktu Perawatan", smControlWidth:=6, lgControlWidth:=6, lgLabelWidth:=4)
                    @Html.WriteFormInput(Html.TextBox("TimeStart", Model.MaintenanceDateStart.TimeOfDay, New With {.class = "form-control"}), "", lgLabelWidth:=4)
                    @Html.WriteFormInput(Html.DateInput("DateEnd", Model.MaintenanceDateEnd.Date, defaultDateFormat), "sampai dengan", smControlWidth:=6, lgControlWidth:=6, lgLabelWidth:=4)
                    @Html.WriteFormInput(Html.TextBox("TimeEnd", Model.MaintenanceDateEnd.TimeOfDay, New With {.class = "form-control"}), "", lgLabelWidth:=4)
                    @Html.WriteFormInput(Html.TextBox("DateEnd", Nothing, defaultDateFormat), "sampai dengan", smControlWidth:=6, lgControlWidth:=6, lgLabelWidth:=4)*@
                    <div class="form-group">
                        <label class="col-lg-4 col-sm-4 control-label">
                            Waktu Perawatan</label>
                            <div class="col-lg-7 col-sm-6">
                            <div class="input-group">
                            <div class="input-group-addon"><span class="fa fa-calendar"></span></div>
                                @Html.TextBox("DateStart", Model.MaintenanceDateStart.Date.ToShortDateString, New With {.class = "form-control"})
                                <span class="input-group-addon"> -</span>
                                 @Html.TextBox("DateEnd", Model.MaintenanceDateEnd.Date.ToShortDateString, New With {.class = "form-control"})
                                 <div class="input-group-addon"><span class="fa fa-calendar"></span></div>
                            </div>
                            </div>
                    </div>
                    <div class="form-group">
                        <label class="col-lg-4 col-sm-4 control-label">
                           </label>
                            <div class="col-lg-7 col-sm-6">
                            <div class="input-group">
                            
                            <div class="input-group-addon" ><span class="fa fa-clock-o"></span></div>
                                @Html.TextBox("TimeStart", Model.MaintenanceDateStart.ToShortTimeString, New With {.class = "form-control"})
                                
                                <span class="input-group-addon"> -</span>
                               
                                 @Html.TextBox("TimeEnd", Model.MaintenanceDateEnd.ToShortTimeString, New With {.class = "form-control"})
                                 <div class="input-group-addon" ><span class="fa fa-clock-o"></span></div>
                             
                            </div>
                            </div>
                    </div>

                    @Html.WriteFormInput(Html.TextBox("OdometerValueMask", Model.OdometerValue.ToString("#,###.#0"), New With {.class = "form-control text-right"}), "Odometer", lgLabelWidth:=4)
                </div>
                <div class="col-sm-7 col-lg-6">
                    @If Model.VehicleId = 0 Then
                        @Html.WriteFormInput(Html.TextBox("VehicleCode", Model.Vehicle.Code, New With {.class = "form-control", .Id = "Vehicle_Code"}), "Kode Kendaraan")

                        
                        
        Else
                        @Html.WriteFormInput(WriteSpanControl(Model.Vehicle.Code, "spCode"), "Kode Kendaraan")        
        End If
                    @Html.WriteFormInput(WriteSpanControl(Model.Vehicle.Species, "spCategory"), "Jenis")
                    @Html.WriteFormInput(WriteSpanControl(Model.Vehicle.PoliceNumber, "spPoliceNumber"), "No. Polisi")
                    @Html.WriteFormInput(WriteSpanControl(Model.Vehicle.Merk & "/" & Model.Vehicle.Type, "spMerk"), "Merk/Type")
                </div>
            </div>
                    
            
    End Using
        <div class="col-lg-12 col-sm-12">
            <div class="panel panel-primary">
                <div class="panel-heading">
                    Item Pekerjaan
                    <div class="pull-right">
                        <button class="btn btn-xs btn-danger " type="button" id='btnNewItemToMaintain'>
                            +</button>
                    </div>
                    <div class="clearfix">
                    </div>
                </div>
                <div class="table-responsive">
                    <table class="table table-bordered" id='tblItemToMaintain'>
                        <colgroup>
                            <col style="width: 60px">
                            <col style="width: auto">
                            <col style="width: 100px">
                        </colgroup>
                        <thead class="bg-default">
                            <tr>
                                <th>
                                    #
                                </th>
                                <th>
                                    Item
                                </th>
                                <th>
                                </th>
                            </tr>
                        </thead>
                    </table>
                </div>
            </div>
            <div class="panel panel-primary" id='form_ItemToMaintain' style='display: none'>
                <div class="panel-body">
                    <div>
                        <form class='form-horizontal' action='/Equipment/VehicleMaintenance/ValidateItemToMaintain'
                        onsubmit='return false;' autocomplete='off' id='frmItemToMaintain'>
                        <input type='hidden' name='Id' id='ItemToMaintainId' value='0' />
                        <input type='hidden' name='VehicleMaintenanceRecordId' id='ItemToMaintainVehicleMaintenanceRecordId'
                            value='0' />
                        <input type='hidden' name='VehicleMaintenanceCheckItemId' value='0' />
                        @Html.WriteFormInput(Html.TextBox("Item", String.Empty, New With {.id = "ItemToMaintainName", .class = "form-control"}), "Poin Pekerjaan")
                        <div class='well'>
                            <div class='center-block' style='width: 50%'>
                                <button class='btn btn-primary' type='button' id='BtnAddItemToMaintain'>
                                    Tambahkan</button>
                                <button class='btn btn-primary' type='button' id="btnCloseFormItemToMaintain">
                                    Tutup</button>
                            </div>
                        </div>
                        </form>
                    </div>
                </div>
            </div>
        </div>
        <div class="col-lg-12 col-sm-12">
            <div class="panel panel-primary">
                <div class="panel-heading">
                    Material/Suku Cadang
                    <div class="pull-right">
                        <button class="btn btn-xs btn-danger " type="button" id="btnNewMaterialUsed">
                            +</button>
                    </div>
                    <div class="clearfix">
                    </div>
                </div>
                <table class="table table-bordered " id="tblVehicleMaintenanceRecordMaterialUsed">
                    <colgroup>
                        <col style="width: 60px">
                        <col style="width: auto">
                        <col style="width: 100px">
                        <col style="width: 100px">
                        <col style="width: 100px">
                    </colgroup>
                    <thead class="bg-default">
                        <tr>
                            <th>
                                #
                            </th>
                            <th>
                                Item
                            </th>
                            <th>
                                Jumlah
                            </th>
                            <th>
                                Satuan
                            </th>
                            <th>
                            </th>
                        </tr>
                    </thead>
                </table>
            </div>
            <div class="panel panel-primary" id='form_ItemMaterialUsed' style='display: none'>
                <div class="panel-body">
                    <div>
                        <form class='form-horizontal' action='/Equipment/VehicleMaintenance/ValidateItemMaterialUsed'
                        onsubmit='return false;' autocomplete='off' id='frmItemMaterialUsed'>
                        <input type='hidden' name='Id' id='MaterialUsedId' value='0' />
                        <input type='hidden' name='VehicleMaintenanceRecordId' id='MaterialUsedVehicleMaintenanceRecordId'
                            value='0' />
                        <input type='hidden' name='rowIdx' id="MaterialUsedRowIdx" value='-1' />
                        @Html.WriteFormInput(Html.TextBox("MaterialUsed", String.Empty, New With {.id = "MaterialUsedName", .class = "form-control"}),
                                             "Material/Suku Cadang", lgControlWidth:=5, smControlWidth:=6)
                        <div class="form-group">
                            <label class="col-lg-3 col-sm-4 control-label">
                                Jumlah</label>
                            <div class="col-lg-2 col-sm-3">
                                <input class="form-control text-right" id="MaterialUsedQuantity" name="Quantity"
                                    value="0" type="text"></div>
                            <div class="col-lg-2 col-sm-3">
                                <input class="form-control" id="MaterialUsedUnitQuantity" name="UnitQuantity" value="Pcs"
                                    type="text"></div>
                        </div>
                        <div class='well'>
                            <div class='center-block' style='width: 50%'>
                                <button class='btn btn-primary' type='button' id='BtnAddItemMaterialUsed'>
                                    Tambahkan</button>
                                <button class='btn btn-primary' type='button' id="btnCloseFormMaterialUsed">
                                    Tutup</button>
                            </div>
                        </div>
                        </form>
                    </div>
                </div>
            </div>
        </div>
        <div class="col-lg-12 col-sm-12">
            <div class="panel panel-primary">
                <div class="panel-heading">
                    Lainnya
                    <div class="pull-right">
                        <button class="btn btn-xs btn-danger " type="button" id="btnNewItemOther">
                            +</button>
                    </div>
                    <div class="clearfix">
                    </div>
                </div>
                <table class="table table-bordered " id="tblVehicleMaintenanceRecordOther">
                    <colgroup>
                        <col style="width: 60px">
                        <col style="width: auto">
                        <col style="width: 200px">
                        <col style="width: 100px">
                        <col style="width: 100px">
                    </colgroup>
                    <thead class="bg-default">
                        <tr>
                            <th>
                                #
                            </th>
                            <th>
                                Item
                            </th>
                            <th>
                                Biaya
                            </th>
                            <th>
                                Catatan
                            </th>
                            <th>
                            </th>
                        </tr>
                    </thead>
                </table>
            </div>
            <div class="panel panel-primary" id="form_ItemOther" style="display: none">
                <div class="panel-body">
                    <form class='form-horizontal' action='/Equipment/VehicleMaintenance/ValidateItemOther'
                    onsubmit='return false;' autocomplete='off' id='frmItemOther'>
                    @Html.Hidden("Id", 0, New With {.id = "ItemOtherId"})
                    <input type='hidden' name='rowIdx' id="ItemOtherRowIdx" value='-1' />
                    @Html.Hidden("VehicleMaintenanceRecordId", 0, New With {.id = "ItemOtherVehicleMaintenanceRecordId"})
                    @Html.WriteFormInput(Html.TextBox("Item", Nothing, New With {.id = "ItemOtherItem", .class = "form-control"}), "Item")
                    @Html.WriteFormInput(WriteCostControl, "Biaya")
                    @Html.WriteFormInput(Html.TextArea("Remarks", Nothing, New With {.id = "ItemOtherRemarks", .class = "form-control"}), "Catatan")
                    <div class='well'>
                        <div class='center-block' style='width: 50%'>
                            <button class='btn btn-primary' type='button' id='BtnAddItemOther'>
                                Tambahkan</button>
                            <button class='btn btn-primary' type='button' id='btnCloseFormItemOther'>
                                Tutup</button>
                        </div>
                    </div>
                    </form>
                </div>
            </div>
        </div>
        <div class="row">
            <div class="col-lg-12 col-sm-12">
                @Html.WriteFormInput(Html.DropDownList("Status", selectStatus, New With {.class = "form-control"}), "Status Pekerjaan")
            </div>
        </div>
        <div class="col-lg-12 col-sm-12">
            <div class="well">
                <div class="center-block" style="width: 300px">
                    <button type="button" class="btn btn-primary" id='btnSave' data-loading-text='Menyimpan...'>
                        Simpan</button>
                    @If Model.Id = 0 Then
                        @<span> <a role="button" class="btn btn-primary" href="/Equipment/VehicleMaintenance/Index">
                            Batal</a></span>
    Else
                        @<div class="btn-group">
                            <a role="button" class="btn btn-primary" href="/Equipment/VehicleMaintenance/Index">
                                Batal</a>
                            <button type="button" class="btn btn-danger dropdown-toggle" data-toggle="dropdown"
                                aria-haspopup="true" aria-expanded="false">
                                <span class="caret"></span><span class="sr-only">Action</span>
                            </button>
                            <ul class="dropdown-menu text-left pull-right">
                                <li><a href="@Url.Action("Detail", New With {.id = Model.Id})">Detail</a></li>
                            </ul>
                        </div>
                                
    End If
                </div>
            </div>
        </div>
    </div>
    
End Using
<link href="../../../../plugins/datatables/dataTables.bootstrap.css" rel="stylesheet"
    type="text/css" />
<link href="../../../../plugins/bootstrap-datetimepicker/bootstrap-datetimepicker.min.css" rel="stylesheet"
    type="text/css" />
<style>
    #tblItemToMaintain, #tblVehicleMaintenanceRecordMaterialUsed, #tblVehicleMaintenanceRecordOther
    {
        margin-top: 0px !important;
    }
    .twitter-typeahead
    {
        width: 100%;
    }
</style>
<script src="@Url.Content("~/plugins/datatables/jquery.dataTables.js")" type="text/javascript"></script>
<script src="../../../../plugins/datatables/dataTables.bootstrapPaging.js" type="text/javascript"></script>
<script src="@Url.Content("~/plugins/datatables/datatablehelper.js")" type="text/javascript"></script>
<script type="text/javascript" src="../../../../plugins/bootstrap-datetimepicker/bootstrap-datetimepicker.js"></script>
<script src="@Url.Content("~/plugins/bootstrap-typeahead/typeahead.bundle.min.js")" type="text/javascript"></script>
<link rel="stylesheet" type="text/css" href="@Url.Content("~/plugins/bootstrap-typeahead/typeahead.css")" />
<script type='text/javascript'>
    var tblItemToMaintain = null;
    var tblVehicleMaintenanceRecordMaterialUsed = null;
    var tblVehicleMaintenanceRecordOther = null;
    var _initDatatableDefaultOptions = function () {
        datatableDefaultOptions.searching = false;
        datatableDefaultOptions.columnDefs = [];
        datatableDefaultOptions.order = [[1, "asc"]];
        datatableDefaultOptions.autoWidth = false;
        datatableDefaultOptions.ordering = false;
        datatableDefaultOptions.fnDrawCallback = function (oSettings) {
            for (var i = 0, iLen = oSettings.aiDisplay.length; i < iLen; i++) {
                $('td:eq(0)', oSettings.aoData[oSettings.aiDisplay[i]].nTr).html((i + 1) + '.');
            }


        };
    }
    var _initAutocompletes = function () {
        $('#ItemToMaintainName').autocomplete({
            source: function (request, response) {
                $.ajax({
                    url: '/Equipment/VehicleMaintenance/GetMaintenanceTemplateItem',
                    type: 'POST',
                    data: { term: $("#ItemToMaintainName").val() },
                    dataType: 'json',
                    success: function (data) {
                        response($.map(data, function (obj) {
                            return {
                                label: obj.Item,
                                value: obj.item,
                                VehicleMaintenanceCheckItemId: obj.VehicleMaintenanceCheckItemId,
                                VehicleMaintenanceRecordItemId: obj.VehicleMaintenanceRecordItemId

                            }
                        }));
                    }
                });
            },
            change: function (event, ui) {

            }
        });

        //-----------------/
        if ($("#Vehicle_Code").length) {

            $('#Vehicle_Code').autocomplete({
                source: function (request, response) {
                    $.ajax({
                        url: '/Equipment/GlobalAjax/AutocompleteVehicle',
                        type: 'POST',
                        data: {
                            term: $('#Vehicle_Code').val()
                        },
                        dataType: 'json',
                        success: function (data) {
                            response($.map(data, function (obj) {
                                return {
                                    label: obj.Code,
                                    value: obj.Code,
                                    merk: obj.Merk + '/' + obj.Type,
                                    kind: obj.Species,
                                    policenumber: obj.PoliceNumber,
                                    id: obj.ID
                                }
                            }));
                        }
                    });
                },
                change: function (event, ui) {
                    if (ui.item != null) {
                        $('#spPoliceNumber').text(ui.item.policenumber);
                        $('#spMerk').text(ui.item.merk);

                        $('#spCategory').text(ui.item.kind);
                        $('#Vehicle_Code').text(ui.item.code);
                        $("#VehicleId").val(ui.item.id);

                    } else {
                        $('#spPoliceNumber').text('');
                        $('#spMerk').text('');

                        $('#spCategory').text('');
                        $('#Vehicle_Code').text('');
                        $("#VehicleId").val(0);

                    }
                }
            }).data('ui-autocomplete')._renderItem = function (ul, item) {
                //location
                return ($('<li>').append('<a><strong>' + item.label + '</strong>, <i><strong>' +
            item.policenumber + ',' + item.merk + '</strong> (' + item.kind + '</i>)</a>').appendTo(ul));
            };

        }; //end autocomplete vehicle code;

        //--------------------
        var unitNames = new Bloodhound({
            datumTokenizer: Bloodhound.tokenizers.obj.whitespace("Value"),
            queryTokenizer: Bloodhound.tokenizers.whitespace,
            prefetch: {
                url: "/Equipment/GlobalAjax/AllUnitQuantity",
                cacheKey: "AllUnitQuantity"
            },
            remote: {
                url: "/Equipment/GlobalAjax/AutocompleteUnitQuantity",
                prepare: function (query, settings) {
                    settings.type = "POST";
                    settings.data = { term: query };
                    return settings;
                }
            }


        });
        unitNames.initialize(true);
        $("#MaterialUsedUnitQuantity").typeahead(null, {
            name: "AllUnitQuantity",
            display: 'Value',
            source: unitNames
        }); //end init quantity unit names

        //-------------goods name

        var goodnames = new Bloodhound({
            datumTokenizer: Bloodhound.tokenizers.whitespace,
            queryTokenizer: Bloodhound.tokenizers.whitespace,
            remote: {
                url: "/Equipment/GlobalAjax/AutocompleteGoods",
                prepare: function (query, settings) {
                    settings.type = "POST";
                    settings.data = { term: query };
                    return settings;
                }
            }


        });
        goodnames.initialize();
        $("#MaterialUsedName").typeahead(null, {
            name: "MaterialUsedName",
            source: goodnames
        }); //end init goods names



    }     //end     _initAutocompletes

    var _initItemToMaintain = function () {
        var _deleteItemToMaintain = function () {

            if (confirm("Hapus item ini?") == false) {
                return;
            }
            var row = tblItemToMaintain.row($(this).closest('tr'));

            if (row.data().Id == 0) {
                row.remove().draw();
                return;
            };
            $.ajax({
                url: '/Equipment/VehicleMaintenance/DeleteItemToMaintain',
                data: { id: row.data().Id },
                type: 'POST',
                success: function (data) {
                    if (data.stat == 1) {
                        row.remove().draw();
                    } else {
                        shownotificationerror(data, "Penghapusan gagal");
                    }
                },
                datatype: 'json'
            });

        }
        var _renderDeleteItem = function (data, type, row) {
            if (type == 'display') {
                var html = '';
                html += "<a class='btn btn-danger itemToMaintainRemove'><span class='fa fa-remove'></span></a>";
                return html;
            }
            return data;
        }
        var cols = [
            { data: "Id", "sClass": "text-right" },
            { data: "Item" },
            { data: "VehicleMaintenanceRecordId", "mRender": _renderDeleteItem, "sClass": "text-center" }
        ];

        datatableDefaultOptions.aoColumns = cols;
        datatableDefaultOptions.ajax = function (data, callback, settings) {
            var _vehicleId = parseInt($("#VehicleId").val());
            var _recordId = parseInt($("#recordId").val());
            if (_vehicleId == 0 && _recordId == 0) {
                return;
            }
            if (_recordId > 0) {
                $.ajax({
                    url: '/Equipment/VehicleMaintenance/GetServicedItems',
                    data: { id: _recordId },
                    type: 'POST',
                    success: callback,
                    datatype: 'json'
                });
                return;
            }
            if (_vehicleId > 0) {
                $.ajax({
                    url: '/Equipment/VehicleMaintenance/GetItemToService',
                    data: { id: _vehicleId },
                    type: 'POST',
                    success: callback,
                    datatype: 'json'
                });
            }
        }
        tblItemToMaintain = $('#tblItemToMaintain').DataTable(datatableDefaultOptions)
            .on("click", ".itemToMaintainRemove", _deleteItemToMaintain);

        //buttons
        $("#BtnAddItemToMaintain").click(function () {

            var d = tblItemToMaintain.columns(1).data().eq(0).indexOf($("#ItemToMaintainName").val());
            if (d != -1) {
                var obj = new Object();
                obj.errors = [{ Key: "ItemToMaintainName", Value: ["Item sudah ada di dalam list"]}];
                showNotificationSaveError(obj, 'Penambahan gagal.');
                return;
            }

            var _data = $("#frmItemToMaintain").serialize();
            var _url = $("#frmItemToMaintain").attr("action");
            $.ajax({
                url: _url,
                data: _data,
                type: 'POST',
                success: function (data) {
                    if (data.stat == 1) {
                        tblItemToMaintain.row.add(data.model).draw();
                        clearItem();
                        $(".has-error").removeClass("has-error");

                    } else {
                        showNotificationSaveError(data, 'Penambahan gagal.');
                    }
                }
            });

        }); //BtnAddItemToMaintain_click

        $("#btnNewItemToMaintain").click(function () {
            clearItem();
            $("#form_ItemToMaintain").show("slow");

        }); //btnNewItemToMaintain_click

        $("#btnCloseFormItemToMaintain").click(function () {
            clearItem();
            $("#form_ItemToMaintain").hide("slow");
        });


    };       //end _initItemToMaintain

    var _inittblVehicleMaintenanceRecordMaterialUsed = function () {

        var _deleteItemMaterialUsed = function () {

            if (confirm("Hapus item ini?") == false) {
                return;
            }
            var row = tblVehicleMaintenanceRecordMaterialUsed.row($(this).closest('tr'));

            if (row.data().Id == 0) {
                row.remove().draw();
                return;
            };
            $.ajax({
                url: '/Equipment/VehicleMaintenance/DeleteMaterialUsed',
                data: { id: row.data().Id },
                type: 'POST',
                success: function (data) {
                    if (data.stat == 1) {
                        row.remove().draw();
                    } else {
                        shownotificationerror(data, "Penghapusan gagal");
                    }
                },
                datatype: 'json'
            });
        }; //_deleteItemMaterialUsed

        var _editItemMaterialUsed = function () {
            var row = tblVehicleMaintenanceRecordMaterialUsed.row($(this).closest('tr'));
            $("#MaterialUsedId").val(row.data().Id);
            $("#MaterialUsedRowIdx").val(row.index());
            $('#MaterialUsedName').typeahead('val', row.data().MaterialUsed);
            $("#MaterialUsedQuantity").val(row.data().Quantity);
            $("#MaterialUsedUnitQuantity").val(row.data().UnitQuantity);
            $("#form_ItemMaterialUsed").show();
            $("#MaterialUsedName").focus();

        }

        var _renderEditDeleteItem = function (data, type, row) {
            if (type == 'display') {
                var html = [];
                html.push("<div class='btn-group'>");
                html.push("<button class='btn btn-primary materialusedEdit'><span class='fa fa-edit'></span></button>");
                html.push("<button class='btn btn-danger materialusedDelete'><span class='fa fa-remove '></span></button>");
                html.push("</div>");
                return html.join("");
            }
            return data;
        }; //_renderEditDeleteItem
        var cols = [
            { data: "Id", "sClass": "text-right" },
            { data: "MaterialUsed" },
            { data: "Quantity", "sClass": "text-right" },
            { data: "UnitQuantity" },
            { data: "VehicleMaintenanceRecordId", "mRender": _renderEditDeleteItem, "sClass": "text-center" }
        ];
        datatableDefaultOptions.aoColumns = cols;
        datatableDefaultOptions.ajax = function (data, callback, settings) {
            var _id = $("#recordId").val();
            if (parseInt(_id) > 0) {
                $.ajax({
                    url: '/Equipment/VehicleMaintenance/GetMaterialUsed',
                    data: { id: _id },
                    type: 'POST',
                    success: callback,
                    datatype: 'json'
                });
            }

        };
        tblVehicleMaintenanceRecordMaterialUsed = $('#tblVehicleMaintenanceRecordMaterialUsed').DataTable(datatableDefaultOptions)
            .on("click", ".materialusedDelete", _deleteItemMaterialUsed)
            .on("click", ".materialusedEdit", _editItemMaterialUsed);

        $("#btnNewMaterialUsed").click(function () {
            clearMateriaUsedForm();
            $("#form_ItemMaterialUsed").show("slow");

        }); // btnNewMaterialUsed_click

        $("#btnCloseFormMaterialUsed").click(function () {
            $("#form_ItemMaterialUsed").hide("slow");
            clearMateriaUsedForm();
        });
        $("#BtnAddItemMaterialUsed").click(function () {
            var _data = $("#frmItemMaterialUsed").serializeArray();
            for (var a in _data) {
                if (_data[a].name == "Quantity") {
                    _data[a].value = _data[a].value.replace(".", ",");
                    break;
                }
            }

            var _url = $("#frmItemMaterialUsed").attr("action");
            $.ajax({
                url: _url,
                data: _data,
                type: 'POST',
                success: function (data) {
                    if (data.stat == 1) {
                        if (data.rowIdx == -1) {
                            tblVehicleMaintenanceRecordMaterialUsed.row.add(data.model);
                        }
                        else {
                            var arrData = tblVehicleMaintenanceRecordMaterialUsed.data();
                            arrData.splice(data.rowIdx, 1, data.model);
                            tblVehicleMaintenanceRecordMaterialUsed.clear();
                            tblVehicleMaintenanceRecordMaterialUsed.rows.add(arrData);
                        };


                        tblVehicleMaintenanceRecordMaterialUsed.draw();
                        clearMateriaUsedForm();
                        $(".has-error").removeClass("has-error");


                    } else {
                        showNotificationSaveError(data, 'Penambahan gagal.');
                    }
                }
            });
        }); //BtnAddItemMaterialUsed_click




    }                    //_inittblVehicleMaintenanceRecordMaterialUsed



    var _initTblVehicleMaintenanceRecordOther = function () {

        var _otherEdit = function () {

        }
        var _otherDelete = function () {

        };
        var _renderEditDeleteItem = function (data, type, row) {
            if (type == 'display') {
                var html = [];
                html.push("<div class='btn-group'>");
                html.push("<button class='btn btn-primary otherEdit'><span class='fa fa-edit'></span></button>");
                html.push("<button class='btn btn-danger otherDelete'><span class='fa fa-remove '></span></button>");
                html.push("</div>");
                return html.join("");
            }
            return data;
        }; //_renderEditDeleteItem
        var cols = [
            { data: "Id", "sClass": "text-right" },
            { data: "Item" },
            { data: "Cost", "sClass": "text-right", "mRender": _fnRender2DigitDecimal },
            { data: "Remarks" },
            { data: "VehicleMaintenanceRecordId", "mRender": _renderEditDeleteItem, "sClass": "text-center" }
        ];
        datatableDefaultOptions.aoColumns = cols;
        datatableDefaultOptions.ajax = function (data, callback, settings) {
            var _id = $("#recordId").val();
            if (parseInt(_id) > 0) {
                $.ajax({
                    url: '/Equipment/VehicleMaintenance/GetMaintenanceOther',
                    data: { id: _id },
                    type: 'POST',
                    success: callback,
                    datatype: 'json'
                });
            }

        };
        tblVehicleMaintenanceRecordOther = $('#tblVehicleMaintenanceRecordOther').DataTable(datatableDefaultOptions)
            .on("click", ".otherDelete", _otherDelete)
            .on("click", ".otherEdit", _otherEdit);

        $("#btnNewItemOther").click(function () {
            clearItemOtherForm();
            $("#form_ItemOther").show("slow");
        });
        $("#btnCloseFormItemOther").click(function () {
            $("#form_ItemOther").hide("slow");
            clearItemOtherForm();
        });

        $("#BtnAddItemOther").click(function () {
            var _data = $("#frmItemOther").serialize();
            var _url = $("#frmItemOther").attr("action");
            $.ajax({
                url: _url,
                data: _data,
                type: 'POST',
                success: function (data) {
                    if (data.stat == 1) {
                        if (data.rowIdx == -1) {
                            tblVehicleMaintenanceRecordOther.row.add(data.model);
                        }
                        else {
                            var arrData = tblVehicleMaintenanceRecordOther.data();
                            arrData.splice(data.rowIdx, 1, data.model);
                            tblVehicleMaintenanceRecordOther.clear();
                            tblVehicleMaintenanceRecordOther.rows.add(arrData);
                        };


                        tblVehicleMaintenanceRecordOther.draw();
                        clearItemOtherForm();
                        $(".has-error").removeClass("has-error");


                    } else {
                        showNotificationSaveError(data, 'Penambahan gagal.');
                    }
                }
            });
        }); //BtnAddItemOther_click
    }

    var clearItemOtherForm = function () {

        $("#ItemOtherId").val(0);
        $("#ItemOtherCost").val(0);
        $("#ItemOtherRowIdx").val(-1);
    }
    var clearItem = function () {
        $("#ItemToMaintainName").val("");
    }
    var clearMateriaUsedForm = function () {
        $('#MaterialUsedName').typeahead('val', "");
        $("#MaterialUsedQuantity").val("");
        $("#MaterialUsedUnitQuantity").val("Pcs");
        $("#MaterialUsedId").val("0");
        $("#MaterialUsedRowIdx").val(-1);

    }
    $(function () {//init 
        $('#TimeStart,#TimeEnd').datetimepicker({
            format: 'HH:mm',
            locale:'id'
        });

        $('#DateStart,#DateEnd').datetimepicker({
            locale: 'id',
            format:'DD/MM/YYYY'
        });
        //        $('#date_pkrange').daterangepicker({
        //            timePicker: true,
        //            timePicker24Hour: true,
        //            locale: {
        //                format: 'DD/MM/YYYY HH:mm',
        //                locale: 'id'
        //            }
        //        });

        //        $('#date_pkrange').on('apply.daterangepicker', function (ev, picker) {
        //            $(this).text(picker.startDate.format('DD/MM/YYYY HH:mm') + ' - ' + picker.endDate.format('DD/MM/YYYY HH:mm'));
        //            $("#MaintenanceDateStart").val(picker.startDate.format('DD/MM/YYYY HH:mm'));
        //            $("#MaintenanceDateEnd").val(picker.endDate.format('DD/MM/YYYY HH:mm'));
        //        });
        _initDatatableDefaultOptions();
        _initItemToMaintain();
        _inittblVehicleMaintenanceRecordMaterialUsed();
        _initTblVehicleMaintenanceRecordOther();
        _initAutocompletes();
        $('#ItemOtherCost').number(true, 2, ",", ".");
        $('#OdometerValueMask').number(true, 1, ",", ".");
        $("#MaterialUsedQuantity").number(true, 2, ",", ".");






        $("#btnSave").click(function () {
            $("#btnSave").button("loading");
            var arrVehicleMaintenanceRecordItem = [];
            $(tblItemToMaintain.data()).each(function (d, e) {
                arrVehicleMaintenanceRecordItem.push(e);
            });
            $("#sVehicleMaintenanceRecordItem").val(JSON.stringify(arrVehicleMaintenanceRecordItem));

            var arrVehicleMaintenanceRecordMaterialUsed = [];
            $(tblVehicleMaintenanceRecordMaterialUsed.data()).each(function (d, e) {
                arrVehicleMaintenanceRecordMaterialUsed.push(e);
            });
            $("#sVehicleMaintenanceRecordMaterialUsed").val(JSON.stringify(arrVehicleMaintenanceRecordMaterialUsed));

            var arrVehicleMaintenanceRecordOther = [];
            $(tblVehicleMaintenanceRecordOther.data()).each(function (d, e) {
                arrVehicleMaintenanceRecordOther.push(e);
            });
            $("#sVehicleMaintenanceRecordOther").val(JSON.stringify(arrVehicleMaintenanceRecordOther));


            $("#MaintenanceDateStart").val($("#DateStart").val() + " " + $("#TimeStart").val());
            $("#MaintenanceDateEnd").val($("#DateEnd").val() + " " + $("#TimeEnd").val());
            var value = new String($("#OdometerValueMask").val());
            $("#OdometerValue").val(value.replace(".", ","));
            var _data = $('#frmMaintenance').serializeArray();
            _data.push({ name: "MaintenanceState", value: $("#Status").val() });
            var _url = $("#frmMaintenance").attr("action");
            $.ajax({
                type: 'POST',
                data: _data,
                url: _url,
                dataType: 'json',
                success: function (data) {
                    if (data.stat == 1) {
                        window.location = "/Equipment/VehicleMaintenance/Index"
                    } else {
                        showNotificationSaveError(data, 'Penambahan gagal.');
                        $("#btnSave").button("reset");
                    }
                }
            });

        });
    });              //end init

</script>