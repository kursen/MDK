

var isAnyError = false;
var attr_DetG, tableDetailG;

GetListDetail_G = function (Id1, Id2) {
    var attr = _attrCRUD();
    attr.dataTable.autoWidth = false;
    attr.useRowNumber = false;
    attr.dataTable.bSort = false;
    attr.usingId.formId = '#frmDetail_G';
    attr.usingId.tableId = '#tb_DetailG';
    attr.url = {
        "Read": "ProjectManagement/Reports/GetDailyDetail_G?ProjectDiv=" + Id1 + "&DayWork=" + Id2,
        "RefreshTable": function (Id1, Id2, Id3) {
            return "ProjectManagement/Reports/GetDailyDetail_G?ProjectDiv=" + Id1 + "&DayWork=" + Id2
        },
        "Delete": function (title) {
            return "ProjectManagement/Reports/RemoveDailyDetail_G?title=" + title
        }
    };
    attr.dataTable.ajax.error = function (xhr, error, thrown) {
        isAnyError = true;
        ajax_error_callback(xhr, error, thrown);
    }
    attr.dataTable.columns = [
        {
            "data": "Alias",
            "className": "text-bold"
        },
        {
            "data": "Value"
        },
        {
            "data": null,
            "bSortable": false,
            "className": "text-center",
            "defaultContent": "" +
                "<div class='btn-group' role='group'>" +
                    "<button class='btn btn-default btn-xs edit_G'><i class='fa fa-edit'></i></button>" +
                    "<button class='btn btn-default btn-xs delete_G'><i class='fa fa-remove'></i></button>" +
                "</div>"
        }
    ];
    attr.dataTable.bPaginate = false;
    tableDetailG = fnGetList(attr, true);

    $(attr.usingId.tableId + ' tbody')
    //for edit button
    .on('click', '.edit_G', function () {
        var Data = tableDetailG.row($(this).parents('tr')).data();
        if ($(attr.usingId.formId + " #ID_G").length == 0) {
            var hiddenID = '<input type="hidden" name="ID" id="ID_G" />';
            $(attr.usingId.formId).append(hiddenID);
        }

        var frm = $('#form_G');

        $(frm).slideUp('fast', function () {
            $('#Alias').val(Data.Alias);
            $('#Value').val(Data.Value);
            $('.Title_').val(Data.Title);
            $('#ID_G').val(Data.ID);
            $(frm).slideDown('normal');
        });
    })
    //for delete button
    .on('click', '.delete_G', function () {
        if ($('#form_G').is(":visible"))
            $("#btnBatal_G").click();

        var Data = tableDetailG.row($(this).parents('tr')).data();
        deleteComfirmModal(function (result) {
            if (result)
                fnDelete(Data.ID, attr.url.Delete(Data.Title), tableDetailG);
        });
    });

    attr_DetG = attr;
}


submitFormCallback_G = function (data) {
    if (data.stat == 1) {
        fnRefreshTable('#tb_DetailG', parseUrl(attr_DetG.url.RefreshTable(modelID, $('.DayWork').val())), function () {
            showNotification("Data telah berhasil disimpan");
            $("#btnBatal_G").click();
        });
        return false;
    }
    showNotificationSaveError(data);
}



$('#btnBatal_G').click(function () {
    $('#form_G').slideToggle('normal', function () {
        fnReset2('#frmDetail_G');
        $('#ID_G').val(0);
    });
});

$('#ProjectTaskDivisionItemId').change(function () {
    $.ajax({
        type: 'POST',
        url: "../GetProjectTaskDivisionUnit?Id=" + $('#ProjectTaskDivisionItemId').val(),
        error: ajax_error_callback,
        dataType: 'json',
        success: function (result) {
            $("#VolumeUnit").html(result);
        }
    })
});

$(document).ready(function () {
    $("#frmDetail_G").submit(function (e) {
        e.stopPropagation();
        e.preventDefault();
        showSavingNotification();
        var _data = $("#frmDetail_G").serialize();
        var _url = $(this).attr("action");
        // var formData = new FormData(this);
        $.ajax({
            type: 'POST',
            url: _url,
            data: _data,
            success: submitFormCallback_G,
            error: ajax_error_callback,
          //  async: false,
         //   contentType: false,
          //  processData: false,
            dataType: 'json'
        });
        return false;
    });
});