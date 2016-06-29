@Code
    ViewData("Title") = "User Rol"
End Code
<h2>
    UserList</h2>
@Html.Hidden("userid", ViewData("userid"))
@Using Html.BeginJUIBox("Daftar Role")
   
    @<div class="row">
        <div class="col-sm-4 col-lg-2">
            Module
        </div>
        <div class="col-sm-4 col-lg-2">
            @Html.DropDownList("ModuleNames", Nothing, Nothing, New With {.class = "form-control"})
        </div>
    </div>
    
    @<table class="table table-bordered table-striped table-hover table-heading table-datatable dataTable"
        id='tblRole'>
        <thead>
            <tr>
                <th style="width: 40px">
                    #
                </th>
                <th>
                    Rolename
                </th>
                <th>
                    Aktif
                </th>
            </tr>
        </thead>
    </table>
End Using
<script src="@Url.Content("~/plugins/datatables/jquery.dataTables.js")" type="text/javascript"></script>
<script src="@Url.Content("~/plugins/datatables/dataTables.bootstrap.js")" type="text/javascript"></script>
<script src="@Url.Content("~/plugins/datatables/datatablehelper.js")" type="text/javascript"></script>
<script type="text/javascript">
    var tblRoles = null;
   

    setRole = function (obj) {
        var userid = $("#userid").val();
        var rolename = $(obj).data('rolename');
        //lock the user;
        showSavingNotification();
        var _data = { "userid": userid, "rolename": rolename, "lockstate": $(obj).is(":checked") };
        var _url = "/Account/SetUserRole";
        $.ajax({
            type: 'POST',
            url: _url,
            data: _data,
            success: setRoleCallback,
            error: ajax_error_callback,
            dataType: 'json'
        });

    }
    setRoleCallback = function (data) {
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
                    rvalue += '<input checked="" type="checkbox" data-rolename="' + row['Rolename'] + '" onclick="setRole(this)"/>';
                } else {
                    rvalue += '<input type="checkbox" data-rolename="' + row['Rolename'] + '" onclick="setRole(this)"/>';
                }
                rvalue += '&nbsp;<i class="fa fa-square-o"></i>' +
                          '</label>' +
                           '</div>';
                return rvalue;
            }
            return data;
        }


        var arrColumns = [
            { "data": "RoleId", "sClass": "text-right" },
            { "data": "Rolename" },
            { "data": "HasRole", "mRender": _drawOnOff, "sClass": "text-center" }

        ];
        var _coldefs = [
                   { "bSortable": false, "targets": [0], "mData": null }


            ];
        var _localLoad = function (data, callback, setting) {

            var _data = { id: $("#userid").val(), modulename: $("#ModuleNames").val() };

            $.ajax({
                url: '/Account/GetUserRole',
                data: _data,
                type: 'POST',
                success: callback,
                datatype: 'json'
            })

        };
        datatableDefaultOptions.searching = true;
        datatableDefaultOptions.aoColumns = arrColumns;
        datatableDefaultOptions.columnDefs = _coldefs;
        datatableDefaultOptions.ajax = _localLoad;
        tblRoles = $("#tblRole").DataTable(datatableDefaultOptions);

        $("#ModuleNames").change(function () {

            tblRoles.ajax.reload();
        });
    });
</script>
