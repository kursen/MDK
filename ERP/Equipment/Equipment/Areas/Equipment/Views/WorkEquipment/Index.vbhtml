@ModelType Equipment.OutFitEqp
@Code
    ViewData("Title") = "Informasi Perlengkapan Kerja"
    Dim roles = "Equipment.Supervisor, Equipment.Manager, Equipment.DataOperator".Split(",")
    Dim isProperUser = ERPBase.ErpAuthorization.UserInAnyRoles(roles, User)
End Code
@If isProperUser Then
@<div>
    <div class="row">
        <div class="col-xs-12">
            <div class="pull-right">
                <button class="btn btn-sm btn-success btn-label-left" id="btn-plus">
                    <span><i class="fa fa-plus"></i></span>Tambah Baru</button>
            </div>
        </div>
    </div>
    <!--form nambah peralatan -->
    <div id="hideForm2" style="display: none;">
        @Using Html.BeginJUIBox("Data Perlengkapan Kerja", False, False, False, False, False, "fa fa-plus")
 
            Using Html.BeginForm("Create", "WorkEquipment", Nothing, FormMethod.Post, New With {.class = "form form-horizontal", .id = "form-data", .autocomplete = "off"})
            @Html.ValidationSummary(True, "Penyimpanan data tidak berhasil. Harap perbaiki kesalahan dan coba lagi.")
            @<div class="row">
                @Html.WriteFormInput(Html.TextBoxFor(Function(model) model.Name, New With {.class = "form-control"}), "Nama")
                @Html.WriteFormIntegerInputFor(Function(m) m.Total, "Jumlah", ".", smLabelWidth:=4, lgLabelWidth:=3, lgControlWidth:=4, smControlWidth:=3)
                @Html.WriteFormIntegerInputFor(Function(m) m.Cost, "Harga Beli", ".", smLabelWidth:=4, lgLabelWidth:=3, lgControlWidth:=4, smControlWidth:=3)
                <div class="form-group">
                    <label class="col-lg-3 col-sm-4 control-label">
                        Keterangan
                    </label>
                    <div class="col-lg-3 col-sm-4">
                        @Html.TextArea("Remark", New With {.class = "form-control"})
                    </div>
                </div>
            </div>
            @<div class="form-actions form-actions-padding-sm">
                <div class="row">
                    <div class="col-md-5 col-md-offset-5">
                        <button class="btn btn-primary" type="submit" id="btnSave">
                            <i class="fa fa-save"></i>Simpan</button>
                        <button class="btn" type="button" onclick="$('#btn-plus').click()">
                            Batal</button>
                    </div>
                </div>
            </div>
            End Using

        End Using
    </div>
    <!-- end form-->
</div>
    End If
    @Html.Hidden("isProperUser",IIf(isProperUser, 1,0))
@Using Html.BeginJUIBox("Daftar Perlengkapan Kerja")
    @<table id="tb_Data" class="table table-bordered table-striped table-hover table-heading table-datatable dataTable responsive no-footer"
        width="100%">
        <thead>
            <tr>
                <th>
                    Perlengkapan
                </th>
                <th>
                    Jumlah
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

    fnremoveid = function (obj) {
        $(obj).find('#ID').remove();
    }
    $(document).ready(function () {

        var _actionIcons =
            "<div class='btn-group' role='group'>" 
            
        if ($("#isProperUser").val() == 1) {
            _actionIcons += "<button class='delete btn btn-danger btn-xs' rel='tooltip' data-toggle='tooltip' data-placement='right' title='Hapus'><i class='fa fa-trash-o'></i></button>";
        }

        _actionIcons += "</div>";
        GenTable = $('#tb_Data').DataTable({
            "ajax": "/WorkEquipment/getData",
            "paging": false,
            "ordering": false,
            "info": false,
            "searching": false,
            "columns": [
        {
            "data": "Name"
        },
        {
            "data": "Total", "sClass": "text-right"
        },
        {
            "className": "text-right",
            "data": "Cost",
            "mRender": _fnRender2DigitDecimal
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
        sBody.on('click', '.edit2', function () {
            fnReset2('#form-data');
            fnremoveid('#form-data');
            var data = GenTable.row($(this).parents('tr')).data();
            $('#Name').val(data.Name);
            $('#Total').val(data.Total);
            $('#Remark').val(data.Remark);
            $('#Cost').val(data.Cost);
            $("<input type='hidden' name='ID' id='ID' value='" + data.ID + "' />").appendTo('#form-data');
            if ($('#hideForm2').is(":hidden")) {
                explodeslide();
            }
        })
    .on('click', '.delete', function () {
        var data = GenTable.row($(this).parents('tr')).data();
        var urlDelete = "/WorkEquipment/Delete";
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
                            //swal(get.msg, get.msgDesc, "error");
                            showFailedNotification(get.msg)
                        }
                        return false;
                    },
                    error: ajax_error_callback
                });
            }
        });
    });


        $('#btn-plus').click(function (e) {
            e.preventDefault();
            fnReset2('#form-data');
            fnremoveid('#form-data');
            explodeslide();
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
                        explodeslide();
                    } else {
                        showNotificationSaveError(get);
                    }
                    GenTable.ajax.reload();
                    return false;
                },
                error: ajax_error_callback
            });
        });

        //set trigger of #idFilter button to refresh
        //    var val =GenTable.column(1).data().sum();
        //    console.log(val);

    });

    var explodeslide = function () {
        if ($('#hideForm2').is(":hidden")) {
            $('#btn-plus').removeClass('btn-success').addClass('btn-danger').html('Tutup');
        } else {
            $('#btn-plus').removeClass('btn-danger').addClass('btn-success').html('<span><i class="fa fa-plus"></i></span>Tambah Baru');
        }
        $('#hideForm2').toggle("explode", 500);
    }
</script>
