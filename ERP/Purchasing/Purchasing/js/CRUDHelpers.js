//*************************
//  Helper: CRUD
//  Created Date: 28 Jumadil Ula 1436 H || Thursday, 19 March, 2015 C
//*************************


var GenTable; // Declare public variable for temporary DataTable
$.fn.dataTableExt.sErrMode = 'none';
/*================================================================*/

/* set default cofiguration of DataTable
------------------------------------------*/
_attrCRUD = function () {
    var o = {
        "usingId": {
            "tableId": "#tb_Data",
            "formId": "#form-Data",
            "modalId": "#modal-Data",
            "filterBtnId": "#filterBtn"
        },
        "useRowNumber":false,
        "url": {},
        "dataTable": {
            "ajax": {
                "url": "", //(url == null ? "" : parseUrl(url)),
                "data": {
//                    "error": function (xhr, error, thrown) {
//                        ajax_error_callback(xhr, error, thrown);
//                    }
                }
            },
            //"sPaginationType": "bootstrap",
            "oLanguage": {
                "sLengthMenu": "_MENU_ data per halaman"
            },
            "bSort": true,
            "bPaginate": true,
            "pageLength": 25,
            "columns": [],
            "columnDefs": [],
            "order": [],
            "dom": '<"top">rt<"bottom"p><"clear">'
        }
    }
    return o;
}

/* function for get list of data
and set the action of
default button (Edit,Delete)
------------------------------------------*/
fnGetList = function (attrCRUD, _tableSet) {
    _tmpTable = null;

    if (attrCRUD.url.Read != undefined) {
        attrCRUD.dataTable.ajax.url = parseUrl(attrCRUD.url.Read);
        if (attrCRUD.dataTable.ajax.error == undefined)
            attrCRUD.dataTable.ajax.error = ajax_error_callback;
    } else {
        delete attrCRUD.dataTable.ajax;
        delete attrCRUD.dataTable.columns;
    }
    _tmpTable = $(attrCRUD.usingId.tableId)
    /*.on('error.dt', function (e, settings, techNote, message) {
    console.log('An error has been reported by DataTables: ', message);
    ajax_error_callback(settings.jqXHR, settings.jqXHR.statusText, message);
    })*/
        .DataTable(attrCRUD.dataTable);

    if (attrCRUD.useRowNumber) {
        _tmpTable.on('order.dt search.dt', function () {
            _tmpTable.column(0, { search: 'applied', order: 'applied' }).nodes().each(function (cell, i) {
                cell.innerHTML = (i + 1) + '.';
            });
        }).draw();
    }

    $(attrCRUD.usingId.tableId + ' tbody')
    //for edit button
    .on('click', '.edit2', function () {
        var Data = _tmpTable.row($(this).parents('tr')).data();
        window.location.href = parseUrl(attrCRUD.url.Update + "?id=" + Data.ID);
    })
    .on('click', '.edit', function () {
        var Data = _tmpTable.row($(this).parents('tr')).data();
        if ($(attrCRUD.usingId.formId + " #ID").length == 0) {
            var hiddenID = '<input type="hidden" name="ID" id="ID" />';
            $(attrCRUD.usingId.formId).append(hiddenID);
        }
        fnGetData(attrCRUD.usingId.modalId, Data);
    })
    //for detail button
    .on('click', '.detail2', function () {
        var Data = _tmpTable.row($(this).parents('tr')).data();
        window.location.href = parseUrl(attrCRUD.url.Details + "?id=" + Data.ID);
    })
    .on('click', '.detail', function () {
        var Data = _tmpTable.row($(this).parents('tr')).data();
        //console.log(Data);
        fnGetData(attrCRUD.usingId.modalId, Data);

    })
    //for delete button
    .on('click', '.delete', function () {
        var Data = _tmpTable.row($(this).parents('tr')).data();
        deleteComfirmModal(function (result) {
            if (result)
                fnDelete(Data.ID, attrCRUD.url.Delete);
        });
    })
    //for lock button in user management
    .on('click', '.icon-lock', function () {
        var Data = _tmpTable.row($(this).parents('tr')).data();
        //alert(attrCRUD.usingId.tableId);
        fnActivateUser(Data.ID, attrCRUD.url.ActivateUser);
    })
    .on('click', '.icon-unlock', function () {
        var Data = _tmpTable.row($(this).parents('tr')).data();
        //alert(attrCRUD.usingId.tableId);
        fnActivateUser(Data.ID, attrCRUD.url.ActivateUser);
    });

    //set trigger for submitting of </form>
    //fnInsert(attrCRUD.usingId.formId, attrCRUD.usingId.modalId);

    //set trigger to reset form when modal opened
    $('[data-toggle="modal"]').click(function () {
        fnReset();
    });

    //insert dataTable object to temporary variable
    var tempTable = [];
    tempTable.push(_tmpTable);

    //set trigger of #idFilter button to refresh
    $('body').on('click', attrCRUD.usingId.filterBtnId, function () {
        $('.loader').removeClass("hidden");
        fnRefreshTable(attrCRUD.usingId.tableId, attrCRUD.url.RefreshTable(), function () {
            $('.loader').addClass("hidden");
        });
    });


    if (_tableSet == undefined || _tableSet == null)
        GenTable = _tmpTable;
    else
        return _tmpTable;

    //set dataTable in temporary variable has expandability
    //    if (_isExpandable) {*
    //        detailRow_add(attrCRUD.usingId.tableId, tempTable[tempTable.length - 1], _strFor);
    //    }

}

/*================================================================*/

/* function for Insert data
------------------------------------------*/
fnInsert = function (idForm) {
    var objForm = $(idForm);
    var url = objForm.attr('action');
    var data = objForm.serialize();
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
            } else {
                showNotificationSaveError(get);
            }
            _tmpTable.ajax.reload();
            return false;
        },
        error: ajax_error_callback
    });
    // });
}

/* function for call data and throw it
into the form
------------------------------------------*/
fnGetData = function (idModal, rowData) {
    $(idModal).modal('show');

    var str = "", obj;
    $.each(rowData, function (index, value) {
        str = "" + value;
        if (str.toLowerCase().indexOf("/date") >= 0) {
            value = formatShortDate(str);
        }
        obj = $("[name='" + index + "']");
        obj.val(value);
        if (obj.hasClass("select2")) {
            var tmp = obj.parent().find(".select2-selection__rendered");
            tmp.text(obj.find("option:selected").text());
            tmp.attr("title", obj.find("option:selected").text());
        }
    });
}

/* function for Delete data
------------------------------------------*/
fnDelete = function (ID, url, _refresh) {
    $.ajax({
        type: 'POST',
        url: parseUrl(url),
        data: { id: ID },
        dataType: 'json',
        beforeSend: function () {
            showSavingNotification("Sedang memproses Data!");
        },
        success: function (get) {
            if (get.stat == 1) {
                showNotification("Data Telah terhapus!");
                if (_refresh != undefined)
                    _refresh.ajax.reload();
                else
                    _tmpTable.ajax.reload();
            } else {
                showFailedNotification(get.msg)
            }
            return false;
        },
        error: ajax_error_callback
    });
}

/* function toggle to refresh table
------------------------------------------*/
function fnRefreshTable(tableId, urlData, _function) {
    /*$.getJSON(urlData, null, function (json) {
        table = $(tableId).dataTable();
        oSettings = table.fnSettings();

        table.fnClearTable(this);
        for (var i = 0; i < json.data.length; i++) {
            table.oApi._fnAddData(oSettings, json.data[i]);
        }
        oSettings.aiDisplay = oSettings.aiDisplayMaster.slice();
        table.fnDraw();

        //run the function which has been set
        if (_function != null || _function != undefined)
            _function();
    });*/
    var table = $(tableId).DataTable().ajax.url(urlData).load();

    //run the function which has been set
    if (_function != null || _function != undefined)
        _function();
}

/* function toggle to activate, lock or unlock user
------------------------------------------*/
fnActivateUser = function (ID, url) {
    $.ajax({
        type: 'POST',
        url: parseUrl(url),
        data: { id: ID },
        dataType: 'json',
        success: function (get) {
            if (get.stat == 1) {
                _tmpTable.ajax.reload();
            } else {
                swal(get.msg, get.msgDesc, "error");
            }
            return false;
        },
        error: function (xhr, status, data) {
            swal("Failed!", "Message : " + xhr.status + xhr.statusText + data, "warning");
        }
    });
}

//==Untuk Reset Form=====
fnReset = function () {
    //var o = $(_attrCRUD().usingId.formId);
    $("[name='ID']").val("");
    var o = document.getElementById("form-Data");
    $.each(o, function (i, v) {
        if ($(v).attr('name') != undefined) {
            //if ($(v).attr('name').toLowerCase().indexOf("date") == 0) {
            $(v).val($(v).prop("defaultValue"));
            //}
            if ($(v).hasClass("select2")) {
                var tmp = $(v).parent().find(".select2-selection__rendered");
                tmp.text('');
                tmp.attr('');
            }
        }
    });
}

var fnReset2 = function (objform) {
    $(objform).trigger("reset");
    $('.form-group,.col-md-5', $(objform)).removeClass("has-error");
}