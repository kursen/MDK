

var isAnyError = false;
var attr_DetE, tableDetailE;

GetListDetail_E = function (Id1, Id2) {
    var attr = _attrCRUD();
    attr.dataTable.autoWidth = false;
    attr.useRowNumber = false;
    attr.dataTable.bSort = false;
    attr.usingId.formId = '#frmDetail_E';
    attr.usingId.formSubId = '#frmsubDetail_E';
    attr.usingId.tableId = '#tb_DetailE';
    attr.url = {
        "Read": "ProjectManagement/Reports/GetDailyDetail_E?ProjectDiv=" + Id1 + "&DayWork=" + Id2,
        "RefreshTable": function (Id1, Id2, Id3) {
            return "ProjectManagement/Reports/GetDailyDetail_E?ProjectDiv=" + Id1 + "&DayWork=" + Id2
        },
        "Delete": "ProjectManagement/Reports/RemoveDailyDetail_E",
        "SubDelete": "ProjectManagement/Reports/RemoveDailySubDetail_E"
    };
    attr.dataTable.ajax.error = function (xhr, error, thrown) {
        isAnyError = true;
        ajax_error_callback(xhr, error, thrown);
    }
    attr.dataTable.columns = [
        {
            "data": "PositionTypeConcat"
        },
        {
            "data": null,
            "defaultContent": "",
            "className" : "border-none"
        },
        {
            "data": "Position",
            "mRender": function (data, type, full) {
                return full['Ordinal'] + ". " + data;
            },
            "className": "border-left-none"
        },
        {
            "data": "Amount",
            "className": "text-center",
            "mRender": function (data) {
                return data + " org";
            }
        },
        {
            "data": null,
            "bSortable": false,
            "className": "text-center",
            "defaultContent": "" +
                "<div class='btn-group' role='group'>" +
                    "<button class='btn btn-default btn-xs subedit_E'><i class='fa fa-edit'></i></button>" +
                    "<button class='btn btn-default btn-xs subdelete_E'><i class='fa fa-remove'></i></button>" +
                "</div>"
        }
    ];
    attr.dataTable.columnDefs = [
            { "visible": false, "targets": 0 }
    ];
    attr.dataTable.order = [[0, 'asc']];
    attr.dataTable.bPaginate = false;
    attr.dataTable.drawCallback = function (settings) {
        var api = this.api();
        var rows = api.rows({ page: 'current' }).nodes();
        var last = null;

        api.column(0, { page: 'current' }).data().each(function (group, i) {
            if (last !== group) {
                $(rows).eq(i).before(
                    '<tr class="group"><td colspan="4" style="background-color:#F9F9F9">' +
                        '<span style="padding-top: 10px; display: inline-block;">' + group + '</span>' +
                        "<div class='pull-right'>" +
                            "<div class='btn-group' role='group'>" +
                            "<button class='btn btn-default btn-xs subadd_E'><i class='fa fa-plus'></i></button>" +
                            //"<button class='btn btn-default btn-xs edit_E'><i class='fa fa-edit'></i></button>" +
                            "<button class='btn btn-default btn-xs delete_E'><i class='fa fa-remove'></i></button>" +
                        "</div>" +
                "</div>" +
                    '</td></tr>'
                );

                last = group;
            }
        });
    }


    tableDetailE = fnGetList(attr, true);

    $(attr.usingId.tableId + ' tbody')
    //for delete button
    .on('click', '.delete_E', function () {
        if ($('#form_E').is(":visible"))
            $("#btnBatal_E").click();
        else if ($('#subform_E').is(":visible"))
            $("#btnSubBatal_E").click();

        var Data = tableDetailE.row($(this).parents('tr').next()).data();
        deleteComfirmModal(function (result) {
            if (result)
                fnDelete(Data.ID, attr.url.Delete, tableDetailE);
        });
    })
    //for add sub button
    .on('click', '.subadd_E', function () {
        var Data = tableDetailE.row($(this).parents('tr').next()).data();
        fnReset2('#frmsubDetail_E');

        if ($('#form_E').is(":visible"))
            $('#form_E').slideUp('fast');

        var frm = $('#subform_E');

        $(frm).slideUp('fast', function () {
            $('#PositionTypeConcat').val(Data.PositionTypeConcat);
            //$('.Position_').val('');
            //$('.Amount_E_').val('');
            $('.ID_E_').val(0);
            $('.PositionType_').val(Data.PositionType);
            $('.PositionTypeName_').val(Data.PositionTypeName);
            $(frm).slideDown('normal');
        });

        $('#btnAdd_E').html('<span><i class="fa fa-plus"></i></span> Tambah');

    })
    //for edit sub button
    .on('click', '.subedit_E', function () {
        var Data = tableDetailE.row($(this).parents('tr')).data();
        if ($(attr.usingId.formId + " #ID_E").length == 0) {
            var hiddenID = '<input type="hidden" name="ID" id="ID_E" />';
            $(attr.usingId.formSubId).append(hiddenID);
        }

        var frm = $('#subform_E');

        if ($('#form_E').is(":visible")) {
            $('#form_E').slideUp('fast');
        }
        $('#btnAdd_E').html('<span><i class="fa fa-plus"></i></span> Tambah');

        $(frm).slideUp('fast', function () {
            $('#PositionTypeConcat').val(Data.PositionTypeConcat);
            $('.Position_').val(Data.Position);
            $('.Amount_E_').val(Data.Amount);
            $('.ID_E_').val(Data.ID);
            $('.PositionType_').val(Data.PositionType);
            $('.PositionTypeName_').val(Data.PositionTypeName);
            $(frm).slideDown('normal');
        });
    })
    //for delete sub button
    .on('click', '.subdelete_E', function () {
        if ($('#form_E').is(":visible"))
            $("#btnBatal_E").click();
        else if ($('#subform_E').is(":visible"))
            $("#btnSubBatal_E").click();

        var Data = tableDetailE.row($(this).parents('tr')).data();
        deleteComfirmModal(function (result) {
            if (result)
                fnDelete(Data.ID, attr.url.SubDelete, tableDetailE);
        });
    });

    attr_DetE = attr;
}


submitFormCallback_E = function (data) {
    if (data.stat == 1) {
        fnRefreshTable('#tb_DetailE', parseUrl(attr_DetE.url.RefreshTable(modelID, $('.DayWork').val())), function () {
            showNotification("Data telah berhasil disimpan");
            
            if ($('#form_E').is(":visible"))
                $("#btnBatal_E").click();
            else
                $("#btnSubBatal_E").click();
        });
        return false;
    }
    showNotificationSaveError(data);
}



$('#btnAdd_E').click(function () {
    $('#form_E').slideToggle('normal', function () {

        if ($('#subform_E').is(":visible"))
            $('#subform_E').slideUp('fast');

        ResetForm_E();
        if ($(this).is(":visible")) {
            $('#btnAdd_E').html('<span><i class="fa fa-remove"></i></span> Tutup');
        } else {
            $('#btnAdd_E').html('<span><i class="fa fa-plus"></i></span> Tambah');
        }
    });
});

$('#btnBatal_E').click(function () {
    $('#btnAdd_E').click();
});

$('#btnSubBatal_E').click(function () {
    $('#subform_E').slideUp();
});

$(document).ready(function () {
    $("#frmDetail_E").submit(function (e) {
        e.stopPropagation();
        e.preventDefault();
        showSavingNotification();
        var _data = $(this).serialize();
        var _url = $(this).attr("action");

        //var formData = new FormData(this);
        $.ajax({
            type: 'POST',
            url: _url,
            data: _data,
            success: submitFormCallback_E,
            error: ajax_error_callback,
            dataType: 'json'
        });
    });

    $("#frmsubDetail_E").submit(function (e) {
        e.stopPropagation();
        e.preventDefault();
        showSavingNotification();
        var _data = $(this).serialize();
        var _url = $(this).attr("action");

        var formData = new FormData(this);
        $.ajax({
            type: 'POST',
            url: _url,
            data: formData,
            success: submitFormCallback_E,
            error: ajax_error_callback,
            async: false,
            contentType: false,
            processData: false,
            dataType: 'json'
        });
    });


    /*==========================*/

    ResetForm_E = function () {
        fnReset2('#frmDetail_E');
        var row = $('.PositionRow');
        var i = row.length - 1;
        while (i > 0) {
            row.eq(i).remove();
            i--;
        }
        $('.has-error').removeClass('has-error');
    }

    removeRowPosition = function (o) {
        $(o).click(function () {
            var obj = $(this).closest('.PositionRow');
            if ($('.PositionRow').length > 1)
                obj.remove();
            else {
                obj.find('input').removeAttr('name').val('');
                obj.hide();
            }
        });
    }

    $('#addPositionRow').click(function () {
        var obj = $('.PositionRow');
        if (obj.is(':visible')) {
            var oRow = $('.PositionRow:last').clone();
            oRow.find('input[id="Position"]').val('');
            oRow.find('input[id="Amount"]').val('0');
            oRow.find('input[id="ID_E"]').val('0');
            removeRowPosition(oRow.find('.deletePositionRow'));
            $(oRow).insertAfter('.PositionRow:last');
        } else {
            obj.find('input[id="Position"]').attr('name', 'Position').val('');
            obj.find('input[id="Amount"]').attr('name', 'Amount').val('0');
            obj.find('input[id="ID_E"]').attr('name', 'ID_E').val('0');
            obj.show();
        }
    });

    removeRowPosition($('.deletePositionRow'));

    $('#PositionTypeName').keyup(function () {
        this.value = this.value.toUpperCase();
    })

});