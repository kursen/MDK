

var isAnyError = false;
var attr_DetD, tableDetailD;

GetListDetail_D = function (Id1, Id2) {
    var attr = _attrCRUD();
    attr.dataTable.autoWidth = false;
    attr.useRowNumber = false;
    attr.dataTable.bSort = false;
    attr.usingId.formId = '#frmDetail_D';
    attr.usingId.tableId = '#tb_DetailD';
    attr.url = {
        "Read": "ProjectManagement/Reports/GetDailyDetail_D?ProjectDiv=" + Id1 + "&DayWork=" + Id2,
        "RefreshTable": function (Id1, Id2, Id3) {
            return "ProjectManagement/Reports/GetDailyDetail_D?ProjectDiv=" + Id1 + "&DayWork=" + Id2
        },
        "Delete": "ProjectManagement/Reports/RemoveDailyDetail_D"
    };
    attr.dataTable.ajax.error = function (xhr, error, thrown) {
        isAnyError = true;
        ajax_error_callback(xhr, error, thrown);
    }
    attr.dataTable.columns = [
        {
            "className": "text-right",
            "data": "Number"
        },
        {
            "data": "Position"
        },
        {
            "data": "Amount",
            "className": "text-right"
        },
        {
            "data": "Unit",
            "className": "text-center"
        },
        {
            "data": null,
            "bSortable": false,
            "className": "text-center",
            "defaultContent": "" +
                "<div class='btn-group' role='group'>" +
                    "<button class='btn btn-default btn-xs edit_D'><i class='fa fa-edit'></i></button>" +
                    "<button class='btn btn-default btn-xs delete_D'><i class='fa fa-remove'></i></button>" +
                "</div>"
        }
    ];
    attr.dataTable.bPaginate = false;
    tableDetailD = fnGetList(attr, true);

    $(attr.usingId.tableId + ' tbody')
    //for edit button
    .on('click', '.edit_D', function () {
        var Data = tableDetailD.row($(this).parents('tr')).data();
        if ($(attr.usingId.formId + " #ID_D").length == 0) {
            var hiddenID = '<input type="hidden" name="ID" id="ID_D" />';
            $(attr.usingId.formId).append(hiddenID);
        }

        var frm = $('#form_D');
        $('#btnAdd_A').html('<span><i class="fa fa-remove"></i></span> Tutup');

        $(frm).slideUp('fast', function () {
            $('#Position').val(Data.Position);
            $('#Amount_D').val(Data.Amount);
            $('#Unit').val(Data.Unit);
            $('#ID_D').val(Data.ID);
            $(frm).slideDown('normal');
        });
    })
    //for delete button
    .on('click', '.delete_D', function () {
        if ($('#form_D').is(":visible"))
            $("#btnBatal_D").click();

        var Data = tableDetailD.row($(this).parents('tr')).data();
        deleteComfirmModal(function (result) {
            if (result)
                fnDelete(Data.ID, attr.url.Delete, tableDetailD);
        });
    });

    attr_DetD = attr;
}


submitFormCallback_D = function (data) {
    if (data.stat == 1) {
        fnRefreshTable('#tb_DetailD', parseUrl(attr_DetD.url.RefreshTable(modelID, $('.DayWork').val())), function () {
            showNotification("Data telah berhasil disimpan");
            $("#btnBatal_D").click();
        });
        return false;
    }
    showNotificationSaveError(data);
}



$('#btnAdd_D').click(function () {
    $('#form_D').slideToggle('normal', function () {
        fnReset2('#frmDetail_D');
        $('#ID_D').val(0);
        if ($(this).is(":visible")) {
            $('#btnAdd_D').html('<span><i class="fa fa-remove"></i></span> Tutup');
        }  else {
            $('#btnAdd_D').html('<span><i class="fa fa-plus"></i></span> Tambah');
        }
    });
});

$('#btnBatal_D').click(function () {
    $('#btnAdd_D').click();
});

$(document).ready(function () {
    $("#frmDetail_D").submit(function (e) {
        e.stopPropagation();
        e.preventDefault();
        showSavingNotification();
        var _data = $(this).serialize();
        var _url = $(this).attr("action");
        $.ajax({
            type: 'POST',
            url: _url,
            data: _data,
            success: submitFormCallback_D,
            error: ajax_error_callback,
            dataType: 'json'
        });
    });
});