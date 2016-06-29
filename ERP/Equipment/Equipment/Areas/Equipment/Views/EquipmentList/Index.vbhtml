@ModelType Equipment.MachineEqp
@Code
    ViewData("Title") = "Informasi Peralatan Mesin"
    
    Dim roles = "Equipment.Supervisor, Equipment.Manager, Equipment.DataOperator".Split(",")
    Dim isProperUser = ERPBase.ErpAuthorization.UserInAnyRoles(roles, User)
    
    
End Code
@If isProperUser Then
    @<div class="row">
    <div class="col-xs-12">
        <div class="pull-right">
            <button class="btn btn-sm btn-success btn-label-left" id="btn-add">
                <span><i class="fa fa-plus"></i></span>Tambah Baru</button>
        </div>
    </div>
</div>
@<!--form nambah peralatan -->
@<div id="hideForm" style="display: none;">
@Using Html.BeginJUIBox("Data Peralatan Mesin", False, False, False, False, False, "fa fa-plus")

    Using Html.BeginForm("Create", "EquipmentList", Nothing, FormMethod.Post, New With {.class = "form form-horizontal", .id = "form-data", .autocomplete = "off"})
            @Html.ValidationSummary(True, "Penyimpanan data tidak berhasil. Harap perbaiki kesalahan dan coba lagi.")
            @<div class="row">
            <div class="col-lg-6">
                @Html.WriteFormInput(Html.TextBoxFor(Function(model) model.Name, New With {.class = "form-control"}), "Nama", smLabelWidth:=4, lgLabelWidth:=3, lgControlWidth:=4, smControlWidth:=3)
                @Html.WriteFormInput(Html.TextBoxFor(Function(model) model.Merk, New With {.class = "form-control"}), "Merk")
                @Html.WriteFormInput(Html.TextBoxFor(Function(model) model.Type, New With {.class = "form-control"}), "Tipe")
                @Html.WriteFormIntegerInputFor(Function(m) m.Cost, "Harga Beli",".",smLabelWidth:=4,lgLabelWidth:=3,lgControlWidth:=4,smControlWidth:=3)
                @Html.WriteFormInput(Html.TextBoxFor(Function(model) model.SerialNumber, New With {.class = "form-control"}), "Nomor Seri")
                </div>
                 <div class="col-lg-6">
                @Html.WriteFormInput(Html.TextBoxFor(Function(model) model.Capacity, New With {.class = "form-control"}), "Kapasitas")
                @Html.WriteFormYearInputFor(Function(m) m.MadeYear, "Tahun Pembelian", smLabelWidth:=4, lgLabelWidth:=3, lgControlWidth:=2, smControlWidth:=2)
                @Html.WriteFormYearInputFor(Function(m) m.BuyYear, "Tahun Pembuatan", smLabelWidth:=4, lgLabelWidth:=3, lgControlWidth:=2, smControlWidth:=2)
                
                <div class="form-group">
                    <label class="col-lg-3 col-sm-4 control-label">
        Keterangan
                    </label>
                    <div class="col-lg-6 col-sm-5">
                        @Html.TextArea("Remark", New With {.class = "form-control"})
                    </div>
                </div>
               
                <div class="form-group">
                    <label class="col-lg-3 col-sm-4 control-label">
        Wilayah
                    </label>
                    <div class="col-lg-6 col-sm-5">
                        <select id="area" name="IDArea" class="form-control">
                        
                        </select>
                    </div>
                </div>
            </div>
            </div>
            @<div class="form-actions form-actions-padding-sm">
                <div class="row">
                    <div class="col-md-5 col-md-offset-5">
                        <button class="btn btn-primary" type="submit" id="btnSave">
                            <i class="fa fa-save"></i>Simpan</button>
                        <button class="btn" type="button" onclick="$('#btn-add').click()">
                            Batal</button>
                    </div>
                </div>
            </div>
    End Using
End Using
  </div>
@<!-- end form-->
End If
@Html.Hidden("isProperUser", IIf(isProperUser,1,0))
@Using Html.BeginJUIBox("Daftar Peralatan Mesin")
    @<table id="tb_Data" class="table table-bordered table-striped table-hover table-heading table-datatable dataTable responsive no-footer"
        width="100%">
        <thead>
            <tr>
                <th>
                    Jenis Peralatan
                </th>
                <th>
                    Merk
                </th>
                <th>
                    Tipe
                </th>
                <th>
                    Harga Beli
                </th>
                <th>
                    Keterangan
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
    var fnReset2 = function (objform) {
        $(objform).trigger("reset");
        $('.form-group,.col-md-5', $(objform)).removeClass("has-error");
    }
    //initiliaze ID Area
    $.post('/Equipment/EquipmentList/getOfficeId', function (data, status) {
        var strhtml = "";
        $.each(data.officelist, function (k, v) {
            strhtml += '<option value="' + v.value + '">' + v.text + '</option>';
        });
        $('#area').append(strhtml);
    });

    fnremoveid = function (obj) {
        $(obj).find('#ID').remove();
    }

    $(document).ready(function () {

        var _actionIcons =
            "<div class='btn-group' role='group'>" +
            "<button class='detail btn btn-warning btn-xs' title='Detail'><i class='fa fa-list-alt'></i></button>";
        if ($("#isProperUser").val() == 1) {
            _actionIcons += "<button class='delete btn btn-danger btn-xs' rel='tooltip' data-toggle='tooltip' data-placement='right' title='Hapus'><i class='fa fa-trash-o'></i></button>";
        }

        _actionIcons += "</div>";


        GenTable = $('#tb_Data').DataTable({
            "ajax": "/EquipmentList/getData",
            "paging": false,
            "ordering": false,
            "info": false,
            "searching": false,
            "columns": [
        {
            "data": "Name", "sClass": "text-center"
        },
        {
            "data": "Merk"
        },
         {
             "data": "Type"
         },
        {
            "className": "text-right",
            "data": "Cost"
        },
        {
            "data": "Remark"
        },
        {
            "className": "action text-center",
            "data": null,
            "bSortable": false,
            "defaultContent": _actionIcons
        }
        ]
        });

        var sBody = $('#tb_Data tbody');
        sBody.on('click', '.detail', function () {
            var data = GenTable.row($(this).parents('tr')).data();
            window.location.href = "/Equipment/EquipmentList/Detail/" + data.ID;
        })
    .on('click', '.delete', function () {
        var data = GenTable.row($(this).parents('tr')).data();
        var urlDelete = "/Equipment/EquipmentList/Delete";
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


        $('#btn-add').click(function (e) {
            e.preventDefault();
            fnReset2('#form-data');
            fnremoveid('#form-data');
            foldslide();
        });

        $('#form-data').submit(function (e) {
            e.preventDefault();
            var url = $(this).attr('action');
            var data = $(this).serialize();
            $.ajax({
                type: 'POST',
                dataType: 'json',
                url: url,
                data: data,
                beforeSend: function () {
                    showNotification("Sedang Menyimpan Data!");
                },
                success: function (get) {
                    if (get.stat == 1) {
                        showNotification("Data Berhasil Disimpan");
                        //slide();
                        foldslide();
                    } else {
                        showNotificationSaveError(get);
                    }
                    GenTable.ajax.reload();
                    return false;
                },
                error: ajax_error_callback
            });
            // });
        });

        //set trigger of #idFilter button to refresh
        //    var val =GenTable.column(1).data().sum();
        //    console.log(val);


    });

    var foldslide = function () {
        if ($('#hideForm').is(":hidden")) {
            $('#btn-add').removeClass('btn-success').addClass('btn-danger').html('Tutup');
        } else {
            $('#btn-add').removeClass('btn-danger').addClass('btn-success').html('<span><i class="fa fa-plus"></i></span>Tambah Baru');
        }
        $('#hideForm').toggle("fold",500);
    }
</script>
