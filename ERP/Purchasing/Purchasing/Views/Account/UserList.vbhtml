@Code
    ViewData("Title") = "User List"
End Code
@Using Html.BeginJUIBox("Daftar User")
    @<div class="well">
        <a href="/Account/Register" type="button" class="btn btn-primary ajax-link">Menambah
            User</a>
    </div>
    
    @<div class="table-responsive">
	<table class="table table-bordered table-striped table-hover table-heading table-datatable dataTable"
        id='tblUser' width="100%">
        <thead>
            <tr>
                <th>
                    #
                </th>
                <th>
                    Username
                </th>
                <th>
                    Email
                </th>
                <th>
                    Aktifitas Terakhir
                </th>
                <th>
                    Aktif
                </th>
                <th>
                </th>
                <th></th>
            </tr>
        </thead>
    </table>
	</div>
End Using
<script src="@Url.Content("~/plugins/datatables/jquery.dataTables.js")" type="text/javascript"></script>
<script src="@Url.Content("~/plugins/datatables/dataTables.bootstrap.js")" type="text/javascript"></script>
<script src="@Url.Content("~/plugins/datatables/datatablehelper.js")" type="text/javascript"></script>
<script type="text/javascript">
    var tblUsers = null;


    lockUser = function (obj) {
        var userid = $(obj).data("userid");
        //lock the user;
        showSavingNotification();
        var _data = { "userid": userid, "lockstate": $(obj).is(":checked") };
        var _url = "/Account/LockAccount";
        $.ajax({
            type: 'POST',
            url: _url,
            data: _data,
            success: lockUserCallback,
            error: ajax_error_callback,
            dataType: 'json'
        });

    }
    lockUserCallback = function (data) {
        if (data.stat == 1) {
            showNotification("Status telah berubah.");
            return;
        }

        showNotificationSaveError(data)
    }


    $(function () {

        var _drawRemove = function (data, type, row) {
            if (type == 'display') {
                return "<div class='btn-group' role='group'>" +
                            "<button type='button' class='btn btn-default btn-xs btnRemoveUser'><i class='fa fa-remove'></i></button>" +
                            "</div>";
            }
            return data;
        }
        var _drawOnOff = function (data, type, row) {
            var rvalue = '<div class="checkbox-inline">' +
                        '<label>';

            if (type == 'display') {
                if (data == true) //isapproved?
                {
                    rvalue += '<input checked="" type="checkbox" data-userid="' + row['UserName'] + '" onclick="lockUser(this)"/>';
                } else {
                    rvalue += '<input type="checkbox" data-userid="' + row['UserName'] + '" onclick="lockUser(this)"/>';
                }
                rvalue += '&nbsp;<i class="fa fa-square-o"></i>' +
                          '</label>' +
                           '</div>';
                return rvalue;
            }
            return data;
        }

        var _fnRenderRoleLink = function (data, type, row) {
            if (type == 'display') {
                return "<a href='/account/UserDetail/" + data + "'>Detail</a>";
            }
            return data;
        }
        var arrColumns = [
            { "data": "UserName", "sClass": "text-right" },
            { "data": "UserName" },
            { "data": "Email" },
            { "data": "LastLoginDate", "mRender": _fnRenderNetDateTime, "sClass": "text-center" },
            { "data": "IsApproved", "mRender": _drawOnOff, "sClass": "text-center" },
            { "data": "ProviderUserKey", "mRender": _fnRenderRoleLink, "sClass": "text-center" },
            { "data": "UserName", "mRender": _drawRemove, "sClass": "text-center" }

        ];
        var _coldefs = [
                   { "bSortable": false, "targets": [0], "mData": null },
                   { "bSortable": false, "targets": [5, 6] }

            ];
        var _localLoad = function (data, callback, setting) {

            $.ajax({
                url: '/Account/GetUserList',
                type: 'POST',
                success: callback,
                datatype: 'json'
            })

        };
        datatableDefaultOptions.searching = true;
        datatableDefaultOptions.aoColumns = arrColumns;
        datatableDefaultOptions.columnDefs = _coldefs;
        datatableDefaultOptions.ajax = _localLoad;
        tblUsers = $("#tblUser").DataTable(datatableDefaultOptions)
        .on("click", ".btnRemoveUser", function (d) {

            if (confirm("Hapus User ini ?") == false) {
                return;
            }
            var tr = $(this).closest('tr');
            var row = tblUsers.row(tr);

            $.ajax({
                type: "POST",
                data: { userid: row.data().UserName },
                url: "/Account/DeleteUser",
                success: function (data) {
                    if (data.stat == 1) {
                        row.remove().draw();
                        return;
                    }
                    showNotificationSaveError(data, "Gagal menghapus user");
                }
            });
        });



    });
</script>
