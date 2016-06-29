@Code
    ViewData("Title") = "Vendor"
End Code
<div class="row">
    <div class="col-xs-12">
        <div class="pull-right">
            <a href="@Url.Action("Create", "Vendor")" class="btn btn-sm btn-success  btn-label-left" >
                <span><i class="fa fa-plus"></i></span>Tambah Baru</a>
        </div>
    </div>
</div>
@Using Html.BeginJUIBox("Daftar Vendor")
    @<div class="table-responsive">
	<table id="tb_Data" class="table table-bordered table-striped table-hover table-heading table-datatable dataTable responsive no-footer" style="width:100%">
        <thead>
            <tr>
                <th></th>
                <th>Name</th>
                <th>Nomor</th>
                <th>Nama Kontak</th>
                <th>No. Telp</th>
                <th class="action"></th>
            </tr>
        </thead>
    </table>
	</div>
End Using
<script src="@Url.Content("~/plugins/datatables/jquery.dataTables.js")" type="text/javascript"></script>
<script src="@Url.Content("~/plugins/datatables/dataTables.bootstrap.js")" type="text/javascript"></script>
<script src="@Url.Content("~/plugins/datatables/dataTables.bootstrapPaging.js")" type="text/javascript"></script>
<script src="@Url.Content("~/plugins/datatables/datatablehelper.js")" type="text/javascript"></script>

<script type="text/javascript">
    var tblData = null;
    var _renderEditColumn = function (data, type, row) {
        if (type == "display") {
            if (row.ID != 0) {
                return ("<div class='btn-group' role='group'>" +
            "<button class='detail btn btn-warning btn-xs' rel='tooltip' data-toggle='tooltip' data-placement='top' title='Detail'><i class='fa fa-list-alt'></i></button>" +
            //"<button class='edit btn btn-primary btn-xs' rel='tooltip' data-toggle='tooltip' data-placement='top' title='Edit'><i class='fa fa-edit'></i></button>" +
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
              { "data": "Id", "sClass": "text-right" },
              { "data": "Name" },
              { "data": "Number", "sClass": "text-center" },
              { "data": "ContactName" },
              { "data": "Phone", "sClass": "text-center" },
              { "data": "Id", "sClass": "text-center", "mRender": _renderEditColumn }
        ];

    var _localLoad = function (data, callback, setting) {
        $.ajax({
            url: '/Vendor/getList',
            type: 'POST',
            success: callback,
            error: ajax_error_callback,
            datatype: 'json'
        });
    };
    datatableDefaultOptions.searching = true;
    datatableDefaultOptions.aoColumns = arrColumns;
    datatableDefaultOptions.columnDefs = [{ "targets": [0, 5], "orderable": false}];
    datatableDefaultOptions.order = [[2, "asc"]];
    datatableDefaultOptions.autoWidth = false;
    datatableDefaultOptions.paging= true;
    datatableDefaultOptions.ordering = true;
    datatableDefaultOptions.sDom = "<'row'<'col-lg-12 col-sm-12'<'pull-right'f>>><'row'<'col-sm-12'tr>><'row'<'col-sm-5'i><'col-sm-7'p>>";
    datatableDefaultOptions.ajax = _localLoad;

    $(document).ready(function () {
        tblData = $('#tb_Data').DataTable(datatableDefaultOptions)
        .on('click', '.delete', function () {
            var tr = $(this).closest('tr');
            var row = tblData.row(tr);
            var urlDelete = "/Purchasing/Vendor/Delete";
            deleteComfirmModal(function (result) {
                if (result) {
                    $.ajax({
                        type: 'POST',
                        url: urlDelete,
                        data: { id: row.data().Id },
                        dataType: 'json',
                        success: function (get) {
                            if (get.stat == 1) {
                                showNotification("Data Telah terhapus!");
                                tblData.ajax.reload();
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
            var row = tblData.row(tr);
            var dataItem = row.data();
            window.location.href = '/Purchasing/Vendor/Detail/' + dataItem.Id;
        })
        .on("click", ".edit", function (d) {
            var tr = $(this).closest('tr');
            var row = tblData.row(tr);
            var dataItem = row.data();
            window.location.href = '/Purchasing/Vendor/Edit/' + dataItem.Id;
        });

        $("#btnRefresh").click(function () {
            tblData.ajax.reload();
        });
    });
</script>
