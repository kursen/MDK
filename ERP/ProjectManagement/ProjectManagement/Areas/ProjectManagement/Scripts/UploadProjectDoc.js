var i = 0;

submitFormCallback = function (data) {
    if (data.stat == 1) {
        //goBack();
        GenTable.ajax.reload(); // The code associated with file CRUDHelper.js
        showNotification("Data telah berhasil disimpan");
        $("#btnBatalUpload").click();
        i = 0;
        return;
    }
    setTimeout(function () {
        if ($('#Files').val() == '') {
            $('#Files').parent().parent().parent().addClass("has-error");
        } else {
            $('#Files').parent().parent().parent().removeClass("has-error");
        }
    }, 500);
    showNotificationSaveError(data);
}


GetList = function () {
    var attr = _attrCRUD();
    attr.useRowNumber = true;
    attr.autoWidth = false;
    attr.usingId.tableId = "#DocList";
    attr.url = {
        "Read": "ProjectManagement/ProjectInfo/GetDocumentList?ProjectInfoID=" + $("#ProjectInfoID").val(),
        "Delete": "ProjectManagement/ProjectInfo/DeleteUploadDocDetail"
    };
    attr.dataTable.columns = [
        {
            "className": "text-right",
            "data": null
        },
        {
            "className": "",
            "data": "DocTitle"
        },
        {
            "className": "",
            "data": "Description",
            "defaultContent": "-"
        },
        {
            "className": "action text-center",
            "data": null,
            "bSortable": false,
            "defaultContent": "" +
                "<div class='btn-group' role='group'>" +
                "  <button class='download btn btn-info btn-xs' title='Download'><i class='fa fa-download'></i></button>" +
                "  <button class='edit btn btn-primary btn-xs' title='Edit'><i class='fa fa-edit'></i></button>" +
                "  <button class='delete btn btn-danger btn-xs' title='Hapus'><i class='fa fa-trash-o'></i></button>" +
                "</div>"
        }
    ];
    attr.dataTable.bPaginate = false;
    i = 0;
    attr.dataTable.columnDefs = [{
        "bSortable": false,
        "targets": [0]
        //        ,
        //        "render": function (data) {
        //            return i = i + 1;
        //        }
    }, { "width": "150px", "targets": 3}];

    attr.dataTable.order = [[1, 'asc']];

    fnGetList(attr);

    $(attr.usingId.tableId + ' tbody')
    .on('click', '.download', function () {
        var Data = GenTable.row($(this).parents('tr')).data(); // The code associated with file CRUDHelper.js
        window.location.href = '/ProjectManagement/ProjectInfo/DownloadDocDetail' + "?ID=" + Data.ID
        //        window.open(
        //          '/ProjectManagement/ProjectInfo/DownloadDocDetail' + "?ID=" + Data.ID
        //        //,'_blank'
        //        );
    })
    .on('click', '.edit', function () {
        $("#btnUpload").click();
    });
}

$(document).ready(function () {
    'use strict';

    GetList();
    $("#ID").val(0);

    $("#frmUploadDocDetail").submit(function (e) {
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
            success: submitFormCallback,
            error: ajax_error_callback,
            mimeType: "multipart/form-data",
            async: false,
            contentType: false,
            processData: false,
            dataType: 'json'
        });
    });

    $('#Files').change(function (e, data) {
        $("#FileName").val(this.value);
        if ($('#ID').val() == 0)
            $("#DocTitle").val(this.value.substr(0, this.value.lastIndexOf('.')));
    });
});