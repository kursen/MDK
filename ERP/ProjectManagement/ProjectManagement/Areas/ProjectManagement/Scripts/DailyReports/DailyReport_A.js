

var isAnyError = false;
var attr_DetA, tableDetailA;

GetListDetail_A = function (Id1, Id2) {
    var attr = _attrCRUD();
    attr.dataTable.autoWidth = false;
    attr.useRowNumber = false;
    attr.dataTable.bSort = false;
    attr.usingId.formId = '#frmDetail_A';
    attr.usingId.tableId = '#tb_DetailA';
    attr.url = {
        "Read": "ProjectManagement/Reports/GetDailyDetail_A?ProjectDiv=" + Id1 + "&DayWork=" + Id2,
        "RefreshTable": function (Id1, Id2, Id3) {
            return "ProjectManagement/Reports/GetDailyDetail_A?ProjectDiv=" + Id1 + "&DayWork=" + Id2
        },
        "Delete": "ProjectManagement/Reports/RemoveDailyDetail_A"
    };
    attr.dataTable.ajax.error = function (xhr, error, thrown) {
        isAnyError = true;
        ajax_error_callback(xhr, error, thrown);
    }
    attr.dataTable.columns = [
        {
            "className": "text-center",
            "data": "PaymentNumber"
        },
        {
            "data": "TaskTitle"
        },
        {
            "data": "Volume",
            "className": "text-right border-none",
            "mRender": function (data) {
                return numberFormat(data);
            }
        },
        {
            "data": "UnitQuantity",
            "className" : "border-none"
        },
        {
            "data": "Location"
        },
        {
            "data": null,
            "bSortable": false,
            "className": "text-center",
            "defaultContent": "" +
                "<div class='btn-group' role='group'>" +
                    "<button class='btn btn-default btn-xs edit_A'><i class='fa fa-edit'></i></button>" +
                    "<button class='btn btn-default btn-xs delete_A'><i class='fa fa-remove'></i></button>" +
                "</div>"
        }
    ];
    attr.dataTable.bPaginate = false;
    tableDetailA = fnGetList(attr, true);

    $(attr.usingId.tableId + ' tbody')
    //for edit button
    .on('click', '.edit_A', function () {
        var Data = tableDetailA.row($(this).parents('tr')).data();
        if ($(attr.usingId.formId + " #ID_A").length == 0) {
            var hiddenID = '<input type="hidden" name="ID" id="ID_A" />';
            $(attr.usingId.formId).append(hiddenID);
        }

        var frm = $('#form_A');
        $('#btnAdd_A').html('<span><i class="fa fa-remove"></i></span> Tutup');

        $(frm).slideUp('fast', function () {
            $('#ProjectTaskDivisionItemId option[value=' + Data.ProjectTaskDivisionItemId + ']').prop('selected', true);
            $('#Volume').val(numberFormat(Data.Volume));
            $('#Location_A').val(Data.Location);
            $('#VolumeUnit').text(Data.UnitQuantity);
            $('#ID_A').val(Data.ID);
            $(frm).slideDown('normal');
        });
    })
    //for delete button
    .on('click', '.delete_A', function () {
        if ($('#form_A').is(":visible"))
            $("#btnBatal_A").click();

        var Data = tableDetailA.row($(this).parents('tr')).data();
        deleteComfirmModal(function (result) {
            if (result)
                fnDelete(Data.ID, attr.url.Delete, tableDetailA);
        });
    });

    attr_DetA = attr;
}


submitFormCallback_A = function (data) {
    if (data.stat == 1) {
        fnRefreshTable('#tb_DetailA', parseUrl(attr_DetA.url.RefreshTable(modelID, $('.DayWork').val())), function () {
            showNotification("Data telah berhasil disimpan");
            $("#btnBatal_A").click();
        });
        return false;
    }
    showNotificationSaveError(data);
}



$('#btnAdd_A').click(function () {
    $('#form_A').slideToggle('normal', function () {
        fnReset2('#frmDetail_A');
        $('#VolumeUnit').text('');
        $('#ID_A').val(0);
        if ($(this).is(":visible"))
            $('#btnAdd_A').html('<span><i class="fa fa-remove"></i></span> Tutup');
        else {
            $('#btnAdd_A').html('<span><i class="fa fa-plus"></i></span> Tambah');
        }
    });
});

$('#btnBatal_A').click(function () {
    $('#btnAdd_A').click();
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
    $("#frmDetail_A").submit(function (e) {
        e.stopPropagation();
        e.preventDefault();
        showSavingNotification();
        var _data = $("#frmDetail_A").serialize();
        var _url = $(this).attr("action");
        // var formData = new FormData(this);
        $.ajax({
            type: 'POST',
            url: _url,
            data: _data,
            success: submitFormCallback_A,
            error: ajax_error_callback,
          //  async: false,
         //   contentType: false,
          //  processData: false,
            dataType: 'json'
        });
        return false;
    });
});