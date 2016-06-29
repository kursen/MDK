@Code
    ViewData("Title") = "Poin Pemeriksaan"
End Code
@helper DrawTable()

    Dim tblNames As String() = {"Kendaraan", "AlatBerat", "Mesin"}
    Dim tblId As Integer() = ViewData("MachineType")
    Dim activeClass = ""
    Dim i As Integer = 0
    @<ul class="nav nav-tabs" role="tablist">
        @For Each tblName In tblNames
            If tblName = tblNames.First Then activeClass = "active" Else activeClass = ""
        @<li role="presentation" class="@activeClass tab-@tblId(i)"><a href="#tab_@(tblName)" aria-controls="tab_@(tblName)"
            role="tab" data-toggle="tab">@tblName</a></li>
            i += 1
        Next
    </ul>
    
    @<div class="tab-content">
        @For Each tblName In tblNames
            If tblName = tblNames.First Then activeClass = "active" Else activeClass = ""
            @<div role="tabpanel" class="tab-pane @activeClass" id="tab_@(tblName)">
                <table id="tbl@(tblName)" class="table table-bordered table-striped table-hover table-heading table-datatable dataTable responsive no-footer">
                    <colgroup>
                        <col style="width: 60px" />
                        <col style="width: auto" />
                        <col style="width: 100px" />
                        <col style="width: 100px" />
                        <col style="width: 100px" />
                    </colgroup>
                    <thead>
                        <tr>
                            <th>
                                #
                            </th>
                            <th>
                                Nama Item
                            </th>
                            <th colspan="2">
                                Pemeriksaan Setiap
                            </th>
                           
                            <th class="action">
                            </th>
                        </tr>
                    </thead>
                </table>
            </div>
        Next
    </div>
    
End Helper
<div class="row">
    <div class="col-xs-12">
        <div class="pull-right">
            <a href="javascript:void(0)" class="btn btn-sm btn-success btn-add btn-label-left" id="btnAdd" >
                <span><i class="fa fa-plus"></i></span>Tambah Baru</a>
        </div>
    </div>
</div>

<div id="frmCreate" style="display:none">
    @Html.Partial("Create")
</div>


@Using Html.BeginJUIBox("Daftar Poin Pemeriksaan", True, False, False, False, False, "fa fa-ship")
 
        @DrawTable()

End Using
<script src="@Url.Content("~/plugins/datatables/jquery.dataTables.js")" type="text/javascript"></script>
<script src="@Url.Content("~/plugins/datatables/datatablehelper.js")" type="text/javascript"></script>
<script src="@Url.Content("~/plugins/jquery-number/jquery.numeric.js")" type="text/javascript"></script>
<script type="text/javascript">
    var fnReset = function (objform) {
        $('[name="ID"], [name="Id"], [name="id"]').val(0);
        $(objform).trigger("reset");
        $('.form-group,.col-md-5', $(objform)).removeClass("has-error");
    }
    var tblKendaraan = null, tblAlatBerat = null, tblMesin = null;
    var _loadData = function () {
        tblAlatBerat.clear();
        tblKendaraan.clear();
        tblMesin.clear();
        $.ajax({
            url: '/MaintenanceTemplate/getListMaintenance',
            type: 'POST',
            success: function (d) {
                for (var i = 0; i < d.data.length; i++) {

                    switch (d.data[i].MachineTypeID) {
                        case 1:
                            tblKendaraan.rows.add(d.data[i].Group).draw();
                            break;
                        case 2:
                            tblAlatBerat.rows.add(d.data[i].Group).draw();
                            break;
                        case 3:
                            tblMesin.rows.add(d.data[i].Group).draw();
                            break;
                        default:
                    }
                }

            },
            error: ajax_error_callback,
            datatype: 'json'
        });

    }
    var _initTables = function () {
        var _renderEditColumn = function (data, type, row) {
            if (type == "display") {
                if (row.ID != 0) {
                    return ("<div class='btn-group' role='group'>" +
                    //"<button class='detail btn btn-warning btn-xs' rel='tooltip' data-toggle='tooltip' data-placement='top' title='Detail'><i class='fa fa-list-alt'></i></button>" +
            "<button class='edit btn btn-warning btn-xs' rel='tooltip' data-toggle='tooltip' data-placement='top' title='Edit'><i class='fa fa-edit'></i></button>" +
            "<button class='delete btn btn-danger btn-xs' rel='tooltip' data-toggle='tooltip' data-placement='right' title='Hapus'><i class='fa fa-trash-o'></i></button>" +
            "</div>");
                } else {
                    return ("<div class='btn-group' role='group'>" +
                 "<button class='create btn btn-primary btn-xs' title='Create'><i class='fa fa-list-alt'></i></button>" +
                 "</div>");
                }
            }
            return data;
        }

        var arrColumns = [
              { "data": "ID", "sClass": "text-right" },
              { "data": "Item" },
              { "data": "Value", "sClass": "text-right", "mRender": function (data) { return data.toString().replace('.', ','); } },
              { "data": "Unit", "sClass": "text-center" },
              { "data": "ID", "sClass": "text-center", "mRender": _renderEditColumn }
        ];

        datatableDefaultOptions.searching = false;
        datatableDefaultOptions.aoColumns = arrColumns;
        datatableDefaultOptions.columnDefs = [{ "targets": [0, 4], "orderable": false}];
        datatableDefaultOptions.order = [[1, "asc"]];
        datatableDefaultOptions.autoWidth = false;
        datatableDefaultOptions.ordering = true;

        tblKendaraan = $('#tblKendaraan').DataTable(datatableDefaultOptions);
        tblAlatBerat = $('#tblAlatBerat').DataTable(datatableDefaultOptions);
        tblMesin = $('#tblMesin').DataTable(datatableDefaultOptions);

        $("#tblKendaraan, #tblAlatBerat, #tblMesin").on('click', '.delete', function () {
            var tr = $(this).closest('tr');
            var tbl = $(this).closest('table');
            var row;

            switch ($(tbl).attr("id")) {
                case "tblKendaraan":
                    row = tblKendaraan.row(tr);
                    break;
                case "tblAlatBerat":
                    row = tblAlatBerat.row(tr);
                    break;
                case "tblMesin":
                    row = tblMesin.row(tr);
                    break;
            }
            var urlDelete = "/Equipment/MaintenanceTemplate/Delete";
            deleteComfirmModal(function (result) {
                if (result) {
                    $.ajax({
                        type: 'POST',
                        url: urlDelete,
                        data: { id: row.data().ID },
                        dataType: 'json',
                        success: function (get) {
                            if (get.stat == 1) {
                                showNotification("Data Telah terhapus!");
                                _loadData();
                            } else {
                                showFailedNotification(get.msg)
                            }
                            return false;
                        },
                        error: ajax_error_callback
                    });
                }
            });
        })
        .on("click", ".detail", function (d) {
            var tr = $(this).closest('tr');
            var tbl = $(this).closest('table');
            var row;

            switch ($(tbl).attr("id")) {
                case "tblKendaraan":
                    row = tblKendaraan.row(tr);
                    break;
                case "tblAlatBerat":
                    row = tblAlatBerat.row(tr);
                    break;
                case "tblMesin":
                    row = tblMesin.row(tr);
                    break;
            }
            var dataItem = row.data();
            window.location.href = '/Equipment/MaintenanceTemplate/Detail/' + dataItem.ID;
        })
        .on("click", ".edit", function (d) {
            var tr = $(this).closest('tr');
            var tbl = $(this).closest('table');
            var row;

            switch ($(tbl).attr("id")) {
                case "tblKendaraan":
                    row = tblKendaraan.row(tr);
                    break;
                case "tblAlatBerat":
                    row = tblAlatBerat.row(tr);
                    break;
                case "tblMesin":
                    row = tblMesin.row(tr);
                    break;
            }
            var dataItem = row.data();

            var frm = $('#frmCreate');
            if (!frm.is(':visible')) {
                $('#btnAdd').click();
            }

            $.each(dataItem, function (index, value) {
                if (index == "Value")
                    value = value.toString().replace('.', ',');
                $("[name='" + index + "']").val(value);
            });

            //window.location.href = '/Equipment/MaintenanceTemplate/Edit/' + dataItem.ID;
        });
    };

    submitFormCallback = function (data) {
        if (data.stat == 0) {
            showNotificationSaveError(data);
            return;
        } else {
            $('ul.nav.nav-tabs > li').removeClass('active').parent().find('li.tab-' + data.machineTypeID + ' a').click();
            _loadData();
            showNotification("Data Berhasil Disimpan");
            $('#btnAdd').click();
        }
    }

    $(function () {

        _initTables();
        $("#btnRefresh").click(function () {
            _loadData();
        });

        $(".numeric").numeric({ decimal: "," });

        _loadData();

        $('#btnAdd').click(function () {
            fnReset($('#form-data'));
            var frm = $('#frmCreate');
            if (frm.is(':visible')) {
                $(this).html('<span><i class="fa fa-plus"></i></span>Tambah Baru').removeClass("btn-danger");
                frm.slideUp();
            } else {
                $(this).html('Tutup').addClass("btn-danger");
                frm.slideDown();
            }
        });

        $('#btnCancel').click(function () {
            $('#btnAdd').click();
        });

        //form submit
        $("#form-data").submit(function (e) {
            e.preventDefault();
            showSavingNotification();
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
