<div id="CategoryForm" style="display:none">
    @Using Html.BeginJUIBox("Data Form Kategori Vendor")
        @<div class="row">
            @Using Html.BeginForm("Save", "VendorCategory", Nothing, FormMethod.Post, New With {.class = "form-horizontal", .id = "frmVendorCategory",
                                                                                                                   .autocomplete = "off"})
                @Html.Hidden("sVendorCategoryItem")
            End Using


            <div class="col-lg-12 col-sm-12">
                <div class="panel panel-primary">
                    <div class="panel-heading">
                        Item Kategori
                        <div class="pull-right">
                            <button class="btn btn-xs btn-danger " type="button" id='btnNewItemCategory'>
                                +</button>
                        </div>
                        <div class="clearfix">
                        </div>
                    </div>
                    <div class="table-responsive">
                        <table class="table table-bordered" id='tblItemCategory'>
                            <colgroup>
                                <col style="width: 60px" />
                                <col style="width: auto" />
                                <col style="width: 100px" />
                            </colgroup>
                            <thead class="bg-default">
                                <tr>
                                    <th>#</th>
                                    <th>Kategori</th>
                                    <th></th>
                                </tr>
                            </thead>
                        </table>
                    </div>
                </div>
                <div class="panel panel-primary" id='form_ItemCategory' style='display: none'>
                    <div class="panel-body">
                        <div>
                            <form class='form-horizontal' action='/Purchasing/VendorCategory/ValidateItemCategory'
                            onsubmit='return false;' autocomplete='off' id='frmItemCategory'>
                                @Html.Hidden("itemCategoryRowIdx", -1)
                                @Html.Hidden("itemCategoryId", 0, New With {.Name = "Id"})
                                @Html.WriteFormInput(Html.TextBox("Name", "", New With {.class = "form-control", .Id = "itemCategoryName"}), "Nama Kategori")
                            <div class='well'>
                                <div class='center-block' style='width: 50%'>
                                    <button class='btn btn-primary' type='button' id='BtnAddItemCategory'>
                                        Tambahkan</button>
                                    <button class='btn btn-primary' type='button' id="btnCloseFormItemCategory">
                                        Tutup</button>
                                </div>
                            </div>
                            </form>
                        </div>
                    </div>
                </div>
            </div>
        </div>
        
        @*@<div class="row">
            @Html.WriteFormInput(Html.TextBox("Nama Kategori", "", New With {.class = "form-control"}), "Kategori")
            @Html.WriteFormInput(Html.TextArea("Description", New With {.class = "form-control"}), "Deskripsi")
        </div>*@
        @<div class="row">
            <div class="col-lg-offset-5 col-sm-offset-5">
                <button class="btn btn-primary btn-label-left" type="submit" id="btnSaveCategory">
                    <span><i class="fa fa-save"></i></span>Simpan</button>
                <button class="btn btn-default" type="button" id="btnCancelCategory">
                    Batal</button>
            </div>
        </div>
    End Using
</div>

<script src="@Url.Content("~/plugins/datatables/jquery.dataTables.js")" type="text/javascript"></script>
<script src="../../../../plugins/datatables/dataTables.bootstrapPaging.js" type="text/javascript"></script>
<script src="@Url.Content("~/plugins/datatables/datatablehelper.js")" type="text/javascript"></script>
<script type="text/javascript" src="../../../../plugins/bootstrap-datetimepicker/bootstrap-datetimepicker.js"></script>
<script src="@Url.Content("~/plugins/bootstrap-typeahead/typeahead.bundle.min.js")" type="text/javascript"></script>
<link rel="stylesheet" type="text/css" href="@Url.Content("~/plugins/bootstrap-typeahead/typeahead.css")" />
<script type="text/javascript">

    var tblItemCategory = null;
    var _initDatatableDefaultOptions = function () {
        datatableDefaultOptions.searching = false;
        datatableDefaultOptions.columnDefs = [];
        datatableDefaultOptions.order = [[1, "asc"]];
        datatableDefaultOptions.autoWidth = false;
        datatableDefaultOptions.ordering = false;
        datatableDefaultOptions.fnDrawCallback = function (oSettings) {
            for (var i = 0, iLen = oSettings.aiDisplay.length; i < iLen; i++) {
                $('td:eq(0)', oSettings.aoData[oSettings.aiDisplay[i]].nTr).html((i + 1) + '.');
            }
        };
    }

    var _initItemCategory = function () {
        var _deleteItemCategory = function () {

            if (confirm("Hapus item ini?") == false) {
                return;
            }
            var row = tblItemCategory.row($(this).closest('tr'));

            if (row.data().Id == 0) {
                row.remove().draw();
                return;
            };
            $.ajax({
                url: '/Purchasing/VendorCategory/DeleteItemCategory',
                data: { id: row.data().id },
                type: 'POST',
                success: function (data) {
                    if (data.stat == 1) {
                        row.remove().draw();
                    } else {
                        shownotificationerror(data, "Penghapusan gagal");
                    }
                },
                datatype: 'json'
            });
        }

        var _editItemMaterialUsed = function () {
            var row = tblItemCategory.row($(this).closest('tr'));
            $("#itemCategoryId").val(row.data().id);
            $("#itemCategoryName").val(row.data().name);
            $("#itemCategoryRowIdx").val(row.index());
            $("#itemCategoryName").focus();
            $("#form_ItemCategory").show('slow');
        }

        var _renderDeleteItem = function (data, type, row) {
            if (type == 'display') {
                var html = [];
                html.push("<div class='btn-group'>");
                html.push("<button class='btn btn-primary ItemCategoryEdit'><span class='fa fa-edit'></span></button>");
                html.push("<button class='btn btn-danger ItemCategoryRemove'><span class='fa fa-remove '></span></button>");
                html.push("</div>");
                return html.join("");
            }
            return data;
        }
        var cols = [
            { data: "id", "sClass": "text-right" },
            { data: "name" },
            { data: "id", "mRender": _renderDeleteItem, "sClass": "text-center" }
        ];

        datatableDefaultOptions.aoColumns = cols;
        datatableDefaultOptions.ajax = function (data, callback, settings) {
            $.ajax({
                url: '/Purchasing/VendorCategory/GetCategoryItems',
                data: {},
                type: 'POST',
                success: callback,
                datatype: 'json'
            });
            return;
        }
        tblItemCategory = $('#tblItemCategory').DataTable(datatableDefaultOptions)
            .on("click", ".ItemCategoryRemove", _deleteItemCategory)
            .on("click", ".ItemCategoryEdit", _editItemMaterialUsed);

        //buttons
        $("#BtnAddItemCategory").click(function () {
            var d = tblItemCategory.columns(1).data().eq(0).indexOf($("#itemCategoryName").val());
            if (d != -1) {
                var obj = new Object();
                obj.errors = [{ Key: "ItemCategoryName", Value: ["Item sudah ada di dalam list"]}];
                showNotificationSaveError(obj, 'Penambahan gagal.');
                return;
            }

            var _data = $("#frmItemCategory").serialize();
            var _url = $("#frmItemCategory").attr("action");
            $.ajax({
                url: _url,
                data: _data,
                type: 'POST',
                success: function (data) {
                    if (data.stat == 1) {
                        if (data.rowIdx == '-1') {
                            tblItemCategory.row.add(data.model);
                        }
                        else {
                            var arrData = tblItemCategory.data();
                            arrData.splice(data.rowIdx, 1, data.model);
                            tblItemCategory.clear();
                            tblItemCategory.rows.add(arrData);
                        };
                        tblItemCategory.draw();
                        clearItem();
                        $(".has-error").removeClass("has-error");
                    } else {
                        showNotificationSaveError(data, 'Penambahan gagal.');
                    }
                }
            });

        }); //BtnAddItemCategory_click

        $("#btnNewItemCategory").click(function () {
            clearItem();
            $("#form_ItemCategory").show("slow");
            $("#itemCategoryRowIdx").val(-1);
        }); //btnNewItemCategory_click

        $("#btnCloseFormItemCategory").click(function () {
            clearItem();
            $("#form_ItemCategory").hide("slow");
        });

    };          //end _initItemCategory


    var clearItem = function () {
        $("#itemCategoryName").val("");
    }

    $(function () {//init

        _initDatatableDefaultOptions();
        _initItemCategory();
        $("#btnSaveCategory").removeAttr('disabled');
        $("#btnSaveCategory").click(function () {
            $("#btnSaveCategory").button("loading");
            var arrCategoryItem = [];
            $(tblItemCategory.data()).each(function (d, e) {
                arrCategoryItem.push(e);
            });
            $("#sVendorCategoryItem").val(JSON.stringify(arrCategoryItem));

            var _data = $('#frmVendorCategory').serializeArray();
            var _url = $("#frmVendorCategory").attr("action");
            $.ajax({
                type: 'POST',
                data: _data,
                url: _url,
                dataType: 'json',
                success: function (data) {
                    if (data.stat == 1) {
                        //window.location = "/Purcasing/Vendor/Index"
                        $('#CategoryId').html('<option selected="selected" value="0">-- Pilih --</option>');
                        $.each(data.ListItem, function (index, item) {
                            $('#CategoryId').append('<option value="' + item.Id + '">' + item.Name + '</option>');
                        });
                        $('#btnCancelCategory').click();
                    } else {
                        showNotificationSaveError(data, 'Penambahan gagal.');
                    }
                    $("#btnSaveCategory").button("reset");
                }
            });
        });
    });

</script>