var i = 0;

submitFormCallback = function (data) {
    if (data.stat == 1) {
        //goBack();
        GenTable.ajax.reload(); // The code associated with file CRUDHelper.js
        showNotification("Data telah berhasil disimpan");
        $("#btnBatalDetail").click();
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
    attr.usingId.tableId = "#DetailList";
    attr.url = {
        "Read": "Purchasing/PurchaseRequisition/GetPRDetailList?PurchaseRequisitionId=" + $("#PurchaseRequisitionId").val(),
        "Delete": "Purchasing/PurchaseRequisition/DeletePRDetail"
    };
    attr.dataTable.columns = [
        {
            "className": "text-right",
            "data": null
        },
        {
            "className": "",
            "data": "Name"
        },
        {
            "className": "",
            "data": "Type"
        },
        {
            "className": "",
            "data": "Qty"
        },
        {
            "className": "",
            "data": "MeasureName"
        },
        {
            "className": "",
            "data": "CurrencyName"
        },
        {
            "className": "",
            "data": "EstUnitPrice"
        },
        {
            "className": "",
            "data": "TotalExtPrice"
        },
        {
            "className": "",
            "data": "Remarks"
        },
        {
            "className": "action text-center",
            "data": null,
            "bSortable": false,
            "defaultContent": "" +
                "<div class='btn-group' role='group'>" +
                "  <button class='edit btn btn-primary btn-xs' title='Edit'><i class='fa fa-edit'></i></button>" +
                "  <button class='delete btn btn-danger btn-xs' title='Hapus'><i class='fa fa-trash-o'></i></button>" +
                "</div>"
        }
    ];
    attr.dataTable.bPaginate = false;
    i = 0;
    attr.dataTable.columnDefs = [{
        "bSortable": false,
        "targets": [0,1,2,3,4,5,6,7,8]
    }, { "width": "150px", "targets": 3}];

//    attr.dataTable.order = [[1, 'asc']];

    fnGetList(attr);

    $(attr.usingId.tableId + ' tbody')
    .on('click', '.edit', function () {
        $("#btnAddDetail").click();
    });
}

$(document).ready(function () {
    'use strict';

    GetList();
    $("#ID").val(0);

    $("#frmPRDetail").submit(function (e) {
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
});