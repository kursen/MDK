//*************************
//  Helper: CRUD
//  Created Date: 28 Jumadil Ula 1436 H || Thursday, 19 March, 2015 C
//*************************


var GenTable; // Declare public variable for temporary DataTable

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
                "data": {}
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
fnGetList = function (attrCRUD, _isExpandable, _strFor) {
    if (attrCRUD.url.Read != undefined) {
        attrCRUD.dataTable.ajax.url = parseUrl(attrCRUD.url.Read);
    } else {
        delete attrCRUD.dataTable.ajax;
        delete attrCRUD.dataTable.columns;
    }
    GenTable = $(attrCRUD.usingId.tableId).DataTable(attrCRUD.dataTable);

    if (attrCRUD.useRowNumber) {
        GenTable.on('order.dt search.dt', function () {
            GenTable.column(0, { search: 'applied', order: 'applied' }).nodes().each(function (cell, i) {
                cell.innerHTML = (i + 1) + '.';
            });
        }).draw();
    }

    $(attrCRUD.usingId.tableId + ' tbody')
    //for edit button
    .on('click', '.edit2', function () {
        var Data = GenTable.row($(this).parents('tr')).data();
        window.location.href = parseUrl(attrCRUD.url.Update + "?id=" + Data.ID);
    })
    .on('click', '.edit', function () {
        var Data = GenTable.row($(this).parents('tr')).data();
        if ($(attrCRUD.usingId.formId + " #ID").length == 0) {
            var hiddenID = '<input type="hidden" name="ID" id="ID" />';
            $(attrCRUD.usingId.formId).append(hiddenID);
        }
        fnGetData(attrCRUD.usingId.modalId, Data);
    })
    //for detail button
    .on('click', '.detail2', function () {
        var Data = GenTable.row($(this).parents('tr')).data();
        window.location.href = parseUrl(attrCRUD.url.Details + "?id=" + Data.ID);
    })
    .on('click', '.detail', function () {
        var Data = GenTable.row($(this).parents('tr')).data();
        //console.log(Data);
        fnGetData(attrCRUD.usingId.modalId, Data);

    })
    //for delete button
    .on('click', '.delete', function () {
        var Data = GenTable.row($(this).parents('tr')).data();
        deleteComfirmModal(function (result) {
            if (result)
                fnDelete(Data.ID, attrCRUD.url.Delete);
        });
    })
    //for lock button in user management
    .on('click', '.icon-lock', function () {
        var Data = GenTable.row($(this).parents('tr')).data();
        //alert(attrCRUD.usingId.tableId);
        fnActivateUser(Data.ID, attrCRUD.url.ActivateUser);
    })
    .on('click', '.icon-unlock', function () {
        var Data = GenTable.row($(this).parents('tr')).data();
        //alert(attrCRUD.usingId.tableId);
        fnActivateUser(Data.ID, attrCRUD.url.ActivateUser);
    });

    //set trigger for submit
    //fnInsert(attrCRUD.usingId.formId, attrCRUD.usingId.modalId);

    //set trigger to reset form when modal opened
    $('[data-toggle="modal"]').click(function () {
        fnReset();
    });

    //insert dataTable object to temporary variable
    var tempTable = [];
    tempTable.push(GenTable);

    //set trigger of #idFilter button to refresh
    $('body').on('click', attrCRUD.usingId.filterBtnId, function () {
        $('.loader').removeClass("hidden");
        fnRefreshTable(attrCRUD.usingId.tableId, attrCRUD.url.RefreshTable(), function () {
            $('.loader').addClass("hidden");
        });
    });

    //set dataTable in temporary variable has expandability
    if (_isExpandable) {
        detailRow_add(attrCRUD.usingId.tableId, tempTable[tempTable.length - 1], _strFor);
    }
}

/*================================================================*/

/* function for Insert data
------------------------------------------*/
fnInsert = function (idForm, callfunction) {
    var callback = callfunction;
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
                //slide();
                callback();
            } else {
                showNotificationSaveError(get);
            }
            GenTable.ajax.reload();
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
fnDelete = function (ID, url) {
    $.ajax({
        type: 'POST',
        url: url,
        data: { id: ID },
        dataType: 'json',
        success: function (get) {
            if (get.stat == 1) {
                showNotification("Data Telah terhapus!");
                GenTable.ajax.reload();
            } else {
                //swal(get.msg, get.msgDesc, "error");
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
    $.getJSON(urlData, null, function (json) {
        table = $(tableId).dataTable();
        oSettings = table.fnSettings();

        table.fnClearTable(this);
        for (var i = 0; i < json.data.length; i++) {
            table.oApi._fnAddData(oSettings, json.data[i]);
        }
        oSettings.aiDisplay = oSettings.aiDisplayMaster.slice();
        table.fnDraw();

        //run the function which has been set
        _function();
    });
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
                GenTable.ajax.reload();
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

fnremoveid = function (obj) {
    $(obj).find('#ID').remove();
}

var slide = function () {
    if ($('.no-drop:first').is(":hidden")) {
        $('.btn-add').removeClass('btn-success').addClass('btn-danger').html('Tutup');
    } else {
        $('.btn-add').removeClass('btn-danger').addClass('btn-success').html('<span><i class="fa fa-plus"></i></span>Tambah Baru');
    }
    $(".no-drop:first").slideToggle("slow");
}