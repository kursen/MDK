@Code
    ViewData("Title") = "Index"
End Code
@Using Html.BeginJUIBox("Roles")
    
    @<div class="row">
        @Using Html.BeginForm("SaveRole", "Roles", Nothing, FormMethod.Post, New With {.class = "form-horizontal", .id = "frmRole",.autocomplete="off"})
            
            @Html.WriteFormInput(Html.DropDownList("ModuleName", Nothing, Nothing, New With {.class = "form-control"}), "Module")
            @Html.WriteFormInput(Html.TextBox("RoleName", Nothing, New With {.class = "form-control"}), "Nama Role")
            
            @<div class="well">
                <div class="row">
                    <div class="col-lg-offset-3">
                        <button class="btn btn-primary" type="button" id="btnSave">
                            Simpan</button>
                    </div>
                </div>
            </div>
    End Using
    </div>
    @<div class="row">
        <div class="col-lg-12 col-sm-12">
		<div class="table-responsive">
            <table class="table table-bordered" id="tblRole">
            <colgroup>
                <col style="width:60px">
                <col style="width:auto;">
                <col style="width:auto;">
                <col style="width:80px; text-align:center">
            </colgroup>
            <thead>
            <tr>
            <th>#</th>
            <th>Module</th>
            <th>Role</th>
            <th></th>
            <th></th>
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
        var tblRole = null;

        var saveRole = function () {
            var _data = $("#frmRole").serialize();
            var _url = $("#frmRole").attr("action");
            $.ajax({
                type: 'POST',
                url: _url,
                data: _data,
                success: saveRoleCallback,
                error: ajax_error_callback,
                dataType: 'json'
            });
        };

        saveRoleCallback = function (data) {
            if (data.stat == 1) {
                tblRole.ajax.reload();
                return;
            }
            showNotificationSaveError(data, "Menyimpan Role")
        };
      
    </script>
   <script type="text/javascript">

       $(function () { //begin init

           $("#btnSave").click(function () {
               saveRole();

           });

           var _renderEditColumn = function (data, type, row) {
               if (type == "display") {
                   return "<div class='btn-group' role='group'>" +
                            "<button type='button' class='btn btn-danger btn-xs btnRemoveRole'><i class='fa fa-remove'></i></button>" +
                            "</div>";
               }
               return data;
           }

           var _renderUserLink = function (data, type, row) {
               if (type == "display") {
                   return "<a class='btn btn-info' href='/Roles/UserinRole/?rolename=" + data + "'>User in Roles</a>";
               }
               return data;
           }



           var arrColumns = [
           { "data": "ModuleName", "sClass": "text-right" },
            { "data": "ModuleName" },
            { "data": "RoleName" },
            { "data": "RoleName", "mRender": _renderEditColumn, "sClass": "text-center" },
            { "data": "FullRoleName", "mRender": _renderUserLink, "sClass": "text-center" }

        ];

           var _localLoad = function (data, callback, setting) {

               $.ajax({
                   url: '/Roles/GetRoles',
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
                        '<tr class="group"><td colspan="4">' + group + '</td></tr>'
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

       });              //end init
   
   
   </script>