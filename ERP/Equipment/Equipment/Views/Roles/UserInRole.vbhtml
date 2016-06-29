@Code
    Dim arrDAta = Me.Request.QueryString("rolename").Split("."c)
    
    ViewData("Title") = "Role"
End Code


<div class="row">
<div class="col-lg-12 col-sm-12">

    <div class="btn-group pull-right">
    <button class="btn btn-danger" onclick="window.location='/Roles/Index'">Kembali</button>
    </div>

</div>
</div>
@Using Html.BeginJUIBox("Users In Role")
    @Html.Hidden("Rolename", Me.Request.QueryString("rolename"))
    @<div class="row">
        <div class="form-horizontal">
            @Html.WriteFormInput(New MvcHtmlString("<span class='form-control'>" & arrDAta.First & "</span>"), "Modul")
            @Html.WriteFormInput(New MvcHtmlString("<span class='form-control'>" & arrDAta.Last & "</span>"), "Nama Role")
        </div>
    </div>
    @<div class="row">
        <div class="col-lg-12 col-sm-12">
		<div class="table-responsive">
            <table class="table table-bordered" id="tblUsers">
                <colgroup>
                    <col style="width: 60px">
                    <col style="width: auto">
                    <col style="width: 60px">
                </colgroup>
                <thead>
                    <tr>
                        <th>
                            #
                        </th>
                        <th>
                            Username
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
    var tblUsers = null;
    $(function () { //begin init



        var _renderEditColumn = function (data, type, row) {
            if (type == "display") {
                return "<div class='btn-group' role='group'>" +
                            "<button type='button' class='btn btn-danger btn-xs btnRemoveRole'><i class='fa fa-remove'></i></button>" +
                            "</div>";
            }
            return data;
        }



        var arrColumns = [
           { "data": "Username", "sClass": "text-right" },
            { "data": "Username" },
            { "data": "Username", "mRender": _renderEditColumn }
       ];

        var _localLoad = function (data, callback, setting) {
            var rolename = $("#Rolename").val();
            var _data = { "Rolename": rolename };
            $.ajax({
                url: '/Roles/GetUserInRole',
                data: _data,
                type: 'POST',
                success: callback,
                DataType: 'json'
            });

        };
        datatableDefaultOptions.searching = true;
        datatableDefaultOptions.aoColumns = arrColumns;
        datatableDefaultOptions.columnDefs = [];
        datatableDefaultOptions.ajax = _localLoad;
        tblUsers = $("#tblUsers").DataTable(datatableDefaultOptions)
           .on("click", ".btnRemoveRole", function () {
               if (confirm("Hapus item ini ?") == false) {
                   return;
               }

               var tr = $(this).closest('tr');
               var row = tblUsers.row(tr);
               $.ajax({
                   type: "POST",
                   data: { rolename: row.data().Rolename, userid: row.data().Username, lockstate: false },
                   url: "/Account/SetUserRole",
                   success: function (data) {
                       if (data.stat == 1) {
                           row.remove().draw();
                           tblUsers.ajax.reload();
                       } else {
                           showNotificationSaveError(Data, "Menghapus Role")
                       };

                   }
               });

           });

    });           //end init
   
   
</script>
