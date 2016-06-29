@Code
    ViewData("Title") = "Kendaraan"
    Dim ListRoles = "Equipment.Supervisor, Equipment.Manager, Equipment.DataOperator".Split(",")
    Dim isProperUser As Boolean = ERPBase.ErpAuthorization.UserInAnyRoles(ListRoles, User)
 
    
End Code
@If isProperUser Then
    @<div class="row">
        <div class="col-xs-12">
            <div class="pull-right">
                <a href="@Url.Action("Create", "Vehicles")" class="btn btn-sm btn-success  btn-label-left" >
                    <span><i class="fa fa-plus"></i></span>Tambah Baru</a>
            </div>
        </div>
    </div>    
End If

@Using Html.BeginJUIBox("Daftar Kendaraan")
    @Html.Hidden("isProperUser", Iif(isProperUser,1,0))
    @<div class="row">
        <div class="form-horizontal">
            @Html.WriteFormInput(Html.DropDownList("filterKendaraan", Nothing, New With {.class = "form-control"}), "Jenis")
        </div>
    </div>
    @<table id="tb_Data" class="table table-bordered table-striped table-hover table-heading table-datatable dataTable responsive no-footer"
        style="width: 100%">
        @* <colgroup>
        <col style="width:60px" />
        <col style="width:200px" />
        <col style="width:110px"/>
        <col style="width:auto">
        <col style="width:80px"/>
        <col style="width:150px"/>
        <col style="width:110px"/>
    </colgroup>*@
        <thead>
            <tr>
                <th>
                    #
                </th>
                <th>
                    Kode
                </th>
                <th>
                    No. Polisi
                </th>
                <th>
                    Jenis
                </th>
                <th>
                    Merk / Type
                </th>
                <th>
                    Tahun
                </th>
                <th>
                    Masa Berlaku TNKB
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

        //capitaList('#Equipments');
        //justCapital('#Unit');

        var iconActions = "<div class='btn-group' role='group'>" +
                                    "<button class='detail btn btn-warning btn-xs' title='Detail'><i class='fa fa-list-alt'></i></button>";
        if ( $("#isProperUser").val()==1) {
            iconActions += "<button class='delete btn btn-danger btn-xs' rel='tooltip' data-toggle='tooltip' data-placement='right' title='Hapus'><i class='fa fa-trash-o'></i></button>";
        }

        iconActions += "</div>";
        

        GenTable = $('#tb_Data').DataTable({
            "ajax": "/Equipment/Vehicles/getData/",
            "paging": false,
            "order": [[1, "asc"]],
            "info": false,
            "searching": true,
            "dom": "t",
            "fnDrawCallback": _fnlocalDrawCallback,
            "columnDefs": [{ "targets": [0], "orderable": false}],
            "columns": [
                { "data": "ID", "orderable": false, "sClass": "text-right" },
                { "data": "Code" },
                { "data": "PoliceNumber", "sClass": "text-center" },
                { "data": "Species", "sClass": "text-center" },
                {
                    "data": "Merk", "mRender": function (data, type, row) {
                        if (type == 'display') {
                            return data + ' / ' + row.Type;
                        }
                        return data;
                    }
                },
                { "data": "Year", "sClass": "text-center" },
                { "data": "DueDate", "sClass": "text-right", "mRender": _fnRenderNetDate },
                {
                    "className": "action text-center",
                    "data": null,
                    "bSortable": false,
                    "defaultContent": iconActions
                }
            ]
        });

        var sBody = $('#tb_Data tbody');
        sBody.on('click', '.detail', function () {
            var data = GenTable.row($(this).parents('tr')).data();
            window.location.href = '/Equipment/Vehicles/Detail/' + data.ID;
        }).on('click', '.delete', function () {
            var data = GenTable.row($(this).parents('tr')).data();
            var urlDelete = "/Equipment/Vehicles/Delete";
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
                                showFailedNotification(get.msg)
                            }
                            return false;
                        },
                        error: ajax_error_callback
                    });
                }
            });
        });

        $("#filterKendaraan").change(function (e) {
            GenTable.search($(this).val(), 2).draw();
        });
    });
    
</script>
