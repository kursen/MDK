@Code
    ViewData("Title") = "Lokasi Alat berat Dan Kendaraan"
      Dim css_table = "table table-bordered table-datatable dataTable"
End Code

 
@Using Html.BeginJUIBox("Lokasi Alat Berat dan Kendaraan")
    @<div class="row">
        <div class="col-lg-12 col-sm-12">
            <ul class="nav nav-tabs" role="tablist">
                <li role="presentation" class="active"><a href="#tab_heavyequipment" aria-controls="home" role="tab"
                    data-toggle="tab">ALAT BERAT</a></li>
                <li role="presentation"><a href="#tab_vehicle" aria-controls="profile" role="tab" data-toggle="tab">
                    KENDARAAN</a></li>
            </ul>
            <div class="tab-content">
                <div role="tabpanel" class="tab-pane active" id="tab_heavyequipment">
                    <table class="@css_table" id="tblHeavyEquipment">
                        <thead>
                            <tr>
                                <th>
                                    #
                                </th>
                                <th>
                                    Jenis
                                </th>
                                <th>
                                    Kode
                                </th>
                                <th>
                                    Merek/Type
                                </th>
                                <th>
                                    Lokasi
                                </th>
                             
                            </tr>
                        </thead>
                    </table>
                </div>
                <div role="tabpanel" class="tab-pane" id="tab_vehicle">
                <table class="@css_table" id="tblVehicle">
                        <thead>
                            <tr>
                                <th>
                                    #
                                </th>
                                <th>
                                    Jenis
                                </th>
                                <th>
                                    Kode
                                </th><th>TNKB</th>
                                <th>
                                    Merek/Type
                                </th>
                                <th>
                                    Lokasi
                                </th>
                               
                            </tr>
                        </thead>
                    </table>
                
                </div>
            </div>
        </div>
    </div>
End Using

<link type="text/css" href="../../../../plugins/bootstrap-editable/bootstrap-editable.css" rel="stylesheet" />

<script src="@Url.Content("~/plugins/datatables/jquery.dataTables.js")" type="text/javascript"></script>
<script src="@Url.Content("~/plugins/datatables/datatablehelper.js")" type="text/javascript"></script>
<script src="../../../../plugins/bootstrap-editable/bootstrap-editable.js" type="text/javascript"></script>
<script src="../../../../plugins/select2/select2.min.js" type="text/javascript"></script>

<script type="text/javascript">
    var tblHeavyEquipment = null;
    var tblVehicle = null;
    var _localLoad = function () {

        $.ajax({
            url: '/Equipment/VehicleCentral/GetHeavyEquipmentList',
            type: 'POST',
            success: function (data) {
                tblHeavyEquipment.clear();
                tblHeavyEquipment.rows.add(data.heavyequipment).draw();
                tblVehicle.clear();
                tblVehicle.rows.add(data.vehicles).draw();

                initEditable(data.officelist);
            },
            error: ajax_error_callback,
            datatype: 'json'
        });
    };


    var initTblHeavyEquipment = function () {
        var _renderHeavyEquipmentEditableColumn = function (data, type, row) {
            if (data == null) {
                data = "[tidak ada data]"
            }
            if (type == "display") {

                return '<a href="#" name="officename" class="officename" data-type="select" data-pk="' + row.ID +
                '" data-url="/Equipment/VehicleCentral/SaveHeavyEquipmentToNewLocation" data-title="Pilih Unit Kerja">' + data + '</a>';
            }
            return data;
        }

        var _renderVehicleEditableColumn = function (data, type, row) {
            if (data == null) {
                data = "[tidak ada data]"
            }
            if (type == "display") {

                return '<a href="#" name="officename" class="officename" data-type="select" data-pk="' + row.ID +
                '" data-url="/Equipment/VehicleCentral/SaveVehicleToNewLocation" data-title="Pilih Unit Kerja">' + data + '</a>';
            }
            return data;
        }
        var _renderOperatorEditableColumn = function (data, type, row) {
            if (data == null) {
                data = "[tidak ada data]"
            }
            if (type == "display") {

                return '<a href="#" name="IDOpr" class="IDOpr" data-type="select2" data-pk="' + row.ID +
                '" data-url="/Equipment/VehicleCentral/SaveDefaultOperator" data-title="Pilih Operator">' + data + '</a>';
            }
            return data;
        }

        var _renderDriverEditableColumn = function (data, type, row) {
            if (data == null) {
                data = "[tidak ada data]"
            }
            if (type == "display") {

                return '<a href="#" name="IDDriver" class="IDDriver" data-type="select2" data-pk="' + row.ID +
                '" data-url="/Equipment/VehicleCentral/SaveDefaultDriver" data-title="Pilih Supir">' + data + '</a>';
            }
            return data;
        }

        var arrColumns = [];
        arrColumns.push({ "data": "ID", "sClass": "text-right" });
        arrColumns.push({ "data": "Category" });
        arrColumns.push({ "data": "Code", "sClass": "text-center" });

        arrColumns.push({ "data": "BrandType" });
        arrColumns.push({ "data": "OfficeName", "mRender": _renderHeavyEquipmentEditableColumn });


        datatableDefaultOptions.searching = false;
        datatableDefaultOptions.aoColumns = arrColumns;
        datatableDefaultOptions.columnDefs = [{ "targets": [0], "orderable": false}];
        datatableDefaultOptions.order = [[2, "asc"]];
        datatableDefaultOptions.autoWidth = false;
        datatableDefaultOptions.ordering = true;

        //datatableDefaultOptions.ajax = _localLoad;
        tblHeavyEquipment = $('#tblHeavyEquipment').DataTable(datatableDefaultOptions)


        var arrColumns2 = [];
        arrColumns2.push({ "data": "ID", "sClass": "text-right" });
        arrColumns2.push({ "data": "Category" });
        arrColumns2.push({ "data": "Code", "sClass": "text-center" });
        arrColumns2.push({ "data": "PoliceNumber", "sClass": "text-center" });
        arrColumns2.push({ "data": "BrandType" });
        arrColumns2.push({ "data": "OfficeName", "mRender": _renderVehicleEditableColumn });
        datatableDefaultOptions.aoColumns = arrColumns2;

        tblVehicle = $("#tblVehicle").DataTable(datatableDefaultOptions);

    };                   //end initTblHeavyEquipment

    var initEditable = function (arrsource) {
        $(".officename").editable({
            source: arrsource,
            success: function () {
                showNotification("Data tersimpan.");

            }
        });
        //

        $('.IDOpr').editable({
            select2: {
                placeholder: 'Pilih Operator',
                allowClear: true,
                id: function (item) {
                    return item.Value;
                },
                ajax: {
                    type: 'POST',
                    url: '/Equipment/VehicleCentral/GetOperators',
                    dataType: 'json',
                    data: function (term, page) {
                        var officeId = 0;

                        if ($(this).parents('.popover').length) {
                            var popv = $(this).parents(".popover");
                            var a = $(popv).siblings(".IDOpr");

                            var tr = $(a).closest('tr');
                            var row = tblHeavyEquipment.row(tr);

                            var dataItem = row.data();
                            officeId = dataItem.OfficeId;
                        }


                        return { query: term, officeId: officeId };
                    },
                    results: function (data, page) {
                        return { results: data };
                    }
                },
                formatResult: function (item) {
                    return item.Text;
                },
                formatSelection: function (item) {
                    return item.Text;
                }
            }
        }); //$('.IDOpr').editable({

        $('.IDDriver').editable({
            select2: {
                placeholder: 'Pilih Supir',
                allowClear: true,
                id: function (item) {
                    return item.Value;
                },
                ajax: {
                    type: 'POST',
                    url: '/Equipment/VehicleCentral/GetDrivers',
                    dataType: 'json',
                    data: function (term, page) {
                        var officeId = 0;

                        if ($(this).parents('.popover').length) {
                            var popv = $(this).parents(".popover");
                            var a = $(popv).siblings(".IDDriver");

                            var tr = $(a).closest('tr');
                            var row = tblVehicle.row(tr);

                            var dataItem = row.data();
                            officeId = dataItem.OfficeId;
                        }


                        return { query: term, officeId: officeId };
                    },
                    results: function (data, page) {
                        return { results: data };
                    }
                },
                formatResult: function (item) {
                    return item.Text;
                },
                formatSelection: function (item) {
                    return item.Text;
                }
            }
        }); //$('.IDDriver').editable({

    };              //end initEditable

    $(function () {
        initTblHeavyEquipment();
        _localLoad()
    });
</script>
