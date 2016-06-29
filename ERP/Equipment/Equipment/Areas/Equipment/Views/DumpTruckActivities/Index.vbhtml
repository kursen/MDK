@Code
    ViewData("Title") = "Daftar Kegiatan Dump Truck"
    Dim Roles = "Equipment.Supervisor, Equipment.Manager, Equipment.DataOperator".Split(",")
    Dim isProperUser = ERPBase.ErpAuthorization.UserInAnyRoles(Roles, User)
End Code
<div class="row">
    <div class="col-xs-12">
        <div class="pull-right">
            <a href="@Url.Action("RptEmptyDT", "Report")" class="btn btn-sm btn-primary  btn-add btn-label-left" >
                <span><i class="fa fa-cloud-download"></i></span>Download Formulir Laporan</a>
                @If isProperUser Then
            @<a href="@Url.Action("Create", "DumpTruckActivities")" class="btn btn-sm btn-primary  btn-add btn-label-left" >
                <span><i class="fa fa-plus"></i></span>Tambah Baru</a>        
                End If
            
        </div>
    </div>
</div>
@code
    Using Html.BeginJUIBox("Kegiatan Dump Truck", True, False, False, False, False, "fa fa-truck")
End Code
<div class="row">
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
@Html.Hidden("isProperUser", CInt(isProperUser))
<table id="tb_Data" class="table table-bordered table-striped table-hover table-heading table-datatable dataTable responsive no-footer">
    <thead>
        <tr>
            <th style="width: 60px">
                #
            </th>
            <th style="width: 100px">
                Tanggal
            </th>
            <th style="width: 120px">
                Kategori
            </th>
            <th style="width: 100px">
                Kode
            </th>
            <th style="width: 80px">
                TNKB
            </th>
            <th>
                Merk/Tipe
            </th>
            <th>
                Supir
            </th>
            <th class="action" style="width: 100px">
            </th>
        </tr>
    </thead>
</table>
@code
    End Using
End Code
<script src="@Url.Content("~/plugins/datatables/jquery.dataTables.js")" type="text/javascript"></script>
<script src="@Url.Content("~/plugins/datatables/datatablehelper.js")" type="text/javascript"></script>
<script src="@Url.Content("~/plugins/datatables/dataTables.overrides_indo.js")" type="text/javascript"></script>
<script type="text/javascript">
    var tblactivity = null;
    var _renderEditColumn = function (data, type, row) {
        if (type == "display") {
            var isProperUser = $("#isProperUser").val();

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
    var _localLoad = function (data, callback, setting) {
        var reportDate = $("#OperationDate").val();
        $.ajax({
            url: '/DumpTruckActivities/getListActivity',
            data: { reportDate: reportDate },
            type: 'POST',
            success: callback,
            error: ajax_error_callback,
            datatype: 'json'
        });
    }


    var arrColumns = [
                {"data":"ID","sClass": "text-right"},
              { "data": "Date", "sClass": "text-center", "mRender": _fnRenderNetDate }, //
              {"data": "Category", "sClass": "text-center" },
              {"data": "Code", "sClass": "text-center" },
              { "data": "PoliceNumber", "sClass": "text-center" }, //
              {"data": "Merk"}, //
              {"data": "Operator" }, //
              {"data": "ID", "sClass":"text-center", "mRender": _renderEditColumn }
        ];

    datatableDefaultOptions.searching = false;
    datatableDefaultOptions.aoColumns = arrColumns;
    datatableDefaultOptions.columnDefs = [{ "targets": [0, 1, 7], "orderable": false}];
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
                url: '/DumpTruckActivities/Delete',
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
            //            if (row.data().Id == 0) {
            //                
            //                return;
            //            }
        })
        .on("click", ".detail", function (d) {
            var tr = $(this).closest('tr');
            var row = tblactivity.row(tr);
            var dataItem = row.data();
            window.location.href = '/Equipment/DumpTruckActivities/Detail/' + dataItem.ID;
        })
        .on("click", ".edit", function () {
            var tr = $(this).closest('tr');
            var row = tblactivity.row(tr);
            var dataItem = row.data();
            window.location.href = '/Equipment/DumpTruckActivities/Edit/' + dataItem.ID;
        })
        .on('click', '.create', function () {
            var tr = $(this).closest('tr');
            var row = tblactivity.row(tr);
            var dataItem = row.data();
            var reportdate = moment(dataItem.Date);
            window.location.href = '/Equipment/DumpTruckActivities/Create/?id=' + dataItem.DTID + "&reportdate=" + reportdate.format("MM-DD-YYYY");
        });

         $("#btnRefresh").click(function () {
             tblactivity.ajax.reload();
         });
     });
</script>
