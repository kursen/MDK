@Code
    ViewData("Title") = "User Detail"
End Code
<div class="row">
    <div class="col-lg-12 col-sm-12">
        <div class="btn-group pull-right">
            <button class="btn btn-danger" onclick="window.location='/account/Userlist'">
                Kembali</button>
        </div>
    </div>
</div>
@Using Html.BeginJUIBox("User Work Unit")
   
    @<div class="row form-horizontal">
        @Using Html.BeginForm("SaveUserWorkUnit", "Account", Nothing, FormMethod.Post, New With {.id = "frmWorkUnit"})
            
            @Html.Hidden("Username", ViewData("userid"), New With {.id = "userid"})       
            @Html.WriteFormInput(New MvcHtmlString("<span class='form-control'>" & ViewData("userid").ToString() & "</span>"), "Nama User")
            @Html.WriteFormInput(Html.DropDownList("OfficeId", Nothing, Nothing, New With {.class = "form-control"}), "Unit Kerja")
            
            @<div class="well">
                <div class="row">
                    <div class="col-lg-offset-3">
                        <button class="btn btn-primary" type="button" id="btnSaveWorkUnit">
                            Simpan</button>
                    </div>
                </div>
            </div>
    End Using
    </div>
        
    
    
End Using
@Using Html.BeginJUIBox("Reset Password")
    Using Html.BeginForm("resetPassword", "Account", Nothing, FormMethod.Post, New With {.class = "form-horizontal", .id = "frmResetPassword"})
    @<div >
    @Html.Hidden("userkey", ViewData("Userkey") )
    <button class="btn btn-danger">Reset Password</button>
    <div class="well">
        Password baru:  <span class="" id="newpassword"></span>
    </div>
    
    </div>
    End Using

    
End Using
@Using Html.BeginJUIBox("Daftar Hak Akses User")
    @<div class="row">
        <div class="col-lg-12 col-sm-12">
            <div class="table-responsive">
                <table class="table table-bordered" id="tblRole">
                    <colgroup>
                        <col style="width: 60px">
                        <col style="width: auto;">
                        <col style="width: 80px; text-align: center">
                    </colgroup>
                    <thead>
                        <tr>
                            <th>
                                #
                            </th>
                            <th>
                                Module
                            </th>
                            <th>
                                Role
                            </th>
                            <th>
                            </th>
                        </tr>
                    </thead>
                </table>
            </div>
        </div>
    </div>     
    
    
   
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

        $("#btnSaveWorkUnit").click(function () {
            showSavingNotification();
            var _data = $("#frmWorkUnit").serialize(); ;
            var _url = $("#frmWorkUnit").attr("action");
            $.ajax({
                type: 'POST',
                url: _url,
                data: _data,
                success: function (data) {
                    showNotification("Kantor telah berubah.");
                   
                },
                error: ajax_error_callback,
                dataType: 'json'
            });



        });


        $("#frmResetPassword").submit(function (e) {
            e.preventDefault();
            var _data = $("#frmResetPassword").serialize(); ;
            var _url = $("#frmResetPassword").attr("action");
            $.ajax({
                type: 'POST',
                url: _url,
                data: _data,
                success: function (data) {
                     showNotification("Password telah berubah");
                    $("#newpassword").text(data.n);
                },
                error: ajax_error_callback,
                dataType: 'json'
            });

        });
        var _drawOnOff = function (data, type, row) {
            var rvalue = '<div class="checkbox-inline">' +
                        '<label>';

            if (type == 'display') {
                if (data == true) //isapproved?
                {
                    rvalue += '<input checked="" type="checkbox" data-rolename="' + row["FullRoleName"] + '" onclick="setRole(this)"/>';
                } else {
                    rvalue += '<input type="checkbox" data-rolename="' + row["FullRoleName"] + '" onclick="setRole(this)"/>';
                }
                rvalue += '&nbsp;<i class="fa fa-square-o"></i>' +
                          '</label>' +
                           '</div>';
                return rvalue;
            }
            return data;
        }

        var arrColumns = [
           { "data": "ModuleName", "sClass": "text-right" },
            { "data": "ModuleName" },
            { "data": "RoleName" },
            { "data": "Active", "mRender": _drawOnOff, "sClass": "text-center" }


        ];

        var _localLoad = function (data, callback, setting) {
            var _data = { "username": $("#userid").val() };
            $.ajax({
                url: '/Account/GetUserRole',
                data: _data,
                type: 'POST',
                success: callback,
                datatype: 'json'
            })

        };

        var grouping = function (settings) {
            _fnlocalDrawCallback(settings);
            var api = this.api();
            var rows = api.rows({ page: 'current' }).nodes();
            var last = null;
            api.column(1, { page: 'current' }).data().each(function (group, i) {
                if (last !== group) {
                    $(rows).eq(i).before(
                        '<tr class="group"><td colspan="3">' + group + '</td></tr>'
                    );

                    last = group;
                }
            });
        };
        datatableDefaultOptions.fnDrawCallback = grouping;
        datatableDefaultOptions.searching = true;
        datatableDefaultOptions.aoColumns = arrColumns;
        datatableDefaultOptions.columnDefs = [{ "visible": false, "targets": 1}];
        datatableDefaultOptions.ajax = _localLoad;
        tblRole = $("#tblRole").DataTable(datatableDefaultOptions)
           .on("click", ".btnRemoveRole", function () {
               if (confirm("Hapus item ini ?") == false) {
                   return;
               }

               var tr = $(this).closest('tr');
               var row = tblRole.row(tr);
               $.ajax({
                   type: "POST",
                   data: { RoleName: row.data().FullRoleName },
                   url: "/Roles/DeleteRole",
                   success: function (data) {
                       if (data.stat == 1) {
                           row.remove().draw();
                           tblRole.ajax.reload();
                       } else {
                           showNotificationSaveError(data, "Menghapus Role")
                       };

                   }
               });

           });

    });        //end init;

</script>
