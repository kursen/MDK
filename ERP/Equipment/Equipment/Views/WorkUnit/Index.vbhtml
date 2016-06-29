@Code
    ViewData("Title") = "Index"
End Code
@Using Html.BeginJUIBox("Kantor Dan Unit Kerja")
    If User.IsInRole("Home.Administrator") Then
        
  
    @<div class="row">
        <div class="col-lg-12 col-sm-12">
            <div class="pull-right">
                <div class="btn-group">
                    <button class="btn btn-primary" id="btnKantorBaru">
                        Kantor Baru</button>
                </div>
            </div>
        </div>
    </div>
    @<div class="row">
        <div class="col-lg-4 col-sm-6">
            KANTOR</div>
        <div class="col-lg-4 col-sm-6">
            @Html.DropDownList("OfficeList", Nothing, New With {.class = "form-control"})
        </div>
        <br />
        <br />
        <hr />
    </div>
Else
    Dim p = ERPBase.ErpUserProfile.GetUserProfile()
        @Html.Hidden("OfficeList", p.WorkUnitId)
@<span>@p.WorkUnitName</span>
End If
    @<div class="row">
        <div class="col-lg-12 col-sm-12">
            <div class="table-responsive">
                <table class="table table-bordered" id="tblOffice">
                    <colgroup>
                        <col style="width: 60px">
                        <col>
                        <col style="width: 120px">
                    </colgroup>
                    <thead>
                        <tr>
                            <th>
                                #
                            </th>
                            <th>
                                Unit Kerja
                            </th>
                            <th>
                            </th>
                        </tr>
                    </thead>
                </table>
            </div>
        </div>
    </div>
    
    @<div class="modal fade" id="dlgOfficeForm" role="dialog">
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
                        @Using Html.BeginForm("saveWorkUnit", "WorkUnit", Nothing, FormMethod.Post, New With {.class = "form-horizontal", .id = "frmOffice", .autocomplete = "off"})
                            @Html.Hidden("Id", 0, New With {.class = "form-control", .id = "office_Id"})
                            @Html.Hidden("Parent_ID", 0, New With {.class = "form-control", .id = "office_Parent_ID"})
                            @Html.Hidden("Ordinal", 0, New With {.class = "form-control", .id = "office_Ordinal"})
                            @Html.WriteFormInput(Html.TextBox("Code", Nothing, New With {.class = "form-control", .id = "office_Code"}), "Kode",
                                              smLabelWidth:=3, smControlWidth:=4, lgLabelWidth:=3, lgControlWidth:=3)
                     
                            @Html.WriteFormInput(Html.TextBox("Name", Nothing, New With {.class = "form-control", .id = "office_Name"}), "Nama",
                                             smLabelWidth:=3, smControlWidth:=3, lgLabelWidth:=3, lgControlWidth:=7)
                            @Html.WriteFormInput(Html.TextBox("Abbreviation", Nothing, New With {.class = "form-control", .id = "office_Abbreviation"}), "Singkatan",
                                              smLabelWidth:=3, smControlWidth:=4, lgLabelWidth:=3, lgControlWidth:=3)
                            @<div class="form-group" id='div_ParentName' style='display: none'>
                                <label class="col-lg-3 col-sm-1 control-label">
                                    Unit
                                </label>
                                <div class="col-lg-7 col-sm-7">
                                    <p class="form-control" id="sp_ParentName" disabled="disabled">
                                        Direksi</p>
                                </div>
                            </div>
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
End Using
<script src="@Url.Content("~/plugins/datatables/jquery.dataTables.js")" type="text/javascript"></script>
<script src="@Url.Content("~/plugins/datatables/dataTables.bootstrap.js")" type="text/javascript"></script>
<script src="@Url.Content("~/plugins/datatables/datatablehelper.js")" type="text/javascript"></script>
<style>
    #tblOffice td
    {
        vertical-align: middle;
    }
</style>
<script type="text/javascript">
    var tblOffice = null;
    var 
        MENAMBAHKANTOR = "Menambah Kantor",
        EDITKANTOR = "Edit Kantor",
        MENAMBAHUNIT = "Menambah Unit",
        EDITUNIT = "Edit Unit";

    var _btnEditClick = function () {

        var tr = $(this).closest('tr');
        var row = tblOffice.row(tr);

        $("#office_Id").val(row.data().Id);
        $("#office_Code").val(row.data().Code);
        $("#office_Name").val(row.data().Name);
        $("#office_Abbreviation").val(row.data().Abbreviation);
        $("#office_Parent_ID").val(row.data().Parent_ID);

        var _parentPath = new String(row.data().pathname);
        _parentPath = _parentPath.replace(row.data().Name, "");
        $("#sp_ParentName").text(_parentPath);
        if (row.data().Parent_ID == 0) {
            $("#div_ParentName").hide();
            $("#modalTitle").text(EDITKANTOR);
        } else {
            $("#modalTitle").text(EDITUNIT);
            $("#div_ParentName").show();
        };
        $("#dlgOfficeForm").modal();
    };

    var _btnAddClick = function () {

        var tr = $(this).closest('tr');
        var row = tblOffice.row(tr);
        $('#frmOffice')[0].reset();
        $("#office_Id").val(0);
        $("#office_Parent_ID").val(row.data().Id);
        var _parentPath = new String(row.data().pathname);

        $("#sp_ParentName").text(_parentPath);
        if (row.data().Parent_ID == 0) {
            $("#div_ParentName").hide();

        } else {

            $("#div_ParentName").show();
        };

        $("#modalTitle").text(MENAMBAHUNIT);

        $("#dlgOfficeForm").modal();



    };

    var _btnRemoveClick = function () {
        var tr = $(this).closest('tr');
        var row = tblOffice.row(tr);
        if (confirm("Hapus Unit ini?")) {
            showSavingNotification("Menghapus unit kerja...");
            $.ajax({
                url: '/WorkUnit/DeleteOffice',
                data: { officeId: row.data().Id },
                type: 'POST',
                success: function (data) {

                    if (data.stat == 1) {
                        showNotification("Data terhapus");

                        if (row.data().Parent_ID == 0) {
                            $("#OfficeList option[value='" + row.data().Id + "']").remove();
                            $("#OfficeList").trigger("change");
                        } else {
                            tblOffice.ajax.reload();
                        }


                    } else {
                        showNotificationSaveError(data, "Menghapus unit kerja");
                    }

                },
                error: ajax_error_callback,
                datatype: 'json'
            });
        }
    };

    //-----
    var _initDataTable = function () {
        //===========table fill ====================
        var _localLoad = function (data, callback, setting) {
            var _officeId = $("#OfficeList").val();
            $.ajax({
                url: '/WorkUnit/GetOfficeList',
                data: { officeId: _officeId },
                type: 'POST',
                success: callback,
                error: ajax_error_callback,
                datatype: 'json'
            });


        };
        var _CellCreated = function (td, cellData, rowData, row, col) {
            $(td).css("padding-left", rowData.lvl * 30 + 30);

        }
        var _RenderEditItem = function (data, type, row) {
            if (type == 'display') {
                return "<div class='btn-group' role='group'>" +
                "<button type='button' class='btn btn-default btn-xs btnAddChild' ><i class='fa fa-plus'></i></button>" +
                "<button type='button' class='btn btn-default btn-xs btnEdit' ><i class='fa fa-edit'></i></button>" +
                "<button type='button' class='btn btn-default btn-xs btnRemove' ><i class='fa fa-remove'></i></button>" +
                "</div>"; ;
            }
            return data;
        }
        var arrColumns = [
          { "data": "Id", "sClass": "text-right" },
          { "data": "Name", 'fnCreatedCell': _CellCreated },
          { "data": "Id", "mRender": _RenderEditItem, "sClass": "text-center" }

        ];

        datatableDefaultOptions.searching = true;
        datatableDefaultOptions.aoColumns = arrColumns;
        datatableDefaultOptions.columnDefs = [];
        datatableDefaultOptions.ajax = _localLoad;
        datatableDefaultOptions.ordering = false;
        datatableDefaultOptions.bAutoWidth = false;


        tblOffice = $("#tblOffice").DataTable(datatableDefaultOptions)
        .on("click", ".btnEdit", _btnEditClick)
        .on("click", ".btnAddChild", _btnAddClick)
        .on("click", ".btnRemove", _btnRemoveClick);



        //============end table fill ================
    }



    ///---
    $(function () { //begin init
        $("#dlgOfficeForm").appendTo($("body"));
        $("#dlgOfficeForm").on('shown.bs.modal', function () {
            $('#office_Code').focus();

        });


        $("#btnKantorBaru").click(function () {
            $('#frmOffice')[0].reset();
            $("#modalTitle").text(MENAMBAHKANTOR);
            $("#dlgOfficeForm").modal();


        });

        $("#btnSave").click(function () {
            var _data = $("#frmOffice").serialize();
            var _url = $("#frmOffice").attr("action");
            var isNew = $("#office_Id").val() == 0;

            $.ajax({
                url: _url,
                data: _data,
                type: 'POST',
                success: function (data) {
                    if (data.stat == 1) {
                        showNotification("Data tersimpan");

                        $("#dlgOfficeForm").modal('hide');

                        if (data.model.Parent_ID == 0) {
                            if (isNew) {
                                $("#OfficeList").append($('<option>', { value: data.model.Id }).text(data.model.Name));
                                $("#OfficeList").val(data.model.Id);
                                $("#OfficeList").trigger("change");
                            } else {
                                $("#OfficeList option[value='" + data.model.Id + "']").remove();
                                $("#OfficeList").append($('<option>', { value: data.model.Id }).text(data.model.Name));
                                $("#OfficeList").val(data.model.Id);
                                $("#OfficeList").trigger("change");
                            }
                            console.log(data.model.Parent_ID);
                            console.log(isNew);
                        } else {
                            tblOffice.ajax.reload();
                        }

                    } else {
                        showNotificationSaveError(data, "Menyimpan data unit kerja");
                    }

                },
                error: ajax_error_callback,
                datatype: 'json'
            });

        });

        $("#OfficeList").change(function () {

            tblOffice.ajax.reload();

        });

        _initDataTable();
    });                                                    //end init



</script>
