

var isAnyError = false;
var attr_DetC, tableDetailC;

GetListDetail_C = function (Id1, Id2) {
    var attr = _attrCRUD();
    attr.dataTable.autoWidth = false;
    attr.useRowNumber = false;
    attr.dataTable.bSort = false;
    attr.usingId.formId = '#frmDetail_C';
    attr.usingId.tableId = '#tb_DetailC';
    attr.url = {
        "Read": "ProjectManagement/Reports/GetDailyDetail_C?ProjectDiv=" + Id1 + "&DayWork=" + Id2,
        "RefreshTable": function (Id1, Id2, Id3) {
            return "ProjectManagement/Reports/GetDailyDetail_C?ProjectDiv=" + Id1 + "&DayWork=" + Id2
        },
        "Delete": "ProjectManagement/Reports/RemoveDailyDetail_C"
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
            "data": "EquipmentName"
        },
        {
            "data": "Amount",
            "className": "text-right"
        },
        {
            "data": "MeasurementUnit",
            "className": "text-center"
        },
        {
            "data": "Condition"
        },
        {
            "data": null,
            "bSortable": false,
            "className": "text-center",
            "defaultContent": "" +
                "<div class='btn-group' role='group'>" +
                    "<button class='btn btn-default btn-xs edit_C'><i class='fa fa-edit'></i></button>" +
                    "<button class='btn btn-default btn-xs delete_C'><i class='fa fa-remove'></i></button>" +
                "</div>"
        }
    ];
    attr.dataTable.bPaginate = false;
    tableDetailC = fnGetList(attr, true);

    $(attr.usingId.tableId + ' tbody')
    //for edit button
    .on('click', '.edit_C', function () {
        var Data = tableDetailC.row($(this).parents('tr')).data();
        if ($(attr.usingId.formId + " #ID_C").length == 0) {
            var hiddenID = '<input type="hidden" name="ID" id="ID_C" />';
            $(attr.usingId.formId).append(hiddenID);
        }

        var frm = $('#form_C');
        $('#btnAdd_A').html('<span><i class="fa fa-remove"></i></span> Tutup');

        $(frm).slideUp('fast', function () {
            $('#EquipmentName').val(Data.EquipmentName);
            $('#Amount_C').val(Data.Amount);
            $('#MeasurementUnit').val(Data.MeasurementUnit);
            $('#Condition').val(Data.Condition);
            $('#ID_C').val(Data.ID);
            $(frm).slideDown('normal');
        });
    })
    //for delete button
    .on('click', '.delete_C', function () {
        if ($('#form_C').is(":visible"))
            $("#btnBatal_C").click();

        var Data = tableDetailC.row($(this).parents('tr')).data();
        deleteComfirmModal(function (result) {
            if (result)
                fnDelete(Data.ID, attr.url.Delete, tableDetailC);
        });
    });

    attr_DetC = attr;
}


submitFormCallback_C = function (data) {
    if (data.stat == 1) {
        fnRefreshTable('#tb_DetailC', parseUrl(attr_DetC.url.RefreshTable(modelID, $('.DayWork').val())), function () {
            showNotification("Data telah berhasil disimpan");
            $("#btnBatal_C").click();
        });
        return false;
    }
    showNotificationSaveError(data);
}


    
$('#btnAdd_C').click(function () {
    $('#form_C').slideToggle('normal', function () {
        fnReset2('#frmDetail_C');
        if ($(this).is(":visible"))
            $('#btnAdd_C').html('<span><i class="fa fa-remove"></i></span> Tutup');
        else {
            $('#btnAdd_C').html('<span><i class="fa fa-plus"></i></span> Tambah');
        }
    });
});

$('#btnBatal_C').click(function () {
    $('#btnAdd_C').click();
});

$(document).ready(function () {
    $("#frmDetail_C").submit(function (e) {
        e.stopPropagation();
        e.preventDefault();
        showSavingNotification();
        var _data = $(this).serialize();
        var _url = $(this).attr("action");
        $.ajax({
            type: 'POST',
            url: _url,
            data: _data,
            success: submitFormCallback_C,
            error: ajax_error_callback,
            dataType: 'json'
        });
    });
});