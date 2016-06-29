@Code
    ViewData("Title") = "User List"
End Code
<h2>
    UserList</h2>
@Using Html.BeginJUIBox("Daftar User")
    @<div class="well">
        <a href="/Account/Register" type="button" class="btn btn-primary ajax-link">Menambah
            User</a>
    </div>
    
    @<table class="table table-bordered table-striped table-hover table-heading table-datatable dataTable"
        id='tblUser'>
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
                    Login Terakhir
                </th>
                <th>
                    Aktif
                </th>
                <th></th>
            </tr>
        </thead>
    </table>
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


        var _drawOnOff = function (data, type, row) {
            var rvalue = '<div class="checkbox-inline">' +
                        '<label>';

            if (type == 'display') {
                if (data == true) //isapproved?
                {
                    rvalue += '<input checked="" type="checkbox" data-userid="' + row['Username'] + '" onclick="lockUser(this)"/>';
                } else {
                    rvalue += '<input type="checkbox" data-userid="' + row['Username'] + '" onclick="lockUser(this)"/>';
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
                return "<a href='/account/UserRole/" + data + "'>Role</a>";
            }
            return data;
        }
        var arrColumns = [
            { "data": "UserId", "sClass": "text-right" },
            { "data": "Username" },
            { "data": "Email" },
            { "data": "LastLoginDate", "mRender": _fnRenderNetDateTime, "sClass": "text-center" },
            { "data": "IsApproved", "mRender": _drawOnOff, "sClass": "text-center" },
            { "data": "Username", "mRender": _fnRenderRoleLink, "sClass": "text-center" }

        ];
        var _coldefs = [
                   { "bSortable": false, "targets": [0], "mData": null },
                   { "bSortable": false, "targets": [5] }

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
        tblUsers = $("#tblUser").DataTable(datatableDefaultOptions);


    });
</script>
