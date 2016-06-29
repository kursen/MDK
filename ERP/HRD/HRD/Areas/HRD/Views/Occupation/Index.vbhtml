@Code
    ViewData("Title") = "Index"
End Code
@Using Html.BeginJUIBox("Jabatan")
    
       @<div class="row">
        <div class="col-lg-12 col-sm-12">
            <div class="pull-right">
                <div class="btn-group">
                    <button class="btn btn-primary" id="btnNewOccupation">
                        Menambah Jabatan</button>
                </div>
            </div>
        </div>
    </div>
    @<div class="row">
        <div class="col-lg-12 col-sm-12">
		<div class="table-responsive">
            <table class="table table-bordered" id="tblOccupation" data-page-length='25'>
                <colgroup>
                <col style="width:60px"/>
                <col style="width:auto"/>
                <col style="width:160px"/>
                <col style="width:100px"/>
                </colgroup>
                <thead>
                    <tr>
                        <th>
                            #
                        </th>
                        <th>
                            Jabatan
                        </th>
                        <th>Level
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
<div class="modal fade" id="dlgOccupation" role="dialog">
    <div class="modal-dialog" role="document">
        <div class="modal-content">
            <div class="modal-header">
                <button type="button" class="close" data-dismiss="modal" aria-label="Close">
                    <span aria-hidden="true">&times;</span></button>
                <h4 class="modal-title" id="modalTitle">
                    Menambah Kantor</h4>
            </div>
            <div class="modal-body">
                <div class="row">
                    @Using Html.BeginForm("SaveOccupation", "Occupation", Nothing, FormMethod.Post, New With {.class = "form-horizontal", .id = "frmOccupation", .autocomplete = "off"})
                        @Html.Hidden("Id", 0)
                        @Html.Hidden("Ordinat", 0)
                        
                        @Html.WriteFormInput(Html.TextBox("Name", Nothing, New With {.class = "form-control"}), "Nama Jabatan")   
                        @Html.WriteFormInput(Html.DropDownList("ManagementLevelId", Nothing, New With {.class = "form-control"}), "Tingkatan Jabatan")
                           
                    End Using
                </div>
                <div class="modal-footer">
                    <button class="btn btn-primary" id="btnSave">
                        Simpan</button>
                    <button class="btn btn-primary" data-dismiss="modal">
                        Batal</button>
                </div>
            </div>
        </div>
    </div>
</div>
<link href="../../../../plugins/datatables/dataTables.bootstrap.css" rel="stylesheet"
    type="text/css" />
<script src="@Url.Content("~/plugins/datatables/jquery.dataTables.js")" type="text/javascript"></script>
<script src="@Url.Content("~/plugins/datatables/dataTables.bootstrap.js")" type="text/javascript"></script>
<script src="@Url.Content("~/plugins/datatables/dataTables.bootstrapPaging.js")" type="text/javascript"></script>
<script src="@Url.Content("~/plugins/datatables/datatablehelper.js")" type="text/javascript"></script>
<script type="text/javascript">

    var tblOccupation = null


    var 
        MENAMBAHJABATAN = "Menambah Jabatan",
        EDITJABATAN = "Edit Jabatan";

    var _btnEditClick = function () {

        var tr = $(this).closest('tr');
        var row = tblOccupation.row(tr);

        $("#Id").val(row.data().Id);

        $("#Name").val(row.data().Name);

        $("#ManagementLevel").val(row.data().LevelId);
        $("#modalTitle").text(EDITJABATAN);

        $("#dlgOccupation").modal();

    };


   

    var _btnRemoveClick = function () {
        var tr = $(this).closest('tr');
        var row = tblOccupation.row(tr);
        if (confirm("Hapus Jabatan ini?")) {
            showSavingNotification("Menghapus jabatan...");
            $.ajax({
                url: '/Occupation/DeleteOccupation',
                data: { OccupationId: row.data().Id },
                type: 'POST',
                success: function (data) {

                    if (data.stat == 1) {
                        showNotification("Data terhapus");
                        tblOccupation.ajax.reload();
                    } else {
                        showNotificationSaveError(data, "Menghapus Jabatan");
                    }

                },
                error: ajax_error_callback,
                datatype: 'json'
            });
        }
    };

    $("#btnSave").click(function () {
        var _data = $("#frmOccupation").serialize();
        var _url = $("#frmOccupation").attr("action");


        $.ajax({
            url: _url,
            data: _data,
            type: 'POST',
            success: function (data) {
                if (data.stat == 1) {
                    showNotification("Data tersimpan");

                    $("#dlgOccupation").modal('hide');
                    tblOccupation.ajax.reload();

                } else {
                    showNotificationSaveError(data, "Menyimpan data jabatan");
                }

            },
            error: ajax_error_callback,
            datatype: 'json'
        });

    });


    var _initOccupationDataTable = function () {
        //===========table fill ====================
        var _localLoad = function (data, callback, setting) {
            $.ajax({
                url: '/Occupation/GetOccupationList',
                type: 'POST',
                success: callback,
                error: ajax_error_callback,
                datatype: 'json'
            });


        };

        var _RenderEditItem = function (data, type, row) {
            if (type == 'display') {
                return "<div class='btn-group' role='group'>" +
                "<button type='button' class='btn btn-default btn-xs btnEdit' ><i class='fa fa-edit'></i></button>" +
                "<button type='button' class='btn btn-default btn-xs btnRemove' ><i class='fa fa-remove'></i></button>" +
                "</div>"; ;
            }
            return data;
        }
        var _RenderOccupationLevel = function (data, type, row) {
            if (type == 'display') {
                return data
            }
            return row.LevelId;
        }
        var arrColumns = [
          { "data": "Id", "sClass": "text-right", "orderable": false },
          { "data": "Name" },
          { "data": "ManagementLevel", "mRender": _RenderOccupationLevel },
          { "data": "Id", "mRender": _RenderEditItem, "sClass": "text-center", "orderable": false }

        ];

        datatableDefaultOptions.searching = true;
        datatableDefaultOptions.aoColumns = arrColumns;
        datatableDefaultOptions.columnDefs = [];
        datatableDefaultOptions.ajax = _localLoad;
        datatableDefaultOptions.ordering = true;
        datatableDefaultOptions.bAutoWidth = false
        datatableDefaultOptions.paging = true;
        datatableDefaultOptions.searching = true;
        datatableDefaultOptions.order = [[2, "asc"]];
        datatableDefaultOptions.bLengthChange = true;
        datatableDefaultOptions.displayLength = 4;
        datatableDefaultOptions.sDom = "<'row'<'col-lg-12 col-sm-12'<'pull-right'f>>><'row'<'col-sm-12'tr>><'row'<'col-sm-5'i><'col-sm-7'p>>";
        tblOccupation = $("#tblOccupation").DataTable(datatableDefaultOptions)
        .on("click", ".btnEdit", _btnEditClick)
        .on("click", ".btnRemove", _btnRemoveClick);



        //============end table fill ================
    }

    $(function () {
        $("#dlgOccupation").appendTo($("body"));
        $("#dlgOccupation").on('shown.bs.modal', function () {
            $('#Name').focus();

        });

        $("#btnNewOccupation").click(function () {
            $('#frmOccupation')[0].reset();
            $("#Id").val(0);
            $("#modalTitle").text(MENAMBAHJABATAN);
            $("#dlgOccupation").modal();


        });

        _initOccupationDataTable();

    });
</script>
