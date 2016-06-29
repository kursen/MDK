

var isAnyError = false;
var attr_DetF, tableDetailF;

GetListDetail_F = function (Id1, Id2) {
    var attr = _attrCRUD();
    attr.dataTable.autoWidth = false;
    attr.useRowNumber = false;
    attr.dataTable.bSort = false;
    attr.usingId.formId = '#frmDetail_F';
    attr.usingId.tableId = '#tb_DetailF';
    attr.url = {
        "Read": "ProjectManagement/Reports/GetDailyDetail_F?ProjectDiv=" + Id1 + "&DayWork=" + Id2,
        "RefreshTable": function (Id1, Id2, Id3) {
            return "ProjectManagement/Reports/GetDailyDetail_F?ProjectDiv=" + Id1 + "&DayWork=" + Id2
        },
        "Delete": "ProjectManagement/Reports/RemoveDailyDetail_F"
    };
    attr.dataTable.ajax.error = function (xhr, error, thrown) {
        isAnyError = true;
        ajax_error_callback(xhr, error, thrown);
    }
    attr.dataTable.columns = [
        {
            "data": "Number"
        },
        {
            "data": "Type"
        },
        {
            "data": "Time",
            "mRender": function (data) {
                return formatCustomDate(data, "HH:mm");
            },
            "className": "text-center"
        },
        {
            "data": "Location"
        },
        {
            "data": "IsResponsibilityOfContractor",
            "mRender": function (data) {
                return data ? "Ya" : "Tidak";
            },
            "className": "text-center"
        },
        {
            "data": null,
            "bSortable": false,
            "className": "text-center",
            "defaultContent": "" +
                "<div class='btn-group' role='group'>" +
                    "<button class='btn btn-default btn-xs edit_F'><i class='fa fa-edit'></i></button>" +
                    "<button class='btn btn-default btn-xs delete_F'><i class='fa fa-remove'></i></button>" +
                "</div>"
        }
    ];
    attr.dataTable.bPaginate = false;


    tableDetailF = fnGetList(attr, true);

    $(attr.usingId.tableId + ' tbody')
    //for edit button
    .on('click', '.edit_F', function () {
        var Data = tableDetailF.row($(this).parents('tr')).data();
        if ($(attr.usingId.formId + " #ID_F").length == 0) {
            var hiddenID = '<input type="hidden" name="ID" id="ID_F" />';
            $(attr.usingId.formId).append(hiddenID);
        }

        var frm = $('#form_F');
        $('#btnAdd_A').html('<span><i class="fa fa-remove"></i></span> Tutup');

        $(frm).slideUp('fast', function () {
            $('#ID_F').val(Data.ID);
            $('#Number').val(Data.Number);
            $('#Type').val(Data.Type);
            $('#Time').val(formatCustomDate(Data.Time, 'HH:mm'));
            $('#Location_F').val(Data.Location);
            $('#IsResponsibilityOfContractor').prop('checked', Data.IsResponsibilityOfContractor);
            $(frm).slideDown('normal');
        });
    })
    //for delete button
    .on('click', '.delete_F', function () {
        var Data = tableDetailF.row($(this).parents('tr')).data();
        deleteComfirmModal(function (result) {
            if (result)
                fnDelete(Data.ID, attr.url.Delete, tableDetailF);
        });
    });

    attr_DetF = attr;
}


submitFormCallback_F = function (data) {
    if (data.stat == 1) {
        fnRefreshTable('#tb_DetailF', parseUrl(attr_DetF.url.RefreshTable(modelID, $('.DayWork').val())), function () {
            showNotification("Data telah berhasil disimpan");
            $("#btnBatal_F").click();
        });
        return false;
    }
    showNotificationSaveError(data);
}



$('#btnAdd_F').click(function () {
    $('#form_F').slideToggle('normal', function () {
        fnReset2('#frmDetail_F');
        if ($(this).is(":visible")) {
            $('#btnAdd_F').html('<span><i class="fa fa-remove"></i></span> Tutup');
        } else {
            $('#btnAdd_F').html('<span><i class="fa fa-plus"></i></span> Tambah');
        }
    });
});

$('#btnBatal_F').click(function () {
    $('#btnAdd_F').click();
});

$(document).ready(function () {
    //datetimePicker_ByDate($("#dtpk_Time"), true);
    timePicker($("#dtpk_Time"), true);
    
    $("#frmDetail_F").submit(function (e) {
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
            success: submitFormCallback_F,
            error: ajax_error_callback,
            async: false,
            contentType: false,
            processData: false,
            dataType: 'json'
        });
    });
});