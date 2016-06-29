

var isAnyError = false;
var attr_DetB, tableDetailB;

GetListDetail_B = function (Id1, Id2) {
    var attr = _attrCRUD();
    attr.dataTable.autoWidth = false;
    attr.useRowNumber = false;
    attr.dataTable.bSort = false;
    attr.usingId.formId = '#frmDetail_B';
    attr.usingId.tableId = '#tb_DetailB';
    attr.url = {
        "Read": "ProjectManagement/Reports/GetDailyDetail_B?ProjectDiv=" + Id1 + "&DayWork=" + Id2,
        "RefreshTable": function (Id1, Id2, Id3) {
            return "ProjectManagement/Reports/GetDailyDetail_B?ProjectDiv=" + Id1 + "&DayWork=" + Id2
        },
        "Delete": "ProjectManagement/Reports/RemoveDailyDetail_B"
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
            "data": "MaterialName"
        },
        {
            "data": "QuantityUse",
            "className": "text-right border-none"
        },
        {
            "data": "QuantityUnit",
            "className": "border-none"
        },
        {
            "data": "QuantityImported",
            "className": "text-right border-right-none"
        },
        {
            "data": "QuantityUnit",
            "className": "border-none"
        },
        {
            "data": null,
            "bSortable": false,
            "className": "text-center",
            "defaultContent": "" +
                "<div class='btn-group' role='group'>" +
                    "<button class='btn btn-default btn-xs edit_B'><i class='fa fa-edit'></i></button>" +
                    "<button class='btn btn-default btn-xs delete_B'><i class='fa fa-remove'></i></button>" +
                "</div>"
        }
    ];
    attr.dataTable.bPaginate = false;
    tableDetailB = fnGetList(attr, true);

    $(attr.usingId.tableId + ' tbody')
    //for edit button
    .on('click', '.edit_B', function () {
        var Data = tableDetailB.row($(this).parents('tr')).data();
        if ($(attr.usingId.formId + " #ID_B").length == 0) {
            var hiddenID = '<input type="hidden" name="ID" id="ID_B" />';
            $(attr.usingId.formId).append(hiddenID);
        }

        var frm = $('#form_B');
        $('#btnAdd_A').html('<span><i class="fa fa-remove"></i></span> Tutup');

        $(frm).slideUp('fast', function () {
            $('#MaterialName').val(Data.MaterialName);
            $('#QuantityUnit').val(Data.QuantityUnit);
            $('#QuantityUse').val(Data.QuantityUse);
            $('#QuantityImported').val(Data.QuantityImported);
            $('#ID_B').val(Data.ID);
            $(frm).slideDown('normal');
        });
    })
    //for delete button
    .on('click', '.delete_B', function () {
        if ($('#form_B').is(":visible"))
            $("#btnBatal_B").click();

        var Data = tableDetailB.row($(this).parents('tr')).data();
        deleteComfirmModal(function (result) {
            if (result)
                fnDelete(Data.ID, attr.url.Delete, tableDetailB);
        });
    });

    attr_DetB = attr;
}


submitFormCallback_B = function (data) {
    if (data.stat == 1) {
        fnRefreshTable('#tb_DetailB', parseUrl(attr_DetB.url.RefreshTable(modelID, $('.DayWork').val())), function () {
            showNotification("Data telah berhasil disimpan");
            $("#btnBatal_B").click();
        });
        return false;
    }
    showNotificationSaveError(data);
}



$('#btnAdd_B').click(function () {
    $('#form_B').slideToggle('normal', function () {
        fnReset2('#frmDetail_B');
        $('#ID_B').val(0);
        $('#QuantityUseUnit,#QuantityImportedUnit').text('');
        if ($(this).is(":visible"))
            $('#btnAdd_B').html('<span><i class="fa fa-remove"></i></span> Tutup');
        else {
            $('#btnAdd_B').html('<span><i class="fa fa-plus"></i></span> Tambah');
        }
    });
});

$('#btnBatal_B').click(function () {
    $('#btnAdd_B').click();
});

$('#MaterialId').change(function () {
    $.ajax({ 
        type: 'POST',
        url:  "../GetMaterialUnit?Id=" + $('#MaterialId').val(),
        error: ajax_error_callback,
        dataType: 'json',
        success: function (result) {
            $("#QuantityUseUnit, #QuantityImportedUnit").html(result);
        }
    })
});

$(document).ready(function () {
    $("#frmDetail_B").submit(function (e) {
        e.stopPropagation();
        e.preventDefault();
        showSavingNotification();
        var _data = $(this).serialize();
        var _url = $(this).attr("action");
        $.ajax({
            type: 'POST',
            url: _url,
            data: _data,
            success: submitFormCallback_B,
            error: ajax_error_callback,
            dataType: 'json'
        });
    });
});