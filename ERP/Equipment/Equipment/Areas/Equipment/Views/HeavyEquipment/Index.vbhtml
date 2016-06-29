@Code
    ViewData("Title") = "Alat Berat"
    Dim roles = "Equipment.Supervisor, Equipment.Manager, Equipment.DataOperator".Split(",")
    Dim isProperUser = ERPBase.ErpAuthorization.UserInAnyRoles(roles, User)
End Code
@If isProperUser Then
@<div class="row">
    <div class="col-xs-12">
        <div class="pull-right">
            <a href="@Url.Action("Create", "HeavyEquipment")" class="btn btn-sm btn-success  btn-add btn-label-left" >
                <span><i class="fa fa-plus"></i></span>Tambah Baru</a>
        </div>
    </div>
</div>    
End If

@Using Html.BeginJUIBox("Daftar Alat Berat")
    @Html.Hidden("isProperUser",IIf(isProperUser,1,0))
    @<div class="row">
        <div class="form-horizontal">
            @Html.WriteFormInput(Html.DropDownList("filterAlatBerat", Nothing, New With {.class = "form-control"}), "Jenis")
        </div>
    </div>
    
@* @Html.Hidden("IDProjectInfo", ViewData("ProjectInfo").id)*@
    @<table id="tb_Data" class="table table-bordered table-striped table-hover table-heading table-datatable dataTable responsive no-footer"
        width="100%">
        <thead>
            <tr><th>#</th>
                <th>
                    Kode
                </th>
                <th>
                    Jenis
                </th>
                <th>
                    Merk/Type
                </th>
                <th>
                    Tahun
                </th>
                <th>
                    Harga
                </th>
                <th class="action">
                </th>
            </tr>
        </thead>
    </table>

End Using
<script src="@Url.Content("~/plugins/datatables/jquery.dataTables.js")" type="text/javascript"></script>
<script src="@Url.Content("~/plugins/datatables/datatablehelper.js")" type="text/javascript"></script>
<script src="@Url.Content("~/plugins/datatables/dataTables.overrides_indo.js")" type="text/javascript"></script>
<script src="@Url.Content("~/js/common.js")" type="text/javascript"></script>
<script type="text/javascript">
    var GenTable;
    $(document).ready(function () {


        var _actionIcons =
            "<div class='btn-group' role='group'>" +
            "<button class='detail btn btn-warning btn-xs' title='Detail'><i class='fa fa-list-alt'></i></button>";
        if ($("#isProperUser").val() == 1) {
            _actionIcons += "<button class='delete btn btn-danger btn-xs' rel='tooltip' data-toggle='tooltip' data-placement='right' title='Hapus'><i class='fa fa-trash-o'></i></button>";
        }

        _actionIcons+= "</div>";

        GenTable = $('#tb_Data').DataTable({
            "ajax": "/HeavyEquipment/getData/",
            "paging": false,
            "order": [[1, "asc"]],
            "info": false,
            "searching": true,
            "dom": "t",
            "fnDrawCallback": _fnlocalDrawCallback,
            "columnDefs": [{ "targets": [0], "orderable": false}],
            "columns": [
            { "data": "ID" },
        {
            "data": "Code"
        }, {

            "data": "Species"


        },
        {
            "data": "Merk", "mRender": function (data, type, row) {
                if (type == 'display') {
                    return data + " / " + row.Type;
                }
                return data;
            }
        },
        {
            "className": "text-right",
            "data": "Year"
        },
         {
             "className": "text-right",
             "data": "Cost", "mRender": _fnRenderInteger
         },
        {
            "className": "action text-center",
            "data": null,
            "bSortable": false,
            "defaultContent": _actionIcons
        }
        ]
        });

        var sBody = $('#tb_Data tbody');
        sBody.on('click', '.detail', function () {
            var data = GenTable.row($(this).parents('tr')).data();
            window.location.href = '/Equipment/HeavyEquipment/Detail/' + data.ID;
        }).on('click', '.delete', function () {
            var data = GenTable.row($(this).parents('tr')).data();
            var urlDelete = "/HeavyEquipment/Delete";
            deleteComfirmModal(function (result) {
                if (result) {
                    $.ajax({
                        type: 'POST',
                        url: urlDelete,
                        data: { id: data.ID },
                        dataType: 'json',
                        success: function (get) {
                            if (get.stat == 1) {
                                showNotification("Data Telah terhapus!");
                                GenTable.ajax.reload();
                            } else {
                                //swal(get.msg, get.msgDesc, "error");
                                showFailedNotification(get.msg)
                            }
                            return false;
                        },
                        error: ajax_error_callback
                    });
                }
            });
        });


        $("#filterAlatBerat").change(function (e) {
            GenTable.search($(this).val(), 1).draw();

        });

    });          //
    
</script>
