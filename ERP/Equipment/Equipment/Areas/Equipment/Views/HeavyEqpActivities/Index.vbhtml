@Code
    ViewData("Title") = "Daftar Kegiatan Alat Berat"
    Dim Roles = "Equipment.Supervisor, Equipment.Manager, Equipment.DataOperator".Split(",")
    Dim isProperUser = ERPBase.ErpAuthorization.UserInAnyRoles(Roles, User)
    
End Code
<div class="row">
    <div class="col-xs-12">
        <div class="pull-right">
            <a href="@Url.Action("RptEmptyHeavyEqp", "Report")" class="btn btn-sm btn-primary btn-add btn-label-left" >
                <span><i class="fa fa-cloud-download"></i></span>Download Formulir Laporan</a>
                @If isProperUser Then
            @<a href="@Url.Action("Create", "HeavyEqpActivities")" class="btn btn-sm btn-success btn-add btn-label-left" >
                <span><i class="fa fa-plus"></i></span>Tambah Baru</a>        
                End If
            
        </div>
    </div>
</div>
@Using Html.BeginJUIBox("Kegiatan Alat Berat", True, False, False, False, False, "fa fa-ship")
    @Html.Hidden("IsProperUser", CInt(isProperUser))
    @<div class="row">
        <div class="form-horizontal">
            @code
    Dim dateStyle = New With {
            .format = "dd-mm-yyyy",
            .language = "id",
            .todayBtn = "linked",
            .endDate = "today+1",
            .todayHighlight = True,
            .orientation = "top left",
            .autoclose = True
        }
    
    Dim thecontrol = HaloUIHelpers.Helpers.Helpers.Concat(Html.DateInput("OperationDate", Now, dateStyle),
                                                          New MvcHtmlString("<button id='btnRefresh' class='btn btn-primary'>Lihat</button>"))
            End Code
            @Html.WriteFormInput(thecontrol, "Tanggal")
        </div>
    </div>
    @<table id="tb_Data" class="table table-bordered table-striped table-hover table-heading table-datatable dataTable responsive no-footer">
        <thead>
            <tr>
                <th style="width: 60px">
                    #
                </th>
              
                <th style="width: 120px">
                    Kategori
                </th>
                <th style="width: 90px">
                    Kode
                </th>
                <th>
                    Merk/Tipe
                </th>
                <th style="width: 90px">
                    Hour Meter
                </th>
                <th>
                    Operator
                </th>
                <th class="action" style="width: 100px">
                </th>
            </tr>
        </thead>
    </table>
        
End Using
<script src="@Url.Content("~/plugins/datatables/jquery.dataTables.js")" type="text/javascript"></script>
<script src="@Url.Content("~/plugins/datatables/datatablehelper.js")" type="text/javascript"></script>
<script src="@Url.Content("~/plugins/datatables/dataTables.overrides_indo.js")" type="text/javascript"></script>
<script type="text/javascript">
    var tblactivity = null;
    var _renderEditColumn = function (data, type, row) {

        if (type == "display") {
            var isProperUser = $("#IsProperUser").val();
            if (row.ID != 0) {
                if (isProperUser != 0) {
                    return ("<div class='btn-group' role='group'>" +
                    "<button class='detail btn btn-primary btn-xs' title='Detail'><i class='fa fa-list-alt'></i></button>" +
                    "<button class='edit  btn btn-warning btn-xs' rel='tooltip' data-toggle='tooltip' data-placement='top' title='Edit'><i class='fa fa-edit'></i></button>" +
                    "<button class='delete btn btn-danger btn-xs' rel='tooltip' data-toggle='tooltip' data-placement='right' title='Hapus'><i class='fa fa-trash-o'></i></button>" +
                    "</div>")
                } else {

                    return ("<div class='btn-group' role='group'>" +
                    "<button class='detail btn btn-primary btn-xs' title='Detail'><i class='fa fa-list-alt'></i></button>" +

                    "</div>")
                }
            } else {
                if (isProperUser != 0) {
                    return ("<div class='btn-group' role='group'>" +
                 "<button class='create btn btn-primary btn-xs' title='Create'><i class='fa fa-plus'></i></button>" +
                 "</div>")
                } else {
                    return "N/A";
                }

            }

        }
        return data;
    }
    var _renderMerkType = function (data, type, row) {
        if (type == 'display') {
            return row.Merk + "/" + row.Type;
        }
        return data;
    }
    var _renderHW = function (data, type, row) {
        if (type == 'display') {
            if (data) {
                return $.number(data, 2, ',', '.');
            }
            return $.number(0, 2, ",", ".");
        }
        return data
    };

    var arrColumns = [
               { "data": "ID", "sClass": "text-right" },
          
               {"data": "Category", "sClass": "text-center" }, //
              {"data": "Code", "sClass": "text-center" },
              { "data": "Merk", "mRender": _renderMerkType }, //
              {"data": "hw", "sClass": "text-right", "mRender": _renderHW },
              { "data": "Operator" }, //
              {"data": "ID", "sClass": "text-center", "mRender": _renderEditColumn }
        ];

    var _localLoad = function (data, callback, setting) {
        var reportDate = $("#OperationDate").val();
        $.ajax({
            url: '/Equipment/HeavyEqpActivities/getListActivity',
            data: { reportDate: reportDate },
            type: 'POST',
            success: callback,
            error: ajax_error_callback,
            datatype: 'json'
        });
    };
    datatableDefaultOptions.searching = false;
    datatableDefaultOptions.aoColumns = arrColumns;
    datatableDefaultOptions.columnDefs = [{ "targets": [0, 6], "orderable": false}];
    datatableDefaultOptions.order = [[2, "asc"]];
    datatableDefaultOptions.autoWidth = false;
    datatableDefaultOptions.ordering = true;
    datatableDefaultOptions.ajax = _localLoad;

    $(document).ready(function () {
        tblactivity = $('#tb_Data').DataTable(datatableDefaultOptions)
        .on("click", ".delete", function (d) {

            if (confirm("Hapus item ini ?") == false) {
                return;
            }
            var tr = $(this).closest('tr');
            var row = tblactivity.row(tr);
            $.ajax({
                type: 'POST',
                url: '/Equipment/HeavyEqpActivities/Delete',
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
        .on("click", ".detail", function (d) {
            var tr = $(this).closest('tr');
            var row = tblactivity.row(tr);
            var dataItem = row.data();
            window.location.href = '/Equipment/HeavyEqpActivities/Detail/' + dataItem.ID;
        })
        .on("click", ".edit", function () {
            var tr = $(this).closest('tr');
            var row = tblactivity.row(tr);
            var dataItem = row.data();
            window.location.href = '/Equipment/HeavyEqpActivities/Edit/' + dataItem.ID;
        }).on("click", ".create", function () {

            var tr = $(this).closest('tr');
            var row = tblactivity.row(tr);
            var dataItem = row.data();
            var theDate = moment(dataItem.Date);
            window.location.href = '/Equipment/HeavyEqpActivities/Create/?id=' + dataItem.HqID + "&reportdate=" + theDate.format("MM-DD-YYYY");

        });

        $("#btnRefresh").click(function () {
            tblactivity.ajax.reload();
        });
    });
</script>
